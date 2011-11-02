<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="~/Login.aspx.cs" Inherits="ThumbsUp.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AD Auth test</title>
</head>
<body>
    <form id="Login" method="post" runat="server">
        <asp:HiddenField ID="txtDomain" runat="server" />
        <asp:Label ID="Label2" runat="server">Username:</asp:Label>
        <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox><br />
        <asp:Label ID="Label3" runat="server">Password:</asp:Label>
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox><br />
        <asp:Button ID="btnLogin" runat="server" OnClick="Login_Click" Text="Login" /><br />
        <asp:Label ID="errorLabel" runat="server" ForeColor="#ff3300"></asp:Label><br />
        <asp:CheckBox ID="chkPersist" runat="server" Text="Persist Cookie" />
    </form>
</body>
</html>