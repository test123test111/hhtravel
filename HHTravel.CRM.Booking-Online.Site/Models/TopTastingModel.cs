using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.CRM.Booking_Online.Model;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class TopNavModel
    {
        private bool _visible = true;
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public ImageInfo TopImage { get; private set; }
        public List<BreadcrumbModel> Breadcrumbs { get; private set; }

        public TopNavModel(bool visible)
        {
            _visible = visible;
        }

        public TopNavModel(ImageInfo topImage, List<BreadcrumbModel> breadcrumbs)
        {
            this.TopImage = topImage;
            this.Breadcrumbs = breadcrumbs;
        }
    }

    public class BreadcrumbModel
    {
        public string Text { get; private set; }
        // todo: 考虑结合Route
        public string Url { get; private set; }

        public BreadcrumbModel(string text, string url)
        {
            this.Text = text;
            this.Url = url;
        }
    }
}