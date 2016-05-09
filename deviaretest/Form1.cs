using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nektra.Deviare2;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace deviaretest

{
    public partial class hooker : Form
    {
        private WMI.Win32.ProcessWatcher procWatcher;
        private static hooker UI;

        public hooker()
        {
            InitializeComponent();
            UI = this;

            //Initialize process creation watcher
            procWatcher = new WMI.Win32.ProcessWatcher();
            procWatcher.ProcessCreated += new WMI.Win32.ProcessEventHandler(WMI.Win32.ProcessWatcher.procWatcher_ProcessCreated);

        }

        public static hooker GetInstance()
        {
            return UI;
        }



        private void hooker_Load(object sender, EventArgs e)
        {


            //NktHook hook = _spyMgr.CreateHook("kernel32.dll!CreateFileW", (int)(eNktHookFlags.flgRestrictAutoHookToSameExecutable | eNktHookFlags.flgOnlyPreCall));

            //_spyMgr.OnFunctionCalled += new DNktSpyMgrEvents_OnFunctionCalledEventHandler(OnFunctionCalled);

            //hook.Hook(true);
            //hook.Attach(_process, true);

        }


        //When a hooked function executes
        void OnFunctionCalled(NktHook hook, INktProcess proc, INktHookCallInfo callInfo)
        {

            Debug.WriteLine("The requested function was called!");

        }



        private void MonitorNewProcessesButton_Click(object sender, EventArgs e)
        {
            MonitorNewProcessesButton.Enabled = false;


            procWatcher.Start();
            StopMonitorButton.Enabled = true;

        }

        private void StopMonitorButton_Click(object sender, EventArgs e)
        {
            StopMonitorButton.Enabled = false;


            procWatcher.Stop();
            MonitorNewProcessesButton.Enabled = true;
        }
    }
}
