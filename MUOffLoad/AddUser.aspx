<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="MUOffLoad.AddUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:panel runat="server" CssClass="alert alert-danger alert-dark alert-page" ID="pnlMessagePanel" Visible="False">
         <asp:Label ID="lblMessageBox" runat="server" Text="[]"></asp:Label>
    </asp:panel>
    <div class="panel panel-default">
       <div class="panel-heading">
           Add New User
       </div>
        <div class="panel-body" style="padding:0px;">
            <table class="table">
                <tr>
                    <td>First Name *</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>User Name *</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Password</td>
                </tr>
                <tr>
                    <td>

                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>

                    </td>
                </tr>
                         </table>
        </div>
        <div class="panel-footer">
            <div style="text-align:right">
                 <asp:Button ID="btnAddUser" runat="server" CssClass="btn btn-primary" Text="Add User" OnClick="btnAddUser_Click" />
            </div>
        </div>
    </div>
</asp:Content>
