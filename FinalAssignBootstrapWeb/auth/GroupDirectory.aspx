<%@ Page Title="" Language="C#" MasterPageFile="~/Primary.Master" AutoEventWireup="true" CodeBehind="GroupDirectory.aspx.cs" Inherits="FinalAssignBootstrapWeb.auth.GroupDirectory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Calf & Cub - Group Directory</title>
    <style type="text/css">
        .auto-style1 {
            margin-right: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div id="outputBox" class="alert alert-warning alert-dismissible" runat="server">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblCreateGroup" runat="server">
            <strong>Message from Calf & Cub: </strong><br />You currently do not own a group, if you like, you may create one by <a href="./CreateGroup.aspx" class="alert-link">clicking here</a>
        </asp:Label>
        <asp:Label ID="lblOutput" runat="server" Text=""></asp:Label>
    </div>
    <div id="invites">
        <h2>Pending Group Invites</h2>
        <asp:GridView ID="GroupInvites" runat="server" CssClass="table table-borderless">
        
            <Columns>            
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnAccept" runat="server" Text="Accept" CommandName="Accept" OnClick="btnAccept_Click" CssClass="btn btn-outline-success" />
                    </ItemTemplate> 
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnReject" runat="server" Text="Reject" CommandName="Reject" OnClick="btnReject_Click" CssClass="btn btn-outline-danger" />
                    </ItemTemplate> 
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <p style="text-align:center;">You have no group invites at this time...</p>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
    <div id="memberships">
        <h2>Your Group Memberships</h2>
        <asp:GridView ID="GroupMemberships" runat="server" CssClass="table table-success table-borderless">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnView" runat="server" Text="View Group" CommandName="View" CssClass="btn btn-outline-primary" OnClick="btnView_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <p style="text-align:center;">You do not belong to any groups...</p>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
    <div id="directory">
    <h2>Group Directory</h2>
        <asp:GridView ID="GroupListing" runat="server" CssClass="table table-borderless">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnJoin" runat="server" Text="Join Group" CommandName="Join" OnClick="btnJoin_Click" CssClass="btn btn-outline-primary" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <p style="text-align:center;">There are no new groups to display...</p>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SmallLeftPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceholder2" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MainContentPlaceholder3" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="SmallRightPlaceholder" runat="server">
</asp:Content>
