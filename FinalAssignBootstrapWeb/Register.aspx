<%@ Page Title="" Language="C#" MasterPageFile="~/Primary.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="FinalAssignBootstrapWeb.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Calf & Cub - Register Account</title>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div id="registerForm" class="col-12">
        <h1 class="text-center mt-4 mb-4">Register Account</h1>
        <div id="outputBox" class="alert alert-danger alert-dismissible" runat="server">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblOutput" runat="server"></asp:Label>
        </div>
        <div class="form-group">
            <label for="txtUser">Username: </label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" ControlToValidate="txtUser" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:TextBox ID="txtUser" runat="server" CssClass="form-control" MaxLength="12"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtEmail">Email Address:</label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required" ControlToValidate="txtEmail" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" MaxLength="50"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtPass">Password:</label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Required" ControlToValidate="txtPass" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="form-control" MaxLength="16"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtPassConfirm">Confirm:</label>
            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Not a Match" ControlToCompare="txtPass" ControlToValidate="txtPassConfirm" ForeColor="Red"></asp:CompareValidator>
            <asp:TextBox ID="txtPassConfirm" runat="server" TextMode="Password" CssClass="form-control" MaxLength="16"></asp:TextBox>
        </div>
        <div id="FormBtns" class="form-group mt-4 text-center">
            <asp:Button CssClass="btn btn-outline-primary btn-lg mr-2" ID="btnSubmit" runat="server" Text="Register" OnClick="btnSubmit_Click" />
            <a href="./LandingPage.aspx" class="btn btn-outline-danger btn-lg">Cancel</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SmallLeftPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceholder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceholder3" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SmallRightPlaceholder" runat="server">
</asp:Content>
