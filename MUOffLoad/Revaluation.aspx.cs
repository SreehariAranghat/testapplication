using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class Revaluation1 : System.Web.UI.Page
    {
        INFINITY_MUEntities entities = new INFINITY_MUEntities();
        MUPRJEntities muentities = new MUPRJEntities();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearchStudent_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtRegisterNumber.Text))
            {
                var student = entities.Students.FirstOrDefault(s => s.RegisterNumber == txtRegisterNumber.Text);

                if(student != null)
                {
                    pnlMessagePanel.Visible = false;
                    pnlStudentDetails.Visible = true;
                    lblStudentName.Text = student.FirstName + " " + student.LastName;
                    lblRegisterNumber.Text = student.RegisterNumber;
                    lblCollegeName.Text = student.College.Name.ToUpper() + " / " + student.College.Code.ToUpper();

                    pnlStudentDataUpdate.Visible = true;

                    var studetails = muentities.EvaluationBatchItems.Where(d => d.ReviewedRegNo == txtRegisterNumber.Text)
                                                                    .Select(d => new {
                                                                        CourseName = d.EvaluationBatch.CourseName,
                                                                        Semester = d.EvaluationBatch.Semester,
                                                                        PaperName = d.EvaluationBatch.PaperName,
                                                                        PaperCode = d.EvaluationBatch.PaperCode,
                                                                        BatchNo = d.EvaluationBatch.BatchNo,
                                                                        DummyNumber = d.IndexedDummyNumber
                                                                        }).OrderBy(d=>d.Semester)
                                                                          .ThenBy(d=>d.PaperName).ToList();
                    var student1 = muentities.Revaluations.Where(d => d.RegisterNumber == txtRegisterNumber.Text);
                    if(student1 != null)
                    {
                        foreach(var st in student1)
                        {
                            if(st.IsPersonalSeeing == 1)
                            {
                               
                            }
                        }
                    }
                    GridView1.DataSource = studetails;
                    GridView1.DataBind();
                }
                else
                {
                    pnlMessagePanel.Visible = true;
                    pnlStudentDataUpdate.Visible = false;
                    pnlStudentDetails.Visible = false;
                    lblMessageBox.Text = "Student Does Not Exists";
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            Revaluation rev = new Revaluation();
                       
            foreach(GridViewRow row in GridView1.Rows)
            {
                CheckBox chkPS = (CheckBox)row.FindControl("chkPersonalSeeing");
                CheckBox chkRev = (CheckBox)row.FindControl("chkRevaluation");

                       rev.CourseName = row.Cells[0].Text;
                        rev.Semester = row.Cells[1].Text;
                        rev.RegisterNumber = txtRegisterNumber.Text;
                        rev.StudentName = entities.Students.FirstOrDefault(d => d.RegisterNumber == txtRegisterNumber.Text).FirstName;
                        rev.SubjectName = row.Cells[2].Text;
                        rev.SubjectCode = row.Cells[3].Text;
                        rev.BatchNo = row.Cells[4].Text;
                        rev.DummyNo = row.Cells[5].Text;
                        rev.IsPersonalSeeing = chkPS.Checked == true ? 1 : 0;
                        rev.IsRevaluation = chkRev.Checked == true ? 1 : 0;

                        muentities.Revaluations.Add(rev);
                        muentities.SaveChanges();

            }
            pnlMessagePanel.Visible = true;
            lblMessageBox.Text = error + "Data Saved Successfully";
        }
    }
}