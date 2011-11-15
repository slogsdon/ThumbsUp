<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="Index.aspx.cs" Inherits="ThumbsUp.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPlcHldr" runat="server">
    <form id="Form1" method="post" runat="server">
    <asp:Label ID="Message" runat="server" />
    <asp:AccessDataSource ID="SubmissionsAccessDS" runat="server" DataFile="~/db.mdb" />
    <asp:PlaceHolder ID="SubmissionsPlcHldr" runat="server" />
    </form>
</asp:Content>