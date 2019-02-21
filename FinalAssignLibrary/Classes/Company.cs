using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalAssignLibrary.Enumerations;

namespace FinalAssignLibrary.Classes
{
    /// <summary>
    /// A class which contains all the variables and behaviors required for a Company
    /// within the Calf & Cub universe.
    /// </summary>
    public class Company
    {
        /// <summary>
        /// The primary key/identifier for the Company record
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// A list of all the Industries which this Company is associated with.
        /// There must be atleast 1 but may be more. (1..*)
        /// </summary>
        public List<Industry> Industries;
        /// <summary>
        /// The Stock object owned by this Company.
        /// </summary>
        private Stock stock;
        public Stock Stock
        {
            get { return stock; }
            set { stock = value; }
        }
        /// <summary>
        /// String containing the company's name
        /// </summary>
        private string companyName;
        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }
        /// <summary>
        /// String containing the company's description
        /// </summary>
        private string companyDesc;
        public string CompanyDesc
        {
            get { return companyDesc; }
            set { companyDesc = value; }
        }
        /// <summary>
        /// String containing the company's CEO name
        /// </summary>
        private string ceoName;
        public string CeoName
        {
            get { return ceoName; }
            set { ceoName = value; }
        }
        /// <summary>
        /// String containing the path to the company's logo image.
        /// </summary>
        private string logoUrl;
        public string LogoUrl
        {
            get { return logoUrl; }
            set { logoUrl = value; }
        }

        
        /// <summary>
        /// Constructor for creating Company objects. This constructor also accepts
        /// the parameters required to construct the Stock object which this Company
        /// is the owner of.
        /// </summary>
        /// <param name="id">Int containing the Company ID (Primary Key)</param>
        /// <param name="industries">List of industries this company is associated with</param>
        /// <param name="comName">String containing the company name</param>
        /// <param name="comDesc">String containing the company description</param>
        /// <param name="ceoName">String containing the company's CEO name</param>
        /// <param name="stockSymbol">String containing the company's Stock symbol</param>
        /// <param name="stockPrice">Float containing the Stock's value per share</param>
        /// <param name="logoUrl">String containing the URL/Path to the company's logo image</param>
        public Company(int id, List<Industry> industries, string comName, string comDesc, 
            string ceoName, string stockSymbol, float stockPrice, string logoUrl)
        {
            Id = id;
            Industries = industries;
            CompanyName = comName;
            CompanyDesc = comDesc;
            CeoName = ceoName;
            LogoUrl = logoUrl;

            // creates the Stock object which this company owns and stores it
            Stock s = new Stock(this, stockPrice, stockSymbol);
            Stock = s;
        }

        public override string ToString()
        {
            return CompanyName;
        }

        /**********************************************************************  
         *  
         *  THIS METHOD BECAME REDUNDANT AS OUR PROJECT PROGRESSED
         *  
         **********************************************************************
        /// <summary>
        /// Method used for generating a random stock price per share
        /// </summary>
        /// <returns>Float value containing the new Stock price per share.</returns>
        public float GenerateStockPrice()
        {
            float value;
            int min = 1;
            int max = 500;

            Random generator = new Random();
            return value = new Random().Next(min, max + 1);
        }
        */
    }
}