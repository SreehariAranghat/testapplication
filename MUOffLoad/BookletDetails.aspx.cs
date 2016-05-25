using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class BookletDetails : System.Web.UI.Page
    {
        MUPRJEntities muEntities = new MUPRJEntities();
        INFINITY_MUEntities entities = new INFINITY_MUEntities();

        List<BookletDetail> bookletDet = new List<BookletDetail>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["BookletDetails"]  != null)
                bookletDet = (List<BookletDetail>)Session["BookletDetails"];
            if(!IsPostBack)
            {
                Session["BookletDetails"] = null;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtDate.Text) || string.IsNullOrEmpty(txtCenterCode.Text) || string.IsNullOrEmpty(txtCenterName.Text)
                || string.IsNullOrEmpty(txtSubjectCode.Text) || string.IsNullOrEmpty(txtCount.Text))
            {
                BookletDetail booklet = new BookletDetail();
                booklet.Date = txtDate.Text;
                booklet.CenterCode = txtCenterCode.Text;
                booklet.CenterName = txtCenterName.Text;
                booklet.SubjectCode = txtSubjectCode.Text;
                booklet.Count = txtCount.Text;

                bookletDet.Add(booklet);

                Session["BookletDetails"] = bookletDet;
                gridData.DataSource = bookletDet;
                gridData.DataBind();
            }
        }
    }
}