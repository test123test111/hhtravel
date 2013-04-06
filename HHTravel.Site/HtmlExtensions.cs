using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;
using HHTravel.ApplicationService.ApplicationServiceImp;
using HHTravel.Infrastructure.Crosscutting;
using HHTravel.Infrastructure;

namespace HHTravel.Site
{
    /// <summary>
    /// 容纳HtmlHelper的扩展方法
    /// </summary>
    public static class HtmlExtensions
    {
        #region 解决Partial中不能用@section的限制

        // http://stackoverflow.com/questions/5433531/using-sections-in-editor-display-templates/5433722#5433722

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

        public static IHtmlString Resource(this HtmlHelper HtmlHelper, Func<object, HelperResult> Template, string key)
        {
            if (HtmlHelper.ViewContext.HttpContext.Items[key] != null) ((List<Func<object, HelperResult>>)HtmlHelper.ViewContext.HttpContext.Items[key]).Add(Template);
            else HtmlHelper.ViewContext.HttpContext.Items[key] = new List<Func<object, HelperResult>>() { Template };

            return new HtmlString(String.Empty);
        }

        #endregion 解决Partial中不能用@section的限制

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

        // http://stackoverflow.com/questions/13124218/asp-net-mvc-4-use-bundles-beneficts-for-url-content?rq=1
        public static IHtmlString DynamicScriptsBundle(this HtmlHelper htmlHelper, string nombre, params string[] urls)
        {
            string path = string.Format("~/{0}", nombre);
            if (BundleTable.Bundles.GetBundleFor(path) == null)
                BundleTable.Bundles.Add(new ScriptBundle(path).Include(urls));
            return Scripts.Render(path);
        }

        public static IHtmlString DynamicStylesBundle(this HtmlHelper htmlHelper, string nombre, params string[] urls)
        {
            string path = string.Format("~/{0}", nombre);
            if (BundleTable.Bundles.GetBundleFor(path) == null)
                BundleTable.Bundles.Add(new StyleBundle(path).Include(urls));
            return Styles.Render(path);
        }

        #region 生成img元素

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
            TagBuilder tagBuilder = new TagBuilder("img");
            tagBuilder.Attributes["alt"] = imageInfo.Alt;
            tagBuilder.Attributes["title"] = imageInfo.Title;
            tagBuilder.Attributes["src"] = imageInfo.Url;
            tagBuilder.MergeAttributes(htmlAttributes, true);
            return tagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        #endregion 生成img元素

        /// <summary>
        /// 生成营销HTML
        /// </summary>
        /// <param name="HtmlHelper"></param>
        /// <param name="htmlLocation"></param>
        /// <returns></returns>
        public static IHtmlString MarketingHtml(this HtmlHelper HtmlHelper, string marketingHtmlLocation)
        {
            var userTestVersion = GlobalConfig.UseTestMarketingHtml;
            var html = new MarketingHtmlServiceImp().GetMarketingHtml(marketingHtmlLocation, userTestVersion);
            return new HtmlString(html);
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

            // todo: 性能优化
            string desp = originalString.Replace("<br/>", " ", StringComparison.OrdinalIgnoreCase);
            desp = desp.Replace("<br>", " ", StringComparison.OrdinalIgnoreCase);
            // !todo: desp = desp.RemoveHtml();

            if (desp.Length <= length)
            {
                newString = desp;
            }
            else
            {
                desp = desp.Substring(0, length);
                StringBuilder sb = new StringBuilder(desp);
                sb.Append("...");
                var link = HtmlHelper.ActionLink("更多", "Index", "ProductDetails",
                    new { productNo = productNo, tab = 0 }, null).ToHtmlString();
                sb.Append(link);
                newString = sb.ToString();
            }

            return new HtmlString(newString);
        }
    }
}