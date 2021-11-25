Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Text
Imports System.IO
Imports Futronic.SDKHelper

Namespace Futronic.SDK.WorkedEx
	''' <summary>
	''' This class represent a user fingerprint database record.
	''' </summary>
	Public Class DbRecord
		''' <summary>
		''' Initialize a new instance of the DbRecord class.
		''' </summary>
		Public Sub New()
			m_UserName = String.Empty
			' Generate user's unique identifier
			m_Key = Guid.NewGuid().ToByteArray()
			m_Template = Nothing
		End Sub

		''' <summary>
		''' Initialize a new instance of the DbRecord class from the file.
		''' </summary>
		''' <param name="szFileName">
		''' A file name with previous saved user's information.
		''' </param>
		Public Sub New(ByVal szFileName As String)
			If szFileName Is Nothing Then
				Throw New ArgumentNullException("szFileName")
			End If
			Load(szFileName)
		End Sub

		''' <summary>
		''' Load user's information from file.
		''' </summary>
		''' <remarks>
		''' The function can throw standard exceptions. It occurs during file operations.
		''' </remarks>
		''' <param name="szFileName">
		''' A file name with previous saved user's information.
		''' </param>
		''' <exception cref="InvalidDataException">
		''' The file has invalid structure.
		''' </exception>
		Private Sub Load(ByVal szFileName As String)
            Dim fileStream As fileStream = New fileStream(szFileName, FileMode.Open)
			Try
				Dim utfEncoder As UTF8Encoding = New UTF8Encoding()
				Dim Data As Byte() = Nothing

				' Read user name length and user name in UTF8
				If fileStream.Length < 2 Then
					Throw New InvalidDataException(String.Format("Bad file {0}", fileStream.Name))
				End If
				Dim nLength As Integer = (fileStream.ReadByte() << 8) Or fileStream.ReadByte()
				Data = New Byte(nLength - 1){}
				If nLength <> fileStream.Read(Data, 0, nLength) Then
					Throw New InvalidDataException(String.Format("Bad file {0}", fileStream.Name))
				End If
				m_UserName = utfEncoder.GetString(Data)

				' Read user unique ID
				m_Key = New Byte(15){}
				If fileStream.Read(m_Key, 0, 16) <> 16 Then
					Throw New InvalidDataException(String.Format("Bad file {0}", fileStream.Name))
				End If

				' Read template length and template data
				If (fileStream.Length - fileStream.Position) < 2 Then
					Throw New InvalidDataException(String.Format("Bad file {0}", fileStream.Name))
				End If
				nLength = (fileStream.ReadByte() << 8) Or fileStream.ReadByte()
				m_Template = New Byte(nLength - 1){}
				If fileStream.Read(m_Template, 0, nLength) <> nLength Then
					Throw New InvalidDataException(String.Format("Bad file {0}", fileStream.Name))
				End If
			Finally
				CType(fileStream, IDisposable).Dispose()
			End Try
        End Sub

        ''' <summary>
        ''' Save user's information to file
        ''' </summary>
        ''' <param name="szFileName">
        ''' File name to save.
        ''' </param>
        ''' <exception cref="InvalidOperationException">
        ''' Some parameters are not set.
        ''' </exception>
        ''' <returns>true if user's information successfully saved to file, otherwise false.</returns>
        Public Function Save(ByVal szFileName As String) As Boolean
            If m_Template Is Nothing OrElse m_UserName = String.Empty Then
                Throw New InvalidOperationException
            End If
            Dim fileStream As fileStream = New fileStream(szFileName, FileMode.Create)
            Try
                Dim utfEncoder As UTF8Encoding = New UTF8Encoding
                Dim Data As Byte() = Nothing

                ' Save user name
                Data = utfEncoder.GetBytes(m_UserName)
                fileStream.WriteByte(CByte((Data.Length >> 8) And &HFF))
                fileStream.WriteByte(CByte(Data.Length And &HFF))
                fileStream.Write(Data, 0, Data.Length)

                ' Save user unique ID
                fileStream.Write(m_Key, 0, m_Key.Length)

                ' Save user template
                fileStream.WriteByte(CByte((m_Template.Length >> 8) And &HFF))
                fileStream.WriteByte(CByte(m_Template.Length And &HFF))
                fileStream.Write(m_Template, 0, m_Template.Length)
            Finally
                CType(fileStream, IDisposable).Dispose()
            End Try

            Return True
        End Function

        Public Function GetRecord() As FtrIdentifyRecord
            Dim item As FtrIdentifyRecord
            item.KeyValue = m_Key
            item.Template = m_Template

            Return item
        End Function

        ''' <summary>
        ''' Get or set the user name.
        ''' </summary>
        Public Property UserName() As String
            Get
                Return m_UserName
            End Get

            Set(ByVal Value As String)
                m_UserName = Value
            End Set
        End Property

        ''' <summary>
        ''' Get or set the user template.
        ''' </summary>
        Public Property Template() As Byte()
            Get
                Return m_Template
            End Get

            Set(ByVal Value As Byte())
                m_Template = Value
            End Set
        End Property

        ''' <summary>
        ''' Get the user unique identifier.
        ''' </summary>
        Public ReadOnly Property UniqueID() As Byte()
            Get
                Return m_Key
            End Get
        End Property

        ''' <summary>
        ''' Function read all records from database.
        ''' </summary>
        ''' <param name="szDbDir">database folder</param>
        ''' <returns>
        ''' reference to List objects with records
        ''' </returns>
        Public Shared Function ReadRecords(ByVal szDbDir As String) As ArrayList
            Dim Users As ArrayList = New ArrayList(10)

            If (Not Directory.Exists(szDbDir)) Then
                Throw New DirectoryNotFoundException(String.Format("The folder {0} is not found", szDbDir))
            End If
            Dim rgFiles As String() = Directory.GetFiles(szDbDir, "*")
            If rgFiles Is Nothing OrElse rgFiles.Length = 0 Then
                Return Users
            End If

            Dim iFiles As Integer = 0
            Do While iFiles < rgFiles.Length
                Try
                    Dim User As DbRecord = New DbRecord(rgFiles(iFiles))
                    Users.Add(User)
                Catch e1 As InvalidDataException
                    ' The user's information has invalid data. Skip it and continue processing.
                End Try
                iFiles += 1
            Loop

            Return Users
        End Function

        ''' <summary>
        ''' User name
        ''' </summary>
        Private m_UserName As String

        ''' <summary>
        ''' User unique key
        ''' </summary>
        Private m_Key As Byte()

        ''' <summary>
        ''' Finger template.
        ''' </summary>
        Private m_Template As Byte()
    End Class
End Namespace
