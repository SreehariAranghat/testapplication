using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace MUOffLoad
{
    public static class MarksCardGenerator
    {

        public static string Generate(string uniquenumber,string path)
        {
            string filename = Guid.NewGuid().ToString() + ".pdf";

            var entities    = new MUPRJEntities();
            var markscard   = entities.MarksCards.FirstOrDefault(d => d.UniqueNumber == uniquenumber);

            if (markscard != null)
            {
                PdfDocument document = new PdfDocument();

                if (markscard.YearSem != 6)
                {
                    if ((markscard.DegreeName.ToUpper() == "BACHELOR OF SCIENCE" || markscard.DegreeName.ToUpper() == "BACHELOR OF ARTS") && markscard.YearSem == 5)
                    {
                        GroupFormat(markscard.RegisterNumber, markscard.YearSem.Value, ref document);
                        document.Save(path + "\\" + filename);
                    }
                    else
                    {
                        GroupedFormat(markscard.RegisterNumber, markscard.YearSem.Value, ref document);
                        document.Save(path + "\\" + filename);
                    }
                }
                else
                {
                    if ((markscard.DegreeName == "BACHELOR OF SCIENCE" || markscard.DegreeName == "BACHELOR OF ARTS"))
                    {
                        GroupFormatSEM6(markscard.RegisterNumber, markscard.YearSem.Value, ref document);
                    }
                    else
                    {
                        GroupedSEM6Format(markscard.RegisterNumber, markscard.YearSem.Value, ref document);
                        document.Save(path + "\\" + filename);
                    }
                }
            }

            return filename;
        }

        public static string GenerateConsolidated(string uniquenumber, string path)
        {
            string filename = Guid.NewGuid().ToString() + ".pdf";

            var entities = new MUPRJEntities();
            var markscard = entities.MarksCards.FirstOrDefault(d => d.UniqueNumber == uniquenumber);

            if (markscard != null)
            {
                PdfDocument document = new PdfDocument();
                ConsolidatedFormat(markscard.RegisterNumber, markscard.YearSem.Value, ref document);
                document.Save(path + "\\" + filename);
            }

            return filename;
        }

        private static void GroupFormat(string regno, int semester, ref PdfDocument document)
        {
            var entiteis = new MUPRJEntities();

            var studentList = entiteis.MarksCards.Where(d => d.RegisterNumber == regno && d.YearSem == semester).OrderBy(d => d.MarksCardOrder).ThenBy(d => d.SubjectCode).ToList();
            var datarow = studentList.First();

            PdfPage page = document.AddPage();
            page.Size = PdfSharp.PageSize.Legal;

            XFont normal = new XFont("Arial", 9, XFontStyle.Regular);
            XFont normalbold = new XFont("Arial", 9, XFontStyle.Bold);

            XFont normalex = new XFont("Arial", 10, XFontStyle.Regular);
            XFont normalboldex = new XFont("Arial", 10, XFontStyle.Bold);

            XFont normalitalic = new XFont("Arial", 9, XFontStyle.BoldItalic);
            XFont title = new XFont("Arial", 12, XFontStyle.Bold);

            using (XGraphics gfx = XGraphics.FromPdfPage(page))
            {
                int xstart = 10;
                int ystart = 10;

                int drawablewidth = (int)page.Width - 10;
                int drawableheight = (int)page.Height - 10;

                int middlepoint = drawablewidth / 2;

                gfx.DrawString("No." + datarow.UniqueNumber, normalbold, XBrushes.Black, xstart, ystart);

                ystart += 150;

                XSize s = gfx.MeasureString(datarow.DegreeName.ToUpper(), title);
                float middle = (float)s.Width / 2;

                gfx.DrawString(datarow.DegreeName.ToUpper(), title, XBrushes.CornflowerBlue, middlepoint - middle, ystart + 30);

                if (semester == 2)
                {
                    XSize size = gfx.MeasureString(" Credit Based Second Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Second Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }

                if (semester == 4)
                {
                    XSize size = gfx.MeasureString(" Credit Based Fourth Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Fourth Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }
                if (semester == 5)
                {
                    XSize size = gfx.MeasureString(" Credit Based Fifth Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Fifth Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }

                gfx.DrawString(datarow.PrintDate != null ? datarow.PrintDate.ToString() : "29-Apr-2016", normalboldex, XBrushes.Black, drawablewidth - 100, ystart + 5);



                using (var wc = new WebClient())
                {
                    using (var imgStream = new MemoryStream(wc.DownloadData("http://attristech.com:9797/muimages/" + datarow.RegisterNumber + "_p.jpg")))
                    {
                        using (var objImage = Image.FromStream(imgStream))
                        {
                            XImage image = XImage.FromGdiPlusImage(resizeImage(150, 180, objImage));
                            gfx.DrawImage(image, xstart - 15, ystart - 15);
                        }
                    }
                }

                ystart += 120;

                gfx.DrawString("Name : ", normalex, XBrushes.Black, xstart, ystart);
                gfx.DrawString(datarow.StudentName, normalboldex, XBrushes.Black, xstart + 35, ystart);

                gfx.DrawString("Register No. : ", normalex, XBrushes.Black, drawablewidth - 115, ystart);
                gfx.DrawString(datarow.RegisterNumber, normalboldex, XBrushes.Black, drawablewidth - 50, ystart);

                ystart += 10;
                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                int startx = xstart;
                int starty = ystart;

                ystart += 15;

                gfx.DrawString("Subject", normalbold, XBrushes.Black, 150, ystart);
                gfx.DrawString("Marks", normalbold, XBrushes.Black, 360, ystart);
                gfx.DrawString("Credit Calculations", normalbold, XBrushes.Black, 450, ystart);
                gfx.DrawString("Result", normalbold, XBrushes.Black, drawablewidth - 45, ystart);

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                ystart += 20;

                gfx.DrawString("Name", normal, XBrushes.Black, xstart + 100, ystart);
                gfx.DrawString("Code", normal, XBrushes.Black, 260, ystart);

                gfx.DrawString("Max", normal, XBrushes.Black, 345, ystart);
                gfx.DrawString("Min", normal, XBrushes.Black, 380, ystart);
                gfx.DrawString("Obtained", normal, XBrushes.Black, 405, ystart);

                gfx.DrawString("Cr", normal, XBrushes.Black, 455, ystart);
                gfx.DrawString("GP", normal, XBrushes.Black, 485, ystart);
                gfx.DrawString("GPW", normal, XBrushes.Black, 510, ystart);

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                ystart += 20;

                var groups = studentList.OrderBy(d => d.MarksCardOrder).ThenBy(d => d.GroupName).Select(d => d.GroupName).Distinct();


                int groupno = 0;
                float netcredits = 0;
                float netgp = 0;
                float netgpw = 0;


                foreach (var g in groups)
                {
                    int i = 0;
                    int externalTotal = 0;
                    int theorynettotal = 0;
                    int theorytotal = 0;

                    float totalcredites = 0;
                    float totalgpw = 0;
                    float totalgp = 0;

                    var grouprow = studentList.Where(d => d.GroupName == g).First();
                    var haspractical = studentList.Where(d => d.GroupName == g).Any(d => d.SubjectType == "PR");
                    int tottheorymax = 0;

                    foreach (var item in studentList.Where(d => d.GroupName == g && d.SubjectType == "TH").OrderBy(d => d.SubjectCode))
                    {
                        tottheorymax += int.Parse(item.TheoryMax);

                        if (i == 0)
                        {
                            if (groupno == 0)
                            {
                                gfx.DrawString("Group II : Optionals", normalbold, XBrushes.Black, xstart + 10, ystart);
                            }

                            gfx.DrawString(g, normalbold, XBrushes.Black, xstart + 10, ystart + 8);

                            if (groupno > 0)
                            {
                                gfx.DrawLine(XPens.Black, xstart, ystart - 5, drawablewidth, ystart - 5);
                            }


                        }


                        i++;

                        if (item.SubjectName == "Human Rights Gender Equity and Environmental Studies")
                        {
                            gfx.DrawString("Human Rights Gender Equity", normal, XBrushes.Black, xstart + 10, ystart + 20);
                            gfx.DrawString("and Environmental Studies", normal, XBrushes.Black, xstart + 10, ystart + 30);
                        }
                        else
                        { gfx.DrawString(item.SubjectName, normal, XBrushes.Black, xstart + 10, ystart + 20); }


                        gfx.DrawString(item.SubjectCode, normal, XBrushes.Black, 240, ystart + 20);

                        if (item.SubjectType == "TH")
                        {
                            gfx.DrawString("Theory", normal, XBrushes.Black, 305, ystart + 5);
                            gfx.DrawString(item.TheoryMax == "0" ? "-" : item.TheoryMax, normal, XBrushes.Black, 350, ystart + 5);
                            gfx.DrawString(item.TheoryMin == "0" ? "-" : item.TheoryMin, normal, XBrushes.Black, 385, ystart + 5);
                            gfx.DrawString(item.TheoryTotal.ToString() == "0" ? "-" : item.TheoryTotal.ToString(), normal, XBrushes.Black, item.TheoryTotal.ToString() == "-" ? 420 : 417, ystart + 5);

                            gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                            gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                            gfx.DrawString(item.TheoryIAMax, normal, XBrushes.Black, 352, ystart + 18);
                            gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                            gfx.DrawString(item.TheoryIATotal.ToString(), normal, XBrushes.Black, item.TheoryTotal.ToString() == "-" ? 420 : 417, ystart + 18);

                            gfx.DrawLine(XPens.Black, 300, ystart + 22, 445, ystart + 22);

                            if (int.TryParse(item.TheoryTotal, out theorytotal))
                            {
                                externalTotal += theorytotal;
                            }

                            int theorynet;
                            if (int.TryParse(item.TheoryNetTotal, out theorynet))
                            {
                                theorynettotal += theorynet;
                            }
                        }

                        ystart += 30;
                    }

                    if (float.TryParse(grouprow.SubjectGPA, out totalgp))
                    {
                        netgp += totalgp;
                    }

                    if (float.TryParse(grouprow.SubjectCredits, out totalcredites))
                    {
                        netcredits += totalcredites;
                    }

                    if (float.TryParse(grouprow.SubjectGPW, out totalgpw))
                    {
                        netgpw += totalgpw;
                    }

                    gfx.DrawLine(XPens.Black, 235, ystart - 8, 445, ystart - 8);

                    XSize ed = gfx.MeasureString("External Total", normal);
                    XSize gd = gfx.MeasureString(grouprow.GroupName + " Total", normal);

                    if (!haspractical)
                    {
                        gfx.DrawString("External Total", normal, XBrushes.Black, 225 - ed.Width, ystart);
                        gfx.DrawString("240", normal, XBrushes.Black, datarow.SubjectMax.ToString().Length == 3 ? 350 : 350, ystart);
                        gfx.DrawString("84", normal, XBrushes.Black, 380, ystart);

                        gfx.DrawString(externalTotal == 0 ? "-" : externalTotal.ToString(), normalitalic, XBrushes.Black, externalTotal.ToString().Length > 2 ? 416 : 420, ystart);
                        gfx.DrawLine(XPens.Black, 235, ystart + 5, 445, ystart + 5);

                        ystart += 15;

                        gfx.DrawString(grouprow.GroupName + " Total", normal, XBrushes.Black, 225 - gd.Width, ystart);
                        gfx.DrawString("300", normal, XBrushes.Black, datarow.SubjectMax.ToString().Length == 3 ? 350 : 350, ystart);
                        gfx.DrawString(grouprow.SubjectMin.ToString(), normal, XBrushes.Black, 380, ystart);
                        gfx.DrawString(grouprow.TotalMarks == "0" ? "-" : grouprow.TotalMarks, normalitalic, XBrushes.Black, grouprow.TotalMarks.ToString().Length > 2 ? 417 : 420, ystart);
                    }
                    else
                    {
                        gfx.DrawString("External Total", normal, XBrushes.Black, 225 - ed.Width, ystart);
                        gfx.DrawString("160", normal, XBrushes.Black, datarow.SubjectMax.ToString().Length == 3 ? 350 : 350, ystart);
                        gfx.DrawLine(XPens.Black, 235, ystart + 5, 445, ystart + 5);
                        gfx.DrawString("56", normal, XBrushes.Black, 380, ystart);

                        gfx.DrawString(externalTotal == 0 ? "-" : externalTotal.ToString(), normalitalic, XBrushes.Black, externalTotal.ToString().Length > 2 ? 417 : 420, ystart);
                        gfx.DrawLine(XPens.Black, 235, ystart + 5, 445, ystart + 5);

                        ystart += 15;

                        XSize xd = gfx.MeasureString(grouprow.GroupName + " Theory Total", normal);

                        gfx.DrawString(grouprow.GroupName + " Theory Total", normal, XBrushes.Black, 225 - xd.Width, ystart);
                        gfx.DrawString("200", normal, XBrushes.Black, datarow.SubjectMax.ToString().Length == 3 ? 350 : 350, ystart);
                        gfx.DrawString("70", normal, XBrushes.Black, 380, ystart);
                        gfx.DrawString(theorynettotal == 0 ? "-" : theorynettotal.ToString(), normalitalic, XBrushes.Black, grouprow.TotalMarks.ToString().Length > 2 ? 417 : 420, ystart);

                        gfx.DrawLine(XPens.Black, 235, ystart + 5, 445, ystart + 5);

                        ystart += 10;



                        var item = studentList.Where(d => d.GroupName == g && d.SubjectType == "PR").First();

                        gfx.DrawString(item.SubjectName, normal, XBrushes.Black, xstart + 10, ystart + 10);
                        gfx.DrawString(item.SubjectCode, normal, XBrushes.Black, 240, ystart + 10);

                        gfx.DrawString("Practical", normal, XBrushes.Black, 305, ystart + 5);
                        gfx.DrawString(item.PracticalMax, normal, XBrushes.Black, 350, ystart + 5);
                        gfx.DrawString(item.PracticalMin, normal, XBrushes.Black, 380, ystart + 5);
                        gfx.DrawString(item.PracticalTotal.ToString(), normal, XBrushes.Black, item.PracticalTotal.ToString().Length > 2 ? 417 : 420, ystart + 5);

                        gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                        gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                        gfx.DrawString(item.PracticalIAMax, normal, XBrushes.Black, 350, ystart + 18);
                        gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                        gfx.DrawString(item.PracticalIATotal.ToString(), normal, XBrushes.Black, item.PracticalIATotal.ToString().Length > 2 ? 417 : 420, ystart + 18);

                        gfx.DrawLine(XPens.Black, 235, ystart + 20, 445, ystart + 20);

                        ystart += 30;

                        XSize pd = gfx.MeasureString(grouprow.GroupName + " Practical Total", normal);

                        gfx.DrawString(grouprow.GroupName + " Practical Total", normal, XBrushes.Black, 225 - pd.Width, ystart);
                        gfx.DrawString("100".ToString(), normal, XBrushes.Black, item.SubjectMax.ToString().Length == 3 ? 347 : 350, ystart);
                        gfx.DrawString("35", normal, XBrushes.Black, 380, ystart);
                        gfx.DrawString(item.PracticalNetTotal == "0" ? "-" : item.PracticalNetTotal, normalitalic, XBrushes.Black, item.PracticalNetTotal.Length > 2 ? 417 : 420, ystart);

                        gfx.DrawLine(XPens.Black, 235, ystart + 5, 445, ystart + 5);

                        ystart += 15;

                        XSize nd = gfx.MeasureString(grouprow.GroupName + " Total", normal);

                        gfx.DrawString(grouprow.GroupName + " Total", normal, XBrushes.Black, 225 - nd.Width, ystart);
                        gfx.DrawString("300", normal, XBrushes.Black, datarow.SubjectMax.ToString().Length == 3 ? 350 : 350, ystart);
                        gfx.DrawString("105", normal, XBrushes.Black, 380, ystart);
                        gfx.DrawString(item.TotalMarks == "0" ? "-" : item.TotalMarks, normalitalic, XBrushes.Black, grouprow.TotalMarks.ToString().Length > 2 ? 417 : 420, ystart);

                        gfx.DrawLine(XPens.Black, 235, ystart + 5, 445, ystart + 5);

                    }





                    if ((grouprow.Remarks != "FAIL" && grouprow.Remarks != "-"))
                    {

                        gfx.DrawString(grouprow.SubjectCredits, normal, XBrushes.Black, grouprow.SubjectCredits.ToString().Length == 3 ? 455 : 460, ystart);
                        gfx.DrawString(grouprow.SubjectGPA, normal, XBrushes.Black, 490, ystart);
                        gfx.DrawString(grouprow.SubjectGPW, normal, XBrushes.Black, 520, ystart);
                    }
                    else
                    {
                        gfx.DrawString("-", normal, XBrushes.Black, grouprow.SubjectCredits.ToString().Length == 3 ? 455 : 460, ystart);
                        gfx.DrawString("-", normal, XBrushes.Black, 490, ystart);
                        gfx.DrawString("-", normal, XBrushes.Black, 520, ystart);
                    }


                    gfx.DrawLine(XPens.Black, 235, ystart + 5, drawablewidth, ystart + 5);

                    if (grouprow.Remarks == "-")
                    {
                        gfx.DrawString(grouprow.Remarks, normal, XBrushes.Black, drawablewidth - 35, ystart);
                    }
                    else
                    {
                        gfx.DrawString(grouprow.Remarks == "ABSENT" ? "FAIL" : grouprow.Remarks, normal, grouprow.Remarks == "PASS" ? XBrushes.Black : XBrushes.Red, drawablewidth - 45, ystart);
                    }

                    ystart += 10;
                    groupno++;
                }

                gfx.DrawLine(XPens.Black, xstart, ystart - 5, 250, ystart - 5);
                gfx.DrawLine(XPens.Black, 235, starty + 20, 235, ystart - 5);
                gfx.DrawLine(XPens.Black, 300, starty, 300, ystart - 5);

                gfx.DrawLine(XPens.Black, 342, starty + 20, 342, ystart + 15);
                gfx.DrawLine(XPens.Black, 370, starty + 20, 370, ystart + 15);
                gfx.DrawLine(XPens.Black, 400, starty + 20, 400, ystart + 15);

                gfx.DrawLine(XPens.Black, 445, starty, 445, ystart + 15);

                gfx.DrawLine(XPens.Black, 475, starty + 20, 475, ystart + 15);
                gfx.DrawLine(XPens.Black, 505, starty + 20, 505, ystart + 15);

                // gfx.DrawLine(XPens.Black, 500, starty + 15, 500, ystart + 10);

                gfx.DrawLine(XPens.Black, drawablewidth - 60, starty, drawablewidth - 60, ystart + 15);

                // ystart -= 30;

                gfx.DrawString("GRAND TOTAL", normalbold, XBrushes.Black, 260, ystart + 8);
                gfx.DrawString(@datarow.SemesterMax.ToString(), normal, XBrushes.Black, 347, ystart + 8);
                gfx.DrawString(datarow.SemesterTotal.ToString(), normalbold, XBrushes.CornflowerBlue, datarow.SemesterTotal.ToString().Length > 2 ? 415 : 420, ystart + 8);

                if (!(studentList.Any(d => d.Remarks == "FAIL") || studentList.Any(d => d.Remarks == "ABSENT")))
                {
                    gfx.DrawString(Math.Round(netcredits, 2).ToString(), normalbold, XBrushes.Black, Math.Round(netcredits, 2).ToString().Length > 1 ? 457 : 460, ystart + 8);
                    gfx.DrawString(Math.Round(netgp, 2).ToString(), normalbold, XBrushes.Black, Math.Round(netgp, 2).ToString().Length > 1 ? 487 : 490, ystart + 8);
                    gfx.DrawString(Math.Round(netgpw, 2).ToString(), normalbold, XBrushes.Black, Math.Round(netgpw, 2).ToString().Length > 2 ? 518 : 520, ystart + 8);
                }
                else
                {
                    gfx.DrawString("-", normalbold, XBrushes.Black, 460, ystart + 8);
                    gfx.DrawString("-", normalbold, XBrushes.Black, 490, ystart + 8);
                    gfx.DrawString("-", normalbold, XBrushes.Black, 520, ystart + 8);
                }

                ystart += 15;

                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                gfx.DrawString("Grand Total (In Words) : ", normal, XBrushes.Black, xstart + 10, ystart + 12);

                if (datarow.SemesterTotal.Value > 0)
                {
                    gfx.DrawString(NumberToWords(datarow.SemesterTotal.Value).ToUpper().Replace("AND", "").Replace("-", " "), normalbold, XBrushes.CornflowerBlue, xstart + 110, ystart + 12);

                }
                else
                {
                    gfx.DrawString("-", normalbold, XBrushes.Black, xstart + 110, ystart + 12);
                }

                ystart += 10;

                string sempercentage = datarow.SemesterPercentage;

                if (datarow.SemesterPercentage.Length > 5)
                {
                    sempercentage = datarow.SemesterPercentage.Substring(0, 5);
                }

                if (datarow.SemesterPercentage.Length == 4)
                {
                    sempercentage = datarow.SemesterPercentage + "0";
                }

                if (datarow.SemesterPercentage.Length == 2)
                {
                    sempercentage = datarow.SemesterPercentage + ".00";
                }

                string semesterGpa = netcredits > 0 ? Math.Round((netgpw / netcredits), 2).ToString() : "-";

                if (semesterGpa.Length > 4)
                {
                    semesterGpa = semesterGpa.Substring(0, 4);
                }

                if (semesterGpa.Length == 3)
                {
                    semesterGpa = semesterGpa + "0";
                }

                if (semesterGpa.Length == 1 && semesterGpa != "-")
                {
                    semesterGpa = semesterGpa + ".00";
                }

                gfx.DrawLine(XPens.Black, xstart, ystart + 10, drawablewidth, ystart + 10);

                if (!(studentList.Any(d => d.Remarks == "FAIL") || studentList.Any(d => d.Remarks == "ABSENT")))
                {
                    gfx.DrawString("Aggregated Marks : ", normal, XBrushes.Black, xstart + 10, ystart + 20);
                    gfx.DrawString(sempercentage, normalbold, XBrushes.Black, xstart + 90, ystart + 20);

                    gfx.DrawString("Grade Point Average : ", normal, XBrushes.Black, 260, ystart + 20);
                    gfx.DrawString(semesterGpa, normalbold, XBrushes.Black, 350, ystart + 20);

                    gfx.DrawString("Alpha Grade : ", normal, XBrushes.Black, 450, ystart + 20);
                    gfx.DrawString(datarow.SemesterAlphaSign, normalbold, XBrushes.Black, 510, ystart + 20);
                }
                else
                {
                    gfx.DrawString("Aggregated Marks : ", normal, XBrushes.Black, xstart + 10, ystart + 20);
                    gfx.DrawString("-", normalbold, XBrushes.Black, xstart + 90, ystart + 20);

                    gfx.DrawString("Grade Point Average : ", normal, XBrushes.Black, 260, ystart + 20);
                    gfx.DrawString("-", normalbold, XBrushes.Black, 350, ystart + 20);

                    gfx.DrawString("Alpha Grade : ", normal, XBrushes.Black, 450, ystart + 20);
                    gfx.DrawString("-", normalbold, XBrushes.Black, 510, ystart + 20);
                }

                ystart += 25;

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                gfx.DrawLine(XPens.Black, xstart, starty, xstart, ystart + 5);
                gfx.DrawLine(XPens.Black, drawablewidth, starty, drawablewidth, ystart + 5);

                gfx.DrawString("Cr = Credit                       GP = Grade Point                    GPW = Grade Point Weightage                            AB = Absent              EX = Exempted", normal, XBrushes.Black, xstart + 10, ystart + 15);

                gfx.DrawString("Transferred to College Records", normalex, XBrushes.Black, xstart + 10, ystart + 40);
                gfx.DrawString("Signature of the Principal", normalex, XBrushes.Black, xstart + 20, ystart + 100);
                gfx.DrawString("(With Seal)", normalex, XBrushes.Black, xstart + 40, ystart + 110);
                gfx.DrawString("Registrar (Evaluation)", normalex, XBrushes.Black, drawablewidth - 120, ystart + 110);
            }

        }

        private static void GroupFormatSEM6(string regno, int semester, ref PdfDocument document)
        {
            var entiteis = new MUPRJEntities();

            var studentList = entiteis.MarksCards.Where(d => d.RegisterNumber == regno && d.YearSem == semester).OrderBy(d => d.MarksCardOrder).ThenBy(d => d.SubjectCode).ToList();
            var datarow = studentList.First();

            PdfPage page = document.AddPage();
            page.Size = PdfSharp.PageSize.Legal;

            XFont normal = new XFont("Arial", 9, XFontStyle.Regular);
            XFont normalbold = new XFont("Arial", 9, XFontStyle.Bold);

            XFont normalex = new XFont("Arial", 10, XFontStyle.Regular);
            XFont normalboldex = new XFont("Arial", 10, XFontStyle.Bold);

            XFont normalitalic = new XFont("Arial", 9, XFontStyle.BoldItalic);
            XFont title = new XFont("Arial", 12, XFontStyle.Bold);

            using (XGraphics gfx = XGraphics.FromPdfPage(page))
            {
                int xstart = 10;
                int ystart = 10;

                int drawablewidth = (int)page.Width - 10;
                int drawableheight = (int)page.Height - 10;

                int middlepoint = drawablewidth / 2;

                gfx.DrawString("No." + datarow.UniqueNumber, normalbold, XBrushes.Black, xstart, ystart);

                ystart += 130;

                XSize s = gfx.MeasureString(datarow.DegreeName.ToUpper(), title);
                float middle = (float)s.Width / 2;

                gfx.DrawString(datarow.DegreeName.ToUpper(), title, XBrushes.CornflowerBlue, middlepoint - middle, ystart + 30);

                if (semester == 2)
                {
                    XSize size = gfx.MeasureString(" Credit Based Second Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Second Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }

                if (semester == 4)
                {
                    XSize size = gfx.MeasureString(" Credit Based Fourth Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Fourth Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }
                if (semester == 5)
                {
                    XSize size = gfx.MeasureString(" Credit Based Fifth Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Fifth Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }

                if (semester == 6)
                {
                    XSize size = gfx.MeasureString(" Credit Based Sixth Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Sixth Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }

                gfx.DrawString(datarow.PrintDate != null ? datarow.PrintDate.ToString() : "29-Apr-2016", normalboldex, XBrushes.Black, drawablewidth - 100, ystart + 5);



                using (var wc = new WebClient())
                {
                    using (var imgStream = new MemoryStream(wc.DownloadData("http://attristech.com:9797/muimages/" + datarow.RegisterNumber + "_p.jpg")))
                    {
                        using (var objImage = Image.FromStream(imgStream))
                        {
                            XImage image = XImage.FromGdiPlusImage(resizeImage(110, 150, objImage));
                            gfx.DrawImage(image, xstart - 15, ystart - 15);
                        }
                    }
                }

                ystart += 80;

                gfx.DrawString("Name : ", normalex, XBrushes.Black, xstart, ystart);
                gfx.DrawString(datarow.StudentName, normalboldex, XBrushes.Black, xstart + 35, ystart);

                gfx.DrawString("Register No. : ", normalex, XBrushes.Black, drawablewidth - 115, ystart);
                gfx.DrawString(datarow.RegisterNumber, normalboldex, XBrushes.Black, drawablewidth - 50, ystart);

                ystart += 10;
                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                int startx = xstart;
                int starty = ystart;

                ystart += 15;

                gfx.DrawString("Subject", normalbold, XBrushes.Black, 150, ystart);
                gfx.DrawString("Marks", normalbold, XBrushes.Black, 360, ystart);
                gfx.DrawString("Credit Calculations", normalbold, XBrushes.Black, 450, ystart);
                gfx.DrawString("Result", normalbold, XBrushes.Black, drawablewidth - 45, ystart);

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                ystart += 20;

                gfx.DrawString("Name", normal, XBrushes.Black, xstart + 100, ystart);
                gfx.DrawString("Code", normal, XBrushes.Black, 260, ystart);

                gfx.DrawString("Max", normal, XBrushes.Black, 345, ystart);
                gfx.DrawString("Min", normal, XBrushes.Black, 380, ystart);
                gfx.DrawString("Obtained", normal, XBrushes.Black, 405, ystart);

                gfx.DrawString("Cr", normal, XBrushes.Black, 455, ystart);
                gfx.DrawString("GP", normal, XBrushes.Black, 485, ystart);
                gfx.DrawString("GPW", normal, XBrushes.Black, 510, ystart);

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                ystart += 20;

                var groups = studentList.OrderBy(d => d.MarksCardOrder).ThenBy(d => d.GroupName).Select(d => d.GroupName).Distinct();


                int groupno = 0;
                float netcredits = 0;
                float netgp = 0;
                float netgpw = 0;


                foreach (var g in groups)
                {
                    int i = 0;
                    int externalTotal = 0;
                    int theorynettotal = 0;
                    int theorytotal = 0;

                    float totalcredites = 0;
                    float totalgpw = 0;
                    float totalgp = 0;

                    var grouprow = studentList.Where(d => d.GroupName == g).First();
                    var haspractical = studentList.Where(d => d.GroupName == g).Any(d => d.SubjectType == "PR");
                    int tottheorymax = 0;

                    foreach (var item in studentList.Where(d => d.GroupName == g && d.SubjectType == "TH").OrderBy(d => d.SubjectCode))
                    {
                        tottheorymax += int.Parse(item.TheoryMax);

                        if (i == 0)
                        {
                            if (groupno == 0)
                            {
                                gfx.DrawString("Group II : Optionals", normalbold, XBrushes.Black, xstart + 10, ystart);
                            }

                            gfx.DrawString(g, normalbold, XBrushes.Black, xstart + 10, ystart + 8);

                            if (groupno > 0)
                            {
                                gfx.DrawLine(XPens.Black, xstart, ystart - 5, drawablewidth, ystart - 5);
                            }


                        }


                        i++;

                        if (item.SubjectName == "Human Rights Gender Equity and Environmental Studies")
                        {
                            gfx.DrawString("Human Rights Gender Equity", normal, XBrushes.Black, xstart + 10, ystart + 20);
                            gfx.DrawString("and Environmental Studies", normal, XBrushes.Black, xstart + 10, ystart + 30);
                        }
                        else
                        { gfx.DrawString(item.SubjectName, normal, XBrushes.Black, xstart + 10, ystart + 20); }


                        gfx.DrawString(item.SubjectCode, normal, XBrushes.Black, 240, ystart + 20);

                        if (item.SubjectType == "TH")
                        {
                            gfx.DrawString("Theory", normal, XBrushes.Black, 305, ystart + 5);
                            gfx.DrawString(item.TheoryMax == "0" ? "-" : item.TheoryMax, normal, XBrushes.Black, 350, ystart + 5);
                            gfx.DrawString(item.TheoryMin == "0" ? "-" : item.TheoryMin, normal, XBrushes.Black, 385, ystart + 5);
                            gfx.DrawString(item.TheoryTotal.ToString() == "0" ? "-" : item.TheoryTotal.ToString(), normal, XBrushes.Black, item.TheoryTotal.ToString() == "-" ? 420 : 417, ystart + 5);

                            gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                            gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                            gfx.DrawString(item.TheoryIAMax, normal, XBrushes.Black, 352, ystart + 18);
                            gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                            gfx.DrawString(item.TheoryIATotal.ToString(), normal, XBrushes.Black, item.TheoryTotal.ToString() == "-" ? 420 : 417, ystart + 18);

                            gfx.DrawLine(XPens.Black, 300, ystart + 22, 445, ystart + 22);

                            if (int.TryParse(item.TheoryTotal, out theorytotal))
                            {
                                externalTotal += theorytotal;
                            }

                            int theorynet;
                            if (int.TryParse(item.TheoryNetTotal, out theorynet))
                            {
                                theorynettotal += theorynet;
                            }
                        }

                        ystart += 30;
                    }

                    if (float.TryParse(grouprow.SubjectGPA, out totalgp))
                    {
                        netgp += totalgp;
                    }

                    if (float.TryParse(grouprow.SubjectCredits, out totalcredites))
                    {
                        netcredits += totalcredites;
                    }

                    if (float.TryParse(grouprow.SubjectGPW, out totalgpw))
                    {
                        netgpw += totalgpw;
                    }

                    gfx.DrawLine(XPens.Black, 235, ystart - 8, 445, ystart - 8);

                    XSize ed = gfx.MeasureString("External Total", normal);
                    XSize gd = gfx.MeasureString(grouprow.GroupName + " Total", normal);

                    if (!haspractical)
                    {
                        gfx.DrawString("External Total", normal, XBrushes.Black, 225 - ed.Width, ystart);
                        gfx.DrawString("240", normal, XBrushes.Black, datarow.SubjectMax.ToString().Length == 3 ? 350 : 350, ystart);
                        gfx.DrawString("84", normal, XBrushes.Black, 380, ystart);

                        gfx.DrawString(externalTotal == 0 ? "-" : externalTotal.ToString(), normalitalic, XBrushes.Black, externalTotal.ToString().Length > 2 ? 416 : 420, ystart);
                        gfx.DrawLine(XPens.Black, 235, ystart + 5, 445, ystart + 5);

                        ystart += 15;

                        gfx.DrawString(grouprow.GroupName + " Total", normal, XBrushes.Black, 225 - gd.Width, ystart);
                        gfx.DrawString("300", normal, XBrushes.Black, datarow.SubjectMax.ToString().Length == 3 ? 350 : 350, ystart);
                        gfx.DrawString(grouprow.SubjectMin.ToString(), normal, XBrushes.Black, 380, ystart);
                        gfx.DrawString(grouprow.TotalMarks == "0" ? "-" : grouprow.TotalMarks, normalitalic, XBrushes.Black, grouprow.TotalMarks.ToString().Length > 2 ? 417 : 420, ystart);
                    }
                    else
                    {
                        gfx.DrawString("External Total", normal, XBrushes.Black, 225 - ed.Width, ystart);
                        gfx.DrawString("160", normal, XBrushes.Black, datarow.SubjectMax.ToString().Length == 3 ? 350 : 350, ystart);
                        gfx.DrawLine(XPens.Black, 235, ystart + 5, 445, ystart + 5);
                        gfx.DrawString("56", normal, XBrushes.Black, 380, ystart);

                        gfx.DrawString(externalTotal == 0 ? "-" : externalTotal.ToString(), normalitalic, XBrushes.Black, externalTotal.ToString().Length > 2 ? 417 : 420, ystart);
                        gfx.DrawLine(XPens.Black, 235, ystart + 5, 445, ystart + 5);

                        ystart += 15;

                        XSize xd = gfx.MeasureString(grouprow.GroupName + " Theory Total", normal);

                        gfx.DrawString(grouprow.GroupName + " Theory Total", normal, XBrushes.Black, 225 - xd.Width, ystart);
                        gfx.DrawString("200", normal, XBrushes.Black, datarow.SubjectMax.ToString().Length == 3 ? 350 : 350, ystart);
                        gfx.DrawString("70", normal, XBrushes.Black, 380, ystart);
                        gfx.DrawString(theorynettotal == 0 ? "-" : theorynettotal.ToString(), normalitalic, XBrushes.Black, grouprow.TotalMarks.ToString().Length > 2 ? 417 : 420, ystart);

                        gfx.DrawLine(XPens.Black, 235, ystart + 5, 445, ystart + 5);

                        ystart += 10;



                        var item = studentList.Where(d => d.GroupName == g && d.SubjectType == "PR").First();

                        gfx.DrawString(item.SubjectName, normal, XBrushes.Black, xstart + 10, ystart + 10);
                        gfx.DrawString(item.SubjectCode, normal, XBrushes.Black, 240, ystart + 10);

                        gfx.DrawString("Practical", normal, XBrushes.Black, 305, ystart + 5);
                        gfx.DrawString(item.PracticalMax, normal, XBrushes.Black, 350, ystart + 5);
                        gfx.DrawString(item.PracticalMin, normal, XBrushes.Black, 380, ystart + 5);
                        gfx.DrawString(item.PracticalTotal.ToString(), normal, XBrushes.Black, item.PracticalTotal.ToString().Length > 2 ? 417 : 420, ystart + 5);

                        gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                        gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                        gfx.DrawString(item.PracticalIAMax, normal, XBrushes.Black, 350, ystart + 18);
                        gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                        gfx.DrawString(item.PracticalIATotal.ToString(), normal, XBrushes.Black, item.PracticalIATotal.ToString().Length > 2 ? 417 : 420, ystart + 18);

                        gfx.DrawLine(XPens.Black, 235, ystart + 20, 445, ystart + 20);

                        ystart += 30;

                        XSize pd = gfx.MeasureString(grouprow.GroupName + " Practical Total", normal);

                        gfx.DrawString(grouprow.GroupName + " Practical Total", normal, XBrushes.Black, 225 - pd.Width, ystart);
                        gfx.DrawString("100".ToString(), normal, XBrushes.Black, item.SubjectMax.ToString().Length == 3 ? 347 : 350, ystart);
                        gfx.DrawString("35", normal, XBrushes.Black, 380, ystart);
                        gfx.DrawString(item.PracticalNetTotal == "0" ? "-" : item.PracticalNetTotal, normalitalic, XBrushes.Black, item.PracticalNetTotal.Length > 2 ? 417 : 420, ystart);

                        gfx.DrawLine(XPens.Black, 235, ystart + 5, 445, ystart + 5);

                        ystart += 15;

                        XSize nd = gfx.MeasureString(grouprow.GroupName + " Total", normal);

                        gfx.DrawString(grouprow.GroupName + " Total", normal, XBrushes.Black, 225 - nd.Width, ystart);
                        gfx.DrawString("300", normal, XBrushes.Black, datarow.SubjectMax.ToString().Length == 3 ? 350 : 350, ystart);
                        gfx.DrawString("105", normal, XBrushes.Black, 380, ystart);
                        gfx.DrawString(item.TotalMarks == "0" ? "-" : item.TotalMarks, normalitalic, XBrushes.Black, grouprow.TotalMarks.ToString().Length > 2 ? 417 : 420, ystart);

                        gfx.DrawLine(XPens.Black, 235, ystart + 5, 445, ystart + 5);

                    }





                    if ((grouprow.Remarks != "FAIL" && grouprow.Remarks != "-" && grouprow.Remarks != "ABSENT"))
                    {

                        gfx.DrawString(grouprow.SubjectCredits, normal, XBrushes.Black, grouprow.SubjectCredits.ToString().Length == 3 ? 455 : 460, ystart);
                        gfx.DrawString(grouprow.SubjectGPA, normal, XBrushes.Black, 490, ystart);
                        gfx.DrawString(grouprow.SubjectGPW, normal, XBrushes.Black, 520, ystart);
                    }
                    else
                    {
                        gfx.DrawString("-", normal, XBrushes.Black, grouprow.SubjectCredits.ToString().Length == 3 ? 455 : 460, ystart);
                        gfx.DrawString("-", normal, XBrushes.Black, 490, ystart);
                        gfx.DrawString("-", normal, XBrushes.Black, 520, ystart);
                    }


                    gfx.DrawLine(XPens.Black, 235, ystart + 5, drawablewidth, ystart + 5);

                    if (grouprow.Remarks == "-")
                    {
                        gfx.DrawString(grouprow.Remarks, normal, XBrushes.Black, drawablewidth - 35, ystart);
                    }
                    else
                    {
                        gfx.DrawString(grouprow.Remarks == "ABSENT" ? "FAIL" : grouprow.Remarks, normal, grouprow.Remarks == "PASS" ? XBrushes.Black : XBrushes.Red, drawablewidth - 45, ystart);
                    }

                    ystart += 10;
                    groupno++;
                }

                gfx.DrawLine(XPens.Black, xstart, ystart - 5, 250, ystart - 5);
                gfx.DrawLine(XPens.Black, 235, starty + 20, 235, ystart - 5);
                gfx.DrawLine(XPens.Black, 300, starty, 300, ystart - 5);

                gfx.DrawLine(XPens.Black, 342, starty + 20, 342, ystart + 15);
                gfx.DrawLine(XPens.Black, 370, starty + 20, 370, ystart + 15);
                gfx.DrawLine(XPens.Black, 400, starty + 20, 400, ystart + 15);

                gfx.DrawLine(XPens.Black, 445, starty, 445, ystart + 15);

                gfx.DrawLine(XPens.Black, 475, starty + 20, 475, ystart + 15);
                gfx.DrawLine(XPens.Black, 505, starty + 20, 505, ystart + 15);

                // gfx.DrawLine(XPens.Black, 500, starty + 15, 500, ystart + 10);

                gfx.DrawLine(XPens.Black, drawablewidth - 60, starty, drawablewidth - 60, ystart + 15);

                // ystart -= 30;

                gfx.DrawString("GRAND TOTAL", normalbold, XBrushes.Black, 260, ystart + 8);
                gfx.DrawString(@datarow.SemesterMax.ToString(), normal, XBrushes.Black, 347, ystart + 8);
                gfx.DrawString(datarow.SemesterTotal.ToString(), normalbold, XBrushes.CornflowerBlue, datarow.SemesterTotal.ToString().Length > 2 ? 415 : 420, ystart + 8);

                if (!(studentList.Any(d => d.Remarks == "FAIL") || studentList.Any(d => d.Remarks == "ABSENT") || studentList.Any(d => d.Remarks == "-" && d.YearText.Length == 0)))
                {
                    gfx.DrawString(Math.Round(netcredits, 2).ToString(), normalbold, XBrushes.Black, Math.Round(netcredits, 2).ToString().Length > 1 ? 457 : 460, ystart + 8);
                    gfx.DrawString(Math.Round(netgp, 2).ToString(), normalbold, XBrushes.Black, Math.Round(netgp, 2).ToString().Length > 1 ? 487 : 490, ystart + 8);
                    gfx.DrawString(Math.Round(netgpw, 2).ToString(), normalbold, XBrushes.Black, Math.Round(netgpw, 2).ToString().Length > 2 ? 518 : 520, ystart + 8);
                }
                else
                {
                    gfx.DrawString("-", normalbold, XBrushes.Black, 460, ystart + 8);
                    gfx.DrawString("-", normalbold, XBrushes.Black, 490, ystart + 8);
                    gfx.DrawString("-", normalbold, XBrushes.Black, 520, ystart + 8);
                }

                ystart += 15;

                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                gfx.DrawString("Grand Total (In Words) : ", normal, XBrushes.Black, xstart + 10, ystart + 12);

                if (datarow.SemesterTotal.Value > 0)
                {
                    gfx.DrawString(NumberToWords(datarow.SemesterTotal.Value).ToUpper().Replace("AND", "").Replace("-", " "), normalbold, XBrushes.CornflowerBlue, xstart + 110, ystart + 12);

                }
                else
                {
                    gfx.DrawString("-", normalbold, XBrushes.Black, xstart + 110, ystart + 12);
                }

                ystart += 10;

                string sempercentage = datarow.SemesterPercentage;

                if (datarow.SemesterPercentage.Length > 5)
                {
                    sempercentage = datarow.SemesterPercentage.Substring(0, 5);
                }

                if (datarow.SemesterPercentage.Length == 4)
                {
                    sempercentage = datarow.SemesterPercentage + "0";
                }

                if (datarow.SemesterPercentage.Length == 2)
                {
                    sempercentage = datarow.SemesterPercentage + ".00";
                }

                string semesterGpa = netcredits > 0 ? Math.Round((netgpw / netcredits), 2).ToString() : "-";

                if (semesterGpa.Length > 4)
                {
                    semesterGpa = semesterGpa.Substring(0, 4);
                }

                if (semesterGpa.Length == 3)
                {
                    semesterGpa = semesterGpa + "0";
                }

                if (semesterGpa.Length == 1 && semesterGpa != "-")
                {
                    semesterGpa = semesterGpa + ".00";
                }

                gfx.DrawLine(XPens.Black, xstart, ystart + 10, drawablewidth, ystart + 10);

                if (!(studentList.Any(d => d.Remarks == "FAIL") || studentList.Any(d => d.Remarks == "ABSENT")))
                {
                    gfx.DrawString("Aggregated Marks : ", normal, XBrushes.Black, xstart + 10, ystart + 20);
                    gfx.DrawString(sempercentage, normalbold, XBrushes.Black, xstart + 90, ystart + 20);

                    gfx.DrawString("Grade Point Average : ", normal, XBrushes.Black, 260, ystart + 20);
                    gfx.DrawString(semesterGpa, normalbold, XBrushes.Black, 350, ystart + 20);

                    gfx.DrawString("Alpha Grade : ", normal, XBrushes.Black, 450, ystart + 20);
                    gfx.DrawString(datarow.SemesterAlphaSign, normalbold, XBrushes.Black, 510, ystart + 20);
                }
                else
                {
                    gfx.DrawString("Aggregated Marks : ", normal, XBrushes.Black, xstart + 10, ystart + 20);
                    gfx.DrawString("-", normalbold, XBrushes.Black, xstart + 90, ystart + 20);

                    gfx.DrawString("Grade Point Average : ", normal, XBrushes.Black, 260, ystart + 20);
                    gfx.DrawString("-", normalbold, XBrushes.Black, 350, ystart + 20);

                    gfx.DrawString("Alpha Grade : ", normal, XBrushes.Black, 450, ystart + 20);
                    gfx.DrawString("-", normalbold, XBrushes.Black, 510, ystart + 20);
                }

                ystart += 25;

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                gfx.DrawLine(XPens.Black, xstart, starty, xstart, ystart + 5);
                gfx.DrawLine(XPens.Black, drawablewidth, starty, drawablewidth, ystart + 5);

                ystart += 10;

                var oldresults = entiteis.OldResultMaps.Where(d => d.RegisterNumber == regno).OrderBy(d => d.YearSem);

                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                int sty = ystart;

                gfx.DrawString("Semester", normalbold, XBrushes.Black, xstart + 10, ystart + 10);
                gfx.DrawString("Total Marks of", normalbold, XBrushes.Black, xstart + 80, ystart + 10);
                gfx.DrawString("   Semester", normalbold, XBrushes.Black, xstart + 80, ystart + 20);

                gfx.DrawString("Total Marks", normalbold, XBrushes.Black, xstart + 170, ystart + 10);
                gfx.DrawString("  Secured", normalbold, XBrushes.Black, xstart + 170, ystart + 20);

                gfx.DrawString("Semester GPA", normalbold, XBrushes.Black, xstart + 250, ystart + 10);
                gfx.DrawString("Semester Credits", normalbold, XBrushes.Black, xstart + 350, ystart + 10);

                gfx.DrawString(" Semester", normalbold, XBrushes.Black, xstart + 450, ystart + 10);
                gfx.DrawString(" Weightage", normalbold, XBrushes.Black, xstart + 450, ystart + 20);

                gfx.DrawString("Remarks", normalbold, XBrushes.Black, drawablewidth - 50, ystart + 10);


                gfx.DrawLine(XPens.Black, xstart, ystart + 30, drawablewidth, ystart + 30);

                ystart += 30;


                float totsemmax = 0;
                float totsemmarks = 0;
                float totalgpa = 0;
                float totalcredits = 0;
                int totalweightage = 0;


                foreach (var oldres in oldresults)
                {
                    //Get the current data
                    int semmarks = 0;
                    float semgpa = 0.0F;
                    int semcredits = 0;
                    int semweightage = 0;



                    gfx.DrawString(oldres.YearSem.ToString(), normal, XBrushes.Black, xstart + 10, ystart + 15);
                    gfx.DrawString(oldres.SemsterMax.ToString(), normal, XBrushes.Black, xstart + 100, ystart + 15);

                    gfx.DrawString((oldres.Remarks == "FAIL" ? "-" : oldres.SemesterMarks.ToString()), normal, XBrushes.Black, xstart + 190, ystart + 15);

                    gfx.DrawString(oldres.Remarks == "FAIL" ? "-" : Math.Round(oldres.SemesterGPA, 2).ToString().Length == 1 ? Math.Round(oldres.SemesterGPA, 2).ToString() + ".00" : Math.Round(oldres.SemesterGPA, 2).ToString(), normal, XBrushes.Black, xstart + 270, ystart + 15);
                    gfx.DrawString(oldres.Remarks == "FAIL" ? "-" : oldres.SemesterCredits.ToString(), normal, XBrushes.Black, xstart + 370, ystart + 15);

                    gfx.DrawString(oldres.Remarks == "FAIL" ? "-" : Math.Round((double)oldres.SemesterWeightage, MidpointRounding.AwayFromZero).ToString(), normal, XBrushes.Black, xstart + 470, ystart + 15);

                    gfx.DrawString(oldres.Remarks == "-" ? "FAIL" : oldres.Remarks, normal, oldres.Remarks == "PASS" ? XBrushes.Black : XBrushes.Red, drawablewidth - 50, ystart + 15);
                    ystart += 10;

                    gfx.DrawLine(XPens.Black, xstart, ystart + 10, drawablewidth, ystart + 10);
                    ystart += 5;

                    totsemmarks += oldres.SemesterMarks;
                    totalgpa += (float)Math.Round(oldres.SemesterGPA, 2);
                    totalcredits += oldres.SemesterCredits;
                    totalweightage += (int)Math.Round((double)oldres.SemesterWeightage, MidpointRounding.AwayFromZero);
                    totsemmax += oldres.SemsterMax;

                }



                ystart += 15;

                gfx.DrawString("GRAND TOTAL", normal, XBrushes.Black, xstart + 10, ystart);
                gfx.DrawString(oldresults.Any(d => d.Remarks == "FAIL") ? "-" : totsemmax.ToString(), normalbold, XBrushes.Black, xstart + 97, ystart);

                gfx.DrawString(oldresults.Any(d => d.Remarks == "FAIL") ? "-" : totsemmarks.ToString(), normalbold, XBrushes.Black, xstart + 187, ystart);

                gfx.DrawString(oldresults.Any(d => d.Remarks == "FAIL") ? "-" : totalgpa.ToString(), normalbold, XBrushes.Black, xstart + 270, ystart);
                gfx.DrawString(oldresults.Any(d => d.Remarks == "FAIL") ? "-" : totalcredits.ToString(), normalbold, XBrushes.Black, xstart + 370, ystart);

                gfx.DrawString(oldresults.Any(d => d.Remarks == "FAIL") ? "-" : totalweightage.ToString(), normalbold, XBrushes.Black, xstart + 470, ystart);

                bool issem6 = oldresults.First(d => d.YearSem == 6).Remarks == "PASS";
                bool isncl = oldresults.Any(d => d.YearSem != 6 && d.Remarks == "FAIL");
                bool iscoursefail = oldresults.Any(d => d.Remarks == "FAIL");


                if (issem6 && isncl)
                {
                    gfx.DrawString("NCL", normalbold, XBrushes.Red, drawablewidth - 50, ystart);
                }
                else
                {
                    gfx.DrawString(oldresults.Any(d => d.Remarks == "FAIL" || d.Remarks == "-") ? "FAIL" : "PASS", normalbold, oldresults.Any(d => d.Remarks == "FAIL") ? XBrushes.Red : XBrushes.Black, drawablewidth - 50, ystart);
                }
                ystart += 10;

                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);



                gfx.DrawLine(XPens.Black, 85, sty, 85, ystart);
                gfx.DrawLine(XPens.Black, 160, sty, 160, ystart);
                gfx.DrawLine(XPens.Black, 240, sty, 240, ystart);
                gfx.DrawLine(XPens.Black, 340, sty, 340, ystart);
                gfx.DrawLine(XPens.Black, 440, sty, 440, ystart);
                gfx.DrawLine(XPens.Black, drawablewidth - 60, sty, drawablewidth - 60, ystart);

                ystart += 10;



                float semesterAggragate = (float)Math.Round(((totsemmarks / totsemmax) * 100), 2);
                string semesterAlphaSign = "FAIL";

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

                string semesterClass = "";

                if (semesterAggragate >= 70)
                    semesterClass = "FIRST CLASS WITH DISTINCTION";
                if (semesterAggragate < 70 && semesterAggragate >= 60)
                    semesterClass = "FIRST CLASS";
                if (semesterAggragate < 60 && semesterAggragate >= 55)
                    semesterClass = "HIGH SECOND CLASS";
                if (semesterAggragate < 55 && semesterAggragate >= 50)
                    semesterClass = "SECOND CLASS";
                if (semesterAggragate < 50 && semesterAggragate >= 40)
                    semesterClass = "PASS CLASS";

                gfx.DrawString("GRAND TOTAL IN WORDS : " + (oldresults.Any(d => d.Remarks == "FAIL") ? "-" : NumberToWords((int)totsemmarks).ToUpper().Replace(" AND ", " ").Replace("-", " ")), normal, XBrushes.Black, xstart + 10, ystart);

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                int footery = ystart + 5;

                ystart += 10;

                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                semesterClass = textInfo.ToTitleCase(semesterClass.ToLower());

                gfx.DrawString("Aggregate Percentage of Marks : " + (oldresults.Any(d => d.Remarks == "FAIL") ? "-" : semesterAggragate.ToString()), normalex, XBrushes.Black, xstart + 10, ystart + 5);
                gfx.DrawString("Classification of Result : " + (oldresults.Any(d => d.Remarks == "FAIL") ? "-" : semesterClass), normalex, XBrushes.Black, middlepoint + 20, ystart + 5);

                ystart += 10;

                float semesavgpa = (float)Math.Round((double)(totalweightage / totalcredits), 2);

                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                gfx.DrawString("Programme Alpha Sign Grade : " + (oldresults.Any(d => d.Remarks == "FAIL") ? "-" : semesterAlphaSign), normalex, XBrushes.Black, xstart + 10, ystart + 10);
                gfx.DrawString("Aggregate Grade Point Average:  " + (oldresults.Any(d => d.Remarks == "FAIL") ? "-" : semesavgpa.ToString()), normalex, XBrushes.Black, middlepoint + 20, ystart + 10);

                gfx.DrawLine(XPens.Black, xstart, ystart + 15, drawablewidth, ystart + 15);
                gfx.DrawLine(XPens.Black, middlepoint, footery, middlepoint, ystart + 15);

                ystart += 10;

                gfx.DrawLine(XPens.Black, xstart, sty, xstart, ystart + 5);
                gfx.DrawLine(XPens.Black, drawablewidth, sty, drawablewidth, ystart + 5);

                ystart += 20;

                gfx.DrawString("Cr = Credit                                        GP = Grade Point                                        GPW = Grade Point Weightage                            AB = Absent", normal, XBrushes.Black, xstart + 10, ystart + 15);



                gfx.DrawString("Transferred to College Records", normalex, XBrushes.Black, xstart + 10, ystart + 40);
                gfx.DrawString("Signature of the Principal", normalex, XBrushes.Black, xstart + 20, ystart + 80);
                gfx.DrawString("(With Seal)", normalex, XBrushes.Black, xstart + 40, ystart + 90);
                gfx.DrawString("Registrar (Evaluation)", normalex, XBrushes.Black, drawablewidth - 120, ystart + 80);
            }

        }

        private static void NormalFormat()
        {
            var entiteis = new MUPRJEntities();

            string regno = "133750112";

            var studentList = entiteis.MarksCards.Where(d => d.RegisterNumber == regno).OrderBy(d => d.SubjectCode).ToList();
            var datarow = studentList.First();

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            page.Size = PdfSharp.PageSize.Legal;

            XFont normal = new XFont("Arial", 9, XFontStyle.Regular);
            XFont normalbold = new XFont("Arial", 9, XFontStyle.Bold);
            XFont normalitalic = new XFont("Arial", 9, XFontStyle.BoldItalic);
            XFont title = new XFont("Arial", 10, XFontStyle.Bold);

            using (XGraphics gfx = XGraphics.FromPdfPage(page))
            {
                int xstart = 10;
                int ystart = 10;

                int drawablewidth = (int)page.Width - 10;
                int drawableheight = (int)page.Height - 10;

                int middlepoint = drawablewidth / 2;

                gfx.DrawString("No." + datarow.UniqueNumber, normalbold, XBrushes.Black, xstart, ystart);

                ystart += 100;

                gfx.DrawString(datarow.DegreeName.ToUpper(), title, XBrushes.CornflowerBlue, middlepoint - 50, ystart);
                gfx.DrawString("Credit Based Fifth Semester Degree Examination Nov 2015", normalbold, XBrushes.Black, middlepoint - 120, ystart + 20);
                gfx.DrawString(datarow.PrintDate != null ? datarow.PrintDate.ToString() : "29-Apr-2016", normalbold, XBrushes.Black, drawablewidth - 100, ystart);



                using (var wc = new WebClient())
                {
                    using (var imgStream = new MemoryStream(wc.DownloadData("http://attristech.com:9797/muimages/" + datarow.RegisterNumber + "_p.jpg")))
                    {
                        using (var objImage = Image.FromStream(imgStream))
                        {
                            XImage image = XImage.FromGdiPlusImage(objImage);
                            gfx.DrawImage(image, xstart, ystart - 20, 100, 100);
                        }
                    }
                }

                ystart += 100;

                gfx.DrawString("Name : ", normal, XBrushes.Black, xstart, ystart);
                gfx.DrawString(datarow.StudentName, normalbold, XBrushes.Black, xstart + 32, ystart);

                gfx.DrawString("Register No : ", normal, XBrushes.Black, drawablewidth - 105, ystart);
                gfx.DrawString(datarow.RegisterNumber, normalbold, XBrushes.Black, drawablewidth - 50, ystart);

                ystart += 10;
                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                int startx = xstart;
                int starty = ystart;

                ystart += 10;

                gfx.DrawString("Subject", normalbold, XBrushes.Black, 150, ystart);
                gfx.DrawString("Marks", normalbold, XBrushes.Black, 360, ystart);
                gfx.DrawString("Credit Calculations", normalbold, XBrushes.Black, 450, ystart);
                gfx.DrawString("Remarks", normalbold, XBrushes.Black, drawablewidth - 45, ystart);

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                ystart += 15;

                gfx.DrawString("Name", normal, XBrushes.Black, xstart + 100, ystart);
                gfx.DrawString("Code", normal, XBrushes.Black, 260, ystart);

                gfx.DrawString("Max", normal, XBrushes.Black, 345, ystart);
                gfx.DrawString("Min", normal, XBrushes.Black, 380, ystart);
                gfx.DrawString("Obtained", normal, XBrushes.Black, 405, ystart);

                gfx.DrawString("Cr", normal, XBrushes.Black, 455, ystart);
                gfx.DrawString("GP", normal, XBrushes.Black, 485, ystart);
                gfx.DrawString("GPW", normal, XBrushes.Black, 510, ystart);

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                ystart += 10;

                int i = 0;
                foreach (var item in studentList)
                {

                    if (i == 0)
                    {
                        gfx.DrawString("Group-II : Optionals", normalbold, XBrushes.Black, xstart + 10, ystart + 5);
                    }

                    i++;
                    gfx.DrawString(item.SubjectName, normal, XBrushes.Black, xstart + 10, ystart + 15);
                    gfx.DrawString(item.SubjectCode, normal, XBrushes.Black, 240, ystart + 15);

                    if (item.SubjectType == "TH")
                    {
                        gfx.DrawString("Theory", normal, XBrushes.Black, 305, ystart + 5);
                        gfx.DrawString(item.TheoryMax, normal, XBrushes.Black, 350, ystart + 5);
                        gfx.DrawString(item.TheoryMin, normal, XBrushes.Black, 380, ystart + 5);
                        gfx.DrawString(item.TheoryTotal.ToString(), normal, XBrushes.Black, 415, ystart + 5);

                        gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                        gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                        gfx.DrawString(item.TheoryIAMax, normal, XBrushes.Black, 350, ystart + 18);
                        gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                        gfx.DrawString(item.TheoryIATotal.ToString(), normal, XBrushes.Black, 415, ystart + 18);

                        gfx.DrawLine(XPens.Black, 300, ystart + 20, 445, ystart + 20);
                    }
                    else if (item.SubjectType == "PR")
                    {
                        gfx.DrawString("Practical", normal, XBrushes.Black, 305, ystart + 5);
                        gfx.DrawString(item.PracticalMax, normal, XBrushes.Black, 350, ystart + 5);
                        gfx.DrawString(item.PracticalMin, normal, XBrushes.Black, 380, ystart + 5);
                        gfx.DrawString(item.PracticalTotal.ToString(), normal, XBrushes.Black, 415, ystart + 5);

                        gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                        gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                        gfx.DrawString(item.PracticalIAMax, normal, XBrushes.Black, 350, ystart + 18);
                        gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                        gfx.DrawString(item.PracticalIATotal.ToString(), normal, XBrushes.Black, 415, ystart + 18);

                        gfx.DrawLine(XPens.Black, 300, ystart + 20, 445, ystart + 20);
                    }
                    else if (item.SubjectType == "TNP")
                    {
                        gfx.DrawString("Theory", normal, XBrushes.Black, 305, ystart + 5);
                        gfx.DrawString(item.TheoryMax, normal, XBrushes.Black, 350, ystart + 5);
                        gfx.DrawString(item.TheoryMin, normal, XBrushes.Black, 380, ystart + 5);
                        gfx.DrawString(item.TheoryTotal.ToString(), normal, XBrushes.Black, 415, ystart + 5);

                        gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                        gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                        gfx.DrawString(item.TheoryIAMax, normal, XBrushes.Black, 350, ystart + 18);
                        gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                        gfx.DrawString(item.TheoryIATotal.ToString(), normal, XBrushes.Black, 415, ystart + 18);

                        gfx.DrawLine(XPens.Black, 300, ystart + 20, 445, ystart + 20);

                        ///////////////////PRACTICAL////////////

                        gfx.DrawString("Practical", normal, XBrushes.Black, 305, ystart + 30);
                        gfx.DrawString(item.PracticalMax, normal, XBrushes.Black, 350, ystart + 30);
                        gfx.DrawString(item.PracticalMin, normal, XBrushes.Black, 380, ystart + 30);
                        gfx.DrawString(item.PracticalTotal.ToString(), normal, XBrushes.Black, 415, ystart + 30);

                        gfx.DrawLine(XPens.Black, 300, ystart + 32, 445, ystart + 32);

                        gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 40);
                        gfx.DrawString(item.PracticalIAMax, normal, XBrushes.Black, 350, ystart + 40);
                        gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 40);
                        gfx.DrawString(item.PracticalIATotal.ToString(), normal, XBrushes.Black, 415, ystart + 40);

                        gfx.DrawLine(XPens.Black, 300, ystart + 42, 445, ystart + 42);

                        ystart += 20;
                    }
                    else if (item.SubjectType == "PRJ")
                    {
                        gfx.DrawString("Project", normal, XBrushes.Black, 305, ystart + 5);
                        gfx.DrawString(item.PracticalMax, normal, XBrushes.Black, 350, ystart + 5);
                        gfx.DrawString(item.PracticalMin, normal, XBrushes.Black, 380, ystart + 5);
                        gfx.DrawString(item.PracticalTotal.ToString(), normal, XBrushes.Black, 415, ystart + 5);

                        gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                        gfx.DrawString("Viva Voce", normal, XBrushes.Black, 305, ystart + 18);
                        gfx.DrawString(item.VivaVoiceMax, normal, XBrushes.Black, 350, ystart + 18);
                        gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                        gfx.DrawString(item.VivaVoice.ToString(), normal, XBrushes.Black, 415, ystart + 18);

                        gfx.DrawLine(XPens.Black, 300, ystart + 20, 445, ystart + 20);
                    }

                    ystart += 30;

                    gfx.DrawString("Total", normal, XBrushes.Black, 305, ystart);
                    gfx.DrawString(item.SubjectMax.ToString(), normal, XBrushes.Black, item.SubjectMax.ToString().Length == 3 ? 347 : 350, ystart);
                    gfx.DrawString(item.SubjectMin.ToString(), normal, XBrushes.Black, 380, ystart);
                    gfx.DrawString(item.TotalMarks, normalitalic, XBrushes.Black, 415, ystart);

                    gfx.DrawString(item.SubjectCredits, normal, XBrushes.Black, item.SubjectCredits.ToString().Length == 3 ? 455 : 460, ystart);
                    gfx.DrawString(item.SubjectGPA, normal, XBrushes.Black, 490, ystart);
                    gfx.DrawString(item.SubjectGPW, normal, XBrushes.Black, 515, ystart);

                    gfx.DrawLine(XPens.Black, 235, ystart + 5, drawablewidth, ystart + 5);

                    if (item.Remarks == "-")
                    {
                        gfx.DrawString(item.Remarks, normal, XBrushes.Black, drawablewidth - 45, ystart);
                    }
                    else
                    {
                        gfx.DrawString(item.Remarks == "ABSENT" ? "FAIL" : item.Remarks, normal, item.Remarks == "PASS" ? XBrushes.Black : XBrushes.Red, drawablewidth - 45, ystart);
                    }


                    ystart += 10;
                }

                gfx.DrawLine(XPens.Black, xstart, ystart - 5, 250, ystart - 5);
                gfx.DrawLine(XPens.Black, 235, starty + 15, 235, ystart - 5);
                gfx.DrawLine(XPens.Black, 300, starty, 300, ystart - 5);

                gfx.DrawLine(XPens.Black, 342, starty + 15, 342, ystart + 10);
                gfx.DrawLine(XPens.Black, 370, starty + 15, 370, ystart + 10);
                gfx.DrawLine(XPens.Black, 400, starty + 15, 400, ystart + 10);

                gfx.DrawLine(XPens.Black, 445, starty, 445, ystart + 10);

                gfx.DrawLine(XPens.Black, 475, starty + 15, 475, ystart + 10);
                gfx.DrawLine(XPens.Black, 505, starty + 15, 505, ystart + 10);

                // gfx.DrawLine(XPens.Black, 500, starty + 15, 500, ystart + 10);

                gfx.DrawLine(XPens.Black, drawablewidth - 60, starty, drawablewidth - 60, ystart + 10);

                // ystart -= 30;

                gfx.DrawString("GRAND TOTAL", normalbold, XBrushes.Black, 260, ystart + 5);
                gfx.DrawString("800", normal, XBrushes.Black, 347, ystart + 5);
                gfx.DrawString(datarow.SemesterTotal.ToString(), normalbold, XBrushes.CornflowerBlue, 415, ystart + 5);

                gfx.DrawString(datarow.SemesterCredits, normalbold, XBrushes.Black, 455, ystart + 5);
                gfx.DrawString(datarow.TotalGP, normalbold, XBrushes.Black, 485, ystart + 5);
                gfx.DrawString(datarow.TotalGPW, normalbold, XBrushes.Black, 514, ystart + 5);

                ystart += 10;

                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                gfx.DrawString("Grand Total (In Words) : ", normal, XBrushes.Black, xstart + 10, ystart + 10);

                if (datarow.SemesterTotal.Value > 0)
                {
                    gfx.DrawString(NumberToWords(datarow.SemesterTotal.Value).ToUpper().Replace("AND", "").Replace("-", " "), normalbold, XBrushes.CornflowerBlue, xstart + 110, ystart + 10);

                }
                else
                {
                    gfx.DrawString("-", normalbold, XBrushes.Black, xstart + 110, ystart + 10);
                }

                ystart += 10;

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                gfx.DrawString("Aggregated Marks : ", normal, XBrushes.Black, xstart + 10, ystart + 15);
                gfx.DrawString(datarow.SemesterPercentage, normalbold, XBrushes.Black, xstart + 90, ystart + 15);

                gfx.DrawString("Grade Point Average : ", normal, XBrushes.Black, 260, ystart + 15);
                gfx.DrawString(datarow.SemesterGPA, normalbold, XBrushes.Black, 350, ystart + 15);

                gfx.DrawString("Alpha Grade : ", normal, XBrushes.Black, 450, ystart + 15);
                gfx.DrawString(datarow.SemesterAlphaSign, normalbold, XBrushes.Black, 510, ystart + 15);

                ystart += 20;

                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                gfx.DrawLine(XPens.Black, xstart, starty, xstart, ystart);
                gfx.DrawLine(XPens.Black, drawablewidth, starty, drawablewidth, ystart);

                gfx.DrawString("Cr = Credit                                        GP = Grade Point                                        GPW = Grade Point Weightage                            AB = Absent", normal, XBrushes.Black, xstart + 10, ystart + 10);

                gfx.DrawString("Transferred to College Records", normal, XBrushes.Black, xstart + 10, ystart + 40);
                gfx.DrawString("Signature of the Principal", normal, XBrushes.Black, xstart + 20, ystart + 100);
                gfx.DrawString("(With Seal)", normal, XBrushes.Black, xstart + 40, ystart + 110);
                gfx.DrawString("Registrar (Evaluation)", normal, XBrushes.Black, drawablewidth - 120, ystart + 110);
            }




            document.Save(@"c:\temp\test\" + regno + ".pdf");
        }
        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        private static void GroupedFormat(string regno, int semester, ref PdfDocument document)
        {
            var entiteis = new MUPRJEntities();

            var studentList = entiteis.MarksCards.Where(d => d.RegisterNumber == regno && d.YearSem == semester).OrderBy(d => d.SubjectCode).ToList();
            var datarow = studentList.First();

            PdfPage page = document.AddPage();
            page.Size = PdfSharp.PageSize.Legal;

            XFont normal = new XFont("Arial", 9, XFontStyle.Regular);
            XFont normalbold = new XFont("Arial", 9, XFontStyle.Bold);

            XFont normalex = new XFont("Arial", 10, XFontStyle.Regular);
            XFont normalboldex = new XFont("Arial", 10, XFontStyle.Bold);

            XFont normalitalic = new XFont("Arial", 9, XFontStyle.BoldItalic);
            XFont title = new XFont("Arial", 12, XFontStyle.Bold);

            using (XGraphics gfx = XGraphics.FromPdfPage(page))
            {
                int xstart = 10;
                int ystart = 10;

                int drawablewidth = (int)page.Width - 10;
                int drawableheight = (int)page.Height - 10;

                int middlepoint = drawablewidth / 2;

                gfx.DrawString("No." + datarow.UniqueNumber, normalbold, XBrushes.Black, xstart, ystart);

                ystart += 150;

                XSize s = gfx.MeasureString(datarow.DegreeName.ToUpper(), title);
                float middle = (float)s.Width / 2;

                gfx.DrawString(datarow.DegreeName.ToUpper(), title, XBrushes.CornflowerBlue, middlepoint - middle, ystart + 30);

                if (semester == 1)
                {
                    XSize size = gfx.MeasureString(" Credit Based First Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based First Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }

                if (semester == 2)
                {
                    XSize size = gfx.MeasureString(" Credit Based Second Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Second Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }

                if (semester == 3)
                {
                    XSize size = gfx.MeasureString(" Credit Based Third Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Third Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }

                if (semester == 4)
                {
                    XSize size = gfx.MeasureString(" Credit Based Fourth Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Fourth Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }


                if (semester == 5)
                {
                    XSize size = gfx.MeasureString(" Credit Based Fifth Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Fifth Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }

                if (semester == 7)
                {
                    XSize size = gfx.MeasureString(" Credit Based Seventh Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Seventh Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }

                gfx.DrawString(datarow.PrintDate != null ? datarow.PrintDate.ToString() : "29-Apr-2016", normalboldex, XBrushes.Black, drawablewidth - 100, ystart + 5);



                using (var wc = new WebClient())
                {
                    using (var imgStream = new MemoryStream(wc.DownloadData("http://attristech.com:9797/muimages/" + datarow.RegisterNumber + "_p.jpg")))
                    {
                        using (var objImage = Image.FromStream(imgStream))
                        {
                            XImage image = XImage.FromGdiPlusImage(resizeImage(150, 180, objImage));
                            gfx.DrawImage(image, xstart - 15, ystart - 15);
                        }
                    }
                }

                ystart += 120;

                gfx.DrawString("Name : ", normalex, XBrushes.Black, xstart, ystart);
                gfx.DrawString(datarow.StudentName, normalboldex, XBrushes.Black, xstart + 35, ystart);

                gfx.DrawString("Register No. : ", normalex, XBrushes.Black, drawablewidth - 115, ystart);
                gfx.DrawString(datarow.RegisterNumber, normalboldex, XBrushes.Black, drawablewidth - 50, ystart);

                ystart += 10;
                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                int startx = xstart;
                int starty = ystart;

                ystart += 15;

                gfx.DrawString("Subject", normalbold, XBrushes.Black, 150, ystart);
                gfx.DrawString("Marks", normalbold, XBrushes.Black, 360, ystart);
                gfx.DrawString("Credit Calculations", normalbold, XBrushes.Black, 450, ystart);
                gfx.DrawString("Result", normalbold, XBrushes.Black, drawablewidth - 45, ystart);

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                ystart += 20;

                gfx.DrawString("Name", normal, XBrushes.Black, xstart + 100, ystart);
                gfx.DrawString("Code", normal, XBrushes.Black, 260, ystart);

                gfx.DrawString("Max", normal, XBrushes.Black, 345, ystart);
                gfx.DrawString("Min", normal, XBrushes.Black, 380, ystart);
                gfx.DrawString("Obtained", normal, XBrushes.Black, 405, ystart);

                gfx.DrawString("Cr", normal, XBrushes.Black, 455, ystart);
                gfx.DrawString("GP", normal, XBrushes.Black, 485, ystart);
                gfx.DrawString("GPW", normal, XBrushes.Black, 510, ystart);

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                ystart += 20;

                var groups = studentList.OrderBy(d => d.GroupName).Select(d => d.GroupName).Distinct();


                int groupno = 0;

                foreach (var g in groups)
                {
                    int i = 0;

                    var studentlist = studentList.Where(d => d.GroupName == g);

                    foreach (var item in g.IndexOf("Foundation Courses") >= 0 ? studentlist.OrderByDescending(d => d.SubjectName) : studentlist.OrderBy(d => d.MarksCardOrder).ThenBy(d => d.SubjectCode))
                    {
                        if (i == 0)
                        {
                            gfx.DrawString(g, normalbold, XBrushes.Black, xstart + 10, ystart + 5);

                            if (groupno > 0)
                            {
                                gfx.DrawLine(XPens.Black, xstart, ystart - 5, drawablewidth, ystart - 5);
                            }

                        }


                        i++;

                        if (item.SubjectName == "Human Rights Gender Equity and Environmental Studies" || item.SubjectName == "Human Rights Gender Equity & Environmental Studies")
                        {
                            gfx.DrawString("Human Rights Gender Equity", normal, XBrushes.Black, xstart + 10, ystart + 20);
                            gfx.DrawString("and Environmental Studies", normal, XBrushes.Black, xstart + 10, ystart + 30);
                        }
                        else
                        {
                            XSize sd = gfx.MeasureString(item.SubjectName, normal);

                            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                            string subejctname = textInfo.ToTitleCase(item.SubjectName.ToLower());

                            subejctname = subejctname.Replace("Iii", "III").Replace("Ii", "II");

                            if (sd.Width > 200)
                            {
                                string[] wordsd = subejctname.Split(' ');

                                int currentpos = xstart + 10;
                                int currenty = ystart + 15;
                                foreach (string w in wordsd)
                                {
                                    XSize wd = gfx.MeasureString(w, normal);
                                    if (wd.Width < 200 - currentpos)
                                    {
                                        gfx.DrawString(w, normal, XBrushes.Black, currentpos, currenty);
                                        currentpos += (int)wd.Width + 2;
                                    }
                                    else
                                    {
                                        currentpos = xstart + 10;
                                        currenty += 10;

                                        gfx.DrawString(w, normal, XBrushes.Black, currentpos, currenty);
                                        currentpos += (int)wd.Width + 2;
                                    }

                                }

                            }
                            else
                            {
                                gfx.DrawString(subejctname, normal, XBrushes.Black, xstart + 10, ystart + 20);
                            }


                        }


                        gfx.DrawString(item.SubjectCode, normal, XBrushes.Black, 240, ystart + 20);

                        if (item.SubjectType == "TH")
                        {

                            if (item.SubjectName == "Co-and Extra Curricular Activities")
                            {

                                gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                                gfx.DrawString(item.TheoryIAMax, normal, XBrushes.Black, 350, ystart + 18);
                                gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                                gfx.DrawString(item.TheoryIATotal.ToString(), normal, XBrushes.Black, item.TheoryTotal.ToString() == "-" ? 420 : 415, ystart + 18);


                            }
                            else
                            {
                                gfx.DrawString("Theory", normal, XBrushes.Black, 305, ystart + 5);
                                gfx.DrawString(item.TheoryMax == "0" ? "-" : item.TheoryMax, normal, XBrushes.Black, 350, ystart + 5);
                                gfx.DrawString(item.TheoryMin == "0" ? "-" : item.TheoryMin, normal, XBrushes.Black, 380, ystart + 5);
                                gfx.DrawString(item.TheoryTotal.ToString() == "0" ? "-" : item.TheoryTotal.ToString(), normal, XBrushes.Black, item.TheoryTotal.Length == 1 ? 420 : 415, ystart + 5);

                                gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                                gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                                gfx.DrawString(item.TheoryIAMax, normal, XBrushes.Black, 350, ystart + 18);
                                gfx.DrawString("-", normal, XBrushes.Black, 380, ystart + 18);
                                gfx.DrawString(item.TheoryIATotal.ToString(), normal, XBrushes.Black, item.TheoryIATotal.Length == 1 ? 420 : 415, ystart + 18);

                                gfx.DrawLine(XPens.Black, 300, ystart + 20, 445, ystart + 20);
                            }
                        }
                        else if (item.SubjectType == "PR")
                        {
                            gfx.DrawString("Practical", normal, XBrushes.Black, 305, ystart + 5);
                            gfx.DrawString(item.PracticalMax, normal, XBrushes.Black, 350, ystart + 5);
                            gfx.DrawString(item.PracticalMin, normal, XBrushes.Black, 380, ystart + 5);
                            gfx.DrawString(item.PracticalTotal.ToString(), normal, XBrushes.Black, item.PracticalTotal.ToString().Length == 1 ? 420 : 415, ystart + 5);

                            gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                            gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                            gfx.DrawString(item.PracticalIAMax, normal, XBrushes.Black, 350, ystart + 18);
                            gfx.DrawString("-", normal, XBrushes.Black, 380, ystart + 18);
                            gfx.DrawString(item.PracticalIATotal.ToString(), normal, XBrushes.Black, item.PracticalIATotal.ToString().Length == 1 ? 420 : 415, ystart + 18);

                            gfx.DrawLine(XPens.Black, 300, ystart + 20, 445, ystart + 20);
                        }
                        else if (item.SubjectType == "TNP")
                        {
                            gfx.DrawString("Theory", normal, XBrushes.Black, 305, ystart + 5);
                            gfx.DrawString(item.TheoryMax == "0" ? "-" : item.TheoryMax, normal, XBrushes.Black, 350, ystart + 5);
                            gfx.DrawString(item.TheoryMin == "0" ? "-" : item.TheoryMin, normal, XBrushes.Black, 380, ystart + 5);
                            gfx.DrawString(item.TheoryTotal.ToString() == "0" ? "-" : item.TheoryTotal.ToString(), normal, XBrushes.Black, item.TheoryTotal.ToString() == "-" ? 420 : 415, ystart + 5);

                            gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                            gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                            gfx.DrawString(item.TheoryIAMax, normal, XBrushes.Black, 350, ystart + 18);
                            gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                            gfx.DrawString(item.TheoryIATotal.ToString(), normal, XBrushes.Black, item.TheoryIATotal.ToString() == "-" ? 420 : 415, ystart + 18);

                            gfx.DrawLine(XPens.Black, 300, ystart + 20, 445, ystart + 20);

                            ///////////////////PRACTICAL////////////

                            gfx.DrawString("Practical", normal, XBrushes.Black, 305, ystart + 30);
                            gfx.DrawString(item.PracticalMax, normal, XBrushes.Black, 350, ystart + 30);
                            gfx.DrawString(item.PracticalMin, normal, XBrushes.Black, 380, ystart + 30);
                            gfx.DrawString(item.PracticalTotal.ToString(), normal, XBrushes.Black, item.PracticalTotal.ToString() == "-" ? 420 : 415, ystart + 30);

                            gfx.DrawLine(XPens.Black, 300, ystart + 32, 445, ystart + 32);

                            gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 40);
                            gfx.DrawString(item.PracticalIAMax, normal, XBrushes.Black, 350, ystart + 40);
                            gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 40);
                            gfx.DrawString(item.PracticalIATotal.ToString(), normal, XBrushes.Black, item.PracticalIATotal.ToString() == "-" ? 420 : 415, ystart + 40);

                            gfx.DrawLine(XPens.Black, 300, ystart + 42, 445, ystart + 42);

                            ystart += 20;
                        }
                        else if (item.SubjectType == "PRJ")
                        {
                            gfx.DrawString("Practical", normal, XBrushes.Black, 305, ystart + 5);
                            gfx.DrawString(item.PracticalMax, normal, XBrushes.Black, 350, ystart + 5);
                            gfx.DrawString(item.PracticalMin, normal, XBrushes.Black, 380, ystart + 5);
                            gfx.DrawString(item.PracticalTotal.ToString(), normal, XBrushes.Black, 415, ystart + 5);

                            gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                            gfx.DrawString("Viva", normal, XBrushes.Black, 305, ystart + 18);
                            gfx.DrawString(item.VivaVoiceMax, normal, XBrushes.Black, 350, ystart + 18);
                            gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                            gfx.DrawString(item.VivaVoice.ToString(), normal, XBrushes.Black, 415, ystart + 18);

                            gfx.DrawLine(XPens.Black, 300, ystart + 20, 445, ystart + 20);
                        }

                        ystart += 30;

                        if (item.SubjectName != "Co-and Extra Curricular Activities")
                        {

                            gfx.DrawString("Total", normal, XBrushes.Black, 305, ystart);
                            gfx.DrawString(item.SubjectMax.ToString(), normal, XBrushes.Black, item.SubjectMax.ToString().Length == 3 ? 347 : 350, ystart);
                            gfx.DrawString(item.SubjectMin.ToString(), normal, XBrushes.Black, 380, ystart);
                            gfx.DrawString(item.TotalMarks, normalitalic, XBrushes.Black, item.TheoryTotal.ToString() == "-" ? 420 : 415, ystart);

                            gfx.DrawString(item.SubjectCredits, normal, XBrushes.Black, item.SubjectCredits.ToString().Length == 3 ? 455 : 460, ystart);
                            gfx.DrawString(item.SubjectGPA, normal, XBrushes.Black, 490, ystart);
                            gfx.DrawString(item.SubjectGPW, normal, XBrushes.Black, 520, ystart);

                            gfx.DrawLine(XPens.Black, 235, ystart + 5, drawablewidth, ystart + 5);

                            if (item.Remarks == "-")
                            {
                                gfx.DrawString(item.Remarks, normal, XBrushes.Black, drawablewidth - 35, ystart);
                            }
                            else
                            {
                                gfx.DrawString(item.Remarks == "ABSENT" ? "FAIL" : item.Remarks, normal, item.Remarks == "PASS" ? XBrushes.Black : XBrushes.Red, drawablewidth - 45, ystart);
                            }
                        }
                        else
                        {
                            gfx.DrawLine(XPens.Black, 235, ystart + 5, drawablewidth, ystart + 5);

                            gfx.DrawString(item.SubjectCredits, normal, XBrushes.Black, item.SubjectCredits.ToString().Length == 3 ? 455 : 460, ystart - 10);
                            gfx.DrawString(item.SubjectGPA, normal, XBrushes.Black, 490, ystart - 10);
                            gfx.DrawString(item.SubjectGPW, normal, XBrushes.Black, 520, ystart - 10);

                            gfx.DrawLine(XPens.Black, 235, ystart + 5, drawablewidth, ystart + 5);

                            if (item.Remarks == "-")
                            {
                                gfx.DrawString(item.Remarks, normal, XBrushes.Black, drawablewidth - 35, ystart - 10);
                            }
                            else
                            {
                                gfx.DrawString(item.Remarks == "ABSENT" ? "FAIL" : item.Remarks, normal, item.Remarks == "PASS" ? XBrushes.Black : XBrushes.Red, drawablewidth - 45, ystart - 10);
                            }
                        }

                        ystart += 10;
                    }



                    groupno++;
                }

                gfx.DrawLine(XPens.Black, xstart, ystart - 5, 250, ystart - 5);
                gfx.DrawLine(XPens.Black, 235, starty + 20, 235, ystart - 5);
                gfx.DrawLine(XPens.Black, 300, starty, 300, ystart - 5);

                gfx.DrawLine(XPens.Black, 342, starty + 20, 342, ystart + 15);
                gfx.DrawLine(XPens.Black, 370, starty + 20, 370, ystart + 15);
                gfx.DrawLine(XPens.Black, 400, starty + 20, 400, ystart + 15);

                gfx.DrawLine(XPens.Black, 445, starty, 445, ystart + 15);

                gfx.DrawLine(XPens.Black, 475, starty + 20, 475, ystart + 15);
                gfx.DrawLine(XPens.Black, 505, starty + 20, 505, ystart + 15);

                // gfx.DrawLine(XPens.Black, 500, starty + 15, 500, ystart + 10);

                gfx.DrawLine(XPens.Black, drawablewidth - 60, starty, drawablewidth - 60, ystart + 15);

                // ystart -= 30;

                gfx.DrawString("GRAND TOTAL", normalbold, XBrushes.Black, 260, ystart + 8);
                gfx.DrawString(@datarow.SemesterMax.ToString(), normal, XBrushes.Black, 347, ystart + 8);
                gfx.DrawString(datarow.SemesterTotal.ToString(), normalbold, XBrushes.CornflowerBlue, 415, ystart + 8);

                gfx.DrawString(datarow.SemesterCredits, normalbold, XBrushes.Black, 455, ystart + 8);
                gfx.DrawString(datarow.TotalGP, normalbold, XBrushes.Black, 485, ystart + 8);
                gfx.DrawString(datarow.TotalGPW, normalbold, XBrushes.Black, 514, ystart + 8);

                ystart += 15;

                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                gfx.DrawString("Grand Total (In Words) : ", normal, XBrushes.Black, xstart + 10, ystart + 12);

                if (datarow.SemesterTotal.Value > 0)
                {
                    gfx.DrawString(NumberToWords(datarow.SemesterTotal.Value).ToUpper().Replace("AND", "").Replace("-", " "), normalbold, XBrushes.CornflowerBlue, xstart + 110, ystart + 12);

                }
                else
                {
                    gfx.DrawString("-", normalbold, XBrushes.Black, xstart + 110, ystart + 12);
                }

                ystart += 10;

                string sempercentage = datarow.SemesterPercentage;

                if (datarow.SemesterPercentage.Length > 5)
                {
                    sempercentage = datarow.SemesterPercentage.Substring(0, 5);
                }

                if (datarow.SemesterPercentage.Length == 4)
                {
                    sempercentage = datarow.SemesterPercentage + "0";
                }

                if (datarow.SemesterPercentage.Length == 2)
                {
                    sempercentage = datarow.SemesterPercentage + ".00";
                }

                string semesterGpa = datarow.SemesterGPA;

                if (datarow.SemesterGPA.Length > 4)
                {
                    semesterGpa = datarow.SemesterGPA.Substring(0, 4);
                }

                if (datarow.SemesterGPA.Length == 3)
                {
                    semesterGpa = datarow.SemesterGPA + "0";
                }

                if (datarow.SemesterGPA.Length == 1 && datarow.SemesterGPA != "-")
                {
                    semesterGpa = datarow.SemesterGPA + ".00";
                }

                gfx.DrawLine(XPens.Black, xstart, ystart + 10, drawablewidth, ystart + 10);

                gfx.DrawString("Aggregated Marks : ", normal, XBrushes.Black, xstart + 10, ystart + 20);
                gfx.DrawString(sempercentage, normalbold, XBrushes.Black, xstart + 90, ystart + 20);

                gfx.DrawString("Grade Point Average : ", normal, XBrushes.Black, 260, ystart + 20);
                gfx.DrawString(semesterGpa, normalbold, XBrushes.Black, 350, ystart + 20);

                gfx.DrawString("Alpha Grade : ", normal, XBrushes.Black, 450, ystart + 20);
                gfx.DrawString(datarow.SemesterAlphaSign, normalbold, XBrushes.Black, 510, ystart + 20);

                ystart += 25;

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                gfx.DrawLine(XPens.Black, xstart, starty, xstart, ystart + 5);
                gfx.DrawLine(XPens.Black, drawablewidth, starty, drawablewidth, ystart + 5);

                gfx.DrawString("Cr = Credit                       GP = Grade Point                    GPW = Grade Point Weightage                            AB = Absent              EX = Exempted", normal, XBrushes.Black, xstart + 10, ystart + 15);

                gfx.DrawString("Transferred to College Records", normalex, XBrushes.Black, xstart + 10, ystart + 40);
                gfx.DrawString("Signature of the Principal", normalex, XBrushes.Black, xstart + 20, ystart + 100);
                gfx.DrawString("(With Seal)", normalex, XBrushes.Black, xstart + 40, ystart + 110);
                gfx.DrawString("Registrar (Evaluation)", normalex, XBrushes.Black, drawablewidth - 120, ystart + 110);
            }




            // document.Save(@"c:\temp\test\" + regno + ".pdf");
        }

        private static void ConsolidatedFormat(string regno, int semester, ref PdfDocument document)
        {
            var entiteis = new MUPRJEntities();

            var studentList = entiteis.MarksCards.Where(d => d.RegisterNumber == regno && d.YearSem == semester).OrderBy(d => d.SubjectCode).ToList();
            var datarow = studentList.First();

            PdfPage page = document.AddPage();
            page.Size = PdfSharp.PageSize.Legal;

            XFont normal = new XFont("Arial", 9, XFontStyle.Regular);
            XFont normalbold = new XFont("Arial", 9, XFontStyle.Bold);

            XFont normalex = new XFont("Arial", 10, XFontStyle.Regular);
            XFont normalboldex = new XFont("Arial", 10, XFontStyle.Bold);

            XFont normalitalic = new XFont("Arial", 9, XFontStyle.BoldItalic);
            XFont title = new XFont("Arial", 12, XFontStyle.Bold);

            using (XGraphics gfx = XGraphics.FromPdfPage(page))
            {
                int xstart = 10;
                int ystart = 10;

                int drawablewidth = (int)page.Width - 10;
                int drawableheight = (int)page.Height - 10;

                int middlepoint = drawablewidth / 2;

                gfx.DrawString("No." + datarow.UniqueNumber, normalbold, XBrushes.Black, xstart, ystart);

                ystart += 150;

                XSize s = gfx.MeasureString(datarow.DegreeName.ToUpper(), title);
                float middle = (float)s.Width / 2;

                gfx.DrawString(datarow.DegreeName.ToUpper(), title, XBrushes.CornflowerBlue, middlepoint - middle, ystart + 30);

                if (semester == 1)
                {
                    XSize size = gfx.MeasureString(" Credit Based First Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based First Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }

                if (semester == 2)
                {
                    XSize size = gfx.MeasureString(" Credit Based Second Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Second Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }

                if (semester == 3)
                {
                    XSize size = gfx.MeasureString(" Credit Based Third Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Third Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }

                if (semester == 4)
                {
                    XSize size = gfx.MeasureString(" Credit Based Fourth Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Fourth Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }


                if (semester == 5)
                {
                    XSize size = gfx.MeasureString(" Credit Based Fifth Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Fifth Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }

                if (semester == 7)
                {
                    XSize size = gfx.MeasureString(" Credit Based Seventh Semester Degree Examination Nov 2015", normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Seventh Semester Degree Examination Nov 2015", normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }

                gfx.DrawString(datarow.PrintDate != null ? datarow.PrintDate.ToString() : "29-Apr-2016", normalboldex, XBrushes.Black, drawablewidth - 100, ystart + 5);



                using (var wc = new WebClient())
                {
                    using (var imgStream = new MemoryStream(wc.DownloadData("http://attristech.com:9797/muimages/" + datarow.RegisterNumber + "_p.jpg")))
                    {
                        using (var objImage = Image.FromStream(imgStream))
                        {
                            XImage image = XImage.FromGdiPlusImage(resizeImage(150, 180, objImage));
                            gfx.DrawImage(image, xstart - 15, ystart - 15);
                        }
                    }
                }

                ystart += 120;

                gfx.DrawString("Name : ", normalex, XBrushes.Black, xstart, ystart);
                gfx.DrawString(datarow.StudentName, normalboldex, XBrushes.Black, xstart + 35, ystart);

                gfx.DrawString("Register No. : ", normalex, XBrushes.Black, drawablewidth - 115, ystart);
                gfx.DrawString(datarow.RegisterNumber, normalboldex, XBrushes.Black, drawablewidth - 50, ystart);

                ystart += 10;
                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                int startx = xstart;
                int starty = ystart;

                ystart += 15;

                gfx.DrawString("Subject", normalbold, XBrushes.Black, 150, ystart);
                gfx.DrawString("Marks", normalbold, XBrushes.Black, 360, ystart);
                gfx.DrawString("Credit Calculations", normalbold, XBrushes.Black, 450, ystart);
                gfx.DrawString("Result", normalbold, XBrushes.Black, drawablewidth - 45, ystart);

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                ystart += 20;

                gfx.DrawString("Name", normal, XBrushes.Black, xstart + 100, ystart);
                gfx.DrawString("Code", normal, XBrushes.Black, 260, ystart);

                gfx.DrawString("Max", normal, XBrushes.Black, 345, ystart);
                gfx.DrawString("Min", normal, XBrushes.Black, 380, ystart);
                gfx.DrawString("Obtained", normal, XBrushes.Black, 405, ystart);

                gfx.DrawString("Cr", normal, XBrushes.Black, 455, ystart);
                gfx.DrawString("GP", normal, XBrushes.Black, 485, ystart);
                gfx.DrawString("GPW", normal, XBrushes.Black, 510, ystart);

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                ystart += 20;

                var groups = studentList.OrderBy(d => d.GroupName).Select(d => d.GroupName).Distinct();


                int groupno = 0;

                foreach (var g in groups)
                {
                    int i = 0;

                    var studentlist = studentList.Where(d => d.GroupName == g);

                    foreach (var item in g.IndexOf("Foundation Courses") >= 0 ? studentlist.OrderByDescending(d => d.SubjectName) : studentlist.OrderBy(d => d.MarksCardOrder).ThenBy(d => d.SubjectCode))
                    {
                        if (i == 0)
                        {
                            gfx.DrawString(g, normalbold, XBrushes.Black, xstart + 10, ystart + 5);

                            if (groupno > 0)
                            {
                                gfx.DrawLine(XPens.Black, xstart, ystart - 5, drawablewidth, ystart - 5);
                            }

                        }


                        i++;

                        if (item.SubjectName == "Human Rights Gender Equity and Environmental Studies" || item.SubjectName == "Human Rights Gender Equity & Environmental Studies")
                        {
                            gfx.DrawString("Human Rights Gender Equity", normal, XBrushes.Black, xstart + 10, ystart + 20);
                            gfx.DrawString("and Environmental Studies", normal, XBrushes.Black, xstart + 10, ystart + 30);
                        }
                        else
                        {
                            XSize sd = gfx.MeasureString(item.SubjectName, normal);

                            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                            string subejctname = textInfo.ToTitleCase(item.SubjectName.ToLower());

                            subejctname = subejctname.Replace("Iii", "III").Replace("Ii", "II");

                            if (sd.Width > 200)
                            {
                                string[] wordsd = subejctname.Split(' ');

                                int currentpos = xstart + 10;
                                int currenty = ystart + 15;
                                foreach (string w in wordsd)
                                {
                                    XSize wd = gfx.MeasureString(w, normal);
                                    if (wd.Width < 200 - currentpos)
                                    {
                                        gfx.DrawString(w, normal, XBrushes.Black, currentpos, currenty);
                                        currentpos += (int)wd.Width + 2;
                                    }
                                    else
                                    {
                                        currentpos = xstart + 10;
                                        currenty += 10;

                                        gfx.DrawString(w, normal, XBrushes.Black, currentpos, currenty);
                                        currentpos += (int)wd.Width + 2;
                                    }

                                }

                            }
                            else
                            {
                                gfx.DrawString(subejctname, normal, XBrushes.Black, xstart + 10, ystart + 20);
                            }


                        }


                        gfx.DrawString(item.SubjectCode, normal, XBrushes.Black, 240, ystart + 20);

                        if (item.SubjectType == "TH")
                        {

                            if (item.SubjectName == "Co-and Extra Curricular Activities")
                            {

                                gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                                gfx.DrawString(item.TheoryIAMax, normal, XBrushes.Black, 350, ystart + 18);
                                gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                                gfx.DrawString(item.TheoryIATotal.ToString(), normal, XBrushes.Black, item.TheoryTotal.ToString() == "-" ? 420 : 415, ystart + 18);


                            }
                            else
                            {
                                gfx.DrawString("Theory", normal, XBrushes.Black, 305, ystart + 5);
                                gfx.DrawString(item.TheoryMax == "0" ? "-" : item.TheoryMax, normal, XBrushes.Black, 350, ystart + 5);
                                gfx.DrawString(item.TheoryMin == "0" ? "-" : item.TheoryMin, normal, XBrushes.Black, 380, ystart + 5);
                                gfx.DrawString(item.TheoryTotal.ToString() == "0" ? "-" : item.TheoryTotal.ToString(), normal, XBrushes.Black, item.TheoryTotal.Length == 1 ? 420 : 415, ystart + 5);

                                gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                                gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                                gfx.DrawString(item.TheoryIAMax, normal, XBrushes.Black, 350, ystart + 18);
                                gfx.DrawString("-", normal, XBrushes.Black, 380, ystart + 18);
                                gfx.DrawString(item.TheoryIATotal.ToString(), normal, XBrushes.Black, item.TheoryIATotal.Length == 1 ? 420 : 415, ystart + 18);

                                gfx.DrawLine(XPens.Black, 300, ystart + 20, 445, ystart + 20);
                            }
                        }
                        else if (item.SubjectType == "PR")
                        {
                            gfx.DrawString("Practical", normal, XBrushes.Black, 305, ystart + 5);
                            gfx.DrawString(item.PracticalMax, normal, XBrushes.Black, 350, ystart + 5);
                            gfx.DrawString(item.PracticalMin, normal, XBrushes.Black, 380, ystart + 5);
                            gfx.DrawString(item.PracticalTotal.ToString(), normal, XBrushes.Black, item.PracticalTotal.ToString().Length == 1 ? 420 : 415, ystart + 5);

                            gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                            gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                            gfx.DrawString(item.PracticalIAMax, normal, XBrushes.Black, 350, ystart + 18);
                            gfx.DrawString("-", normal, XBrushes.Black, 380, ystart + 18);
                            gfx.DrawString(item.PracticalIATotal.ToString(), normal, XBrushes.Black, item.PracticalIATotal.ToString().Length == 1 ? 420 : 415, ystart + 18);

                            gfx.DrawLine(XPens.Black, 300, ystart + 20, 445, ystart + 20);
                        }
                        else if (item.SubjectType == "TNP")
                        {
                            gfx.DrawString("Theory", normal, XBrushes.Black, 305, ystart + 5);
                            gfx.DrawString(item.TheoryMax == "0" ? "-" : item.TheoryMax, normal, XBrushes.Black, 350, ystart + 5);
                            gfx.DrawString(item.TheoryMin == "0" ? "-" : item.TheoryMin, normal, XBrushes.Black, 380, ystart + 5);
                            gfx.DrawString(item.TheoryTotal.ToString() == "0" ? "-" : item.TheoryTotal.ToString(), normal, XBrushes.Black, item.TheoryTotal.ToString() == "-" ? 420 : 415, ystart + 5);

                            gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                            gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                            gfx.DrawString(item.TheoryIAMax, normal, XBrushes.Black, 350, ystart + 18);
                            gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                            gfx.DrawString(item.TheoryIATotal.ToString(), normal, XBrushes.Black, item.TheoryIATotal.ToString() == "-" ? 420 : 415, ystart + 18);

                            gfx.DrawLine(XPens.Black, 300, ystart + 20, 445, ystart + 20);

                            ///////////////////PRACTICAL////////////

                            gfx.DrawString("Practical", normal, XBrushes.Black, 305, ystart + 30);
                            gfx.DrawString(item.PracticalMax, normal, XBrushes.Black, 350, ystart + 30);
                            gfx.DrawString(item.PracticalMin, normal, XBrushes.Black, 380, ystart + 30);
                            gfx.DrawString(item.PracticalTotal.ToString(), normal, XBrushes.Black, item.PracticalTotal.ToString() == "-" ? 420 : 415, ystart + 30);

                            gfx.DrawLine(XPens.Black, 300, ystart + 32, 445, ystart + 32);

                            gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 40);
                            gfx.DrawString(item.PracticalIAMax, normal, XBrushes.Black, 350, ystart + 40);
                            gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 40);
                            gfx.DrawString(item.PracticalIATotal.ToString(), normal, XBrushes.Black, item.PracticalIATotal.ToString() == "-" ? 420 : 415, ystart + 40);

                            gfx.DrawLine(XPens.Black, 300, ystart + 42, 445, ystart + 42);

                            ystart += 20;
                        }
                        else if (item.SubjectType == "PRJ")
                        {
                            gfx.DrawString("Practical", normal, XBrushes.Black, 305, ystart + 5);
                            gfx.DrawString(item.PracticalMax, normal, XBrushes.Black, 350, ystart + 5);
                            gfx.DrawString(item.PracticalMin, normal, XBrushes.Black, 380, ystart + 5);
                            gfx.DrawString(item.PracticalTotal.ToString(), normal, XBrushes.Black, 415, ystart + 5);

                            gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                            gfx.DrawString("Viva", normal, XBrushes.Black, 305, ystart + 18);
                            gfx.DrawString(item.VivaVoiceMax, normal, XBrushes.Black, 350, ystart + 18);
                            gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                            gfx.DrawString(item.VivaVoice.ToString(), normal, XBrushes.Black, 415, ystart + 18);

                            gfx.DrawLine(XPens.Black, 300, ystart + 20, 445, ystart + 20);
                        }

                        ystart += 30;

                        if (item.SubjectName != "Co-and Extra Curricular Activities")
                        {

                            gfx.DrawString("Total", normal, XBrushes.Black, 305, ystart);
                            gfx.DrawString(item.SubjectMax.ToString(), normal, XBrushes.Black, item.SubjectMax.ToString().Length == 3 ? 347 : 350, ystart);
                            gfx.DrawString(item.SubjectMin.ToString(), normal, XBrushes.Black, 380, ystart);
                            gfx.DrawString(item.TotalMarks, normalitalic, XBrushes.Black, item.TheoryTotal.ToString() == "-" ? 420 : 415, ystart);

                            gfx.DrawString(item.SubjectCredits, normal, XBrushes.Black, item.SubjectCredits.ToString().Length == 3 ? 455 : 460, ystart);
                            gfx.DrawString(item.SubjectGPA, normal, XBrushes.Black, 490, ystart);
                            gfx.DrawString(item.SubjectGPW, normal, XBrushes.Black, 520, ystart);

                            gfx.DrawLine(XPens.Black, 235, ystart + 5, drawablewidth, ystart + 5);

                            gfx.DrawString(item.YearText, normal, XBrushes.Black, drawablewidth - 50, ystart - 15);

                            if (item.Remarks == "-")
                            {
                                gfx.DrawString(item.Remarks, normal, XBrushes.Black, drawablewidth - 35, ystart);
                            }
                            else
                            {
                                gfx.DrawString(item.Remarks == "ABSENT" ? "FAIL" : item.Remarks, normal, item.Remarks == "PASS" ? XBrushes.Black : XBrushes.Red, drawablewidth - 45, ystart);
                            }
                        }
                        else
                        {
                            gfx.DrawLine(XPens.Black, 235, ystart + 5, drawablewidth, ystart + 5);

                            gfx.DrawString(item.SubjectCredits, normal, XBrushes.Black, item.SubjectCredits.ToString().Length == 3 ? 455 : 460, ystart - 10);
                            gfx.DrawString(item.SubjectGPA, normal, XBrushes.Black, 490, ystart - 10);
                            gfx.DrawString(item.SubjectGPW, normal, XBrushes.Black, 520, ystart - 10);

                            gfx.DrawLine(XPens.Black, 235, ystart + 5, drawablewidth, ystart + 5);

                            gfx.DrawString(item.YearText, normal, XBrushes.Black, drawablewidth - 50, ystart - 20);

                            if (item.Remarks == "-")
                            {
                                gfx.DrawString(item.Remarks, normal, XBrushes.Black, drawablewidth - 35, ystart - 10);
                            }
                            else
                            {
                                gfx.DrawString(item.Remarks == "ABSENT" ? "FAIL" : item.Remarks, normal, item.Remarks == "PASS" ? XBrushes.Black : XBrushes.Red, drawablewidth - 45, ystart - 10);
                            }
                        }

                        ystart += 10;
                    }



                    groupno++;
                }

                gfx.DrawLine(XPens.Black, xstart, ystart - 5, 250, ystart - 5);
                gfx.DrawLine(XPens.Black, 235, starty + 20, 235, ystart - 5);
                gfx.DrawLine(XPens.Black, 300, starty, 300, ystart - 5);

                gfx.DrawLine(XPens.Black, 342, starty + 20, 342, ystart + 15);
                gfx.DrawLine(XPens.Black, 370, starty + 20, 370, ystart + 15);
                gfx.DrawLine(XPens.Black, 400, starty + 20, 400, ystart + 15);

                gfx.DrawLine(XPens.Black, 445, starty, 445, ystart + 15);

                gfx.DrawLine(XPens.Black, 475, starty + 20, 475, ystart + 15);
                gfx.DrawLine(XPens.Black, 505, starty + 20, 505, ystart + 15);

                // gfx.DrawLine(XPens.Black, 500, starty + 15, 500, ystart + 10);

                gfx.DrawLine(XPens.Black, drawablewidth - 60, starty, drawablewidth - 60, ystart + 15);

                // ystart -= 30;

                gfx.DrawString("GRAND TOTAL", normalbold, XBrushes.Black, 260, ystart + 8);
                gfx.DrawString(@datarow.SemesterMax.ToString(), normal, XBrushes.Black, 347, ystart + 8);
                gfx.DrawString(datarow.SemesterTotal.ToString(), normalbold, XBrushes.CornflowerBlue, 415, ystart + 8);

                gfx.DrawString(datarow.SemesterCredits, normalbold, XBrushes.Black, 455, ystart + 8);
                gfx.DrawString(datarow.TotalGP, normalbold, XBrushes.Black, 485, ystart + 8);
                gfx.DrawString(datarow.TotalGPW, normalbold, XBrushes.Black, 514, ystart + 8);

                ystart += 15;

                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                gfx.DrawString("Grand Total (In Words) : ", normal, XBrushes.Black, xstart + 10, ystart + 12);

                if (datarow.SemesterTotal.Value > 0)
                {
                    gfx.DrawString(NumberToWords(datarow.SemesterTotal.Value).ToUpper().Replace("AND", "").Replace("-", " "), normalbold, XBrushes.CornflowerBlue, xstart + 110, ystart + 12);

                }
                else
                {
                    gfx.DrawString("-", normalbold, XBrushes.Black, xstart + 110, ystart + 12);
                }

                ystart += 10;

                string sempercentage = datarow.SemesterPercentage;

                if (datarow.SemesterPercentage.Length > 5)
                {
                    sempercentage = datarow.SemesterPercentage.Substring(0, 5);
                }

                if (datarow.SemesterPercentage.Length == 4)
                {
                    sempercentage = datarow.SemesterPercentage + "0";
                }

                if (datarow.SemesterPercentage.Length == 2)
                {
                    sempercentage = datarow.SemesterPercentage + ".00";
                }

                string semesterGpa = datarow.SemesterGPA;

                if (datarow.SemesterGPA.Length > 4)
                {
                    semesterGpa = datarow.SemesterGPA.Substring(0, 4);
                }

                if (datarow.SemesterGPA.Length == 3)
                {
                    semesterGpa = datarow.SemesterGPA + "0";
                }

                if (datarow.SemesterGPA.Length == 1 && datarow.SemesterGPA != "-")
                {
                    semesterGpa = datarow.SemesterGPA + ".00";
                }

                gfx.DrawLine(XPens.Black, xstart, ystart + 10, drawablewidth, ystart + 10);

                gfx.DrawString("Aggregated Marks : ", normal, XBrushes.Black, xstart + 10, ystart + 20);
                gfx.DrawString(sempercentage, normalbold, XBrushes.Black, xstart + 90, ystart + 20);

                gfx.DrawString("Grade Point Average : ", normal, XBrushes.Black, 260, ystart + 20);
                gfx.DrawString(semesterGpa, normalbold, XBrushes.Black, 350, ystart + 20);

                gfx.DrawString("Alpha Grade : ", normal, XBrushes.Black, 450, ystart + 20);
                gfx.DrawString(datarow.SemesterAlphaSign, normalbold, XBrushes.Black, 510, ystart + 20);

                ystart += 25;

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                gfx.DrawLine(XPens.Black, xstart, starty, xstart, ystart + 5);
                gfx.DrawLine(XPens.Black, drawablewidth, starty, drawablewidth, ystart + 5);

                gfx.DrawString("Cr = Credit                       GP = Grade Point                    GPW = Grade Point Weightage                            AB = Absent              EX = Exempted", normal, XBrushes.Black, xstart + 10, ystart + 15);

                gfx.DrawString("Transferred to College Records", normalex, XBrushes.Black, xstart + 10, ystart + 40);
                gfx.DrawString("Signature of the Principal", normalex, XBrushes.Black, xstart + 20, ystart + 100);
                gfx.DrawString("(With Seal)", normalex, XBrushes.Black, xstart + 40, ystart + 110);
                gfx.DrawString("Registrar (Evaluation)", normalex, XBrushes.Black, drawablewidth - 120, ystart + 110);
            }




            // document.Save(@"c:\temp\test\" + regno + ".pdf");
        }

        private static void GroupedSEM6Format(string regno, int semester, ref PdfDocument document)
        {
            var entiteis = new MUPRJEntities();

            var studentList = entiteis.MarksCards.Where(d => d.RegisterNumber == regno && d.YearSem == semester).OrderBy(d => d.SubjectCode).ToList();
            var datarow = studentList.First();

            //int totalgpa = 0;
            //int totalgpw = 0;

            var isfail = studentList.Any(d => d.Remarks == "FAIL");

            PdfPage page = document.AddPage();
            page.Size = PdfSharp.PageSize.Legal;

            XFont normal = new XFont("Arial", 9, XFontStyle.Regular);
            XFont normalbold = new XFont("Arial", 9, XFontStyle.Bold);

            XFont normalex = new XFont("Arial", 10, XFontStyle.Regular);
            XFont normalboldex = new XFont("Arial", 10, XFontStyle.Bold);

            XFont normalitalic = new XFont("Arial", 9, XFontStyle.BoldItalic);
            XFont title = new XFont("Arial", 11, XFontStyle.Bold);

            using (XGraphics gfx = XGraphics.FromPdfPage(page))
            {
                int xstart = 10;
                int ystart = 10;

                int drawablewidth = (int)page.Width - 10;
                int drawableheight = (int)page.Height - 10;

                int middlepoint = drawablewidth / 2;

                gfx.DrawString("No." + datarow.UniqueNumber, normalbold, XBrushes.Black, xstart, ystart);

                ystart += 150;

                XSize s = gfx.MeasureString(datarow.DegreeName.ToUpper(), title);
                float middle = (float)s.Width / 2;

                gfx.DrawString(datarow.DegreeName.ToUpper(), title, XBrushes.CornflowerBlue, middlepoint - middle, ystart + 30);
                string examiationyear = datarow.ExaminationYear.Length > 0 ? datarow.ExaminationYear : "Nov 2015";

                if (semester == 2)
                {
                    XSize size = gfx.MeasureString(" Credit Based Second Semester Degree Examination " + examiationyear, normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Second Semester Degree Examination " + examiationyear, normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }

                if (semester == 4)
                {
                    XSize size = gfx.MeasureString(" Credit Based Fourth Semester Degree Examination " + examiationyear, normalboldex);
                    float m = (float)size.Width / 2;
                    gfx.DrawString("Credit Based Fourth Semester Degree Examination " + examiationyear, normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                }


                if (semester == 6)
                {
                    XSize size = gfx.MeasureString(" Credit Based Sixth Semester Degree Examination "+ examiationyear, normalboldex);
                    float m = (float)size.Width / 2;
                    if (examiationyear != null)
                    {
                        gfx.DrawString("Credit Based Sixth Semester Degree Examination " + examiationyear, new XFont("Arial", 9, XFontStyle.Bold), XBrushes.Black, middlepoint - m + 13, ystart + 50);
                    }
                    else
                    {
                        gfx.DrawString("Credit Based Sixth Semester Degree Examination " + examiationyear, normalboldex, XBrushes.Black, middlepoint - m, ystart + 50);
                    }
                }

                gfx.DrawString(datarow.PrintDate != null ? datarow.PrintDate.ToString() : "29-Apr-2016", normalboldex, XBrushes.Black, drawablewidth - 100, ystart + 5);



                using (var wc = new WebClient())
                {
                    using (var imgStream = new MemoryStream(wc.DownloadData("http://attristech.com:9797/muimages/" + datarow.RegisterNumber + "_p.jpg")))
                    {
                        using (var objImage = Image.FromStream(imgStream))
                        {
                            XImage image = XImage.FromGdiPlusImage(resizeImage(150, 180, objImage));
                            gfx.DrawImage(image, xstart - 15, ystart - 15);
                        }
                    }
                }

                ystart += 120;

                gfx.DrawString("Name : ", normalex, XBrushes.Black, xstart, ystart);
                gfx.DrawString(datarow.StudentName, normalboldex, XBrushes.Black, xstart + 35, ystart);

                gfx.DrawString("Register No. : ", normalex, XBrushes.Black, drawablewidth - 115, ystart);
                gfx.DrawString(datarow.RegisterNumber, normalboldex, XBrushes.Black, drawablewidth - 50, ystart);

                ystart += 10;
                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                int startx = xstart;
                int starty = ystart;

                ystart += 15;

                gfx.DrawString("Subject", normalbold, XBrushes.Black, 150, ystart);
                gfx.DrawString("Marks", normalbold, XBrushes.Black, 360, ystart);
                gfx.DrawString("Credit Calculations", normalbold, XBrushes.Black, 450, ystart);
                gfx.DrawString("Result", normalbold, XBrushes.Black, drawablewidth - 45, ystart);

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                ystart += 20;

                gfx.DrawString("Name", normal, XBrushes.Black, xstart + 100, ystart);
                gfx.DrawString("Code", normal, XBrushes.Black, 260, ystart);

                gfx.DrawString("Max", normal, XBrushes.Black, 345, ystart);
                gfx.DrawString("Min", normal, XBrushes.Black, 380, ystart);
                gfx.DrawString("Obtained", normal, XBrushes.Black, 405, ystart);

                gfx.DrawString("Cr", normal, XBrushes.Black, 455, ystart);
                gfx.DrawString("GP", normal, XBrushes.Black, 485, ystart);
                gfx.DrawString("GPW", normal, XBrushes.Black, 510, ystart);

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                ystart += 20;

                var groups = studentList.OrderBy(d => d.GroupName).Select(d => d.GroupName).Distinct();


                int groupno = 0;

                foreach (var g in groups)
                {
                    int i = 0;

                    var studentlist = studentList.Where(d => d.GroupName == g);

                    foreach (var item in g.IndexOf("Foundation Courses") >= 0 ? studentlist.OrderByDescending(d => d.SubjectName) : studentlist.OrderBy(d => d.MarksCardOrder).ThenBy(d => d.SubjectCode))
                    {
                        if (i == 0)
                        {
                            gfx.DrawString(g, normalbold, XBrushes.Black, xstart + 10, ystart + 5);

                            if (groupno > 0)
                            {
                                gfx.DrawLine(XPens.Black, xstart, ystart - 5, drawablewidth, ystart - 5);
                            }

                        }


                        i++;

                        if (item.SubjectName == "Human Rights Gender Equity and Environmental Studies")
                        {
                            gfx.DrawString("Human Rights Gender Equity", normal, XBrushes.Black, xstart + 10, ystart + 20);
                            gfx.DrawString("and Environmental Studies", normal, XBrushes.Black, xstart + 10, ystart + 30);
                        }
                        else
                        { gfx.DrawString(item.SubjectName, normal, XBrushes.Black, xstart + 10, ystart + 20); }


                        gfx.DrawString(item.SubjectCode, normal, XBrushes.Black, 240, ystart + 20);

                        if (item.SubjectType == "TH")
                        {

                            if (item.SubjectName == "Co-and Extra Curricular Activities")
                            {

                                gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                                gfx.DrawString(item.TheoryIAMax, normal, XBrushes.Black, 350, ystart + 18);
                                gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                                gfx.DrawString(item.TheoryIATotal.ToString(), normal, XBrushes.Black, item.TheoryTotal.ToString() == "-" ? 420 : 415, ystart + 18);


                            }
                            else
                            {
                                gfx.DrawString("Theory", normal, XBrushes.Black, 305, ystart + 5);
                                gfx.DrawString(item.TheoryMax == "0" ? "-" : item.TheoryMax, normal, XBrushes.Black, 350, ystart + 5);
                                gfx.DrawString(item.TheoryMin == "0" ? "-" : item.TheoryMin, normal, XBrushes.Black, 380, ystart + 5);
                                gfx.DrawString(item.TheoryTotal.ToString() == "0" ? "-" : item.TheoryTotal.ToString(), normal, XBrushes.Black, item.TheoryTotal.Length == 1 ? 420 : 415, ystart + 5);

                                gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                                gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                                gfx.DrawString(item.TheoryIAMax, normal, XBrushes.Black, 350, ystart + 18);
                                gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                                gfx.DrawString(item.TheoryIATotal.ToString(), normal, XBrushes.Black, item.TheoryIATotal.Length == 1 ? 420 : 415, ystart + 18);

                                gfx.DrawLine(XPens.Black, 300, ystart + 20, 445, ystart + 20);
                            }
                        }
                        else if (item.SubjectType == "PR")
                        {
                            gfx.DrawString("Practical", normal, XBrushes.Black, 305, ystart + 5);
                            gfx.DrawString(item.PracticalMax, normal, XBrushes.Black, 350, ystart + 5);
                            gfx.DrawString(item.PracticalMin, normal, XBrushes.Black, 380, ystart + 5);
                            gfx.DrawString(item.PracticalTotal.ToString(), normal, XBrushes.Black, 420, ystart + 5);

                            gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                            gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                            gfx.DrawString(item.PracticalIAMax, normal, XBrushes.Black, 350, ystart + 18);
                            gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                            gfx.DrawString(item.PracticalIATotal.ToString(), normal, XBrushes.Black, 420, ystart + 18);

                            gfx.DrawLine(XPens.Black, 300, ystart + 20, 445, ystart + 20);
                        }
                        else if (item.SubjectType == "TNP")
                        {
                            gfx.DrawString("Theory", normal, XBrushes.Black, 305, ystart + 5);
                            gfx.DrawString(item.TheoryMax == "0" ? "-" : item.TheoryMax, normal, XBrushes.Black, 350, ystart + 5);
                            gfx.DrawString(item.TheoryMin == "0" ? "-" : item.TheoryMin, normal, XBrushes.Black, 380, ystart + 5);
                            gfx.DrawString(item.TheoryTotal.ToString() == "0" ? "-" : item.TheoryTotal.ToString(), normal, XBrushes.Black, item.TheoryTotal.ToString() == "-" ? 420 : 415, ystart + 5);

                            gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                            gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 18);
                            gfx.DrawString(item.TheoryIAMax, normal, XBrushes.Black, 350, ystart + 18);
                            gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                            gfx.DrawString(item.TheoryIATotal.ToString(), normal, XBrushes.Black, item.TheoryIATotal.ToString() == "-" ? 420 : 415, ystart + 18);

                            gfx.DrawLine(XPens.Black, 300, ystart + 20, 445, ystart + 20);

                            ///////////////////PRACTICAL////////////

                            gfx.DrawString("Practical", normal, XBrushes.Black, 305, ystart + 30);
                            gfx.DrawString(item.PracticalMax, normal, XBrushes.Black, 350, ystart + 30);
                            gfx.DrawString(item.PracticalMin, normal, XBrushes.Black, 380, ystart + 30);
                            gfx.DrawString(item.PracticalTotal.ToString(), normal, XBrushes.Black, item.PracticalTotal.ToString() == "-" ? 420 : 415, ystart + 30);

                            gfx.DrawLine(XPens.Black, 300, ystart + 32, 445, ystart + 32);

                            gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 40);
                            gfx.DrawString(item.PracticalIAMax, normal, XBrushes.Black, 350, ystart + 40);
                            gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 40);
                            gfx.DrawString(item.PracticalIATotal.ToString(), normal, XBrushes.Black, item.PracticalIATotal.ToString() == "-" ? 420 : 415, ystart + 40);

                            gfx.DrawLine(XPens.Black, 300, ystart + 42, 445, ystart + 42);

                            ystart += 20;
                        }
                        else if (item.SubjectType == "PRJ")
                        {
                            gfx.DrawString("Project", normal, XBrushes.Black, 305, ystart + 5);
                            gfx.DrawString(item.PracticalMax, normal, XBrushes.Black, 350, ystart + 5);
                            gfx.DrawString(item.PracticalMin, normal, XBrushes.Black, 380, ystart + 5);
                            gfx.DrawString(item.PracticalTotal.ToString(), normal, XBrushes.Black, 415, ystart + 5);

                            gfx.DrawLine(XPens.Black, 300, ystart + 8, 445, ystart + 8);

                            gfx.DrawString("Viva", normal, XBrushes.Black, 305, ystart + 18);
                            gfx.DrawString(item.VivaVoiceMax, normal, XBrushes.Black, 350, ystart + 18);
                            gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 18);
                            gfx.DrawString(item.VivaVoice.ToString(), normal, XBrushes.Black, 415, ystart + 18);

                            gfx.DrawLine(XPens.Black, 300, ystart + 20, 445, ystart + 20);

                            if(int.Parse(item.PracticalIAMarks) > 0)
                            {
                                gfx.DrawString("IA", normal, XBrushes.Black, 305, ystart + 31);
                                gfx.DrawString(item.PracticalIAMax, normal, XBrushes.Black, 350, ystart + 31);
                                gfx.DrawString("-", normal, XBrushes.Black, 385, ystart + 31);
                                gfx.DrawString(item.PracticalIATotal.ToString(), normal, XBrushes.Black, 415, ystart + 31);

                                gfx.DrawLine(XPens.Black, 300, ystart + 32, 445, ystart + 32);
                            }
                        }

                        ystart += 45;

                        if (item.SubjectName != "Co-and Extra Curricular Activities")
                        {

                            gfx.DrawString("Total", normal, XBrushes.Black, 305, ystart);
                            gfx.DrawString(item.SubjectMax.ToString(), normal, XBrushes.Black, item.SubjectMax.ToString().Length == 3 ? 347 : 350, ystart);
                            gfx.DrawString(item.SubjectMin.ToString(), normal, XBrushes.Black, 380, ystart);
                            gfx.DrawString(item.TotalMarks, normalitalic, XBrushes.Black, item.TheoryTotal.ToString() == "-" ? 420 : 415, ystart);

                            gfx.DrawString(item.SubjectCredits, normal, XBrushes.Black, item.SubjectCredits.ToString().Length == 3 ? 455 : 460, ystart);
                            gfx.DrawString(item.SubjectGPA, normal, XBrushes.Black, 490, ystart);
                            gfx.DrawString(item.SubjectGPW, normal, XBrushes.Black, 520, ystart);

                            gfx.DrawLine(XPens.Black, 235, ystart + 5, drawablewidth, ystart + 5);

                            gfx.DrawString(item.YearText, normal, XBrushes.Black, drawablewidth - 50, ystart - 15);

                            if (item.Remarks == "-")
                            {
                                gfx.DrawString(item.Remarks, normal, XBrushes.Black, drawablewidth - 35, ystart);
                            }
                            else
                            {
                                Color col = item.Remarks == "ABSENT" ? Color.Red : Color.Black;
                                col = item.Remarks == "FAIL" ? Color.Red : Color.Black;
                                col = item.Remarks.ToUpper().Contains("PASSES IN") ? Color.Brown : Color.Black;

                                //gfx.DrawString(item.Remarks == "ABSENT" ? "FAIL" : item.Remarks, normal, new SolidBrush(col) , drawablewidth - 45, ystart);
                                if (item.Remarks.ToUpper().Contains("PASSES IN"))
                                {
                                    string[] pass = item.Remarks.Split(' ');
                                    string year = pass[2] + " " + pass[3];

                                    gfx.DrawString("PASSES IN", normal, new SolidBrush(col), drawablewidth - 50, ystart - 15);
                                    gfx.DrawString(year, normal, new SolidBrush(col), drawablewidth - 45, ystart);
                                    
                                }
                                else
                                {
                                    gfx.DrawString(item.Remarks == "ABSENT" ? "FAIL" : item.Remarks, normal, new SolidBrush(col), drawablewidth - 45, ystart);
                                }
                            }
                        }
                        else
                        {
                            gfx.DrawLine(XPens.Black, 235, ystart + 5, drawablewidth, ystart + 5);


                            gfx.DrawString(item.SubjectCredits, normal, XBrushes.Black, item.SubjectCredits.ToString().Length == 3 ? 455 : 460, ystart - 10);
                            gfx.DrawString(item.SubjectGPA, normal, XBrushes.Black, 490, ystart - 10);
                            gfx.DrawString(item.SubjectGPW, normal, XBrushes.Black, 520, ystart - 10);

                            gfx.DrawLine(XPens.Black, 235, ystart + 5, drawablewidth, ystart + 5);

                           // gfx.DrawString(item.YearText, normal, XBrushes.Black, drawablewidth - 50, ystart - 15);

                            if (item.Remarks == "-")
                            {
                                gfx.DrawString(item.Remarks, normal, XBrushes.Black, drawablewidth - 35, ystart - 10);
                            }
                            else
                            {
                                Color col = item.Remarks == "ABSENT" ? Color.Red : Color.Black;
                                col = item.Remarks == "FAIL" ? Color.Red : Color.Black;
                                col = item.Remarks.ToUpper().Contains("PASSES IN") ? Color.Brown : Color.Black;

                                if (item.Remarks.ToUpper().Contains("PASSES IN"))
                                {
                                    gfx.DrawString(item.Remarks == "ABSENT" ? "FAIL" : item.Remarks, normal, new SolidBrush(col), drawablewidth - 45, ystart);
                                }
                                else
                                {
                                    string[] pass = item.Remarks.Split(' ');
                                    string year = pass[2] + " " + pass[3];

                                    gfx.DrawString("PASSES IN", normal, new SolidBrush(col), drawablewidth - 50, ystart - 15);
                                    gfx.DrawString(year, normal, new SolidBrush(col), drawablewidth - 45, ystart);
                                }
                            }
                        }

                        ystart += 10;
                    }



                    groupno++;
                }

                gfx.DrawLine(XPens.Black, xstart, ystart - 5, 250, ystart - 5);
                gfx.DrawLine(XPens.Black, 235, starty + 20, 235, ystart - 5);
                gfx.DrawLine(XPens.Black, 300, starty, 300, ystart - 5);

                gfx.DrawLine(XPens.Black, 342, starty + 20, 342, ystart + 15);
                gfx.DrawLine(XPens.Black, 370, starty + 20, 370, ystart + 15);
                gfx.DrawLine(XPens.Black, 400, starty + 20, 400, ystart + 15);

                gfx.DrawLine(XPens.Black, 445, starty, 445, ystart + 15);

                gfx.DrawLine(XPens.Black, 475, starty + 20, 475, ystart + 15);
                gfx.DrawLine(XPens.Black, 505, starty + 20, 505, ystart + 15);

                // gfx.DrawLine(XPens.Black, 500, starty + 15, 500, ystart + 10);

                gfx.DrawLine(XPens.Black, drawablewidth - 60, starty, drawablewidth - 60, ystart + 15);

                // ystart -= 30;

                gfx.DrawString("GRAND TOTAL", normalbold, XBrushes.Black, 260, ystart + 8);
                gfx.DrawString(datarow.SemesterMax.ToString(), normal, XBrushes.Black, 347, ystart + 8);
                gfx.DrawString(datarow.SemesterTotal.ToString(), normalbold, XBrushes.CornflowerBlue, 415, ystart + 8);

                gfx.DrawString(datarow.SemesterCredits, normalbold, XBrushes.Black, 455, ystart + 8);
                gfx.DrawString(datarow.TotalGP, normalbold, XBrushes.Black, 483, ystart + 8);
                gfx.DrawString(datarow.TotalGPW, normalbold, XBrushes.Black, 514, ystart + 8);

                ystart += 15;

                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                gfx.DrawString("Grand Total (In Words) : ", normal, XBrushes.Black, xstart + 10, ystart + 12);

                if (datarow.SemesterTotal.Value > 0)
                {
                    gfx.DrawString(NumberToWords(datarow.SemesterTotal.Value).ToUpper().Replace("AND", "").Replace("-", " "), normalbold, XBrushes.CornflowerBlue, xstart + 110, ystart + 12);

                }
                else
                {
                    gfx.DrawString("-", normalbold, XBrushes.Black, xstart + 110, ystart + 12);
                }

                ystart += 10;

                string sempercentage = datarow.SemesterPercentage;

                if (datarow.SemesterPercentage.Length > 5)
                {
                    sempercentage = datarow.SemesterPercentage.Substring(0, 5);
                }

                if (datarow.SemesterPercentage.Length == 4)
                {
                    sempercentage = datarow.SemesterPercentage + "0";
                }

                if (datarow.SemesterPercentage.Length == 2)
                {
                    sempercentage = datarow.SemesterPercentage + ".00";
                }

                string semesterGpa = datarow.SemesterGPA;

                if (datarow.SemesterGPA.Length > 4)
                {
                    semesterGpa = datarow.SemesterGPA.Substring(0, 4);
                }

                if (datarow.SemesterGPA.Length == 3)
                {
                    semesterGpa = datarow.SemesterGPA + "0";
                }

                if (datarow.SemesterGPA.Length == 1 && datarow.SemesterGPA != "-")
                {
                    semesterGpa = datarow.SemesterGPA + ".00";
                }

                gfx.DrawLine(XPens.Black, xstart, ystart + 10, drawablewidth, ystart + 10);

                gfx.DrawString("Aggregated Marks : ", normal, XBrushes.Black, xstart + 10, ystart + 20);
                gfx.DrawString(sempercentage, normalbold, XBrushes.Black, xstart + 90, ystart + 20);

                gfx.DrawString("Grade Point Average : ", normal, XBrushes.Black, 260, ystart + 20);
                gfx.DrawString(semesterGpa, normalbold, XBrushes.Black, 350, ystart + 20);

                gfx.DrawString("Alpha Grade : ", normal, XBrushes.Black, 450, ystart + 20);
                gfx.DrawString(datarow.SemesterAlphaSign, normalbold, XBrushes.Black, 510, ystart + 20);

                ystart += 25;

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                gfx.DrawLine(XPens.Black, xstart, starty, xstart, ystart + 5);
                gfx.DrawLine(XPens.Black, drawablewidth, starty, drawablewidth, ystart + 5);

                ystart += 10;

                var oldresults = entiteis.OldResultMaps.Where(d => d.RegisterNumber == regno).OrderBy(d => d.YearSem);

                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                int sty = ystart;

                gfx.DrawString("Semester", normalbold, XBrushes.Black, xstart + 10, ystart + 10);
                gfx.DrawString("Total Marks of", normalbold, XBrushes.Black, xstart + 80, ystart + 10);
                gfx.DrawString("   Semester", normalbold, XBrushes.Black, xstart + 80, ystart + 20);

                gfx.DrawString("Total Marks", normalbold, XBrushes.Black, xstart + 170, ystart + 10);
                gfx.DrawString("  Secured", normalbold, XBrushes.Black, xstart + 170, ystart + 20);

                gfx.DrawString("Semester GPA", normalbold, XBrushes.Black, xstart + 250, ystart + 10);
                gfx.DrawString("Semester Credits", normalbold, XBrushes.Black, xstart + 350, ystart + 10);

                gfx.DrawString(" Semester", normalbold, XBrushes.Black, xstart + 450, ystart + 10);
                gfx.DrawString(" Weightage", normalbold, XBrushes.Black, xstart + 450, ystart + 20);

                gfx.DrawString("Remarks", normalbold, XBrushes.Black, drawablewidth - 50, ystart + 10);


                gfx.DrawLine(XPens.Black, xstart, ystart + 30, drawablewidth, ystart + 30);

                ystart += 30;


                float totsemmax = 0;
                float totsemmarks = 0;
                float totalgpa = 0;
                float totalcredits = 0;
                int totalweightage = 0;


                foreach (var oldres in oldresults)
                {
                    //Get the current data
                    int semmarks = 0;
                    float semgpa = 0.0F;
                    int semcredits = 0;
                    int semweightage = 0;



                    gfx.DrawString(oldres.YearSem.ToString(), normal, XBrushes.Black, xstart + 10, ystart + 15);
                    gfx.DrawString(oldres.SemsterMax.ToString(), normal, XBrushes.Black, xstart + 100, ystart + 15);

                    gfx.DrawString((oldres.Remarks == "FAIL" ? "-" : oldres.SemesterMarks.ToString()), normal, XBrushes.Black, xstart + 190, ystart + 15);

                    gfx.DrawString(oldres.Remarks == "FAIL" ? "-" : Math.Round(oldres.SemesterGPA, 2).ToString().Length == 1 ? Math.Round(oldres.SemesterGPA, 2).ToString() + ".00" : Math.Round(oldres.SemesterGPA, 2).ToString(), normal, XBrushes.Black, xstart + 270, ystart + 15);
                    gfx.DrawString(oldres.Remarks == "FAIL" ? "-" : oldres.SemesterCredits.ToString(), normal, XBrushes.Black, xstart + 370, ystart + 15);

                    gfx.DrawString(oldres.Remarks == "FAIL" ? "-" : Math.Round((double)oldres.SemesterWeightage, MidpointRounding.AwayFromZero).ToString(), normal, XBrushes.Black, xstart + 470, ystart + 15);

                    if (oldres.Remarks.ToUpper().Contains("PASSES IN"))
                    {
                        string[] pass = oldres.Remarks.Split(' ');
                        string year = pass[2] + " " + pass[3];

                        gfx.DrawString("PASSES IN", normal, XBrushes.Brown, xstart + 533, ystart + 15);
                        gfx.DrawString(year, normal, XBrushes.Brown, xstart + 533, ystart + 23);
                        ystart += 5;
                    }
                    else
                    {
                        gfx.DrawString(oldres.Remarks == "-" ? "FAIL" : oldres.Remarks, normal, oldres.Remarks == "PASS" ? XBrushes.Black : XBrushes.Red, drawablewidth - 50, ystart + 15);
                    }
                    
                    ystart += 10;

                    gfx.DrawLine(XPens.Black, xstart, ystart + 10, drawablewidth, ystart + 10);
                    ystart += 5;

                    totsemmarks += oldres.SemesterMarks;
                    totalgpa += (float)Math.Round(oldres.SemesterGPA, 2);
                    totalcredits += oldres.SemesterCredits;
                    totalweightage += (int)Math.Round((double)oldres.SemesterWeightage, MidpointRounding.AwayFromZero);
                    totsemmax += oldres.SemsterMax;

                }



                ystart += 15;

                gfx.DrawString("GRAND TOTAL", normal, XBrushes.Black, xstart + 10, ystart);
                gfx.DrawString(oldresults.Any(d => d.Remarks == "FAIL") ? "-" : totsemmax.ToString(), normalbold, XBrushes.Black, xstart + 97, ystart);

                gfx.DrawString(oldresults.Any(d => d.Remarks == "FAIL") ? "-" : totsemmarks.ToString(), normalbold, XBrushes.Black, xstart + 187, ystart);

                gfx.DrawString(oldresults.Any(d => d.Remarks == "FAIL") ? "-" : totalgpa.ToString(), normalbold, XBrushes.Black, xstart + 270, ystart);
                gfx.DrawString(oldresults.Any(d => d.Remarks == "FAIL") ? "-" : totalcredits.ToString(), normalbold, XBrushes.Black, xstart + 370, ystart);

                gfx.DrawString(oldresults.Any(d => d.Remarks == "FAIL") ? "-" : totalweightage.ToString(), normalbold, XBrushes.Black, xstart + 470, ystart);

                bool issem6 = oldresults.First(d => d.YearSem == 6).Remarks == "PASS";
                bool isncl = oldresults.Any(d => d.YearSem != 6 && d.Remarks == "FAIL");
                bool iscoursefail = oldresults.Any(d => d.Remarks == "FAIL");


                if (issem6 && isncl)
                {
                    gfx.DrawString("NCL", normalbold, XBrushes.Red, drawablewidth - 50, ystart);
                }
                else
                {
                    gfx.DrawString(oldresults.Any(d => d.Remarks == "FAIL" || d.Remarks == "-") ? "FAIL" : "PASS", normalbold, oldresults.Any(d => d.Remarks == "FAIL") ? XBrushes.Red : XBrushes.Black, drawablewidth - 50, ystart);
                }
                ystart += 10;

                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);



                gfx.DrawLine(XPens.Black, 85, sty, 85, ystart);
                gfx.DrawLine(XPens.Black, 160, sty, 160, ystart);
                gfx.DrawLine(XPens.Black, 240, sty, 240, ystart);
                gfx.DrawLine(XPens.Black, 340, sty, 340, ystart);
                gfx.DrawLine(XPens.Black, 440, sty, 440, ystart);
                gfx.DrawLine(XPens.Black, drawablewidth - 60, sty, drawablewidth - 60, ystart);

                ystart += 10;



                float semesterAggragate = (float)Math.Round(((totsemmarks / totsemmax) * 100), 2);
                string semesterAlphaSign = "FAIL";

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

                string semesterClass = "";

                if (semesterAggragate >= 70)
                    semesterClass = "FIRST CLASS WITH DISTINCTION";
                if (semesterAggragate < 70 && semesterAggragate >= 60)
                    semesterClass = "FIRST CLASS";
                if (semesterAggragate < 60 && semesterAggragate >= 55)
                    semesterClass = "HIGH SECOND CLASS";
                if (semesterAggragate < 55 && semesterAggragate >= 50)
                    semesterClass = "SECOND CLASS";
                if (semesterAggragate < 50 && semesterAggragate >= 40)
                    semesterClass = "PASS CLASS";

                gfx.DrawString("GRAND TOTAL IN WORDS : " + (oldresults.Any(d => d.Remarks == "FAIL") ? "-" : NumberToWords((int)totsemmarks).ToUpper().Replace(" AND ", " ").Replace("-", " ")), normal, XBrushes.Black, xstart + 10, ystart);

                gfx.DrawLine(XPens.Black, xstart, ystart + 5, drawablewidth, ystart + 5);

                int footery = ystart + 5;

                ystart += 10;

                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                semesterClass = textInfo.ToTitleCase(semesterClass.ToLower());

                gfx.DrawString("Aggregate Percentage of Marks : " + (oldresults.Any(d => d.Remarks == "FAIL") ? "-" : semesterAggragate.ToString()), normalex, XBrushes.Black, xstart + 10, ystart + 5);
                gfx.DrawString("Classification of Result : " + (oldresults.Any(d => d.Remarks == "FAIL") ? "-" : semesterClass), normalex, XBrushes.Black, middlepoint + 20, ystart + 5);

                ystart += 10;

                float semesavgpa = (float)Math.Round((double)(totalweightage / totalcredits), 2);

                gfx.DrawLine(XPens.Black, xstart, ystart, drawablewidth, ystart);

                gfx.DrawString("Programme Alpha Sign Grade : " + (oldresults.Any(d => d.Remarks == "FAIL") ? "-" : semesterAlphaSign), normalex, XBrushes.Black, xstart + 10, ystart + 10);
                gfx.DrawString("Aggregate Grade Point Average:  " + (oldresults.Any(d => d.Remarks == "FAIL") ? "-" : semesavgpa.ToString()), normalex, XBrushes.Black, middlepoint + 20, ystart + 10);

                gfx.DrawLine(XPens.Black, xstart, ystart + 15, drawablewidth, ystart + 15);
                gfx.DrawLine(XPens.Black, middlepoint, footery, middlepoint, ystart + 15);

                ystart += 10;

                gfx.DrawLine(XPens.Black, xstart, sty, xstart, ystart + 5);
                gfx.DrawLine(XPens.Black, drawablewidth, sty, drawablewidth, ystart + 5);

                ystart += 20;

                gfx.DrawString("Cr = Credit                                        GP = Grade Point                                        GPW = Grade Point Weightage                            AB = Absent", normal, XBrushes.Black, xstart + 10, ystart + 15);



                gfx.DrawString("Transferred to College Records", normalex, XBrushes.Black, xstart + 10, ystart + 40);
                gfx.DrawString("Signature of the Principal", normalex, XBrushes.Black, xstart + 20, ystart + 100);
                gfx.DrawString("(With Seal)", normalex, XBrushes.Black, xstart + 40, ystart + 110);
                gfx.DrawString("Registrar (Evaluation)", normalex, XBrushes.Black, drawablewidth - 120, ystart + 110);
            }




            // document.Save(@"c:\temp\test\" + regno + ".pdf");
        }

        public static Image resizeImage(int newWidth, int newHeight, Image imgPhoto)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = newHeight;
                newHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)newHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((newWidth -
                      (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((newHeight -
                      (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
                      PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                     imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode =
                InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}