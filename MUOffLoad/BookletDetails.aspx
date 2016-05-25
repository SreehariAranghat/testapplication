<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="BookletDetails.aspx.cs" Inherits="MUOffLoad.BookletDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style2 {
            width: 144px;
        }
        .auto-style3 {
            width: 151px;
        }
        .auto-style4 {
            width: 417px;
        }
        .auto-style5 {
            width: 163px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:panel runat="server" ID="pnlStudentDataUpdate" Visible="False" CssClass="panel panel-info">
                 <div class="panel-heading">
                     Booklet Received&nbsp; Details
                </div>
                 <div class="panel-body" style="padding:0px;">
                     <table class="table">
                         <tr>
                             <td class="auto-style2">Date</td>
                             <td class="auto-style3">CenterCode</td>
                             <td class="auto-style4">CenterName</td>
                             <td class="auto-style5">SubjectCode</td>
                             <td>Count</td>
                         </tr>
                          <tr>
                             <td class="auto-style2">
                                 <asp:TextBox ID="txtDate" CssClass="form-control" runat="server" Text="DD/MM/YYYY" Width="113px"></asp:TextBox>
                             </td>
                             <td class="auto-style3">
                                 <asp:TextBox ID="txtCenterCode" CssClass="form-control" runat="server"></asp:TextBox>
                             </td>
                             <td class="auto-style4">
                                 <asp:TextBox ID="txtCenterName" CssClass="form-control" runat="server" Width="381px"></asp:TextBox>
                             </td>
                             <td class="auto-style5">
                                 <asp:TextBox ID="txtSubjectCode" CssClass="form-control" runat="server"></asp:TextBox>
                             </td>
                             <td>
                                 <asp:TextBox ID="txtCount" CssClass="form-control" runat="server" Width="78px"></asp:TextBox>
                             </td>

                              <td>
                                    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add" CausesValidation="False" OnClick="btnAdd_Click" />
                              </td>
                         </tr>
                         </table>
                     <br />
                     <br />
                    <asp:GridView ID="gridData" runat="server" CssClass="table"></asp:GridView>
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
