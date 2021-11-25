Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms

Namespace Futronic.SDK.WorkedEx
	''' <summary>
	''' Summary description for EnrollmentName.
	''' </summary>
	Public Class EnrollmentName
		Inherits System.Windows.Forms.Form
		Private label1 As System.Windows.Forms.Label
		Private txtUserName As System.Windows.Forms.TextBox
		Private btnOK As System.Windows.Forms.Button
		Private btnCancel As System.Windows.Forms.Button
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.Container = Nothing

		Public Sub New()
			'
			' Required for Windows Form Designer support
			'
			InitializeComponent()

		End Sub

		Public ReadOnly Property UserName() As String
			Get
				Return txtUserName.Text
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
			Me.txtUserName = New System.Windows.Forms.TextBox()
			Me.btnOK = New System.Windows.Forms.Button()
			Me.btnCancel = New System.Windows.Forms.Button()
			Me.SuspendLayout()
			' 
			' label1
			' 
			Me.label1.Location = New System.Drawing.Point(8, 40)
			Me.label1.Name = "label1"
			Me.label1.Size = New System.Drawing.Size(64, 16)
			Me.label1.TabIndex = 0
			Me.label1.Text = "User name"
			' 
			' txtUserName
			' 
			Me.txtUserName.Location = New System.Drawing.Point(72, 38)
			Me.txtUserName.Name = "txtUserName"
			Me.txtUserName.Size = New System.Drawing.Size(232, 20)
			Me.txtUserName.TabIndex = 1
			Me.txtUserName.Text = ""
			' 
			' btnOK
			' 
			Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
			Me.btnOK.Location = New System.Drawing.Point(336, 16)
			Me.btnOK.Name = "btnOK"
			Me.btnOK.TabIndex = 2
			Me.btnOK.Text = "Ok"
			' 
			' btnCancel
			' 
			Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
			Me.btnCancel.Location = New System.Drawing.Point(336, 48)
			Me.btnCancel.Name = "btnCancel"
			Me.btnCancel.TabIndex = 3
			Me.btnCancel.Text = "Cancel"
			' 
			' EnrollmentName
			' 
			Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
			Me.ClientSize = New System.Drawing.Size(422, 96)
			Me.ControlBox = False
			Me.Controls.Add(Me.btnCancel)
			Me.Controls.Add(Me.btnOK)
			Me.Controls.Add(Me.txtUserName)
			Me.Controls.Add(Me.label1)
			Me.Name = "EnrollmentName"
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
			Me.Text = "User name"
			Me.ResumeLayout(False)

		End Sub
		#End Region
	End Class
End Namespace
