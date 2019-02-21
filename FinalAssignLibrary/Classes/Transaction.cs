using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalAssignLibrary.Enumerations;

namespace FinalAssignLibrary.Classes
{
    /// <summary>
    /// Class containing the parameters and behaviors for a Transaction in the 
    /// Calf & Cub universe. These are owned by StockOwnership objects.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// StockOwnership object which owns this Transaction record
        /// </summary>
        private StockOwnership accountStock;
        public StockOwnership AccountStock
        {
            get { return accountStock; }
            set { accountStock = value; }
        }
        /// <summary>
        /// Int containing the ID Primary key for the Transaction
        /// </summary>
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// TransactionType Enum - Either BUY or SELL
        /// </summary>
        private TransactionType type;
        public TransactionType Type
        {
            get { return type; }
            set { type = value; }
        }
        /// <summary>
        /// Float containing the price per share at which this Transaction was processed
        /// using
        /// </summary>
        private float transactionPrice;
        public float TransactionPrice
        {
            get { return transactionPrice; }
            set { transactionPrice = value; }
        }
        /// <summary>
        /// Int containing the quantity of shares either bought or sold
        /// </summary>
        private int transactionQuantity;
        public int TransactionQuantity
        {
            get { return transactionQuantity; }
            set { transactionQuantity = value; }
        }
        /// <summary>
        /// DateTime for when the transaction was processed
        /// </summary>
        private DateTime transactionDate;
        public DateTime TransactionDate
        {
            get { return transactionDate; }
            set { transactionDate = value; }
        }

        /// <summary>
        /// Constructor used when the user processes a new Transaction
        /// </summary>
        /// <param name="type">The type of transaction (Either BUY or SELL)</param>
        /// <param name="tPrice">Float containing the transaction price per share</param>
        /// <param name="tQty">Int containing the transaction quantity</param>
        public Transaction(TransactionType type, float tPrice, int tQty)
        {
            Type = type;
            TransactionPrice = tPrice;
            TransactionQuantity = tQty;
            TransactionDate = DateTime.Now;
        }

        /// <summary>
        /// Constructor used when retrieving the Transaction data from the database
        /// </summary>
        /// <param name="id">Int containing the Transaction ID</param>
        /// <param name="type">The type of transaction (Either BUY or SELL)</param>
        /// <param name="tPrice">Float containing the transaction price per share</param>
        /// <param name="tQty">Int containing the transaction quantity</param>
        /// <param name="tDate">DateTime containing the date and time the transaction occurred</param>
        public Transaction(int id, TransactionType type, int tQty, float tPrice, DateTime tDate)
        {
            Type = type;
            TransactionPrice = tPrice;
            TransactionQuantity = tQty;
            TransactionDate = tDate;
            Id = id;
        }
    }
}