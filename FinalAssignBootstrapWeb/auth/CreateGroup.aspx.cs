using FinalAssignLibrary.Classes;
using FinalAssignLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalAssignBootstrapWeb.auth
{
    public partial class CreateGroup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Changing the navbar to highlight the appropriate link as active
            HyperLink pageLink = this.Page.Master.FindControl("lnkGroups") as HyperLink;
            pageLink.CssClass += " active";

            // hiding the Alert box containing the output message when the output
            // is empty.
            if (lblOutput.Text.Equals(""))
                outputBox.Visible = false;

            AccountDAL accountDAL = new AccountDAL();
            Account user = accountDAL.GetAccountByUsername(Context.User.Identity.Name, false);
            if (!(user.OwnerOf is null))
                Response.Redirect("./GroupDirectory.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                AccountDAL accountDAL = new AccountDAL();
                Account user = accountDAL.GetAccountByUsername(Context.User.Identity.Name, false);

                if (user.OwnerOf is null)
                {
                    string name = txtName.Text;
                    bool inviteRequired = false;
                    if (ddStatus.SelectedIndex == 1)
                        inviteRequired = true;

                    Group newGroup = new Group(user, name, inviteRequired);
                    GroupDAL groupDAL = new GroupDAL();
                    
                    int id = groupDAL.AddGroup(newGroup);
                    newGroup.Id = id;
                    user.OwnerOf = newGroup;

                    accountDAL.UpdateAccount(user);
                    GroupMembership membership = new GroupMembership(user, newGroup, true, null, false);
                    groupDAL.AddGroupMembership(membership);

                    Session["ActiveGroup"] = newGroup;
                    Response.Redirect("./ViewGroup.aspx");
                }
            }
        }
    }
}