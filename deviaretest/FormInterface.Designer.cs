namespace CryptAware
{
    partial class FormInterface
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInterface));
            this.processListView = new System.Windows.Forms.ListView();
            this.caProcess = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.stringListView = new System.Windows.Forms.ListView();
            this.suspendRadioButton = new System.Windows.Forms.RadioButton();
            this.killRadioButton = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.whiteListButton = new System.Windows.Forms.Button();
            this.passiveRadioButton = new System.Windows.Forms.RadioButton();
            this.signCountListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.iconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stringLabel = new System.Windows.Forms.Label();
            this.startButton = new RoundButton();
            this.startToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.iconContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // processListView
            // 
            this.processListView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.processListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.processListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.caProcess,
            this.columnHeader4});
            this.processListView.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.processListView.HideSelection = false;
            this.processListView.Location = new System.Drawing.Point(247, 61);
            this.processListView.MultiSelect = false;
            this.processListView.Name = "processListView";
            this.processListView.Size = new System.Drawing.Size(216, 273);
            this.processListView.TabIndex = 2;
            this.processListView.UseCompatibleStateImageBehavior = false;
            this.processListView.View = System.Windows.Forms.View.Details;
            this.processListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.processListView_ItemSelectionChanged);
            // 
            // caProcess
            // 
            this.caProcess.Text = "Process";
            this.caProcess.Width = 156;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "PID";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(292, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tracked Processes";
            this.label1.UseMnemonic = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(623, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Indicators";
            this.label2.UseMnemonic = false;
            // 
            // stringListView
            // 
            this.stringListView.Location = new System.Drawing.Point(15, 351);
            this.stringListView.MultiSelect = false;
            this.stringListView.Name = "stringListView";
            this.stringListView.Size = new System.Drawing.Size(448, 120);
            this.stringListView.TabIndex = 7;
            this.stringListView.UseCompatibleStateImageBehavior = false;
            this.stringListView.View = System.Windows.Forms.View.List;
            this.stringListView.Click += new System.EventHandler(this.stringListView_Click);
            // 
            // suspendRadioButton
            // 
            this.suspendRadioButton.AutoSize = true;
            this.suspendRadioButton.Location = new System.Drawing.Point(0, 19);
            this.suspendRadioButton.Name = "suspendRadioButton";
            this.suspendRadioButton.Size = new System.Drawing.Size(96, 17);
            this.suspendRadioButton.TabIndex = 8;
            this.suspendRadioButton.Text = "Suspend mode";
            this.suspendRadioButton.UseVisualStyleBackColor = true;
            // 
            // killRadioButton
            // 
            this.killRadioButton.AutoSize = true;
            this.killRadioButton.Location = new System.Drawing.Point(0, 42);
            this.killRadioButton.Name = "killRadioButton";
            this.killRadioButton.Size = new System.Drawing.Size(67, 17);
            this.killRadioButton.TabIndex = 9;
            this.killRadioButton.Text = "Kill mode";
            this.killRadioButton.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.whiteListButton);
            this.panel1.Controls.Add(this.passiveRadioButton);
            this.panel1.Controls.Add(this.killRadioButton);
            this.panel1.Controls.Add(this.suspendRadioButton);
            this.panel1.Location = new System.Drawing.Point(15, 228);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(226, 101);
            this.panel1.TabIndex = 10;
            // 
            // whiteListButton
            // 
            this.whiteListButton.Location = new System.Drawing.Point(107, 32);
            this.whiteListButton.Name = "whiteListButton";
            this.whiteListButton.Size = new System.Drawing.Size(101, 37);
            this.whiteListButton.TabIndex = 11;
            this.whiteListButton.Text = "Manage whitelist";
            this.whiteListButton.UseVisualStyleBackColor = true;
            this.whiteListButton.Click += new System.EventHandler(this.whiteListButton_Click);
            // 
            // passiveRadioButton
            // 
            this.passiveRadioButton.AutoSize = true;
            this.passiveRadioButton.Checked = true;
            this.passiveRadioButton.Location = new System.Drawing.Point(0, 65);
            this.passiveRadioButton.Name = "passiveRadioButton";
            this.passiveRadioButton.Size = new System.Drawing.Size(91, 17);
            this.passiveRadioButton.TabIndex = 10;
            this.passiveRadioButton.TabStop = true;
            this.passiveRadioButton.Text = "Passive mode";
            this.passiveRadioButton.UseVisualStyleBackColor = true;
            // 
            // signCountListView
            // 
            this.signCountListView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.signCountListView.AutoArrange = false;
            this.signCountListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.signCountListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.signCountListView.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.signCountListView.FullRowSelect = true;
            this.signCountListView.Location = new System.Drawing.Point(479, 61);
            this.signCountListView.Name = "signCountListView";
            this.signCountListView.Size = new System.Drawing.Size(355, 410);
            this.signCountListView.TabIndex = 12;
            this.signCountListView.UseCompatibleStateImageBehavior = false;
            this.signCountListView.View = System.Windows.Forms.View.Details;
            this.signCountListView.Click += new System.EventHandler(this.signCountListView_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Indicator Name";
            this.columnHeader1.Width = 215;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 55;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Suspicious";
            this.columnHeader3.Width = 85;
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.iconContextMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "CryptAware";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // iconContextMenu
            // 
            this.iconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitMenuItem});
            this.iconContextMenu.Name = "iconContextMenu";
            this.iconContextMenu.Size = new System.Drawing.Size(93, 26);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // stringLabel
            // 
            this.stringLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stringLabel.AutoSize = true;
            this.stringLabel.BackColor = System.Drawing.Color.Transparent;
            this.stringLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stringLabel.Location = new System.Drawing.Point(12, 332);
            this.stringLabel.Name = "stringLabel";
            this.stringLabel.Size = new System.Drawing.Size(91, 16);
            this.stringLabel.TabIndex = 13;
            this.stringLabel.Text = "String analysis";
            this.stringLabel.UseMnemonic = false;
            // 
            // startButton
            // 
            this.startButton.BackgroundImage = global::CryptAware.Properties.Resources.eyec;
            this.startButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.startButton.Location = new System.Drawing.Point(15, 24);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(205, 205);
            this.startButton.TabIndex = 11;
            this.startToolTip.SetToolTip(this.startButton, "Click to start monitoring");
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // FormInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::CryptAware.Properties.Resources.formbg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(862, 491);
            this.Controls.Add(this.stringLabel);
            this.Controls.Add(this.signCountListView);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.stringListView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.processListView);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormInterface";
            this.Text = "CryptAware";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormInterface_Closing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormInterface_FormClosed);
            this.Load += new System.EventHandler(this.FormInterface_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.iconContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ListView processListView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader caProcess;
        public System.Windows.Forms.ListView stringListView;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.RadioButton suspendRadioButton;
        public System.Windows.Forms.RadioButton killRadioButton;
        public System.Windows.Forms.RadioButton passiveRadioButton;
        private RoundButton startButton;
        public System.Windows.Forms.ListView signCountListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip iconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.Label stringLabel;
        internal System.Windows.Forms.Button whiteListButton;
        private System.Windows.Forms.ToolTip startToolTip;
    }
}

