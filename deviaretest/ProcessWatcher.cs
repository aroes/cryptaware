using System;
using System.ComponentModel;
using System.Collections;
using System.Globalization;
using System.Management;
using System.Diagnostics;
using deviaretest;
using Nektra.Deviare2;
using static deviaretest.HookManager;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections.Generic;

public class ProcessWatcher
{
    private static ProcessWatcher pWatcher;
    internal NktSpyMgr spyMgr;
    private FormInterface UI;
    private Dictionary<int,HookManager> hManagers;
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


    private void HandleStartedProcess(NktProcess createdProcess)
    {
        //int bits = createdProcess.PlatformBits;
        HookManager hm = new HookManager(createdProcess);
        hManagers.Add(createdProcess.Id, hm);
        if (hm.InstallHooks() >= 0)
        {
            Debug.WriteLine("Success");
        }
        else
        {
            Debug.WriteLine("Hooking failed: Process no longer exists");
        }

        Debug.WriteLine("Created " + createdProcess.Name + ' ' + createdProcess.Id + " DateTime:" + DateTime.Now);
    }

    private void HandleTerminatedProcess(NktProcess terminatedProcess)
    {
        hManagers.Remove(terminatedProcess.Id);
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

