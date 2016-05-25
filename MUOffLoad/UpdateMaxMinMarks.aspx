<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="UpdateMaxMinMarks.aspx.cs" Inherits="MUOffLoad.UpdateMaxMinMarks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:panel runat="server" CssClass="alert alert-danger alert-dark alert-page" ID="pnlMessagePanel" Visible="False">
         <asp:Label ID="lblMessageBox" runat="server" Text="[]"></asp:Label>
    </asp:panel>
    <div class="panel panel-default">
        <div class="panel-heading">
            Update Maximum and Minimum Marks
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
    </table>
        </div>
        <asp:panel runat="server" ID="pnlSubjectMaxMinUpdate" Visible="False" CssClass="panel panel-info">
                 <div class="panel-heading">
                     Subject Details
                </div>
                <div class="panel-body">
                   <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table">
                     <Columns>

                         <asp:TemplateField HeaderText ="SubjectName">
                             <ItemTemplate>
                                <asp:Label runat="server" ID="lblSubjectName" Text='<%# Eval("Name") %>'></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                         <asp:TemplateField HeaderText ="SubjectCode">
                             <ItemTemplate>
                                 <asp:Label ID="lblSubjectCode" runat="server" Text='<%# Eval("Code") %>'></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                         <asp:TemplateField HeaderText ="ComponentID">
                             <ItemTemplate>
                                 <asp:Label ID="lblComponentID" runat="server" Text='<%# Eval("SubjectComponentId") %>'></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                         <asp:TemplateField HeaderText ="ComponentName">
                             <ItemTemplate>
                                 <asp:Label ID="lblComponentName" runat="server" Text='<%# Eval("SubjectCCName") %>'></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                         <asp:TemplateField HeaderText ="Max">
                             <ItemTemplate>
                                 <asp:TextBox ID="txtMax" runat="server" Text='<%# Eval("Max") %>'></asp:TextBox>
                             </ItemTemplate>
                         </asp:TemplateField>
                         <asp:TemplateField HeaderText ="Min">
                             <ItemTemplate>
                                 <asp:TextBox ID="txtMin" runat="server" Text='<%# Eval("Min") %>'></asp:TextBox>
                             </ItemTemplate>
                         </asp:TemplateField>
                         
                    </Columns>
                  </asp:GridView>
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
    </div>
</asp:Content>
