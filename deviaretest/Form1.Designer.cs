namespace deviaretest
{
    partial class hooker
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
            // hooker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.StopMonitorButton);
            this.Controls.Add(this.MonitorNewProcessesButton);
            this.Name = "hooker";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.hooker_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button MonitorNewProcessesButton;
        private System.Windows.Forms.Button StopMonitorButton;
    }
}

