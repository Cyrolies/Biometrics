using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Futronic.SDKHelper;

namespace Service
{
    /// <summary>
    /// This class represent a user fingerprint database record.
    /// </summary>
    public class DbRecord
    {
        /// <summary>
        /// Initialize a new instance of DbRecord class.
        /// </summary>
        public DbRecord()
        {
            m_UserName = String.Empty;
            // Generate user's unique identifier
            m_Key = Guid.NewGuid().ToByteArray();
            m_Template = null;
        }

        /// <summary>
        /// Initialize a new instance of DbRecord class from the file.
        /// </summary>
        /// <param name="szFileName">
        /// A file name with previous saved user's information.
        /// </param>
        public DbRecord( String szFileName )
        {
            if (szFileName == null)
                throw new ArgumentNullException("szFileName");
            Load(szFileName);
        }

        /// <summary>
        /// Load user's information from file.
        /// </summary>
        /// <remarks>
        /// The function can throw standard exceptions. It occurs during file operations.
        /// </remarks>
        /// <param name="szFileName">
        /// A file name with previous saved user's information.
        /// </param>
        /// <exception cref="InvalidDataException">
        /// The file has invalid structure.
        /// </exception>
        private void Load(String szFileName)
        {
            using (FileStream fileStream = new FileStream(szFileName, FileMode.Open))
            {
                UTF8Encoding utfEncoder = new UTF8Encoding();
                byte[] Data = null;

                // Read user name length and user name in UTF8
                if( fileStream.Length < 2 )
                    throw new InvalidDataException(String.Format("Bad file {0}", fileStream.Name));
                int nLength = (fileStream.ReadByte() << 8) | fileStream.ReadByte();
                Data = new byte[nLength];
                if( nLength != fileStream.Read(Data, 0, nLength) )
                    throw new InvalidDataException(String.Format("Bad file {0}", fileStream.Name));
                m_UserName = utfEncoder.GetString(Data);

                // Read user unique ID
                m_Key = new byte[16];
                if( fileStream.Read(m_Key, 0, 16) != 16)
                    throw new InvalidDataException(String.Format("Bad file {0}", fileStream.Name));

                // Read template length and template data
                if( (fileStream.Length - fileStream.Position) < 2)
                    throw new InvalidDataException(String.Format("Bad file {0}", fileStream.Name));
                nLength = (fileStream.ReadByte() << 8) | fileStream.ReadByte();
                m_Template = new byte[nLength];
                if( fileStream.Read(m_Template, 0, nLength) != nLength )
                    throw new InvalidDataException(String.Format("Bad file {0}", fileStream.Name));
            }
        }

        /// <summary>
        /// Save user's information to file
        /// </summary>
        /// <param name="szFileName">
        /// File name to save.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Some parameters are not set.
        /// </exception>
        /// <returns>true if user's information successfully saved to file, otherwise false.</returns>
        public bool Save(String szFileName)
        {
            if (m_Template == null || m_UserName == String.Empty)
                throw new InvalidOperationException();
            using (FileStream fileStream = new FileStream(szFileName, FileMode.Create))
            {
                UTF8Encoding utfEncoder = new UTF8Encoding();
                byte[] Data = null;

                // Save user name
                Data = utfEncoder.GetBytes(m_UserName);
                fileStream.WriteByte( (byte)((Data.Length >> 8) & 0xFF) );
                fileStream.WriteByte( (byte)(Data.Length & 0xFF) );
                fileStream.Write(Data, 0, Data.Length);

                // Save user unique ID
                fileStream.Write(m_Key, 0, m_Key.Length);

                // Save user template
                fileStream.WriteByte((byte)((m_Template.Length >> 8) & 0xFF));
                fileStream.WriteByte((byte)(m_Template.Length & 0xFF));
                fileStream.Write(m_Template, 0, m_Template.Length);
            }

            return true;
        }

        public FtrIdentifyRecord GetRecord()
        {
            FtrIdentifyRecord item;
            item.KeyValue = m_Key;
            item.Template = m_Template;

            return item;
        }

        public int UserID { get; set; }
       
        /// <summary>
        /// Get or set the user name.
        /// </summary>
        public String UserName
        {
            get
            {
                return m_UserName;
            }

            set
            {
                m_UserName = value;
            }
        }

        /// <summary>
        /// Get or set the user template.
        /// </summary>
        public byte[] Template
        {
            get
            {
                return m_Template;
            }

            set
            {
                m_Template = value;
            }
        }

        /// <summary>
        /// Get the user unique identifier.
        /// </summary>
        public byte[] UniqueID
        {
            get
            {
                return m_Key;
            }
        }

        /// <summary>
        /// Function read all records from database.
        /// </summary>
        /// <param name="szDbDir">database folder</param>
        /// <returns>
        /// reference to List objects with records
        /// </returns>
        public static List<DbRecord> ReadRecords( String szDbDir )
        {
            List<DbRecord> Users = new List<DbRecord>(10);

            if( !Directory.Exists( szDbDir ) )
                throw new DirectoryNotFoundException(String.Format("The folder {0} is not found", szDbDir));
            string[] rgFiles = Directory.GetFiles(szDbDir, "*");
            if (rgFiles == null || rgFiles.Length == 0)
                return Users;

            for( int iFiles = 0; iFiles < rgFiles.Length; iFiles++ )
            {
                try
                {
                    DbRecord User = new DbRecord(rgFiles[iFiles]);
                    Users.Add( User );
                }
                catch (InvalidDataException)
                {
                    // The user's information has invalid data. Skip it and continue processing.
                }
            }

            return Users;
        }

        /// <summary>
        /// User name
        /// </summary>
        private String m_UserName;

        /// <summary>
        /// User unique key
        /// </summary>
        private byte[] m_Key;

        /// <summary>
        /// Finger template.
        /// </summary>
        private byte[] m_Template;
    }
}
