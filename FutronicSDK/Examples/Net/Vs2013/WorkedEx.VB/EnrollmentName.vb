Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

Namespace Futronic.SDK.WorkedEx
	Public Partial Class EnrollmentName
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Public ReadOnly Property UserName() As String
			Get
				Return txtUserName.Text
			End Get
		End Property
	End Class
End Namespace