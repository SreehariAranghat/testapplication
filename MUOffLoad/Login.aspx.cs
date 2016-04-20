using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class Login : System.Web.UI.Page
    {
        MUPRJEntities entities = new MUPRJEntities();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            if(!(string.IsNullOrEmpty(txtUserName.Text)
                || string.IsNullOrEmpty(txtPassword.Text)))
            {
                MUOffLoad.User user = entities.Users.FirstOrDefault(d => d.UserName == txtUserName.Text
                                                                   && d.Password == txtPassword.Text);
                
                if(user != null)
                {
                    Session["LoggedInUser"] = user;
                    FormsAuthentication.RedirectFromLoginPage(user.UserId.ToString(), true);
                }
                else
                {
                    lblLoginMessageBox.Text = "Authentication Failed";
                }

            }
            else
            {
                lblLoginMessageBox.Text = "Username or Password cannot be blank";
            }
        }
    }
}