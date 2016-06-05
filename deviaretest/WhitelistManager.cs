using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CryptAware
{
    public partial class WhitelistManager : Form
    {

        public WhitelistManager()
        {
            InitializeComponent();
        }

        private void WhitelistManager_Load(object sender, EventArgs e)
        {
            refreshList();
        }

        private void WhitelistManager_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormInterface.GetInstance().whiteListButton.Enabled = true;
        }
        #region Button events

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void minusButton_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selected = pathsListView.SelectedItems;
            if (selected.Count != 0 && File.Exists(".\\whitelist.wca"))
            {
                List<string> lines = new List<string>(File.ReadAllLines(".\\whitelist.wca"));
                foreach (ListViewItem item in selected)
                {
                    lines.Remove(item.Text);
                }
                File.WriteAllLines(".\\whitelist.wca", lines);
            }
            refreshList();
        }

        private void plusButton_Click(object sender, EventArgs e)
        {
            DialogResult r = openFileDialog.ShowDialog();

            // Process input if the user clicked OK.
            if (r == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                //Add path to whitelist file
                if (!File.Exists(".\\whitelist.wca") || !File.ReadAllText(".\\whitelist.wca").Contains(path, StringComparison.OrdinalIgnoreCase))
                {
                    StreamWriter sw = File.AppendText(".\\whitelist.wca");
                    sw.WriteLine(path);
                    sw.Close();
                }
            }
            refreshList();
        }
        #endregion

        //Refresh items from file
        private void refreshList()
        {
            pathsListView.Items.Clear();
            if (File.Exists(".\\whitelist.wca")) {
                foreach (string line in File.ReadAllLines(".\\whitelist.wca"))
                {
                    pathsListView.Items.Add(line);
                }
            }
        }
    }
}

