<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="ThumbsUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AD Auth test</title>
</head>
<body>
    <form id="Login" method="post" runat="server">
        <asp:Label id="Label1" runat="server">Domain:</asp:Label>
        <asp:TextBox id="txtDomain" runat="server"></asp:TextBox><br />
        <asp:Label ID="Label2" runat="server">Username:</asp:Label>
        <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox><br>
        <asp:Label ID="Label3" runat="server">Password:</asp:Label>
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox><br>
        <asp:Button ID="btnLogin" runat="server" OnClick="Login_Click" Text="Login" /><br>
        <asp:Label ID="errorLabel" runat="server" ForeColor="#ff3300"></asp:Label><br>
        <asp:CheckBox ID="chkPersist" runat="server" Text="Persist Cookie" />
    </form>
</body>
</html>
<script runat="server">
    void Page_Load(object sender, EventArgs e)
    {
        txtDomain.Text = "classroom";
    }
    void Login_Click(object sender, EventArgs e)
    {
        string adPath = "LDAP://" + txtDomain.Text;

        LdapAuthentication adAuth = new LdapAuthentication(adPath);
        try
        {
            if (true == adAuth.IsAuthenticated(txtDomain.Text, txtUsername.Text, txtPassword.Text))
            {
                string groups = adAuth.GetGroups();

                //create the ticket, and add the groups
                bool isCookiePersistent = chkPersist.Checked;
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, txtUsername.Text, DateTime.Now, DateTime.Now.AddMinutes(60), isCookiePersistent, groups);

                //encrypt the ticket
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                //create a cookie, and then add the encrypted ticket to the cookie as data.
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                if (true == isCookiePersistent)
                {
                    authCookie.Expires = authTicket.Expiration;
                }

                //add the cookie to the outgoing cookies collection
                Response.Cookies.Add(authCookie);

                //you can redirect now
                Response.Redirect(FormsAuthentication.GetRedirectUrl(txtUsername.Text, false));
            }
            else
            {
                errorLabel.Text = "Authentication did not succeed. Check your username and password.";
            }
        }
        catch (Exception ex)
        {
            errorLabel.Text = "Error authenticating. " + ex.Message;
        }
    }
</script>