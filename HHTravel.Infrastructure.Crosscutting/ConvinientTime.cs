using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting
{
    /// <summary>
    /// 方便联系（客户）的时段
    /// </summary>
    [DataContract]
    public enum ConvinientTime
    {
        [DataMember]
        All = -1,

        [DataMember]
        NineToTwelve = 0,

        [DataMember]
        TwelveToFourteen = 1,

        [DataMember]
        FourteenToEighteen = 2,
    }

    public static class ConvinientTimeExtensions
    {
        public static readonly Dictionary<ConvinientTime, string> s_dict = new Dictionary<ConvinientTime, string> {
            {ConvinientTime.NineToTwelve, "09:00-12:00"},
            {ConvinientTime.TwelveToFourteen, "12:00-14:00"},
            {ConvinientTime.FourteenToEighteen, "14:00-18:00"},
            {ConvinientTime.All, "以上时间皆可"},
        };

        public static string GetText(this ConvinientTime ConvinientTime)
        {
            string text = s_dict[ConvinientTime];
            return text;
        }
    }
}