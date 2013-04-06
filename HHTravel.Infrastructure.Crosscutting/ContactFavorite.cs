using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HHTravel.Infrastructure.Crosscutting
{
    /// <summary>
    /// （客户）倾向的联系方式
    /// </summary>
    [DataContract]
    public enum ContactFavorite
    {
        [DataMember]
        Phone = 0,

        [DataMember]
        Email = 1,
    }

    public static class ContactFavoriteExtensions
    {
        public static readonly Dictionary<ContactFavorite, string> s_dict = new Dictionary<ContactFavorite, string> {
            {ContactFavorite.Phone, "电话或手机"},
            {ContactFavorite.Email, "E-mail"},
        };

        public static string GetText(this ContactFavorite ContactFavorite)
        {
            string text = s_dict[ContactFavorite];
            return text;
        }
    }
}