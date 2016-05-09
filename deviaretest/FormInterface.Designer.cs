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
            this.MonitorNewProcessesButton = new System.Windows.Forms.Button();
            this.StopMonitorButton = new System.Windows.Forms.Button();
            this.processListView = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
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
            this.processListView.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processListView.GridLines = true;
            this.processListView.Location = new System.Drawing.Point(333, 76);
            this.processListView.Name = "processListView";
            this.processListView.Size = new System.Drawing.Size(216, 273);
            this.processListView.TabIndex = 2;
            this.processListView.UseCompatibleStateImageBehavior = false;
            this.processListView.View = System.Windows.Forms.View.List;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(330, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tracked Processes";
            this.label1.UseMnemonic = false;
            // 
            // FormInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 423);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.processListView);
            this.Controls.Add(this.StopMonitorButton);
            this.Controls.Add(this.MonitorNewProcessesButton);
            this.Name = "FormInterface";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.hooker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button MonitorNewProcessesButton;
        private System.Windows.Forms.Button StopMonitorButton;
        public System.Windows.Forms.ListView processListView;
        private System.Windows.Forms.Label label1;
    }
}

