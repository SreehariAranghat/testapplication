using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedInUser"] != null)
            {
                MUOffLoad.User u = (MUOffLoad.User)Session["LoggedInUser"];
                if (u.UserName == "SysAdmin")
                {
                    pnlAdmin.Visible = true;
                    pnlExaminationDetails.Visible = true;
                }
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }
    }
}