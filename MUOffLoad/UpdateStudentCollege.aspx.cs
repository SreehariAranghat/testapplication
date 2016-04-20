using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class UpdateStudentCollege : System.Web.UI.Page
    {
        INFINITY_MUEntities entities = new INFINITY_MUEntities();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearchStudent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRegisterNumber.Text))
            {

                var student = entities.Students.FirstOrDefault(s => s.RegisterNumber == txtRegisterNumber.Text);
                if (student != null)
                {
                    pnlMessagePanel.Visible = false;
                    pnlStudentDetails.Visible = true;
                    lblStudentName.Text = student.FirstName + " " + student.LastName;
                    lblRegisterNumber.Text = student.RegisterNumber;
                    lblCollegeName.Text = student.College.Name.ToUpper() + " / " + student.College.Code.ToUpper();
                    
                    pnlStudentDataUpdate.Visible = true;
                    txtCurrentCollegeCode.Text = student.College.Code;
                }
                else
                {
                    pnlMessagePanel.Visible = true;
                    lblMessageBox.Text = "Student Does Not Exist";
                    pnlStudentDetails.Visible = false;
                    pnlStudentDataUpdate.Visible = false;
                }
            }
        }

        protected void btnSubmitStudent_Click(EventArgs e)
        {
           
        }

        protected void btnSubmitStudent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCurrentCollegeCode.Text))
            {
                College col = entities.Colleges.FirstOrDefault(d => d.Code == txtCurrentCollegeCode.Text);

                if (col != null)
                {
                    Student stu = entities.Students.FirstOrDefault(s => s.RegisterNumber == txtRegisterNumber.Text);
                    stu.College = col;

                    entities.SaveChanges();

                    pnlMessagePanel.Visible = true;
                    lblMessageBox.Text = "Student details saved successfully";
                }
                else
                {
                    pnlMessagePanel.Visible = true;
                    lblMessageBox.Text = "The college code does not exist";
                }
            }
            else
            {
                pnlMessagePanel.Visible = true;
                lblMessageBox.Text = "Please enter a college code";
            }
        }
    }
}