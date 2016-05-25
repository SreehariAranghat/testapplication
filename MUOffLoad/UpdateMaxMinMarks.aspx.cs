using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class UpdateMaxMinMarks : System.Web.UI.Page
    {
        INFINITY_MUEntities muentities = new INFINITY_MUEntities();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSubejctCode.Text))
            {
                Subject subject = muentities.Subjects.FirstOrDefault(d => d.Code == txtSubejctCode.Text);
                //SubjectMark subjectmark = muentities.SubjectMarks.FirstOrDefault(d => d.SubjectId == subject.SubjectId);
                if (subject != null)
                {
                    pnlSubjectMaxMinUpdate.Visible = true;
                    var subdetails = muentities.SubjectMarks.Where(s => s.SubjectId == subject.SubjectId)
                                                                                        .Select(s1 => new
                                                                                        {
                                                                                            Code = subject.Code,
                                                                                            Name = subject.Name,
                                                                                            SubjectComponentId = s1.SubjectComponentId,
                                                                                            Max = s1.Max,
                                                                                            Min = s1.Min,
                                                                                            SubjectCCName = muentities.SubjectComponents.FirstOrDefault(d => d.SubjectComponentId == s1.SubjectComponentId).Name
                                                                                        }).OrderBy(s1=>s1.SubjectComponentId).ToList();
                    GridView1.DataSource = subdetails;
                    GridView1.DataBind();
                                                                            
                }
                else
                {
                    lblMessageBox.Text = "Subject Code Does Not Exist";
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Subject subject = muentities.Subjects.FirstOrDefault(d => d.Code == txtSubejctCode.Text);
            

            foreach(GridViewRow row in GridView1.Rows)
            {
                Label lblCID = (Label)row.FindControl("lblComponentID");
                TextBox txtMaximum = (TextBox)row.FindControl("txtMax");
                TextBox txtMinimum = (TextBox)row.FindControl("txtMin");
                int c = int.Parse(lblCID.Text);
                if (muentities.SubjectMarks.Any(d=>d.SubjectId == subject.SubjectId && d.SubjectComponentId == c))
                {
                    SubjectMark s = muentities.SubjectMarks.FirstOrDefault(d => d.SubjectId == subject.SubjectId && d.SubjectComponentId == c);

                    s.Max = int.Parse(txtMaximum.Text);
                    s.Min = int.Parse(txtMinimum.Text);

                    muentities.SaveChanges();
                }
            }
            pnlMessagePanel.Visible = true;
            lblMessageBox.Text = "Data Saved Successfully";
        }
    }
}