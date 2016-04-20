using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class UpdateSubjectCode : System.Web.UI.Page
    {
        INFINITY_MUEntities entities = new INFINITY_MUEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlSelectDegree.DataSource = entities.Degrees.OrderBy(d => d.Name).ToList();
                ddlSelectDegree.DataTextField = "Name";
                ddlSelectDegree.DataValueField = "DegreeId";
                ddlSelectDegree.DataBind();

                ddlSelectDegree.Items.Insert(0, new ListItem("Select Degree", "-1"));
            }
        }

        protected void ddlSelectSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSubjectList();
        }

        void UpdateSubjectList()
        {
            if (ddlSelectSemester.SelectedIndex > 0 && ddlSelectDegree.SelectedIndex > 0)
            {
                int selSemester = int.Parse(ddlSelectSemester.SelectedValue);
                int selDegree = int.Parse(ddlSelectDegree.SelectedValue);

                //  var subejcts = entities.CourseSubjects.Where(d => d.Semester == selSemester && d.Cours.Degree.DegreeId == selDegree).Select(d => d.Subject).ToList();
                DS_GETCOURSESUBJECTSTableAdapters.SubjectsTableAdapter sa = new DS_GETCOURSESUBJECTSTableAdapters.SubjectsTableAdapter();


                //  var distSubjectName = subejcts.Select(d => d.Name).Distinct().ToList();
                ddlSubjectName.DataSource = sa.GetData(selDegree, selSemester);
                ddlSubjectName.DataTextField = "Name";
                ddlSubjectName.DataValueField = "Name";
                ddlSubjectName.DataBind();

                ddlSubjectName.Items.Insert(0, new ListItem("Select Subject", "-1"));
            }
        }

        protected void ddlSubjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSubjectName.SelectedIndex > 0)
            {
                int selSemester = int.Parse(ddlSelectSemester.SelectedValue);
                int selDegree = int.Parse(ddlSelectDegree.SelectedValue);
                var subejcts = entities.CourseSubjects.Where(d => d.Semester == selSemester && d.Cours.Degree.DegreeId == selDegree).Select(d => d.Subject).ToList();

                var distinctSubjectCodes = subejcts.Where(d => d.Name == ddlSubjectName.SelectedItem.Text).Select(d => d.Code).Distinct().ToList();
                ddlSubjectCodes.DataSource = distinctSubjectCodes;
                ddlSubjectCodes.DataBind();
                ddlSubjectCodes.Items.Insert(0, new ListItem("Select Old Subject Code", "-1"));

                var getallpapercodes = entities.Subjects.Where(d => d.Name == ddlSubjectName.SelectedItem.Text).Select(d => d.Code).Distinct().ToList();

                ddlNewSubjectCodes.DataSource = getallpapercodes;
                ddlNewSubjectCodes.DataBind();
                ddlNewSubjectCodes.Items.Insert(0, new ListItem("Select New Subject Code", "-1"));

            }
        }

        protected void ddlSelectDegree_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSubjectList();
        }

        protected void btnMigrate_Click(object sender, EventArgs e)
        {
            if (ddlSelectDegree.SelectedIndex > 0
               && ddlSelectSemester.SelectedIndex > 0
               && ddlSubjectName.SelectedIndex > 0
               && ddlSubjectCodes.SelectedIndex > 0
               && ddlNewSubjectCodes.SelectedIndex > 0)
            {
                var degreeid = int.Parse(ddlSelectDegree.SelectedValue);
                int semester = int.Parse(ddlSelectSemester.SelectedValue);
                string oldsubjectcode = ddlSubjectCodes.SelectedValue;
                string newsubjectcode = ddlNewSubjectCodes.SelectedValue;
                int revisionnumber = int.Parse(ddlAcademicYear.SelectedValue);

                //Select the subejctid 

                var oldsubject = entities.CourseSubjects.Where(d => d.Cours.Degree.DegreeId == degreeid && d.Subject.Code == oldsubjectcode && d.Cours.RevisionNo == revisionnumber)
                    .Select(d => d.Subject).FirstOrDefault();
                var newsubject = entities.Subjects.FirstOrDefault(d => d.Code == newsubjectcode);

                if (oldsubject != null && newsubject != null)
                {
                    var coursesubjects = entities.CourseSubjects.Where(d => d.Cours.Degree.DegreeId == degreeid && d.Subject.Code == oldsubjectcode && d.Cours.RevisionNo == revisionnumber).ToList();

                    foreach (var c in coursesubjects.ToList())
                    {
                        var cs = entities.CourseSubjects.First(d => d.CourseSubjectId == c.CourseSubjectId);
                        cs.SubjectId = newsubject.SubjectId;
                        entities.SaveChanges();
                    }

                    lblMessageBox.Text = "Subject Code has been updated successfully";
                }
                else
                {
                    lblMessageBox.Text = "One or more values are invalid";
                }
            }
        }
    }
}