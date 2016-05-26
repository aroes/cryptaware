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


        #region Thread safe UI update
        //Add item by string
        public static void listViewAddItem(ListView varListView, string text)
        {
            if (varListView.InvokeRequired)
            {
                varListView.BeginInvoke(new MethodInvoker(() => listViewAddItem(varListView, text)));
            }
            else
            {
                ListViewItem item = new ListViewItem(text);
                varListView.Items.Add(item);
            }
        }
        //Add item by string with name
        public static void listViewAddItem(ListView varListView, string text, string name)
        {
            if (varListView.InvokeRequired)
            {
                varListView.BeginInvoke(new MethodInvoker(() => listViewAddItem(varListView, text, name)));
            }
            else
            {
                ListViewItem item = new ListViewItem(text);
                item.Name = name;
                varListView.Items.Add(item);
            }
        }

        //Add item row
        public static void listViewAddItemRange(ListView varListView, string s, string[] row)
        {
            if (varListView.InvokeRequired)
            {
                varListView.BeginInvoke(new MethodInvoker(() => listViewAddItemRange(varListView, s, row)));
            }
            else
            {
                ListViewItem item = new ListViewItem(s);
                varListView.Items.Add(item).SubItems.AddRange(row);
            }
        }
        //Add item row with name
        public static void listViewAddItemRange(ListView varListView, string s, string[] row, string name)
        {
            if (varListView.InvokeRequired)
            {
                varListView.BeginInvoke(new MethodInvoker(() => listViewAddItemRange(varListView, s, row, name)));
            }
            else
            {
                ListViewItem item = new ListViewItem(s);
                item.Name = name;
                varListView.Items.Add(item).SubItems.AddRange(row);
            }
        }

        //Delete item by key (name
        public static void listViewDelItem(ListView varListView, string key)
        {
            if (varListView.InvokeRequired)
            {
                varListView.BeginInvoke(new MethodInvoker(() => listViewDelItem(varListView, key)));
            }
            else
            {
                varListView.Items.RemoveByKey(key);
            }
        }

        #endregion

        #region Events
        private void startButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;
            startButton.BackgroundImage = Properties.Resources.eyecg;
            procWatcher.StartService();

        }

        private void debugCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (debugCheckBox.Checked == false)
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
        

        private void processListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            signCountListView.Items.Clear();
            if (e.IsSelected)
            {
                ListViewItem item = e.Item;
                string pid = item.Name;
                procWatcher.hManagers[Convert.ToInt32(pid)].intelligence.displaySigns();
            }
        }

        private void signCountListView_Click(object sender, EventArgs e)
        {
            signCountListView.Items.Clear();
            ListView lv = processListView;
            if (lv.SelectedItems.Count > 0)
            {
                string pid = lv.SelectedItems[0].Name;
                procWatcher.hManagers[Convert.ToInt32(pid)].intelligence.displaySigns();
            }
        }
    }
}
