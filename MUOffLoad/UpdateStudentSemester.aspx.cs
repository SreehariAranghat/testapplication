using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class UpdateStudentSemester : System.Web.UI.Page
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
                    txtStudentSemester.Text = student.YearSem.ToString();
                }
                else
                {
                    pnlMessagePanel.Visible         = true;
                    lblMessageBox.Text              = "Student Does Not Exist";
                    pnlStudentDetails.Visible       = false;
                    pnlStudentDataUpdate.Visible    = false;
                }
            }
        }

        protected void btnSubmitStudent_Click(object sender, EventArgs e)
        {
            int semester;

            if(int.TryParse(txtStudentSemester.Text,out semester))
            {
                var student = entities.Students.FirstOrDefault(s => s.RegisterNumber == txtRegisterNumber.Text);
                student.YearSem = semester;
                entities.SaveChanges();

                pnlMessagePanel.Visible = true;
                lblMessageBox.Text = "Student detailes saved successfully";
            }
        }
    }
}