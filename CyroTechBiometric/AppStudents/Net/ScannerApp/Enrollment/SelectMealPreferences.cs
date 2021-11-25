using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Service;
using DALEFModel;

namespace StudentEnrollment
{
    public partial class SelectMealPreferences : Form
    {
        public int _studentID;
        public SelectResult selectedMealPreference;
        public string allergies;
        public SelectMealPreferences(int studentID)
        {
            _studentID = studentID;
            InitializeComponent();
            PopulateMealPreferences();
            GetStudentData();
        }
        private void PopulateMealPreferences()
        {
            APIProxy proxy = new APIProxy();
            List<SelectResult> students = proxy.GetMealPreferences();
            cmbMealPreferences.DataSource = students;
            cmbMealPreferences.ValueMember = "ID";
            cmbMealPreferences.DisplayMember = "Description";
        }
        private void GetStudentData()
		{
            try
            {
                APIProxy proxy = new APIProxy();
                string res = string.Empty;
                Student student = proxy.GetStudent(_studentID, out res);
                if (student != null)
                {
                    txtAllergies.Text = student.Allergies;
                    cmbMealPreferences.SelectedValue = student.StpMealTypeID??141;
                }


            }
            catch (Exception ex)
            {
            }
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            selectedMealPreference = (SelectResult)cmbMealPreferences.SelectedItem;
            allergies = txtAllergies.Text;
        }
    }
}