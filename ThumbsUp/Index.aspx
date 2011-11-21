<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="Index.aspx.cs" Inherits="ThumbsUp.Index" %>
<%@ MasterType virtualPath="~/Site.master" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="HeaderPlcHldr" runat="server">
    &mdash; <a href="Submit.aspx">Submit</a>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainPlcHldr" runat="server">
    <form id="Form1" method="post" runat="server">
    <div id="submissions">
        <asp:Repeater ID="SubmissionsRepeater" runat="server">
            <ItemTemplate>
                <div class="submission">
                    <span class="DateTime">
                        <%# DataBinder.Eval(Container.DataItem, "DateTime") %>
                    </span>
                    <p>
                        <%# DataBinder.Eval(Container.DataItem, "stripNames") %>
                    </p>
                    <div>
                        <a href="Index.aspx?VoteID=<%# DataBinder.Eval(Container.DataItem, "ID") %>">
                            <asp:Image runat="server" ImageUrl="~/img/thumb.png" />
                        </a>
                        <span class="vote_count">
                            <%# DataBinder.Eval(Container.DataItem, "VoteCount") %>
                        </span>
                    </div>
                    <span class="clear">&nbsp;</span>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </form>
</asp:Content>
<asp:Content ContentPlaceHolderID="MenuPlcHldr" runat="server">
</asp:Content>