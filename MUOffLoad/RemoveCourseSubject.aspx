<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="RemoveCourseSubject.aspx.cs" Inherits="MUOffLoad.RemoveCourseSubject" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:panel runat="server" CssClass="alert alert-danger alert-dark alert-page" ID="pnlMessagePanel" Visible="False">
         <asp:Label ID="lblMessageBox" runat="server" Text="[]"></asp:Label>
    </asp:panel>
    <div class="panel panel-default">
        <div class="panel-heading">
            Remove Course Subjects
        </div>
        <div class="panel-body" style="padding:0px;">
            <table class="table">
                <tr>
                        <td>Select Degree</td>
                        <td>Select Course</td>
                        <td>Select Academic Year</td>
                        <td>Select Semester</td>
                        <td>Subject Code</td>
                        <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlSelectDegree" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlSelectDegree_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSelectCourse" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlSelectCourse_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSelectAcademicYear" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlSelectAcademicYear_SelectedIndexChanged">
                            <asp:ListItem Value="-1">Select Academic Year</asp:ListItem>
                            <asp:ListItem Value="4">2015</asp:ListItem>
                            <asp:ListItem Value="3">2014</asp:ListItem>
                            <asp:ListItem Value="2">2013</asp:ListItem>
                            <asp:ListItem Value="1">PRE 2013</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSemester" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlSemester_SelectedIndexChanged">
                            <asp:ListItem Value="-1">Select Semester</asp:ListItem>
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
                        <asp:TextBox ID="txtSubjectCode" runat="server" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnAddCourseSubject" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnAddCourseSubject_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Current Subject List
        </div>
        <div class="panel-body">

            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="CourseSubjectId">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Subject Name" />
                    <asp:BoundField DataField="Code" HeaderText="Code" />
                    
                    <asp:TemplateField>
                       
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" />
                        </ItemTemplate>
                       
                    </asp:TemplateField>
                    
                </Columns>
            </asp:GridView>

        </div>
    </div>
</asp:Content>
