using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class PagerModel
    {
        private bool _textVisible = true;

        public bool TextVisible
        {
            get { return _textVisible; }
            set { _textVisible = value; }
        }

        private bool _enableMore = true;
        /// <summary>
        /// 启用“更多”
        /// </summary>
        public bool EnableMore
        {
            get { return _enableMore; }
            set { _enableMore = value; }
        }

        private bool _inReverserOrder = true;
        /// <summary>
        /// 以反序构造
        /// </summary>
        public bool InReverserOrder
        {
            get { return _inReverserOrder; }
            set { _inReverserOrder = value; }
        }

        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public string BaseUrl { get; set; }
        public Func<string, int?, string, string> GetUrl { get; set; }
    }
}