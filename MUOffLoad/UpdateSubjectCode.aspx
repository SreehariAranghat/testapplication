<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="UpdateSubjectCode.aspx.cs" Inherits="MUOffLoad.UpdateSubjectCode" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="alert alert-danger alert-dark alert-page">
         <asp:Label ID="lblMessageBox" runat="server" Text="[]"></asp:Label>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">Migrate Subject Code</div>
        <div class="panel-body" style="padding:0px;">
            <table class="table">
        <tr>
            <td style="width:300px;">Select Degree</td>
            <td>
                <asp:DropDownList ID="ddlSelectDegree" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlSelectDegree_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Select Semester</td>
            <td>
                 <asp:DropDownList ID="ddlSelectSemester" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlSelectSemester_SelectedIndexChanged">
                     <asp:ListItem>Select Semester</asp:ListItem>
                     <asp:ListItem>1</asp:ListItem>
                     <asp:ListItem>2</asp:ListItem>
                     <asp:ListItem>3</asp:ListItem>
                     <asp:ListItem>4</asp:ListItem>
                     <asp:ListItem>5</asp:ListItem>
                     <asp:ListItem>6</asp:ListItem>
                     <asp:ListItem>7</asp:ListItem>
                     <asp:ListItem>8</asp:ListItem>
                     <asp:ListItem>9</asp:ListItem>
                     <asp:ListItem>10</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table class="table">
        <tr>
            <td class="auto-style1">Select Subject Name</td>   
            <td class="auto-style1">Old Subject Code</td>
            <td class="auto-style1">For Academic Year</td>
            <td class="auto-style1">New Subejct Code</td>
        </tr>
        <tr>
            <td>
                 <asp:DropDownList ID="ddlSubjectName" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlSubjectName_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlSubjectCodes" runat="server" class="form-control">
                </asp:DropDownList>
            </td>
            <td>
               <asp:DropDownList ID="ddlAcademicYear" runat="server" class="form-control" AutoPostBack="True">
                   <asp:ListItem Value="-1">Select Academic Year</asp:ListItem>
                   <asp:ListItem Value="4">2015</asp:ListItem>
                   <asp:ListItem Value="3">2014</asp:ListItem>
                   <asp:ListItem Value="2">2013</asp:ListItem>
                   <asp:ListItem Value="1">Earlier to 2013</asp:ListItem>
               </asp:DropDownList>
            </td>
            <td>
                 <asp:DropDownList ID="ddlNewSubjectCodes" runat="server" class="form-control">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
        </div>
        <div class="panel-footer">
            <div style="text-align:right">
                <asp:Button ID="btnMigrate" runat="server" CssClass="btn btn-primary" Text="Migrate" OnClick="btnMigrate_Click" />
            </div>
        </div>
    </div>
</asp:Content>
