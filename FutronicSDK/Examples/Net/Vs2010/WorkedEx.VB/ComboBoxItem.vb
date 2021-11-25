Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Futronic.SDK.WorkedEx
	Friend Class ComboBoxItem
		Public Sub New(ByVal message As String, ByVal tag As Object)
			m_Message = message
			m_Tag = tag
		End Sub

		Public ReadOnly Property Message() As String
			Get
				Return m_Message
			End Get
		End Property

		Public ReadOnly Property Tag() As Object
			Get
				Return m_Tag
			End Get
		End Property

		Public Overrides Function ToString() As String
			Return m_Message
		End Function

		Protected m_Message As String
		Protected m_Tag As Object
	End Class
End Namespace
