using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Futronic.SDK.WorkedEx
{
    public partial class SelectUser : Form
    {
        private List<DbRecord> m_Users;
        private int m_SelectedIndex;

        public SelectUser( List<DbRecord> Users, String szDbDir )
        {
            InitializeComponent();
            txtDatabaseDir.Text = szDbDir;
            m_Users = Users;
            for( int i = 0; i < m_Users.Count; i++ )
            {
                lstUsers.Items.Add(m_Users[i].UserName);
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
                return m_Users[m_SelectedIndex];
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            m_SelectedIndex = lstUsers.SelectedIndex;
        }
    }
}