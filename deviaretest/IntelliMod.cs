using deviaretest;
using Nektra.Deviare2;
using System;
using System.Diagnostics;
using System.Linq;

//One per process; determines if it is ransomware
class IntelliMod
{
    private int processID;
    private Process winProc;
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
    //C suffix is count; T suffix is threshold;
    private bool startup = false;
    private int cryptAcquireContextC = 0;
    private int cryptImportKeyC = 0;
    private int cryptGenKeyC = 0;
    private int cryptGenKeyT = 20;
    private int cryptEncryptC = 0;
    private int cryptEncryptT = 20;
    private int cryptExportKeyC = 0;
    private int cryptExportKeyT = 20;
    private int cryptDestroyKeyC = 0;
    private int cryptDestroyKeyT = 20;
    private int getComputerNameC = 0;
    private int suspendThreadC = 0;
    private int createRemoteThreadC = 0;
    private int findFirstFileC = 0;
    private int writeFileC = 0;
    private int writeFileT = 500;
    private int deleteFileC = 0;
    private int deleteFileT = 20;
    #endregion

    public IntelliMod(NktProcess process)
    {
        processID = process.Id;
        winProc = Process.GetProcessById(processID);
        this.searcher = new SectionSearch(process, false);
    }

    //Considering all calls, determine if this process is malicious
    private void evaluate()
    {
        bool[] signs = {startup, //Program installed itself into startup
            ransomLikelyhoodFromStrings > stringsThreshold, //Strings indicate ransomware
            foundExtensionsWhitelist || foundExtensionsBlacklist, //Memory contains a list of extensions
            foundExtensionsWhitelist ^ foundExtensionsBlacklist, //Memory contains a whitelist xor a blacklist (most likely in ransomware)

        };
    }

    //Scans for strings and decides whether they are a signifiacant indicator
    private void scanForSuspiciousStringsAndExtensions()
    {
        long maxWorkingSetSize = 104857600;
        //Proceed if used RAM is small enough (<100mb)
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
    //evals+scans
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
    #endregion
}
