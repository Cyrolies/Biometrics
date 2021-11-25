using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace Futronic.SDK.WorkedEx
{
	/// <summary>
	/// Summary description for SelectUser.
	/// </summary>
	public class SelectUser : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnSelect;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox txtDatabaseDir;
		private System.Windows.Forms.ListBox lstUsers;

		private ArrayList m_Users;
		private int m_SelectedIndex;

		public SelectUser( ArrayList Users, String szDbDir )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			txtDatabaseDir.Text = szDbDir;
			m_Users = Users;
			for( int i = 0; i < m_Users.Count; i++ )
			{
				lstUsers.Items.Add( ((DbRecord)m_Users[i]).UserName);
			}
			lstUsers.SelectedIndex = 0;
			m_SelectedIndex = -1;
		}

		public DbRecord SelectedUser
		{
			get
			{
				if (m_Users.Count == 0 || m_SelectedIndex == -1)
					return null;
				return (DbRecord)m_Users[m_SelectedIndex];
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtDatabaseDir = new System.Windows.Forms.TextBox();
			this.lstUsers = new System.Windows.Forms.ListBox();
			this.btnSelect = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Database folder: ";
			// 
			// txtDatabaseDir
			// 
			this.txtDatabaseDir.Location = new System.Drawing.Point(104, 6);
			this.txtDatabaseDir.Name = "txtDatabaseDir";
			this.txtDatabaseDir.ReadOnly = true;
			this.txtDatabaseDir.Size = new System.Drawing.Size(424, 20);
			this.txtDatabaseDir.TabIndex = 1;
			this.txtDatabaseDir.TabStop = false;
			this.txtDatabaseDir.Text = "";
			// 
			// lstUsers
			// 
			this.lstUsers.Location = new System.Drawing.Point(8, 32);
			this.lstUsers.Name = "lstUsers";
			this.lstUsers.Size = new System.Drawing.Size(520, 160);
			this.lstUsers.TabIndex = 2;
			// 
			// btnSelect
			// 
			this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSelect.Location = new System.Drawing.Point(231, 208);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.TabIndex = 3;
			this.btnSelect.Text = "Select";
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// SelectUser
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(537, 241);
			this.ControlBox = false;
			this.Controls.Add(this.btnSelect);
			this.Controls.Add(this.lstUsers);
			this.Controls.Add(this.txtDatabaseDir);
			this.Controls.Add(this.label1);
			this.Name = "SelectUser";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select User";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnSelect_Click(object sender, System.EventArgs e)
		{
			m_SelectedIndex = lstUsers.SelectedIndex;
		}
	}
}
