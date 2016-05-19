using deviaretest;
using Nektra.Deviare2;
using System;
using System.IO;
using System.Runtime.InteropServices;

//One per process; determines if it is ransomware
class IntelliMod
{
    private int called = 0;
    private int processID;
    private FormInterface UI = FormInterface.GetInstance();
    private int lastScan = 0;
    private static string[] suspiciousStrings = {"encrypted",
        "aes",
        "rsa",
        "payment",
        "pay",
        "bitcoin",
        "moneypak",
        "ransom",
        "vssadmin",
        "protected",
        "restore",
        "tor",
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
    private int firstFirstFileC = 0;
    private int writeFileC = 0;
    private int deleteFileC = 0;
    #endregion
    public IntelliMod()
    {

    }

    //Considering all calls, determine if this process is malicious
    private void evaluate()
    {
        if (cryptAcquireContextC > 0)
        {

        }
    }

    //Set process associated with this intelligence
    public void setProcess(int processID)
    {
        this.processID = processID;
        called++;
        if (called > 1)
        {
            throw (new Exception());
        }
    }

    private void scanForSuspiciousStrings()
    {
        int now = Environment.TickCount;
        //Only rescan if the last scan was >15 seconds ago
        if (now - lastScan > 15000)
        {
            //Special case for first string: refresh memory dump
            //If string found display sign on the UI
            if (UI.debugCheckBox.Checked && searchMemory(suspiciousStrings[0], true))
            {
                FormInterface.listViewAddItem(UI.signsListView, "String:" + suspiciousStrings[0]);
            }
            //Continue through the list of suspicious strings
            for (int i = 1; i < suspiciousStrings.Length; i++)
            {
                if (UI.debugCheckBox.Checked && searchMemory(suspiciousStrings[i]))
                {
                    FormInterface.listViewAddItem(UI.signsListView, "String:" + suspiciousStrings[i]);
                }
            }
            //Update last scan time
            lastScan = now;
        }
    }

    //Refresh the memory dump file and search it
    private bool searchMemory(string query, bool refresh)
    {
        NktProcess p = HookManager.GetProcess(processID);
        return new SectionSearch(p, false).containsString(query, refresh);
    }
    //Search the memory, does not create a dump file if it is already present
    private bool searchMemory(string query)
    {
        NktProcess p = HookManager.GetProcess(processID);
        return new SectionSearch(p, false).containsString(query, false);
    }

    //Following functions handle statistics for suspicious calls

    #region Statistics gathering
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

    }

    internal void cryptAcquireContextS()
    {
        cryptAcquireContextC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "Acquired enhanced AES and RSA context");
        }
        evaluate();
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
    //
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

    internal void getComputerNameS()
    {
        getComputerNameC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "Collection of PC Name");
        }
    }
    //
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

    internal void findFirstFileS()
    {
        firstFirstFileC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "Finding all files in directory");
        }

    }

    internal void writeFileS()
    {
        writeFileC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "Writefile");
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
