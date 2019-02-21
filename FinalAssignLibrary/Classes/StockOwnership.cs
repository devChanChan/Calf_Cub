using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using FinalAssignLibrary.Enumerations;

namespace FinalAssignLibrary.Classes
{
    /// <summary>
    /// Class containing the parameters and behaviors for a StockOwnership in the 
    /// Calf & Cub universe. These are used to track the Stocks which a user has
    /// created Transactions for (Stock-Account association class).
    /// </summary>
    public class StockOwnership 
    {
        /// <summary>
        /// Int containing the Primary Key for this Account-Stock relationship.
        /// </summary>
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// Stock which the user is buying/selling.
        /// </summary>
        private Stock stock;
        public Stock Stock
        {
            get { return stock; }
            set { stock = value; }
        }
        /// <summary>
        /// Account object for the user
        /// </summary>
        private Account account;
        public Account Account
        {
            get { return account; }
            set { account = value; }
        }
        /// <summary>
        /// List containing all Transactions which are owned by this StockOwnership
        /// relationship.
        /// </summary>
        public List<Transaction> Transactions;

        
        /// <summary>
        /// Constructor for creating a new StockOwnership relationship for an account.
        /// </summary>
        /// <param name="owner">Account who will own the Stock</param>
        /// <param name="stock">Stock which the user is targetting</param>
        public StockOwnership(Account owner, Stock stock)
        {
            Account = owner;
            Stock = stock;
            Transactions = new List<Transaction>();
        }

        /// <summary>
        /// Constructor used when retrieving a StockOwnership relationship from the
        /// database
        /// </summary>
        /// <param name="id">Integer containing the id</param>
        /// <param name="owner">Account who will own the Stock</param>
        /// <param name="stock">Stock which is being bought/sold</param>
        /// <param name="transactions"></param>
        public StockOwnership(int id, Account owner, Stock stock, List<Transaction> transactions)
        {
            Id = id;
            Account = owner;
            Stock = stock;
            Transactions = transactions;
        }        

        /// <summary>
        /// Get the current profit rate of the stock
        /// (Qty x (Current Price - Avg Purchasing Price)) / (Qty x Avg Purchasing Price)
        /// </summary>
        /// <returns>Double representing the stock rate percentage</returns>
        public double CalCurrRate()
        {
            double buyQtys = 0;  // total quantity of the purchased stock
            double buyAmts = 0;  // total amount(value) of the purchased stock
            double avgBuyPrice = 0.0; // Weighted average of the purchasing price
            double currentRate = 0.0; // current profit rate of the stock

            // Execute if there is one more than transaction
            if (Transactions.Count > 0)
            {
                // Iterate the transcations of the stock 
                foreach (var t in Transactions)
                {
                    // Execute if the transaction is BUY type
                    if (t.Type == TransactionType.BUY)
                    {
                        // Add the purchased quantity of all of the transactions of the Stock
                        buyQtys += t.TransactionQuantity;
                        // Add the purchased amount(value) of all of the transactions of the Stock
                        buyAmts += t.TransactionPrice * t.TransactionQuantity;
                    }
                }
                avgBuyPrice = buyAmts / buyQtys; // Weighted average of the purchasing price 

                // Current profit rate of the stock based on the current price of stock
                // and average of purchasing price of the stock
                currentRate = (stock.ValuePerShare - avgBuyPrice) / avgBuyPrice;
            }
            return currentRate;
        }

        /// <summary>
        /// Method used to calculate the total quantity which this user currently
        /// owns.
        /// </summary>
        /// <returns>Int value representing the quantity the user has available
        /// to sell.</returns>
        public int GetTotalQuantity()
        {
            if(Transactions is null || Transactions.Count == 0)
            {
                return 0;
            }
            else
            {
                int totalQty = 0;

                foreach (Transaction t in Transactions)
                    if (t.Type == TransactionType.BUY)
                        totalQty += t.TransactionQuantity;
                    else
                        totalQty -= t.TransactionQuantity;

                return totalQty;
            }
        }


        /**********************************************************************  
         *  
         *  THESE METHODS BECAME REDUNDANT AS OUR PROJECT PROGRESSED, THE FOCUS
         *  SWITCHED TO RELY HEAVILY ON USING DATA ACCESS OBJECTS TO PERFORM THESE 
         *  STOCKOWNERSHIP TASKS.
         *  
         **********************************************************************
        public Transaction Buy(int buyQty)
        {
            Transaction transaction = new Transaction(TransactionType.BUY, stock.ValuePerShare, buyQty, DateTime.Now);
            AddTransaction(transaction);

            return transaction;
        }

        public Transaction Sell(int sellQty)
        {
            Transaction transaction = new Transaction(TransactionType.SELL, stock.ValuePerShare, sellQty, DateTime.Now);
            AddTransaction(transaction);

            if (GetTotalQuantity() >= sellQty)
            {
                return transaction;
            }
            else
            {
                throw new ArgumentException("You don't have enough stock to sell.");
            }
        }

        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }
        */
    }
}