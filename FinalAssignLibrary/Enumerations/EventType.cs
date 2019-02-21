using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalAssignLibrary.Enumerations
{
    /// <summary>
    /// Enum used to list the possible types (causes) of Random Events
    /// These values start at 1 to match the order of the database records to
    /// simplify processing.
    /// </summary>
    public enum EventType
    {
        CeoQuit = 1,
        StockPriceChange,
        CeoFired,
        CompanyBankruptcy,
        NewBreakthrough,
        EnergyCrisis,
        SupplyCrisis,
        GlobalTradeWar,
        NewProductLaunch,
        Hype,
        Rumours,
        CompanyShuffle,
        CurrencyExchangeRate,
        InterestRates,
        InflationRates,
        WorldEvent,
        NaturalDisaster
    }
}