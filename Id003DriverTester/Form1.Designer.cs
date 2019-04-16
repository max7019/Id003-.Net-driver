namespace Id003DriverTester
{
    partial class Form1
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
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.cbPorts = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnStartPooling = new System.Windows.Forms.Button();
            this.btnStopPooling = new System.Windows.Forms.Button();
            this.clbDenominations = new System.Windows.Forms.CheckedListBox();
            this.btnEnableAll = new System.Windows.Forms.Button();
            this.btnDisableAll = new System.Windows.Forms.Button();
            this.btnEnable = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnGetStatus = new System.Windows.Forms.Button();
            this.btnGetModelId = new System.Windows.Forms.Button();
            this.btnGetBootVersion = new System.Windows.Forms.Button();
            this.btnEnableAcceptance = new System.Windows.Forms.Button();
            this.btnDisableAcceptance = new System.Windows.Forms.Button();
            this.btnStack1 = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnStack2 = new System.Windows.Forms.Button();
            this.btnHold = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtbOutput
            // 
            this.rtbOutput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbOutput.Location = new System.Drawing.Point(0, 286);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.Size = new System.Drawing.Size(968, 238);
            this.rtbOutput.TabIndex = 0;
            this.rtbOutput.Text = "";
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(139, 6);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(104, 30);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // cbPorts
            // 
            this.cbPorts.FormattingEnabled = true;
            this.cbPorts.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9"});
            this.cbPorts.Location = new System.Drawing.Point(12, 12);
            this.cbPorts.Name = "cbPorts";
            this.cbPorts.Size = new System.Drawing.Size(121, 24);
            this.cbPorts.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(249, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(104, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnStartPooling
            // 
            this.btnStartPooling.Location = new System.Drawing.Point(139, 42);
            this.btnStartPooling.Name = "btnStartPooling";
            this.btnStartPooling.Size = new System.Drawing.Size(104, 30);
            this.btnStartPooling.TabIndex = 4;
            this.btnStartPooling.Text = "Start pooling";
            this.btnStartPooling.UseVisualStyleBackColor = true;
            this.btnStartPooling.Click += new System.EventHandler(this.btnStartPooling_Click);
            // 
            // btnStopPooling
            // 
            this.btnStopPooling.Location = new System.Drawing.Point(249, 42);
            this.btnStopPooling.Name = "btnStopPooling";
            this.btnStopPooling.Size = new System.Drawing.Size(104, 30);
            this.btnStopPooling.TabIndex = 5;
            this.btnStopPooling.Text = "Stop pooling";
            this.btnStopPooling.UseVisualStyleBackColor = true;
            this.btnStopPooling.Click += new System.EventHandler(this.btnStopPooling_Click);
            // 
            // clbDenominations
            // 
            this.clbDenominations.FormattingEnabled = true;
            this.clbDenominations.Items.AddRange(new object[] {
            "Denomination 1",
            "Denomination 2",
            "Denomination 3",
            "Denomination 4",
            "Denomination 5",
            "Denomination 6",
            "Denomination 7",
            "Denomination 8"});
            this.clbDenominations.Location = new System.Drawing.Point(471, 12);
            this.clbDenominations.Name = "clbDenominations";
            this.clbDenominations.Size = new System.Drawing.Size(155, 140);
            this.clbDenominations.TabIndex = 6;
            // 
            // btnEnableAll
            // 
            this.btnEnableAll.Location = new System.Drawing.Point(632, 12);
            this.btnEnableAll.Name = "btnEnableAll";
            this.btnEnableAll.Size = new System.Drawing.Size(104, 30);
            this.btnEnableAll.TabIndex = 7;
            this.btnEnableAll.Text = "Enable all";
            this.btnEnableAll.UseVisualStyleBackColor = true;
            this.btnEnableAll.Click += new System.EventHandler(this.btnEnableAll_Click);
            // 
            // btnDisableAll
            // 
            this.btnDisableAll.Location = new System.Drawing.Point(742, 12);
            this.btnDisableAll.Name = "btnDisableAll";
            this.btnDisableAll.Size = new System.Drawing.Size(104, 30);
            this.btnDisableAll.TabIndex = 8;
            this.btnDisableAll.Text = "Disable all";
            this.btnDisableAll.UseVisualStyleBackColor = true;
            this.btnDisableAll.Click += new System.EventHandler(this.btnDisableAll_Click);
            // 
            // btnEnable
            // 
            this.btnEnable.Location = new System.Drawing.Point(852, 12);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(104, 30);
            this.btnEnable.TabIndex = 9;
            this.btnEnable.Text = "Enable";
            this.btnEnable.UseVisualStyleBackColor = true;
            this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(139, 114);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(104, 30);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnGetStatus
            // 
            this.btnGetStatus.Location = new System.Drawing.Point(249, 114);
            this.btnGetStatus.Name = "btnGetStatus";
            this.btnGetStatus.Size = new System.Drawing.Size(104, 30);
            this.btnGetStatus.TabIndex = 11;
            this.btnGetStatus.Text = "Get status";
            this.btnGetStatus.UseVisualStyleBackColor = true;
            this.btnGetStatus.Click += new System.EventHandler(this.btnGetStatus_Click);
            // 
            // btnGetModelId
            // 
            this.btnGetModelId.Location = new System.Drawing.Point(139, 150);
            this.btnGetModelId.Name = "btnGetModelId";
            this.btnGetModelId.Size = new System.Drawing.Size(104, 30);
            this.btnGetModelId.TabIndex = 12;
            this.btnGetModelId.Text = "Get model ID";
            this.btnGetModelId.UseVisualStyleBackColor = true;
            this.btnGetModelId.Click += new System.EventHandler(this.btnGetModelId_Click);
            // 
            // btnGetBootVersion
            // 
            this.btnGetBootVersion.Location = new System.Drawing.Point(249, 150);
            this.btnGetBootVersion.Name = "btnGetBootVersion";
            this.btnGetBootVersion.Size = new System.Drawing.Size(104, 30);
            this.btnGetBootVersion.TabIndex = 13;
            this.btnGetBootVersion.Text = "Get boot ver.";
            this.btnGetBootVersion.UseVisualStyleBackColor = true;
            this.btnGetBootVersion.Click += new System.EventHandler(this.btnGetBootVersion_Click);
            // 
            // btnEnableAcceptance
            // 
            this.btnEnableAcceptance.Location = new System.Drawing.Point(139, 78);
            this.btnEnableAcceptance.Name = "btnEnableAcceptance";
            this.btnEnableAcceptance.Size = new System.Drawing.Size(104, 30);
            this.btnEnableAcceptance.TabIndex = 14;
            this.btnEnableAcceptance.Text = "En. accept.";
            this.btnEnableAcceptance.UseVisualStyleBackColor = true;
            this.btnEnableAcceptance.Click += new System.EventHandler(this.btnEnableAcceptance_Click);
            // 
            // btnDisableAcceptance
            // 
            this.btnDisableAcceptance.Location = new System.Drawing.Point(249, 78);
            this.btnDisableAcceptance.Name = "btnDisableAcceptance";
            this.btnDisableAcceptance.Size = new System.Drawing.Size(104, 30);
            this.btnDisableAcceptance.TabIndex = 15;
            this.btnDisableAcceptance.Text = "Dis. accept.";
            this.btnDisableAcceptance.UseVisualStyleBackColor = true;
            this.btnDisableAcceptance.Click += new System.EventHandler(this.btnDisableAcceptance_Click);
            // 
            // btnStack1
            // 
            this.btnStack1.Location = new System.Drawing.Point(632, 86);
            this.btnStack1.Name = "btnStack1";
            this.btnStack1.Size = new System.Drawing.Size(104, 30);
            this.btnStack1.TabIndex = 16;
            this.btnStack1.Text = "Stack1";
            this.btnStack1.UseVisualStyleBackColor = true;
            this.btnStack1.Click += new System.EventHandler(this.btnStack_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.Location = new System.Drawing.Point(632, 122);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(104, 30);
            this.btnReturn.TabIndex = 17;
            this.btnReturn.Text = "Return";
            this.btnReturn.UseVisualStyleBackColor = true;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnStack2
            // 
            this.btnStack2.Location = new System.Drawing.Point(742, 86);
            this.btnStack2.Name = "btnStack2";
            this.btnStack2.Size = new System.Drawing.Size(104, 30);
            this.btnStack2.TabIndex = 18;
            this.btnStack2.Text = "Stack2";
            this.btnStack2.UseVisualStyleBackColor = true;
            this.btnStack2.Click += new System.EventHandler(this.btnStack2_Click);
            // 
            // btnHold
            // 
            this.btnHold.Location = new System.Drawing.Point(742, 122);
            this.btnHold.Name = "btnHold";
            this.btnHold.Size = new System.Drawing.Size(104, 30);
            this.btnHold.TabIndex = 19;
            this.btnHold.Text = "Hold";
            this.btnHold.UseVisualStyleBackColor = true;
            this.btnHold.Click += new System.EventHandler(this.btnHold_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(968, 524);
            this.Controls.Add(this.btnHold);
            this.Controls.Add(this.btnStack2);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnStack1);
            this.Controls.Add(this.btnDisableAcceptance);
            this.Controls.Add(this.btnEnableAcceptance);
            this.Controls.Add(this.btnGetBootVersion);
            this.Controls.Add(this.btnGetModelId);
            this.Controls.Add(this.btnGetStatus);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnEnable);
            this.Controls.Add(this.btnDisableAll);
            this.Controls.Add(this.btnEnableAll);
            this.Controls.Add(this.clbDenominations);
            this.Controls.Add(this.btnStopPooling);
            this.Controls.Add(this.btnStartPooling);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.cbPorts);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.rtbOutput);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ComboBox cbPorts;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnStartPooling;
        private System.Windows.Forms.Button btnStopPooling;
        private System.Windows.Forms.CheckedListBox clbDenominations;
        private System.Windows.Forms.Button btnEnableAll;
        private System.Windows.Forms.Button btnDisableAll;
        private System.Windows.Forms.Button btnEnable;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnGetStatus;
        private System.Windows.Forms.Button btnGetModelId;
        private System.Windows.Forms.Button btnGetBootVersion;
        private System.Windows.Forms.Button btnEnableAcceptance;
        private System.Windows.Forms.Button btnDisableAcceptance;
        private System.Windows.Forms.Button btnStack1;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnStack2;
        private System.Windows.Forms.Button btnHold;
    }
}

