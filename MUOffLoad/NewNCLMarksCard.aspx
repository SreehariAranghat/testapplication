<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="NewNCLMarksCard.aspx.cs" Inherits="MUOffLoad.NewNCLMarksCard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:panel runat="server" CssClass="alert alert-danger alert-dark alert-page" ID="pnlMessagePanel" Visible="False">
         <asp:Label ID="lblMessageBox" runat="server" Text="[]"></asp:Label>
    </asp:panel>
    <div class="panel panel-default">
        <div class="panel-heading">
            Update Student Details
        </div>
        <div class="panel-body" style="padding:0px;">
            <table class="table">
                <tr>
                    <td>
                        
                        REGISTER NUMBER :</td>
                    <td>
                        <asp:TextBox ID="txtRegisterNumber" placeholder="Give Student Register Number" runat="server" class="form-control"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnSearchStudent" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="btnSearchStudent_Click" CausesValidation="False" />
                    </td>
                </tr>
            </table>

            
        </div>
    </div>
    <asp:panel runat="server" ID="pnlStudentDetails" Visible="False" CssClass="panel panel-info">
                <div class="panel-heading">
                     Student Details
                </div>
                <div class="panel-body">
                    <table class="table">
                        <tr>
                            <td class="auto-style1"><strong>STUDENT NAME</strong></td>
                            <td>

                                <asp:Label ID="lblStudentName" runat="server" Text="[Student Name]"></asp:Label>

                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1"><strong>REGISTER NUMBER</strong></td>
                            <td>
                                <asp:Label ID="lblRegisterNumber" runat="server" Text="[Register Number]"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1"><strong>COLLEGE NAME /  CODE</strong></td>
                            <td>
                                <asp:Label ID="lblCollegeName" runat="server" Text="[College Name]"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
       </asp:panel>
     <asp:panel runat="server" ID="pnlStudentDataUpdate" Visible="False" CssClass="panel panel-info">
                 <div class="panel-heading">
                     Subject&nbsp; Details
                </div>
                 <div class="panel-body" style="padding:0px;">
                     <table class="table">
                         <tr>
                             <td>Subject Name</td>
                             <td>Code</td>
                             <td>Type</td>
                             <td></td>
                         </tr>
                          <tr>
                             <td>
                                 <asp:TextBox ID="txtSubjectName" CssClass="form-control" runat="server"></asp:TextBox>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtSubjectCode" CssClass="form-control" runat="server"></asp:TextBox>
                             </td>
                               <td>
                                 <asp:TextBox ID="txtSubjectType" CssClass="form-control" runat="server"></asp:TextBox>
                             </td>
                              <td>
                                    <asp:Button  runat="server" Text="Search" ID="btnSearchSubjectDetails" CssClass="btn btn-default" OnClick="btnSearchSubjectDetails_Click" CausesValidation="False"></asp:Button>
                              </td>
                         </tr>
                         <tr>
                             <td>TheoryMarks</td>
                             <td>Theory Min</td>
                             <td>Theory Max</td>
                             <td>Theory IA Marks</td>
                             <td>Theory IA Min</td>
                             <td>Theory IA Max</td>
                             <td>Theory Net Total</td>
                      </tr>
                          <tr>
                             <td>
                                 <asp:TextBox ID="txtTheoryMarks" CssClass="form-control" runat="server" BorderColor="#99CC00" BorderStyle="Solid" ForeColor="#663300"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ControlToValidate="txtTheoryMarks" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9 -]+$"></asp:RegularExpressionValidator>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtTheoryMin" CssClass="form-in form-control" runat="server" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="#3333FF"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTheoryMin" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtTheoryMax" CssClass="form-control" runat="server" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="#3333FF"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtTheoryMax" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtTheoryIAMarks" CssClass="form-control" runat="server" BorderColor="#99CC00" BorderStyle="Solid" ForeColor="#663300"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator16" runat="server" ControlToValidate="txtTheoryIAMarks" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9 -]+$"></asp:RegularExpressionValidator>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtTheoryIAMin" CssClass="form-control" runat="server" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="#3333FF"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtTheoryIAMin" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtTheoryIAMax" CssClass="form-control" runat="server" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="#3333FF"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtTheoryMax" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                             </td>
                              <td>
                                 <asp:TextBox ID="txtTheoryNetTotal" CssClass="form-control" runat="server" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="#3333FF"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator18" runat="server" ControlToValidate="txtTheoryNetTotal" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                             </td>
                         </tr>
                         <tr>
                             <td>Practial Marks</td>
                             <td>Practial Min</td>
                             <td>Practial Max</td>
                             <td>Practial IA Marks</td>
                             <td>Practial IA Min</td>
                             <td>Practial IA Max</td>
                             <td>Practial Net Total</td>
                        </tr>
                          <tr>
                             <td>
                                 <asp:TextBox ID="txtPractialMarks" CssClass="form-control" runat="server" BorderColor="#99CC00" BorderStyle="Solid" ForeColor="#663300"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server" ControlToValidate="txtPractialMarks" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9 -]+$"></asp:RegularExpressionValidator>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtPracticalMin" CssClass="form-control" runat="server" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="#3333FF"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtPracticalMin" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtPractialMax" CssClass="form-control" runat="server" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="#3333FF"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtTheoryMax" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtPractialIAMarks" CssClass="form-control" runat="server" BorderColor="#99CC00" BorderStyle="Solid" ForeColor="#663300"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator17" runat="server" ControlToValidate="txtPractialIAMarks" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9 -]+$"></asp:RegularExpressionValidator>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtPractialIAMin" CssClass="form-control" runat="server" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="#3333FF"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtPractialIAMin" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtPractialIAMax" CssClass="form-control" runat="server" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="#3333FF"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtPractialIAMAx" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                             </td>
                               <td>
                                 <asp:TextBox ID="txtPractialNetTotal" CssClass="form-control" runat="server" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="#3333FF"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator19" runat="server" ControlToValidate="txtPractialNetTotal" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                             </td>
                             </tr>
                         <tr>
                             <td>Viva Voce</td>
                             <td>Viva Voc Min</td>
                             <td>Viva Voc Max</td>
                        </tr>
                         
                             <tr>
                                 <td>
                                     <asp:TextBox ID="txtVivaVoice" CssClass="form-control" runat="server" BorderColor="#99CC00" BorderStyle="Solid" ForeColor="#663300"></asp:TextBox>
                                     <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server" ControlToValidate="txtVivaVoice" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9 -]+$"></asp:RegularExpressionValidator>
                                 </td>
                                 <td>
                                     <asp:TextBox ID="txtVivaVoiceMin" CssClass="form-control" runat="server" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="#3333FF"></asp:TextBox>
                                     <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txtVivaVoiceMin" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                                 </td>
                                 <td>
                                     <asp:TextBox ID="txtVivaVoiceMax" CssClass="form-control" runat="server" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="#3333FF"></asp:TextBox>
                                     <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ControlToValidate="txtVivaVoiceMax" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                                 </td>
                           </tr>
                         <tr>
                             <td>Total Marks</td>
                             <td>Credits</td>
                             <td>GPA</td>
                             <td>GPW</td>
                             <td></td>
                         </tr>
                            <tr>
                             <td>
                                 <asp:TextBox ID="txtTotalMarks" runat="server" BorderColor="#CC6600" BorderStyle="Solid" CssClass="form-control" ForeColor="#FF6666"></asp:TextBox>
                                </td>
                             <td>
                                 <asp:TextBox ID="txtCredits" runat="server" BorderColor="#CC6600" BorderStyle="Solid" CssClass="form-control" ForeColor="#FF6666"></asp:TextBox>
                                </td>
                             <td>
                                 <asp:TextBox ID="txtGPA" runat="server" BorderColor="#CC6600" BorderStyle="Solid" CssClass="form-control" ForeColor="#FF6666"></asp:TextBox>
                                </td>
                             <td>
                                 <asp:TextBox ID="txtGPW" runat="server" BorderColor="#CC6600" BorderStyle="Solid" CssClass="form-control" ForeColor="#FF6666"></asp:TextBox>
                                </td>
                                <td>

                                    <asp:Button ID="btnCalculate" runat="server" CausesValidation="False" CssClass="btn btn-primary" OnClick="btnCalculate_Click" Text="Calculate" />

                                </td>
                         </tr>
                         <tr>
                             <td>Subject Min</td>
                             <td>Subject Max</td>
                             <td>Examination Month Year</td>
                             <td>Remarks</td>
                             <td></td>
                         </tr>
                            <tr>
                             <td>
                                 <asp:TextBox ID="txtSubjectMin" CssClass="form-control" runat="server" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="#3333FF"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ControlToValidate="txtSubjectMin" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSubjectMin" Display="Dynamic" ErrorMessage="* Mandatory" ForeColor="Red"></asp:RequiredFieldValidator>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtSubjectMax" CssClass="form-control" runat="server" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" ForeColor="#3333FF"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ControlToValidate="txtSubjectMax" Display="Dynamic" ErrorMessage="* Invalid " ForeColor="Red" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSubjectMax" Display="Dynamic" ErrorMessage="* Mandatory" ForeColor="Red"></asp:RequiredFieldValidator>
                             </td>
                                <td>

                                    <asp:TextBox ID="txtExaminationMonthYear" runat="server" CssClass="form-control"></asp:TextBox>

                                </td>
                             <td>
                                 <asp:TextBox ID="txtRemarks" CssClass="form-control" runat="server"></asp:TextBox>
                             </td>
                             <td>
                                 <asp:Button ID="btnAddMarks" CssClass="btn btn-primary" runat="server" Text="Add" OnClick="btnAddMarks_Click" />
                             </td>
                         </tr>
                     </table>
                     <br />
                     <br />
                    <asp:GridView ID="gridMarks" runat="server" CssClass="table" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="GroupName" HeaderText="GroupName" SortExpression="GroupName" />
                            <asp:BoundField DataField="SubjectName" HeaderText="SubjectName" SortExpression="SubjectName" />
                            <asp:BoundField DataField="SubjectCode" HeaderText="SubjectCode" SortExpression="SubjectCode" />
                            <asp:BoundField DataField="TheoryMarks" HeaderText="TheoryMarks" SortExpression="TheoryMarks" />
                            <asp:BoundField DataField="TheoryMin" HeaderText="TheoryMin" SortExpression="TheoryMin" />
                            <asp:BoundField DataField="TheoryMax" HeaderText="TheoryMax" SortExpression="TheoryMax" />
                            <asp:BoundField DataField="TheoryTotal" HeaderText="TheoryTotal" SortExpression="TheoryTotal" />
                            <asp:BoundField DataField="TheoryIAMarks" HeaderText="TheoryIAMarks" SortExpression="TheoryIAMarks" />
                            <asp:BoundField DataField="TheoryIAMin" HeaderText="TheoryIAMin" SortExpression="TheoryIAMin" />
                            <asp:BoundField DataField="TheoryIAMax" HeaderText="TheoryIAMax" SortExpression="TheoryIAMax" />
                            <asp:BoundField DataField="TheoryIATotal" HeaderText="TheoryIATotal" SortExpression="TheoryIATotal" />
                            <asp:BoundField DataField="TheoryNetTotal" HeaderText="TheoryNetTotal" SortExpression="TheoryNetTotal" />
                            <asp:BoundField DataField="PracticalMarks" HeaderText="PracticalMarks" SortExpression="PracticalMarks" />
                            <asp:BoundField DataField="PracticalMin" HeaderText="PracticalMin" SortExpression="PracticalMin" />
                            <asp:BoundField DataField="PracticalMax" HeaderText="PracticalMax" SortExpression="PracticalMax" />
                            <asp:BoundField DataField="PracticalTotal" HeaderText="PracticalTotal" SortExpression="PracticalTotal" />
                            <asp:BoundField DataField="PracticalIAMarks" HeaderText="PracticalIAMarks" SortExpression="PracticalIAMarks" />
                            <asp:BoundField DataField="PracticalIAGrace" HeaderText="PracticalIAGrace" SortExpression="PracticalIAGrace" />
                            <asp:BoundField DataField="PracticalIAMin" HeaderText="PracticalIAMin" SortExpression="PracticalIAMin" />
                            <asp:BoundField DataField="PracticalIAMax" HeaderText="PracticalIAMax" SortExpression="PracticalIAMax" />
                            <asp:BoundField DataField="PracticalIATotal" HeaderText="PracticalIATotal" SortExpression="PracticalIATotal" />
                            <asp:BoundField DataField="PracticalNetTotal" HeaderText="PracticalNetTotal" SortExpression="PracticalNetTotal" />
                            <asp:BoundField DataField="VivaVoice" HeaderText="VivaVoice" SortExpression="VivaVoice" />
                            <asp:BoundField DataField="VivaVoiceMin" HeaderText="VivaVoiceMin" SortExpression="VivaVoiceMin" />
                            <asp:BoundField DataField="VivaVoiceMax" HeaderText="VivaVoiceMax" SortExpression="VivaVoiceMax" />
                            <asp:BoundField DataField="SubjectTotal" HeaderText="SubjectTotal" SortExpression="SubjectTotal" />
                            <asp:BoundField DataField="GroupTotal" HeaderText="GroupTotal" SortExpression="GroupTotal" />
                            <asp:BoundField DataField="TotalMarks" HeaderText="TotalMarks" SortExpression="TotalMarks" />
                            <asp:BoundField DataField="SubjectMin" HeaderText="SubjectMin" SortExpression="SubjectMin" />
                            <asp:BoundField DataField="SubjectMax" HeaderText="SubjectMax" SortExpression="SubjectMax" />
                            <asp:BoundField DataField="SubjectCredits" HeaderText="SubjectCredits" SortExpression="SubjectCredits" />
                            <asp:BoundField DataField="SubjectGPA" HeaderText="SubjectGPA" SortExpression="SubjectGPA" />
                            <asp:BoundField DataField="SubjectGPW" HeaderText="SubjectGPW" SortExpression="SubjectGPW" />
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
                            <asp:BoundField DataField="SubjectType" HeaderText="SubjectType" SortExpression="SubjectType" />
                        </Columns>
                     </asp:GridView>
                </div>
        </asp:panel>
       <asp:panel runat="server" ID="Panel2" Visible="False" CssClass="panel panel-info">
                 <div class="panel-heading">
                   Other Details
                </div>
                 <div class="panel-body" style="padding:0px;">
                     <table class="table">
                          <tr>
                             <td>Total Credits</td>
                             <td>Total GPA</td>
                             <td>Total GPW</td>
                         </tr>
                         <tr>
                             <td>
                                 <asp:TextBox ID="txtNetCredits" runat="server" CssClass="form-control"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNetCredits" Display="Dynamic" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="FinalSet"></asp:RequiredFieldValidator>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtNetGPA" runat="server" CssClass="form-control"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtNetGPA" Display="Dynamic" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="FinalSet"></asp:RequiredFieldValidator>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtNetGPW" runat="server" CssClass="form-control"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtGPW" Display="Dynamic" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="FinalSet"></asp:RequiredFieldValidator>
                             </td>
                         </tr>
                         <tr>
                             <td>Semester Max</td>
                             <td>Semester Total</td>
                             <td>Semester Percentage</td>
                         </tr>
                         <tr>
                             <td>
                                 <asp:TextBox ID="txtNetSemesterMax" runat="server" CssClass="form-control"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtNetSemesterMax" Display="Dynamic" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="FinalSet"></asp:RequiredFieldValidator>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtNetSemesterTotal" runat="server" CssClass="form-control"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtNetSemesterTotal" Display="Dynamic" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="FinalSet"></asp:RequiredFieldValidator>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtNetSemesterPercentage" runat="server" CssClass="form-control"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtNetSemesterPercentage" Display="Dynamic" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="FinalSet"></asp:RequiredFieldValidator>
                             </td>
                         </tr>
                         <tr>
                             <td>Semester Class</td>
                             <td>
                                Semester Alpha Sign
                             </td>
                            <td>Semester Credits</td>
                         </tr>
                         <tr>
                              <td>
                                  <%--<asp:TextBox ID="txtNetSemesterClass" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                  <asp:DropDownList ID="ddlSelectSemesterClass" runat="server" CssClass="form-control">
                                      <asp:ListItem Value = "-1"> Select Semester Class</asp:ListItem>
                                      <asp:ListItem Value = "FIRST CLASS WITH DISTINCTION"> FIRST CLASS WITH DISTINCTION </asp:ListItem>
                                      <asp:ListItem Value ="FIRST CLASS"> FIRST CLASS </asp:ListItem>
                                      <asp:ListItem Value ="HIGH SECOND CLASS"> HIGH SECOND CLASS </asp:ListItem>
                                      <asp:ListItem Value ="SECOND CLASS"> SECOND CLASS </asp:ListItem>
                                      <asp:ListItem Value ="PASS CLASS"> PASS CLASS </asp:ListItem>
                                  </asp:DropDownList>
                                  <br />
                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlSelectSemesterClass" Display="Dynamic" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="FinalSet"></asp:RequiredFieldValidator>
                             </td>
                             <td>

                                 <%--<asp:TextBox ID="txtSemesterAlphaSign" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                 <asp:DropDownList ID="ddlSemesterAlphaSign" runat="server" CssClass="form-control">
                                     <asp:ListItem Value ="-1"> Select Alpha Sign</asp:ListItem>
                                     <asp:ListItem Value ="A"> A </asp:ListItem>
                                     <asp:ListItem Value ="A+"> A+ </asp:ListItem>
                                     <asp:ListItem Value ="A++"> A++ </asp:ListItem>
                                     <asp:ListItem Value ="B"> B </asp:ListItem>
                                     <asp:ListItem Value ="B+"> B+ </asp:ListItem>
                                     <asp:ListItem Value ="C"> C </asp:ListItem>
                                     <asp:ListItem Value ="D"> D </asp:ListItem>
                                     <asp:ListItem Value ="O"> O </asp:ListItem>
                                 </asp:DropDownList>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlSemesterAlphaSign" Display="Dynamic" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="FinalSet"></asp:RequiredFieldValidator>

                             </td>
                             <td>

                                 <asp:TextBox ID="txtNetSemesterCredits" runat="server" CssClass="form-control"></asp:TextBox>

                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtNetSemesterCredits" Display="Dynamic" ErrorMessage="* Required" ForeColor="Red" ValidationGroup="FinalSet"></asp:RequiredFieldValidator>

                             </td>
                         </tr>
                         <tr>
                             <td>Semester GPA</td>
                             <td>Print Date</td>
                             <td></td>
                         </tr>
                          <tr>
                              <td>
                                  <asp:TextBox ID="txtSemGPA" runat="server" CssClass="form-control"></asp:TextBox>
                              </td>
                              <td>
                                  <asp:TextBox ID="txtPrintDate" runat="server" CssClass="form-control" placeholder="DD-MMM-YYYY"></asp:TextBox>
                              </td>
                              <td>
                                  <asp:TextBox ID="txtExaminationYear" runat="server" CssClass="form-control">Nov 2015</asp:TextBox>
                              </td>
                              <td>&nbsp;</td>
                              <td>&nbsp;</td>
                          </tr>
                     </table>
                 </div>
        </asp:panel>
     <asp:panel runat="server" ID="Panel1" Visible="False" CssClass="panel panel-info">
                 <div class="panel-heading">
                    Previous Semester Marks
                </div>
                 <div class="panel-body" style="padding:0px;">
                     <table class="table">
                         <tr>
                             <td>Semester</td>
                             <td>Semester Max</td>
                             <td>Marks</td>
                             <td>GPA</td>
                             <td>Credits</td>
                             <td>Semester Weightage</td>
                             <td>Remarks</td>
                             <td></td>
                         </tr>
                         <tr>
                             <td>
                                 <asp:DropDownList ID="ddlSemester" runat="server" CssClass="form-control">
                                     <asp:ListItem>Select Semester</asp:ListItem>
                                     <asp:ListItem>1</asp:ListItem>
                                     <asp:ListItem>2</asp:ListItem>
                                     <asp:ListItem>3</asp:ListItem>
                                     <asp:ListItem>4</asp:ListItem>
                                     <asp:ListItem>5</asp:ListItem>
                                     <asp:ListItem>6</asp:ListItem>
                                     <asp:ListItem>7</asp:ListItem>
                                     <asp:ListItem>8</asp:ListItem>
                                 </asp:DropDownList>
                                 </td>
                                 <td>
                                     <asp:TextBox ID="txtSemesterMax" runat="server" CssClass="form-control"></asp:TextBox>
                                 </td>
                                 <td>
                                     <asp:TextBox ID="txtSemesterMarks" runat="server" CssClass="form-control"></asp:TextBox>
                                 </td>
                                 <td>
                                     <asp:TextBox ID="txtSemesterGPA" runat="server" CssClass="form-control"></asp:TextBox>
                                 </td>
                                 <td>
                                     <asp:TextBox ID="txtSemesterCredits" runat="server" CssClass="form-control"></asp:TextBox>
                                 </td>
                                 <td>
                                     <asp:TextBox ID="txtSemesterWeightage" runat="server" CssClass="form-control"></asp:TextBox>
                                 </td>
                                 <td>
                                     <asp:TextBox ID="txtPreSemRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                                 </td>
                                 <td>

                                     <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add" CausesValidation="False" OnClick="btnAdd_Click" />

                                 </td>
                         </tr>
                     </table>
                     <asp:GridView ID="gridOldResultMap" runat="server" CssClass="table"></asp:GridView>
                 </div>
         </asp:panel>
         <div class="panel panel-success">
             <div class="panel-heading">Save</div>
             <div class="panel-body">
                 <div style="text-align:right">
                     <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="FinalSet" />
                 </div>
             </div>
         </div>
</asp:Content>
