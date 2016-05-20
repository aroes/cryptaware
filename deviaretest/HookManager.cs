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

        private FormInterface UI;
        private ProcessWatcher pw;
        private IntelliMod intelligence;

        //The process associated with this hookmanager
        private NktProcess process;
        //The PID of the process associated with this hookmanager
        public int ID;

        public HookManager(NktProcess process)
        {
            this.process = process;
            this.ID = process.Id;
            this.UI = FormInterface.GetInstance();
            this.pw = ProcessWatcher.GetInstance();
            intelligence = new IntelliMod(process);
        }




        //Installs the required hooks
        public void InstallHooks()
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

                InstallFunctionHook("kernel32.dll!GetComputerNameA");
                InstallFunctionHook("kernel32.dll!GetComputerNameW");
                InstallFunctionHook("kernel32.dll!GetComputerNameExA");
                InstallFunctionHook("kernel32.dll!GetComputerNameExW");

                InstallFunctionHook("kernel32.dll!SuspendThread");

                InstallFunctionHook("kernel32.dll!CreateRemoteThread");
                InstallFunctionHook("kernel32.dll!CreateRemoteThreadEx");

                InstallFunctionHook("kernel32.dll!FindFirstFileA");
                InstallFunctionHook("kernel32.dll!FindFirstFileW");
                InstallFunctionHook("kernel32.dll!FindFirstFileExA");
                InstallFunctionHook("kernel32.dll!FindFirstFileExW");

                InstallFunctionHook("kernel32.dll!WriteFile");


                if (UI.debugCheckBox.Checked)
                {
                    //Display the new process on the UI
                    FormInterface.listViewAddItem(UI.processListView, process.Name + ' ' + process.PlatformBits);
                }
                Debug.WriteLine("Success");
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine("Hooking failed: Process no longer exists");
            }

        }

        private void InstallFunctionHook(string functionName)
        {
            //Removed this flag eNktHookFlags.flgOnly32Bits
            //Create hook for the given function
            NktHook hook = pw.spyMgr.CreateHook(functionName, (int)eNktHookFlags.flgOnlyPreCall);
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

        private void suspendThreadH(INktHookCallInfo callInfo)
        {
            intelligence.suspendThreadS();
        }

        private void createRemoteThreadH(INktHookCallInfo callInfo)
        {
            intelligence.createRemoteThreadS();
        }
        private void createRemoteThreadExH(INktHookCallInfo callInfo)
        {
            createRemoteThreadH(callInfo);
        }

        //Filters by path
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
            //Path to search
            string path = callInfo.Params().GetAt(0).Value;
            //Distiguishes between 2 methods of scanning:
            //1:Search for all files, filter later
            if (path.EndsWith("*"))
            {
                intelligence.findFirstFileS();
            }
            //2:Search for each extension separately
            if (path.EndsWith("*.txt")) {
                intelligence.findFirstFileTxtS();
            }
        }

        private void writeFileH(INktHookCallInfo callInfo)
        {
            intelligence.writeFileS();
        }

        private void deleteFileAH(INktHookCallInfo callInfo)
        {
            deleteFileH(callInfo);
        }
        private void deleteFileWH(INktHookCallInfo callInfo)
        {
            deleteFileH(callInfo);
        }
        private void deleteFileH(INktHookCallInfo callInfo)
        {
            intelligence.deleteFileS();
        }
        #endregion


        //Get NktProcess by name (maybe outdated method)
        public NktProcess GetProcess(string processName)
        {
            NktProcessesEnum enumProcess = pw.spyMgr.Processes();
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
        public static NktProcess GetProcess(int ID)
        {
            NktProcessesEnum enumProcess = ProcessWatcher.GetInstance().spyMgr.Processes();
            NktProcess process = enumProcess.GetById(ID);
            if (process == null)
            {
                Debug.WriteLine("Error while retrieving process by PID " + ID);
            }
            return process;
        }
    }
}
