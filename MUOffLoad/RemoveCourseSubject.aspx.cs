using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class RemoveCourseSubject : System.Web.UI.Page
    {
        INFINITY_MUEntities entities = new INFINITY_MUEntities();
        DS_GETCOURSESUBJECTSTableAdapters.DataTable1TableAdapter ta = new DS_GETCOURSESUBJECTSTableAdapters.DataTable1TableAdapter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ddlSelectDegree.DataSource      = entities.Degrees.OrderBy(d => d.Name).ToList();
                ddlSelectDegree.DataTextField   = "Name";
                ddlSelectDegree.DataValueField  = "DegreeId";
                ddlSelectDegree.DataBind();

                ddlSelectDegree.Items.Insert(0, new ListItem("Select a Degree","-1"));
            }
        }

        protected void ddlSelectDegree_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlSelectDegree.SelectedIndex > 0)
            {
                int degreeid = int.Parse(ddlSelectDegree.SelectedValue);
                var courses = entities.Courses.Where(d => d.Degree.DegreeId == degreeid).OrderBy(d => d.Name)
                                        .Select(d => new { CourseName = d.Name + "(" + d.Code + ")", CourseCode = d.Code}).Distinct().ToList();
                ddlSelectCourse.DataSource = courses;
                ddlSelectCourse.DataTextField = "CourseName";
                ddlSelectCourse.DataValueField = "CourseCode";
                ddlSelectCourse.DataBind();

                ddlSelectCourse.Items.Insert(0, new ListItem("Select Course", "-1"));

            }
        }

        protected void ddlSelectCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            FetchSubjectCodes();
        }

        void FetchSubjectCodes()
        {
             if(ddlSelectDegree.SelectedIndex > 0
                && ddlSelectCourse.SelectedIndex > 0
                && ddlSelectAcademicYear.SelectedIndex > 0
                && ddlSemester.SelectedIndex > 0)
            {
                int degreeid        = int.Parse(ddlSelectDegree.SelectedValue);
                int semester        = int.Parse(ddlSemester.SelectedValue);
                int academicyear    = int.Parse(ddlSelectAcademicYear.SelectedValue);

                string coursecode    = ddlSelectCourse.SelectedValue;

                GridView1.DataSource = ta.GetData(degreeid, semester, coursecode, academicyear);
                GridView1.DataBind();
            }
        }

        protected void ddlSelectAcademicYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            FetchSubjectCodes();
        }

        protected void ddlSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            FetchSubjectCodes();
        }

        protected void btnAddCourseSubject_Click(object sender, EventArgs e)
        {
            if (ddlSelectDegree.SelectedIndex > 0
              && ddlSelectCourse.SelectedIndex > 0
              && ddlSelectAcademicYear.SelectedIndex > 0
              && ddlSemester.SelectedIndex > 0
              && (!string.IsNullOrEmpty(txtSubjectCode.Text)))
            {
                int degreeid = int.Parse(ddlSelectDegree.SelectedValue);
                int semester = int.Parse(ddlSemester.SelectedValue);
                int academicyear = int.Parse(ddlSelectAcademicYear.SelectedValue);

                string coursecode  = ddlSelectCourse.SelectedValue;
                string subjectCode = txtSubjectCode.Text;

                Subject subject = entities.Subjects.FirstOrDefault(d => d.Code == subjectCode);

                if(subject != null)
                {
                    Cours c = entities.Courses.First(d => d.Code == coursecode && d.RevisionNo == academicyear);

                    if(c != null)
                    {
                        if (!entities.CourseSubjects.Any(d => d.CourseId == c.CourseId && d.SubjectId == subject.SubjectId))
                        {
                            CourseSubject s = new CourseSubject();
                            s.Cours = c;
                            s.DegreeConsider = true;
                            s.HasExamination = true;
                            s.IsMandatory = true;
                            s.Semester = semester;
                            s.Subject = subject;
                            s.TotalTeachingHours = 0;

                            entities.CourseSubjects.Add(s);
                            entities.SaveChanges();

                            FetchSubjectCodes();

                            pnlMessagePanel.Visible = true;
                            lblMessageBox.Text = "Subject added successfully";
                        }
                        else
                        {
                            pnlMessagePanel.Visible = true;
                            lblMessageBox.Text = "Subject already exists for this course";
                        }
                    }
                    else
                    {
                        pnlMessagePanel.Visible = true;
                        lblMessageBox.Text = "Code does not exist";
                    }
                }
                else
                {
                    pnlMessagePanel.Visible = true;
                    lblMessageBox.Text = "Subject Code does not exist";
                }
            }
       }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            //Get the row that contains this button
            GridViewRow gvr     = (GridViewRow)btn.NamingContainer;
            int coursesubjectid = (int)GridView1.DataKeys[gvr.RowIndex].Values["CourseSubjectId"];
            CourseSubject cs = entities.CourseSubjects.FirstOrDefault(d => d.CourseSubjectId == coursesubjectid);

            if(cs != null)
            {
                try
                {
                    entities.CourseSubjects.Remove(cs);
                    entities.SaveChanges();

                    pnlMessagePanel.Visible = true;
                    lblMessageBox.Text = "Subject deleted successfully";

                    FetchSubjectCodes();
                }
                catch (Exception excp) {
                    pnlMessagePanel.Visible = true;
                    lblMessageBox.Text = "Cannot delete the subject " + excp.Message;
                }
            }
        }
    }
}