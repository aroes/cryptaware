namespace deviaretest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInterface));
            this.processListView = new System.Windows.Forms.ListView();
            this.caProcess = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.calledFListView = new System.Windows.Forms.ListView();
            this.fName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.debugCheckBox = new System.Windows.Forms.CheckBox();
            this.signsListView = new System.Windows.Forms.ListView();
            this.suspendRadioButton = new System.Windows.Forms.RadioButton();
            this.killRadioButton = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.passiveRadioButton = new System.Windows.Forms.RadioButton();
            this.signCountListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.startButton = new RoundButton();
            this.panel1.SuspendLayout();
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
            this.processListView.Location = new System.Drawing.Point(364, 63);
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
            this.caProcess.Width = 160;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(413, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tracked Processes";
            this.label1.UseMnemonic = false;
            // 
            // calledFListView
            // 
            this.calledFListView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.calledFListView.AutoArrange = false;
            this.calledFListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.calledFListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fName,
            this.pName,
            this.time});
            this.calledFListView.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calledFListView.FullRowSelect = true;
            this.calledFListView.Location = new System.Drawing.Point(596, 476);
            this.calledFListView.Name = "calledFListView";
            this.calledFListView.Size = new System.Drawing.Size(355, 416);
            this.calledFListView.TabIndex = 4;
            this.calledFListView.UseCompatibleStateImageBehavior = false;
            this.calledFListView.View = System.Windows.Forms.View.Details;
            // 
            // fName
            // 
            this.fName.Text = "Function Name";
            this.fName.Width = 187;
            // 
            // pName
            // 
            this.pName.Text = "Process";
            this.pName.Width = 92;
            // 
            // time
            // 
            this.time.Text = "Time";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(734, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Indicators";
            this.label2.UseMnemonic = false;
            // 
            // debugCheckBox
            // 
            this.debugCheckBox.AutoSize = true;
            this.debugCheckBox.Location = new System.Drawing.Point(3, 76);
            this.debugCheckBox.Name = "debugCheckBox";
            this.debugCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.debugCheckBox.Size = new System.Drawing.Size(212, 17);
            this.debugCheckBox.TabIndex = 6;
            this.debugCheckBox.Text = "Show debug info (Lowers Performance)";
            this.debugCheckBox.UseVisualStyleBackColor = true;
            this.debugCheckBox.CheckedChanged += new System.EventHandler(this.debugCheckBox_CheckedChanged);
            // 
            // signsListView
            // 
            this.signsListView.Location = new System.Drawing.Point(29, 370);
            this.signsListView.Name = "signsListView";
            this.signsListView.Size = new System.Drawing.Size(431, 109);
            this.signsListView.TabIndex = 7;
            this.signsListView.UseCompatibleStateImageBehavior = false;
            this.signsListView.View = System.Windows.Forms.View.List;
            // 
            // suspendRadioButton
            // 
            this.suspendRadioButton.AutoSize = true;
            this.suspendRadioButton.Location = new System.Drawing.Point(3, 7);
            this.suspendRadioButton.Name = "suspendRadioButton";
            this.suspendRadioButton.Size = new System.Drawing.Size(96, 17);
            this.suspendRadioButton.TabIndex = 8;
            this.suspendRadioButton.Text = "Suspend mode";
            this.suspendRadioButton.UseVisualStyleBackColor = true;
            // 
            // killRadioButton
            // 
            this.killRadioButton.AutoSize = true;
            this.killRadioButton.Location = new System.Drawing.Point(3, 30);
            this.killRadioButton.Name = "killRadioButton";
            this.killRadioButton.Size = new System.Drawing.Size(67, 17);
            this.killRadioButton.TabIndex = 9;
            this.killRadioButton.Text = "Kill mode";
            this.killRadioButton.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.passiveRadioButton);
            this.panel1.Controls.Add(this.killRadioButton);
            this.panel1.Controls.Add(this.debugCheckBox);
            this.panel1.Controls.Add(this.suspendRadioButton);
            this.panel1.Location = new System.Drawing.Point(12, 235);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(226, 101);
            this.panel1.TabIndex = 10;
            // 
            // passiveRadioButton
            // 
            this.passiveRadioButton.AutoSize = true;
            this.passiveRadioButton.Checked = true;
            this.passiveRadioButton.Location = new System.Drawing.Point(3, 53);
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
            this.signCountListView.Location = new System.Drawing.Point(596, 63);
            this.signCountListView.Name = "signCountListView";
            this.signCountListView.Size = new System.Drawing.Size(355, 390);
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
            // columnHeader4
            // 
            this.columnHeader4.Text = "PID";
            // 
            // startButton
            // 
            this.startButton.BackgroundImage = global::deviaretest.Properties.Resources.eyec;
            this.startButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.startButton.Location = new System.Drawing.Point(15, 24);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(205, 205);
            this.startButton.TabIndex = 11;
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // FormInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::deviaretest.Properties.Resources.formbg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(979, 597);
            this.Controls.Add(this.signCountListView);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.signsListView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.calledFListView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.processListView);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormInterface";
            this.Text = "CryptAware";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormInterface_FormClosed);
            this.Load += new System.EventHandler(this.FormInterface_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ListView processListView;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ListView calledFListView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader fName;
        private System.Windows.Forms.ColumnHeader pName;
        private System.Windows.Forms.ColumnHeader time;
        private System.Windows.Forms.ColumnHeader caProcess;
        public System.Windows.Forms.CheckBox debugCheckBox;
        public System.Windows.Forms.ListView signsListView;
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
    }
}

