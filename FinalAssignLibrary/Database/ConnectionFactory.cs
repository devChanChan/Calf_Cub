using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalAssignLibrary.Database
{
    /// <summary>
    /// Class used by the Data Access Layer classes to establish "Connected" connections
    /// to the database in order to retrieve or store data.
    /// </summary>
    public class ConnectionFactory
    {
        // JORDAN
        //private string sql = "Data Source=JORDID-LAPTOP;Initial Catalog=CalfCubDB;Integrated Security=True";
        // CHAN
        private string sql = "Data Source=DESKTOP-U9NL06B;Initial Catalog=CalfCubDB;Integrated Security=True";
        public string Sql
        {
            get { return sql; }
            private set { sql = value; }
        }

        /// <summary>
        /// Explicit default constructor
        /// </summary>
        public ConnectionFactory()
        {
        }

        /// <summary>
        /// Method used to establish a "connected" connection using the Sql 
        /// Connection String parameter.
        /// </summary>
        /// <returns>SqlConnection object</returns>
        public SqlConnection GetConnection()
        {
            SqlConnection cnn = new SqlConnection(@sql);
            return cnn;
        }
    }
}
