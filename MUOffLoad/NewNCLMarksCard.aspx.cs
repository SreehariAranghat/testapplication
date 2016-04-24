using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MUOffLoad
{
    public partial class NewNCLMarksCard : System.Web.UI.Page
    {

        MUPRJEntities muentities     = new MUPRJEntities();
        INFINITY_MUEntities entities = new INFINITY_MUEntities();

        List<MarksCard> markscard    = new List<MarksCard>();
        List<OldResultMap> resultMap = new List<OldResultMap>();

        //public float totalGPW = 0;
        //public float totalGPA = 0;
        //public float totalCredit = 0;
        //public float totalSubMax = 0;
        //public float semesterTotal = 0;
        //public float semesterPercent = 0;
        //public float semesterGPA = 0;
        //public string semesterClass = "FAIL";
        //public string semesterAlphaSign = "D";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MC"] != null)
                markscard = (List<MarksCard>)Session["MC"];

            if (Session["ResultMap"] != null)
                resultMap = (List<OldResultMap>)Session["ResultMap"];

            if(!IsPostBack)
            {
                Session["MC"] = null;
                Session["ResultMap"] = null;

            }
        }

        protected void btnAddMarks_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRegisterNumber.Text))
            {
                bool stumarks = Validation_Marks();

                if (!stumarks)
                {

                    var student = entities.Students.FirstOrDefault(s => s.RegisterNumber == txtRegisterNumber.Text);


                    MarksCard mc = new MarksCard();
                    mc.AlphaSign = "";
                    mc.CollegeCode = student.College.Code;
                    mc.CollegeName = student.College.Name;
                    mc.CourseCode = student.Cours.Code;
                    mc.CreatedDate = DateTime.Now;
                    mc.DegreeName = student.Degree.Name;
                    mc.GroupName = "Group-II : Optionals";
                    mc.GroupTotal = "";
                    mc.IsSemesterPass = "1";
                    mc.IsSubjectPass = true;
                    mc.MarksCardOrder = 0;
                    mc.MarksPercent = 0;
                    mc.PracticalGrace = "0";
                    mc.PracticalIAGrace = "0";
                    mc.PracticalIAMarks = txtPractialIAMarks.Text;
                    mc.PracticalIAMax = txtPractialIAMax.Text;
                    mc.PracticalIAMin = txtPractialIAMin.Text;
                    mc.PracticalIATotal = txtPractialIAMarks.Text;
                    mc.PracticalMarks = txtPractialMarks.Text;
                    mc.PracticalMax = txtPractialMax.Text;
                    mc.PracticalMin = txtPracticalMin.Text;
                    mc.PracticalNetTotal = txtPractialNetTotal.Text;
                    mc.PracticalTotal = txtPractialMarks.Text;
                    mc.RegisterNumber = student.RegisterNumber;
                    mc.Remarks = txtRemarks.Text;
                    mc.SemesterAlphaSign = "";
                    mc.SemesterClass = "";
                    mc.SemesterCredits = "";
                    mc.SemesterGPA = "";
                    mc.SemesterMax = 0;
                    mc.SemesterMin = 0;
                    mc.SemesterPercentage = "";
                    mc.SemesterTotal = 0;
                    mc.StudentName = student.FirstName + " " + student.LastName;
                    mc.SubGroupName = "";
                    mc.SubGroupTotal = "";
                    mc.SubjectCode = txtSubjectCode.Text;
                    mc.SubjectCredits = txtCredits.Text;
                    mc.SubjectGPA = txtGPA.Text;
                    mc.SubjectGPW = txtGPW.Text;
                    mc.SubjectMax = int.Parse(txtSubjectMax.Text);
                    mc.SubjectMin = int.Parse(txtSubjectMin.Text);
                    mc.SubjectName = txtSubjectName.Text;
                    mc.SubjectTotal = txtTotalMarks.Text;
                    mc.SubjectType = txtSubjectType.Text;
                    mc.TheoryGrace = "0";
                    mc.TheoryIAGrace = "0";
                    mc.TheoryIAMarks = txtTheoryIAMarks.Text;
                    mc.TheoryIAMax = txtTheoryIAMax.Text;
                    mc.TheoryIAMin = txtTheoryIAMin.Text;
                    mc.TheoryIATotal = txtTheoryIAMarks.Text;
                    mc.TheoryMarks = txtTheoryMarks.Text;
                    mc.TheoryMax = txtTheoryMax.Text;
                    mc.TheoryMin = txtTheoryMin.Text;
                    mc.TheoryNetTotal = txtTheoryNetTotal.Text;
                    mc.TheoryTotal = txtTheoryMarks.Text;
                    mc.TotalMarks = txtTotalMarks.Text;
                    mc.TotalGP = "0";
                    mc.TotalGPW = "0";
                    mc.UniqueNumber = student.Cours.Code + student.StudentId;
                    mc.VivaVoice = txtVivaVoice.Text;
                    mc.VivaVoiceMax = txtVivaVoiceMax.Text;
                    mc.VivaVoiceMin = txtVivaVoiceMin.Text;
                    mc.WordText = "";
                    mc.YearSem = student.YearSem;
                    mc.YearText = txtExaminationMonthYear.Text;

                    if (!markscard.Any(d => d.SubjectCode == mc.SubjectCode))
                    {
                        markscard.Add(mc);

                        Session["MC"] = markscard;

                        gridMarks.DataSource = markscard;
                        gridMarks.DataBind();
                    }
                    else
                    {
                        pnlMessagePanel.Visible = true;
                        lblMessageBox.Text = "Subject Already Added";
                    }
                }
            }
        }

        protected bool Validation_Marks()
        {
            string error = string.Empty;
            bool err = false;
            if(int.Parse(txtTheoryMarks.Text) > int.Parse(txtTheoryMax.Text))
            {
                error += "Theory Marks Shouldn't Greater Than Theory Max | ";
                err = true;
            }
            if(int.Parse(txtTheoryIAMarks.Text) > int.Parse(txtTheoryIAMax.Text))
            {
                error += "Theory IA Marks Shouln't Greater Than Theory IA Max | ";
                err = true;
            }
            if((int.Parse(txtTheoryMarks.Text) + int.Parse(txtTheoryIAMarks.Text)) != int.Parse(txtTheoryNetTotal.Text))
            {
                error += "Theory Net Total Not Matched | ";
                err = true;
            }
            if(int.Parse(txtPractialMarks.Text) > int.Parse(txtPractialMax.Text))
            {
                error += "Practical Marks Shouldn't Greater Than Practical Max | ";
                err = true;
            }
            if(int.Parse(txtPractialIAMarks.Text) > int.Parse(txtPractialIAMax.Text))
            {
                error += "Practical IA Marks Shouldn't Greater Than Practical IA Max | ";
                err = true;
            }
            if((int.Parse(txtPractialMarks.Text) + int.Parse(txtPractialIAMarks.Text)) != int.Parse(txtPractialNetTotal.Text))
            {
                error += "Practical Net Total Not Matched | ";
                err = true;
            }
            if(int.Parse(txtVivaVoice.Text) > int.Parse(txtVivaVoiceMax.Text))
            {
                error += "VivaVoice Marks Shouldn't Greater Than VivaVoice Max | ";
                err = true;
            }
            if((int.Parse(txtTheoryMarks.Text) + int.Parse(txtTheoryIAMarks.Text) + int.Parse(txtPractialMarks.Text) + int.Parse(txtPractialIAMarks.Text) + int.Parse(txtVivaVoice.Text)) != int.Parse(txtTotalMarks.Text))
            {
                error += "Total Marks Not Matched | ";
                err = true;
            }
            if(int.Parse(txtTotalMarks.Text) > int.Parse(txtSubjectMax.Text))
            {
                error += "Total Marks Shouldn't Greater Than Subject Maximum | ";
                err = true;
            }

            pnlMessagePanel.Visible = true;
            lblMessageBox.Text = error;
            return err;
        }

        protected void btnSearchStudent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRegisterNumber.Text))
            {

                var student = entities.Students.FirstOrDefault(s => s.RegisterNumber == txtRegisterNumber.Text);
                if (student != null)
                {
                    pnlMessagePanel.Visible = false;
                    pnlStudentDetails.Visible = true;
                    lblStudentName.Text = student.FirstName + " " + student.LastName;
                    lblRegisterNumber.Text = student.RegisterNumber;
                    lblCollegeName.Text = student.College.Name.ToUpper() + " / " + student.College.Code.ToUpper();

                    pnlStudentDataUpdate.Visible = true;
                    Panel1.Visible = true;
                    Panel2.Visible = true;
                }
                else
                {
                    pnlMessagePanel.Visible = true;
                    lblMessageBox.Text = "Student Does Not Exist";
                    pnlStudentDetails.Visible = false;
                    pnlStudentDataUpdate.Visible = false;
                }
            }
        }

        protected void btnSearchSubjectDetails_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtSubjectCode.Text))
            {
                var subject =  muentities.mdbSubjectPaperDetails.FirstOrDefault(d => d.SubjectPaperCode == txtSubjectCode.Text);
                if(subject != null)
                {
                    
                    txtSubjectName.Text     = subject.SubjectPaperName;
                    txtTheoryIAMax.Text     = subject.TheoryIAMax.HasValue ? subject.TheoryIAMax.Value.ToString() : "";
                    txtTheoryIAMin.Text     = subject.TheoryIAMin.HasValue ? subject.TheoryIAMin.Value.ToString() : "";
                    txtTheoryMax.Text       = subject.TheoryMax.HasValue ? subject.TheoryMax.Value.ToString() : "";
                    txtTheoryMin.Text       = subject.TheoryMin.HasValue ? subject.TheoryMin.Value.ToString() : "";
                    txtVivaVoiceMax.Text    = subject.VivaVoice.HasValue ? subject.VivaVoice.Value.ToString() : "";
                    txtVivaVoiceMin.Text    = subject.VivaVoiceMin.HasValue ? subject.VivaVoiceMin.Value.ToString() : "";
                    txtPractialIAMax.Text   = subject.PracticalIAMax.HasValue ? subject.PracticalIAMax.Value.ToString() : "";
                    txtPractialIAMin.Text   = subject.PracticalIAMin.HasValue ? subject.PracticalIAMin.Value.ToString() : "";
                    txtPractialMax.Text     = subject.PracticalMax.HasValue ? subject.PracticalMax.Value.ToString() : "";
                    txtPracticalMin.Text    = subject.PracticalMin.HasValue ? subject.PracticalMin.Value.ToString() : "";
                    txtSubjectMin.Text      = subject.TotalPassMarks.HasValue ? subject.TotalPassMarks.Value.ToString() : "";
                    txtSubjectType.Text     = subject.SubjectPaperType.ToString();

                    int subjectMax = 0;

                    subjectMax +=  (int)(subject.TheoryIAMax.HasValue? subject.TheoryIAMax.Value : 0);
                    subjectMax += (int)(subject.TheoryMax.HasValue ? subject.TheoryMax.Value : 0);
                    subjectMax += (int)(subject.VivaVoice.HasValue ? subject.VivaVoice.Value : 0);
                    subjectMax += (int)(subject.PracticalIAMax.HasValue ? subject.PracticalIAMax.Value : 0);
                    subjectMax += (int)(subject.PracticalMax.HasValue ? subject.PracticalMax.Value : 0);

                    txtSubjectMax.Text = subjectMax.ToString();

                }
            }
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            int totalMarks = 0;
            int marks = 0;

            if (int.TryParse(txtTheoryMarks.Text, out marks))
                totalMarks += marks;

            if (int.TryParse(txtTheoryIAMarks.Text, out marks))
                totalMarks += marks;

            if (int.TryParse(txtPractialMarks.Text, out marks))
                totalMarks += marks;

            if (int.TryParse(txtPractialIAMarks.Text, out marks))
                totalMarks += marks;

            if (int.TryParse(txtVivaVoice.Text, out marks))
                totalMarks += marks;

            float subjecMax = 0;

            if (float.TryParse(txtSubjectMax.Text, out subjecMax))
            {
                float ap = ((((float)totalMarks / (float)subjecMax)) * 100);

                string grade = "D";
                int GPA = 2;

                if (ap < 35)
                {
                    GPA = 2;
                    grade = "D";

                }

                if (ap >= 35 && ap < 50)
                {
                    GPA = 3;
                    grade = "C";

                }

                if (ap >= 50 && ap < 55)
                {
                    GPA = 4;
                    grade = "B";
                }

                if (ap >= 55 && ap < 60)
                {
                    GPA = 5;
                    grade = "B+";
                }

                if (ap >= 60 && ap < 70)
                {
                    GPA = 6;
                    grade = "A";
                }

                if (ap >= 70 && ap < 80)
                {
                    GPA = 7;
                    grade = "A+";
                }

                if (ap >= 80 && ap < 90)
                {
                    GPA = 8;
                    grade = "A++";
                }

                if (ap >= 90 && ap <= 100)
                {
                    GPA = 9;
                    grade = "O";
                }


                float totalCredits = 0;
                if (subjecMax == 300)
                    totalCredits = 6;
                if (subjecMax == 150)
                    totalCredits = 3;
                if (subjecMax == 125)
                    totalCredits = 2.5F;
                if (subjecMax == 100)
                    totalCredits = 2;
                if (subjecMax == 75)
                    totalCredits = 1.5F;
                if (subjecMax == 50)
                    totalCredits = 1;

                float GPW = GPA * totalCredits;

                txtTotalMarks.Text  = ((int)totalMarks).ToString();
                txtCredits.Text     = totalCredits.ToString();
                txtGPA.Text         = GPA.ToString();
                txtGPW.Text         = GPW.ToString();

                //semesterTotal += totalMarks;
                //totalGPW += GPW;
                //totalGPA += GPA;
                //totalCredit += totalCredits;
                //totalSubMax += subjecMax;

                //txtNetCredits.Text = totalCredit.ToString();
                //txtNetSemesterTotal.Text = semesterTotal.ToString();
                //txtNetGPA.Text = totalGPA.ToString();
                //txtNetGPW.Text = totalGPW.ToString();
                //txtNetSemesterMax.Text = totalSubMax.ToString();

                //semesterPercent = ((((float)semesterTotal / (float)totalSubMax)) * 100);
                //semesterGPA = ((((float)totalGPW / (float)totalCredit)) * 100);

                //float semesterAggragate = semesterPercent;

                //if (semesterAggragate < 35)
                //{
                //    semesterAlphaSign = "D";

                //}

                //if (semesterAggragate >= 35 && semesterAggragate < 50)
                //{
                //    semesterAlphaSign = "C";

                //}

                //if (semesterAggragate >= 50 && semesterAggragate < 55)
                //{
                //    semesterAlphaSign = "B";
                //}

                //if (semesterAggragate >= 55 && semesterAggragate < 60)
                //{
                //    semesterAlphaSign = "B+";
                //}

                //if (semesterAggragate >= 60 && semesterAggragate < 70)
                //{
                //    semesterAlphaSign = "A";
                //}

                //if (semesterAggragate >= 70 && semesterAggragate < 80)
                //{
                //    semesterAlphaSign = "A+";
                //}

                //if (semesterAggragate >= 80 && semesterAggragate < 90)
                //{

                //    semesterAlphaSign = "A++";
                //}

                //if (semesterAggragate >= 90 && semesterAggragate < 100)
                //{
                //    semesterAlphaSign = "O";
                //}

                //txtSemesterAlphaSign.Text = semesterAlphaSign.ToString();

                //if (semesterAggragate >= 70)
                //    semesterClass = "FIRST CLASS WITH DISTINCTION";
                //if (semesterAggragate < 70 && semesterAggragate >= 60)
                //    semesterClass = "FIRST CLASS";
                //if (semesterAggragate < 60 && semesterAggragate >= 55)
                //    semesterClass = "HIGH SECOND CLASS";
                //if (semesterAggragate < 55 && semesterAggragate >= 50)
                //    semesterClass = "SECOND CLASS";
                //if (semesterAggragate < 50 && semesterAggragate >= 40)
                //    semesterClass = "PASS CLASS";

                //txtNetSemesterClass.Text = semesterClass.ToString();
                //txtNetSemesterPercentage.Text = semesterPercent.ToString();
                //txtNetSemesterCredits.Text = "0";

            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if(ddlSemester.SelectedIndex > 0)
            {
                int semester = int.Parse(ddlSemester.SelectedValue);

                if(!resultMap.Any(s => s.YearSem == semester))
                {
                    var student = entities.Students.FirstOrDefault(s => s.RegisterNumber == txtRegisterNumber.Text);
                    OldResultMap m = new OldResultMap();
                    m.CourseCode = student.Cours.Code;
                    m.LastExaminationBatchId = 0;
                    m.RegisterNumber = student.RegisterNumber;
                    m.Remarks = txtPreSemRemarks.Text;
                    m.SemesterCredits = int.Parse(txtSemesterCredits.Text);
                    m.SemesterGPA = decimal.Parse(txtSemesterGPA.Text);
                    m.SemesterMarks = int.Parse(txtSemesterMarks.Text);
                    m.SemesterWeightage = decimal.Parse(txtSemesterWeightage.Text);
                    m.SemsterMax = int.Parse(txtSemesterMax.Text);
                    m.YearSem = semester;

                    resultMap.Add(m);

                    Session["ResultMap"] = resultMap;
                    gridOldResultMap.DataSource = resultMap;
                    gridOldResultMap.DataBind();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            var existmarkscard = muentities.MarksCards.Where(d => d.RegisterNumber == txtRegisterNumber.Text).ToList();

            foreach(var v in existmarkscard)
            {
                v.RegisterNumber += v.RegisterNumber + "_OLD";

            }


            var oldexistmarkscard = muentities.OldResultMaps.Where(d => d.RegisterNumber == txtRegisterNumber.Text).ToList();

            foreach (var v in oldexistmarkscard)
            {
                v.RegisterNumber += v.RegisterNumber + "_OLD";

            }

            muentities.SaveChanges();

            foreach(OldResultMap m in resultMap)
            {
                muentities.OldResultMaps.Add(m);
            }

            var student = entities.Students.FirstOrDefault(s => s.RegisterNumber == txtRegisterNumber.Text);

            string uniqueNumber = student.Cours.Code + Guid.NewGuid().ToString().ToUpper().Replace("-","");

           foreach (MarksCard m in markscard)
           {
                m.UniqueNumber       = uniqueNumber.Replace("-","");
                m.SemesterAlphaSign  = ddlSemesterAlphaSign.SelectedValue.ToString();
                m.SemesterClass      = ddlSelectSemesterClass.SelectedValue.ToString();
                m.SemesterCredits    = txtNetCredits.Text;
                m.SemesterMax        = int.Parse(txtNetSemesterMax.Text);
                m.SemesterPercentage = txtNetSemesterPercentage.Text;
                m.SemesterTotal      = int.Parse(txtNetSemesterTotal.Text);
                m.TotalGP            = txtNetGPA.Text;
                m.TotalGPW           = txtNetGPW.Text;
                m.SemesterGPA        = txtSemGPA.Text;
                m.ExaminationYear    = txtExaminationYear.Text;
                m.PrintDate          = txtPrintDate.Text;
                muentities.MarksCards.Add(m);
           }

            muentities.SaveChanges();

            Response.Redirect("GenerateMarksCard.aspx?mid=" + uniqueNumber);
        }
    }
}