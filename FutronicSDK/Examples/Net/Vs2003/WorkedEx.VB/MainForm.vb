Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.IO
Imports System.Text
Imports Futronic.SDKHelper

Namespace Futronic.SDK.WorkedEx
	''' <summary>
	''' Summary description for Form1.
	''' </summary>
	Public Class MainForm
		Inherits System.Windows.Forms.Form
		Private groupBox1 As System.Windows.Forms.GroupBox
		Private chDetectFakeFinger As System.Windows.Forms.CheckBox
		Private label1 As System.Windows.Forms.Label
		Private WithEvents cbFARNLevel As System.Windows.Forms.ComboBox
		Private label2 As System.Windows.Forms.Label
		Private WithEvents tbFARN As System.Windows.Forms.TextBox
        Private label3 As System.Windows.Forms.Label
		Private cbMaxFrames As System.Windows.Forms.ComboBox
		Private txtMessage As System.Windows.Forms.TextBox
		Private PictureFingerPrint As System.Windows.Forms.PictureBox
		Private groupBox2 As System.Windows.Forms.GroupBox
		Private WithEvents btnEnroll As System.Windows.Forms.Button
		Private WithEvents btnVerify As System.Windows.Forms.Button
		Private WithEvents btnIdentify As System.Windows.Forms.Button
		Private WithEvents btnStop As System.Windows.Forms.Button
		Private WithEvents btnExit As System.Windows.Forms.Button

        Const kCompanyName As String = "Futronic"
        Const kProductName As String = "SDK 4.0"
        Const kDbName As String = "DataBaseNet"

        ''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.Container = Nothing

		''' <summary>
		''' This delegate enables asynchronous calls for setting
		''' the text property on a status control.
		''' </summary>
		''' <param name="text"></param>
		Private Delegate Sub SetTextCallback(ByVal text As String)

		''' <summary>
		''' This delegate enables asynchronous calls for setting
		''' the text property on a identification limit control.
		''' </summary>
		''' <param name="text"></param>
		Private Delegate Sub SetIdentificationLimitCallback(ByVal limit As Integer)

		''' <summary>
		''' This delegate enables asynchronous calls for setting
		''' the Image property on a PictureBox control.
		''' </summary>
		''' <param name="hBitmap">the instance of Bitmap class</param>
		Private Delegate Sub SetImageCallback(ByVal hBitmap As Bitmap)

		''' <summary>
		''' This delegate enables asynchronous calls for setting
		''' the Enable property on a buttons.
		''' </summary>
		''' <param name="bEnable">true to enable buttons, otherwise to disable</param>
		Private Delegate Sub EnableControlsCallback(ByVal bEnable As Boolean)

		''' <summary>
		''' Contain reference for current operation object
		''' </summary>
		Private m_Operation As FutronicSdkBase

		''' <summary>
		''' The type of this parameter is depending from current operation. For
		''' enrollment operation this is DbRecord.
		''' </summary>
		Private m_OperationObj As Object
		Private m_lblIdentificationsLimit As System.Windows.Forms.Label
		Private label4 As System.Windows.Forms.Label
		Private m_cmbVersion As System.Windows.Forms.ComboBox

		''' <summary>
		''' A directory name to write user's information.
		''' </summary>
		Private m_DatabaseDir As String

        Private Shared rgVersionItems As ComboBoxItem() = New ComboBoxItem() {New ComboBoxItem("SDK 3.0", VersionCompatible.ftr_version_previous), New ComboBoxItem("SDK 3.5", VersionCompatible.ftr_version_current), New ComboBoxItem("Both", VersionCompatible.ftr_version_compatible)}

		Public Sub New()
			'
			' Required for Windows Form Designer support
			'
			InitializeComponent()

			Try
				m_DatabaseDir = GetDatabaseDir()
			Catch e1 As IOException
				MessageBox.Show(Me, "Initialization failed. Application will be close." & Constants.vbLf & "Can not create database folder", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				Return
			End Try

			' Create FutronicEnrollment object for retrieve default values only
			Dim dummy As FutronicEnrollment = New FutronicEnrollment()
			cbFARNLevel.SelectedIndex = CInt(Fix(dummy.FARnLevel))
			cbMaxFrames.SelectedItem = dummy.MaxModels.ToString()
			chDetectFakeFinger.Checked = dummy.FakeDetection
            cbMIOTOff.Checked = dummy.MIOTControlOff
			SetIdentificationLimit(dummy.IdentificationsLeft)
            chFastMode.Checked = dummy.FastMode

			Dim selectedIndex As Integer = 0, itemIndex As Integer
			For Each item As ComboBoxItem In rgVersionItems
				itemIndex = m_cmbVersion.Items.Add(item)
				If CType(item.Tag, VersionCompatible) = dummy.Version Then
					selectedIndex = itemIndex
				End If
			Next item
			m_cmbVersion.SelectedIndex = selectedIndex

			btnStop.Enabled = False
		End Sub

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		Protected Overrides Overloads Sub Dispose(ByVal disposing As Boolean)
			If disposing Then
				If Not components Is Nothing Then
					components.Dispose()
				End If
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"
		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
        Private WithEvents cbMIOTOff As System.Windows.Forms.CheckBox
        Friend WithEvents chFastMode As System.Windows.Forms.CheckBox
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
            Me.groupBox1 = New System.Windows.Forms.GroupBox
            Me.m_cmbVersion = New System.Windows.Forms.ComboBox
            Me.label4 = New System.Windows.Forms.Label
            Me.m_lblIdentificationsLimit = New System.Windows.Forms.Label
            Me.cbMaxFrames = New System.Windows.Forms.ComboBox
            Me.label3 = New System.Windows.Forms.Label
            Me.cbMIOTOff = New System.Windows.Forms.CheckBox
            Me.tbFARN = New System.Windows.Forms.TextBox
            Me.label2 = New System.Windows.Forms.Label
            Me.cbFARNLevel = New System.Windows.Forms.ComboBox
            Me.label1 = New System.Windows.Forms.Label
            Me.chDetectFakeFinger = New System.Windows.Forms.CheckBox
            Me.txtMessage = New System.Windows.Forms.TextBox
            Me.PictureFingerPrint = New System.Windows.Forms.PictureBox
            Me.groupBox2 = New System.Windows.Forms.GroupBox
            Me.btnStop = New System.Windows.Forms.Button
            Me.btnIdentify = New System.Windows.Forms.Button
            Me.btnVerify = New System.Windows.Forms.Button
            Me.btnEnroll = New System.Windows.Forms.Button
            Me.btnExit = New System.Windows.Forms.Button
            Me.chFastMode = New System.Windows.Forms.CheckBox
            Me.groupBox1.SuspendLayout()
            Me.groupBox2.SuspendLayout()
            Me.SuspendLayout()
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
            Me.groupBox1.Location = New System.Drawing.Point(8, 8)
            Me.groupBox1.Name = "groupBox1"
            Me.groupBox1.Size = New System.Drawing.Size(464, 216)
            Me.groupBox1.TabIndex = 0
            Me.groupBox1.TabStop = False
            Me.groupBox1.Text = " Settings "
            '
            'm_cmbVersion
            '
            Me.m_cmbVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.m_cmbVersion.Location = New System.Drawing.Point(208, 144)
            Me.m_cmbVersion.Name = "m_cmbVersion"
            Me.m_cmbVersion.Size = New System.Drawing.Size(121, 21)
            Me.m_cmbVersion.TabIndex = 10
            '
            'label4
            '
            Me.label4.Location = New System.Drawing.Point(16, 146)
            Me.label4.Name = "label4"
            Me.label4.Size = New System.Drawing.Size(184, 16)
            Me.label4.TabIndex = 9
            Me.label4.Text = "Do image processing compatible to: "
            '
            'm_lblIdentificationsLimit
            '
            Me.m_lblIdentificationsLimit.Location = New System.Drawing.Point(16, 184)
            Me.m_lblIdentificationsLimit.Name = "m_lblIdentificationsLimit"
            Me.m_lblIdentificationsLimit.Size = New System.Drawing.Size(392, 16)
            Me.m_lblIdentificationsLimit.TabIndex = 8
            '
            'cbMaxFrames
            '
            Me.cbMaxFrames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbMaxFrames.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
            Me.cbMaxFrames.Location = New System.Drawing.Point(176, 110)
            Me.cbMaxFrames.Name = "cbMaxFrames"
            Me.cbMaxFrames.Size = New System.Drawing.Size(48, 21)
            Me.cbMaxFrames.TabIndex = 7
            '
            'label3
            '
            Me.label3.Location = New System.Drawing.Point(16, 112)
            Me.label3.Name = "label3"
            Me.label3.Size = New System.Drawing.Size(152, 16)
            Me.label3.TabIndex = 6
            Me.label3.Text = "Set max frames in template:"
            '
            'cbMIOTOff
            '
            Me.cbMIOTOff.Location = New System.Drawing.Point(16, 88)
            Me.cbMIOTOff.Name = "cbMIOTOff"
            Me.cbMIOTOff.Size = New System.Drawing.Size(104, 16)
            Me.cbMIOTOff.TabIndex = 5
            Me.cbMIOTOff.Text = "Disable MIOT"
            '
            'tbFARN
            '
            Me.tbFARN.Location = New System.Drawing.Point(304, 54)
            Me.tbFARN.MaxLength = 5
            Me.tbFARN.Name = "tbFARN"
            Me.tbFARN.TabIndex = 4
            Me.tbFARN.Text = ""
            '
            'label2
            '
            Me.label2.Location = New System.Drawing.Point(256, 56)
            Me.label2.Name = "label2"
            Me.label2.Size = New System.Drawing.Size(40, 16)
            Me.label2.TabIndex = 3
            Me.label2.Text = "value"
            '
            'cbFARNLevel
            '
            Me.cbFARNLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbFARNLevel.Items.AddRange(New Object() {"low", "below normal", "normal", "above normal", "high", "maximum", "custom"})
            Me.cbFARNLevel.Location = New System.Drawing.Point(120, 54)
            Me.cbFARNLevel.Name = "cbFARNLevel"
            Me.cbFARNLevel.Size = New System.Drawing.Size(121, 21)
            Me.cbFARNLevel.TabIndex = 2
            '
            'label1
            '
            Me.label1.Location = New System.Drawing.Point(16, 56)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(100, 16)
            Me.label1.TabIndex = 1
            Me.label1.Text = "Set measure level: "
            '
            'chDetectFakeFinger
            '
            Me.chDetectFakeFinger.Location = New System.Drawing.Point(16, 24)
            Me.chDetectFakeFinger.Name = "chDetectFakeFinger"
            Me.chDetectFakeFinger.Size = New System.Drawing.Size(128, 24)
            Me.chDetectFakeFinger.TabIndex = 0
            Me.chDetectFakeFinger.Text = "Detect fake finger"
            '
            'txtMessage
            '
            Me.txtMessage.Location = New System.Drawing.Point(8, 232)
            Me.txtMessage.Name = "txtMessage"
            Me.txtMessage.ReadOnly = True
            Me.txtMessage.Size = New System.Drawing.Size(464, 20)
            Me.txtMessage.TabIndex = 1
            Me.txtMessage.TabStop = False
            Me.txtMessage.Text = ""
            '
            'PictureFingerPrint
            '
            Me.PictureFingerPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.PictureFingerPrint.Image = CType(resources.GetObject("PictureFingerPrint.Image"), System.Drawing.Image)
            Me.PictureFingerPrint.Location = New System.Drawing.Point(312, 264)
            Me.PictureFingerPrint.Name = "PictureFingerPrint"
            Me.PictureFingerPrint.Size = New System.Drawing.Size(160, 210)
            Me.PictureFingerPrint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
            Me.PictureFingerPrint.TabIndex = 2
            Me.PictureFingerPrint.TabStop = False
            '
            'groupBox2
            '
            Me.groupBox2.Controls.Add(Me.btnStop)
            Me.groupBox2.Controls.Add(Me.btnIdentify)
            Me.groupBox2.Controls.Add(Me.btnVerify)
            Me.groupBox2.Controls.Add(Me.btnEnroll)
            Me.groupBox2.Location = New System.Drawing.Point(8, 264)
            Me.groupBox2.Name = "groupBox2"
            Me.groupBox2.Size = New System.Drawing.Size(288, 160)
            Me.groupBox2.TabIndex = 3
            Me.groupBox2.TabStop = False
            Me.groupBox2.Text = "Operations"
            '
            'btnStop
            '
            Me.btnStop.Location = New System.Drawing.Point(200, 104)
            Me.btnStop.Name = "btnStop"
            Me.btnStop.TabIndex = 3
            Me.btnStop.Text = "Stop"
            '
            'btnIdentify
            '
            Me.btnIdentify.Location = New System.Drawing.Point(16, 104)
            Me.btnIdentify.Name = "btnIdentify"
            Me.btnIdentify.TabIndex = 2
            Me.btnIdentify.Text = "Identify"
            '
            'btnVerify
            '
            Me.btnVerify.Location = New System.Drawing.Point(16, 64)
            Me.btnVerify.Name = "btnVerify"
            Me.btnVerify.TabIndex = 1
            Me.btnVerify.Text = "Verify"
            '
            'btnEnroll
            '
            Me.btnEnroll.Location = New System.Drawing.Point(16, 24)
            Me.btnEnroll.Name = "btnEnroll"
            Me.btnEnroll.TabIndex = 0
            Me.btnEnroll.Text = "Enroll"
            '
            'btnExit
            '
            Me.btnExit.Location = New System.Drawing.Point(208, 456)
            Me.btnExit.Name = "btnExit"
            Me.btnExit.TabIndex = 4
            Me.btnExit.Text = "Exit"
            '
            'chFastMode
            '
            Me.chFastMode.Location = New System.Drawing.Point(208, 24)
            Me.chFastMode.Name = "chFastMode"
            Me.chFastMode.TabIndex = 12
            Me.chFastMode.Text = "Fast mode"
            '
            'MainForm
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(487, 492)
            Me.Controls.Add(Me.btnExit)
            Me.Controls.Add(Me.groupBox2)
            Me.Controls.Add(Me.PictureFingerPrint)
            Me.Controls.Add(Me.txtMessage)
            Me.Controls.Add(Me.groupBox1)
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.Name = "MainForm"
            Me.Text = "VB.Net example for Futronic SDK v.4.2"
            Me.groupBox1.ResumeLayout(False)
            Me.groupBox2.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
#End Region

		''' <summary>
		''' The main entry point for the application.
		''' </summary>
		<STAThread> _
		Shared Sub Main()
			Application.Run(New MainForm())
		End Sub

		Private Sub SetIdentificationLimit(ByVal nLimit As Integer)
			If Me.m_lblIdentificationsLimit.InvokeRequired Then
				Dim d As SetIdentificationLimitCallback = New SetIdentificationLimitCallback(AddressOf Me.SetIdentificationLimit)
				Me.Invoke(d, New Object() { nLimit })
			Else
				If nLimit = Int32.MaxValue Then
					m_lblIdentificationsLimit.Text = "Identification limit: No limits"
				Else
					m_lblIdentificationsLimit.Text = String.Format("Identification limit: {0}", nLimit)
				End If
			End If
		End Sub

		Private Sub cbFARNLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbFARNLevel.SelectedIndexChanged
			If cbFARNLevel.SelectedIndex = 6 Then
				tbFARN.ReadOnly = False
			Else
				tbFARN.Text = FutronicSdkBase.rgFARN(cbFARNLevel.SelectedIndex).ToString()
				tbFARN.ReadOnly = True
			End If
		End Sub

		Private Sub tbFARN_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles tbFARN.Validating
			Dim nValue As Integer = -1

			Try
				nValue = Int32.Parse(tbFARN.Text)
			Catch e1 As FormatException
			End Try
			If nValue > 1000 OrElse nValue < 1 Then
				MessageBox.Show(Me, "Invalid FARN value. The range of value is from 1 to 1000", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				e.Cancel = True
			End If

		End Sub

		Private Sub btnEnroll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEnroll.Click
			Dim User As DbRecord = New DbRecord()

			' Get user name
			Dim frmName As EnrollmentName = New EnrollmentName()
			frmName.ShowDialog(Me)
			If frmName.DialogResult <> System.Windows.Forms.DialogResult.OK Then
				Return
			End If
			If frmName.UserName.Length = 0 Then
				MessageBox.Show(Me, "You must enter a user name.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				Return
			End If
			' Try creat the file for user's information
			If isUserExists(frmName.UserName) Then
				Dim nResponse As DialogResult
                nResponse = MessageBox.Show("User already exists. Do you want replace it?", "VB.Net example for Futronic SDK", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				If nResponse = DialogResult.No Then
					Return
				End If
			Else
				Try
					CreateFile(frmName.UserName)
				Catch e1 As DirectoryNotFoundException
					MessageBox.Show(Me, "Can not create file to save an user's information.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
					Return
				Catch e2 As IOException
					MessageBox.Show(Me, String.Format("Bad user name '{0}'.", frmName.UserName), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
					Return
				End Try
			End If
			User.UserName = frmName.UserName

			m_OperationObj = User
			If Not m_Operation Is Nothing Then
				' Do not call Dispose function in completion routine.
				m_Operation.Dispose()
				m_Operation = Nothing
			End If
			m_Operation = New FutronicEnrollment()

			' Set control properties
			m_Operation.FakeDetection = chDetectFakeFinger.Checked
			m_Operation.FFDControl = True
			m_Operation.FARN = Int32.Parse(tbFARN.Text)
			m_Operation.Version = CType((CType(m_cmbVersion.SelectedItem, ComboBoxItem)).Tag, VersionCompatible)
            m_Operation.FastMode() = chFastMode.Checked
            CType(m_Operation, FutronicEnrollment).MIOTControlOff = cbMIOTOff.Checked
			CType(m_Operation, FutronicEnrollment).MaxModels = Int32.Parse(CType(cbMaxFrames.SelectedItem, String))

			EnableControls(False)

			' register events
			AddHandler m_Operation.OnPutOn, AddressOf OnPutOn
			AddHandler m_Operation.OnTakeOff, AddressOf OnTakeOff
			AddHandler m_Operation.UpdateScreenImage, AddressOf UpdateScreenImage
            AddHandler m_Operation.OnFakeSource, AddressOf OnFakeSource
            AddHandler (CType(m_Operation, FutronicEnrollment)).OnEnrollmentComplete, AddressOf OnEnrollmentComplete

			' start enrollment process
			CType(m_Operation, FutronicEnrollment).Enrollment()

		End Sub

		Private Sub btnVerify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVerify.Click
			EnableControls(False)
			SetStatusText("Programme is loading database, please wait ...")
			Dim Users As ArrayList = DbRecord.ReadRecords(m_DatabaseDir)
			SetStatusText(String.Empty)
			If Users.Count = 0 Then
				EnableControls(True)
				MessageBox.Show(Me, "Users not found. Please, run enrollment process first.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				Return
			End If
			' select user for verification
			Dim frmSelectUser As SelectUser = New SelectUser(Users, m_DatabaseDir)
			frmSelectUser.ShowDialog(Me)

			If frmSelectUser.SelectedUser Is Nothing Then
				EnableControls(True)
				MessageBox.Show(Me, "No selected user", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				Return
			End If
			m_OperationObj = frmSelectUser.SelectedUser

			If Not m_Operation Is Nothing Then
				' Do not call Dispose function in completion routine.
				m_Operation.Dispose()
				m_Operation = Nothing
			End If
			m_Operation = New FutronicVerification((CType(m_OperationObj, DbRecord)).Template)

			' Set control properties
			m_Operation.FakeDetection = chDetectFakeFinger.Checked
			m_Operation.FFDControl = True
			m_Operation.FARN = Int32.Parse(tbFARN.Text)
			m_Operation.Version = CType((CType(m_cmbVersion.SelectedItem, ComboBoxItem)).Tag, VersionCompatible)
            m_Operation.FastMode() = chFastMode.Checked

            ' register events
			AddHandler m_Operation.OnPutOn, AddressOf OnPutOn
			AddHandler m_Operation.OnTakeOff, AddressOf OnTakeOff
			AddHandler m_Operation.UpdateScreenImage, AddressOf UpdateScreenImage
            AddHandler m_Operation.OnFakeSource, AddressOf OnFakeSource
            AddHandler (CType(m_Operation, FutronicVerification)).OnVerificationComplete, AddressOf OnVerificationComplete

			' start verification process
			CType(m_Operation, FutronicVerification).Verification()
		End Sub

		Private Sub btnIdentify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIdentify.Click
			EnableControls(False)
			SetStatusText("Programme is loading database, please wait ...")
			Dim Users As ArrayList = DbRecord.ReadRecords(m_DatabaseDir)
			SetStatusText(String.Empty)
			If Users.Count = 0 Then
				MessageBox.Show(Me, "Users not found. Please, run enrollment process first.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				EnableControls(True)
				Return
			End If

			m_OperationObj = Users
			If Not m_Operation Is Nothing Then
				' Do not call Dispose function in completion routine.
				m_Operation.Dispose()
				m_Operation = Nothing
			End If
			m_Operation = New FutronicIdentification()

			' Set control property
			m_Operation.FakeDetection = chDetectFakeFinger.Checked
			m_Operation.FFDControl = True
			m_Operation.FARN = Int32.Parse(tbFARN.Text)
			m_Operation.Version = CType((CType(m_cmbVersion.SelectedItem, ComboBoxItem)).Tag, VersionCompatible)
            m_Operation.FastMode() = chFastMode.Checked

			' register events
			AddHandler m_Operation.OnPutOn, AddressOf OnPutOn
			AddHandler m_Operation.OnTakeOff, AddressOf OnTakeOff
			AddHandler m_Operation.UpdateScreenImage, AddressOf UpdateScreenImage
            AddHandler m_Operation.OnFakeSource, AddressOf OnFakeSource
            AddHandler (CType(m_Operation, FutronicIdentification)).OnGetBaseTemplateComplete, AddressOf OnGetBaseTemplateComplete

			' start identification process
			CType(m_Operation, FutronicIdentification).GetBaseTemplate()
		End Sub

		Private Sub OnPutOn(ByVal Progress As MFTR_PROGRESS)
			Me.SetStatusText("Put finger into device, please ...")
		End Sub

		Private Sub OnTakeOff(ByVal Progress As MFTR_PROGRESS)
			Me.SetStatusText("Take off finger from device, please ...")
		End Sub

		Private Sub UpdateScreenImage(ByVal hBitmap As Bitmap)
			If PictureFingerPrint.InvokeRequired Then
				Dim d As SetImageCallback = New SetImageCallback(AddressOf Me.UpdateScreenImage)
				Me.Invoke(d, New Object() { hBitmap })
			Else
				PictureFingerPrint.Image = hBitmap
			End If
		End Sub

		Private Function OnFakeSource(ByVal Progress As MFTR_PROGRESS) As Boolean
			Dim result As DialogResult
            result = MessageBox.Show("Fake source detected. Do you want continue process?", "VB.Net example for Futronic SDK", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
			Return (result = DialogResult.No)
		End Function

		Private Sub OnEnrollmentComplete(ByVal bSuccess As Boolean, ByVal nRetCode As Integer)
			Dim szMessage As StringBuilder = New StringBuilder()
			If bSuccess Then
				' set status string
				szMessage.Append("Enrollment process finished successfully.")
				szMessage.Append("Quality: ")
				szMessage.Append((CType(m_Operation, FutronicEnrollment)).Quality.ToString())
				Me.SetStatusText(szMessage.ToString())

				' Set template into user's information and save it
				Dim User As DbRecord = CType(m_OperationObj, DbRecord)
				User.Template = (CType(m_Operation, FutronicEnrollment)).Template

				Dim szFileName As String = Path.Combine(m_DatabaseDir, User.UserName)
				If (Not User.Save(szFileName)) Then
                    MessageBox.Show("Can not save users's information to file " & szFileName, "VB.Net example for Futronic SDK", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				End If
			Else
				szMessage.Append("Enrollment process failed.")
				szMessage.Append("Error description: ")
				szMessage.Append(FutronicSdkBase.SdkRetCode2Message(nRetCode))
				Me.SetStatusText(szMessage.ToString())
			End If

            ' unregister events
            RemoveHandler m_Operation.OnPutOn, AddressOf OnPutOn
            RemoveHandler m_Operation.OnTakeOff, AddressOf OnTakeOff
            RemoveHandler m_Operation.UpdateScreenImage, AddressOf UpdateScreenImage
            RemoveHandler m_Operation.OnFakeSource, AddressOf OnFakeSource
            RemoveHandler (CType(m_Operation, FutronicEnrollment)).OnEnrollmentComplete, AddressOf OnEnrollmentComplete

            m_OperationObj = Nothing
			EnableControls(True)
		End Sub

		Private Sub OnVerificationComplete(ByVal bSuccess As Boolean, ByVal nRetCode As Integer, ByVal bVerificationSuccess As Boolean)
			Dim szResult As StringBuilder = New StringBuilder()
			If bSuccess Then
				If bVerificationSuccess Then
					szResult.Append("Verification is successful.")
					szResult.Append("User Name: ")
					szResult.Append((CType(m_OperationObj, DbRecord)).UserName)
				Else
					szResult.Append("Verification failed.")
				End If
			Else
				szResult.Append("Verification process failed.")
				szResult.Append("Error description: ")
				szResult.Append(FutronicSdkBase.SdkRetCode2Message(nRetCode))
			End If

			Me.SetStatusText(szResult.ToString())
			Me.SetIdentificationLimit(m_Operation.IdentificationsLeft)

            ' unregister events
            RemoveHandler m_Operation.OnPutOn, AddressOf OnPutOn
            RemoveHandler m_Operation.OnTakeOff, AddressOf OnTakeOff
            RemoveHandler m_Operation.UpdateScreenImage, AddressOf UpdateScreenImage
            RemoveHandler m_Operation.OnFakeSource, AddressOf OnFakeSource
            RemoveHandler (CType(m_Operation, FutronicVerification)).OnVerificationComplete, AddressOf OnVerificationComplete

            m_OperationObj = Nothing
			EnableControls(True)
		End Sub

		Private Sub OnGetBaseTemplateComplete(ByVal bSuccess As Boolean, ByVal nRetCode As Integer)
			Dim szMessage As StringBuilder = New StringBuilder()
			If bSuccess Then
				Me.SetStatusText("Starting identification...")
				Dim Users As ArrayList = CType(m_OperationObj, ArrayList)

				Dim iRecords As Integer = 0
				Dim nResult As Integer
				Dim rgRecords As FtrIdentifyRecord() = New FtrIdentifyRecord(Users.Count - 1){}
				For Each item As DbRecord In Users
					rgRecords(iRecords) = item.GetRecord()
					iRecords += 1
				Next item
				nResult = (CType(m_Operation, FutronicIdentification)).Identification(rgRecords, iRecords)
				If nResult = FutronicSdkBase.RETCODE_OK Then
					szMessage.Append("Identification process complete. User: ")
					If iRecords <> -1 Then
						szMessage.Append((CType(Users(iRecords), DbRecord)).UserName)
					Else
						szMessage.Append("not found")
					End If
				Else
					szMessage.Append("Identification failed.")
					szMessage.Append(FutronicSdkBase.SdkRetCode2Message(nResult))
				End If
				Me.SetIdentificationLimit(m_Operation.IdentificationsLeft)
			Else
				szMessage.Append("Can not retrieve base template.")
				szMessage.Append("Error description: ")
				szMessage.Append(FutronicSdkBase.SdkRetCode2Message(nRetCode))
			End If
			Me.SetStatusText(szMessage.ToString())

            ' unregister events
            RemoveHandler m_Operation.OnPutOn, AddressOf OnPutOn
            RemoveHandler m_Operation.OnTakeOff, AddressOf OnTakeOff
            RemoveHandler m_Operation.UpdateScreenImage, AddressOf UpdateScreenImage
            RemoveHandler m_Operation.OnFakeSource, AddressOf OnFakeSource
            RemoveHandler (CType(m_Operation, FutronicIdentification)).OnGetBaseTemplateComplete, AddressOf OnGetBaseTemplateComplete

            m_OperationObj = Nothing
			EnableControls(True)
		End Sub

		Private Sub btnStop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStop.Click
			m_Operation.OnCalcel()
		End Sub

		Private Sub EnableControls(ByVal bEnable As Boolean)
			If Me.InvokeRequired Then
				Dim d As EnableControlsCallback = New EnableControlsCallback(AddressOf Me.EnableControls)
				Me.Invoke(d, New Object() { bEnable })
			Else
				btnEnroll.Enabled = bEnable
				btnIdentify.Enabled = bEnable
				btnVerify.Enabled = bEnable
				btnStop.Enabled = Not bEnable
			End If
		End Sub

		Private Sub SetStatusText(ByVal text As String)
			If Me.txtMessage.InvokeRequired Then
				Dim d As SetTextCallback = New SetTextCallback(AddressOf Me.SetStatusText)
				Me.Invoke(d, New Object() { text })
			Else
				Me.txtMessage.Text = text
				Me.Update()
			End If
		End Sub

		''' <summary>
		''' Get the database directory.
		''' </summary>
		''' <returns>returns the database directory.</returns>
		Public Shared Function GetDatabaseDir() As String
			Dim szDbDir As String
            szDbDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal)

            szDbDir = Path.Combine(szDbDir, kCompanyName)
            If (Not Directory.Exists(szDbDir)) Then
                Directory.CreateDirectory(szDbDir)
            End If

            szDbDir = Path.Combine(szDbDir, kProductName)
            If (Not Directory.Exists(szDbDir)) Then
                Directory.CreateDirectory(szDbDir)
            End If

            szDbDir = Path.Combine(szDbDir, kDbName)
            If (Not Directory.Exists(szDbDir)) Then
                Directory.CreateDirectory(szDbDir)
            End If

            Return szDbDir
		End Function

		Protected Function isUserExists(ByVal UserName As String) As Boolean
			Dim szFileName As String
			szFileName = Path.Combine(m_DatabaseDir, UserName)
			Return File.Exists(szFileName)
		End Function

		Protected Sub CreateFile(ByVal UserName As String)
			Dim szFileName As String
			szFileName = Path.Combine(m_DatabaseDir, UserName)
			File.Create(szFileName).Close()
			File.Delete(szFileName)
		End Sub

		Protected Overrides Sub OnClosing(ByVal e As CancelEventArgs)
			If Not m_Operation Is Nothing Then
				m_Operation.Dispose()
				m_Operation = Nothing
			End If
			MyBase.OnClosing(e)
		End Sub

		Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
			If Not m_Operation Is Nothing Then
				m_Operation.Dispose()
				m_Operation = Nothing
			End If
			Me.Close()
		End Sub

	End Class
End Namespace
