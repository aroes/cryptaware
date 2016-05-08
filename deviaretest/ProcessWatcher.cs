using System;
using System.ComponentModel;
using System.Collections;
using System.Globalization;
using System.Management;
using System.Diagnostics;
using deviaretest;
using Nektra.Deviare2;

namespace WMI.Win32
{
    public delegate void ProcessEventHandler(Win32_Process proc);
    public class ProcessWatcher : ManagementEventWatcher
    {
        // Process creation event
        public event ProcessEventHandler ProcessCreated;


        // WMI WQL process query strings
        static readonly string WMI_OPER_EVENT_QUERY = @"SELECT * FROM 
                __InstanceOperationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'";
        static readonly string WMI_OPER_EVENT_QUERY_WITH_PROC =
            WMI_OPER_EVENT_QUERY + " and TargetInstance.Name = '{0}'";

        public ProcessWatcher()
        {
            Init(string.Empty);
        }

        public ProcessWatcher(string processName)
        {
            Init(processName);
        }

        private void Init(string processName)
        {
            this.Query.QueryLanguage = "WQL";
            if (string.IsNullOrEmpty(processName))
            {
                this.Query.QueryString = WMI_OPER_EVENT_QUERY;
            }
            else
            {
                this.Query.QueryString =
                    string.Format(WMI_OPER_EVENT_QUERY_WITH_PROC, processName);
            }

            this.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
        }

        private void watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string eventType = e.NewEvent.ClassPath.ClassName;
            Win32_Process proc = new
                Win32_Process(e.NewEvent["TargetInstance"] as ManagementBaseObject);

            if (eventType == "__InstanceCreationEvent")
            {
                ProcessCreated?.Invoke(proc);
            }
        }

        //Handler for 'created process' event
        public static void procWatcher_ProcessCreated(Win32_Process process)
        {
            Debug.WriteLine("Created " + process.Name + " " + process.ProcessId + " " + "DateTime:" + DateTime.Now);
            NktProcess createdProcess = HookManager.GetProcess(process.Name);
            HookManager.InstallHooks(createdProcess);
        }

    }
}
