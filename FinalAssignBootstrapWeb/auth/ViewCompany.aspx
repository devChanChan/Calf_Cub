<%@ Page Title="" Language="C#" MasterPageFile="~/Primary.Master" AutoEventWireup="true" CodeBehind="ViewCompany.aspx.cs" Inherits="FinalAssignBootstrapWeb.auth.ViewCompany" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SmallLeftPlaceholder" runat="server">
    <div class="companyInfo">                    
        <asp:Image ID="imgLogo" CssClass="img-thumbnail" runat="server" ImageUrl="/images/pear.jpeg" />
        <div class="text-center mt-5 mb-5">
            <asp:Label ID="lblDesc" runat="server" Font-Italic="True" Font-Size="Larger"></asp:Label>
        </div>
        <hr />
        <div class="form-group row mt-4 mb-4">
            <div class="col-12">
                <asp:Label CssClass="text-lg-right" ID="lblIndustries" runat="server" Text="Industries" Font-Bold="True" Font-Size="Medium"></asp:Label><br />
            </div>
            <div class="col-12">
                <ul class="list-group">
                    <asp:Repeater ID="IndustryRepeater" runat="server">
                        <ItemTemplate>
                            <li class="list-group-item"><%# Eval("Industry") %></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>
        <div class="eventFeed container-fluid">
            <h3>Events Feed</h3>
            <div id="AllEvents" runat="server">
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceholder2" runat="server">
    <div class="container-fluid">
        <div>
            <asp:Label ID="lblName" runat="server" Font-Bold="True" Font-Size="XX-Large" ForeColor="#F36021"></asp:Label>
        </div>
        <div class="form-group mt-2" id="newTransaction">
            <table id="transactions" class="table">
                <tr>
                    <td colspan="3">
                        <h4 class="block w-100">New Transaction</h4>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Transaction Type</label>
                        <asp:DropDownList ID="ddType" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddType_SelectedIndexChanged">
                            <asp:ListItem>BUY</asp:ListItem>
                            <asp:ListItem>SELL</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <label>Quantity</label>
                        <asp:TextBox ID="txtQty" TextMode="Number" runat="server" Width="125px" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnProcess" CssClass="btn btn-primary" runat="server" Text="Buy Now" OnClick="btnProcess_Click" Height="70px"/>
                    </td>
                </tr>
            </table>
            <div>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddType" ErrorMessage="Transaction Type is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Quantity is required" ControlToValidate="txtQty" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtQty" ErrorMessage="Only positive number is allowed." ForeColor="Red" ValidationExpression="\d+" Display="Dynamic"></asp:RegularExpressionValidator>
                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="Dynamic" ForeColor="Red" OnServerValidate="NewTransaction_ServerValidate"></asp:CustomValidator>
            </div>
        </div>
        <hr />
        <h4 class="w-100">Stock Information</h4>
        <table class="table">
            <tr>
                <td>Symbol:</td>
                <td><asp:Label ID="lblSymbol" runat="server"></asp:Label></td>
                <td>Price Per Share:</td>
                <td><asp:Label ID="lblPrice" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Current Rate:</td>
                <td>
                    <asp:Label ID="lblRate" runat="server"></asp:Label>
                    <asp:Image ID="stockRateImg" runat="server" />
                </td>
                <td>Your Rate:</td>
                <td>
                    <asp:Label ID="lblPlayerRate" runat="server"></asp:Label>
                    <asp:Image ID="playerRateImg" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Your Shares:</td>
                <td><asp:Label ID="lblOwnedStocks" runat="server">0</asp:Label></td>
                <td colspan="2"><asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CssClass="btn btn-secondary" Enabled="False">View Transaction History</asp:LinkButton></td>                
            </tr>
        </table>
        <hr />
        <div class="mt-5 mb-5">
            <h4>Stock History</h4>
            <div class="text-center">
                <asp:Chart ID="Chart1" runat="server" IsMapAreaAttributesEncoded="True" Palette="Berry" Width="500px">
                    <Series>
                        <asp:Series Name="StockHistory" ChartType="Line"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MainContentPlaceholder3" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="SmallRightPlaceholder" runat="server">
</asp:Content>
