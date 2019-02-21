using FinalAssignLibrary.Classes;
using FinalAssignLibrary.Database;
using FinalAssignLibrary.Enumerations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalAssignBootstrapWeb.auth
{
    public partial class ViewGroup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Changing the navbar to highlight the appropriate link as active
            HyperLink pageLink = this.Page.Master.FindControl("lnkGroups") as HyperLink;
            pageLink.CssClass += " active";

            Group activeGroup = (Group)Session["ActiveGroup"];            

            if (activeGroup is null)
                Response.Redirect("./GroupDirectory.aspx");

            Title = "Calf & Cub - " + activeGroup.Name;

            if (lblOutput.Text.Equals(""))
                outputBox.Visible = false;

            lblId.Text = activeGroup.Id.ToString();
            lblName.Text = activeGroup.Name;
            lblOwner.Text = activeGroup.Owner.Username;
            lblStatus.Text = (activeGroup.InviteRequired) ? "Invite Required" : "Open to Anyone";
            lblMembers.Text = activeGroup.Members.Count.ToString();
            lblWorth.Text = activeGroup.GetGroupWorth().ToString("C");

            if (!IsPostBack)
            {
                PopulateMembers(activeGroup);
            }
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            Session["ActiveGroup"] = null;
            Response.Redirect("./GroupDirectory.aspx");
        }

        protected void btnKick_Click(object sender, EventArgs e)
        {
            Button btnView = (Button)sender;
            GridViewRow row = (GridViewRow)btnView.NamingContainer;
            string user = null;
            if (mvGroupMemberList.ActiveViewIndex == 1)
                user = row.Cells[2].Text;
            else
                user = row.Cells[4].Text;

            if (!user.Equals(Context.User.Identity.Name))
            {                
                GroupDAL groupDAL = new GroupDAL();
                Group activeGroup = (Group)Session["ActiveGroup"];
                GroupMembership member = groupDAL.GetGroupMembership(user, activeGroup.Id);

                // if the active user is the group owner or the targeted member does not have rights
                if (activeGroup.Owner.Username.Equals(Context.User.Identity.Name) ||
                    !(member.HasRights))
                {                    
                    groupDAL.DeleteGroupMembership(member);
                }
                else
                {
                    lblOutput.Text = member.Account.Username + " has rights: only the group owner can kick them!";
                    outputBox.Visible = true;
                }
            }
            else
            {
                lblOutput.Text = "You can't kick yourself from the group!";
                outputBox.Visible = true;
            }
        }

        protected void btnGive_Click(object sender, EventArgs e)
        {
            Button btnView = (Button)sender;
            GridViewRow row = (GridViewRow)btnView.NamingContainer;
            string user = row.Cells[4].Text;

            if (!user.Equals(Context.User.Identity.Name))
            {
                GroupDAL groupDAL = new GroupDAL();
                Group activeGroup = (Group)Session["ActiveGroup"];
                GroupMembership member = groupDAL.GetGroupMembership(user, activeGroup.Id);
                if (!member.HasRights)
                {
                    member.HasRights = true;
                    groupDAL.UpdateGroupMembership(member);
                }
                else
                {
                    lblOutput.Text = member.Account.Username + " already has rights!";
                    outputBox.Visible = true;
                }
            }
            else
            {
                lblOutput.Text = "You can't give rights to yourself!";
                outputBox.Visible = true;
            }
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            Button btnView = (Button)sender;
            GridViewRow row = (GridViewRow)btnView.NamingContainer;
            string user = row.Cells[4].Text;

            if (!user.Equals(Context.User.Identity.Name))
            {
                GroupDAL groupDAL = new GroupDAL();
                Group activeGroup = (Group)Session["ActiveGroup"];
                GroupMembership member = groupDAL.GetGroupMembership(user, activeGroup.Id);
                if (member.HasRights)
                {
                    member.HasRights = false;
                    groupDAL.UpdateGroupMembership(member);
                }
                else
                {
                    lblOutput.Text = member.Account.Username + " does not have rights to remove!";
                    outputBox.Visible = true;
                }
            }
            else
            {
                lblOutput.Text = "You can't remove rights from yourself!";
                outputBox.Visible = true;
            }
        }

        protected void btnToggleControls_Click(object sender, EventArgs e)
        {
            if (mvGroupMemberList.ActiveViewIndex == 1 || mvGroupMemberList.ActiveViewIndex == 2) {
                Session["ToggleGroupIndex"] = mvGroupMemberList.ActiveViewIndex;
                mvGroupMemberList.ActiveViewIndex = 0;
            }
            else
            {
                mvGroupMemberList.ActiveViewIndex = Convert.ToInt16(Session["ToggleGroupIndex"]);
            }
                
        }

        protected void btnManageGroup_Click(object sender, EventArgs e)
        {
            Group group = (Group)Session["ActiveGroup"];
            txtName.Text = group.Name;
            if (group.InviteRequired)
                rlPrivacy.SelectedIndex = 1;
            else
                rlPrivacy.SelectedIndex = 0;

            Session["ManageGroupIndex"] = mvGroupMemberList.ActiveViewIndex;
            mvGroupMemberList.ActiveViewIndex = 3;
            toggleControls.Visible = false;
            manageGroup.Visible = false;
            closeManage.Visible = true;
        }

        protected void btnCloseManage_Click(object sender, EventArgs e)
        {
            mvGroupMemberList.ActiveViewIndex = Convert.ToInt16(Session["ManageGroupIndex"]);
            toggleControls.Visible = true;
            manageGroup.Visible = true;
            closeManage.Visible = false;
        }

        private void PopulateMembers(Group group)
        {
            GroupMembershipStatus userRank = GroupMembershipStatus.NotAMember;

            if (group.Members is null || group.Members.Count == 0)
            {
                GroupDAL groupDAL = new GroupDAL();
                group.Members = groupDAL.GetMembersByGroup(group);
            }

            DataTable membersTable = new DataTable();
            membersTable.Columns.Add("Rank");
            membersTable.Columns.Add("Username");
            membersTable.Columns.Add("Player Networth");

            foreach (GroupMembership member in group.Members)
            {
                AccountDAL accountDAL = new AccountDAL();
                if (member.Account.Username.ToUpper().Equals(Context.User.Identity.Name.ToUpper()))
                    userRank = member.GetMemberStatus();

                if (member.Account.Stocks is null || member.Account.Stocks.Count == 0)
                {
                    member.Account.Stocks = accountDAL.GetStockOwnership(member.Account);
                }
                string status = "";
                GroupMembershipStatus statusEnum = member.GetMemberStatus();
                if (statusEnum == GroupMembershipStatus.Owner)
                    status = "Owner";
                if (statusEnum == GroupMembershipStatus.HasRights)
                    status = "Rights";
                if (statusEnum == GroupMembershipStatus.Member)
                    status = "Member";

                DataRow dataRow = membersTable.NewRow();
                dataRow["Rank"] = status;
                dataRow["Username"] = member.Account.Username;
                dataRow["Player Networth"] = member.Account.GetAccountValue().ToString("C");
                membersTable.Rows.Add(dataRow);
            }

            lvlMembers.DataSource = membersTable;
            lvlMembers.DataBind();
            mvGroupMemberList.ActiveViewIndex = 0;
            GroupInvite.Visible = !(group.InviteRequired);


            if (userRank == GroupMembershipStatus.HasRights)
            {
                lvlRights.DataSource = membersTable;
                lvlRights.DataBind();
                mvGroupMemberList.ActiveViewIndex = 1;
                toggleControls.Visible = true;
            }
            else if (userRank == GroupMembershipStatus.Owner)
            {
                lvlOwner.DataSource = membersTable;
                lvlOwner.DataBind();
                mvGroupMemberList.ActiveViewIndex = 2;
                toggleControls.Visible = true;
                manageGroup.Visible = true;
                leaveGroup.Visible = false;
            }
            else if (userRank == GroupMembershipStatus.NotAMember)
            {
                Response.Redirect("./GroupDirectory.aspx");
            }
        }

        protected void btnLeaveGroup_Click(object sender, EventArgs e)
        {
            GroupDAL groupDAL = new GroupDAL();
            Group activeGroup = (Group)Session["ActiveGroup"];
            GroupMembership member = groupDAL.GetGroupMembership(Context.User.Identity.Name, activeGroup.Id);
            groupDAL.DeleteGroupMembership(member);
            Session["ActiveGroup"] = null;
            Response.Redirect("./GroupDirectory.aspx");

        }

        protected void CustomCheck_UserExists_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string targetUsername = txtUsername.Text;
            AccountDAL accountDAL = new AccountDAL();
            Account target = accountDAL.GetAccountByUsername(targetUsername, false);
            if (target is null)
            {
                args.IsValid = false;
                CustomCheck_UserExists.ErrorMessage = "That user doesn't exist!";
            }
            else if (target.Username.Equals(Context.User.Identity.Name))
            {
                args.IsValid = false;
                CustomCheck_UserExists.ErrorMessage = "You can't send yourself an invite!";
            }
            else
            {
                args.IsValid = true;
                Session["InviteTarget"] = target;
            }

        }

        protected void btnInvite_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Account target = (Account)Session["InviteTarget"];
                Group group = (Group)Session["ActiveGroup"];

                if (target is null) {
                    AccountDAL accountDAL = new AccountDAL();
                    target = accountDAL.GetAccountByUsername(txtUsername.Text, false);
                }

                GroupMembership membership = new GroupMembership(
                    target, group, Context.User.Identity.Name
                );

                GroupDAL groupDAL = new GroupDAL();
                groupDAL.AddGroupMembership(membership);
                
                lblOutput.Text = "Invite successfully sent to " + target.Username + "!";
                outputBox.Visible = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                bool update = false;
                string name = txtName.Text;
                bool isPrivate = false;

                if (rlPrivacy.SelectedIndex == 1)
                    isPrivate = true;

                Group group = (Group)Session["ActiveGroup"];
                if (isPrivate != group.InviteRequired)
                    update = true;
                else if (!(name.Equals(group.Name)))
                    update = true;

                if (update)
                {
                    GroupDAL groupDAL = new GroupDAL();
                    group.InviteRequired = isPrivate;
                    group.Name = name;
                    groupDAL.UpdateGroup(group);
                }
            }
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            string newOwner = txtOwner.Text;
            AccountDAL accountDAL = new AccountDAL();
            Account owner = accountDAL.GetAccountByUsername(newOwner, false);
            if(owner is null)
            {
                lblCheckOutput.Text = "Invalid username entered...";
                lblCheckOutput.ForeColor = System.Drawing.Color.Red;
                lblCheckOutput.Visible = true;
            }
            else
            {
                if (owner.OwnerOf is null)
                {
                    lblCheckOutput.Text = "Valid User... Click confirm to transfer";
                    lblCheckOutput.Visible = true;
                    txtOwner.Enabled = false;
                    btnCheck.Enabled = false;
                    btnConfirm.Enabled = true;
                }
            }

        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            string newOwner = txtOwner.Text;
            AccountDAL accountDAL = new AccountDAL();
            Account owner = accountDAL.GetAccountByUsername(newOwner, false);
            Account user = accountDAL.GetAccountByUsername(Context.User.Identity.Name, false);

            if(!(owner is null) && !(user is null))
            {
                GroupDAL groupDAL = new GroupDAL();
                Group group = user.OwnerOf; // getting the group that's being transferred
                // getting the new owners membership to the group.
                GroupMembership ownership = groupDAL.GetGroupMembership(owner.Username, group.Id);

                // if the new owner is not yet a member, add them
                if (ownership is null)
                {
                    ownership = new GroupMembership(owner, group, true, user.Username, false);
                    groupDAL.AddGroupMembership(ownership);
                }
                else
                {
                    // giving the new owner rights to the group
                    if (!(ownership.HasRights))
                    {
                        ownership.HasRights = true;
                        groupDAL.UpdateGroupMembership(ownership);
                    }
                }
                // updating group record with new owner
                group.Owner = owner;
                groupDAL.UpdateGroup(group);

                // updating accounts
                owner.OwnerOf = user.OwnerOf; // transferring group to new owner Account
                user.OwnerOf = null; // setting old owner Account to null
                accountDAL.UpdateAccount(owner);
                accountDAL.UpdateAccount(user);
                
                Response.Redirect("./GroupDirectory.aspx");
            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            AccountDAL accountDAL = new AccountDAL();
            Account account = accountDAL.GetAccountByUsername(Context.User.Identity.Name, false);
            account.OwnerOf = null;
            Group group = (Group)Session["ActiveGroup"];
            GroupDAL groupDAL = new GroupDAL();
            accountDAL.UpdateAccount(account);
            groupDAL.DeleteGroup(group);
            Response.Redirect("./GroupDirectory.aspx");
        }
    }
}