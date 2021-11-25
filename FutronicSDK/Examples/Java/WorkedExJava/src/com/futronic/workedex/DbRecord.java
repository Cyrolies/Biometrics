/*
 * DbRecord.java
 */

package com.futronic.workedex;

import java.io.*;
import java.nio.*;
import java.nio.charset.*;
import java.util.Vector;
import com.futronic.SDKHelper.*;

/**
 * This class represent a user fingerprint database record.
 *
 * @author Shustikov
 */
public class DbRecord
{
    /**
     * User name
     */
    private String m_UserName;

    /**
     * User unique key
     */
    private byte[] m_Key;

    /**
     * Finger template.
     */
    private byte[] m_Template;
    
    /**
     * Creates a new instance of DbRecord class.
     */
    public DbRecord()
    {
        m_UserName = "";
        // Generate user's unique identifier
        m_Key = new byte[16];
        java.util.UUID guid = java.util.UUID.randomUUID();
        long itemHigh = guid.getMostSignificantBits();
        long itemLow = guid.getLeastSignificantBits();
        for( int i = 7; i >= 0; i-- )
        {
            m_Key[i]   = (byte)(itemHigh & 0xFF);
            itemHigh >>>= 8;
            m_Key[8+i] = (byte)(itemLow & 0xFF);
            itemLow >>>= 8;
        }
        m_Template = null;
    }

    /**
     * Initialize a new instance of DbRecord class from the file.
     * 
     * @param szFileName a file name with previous saved passport.
     */
    public DbRecord( String szFileName )
        throws FileNotFoundException, NullPointerException, AppException
    {
        Load( szFileName );
    }

    /**
     * Load user's information from file. 
     *
     * @param szFileName a file name with previous saved passport.
     *
     * @exception NullPointerException szFileName parameter has null reference.
     * @exception InvalidObjectException the file has invalid structure.
     * @exception FileNotFoundException the file not found or access denied.
     */
    private void Load( String szFileName )
        throws FileNotFoundException, NullPointerException, AppException
    {
        FileInputStream fs = null;
        File f = null;
        long nFileSize;
        
        f = new File( szFileName );
        if( !f.exists() || !f.canRead() )
            throw new FileNotFoundException( "File " + f.getPath() );

        try
        {
            nFileSize = f.length();
            fs = new FileInputStream( f );

            CharsetDecoder utf8Decoder = Charset.forName( "UTF-8" ).newDecoder();
            byte[] Data = null;

            // Read user name length and user name in UTF8
            if( nFileSize < 2 )
                throw new AppException( "Bad file " + f.getPath() );
            int nLength = (fs.read() << 8) | fs.read();
            nFileSize -= 2;
            if( nFileSize < nLength )
                throw new AppException( "Bad file " + f.getPath() );
            nFileSize -= nLength;
            Data = new byte[nLength];
            fs.read( Data );
            m_UserName = utf8Decoder.decode( ByteBuffer.wrap( Data ) ).toString();

            // Read user unique ID
            if( nFileSize < 16 )
                throw new AppException( "Bad file " + f.getPath() );
            nFileSize -= 16;
            m_Key = new byte[16];
            fs.read( m_Key );

            // Read template length and template data
            if( nFileSize < 2 )
                throw new AppException( "Bad file " + f.getPath() );
            nLength = (fs.read() << 8) | fs.read();
            nFileSize -= 2;
            if( nFileSize != nLength )
                throw new AppException( "Bad file " + f.getPath() );
            m_Template = new byte[nLength];
            fs.read( m_Template );
            fs.close();
        }
        catch( SecurityException e )
        {
            if( f == null )
                throw new AppException( "Denies read access to the file " + f.getPath() );
            else {
                throw new AppException( "Denies read access to the file " + szFileName );
            }
        }
        catch( IOException e)
        {
            if( f == null )
                throw new AppException( "Bad file " + f.getPath() );
            else
                throw new AppException( "Bad file " + szFileName );
        }
    }

    /**
     * Save user's information to file. 
     *
     * @param szFileName a file name to save.
     *
     * @return true if passport successfully saved to file, otherwise false.
     *
     * @exception NullPointerException szFileName parameter has null reference.
     * @exception IllegalStateException some parameters are not set.
     * @exception IOException can not create file or can not write data into file.
     */
    public boolean Save(String szFileName )
        throws NullPointerException, IllegalStateException, IOException
    {
        FileOutputStream fs = null;
        File f = null;
        boolean bRetcode = false;
        boolean bExist;

        if( m_Template == null || m_UserName == null || m_UserName.length() == 0 )
            throw new IllegalStateException();

        try
        {
            f = new File( szFileName );
            fs = new FileOutputStream( f );

            CharsetEncoder utf8Encoder = Charset.forName( "UTF-8" ).newEncoder();
            byte[] Data = null;

            // Save user name
            ByteBuffer bBuffer = utf8Encoder.encode( CharBuffer.wrap( m_UserName.toCharArray() ) );
            Data = new byte[ bBuffer.limit()];
            bBuffer.get( Data );
            fs.write( ((Data.length >>> 8) & 0xFF) );
            fs.write( (Data.length & 0xFF) );
            fs.write(Data);

            // Save user unique ID
            fs.write( m_Key );

            // Save user template
            fs.write( ((m_Template.length >>> 8) & 0xFF) );
            fs.write( (m_Template.length & 0xFF) );
            fs.write( m_Template );
            fs.close();
            bRetcode = true;
        }
        finally
        {
            if( !bRetcode && f != null )
                f.delete();
        }

        return bRetcode;
    }

    /**
     * Get the user name.
     */
    public String getUserName()
    {
        return m_UserName;
    }

    /**
     * Set the user name.
     */
    public void setUserName( String value )
    {
        m_UserName = value;
    }

    /**
     * Get the user template.
     */
    public byte[] getTemplate()
    {
        return m_Template;
    }

    /**
     * Set the user template.
     */
    public void setTemplate( byte[] value)
    {
        m_Template = value;
    }

    /**
     * Get the user unique identifier.
     */
    public byte[] getUniqueID()
    {
        return m_Key;
    }
    
    public FtrIdentifyRecord getFtrIdentifyRecord()
    {
        FtrIdentifyRecord r = new FtrIdentifyRecord();
        r.m_KeyValue = m_Key;
        r.m_Template = m_Template;
        
        return r;
    }
    
    /**
     * Function read all records from database.
     *
     * @param szDbDir database folder
     *
     * @return reference to Vector objects with records
     */
    static Vector< DbRecord > ReadRecords( String szDbDir )
    {
        File DbDir;
        File[] files;
        Vector<DbRecord> Users = new Vector<DbRecord>( 10, 10 );

        // Read all records to identify
        DbDir = new File( szDbDir );
        files = DbDir.listFiles();

        if( (files == null) || (files.length == 0) )
        {
            return Users;
        }

        for( int iFiles = 0; iFiles < files.length; iFiles++)
        {
            try
            {
                if( files[iFiles].isFile() )
                {
                    DbRecord User = new DbRecord( files[iFiles].getAbsolutePath() );
                    Users.add( User );
                }
            }
            catch( FileNotFoundException e )
            {
                // The record has invalid data. Skip it and continue processing.
            }
            catch( NullPointerException e )
            {
                // The record has invalid data. Skip it and continue processing.
            }
            catch( AppException e )
            {
                // The record has invalid data or access denied. Skip it and continue processing.
            }
        }
       
        return Users;
    }
}
