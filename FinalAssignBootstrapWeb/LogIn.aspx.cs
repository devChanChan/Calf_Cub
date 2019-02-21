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
    public partial class LogIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSubmit.UniqueID;
            // Changing the navbar to highlight the appropriate link as active
            HyperLink pageLink = this.Page.Master.FindControl("lnkLogIn") as HyperLink;
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
            string username = txtUser.Text;
            string password = txtPass.Text;

            AccountDAL accountDAL = new AccountDAL();
            Account account = null;
            account = accountDAL.GetAccountByUsername(username, true);

            if(account != null)
            {
                if (SecurityUtil.AuthenticatePassword(account.Salt, account.Password, password)) {
                    FormsAuthentication.RedirectFromLoginPage(username, chkRemember.Checked);
                }
            }
            // otherwise display an error & switch the Alert box visibility.
            lblOutput.Text = "Invalid login credentials.";
            outputBox.Visible = true;
        }
    }
}