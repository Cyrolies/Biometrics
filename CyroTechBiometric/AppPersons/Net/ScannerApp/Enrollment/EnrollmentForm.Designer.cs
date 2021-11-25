namespace Futronic.SDK.WorkedEx
{
    partial class EnrollmentForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnrollmentForm));
			this.btnEnroll = new System.Windows.Forms.Button();
			this.PictureFingerPrint = new System.Windows.Forms.PictureBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chFastMode = new System.Windows.Forms.CheckBox();
			this.m_cmbVersion = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.m_lblIdentificationsLimit = new System.Windows.Forms.Label();
			this.cbMaxFrames = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cbMIOTOff = new System.Windows.Forms.CheckBox();
			this.tbFARN = new System.Windows.Forms.MaskedTextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cbFARNLevel = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.chDetectFakeFinger = new System.Windows.Forms.CheckBox();
			this.btnVerify = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnExit = new System.Windows.Forms.Button();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.cmbStudents = new System.Windows.Forms.ComboBox();
			this.btnSearch = new System.Windows.Forms.Button();
			this.txtSurname = new System.Windows.Forms.TextBox();
			this.lblSurname = new System.Windows.Forms.Label();
			this.lblList = new System.Windows.Forms.Label();
			this.btnIdentify = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.PictureFingerPrint)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnEnroll
			// 
			this.btnEnroll.BackColor = System.Drawing.Color.Transparent;
			this.btnEnroll.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnEnroll.Location = new System.Drawing.Point(148, 147);
			this.btnEnroll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnEnroll.Name = "btnEnroll";
			this.btnEnroll.Size = new System.Drawing.Size(211, 37);
			this.btnEnroll.TabIndex = 0;
			this.btnEnroll.Text = "Enroll Student";
			this.btnEnroll.UseVisualStyleBackColor = false;
			this.btnEnroll.Click += new System.EventHandler(this.btnEnroll_Click);
			// 
			// PictureFingerPrint
			// 
			this.PictureFingerPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.PictureFingerPrint.ErrorImage = global::Futronic.SDK.WorkedEx.Properties.Resources.Futronic;
			this.PictureFingerPrint.Image = ((System.Drawing.Image)(resources.GetObject("PictureFingerPrint.Image")));
			this.PictureFingerPrint.InitialImage = null;
			this.PictureFingerPrint.Location = new System.Drawing.Point(587, 102);
			this.PictureFingerPrint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.PictureFingerPrint.Name = "PictureFingerPrint";
			this.PictureFingerPrint.Size = new System.Drawing.Size(212, 258);
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
			this.groupBox1.Location = new System.Drawing.Point(16, 352);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox1.Size = new System.Drawing.Size(924, 132);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = " Settings ";
			this.groupBox1.Visible = false;
			// 
			// chFastMode
			// 
			this.chFastMode.AutoSize = true;
			this.chFastMode.Location = new System.Drawing.Point(253, 21);
			this.chFastMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.chFastMode.Name = "chFastMode";
			this.chFastMode.Size = new System.Drawing.Size(93, 20);
			this.chFastMode.TabIndex = 12;
			this.chFastMode.Text = "Fast mode";
			this.chFastMode.UseVisualStyleBackColor = true;
			// 
			// m_cmbVersion
			// 
			this.m_cmbVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cmbVersion.FormattingEnabled = true;
			this.m_cmbVersion.Location = new System.Drawing.Point(267, 48);
			this.m_cmbVersion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.m_cmbVersion.Name = "m_cmbVersion";
			this.m_cmbVersion.Size = new System.Drawing.Size(160, 24);
			this.m_cmbVersion.TabIndex = 11;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(21, 53);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(225, 16);
			this.label4.TabIndex = 10;
			this.label4.Text = "Do image processing compatible to: ";
			// 
			// m_lblIdentificationsLimit
			// 
			this.m_lblIdentificationsLimit.Location = new System.Drawing.Point(21, 90);
			this.m_lblIdentificationsLimit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.m_lblIdentificationsLimit.Name = "m_lblIdentificationsLimit";
			this.m_lblIdentificationsLimit.Size = new System.Drawing.Size(407, 28);
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
			this.cbMaxFrames.Location = new System.Drawing.Point(619, 48);
			this.cbMaxFrames.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.cbMaxFrames.Name = "cbMaxFrames";
			this.cbMaxFrames.Size = new System.Drawing.Size(56, 24);
			this.cbMaxFrames.TabIndex = 8;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(436, 58);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(178, 16);
			this.label3.TabIndex = 7;
			this.label3.Text = "Set max frames in template:";
			// 
			// cbMIOTOff
			// 
			this.cbMIOTOff.AutoSize = true;
			this.cbMIOTOff.Location = new System.Drawing.Point(697, 53);
			this.cbMIOTOff.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.cbMIOTOff.Name = "cbMIOTOff";
			this.cbMIOTOff.Size = new System.Drawing.Size(111, 20);
			this.cbMIOTOff.TabIndex = 6;
			this.cbMIOTOff.Text = "Disable MIOT";
			this.cbMIOTOff.UseVisualStyleBackColor = true;
			// 
			// tbFARN
			// 
			this.tbFARN.Culture = new System.Globalization.CultureInfo("");
			this.tbFARN.Location = new System.Drawing.Point(749, 21);
			this.tbFARN.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.tbFARN.Mask = "0000";
			this.tbFARN.Name = "tbFARN";
			this.tbFARN.Size = new System.Drawing.Size(63, 24);
			this.tbFARN.TabIndex = 5;
			this.tbFARN.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
			this.tbFARN.ValidatingType = typeof(int);
			this.tbFARN.Validating += new System.ComponentModel.CancelEventHandler(this.tbFARN_Validating);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(695, 31);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 16);
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
			this.cbFARNLevel.Location = new System.Drawing.Point(515, 21);
			this.cbFARNLevel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.cbFARNLevel.Name = "cbFARNLevel";
			this.cbFARNLevel.Size = new System.Drawing.Size(160, 24);
			this.cbFARNLevel.TabIndex = 2;
			this.cbFARNLevel.SelectedIndexChanged += new System.EventHandler(this.cbFARNLevel_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(387, 31);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(122, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Set measure level: ";
			// 
			// chDetectFakeFinger
			// 
			this.chDetectFakeFinger.AutoSize = true;
			this.chDetectFakeFinger.Location = new System.Drawing.Point(24, 21);
			this.chDetectFakeFinger.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.chDetectFakeFinger.Name = "chDetectFakeFinger";
			this.chDetectFakeFinger.Size = new System.Drawing.Size(136, 20);
			this.chDetectFakeFinger.TabIndex = 0;
			this.chDetectFakeFinger.Text = "Detect fake finger";
			this.chDetectFakeFinger.UseVisualStyleBackColor = true;
			// 
			// btnVerify
			// 
			this.btnVerify.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnVerify.Location = new System.Drawing.Point(840, 308);
			this.btnVerify.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnVerify.Name = "btnVerify";
			this.btnVerify.Size = new System.Drawing.Size(92, 37);
			this.btnVerify.TabIndex = 3;
			this.btnVerify.Text = "Verify";
			this.btnVerify.UseVisualStyleBackColor = true;
			this.btnVerify.Visible = false;
			this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lblList);
			this.groupBox2.Controls.Add(this.lblSurname);
			this.groupBox2.Controls.Add(this.txtSurname);
			this.groupBox2.Controls.Add(this.btnSearch);
			this.groupBox2.Controls.Add(this.cmbStudents);
			this.groupBox2.Controls.Add(this.btnStop);
			this.groupBox2.Controls.Add(this.btnEnroll);
			this.groupBox2.Controls.Add(this.btnIdentify);
			this.groupBox2.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(182)))), ((int)(((byte)(141)))));
			this.groupBox2.Location = new System.Drawing.Point(13, 92);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox2.Size = new System.Drawing.Size(478, 252);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Select student to enroll";
			// 
			// btnStop
			// 
			this.btnStop.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStop.Location = new System.Drawing.Point(352, 192);
			this.btnStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(100, 37);
			this.btnStop.TabIndex = 5;
			this.btnStop.Text = "Clear";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnExit
			// 
			this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(182)))), ((int)(((byte)(141)))));
			this.btnExit.Location = new System.Drawing.Point(840, 212);
			this.btnExit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(100, 42);
			this.btnExit.TabIndex = 6;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// txtMessage
			// 
			this.txtMessage.Font = new System.Drawing.Font("Cambria", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtMessage.ForeColor = System.Drawing.SystemColors.ControlDark;
			this.txtMessage.Location = new System.Drawing.Point(37, 32);
			this.txtMessage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.ReadOnly = true;
			this.txtMessage.Size = new System.Drawing.Size(877, 34);
			this.txtMessage.TabIndex = 7;
			this.txtMessage.TabStop = false;
			// 
			// cmbStudents
			// 
			this.cmbStudents.FormattingEnabled = true;
			this.cmbStudents.Location = new System.Drawing.Point(24, 104);
			this.cmbStudents.Name = "cmbStudents";
			this.cmbStudents.Size = new System.Drawing.Size(425, 30);
			this.cmbStudents.TabIndex = 6;
			// 
			// btnSearch
			// 
			this.btnSearch.Location = new System.Drawing.Point(330, 37);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(122, 30);
			this.btnSearch.TabIndex = 7;
			this.btnSearch.Text = "Filter List";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// txtSurname
			// 
			this.txtSurname.Location = new System.Drawing.Point(117, 38);
			this.txtSurname.Name = "txtSurname";
			this.txtSurname.Size = new System.Drawing.Size(196, 29);
			this.txtSurname.TabIndex = 8;
			// 
			// lblSurname
			// 
			this.lblSurname.AutoSize = true;
			this.lblSurname.Location = new System.Drawing.Point(23, 45);
			this.lblSurname.Name = "lblSurname";
			this.lblSurname.Size = new System.Drawing.Size(88, 22);
			this.lblSurname.TabIndex = 9;
			this.lblSurname.Text = "Surname:";
			// 
			// lblList
			// 
			this.lblList.AutoSize = true;
			this.lblList.Location = new System.Drawing.Point(23, 79);
			this.lblList.Name = "lblList";
			this.lblList.Size = new System.Drawing.Size(232, 22);
			this.lblList.TabIndex = 10;
			this.lblList.Text = "Student list from Ed-Admin";
			// 
			// btnIdentify
			// 
			this.btnIdentify.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnIdentify.Location = new System.Drawing.Point(27, 192);
			this.btnIdentify.Margin = new System.Windows.Forms.Padding(4);
			this.btnIdentify.Name = "btnIdentify";
			this.btnIdentify.Size = new System.Drawing.Size(211, 37);
			this.btnIdentify.TabIndex = 4;
			this.btnIdentify.Text = "Test Verification";
			this.btnIdentify.UseVisualStyleBackColor = true;
			this.btnIdentify.Click += new System.EventHandler(this.btnIdentify_Click);
			// 
			// EnrollmentForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(99)))), ((int)(((byte)(72)))));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.ClientSize = new System.Drawing.Size(956, 505);
			this.Controls.Add(this.txtMessage);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.btnVerify);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.PictureFingerPrint);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "EnrollmentForm";
			this.Text = "DSG Student Enrollment";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.PictureFingerPrint)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

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
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label m_lblIdentificationsLimit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox m_cmbVersion;
        private System.Windows.Forms.CheckBox chFastMode;
		private System.Windows.Forms.Button btnEnroll;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.ComboBox cmbStudents;
		private System.Windows.Forms.Label lblSurname;
		private System.Windows.Forms.TextBox txtSurname;
		private System.Windows.Forms.Label lblList;
		private System.Windows.Forms.Button btnIdentify;
	}
}

