﻿using System;
using System.Diagnostics;
using deviaretest;
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
    private Dictionary<int, HookManager> hManagers;
    private ManualResetEvent shutdownDeviareEvent = new ManualResetEvent(false);
    private ManualResetEvent deviareInitializedEvent = new ManualResetEvent(false);
    private Thread deviareWorker;

    public ProcessWatcher()
    {
        pWatcher = this;
        this.UI = FormInterface.GetInstance();
        InitPWatch();
        //Initialize spy manager
        spyMgr = new NktSpyMgr();
        //Keeps all the hookmanagers with their process IDs
        hManagers = new Dictionary<int, HookManager>();
        StartDeviareWorker();

    }

    public static ProcessWatcher GetInstance()
    {
        return pWatcher;
    }
    public Action<INktProcess> ProcessStartedHandler { get; set; }

    private void InitPWatch()
    {
        ProcessStartedHandler = process => { };
    }
    private void StartDeviareWorker()
    {
        deviareWorker = new Thread(StartDeviare);
        deviareWorker.SetApartmentState(ApartmentState.MTA);
        deviareWorker.Name = "Deviare API Thread";
        deviareWorker.Start();

        deviareInitializedEvent.WaitOne();
    }

    private void StartDeviare()
    {
        Start();
        WaitForShutdownRequest();
        Stop();
    }

    private void Start()
    {
        spyMgr.Initialize();

        spyMgr.OnProcessStarted += HandleStartedProcess;
        spyMgr.OnProcessTerminated += HandleTerminatedProcess;
        spyMgr.OnFunctionCalled += OnFunctionCalled;



        deviareInitializedEvent.Set();
    }

    private void WaitForShutdownRequest()
    {
        shutdownDeviareEvent.WaitOne();
    }

    private void Stop()
    {
        spyMgr.OnProcessStarted -= HandleStartedProcess;
        spyMgr.OnProcessTerminated -= HandleTerminatedProcess;
        spyMgr.OnFunctionCalled -= OnFunctionCalled;


        try
        {
            Marshal.ReleaseComObject(spyMgr);
        }
        catch
        {

        }
    }

    [DllImport("kernel32.dll")]
    static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

    [DllImport("kernel32.dll")]
    static extern uint SuspendThread(IntPtr hThread);
    [DllImport("kernel32.dll")]
    static extern uint ResumeThread(IntPtr hThread);

    private void HandleStartedProcess(NktProcess createdProcess)
    {
        //Restrict hooking to 32 bit processes; possible 64 bit support later
        if (createdProcess.PlatformBits == 32)
        {
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
            //Make a new hookmanager for the process
            HookManager hm = new HookManager(createdProcess);
            //Add it to the list of hookmanagers
            hManagers.Add(createdProcess.Id, hm);
            //Install hooks
            hm.InstallHooks();
            //Resume all threads
            foreach (IntPtr hThread in threadHandles)
            {
                ResumeThread(hThread);
            }
            Debug.WriteLine("Hooked " + createdProcess.Name + ' ' + createdProcess.Id + " DateTime:" + DateTime.Now);
        }
    }

    private void HandleTerminatedProcess(NktProcess terminatedProcess)
    {
        hManagers.Remove(terminatedProcess.Id);
        File.Delete(terminatedProcess.Id.ToString() + ".mca");
        Debug.WriteLine("Terminated " + terminatedProcess.Name + ' ' + terminatedProcess.Id + " DateTime:" + DateTime.Now);
    }

    //When a hooked function executes
    private void OnFunctionCalled(NktHook hook, INktProcess proc, INktHookCallInfo callInfo)
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

