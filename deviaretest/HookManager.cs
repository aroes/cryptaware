using Nektra.Deviare2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace deviaretest
{
    public class HookManager
    {
        private NktSpyMgr spyMgr;

        private FormInterface UI;

        private IntelliMod intelligence;

        public HookManager(NktSpyMgr spyMgr)
        {
            this.spyMgr = spyMgr;
            this.UI = FormInterface.GetInstance();
            intelligence = new IntelliMod();

        }




        //Installs the required hooks and initialises intelligence
        public int InstallHooks(NktProcess process)
        {
            try
            {
                Debug.WriteLine("Installing hooks in " + process.Name);

                //Install each function hook

                //InstallFunctionHook("kernel32.dll!CreateFileA", process);
                //InstallFunctionHook("kernel32.dll!CreateFileW", process);


                InstallFunctionHook("advapi32.dll!RegCreateKeyExA", process);
                InstallFunctionHook("advapi32.dll!RegCreateKeyExW", process);

                InstallFunctionHook("advapi32.dll!CryptAcquireContextA", process);
                InstallFunctionHook("advapi32.dll!CryptAcquireContextW", process);

                InstallFunctionHook("advapi32.dll!CryptImportKey", process);

                InstallFunctionHook("advapi32.dll!CryptGenKey", process);

                InstallFunctionHook("advapi32.dll!CryptEncrypt", process);

                InstallFunctionHook("advapi32.dll!CryptExportKey", process);

                InstallFunctionHook("advapi32.dll!CryptDestroyKey", process);
                //Normally the intelligence module is specific to each process
                intelligence.setProcess(process);

                if (UI.debugCheckBox.Checked)
                {
                    //Display the new process on the UI
                    FormInterface.listViewAddItem(UI.processListView, process.Name);
                }
                return 0;
            }
            catch (NullReferenceException)
            {
                return -1;
            }

        }

        private void InstallFunctionHook(string functionName, NktProcess process)
        {
            // Removed this flag eNktHookFlags.flgOnly32Bits
            //Create hook for the given function
            NktHook hook = spyMgr.CreateHook(functionName, (int)eNktHookFlags.flgOnlyPreCall);
            //Event watcher
            spyMgr.OnFunctionCalled += new DNktSpyMgrEvents_OnFunctionCalledEventHandler(OnFunctionCalled);
            //Activate the hook
            hook.Hook(true);
            hook.Attach(process, true);
        }


        //When a hooked function executes
        void OnFunctionCalled(NktHook hook, INktProcess proc, INktHookCallInfo callInfo)
        {
            if (UI.debugCheckBox.Checked)
            {
                //Display call on the UI
                string[] row = { proc.Name, DateTime.Now.ToString("h:mm:ss") };
                FormInterface.listViewAddItemRange(UI.calledFListView, hook.FunctionName, row);
            }


            //Call the function specific handler from string
            //1:Split function name to the right of '!' and add the handler tag
            string mn = hook.FunctionName.Substring(hook.FunctionName.LastIndexOf('!') + 1) + 'H';
            //2:Lowercase first letter
            mn = Char.ToLowerInvariant(mn[0]) + mn.Substring(1);
            //3:Invoke
            try
            {
                MethodInfo mi = this.GetType().GetMethod(mn, BindingFlags.Instance | BindingFlags.NonPublic);
                Object[] funcParams = { callInfo };
                mi.Invoke(this, funcParams);
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine(mn + " has no handler");
            }
        }

        //Send suspicious regCreateKeyEx calls to intelligence
        private void regCreateKeyExAH(INktHookCallInfo callInfo)
        {
            regCreateKeyExH(callInfo);
        }
        private void regCreateKeyExWH(INktHookCallInfo callInfo)
        {
            regCreateKeyExH(callInfo);
        }
        private void regCreateKeyExH(INktHookCallInfo callInfo)
        {
            string path = callInfo.Params().GetAt(1).ReadString();
            if (path.Contains("Windows\\CurrentVersion\\Run") || path.Contains("Windows\\CurrentVersion\\RunOnce"))
            {
                intelligence.foundStartup();

            }

        }

        //Send suspicious cryptAcquireContext calls to intelligence
        private void cryptAcquireContextAH(INktHookCallInfo callInfo)
        {
            cryptAcquireContextH(callInfo);
        }
        private void cryptAcquireContextWH(INktHookCallInfo callInfo)
        {
            cryptAcquireContextH(callInfo);
        }
        private void cryptAcquireContextH(INktHookCallInfo callInfo)
        {
            intelligence.cryptAcquireContextS();
        }

        private void cryptImportKeyH(INktHookCallInfo callInfo)
        {
            intelligence.cryptImportKeyS();
        }

        private void cryptGenKeyH(INktHookCallInfo callInfo)
        {
            intelligence.cryptGenKeyS();
        }

        private void cryptEncryptH(INktHookCallInfo callInfo)
        {
            intelligence.cryptEncryptS();
        }

        private void cryptExportKeyH(INktHookCallInfo callInfo)
        {
            intelligence.cryptExportKeyS();
        }

        private void cryptDestroyKeyH(INktHookCallInfo callInfo)
        {
            intelligence.cryptDestroyKeyS();
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
                Debug.WriteLine("Error while retrieving process by PID " + ID);
            }
            return process;
        }
    }
}
