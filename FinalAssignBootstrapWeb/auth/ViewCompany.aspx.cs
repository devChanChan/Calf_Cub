using FinalAssignLibrary.Classes;
using FinalAssignLibrary.Database;
using FinalAssignLibrary.Enumerations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace FinalAssignBootstrapWeb.auth
{
    public partial class ViewCompany : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int activeCompany = Convert.ToInt32(ViewState["activeCompany"]);            

            if (!IsPostBack)
            {
                if (Request.QueryString["cid"] != null)
                {
                    activeCompany = Convert.ToInt32(Request.QueryString["cid"]);
                    ViewState["activeCompany"] = activeCompany;                    
                }
                else
                {
                    Response.Redirect("./Dashboard.aspx");
                }
            }

            CompanyDAL companyDAL = new CompanyDAL();
            Company company = companyDAL.GetCompanyById(activeCompany);
            Title = "Calf & Cub - " + company.CompanyName;

            AccountDAL accountDAL = new AccountDAL();
            StockOwnership ownedStock =
                accountDAL.GetStockOwnership(Context.User.Identity.Name, company.Stock.Symbol);
            PopulatePageData(company, ownedStock);
            
            Dictionary<DateTime, float> stockHistory = companyDAL.GetStockHistoryById(company.Stock.Symbol);
            List<Series> series = new List<Series>();
            Chart1.Series["StockHistory"].Points.DataBindXY(stockHistory.Keys, stockHistory.Values);
            Chart1.Series["StockHistory"].BorderWidth = 5;

            ViewState["activeStock"] = company.Stock.Symbol;            
        }

        protected void PopulatePageData(Company company, StockOwnership ownedStock)
        {
            // STOCK INFORMATION
            imgLogo.ImageUrl = company.LogoUrl;
            lblSymbol.Text = company.Stock.Symbol;
            lblPrice.Text = company.Stock.ValuePerShare.ToString("C");
            float stockRate = company.Stock.GetStockRate();

            if(stockRate > 0) // positive rate
            {
                lblRate.Text = stockRate.ToString("P1");
                lblRate.ForeColor = System.Drawing.Color.Green;
                stockRateImg.ImageUrl = "../images/a-up.png";
            }
            else if (stockRate < 0) // negative rate
            {
                lblRate.Text = stockRate.ToString("P1");
                lblRate.ForeColor = System.Drawing.Color.Red;
                stockRateImg.ImageUrl = "../images/a-down.png";
            }
            else // neutral rate
            {
                lblRate.Text = stockRate.ToString("P1");
                lblRate.ForeColor = System.Drawing.Color.Black;
            }

            double ownedRate = 0.0;

            if (ownedStock != null)
            {
                ownedRate = ownedStock.CalCurrRate();
                lblOwnedStocks.Text = ownedStock.GetTotalQuantity().ToString();
            }

            if (ownedRate > 0) // positive rate
            {
                lblPlayerRate.Text = ownedRate.ToString("P1");
                lblPlayerRate.ForeColor = System.Drawing.Color.Green;
                playerRateImg.ImageUrl = "../images/a-up.png";
            }
            else if (ownedRate < 0) // negative rate
            {
                lblPlayerRate.Text = ownedRate.ToString("P1");
                lblPlayerRate.ForeColor = System.Drawing.Color.Red;
                playerRateImg.ImageUrl = "../images/a-down.png";
            }
            else // neutral rate
            {
                lblPlayerRate.Text = ownedRate.ToString("P1");
                lblPlayerRate.ForeColor = System.Drawing.Color.Black;
            }            

            // COMPANY INFORMATION            
            lblName.Text = company.CompanyName;
            lblDesc.Text = company.CompanyDesc;

            DataTable industryData = new DataTable();
            industryData.Columns.Add("Industry");
            foreach (Industry i in company.Industries)
            {
                DataRow dataRow = industryData.NewRow();
                dataRow["Industry"] = i;
                industryData.Rows.Add(dataRow);
            }
            IndustryRepeater.DataSource = industryData;
            IndustryRepeater.DataBind();

            // Populating Random Events Feed
            RandomEventDAL eventDAL = new RandomEventDAL();
            List<RandomEvent> events = eventDAL.GetEvents(null, company);
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
        

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            // if the user inputs for a new transaction are valid
            if (Page.IsValid)
            {        
                // converting selected radio to TransactionType enum
                TransactionType type = (TransactionType)Enum.Parse(typeof(TransactionType), 
                    ddType.SelectedValue, true); 
                int qty = Convert.ToInt32(txtQty.Text);                

                AccountDAL accountDAL = new AccountDAL();
                
                StockOwnership so = accountDAL.GetStockOwnership(
                    Context.User.Identity.Name, ViewState["activeStock"].ToString());

                Transaction transaction = new Transaction(type, 
                    so.Stock.ValuePerShare, qty);

                transaction.AccountStock = so;
                int result = accountDAL.AddTransaction(transaction);

                if (result > 0)
                {
                    double transactionTotal = qty * so.Stock.ValuePerShare;
                    if (type == TransactionType.BUY)
                    {
                        so.Account.Balance -= transactionTotal;
                    }
                    else
                    {
                        so.Account.Balance += transactionTotal;
                    }
                    accountDAL.UpdateAccount(so.Account);
                    Response.Redirect("./MyProfile.aspx?cid=" + so.Stock.Company.Id.ToString());
                }

            }

        }

        protected void NewTransaction_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CompanyDAL companyDAL = new CompanyDAL();
            AccountDAL accountDAL = new AccountDAL();

            Stock stock = companyDAL.GetStockById(ViewState["activeStock"].ToString());
            Account user = accountDAL.GetAccountByUsername(Context.User.Identity.Name, false);
            StockOwnership so = accountDAL.GetStockOwnership(user.Username, stock.Symbol);

            string type = ddType.SelectedValue;
            int qty = Convert.ToInt32(txtQty.Text);

            if (type == "BUY")
            {
                if (qty * stock.ValuePerShare > user.Balance)
                {
                    CustomValidator1.ErrorMessage = "You do not have enough money to buy that!";
                    args.IsValid = false;
                }
            }
            else if (type == "SELL")
            {
                if (!(so is null))
                {
                    if (so.GetTotalQuantity() - qty < 0)
                    {
                        CustomValidator1.ErrorMessage = "You do not have that many shares to sell!";
                        args.IsValid = false;
                    }
                }
                else
                {
                    CustomValidator1.ErrorMessage = "You do not have any shares to sell!";
                    args.IsValid = false;
                }
            }

            if (so is null)
                accountDAL.AddStockOwnership(user, stock);
        }


        protected void ddType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddType.SelectedIndex == 0)
            {
                btnProcess.Text = "Buy Now";
                btnProcess.CssClass = "btn btn-primary";
            }
            else
            {
                btnProcess.Text = "Sell Now";
                btnProcess.CssClass = "btn btn-danger";
            }
        }
    }
}