<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="Index.aspx.cs" Inherits="ThumbsUp.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPlcHldr" runat="server">
    <form id="Form1" method="post" runat="server">
        <asp:Label ID="lblName" runat="server" /><br />
        <asp:Label ID="lblAuthType" runat="server" /><br />
        <asp:DropDownList ID="selectOUUsers" runat="server"></asp:DropDownList>
        <asp:RadioButtonList ID="lbRating" runat="server" RepeatDirection="Vertical" RepeatLayout="UnorderedList"></asp:RadioButtonList>
    </form>
</asp:Content>