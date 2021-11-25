Imports Microsoft.VisualBasic
Imports System
Namespace Futronic.SDK.WorkedEx
	Public Partial Class MainForm
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (Not components Is Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
            Me.btnEnroll = New System.Windows.Forms.Button()
            Me.PictureFingerPrint = New System.Windows.Forms.PictureBox()
            Me.groupBox1 = New System.Windows.Forms.GroupBox()
            Me.chFastMode = New System.Windows.Forms.CheckBox()
            Me.m_cmbVersion = New System.Windows.Forms.ComboBox()
            Me.label4 = New System.Windows.Forms.Label()
            Me.m_lblIdentificationsLimit = New System.Windows.Forms.Label()
            Me.cbMaxFrames = New System.Windows.Forms.ComboBox()
            Me.label3 = New System.Windows.Forms.Label()
            Me.cbMIOTOff = New System.Windows.Forms.CheckBox()
            Me.tbFARN = New System.Windows.Forms.MaskedTextBox()
            Me.label2 = New System.Windows.Forms.Label()
            Me.cbFARNLevel = New System.Windows.Forms.ComboBox()
            Me.label1 = New System.Windows.Forms.Label()
            Me.chDetectFakeFinger = New System.Windows.Forms.CheckBox()
            Me.btnVerify = New System.Windows.Forms.Button()
            Me.btnIdentify = New System.Windows.Forms.Button()
            Me.groupBox2 = New System.Windows.Forms.GroupBox()
            Me.btnStop = New System.Windows.Forms.Button()
            Me.btnExit = New System.Windows.Forms.Button()
            Me.txtMessage = New System.Windows.Forms.TextBox()
            CType(Me.PictureFingerPrint, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.groupBox1.SuspendLayout()
            Me.groupBox2.SuspendLayout()
            Me.SuspendLayout()
            '
            'btnEnroll
            '
            Me.btnEnroll.Location = New System.Drawing.Point(16, 32)
            Me.btnEnroll.Name = "btnEnroll"
            Me.btnEnroll.Size = New System.Drawing.Size(75, 23)
            Me.btnEnroll.TabIndex = 0
            Me.btnEnroll.Text = "Enroll"
            Me.btnEnroll.UseVisualStyleBackColor = True
            '
            'PictureFingerPrint
            '
            Me.PictureFingerPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.PictureFingerPrint.ErrorImage = Global.My.Resources.Resources.Futronic
            Me.PictureFingerPrint.Image = Global.My.Resources.Resources.Futronic
            Me.PictureFingerPrint.InitialImage = Nothing
            Me.PictureFingerPrint.Location = New System.Drawing.Point(312, 251)
            Me.PictureFingerPrint.Name = "PictureFingerPrint"
            Me.PictureFingerPrint.Size = New System.Drawing.Size(160, 210)
            Me.PictureFingerPrint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
            Me.PictureFingerPrint.TabIndex = 1
            Me.PictureFingerPrint.TabStop = False
            Me.PictureFingerPrint.WaitOnLoad = True
            '
            'groupBox1
            '
            Me.groupBox1.Controls.Add(Me.chFastMode)
            Me.groupBox1.Controls.Add(Me.m_cmbVersion)
            Me.groupBox1.Controls.Add(Me.label4)
            Me.groupBox1.Controls.Add(Me.m_lblIdentificationsLimit)
            Me.groupBox1.Controls.Add(Me.cbMaxFrames)
            Me.groupBox1.Controls.Add(Me.label3)
            Me.groupBox1.Controls.Add(Me.cbMIOTOff)
            Me.groupBox1.Controls.Add(Me.tbFARN)
            Me.groupBox1.Controls.Add(Me.label2)
            Me.groupBox1.Controls.Add(Me.cbFARNLevel)
            Me.groupBox1.Controls.Add(Me.label1)
            Me.groupBox1.Controls.Add(Me.chDetectFakeFinger)
            Me.groupBox1.Location = New System.Drawing.Point(12, 12)
            Me.groupBox1.Name = "groupBox1"
            Me.groupBox1.Size = New System.Drawing.Size(460, 207)
            Me.groupBox1.TabIndex = 2
            Me.groupBox1.TabStop = False
            Me.groupBox1.Text = " Settings "
            '
            'chFastMode
            '
            Me.chFastMode.AutoSize = True
            Me.chFastMode.Location = New System.Drawing.Point(200, 28)
            Me.chFastMode.Name = "chFastMode"
            Me.chFastMode.Size = New System.Drawing.Size(75, 17)
            Me.chFastMode.TabIndex = 13
            Me.chFastMode.Text = "Fast mode"
            Me.chFastMode.UseVisualStyleBackColor = True
            '
            'm_cmbVersion
            '
            Me.m_cmbVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.m_cmbVersion.FormattingEnabled = True
            Me.m_cmbVersion.Location = New System.Drawing.Point(200, 138)
            Me.m_cmbVersion.Name = "m_cmbVersion"
            Me.m_cmbVersion.Size = New System.Drawing.Size(121, 21)
            Me.m_cmbVersion.TabIndex = 11
            '
            'label4
            '
            Me.label4.AutoSize = True
            Me.label4.Location = New System.Drawing.Point(16, 142)
            Me.label4.Name = "label4"
            Me.label4.Size = New System.Drawing.Size(178, 13)
            Me.label4.TabIndex = 10
            Me.label4.Text = "Do image processing compatible to: "
            '
            'm_lblIdentificationsLimit
            '
            Me.m_lblIdentificationsLimit.Location = New System.Drawing.Point(16, 172)
            Me.m_lblIdentificationsLimit.Name = "m_lblIdentificationsLimit"
            Me.m_lblIdentificationsLimit.Size = New System.Drawing.Size(402, 23)
            Me.m_lblIdentificationsLimit.TabIndex = 9
            Me.m_lblIdentificationsLimit.Text = "label4"
            '
            'cbMaxFrames
            '
            Me.cbMaxFrames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbMaxFrames.FormattingEnabled = True
            Me.cbMaxFrames.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
            Me.cbMaxFrames.Location = New System.Drawing.Point(158, 111)
            Me.cbMaxFrames.Name = "cbMaxFrames"
            Me.cbMaxFrames.Size = New System.Drawing.Size(43, 21)
            Me.cbMaxFrames.TabIndex = 8
            '
            'label3
            '
            Me.label3.AutoSize = True
            Me.label3.Location = New System.Drawing.Point(16, 114)
            Me.label3.Name = "label3"
            Me.label3.Size = New System.Drawing.Size(136, 13)
            Me.label3.TabIndex = 7
            Me.label3.Text = "Set max frames in template:"
            '
            'cbMIOTOff
            '
            Me.cbMIOTOff.AutoSize = True
            Me.cbMIOTOff.Location = New System.Drawing.Point(16, 87)
            Me.cbMIOTOff.Name = "cbMIOTOff"
            Me.cbMIOTOff.Size = New System.Drawing.Size(91, 17)
            Me.cbMIOTOff.TabIndex = 6
            Me.cbMIOTOff.Text = "Disable MIOT"
            Me.cbMIOTOff.UseVisualStyleBackColor = True
            '
            'tbFARN
            '
            Me.tbFARN.Culture = New System.Globalization.CultureInfo("")
            Me.tbFARN.Location = New System.Drawing.Point(300, 55)
            Me.tbFARN.Mask = "0000"
            Me.tbFARN.Name = "tbFARN"
            Me.tbFARN.Size = New System.Drawing.Size(48, 20)
            Me.tbFARN.TabIndex = 5
            Me.tbFARN.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
            Me.tbFARN.ValidatingType = GetType(Integer)
            '
            'label2
            '
            Me.label2.AutoSize = True
            Me.label2.Location = New System.Drawing.Point(250, 59)
            Me.label2.Name = "label2"
            Me.label2.Size = New System.Drawing.Size(33, 13)
            Me.label2.TabIndex = 3
            Me.label2.Text = "value"
            '
            'cbFARNLevel
            '
            Me.cbFARNLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbFARNLevel.FormattingEnabled = True
            Me.cbFARNLevel.Items.AddRange(New Object() {"low", "below normal", "normal", "above normal", "high", "maximum", "custom"})
            Me.cbFARNLevel.Location = New System.Drawing.Point(119, 55)
            Me.cbFARNLevel.Name = "cbFARNLevel"
            Me.cbFARNLevel.Size = New System.Drawing.Size(121, 21)
            Me.cbFARNLevel.TabIndex = 2
            '
            'label1
            '
            Me.label1.AutoSize = True
            Me.label1.Location = New System.Drawing.Point(16, 59)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(97, 13)
            Me.label1.TabIndex = 1
            Me.label1.Text = "Set measure level: "
            '
            'chDetectFakeFinger
            '
            Me.chDetectFakeFinger.AutoSize = True
            Me.chDetectFakeFinger.Location = New System.Drawing.Point(16, 28)
            Me.chDetectFakeFinger.Name = "chDetectFakeFinger"
            Me.chDetectFakeFinger.Size = New System.Drawing.Size(111, 17)
            Me.chDetectFakeFinger.TabIndex = 0
            Me.chDetectFakeFinger.Text = "Detect fake finger"
            Me.chDetectFakeFinger.UseVisualStyleBackColor = True
            '
            'btnVerify
            '
            Me.btnVerify.Location = New System.Drawing.Point(16, 61)
            Me.btnVerify.Name = "btnVerify"
            Me.btnVerify.Size = New System.Drawing.Size(75, 23)
            Me.btnVerify.TabIndex = 3
            Me.btnVerify.Text = "Verify"
            Me.btnVerify.UseVisualStyleBackColor = True
            '
            'btnIdentify
            '
            Me.btnIdentify.Location = New System.Drawing.Point(16, 90)
            Me.btnIdentify.Name = "btnIdentify"
            Me.btnIdentify.Size = New System.Drawing.Size(75, 23)
            Me.btnIdentify.TabIndex = 4
            Me.btnIdentify.Text = "Identify"
            Me.btnIdentify.UseVisualStyleBackColor = True
            '
            'groupBox2
            '
            Me.groupBox2.Controls.Add(Me.btnStop)
            Me.groupBox2.Controls.Add(Me.btnEnroll)
            Me.groupBox2.Controls.Add(Me.btnIdentify)
            Me.groupBox2.Controls.Add(Me.btnVerify)
            Me.groupBox2.Location = New System.Drawing.Point(12, 251)
            Me.groupBox2.Name = "groupBox2"
            Me.groupBox2.Size = New System.Drawing.Size(283, 139)
            Me.groupBox2.TabIndex = 5
            Me.groupBox2.TabStop = False
            Me.groupBox2.Text = "Operations"
            '
            'btnStop
            '
            Me.btnStop.Location = New System.Drawing.Point(194, 90)
            Me.btnStop.Name = "btnStop"
            Me.btnStop.Size = New System.Drawing.Size(75, 23)
            Me.btnStop.TabIndex = 5
            Me.btnStop.Text = "Stop"
            Me.btnStop.UseVisualStyleBackColor = True
            '
            'btnExit
            '
            Me.btnExit.Location = New System.Drawing.Point(220, 438)
            Me.btnExit.Name = "btnExit"
            Me.btnExit.Size = New System.Drawing.Size(75, 23)
            Me.btnExit.TabIndex = 6
            Me.btnExit.Text = "Exit"
            Me.btnExit.UseVisualStyleBackColor = True
            '
            'txtMessage
            '
            Me.txtMessage.Location = New System.Drawing.Point(12, 225)
            Me.txtMessage.Name = "txtMessage"
            Me.txtMessage.ReadOnly = True
            Me.txtMessage.Size = New System.Drawing.Size(460, 20)
            Me.txtMessage.TabIndex = 7
            Me.txtMessage.TabStop = False
            '
            'MainForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(487, 473)
            Me.Controls.Add(Me.txtMessage)
            Me.Controls.Add(Me.btnExit)
            Me.Controls.Add(Me.groupBox2)
            Me.Controls.Add(Me.groupBox1)
            Me.Controls.Add(Me.PictureFingerPrint)
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.Name = "MainForm"
            Me.Text = "VB.Net example for Futronic SDK v.4.2"
            CType(Me.PictureFingerPrint, System.ComponentModel.ISupportInitialize).EndInit()
            Me.groupBox1.ResumeLayout(False)
            Me.groupBox1.PerformLayout()
            Me.groupBox2.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Private WithEvents btnEnroll As System.Windows.Forms.Button
        Private PictureFingerPrint As System.Windows.Forms.PictureBox
        Private groupBox1 As System.Windows.Forms.GroupBox
        Private chDetectFakeFinger As System.Windows.Forms.CheckBox
        Private WithEvents cbFARNLevel As System.Windows.Forms.ComboBox
        Private label1 As System.Windows.Forms.Label
        Private label2 As System.Windows.Forms.Label
        Private WithEvents tbFARN As System.Windows.Forms.MaskedTextBox
        Private cbMaxFrames As System.Windows.Forms.ComboBox
        Private label3 As System.Windows.Forms.Label
        Private cbMIOTOff As System.Windows.Forms.CheckBox
        Private WithEvents btnVerify As System.Windows.Forms.Button
        Private WithEvents btnIdentify As System.Windows.Forms.Button
        Private groupBox2 As System.Windows.Forms.GroupBox
        Private WithEvents btnStop As System.Windows.Forms.Button
        Private WithEvents btnExit As System.Windows.Forms.Button
        Private txtMessage As System.Windows.Forms.TextBox
        Private m_lblIdentificationsLimit As System.Windows.Forms.Label
        Private label4 As System.Windows.Forms.Label
        Private m_cmbVersion As System.Windows.Forms.ComboBox
        Private WithEvents chFastMode As System.Windows.Forms.CheckBox
	End Class
End Namespace

