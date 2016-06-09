namespace CryptAware
{
    partial class WhitelistManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WhitelistManager));
            this.pathsListView = new System.Windows.Forms.ListView();
            this.pathColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.closeButton = new System.Windows.Forms.Button();
            this.plusButton = new System.Windows.Forms.Button();
            this.minusButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pathsListView
            // 
            this.pathsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.pathColumnHeader});
            this.pathsListView.FullRowSelect = true;
            this.pathsListView.Location = new System.Drawing.Point(31, 36);
            this.pathsListView.Name = "pathsListView";
            this.pathsListView.Size = new System.Drawing.Size(709, 303);
            this.pathsListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.pathsListView.TabIndex = 0;
            this.pathsListView.UseCompatibleStateImageBehavior = false;
            this.pathsListView.View = System.Windows.Forms.View.Details;
            // 
            // pathColumnHeader
            // 
            this.pathColumnHeader.Text = "Path";
            this.pathColumnHeader.Width = 600;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Executables|*.exe|All files|*.*";
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(660, 345);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 41);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // plusButton
            // 
            this.plusButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("plusButton.BackgroundImage")));
            this.plusButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.plusButton.Location = new System.Drawing.Point(613, 345);
            this.plusButton.Name = "plusButton";
            this.plusButton.Size = new System.Drawing.Size(41, 41);
            this.plusButton.TabIndex = 2;
            this.plusButton.UseVisualStyleBackColor = true;
            this.plusButton.Click += new System.EventHandler(this.plusButton_Click);
            // 
            // minusButton
            // 
            this.minusButton.BackgroundImage = global::CryptAware.Properties.Resources.minus;
            this.minusButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.minusButton.Location = new System.Drawing.Point(566, 345);
            this.minusButton.Name = "minusButton";
            this.minusButton.Size = new System.Drawing.Size(41, 41);
            this.minusButton.TabIndex = 3;
            this.minusButton.UseVisualStyleBackColor = true;
            this.minusButton.Click += new System.EventHandler(this.minusButton_Click);
            // 
            // WhitelistManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CryptAware.Properties.Resources.formbg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(775, 406);
            this.Controls.Add(this.minusButton);
            this.Controls.Add(this.plusButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.pathsListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WhitelistManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Whitelist manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WhitelistManager_FormClosed);
            this.Load += new System.EventHandler(this.WhitelistManager_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView pathsListView;
        private System.Windows.Forms.ColumnHeader pathColumnHeader;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button plusButton;
        private System.Windows.Forms.Button minusButton;
    }
}