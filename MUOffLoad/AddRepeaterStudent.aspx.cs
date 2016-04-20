using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class AddRepeaterStudent : System.Web.UI.Page
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



                    var studentRepeaterList = entities.StudentRepeaterDetails.Where(d => d.Student.RegisterNumber == txtRegisterNumber.Text).Select(d => new
                    {
                        SubjectName = d.CourseSubject.Subject.Name,
                        SubjectCode = d.CourseSubject.Subject.Code,
                        Semester = d.CourseSubject.Semester

                    }).OrderBy(d => d.Semester).ToList();

                    gvStudentCurrentSubjects.DataSource = studentRepeaterList;
                    gvStudentCurrentSubjects.DataBind();

                    pnlSubjectsList.Visible = true;
                    pnlStudentDetails.Visible = true;

                }
                else
                {
                    pnlMessagePanel.Visible = true;
                    lblMessageBox.Text = "Student Does Not Exist";

                    pnlSubjectsList.Visible = false;
                    pnlStudentDetails.Visible = false;
                }
            }
        }

        protected void btnAddSubjectToRepeaterDetails_Click(object sender, EventArgs e)
        {
            if (ddlSubjectList.SelectedIndex > 0 && (!(string.IsNullOrEmpty(txtRegisterNumber.Text))))
            {
                pnlMessagePanel.Visible = false;
                StudentRepeaterDetail rd = new StudentRepeaterDetail();

                int courseSubjectId = int.Parse(ddlSubjectList.SelectedValue);
                CourseSubject cs = entities.CourseSubjects.FirstOrDefault(d => d.CourseSubjectId == courseSubjectId);
                Student student = entities.Students.FirstOrDefault(d => d.RegisterNumber == txtRegisterNumber.Text);

                if (cs != null && student != null)
                {
                    if (!entities.StudentRepeaterDetails.Any(d => d.Student.RegisterNumber == txtRegisterNumber.Text && d.CourseSubject.CourseSubjectId == courseSubjectId))
                    {
                        rd.CourseSubject = cs;
                        rd.CreatedDate = DateTime.Now;
                        rd.CreatedUserId = 1;
                        rd.ExaminationId = 2;
                        rd.Student = student;

                        entities.StudentRepeaterDetails.Add(rd);
                        entities.SaveChanges();

                        var studentRepeaterList = entities.StudentRepeaterDetails.Where(d => d.Student.RegisterNumber == txtRegisterNumber.Text).Select(d => new
                        {
                            SubjectName = d.CourseSubject.Subject.Name,
                            SubjectCode = d.CourseSubject.Subject.Code,
                            Semester = d.CourseSubject.Semester

                        }).OrderBy(d => d.Semester).ToList();

                        gvStudentCurrentSubjects.DataSource = studentRepeaterList;
                        gvStudentCurrentSubjects.DataBind();

                        pnlMessagePanel.Visible = true;
                        lblMessageBox.Text = "Successfully added the Subject";
                    }
                    else
                    {
                        pnlMessagePanel.Visible = true;
                        lblMessageBox.Text = "The subject already exists for the student";
                    }
                }
                else
                {
                    pnlMessagePanel.Visible = true;
                    lblMessageBox.Text = "Invalid Student or Course Subject";
                }

            }
            else
            {
                pnlMessagePanel.Visible = true;
                lblMessageBox.Text = "Please select a valid subject and a valid register number";
            }
        }

        protected void ddlSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSemester.SelectedIndex > 0 && (!string.IsNullOrEmpty(txtRegisterNumber.Text)))
            {
                var student = entities.Students.FirstOrDefault(s => s.RegisterNumber == txtRegisterNumber.Text);
                if (student != null)
                {
                    int semester = int.Parse(ddlSemester.SelectedValue);
                    var subjects = entities.CourseSubjects.Where(d => d.Semester == semester && d.Cours.CourseId == student.Cours.CourseId)
                                                                .Select(d => new { SubjectName = d.Subject.Name + "(" + d.Subject.Code + ")", Id = d.CourseSubjectId }).Distinct().ToList();


                    ddlSubjectList.DataSource = subjects;
                    ddlSubjectList.DataTextField = "SubjectName";
                    ddlSubjectList.DataValueField = "Id";
                    ddlSubjectList.DataBind();

                    ddlSubjectList.Items.Insert(0, new ListItem("Select Subject", "-1"));
                }
            }
            else
            {
                ddlSubjectList.Enabled = false;
            }
        }
    }
}