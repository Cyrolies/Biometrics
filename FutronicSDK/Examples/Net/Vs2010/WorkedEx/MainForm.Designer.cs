namespace Futronic.SDK.WorkedEx
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
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
            this.btnIdentify = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.PictureFingerPrint)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEnroll
            // 
            this.btnEnroll.Location = new System.Drawing.Point(16, 32);
            this.btnEnroll.Name = "btnEnroll";
            this.btnEnroll.Size = new System.Drawing.Size(75, 23);
            this.btnEnroll.TabIndex = 0;
            this.btnEnroll.Text = "Enroll";
            this.btnEnroll.UseVisualStyleBackColor = true;
            this.btnEnroll.Click += new System.EventHandler(this.btnEnroll_Click);
            // 
            // PictureFingerPrint
            // 
            this.PictureFingerPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PictureFingerPrint.ErrorImage = global::Futronic.SDK.WorkedEx.Properties.Resources.Futronic;
            this.PictureFingerPrint.Image = global::Futronic.SDK.WorkedEx.Properties.Resources.Futronic;
            this.PictureFingerPrint.InitialImage = null;
            this.PictureFingerPrint.Location = new System.Drawing.Point(312, 251);
            this.PictureFingerPrint.Name = "PictureFingerPrint";
            this.PictureFingerPrint.Size = new System.Drawing.Size(160, 210);
            this.PictureFingerPrint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureFingerPrint.TabIndex = 1;
            this.PictureFingerPrint.TabStop = false;
            this.PictureFingerPrint.WaitOnLoad = true;
            // 
            // groupBox1
            // 
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
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(460, 207);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Settings ";
            // 
            // chFastMode
            // 
            this.chFastMode.AutoSize = true;
            this.chFastMode.Location = new System.Drawing.Point(200, 28);
            this.chFastMode.Name = "chFastMode";
            this.chFastMode.Size = new System.Drawing.Size(75, 17);
            this.chFastMode.TabIndex = 13;
            this.chFastMode.Text = "Fast mode";
            this.chFastMode.UseVisualStyleBackColor = true;
            // 
            // m_cmbVersion
            // 
            this.m_cmbVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cmbVersion.FormattingEnabled = true;
            this.m_cmbVersion.Location = new System.Drawing.Point(200, 138);
            this.m_cmbVersion.Name = "m_cmbVersion";
            this.m_cmbVersion.Size = new System.Drawing.Size(121, 21);
            this.m_cmbVersion.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(178, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Do image processing compatible to: ";
            // 
            // m_lblIdentificationsLimit
            // 
            this.m_lblIdentificationsLimit.Location = new System.Drawing.Point(16, 172);
            this.m_lblIdentificationsLimit.Name = "m_lblIdentificationsLimit";
            this.m_lblIdentificationsLimit.Size = new System.Drawing.Size(402, 23);
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
            this.cbMaxFrames.Location = new System.Drawing.Point(158, 111);
            this.cbMaxFrames.Name = "cbMaxFrames";
            this.cbMaxFrames.Size = new System.Drawing.Size(43, 21);
            this.cbMaxFrames.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Set max frames in template:";
            // 
            // cbMIOTOff
            // 
            this.cbMIOTOff.AutoSize = true;
            this.cbMIOTOff.Location = new System.Drawing.Point(16, 87);
            this.cbMIOTOff.Name = "cbMIOTOff";
            this.cbMIOTOff.Size = new System.Drawing.Size(91, 17);
            this.cbMIOTOff.TabIndex = 6;
            this.cbMIOTOff.Text = "Disable MIOT";
            this.cbMIOTOff.UseVisualStyleBackColor = true;
            // 
            // tbFARN
            // 
            this.tbFARN.Culture = new System.Globalization.CultureInfo("");
            this.tbFARN.Location = new System.Drawing.Point(300, 55);
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
            this.label2.Location = new System.Drawing.Point(250, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
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
            this.cbFARNLevel.Location = new System.Drawing.Point(119, 55);
            this.cbFARNLevel.Name = "cbFARNLevel";
            this.cbFARNLevel.Size = new System.Drawing.Size(121, 21);
            this.cbFARNLevel.TabIndex = 2;
            this.cbFARNLevel.SelectedIndexChanged += new System.EventHandler(this.cbFARNLevel_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Set measure level: ";
            // 
            // chDetectFakeFinger
            // 
            this.chDetectFakeFinger.AutoSize = true;
            this.chDetectFakeFinger.Location = new System.Drawing.Point(16, 28);
            this.chDetectFakeFinger.Name = "chDetectFakeFinger";
            this.chDetectFakeFinger.Size = new System.Drawing.Size(111, 17);
            this.chDetectFakeFinger.TabIndex = 0;
            this.chDetectFakeFinger.Text = "Detect fake finger";
            this.chDetectFakeFinger.UseVisualStyleBackColor = true;
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(16, 61);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(75, 23);
            this.btnVerify.TabIndex = 3;
            this.btnVerify.Text = "Verify";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnIdentify
            // 
            this.btnIdentify.Location = new System.Drawing.Point(16, 90);
            this.btnIdentify.Name = "btnIdentify";
            this.btnIdentify.Size = new System.Drawing.Size(75, 23);
            this.btnIdentify.TabIndex = 4;
            this.btnIdentify.Text = "Identify";
            this.btnIdentify.UseVisualStyleBackColor = true;
            this.btnIdentify.Click += new System.EventHandler(this.btnIdentify_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnStop);
            this.groupBox2.Controls.Add(this.btnEnroll);
            this.groupBox2.Controls.Add(this.btnIdentify);
            this.groupBox2.Controls.Add(this.btnVerify);
            this.groupBox2.Location = new System.Drawing.Point(12, 251);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(283, 139);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Operations";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(194, 90);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(220, 438);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(12, 225);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(460, 20);
            this.txtMessage.TabIndex = 7;
            this.txtMessage.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 473);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.PictureFingerPrint);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "C# example for Futronic SDK v.4.2";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PictureFingerPrint)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEnroll;
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
        private System.Windows.Forms.Button btnIdentify;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label m_lblIdentificationsLimit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox m_cmbVersion;
        private System.Windows.Forms.CheckBox chFastMode;
    }
}

