<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="UpdateSubjectName.aspx.cs" Inherits="MUOffLoad.UpdateSubjectName" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="alert alert-danger alert-dark alert-page">
         <asp:Label ID="lblMessageBox" runat="server" Text="[]"></asp:Label>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Update Existing Subject Name
        </div>
        <div class="panel-body" style="padding:0px;">
              <table class="table">
      <tr>
          <td class="auto-style1">
              Subject Code
          </td>
          <td>

              <asp:TextBox ID="txtSubejctCode" CssClass="form-control" runat="server"></asp:TextBox>

          </td>
          <td>

              <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-default" Text="Search" OnClick="btnSearch_Click" Width="230px" />

          </td>
      </tr>
      <tr>
          <td class="auto-style1">
              Subject Name</td>
          <td>

                <asp:TextBox ID="txtNewSubjectName" CssClass="form-control" runat="server"></asp:TextBox>

          </td>
          <td>

                 <asp:Button ID="btnUpdate" runat="server" CssClass="btn  btn-primary" Text="Update Subject Name" OnClick="btnUpdate_Click" Width="230px" />

          </td>
      </tr>
    </table>
        </div>
        <div class="panel-footer">
            <div style="text-align:right">
            </div>
        </div>
    </div>
</asp:Content>
