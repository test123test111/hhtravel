using System;
using System.Collections.Generic;
using HHTravel.CRM.Booking_Online.Infrastructure;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class BreadcrumbModel
    {
        public BreadcrumbModel(string text, string url)
        {
            this.Text = text;
            this.Url = url;
        }

        public BreadcrumbModel(SiteColumn siteColumn, string url)
            : this(Enum.GetName(typeof(SiteColumn), siteColumn), url)
        {
        }

        public string Text { get; private set; }

        // todo: 考虑结合Route
        public string Url { get; set; }
    }

    public class TopNavModel
    {
        private bool _visible = true;

        public TopNavModel(bool visible)
        {
            _visible = visible;
        }

        public TopNavModel(ImageInfo topImage)
            : this(topImage, null, new List<BreadcrumbModel>())
        {
        }

        public TopNavModel(ImageInfo topImage, List<BreadcrumbModel> breadcrumbs)
            : this(topImage, null, breadcrumbs)
        {
        }

        public TopNavModel(ImageInfo topImage, DepartureCity? departureCity, List<BreadcrumbModel> breadcrumbs)
        {
            this.TopImage = topImage;
            this.Breadcrumbs = breadcrumbs;
            this.DepartureCity = departureCity;
        }

        public List<BreadcrumbModel> Breadcrumbs { get; private set; }

        public DepartureCity? DepartureCity { get; private set; }

        public bool HideTopImage { get; set; }

        public ImageInfo TopImage { get; set; }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }
    }
}