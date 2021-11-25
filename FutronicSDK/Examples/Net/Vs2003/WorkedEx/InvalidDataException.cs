using System;

namespace Futronic.SDK.WorkedEx
{
	/// <summary>
	/// The exception that is thrown when a data stream is in an invalid format.
	/// </summary>
	public class InvalidDataException : SystemException
	{
		/// <summary>
		/// Initializes a new instance of the InvalidDataException class.
		/// </summary>
		public InvalidDataException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the InvalidDataException class with 
		/// a specified error message. 
		/// </summary>
		/// <param name="message">
		/// The error message that explains the reason for the exception.
		/// </param>
		public InvalidDataException( String message )
			: base( message )
		{
		}
	}
}
