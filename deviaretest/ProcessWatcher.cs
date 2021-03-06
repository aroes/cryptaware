﻿using System;
using System.Diagnostics;
using CryptAware;
using Nektra.Deviare2;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

public class ProcessWatcher
{

    private static ProcessWatcher pWatcher;
    internal NktSpyMgr spyMgr;
    private FormInterface UI;
    internal Dictionary<int, HookManager> hManagers;

    public ProcessWatcher()
    {
        pWatcher = this;
        this.UI = FormInterface.GetInstance();
        //Initialize spy manager
        spyMgr = new NktSpyMgr();
        spyMgr.Initialize();
        //Keeps all the hookmanagers with their process IDs
        hManagers = new Dictionary<int, HookManager>();
    }

    public static ProcessWatcher GetInstance()
    {
        return pWatcher;
    }
  

    public void StartService()
    {
        spyMgr.OnProcessStarted += HandleStartedProcess;
        spyMgr.OnProcessTerminated += HandleTerminatedProcess;
        spyMgr.OnFunctionCalled += OnFunctionCalled;

    }


    public void Stop()
    {
        spyMgr.OnProcessStarted -= HandleStartedProcess;
        spyMgr.OnProcessTerminated -= HandleTerminatedProcess;
        spyMgr.OnFunctionCalled -= OnFunctionCalled;

    }

    [DllImport("kernel32.dll")]
    static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
    [DllImport("kernel32.dll")]
    static extern uint SuspendThread(IntPtr hThread);
    [DllImport("kernel32.dll")]
    static extern uint ResumeThread(IntPtr hThread);
    [DllImport("kernel32.dll")]
    static extern Boolean CloseHandle(IntPtr handle);

    [MTAThread]
    private void HandleStartedProcess(NktProcess createdProcess)
    {
        int maxImageSize = 15728640;
        //Restrict hooking to 32 bit processes; possible 64 bit support later. 
        //AND ignore if size >15mb (all ransomware is small; even injected processes, explorer and svchost are <15mb)
        long imageSize = new FileInfo(createdProcess.Path).Length;
        if (createdProcess.PlatformBits == 32 && imageSize < maxImageSize)
        {
            //Suspend process:
            //Get all of the process's threads
            ProcessThreadCollection threads = Process.GetProcessById(createdProcess.Id).Threads;
            //This list will hold all the handles to the opened threads
            List<IntPtr> threadHandles = new List<IntPtr>();
            //Suspend all threads and populate the handle list
            foreach (ProcessThread t in threads)
            {
                IntPtr hThread = OpenThread(0, false, (uint)t.Id);
                SuspendThread(hThread);
                threadHandles.Add(hThread);
            }

            //If file is not whitelisted, proceed to hook
            if (!File.Exists(".\\whitelist.wca") ||
                !File.ReadAllText(".\\whitelist.wca").Contains(createdProcess.Path, StringComparison.OrdinalIgnoreCase))
            {
                //Hook process:
                //Make a new hookmanager for the process
                HookManager hm = new HookManager(createdProcess);
                //Add it to the list of hookmanagers
                hManagers.Add(createdProcess.Id, hm);
                //Install hooks
                hm.InstallHooks();
                Debug.WriteLine("Hooked " + createdProcess.Name + ' ' + createdProcess.Id + " DateTime:" + DateTime.Now);
            }

            //Resume all threads
            foreach (IntPtr hThread in threadHandles)
            {
                ResumeThread(hThread);
                CloseHandle(hThread);
            }
        }
    }

    [MTAThread]
    private void HandleTerminatedProcess(NktProcess terminatedProcess)
    {
        //All the subsequent calls can fail safely
        if (terminatedProcess.PlatformBits == 32)
        {
            try
            {
                hManagers[terminatedProcess.Id].intelligence.killTimer();
            }
            catch (Exception ex) when (ex is NullReferenceException || ex is KeyNotFoundException)
            {
                Debug.WriteLine("Timer release failed");
            }
            File.Delete(terminatedProcess.Id.ToString() + ".mca");
            Debug.WriteLine("Terminated " + terminatedProcess.Name + ' ' + terminatedProcess.Id + " DateTime:" + DateTime.Now);
        }
    }

    [MTAThread]
    //When a hooked function executes (reflection)
    private void OnFunctionCalled(NktHook hook, INktProcess proc, INktHookCallInfo callInfo)
    {
        //Call the function specific handler from string
        //1:Split function name to the right of '!' and add the handler tag
        string mn = hook.FunctionName.Substring(hook.FunctionName.LastIndexOf('!') + 1) + 'H';
        //2:Lowercase first letter
        mn = Char.ToLowerInvariant(mn[0]) + mn.Substring(1);
        //3:Invoke
        try
        {
            //3.1:Get correct hookmanager
            HookManager h = hManagers[proc.Id];
            //3.2:Get its function handler
            MethodInfo mi = h.GetType().GetMethod(mn, BindingFlags.Instance | BindingFlags.NonPublic);
            Object[] funcParams = { callInfo };
            mi.Invoke(h, funcParams);
        }
        catch (NullReferenceException)
        {
            Debug.WriteLine(mn + " has no handler");
        }
    }



}

