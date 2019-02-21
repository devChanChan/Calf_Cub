using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalAssignLibrary.Enumerations
{
    /// <summary>
    /// Enum used to keep track of the Industries which a Company may belong to.
    /// None is used in place of null as an Enum cannot be set to null (simplifies
    /// processing in the DAL).
    /// </summary>
    public enum Industry
    {
        None,
        Communications,
        Retail,
        Fashion,
        Finance,
        Food,
        Grocery,
        Technology,
        Transportation,
        Vehicle
    }
}