<%@ Page Title="" Language="C#" MasterPageFile="~/Primary.Master" AutoEventWireup="true" CodeBehind="LandingPage.aspx.cs" Inherits="FinalAssignBootstrapWeb.LandingPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Calf & Cub - Landing Page</title>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SmallLeftPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceholder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceholder3" runat="server">
    <div class="row">
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
                    <img src="./images/NYSE.jpg" width="725" height="325">
                </div>
                <div class="carousel-item">
                    <img src="./images/bull.jpg" width="725" height="325">
                </div>
                <div class="carousel-item">
                    <img src="./images/bear.jpg" width="725" height="325">
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
    </div>
    <div class="row">
        <div class="col-sm-12">
            <h2>Available Stocks</h2>
            <div id="companyListing" class="d-flex flex-wrap">
                <asp:Repeater ID="CompanyRepeater" runat="server">
                    <ItemTemplate>
                        <div class="card">
                            <img class="card-img-top" src="<%# Eval("logoUrl") %>" height="200" width="200" alt="Company Logo" />
                            <div class="card-body">
                                <h5><%# Eval("companyName") %></h5>
                                <p class="card-text">Share Price: &nbsp;&nbsp;&nbsp;&nbsp;<%# Eval("pricePerShare") %></p>
                                <p class="card-text">Stock Rate: &nbsp;&nbsp;&nbsp;&nbsp; <%# Eval("currentRate") %>&nbsp;<%# Eval("rateImg") %></p>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SmallRightPlaceholder" runat="server">
    <asp:Panel ID="panelLogIn" runat="server" DefaultButton="btnSubmit">
        <div id="loginForm" class="container-fluid">
            <h3>Log In</h3>
            <div id="outputBox" class="alert alert-danger alert-dismissible" runat="server">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <asp:Label ID="lblOutput" runat="server"></asp:Label>
            </div>
            <div class="form-group">
                <label for="txtUser">Username:</label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" ControlToValidate="txtUser" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:TextBox ID="txtUser" runat="server" CssClass="form-control" MaxLength="12"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtPass">Password:</label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required" ControlToValidate="txtPass" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="form-control" MaxLength="16"></asp:TextBox>
            </div>
            <div class="form-group">
                <div class="form-check form-check-inline" style="left: 0px; top: 0px">
                    <asp:CheckBox ID="chkRemember" runat="server" CssClass="form-check-input" />
                    <label for="SmallRightPlaceholder_chkRemember" class="form-check-label">Stay Logged In?</label>            
                </div>
            </div>
            <div id="FormBtns" class="form-group">
            <asp:Button ID="btnSubmit" runat="server" Text="Log In" OnClick="btnSubmit_Click" CssClass="btn btn-outline-primary btn-lg" />
            <a href="Register.aspx" class="btn btn-outline-success btn-lg">Register Account</a><br />        
            </div>
        </div>
    </asp:Panel>
    <div class="eventFeed container-fluid">
        <h3>Events Feed</h3>
        <div id="AllEvents" runat="server">

        </div>
    </div>
</asp:Content>
