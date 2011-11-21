<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="Submit.aspx.cs" Inherits="ThumbsUp.Submit" ClientIDMode="Static" %>
<%@ MasterType virtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderPlcHldr" runat="server">
    &mdash; <a href="Index.aspx">Back</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlcHldr" runat="server">
    <form id="Form1" method="post" runat="server">
        <div id="employee_desc">Which employee?</div>
        <asp:RequiredFieldValidator ID="employeeValidator" runat="server" 
            ErrorMessage="Please select an employee." 
            CssClass="validator" 
            ControlToValidate="selectOUUsers"
            Display="Dynamic" />
        <asp:DropDownList ID="selectOUUsers" runat="server"></asp:DropDownList>

        <div id="rating_desc">What kind of good deed?</div>
        <asp:RequiredFieldValidator ID="ratingValidator" runat="server" 
            ErrorMessage="Please select a type of deed." 
            CssClass="validator" 
            ControlToValidate="lbRating"
            Display="Dynamic" />
        <asp:RadioButtonList ID="lbRating" runat="server" RepeatDirection="Vertical" RepeatLayout="UnorderedList"></asp:RadioButtonList>

        <div id="description_desc">This employee...</div>
        <asp:RequiredFieldValidator ID="descriptionValidator" runat="server" 
            ErrorMessage="Please add a description." 
            CssClass="validator" 
            ControlToValidate="txtBoxDescription"
            Display="Dynamic" />
        <asp:TextBox ID="txtBoxDescription" runat="server" TextMode="MultiLine" Columns="80" Rows="8"></asp:TextBox>
        <span class="note">Note: Please do not use the employee's name.</span>

        <input type="submit" id="btnSubmit" value="Submit" runat="server" causesvalidation="true" />
    </form>
</asp:Content>