Imports Microsoft.VisualBasic
Imports System
Namespace Futronic.SDK.WorkedEx
	Public Partial Class SelectUser
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
			Me.lstUsers = New System.Windows.Forms.ListBox()
			Me.btnSelect = New System.Windows.Forms.Button()
			Me.label1 = New System.Windows.Forms.Label()
			Me.txtDatabaseDir = New System.Windows.Forms.TextBox()
			Me.SuspendLayout()
			' 
			' lstUsers
			' 
			Me.lstUsers.FormattingEnabled = True
			Me.lstUsers.Location = New System.Drawing.Point(12, 38)
			Me.lstUsers.Name = "lstUsers"
			Me.lstUsers.Size = New System.Drawing.Size(513, 147)
			Me.lstUsers.TabIndex = 0
			' 
			' btnSelect
			' 
			Me.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK
			Me.btnSelect.Location = New System.Drawing.Point(231, 206)
			Me.btnSelect.Name = "btnSelect"
			Me.btnSelect.Size = New System.Drawing.Size(75, 23)
			Me.btnSelect.TabIndex = 1
			Me.btnSelect.Text = "Select"
			Me.btnSelect.UseVisualStyleBackColor = True
'			Me.btnSelect.Click += New System.EventHandler(Me.btnSelect_Click);
			' 
			' label1
			' 
			Me.label1.AutoSize = True
			Me.label1.Location = New System.Drawing.Point(12, 9)
			Me.label1.Name = "label1"
			Me.label1.Size = New System.Drawing.Size(88, 13)
			Me.label1.TabIndex = 2
			Me.label1.Text = "Database folder: "
			' 
			' txtDatabaseDir
			' 
			Me.txtDatabaseDir.Location = New System.Drawing.Point(106, 6)
			Me.txtDatabaseDir.Name = "txtDatabaseDir"
			Me.txtDatabaseDir.ReadOnly = True
			Me.txtDatabaseDir.Size = New System.Drawing.Size(419, 20)
			Me.txtDatabaseDir.TabIndex = 3
			Me.txtDatabaseDir.TabStop = False
			' 
			' SelectUser
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(537, 241)
			Me.ControlBox = False
			Me.Controls.Add(Me.txtDatabaseDir)
			Me.Controls.Add(Me.label1)
			Me.Controls.Add(Me.btnSelect)
			Me.Controls.Add(Me.lstUsers)
			Me.MaximizeBox = False
			Me.MinimizeBox = False
			Me.Name = "SelectUser"
			Me.ShowIcon = False
			Me.ShowInTaskbar = False
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
			Me.Text = "Select User"
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		#End Region

		Private lstUsers As System.Windows.Forms.ListBox
		Private WithEvents btnSelect As System.Windows.Forms.Button
		Private label1 As System.Windows.Forms.Label
		Private txtDatabaseDir As System.Windows.Forms.TextBox
	End Class
End Namespace