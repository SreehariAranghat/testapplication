<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MUOffLoad.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" style="padding:50px;">
              <div class="panel panel-default">
                  <div class="panel-heading">Sign In</div>
                  <div class="panel-body">
                      <table class="table">
                          <tr>
                              <td colspan="2" style="text-align: center">
                                  <asp:Label ID="lblLoginMessageBox" runat="server" ForeColor="#CC0000"></asp:Label>
                              </td>
                          </tr>
                          <tr>
                              <td>User Name</td>
                              <td>

                                  <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control"></asp:TextBox>

                              </td>
                          </tr>
                          <tr>
                              <td>Password</td>
                              <td>

                                  <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" CssClass="form-control"></asp:TextBox>

                              </td>
                          </tr>
                          <tr>
                              <td>&nbsp;</td>
                              <td style="text-align: right">

                                  <asp:Button ID="btnSignIn" runat="server" Text="Sign In" CssClass="btn btn-primary" OnClick="btnSignIn_Click" />

                              </td>
                          </tr>
                      </table>
                  </div>
              </div>
    </form>
</body>
</html>
