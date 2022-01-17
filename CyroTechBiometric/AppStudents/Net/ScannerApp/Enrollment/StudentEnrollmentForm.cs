using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Futronic.SDKHelper;
using Service;
using DALEFModel;
using System.Configuration;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;

namespace StudentEnrollment
{
    public partial class StudentEnrollmentForm : Form
    {
        const string kCompanyName = "CyroTech";
        const string kProductName = "SDK 4.0";
        const string kDbName = "ScanningData";
        const string kDbPath = "../Data";
        private List<DbRecord> Users;
        private SelectResult mealPreference;
        private string allergies;
        /// <summary>
        /// This delegate enables asynchronous calls for setting
        /// the text property on a status control.
        /// </summary>
        /// <param name="text"></param>
        delegate void SetTextCallback(string text);

        /// <summary>
        /// This delegate enables asynchronous calls for setting
        /// the text property on a identification limit control.
        /// </summary>
        /// <param name="text"></param>
        delegate void SetIdentificationLimitCallback(int limit);

        /// <summary>
        /// This delegate enables asynchronous calls for setting
        /// the Image property on a PictureBox control.
        /// </summary>
        /// <param name="hBitmap">the instance of Bitmap class</param>
        delegate void SetImageCallback(Bitmap hBitmap );

        /// <summary>
        /// This delegate enables asynchronous calls for setting
        /// the Enable property on a buttons.
        /// </summary>
        /// <param name="bEnable">true to enable buttons, otherwise to disable</param>
        delegate void EnableControlsCallback(bool bEnable );

        /// <summary>
        /// Contain reference for current operation object
        /// </summary>
        private FutronicSdkBase m_Operation;

        private bool m_bExit;

        /// <summary>
        /// The type of this parameter is depending from current operation. For
        /// enrollment operation this is DbRecord.
        /// </summary>
        private Object m_OperationObj;

        /// <summary>
        /// A directory name to write user's information.
        /// </summary>
        private String m_DatabaseDir;

        private bool m_bInitializationSuccess;

        static private ComboBoxItem[] rgVersionItems = new ComboBoxItem[]
            {
                new ComboBoxItem( "SDK 3.0", VersionCompatible.ftr_version_previous ),
                new ComboBoxItem( "SDK 3.5", VersionCompatible.ftr_version_current ),
                new ComboBoxItem( "Both", VersionCompatible.ftr_version_compatible )
            };

        public StudentEnrollmentForm()
        {
            try
            {
                InitializeComponent();
           
                m_DatabaseDir = GetDatabaseDir();
           

            LoadStudentBiometricList();
            m_bInitializationSuccess = false;
            // Create FutronicEnrollment object for retrieve default values only
            FutronicEnrollment dummy = new FutronicEnrollment();
            cbFARNLevel.SelectedIndex = (int)dummy.FARnLevel;
            cbMaxFrames.SelectedItem = dummy.MaxModels.ToString();
            chDetectFakeFinger.Checked = dummy.FakeDetection;
            cbMIOTOff.Checked = dummy.MIOTControlOff;
            chFastMode.Checked = dummy.FastMode;
            SetIdentificationLimit(dummy.IdentificationsLeft);
          //  btnStop.Enabled = false;
            m_bExit = false;
            int selectedIndex = 0, itemIndex;
            foreach( ComboBoxItem item in rgVersionItems )
            {
                itemIndex = m_cmbVersion.Items.Add(item);
                if ((VersionCompatible)item.Tag == dummy.Version)
                {
                    selectedIndex = itemIndex;
                }
            }
            m_cmbVersion.SelectedIndex = selectedIndex;
			try
			{
                PopulateStudentList();
            }
            catch(Exception ex)
			{
                MessageBox.Show(this, ex.Message,
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            
            m_bInitializationSuccess = true;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(this, "Initialization failed. Application will be close.\nCan not create database folder. Access denied.",
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            catch (IOException)
            {
                MessageBox.Show(this, "Initialization failed. Application will be close.\nCan not create database folder",
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Initialization failed",
                    ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void SetIdentificationLimit(int nLimit)
        {
            try
            {
                if (this.m_lblIdentificationsLimit.InvokeRequired)
                {
                    SetIdentificationLimitCallback d = new SetIdentificationLimitCallback(this.SetIdentificationLimit);
                    this.Invoke(d, new object[] { nLimit });
                }
                else
                {
                    if (nLimit == Int32.MaxValue)
                    {
                        m_lblIdentificationsLimit.Text = "Identification limit: No limits";
                    }
                    else
                    {
                        m_lblIdentificationsLimit.Text = String.Format("Identification limit: {0}", nLimit);
                    }
                }
            }catch(Exception ex)
			{
                throw ex;
			}
        }

        private void btnEnroll_Click(object sender, EventArgs e)
        {
            DbRecord User = new DbRecord();

            
			if (cmbStudents.SelectedItem == null)
            {
                MessageBox.Show( this, "You must select a student", this.Text, 
                                 MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                return;
            }
            SelectResult selItem = (SelectResult)cmbStudents.SelectedItem;
            // Get meal preferences
            SelectMealPreferences frmMealPref = new SelectMealPreferences(selItem.id);
            
            frmMealPref.ShowDialog(this);
            if (frmMealPref.DialogResult != DialogResult.OK)
            {
                return;
            }
            mealPreference = frmMealPref.selectedMealPreference;
            allergies = frmMealPref.allergies;
            // Try create the file for user's information
           
            if(isUserExists(selItem.Description) )
            {
                DialogResult nResponse;
                nResponse = MessageBox.Show("Student's biometric already exists. Would you like to update it?",
                                            "",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (nResponse == DialogResult.No)
                    return;
            } else {
                try
                {
                    CreateFile(selItem.Description);
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show(this, "Can not create file to save an student's information.", this.Text,
                                     MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                catch (IOException )
                {
                    MessageBox.Show(this, String.Format("Bad student name '{0}'.", selItem.Description),
                                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            User.UserName = selItem.Description+"-"+selItem.id;
            
            try
            {
                m_OperationObj = User;
                FutronicSdkBase dummy = new FutronicEnrollment();
                if (m_Operation != null)
                {
                    m_Operation.Dispose();
                    m_Operation = null;
                }
                m_Operation = dummy;

                // Set control properties
                m_Operation.FakeDetection = chDetectFakeFinger.Checked;
                m_Operation.FFDControl = true;
                m_Operation.FARN = Int32.Parse(tbFARN.Text);
                m_Operation.Version = (VersionCompatible)((ComboBoxItem)m_cmbVersion.SelectedItem).Tag;
                m_Operation.FastMode = chFastMode.Checked;
                ((FutronicEnrollment)m_Operation).MIOTControlOff = cbMIOTOff.Checked;
                ((FutronicEnrollment)m_Operation).MaxModels = Int32.Parse((String)cbMaxFrames.SelectedItem);

                EnableControls(false);

                // register events
                m_Operation.OnPutOn += new OnPutOnHandler(this.OnPutOn);
                m_Operation.OnTakeOff += new OnTakeOffHandler(this.OnTakeOff);
                m_Operation.UpdateScreenImage += new UpdateScreenImageHandler(this.UpdateScreenImage);
                m_Operation.OnFakeSource += new OnFakeSourceHandler(this.OnFakeSource);
                ((FutronicEnrollment)m_Operation).OnEnrollmentComplete += new OnEnrollmentCompleteHandler(this.OnEnrollmentComplete);

                // start enrollment process
                ((FutronicEnrollment)m_Operation).Enrollment();
            }
            catch(Exception ex)
			{
                throw ex;
			}
        }

   //     private void btnVerify_Click(object sender, EventArgs e)
   //     {
   //         EnableControls(false);
   //         SetStatusText("Programme is loading database, please wait ...");
   //         List<DbRecord> Users = DbRecord.ReadRecords(m_DatabaseDir);
   //         SetStatusText( String.Empty );
   //         if (Users.Count == 0)
   //         {
   //             EnableControls(true);
   //             MessageBox.Show(this, "Student not found. Please update students list or enrollment student", 
   //                 this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
   //             return;
   //         }

			//// select user's information for verification
			//SelectUser frmSelectUser = new SelectUser(Users, m_DatabaseDir);
			//frmSelectUser.ShowDialog(this);

			//if ( frmSelectUser.SelectedUser == null )
   //         {
   //             EnableControls(true);
   //             MessageBox.Show(this, "No selected user", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
   //             return;
   //         }
   //         m_OperationObj = frmSelectUser.SelectedUser;

   //         FutronicSdkBase dummy = new FutronicVerification(((DbRecord)m_OperationObj).Template);
   //         if (m_Operation != null)
   //         {
   //             m_Operation.Dispose();
   //             m_Operation = null;
   //         }
   //         m_Operation = dummy;

   //         // Set control properties
   //         m_Operation.FakeDetection = chDetectFakeFinger.Checked;
   //         m_Operation.FFDControl = true;
   //         m_Operation.FARN = Int32.Parse(tbFARN.Text);
   //         m_Operation.Version = (VersionCompatible)((ComboBoxItem)m_cmbVersion.SelectedItem).Tag;
   //         m_Operation.FastMode = chFastMode.Checked;

   //         // register events
   //         m_Operation.OnPutOn += new OnPutOnHandler(this.OnPutOn);
   //         m_Operation.OnTakeOff += new OnTakeOffHandler(this.OnTakeOff);
   //         m_Operation.UpdateScreenImage += new UpdateScreenImageHandler(this.UpdateScreenImage);
   //         m_Operation.OnFakeSource += new OnFakeSourceHandler(this.OnFakeSource);
   //         ((FutronicVerification)m_Operation).OnVerificationComplete += new OnVerificationCompleteHandler(this.OnVerificationComplete);

   //         // start verification process
   //         ((FutronicVerification)m_Operation).Verification();
   //     }
        private void LoadStudentBiometricList()
        {
            APIProxy proxy = new APIProxy();
            string res = string.Empty;
            List<StudentBiometric> list = proxy.GetStudentBiometricList(5, 139,out res);
            if(list != null)
			{
                Users = new List<DbRecord>();
                foreach (StudentBiometric pb in list)
				{
                    if (pb.Biometric != null)
                    {
                        
                        DbRecord dbrecord = new DbRecord();
                        dbrecord.Template  = pb.Biometric.Code;
                        dbrecord.UserName = pb.Student.Firstname + " " + pb.Student.Surname + " " + pb.Student.AdmissionNo + "-" +pb.Student.StudentID.ToString();

                        CreateFile(dbrecord.UserName);
                        String szFileName = Path.Combine(m_DatabaseDir, dbrecord.UserName);
                        dbrecord.Save(szFileName);
                        Users.Add(dbrecord);
                    }
                }
			}
        }
       
        private void btnIdentify_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            SetStatusText("Programme is loading database, please wait ...");
            
            SetStatusText(String.Empty);
            if (Users.Count == 0)
            {
                MessageBox.Show(this, "No students found", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                EnableControls(true);
                return;
            }
            m_OperationObj = Users;
            FutronicSdkBase dummy = new FutronicIdentification();
            if (m_Operation != null)
            {
                m_Operation.Dispose();
                m_Operation = null;
            }
            m_Operation = dummy;

            // Set control property
            m_Operation.FakeDetection = chDetectFakeFinger.Checked;
            m_Operation.FFDControl = true;
            m_Operation.FARN = Int32.Parse(tbFARN.Text);
            m_Operation.Version = (VersionCompatible)((ComboBoxItem)m_cmbVersion.SelectedItem).Tag;
            m_Operation.FastMode = chFastMode.Checked;

            // register events
            m_Operation.OnPutOn += new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff += new OnTakeOffHandler(this.OnTakeOff);
            m_Operation.UpdateScreenImage += new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource += new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicIdentification)m_Operation).OnGetBaseTemplateComplete += 
                    new OnGetBaseTemplateCompleteHandler(this.OnGetBaseTemplateComplete );

            // start identification process
            ((FutronicIdentification)m_Operation).GetBaseTemplate();
        }

        private void cbFARNLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if( cbFARNLevel.SelectedIndex == 6 )
            {
                tbFARN.ReadOnly = false;
            } else {
                tbFARN.Text = FutronicSdkBase.rgFARN[cbFARNLevel.SelectedIndex].ToString();
                tbFARN.ReadOnly = true;
            }
        }

        private void OnPutOn(FTR_PROGRESS Progress)
        {
            this.SetStatusText("Put finger onto device, please ...");
        }

        private void OnTakeOff(FTR_PROGRESS Progress)
        {
            this.SetStatusText("Take off finger from device, please ...");
        }

        private void UpdateScreenImage(Bitmap hBitmap)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if( PictureFingerPrint.InvokeRequired )
            {
                SetImageCallback d = new SetImageCallback(this.UpdateScreenImage);
                this.Invoke(d, new object[] { hBitmap });
            }
            else
            {
                PictureFingerPrint.Image = hBitmap;
            }
        }

        private bool OnFakeSource(FTR_PROGRESS Progress)
        {
            if( m_bExit )
                return true;

            DialogResult result;
            result = MessageBox.Show("Fake source detected. Do you want to continue process?", 
                                     "",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question );
            return (result == DialogResult.No);
        }

        private void OnEnrollmentComplete(bool bSuccess, int nRetCode )
        {
            StringBuilder szMessage = new StringBuilder();
            if (bSuccess)
            {
                // set status string
                szMessage.Append("Enrollment process finished successfully.");
                szMessage.Append("Quality: ");
                szMessage.Append(((FutronicEnrollment)m_Operation).Quality.ToString() );
                this.SetStatusText(szMessage.ToString());

                // Set template into user's information and save it
                DbRecord User = (DbRecord)m_OperationObj;
                User.Template = ((FutronicEnrollment)m_Operation).Template;
                //Save to Studentdatabase
                string res = string.Empty;
                APIProxy proxy = new APIProxy();
                StudentBiometric studentBiometric = null;
                int studentID = Convert.ToInt32(User.UserName.Substring(User.UserName.IndexOf("-")+1));
                try
                {
                    studentBiometric = proxy.GetStudentBiometric(studentID, 139, out res);
                    if(res != "Success")
					{
                        MessageBox.Show("Error " + res + " "  + User.UserName.Substring(0, User.UserName.IndexOf("-")),
                                  "",
                                  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                catch(Exception ex)
				{
                    MessageBox.Show("Can not save student's information to database ! Please try again for " + User.UserName.Substring(0,User.UserName.IndexOf("-")),
                                   "",
                                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    //TODO write to log
                }
                //Check if a biometric entry already exists
                if (studentBiometric == null)
                {
                    //Create new biometric entry
                    Biometric biometric = new Biometric() { Code = User.Template, StpBiometricTypeID = 139, OrgID = 5, IsActive = true, CreateDateTime = DateTime.Now };
                    Student student = proxy.GetStudent(studentID, out res);
                    if (student != null)
                    {
                        student.StpMealTypeID = mealPreference.id;
                        student.Allergies = allergies;
                    }
                    StudentBiometric pb = new StudentBiometric() { StudentID  = studentID, Biometric = biometric };
                 
                    if (!proxy.EditStudentBiometric(pb, out res))
                    {
                        MessageBox.Show("Can not save student biometric's information to database ! Please try again for " + User.UserName.Substring(0, User.UserName.IndexOf("-")),
                                        "",
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    }
                    else if (!proxy.EditStudent(student, out res))
                    {
                        MessageBox.Show("Can not save student's meal preferences to database ! Please try again for " + User.UserName.Substring(0, User.UserName.IndexOf("-")),
                                        "",
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    }
                    else
                    {
                        String szFileName = Path.Combine(m_DatabaseDir, User.UserName);
                        if (!User.Save(szFileName))
                        {
                            MessageBox.Show("Can not save student's information to file ! Please try again " + szFileName,
                                             "",
                                             MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
                else
                {
                    //Update student meal preferences
                    Student student = proxy.GetStudent(studentID, out res);
                    if (student != null)
                    {
                        student.StpMealTypeID = mealPreference.id;
                        student.Allergies = allergies;
                    }
                    //Upate existing biometric entry
                    Biometric biometric = new Biometric() { BiometricID = studentBiometric.Biometric.BiometricID, Code = User.Template, StpBiometricTypeID = 139, OrgID = 5, IsActive = true, ChangeDateTime = DateTime.Now };
                    if(!proxy.EditBiometric(biometric,out res))
                    {
                        MessageBox.Show("Can not save student's biometric information to database ! Please try again for " + User.UserName.Substring(0, User.UserName.IndexOf("-")),
                                       "",
                                       MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    }
                    else if (!proxy.EditStudent(student, out res))
                    {
                        MessageBox.Show("Can not save student's meal preferences to database ! Please try again for " + User.UserName.Substring(0, User.UserName.IndexOf("-")),
                                        "",
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    }
                    else
                    {
                        String szFileName = Path.Combine(m_DatabaseDir, User.UserName);
                        if (!User.Save(szFileName))
                        {
                            MessageBox.Show("Can not save student's information to file ! Please try again " + szFileName,
                                             "",
                                             MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
            }
            else
            {
                szMessage.Append("Enrollment process failed. Please try again ");
                szMessage.Append("Error description: ");
                szMessage.Append(FutronicSdkBase.SdkRetCode2Message(nRetCode));
                MessageBox.Show(szMessage.ToString(), "",
                                          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.SetStatusText("Please Try Again");
            }

            // unregister events
            m_Operation.OnPutOn -= new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff -= new OnTakeOffHandler(this.OnTakeOff);
            m_Operation.UpdateScreenImage -= new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource -= new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicEnrollment)m_Operation).OnEnrollmentComplete -= new OnEnrollmentCompleteHandler(this.OnEnrollmentComplete);

            m_OperationObj = null;
            LoadStudentBiometricList();
            EnableControls(true);
        }

        private void OnVerificationComplete(bool bSuccess,
                                            int nRetCode,
                                            bool bVerificationSuccess)
        {
            StringBuilder szResult = new StringBuilder();
            if (bSuccess)
            {
                if (bVerificationSuccess)
                {
                    szResult.Append("Verification successful.");
                    szResult.Append("User Name: ");
                    szResult.Append(((DbRecord)m_OperationObj).UserName);
                }
                else
                    szResult.Append("Verification failed. Please try again");
            }
            else
            {
                szResult.Append( "Verification process failed. Please try again" );
                szResult.Append( "Error description: ");
                szResult.Append(FutronicSdkBase.SdkRetCode2Message(nRetCode));
            }

            this.SetStatusText(szResult.ToString());
            this.SetIdentificationLimit(m_Operation.IdentificationsLeft);

            // unregister events
            m_Operation.OnPutOn -= new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff -= new OnTakeOffHandler(this.OnTakeOff);
            m_Operation.UpdateScreenImage -= new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource -= new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicVerification)m_Operation).OnVerificationComplete -= new OnVerificationCompleteHandler(this.OnVerificationComplete);

            m_OperationObj = null;
            EnableControls(true);
        }

        private void OnGetBaseTemplateComplete(bool bSuccess, int nRetCode)
        {
            StringBuilder szMessage = new StringBuilder();
            if (bSuccess)
            {
                this.SetStatusText("Starting identification...");
                List<DbRecord> Users = (List<DbRecord>)m_OperationObj;

                int iRecords = 0;
                int nResult;
                FtrIdentifyRecord[] rgRecords = new FtrIdentifyRecord[Users.Count];
                foreach (DbRecord item in Users)
                {
                    rgRecords[iRecords] = item.GetRecord();
                    iRecords++;
                }
                nResult = ((FutronicIdentification)m_Operation).Identification(rgRecords, ref iRecords);
                if (nResult == FutronicSdkBase.RETCODE_OK)
                {
                    szMessage.Append("Verification test complete. User: ");
                    if (iRecords != -1)
                        szMessage.Append(Users[iRecords].UserName.Substring(0, Users[iRecords].UserName.IndexOf("-")) + " found");
                    else
                        szMessage.Append("Not found Please try enroll again.");
                }
                else
                {
                    szMessage.Append("Identification failed.");
                    szMessage.Append(FutronicSdkBase.SdkRetCode2Message(nResult));
                }
                this.SetIdentificationLimit(m_Operation.IdentificationsLeft);
            }
            else
            {
                szMessage.Append("Can not retrieve base template.");
                szMessage.Append("Error description: ");
                szMessage.Append( FutronicSdkBase.SdkRetCode2Message(nRetCode) );
            }
            this.SetStatusText(szMessage.ToString());
            
            // unregister events
            m_Operation.OnPutOn -= new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff -= new OnTakeOffHandler(this.OnTakeOff);
            m_Operation.UpdateScreenImage -= new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource -= new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicIdentification)m_Operation).OnGetBaseTemplateComplete -=
                    new OnGetBaseTemplateCompleteHandler(this.OnGetBaseTemplateComplete);

            m_OperationObj = null;
            EnableControls(true);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.SetStatusText("");
            if (m_OperationObj == null)
                return;

            m_Operation.OnCalcel();
			// unregister events
			m_Operation.OnPutOn -= new OnPutOnHandler(this.OnPutOn);
			m_Operation.OnTakeOff -= new OnTakeOffHandler(this.OnTakeOff);
			m_Operation.UpdateScreenImage -= new UpdateScreenImageHandler(this.UpdateScreenImage);
			m_Operation.OnFakeSource -= new OnFakeSourceHandler(this.OnFakeSource);
			((FutronicIdentification)m_Operation).OnGetBaseTemplateComplete -=
					new OnGetBaseTemplateCompleteHandler(this.OnGetBaseTemplateComplete);

			m_OperationObj = null;
			this.SetStatusText("");
			//EnableControls(true);
		}

        private void EnableControls(bool bEnable)
        {
            // Do not change the state control during application closing.
            if( m_bExit )
                return;
            if (this.InvokeRequired)
            {
                EnableControlsCallback d = new EnableControlsCallback(this.EnableControls);
                this.Invoke( d, new object[] { bEnable } );
            }
            else
            {
                btnEnroll.Enabled = bEnable;
                btnIdentify.Enabled = bEnable;
                btnVerify.Enabled = bEnable;
                btnStop.Enabled = bEnable;
            }
        }

        private void SetStatusText(String text)
        {
            // Do not change the state control during application closing.
            if( m_bExit )
                return;

            if (this.txtMessage.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(this.SetStatusText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtMessage.Text = text;
                this.Update();
            }
        }

        /// <summary>
        /// Get the database directory.
        /// </summary>
        /// <returns>returns the database directory.</returns>
        public static String GetDatabaseDir()
        {
            String szDbDir;
            szDbDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create);
            szDbDir = Path.Combine(szDbDir, kCompanyName);
            if (!Directory.Exists(szDbDir))
            {
                Directory.CreateDirectory(szDbDir);
            }
            szDbDir = Path.Combine(szDbDir, kProductName);
            if (!Directory.Exists(szDbDir))
            {
                Directory.CreateDirectory(szDbDir);
            }

            szDbDir = Path.Combine(szDbDir, kDbName);
            if (!Directory.Exists(szDbDir))
            {
                Directory.CreateDirectory(szDbDir);
            }

            return szDbDir;
        }

        protected bool isUserExists(String UserName)
        {
            String szFileName;
            szFileName = Path.Combine(m_DatabaseDir, UserName );
            return File.Exists(szFileName);
        }

        protected void CreateFile(String UserName)
        {
            String szFileName;
            szFileName = Path.Combine(m_DatabaseDir, UserName);
            File.Create(szFileName).Close();
            File.Delete(szFileName);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            m_bExit = true;
            if (m_Operation != null)
            {
                m_Operation.Dispose();
            }
            base.OnFormClosing(e);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.Hide();
            m_bExit = true;
            this.Close();
        }

        private void tbFARN_Validating(object sender, CancelEventArgs e)
        {
            int nValue = -1;

            try
            {
                nValue = Int32.Parse(tbFARN.Text);
            }
            catch (FormatException)
            {
            }
            if(nValue > 1000 || nValue < 1)
            {
                MessageBox.Show(this, "Invalid FARN value. The range of value is from 1 to 1000", this.Text,
                                          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!m_bInitializationSuccess)
            {
                this.Close();
            }
        }

        private void PopulateStudentList()
		{
            APIProxy proxy = new APIProxy();
            List<SelectResult> students = proxy.GetStudentList("");
            cmbStudents.DataSource = students;
            cmbStudents.ValueMember = "ID";
            cmbStudents.DisplayMember = "Description";
        }

		private void btnSearch_Click(object sender, EventArgs e)
		{
            cmbStudents.DataSource = null;
            APIProxy proxy = new APIProxy();
            List<SelectResult> students = proxy.GetStudentList(txtSurname.Text);
            cmbStudents.DataSource = students;
            cmbStudents.ValueMember = "ID";
            cmbStudents.DisplayMember = "Description";
        }

		private void btnRefresh_Click(object sender, EventArgs e)
		{
            APIProxy proxy = new APIProxy();
            string result = string.Empty;
            if(!proxy.UpdateStudentsFromEdAdmin(ConfigurationManager.AppSettings["School"],out result))
			{
                MessageBox.Show(this, result, this.Text,
                                         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
			else
			{
                LoadStudentBiometricList();
                MessageBox.Show(this, "Student list updated from Ed Admin Successfully ", this.Text,
                                         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                PopulateStudentList();
            }
        }
	}
}
