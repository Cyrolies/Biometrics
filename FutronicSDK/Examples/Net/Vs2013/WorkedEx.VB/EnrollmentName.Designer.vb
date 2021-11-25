Imports Microsoft.VisualBasic
Imports System
Namespace Futronic.SDK.WorkedEx
	Public Partial Class EnrollmentName
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
			Me.label1 = New System.Windows.Forms.Label()
			Me.txtUserName = New System.Windows.Forms.TextBox()
			Me.btnOK = New System.Windows.Forms.Button()
			Me.btnCancel = New System.Windows.Forms.Button()
			Me.SuspendLayout()
			' 
			' label1
			' 
			Me.label1.AutoSize = True
			Me.label1.Location = New System.Drawing.Point(12, 42)
			Me.label1.Name = "label1"
			Me.label1.Size = New System.Drawing.Size(64, 13)
			Me.label1.TabIndex = 0
			Me.label1.Text = "User name: "
			' 
			' txtUserName
			' 
			Me.txtUserName.Location = New System.Drawing.Point(82, 38)
			Me.txtUserName.Name = "txtUserName"
			Me.txtUserName.Size = New System.Drawing.Size(248, 20)
			Me.txtUserName.TabIndex = 1
			' 
			' btnOK
			' 
			Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
			Me.btnOK.Location = New System.Drawing.Point(335, 12)
			Me.btnOK.Name = "btnOK"
			Me.btnOK.Size = New System.Drawing.Size(75, 23)
			Me.btnOK.TabIndex = 2
			Me.btnOK.Text = "Ok"
			Me.btnOK.UseVisualStyleBackColor = True
			' 
			' btnCancel
			' 
			Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
			Me.btnCancel.Location = New System.Drawing.Point(336, 42)
			Me.btnCancel.Name = "btnCancel"
			Me.btnCancel.Size = New System.Drawing.Size(75, 23)
			Me.btnCancel.TabIndex = 3
			Me.btnCancel.Text = "Cancel"
			Me.btnCancel.UseVisualStyleBackColor = True
			' 
			' EnrollmentName
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(422, 96)
			Me.ControlBox = False
			Me.Controls.Add(Me.btnCancel)
			Me.Controls.Add(Me.btnOK)
			Me.Controls.Add(Me.txtUserName)
			Me.Controls.Add(Me.label1)
			Me.MaximizeBox = False
			Me.MaximumSize = New System.Drawing.Size(430, 130)
			Me.MinimizeBox = False
			Me.MinimumSize = New System.Drawing.Size(430, 130)
			Me.Name = "EnrollmentName"
			Me.ShowIcon = False
			Me.ShowInTaskbar = False
			Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
			Me.Text = "User name"
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		#End Region

		Private label1 As System.Windows.Forms.Label
		Private txtUserName As System.Windows.Forms.TextBox
		Private btnOK As System.Windows.Forms.Button
		Private btnCancel As System.Windows.Forms.Button
	End Class
End Namespace