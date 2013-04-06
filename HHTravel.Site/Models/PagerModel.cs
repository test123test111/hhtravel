using System;

namespace HHTravel.Site.Models
{
    public class PagerModel
    {
        private bool _enableMore = true;
        private bool _inReverserOrder = true;
        private bool _textVisible = true;

        public string BaseUrl { get; set; }

        /// <summary>
        /// 启用“更多”
        /// </summary>
        public bool EnableMore
        {
            get { return _enableMore; }
            set { _enableMore = value; }
        }

        public Func<string, string, string, string> GetUrl { get; set; }

        /// <summary>
        /// 以反序构造
        /// </summary>
        public bool InReverserOrder
        {
            get { return _inReverserOrder; }
            set { _inReverserOrder = value; }
        }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }

        public bool TextVisible
        {
            get { return _textVisible; }
            set { _textVisible = value; }
        }
    }
}