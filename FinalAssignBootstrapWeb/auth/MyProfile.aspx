<%@ Page Title="" Language="C#" MasterPageFile="~/Primary.Master" AutoEventWireup="true" CodeBehind="MyProfile.aspx.cs" Inherits="FinalAssignBootstrapWeb.auth.MyProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <title>Calf & Cub - My Profile</title>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div id="outputBox" class="alert alert-success alert-dismissible" runat="server">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblOutput" runat="server"></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SmallLeftPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceholder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceholder3" runat="server">
    <div class="row">
        <div class="jumbotron-fluid" id="ProfileHeader">
            <h1 class="display-4">My Profile</h1>
            <hr class="my-4">
                <p class="lead mt-2 mb-2">Current Account Balance:</p>
                <asp:Label ID="lblAcctBalance" runat="server" Text=""></asp:Label><br />
                <p class="lead mt-2 mb-2">Current Earning Rate:</p>
                <asp:Label ID="lblAcctRate" runat="server" Text=""></asp:Label>
                <asp:Image ID="imgAcctRate" runat="server" />
                <br />
                <p class="lead mt-2 mb-2">Current Networth: </p>
                <asp:Label ID="lblNetworth" runat="server" Text=""></asp:Label>
            <hr class="my-4">
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12">
            <h2>My Stock Portfolio</h2>
            <asp:DropDownList ID="ddCompanies" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddCompanies_SelectedIndexChanged">
                <asp:ListItem>Select a Company...</asp:ListItem>
            </asp:DropDownList>
            <div id="companyListing" class="d-flex flex-wrap" runat="server">
                <asp:Repeater ID="CompanyRepeater" runat="server">
                    <ItemTemplate>
                        <a class="companyContent"
                            href="./ViewCompany.aspx?cid=<%# Eval("companyId")%>">
                            <div class="card">
                                <img class="card-img-top" src="<%# Eval("logoUrl") %>" height="200" width="200" alt="Company Logo" />
                                <div class="card-body">
                                    <h5><%# Eval("companyName") %></h5>
                                    <p class="card-text">Price: &nbsp;&nbsp;&nbsp;&nbsp;<%# Eval("pricePerShare") %></p>
                                    <p class="card-text">Rate: &nbsp;&nbsp;&nbsp;&nbsp; <%# Eval("currentRate") %>&nbsp;<%# Eval("rateImg") %></p>
                                </div>
                            </div>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SmallRightPlaceholder" runat="server">
    <div class="eventFeed container-fluid">
        <h3>Events Feed</h3>
        <div id="AllEvents" runat="server">

        </div>
    </div>
</asp:Content>
