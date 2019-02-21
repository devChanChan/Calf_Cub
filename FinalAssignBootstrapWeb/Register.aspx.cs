using FinalAssignLibrary.Classes;
using FinalAssignLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalAssignBootstrapWeb
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Changing the navbar to highlight the appropriate link as active
            HyperLink pageLink = this.Page.Master.FindControl("lnkRegister") as HyperLink;
            pageLink.CssClass += " active";

            // forcing the user back to the Game Dashboard in cases where a 
            // user is already logged in but they try and go back to this page.
            if (Request.IsAuthenticated)
                Response.Redirect("./auth/Dashboard.aspx");

            // hiding the Alert box containing the output message when the output
            // is empty.
            if (lblOutput.Text.Equals(""))
                outputBox.Visible = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                lblOutput.Text = "";
                string username = txtUser.Text;
                string email = txtEmail.Text;
                string password = txtPass.Text;

                AccountDAL accountDAL = new AccountDAL();
                Account account = accountDAL.GetAccountByUsername(username, true);

                if (account == null)
                {
                    string[] hashes = SecurityUtil.HashNewPass(password);
                    account = new Account(username, email, hashes[1], hashes[0]);
                    accountDAL.RegisterAccount(account);
                    FormsAuthentication.RedirectFromLoginPage(account.Username, false);
                }
                else
                {
                    lblOutput.Text = "An account with that username already exists.";
                }
            }
        }
    }
}