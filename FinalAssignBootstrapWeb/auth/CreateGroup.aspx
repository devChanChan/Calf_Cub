<%@ Page Title="" Language="C#" MasterPageFile="~/Primary.Master" AutoEventWireup="true" CodeBehind="CreateGroup.aspx.cs" Inherits="FinalAssignBootstrapWeb.auth.CreateGroup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div id="createGroup" class="col-12">
        <h1 class="text-center mt-4 mb-4">Create a Group</h1>
        <div id="outputBox" class="alert alert-danger alert-dismissible" runat="server">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblOutput" runat="server"></asp:Label>
        </div>
        <div class="form-group row">
            <label for="txtName" class="col-lg-2 col-form-label text-lg-right font-weight-bold">Group Name </label>
            <div class="col-sm-10">
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="40"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" ControlToValidate="txtName" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group row">
            <label for="ddStatus" class="col-lg-2 col-form-label text-lg-right font-weight-bold">Select Privacy</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="ddStatus" runat="server" CssClass="form-control">
                    <asp:ListItem>Open / No Invite Required</asp:ListItem>
                    <asp:ListItem>Private / Invite Required</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div id="FormBtns" class="form-group text-center mt-5">
            <asp:Button CssClass="mr-3 btn btn-outline-primary btn-lg" ID="btnSubmit" runat="server" Text="Create" OnClick="btnSubmit_Click" />
            <a href="./GroupDirectory.aspx" class="btn btn-outline-danger btn-lg">Cancel</a>
        </div>
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
