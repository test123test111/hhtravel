using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting
{
    [DataContract]
    public enum DaysSection
    {
        [Description("1～5天")]
        [Range(1, 5)]
        [DataMember]
        First = 1,

        [Description("6～10天")]
        [Range(6, 10)]
        [DataMember]
        Second = 2,

        [Description("11～15天")]
        [Range(11, 15)]
        [DataMember]
        Third = 3,

        [Description("16天以上")]
        [Range(16, int.MaxValue)]
        [DataMember]
        Fourth = 4,
    }

    public static class DaysSectionExtensions
    {
        public static void ParseRange(this DaysSection? value, out int? min, out int? max)
        {
            if (value == null)
            {
                min = null;
                max = null;
                return;
            }

            var attributes = (RangeAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(RangeAttribute), false);
            if (!attributes.Any())
            {
                throw new ArgumentException("RangeAttribute is required", "value");
            }

            min = (int)attributes[0].Minimum;
            max = (int)attributes[0].Maximum;
            max = (max != int.MaxValue) ? max : null;
        }
    }
}