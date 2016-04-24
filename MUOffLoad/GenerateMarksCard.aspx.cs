using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class GenerateMarksCard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request["mid"] != null)
            {
                string uniquenumber = Request["mid"];
                string path = Server.MapPath(".") + "\\Downloads";

                if (Request["console"] != null)
                {
                    string filename = MarksCardGenerator.GenerateConsolidated(uniquenumber, path);
                    Response.Redirect("/Downloads/" + filename);
                }
                else
                {
                    string filename = MarksCardGenerator.Generate(uniquenumber, path);
                    Response.Redirect("/Downloads/" + filename);
                }
            }
        }
    }
}