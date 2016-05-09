using Nektra.Deviare2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace deviaretest
{
    public class HookManager
    {
        private NktSpyMgr spyMgr;

        public HookManager()
        {
            //Initialize spy manager
            spyMgr = new NktSpyMgr();
            spyMgr.Initialize();

        }

        public static void listViewAddItem(ListView varListView, ListViewItem item)
        {
            if (varListView.InvokeRequired)
            {
                varListView.BeginInvoke(new MethodInvoker(() => listViewAddItem(varListView, item)));
            }
            else
            {
                varListView.Items.Add(item);
            }
        }

        //Installs the required hooks and activates them
        public void InstallHooks(NktProcess process)
        {
            Debug.WriteLine("Installing hooks in " + process.Name);

            //Display the new process on the UI
            ListViewItem item = new ListViewItem(process.Name);
            hooker UI = hooker.GetInstance();
            listViewAddItem(UI.processListView, item);

            //Install each function hook
            InstallFunctionHook("createrewgr");
        }

        private void InstallFunctionHook(string functionName)
        {

        }

        //Get NktProcess by name (maybe outdated method)
        public NktProcess GetProcess(string processName)
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

        //Get NktProcess by its PID
        public NktProcess GetProcess(int ID)
        {
            NktProcessesEnum enumProcess = spyMgr.Processes();
            NktProcess process = enumProcess.GetById(ID);
            if (process == null)
            {
                Debug.WriteLine("Fatal error while retrieving process by PID " + ID);
            }
            return process;
        }
    }
}
