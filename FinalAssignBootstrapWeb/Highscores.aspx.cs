using FinalAssignLibrary.Classes;
using FinalAssignLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalAssignBootstrapWeb
{
    public partial class Highscores : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {   
            // Changing the navbar to highlight the appropriate link as active
            HyperLink pageLink = this.Page.Master.FindControl("lnkScores") as HyperLink;
            pageLink.CssClass += " active";

            DataTable playerData = new DataTable();
            playerData.Columns.Add("Rank");
            playerData.Columns.Add("Username");
            playerData.Columns.Add("UserNetworth");
            playerData.Columns.Add("ToSort", typeof(double));
            AccountDAL accountDAL = new AccountDAL();
            List<Account> accounts = accountDAL.GetAccounts();


            foreach (Account a in accounts) 
            {
                double value = a.GetAccountValue();
                DataRow dataRow = playerData.NewRow();
                dataRow["Username"] = a.Username;
                dataRow["UserNetworth"] = value.ToString("C");
                dataRow["ToSort"] = value;
                playerData.Rows.Add(dataRow);
            }
                        
            DataView dv = playerData.DefaultView;
            dv.Sort = "ToSort desc";
            playerData = dv.ToTable();

            int count = 1;

            foreach (DataRow dt in playerData.Rows)
            {
                dt["Rank"] = count;
                count++;
            }
            
            HighscoresUsers.DataSource = playerData;
            HighscoresUsers.DataBind();

            if (Request.IsAuthenticated)
            {
                string searchCriteria = "Username='" + Context.User.Identity.Name+"'";
                DataRow[] playerScore = playerData.Select(searchCriteria);
                rank.InnerText = playerScore[0][0].ToString();
                username.InnerText = playerScore[0][1].ToString();
                networth.InnerText = playerScore[0][2].ToString();
            }
            else
            {
                ActivePlayer.Visible = false;
            }
        }
    }
}