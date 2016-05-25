using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class ReCreatingBills : System.Web.UI.Page
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
                    lblStudentSemester.Text = student.YearSem.ToString();


                    pnlStudentDataUpdate.Visible = true;
                }
                else
                {
                    pnlMessagePanel.Visible = true;
                    lblMessageBox.Text = "Student Does Not Exists";
                    pnlStudentDataUpdate.Visible = false;
                    pnlStudentDetails.Visible = false;
                }
            }
        }

        protected void btnSubmitStudent_Click(object sender, EventArgs e)
        {
            var student = entities.Students.FirstOrDefault(s => s.RegisterNumber == txtRegisterNumber.Text).StudentId;
            var stuBill = entities.StudentBills.FirstOrDefault(r => r.StudentId == student && r.IsDeleted == false);

            stuBill.BatchId = null;
            entities.SaveChanges();


            pnlMessagePanel.Visible = true;
            lblMessageBox.Text = "Student details Saved Successfully";
        }
    }
}