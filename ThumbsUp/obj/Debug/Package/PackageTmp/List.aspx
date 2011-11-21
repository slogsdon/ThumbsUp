<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="List.aspx.cs" Inherits="ThumbsUp.List" %>
<%@ MasterType virtualPath="~/Site.master" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="HeaderPlcHldr" runat="server">
    &mdash; <a href="Index.aspx">Index</a> &mdash; <a href="Submit.aspx">Submit</a>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainPlcHldr" runat="server">
    <form id="Form1" method="post" runat="server">
    <asp:Label ID="Message" runat="server" />
    <div id="submissions" class="listing">
        <div class="submission header">
            <div class="person">
                Person
            </div>
            <div class="rating">
                Rating
            </div>
            <div class="ids">
                Submissions
            </div>
            <span class="clear">&nbsp;</span>
        </div>
        <asp:Repeater ID="SubmissionsRepeater" runat="server">
            <ItemTemplate>
                <div class="submission">
                    <div class="person">
                        <%# DataBinder.Eval(Container.DataItem, "Person") %>
                    </div>
                    <div class="rating">
                        <%# DataBinder.Eval(Container.DataItem, "RatingSum") %>
                    </div>
                    <ul class="ids">
                    <asp:Repeater ID="IDRepeater" runat="server" DataSource='<%# DataBinder.Eval(Container.DataItem,"IDList") %>'>
                        <ItemTemplate>
                            <li><a href="Index.aspx?detailed&id=<%# DataBinder.Eval(Container.DataItem, "ID") %>"><%# DataBinder.Eval(Container.DataItem, "ID") %></a></li>
                        </ItemTemplate>
                    </asp:Repeater>
                    </ul>
                    <span class="clear">&nbsp;</span>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </form>
</asp:Content>
<asp:Content ContentPlaceHolderID="MenuPlcHldr" runat="server">
</asp:Content>