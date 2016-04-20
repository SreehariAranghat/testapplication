using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class GetScanBatch : System.Web.UI.Page
    {
        INFINITY_MUEntities entities = new INFINITY_MUEntities();
        MUPRJEntities muentities     = new MUPRJEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedInUser"] != null)
            {
                MUOffLoad.User u = (MUOffLoad.User)Session["LoggedInUser"];
                if (u.UserName == "SysAdmin")
                {

                }
                else
                {
                    FormsAuthentication.RedirectToLoginPage();
                }

            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }
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

                    var scandetails = muentities.EvaluationBatchItems.Where(d => d.ReviewedRegNo == txtRegisterNumber.Text)
                                                                      .Select(d => new
                                                                      {
                                                                          Semester = d.EvaluationBatch.Semester
                                                                                         ,
                                                                          PaperName = d.EvaluationBatch.PaperName
                                                                                         ,
                                                                          PaperCode = d.EvaluationBatch.PaperCode
                                                                                         ,
                                                                          BatchNo = d.EvaluationBatch.BatchNo
                                                                                         ,
                                                                          DummyNumber = d.IndexedDummyNumber
                                                                      })
                                                                                         .OrderBy(d => d.Semester)
                                                                                         .ThenBy(d => d.PaperName).ToList();

                    GridView1.DataSource = scandetails;
                    GridView1.DataBind();
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
    }
}