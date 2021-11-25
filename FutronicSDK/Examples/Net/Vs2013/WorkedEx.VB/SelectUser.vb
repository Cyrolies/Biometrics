Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.IO
Imports System.Windows.Forms

Namespace Futronic.SDK.WorkedEx
	Public Partial Class SelectUser
		Inherits Form
		Private m_Users As List(Of DbRecord)
		Private m_SelectedIndex As Integer

		Public Sub New(ByVal Users As List(Of DbRecord), ByVal szDbDir As String)
			InitializeComponent()
			txtDatabaseDir.Text = szDbDir
			m_Users = Users
			Dim i As Integer = 0
			Do While i < m_Users.Count
				lstUsers.Items.Add(m_Users(i).UserName)
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
				Return m_Users(m_SelectedIndex)
			End Get
		End Property

		Private Sub btnSelect_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSelect.Click
			m_SelectedIndex = lstUsers.SelectedIndex
		End Sub
	End Class
End Namespace