using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalAssignLibrary.Enumerations;

namespace FinalAssignLibrary.Classes
{
    /// <summary>
    /// Class object containing the parameters and behaviors for the Calf & Cub
    /// RandomEvent records. These are randomly created by the database via a
    /// Cron job and saved to the RandomEvent table.
    /// </summary>
    public class RandomEvent
    {
        /// <summary>
        /// Industry which was targeted by the event. May contain a value or be null.
        /// </summary>
        private Industry targetedIndustry;
        public Industry TargetedIndustry
        {
            get { return targetedIndustry; }
            private set { targetedIndustry = value; }
        }
        /// <summary>
        /// Company which was targeted by the event. May contain a value or be null.
        /// </summary>
        private Company targetedCompany;
        public Company TargetedCompany
        {
            get { return targetedCompany; }
            private set { targetedCompany = value; }
        }
        /// <summary>
        /// Bool tracking whether the Event is a positive or negative one
        /// </summary>
        private bool isGood;
        public bool IsGood
        {
            get { return isGood; }
            private set { isGood = value; }
        }
        /// <summary>
        /// Float containing the weight of the event (how much the share price was
        /// adjusted by the event).
        /// </summary>
        private float weight;
        public float Weight
        {
            get { return weight; }
            private set { weight = value; }
        }
        /// <summary>
        /// String containing the description of the Event which is generated based
        /// on the EventType, whether the Event was positive or negative and whether
        /// the Event was industry level or company level.
        /// </summary>
        private string eventDesc;
        public string EventDesc
        {
            get { return eventDesc; }
            private set { eventDesc = value; }
        }
        /// <summary>
        /// EventType Enum tracking what the main cause of the Event was
        /// </summary>
        private EventType eventType;
        public EventType EventType
        {
            get { return eventType; }
            private set { eventType = value; }
        }
        /// <summary>
        /// The Date and Time when the event occurred
        /// </summary>
        private DateTime occurredAt;
        public DateTime OccurredAt
        {
            get { return occurredAt; }
            private set { occurredAt = value; }
        }
        
        /// <summary>
        /// Constructor used when the data is retrieved from the database to 
        /// construct the RandomEvent objects
        /// </summary>
        /// <param name="targetI">Industry which was targeted by the Event.</param>
        /// <param name="targetC">Company which was targeted by the Event. May be null.</param>
        /// <param name="isGood">Bool which determines if the event was positive or negative</param>
        /// <param name="weight">Float containing the weight of the event</param>
        /// <param name="eventType">EventType Enum containing the main cause of the event</param>
        /// <param name="occurredAt">DateTime containing the date/time the event occurred</param>
        public RandomEvent(Industry targetI, Company targetC, Boolean isGood, 
            float weight, EventType eventType, DateTime occurredAt)
        {
            // checking if industry is None (Industry Enum version of null in Calf & Cub)
            if(!(targetI == Industry.None))
                TargetedIndustry = targetI;
            TargetedCompany = targetC;
            IsGood = isGood;
            Weight = weight / 100;
            EventType = eventType;
            OccurredAt = occurredAt;
            EventDesc = GetDescription(); // Retrieving the Event description string
        }

        /// <summary>
        /// Method used to generate an Event description. This method is called during
        /// creation of the Event object.
        /// </summary>
        /// <returns>String containing the event description</returns>
        private string GetDescription()
        {
            string desc = "";

            if (EventType == EventType.CeoQuit) // the EventType that occurred
            {
                if (IsGood) // is the event positive?
                {                    
                    if (targetedCompany is null) // industry-level event?
                    {
                        desc = "Many resident CEO's in the <strong>" + TargetedIndustry + "</strong> industry are quitting their jobs leaving openings for younger fresher leaders to step up and revolutionalize.";
                    }
                    else // company-level event?
                    {
                        desc = "The tyrannical CEO of <strong>" + TargetedCompany + "</strong> has quit leading board members and shareholders to breathe a sigh of relief.";
                    }
                }
                else // is the event negative?
                {
                    if (targetedCompany is null) // industry-level event?
                    {
                        desc = "A major scandal has hit the <strong>" + TargetedIndustry + "</strong> industry leading to the resignation of many top CEO's.";
                    }
                    else // company-level event?
                    {
                        desc = "The iconic CEO of <strong>" + TargetedCompany + "</strong> has quit leading to uncertainty regarding the company's future.";
                    }
                }
            }
            else if (EventType == EventType.StockPriceChange)
            {
                if (IsGood)
                {
                    if (targetedCompany is null)
                    {
                        desc = "The <strong>" + TargetedIndustry + "</strong> industry is experiencing positive sales growth and profits.";
                    }
                    else
                    {
                        desc = "The <strong>" + TargetedCompany + "</strong> industry is experiencing positive sales growth and profits.";
                    }
                }
                else
                {
                    if (targetedCompany is null)
                    {
                        desc = "The <strong>" + TargetedIndustry + "</strong> industry is experiencing negative sales growth and profits.";
                    }
                    else
                    {
                        desc = "The <strong>" + TargetedCompany + "</strong> industry is experiencing negative sales growth and profits.";
                    }
                }
            }
            else if (EventType == EventType.CeoFired)
            {
                if (IsGood)
                {
                    if (targetedCompany is null)
                    {
                        desc = "The <strong>" + TargetedIndustry + "</strong> industry is draining the swamp by collectively firing many of their CEO's in a power move to improve their value.";
                    }
                    else
                    {
                        desc = "<strong>" + TargetedCompany + "</strong> has fired their CEO who has been beseiged in scandal for many weeks now.";
                    }
                }
                else
                {
                    if (targetedCompany is null)
                    {
                        desc = "The <strong>" + TargetedIndustry + "</strong> industry is attempting to drain the swamp by collectively firing many of their CEO's in a power move which at a glance seems to be causing panic amongst their investors.";
                    }
                    else
                    {
                        desc = "<strong>" + TargetedCompany + "</strong> has fired their CEO, a move which some are saying puts the company in a bad position competitively.";
                    }
                }
            }
            else if (EventType == EventType.CompanyBankruptcy)
            {
                if (IsGood)
                {
                    if (targetedCompany is null)
                    {
                        desc = "The <strong>" + TargetedIndustry + "</strong> industry is experiencing widespread financial difficulties which is giving them the upper hand at the negotiations table.";
                    }
                    else
                    {
                        desc = "<strong>" + TargetedCompany + "</strong> has announced that they will file for bankruptcy in a move that has analysts agreeing that the move makes their future outlook positive.";
                    }
                }
                else
                {
                    if (targetedCompany is null)
                    {
                        desc = "The <strong>" + TargetedIndustry + "</strong> is experiencing widespread financial difficulties leading to mass panic amongst investors.";
                    }
                    else
                    {
                        desc = "<strong>" + TargetedCompany + "</strong> has filed for bankruptcy in a move that has analysts predicting that this move may be their downfall.";
                    }
                }
            }
            else if (EventType == EventType.NewBreakthrough)
            {
                if (IsGood)
                {
                    if (targetedCompany is null)
                    {
                        desc = "The <strong>" + TargetedIndustry + "</strong> industry has announced a massive new breakthrough which will revolutionalize the entire industry.";
                    }
                    else
                    {
                        desc = "<strong>" + TargetedCompany + "</strong> has announced a massive new breakthrough which they have patented.";
                    }
                }
                else
                {
                    if (targetedCompany is null)
                    {
                        desc = "The <strong>" + TargetedIndustry + "</strong> industry has announced a new breakthrough which will pose a challenge to the future of their entire industry.";
                    }
                    else
                    {
                        desc = "<strong>" + TargetedCompany + "</strong> has made a new breakthrough that has led to a global backlash against their products.";
                    }
                }
            }
            else if (EventType == EventType.EnergyCrisis)
            {
                if (IsGood)
                {
                    if (targetedCompany is null)
                    {
                        desc = "An energy crisis is leading to a higher than normal demand for the <strong>" + TargetedIndustry + "</strong> industry.";
                    }
                    else
                    {
                        desc = "An energy crisis is leading to a higher than normal demand for <strong>" + TargetedCompany + "</strong>'s products.";
                    }
                }
                else
                {
                    if (targetedCompany is null)
                    {
                        desc = "The <strong>" + TargetedIndustry + "</strong> industry is experiencing an energy crisis which is crippling their marketability.";
                    }
                    else
                    {
                        desc = "An energy crisis is hitting <strong>" + TargetedCompany + "</strong> hard.";
                    }
                }
            }
            else if (EventType == EventType.SupplyCrisis)
            {
                if (IsGood)
                {
                    if (targetedCompany is null)
                    {
                        desc = "A supply crisis is leading to a higher than normal demand for the <strong>" + TargetedIndustry + "</strong> industry.";
                    }
                    else
                    {
                        desc = "A supply crisis is leading to a higher than normal demand for <strong>" + TargetedCompany + "</strong>'s products.";
                    }
                }
                else
                {
                    if (targetedCompany is null)
                    {
                        desc = "The <strong>" + TargetedIndustry + "</strong> industry is experiencing a supply crisis which is crippling their marketability.";
                    }
                    else
                    {
                        desc = "A supply crisis is hitting <strong>" + TargetedCompany + "</strong> hard.";
                    }
                }
            }
            else if (EventType == EventType.GlobalTradeWar)
            {
                if (IsGood)
                {
                    if (targetedCompany is null)
                    {
                        desc = "The <strong>" + TargetedIndustry + "</strong> industry is currently benefitting from the Global Trade War.";
                    }
                    else
                    {
                        desc = "<strong>" + TargetedCompany + "</strong> is currently benefitting from the Global Trade War.";
                    }
                }
                else
                {
                    if (targetedCompany is null)
                    {
                        desc = "The <strong>" + TargetedIndustry + "</strong> is currently being hit hard by the Global Trade War.";
                    }
                    else
                    {
                        desc = "<strong>" + TargetedCompany + "</strong> is currently being hit hard by the Global Trade War.";
                    }
                }
            }
            else if (EventType == EventType.NewProductLaunch)
            {
                if (IsGood)
                {
                    if (targetedCompany is null)
                    {
                        desc = "The companies from the <strong>" + TargetedIndustry + "</strong> have each announced their next generation of products and the future looks bright.";
                    }
                    else
                    {
                        desc = "<strong>" + TargetedCompany + "</strong> has announced their next generation of products and everyone is hyped for the launch.";
                    }
                }
                else
                {
                    if (targetedCompany is null)
                    {
                        desc = "The companies from the <strong>" + TargetedIndustry + "</strong> industry have each announced their next generation of products but the lineup has everyone wondering what they were thinking.";
                    }
                    else
                    {
                        desc = "<strong>" + TargetedCompany + "</strong> has presented their next generation of products but the reaction from fans was overwhelmingly negative.";
                    }
                }
            }
            else if (EventType == EventType.Hype)
            {
                if (IsGood)
                {
                    if (targetedCompany is null)
                    {
                        desc = "Speculation surrounding a huge upcoming announcement has the stocks for the <strong>" + TargetedIndustry + "</strong> industry trending upwards.";
                    }
                    else
                    {
                        desc = "Fans of <strong>" + TargetedCompany + "</strong> are excited for a huge announcement which has share prices climbing.";
                    }
                }
                else
                {
                    if (targetedCompany is null)
                    {
                        desc = "A huge upcoming announcement is anticipated for the <strong>" + TargetedIndustry + "</strong> industry which has investors nervous.";
                    }
                    else
                    {
                        desc = "Fans are anticipating a huge announcement which may lead to some bad news surrounding the products which <strong>" + TargetedCompany + "</strong> produces.";
                    }
                }
            }
            else if (EventType == EventType.Rumours)
            {
                if (IsGood)
                {
                    if (targetedCompany is null)
                    {
                        desc = "Rumours surrounding the future of the <strong>" + TargetedIndustry + "</strong> industry have share prices climbing.";
                    }
                    else
                    {
                        desc = "Rumours surrounding the future of <strong>" + TargetedCompany + "</strong> have share prices climbing.";
                    }
                }
                else
                {
                    if (targetedCompany is null)
                    {
                        desc = "Rumours surrounding the future of the <strong>" + TargetedIndustry + "</strong> industry have share prices falling.";
                    }
                    else
                    {
                        desc = "Rumours surrounding the future of <strong>" + TargetedCompany + "</strong> have share prices falling.";
                    }
                }
            }
            else if (EventType == EventType.CompanyShuffle)
            {
                if (IsGood)
                {
                    if (targetedCompany is null)
                    {
                        desc = "Reorganization from many companies within the <strong>" + TargetedIndustry + "</strong> industry has share prices climbing.";
                    }
                    else
                    {
                        desc = "As <strong>" + TargetedCompany + "</strong> focuses on it's internal realignment, the future looks bright and so does it's share prices.";
                    }
                }
                else
                {
                    if (targetedCompany is null)
                    {
                        desc = "Reorganization from many companies within the <strong>" + TargetedIndustry + "</strong> industry has share prices falling.";
                    }
                    else
                    {
                        desc = "The industry watches as <strong>" + TargetedCompany + "</strong> attempts to restructure leading to hesitation amongst investors.";
                    }
                }
            }
            else if (EventType == EventType.CurrencyExchangeRate)
            {
                if (IsGood)
                {
                    if (targetedCompany is null)
                    {
                        desc = "The <strong>" + TargetedIndustry + "</strong> industry is benefitting from a strong exchange rate.";
                    }
                    else
                    {
                        desc = "<strong>" + TargetedCompany + "</strong> is benefitting from a strong exchange rate.";
                    }
                }
                else
                {
                    if (targetedCompany is null)
                    {
                        desc = "The <strong>" + TargetedIndustry + "</strong> industry is being hit hard by a weak exchange rate.";
                    }
                    else
                    {
                        desc = "<strong>" + TargetedCompany + "</strong> is being hit hard by a weak exchange rate.";
                    }
                }
            }
            else if (EventType == EventType.InterestRates)
            {
                if (IsGood)
                {
                    if (targetedCompany is null)
                    {
                        desc = "Interest rates have been lowered which is increasing the performance of the <strong>" + TargetedIndustry + "</strong> industry.";
                    }
                    else
                    {
                        desc = "<strong>" + TargetedCompany + "</strong> is experiencing increased performance from lower interest rates.";
                    }
                }
                else
                {
                    if (targetedCompany is null)
                    {
                        desc = "The <strong>" + TargetedIndustry + "</strong> industry is experiencing rising interest rates which is restricting their performance.";
                    }
                    else
                    {
                        desc = "<strong>" + TargetedCompany + "</strong>'s performance is being restricted by rising interest rates.";
                    }
                }
            }
            else if (EventType == EventType.InflationRates)
            {
                if (IsGood)
                {
                    if (targetedCompany is null)
                    {
                        desc = "A recent study has shown that the <strong>" + TargetedIndustry + "</strong> is ahead of the game on adapting to rising inflation.";
                    }
                    else
                    {
                        desc = "A recent study has shown that <strong>" + TargetedCompany + "</strong> is ahead of the game on adapting to rising inflation.";
                    }
                }
                else
                {
                    if (targetedCompany is null)
                    {
                        desc = "A recent study has shown that the <strong>" + TargetedIndustry + "</strong> is falling behind their competition on adapting to rising inflation.";
                    }
                    else
                    {
                        desc = "A recent study has shown that <strong>" + TargetedCompany + "</strong> is falling behind their competition on adapting to rising inflation.";
                    }
                }
            }
            else if (EventType == EventType.WorldEvent)
            {
                if (IsGood)
                {
                    if (targetedCompany is null)
                    {
                        desc = "A global event has led to the rise of stock prices for the <strong>" + TargetedIndustry + "</strong> industry.";
                    }
                    else
                    {
                        desc = "A global event has the stock prices for <strong>" + TargetedCompany + "</strong> rising.";
                    }
                }
                else
                {
                    if (targetedCompany is null)
                    {
                        desc = "A global event has led to the fall of stock prices for the <strong>" + TargetedIndustry + "</strong> industry.";
                    }
                    else
                    {
                        desc = "A global event has the stock prices for <strong>" + TargetedCompany + "</strong> falling.";
                    }
                }
            }
            else if (EventType == EventType.NaturalDisaster)
            {
                if (IsGood)
                {
                    if (targetedCompany is null)
                    {
                        desc = "A natural disaster has led to a rise in stock prices for the <strong>" + TargetedIndustry + "</strong> industry.";
                    }
                    else
                    {
                        desc = "In the aftermath of a natural disaster, the stock prices for <strong>" + TargetedCompany + "</strong> are rising.";
                    }
                }
                else
                {
                    if (targetedCompany is null)
                    {
                        desc = "A natural disaster has led to a fall in stock prices for the <strong>" + TargetedIndustry + "</strong> industry.";
                    }
                    else
                    {
                        desc = "In the aftermath of a natural disaster, the stock prices for <strong>" + TargetedCompany + "</strong> are falling.";
                    }
                }
            }
                    
            return desc;
        }


    }

}