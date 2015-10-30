using System.ComponentModel;

namespace CafeManagerLib.SharedModels
{
    public enum StatsPeriod
    {
        [Description("Day")]
        Day = 0,
        //ToDo: Week,
        [Description("Month")]
        Month = 1,
        [Description("Year")]
        Year = 2,
    }
}