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
    public partial class FormInterface : Form
    {
        private ProcessWatcher procWatcher;
        private static FormInterface UI;

        public FormInterface()
        {
            InitializeComponent();
            UI = this;

            //Initialize process creation watcher
            procWatcher = new ProcessWatcher();
            //procWatcher.ProcessCreated += new WMI.Win32.ProcessEventHandler(WMI.Win32.ProcessWatcher.procWatcher_ProcessCreated);

        }

        public static FormInterface GetInstance()
        {
            return UI;
        }



        private void FormInterface_Load(object sender, EventArgs e)
        {


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
