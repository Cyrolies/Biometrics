Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports System.Threading
Imports Futronic.SDKHelper


Namespace Futronic.SDK.WorkedEx
	Public Partial Class MainForm
		Inherits Form

        Const kCompanyName As String = "Futronic"
        Const kProductName As String = "SDK 4.0"
        Const kDbName As String = "DataBaseNet"

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

		Private m_bExit As Boolean

		''' <summary>
		''' The type of this parameter is depending from current operation. For
		''' enrollment operation this is DbRecord.
		''' </summary>
		Private m_OperationObj As Object

		''' <summary>
		''' A directory name to write user's information.
		''' </summary>
		Private m_DatabaseDir As String

        Private m_bInitializationSuccess As Boolean

        Private Shared rgVersionItems As ComboBoxItem() = New ComboBoxItem() {New ComboBoxItem("SDK 3.0", VersionCompatible.ftr_version_previous), New ComboBoxItem("SDK 3.5", VersionCompatible.ftr_version_current), New ComboBoxItem("Both", VersionCompatible.ftr_version_compatible)}

		Public Sub New()
			InitializeComponent()
            m_bInitializationSuccess = False
            ' Create FutronicEnrollment object for retrieve default values only
			Dim dummy As FutronicEnrollment = New FutronicEnrollment()
			cbFARNLevel.SelectedIndex = CInt(Fix(dummy.FARnLevel))
			cbMaxFrames.SelectedItem = dummy.MaxModels.ToString()
			chDetectFakeFinger.Checked = dummy.FakeDetection
            cbMIOTOff.Checked = dummy.MIOTControlOff
            chFastMode.Checked = dummy.FastMode
            SetIdentificationLimit(dummy.IdentificationsLeft)
            btnStop.Enabled = False
            m_bExit = False
            Dim selectedIndex As Integer = 0, itemIndex As Integer
            For Each item As ComboBoxItem In rgVersionItems
                itemIndex = m_cmbVersion.Items.Add(item)
                If CType(item.Tag, VersionCompatible) = dummy.Version Then
                    selectedIndex = itemIndex
                End If
            Next item
            m_cmbVersion.SelectedIndex = selectedIndex

            Try
                m_DatabaseDir = GetDatabaseDir()
            Catch e1 As UnauthorizedAccessException
                MessageBox.Show(Me, "Initialization failed. Application will be close." & Constants.vbLf & "Can not create database folder. Access denied.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return
            Catch e1 As IOException
                MessageBox.Show(Me, "Initialization failed. Application will be close." & Constants.vbLf & "Can not create database folder", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return
            End Try
            m_bInitializationSuccess = True
        End Sub

        Private Sub SetIdentificationLimit(ByVal nLimit As Integer)
            If Me.m_lblIdentificationsLimit.InvokeRequired Then
                Dim d As SetIdentificationLimitCallback = New SetIdentificationLimitCallback(AddressOf Me.SetIdentificationLimit)
                Me.Invoke(d, New Object() {nLimit})
            Else
                If nLimit = Int32.MaxValue Then
                    m_lblIdentificationsLimit.Text = "Identification limit: No limits"
                Else
                    m_lblIdentificationsLimit.Text = String.Format("Identification limit: {0}", nLimit)
                End If
            End If
        End Sub

        Private Sub btnEnroll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEnroll.Click
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
                m_Operation.Dispose()
                m_Operation = Nothing
            End If
            m_Operation = New FutronicEnrollment()

            ' Set control properties
            m_Operation.FakeDetection = chDetectFakeFinger.Checked
            m_Operation.FFDControl = True
            m_Operation.FARN = Int32.Parse(tbFARN.Text)
            m_Operation.Version = CType((CType(m_cmbVersion.SelectedItem, ComboBoxItem)).Tag, VersionCompatible)
            m_Operation.FastMode = chFastMode.Checked

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

        Private Sub btnVerify_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVerify.Click
            EnableControls(False)
            SetStatusText("Programme is loading database, please wait ...")
            Dim Users As List(Of DbRecord) = DbRecord.ReadRecords(m_DatabaseDir)
            SetStatusText(String.Empty)
            If Users.Count = 0 Then
                EnableControls(True)
                MessageBox.Show(Me, "Users not found. Please, run enrollment process first.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return
            End If

            ' select user's information for verification
            Dim frmSelectUser As SelectUser = New SelectUser(Users, m_DatabaseDir)
            frmSelectUser.ShowDialog(Me)

            If frmSelectUser.SelectedUser Is Nothing Then
                EnableControls(True)
                MessageBox.Show(Me, "No selected user", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return
            End If
            m_OperationObj = frmSelectUser.SelectedUser

            If Not m_Operation Is Nothing Then
                m_Operation.Dispose()
                m_Operation = Nothing
            End If
            m_Operation = New FutronicVerification((CType(m_OperationObj, DbRecord)).Template)

            ' Set control properties
            m_Operation.FakeDetection = chDetectFakeFinger.Checked
            m_Operation.FFDControl = True
            m_Operation.FARN = Int32.Parse(tbFARN.Text)
            m_Operation.Version = CType((CType(m_cmbVersion.SelectedItem, ComboBoxItem)).Tag, VersionCompatible)
            m_Operation.FastMode = chFastMode.Checked

            ' register events
            AddHandler m_Operation.OnPutOn, AddressOf OnPutOn
            AddHandler m_Operation.OnTakeOff, AddressOf OnTakeOff
            AddHandler m_Operation.UpdateScreenImage, AddressOf UpdateScreenImage
            AddHandler m_Operation.OnFakeSource, AddressOf OnFakeSource
            AddHandler (CType(m_Operation, FutronicVerification)).OnVerificationComplete, AddressOf OnVerificationComplete

            ' start verification process
            CType(m_Operation, FutronicVerification).Verification()
        End Sub

        Private Sub btnIdentify_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnIdentify.Click
            EnableControls(False)
            SetStatusText("Programme is loading database, please wait ...")
            Dim Users As List(Of DbRecord) = DbRecord.ReadRecords(m_DatabaseDir)
            SetStatusText(String.Empty)
            If Users.Count = 0 Then
                MessageBox.Show(Me, "Users not found. Please, run enrollment process first.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                EnableControls(True)
                Return
            End If
            m_OperationObj = Users
            If Not m_Operation Is Nothing Then
                m_Operation.Dispose()
                m_Operation = Nothing
            End If
            m_Operation = New FutronicIdentification()

            ' Set control property
            m_Operation.FakeDetection = chDetectFakeFinger.Checked
            m_Operation.FFDControl = True
            m_Operation.FARN = Int32.Parse(tbFARN.Text)
            m_Operation.Version = CType((CType(m_cmbVersion.SelectedItem, ComboBoxItem)).Tag, VersionCompatible)
            m_Operation.FastMode = chFastMode.Checked

            ' register events
            AddHandler m_Operation.OnPutOn, AddressOf OnPutOn
            AddHandler m_Operation.OnTakeOff, AddressOf OnTakeOff
            AddHandler m_Operation.UpdateScreenImage, AddressOf UpdateScreenImage
            AddHandler m_Operation.OnFakeSource, AddressOf OnFakeSource
            AddHandler (CType(m_Operation, FutronicIdentification)).OnGetBaseTemplateComplete, AddressOf OnGetBaseTemplateComplete

            ' start identification process
            CType(m_Operation, FutronicIdentification).GetBaseTemplate()
        End Sub

		Private Sub cbFARNLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbFARNLevel.SelectedIndexChanged
			If cbFARNLevel.SelectedIndex = 6 Then
				tbFARN.ReadOnly = False
			Else
				tbFARN.Text = FutronicSdkBase.rgFARN(cbFARNLevel.SelectedIndex).ToString()
				tbFARN.ReadOnly = True
			End If
		End Sub

		Private Sub OnPutOn(ByVal Progress As FTR_PROGRESS)
			Me.SetStatusText("Put finger into device, please ...")
		End Sub

		Private Sub OnTakeOff(ByVal Progress As FTR_PROGRESS)
			Me.SetStatusText("Take off finger from device, please ...")
		End Sub

		Private Sub UpdateScreenImage(ByVal hBitmap As Bitmap)
			' Do not change the state control during application closing.
			If m_bExit Then
				Return
			End If

			If PictureFingerPrint.InvokeRequired Then
				Dim d As SetImageCallback = New SetImageCallback(AddressOf Me.UpdateScreenImage)
				Me.Invoke(d, New Object() { hBitmap })
			Else
				PictureFingerPrint.Image = hBitmap
			End If
		End Sub

		Private Function OnFakeSource(ByVal Progress As FTR_PROGRESS) As Boolean
			If m_bExit Then
				Return True
			End If

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
				Dim Users As List(Of DbRecord) = CType(m_OperationObj, List(Of DbRecord))

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
						szMessage.Append(Users(iRecords).UserName)
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

		Private Sub btnStop_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStop.Click
			m_Operation.OnCalcel()
		End Sub

		Private Sub EnableControls(ByVal bEnable As Boolean)
			' Do not change the state control during application closing.
			If m_bExit Then
				Return
			End If
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
			' Do not change the state control during application closing.
			If m_bExit Then
				Return
			End If

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
            szDbDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)

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

		Protected Overrides Sub OnFormClosing(ByVal e As FormClosingEventArgs)
			m_bExit = True
			If Not m_Operation Is Nothing Then
				m_Operation.Dispose()
			End If
			MyBase.OnFormClosing(e)
		End Sub

		Private Sub btnExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExit.Click
			MyBase.Hide()
			m_bExit = True
			Me.Close()
		End Sub

		Private Sub tbFARN_Validating(ByVal sender As Object, ByVal e As CancelEventArgs) Handles tbFARN.Validating
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

        Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not m_bInitializationSuccess Then
                Me.Close()
            End If
        End Sub
    End Class
End Namespace
