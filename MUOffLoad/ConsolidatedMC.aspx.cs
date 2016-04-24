using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class ConsolidatedMC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        MUPRJEntities entities = new MUPRJEntities();

        protected void btnSearchStudent_Click(object sender, EventArgs e)
        {

             MarksCardCreator.CreateMarksCard(txtRegisterNumber.Text, int.Parse(ddlSemester.SelectedValue));
            int yearSem = int.Parse(ddlSemester.SelectedValue);
            string regno = txtRegisterNumber.Text;
            string uid = entities.MarksCards.First(d => d.RegisterNumber == regno && d.YearSem == yearSem).UniqueNumber;
            Response.Redirect("GenerateMarksCard.aspx?console=y&mid="+uid);

        }
    }
}