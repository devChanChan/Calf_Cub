<%@ Page Title="" Language="C#" MasterPageFile="~/Primary.Master" AutoEventWireup="true" CodeBehind="ViewGroup.aspx.cs" Inherits="FinalAssignBootstrapWeb.auth.ViewGroup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div id="outputBox" class="alert alert-warning alert-dismissible" runat="server">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblOutput" runat="server"></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SmallLeftPlaceholder" runat="server">
    <div class="mb-4 mt-4">
        <asp:LinkButton ID="lnkBack" runat="server" OnClick="lnkBack_Click" ForeColor="#F36021" Font-Size="Larger">Return to Group Directory</asp:LinkButton>
    </div>
    <div id="GroupDetails" class="form-group">
        <h3>Group Details</h3>
        <div class="font-weight-bold">Group ID:</div>
        <asp:Label ID="lblId" runat="server" Text=""></asp:Label><br />
        <div class="font-weight-bold">Group Name:</div>
        <asp:Label ID="lblName" runat="server" Text=""></asp:Label><br />
        <div class="font-weight-bold">Group Owner:</div>
        <asp:Label ID="lblOwner" runat="server" Text=""></asp:Label><br />
        <div class="font-weight-bold">Group Status:</div>
        <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label><br />
        <div class="font-weight-bold">Member Count:</div>
        <asp:Label ID="lblMembers" runat="server" Text=""></asp:Label><br />
        <div class="font-weight-bold">Group Networth:</div>
        <asp:Label ID="lblWorth" runat="server" Text=""></asp:Label><br />
    </div>
    <div runat="server" id="GroupInvite" class="form-group mt-4 mb-4">
        <h3>Invite User</h3>
        <div class="form-group row pl-3 pr-3">
        <asp:TextBox CssClass="form-control col-sm-8" ID="txtUsername" runat="server" placeholder="Enter username..." ValidationGroup="INVITE" MaxLength="12"></asp:TextBox>
        <asp:Button CssClass="btn btn-outline-primary col-sm-4" ID="btnInvite" runat="server" Text="Send Invite" OnClick="btnInvite_Click" ValidationGroup="INVITE" />
        <asp:CustomValidator ID="CustomCheck_UserExists" runat="server" ErrorMessage="" class="d-block" Display="Dynamic" ControlToValidate="txtUsername" ForeColor="Red" OnServerValidate="CustomCheck_UserExists_ServerValidate" ValidationGroup="INVITE"></asp:CustomValidator>
        </div>
    </div>
    <div runat="server" id="GroupControls" class="form-group">
        <h3>Group Controls</h3>
        <div class="mb-2" id="toggleControls" runat="server" visible="False"><asp:Button ID="btnToggleControls" runat="server" Text="Toggle Member List Controls" OnClick="btnToggleControls_Click" CssClass="btn btn-outline-primary btn-block" CausesValidation="False" /></div>
        <div class="mb-2" id="manageGroup" runat="server" visible="False"><asp:Button ID="btnManageGroup" runat="server" Text="Manage Group Settings" OnClick="btnManageGroup_Click" CssClass="btn btn-outline-primary btn-block" CausesValidation="False"/></div>
        <div class="mb-2" id="closeManage" runat="server" visible="False"><asp:Button ID="btnCloseManage" runat="server" Text="Close Group Settings" OnClick="btnCloseManage_Click" CssClass="btn btn-outline-primary btn-block" CausesValidation="False"/></div>
        <div class="mb-2" id="leaveGroup" runat="server"><asp:Button ID="btnLeaveGroup" runat="server" Text="Leave Group" OnClick="btnLeaveGroup_Click" CssClass="btn btn-outline-danger btn-block" CausesValidation="False" /></div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceholder2" runat="server">
    <asp:MultiView ID="mvGroupMemberList" runat="server">
        <asp:View ID="Members_Member" runat="server">
            <asp:GridView ID="lvlMembers" runat="server" CssClass="table table-bordered"></asp:GridView>
        </asp:View>
        <asp:View ID="Members_Rights" runat="server">
            <asp:GridView ID="lvlRights" runat="server" CssClass="table table-bordered">

                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnKick" runat="server" CommandName="Kick" CssClass="btn btn-outline-danger btn-sm" OnClick="btnKick_Click" Text="Kick" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>
        </asp:View>
        <asp:View ID="Members_Owner" runat="server">
            <asp:GridView ID="lvlOwner" runat="server" CssClass="table table-bordered">

                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnGive" runat="server" CommandName="Give" CssClass="btn btn-outline-primary btn-sm" OnClick="btnGive_Click" Text="Give Rights" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnRemove" runat="server" CommandName="Remove" CssClass="btn btn-outline-warning btn-sm" OnClick="btnRemove_Click" Text="Remove Rights" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnKick" runat="server" CommandName="Kick" CssClass="btn btn-outline-danger btn-sm" OnClick="btnKick_Click" Text="Kick" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>
        </asp:View>
        <asp:View ID="Edit_Owner" runat="server" >
            <div class="form-group text-center">
                <fieldset>
                    <h3 class="text-center">Basic Modifications</h3>
                    <div class="form-group row">
                        <label for="txtName" class="col-form-label col-sm-4 font-weight-bold text-lg-right">Group Name </label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtName" CssClass="form-control" runat="server" MaxLength="40" Width="400px"></asp:TextBox>
                        </div>
                    </div>
                    <asp:RequiredFieldValidator CssClass="d-block" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Group name is required!" ValidationGroup="BASIC" ControlToValidate="txtName" Display="Dynamic" ForeColor="Red" EnableClientScript="False"></asp:RequiredFieldValidator>
                </fieldset>
                <fieldset>
                    <div class="form-group row">
                        <label class="col-form-label col-sm-4 font-weight-bold text-lg-right">Set Group Privacy Status</label>
                        <div class="col-sm-8">
                            <asp:RadioButtonList ID="rlPrivacy" runat="server" ValidationGroup="BASIC">
                                <asp:ListItem>Open - Anyone Can Join</asp:ListItem>
                                <asp:ListItem>Private - Invites Required</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </fieldset>
                <div class="d-block"><asp:Button CssClass="btn btn-outline-primary" ID="btnSave" runat="server" Text="Save Changes" OnClick="btnSave_Click" ValidationGroup="BASIC" /></div>
                <hr class="mt-5" />
                <fieldset class="mt-5">
                    <h3 class="text-center">Advanced Modifications</h3>
                    <div class="form-group row">
                        <label class="col-form-label col-sm-4 font-weight-bold text-lg-right">Transfer Ownership</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtOwner" CssClass="form-control" runat="server" MaxLength="12" Placeholder="Enter new owner's username..."></asp:TextBox>
                        </div>
                    </div>
                    <div class="d-block">
                        <asp:Label ID="lblCheckOutput" runat="server" Visible="False" Font-Bold="True" ForeColor="Green"></asp:Label>
                    </div>
                    <div class="d-block">
                        <asp:Button CssClass="btn btn-outline-primary" ID="btnCheck" runat="server" Text="Check Validity" OnClick="btnCheck_Click" />
                        <asp:Button CssClass="btn btn-outline-success" ID="btnConfirm" runat="server" Text="Confirm Transfer" Enabled="False" OnClick="btnConfirm_Click" />
                    </div>
                    <hr class="mt-5" />
                    <div class="mt-5 float-right">
                        <asp:Button CssClass="d-block btn btn-danger" ID="btnDelete" runat="server" Text="Disband Group" OnClick="btnDelete_Click" />
                    </div>
                </fieldset>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MainContentPlaceholder3" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="SmallRightPlaceholder" runat="server">
</asp:Content>
