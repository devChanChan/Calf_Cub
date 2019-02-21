using FinalAssignLibrary.Classes;
using FinalAssignLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalAssignBootstrapWeb
{
    public partial class LandingPage : System.Web.UI.Page
    {
        Random r = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {
            // forcing the user back to the Game Dashboard in cases where a 
            // user is already logged in but they try and go back to this page.
            if (Request.IsAuthenticated)
                Response.Redirect("./auth/Dashboard.aspx");

            // hiding the Alert box containing the output message when the output
            // is empty.
            if (lblOutput.Text.Equals(""))
                outputBox.Visible = false;

            // Populating the Company Cards
            CompanyDAL companyDAL = new CompanyDAL();
            List<Company> companies = companyDAL.GetCompanies();
            PopulateCompanies(companies);

            // Populating the RandomEvents Feed
            RandomEventDAL eventDAL = new RandomEventDAL();
            List<RandomEvent> events = eventDAL.GetEvents();
            AllEvents.InnerHtml = "";
            foreach(RandomEvent re in events)
            {
                if(re.IsGood) // positive events
                    AllEvents.InnerHtml += "<p>(" + re.OccurredAt.ToShortDateString() 
                        + ", <span style='color:green'>" + re.Weight.ToString("P1") 
                        + "</span><img src='/images/a-up.png'/>)<br>" + re.EventDesc 
                        + "</p><hr>";
                else // negative events
                    AllEvents.InnerHtml += "<p>(" + re.OccurredAt.ToShortDateString() 
                        + ", <span style='color:red'>" + re.Weight.ToString("P1") 
                        + "</span><img src='/images/a-down.png'/>)<br>" + re.EventDesc 
                        + "</p><hr>";
            }
        }

        protected void PopulateCompanies(List<Company> companies)
        {
            DataTable companyTable = new DataTable();
            companyTable.Columns.Add("companyId");
            companyTable.Columns.Add("companyName");
            companyTable.Columns.Add("pricePerShare");
            companyTable.Columns.Add("currentRate");
            companyTable.Columns.Add("rateImg");
            companyTable.Columns.Add("logoUrl");

            foreach (Company c in companies)
            {
                DataRow dataRow = companyTable.NewRow();
                dataRow["companyId"] = c.Id.ToString();
                dataRow["companyName"] = c.CompanyName;
                dataRow["pricePerShare"] = c.Stock.ValuePerShare.ToString("C");
                dataRow["currentRate"] = c.Stock.GetStockRate().ToString("P1");
                if (c.Stock.GetStockRate() >= 0)
                    dataRow["rateImg"] = "<img width='19' height='19' src='/images/a-up.png'/>";
                else
                    dataRow["rateImg"] = "<img width='19' height='19' src='/images/a-down.png'/>";
                dataRow["logoUrl"] = c.LogoUrl;
                companyTable.Rows.Add(dataRow);
            }

            CompanyRepeater.DataSource = companyTable;
            CompanyRepeater.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Text;

            AccountDAL accountDAL = new AccountDAL();
            Account account = null;
            account = accountDAL.GetAccountByUsername(username, true);

            if (account != null)
            {
                if (SecurityUtil.AuthenticatePassword(account.Salt, account.Password, password))
                {
                    FormsAuthentication.RedirectFromLoginPage(username, chkRemember.Checked);
                }
            }
            // otherwise display an error & switch the Alert box visibility.
            lblOutput.Text = "Invalid login credentials.";
            outputBox.Visible = true;
        }
    }
}