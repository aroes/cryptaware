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
            

        }

        public static FormInterface GetInstance()
        {
            return UI;
        }



        private void FormInterface_Load(object sender, EventArgs e)
        {


        }


        //Thread safe UI update
        public static void listViewAddItem(ListView varListView, string s)
        {
            ListViewItem item = new ListViewItem(s);
            if (varListView.InvokeRequired)
            {
                varListView.BeginInvoke(new MethodInvoker(() => listViewAddItem(varListView, s)));
            }
            else
            {
                varListView.Items.Add(item);
            }
        }

        public static void listViewAddItemRange(ListView varListView, string s, string[] row)
        {
            ListViewItem item = new ListViewItem(s);
            if (varListView.InvokeRequired)
            {
                varListView.BeginInvoke(new MethodInvoker(() => listViewAddItemRange(varListView, s, row)));
            }
            else
            {
                varListView.Items.Add(item).SubItems.AddRange(row);
            }
        }


        private void MonitorNewProcessesButton_Click(object sender, EventArgs e)
        {
            MonitorNewProcessesButton.Enabled = false;


            //procWatcher.Start();
            StopMonitorButton.Enabled = true;

        }

        private void StopMonitorButton_Click(object sender, EventArgs e)
        {
            StopMonitorButton.Enabled = false;


            //procWatcher.Stop();
            MonitorNewProcessesButton.Enabled = true;
        }
    }
}
