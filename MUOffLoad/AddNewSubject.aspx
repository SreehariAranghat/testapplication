<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AddNewSubject.aspx.cs" Inherits="MUOffLoad.AddNewSubject" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:panel runat="server" CssClass="alert alert-danger alert-dark alert-page" ID="pnlMessagePanel" Visible="False">
         <asp:Label ID="lblMessageBox" runat="server" Text="[]"></asp:Label>
    </asp:panel>
    <div class="panel panel-default">
        <div class="panel-heading">
            Add a New Subject
        </div>
        <div class="panel-body" style="padding:0px;">
              <table class="table">
        <tr>
            <td class="auto-style1">Subject Name</td>
            <td>Subject Code</td>
            <td>Max</td>
            <td>Min</td>
        </tr>
        <tr>
            <td class="auto-style1">
                <asp:TextBox ID="txtSubjectName" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSubjectCode" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSubjectMax" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSubjectMin" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">THEORY MAX</td>
            <td>THEORY MIN</td>
            <td>THEORY IA MAX</td>
            <td>THEORY IA MIN</td>
        </tr>
        <tr>
            <td class="auto-style1">
                <asp:TextBox ID="txtTheoryMax" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtTheoryMin" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtTheoryIAMax" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtTheoryIAMin" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">PRACTICAL MAX</td>
            <td>PRACTIAL MIN</td>
            <td>PRACTIAL IA MAX</td>
            <td>PRACTIAL IA MIN</td>
        </tr>
        <tr>
            <td class="auto-style1">
                <asp:TextBox ID="txtPractialMax" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtPractialMin" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtPractialIAMax" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtPractialIAMin" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
    </table>
        </div>
        <div class="panel-footer">
            <div style="text-align:right">
                  <asp:Button ID="Button1" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="Button1_Click" />
            </div>
        </div>
    </div>
  
</asp:Content>
