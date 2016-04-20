using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class AddUser : System.Web.UI.Page
    {
        MUPRJEntities muentities = new MUPRJEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedInUser"] != null)
            {
                MUOffLoad.User u = (MUOffLoad.User)Session["LoggedInUser"];
                if (u.UserName == "SysAdmin")
                {

                }
                else
                {
                    FormsAuthentication.RedirectToLoginPage();
                }

            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }
        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            if(!(string.IsNullOrEmpty(txtFirstName.Text) 
                || string.IsNullOrEmpty(txtUserName.Text) 
                || string.IsNullOrEmpty(txtPassword.Text)))
            {
                MUOffLoad.User user = new MUOffLoad.User();
                user.CreatedDate    = DateTime.Now;
                user.FirstName      = txtFirstName.Text;
                user.Password       = txtPassword.Text;
                user.UserName       = txtUserName.Text;

                muentities.Users.Add(user);
                muentities.SaveChanges();
                pnlMessagePanel.Visible = true;
                lblMessageBox.Text = "User added successfully";
            }
            else
            {
                pnlMessagePanel.Visible = true;
                lblMessageBox.Text = "All fields are mandatory";
            }
        }
    }
}