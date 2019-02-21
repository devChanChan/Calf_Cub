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
    /// and storing data within the Calf & Cub database for tasks which are Random
    /// Event related.
    /// </summary>
    public class RandomEventDAL
    {
        /// <summary>
        /// SqlDataAdapter used for "offline" connections
        /// </summary>
        SqlDataAdapter eventsAdapter = null;
        /// <summary>
        /// DataTable which holds the offline data retrieved from the Adapter
        /// </summary>
        DataTable eventsTable = null;

        /// <summary>
        /// Constructor to allow access to the RandomEvent's Data Access Layer
        /// </summary>
        public RandomEventDAL()
        {
            // creating ConnectionFactory instance to get access to the SQL Connection String
            ConnectionFactory connection = new ConnectionFactory();
            string conn = connection.Sql;

            eventsAdapter = new SqlDataAdapter("SELECT * FROM RandomEvent", conn);

            eventsTable = new DataTable();
            eventsAdapter.Fill(eventsTable);
        }

        /// <summary>
        /// Method used to retrieve all Event records from the database. All records
        /// can be obtained or they can be filtered by a specific Company or a list of
        /// Companies depending on the requirement of the page which will display them.
        /// </summary>
        /// <param name="companies">List of Company objects that Events will be retrieved for</param>
        /// <param name="company">A single Company which Events will be retrieved for</param>
        /// <returns>List of RandomEvent objects</returns>
        public List<RandomEvent> GetEvents(List<Company> companies = null, Company company = null)
        {
            List<RandomEvent> events = new List<RandomEvent>();

            // For LandingPage and Dashboard (Retrieves all Event records)
            string filterEvents = "1 = 1 ";

            // For ViewCompany page (Single Company filter)
            if(!(company is null))
            {
                filterEvents = "targetedCompanyID = " + company.Id + " OR targetedIndustryID IN (";
                int count = company.Industries.Count;
                foreach (Industry i in company.Industries)
                {
                    filterEvents += ((int)i);
                    count--;
                    if (count > 0)
                        filterEvents += ", ";
                    else
                        filterEvents += ")";
                }
            }
            // For MyProfile page (List of Companies filter)
            else if (!(companies is null))
            {
                filterEvents = "targetedCompanyID IN (";

                string companyIDs = "";
                string industryIDs = "";

                int count = companies.Count;
                foreach (Company c in companies)
                {
                    companyIDs += c.Id;
                    count--;
                    if (count > 0)
                        companyIDs += ", ";
                    else
                        companyIDs += ") ";

                    foreach (Industry i in c.Industries)
                    {
                        int ind = (int)i;
                        industryIDs += ind + ", ";
                    }
                }
                filterEvents += companyIDs + " OR targetedIndustryID IN (";
                char[] charsToTrim = { ',', ' ' };
                industryIDs = industryIDs.TrimEnd(charsToTrim);
                filterEvents += industryIDs + ")";
            }

            string orderBy = "time DESC";
            DataRow[] rows = eventsTable.Select($"{filterEvents}", orderBy);
            if(rows.Length > 0)
            {
                CompanyDAL companyDAL = new CompanyDAL();
                foreach(DataRow row in rows)
                {
                    // checking to see if industry is null or not
                    Industry targetedIndustry;                    
                    if (row[5] != DBNull.Value)
                        targetedIndustry = (Industry)Convert.ToInt32(row[5]);
                    else
                        targetedIndustry = Industry.None;
                    // checking to see if company is null or not
                    Company targetedCompany;
                    if (row[4] != DBNull.Value)
                        targetedCompany = companyDAL.GetCompanyById(Convert.ToInt32(row[4]));
                    else
                        targetedCompany = null;
                    // extracting the EventType from the record
                    EventType eventType = (EventType)Convert.ToInt32(row[3]);
                    
                    // building the RandomEvent object
                    RandomEvent e = new RandomEvent(
                        targetedIndustry,
                        targetedCompany,
                        Convert.ToBoolean(row[1]),
                        Convert.ToSingle(row[2]),
                        eventType,
                        Convert.ToDateTime(row[6])
                    );
                    events.Add(e);
                }
            }
            return events;
        }
    }
}
