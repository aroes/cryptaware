using Nektra.Deviare2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

                InstallFunctionHook("kernel32.dll!CreateFileA");
                InstallFunctionHook("kernel32.dll!CreateFileW");

                InstallFunctionHook("kernel32.dll!FindFirstFileA");
                InstallFunctionHook("kernel32.dll!FindFirstFileW");
                InstallFunctionHook("kernel32.dll!FindFirstFileExA");
                InstallFunctionHook("kernel32.dll!FindFirstFileExW");

                //Consider adding WriteFileEx
                InstallFunctionHook("kernel32.dll!WriteFile");

                InstallFunctionHook("kernel32.dll!DeleteFileA");
                InstallFunctionHook("kernel32.dll!DeleteFileW");

                InstallFunctionHook("kernel32.dll!WinExec");

                InstallFunctionHook("kernel32.dll!CreateProcessA");
                InstallFunctionHook("kernel32.dll!CreateProcessW");



                if (UI.debugCheckBox.Checked)
                {
                    string text = process.Name + ' ' + process.PlatformBits;
                    //Display the new process on the UI
                    FormInterface.listViewAddItem(UI.processListView, text, process.Id.ToString());
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

        //Only send startup installs to intelligence
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

        //Only send if context is MS Enhanced RSA and AES
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

        //Send all
        private void cryptImportKeyH(INktHookCallInfo callInfo)
        {
            intelligence.cryptImportKeyS();
        }

        //Send all
        private void cryptGenKeyH(INktHookCallInfo callInfo)
        {
            intelligence.cryptGenKeyS();
        }

        //Send all
        private void cryptEncryptH(INktHookCallInfo callInfo)
        {
            intelligence.cryptEncryptS();
        }

        //Send all
        private void cryptExportKeyH(INktHookCallInfo callInfo)
        {
            intelligence.cryptExportKeyS();
        }

        //Send all
        private void cryptDestroyKeyH(INktHookCallInfo callInfo)
        {
            intelligence.cryptDestroyKeyS();
        }

        //Send all
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

        //Send all
        private void suspendThreadH(INktHookCallInfo callInfo)
        {
            intelligence.suspendThreadS();
        }

        //Send all
        private void createRemoteThreadH(INktHookCallInfo callInfo)
        {
            intelligence.createRemoteThreadS();
        }
        private void createRemoteThreadExH(INktHookCallInfo callInfo)
        {
            createRemoteThreadH(callInfo);
        }

        //Only send when path doesn't include \appdata\
        private void createFileAH(INktHookCallInfo callInfo)
        {
            createFileH(callInfo);
        }
        private void createFileWH(INktHookCallInfo callInfo)
        {
            createFileH(callInfo);
        }
        private void createFileH(INktHookCallInfo callInfo)
        {
            string path = callInfo.Params().GetAt(0).Value;
            if (!path.Contains("\\appdata\\", StringComparison.OrdinalIgnoreCase))
            {
                intelligence.createFileS();
            }
        }

        //Directory wide searches. Only send when path doesnt include \appdata\ 
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
            if (path.EndsWith("*") && !path.Contains("\\appdata\\", StringComparison.OrdinalIgnoreCase))
            {
                intelligence.findFirstFileS();
            }
            //2:Search for each extension separately
            if (path.EndsWith("*.txt") && !path.Contains("\\appdata\\", StringComparison.OrdinalIgnoreCase))
            {
                intelligence.findFirstFileTxtS();
            }
        }

        //Checks entropy of buffer, and that path is not REG or appdata
        private void writeFileH(INktHookCallInfo callInfo)
        {
            //Get written path from file handle
            NktTools tool = new NktTools();
            string path = tool.GetFileNameFromHandle(callInfo.Params().GetAt(0).PointerVal, callInfo.Process());

            //If path is relevant
            if (!path.Contains("\\appdata\\", StringComparison.OrdinalIgnoreCase) &&
                !path.Contains("\\REGISTRY\\"))
            {
                INktParam pBuf = callInfo.Params().GetAt(1); //Data to write
                INktParam pBytes = callInfo.Params().GetAt(2); //Length of data

                uint bytesToWrite = pBytes.ULongVal;
                double entropy = 0;
                if (pBuf.PointerVal != IntPtr.Zero && bytesToWrite > 0)
                {
                    INktProcessMemory procMem = process.Memory();
                    byte[] buffer = new byte[bytesToWrite];
                    GCHandle pinnedBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    IntPtr pDest = pinnedBuffer.AddrOfPinnedObject();
                    procMem.ReadMem(pDest, pBuf.PointerVal, (IntPtr)bytesToWrite);
                    pinnedBuffer.Free();
                    var str = System.Text.Encoding.UTF8.GetString(buffer);
                    //Get per-byte entropy
                    entropy = getEntropy(buffer);
                }
                if (entropy > 6)
                {
                    intelligence.writeFileS();
                }
            }
        }

        private double getEntropy(byte[] buffer)
        {
            int[] counts = new int[256];
            foreach (byte b in buffer)
            {
                counts[(int)b] += 1;
            }
            double entropy = 0.0;
            int len = buffer.Length;
            foreach (int c in counts)
            {
                double frequency = (double)c / len;
                if (frequency > 0)
                {
                    entropy -= frequency * (Math.Log(frequency) / Math.Log(2));
                }
            }
            return entropy;
        }

        //Checks path
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
            string path = callInfo.Params().GetAt(0).Value;
            if (!path.Contains("\\appdata\\", StringComparison.OrdinalIgnoreCase))
            {
                intelligence.deleteFileS();
            }
        }

        //Checks if vssadmin or bcdedit
        private void winExecH(INktHookCallInfo callInfo)
        {
            string path = callInfo.Params().GetAt(0).Value;
            if (path.Contains("vssadmin", StringComparison.OrdinalIgnoreCase) ||
                path.Contains("bcdedit", StringComparison.OrdinalIgnoreCase))
            {
                intelligence.createProcessS();
            }
        }

        //Checks if vssadmin or bcdedit
        private void createProcessAH(INktHookCallInfo callInfo)
        {
            createProcessH(callInfo);
        }
        private void createProcessWH(INktHookCallInfo callInfo)
        {
            createProcessH(callInfo);
        }
        private void createProcessH(INktHookCallInfo callInfo)
        {
            string path = callInfo.Params().GetAt(0).Value;
            string cmd = callInfo.Params().GetAt(1).Value;
            if (path.Contains("vssadmin", StringComparison.OrdinalIgnoreCase) ||
                path.Contains("bcdedit", StringComparison.OrdinalIgnoreCase) ||
                cmd.Contains("vssadmin", StringComparison.OrdinalIgnoreCase) ||
                cmd.Contains("bcdedit", StringComparison.OrdinalIgnoreCase))
            {
                intelligence.createProcessS();
            }
        }
        #endregion


    }
}
