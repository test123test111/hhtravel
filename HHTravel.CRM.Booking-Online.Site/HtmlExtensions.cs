using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using HHTravel.CRM.Booking_Online.Model;
using System.Text;
using System.Web.Routing;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HHTravel.CRM.Booking_Online.Site
{
    /// <summary>
    /// 容纳HtmlHelper的扩展方法
    /// </summary>
    public static class HtmlExtensions
    {
        #region 解决Partial中不能用@section的限制
        // http://stackoverflow.com/questions/5433531/using-sections-in-editor-display-templates/5433722#5433722

        public static IHtmlString Resource(this HtmlHelper HtmlHelper, Func<object, HelperResult> Template, string key)
        {
            if (HtmlHelper.ViewContext.HttpContext.Items[key] != null) ((List<Func<object, HelperResult>>)HtmlHelper.ViewContext.HttpContext.Items[key]).Add(Template);
            else HtmlHelper.ViewContext.HttpContext.Items[key] = new List<Func<object, HelperResult>>() { Template };

            return new HtmlString(String.Empty);
        }

        public static IHtmlString RenderResources(this HtmlHelper HtmlHelper, string key)
        {
            if (HtmlHelper.ViewContext.HttpContext.Items[key] != null)
            {
                List<Func<object, HelperResult>> Resources = (List<Func<object, HelperResult>>)HtmlHelper.ViewContext.HttpContext.Items[key];

                foreach (var Resource in Resources)
                {
                    if (Resource != null) HtmlHelper.ViewContext.Writer.Write(Resource(null));
                }
            }

            return new HtmlString(String.Empty);
        }
        #endregion

        /// <summary>
        /// 生成img标签
        /// </summary>
        /// <param name="HtmlHelper"></param>
        /// <param name="imageInfo"></param>
        /// <returns></returns>
        public static IHtmlString Image(this HtmlHelper HtmlHelper, ImageInfo imageInfo)
        {
            return Image(HtmlHelper, imageInfo, null);
        }
        /// <summary>
        /// 生成img标签
        /// </summary>
        /// <param name="HtmlHelper"></param>
        /// <param name="imageInfo"></param>
        /// <param name="defaultImageUrl"></param>
        /// <returns></returns>
        public static IHtmlString Image(this HtmlHelper HtmlHelper, ImageInfo imageInfo, string defaultImageUrl)
        {
            return Image(HtmlHelper, imageInfo, defaultImageUrl, null);
        }
        /// <summary>
        /// 生成img标签
        /// </summary>
        /// <param name="HtmlHelper"></param>
        /// <param name="imageInfo"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlString Image(this HtmlHelper HtmlHelper, ImageInfo imageInfo, object htmlAttributes)
        {
            return Image(HtmlHelper, imageInfo, null, htmlAttributes);
        }
        /// <summary>
        /// 生成img标签
        /// </summary>
        /// <param name="HtmlHelper"></param>
        /// <param name="imageInfo"></param>
        /// <param name="defaultImageUrl"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlString Image(this HtmlHelper HtmlHelper, ImageInfo imageInfo, string defaultImageUrl, object htmlAttributes)
        {
            if (imageInfo == null || string.IsNullOrEmpty(imageInfo.Url))
            {
                if (!string.IsNullOrEmpty(defaultImageUrl))
                {
                    imageInfo = new ImageInfo { Url = defaultImageUrl };
                }
                else
                {
                    return null;
                }
            }

            imageInfo.Alt = !string.IsNullOrEmpty(imageInfo.Title) ? imageInfo.Title : "";
            string imgHtml = BuildImgHtml(imageInfo, new RouteValueDictionary(htmlAttributes));
            return new HtmlString(imgHtml);
        }

        /// <summary>
        /// 生成对应img标签的字符串
        /// </summary>
        /// <param name="imageInfo"></param>
        /// <returns></returns>
        private static string BuildImgHtml(ImageInfo imageInfo, IDictionary<string, object> htmlAttributes)
        {
            // todo: 解决相对路径的问题
            TagBuilder tagBuilder = new TagBuilder("img");
            tagBuilder.Attributes["alt"] = imageInfo.Alt;
            tagBuilder.Attributes["title"] = imageInfo.Title;
            tagBuilder.Attributes["src"] = imageInfo.Url;
            tagBuilder.MergeAttributes(htmlAttributes, true);
            return tagBuilder.ToString(TagRenderMode.SelfClosing);
        }
        /// <summary>
        /// 产品行程说明截断
        /// </summary>
        /// <param name="HtmlHelper"></param>
        /// <param name="originalString"></param>
        /// <param name="length"></param>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public static IHtmlString TruncateProductDescription(this HtmlHelper HtmlHelper, string originalString, int length, string productNo)
        {
            string newString;
            if (length >= originalString.Length)
            {
                newString = originalString;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(originalString.Substring(0, length));
                sb.Append("...");
                var link = HtmlHelper.ActionLink("更多", "Details", "Product",
                    new { productNo = productNo, tab = 0 }, null).ToHtmlString();
                sb.Append(link);
                newString = sb.ToString();
            }

            return new HtmlString(newString);
        }
        /// <summary>
        /// 将DateTime转成如下格式
        /// 2012年10月10号（三）
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static IHtmlString DepartureDate(this HtmlHelper HtmlHelper, DateTime date)
        {
            string dateString;
            string[] chineseDayOfWeekCollect = new string[]{
                "日","一","二", "三","四","五", "六",
            };
            dateString = string.Format("{0}（{1}）",
                date.ToString("yyyy年MM月dd号"), chineseDayOfWeekCollect[(int)date.DayOfWeek]);
            return new HtmlString(dateString);
        }
    }


    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DateAttribute : ValidationAttribute
    {
        public DateAttribute() :
            base("\"{0}\" must be a date without time portion or is empty.")
        {
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                if (value.GetType() == typeof(DateTime))
                {
                    DateTime dateTime = (DateTime)value;
                    return dateTime.TimeOfDay == TimeSpan.Zero;
                }
                else if (value.GetType() == typeof(Nullable<DateTime>))
                {
                    DateTime? dateTime = (DateTime?)value;
                    return !dateTime.HasValue
                        || dateTime.Value.TimeOfDay == TimeSpan.Zero;
                }
            }
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture,
                ErrorMessageString, name);
        }
    }
}