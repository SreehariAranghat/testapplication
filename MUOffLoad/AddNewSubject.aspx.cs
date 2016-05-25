using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class AddNewSubject : System.Web.UI.Page
    {

        INFINITY_MUEntities entities = new INFINITY_MUEntities();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (!entities.Subjects.Any(d => d.Code == txtSubjectCode.Text))
            {
                if (!(string.IsNullOrEmpty(txtSubjectName.Text)
                    || string.IsNullOrEmpty(txtSubjectCode.Text)
                    || string.IsNullOrEmpty(txtSubjectMin.Text)
                    || string.IsNullOrEmpty(txtSubjectMax.Text)))
                {
                    Subject subject = new Subject();
                    subject.Code = txtSubjectCode.Text.ToUpper();
                    subject.CreatedDate = DateTime.Now;
                    subject.CreatedUserId = 1;
                    subject.Max = int.Parse(txtSubjectMax.Text);
                    subject.Min = int.Parse(txtSubjectMin.Text);
                    subject.Name = txtSubjectName.Text;
                    subject.RevisionNo = 1;
                    subject.RevisionRemarks = "Created";
                    subject.YearIntroduced = 2016;

                    if (!(string.IsNullOrEmpty(txtTheoryMax.Text)))
                    {
                        SubjectMark theory = new SubjectMark();
                        theory.Max = int.Parse(txtTheoryMax.Text);
                        theory.Min = int.Parse(txtTheoryMin.Text);
                        theory.SubjectComponentId = 1;

                        SubjectMark theoryIA = new SubjectMark();
                        theoryIA.Max = int.Parse(txtTheoryIAMax.Text);
                        theoryIA.Min = int.Parse(txtTheoryIAMin.Text);
                        theoryIA.SubjectComponentId = 2;

                        subject.SubjectMarks.Add(theory);
                        subject.SubjectMarks.Add(theoryIA);

                    }

                    if (!string.IsNullOrEmpty(txtPractialMax.Text))
                    {
                        SubjectMark practical = new SubjectMark();
                        practical.Max = int.Parse(txtPractialMax.Text);
                        practical.Min = int.Parse(txtPractialMin.Text);
                        practical.SubjectComponentId = 3;

                        SubjectMark practialIA = new SubjectMark();
                        practialIA.Max = int.Parse(txtPractialIAMax.Text);
                        practialIA.Min = int.Parse(txtPractialIAMin.Text);
                        practialIA.SubjectComponentId = 6;

                        subject.SubjectMarks.Add(practical);
                        subject.SubjectMarks.Add(practialIA);
                    }

                    if(!string.IsNullOrEmpty(txtProjectMax.Text))
                    {
                        SubjectMark project = new SubjectMark();
                        project.Max = int.Parse(txtProjectMax.Text);
                        project.Min = int.Parse(txtProjectMin.Text);
                        project.SubjectComponentId = 4;

                        SubjectMark viva = new SubjectMark();
                        viva.Max = int.Parse(txtVivaMax.Text);
                        viva.Min = int.Parse(txtVivaMin.Text);
                        viva.SubjectComponentId = 5;

                        subject.SubjectMarks.Add(project);
                        subject.SubjectMarks.Add(viva);
                    }

                    entities.Subjects.Add(subject);
                    entities.SaveChanges();

                    pnlMessagePanel.Visible = true;
                    lblMessageBox.Text = "Subject Saved Successfully";
                }
                else
                {
                    pnlMessagePanel.Visible = true;
                    lblMessageBox.Text = "One or more fields are missing";
                }
            }
            else
            {
                pnlMessagePanel.Visible = true;
                lblMessageBox.Text = "The subject with the same code already exist";
            }
        }
    }
}