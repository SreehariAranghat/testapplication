<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ReCreatingBills.aspx.cs" Inherits="MUOffLoad.ReCreatingBills" %>
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
                        <asp:Button ID="btnSearchStudent" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="btnSearchStudent_Click" />
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
                        <tr>
                            <td class="auto-style1"><strong>CURRENT SEMESTER</strong></td>
                            <td>
                                <asp:Label ID="lblStudentSemester" runat="server" Text="[Current Semester]"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
       </asp:panel>
        <asp:panel runat="server" ID="pnlStudentDataUpdate" Visible="False" CssClass="panel panel-info">
                <div class="panel-footer">
                    <div style="text-align:right">

                        <asp:Button ID="btnSubmitStudent" runat="server" Text="Clear Submitted Bills" CssClass="btn btn-primary" OnClick="btnSubmitStudent_Click" />

                    </div>
                </div>
        </asp:panel>
</asp:Content>
