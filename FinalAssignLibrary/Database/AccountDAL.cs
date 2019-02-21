using FinalAssignLibrary.Classes;
using FinalAssignLibrary.Enumerations;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalAssignLibrary.Database
{
    /// <summary>
    /// Data Access Layer Class which contains the methods required to retrieving 
    /// and storing data within the Calf & Cub database for tasks which are Account 
    /// related.
    /// </summary>
    public class AccountDAL
    {
        /// <summary>
        /// Stores SQL "connected" connection information
        /// </summary>
        ConnectionFactory connection = new ConnectionFactory();

        /// <summary>
        /// Method used during registration of a new user Account. Inserts the
        /// Account data into the database.
        /// </summary>
        /// <param name="account">Account to be inserted into the database</param>
        public void RegisterAccount(Account account)
        {
            string sql = String.Format("INSERT INTO account " +
                "VALUES (@username, @email, @password, @salt, " +
                "@balance, @ownedGroup)");

            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@username", account.Username);
                cmd.Parameters.AddWithValue("@email", account.Email);
                cmd.Parameters.AddWithValue("@password", account.Password);
                cmd.Parameters.AddWithValue("@salt", account.Salt);
                cmd.Parameters.AddWithValue("@balance", account.Balance);
                // if Account does not have an owned group, pass a null value to the DB
                if (account.OwnerOf == null)
                    cmd.Parameters.AddWithValue("@ownedGroup", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ownedGroup", account.OwnerOf);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Method used to obtain an Account from the database by the username.
        /// </summary>
        /// <param name="username">String containing the username of the Account which
        /// is being requested.</param>
        /// <param name="loginRequest">Boolean determining which Account constructor to
        /// user</param>
        /// <returns>Account object retrieved from the database</returns>
        public Account GetAccountByUsername(string username, bool loginRequest)
        {
            Account account = null;

            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT a.*, g.name, g.private FROM account a " +
                                            "LEFT OUTER JOIN [Group] g ON a.ownedGroup = g.id " +
                                            "WHERE username=@un", cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@un", username);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (loginRequest)
                    {
                        account = new Account(
                            reader["username"].ToString(),
                            reader["password"].ToString(),
                            reader["salt"].ToString()
                        );
                    }
                    else
                    {
                        if(!(String.IsNullOrEmpty(reader["ownedGroup"].ToString()))) // user owns a group
                        {
                            account = new Account(
                                reader["username"].ToString(),
                                Convert.ToDouble(reader["balance"]),
                                Convert.ToInt32(reader["ownedGroup"]),
                                reader["name"].ToString(),
                                Convert.ToBoolean(reader["private"])
                            );
                        }
                        else // user does not own a group
                        {
                            account = new Account(
                                reader["username"].ToString(),
                                Convert.ToDouble(reader["balance"]),
                                -1, null, false
                            );
                        }
                    }
                }
            }
            return account;
        }

        /// <summary>
        /// Method used to retrieve all user accounts from the database as well as
        /// their StockOwnership records. The data retrieved from this method is used
        /// to display on the Highscores & Ranking pages.
        /// </summary>
        /// <returns>Returns a list of Accounts with their populated StockOwnership lists</returns>
        public List<Account> GetAccounts()
        {
            List<Account> accounts = new List<Account>();

            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                    "SELECT a.*, g.name, g.private FROM account a " +
                        "LEFT OUTER JOIN [Group] g ON a.ownedGroup = g.id ", cnn))
            {
                cnn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Account account = null;
                    account = new Account(
                        reader["username"].ToString(),
                        Convert.ToDouble(reader["balance"]),
                        -1, null, false
                    );
                    account.Stocks = GetStockOwnership(account);
                    accounts.Add(account);
                }
            }
            return accounts;
        }

        /// <summary>
        /// Method used to retrieve all StockOwnership records by account.
        /// </summary>
        /// <param name="account">Account object associated with the stock purchases</param>
        /// <returns>List of StockOwnership records associated with the provided Account</returns>
        public List<StockOwnership> GetStockOwnership(Account account)
        {
            List<StockOwnership> stocks = new List<StockOwnership>();
            
            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                    "SELECT so.*, s.valuePerShare, c.id AS cid, c.name as cname, c.description, c.ceoName, c.logoUrl " +
                    "FROM stockownership so " +
                    "JOIN stock s ON so.stock = s.id " +
                    "JOIN company c ON s.companyID = c.id " +
                    "WHERE username=@un", cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@un", account.Username);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    List<Company> companies = new List<Company>();
                    CompanyDAL companyDAL = new CompanyDAL();
                   

                    Company c = new Company(
                        Convert.ToInt32(reader["cid"]),
                        companyDAL.GetCompanyIndustries(Convert.ToInt32(reader["cid"])),
                        reader["cname"].ToString(),
                        reader["description"].ToString(),
                        reader["ceoName"].ToString(),
                        reader["stock"].ToString(),
                        Convert.ToSingle(reader["valuePerShare"]),
                        reader["logoUrl"].ToString()
                    );
                    List<Transaction> transactions = GetTransactionsBySOId(Convert.ToInt32(reader["id"]));

                    StockOwnership stock = new StockOwnership(
                        Convert.ToInt32(reader["id"]), account, c.Stock, transactions);
                    stocks.Add(stock);
                }
            }
            
            return stocks;
        }


        /// <summary>
        /// Method used to retrieve a single StockOwnership record from the database.
        /// </summary>
        /// <param name="username">String with the account username</param>
        /// <param name="stockSymbol">String with the stock symbol</param>
        /// <returns>A StockOwnership record</returns>
        public StockOwnership GetStockOwnership(string username, string stockSymbol)
        {
            StockOwnership playerStock = null;

            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT * " +
                                    "FROM stockownership " +
                                    "WHERE username=@username " +
                                    "AND stock=UPPER(@symbol)", cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@symbol", stockSymbol);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    List<Transaction> transactions =
                        this.GetTransactionsBySOId(Convert.ToInt32(reader["id"]));

                    CompanyDAL companyDAL = new CompanyDAL();
                    playerStock = new StockOwnership(
                        Convert.ToInt32(reader["id"]),
                        GetAccountByUsername(username, false),
                        companyDAL.GetStockById(stockSymbol),
                        transactions);
                }
            }
            return playerStock;
        }

        /// <summary>
        /// Method used to add a new StockOwnership record into the Database
        /// </summary>
        /// <param name="account">Account associated with this record</param>
        /// <param name="stock">Stock associated with this record</param>
        /// <returns>Int value representing the amount of records added to the 
        /// database (for tracking purposes)</returns>
        public int AddStockOwnership(Account account, Stock stock)
        {
            int added = 0;
            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "INSERT INTO stockownership (username, stock) " +
                    "VALUES (@username, @stock)", cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@username", account.Username);
                cmd.Parameters.AddWithValue("@stock", stock.Symbol);
                added = cmd.ExecuteNonQuery();
            }
            return added;
        }

        /// <summary>
        /// Method used to get all Transactions by the StockOwnership ID which they
        /// are associated with. Each Transaction is affiliated with one StockOwnership
        /// record which contains the Stock and Account details.
        /// </summary>
        /// <param name="id">Integer containing the StockOwnership ID</param>
        /// <returns>List of Transactions associated with the given StockOwnership ID</returns>
        public List<Transaction> GetTransactionsBySOId (int id)
        {
            List<Transaction> transactions = new List<Transaction>();
            
            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM [Transaction] " +
                                        "WHERE stockownershipid=@id", cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TransactionType type = (TransactionType)Enum.Parse(
                        typeof(TransactionType), reader["type"].ToString(), true);

                    Transaction transaction = new Transaction(
                        Convert.ToInt32(reader["id"]),
                        type,
                        Convert.ToInt32(reader["quantity"]),
                        Convert.ToSingle(reader["price"]),
                        Convert.ToDateTime(reader["date"])
                    );

                    transactions.Add(transaction);
                }
            }
            
            return transactions;
        }
        
        /// <summary>
        /// Method used when pushing updates for an Account to the database. Will be
        /// utilized in cases to update account balance and/or group ownership. 
        /// </summary>
        /// <param name="account">Account object which is being updated</param>
        public void UpdateAccount(Account account)
        {
            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                            "UPDATE account " +
                            "SET balance=@balance, " +
                                "ownedGroup=@ownedGroup " +
                            "WHERE UPPER(username)=@username", cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@username", account.Username.ToUpper());
                if(account.OwnerOf is null)
                    cmd.Parameters.AddWithValue("@ownedGroup", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ownedGroup", account.OwnerOf.Id);
                cmd.Parameters.AddWithValue("@balance", account.Balance);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Method used to Add a new Transaction into the database.
        /// </summary>
        /// <param name="t">Transaction which is being added to the database</param>
        /// <returns>Int value representing the amount of records added to the 
        /// database (for tracking purposes)</returns>
        public int AddTransaction(Transaction t)
        {
            int updated = 0;
            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                            "INSERT INTO [Transaction] (quantity, price, date, type, stockownershipid) " +
                            "VALUES (@qty, @price, @date, @type, @soId)", cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@qty", t.TransactionQuantity);
                cmd.Parameters.AddWithValue("@price", t.TransactionPrice);
                cmd.Parameters.AddWithValue("@date", t.TransactionDate);
                cmd.Parameters.AddWithValue("@type", t.Type.ToString());
                cmd.Parameters.AddWithValue("@soId", t.AccountStock.Id);
                updated = cmd.ExecuteNonQuery();
            }
            return updated;
        }

        
    }
}
