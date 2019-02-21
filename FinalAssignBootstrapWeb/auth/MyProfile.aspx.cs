using FinalAssignLibrary.Classes;
using FinalAssignLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalAssignBootstrapWeb.auth
{
    public partial class MyProfile : System.Web.UI.Page
    {
        Random r = new Random();
        Account user = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            outputBox.Visible = false;
            if (!IsPostBack)
            {
                if (Request.QueryString["cid"] != null)
                {
                    int cid = Convert.ToInt32(Request.QueryString["cid"]);
                    CompanyDAL companyDAL = new CompanyDAL();
                    Company company = companyDAL.GetCompanyById(cid);
                    outputBox.Visible = true;
                    lblOutput.Text = "Transaction on <strong>" + company.CompanyName + "</strong> stocks approved!";
                }
            }

            // Changing the navbar to highlight the appropriate link as active
            HyperLink pageLink = this.Page.Master.FindControl("lnkProfile") as HyperLink;
            pageLink.CssClass += " active";

            AccountDAL accountDAL = new AccountDAL();
            user = accountDAL.GetAccountByUsername(Context.User.Identity.Name, false);
            user.Stocks = accountDAL.GetStockOwnership(user);

            lblAcctBalance.Text = user.Balance.ToString("C");
            double acctRate = user.GetAccountPerformance();
            lblAcctRate.Text = acctRate.ToString("P2");
            if (acctRate >= 0)
                imgAcctRate.ImageUrl = "/images/a-up.png";
            else
                lblNetworth.Text = "/images/a-down.png";
            lblNetworth.Text = user.GetAccountValue().ToString("C");

            if (user.Stocks != null && user.Stocks.Count > 0)
            {
                PopulateOwnedStocks(user.Stocks);

                // Populating the RandomEvents Feed
                List<Company> investedIn = new List<Company>();
                foreach (StockOwnership stock in user.Stocks)
                {
                    investedIn.Add(stock.Stock.Company);
                }
                RandomEventDAL eventDAL = new RandomEventDAL();
                List<RandomEvent> events = eventDAL.GetEvents(investedIn);
                AllEvents.InnerHtml = "";
                foreach (RandomEvent re in events)
                {
                    if (re.IsGood) // positive events
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
            else
            {
                ddCompanies.Visible = false;
                CompanyRepeater.Visible = false;
                companyListing.InnerHtml = "<p class='lead'>You currently don't own any stocks.<br>" +
                    "View available stocks from the&nbsp;<a href='./Dashboard.aspx'>" +
                    "Dashboard</a>&nbsp;to get started!</p>";
                AllEvents.InnerHtml = "<p>Calf & Cub will display any events that have " +
                    "occurred which relate to the <strong>Companies</strong> which " +
                    "you invest in or any of the <strong>Industries</strong> which " +
                    "they are affiliated with here. Invest in some stocks to start " +
                    "seeing the data!</p>";
            }
        }

        protected void PopulateOwnedStocks(List<StockOwnership> stocks)
        {
            DataTable ownedStocks = new DataTable();
            ownedStocks.Columns.Add("companyId");
            ownedStocks.Columns.Add("companyName");
            ownedStocks.Columns.Add("pricePerShare");
            ownedStocks.Columns.Add("currentRate");
            ownedStocks.Columns.Add("rateImg");
            ownedStocks.Columns.Add("logoUrl");

            foreach (StockOwnership s in stocks)
            {
                DataRow dataRow = ownedStocks.NewRow();
                dataRow["companyId"] = s.Stock.Company.Id;
                dataRow["companyName"] = s.Stock.Company.CompanyName;
                dataRow["pricePerShare"] = s.Stock.ValuePerShare.ToString("C");
                dataRow["currentRate"] = s.CalCurrRate().ToString("P");
                if (s.CalCurrRate() >= 0)
                    dataRow["rateImg"] = "<img width='19' height='19' src='/images/a-up.png'/>";
                else
                    dataRow["rateImg"] = "<img width='19' height='19' src='/images/a-down.png'/>";
                dataRow["logoUrl"] = s.Stock.Company.LogoUrl;
                ownedStocks.Rows.Add(dataRow);

                ddCompanies.Items.Add(s.Stock.Company.CompanyName);
            }

            CompanyRepeater.DataSource = ownedStocks;
            CompanyRepeater.DataBind();
        }

        protected void ddCompanies_SelectedIndexChanged(object sender, EventArgs e)
        {
            string companyName = ddCompanies.SelectedValue;
            CompanyDAL companyDAL = new CompanyDAL();
            List<Company> companies = companyDAL.GetCompanies();
            foreach (Company c in companies)
            {
                if (companyName.Equals(c.CompanyName))
                    Response.Redirect("./ViewCompany.aspx?cid=" + c.Id);
            }
        }
    }
}