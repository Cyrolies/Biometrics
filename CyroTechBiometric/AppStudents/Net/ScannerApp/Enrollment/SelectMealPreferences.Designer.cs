namespace StudentEnrollment
{
    partial class SelectMealPreferences
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.btnSelect = new System.Windows.Forms.Button();
			this.lblAllergies = new System.Windows.Forms.Label();
			this.lblPreference = new System.Windows.Forms.Label();
			this.cmbMealPreferences = new System.Windows.Forms.ComboBox();
			this.txtAllergies = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnSelect
			// 
			this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSelect.Location = new System.Drawing.Point(144, 119);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(164, 23);
			this.btnSelect.TabIndex = 1;
			this.btnSelect.Text = "Continue with Enrollment";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// lblAllergies
			// 
			this.lblAllergies.AutoSize = true;
			this.lblAllergies.Location = new System.Drawing.Point(48, 72);
			this.lblAllergies.Name = "lblAllergies";
			this.lblAllergies.Size = new System.Drawing.Size(52, 13);
			this.lblAllergies.TabIndex = 2;
			this.lblAllergies.Text = "Allergies: ";
			// 
			// lblPreference
			// 
			this.lblPreference.AutoSize = true;
			this.lblPreference.Location = new System.Drawing.Point(12, 24);
			this.lblPreference.Name = "lblPreference";
			this.lblPreference.Size = new System.Drawing.Size(88, 13);
			this.lblPreference.TabIndex = 4;
			this.lblPreference.Text = "Meal Preference:";
			// 
			// cmbMealPreferences
			// 
			this.cmbMealPreferences.FormattingEnabled = true;
			this.cmbMealPreferences.Location = new System.Drawing.Point(106, 17);
			this.cmbMealPreferences.Name = "cmbMealPreferences";
			this.cmbMealPreferences.Size = new System.Drawing.Size(343, 21);
			this.cmbMealPreferences.TabIndex = 5;
			// 
			// txtAllergies
			// 
			this.txtAllergies.Location = new System.Drawing.Point(106, 65);
			this.txtAllergies.Name = "txtAllergies";
			this.txtAllergies.Size = new System.Drawing.Size(343, 20);
			this.txtAllergies.TabIndex = 6;
			// 
			// SelectMealPreferences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(484, 154);
			this.ControlBox = false;
			this.Controls.Add(this.txtAllergies);
			this.Controls.Add(this.cmbMealPreferences);
			this.Controls.Add(this.lblPreference);
			this.Controls.Add(this.lblAllergies);
			this.Controls.Add(this.btnSelect);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectMealPreferences";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Specify Meal Preferences for Student";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label lblAllergies;
		private System.Windows.Forms.Label lblPreference;
		private System.Windows.Forms.ComboBox cmbMealPreferences;
		private System.Windows.Forms.TextBox txtAllergies;
	}
}