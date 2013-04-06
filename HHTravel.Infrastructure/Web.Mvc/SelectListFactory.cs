using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace HHTravel.Infrastructure.Web.Mvc
{
    public class SelectListFactory
    {
        public static IEnumerable<SelectListItem> Create<TEnum, T>(bool addAll)
            where TEnum : struct
            where T : struct
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("Enum is required.", "TEnum");
            if (typeof(T) != Enum.GetUnderlyingType(typeof(TEnum))) throw new ArgumentException("Enum's underlying type is required", "T");

            var collection = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            return Create<TEnum, T>(collection, addAll);
        }

        public static IEnumerable<SelectListItem> Create<TEnum, T>(IEnumerable<TEnum> collection, bool addAll)
            where TEnum : struct
            where T : struct
        {
            // http://stackoverflow.com/questions/388483/how-do-you-create-a-dropdownlist-from-an-enum-in-asp-net-mvc
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("Enum is required.", "TEnum");
            if (typeof(T) != Enum.GetUnderlyingType(typeof(TEnum))) throw new ArgumentException("Enum's underlying type is required", "T");

            var values = from TEnum e in collection
                         select new
                         {
                             Id = (T)Convert.ChangeType(e, typeof(T), CultureInfo.InvariantCulture),
                             Name = GetDescription(e)
                         };

            return Create<object>(values, "Id", "Name", addAll);
        }

        public static IEnumerable<SelectListItem> Create<T>(IEnumerable<T> collection, string dataValueField, string dataTextField)
        {
            return Create<T>(collection, dataValueField, dataTextField, false);
        }

        public static IEnumerable<SelectListItem> Create<T>(IEnumerable<T> collection, string dataValueField, string dataTextField, bool addAll)
        {
            List<SelectListItem> sliList = new SelectList(collection, dataValueField, dataTextField).ToList();
            if (addAll)
            {
                sliList.Insert(0, new SelectListItem { Text = "全部", Value = "" });
            }
            return sliList;
        }

        public static SelectList CreateSelectList(string textFormat, int selectedValue = 0, int beginValue = 0, int endValue = 8)
        {
            SelectList sl;
            var q = from i in new IntRange(beginValue, endValue)
                    select new
                    {
                        Id = i,
                        Name = string.Format(textFormat, i)
                    };
            var list = SelectListFactory.Create(q, "Id", "Name");
            sl = new SelectList(list, "Value", "Text", selectedValue);
            return sl;
        }

        public static SelectList CreateTicketCountSelectList(int selectedValue = 0)
        {
            SelectList sl;
            int[] arr = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 9999 };
            var q = from i in arr
                    select new
                    {
                        Id = i,

                        // todo: 注意：文字变化时，同步修改\Views\Order\Index3Success.cshtml中ticket.Count的显示
                        Name = (i != 9999) ? string.Format("{0}人", i) : "9人以上",
                    };
            var list = SelectListFactory.Create(q, "Id", "Name");
            sl = new SelectList(list, "Value", "Text", selectedValue);
            return sl;
        }

        public static string GetDescription<TEnum>(TEnum value)
            where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("Enum is required.", "TEnum");

            var attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}