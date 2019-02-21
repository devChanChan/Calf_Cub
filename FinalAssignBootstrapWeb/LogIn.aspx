<%@ Page Title="" Language="C#" MasterPageFile="~/Primary.Master" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs" Inherits="FinalAssignBootstrapWeb.LogIn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Calf & Cub - Log In</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SmallLeftPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div id="loginForm" class="col-12">
        <h1 class="text-center mt-4 mb-4">Log In</h1>
        <div id="outputBox" class="alert alert-danger alert-dismissible" runat="server">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblOutput" runat="server"></asp:Label>
        </div>
        <div class="form-group row">
            <label for="txtUser" class="col-lg-1 col-form-label text-lg-right font-weight-bold">Username</label>
            <div class="col-sm-11">
                <asp:TextBox ID="txtUser" runat="server" CssClass="form-control" MaxLength="12"></asp:TextBox>
            </div>
        </div>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" ControlToValidate="txtUser" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
        <div class="form-group row">
            <label for="txtPass" class="col-lg-1 col-form-label text-lg-right font-weight-bold">Password</label>
            <div class="col-sm-11">
                <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="form-control" MaxLength="16"></asp:TextBox>
            </div>
        </div>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required" ControlToValidate="txtPass" ForeColor="Red"></asp:RequiredFieldValidator>
        <div class="form-group mt-4">
            <div class="form-check form-check-inline">
                <asp:CheckBox ID="chkRemember" runat="server" CssClass="form-check-input" />
                <label for="MainContentPlaceholder_chkRemember" class="form-check-label">Stay Logged In?</label>            
            </div>
        </div>
        <div id="FormBtns" class="form-group mt-4">
        <asp:Button ID="btnSubmit" runat="server" Text="Log In" OnClick="btnSubmit_Click" CssClass="mr-3 btn btn-outline-primary btn-lg" />
        <a href="Register.aspx" class="btn btn-outline-success btn-lg">Register Account</a><br />        
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceholder2" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SmallRightPlaceholder" runat="server">
</asp:Content>
