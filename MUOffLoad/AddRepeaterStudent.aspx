<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AddRepeaterStudent.aspx.cs" Inherits="MUOffLoad.AddRepeaterStudent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:panel runat="server" CssClass="alert alert-danger alert-dark alert-page" ID="pnlMessagePanel" Visible="False">
         <asp:Label ID="lblMessageBox" runat="server" Text="[]"></asp:Label>
    </asp:panel>
    <div class="panel panel-default">
        <div class="panel-heading">
            Add Mising Student
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
                        <asp:Button ID="btnSearchStudent" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="btnSearchStudent_Click" />
                    </td>
                </tr>
            </table>

            <asp:panel runat="server" ID="pnlStudentDetails" Visible="False" CssClass="panel panel-info">
                <div class="panel-heading">
                     Student Details
                </div>
                <div class="panel-body">
                    <table class="table">
                        <tr>
                            <td class="auto-style3"><strong>STUDENT NAME</strong></td>
                            <td>

                                <asp:Label ID="lblStudentName" runat="server" Text="[Student Name]"></asp:Label>

                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style3"><strong>REGISTER NUMBER</strong></td>
                            <td>
                                <asp:Label ID="lblRegisterNumber" runat="server" Text="[Register Number]"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style3"><strong>COLLEGE NAME /  CODE</strong></td>
                            <td>
                                <asp:Label ID="lblCollegeName" runat="server" Text="[College Name]"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:panel>
            <asp:panel runat="server" ID="pnlSubjectsList"  CssClass="panel panel-default" Visible="False">
                 <div class="panel-heading">
                     Student Details
                </div>
                <div class="panel-body">
                    <table class="table">
                        <tr>
                            <td class="auto-style2" style="width: 200px">
                                Semester
                            </td>
                            <td class="auto-style2">
                                Subjects
                            </td>
                            <td style="width: 150px"></td>
                        </tr>
                        <tr>
                            <td>

                                <asp:DropDownList ID="ddlSemester" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlSemester_SelectedIndexChanged">
                                    <asp:ListItem Value="-1">Select Semesters</asp:ListItem>
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
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlSubjectList"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button runat="server" CssClass="btn btn-primary" Text="Add Subject" ID="btnAddSubjectToRepeaterDetails" OnClick="btnAddSubjectToRepeaterDetails_Click"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="gvStudentCurrentSubjects" CssClass="table" runat="server" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="Semester" HeaderText="Semester">
                                        <HeaderStyle Width="200px" />
                                        <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SubjectName" HeaderText="Subject Name" />
                                        <asp:BoundField DataField="SubjectCode" HeaderText="Subject Code" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </div>
            </asp:panel>
        </div>
    </div>
</asp:Content>
