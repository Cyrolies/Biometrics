namespace StudentScanner
{
    partial class ScanningForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanningForm));
			this.PictureFingerPrint = new System.Windows.Forms.PictureBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chFastMode = new System.Windows.Forms.CheckBox();
			this.m_cmbVersion = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.btnVerify = new System.Windows.Forms.Button();
			this.m_lblIdentificationsLimit = new System.Windows.Forms.Label();
			this.cbMaxFrames = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cbMIOTOff = new System.Windows.Forms.CheckBox();
			this.tbFARN = new System.Windows.Forms.MaskedTextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cbFARNLevel = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.chDetectFakeFinger = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lblCounter = new System.Windows.Forms.Label();
			this.lblTimerCounter = new System.Windows.Forms.Label();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.btnIdentify = new System.Windows.Forms.Button();
			this.btnExit = new System.Windows.Forms.Button();
			this.timer = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.PictureFingerPrint)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// PictureFingerPrint
			// 
			this.PictureFingerPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.PictureFingerPrint.Image = ((System.Drawing.Image)(resources.GetObject("PictureFingerPrint.Image")));
			this.PictureFingerPrint.InitialImage = null;
			this.PictureFingerPrint.Location = new System.Drawing.Point(905, 120);
			this.PictureFingerPrint.Name = "PictureFingerPrint";
			this.PictureFingerPrint.Size = new System.Drawing.Size(236, 298);
			this.PictureFingerPrint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PictureFingerPrint.TabIndex = 1;
			this.PictureFingerPrint.TabStop = false;
			this.PictureFingerPrint.WaitOnLoad = true;
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(99)))), ((int)(((byte)(72)))));
			this.groupBox1.Controls.Add(this.chFastMode);
			this.groupBox1.Controls.Add(this.m_cmbVersion);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.btnVerify);
			this.groupBox1.Controls.Add(this.m_lblIdentificationsLimit);
			this.groupBox1.Controls.Add(this.cbMaxFrames);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.cbMIOTOff);
			this.groupBox1.Controls.Add(this.tbFARN);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.cbFARNLevel);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.chDetectFakeFinger);
			this.groupBox1.Font = new System.Drawing.Font("Cambria", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(182)))), ((int)(((byte)(141)))));
			this.groupBox1.Location = new System.Drawing.Point(43, 498);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(693, 107);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = " Settings ";
			this.groupBox1.Visible = false;
			// 
			// chFastMode
			// 
			this.chFastMode.AutoSize = true;
			this.chFastMode.Location = new System.Drawing.Point(190, 17);
			this.chFastMode.Name = "chFastMode";
			this.chFastMode.Size = new System.Drawing.Size(72, 16);
			this.chFastMode.TabIndex = 12;
			this.chFastMode.Text = "Fast mode";
			this.chFastMode.UseVisualStyleBackColor = true;
			// 
			// m_cmbVersion
			// 
			this.m_cmbVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cmbVersion.FormattingEnabled = true;
			this.m_cmbVersion.Location = new System.Drawing.Point(200, 39);
			this.m_cmbVersion.Name = "m_cmbVersion";
			this.m_cmbVersion.Size = new System.Drawing.Size(121, 20);
			this.m_cmbVersion.TabIndex = 11;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(16, 43);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(171, 12);
			this.label4.TabIndex = 10;
			this.label4.Text = "Do image processing compatible to: ";
			// 
			// btnVerify
			// 
			this.btnVerify.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnVerify.Location = new System.Drawing.Point(610, 66);
			this.btnVerify.Name = "btnVerify";
			this.btnVerify.Size = new System.Drawing.Size(69, 30);
			this.btnVerify.TabIndex = 3;
			this.btnVerify.Text = "Verify";
			this.btnVerify.UseVisualStyleBackColor = true;
			this.btnVerify.Visible = false;
			// 
			// m_lblIdentificationsLimit
			// 
			this.m_lblIdentificationsLimit.Location = new System.Drawing.Point(16, 73);
			this.m_lblIdentificationsLimit.Name = "m_lblIdentificationsLimit";
			this.m_lblIdentificationsLimit.Size = new System.Drawing.Size(305, 23);
			this.m_lblIdentificationsLimit.TabIndex = 9;
			this.m_lblIdentificationsLimit.Text = "label4";
			// 
			// cbMaxFrames
			// 
			this.cbMaxFrames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbMaxFrames.FormattingEnabled = true;
			this.cbMaxFrames.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
			this.cbMaxFrames.Location = new System.Drawing.Point(464, 39);
			this.cbMaxFrames.Name = "cbMaxFrames";
			this.cbMaxFrames.Size = new System.Drawing.Size(43, 20);
			this.cbMaxFrames.TabIndex = 8;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(327, 47);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(131, 12);
			this.label3.TabIndex = 7;
			this.label3.Text = "Set max frames in template:";
			// 
			// cbMIOTOff
			// 
			this.cbMIOTOff.AutoSize = true;
			this.cbMIOTOff.Location = new System.Drawing.Point(523, 43);
			this.cbMIOTOff.Name = "cbMIOTOff";
			this.cbMIOTOff.Size = new System.Drawing.Size(87, 16);
			this.cbMIOTOff.TabIndex = 6;
			this.cbMIOTOff.Text = "Disable MIOT";
			this.cbMIOTOff.UseVisualStyleBackColor = true;
			// 
			// tbFARN
			// 
			this.tbFARN.Culture = new System.Globalization.CultureInfo("");
			this.tbFARN.Location = new System.Drawing.Point(562, 17);
			this.tbFARN.Mask = "0000";
			this.tbFARN.Name = "tbFARN";
			this.tbFARN.Size = new System.Drawing.Size(48, 20);
			this.tbFARN.TabIndex = 5;
			this.tbFARN.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
			this.tbFARN.ValidatingType = typeof(int);
			this.tbFARN.Validating += new System.ComponentModel.CancelEventHandler(this.tbFARN_Validating);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(521, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(30, 12);
			this.label2.TabIndex = 3;
			this.label2.Text = "value";
			// 
			// cbFARNLevel
			// 
			this.cbFARNLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbFARNLevel.FormattingEnabled = true;
			this.cbFARNLevel.Items.AddRange(new object[] {
            "low",
            "below normal",
            "normal",
            "above normal",
            "high",
            "maximum",
            "custom"});
			this.cbFARNLevel.Location = new System.Drawing.Point(386, 17);
			this.cbFARNLevel.Name = "cbFARNLevel";
			this.cbFARNLevel.Size = new System.Drawing.Size(121, 20);
			this.cbFARNLevel.TabIndex = 2;
			this.cbFARNLevel.SelectedIndexChanged += new System.EventHandler(this.cbFARNLevel_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(290, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(90, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "Set measure level: ";
			// 
			// chDetectFakeFinger
			// 
			this.chDetectFakeFinger.AutoSize = true;
			this.chDetectFakeFinger.Location = new System.Drawing.Point(18, 17);
			this.chDetectFakeFinger.Name = "chDetectFakeFinger";
			this.chDetectFakeFinger.Size = new System.Drawing.Size(104, 16);
			this.chDetectFakeFinger.TabIndex = 0;
			this.chDetectFakeFinger.Text = "Detect fake finger";
			this.chDetectFakeFinger.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.lblCounter);
			this.groupBox2.Controls.Add(this.groupBox1);
			this.groupBox2.Controls.Add(this.lblTimerCounter);
			this.groupBox2.Controls.Add(this.txtMessage);
			this.groupBox2.Controls.Add(this.PictureFingerPrint);
			this.groupBox2.Controls.Add(this.btnIdentify);
			this.groupBox2.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(182)))), ((int)(((byte)(141)))));
			this.groupBox2.Location = new System.Drawing.Point(10, 11);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(1236, 620);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Fingerprint Attendance Scanner";
			// 
			// lblCounter
			// 
			this.lblCounter.AutoSize = true;
			this.lblCounter.Location = new System.Drawing.Point(435, 401);
			this.lblCounter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblCounter.Name = "lblCounter";
			this.lblCounter.Size = new System.Drawing.Size(0, 17);
			this.lblCounter.TabIndex = 9;
			// 
			// lblTimerCounter
			// 
			this.lblTimerCounter.AutoSize = true;
			this.lblTimerCounter.Location = new System.Drawing.Point(315, 401);
			this.lblTimerCounter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblTimerCounter.Name = "lblTimerCounter";
			this.lblTimerCounter.Size = new System.Drawing.Size(91, 17);
			this.lblTimerCounter.TabIndex = 8;
			this.lblTimerCounter.Text = "Resetting in  :";
			// 
			// txtMessage
			// 
			this.txtMessage.BackColor = System.Drawing.Color.Ivory;
			this.txtMessage.Font = new System.Drawing.Font("Cambria", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(182)))), ((int)(((byte)(141)))));
			this.txtMessage.Location = new System.Drawing.Point(64, 120);
			this.txtMessage.MaxLength = 9999999;
			this.txtMessage.Multiline = true;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.ReadOnly = true;
			this.txtMessage.Size = new System.Drawing.Size(715, 278);
			this.txtMessage.TabIndex = 7;
			this.txtMessage.TabStop = false;
			this.txtMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// btnIdentify
			// 
			this.btnIdentify.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnIdentify.Location = new System.Drawing.Point(849, 435);
			this.btnIdentify.Name = "btnIdentify";
			this.btnIdentify.Size = new System.Drawing.Size(158, 30);
			this.btnIdentify.TabIndex = 4;
			this.btnIdentify.Text = "Test Verification";
			this.btnIdentify.UseVisualStyleBackColor = true;
			this.btnIdentify.Visible = false;
			// 
			// btnExit
			// 
			this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(182)))), ((int)(((byte)(141)))));
			this.btnExit.Location = new System.Drawing.Point(1076, 561);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 34);
			this.btnExit.TabIndex = 6;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// timer
			// 
			this.timer.Enabled = true;
			this.timer.Interval = 1000;
			this.timer.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// ScanningForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(99)))), ((int)(((byte)(72)))));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.ClientSize = new System.Drawing.Size(1269, 667);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.groupBox2);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Name = "ScanningForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DSG Student Scanner";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.PictureFingerPrint)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox PictureFingerPrint;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chDetectFakeFinger;
        private System.Windows.Forms.ComboBox cbFARNLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox tbFARN;
        private System.Windows.Forms.ComboBox cbMaxFrames;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbMIOTOff;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label m_lblIdentificationsLimit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox m_cmbVersion;
        private System.Windows.Forms.CheckBox chFastMode;
		private System.Windows.Forms.Button btnIdentify;
	    private System.Windows.Forms.Label lblCounter;
		private System.Windows.Forms.Label lblTimerCounter;
		private System.Windows.Forms.Timer timer;
	}
}

