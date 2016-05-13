using Nektra.Deviare2;
using System;

//One per process; determines if it is ransomware
class IntelliMod
{

    private NktProcess process;
    private bool startup = false;
    private int called = 0;

    public IntelliMod()
    {

    }

    public void foundStartup()
    {
        startup = true;
        //scanMemory();
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

    private bool evaluate()
    {
        return startup;
    }
}
