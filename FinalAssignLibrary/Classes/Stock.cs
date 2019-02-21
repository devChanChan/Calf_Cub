using FinalAssignLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalAssignLibrary.Classes
{
    /// <summary>
    /// Class containing the parameters and behaviors for a Stock in the Calf & Cub
    /// universe. Stocks are always constructed by the Company who owns them.
    /// </summary>
    public class Stock
    {
        /// <summary>
        /// String containing the 4-Letter symbol for this Stock.
        /// </summary>
        private string symbol;
        public string Symbol
        {
            get { return symbol; }
            private set { symbol = value; }
        }
        /// <summary>
        /// Company who owns this stock.
        /// </summary>
        private Company company;
        public Company Company
        {
            get { return company; }
            set { company = value; }
        }
        /// <summary>
        /// Float containing the current value per share.
        /// </summary>
        private float valuePerShare;
        public float ValuePerShare
        {
            get
            {
                // if the value per share drops below 0, returns 0.
                if (valuePerShare >= 0)
                    return valuePerShare;
                else
                    return 0;
            }
            set { valuePerShare = value; }
        }
        /// <summary>
        /// Dictionary which can contain the price history for the Stock.
        /// </summary>
        private Dictionary<DateTime, float> priceHistory;
        /// <summary>
        /// Method used to add a value to the PriceHistory dictionary
        /// </summary>
        /// <param name="date">KEY - DateTime when the price was stored in the history table</param>
        /// <param name="price">VALUE - Float value of the price per stock at that time</param>
        public void AddPriceHistory(DateTime date, float price)
        {
            priceHistory.Add(date, price);
        }

        /// <summary>
        /// Constructor used to create a new Company Stock object
        /// </summary>
        /// <param name="company">Company who owns this Stock</param>
        /// <param name="symbol">String containing the 4-Letter primary key</param>
        public Stock(Company company, string symbol)
        {
            Company = company;
            ValuePerShare = 0.0f; // set the value per share to 0 by default
            priceHistory = new Dictionary<DateTime, float>(); 
            Symbol = symbol;
        }

        /// <summary>
        /// Constructor used to create a Company Stock object retrieved from the database
        /// </summary>
        /// <param name="company">Company who owns this Stock</param>
        /// <param name="valuePerShare">Float containing the current value per share</param>
        /// <param name="symbol">String containing the 4-Letter primary key</param>
        public Stock(Company company, float valuePerShare, string symbol)
        {
            Company = company;
            ValuePerShare = valuePerShare;
            priceHistory = new Dictionary<DateTime, float>();
            Symbol = symbol;
        }

        /// <summary>
        /// Method to retreive the stock history data and 
        /// return a calculated percentage rate to show the stock performance based 
        /// off the history data
        /// </summary>
        /// <returns>The rate of change of the stock price</returns>
        public float GetStockRate()
        {
            float rate = 0.0f;

            // populating the Stock's PriceHistory Dictionary.
            CompanyDAL companyDAL = new CompanyDAL();
            priceHistory = companyDAL.GetStockHistoryById(this.symbol);

            // Only if stock history started to be recorded
            if (priceHistory.Count > 0)
            {
                // After the today's stock history was recorded in the morning 
                if (priceHistory.ContainsKey(DateTime.Today))
                {
                    // the rate of change of stock price compared to today morning price
                    rate = (this.ValuePerShare - priceHistory[DateTime.Today]) / priceHistory[DateTime.Today];
                }
                // Before the today's stock history was recorded in the morning
                else
                {
                    // the rate of change of stock price compared to yesterday morning price
                    rate = (this.ValuePerShare - priceHistory[DateTime.Today.AddDays(-1)]) / priceHistory[DateTime.Today.AddDays(-1)];
                }
            }
            return rate;
        }
    }
}