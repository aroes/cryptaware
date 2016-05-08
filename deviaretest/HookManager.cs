using Nektra.Deviare2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deviaretest
{
    public class HookManager
    {
        private static NktSpyMgr spyMgr;
        private NktProcess process;
        public HookManager()
        {
            //Initialize spy manager
            spyMgr = new NktSpyMgr();
            spyMgr.Initialize();

        }

        //Installs the required hooks and activates them
        public static void InstallHooks(NktProcess process)
        {
            Debug.WriteLine("Installing hooks");
        }

        //Gets the NktProcess handle to the named process
        public static NktProcess GetProcess(string processName)
        {
            NktProcessesEnum enumProcess = spyMgr.Processes();
            NktProcess tempProcess = enumProcess.First();
            while (tempProcess != null)
            {
                if (tempProcess.Name.Equals(processName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return tempProcess;
                }
                tempProcess = enumProcess.Next();
            }
            return null;
        }
    }
}
