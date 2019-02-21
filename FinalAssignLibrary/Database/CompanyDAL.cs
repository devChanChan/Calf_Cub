using FinalAssignLibrary.Classes;
using FinalAssignLibrary.Enumerations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalAssignLibrary.Database
{
    /// <summary>
    /// Data Access Layer Class which contains the methods required to retrieving 
    /// and storing data within the Calf & Cub database for tasks which are Company
    /// related.
    /// </summary>
    public class CompanyDAL
    {
        /// <summary>
        /// Stores SQL "connected" connection information
        /// </summary>
        ConnectionFactory connection = new ConnectionFactory();
        /// <summary>
        /// SqlDataAdapter used for "offline" connections
        /// </summary>
        SqlDataAdapter companyAdapter = null;
        /// <summary>
        /// DataTable which holds the offline data retrieved from the Adapter
        /// </summary>
        DataTable companiesTable = null;

        /// <summary>
        /// Constructor used to set up an offline table of Company data and provide
        /// access to the associated Company DAL methods.
        /// </summary>
        public CompanyDAL()
        {
            string conn = connection.Sql;
            companyAdapter = new SqlDataAdapter("SELECT c.*, s.id AS stockId, s.valuePerShare " +
                                                "FROM company AS c " +
                                                "JOIN stock AS s ON s.id = c.stockid ", conn);
            companiesTable = new DataTable();
            companyAdapter.Fill(companiesTable);
        }

        /// <summary>
        /// Method used to get the Industries which are associated with a particular
        /// Company. Each Company should have at least 1 industry affiliations but 
        /// may have several.
        /// </summary>
        /// <param name="id">Integer containing the Company ID whose Industries are
        /// being requested.</param>
        /// <returns>List of Industry Enums which are associated with the provided
        /// Company.</returns>
        public List<Industry> GetCompanyIndustries(int id)
        {
            List<Industry> industries = new List<Industry>();
            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT c.id, i.id " +
                                                    "FROM company c " +
                                                    "JOIN companyindustry ci ON ci.companyid = c.id " +
                                                    "JOIN industry i ON ci.industryid = i.id " +
                                                    "WHERE c.id=@id", cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string i = reader[1].ToString();
                    Industry ind = (Industry)Enum.Parse(typeof(Industry), i, true);
                    industries.Add(ind);
                }
            }
            return industries;
        }

        /// <summary>
        /// Method used to get all company records from the database.
        /// Each company will construct it's owned Stock object as well.
        /// </summary>
        /// <returns>List of Companies which have been constructed via the
        /// database</returns>
        public List<Company> GetCompanies()
        {
            List<Company> companies = null;
            if (companiesTable.Rows.Count > 0)
            {
                companies = new List<Company>();
                foreach (DataRow row in companiesTable.Rows)
                {
                    List<Industry> industry = this.GetCompanyIndustries(Convert.ToInt32(row["id"]));
                    Company company = new Company(
                        Convert.ToInt32(row["id"]), 
                        industry, 
                        row["name"].ToString(), 
                        row["description"].ToString(), 
                        row["ceoName"].ToString(),
                        row["stockId"].ToString(), 
                        Convert.ToSingle(row["valuePerShare"]),
                        row["logoUrl"].ToString()
                    );
                    companies.Add(company);
                }
            }
            return companies;
        }

        /// <summary>
        /// Method used to retrieve a Stock from the database by it's 4-letter symbol
        /// identifier.
        /// </summary>
        /// <param name="id">A string containing the 4-characters used for the Stock
        /// Symbol</param>
        /// <returns>The requested Stock object</returns>
        public Stock GetStockById(string id)
        {
            Stock stock = null;
            if (companiesTable.Rows.Count > 0)
            {
                DataRow[] rows = companiesTable.Select($"stockId = '{id}'");

                if(rows.Length > 0)
                {
                    Company company = GetCompanyById(Convert.ToInt32(rows[0]["id"]));
                    stock = company.Stock;
                }
                
            }
            return stock;
        }

        /// <summary>
        /// Method used to retrieve a stock from the database using the Company ID
        /// </summary>
        /// <param name="id">Integer containing the Company ID.</param>
        /// <returns>Stock which matches the Company ID provided</returns>
        public Stock GetStockById(int id)
        {
            Stock stock = null;
            if (companiesTable.Rows.Count > 0)
            {
                DataRow[] rows = companiesTable.Select($"id = {id}");

                if (rows.Length > 0)
                {
                    Company company = GetCompanyById(id);
                    stock = company.Stock;
                }

            }
            return stock;
        }

        /// <summary>
        /// Method used to retrieve a Company from the database by it's ID.
        /// </summary>
        /// <param name="id">Integer containing the Company identifier</param>
        /// <returns>The requested Company object</returns>
        public Company GetCompanyById(int id)
        {
            Company company = null;
            if (companiesTable.Rows.Count > 0)
            {
                DataRow[] rows = companiesTable.Select($"id = {id}");

                if (rows.Length > 0)
                {
                    List<Industry> industry = GetCompanyIndustries(id);                    
                    company = new Company(
                        id,
                        industry, 
                        rows[0]["name"].ToString(), 
                        rows[0]["description"].ToString(),
                        rows[0]["ceoName"].ToString(), 
                        rows[0]["stockId"].ToString(),
                        Convert.ToSingle(rows[0]["valuePerShare"]),
                        rows[0]["logoUrl"].ToString()
                    );
                }

            }
            return company;
        }

        /// <summary>
        /// Method used to retrieve StockHistory from the database by it's 4-letter symbol
        /// identifier
        /// </summary>
        /// <param name="id">String containing the Stock identifier</param>
        /// <returns>The History of the date and stock price pair</returns>
        public Dictionary<DateTime, float> GetStockHistoryById(string id)
        {
            Dictionary<DateTime, float> stockHistory = new Dictionary<DateTime, float>();

            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT date, price " +
                                                    "FROM stockhistory sh " +
                                                    "WHERE sh.stockid=@id " +
                                                    "ORDER BY date ASC", cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stockHistory.Add(Convert.ToDateTime(reader["date"]), (float)Convert.ToDouble(reader["price"]));
                }
            }

            return stockHistory;
        }
    }
}
