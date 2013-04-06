using System;
using System.Text;

namespace HHTravel.CRM.Booking_Online.Infrastructure
{
    public static class StringExtensions
    {
        // http://stackoverflow.com/questions/244531/is-there-an-alternative-to-string-replace-that-is-case-insensitive
        public static string Replace(this string str, string oldValue, string newValue, StringComparison comparison)
        {
            if (string.IsNullOrEmpty(str)) return "";

            StringBuilder sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }

        // http://www.codeproject.com/Articles/14936/StringBuilder-vs-String-Fast-String-Operations-wit
        // http://stackoverflow.com/questions/1620410/regex-replace-string-replace-or-stringbuilder-replace-which-is-the-fastest
        // http://stackoverflow.com/questions/287842/is-stringbuilder-replace-more-efficient-than-string-replace
        // http://stackoverflow.com/questions/244531/is-there-an-alternative-to-string-replace-that-is-case-insensitive
    }
}