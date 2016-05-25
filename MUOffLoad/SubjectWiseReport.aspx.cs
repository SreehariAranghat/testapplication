using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Web.Security;

namespace MUOffLoad
{
    public partial class SubjectWiseReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rptExam.ServerReport.DisplayName = "ExamRegSubjectCount.rdl";
                rptExam.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerUrl"]);
                rptExam.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReportServerFolder"] + "ExamRegSubjectCount";

                NetworkCredential MyCred = new NetworkCredential(ConfigurationManager.AppSettings["ReportServerUserName"], ConfigurationManager.AppSettings["ReportServerPassword"], ConfigurationManager.AppSettings["ReportServerDomain"]);

                CredentialCache myCache = new CredentialCache();
                myCache.Add(new Uri(ConfigurationManager.AppSettings["ReportServerUrl"]), "Basic", MyCred);

                rptExam.LocalReport.Refresh();
            }
        }
    }
}