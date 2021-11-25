using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Futronic.SDK.WorkedEx
{
    public partial class EnrollmentName : Form
    {
        public EnrollmentName()
        {
            InitializeComponent();
        }

        public String UserName
        {
            get
            {
                return txtUserName.Text;
            }
        }
    }
}