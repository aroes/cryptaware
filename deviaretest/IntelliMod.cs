﻿using CryptAware;
using Nektra.Deviare2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;

//One per process; determines if it is ransomware
class IntelliMod
{
    private int processID;
    private Process winProc;
    private NktProcess nktProc;
    private bool ignore = false;
    private FormInterface UI = FormInterface.GetInstance();

    #region Strings evaluation fields
    //String searching class
    SectionSearch searcher;
    //Time of last string scan
    private int lastScan = 0;

    //Likelihood based on found strings
    double ransomLikelihoodFromStrings = 0;

    //Threshold for malicious Likelihood
    private double stringsThreshold = 0.25;
    //Fraction of extensions that need to be found to mark a found white/blacklist
    double wSuspiciousFraction = (double)1 / 2;
    double bSuspiciousFraction = (double)5 / 8;

    //Was an extension list found
    bool foundExtensionsWhitelist = false;
    bool foundExtensionsBlacklist = false;

    bool[] discoveredStrings = new bool[suspiciousStrings.Length];

    //While not particularly legible, this dual array method to match strings to weights is the most straightforward
    private static string[] suspiciousStrings = {"encrypted",
        "aes ",
        "rsa ",
        "payment ",
        "pay ",
        "bitcoin",
        "moneypak",
        "ransom",
        "vssadmin",
        "protected",
        "restore",
        " tor ",
        "tor browser",
        "private key",};

    private static double[] suspiciousStringsWeights = {5,//encrypted
        5,//aes
        5,//rsa
        5,//payment
        5,//pay
        15,//bitcoin
        20,//moneypak
        15,//ransom
        20,//vssadmin
        5,//protected
        5,//restore
        5,//tor 
        10,//tor browser
        10 };//private key

    private static string[] whitelist = {"3dm",
        "3ds",
        "3fr",
        "3g2",
        "3gp",
        "m4v",
        "mp3",
        "mp4",
        "mpg",
        "msg",
        "nef",
        "odt",
        "ppt",
        ".raw",
        "rtf",
        "xls",
        "docx",
        "flac",
        "pptm",
        "pptx",
        "pdf"};

    private static string[] blacklist = {"exe",
        "pif",
        ".scr",
        ".sys",
        "msi",
        "msp",
        "hta",
        "cpl",
        "msc",
        ".bat",
        "cmd",
        "scf"};


    #endregion

    #region Number of calls fields
    //Decay timer
    private System.Timers.Timer timer = new System.Timers.Timer();
    //Use Max value when it exists, C otherwise (if M value does not exists it means C is exempt from the decay process)
    //C suffix is count; T suffix is threshold; M is maximum; D is decay amount
    //An application is allowed T calls every [20 seconds] Decay tick. Else call will be tagged as malicious
    private bool startup = false;
    private int cryptAcquireContextC = 0; //T= C>1
    private int cryptImportKeyC = 0; //T= C>1
    private int cryptGenKeyC = 0;
    private int cryptGenKeyT = 5;
    private int cryptGenKeyD = 5;
    private int cryptGenKeyM = 0;
    private int cryptEncryptC = 0;
    private int cryptEncryptT = 5;
    private int cryptEncryptM = 0;
    private int cryptEncryptD = 5;
    private int cryptExportKeyC = 0;
    private int cryptExportKeyT = 5;
    private int cryptExportKeyM = 0;
    private int cryptExportKeyD = 5;
    private int cryptDestroyKeyC = 0;
    private int cryptDestroyKeyT = 5;
    private int cryptDestroyKeyM = 0;
    private int cryptDestroyKeyD = 5;
    private int getComputerNameC = 0; //T= C>1
    private int suspendThreadC = 0; //T= C>1
    private int createRemoteThreadC = 0; //T= C>1
    private int createFileC = 0;
    private int createFileT = 20;
    private int createFileM = 0;
    private int createFileD = 20;
    private int findFirstFileC = 0;
    private int findFirstFileT = 20;
    private int findFirstFileM = 0;
    private int findFirstFileD = 20;
    private int writeFileC = 0;
    private int writeFileT = 50;
    private int writeFileM = 0;
    private int writeFileD = 50;
    private int deleteFileC = 0;
    private int deleteFileT = 10;
    private int deleteFileM = 0;
    private int deleteFileD = 10;
    private int winExecC = 0; //T= C>1
    private int createProcessC = 0; //T= C>1
    #endregion

    public IntelliMod(NktProcess process)
    {
        processID = process.Id;
        winProc = Process.GetProcessById(processID);
        nktProc = process;
        this.searcher = new SectionSearch(process, false);
        //Set the timer to trigger decay() every x seconds
        timer.Elapsed += decay;
        timer.Interval = 20000;
        timer.Enabled = true;
    }
    //Stop decaying (for when the tracked process terminates)
    internal void killTimer()
    {
        timer.Elapsed -= decay;
        timer.Enabled = false;
    }

    //Display sign list on UI
    internal void displaySigns()
    {
        displaySign("Startup install", startup ? 1 : 0, 0);
        displaySign("Some AES/RSA cryptography", cryptAcquireContextC, 0);
        displaySign("Key import", cryptImportKeyC, 0);
        displaySign("Key generation", cryptGenKeyM, cryptGenKeyT);
        displaySign("Encryption", cryptEncryptM, cryptEncryptT);
        displaySign("Key export", cryptExportKeyM, cryptExportKeyT);
        displaySign("Key destruction", cryptDestroyKeyM, cryptDestroyKeyT);
        displaySign("Collecting PC name", getComputerNameC, 0);
        displaySign("Suspend thread call", suspendThreadC, 0);
        displaySign("Created remote thread", createRemoteThreadC, 0);
        displaySign("Opened or created a file", createFileM, createFileT);
        displaySign("Searching for files", findFirstFileM, findFirstFileT);
        displaySign("High entropy file write", writeFileM, writeFileT);
        displaySign("File deletion", deleteFileM, deleteFileT);
        displaySign("Unusual WinExec", winExecC, 0);
        displaySign("Unusual CreateProcess", createProcessC, 0);
        displaySign("String suspicion", ransomLikelihoodFromStrings, stringsThreshold);
        displaySign("Overall suspicion", maxLikelihood, overallThreshold);
    }

    //Helper to display signs on UI
    private void displaySign(string name, double value, double threshold)
    {
        string[] row = new string[2];
        row[0] = value.ToString();
        row[1] = value > threshold ? "Yes" : "No";
        FormInterface.listViewAddItemRange(UI.signCountListView, name, row);
    }

    //Apply linear decay for relevant count values; also sets the maximum values when needed
    private void decay(object source, ElapsedEventArgs e)
    {
        //For each function, update Max count value
        refreshMax();
        //Then subtract the decay value from the count (dont let it drop below 0)
        cryptGenKeyC = Math.Max(cryptGenKeyC - cryptGenKeyD, 0);

        cryptEncryptC = Math.Max(cryptEncryptC - cryptEncryptD, 0);

        cryptExportKeyC = Math.Max(cryptExportKeyC - cryptExportKeyD, 0);

        cryptDestroyKeyC = Math.Max(cryptDestroyKeyC - cryptDestroyKeyD, 0);

        createFileC = Math.Max(createFileC - createFileD, 0);

        findFirstFileC = Math.Max(findFirstFileC - findFirstFileD, 0);

        writeFileC = Math.Max(writeFileC - writeFileD, 0);

        deleteFileC = Math.Max(deleteFileC - deleteFileD, 0);
    }
    //Updates M values
    private void refreshMax()
    {
        cryptGenKeyM = Math.Max(cryptGenKeyM, cryptGenKeyC);

        cryptEncryptM = Math.Max(cryptEncryptM, cryptEncryptC);

        cryptExportKeyM = Math.Max(cryptExportKeyM, cryptExportKeyC);

        cryptDestroyKeyM = Math.Max(cryptDestroyKeyM, cryptDestroyKeyC);

        createFileM = Math.Max(createFileM, createFileC);

        findFirstFileM = Math.Max(findFirstFileM, findFirstFileC);

        writeFileM = Math.Max(writeFileM, writeFileC);

        deleteFileM = Math.Max(deleteFileM, deleteFileC);
    }

    #region Decision process

    //0-1 is or was this program ransomware
    double maxLikelihood = 0;

    //Threshold for Likelihood
    private double overallThreshold = 0.25;

    #region Sign lists (update them together)
    private double[] signWeights = {
            //String analysis
            20, //ransomLikelihoodFromStrings > stringsThreshold, //Strings indicate ransomware
            5,//foundExtensionsWhitelist || foundExtensionsBlacklist, //Memory contains a list of extensions
            15,//foundExtensionsWhitelist ^ foundExtensionsBlacklist, //Memory contains a whitelist xor a blacklist (most likely in ransomware)
            //Call analysis
            15,//startup, //Program installed itself into startup -> Suspicious
            10,//cryptAcquireContextC > 0, //Some AES/RSA cryptography
            5,//cryptImportKeyC > 0, //Imported a key
            5,//cryptGenKeyM > 0, //Generated a key
            20,//cryptGenKeyM > cryptGenKeyT, //Generating lots of keys -> Very suspicious
            15,//cryptEncryptM > cryptEncryptT, //Encrypting lots of things -> Suspicious
            5,//cryptExportKeyM > 0, //Exported a key
            10,//cryptExportKeyM > cryptExportKeyT, //Exporting lots of keys -> Suspicious
            10,//cryptDestroyKeyM > 0, //Destroyed a key
            15,//cryptDestroyKeyM > cryptDestroyKeyT, //Destroying lots of keys -> Very suspicious
            5,//getComputerNameC > 0, //Collecting ComputerName ->Fairly standard
            15,//suspendThreadC > 0, //Suspended a thread -> Suspicious
            15,//createRemoteThreadC > 0 && suspendThreadC > 0, //Likely process injection -> Very suspicious
            10,//createFileM > createFileT, //Opening lots of files -> Unusual
            10,//findFirstFileM > findFirstFileT, //Lots of directory searches -> Suspicious
            15,//writeFileM > writeFileT, //Lots of high entropy writes -> Very suspicious
            20,//deleteFileM > deleteFileT, //Lots of file deletes -> Very suspicious
            25,//winExecC > 0 || createProcessC > 0, //Starting vssadmin or bcdedit -> Very suspicious/Basically sufficient
            25//createFileM > createFileT && findFirstFileM > findFirstFileT && writeFileM > writeFileT //All ransomware file ops -> Almost sufficient
        };

    //Gets the true or false values for each indicator
    private bool[] aggregateSigns()
    {
        //Aggregate all indicators
        bool[] signs = {
            //String analysis
            ransomLikelihoodFromStrings > stringsThreshold, //Strings indicate ransomware0
            foundExtensionsWhitelist || foundExtensionsBlacklist, //Memory contains a list of extensions1
            foundExtensionsWhitelist ^ foundExtensionsBlacklist, //Memory contains a whitelist xor a blacklist (most likely in ransomware)2
            //Call analysis
            startup, //Program installed itself into startup -> Suspicious3
            cryptAcquireContextC > 0, //Some AES/RSA cryptography4
            cryptImportKeyC > 0, //Imported a key5
            cryptGenKeyM > 0, //Generated a key6
            cryptGenKeyM > cryptGenKeyT, //Generating lots of keys -> Very suspicious7
            cryptEncryptM > cryptEncryptT, //Encrypting lots of things -> Suspicious8
            cryptExportKeyM > 0, //Exported a key9
            cryptExportKeyM > cryptExportKeyT, //Exporting lots of keys -> Suspicious10
            cryptDestroyKeyM > 0, //Destroyed a key11
            cryptDestroyKeyM > cryptDestroyKeyT, //Destroying lots of keys -> Very suspicious12
            getComputerNameC > 0, //Collecting ComputerName ->Fairly standard13
            suspendThreadC > 0, //Suspended a thread -> Suspicious14
            createRemoteThreadC > 0 && suspendThreadC > 0, //Likely process injection -> Very suspicious15
            createFileM > createFileT, //Opening lots of files -> Unusual16
            findFirstFileM > findFirstFileT, //Lots of directory searches -> Suspicious17
            writeFileM > writeFileT, //Lots of high entropy writes -> Very suspicious18
            deleteFileM > deleteFileT, //Lots of file deletes -> Very suspicious19
            winExecC > 0 || createProcessC > 0, //Starting vssadmin or bcdedit -> Very suspicious/Basically sufficient20
            createFileM > createFileT && findFirstFileM > findFirstFileT && writeFileM > writeFileT //All ransomware file ops -> Almost sufficient21
        };
        return signs;
    }

    #endregion

    //Considering all calls, determine if this process is malicious
    private void evaluate()
    {
        //0-1 is this program ransomware
        double tempLikelihood = 0;
        //Get up to date Max values
        refreshMax();
        bool[] signs = aggregateSigns();
        double sum = signWeights.Sum();
        for (int i = 0; i < signs.Length; i++)
        {
            if (signs[i])
            {
                tempLikelihood += signWeights[i] / sum;
            }
        }
        //Update max Likelihood 
        maxLikelihood = Math.Max(maxLikelihood, tempLikelihood);
        //If process is ransomware
        if (maxLikelihood > overallThreshold && !ignore)
        {
            handleRansomware();
        }

    }

    //Thread suspend windows api functions
    [DllImport("kernel32.dll")]
    static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
    [DllImport("kernel32.dll")]
    static extern uint SuspendThread(IntPtr hThread);
    [DllImport("kernel32.dll")]
    static extern uint ResumeThread(IntPtr hThread);
    [DllImport("kernel32.dll")]
    static extern Boolean CloseHandle(IntPtr handle);

    //Notify user and suspend or kill process
    private void handleRansomware()
    {
        //Suspend and notify
        if (UI.suspendRadioButton.Checked)
        {
            //Get all of the process's threads
            ProcessThreadCollection threads = Process.GetProcessById(processID).Threads;
            //This list will hold all the handles to the opened threads
            List<IntPtr> threadHandles = new List<IntPtr>();
            //Suspend all threads and populate the handle list
            foreach (ProcessThread t in threads)
            {
                IntPtr hThread = OpenThread(0, false, (uint)t.Id);
                SuspendThread(hThread);
                threadHandles.Add(hThread);
            }

            //Notify
            DialogResult result = MessageBox.Show(
                "Suspicious activity detected and suspended in " + nktProc.Name + 
                ".\n(" + nktProc.Path + ")\nIgnore and whitelist?",
                "Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                //Resume process
                foreach (IntPtr hThread in threadHandles)
                {
                    ResumeThread(hThread);
                    CloseHandle(hThread);
                }
                //Add path to whitelist file
                if (!File.Exists(".\\whitelist.wca") || 
                    !File.ReadAllText(".\\whitelist.wca").Contains(nktProc.Path, StringComparison.OrdinalIgnoreCase))
                {
                    StreamWriter sw = File.AppendText(".\\whitelist.wca");
                    sw.WriteLine(nktProc.Path);
                    sw.Close();
                }
                //Set intellimod to ignore
                ignore = true;
            }
            else
            {
                //Kill process
                nktProc.Terminate();
            }

        }
        //Kill and notify
        if (UI.killRadioButton.Checked)
        {
            nktProc.Terminate();
            MessageBox.Show(
                "Suspicious activity detected. " + nktProc.Name +
                "\n(" + nktProc.Path + ")\nwas terminated.",
                "Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
        }
        if (UI.passiveRadioButton.Checked)
        {
            //Do something passive
        }
    }

    #endregion

    #region String scan functions

    //Scans for strings and decides whether they are a significant indicator !Will be skipped if mem is too large!
    private void scanForSuspiciousStringsAndExtensions()
    {
        long maxWorkingSetSize = 104857600 * 2;
        //Proceed if used RAM is small enough (<200mb)
        if (winProc.WorkingSet64 + winProc.PrivateMemorySize64 < maxWorkingSetSize)
        {
            int now = Environment.TickCount;
            //Only rescan if the last scan was >20 seconds ago
            if (now - lastScan > 20000)
            {
                //Update last scan time
                lastScan = now;
                //Search for strings
                ransomLikelihoodFromStrings = Math.Max(ransomLikelihoodFromStrings, scanForStrings());
                //Search for extension list
                scanForExtensions();
            }
        }
    }
    //Returns the Likelihood of the process being ransomware based on found strings
    private double scanForStrings()
    {
        double likelihood = 0;
        double sum = suspiciousStringsWeights.Sum();
        //Search 
        for (int i = 0; i < suspiciousStrings.Length; i++)
        {
            //If string found
            //Special case for first string: ask to refresh memory dump file
            if (searcher.containsString(suspiciousStrings[i], i == 0))
            {
                //Update likelihood that that the program is ransomware
                likelihood += suspiciousStringsWeights[i] / sum;
                //Set string for display
                discoveredStrings[i] = true;
            }
        }
        return likelihood;

    }
    //Display string evaluation result to UI
    internal void displayStrings()
    {
        for (int i = 0; i < suspiciousStrings.Length; i++)
        {
            if (discoveredStrings[i])
            {
                FormInterface.listViewAddItem(UI.stringListView, suspiciousStrings[i]);
            }
        }
        FormInterface.listViewAddItem(UI.stringListView, "---------------");
        if (foundExtensionsWhitelist && !foundExtensionsBlacklist)
        {
            FormInterface.listViewAddItem(UI.stringListView, "Found extension whitelist");
        }
        if (!foundExtensionsWhitelist && foundExtensionsBlacklist)
        {
            FormInterface.listViewAddItem(UI.stringListView, "Found extension blacklist");
        }
        if (foundExtensionsWhitelist && foundExtensionsBlacklist)
        {
            FormInterface.listViewAddItem(UI.stringListView, "Found extension list");
        }
        FormInterface.listViewAddItem(UI.stringListView, "String suspicion: " + ransomLikelihoodFromStrings);
    }

    //Updates class fields if it found a whitelist or blacklist of extensions
    private void scanForExtensions()
    {

        int wExtensionsCount = 0;
        int bExtensionsCount = 0;
        //Search for whitelist
        for (int i = 0; i < whitelist.Length; i++)
        {
            //If extension found increment count
            if (searcher.containsString(whitelist[i], false))
            {
                wExtensionsCount++;
            }
        }
        //If over half of the string were found
        if (wExtensionsCount > (double)whitelist.Length * wSuspiciousFraction)
        {
            foundExtensionsWhitelist = true;
        }

        //Search for blacklist
        for (int i = 0; i < blacklist.Length; i++)
        {
            //If extension found increment count
            if (searcher.containsString(blacklist[i], false))
            {
                bExtensionsCount++;
            }
        }
        //If over 5/8 of the string were found
        if (bExtensionsCount > (double)blacklist.Length * bSuspiciousFraction)
        {
            foundExtensionsBlacklist = true;
        }
    }

    #endregion



    //The following functions handle statistics for suspicious calls

    #region Statistics gathering

    //evals+scans
    internal void foundStartup()
    {
        startup = true;
        scanForSuspiciousStringsAndExtensions();
        evaluate();
    }
    //Enhanced RSA and AES provider acquired evals+scans
    internal void cryptAcquireContextS()
    {
        cryptAcquireContextC++;
        scanForSuspiciousStringsAndExtensions();
        evaluate();
    }

    internal void cryptImportKeyS()
    {
        cryptImportKeyC++;
    }

    internal void cryptGenKeyS()
    {
        cryptGenKeyC++;
    }
    //scans + eval
    internal void cryptEncryptS()
    {
        cryptEncryptC++;
        scanForSuspiciousStringsAndExtensions();
        evaluate();
    }

    internal void cryptExportKeyS()
    {
        cryptExportKeyC++;
    }
    //scans + eval
    internal void cryptDestroyKeyS()
    {
        cryptDestroyKeyC++;
        evaluate();
        scanForSuspiciousStringsAndExtensions();
    }


    //scan+eval only once
    internal void getComputerNameS()
    {
        getComputerNameC++;
        //Only once
        if (getComputerNameC == 1)
        {
            scanForSuspiciousStringsAndExtensions();
            evaluate();
        }
    }
    //scan + eval
    internal void suspendThreadS()
    {
        suspendThreadC++;
        scanForSuspiciousStringsAndExtensions();
        evaluate();
    }
    //eval, scans once
    internal void createRemoteThreadS()
    {
        createRemoteThreadC++;
        if (createRemoteThreadC == 1)
        {
            scanForSuspiciousStringsAndExtensions();
            evaluate();
        }

    }
    //File openings and creations (\appdata\ ignored). eval every 10
    internal void createFileS()
    {
        createFileC++;
        //Once every 10 calls
        if (createFileC % 10 == 0)
        {
            evaluate();
        }
    }

    //Finding all files (\appdata\ ignored) eval every 5
    internal void findFirstFileS()
    {
        findFirstFileC++;
        //Once every 5 calls
        if (findFirstFileC % 5 == 0)
        {
            evaluate();
        }

    }
    //Finding all txt files (\appdata\ ignored) eval every 5
    internal void findFirstFileTxtS()
    {
        findFirstFileC++;
        //Once every 5 calls
        if (findFirstFileC % 5 == 0)
        {
            evaluate();
        }
    }
    //High entropy writefiles
    internal void writeFileS()
    {
        writeFileC++;
        scanForSuspiciousStringsAndExtensions();
        evaluate();
    }
    //DeleteFiles not in \appdata\
    internal void deleteFileS()
    {
        deleteFileC++;
        scanForSuspiciousStringsAndExtensions();
        evaluate();
    }
    //Execution of vssadmin or bcdedit
    internal void winExecS()
    {
        winExecC++;
        scanForSuspiciousStringsAndExtensions();
        evaluate();
    }
    //Execution of vssadmin or bcdedit
    internal void createProcessS()
    {
        createProcessC++;
        scanForSuspiciousStringsAndExtensions();
        evaluate();
    }
    #endregion

}
