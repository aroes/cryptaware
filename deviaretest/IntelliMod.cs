﻿using deviaretest;
using Nektra.Deviare2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    private FormInterface UI = FormInterface.GetInstance();

    #region Strings evaluation fields
    //String searching class
    SectionSearch searcher;
    //Time of last string scan
    private int lastScan = 0;

    //Likelyhood based on found strings
    double ransomLikelyhoodFromStrings = 0;

    //Threshold for malicious likelyhood
    private double stringsThreshold = 0.25;
    //Fraction of extensions that need to be found to mark a found white/blacklist
    double wSuspiciousFraction = 1 / 2;
    double bSuspiciousFraction = 5 / 8;

    //Was an extension list found
    bool foundExtensionsWhitelist = false;
    bool foundExtensionsBlacklist = false;

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

    private static double[] suspiciousStringsWeights = {8,//encrypted
        5,//aes
        5,//rsa
        6,//payment
        4,//pay
        9,//bitcoin
        20,//moneypak
        10,//ransom
        20,//vssadmin
        4,//protected
        4,//restore
        4,//tor 
        9,//tor browser
        7 };//private key

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
    private Timer timer = new Timer();
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
    private int createFileT = 10;
    private int createFileM = 0;
    private int createFileD = 10;
    private int findFirstFileC = 0;
    private int findFirstFileT = 20;
    private int findFirstFileM = 0;
    private int findFirstFileD = 20;
    private int writeFileC = 0;
    private int writeFileT = 100;
    private int writeFileM = 0;
    private int writeFileD = 100;
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
        timer.Elapsed += new ElapsedEventHandler(decay);
        timer.Interval = 20000;
        timer.Enabled = true;
    }

    //Apply linear decay for relevant count values; also sets the maximum values when needed
    private void decay(object source, ElapsedEventArgs e)
    {
        //For each function, check if it the max count encountered yet
        //Then subtract the decay value from the count (dont let it drop below 0)
        cryptGenKeyM = Math.Max(cryptGenKeyM, cryptGenKeyC);
        cryptGenKeyC = Math.Max(cryptGenKeyC - cryptGenKeyD, 0);

        cryptEncryptM = Math.Max(cryptEncryptM, cryptEncryptC);
        cryptEncryptC = Math.Max(cryptEncryptC - cryptEncryptD, 0);

        cryptExportKeyM = Math.Max(cryptExportKeyM, cryptExportKeyC);
        cryptExportKeyC = Math.Max(cryptExportKeyC - cryptExportKeyD, 0);

        cryptDestroyKeyM = Math.Max(cryptDestroyKeyM, cryptDestroyKeyC);
        cryptDestroyKeyC = Math.Max(cryptDestroyKeyC - cryptDestroyKeyD, 0);

        createFileM = Math.Max(createFileM, createFileC);
        createFileC = Math.Max(createFileC - createFileD, 0);

        findFirstFileM = Math.Max(findFirstFileM, findFirstFileC);
        findFirstFileC = Math.Max(findFirstFileC - findFirstFileD, 0);

        writeFileM = Math.Max(writeFileM, writeFileC);
        writeFileC = Math.Max(writeFileC - writeFileD, 0);

        deleteFileM = Math.Max(deleteFileM, deleteFileC);
        deleteFileC = Math.Max(deleteFileC - deleteFileD, 0);
    }

    #region Decision process

    //0-1 is this program ransomware
    double overallLikelyhood = 0;

    //Threshold for likelyhood
    private double overallThreshold = 0.45;

    private double[] signWeights = {
            //String analysis
            10, //ransomLikelyhoodFromStrings > stringsThreshold, //Strings indicate ransomware
            7,//foundExtensionsWhitelist || foundExtensionsBlacklist, //Memory contains a list of extensions
            14,//foundExtensionsWhitelist ^ foundExtensionsBlacklist, //Memory contains a whitelist xor a blacklist (most likely in ransomware)
            //Call analysis
            15,//startup, //Program installed itself into startup -> Suspicious
            10,//cryptAcquireContextC > 0, //Some AES/RSA cryptography
            5,//cryptImportKeyC > 0, //Imported a key
            6,//cryptGenKeyM > 0, //Generated a key
            18,//cryptGenKeyM > cryptGenKeyT, //Generating lots of keys -> Very suspicious
            16,//cryptEncryptM > cryptEncryptT, //Encrypting lots of things -> Suspicious
            5,//cryptExportKeyM > 0, //Exported a key
            10,//cryptExportKeyM > cryptExportKeyT, //Exporting lots of keys -> Suspicious
            5,//cryptDestroyKeyM > 0, //Destroyed a key
            16,//cryptDestroyKeyM > cryptDestroyKeyT, //Destroying lots of keys -> Very suspicious
            14,//suspendThreadC > 0, //Suspended a thread -> Suspicious
            17,//createRemoteThreadC > 0 && suspendThreadC > 0, //Likely process injection -> Very suspicious
            10,//createFileM > createFileT, //Opening lots of files -> Unusual
            14,//findFirstFileM > findFirstFileT, //Lots of directory searches -> Suspicious
            17,//writeFileM > writeFileT, //Lots of high entropy writes -> Very suspicious
            18,//deleteFileM > deleteFileT, //Lots of file deletes -> Very suspicious
            20,//winExecC > 0 || createProcessC > 0, //Starting vssadmin or bcdedit -> Very suspicious/Basically sufficient
            10//createFileM > createFileT && findFirstFileM > findFirstFileT && writeFileM > writeFileT //All ransomware file ops -> Almost sufficient
        };

    //Thread windows api functions
    [DllImport("kernel32.dll")]
    static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
    [DllImport("kernel32.dll")]
    static extern uint SuspendThread(IntPtr hThread);
    [DllImport("kernel32.dll")]
    static extern uint ResumeThread(IntPtr hThread);

    //Considering all calls, determine if this process is malicious
    private void evaluate()
    {
        //Aggregate all indicators
        bool[] signs = {
            //String analysis
            ransomLikelyhoodFromStrings > stringsThreshold, //Strings indicate ransomware
            foundExtensionsWhitelist || foundExtensionsBlacklist, //Memory contains a list of extensions
            foundExtensionsWhitelist ^ foundExtensionsBlacklist, //Memory contains a whitelist xor a blacklist (most likely in ransomware)
            //Call analysis
            startup, //Program installed itself into startup -> Suspicious
            cryptAcquireContextC > 0, //Some AES/RSA cryptography
            cryptImportKeyC > 0, //Imported a key
            cryptGenKeyM > 0, //Generated a key
            cryptGenKeyM > cryptGenKeyT, //Generating lots of keys -> Very suspicious
            cryptEncryptM > cryptEncryptT, //Encrypting lots of things -> Suspicious
            cryptExportKeyM > 0, //Exported a key
            cryptExportKeyM > cryptExportKeyT, //Exporting lots of keys -> Suspicious
            cryptDestroyKeyM > 0, //Destroyed a key
            cryptDestroyKeyM > cryptDestroyKeyT, //Destroying lots of keys -> Very suspicious
            suspendThreadC > 0, //Suspended a thread -> Suspicious
            createRemoteThreadC > 0 && suspendThreadC > 0, //Likely process injection -> Very suspicious
            createFileM > createFileT, //Opening lots of files -> Unusual
            findFirstFileM > findFirstFileT, //Lots of directory searches -> Suspicious
            writeFileM > writeFileT, //Lots of high entropy writes -> Very suspicious
            deleteFileM > deleteFileT, //Lots of file deletes -> Very suspicious
            winExecC > 0 || createProcessC > 0, //Starting vssadmin or bcdedit -> Very suspicious/Basically sufficient
            createFileM > createFileT && findFirstFileM > findFirstFileT && writeFileM > writeFileT //All ransomware file ops -> Almost sufficient
        };
        double sum = signWeights.Sum();
        for (int i = 0; i > signs.Length; i++)
        {
            if (signs[i])
            {
                overallLikelyhood += signWeights[i] / sum;
            }
        }
        //Show likelyhood on UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "Overall suspicion:" + overallLikelyhood);
        }
        //If process is ransomware
        if (overallLikelyhood > overallThreshold)
        {
            handleRansomware();
        }
    }

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
                "Suspicious activity detected and suspended in " + nktProc.Name + ". Do you want to resume it?",
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
                }
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
            MessageBox.Show(
                "Suspicious activity detected. " + nktProc.Name + " was terminated.",
                "Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            nktProc.Terminate();
        }
    }

    #endregion

    #region String scan functions

    //Scans for strings and decides whether they are a significant indicator !Will be skipped if mem is too large!
    private void scanForSuspiciousStringsAndExtensions()
    {
        long maxWorkingSetSize = 104857600 * 2;
        //Proceed if used RAM is small enough (<200mb)
        if (winProc.WorkingSet64 < maxWorkingSetSize)
        {
            int now = Environment.TickCount;
            //Only rescan if the last scan was >20 seconds ago
            if (now - lastScan > 20000)
            {
                //Update last scan time
                lastScan = now;
                //Search for strings
                ransomLikelyhoodFromStrings = scanForStrings();
                //Search for extension list
                scanForExtensions();
            }
        }
    }
    //Returns the likelyhood of the process being ransomware based on found strings
    private double scanForStrings()
    {
        double likelyhood = 0;
        double sum = suspiciousStringsWeights.Sum();
        //Search 
        for (int i = 0; i < suspiciousStrings.Length; i++)
        {
            //If string found
            //Special case for first string: ask to refresh memory dump file
            if (searcher.containsString(suspiciousStrings[i], i == 0))
            {
                //Update likelyhood that that the program is ransomware
                likelyhood += suspiciousStringsWeights[i] / sum;
                //Display string on UI
                if (UI.debugCheckBox.Checked)
                {
                    FormInterface.listViewAddItem(UI.signsListView, "String:" + suspiciousStrings[i]);
                }
            }
        }
        //Show likelyhood on UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "String suspicion:" + likelyhood);
        }
        return likelyhood;

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
            if (UI.debugCheckBox.Checked)
            {
                FormInterface.listViewAddItem(UI.signsListView, "Found whitelist");
            }
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
        //If over half of the string were found
        if (bExtensionsCount > (double)blacklist.Length * bSuspiciousFraction)
        {
            if (UI.debugCheckBox.Checked)
            {
                FormInterface.listViewAddItem(UI.signsListView, "Found blacklist");
            }
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
        //scanMemory();
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "Startup Install");
        }
        evaluate();
        scanForSuspiciousStringsAndExtensions();

    }
    //evals+scans Filters by provider
    internal void cryptAcquireContextS()
    {
        cryptAcquireContextC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "Acquired enhanced AES and RSA context");
        }
        evaluate();
        scanForSuspiciousStringsAndExtensions();
    }

    internal void cryptImportKeyS()
    {
        cryptImportKeyC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "CryptImportKey call");
        }
    }

    internal void cryptGenKeyS()
    {
        cryptGenKeyC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "CryptGenKey call");
        }
    }

    internal void cryptEncryptS()
    {
        cryptEncryptC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "CryptEncrypt call");
        }
    }

    internal void cryptExportKeyS()
    {
        cryptExportKeyC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "CryptExportKey call");
        }
    }
    //scans
    internal void cryptDestroyKeyS()
    {
        cryptDestroyKeyC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "CryptDestroyKey call");
        }
        scanForSuspiciousStringsAndExtensions();
    }


    //Update ui only once
    internal void getComputerNameS()
    {
        getComputerNameC++;
        //Only once
        if (getComputerNameC == 1)
        {
            //Display sign on the UI
            if (UI.debugCheckBox.Checked)
            {
                FormInterface.listViewAddItem(UI.signsListView, "Collection of PC Name");
            }
        }
    }

    internal void suspendThreadS()
    {
        suspendThreadC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "SuspendThread: possible process injection");
        }
        scanForSuspiciousStringsAndExtensions();
    }
    //scans
    internal void createRemoteThreadS()
    {
        createRemoteThreadC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "CreateRemoteThread: possible process injection");
        }
        scanForSuspiciousStringsAndExtensions();
    }

    internal void createFileS()
    {
        createFileC++;
        //Once every 10 calls
        if (createFileC % 50 == 0)
        {
            //Display sign on the UI
            if (UI.debugCheckBox.Checked)
            {
                FormInterface.listViewAddItem(UI.signsListView, "50xCreateFile");
            }
        }
    }

    //update ui every 10
    internal void findFirstFileS()
    {
        findFirstFileC++;
        //Once every 10 calls
        if (findFirstFileC % 10 == 0)
        {
            //Display sign on the UI
            if (UI.debugCheckBox.Checked)
            {
                FormInterface.listViewAddItem(UI.signsListView, "10xFinding all files in directory");
            }
        }

    }
    internal void findFirstFileTxtS()
    {
        findFirstFileC++;
        //Once every 10 calls
        if (findFirstFileC % 10 == 0)
        {
            //Display sign on the UI
            if (UI.debugCheckBox.Checked)
            {
                FormInterface.listViewAddItem(UI.signsListView, "10xFinding txt files in directory");
            }
        }

    }
    //update ui every 50
    internal void writeFileS()
    {
        writeFileC++;
        //Once every 50 calls
        if (writeFileC % 50 == 0)
        {
            //Display sign on the UI
            if (UI.debugCheckBox.Checked)
            {
                FormInterface.listViewAddItem(UI.signsListView, "50xWritefile");
            }
        }
    }

    internal void deleteFileS()
    {
        deleteFileC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "Deletefile");
        }
    }

    internal void winExecS()
    {
        winExecC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "Suspicious winExec usage");
        }
    }

    internal void createProcessS()
    {
        createProcessC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "Suspicious createProcess usage");
        }
    }
    #endregion

}
