using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUOffLoad
{
    public enum ImportType
    {
        Regular
        ,Consolidated
    }
    
    public class MarksCardCreator
    {
       static  MUPRJEntities entities               = new MUPRJEntities();
        static INFINITY_MUEntities infinityentities = new INFINITY_MUEntities();
      
        static int semesterTotal = 0;
        public static string CreateMarksCard(string registernumber,int yearsem)
        {
            ImportData(registernumber, yearsem);

            AllocateUniqueNumbers(registernumber, yearsem);
            //UpdateSubjectGPA(registernumber, yearsem);

            UpdateAbsentes(registernumber, yearsem);
            Console.WriteLine("Completed Absentee List");
            UpdateSemesterGPW(registernumber, yearsem);
            Console.WriteLine("Completed GPW");
            UpdateFailResults(registernumber, yearsem);
            Console.WriteLine("Completed Fail Results");
            //UpdateRepeateResults(registernumber, yearsem);

            UpdateAbsentes(registernumber, yearsem);

            var mcnumber = entities.MarksCards.First(d => d.RegisterNumber == registernumber && d.YearSem == yearsem);
            return mcnumber.UniqueNumber;
        }

        public static void ImportData(string regno, int yearsem)
        {
            var exist = entities.MarksCards.Where(e => e.RegisterNumber == regno && e.YearSem == yearsem);

            foreach(var r in exist)
            {
                r.RegisterNumber += "_OLD";
            }
            entities.SaveChanges();

            var student = infinityentities.Students.FirstOrDefault(d => d.RegisterNumber == regno);
            var did = infinityentities.Degrees.FirstOrDefault(i => i.DegreeId == student.DegreeId);
            if (student != null)
            {

                var UGres = entities.UGFinalResultTables.Where(r => r.RegisterNumber == regno && r.YearSem == yearsem).ToList();

                foreach (var r in UGres)
                {
                    var subject = entities.mdbSubjectPaperDetails.FirstOrDefault(d => d.SubjectPaperCode == r.SubjectCode);

                    var max = entities.DegreeMaxes.FirstOrDefault(x => x.DegreeName == did.Name && x.YearSem == yearsem);
                    var grp = entities.GroupNames.FirstOrDefault(g => g.SubjectName == r.SubjectName);

                    MarksCard m = new MarksCard();
                    m.RegisterNumber = r.RegisterNumber;
                    m.StudentName = student.FirstName;
                    m.CollegeName = student.College.Name;
                    m.CollegeCode = student.College.Code;
                    m.DegreeName = r.DegreeName;
                    m.YearSem = r.YearSem;
                    m.GroupName = grp.GroupName1;
                    m.SubGroupName = r.SubGroupName;
                    m.SubjectName = r.SubjectName;
                    m.SubjectCode = r.SubjectCode;
                    m.TheoryGrace = r.TheoryGrace;
                    m.TheoryMarks = r.TheoryMarks;
                    m.TheoryMin = r.TheoryMin;
                    m.TheoryMax = r.TheoryMax;
                    m.TheoryTotal = r.TheoryTotal;
                    m.TheoryIAMarks = r.TheoryIAMarks;
                    m.TheoryIAGrace = r.TheoryIAGrace;
                    m.TheoryIAMin = r.TheoryIAMin;
                    m.TheoryIAMax = r.TheoryIAMax;
                    m.TheoryIATotal = r.TheoryIATotal;
                    m.TheoryNetTotal = r.TheoryNetTotal;
                    m.PracticalMarks = r.PracticalMarks;
                    m.PracticalGrace = r.PracticalGrace;
                    m.PracticalMin = r.PracticalMin;
                    m.PracticalMax = r.PracticalMax;
                    m.PracticalTotal = r.PracticalTotal;
                    m.PracticalIAMarks = r.PracticalIAMarks;
                    m.PracticalIAGrace = r.PracticalIAGrace;
                    m.PracticalIAMin = r.PracticalIAMin;
                    m.PracticalIAMax = r.PracticalIAMax;
                    m.PracticalIATotal = r.PracticalIATotal;
                    m.PracticalNetTotal = r.PracticalNetTotal;
                    m.VivaVoice = r.VivaVoice;
                    m.VivaVoiceMin = r.VivaVoiceMin;
                    m.VivaVoiceMax = r.VivaVoiceMax;
                    m.SubjectTotal = r.SubjectTotal;
                    m.SubGroupTotal = r.SubGroupTotal;
                    m.GroupTotal = r.GroupTotal;
                    m.TotalMarks = r.TotalMarks;
                    m.SubjectMin = r.SubjectMin;
                    m.SubjectMax = r.SubjectMax;
                    m.SubjectCredits = r.SubjectCredits.ToString();
                    m.SubjectGPA = r.SubjectGPA.ToString();
                    m.SubjectGPW = r.SubjectGPW.ToString();
                    m.CreatedDate = r.CreatedDate;
                    m.IsSubjectPass = r.IsSubjectPass;
                    m.MarksPercent = r.MarksPercent;
                    m.AlphaSign = r.AlphaSign;
                    m.SemesterCredits = r.SemesterCredits.ToString();
                    m.SemesterMax = (int)max.SemesterMax;
                    m.SemesterMin = r.SemesterMin;
                    m.SemesterTotal = r.SemesterTotal;
                    m.SemesterPercentage = r.SemesterPercentage.ToString();
                    m.SemesterGPA = r.SemesterGPA.ToString();
                    m.SemesterClass = r.SemesterClass;
                    m.SemesterAlphaSign = r.SemesterAlphaSign;
                    m.IsSemesterPass = r.IsSemesterPass.ToString();
                    m.Remarks = r.Remarks;
                    m.YearText = r.YearText;
                    m.CourseCode = student.Cours.Code;
                    m.SubjectType = subject.SubjectPaperType;
                    m.MarksCardOrder = (int)grp.MarksCardOrder;

                    entities.MarksCards.Add(m);
                    semesterTotal = (int)max.SemesterMax;
                    entities.SaveChanges();
                }
            }

    
        }

        public static void UpdateSubjectGPA(string registernumber, int yearsem)
        {
            var entities = new MUPRJEntities();

            var distictRegisterNumbers = entities.MarksCards.Where(d => d.RegisterNumber == registernumber && d.YearSem == yearsem)
                                                          .Select(d => d.RegisterNumber).Distinct();
            int i = 0;
            foreach (var regno in distictRegisterNumbers)
            {
                Console.WriteLine((i++) + "|" + regno);
                foreach (var items in entities.MarksCards.Where(d => d.RegisterNumber == regno && d.YearSem == yearsem))
                {
                    int subjectTotal = 0;


                    if (int.TryParse(items.TotalMarks, out subjectTotal))
                    {
                        float ap = subjectTotal > 0 ? ((float)subjectTotal / (float)items.SubjectMax) * 100 : 0;

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

                        items.SubjectGPA = GPA.ToString();
                        items.SubjectGPW = (int.Parse(items.SubjectCredits) * GPA).ToString();

                        //if (!subject)
                        //{
                        //    GPA = 0;
                        //    grade = "";

                        //}
                    }
                }
            }

            entities.SaveChanges();
        }
        public static void UpdateSemesterGPW(string registernumber, int yearsem)
        {
            var entities = new MUPRJEntities();

            var distictRegisterNumbers = entities.MarksCards.Where(d => d.RegisterNumber == registernumber && d.YearSem == yearsem)
                                                          .Select(d => d.RegisterNumber).Distinct().ToList();

            int i = 0;
            foreach (var regno in distictRegisterNumbers)
            {
                Console.WriteLine((i++) + "|" + regno);
                entities = new MUPRJEntities();

                float totalGPW = 0;
                float totalGP = 0;
                float totalSemester = 0;
                float totalPercentage = 0.0F;
                float totalCredits = 0.0F;

                foreach (var items in entities.MarksCards.Where(d => d.RegisterNumber == regno && d.YearSem == yearsem))
                {
                    int subjecttotal;
                    float subjectCredits;
                    float subjectGP;
                    float subjectGPW;


                    if (int.TryParse(items.TotalMarks, out subjecttotal))
                    {
                        totalSemester += subjecttotal;
                    }

                    if (float.TryParse(items.SubjectCredits, out subjectCredits))
                    {
                        totalCredits += subjectCredits;
                    }

                    if (float.TryParse(items.SubjectGPA, out subjectGP))
                    {
                        totalGP += subjectGP;
                    }

                    if (float.TryParse(items.SubjectGPW, out subjectGPW))
                    {
                        totalGPW += subjectGPW;
                    }
                }

                foreach (var items in entities.MarksCards.Where(d => d.RegisterNumber == regno && d.YearSem == yearsem))
                {
                    items.TotalGPW = totalGPW.ToString();
                    items.TotalGP = totalGP.ToString();
                    items.SemesterTotal = (int)totalSemester;
                    items.SemesterCredits = totalCredits.ToString();
                    items.SemesterMax = semesterTotal;
                    items.SemesterGPA = (totalCredits > 0 ? (totalGPW / totalCredits) : 0.0).ToString();

                    items.SemesterPercentage = Math.Round(((totalSemester / (float)semesterTotal) * 100), 2).ToString();

                    float semesterAggragate = ((totalSemester / (float)semesterTotal) * 100);
                    string semesterAlphaSign = "D";

                    float semesterGPA = 0.0F;

                    if (float.TryParse(items.SemesterGPA, out semesterGPA))
                    {
                        items.SemesterGPA = Math.Round(semesterGPA, 2).ToString();
                    }


                    if (semesterAggragate < 35)
                    {
                        semesterAlphaSign = "D";

                    }

                    if (semesterAggragate >= 35 && semesterAggragate < 50)
                    {
                        semesterAlphaSign = "C";

                    }

                    if (semesterAggragate >= 50 && semesterAggragate < 55)
                    {
                        semesterAlphaSign = "B";
                    }

                    if (semesterAggragate >= 55 && semesterAggragate < 60)
                    {
                        semesterAlphaSign = "B+";
                    }

                    if (semesterAggragate >= 60 && semesterAggragate < 70)
                    {
                        semesterAlphaSign = "A";
                    }

                    if (semesterAggragate >= 70 && semesterAggragate < 80)
                    {
                        semesterAlphaSign = "A+";
                    }

                    if (semesterAggragate >= 80 && semesterAggragate < 90)
                    {

                        semesterAlphaSign = "A++";
                    }

                    if (semesterAggragate >= 90 && semesterAggragate < 100)
                    {
                        semesterAlphaSign = "O";
                    }

                    items.SemesterAlphaSign = semesterAlphaSign;
                }

                entities.SaveChanges();
            }




        }

        public static void UpdateFailResults(string registernumber, int yearsem)
        {
            var entities = new MUPRJEntities();

            var distictRegisterNumbers = entities.MarksCards.Where(d => d.RegisterNumber == registernumber && d.YearSem == yearsem && d.Remarks == "FAIL")
                                                          .Select(d => d.RegisterNumber).Distinct();
            int i = 0;
            foreach (var regno in distictRegisterNumbers)
            {
                Console.WriteLine((i++) + "|" + regno);
                foreach (var items in entities.MarksCards.Where(d => d.RegisterNumber == regno
                            && d.YearSem == yearsem
                            && d.Remarks == "FAIL"
                            ))
                {
                    items.SubjectCredits = "-";
                    items.SubjectGPA = "-";
                    items.SubjectGPW = "-";
                }

                foreach (var items in entities.MarksCards.Where(d => d.RegisterNumber == regno
                            && d.YearSem == yearsem
                            ))
                {
                    items.SemesterPercentage = "-";
                    items.SemesterCredits = "-";
                    items.SemesterGPA = "-";
                    items.TotalGP = "-";
                    items.TotalGPW = "-";
                    items.SemesterAlphaSign = "-";
                }
            }

            entities.SaveChanges();
        }

        public static void UpdateRepeateResults(string registernumber, int yearsem)
        {
            var entities = new MUPRJEntities();

            var distictRegisterNumbers = entities.MarksCards.Where(d => d.RegisterNumber == registernumber && d.YearSem == yearsem && d.YearText.Length > 0)
                                                          .Select(d => d.RegisterNumber).Distinct().ToList();
            int i = 0;
            foreach (var regno in distictRegisterNumbers)
            {
                Console.WriteLine((i++) + "|" + regno);
                entities = new MUPRJEntities();

                foreach (var items in entities.MarksCards.Where(d => d.RegisterNumber == regno
                            && d.YearSem == yearsem
                            && d.YearText.Length > 0
                            ))
                {
                    items.TheoryNetTotal = "-";
                    items.TheoryTotal = "-";
                    items.TheoryIATotal = "-";

                    items.PracticalNetTotal = "-";
                    items.PracticalTotal = "-";
                    items.PracticalIATotal = "-";
                    items.VivaVoice = "-";
                    items.Remarks = "-";

                    items.TotalMarks = "-";
                    items.SubjectCredits = "-";
                    items.SubjectGPA = "-";
                    items.SubjectGPW = "-";
                }



                foreach (var items in entities.MarksCards.Where(d => d.RegisterNumber == regno
                            && d.YearSem == yearsem
                            && d.IsSemesterPass == "0"
                            ))
                {
                    items.SemesterPercentage = "-";
                    items.SemesterCredits = "-";
                    items.SemesterGPA = "-";
                    items.TotalGP = "-";
                    items.TotalGPW = "-";
                    items.SemesterAlphaSign = "-";
                }

                entities.SaveChanges();
            }


        }

        public static void UpdateAbsentes(string registernumber, int yearsem)
        {
            var entities = new MUPRJEntities();

            var distictRegisterNumbers = entities.MarksCards.Where(d => d.RegisterNumber == registernumber && d.YearSem == yearsem && d.Remarks == "ABSENT")
                                                          .Select(d => d.RegisterNumber).Distinct();

            int i = 0;
            foreach (var regno in distictRegisterNumbers)
            {
                Console.WriteLine((i++) + "|" + regno);
                entities = new MUPRJEntities();

                foreach (var items in entities.MarksCards.Where(d => d.RegisterNumber == regno
                            && d.YearSem == yearsem
                            && d.Remarks == "ABSENT"
                            ))
                {
                    int subjectMarks = 0;
                    int currentMarks = 0;

                    if (int.TryParse(items.TheoryTotal, out currentMarks))
                        subjectMarks += currentMarks;

                    if (int.TryParse(items.TheoryIATotal, out currentMarks))
                        subjectMarks += currentMarks;

                    if (int.TryParse(items.PracticalTotal, out currentMarks))
                        subjectMarks += currentMarks;

                    if (int.TryParse(items.PracticalIATotal, out currentMarks))
                        subjectMarks += currentMarks;

                    items.SubjectCredits = "-";
                    items.SubjectGPA = "-";
                    items.SubjectGPW = "-";

                    items.TotalMarks = subjectMarks.ToString();
                }

                foreach (var items in entities.MarksCards.Where(d => d.RegisterNumber == regno
                           && d.YearSem == yearsem
                           ))
                {
                    items.SemesterPercentage = "-";
                    items.SemesterCredits = "-";
                    items.SemesterGPA = "-";
                    items.TotalGP = "-";
                    items.TotalGPW = "-";
                    items.SemesterAlphaSign = "-";

                }

                entities.SaveChanges();
            }


        }

        public static void AllocateUniqueNumbers(string registernumber, int yearsem)
        {
            var entities = new MUPRJEntities();

            var distictRegisterNumbers = entities.MarksCards.Where(d => d.RegisterNumber == registernumber && d.YearSem == yearsem)
                                                          .Select(d => d.RegisterNumber).Distinct();

            int i = 0;
            foreach (var regno in distictRegisterNumbers)
            {
                entities = new MUPRJEntities();
                string markscard = Guid.NewGuid().ToString();
                Console.WriteLine((i++) + "|" + regno + "|" + markscard);
                foreach (var items in entities.MarksCards.Where(d => d.RegisterNumber == regno  && d.YearSem == yearsem))
                {
                    items.UniqueNumber = items.CourseCode + "2015" + markscard.Replace("-", "").ToUpper();
                }

                entities.SaveChanges();
            }


        }
    }
}