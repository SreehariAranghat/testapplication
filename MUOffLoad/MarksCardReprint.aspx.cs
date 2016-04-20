using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class MarksCardReprint : System.Web.UI.Page
    {
        INFINITY_MUEntities entities = new INFINITY_MUEntities();
        MUPRJEntities muentities     = new MUPRJEntities();

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
                    pnlMessagePanel.Visible     = false;
                    pnlStudentDetails.Visible   = true;
                    lblStudentName.Text         = student.FirstName + " " + student.LastName;
                    lblRegisterNumber.Text      = student.RegisterNumber;
                    lblCollegeName.Text         = student.College.Name.ToUpper() + " / " + student.College.Code.ToUpper();

                    pnlStudentDataUpdate.Visible = true;

                    var marksCards = muentities.MarksCards.Where(d => d.RegisterNumber == txtRegisterNumber.Text)
                                                                       .Select(d => new { UniqueNumber = d.UniqueNumber, YearSem = d.YearSem }).Distinct().ToList();

                    GridView1.DataSource = marksCards;
                    GridView1.DataBind();

                }
                else
                {
                    pnlMessagePanel.Visible      = true;
                    lblMessageBox.Text           = "Student Does Not Exist";
                    pnlStudentDetails.Visible    = false;
                    pnlStudentDataUpdate.Visible = false;
                }
            }
        }
    }
}