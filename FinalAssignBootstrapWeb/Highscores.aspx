<%@ Page Title="" Language="C#" MasterPageFile="~/Primary.Master" AutoEventWireup="true" CodeBehind="Highscores.aspx.cs" Inherits="FinalAssignBootstrapWeb.Highscores" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Calf & Cub - Highscores</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceholder" runat="server">    
    <div class="container-fluid">        
        <h2>Highscores</h2>
        <table class="table table-striped">
            <tr>
                <th scope="col">Rank</th>
                <th scope="col">Username</th>
                <th scope="col">Total Networth</th>
            </tr>
            <tr id="ActivePlayer" runat="server">
                <td id="rank" runat="server"></td>
                <th id="username" runat="server" scope="row"></th>
                <td id="networth" runat="server"></td>
            </tr>
        <asp:Repeater ID="HighscoresUsers" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%# Eval("Rank") %></td>
                    <th scope="row"><%# Eval("Username") %></th>
                    <td><%# Eval("UserNetworth") %></td>  
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        </table>
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
