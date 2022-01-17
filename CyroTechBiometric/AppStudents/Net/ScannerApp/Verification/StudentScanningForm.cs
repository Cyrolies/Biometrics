using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Futronic.SDKHelper;
using Service;
using DALEFModel;
using System.Configuration;

namespace StudentScanner
{
	public partial class StudentScanningForm : Form
    {
        const string kCompanyName = "CyroTech";
        const string kProductName = "SDK 4.0";
        const string kDbName = "ScanningData";
        const string kDbPath = "../Data";
        private List<DbRecord> Users;
        int countdown = 90;
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

        public StudentScanningForm()
        {
            try
            {
                InitializeComponent();
            
                m_DatabaseDir = GetDatabaseDir();
            

            LoadBiometricList();
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
			
            m_bInitializationSuccess = true;
            InitializerScanner();
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
            catch(Exception ex)
			{
                MessageBox.Show(this,"Initialization failed",
                    ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void SetIdentificationLimit(int nLimit)
        {
			try 
            { 
                if( this.m_lblIdentificationsLimit.InvokeRequired )
                {
                    SetIdentificationLimitCallback d = new SetIdentificationLimitCallback(this.SetIdentificationLimit);
                    this.Invoke( d, new object[] { nLimit } );
                } else {
                    if (nLimit == Int32.MaxValue)
                    {
                        m_lblIdentificationsLimit.Text = "Identification limit: No limits";
                    }
                    else
                    {
                        m_lblIdentificationsLimit.Text = String.Format("Identification limit: {0}", nLimit);
                    }
                }
        
            }
            catch(Exception ex)
			{
                throw ex;
			}
}

    
        
        private void LoadBiometricList()
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
                        dbrecord.UserName = pb.Student.Firstname + " " + pb.Student.Surname + " " +pb.Student.AdmissionNo + "-" + pb.Student.StudentID.ToString();

                        CreateFile(dbrecord.UserName);
                        String szFileName = Path.Combine(m_DatabaseDir, dbrecord.UserName);
                        dbrecord.Save(szFileName);
                        Users.Add(dbrecord);
                    }
                }
			}
        }
       
        //private void btnIdentify_Click(object sender, EventArgs e)
        private void InitializerScanner()
        {
           // EnableControls(false);
            SetStatusText("Programme is loading database, please wait ...");
            
            SetStatusText(String.Empty);
            if (Users.Count == 0)
            {
               SetStatusText("Error !! No fingerprint data was retrieved from server "+ Environment.NewLine +"This is a system issue Please notify the Administration office.");
               // EnableControls(true);
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
                    DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,
                    TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
                    if (iRecords != -1)
                    {
                        int studentID = Convert.ToInt32(Users[iRecords].UserName.Substring(Users[iRecords].UserName.IndexOf("-")+1));
                        Attendance att = new Attendance() {OrgID = Convert.ToInt32(ConfigurationManager.AppSettings["OrgID"]), CreateDateTime = SATime, StudentID = studentID, Location= ConfigurationManager.AppSettings["Location"] };
                        APIProxy proxy = new APIProxy();
                        string res = string.Empty;
                        if (!proxy.EditAttendance(att, out res))
                        {
                            szMessage.Append(" Please try again after reset");
                        }
                        else
                        {
                            szMessage.Append("Thank you ! You are registered for today : " + Environment.NewLine);
                            szMessage.Append(Users[iRecords].UserName.Substring(0, Users[iRecords].UserName.IndexOf("-")) + " at " + SATime.ToString("yyyy-MM-dd  HH:mm"));
                        }
                        StartTimer(Convert.ToInt32(ConfigurationManager.AppSettings["SuccessResetTime"].ToString()));
                            
                    }
                    else
                    {
                        szMessage.Append("Your details were not found " + Environment.NewLine + " Please try again when it has reset " + Environment.NewLine + " Also try cleaning scanner with a dry cloth " + Environment.NewLine + " If no luck then please check with administration if you are enrolled.");
                        StartTimer(Convert.ToInt32(ConfigurationManager.AppSettings["ErrorResetTime"].ToString()));
                    }
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
                szMessage.Append("Can not retrieve base template." + Environment.NewLine);
                szMessage.Append("Error description: " + Environment.NewLine);
                szMessage.Append( FutronicSdkBase.SdkRetCode2Message(nRetCode)  + Environment.NewLine);
               // szMessage.Append("Please notify administration.");
            }
            this.SetStatusText(szMessage.ToString());

            // unregister events
           // m_Operation.OnCalcel();
            m_Operation.OnPutOn -= new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff -= new OnTakeOffHandler(this.OnTakeOff);
            m_Operation.UpdateScreenImage -= new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource -= new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicIdentification)m_Operation).OnGetBaseTemplateComplete -=
                    new OnGetBaseTemplateCompleteHandler(this.OnGetBaseTemplateComplete);

           // m_Operation = null;
           // EnableControls(true);
        }

		
        private void StartTimer(int interval)
		{
            countdown = interval;
            timer.Start();
        }
		// private void btnStop_Click(object sender, EventArgs e)
		private void Reset()
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
			EnableControls(true);
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
           //     btnEnroll.Enabled = bEnable;
           //     btnIdentify.Enabled = bEnable;
           //     btnVerify.Enabled = bEnable;
           //     btnStop.Enabled = bEnable;
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

		private void timer1_Tick(object sender, EventArgs e)
		{
            if (countdown > 0)
            {
                countdown--;
                this.lblCounter.Text = countdown.ToString();
            }
            else
            {
                if (txtMessage.Text.Length > 0 && txtMessage.Text.Substring(0,3).ToUpper() != "PUT" && txtMessage.Text.Substring(0, 8).ToUpper() != "STARTING")
                {
                    Reset();
                    InitializerScanner();
                }
            }
        }
	}
}
