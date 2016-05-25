<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="SubjectWiseReport.aspx.cs" Inherits="MUOffLoad.SubjectWiseReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
       <%-- <tr>
            <td><asp:Button ID="Button1" runat="server" Text="Generate Report" OnClick="Button1_Click" /></td>
        </tr>--%>
        <tr>
            <td>
                
                <rsweb:ReportViewer ID="rptExam" runat="server" ProcessingMode="Remote" Width="100%" ShowParameterPrompts="false">
                </rsweb:ReportViewer>
            </td>
        </tr>
    </table>
</asp:Content>
