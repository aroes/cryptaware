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
        ProcessWatcher.GetInstance().startWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
        ProcessWatcher.GetInstance().startWatch.EventArrived += new EventArrivedEventHandler(startWatch_EventArrived);
    }

    public void Start()
    {
        startWatch.Start();
    }

    public void Stop()
    {
        startWatch.Stop();
    }

    private static void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
    {
        Debug.WriteLine("Process started: {0}", e.NewEvent.Properties["ProcessId"].Value);
        Debug.WriteLine("Created " + e.NewEvent.Properties["ProcessName"].Value + " " + e.NewEvent.Properties["ProcessId"].Value + " " + "DateTime:" + DateTime.Now);
        HookManager hm = new HookManager(ProcessWatcher.GetInstance().spyMgr);
        NktProcess createdProcess = hm.GetProcess(Convert.ToInt32(e.NewEvent.Properties["ProcessId"].Value));
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

