using System;
using System.ComponentModel;
using System.Collections;
using System.Globalization;
using System.Management;
using System.Diagnostics;
using deviaretest;
using Nektra.Deviare2;
using static deviaretest.HookManager;


public class ProcessWatcher
{
    private static ProcessWatcher pWatcher;
    private ManagementEventWatcher startWatch;
    private NktSpyMgr spyMgr;


    public ProcessWatcher()
    {
        pWatcher = this;
        Init();

        //Initialize spy manager
        spyMgr = new NktSpyMgr();
        spyMgr.Initialize();

    }

    public static ProcessWatcher GetInstance()
    {
        return pWatcher;
    }

    private static void Init()
    {
        ProcessWatcher.GetInstance().startWatch = new ManagementEventWatcher(@"SELECT * FROM 
                    __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'");
    }

    public void Start()
    {
        startWatch.EventArrived += startWatch_EventArrived;
        startWatch.Start();
    }

    public void Stop()
    {
        startWatch.EventArrived -= startWatch_EventArrived;
        startWatch.Stop();
    }

    //Executed at any process creation
    private static void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
    {
        ManagementBaseObject obj = e.NewEvent["TargetInstance"] as ManagementBaseObject;
        Debug.WriteLine("Process started: {0}", obj.Properties["ProcessId"].Value);
        Debug.WriteLine("Created " + obj.Properties["Name"].Value + " " + obj.Properties["ProcessId"].Value + " " + "DateTime:" + DateTime.Now);
        HookManager hm = new HookManager(ProcessWatcher.GetInstance().spyMgr);
        NktProcess createdProcess = hm.GetProcess(Convert.ToInt32(obj.Properties["ProcessId"].Value));
        int status = hm.InstallHooks(createdProcess);
        if (status >= 0)
        {
            Debug.WriteLine("Success");
        }
        else
        {
            Debug.WriteLine("Hooking failed: Process no longer exists");
        }
    }

}

