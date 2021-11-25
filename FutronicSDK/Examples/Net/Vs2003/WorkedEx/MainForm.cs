using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Text;
using Futronic.SDKHelper;

namespace Futronic.SDK.WorkedEx
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chDetectFakeFinger;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbFARNLevel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbFARN;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbMaxFrames;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.PictureBox PictureFingerPrint;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button btnEnroll;
		private System.Windows.Forms.Button btnVerify;
		private System.Windows.Forms.Button btnIdentify;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Button btnExit;

		const string kCompanyName = "Futronic";
		const string kProductName = "SDK 4.0";
		const string kDbName = "DataBaseNet";

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

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

		/// <summary>
		/// The type of this parameter is depending from current operation. For
		/// enrollment operation this is DbRecord.
		/// </summary>
		private Object m_OperationObj;
		private System.Windows.Forms.Label m_lblIdentificationsLimit;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox m_cmbVersion;

		/// <summary>
		/// A directory name to write user's information.
		/// </summary>
		private String m_DatabaseDir;
		private System.Windows.Forms.CheckBox cbMIOTOff;
		private System.Windows.Forms.CheckBox chFastMode;

		static private ComboBoxItem[] rgVersionItems = new ComboBoxItem[]
			{
				new ComboBoxItem( "SDK 3.0", VersionCompatible.ftr_version_previous ),
				new ComboBoxItem( "SDK 3.5", VersionCompatible.ftr_version_current ),
				new ComboBoxItem( "Both", VersionCompatible.ftr_version_compatible )
			};

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			try
			{
				m_DatabaseDir = GetDatabaseDir();
			}
			catch( IOException )
			{
				MessageBox.Show(this, "Initialization failed. Application will be close.\nCan not create database folder",
					this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			// Create FutronicEnrollment object for retrieve default values only
			FutronicEnrollment dummy = new FutronicEnrollment();
			cbFARNLevel.SelectedIndex = (int)dummy.FARnLevel;
			cbMaxFrames.SelectedItem = dummy.MaxModels.ToString();
			chDetectFakeFinger.Checked = dummy.FakeDetection;
			cbMIOTOff.Checked = dummy.MIOTControlOff;
			SetIdentificationLimit( dummy.IdentificationsLeft );
			chFastMode.Checked = dummy.FastMode;

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

			btnStop.Enabled = false;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.m_cmbVersion = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.m_lblIdentificationsLimit = new System.Windows.Forms.Label();
			this.cbMaxFrames = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cbMIOTOff = new System.Windows.Forms.CheckBox();
			this.tbFARN = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cbFARNLevel = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.chDetectFakeFinger = new System.Windows.Forms.CheckBox();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.PictureFingerPrint = new System.Windows.Forms.PictureBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnIdentify = new System.Windows.Forms.Button();
			this.btnVerify = new System.Windows.Forms.Button();
			this.btnEnroll = new System.Windows.Forms.Button();
			this.btnExit = new System.Windows.Forms.Button();
			this.chFastMode = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chFastMode);
			this.groupBox1.Controls.Add(this.m_cmbVersion);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.m_lblIdentificationsLimit);
			this.groupBox1.Controls.Add(this.cbMaxFrames);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.cbMIOTOff);
			this.groupBox1.Controls.Add(this.tbFARN);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.cbFARNLevel);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.chDetectFakeFinger);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(464, 216);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = " Settings ";
			// 
			// m_cmbVersion
			// 
			this.m_cmbVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cmbVersion.Location = new System.Drawing.Point(208, 144);
			this.m_cmbVersion.Name = "m_cmbVersion";
			this.m_cmbVersion.Size = new System.Drawing.Size(121, 21);
			this.m_cmbVersion.TabIndex = 10;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 146);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(184, 16);
			this.label4.TabIndex = 9;
			this.label4.Text = "Do image processing compatible to: ";
			// 
			// m_lblIdentificationsLimit
			// 
			this.m_lblIdentificationsLimit.Location = new System.Drawing.Point(16, 184);
			this.m_lblIdentificationsLimit.Name = "m_lblIdentificationsLimit";
			this.m_lblIdentificationsLimit.Size = new System.Drawing.Size(392, 16);
			this.m_lblIdentificationsLimit.TabIndex = 8;
			// 
			// cbMaxFrames
			// 
			this.cbMaxFrames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbMaxFrames.Items.AddRange(new object[] {
															 "1",
															 "2",
															 "3",
															 "4",
															 "5",
															 "6",
															 "7",
															 "8",
															 "9",
															 "10"});
			this.cbMaxFrames.Location = new System.Drawing.Point(176, 110);
			this.cbMaxFrames.Name = "cbMaxFrames";
			this.cbMaxFrames.Size = new System.Drawing.Size(48, 21);
			this.cbMaxFrames.TabIndex = 7;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 112);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(152, 16);
			this.label3.TabIndex = 6;
			this.label3.Text = "Set max frames in template:";
			// 
			// cbMIOTOff
			// 
			this.cbMIOTOff.Location = new System.Drawing.Point(16, 88);
			this.cbMIOTOff.Name = "cbMIOTOff";
			this.cbMIOTOff.Size = new System.Drawing.Size(104, 16);
			this.cbMIOTOff.TabIndex = 5;
			this.cbMIOTOff.Text = "Disable MIOT";
			// 
			// tbFARN
			// 
			this.tbFARN.Location = new System.Drawing.Point(304, 54);
			this.tbFARN.MaxLength = 5;
			this.tbFARN.Name = "tbFARN";
			this.tbFARN.TabIndex = 4;
			this.tbFARN.Text = "";
			this.tbFARN.Validating += new System.ComponentModel.CancelEventHandler(this.tbFARN_Validating);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(256, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "value";
			// 
			// cbFARNLevel
			// 
			this.cbFARNLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbFARNLevel.Items.AddRange(new object[] {
															 "low",
															 "below normal",
															 "normal",
															 "above normal",
															 "high",
															 "maximum",
															 "custom"});
			this.cbFARNLevel.Location = new System.Drawing.Point(120, 54);
			this.cbFARNLevel.Name = "cbFARNLevel";
			this.cbFARNLevel.Size = new System.Drawing.Size(121, 21);
			this.cbFARNLevel.TabIndex = 2;
			this.cbFARNLevel.SelectedIndexChanged += new System.EventHandler(this.cbFARNLevel_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Set measure level: ";
			// 
			// chDetectFakeFinger
			// 
			this.chDetectFakeFinger.Location = new System.Drawing.Point(16, 24);
			this.chDetectFakeFinger.Name = "chDetectFakeFinger";
			this.chDetectFakeFinger.Size = new System.Drawing.Size(128, 24);
			this.chDetectFakeFinger.TabIndex = 0;
			this.chDetectFakeFinger.Text = "Detect fake finger";
			// 
			// txtMessage
			// 
			this.txtMessage.Location = new System.Drawing.Point(8, 232);
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.ReadOnly = true;
			this.txtMessage.Size = new System.Drawing.Size(464, 20);
			this.txtMessage.TabIndex = 1;
			this.txtMessage.TabStop = false;
			this.txtMessage.Text = "";
			// 
			// PictureFingerPrint
			// 
			this.PictureFingerPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.PictureFingerPrint.Image = ((System.Drawing.Image)(resources.GetObject("PictureFingerPrint.Image")));
			this.PictureFingerPrint.Location = new System.Drawing.Point(312, 264);
			this.PictureFingerPrint.Name = "PictureFingerPrint";
			this.PictureFingerPrint.Size = new System.Drawing.Size(160, 210);
			this.PictureFingerPrint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PictureFingerPrint.TabIndex = 2;
			this.PictureFingerPrint.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.btnStop);
			this.groupBox2.Controls.Add(this.btnIdentify);
			this.groupBox2.Controls.Add(this.btnVerify);
			this.groupBox2.Controls.Add(this.btnEnroll);
			this.groupBox2.Location = new System.Drawing.Point(8, 264);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(288, 160);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Operations";
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(200, 104);
			this.btnStop.Name = "btnStop";
			this.btnStop.TabIndex = 3;
			this.btnStop.Text = "Stop";
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnIdentify
			// 
			this.btnIdentify.Location = new System.Drawing.Point(16, 104);
			this.btnIdentify.Name = "btnIdentify";
			this.btnIdentify.TabIndex = 2;
			this.btnIdentify.Text = "Identify";
			this.btnIdentify.Click += new System.EventHandler(this.btnIdentify_Click);
			// 
			// btnVerify
			// 
			this.btnVerify.Location = new System.Drawing.Point(16, 64);
			this.btnVerify.Name = "btnVerify";
			this.btnVerify.TabIndex = 1;
			this.btnVerify.Text = "Verify";
			this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
			// 
			// btnEnroll
			// 
			this.btnEnroll.Location = new System.Drawing.Point(16, 24);
			this.btnEnroll.Name = "btnEnroll";
			this.btnEnroll.TabIndex = 0;
			this.btnEnroll.Text = "Enroll";
			this.btnEnroll.Click += new System.EventHandler(this.btnEnroll_Click);
			// 
			// btnExit
			// 
			this.btnExit.Location = new System.Drawing.Point(208, 456);
			this.btnExit.Name = "btnExit";
			this.btnExit.TabIndex = 4;
			this.btnExit.Text = "Exit";
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// chFastMode
			// 
			this.chFastMode.Location = new System.Drawing.Point(208, 24);
			this.chFastMode.Name = "chFastMode";
			this.chFastMode.TabIndex = 11;
			this.chFastMode.Text = "Fast mode";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(487, 492);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.PictureFingerPrint);
			this.Controls.Add(this.txtMessage);
			this.Controls.Add(this.groupBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Text = "C# example for Futronic SDK v.4.2";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

		private void SetIdentificationLimit(int nLimit)
		{
			if( this.m_lblIdentificationsLimit.InvokeRequired )
			{
				SetIdentificationLimitCallback d = new SetIdentificationLimitCallback(this.SetIdentificationLimit);
				this.Invoke( d, new object[] { nLimit } );
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
		}

		private void cbFARNLevel_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if( cbFARNLevel.SelectedIndex == 6 )
			{
				tbFARN.ReadOnly = false;
			} 
			else 
			{
				tbFARN.Text = FutronicSdkBase.rgFARN[cbFARNLevel.SelectedIndex].ToString();
				tbFARN.ReadOnly = true;
			}
		}

		private void tbFARN_Validating(object sender, System.ComponentModel.CancelEventArgs e)
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

		private void btnEnroll_Click(object sender, System.EventArgs e)
		{
			DbRecord User = new DbRecord();
            
			// Get user name
			EnrollmentName frmName = new EnrollmentName();
			frmName.ShowDialog( this );
			if (frmName.DialogResult != DialogResult.OK )
			{
				return;
			}
			if( frmName.UserName.Length == 0)
			{
				MessageBox.Show( this, "You must enter a user name.", this.Text, 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				return;
			}
			// Try creat the file for user's information
			if( isUserExists(frmName.UserName) )
			{
				DialogResult nResponse;
				nResponse = MessageBox.Show("User already exists. Do you want replace it?",
											"C# example for Futronic SDK",
											MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (nResponse == DialogResult.No)
					return;
			} else {
				try
				{
					CreateFile(frmName.UserName);
				}
				catch (DirectoryNotFoundException)
				{
					MessageBox.Show(this, "Can not create file to save an user's information.", this.Text,
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				catch (IOException )
				{
					MessageBox.Show(this, String.Format("Bad user name '{0}'.", frmName.UserName),
						this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			User.UserName = frmName.UserName;

			m_OperationObj = User;
			if (m_Operation != null)
			{
				// Do not call Dispose function in completion routine.
				m_Operation.Dispose();
				m_Operation = null;
			}
			m_Operation = new FutronicEnrollment();

			// Set control properties
			m_Operation.FakeDetection = chDetectFakeFinger.Checked;
			m_Operation.FFDControl = true;
			m_Operation.FARN = Int32.Parse(tbFARN.Text);
			m_Operation.Version = (VersionCompatible)((ComboBoxItem)m_cmbVersion.SelectedItem).Tag;
			m_Operation.FastMode = chFastMode.Checked;
			((FutronicEnrollment)m_Operation).MIOTControlOff = cbMIOTOff.Checked;
			((FutronicEnrollment)m_Operation).MaxModels = Int32.Parse( (String)cbMaxFrames.SelectedItem );

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

		private void btnVerify_Click(object sender, System.EventArgs e)
		{
			EnableControls(false);
			SetStatusText("Programme is loading database, please wait ...");
			ArrayList Users = DbRecord.ReadRecords(m_DatabaseDir);
			SetStatusText( String.Empty );
			if( Users.Count == 0 )
			{
				EnableControls(true);
				MessageBox.Show(this, "Users not found. Please, run enrollment process first.", 
					this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			// select user for verification
			SelectUser frmSelectUser = new SelectUser( Users, m_DatabaseDir );
			frmSelectUser.ShowDialog( this );

			if( frmSelectUser.SelectedUser == null )
			{
				EnableControls(true);
				MessageBox.Show(this, "No selected user", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			m_OperationObj = frmSelectUser.SelectedUser;

			if (m_Operation != null)
			{
				// Do not call Dispose function in completion routine.
				m_Operation.Dispose();
				m_Operation = null;
			}
			m_Operation = new FutronicVerification( ((DbRecord)m_OperationObj).Template );

			// Set control properties
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
			((FutronicVerification)m_Operation).OnVerificationComplete += new OnVerificationCompleteHandler(this.OnVerificationComplete);

			// start verification process
			((FutronicVerification)m_Operation).Verification();
		}

		private void btnIdentify_Click(object sender, System.EventArgs e)
		{
			EnableControls(false);
			SetStatusText("Programme is loading database, please wait ...");
			ArrayList Users = DbRecord.ReadRecords( m_DatabaseDir );
			SetStatusText(String.Empty);
			if( Users.Count == 0 )
			{
				MessageBox.Show(this, "Users not found. Please, run enrollment process first.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				EnableControls(true);
				return;
			}

			m_OperationObj = Users;
			if (m_Operation != null)
			{
				// Do not call Dispose function in completion routine.
				m_Operation.Dispose();
				m_Operation = null;
			}
			m_Operation = new FutronicIdentification();

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

		private void OnPutOn( MFTR_PROGRESS Progress)
		{
			this.SetStatusText("Put finger into device, please ...");
		}

		private void OnTakeOff( MFTR_PROGRESS Progress )
		{
			this.SetStatusText("Take off finger from device, please ...");
		}

		private void UpdateScreenImage(Bitmap hBitmap)
		{
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

		private bool OnFakeSource( MFTR_PROGRESS Progress)
		{
			DialogResult result;
			result = MessageBox.Show("Fake source detected. Do you want continue process?", 
				"C# example for Futronic SDK",
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
				this.SetStatusText( szMessage.ToString() );

				// Set template into user's information and save it
				DbRecord User = (DbRecord)m_OperationObj;
				User.Template = ((FutronicEnrollment)m_Operation).Template;

				String szFileName = Path.Combine(m_DatabaseDir, User.UserName );
				if (!User.Save(szFileName) )
				{
					MessageBox.Show( "Can not save users's information to file " + szFileName,
						"C# example for Futronic SDK",
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			else
			{
				szMessage.Append("Enrollment process failed.");
				szMessage.Append("Error description: ");
				szMessage.Append(FutronicSdkBase.SdkRetCode2Message(nRetCode));
				this.SetStatusText(szMessage.ToString() );
			}

            // unregister events
            m_Operation.OnPutOn -= new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff -= new OnTakeOffHandler(this.OnTakeOff);
            m_Operation.UpdateScreenImage -= new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource -= new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicEnrollment)m_Operation).OnEnrollmentComplete -= new OnEnrollmentCompleteHandler(this.OnEnrollmentComplete);

            m_OperationObj = null;
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
					szResult.Append("Verification is successful.");
					szResult.Append("User Name: ");
					szResult.Append(((DbRecord)m_OperationObj).UserName);
				}
				else
					szResult.Append("Verification failed.");
			}
			else
			{
				szResult.Append( "Verification process failed." );
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
				ArrayList Users = (ArrayList)m_OperationObj;

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
					szMessage.Append("Identification process complete. User: ");
					if (iRecords != -1)
						szMessage.Append( ((DbRecord)Users[iRecords]).UserName);
					else
						szMessage.Append("not found");
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

		private void btnStop_Click(object sender, System.EventArgs e)
		{
			m_Operation.OnCalcel();
		}

		private void EnableControls(bool bEnable)
		{
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
				btnStop.Enabled = !bEnable;
			}
		}

		private void SetStatusText(String text)
		{
			if( this.txtMessage.InvokeRequired )
			{
				SetTextCallback d = new SetTextCallback(this.SetStatusText);
				this.Invoke( d, new object[] { text } );
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
			szDbDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal );
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

		protected override void OnClosing( CancelEventArgs e)
		{
			if (m_Operation != null)
			{
				m_Operation.Dispose();
				m_Operation = null;
			}
			base.OnClosing(e);
		}

		private void btnExit_Click(object sender, System.EventArgs e)
		{
			if (m_Operation != null)
			{
				m_Operation.Dispose();
				m_Operation = null;
			}
			this.Close();
		}

	}
}
