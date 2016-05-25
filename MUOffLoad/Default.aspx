<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MUOffLoad.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="col-lg-4">
    <div class="panel panel-default">
        <div class="panel-heading">
            Manage Subject
        </div>
        <div class="panel-body">
            <ul>
                <li><a href="AddNewSubject.aspx">Add New Subject</a></li>
                <li><a href="UpdateSubjectName.aspx">Update Subject Name</a></li>
                <li><a href="RemoveCourseSubject.aspx">Add / Remove Subjects to Course</a></li>
                <li><a href="UpdateSubjectCode.aspx">Migrate Course Subjects</a></li>
                <li><a href="UpdateMaxMinMarks.aspx">Update Subject MaxMin Marks</a></li>
            </ul>
        </div>
    </div>

</div>
<div class="col-lg-4">
 <div class="panel panel-default">
        <div class="panel-heading">
            Manage Student Master
        </div>
        <div class="panel-body">
            <ul>
                <li><a href="AddRepeaterStudent.aspx">Add Repeat Student</a></li>
                <li><a href="UpdateStudentSemester.aspx">Update Student Semester</a></li>
                <li><a href="UpdateStudentCollege.aspx">Update Student College</a></li>
                <li><a href="UpdateStudentName.aspx">Update Student Name</a></li>
                <li><a href="ReAdmission.aspx">Update Student Readmission Status</a></li>
                <li><a href="ReCreatingBills.aspx">Update Student Bill Submit Status</a></li>
            </ul>
        </div>
    </div>    
</div>
<div class="col-lg-4">
     <div class="panel panel-default">
        <div class="panel-heading">
            Marks Card Printing
        </div>
        <div class="panel-body">
            <ul>
                <li><a href="ConsolidatedMC.aspx">Consolidated Marks Card</a></li>
                <li><a href="MarksCardReprint.aspx">Marks Card Reprint</a></li>
                <li><a href="NewNCLMarksCard.aspx">NCL Marks Card</a></li>
            </ul>
        </div>
    </div>
</div>
<div class="col-lg-4">
    <asp:Panel runat="server" ID="pnlExaminationDetails" Visible="false" class="panel panel-default">
        <div class="panel-heading">
            Examination
        </div>
        <div class="panel-body">
            <ul>
                <li><a href="Revaluation.aspx">PersonalSeeing /  Revaluation</a></li>
                <li><a href="GetScanBatch.aspx">Retrive Scan Batch</a></li>
            </ul>
        </div>
    </asp:Panel>

</div>
 <div class="col-lg-4">
    <asp:Panel runat="server" ID="pnlAdmin" Visible="false" class="panel panel-default">
        <div class="panel-heading">
            User Management
        </div>

        <div class="panel-body">
            <ul>
                <li><a href="AddUser.aspx">Add User</a></li>
            </ul>
        </div>
    </asp:Panel>

</div>
 </asp:Content>