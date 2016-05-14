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

        private FormInterface UI;

        private IntelliMod intelligence;

        public HookManager(NktSpyMgr spyMgr)
        {
            this.spyMgr = spyMgr;
            this.UI = FormInterface.GetInstance();
            intelligence = new IntelliMod();

        }




        //Installs the required hooks and activates them and initialises intelligence
        public int InstallHooks(NktProcess process)
        {
            try
            {
                Debug.WriteLine("Installing hooks in " + process.Name);

                //Install each function hook

                //InstallFunctionHook("kernel32.dll!CreateFileA", process);
                //InstallFunctionHook("kernel32.dll!CreateFileW", process);

                //InstallFunctionHook("advapi32.dll!RegOpenKeyExA", process);
                //InstallFunctionHook("advapi32.dll!RegOpenKeyExW", process);

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


            switch (hook.FunctionName)
            {
                case "kernel32.dll!CreateFileW":
                    Debug.WriteLine(DateTime.Now.ToString("h:mm:ss tt :: ") + "CreateFileW call in " + proc.Name + " intercepted");
                    break;
                case "advapi32.dll!RegOpenKeyExA":
                    Debug.WriteLine("RegOpenKey " + callInfo.Params().GetAt(1).ReadString());
                    break;
                case "advapi32.dll!RegOpenKeyExW":
                    Debug.WriteLine("RegOpenKey " + callInfo.Params().GetAt(1).ReadString());
                    break;
                case "advapi32.dll!RegCreateKeyExA":
                    Debug.WriteLine("RegCreateKey " + callInfo.Params().GetAt(1).ReadString());
                    checkStartupInstallation(callInfo.Params().GetAt(1).ReadString());
                    break;
                case "advapi32.dll!RegCreateKeyExW":
                    Debug.WriteLine("RegCreateKey " + callInfo.Params().GetAt(1).ReadString());
                    checkStartupInstallation(callInfo.Params().GetAt(1).ReadString());
                    break;
                case "advapi32.dll!CryptAcquireContextA":
                    cryptAcquireContextH();
                    break;
                case "advapi32.dll!CryptAcquireContextW":
                    cryptAcquireContextH();
                    break;
                case "advapi32.dll!CryptImportKey":
                    cryptImportKeyH();
                    break;
                case "advapi32.dll!CryptGenKey":
                    cryptGenKeyH();
                    break;
                case "advapi32.dll!CryptEncrypt":
                    cryptEncryptH();
                    break;
                case "advapi32.dll!CryptExportKey":
                    cryptExportKeyH();
                    break;
                case "advapi32.dll!CryptDestroyKey":
                    cryptDestroyKeyH();
                    break;
                default:
                    Debug.WriteLine("Something went wrong: the called function has no handler");
                    break;
            }


        }

        private void cryptAcquireContextH()
        {
            intelligence.cryptAcquireContextF();
        }

        private void checkStartupInstallation(string path)
        {
            if (path.Contains("Windows\\CurrentVersion\\Run") || path.Contains("Windows\\CurrentVersion\\RunOnce"))
            {
                intelligence.foundStartup();
                //Display sign on the UI
                if (UI.debugCheckBox.Checked)
                {
                    FormInterface.listViewAddItem(UI.signsListView, "Startup Install");
                }
            }

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
