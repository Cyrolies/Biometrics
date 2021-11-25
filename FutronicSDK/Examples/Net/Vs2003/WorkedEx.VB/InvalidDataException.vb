Imports Microsoft.VisualBasic
Imports System

Namespace Futronic.SDK.WorkedEx
	''' <summary>
	''' The exception that is thrown when a data stream is in an invalid format.
	''' </summary>
	Public Class InvalidDataException
		Inherits SystemException
		''' <summary>
		''' Initializes a new instance of the InvalidDataException class.
		''' </summary>
		Public Sub New()
		End Sub

		''' <summary>
		''' Initializes a new instance of the InvalidDataException class with 
		''' a specified error message. 
		''' </summary>
		''' <param name="message">
		''' The error message that explains the reason for the exception.
		''' </param>
		Public Sub New(ByVal message As String)
			MyBase.New(message)
		End Sub
	End Class
End Namespace
