using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;

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

        #region Events
        private void startButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;
            startButton.BackgroundImage = Properties.Resources.eyecg;
            procWatcher.StartService();

        }

        private void debugCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(debugCheckBox.Checked == false)
            {
                processListView.Items.Clear();
                calledFListView.Items.Clear();
                signsListView.Items.Clear();
            }
        }

        #endregion

        private void FormInterface_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Confirm exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void FormInterface_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Clean temp files
            foreach (string file in Directory.GetFiles(".\\", "*.mca").Where(item => item.EndsWith(".mca")))
            {
                File.Delete(file);
            }
        }


    }
}
