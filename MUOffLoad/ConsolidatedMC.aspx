<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ConsolidatedMC.aspx.cs" Inherits="MUOffLoad.ConsolidatedMC" %>
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
                        
                        SEMESTER :</td>
                    <td>
                        <%--<asp:TextBox ID="TextBox1" placeholder="Give Student Register Number" runat="server" class="form-control"></asp:TextBox>--%>
                        <asp:DropDownList ID="ddlSemester" runat="server">
                            <asp:ListItem Value ="1"> 1 </asp:ListItem>
                            <asp:ListItem Value ="2"> 2 </asp:ListItem>
                            <asp:ListItem Value ="3"> 3 </asp:ListItem>
                            <asp:ListItem Value ="4"> 4 </asp:ListItem>
                            <asp:ListItem Value ="5"> 5 </asp:ListItem>
                            <asp:ListItem Value ="6"> 6 </asp:ListItem>
                            <asp:ListItem Value ="7"> 7 </asp:ListItem>
                            <asp:ListItem Value ="8"> 8 </asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnSearchStudent" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="btnSearchStudent_Click" />
                    </td>
                    
                </tr>
            </table>

            
        </div>
    </div>
    
</asp:Content>
