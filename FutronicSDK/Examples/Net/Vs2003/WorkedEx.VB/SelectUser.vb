Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.IO

Namespace Futronic.SDK.WorkedEx
	''' <summary>
	''' Summary description for SelectUser.
	''' </summary>
	Public Class SelectUser
		Inherits System.Windows.Forms.Form
		Private label1 As System.Windows.Forms.Label
		Private WithEvents btnSelect As System.Windows.Forms.Button
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.Container = Nothing
		Private txtDatabaseDir As System.Windows.Forms.TextBox
		Private lstUsers As System.Windows.Forms.ListBox

		Private m_Users As ArrayList
		Private m_SelectedIndex As Integer

		Public Sub New(ByVal Users As ArrayList, ByVal szDbDir As String)
			'
			' Required for Windows Form Designer support
			'
			InitializeComponent()

			txtDatabaseDir.Text = szDbDir
			m_Users = Users
			Dim i As Integer = 0
			Do While i < m_Users.Count
				lstUsers.Items.Add((CType(m_Users(i), DbRecord)).UserName)
				i += 1
			Loop
			lstUsers.SelectedIndex = 0
			m_SelectedIndex = -1
		End Sub

		Public ReadOnly Property SelectedUser() As DbRecord
			Get
				If m_Users.Count = 0 OrElse m_SelectedIndex = -1 Then
					Return Nothing
				End If
				Return CType(m_Users(m_SelectedIndex), DbRecord)
			End Get
		End Property

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
		Private Sub InitializeComponent()
			Me.label1 = New System.Windows.Forms.Label()
			Me.txtDatabaseDir = New System.Windows.Forms.TextBox()
			Me.lstUsers = New System.Windows.Forms.ListBox()
			Me.btnSelect = New System.Windows.Forms.Button()
			Me.SuspendLayout()
			' 
			' label1
			' 
			Me.label1.Location = New System.Drawing.Point(8, 8)
			Me.label1.Name = "label1"
			Me.label1.Size = New System.Drawing.Size(96, 16)
			Me.label1.TabIndex = 0
			Me.label1.Text = "Database folder: "
			' 
			' txtDatabaseDir
			' 
			Me.txtDatabaseDir.Location = New System.Drawing.Point(104, 6)
			Me.txtDatabaseDir.Name = "txtDatabaseDir"
			Me.txtDatabaseDir.ReadOnly = True
			Me.txtDatabaseDir.Size = New System.Drawing.Size(424, 20)
			Me.txtDatabaseDir.TabIndex = 1
			Me.txtDatabaseDir.TabStop = False
			Me.txtDatabaseDir.Text = ""
			' 
			' lstUsers
			' 
			Me.lstUsers.Location = New System.Drawing.Point(8, 32)
			Me.lstUsers.Name = "lstUsers"
			Me.lstUsers.Size = New System.Drawing.Size(520, 160)
			Me.lstUsers.TabIndex = 2
			' 
			' btnSelect
			' 
			Me.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK
			Me.btnSelect.Location = New System.Drawing.Point(231, 208)
			Me.btnSelect.Name = "btnSelect"
			Me.btnSelect.TabIndex = 3
			Me.btnSelect.Text = "Select"
'			Me.btnSelect.Click += New System.EventHandler(Me.btnSelect_Click);
			' 
			' SelectUser
			' 
			Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
			Me.ClientSize = New System.Drawing.Size(537, 241)
			Me.ControlBox = False
			Me.Controls.Add(Me.btnSelect)
			Me.Controls.Add(Me.lstUsers)
			Me.Controls.Add(Me.txtDatabaseDir)
			Me.Controls.Add(Me.label1)
			Me.Name = "SelectUser"
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
			Me.Text = "Select User"
			Me.ResumeLayout(False)

		End Sub
		#End Region

		Private Sub btnSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelect.Click
			m_SelectedIndex = lstUsers.SelectedIndex
		End Sub
	End Class
End Namespace
