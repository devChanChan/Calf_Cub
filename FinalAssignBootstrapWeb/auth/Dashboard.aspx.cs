using FinalAssignLibrary.Classes;
using FinalAssignLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace FinalAssignBootstrapWeb.auth
{
    public partial class Dashboard : System.Web.UI.Page
    {
        Random r = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Changing the navbar to highlight the appropriate link as active
            HyperLink pageLink = this.Page.Master.FindControl("lnkDashboard") as HyperLink;
            pageLink.CssClass += " active";

            // Populating the Company Cards
            CompanyDAL companyDAL = new CompanyDAL();
            List<Company> companies = companyDAL.GetCompanies();
            PopulateCompanies(companies);

            // Populating the RandomEvents Feed
            RandomEventDAL eventDAL = new RandomEventDAL();
            List<RandomEvent> events = eventDAL.GetEvents();
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

                ddCompanies.Items.Add(c.CompanyName);
            }

            CompanyRepeater.DataSource = companyTable;
            CompanyRepeater.DataBind();
        }

        protected void ddCompanies_SelectedIndexChanged(object sender, EventArgs e)
        {
            string companyName = ddCompanies.SelectedValue;
            CompanyDAL companyDAL = new CompanyDAL();
            List<Company> companies = companyDAL.GetCompanies();
            foreach(Company c in companies)
            {
                if (companyName.Equals(c.CompanyName))
                    Response.Redirect("./ViewCompany.aspx?cid=" + c.Id);
            }
        }
    }
}