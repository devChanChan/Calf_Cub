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
    public partial class GroupDirectory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Changing the navbar to highlight the appropriate link as active
            HyperLink pageLink = this.Page.Master.FindControl("lnkGroups") as HyperLink;
            pageLink.CssClass += " active";

            Account user = (Account)Session["User"];
            
            if (!IsPostBack)
            {
                AccountDAL accountDAL = new AccountDAL();
                user = accountDAL.GetAccountByUsername(Context.User.Identity.Name, false);

                PopulateGroups(user);
                Session["User"] = user;

                lblOutput.Visible = false;

                // hiding the create group message for users who already own a group
                if (!(user.OwnerOf is null))
                {
                    outputBox.Visible = false;                    
                }
            }

            if (!(lblOutput.Text.Equals("")))
            {
                outputBox.Visible = true;
                lblCreateGroup.Visible = false;
                lblOutput.Visible = true;
            }                

        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            Button btnAccept = (Button)sender;
            GridViewRow row = (GridViewRow)btnAccept.NamingContainer;
            int groupId = Convert.ToInt32(row.Cells[2].Text);
            GroupDAL groupDAL = new GroupDAL();
            Account user = (Account)Session["User"];
            GroupMembership invite = groupDAL.GetGroupMembership(user.Username, groupId);
            invite.InvitePending = false;
            groupDAL.UpdateGroupMembership(invite);
            Session["ActiveGroup"] = groupDAL.GetGroupById(groupId, true);
            Response.Redirect("./ViewGroup.aspx");
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            Button btnReject = (Button)sender;
            GridViewRow row = (GridViewRow)btnReject.NamingContainer;
            int groupId = Convert.ToInt32(row.Cells[2].Text);
            GroupDAL groupDAL = new GroupDAL();
            Account user = (Account)Session["User"];
            GroupMembership invite = groupDAL.GetGroupMembership(user.Username, groupId);
            groupDAL.DeleteGroupMembership(invite);
            Response.Redirect("./GroupDirectory.aspx"); // refresh page
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            Button btnView = (Button)sender;
            GridViewRow row = (GridViewRow)btnView.NamingContainer;
            int groupId = Convert.ToInt32(row.Cells[2].Text);
            GroupDAL groupDAL = new GroupDAL();
            Session["ActiveGroup"] = groupDAL.GetGroupById(groupId, true);
            Response.Redirect("./ViewGroup.aspx");
        }

        protected void btnJoin_Click(object sender, EventArgs e)
        {
            Button btnJoin = (Button)sender;
            GridViewRow row = (GridViewRow)btnJoin.NamingContainer;
            int groupId = Convert.ToInt32(row.Cells[1].Text);
            Account user = (Account)Session["User"];
            GroupDAL groupDAL = new GroupDAL();
            Group target = groupDAL.GetGroupById(groupId, false);
            if (!(target.InviteRequired))
            {
                GroupMembership membership = new GroupMembership(user, target, false, null, false);
                groupDAL.AddGroupMembership(membership);
                Session["ActiveGroup"] = target;
                Response.Redirect("./ViewGroup.aspx");
            }
            else
            {
                outputBox.Attributes.Add("class", "alert-danger");
                outputBox.Visible = true;
                lblCreateGroup.Visible = false;
                lblOutput.Visible = true;
                lblOutput.Text = target.Name + " is private and requires an invitation to join.";                
            }

        }

        protected void PopulateGroups(Account user)
        {
            GroupDAL groupDAL = new GroupDAL();
            List<Group> allGroups = null;

            groupDAL.GetInvitesByAccount(user);
            groupDAL.GetMembershipsByAccount(user);
            allGroups = groupDAL.GetGroups(user.GroupInvites, user.Groups);

            DataTable invitesTable = new DataTable();
            invitesTable.Columns.Add("Id");
            invitesTable.Columns.Add("Name");
            invitesTable.Columns.Add("Invited By");
            invitesTable.Columns.Add("Group Owner");
            invitesTable.Columns.Add("Private");

            if(!(user.GroupInvites is null))
            foreach (GroupMembership membership in user.GroupInvites)
            {
                DataRow dataRow = invitesTable.NewRow();
                dataRow["Id"] = membership.Group.Id;
                dataRow["Name"] = membership.Group.Name;
                dataRow["Invited By"] = membership.InvitedBy;
                dataRow["Group Owner"] = membership.Group.Owner.Username;
                dataRow["Private"] = membership.Group.InviteRequired;
                invitesTable.Rows.Add(dataRow);
            }

            DataTable memberOfTable = new DataTable();
            memberOfTable.Columns.Add("Member Status");
            memberOfTable.Columns.Add("Id");
            memberOfTable.Columns.Add("Name");
            memberOfTable.Columns.Add("Private");

            if (!(user.Groups is null))
            foreach (GroupMembership membership in user.Groups)
            {
                DataRow dataRow = memberOfTable.NewRow();

                string status = "";
                GroupMembershipStatus statusEnum = membership.GetMemberStatus();
                if (statusEnum == GroupMembershipStatus.Owner)
                    status = "Group Owner";
                if (statusEnum == GroupMembershipStatus.HasRights)
                    status = "Member with Rights";
                if (statusEnum == GroupMembershipStatus.Member)
                    status = "Member";

                dataRow["Member Status"] = status;
                dataRow["Id"] = membership.Group.Id;
                dataRow["Name"] = membership.Group.Name;
                dataRow["Private"] = membership.Group.InviteRequired;
                memberOfTable.Rows.Add(dataRow);
            }

            GroupInvites.DataSource = invitesTable;
            GroupInvites.DataBind();

            GroupMemberships.DataSource = memberOfTable;
            GroupMemberships.DataBind();

            GroupListing.DataSource = allGroups;
            GroupListing.DataBind();
        }
    }
}