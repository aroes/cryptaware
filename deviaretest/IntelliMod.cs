using deviaretest;
using Nektra.Deviare2;
using System;

//One per process; determines if it is ransomware
class IntelliMod
{
    private int called = 0;
    private int processID;
    private FormInterface UI = FormInterface.GetInstance();
    private bool startup = false;
    private int cryptAcquireContextC = 0;
    private int cryptImportKeyC = 0;
    private int cryptGenKeyC = 0;
    private int cryptEncryptC = 0;
    private int cryptExportKeyC = 0;
    private int cryptDestroyKeyC = 0;
    //private int cryptGenRandomC = 0;
    private int getComputerNameC = 0;
    private int createRemoteThreadC = 0;
    private int firstFirstFileC = 0;

    public IntelliMod()
    {

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

    //Following functions handle statistics for suspicious calls
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

    internal void cryptDestroyKeyS()
    {
        cryptDestroyKeyC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "CryptDestroyKey call");
        }
    }

    //internal void cryptGenRandomS()
    //{
    //    cryptGenRandomC++;
    //    //Display sign on the UI
    //    if (UI.debugCheckBox.Checked)
    //    {
    //        FormInterface.listViewAddItem(UI.signsListView, "Generated small random bit sequence");
    //    }
    //}

    internal void getComputerNameS()
    {
        getComputerNameC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "Collection of PC Name");
        }
    }

    //Considering all calls, determine if this process is malicious
    private void evaluate()
    {
        if(cryptAcquireContextC>0)
        {

        }
    }
    internal void createRemoteThreadS()
    {
        createRemoteThreadC++;
        //Display sign on the UI
        if (UI.debugCheckBox.Checked)
        {
            FormInterface.listViewAddItem(UI.signsListView, "CreateRemoteThread: possible process injection");
        }
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
}
