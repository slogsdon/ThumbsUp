<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="Submit.aspx.cs" Inherits="ThumbsUp.Submit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPlcHldr" runat="server">
    <form id="Form1" method="post" runat="server">
        <asp:Label ID="lblName" runat="server" /><br />
        <asp:Label ID="lblAuthType" runat="server" /><br />
        <asp:DropDownList ID="selectOUUsers" runat="server"></asp:DropDownList>
        <asp:RadioButtonList ID="lbRating" runat="server" RepeatDirection="Vertical" RepeatLayout="UnorderedList"></asp:RadioButtonList>
        <asp:TextBox ID="txtBoxDescription" runat="server" TextMode="MultiLine" rows="8" cols="40"></asp:TextBox>
        <input type="submit" id="btnSubmit" value="Submit" />
    </form>
    </asp:Content>
