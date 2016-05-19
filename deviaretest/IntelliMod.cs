using deviaretest;
using Nektra.Deviare2;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

//One per process; determines if it is ransomware
class IntelliMod
{
    private int processID;
    private FormInterface UI = FormInterface.GetInstance();

    #region Strings evaluation
    SectionSearch searcher;
    private int lastScan = 0;
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
        "sample music",
        "pdf",
        "jpg",
        "docx",
        "mp3",
        "asm",
        "pif",
        "msp",
        "hta",
        "cpl",
        "msc",
        "scf",
        "msi"};
    private static double[] suspiciousStringsWeights = {6,//encrypted
        3,//aes
        3,//rsa
        6,//payment
        4,//pay
        7,//bitcoin
        9,//moneypak
        9,//ransom
        10,//vssadmin
        4,//protected
        4,//restore
        4,//tor 
        8,//tor browser
        6,//sample music
        3,//pdf
        3,//jpg
        3,//docx
        3,//mp3
        3,//asm
        5,//pif
        5,//msp
        5,//hta
        5,//cpl
        5,//msc
        5,//scf
        3 };//msi


    private double stringsThreshold = 0.5;
    private bool isRansomFromStrings = false;

    #endregion

    #region Number of calls
    private bool startup = false;
    private int cryptAcquireContextC = 0;
    private int cryptImportKeyC = 0;
    private int cryptGenKeyC = 0;
    private int cryptEncryptC = 0;
    private int cryptExportKeyC = 0;
    private int cryptDestroyKeyC = 0;
    private int getComputerNameC = 0;
    private int createRemoteThreadC = 0;
    private int findFirstFileC = 0;
    private int writeFileC = 0;
    private int deleteFileC = 0;
    #endregion

    public IntelliMod(NktProcess process)
    {
        processID = process.Id;
        this.searcher = new SectionSearch(process, false);
    }

    //Considering all calls, determine if this process is malicious
    private void evaluate()
    {
        if (cryptAcquireContextC > 0)
        {

        }
    }

    //Scans for strings and decides whether they are a signifiacant indicator
    private void scanForSuspiciousStrings()
    {
        double likelyhood = 0;
        double sum = suspiciousStringsWeights.Sum();
        int now = Environment.TickCount;
        //Only rescan if the last scan was >15 seconds ago
        if (now - lastScan > 15000)
        {
            //Search for each suspicious string
            for (int i = 0; i < suspiciousStrings.Length; i++)
            {
                //If string found
                //Special case for first string: ask to refresh memory dump file (will not refresh is last time was <15s ago)
                if (searcher.containsString(suspiciousStrings[i], i == 0))
                {
                    //Update likelyhood that that the program is ransomware
                    likelyhood += suspiciousStringsWeights[i] / sum;
                    //Display sign on UI
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
            if (likelyhood > stringsThreshold)
            {
                isRansomFromStrings = true;
            }
            //Update last scan time
            lastScan = now;
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
        scanForSuspiciousStrings();

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
        scanForSuspiciousStrings();
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
        scanForSuspiciousStrings();
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
    //scans
    internal void createRemoteThreadS()
    {
        createRemoteThreadC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "CreateRemoteThread: possible process injection");
        }
        scanForSuspiciousStrings();
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
