using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class UpdateSubjectName : System.Web.UI.Page
    {
        INFINITY_MUEntities entities = new INFINITY_MUEntities();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewSubjectName.Text))
            {
                Subject subject = entities.Subjects.FirstOrDefault(d => d.Code == txtSubejctCode.Text);
                subject.Name    = txtNewSubjectName.Text;
                entities.SaveChanges();

                lblMessageBox.Text = "Subject Name Changed Successfully";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSubejctCode.Text))
            {
                Subject subject          = entities.Subjects.FirstOrDefault(d => d.Code == txtSubejctCode.Text);
                if (subject != null)
                {
                    txtNewSubjectName.Text = subject.Name;
                }
                else
                {
                    lblMessageBox.Text = "Subject Code Does Not Exist";
                }
            }
        }
    }
}