<%@ Page Title="" Language="C#" MasterPageFile="~/Primary.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="FinalAssignBootstrapWeb.auth.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Calf & Cub - Market Dashboard</title>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SmallLeftPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceholder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceholder3" runat="server">
    <div class="col-sm-12 carousel slide" data-ride="carousel" id="banner">
        <!-- Indicators -->
        <ul class="carousel-indicators">
            <li data-target="#banner" data-slide-to="0" class="active"></li>
            <li data-target="#banner" data-slide-to="1"></li>
            <li data-target="#banner" data-slide-to="2"></li>
        </ul>
  
        <!-- The slideshow -->
        <div class="carousel-inner">
            <div class="carousel-item active">
                <img src="../images/NYSE.jpg" width="725" height="325">
            </div>
            <div class="carousel-item">
                <img src="../images/bull.jpg" width="725" height="325">
            </div>
            <div class="carousel-item">
                <img src="../images/bear.jpg" width="725" height="325">
            </div>
        </div>
  
        <!-- Left and right controls -->
        <a class="carousel-control-prev" href="#banner" data-slide="prev">
            <span class="carousel-control-prev-icon"></span>
        </a>
        <a class="carousel-control-next" href="#banner" data-slide="next">
            <span class="carousel-control-next-icon"></span>
        </a>
    </div>
    <div class="row">
        <div class="col-sm-12">
            <h2>Available Stocks</h2>
            <asp:DropDownList ID="ddCompanies" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddCompanies_SelectedIndexChanged">
                <asp:ListItem>Select a Company...</asp:ListItem>
            </asp:DropDownList>
            <div id="companyListing" class="d-flex flex-wrap">
                <asp:Repeater ID="CompanyRepeater" runat="server">
                    <ItemTemplate>
                        <a class="companyContent"
                            href="./ViewCompany.aspx?cid=<%# Eval("companyId")%>">
                            <div class="card">
                                <img class="card-img-top" src="<%# Eval("logoUrl") %>" height="200" width="200" alt="Company Logo" />
                                <div class="card-body">
                                    <h5><%# Eval("companyName") %></h5>
                                    <p class="card-text">Share Price: &nbsp;&nbsp;&nbsp;&nbsp;<%# Eval("pricePerShare") %></p>
                                    <p class="card-text">Stock Rate: &nbsp;&nbsp;&nbsp;&nbsp; <%# Eval("currentRate") %>&nbsp;<%# Eval("rateImg") %></p>
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