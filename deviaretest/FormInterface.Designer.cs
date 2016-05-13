﻿namespace deviaretest
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
            this.MonitorNewProcessesButton = new System.Windows.Forms.Button();
            this.StopMonitorButton = new System.Windows.Forms.Button();
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
            this.SuspendLayout();
            // 
            // MonitorNewProcessesButton
            // 
            this.MonitorNewProcessesButton.BackColor = System.Drawing.SystemColors.Control;
            this.MonitorNewProcessesButton.Location = new System.Drawing.Point(60, 63);
            this.MonitorNewProcessesButton.Name = "MonitorNewProcessesButton";
            this.MonitorNewProcessesButton.Size = new System.Drawing.Size(162, 57);
            this.MonitorNewProcessesButton.TabIndex = 0;
            this.MonitorNewProcessesButton.Text = "Monitor new processes";
            this.MonitorNewProcessesButton.UseVisualStyleBackColor = false;
            this.MonitorNewProcessesButton.Click += new System.EventHandler(this.MonitorNewProcessesButton_Click);
            // 
            // StopMonitorButton
            // 
            this.StopMonitorButton.Enabled = false;
            this.StopMonitorButton.Location = new System.Drawing.Point(74, 165);
            this.StopMonitorButton.Name = "StopMonitorButton";
            this.StopMonitorButton.Size = new System.Drawing.Size(130, 50);
            this.StopMonitorButton.TabIndex = 1;
            this.StopMonitorButton.Text = "Stop monitoring";
            this.StopMonitorButton.UseVisualStyleBackColor = true;
            this.StopMonitorButton.Click += new System.EventHandler(this.StopMonitorButton_Click);
            // 
            // processListView
            // 
            this.processListView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.processListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.processListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.caProcess});
            this.processListView.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.processListView.Location = new System.Drawing.Point(244, 63);
            this.processListView.Name = "processListView";
            this.processListView.Size = new System.Drawing.Size(216, 273);
            this.processListView.TabIndex = 2;
            this.processListView.UseCompatibleStateImageBehavior = false;
            this.processListView.View = System.Windows.Forms.View.Details;
            // 
            // caProcess
            // 
            this.caProcess.Text = "Process";
            this.caProcess.Width = 170;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(293, 44);
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
            this.calledFListView.Location = new System.Drawing.Point(494, 63);
            this.calledFListView.Name = "calledFListView";
            this.calledFListView.Size = new System.Drawing.Size(353, 416);
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
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(578, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Called functions";
            this.label2.UseMnemonic = false;
            // 
            // debugCheckBox
            // 
            this.debugCheckBox.AutoSize = true;
            this.debugCheckBox.Location = new System.Drawing.Point(10, 319);
            this.debugCheckBox.Name = "debugCheckBox";
            this.debugCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.debugCheckBox.Size = new System.Drawing.Size(212, 17);
            this.debugCheckBox.TabIndex = 6;
            this.debugCheckBox.Text = "Show debug info (Lowers Performance)";
            this.debugCheckBox.UseVisualStyleBackColor = true;
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
            // FormInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 491);
            this.Controls.Add(this.signsListView);
            this.Controls.Add(this.debugCheckBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.calledFListView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.processListView);
            this.Controls.Add(this.StopMonitorButton);
            this.Controls.Add(this.MonitorNewProcessesButton);
            this.Name = "FormInterface";
            this.Text = "Cryptaware";
            this.Load += new System.EventHandler(this.FormInterface_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button MonitorNewProcessesButton;
        private System.Windows.Forms.Button StopMonitorButton;
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
    }
}
