using Nektra.Deviare2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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

        //The process associated with this hookmanager
        private NktProcess process;
        //The PID of the process associated with this hookmanager
        public int ID;

        public HookManager(NktSpyMgr spyMgr, NktProcess process)
        {
            this.spyMgr = spyMgr;
            this.process = process;
            this.ID = process.Id;
            this.UI = FormInterface.GetInstance();
            intelligence = new IntelliMod();
            //spyMgr.OnFunctionCalled += new DNktSpyMgrEvents_OnFunctionCalledEventHandler(OnFunctionCalled);
        }




        //Installs the required hooks and initialises intelligence
        public int InstallHooks()
        {
            try
            {
                Debug.WriteLine("Installing hooks in " + process.Name);

                //Install each function hook

                //InstallFunctionHook("kernel32.dll!CreateFileA", process);
                //InstallFunctionHook("kernel32.dll!CreateFileW", process);


                InstallFunctionHook("advapi32.dll!RegCreateKeyExA");
                InstallFunctionHook("advapi32.dll!RegCreateKeyExW");

                InstallFunctionHook("advapi32.dll!CryptAcquireContextA");
                InstallFunctionHook("advapi32.dll!CryptAcquireContextW");

                InstallFunctionHook("advapi32.dll!CryptImportKey");

                InstallFunctionHook("advapi32.dll!CryptGenKey");

                InstallFunctionHook("advapi32.dll!CryptEncrypt");

                InstallFunctionHook("advapi32.dll!CryptExportKey");

                InstallFunctionHook("advapi32.dll!CryptDestroyKey");

                //InstallFunctionHook("advapi32.dll!CryptGenRandom");

                InstallFunctionHook("kernel32.dll!GetComputerNameA");
                InstallFunctionHook("kernel32.dll!GetComputerNameW");
                InstallFunctionHook("kernel32.dll!GetComputerNameExA");
                InstallFunctionHook("kernel32.dll!GetComputerNameExW");

                InstallFunctionHook("kernel32.dll!CreateRemoteThread");
                InstallFunctionHook("kernel32.dll!CreateRemoteThreadEx");

                InstallFunctionHook("kernel32.dll!FindFirstFileA");
                InstallFunctionHook("kernel32.dll!FindFirstFileW");
                InstallFunctionHook("kernel32.dll!FindFirstFileExA");
                InstallFunctionHook("kernel32.dll!FindFirstFileExW");

                //Normally the intelligence module is specific to each process
                intelligence.setProcess(process.Id);

                if (UI.debugCheckBox.Checked)
                {
                    //Display the new process on the UI
                    FormInterface.listViewAddItem(UI.processListView, process.Name + ' ' + process.PlatformBits);
                }
                return 0;
            }
            catch (NullReferenceException)
            {
                return -1;
            }

        }

        private void InstallFunctionHook(string functionName)
        {
            //Removed this flag eNktHookFlags.flgOnly32Bits
            //Create hook for the given function
            NktHook hook = spyMgr.CreateHook(functionName, (int)eNktHookFlags.flgOnlyPreCall);
            //Event watcher

            //Activate the hook
            hook.Hook(true);
            hook.Attach(process, true);
        }


        #region Function call handlers

        //Send suspicious ... calls to intelligence
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

        //Send suspicious ... calls to intelligence
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
            string csp = callInfo.Params().GetAt(2).Value;
            if (csp.Contains("Microsoft Enhanced RSA and AES Cryptographic Provider"))
            {
                intelligence.cryptAcquireContextS();
            }
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

        //private void cryptGenRandomH(INktHookCallInfo callInfo)
        //{
        //    if (callInfo.Params().GetAt(1).Value <= 16)
        //    {
        //        intelligence.cryptGenRandomS();
        //    }
        //}

        private void getComputerNameAH(INktHookCallInfo callInfo)
        {
            getComputerNameH(callInfo);
        }
        private void getComputerNameWH(INktHookCallInfo callInfo)
        {
            getComputerNameH(callInfo);
        }
        private void getComputerNameExAH(INktHookCallInfo callInfo)
        {
            getComputerNameH(callInfo);
        }
        private void getComputerNameExWH(INktHookCallInfo callInfo)
        {
            getComputerNameH(callInfo);
        }
        private void getComputerNameH(INktHookCallInfo callInfo)
        {
            intelligence.getComputerNameS();
        }

        private void createRemoteThreadH(INktHookCallInfo callInfo)
        {
            intelligence.createRemoteThreadS();
        }
        private void createRemoteThreadExH(INktHookCallInfo callInfo)
        {
            createRemoteThreadH(callInfo);
        }

        private void findFirstFileAH(INktHookCallInfo callInfo)
        {
            findFirstFileH(callInfo);
        }
        private void findFirstFileWH(INktHookCallInfo callInfo)
        {
            findFirstFileH(callInfo);
        }
        private void findFirstFileExAH(INktHookCallInfo callInfo)
        {
            findFirstFileH(callInfo);
        }
        private void findFirstFileExWH(INktHookCallInfo callInfo)
        {
            findFirstFileH(callInfo);
        }
        private void findFirstFileH(INktHookCallInfo callInfo)
        {
            string path = callInfo.Params().GetAt(0).Value;
            if (path.EndsWith("*"))
            {
                intelligence.findFirstFileS();
            }
        }
        #endregion


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

        //Get NktProcess by its PID (likely useless now)
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
