using deviaretest;
using Nektra.Deviare2;
using System;

//One per process; determines if it is ransomware
class IntelliMod
{

    private NktProcess process;
    private FormInterface UI = FormInterface.GetInstance();
    private bool startup = false;
    private int cryptAcquireContextC = 0;
    private int called = 0;

    public IntelliMod()
    {

    }

    //Following functions handle statistics for function calls
    public void foundStartup()
    {
        startup = true;
        //scanMemory();
        evaluate();

    }

    public void cryptAcquireContextF()
    {
        cryptAcquireContextC++;
        evaluate();
    }



    public void setProcess(NktProcess process)
    {
        this.process = process;
        called++;
        if (called>1) {
            throw(new Exception());
        }
    }

    private void evaluate()
    {
        if(cryptAcquireContextC>0)
        {
            //Display sign on the UI
            if (UI.debugCheckBox.Checked)
            {
                FormInterface.listViewAddItem(UI.signsListView, "CryptAcquireContext call");
            }
        }
    }


}
