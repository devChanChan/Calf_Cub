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
    public partial class Primary : System.Web.UI.MasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            // changing which nav bar items are visible when user is logged in or not.
            if (Request.IsAuthenticated)
            {
                lnkDashboard.Visible = true;
                lnkProfile.Visible = true;
                lnkGroups.Visible = true;
                lnkLogOut.Visible = true;
                lnkRegister.Visible = false;
                lnkLogIn.Visible = false;

                AccountDAL accountDAL = new AccountDAL();
                Account user = accountDAL.GetAccountByUsername(Context.User.Identity.Name, false);
                lblBalance.Text = user.Balance.ToString("C");
            }
            else
            {
                lnkDashboard.Visible = false;
                lnkProfile.Visible = false;
                lnkGroups.Visible = false;
                lnkLogOut.Visible = false;
                lnkRegister.Visible = true;
                lnkLogIn.Visible = true;
                lblBalance.Text = "";
                balanceContainer.InnerText = "Log In or Register to Play Now!";
            }

            CompanyDAL companyDAL = new CompanyDAL();
            List<Company> companies = companyDAL.GetCompanies();
            pTicker.InnerHtml = "";

            foreach (Company c in companies)
            {
                float stockRate = c.Stock.GetStockRate(); // TO DO: Change to calculate stock rate method
                if (stockRate >= 0)
                    pTicker.InnerHtml += "<span class='font-weight-bold' style='color:darkgoldenrod'>" + c.Stock.Symbol
                        + "</span>&nbsp;<span style='color:limegreen'>" + c.Stock.ValuePerShare.ToString("C")
                        + "&nbsp;<img src='/images/a-up.png'/>&nbsp;" + stockRate.ToString("P2") + "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                else if (stockRate < 0)
                {
                    pTicker.InnerHtml += "<span style='color:darkgoldenrod'>" + c.Stock.Symbol
                        + "</span>&nbsp;<span style='color:red'>" + c.Stock.ValuePerShare.ToString("C")
                        + "&nbsp;<img src='/images/a-down.png'/>&nbsp;" + stockRate.ToString("P2") + "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }

            }

        }

        protected void imgBtnLogo_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.IsAuthenticated)
                Response.Redirect("~/auth/Dashboard.aspx");
            else
                Response.Redirect("~/LandingPage.aspx");
        }

        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}