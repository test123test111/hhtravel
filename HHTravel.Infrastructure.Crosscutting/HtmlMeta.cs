using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.Infrastructure.Crosscutting
{
    public class HtmlMeta
    {
        public string Description { get; set; }

        public string Keywords { get; set; }

        public string Title { get; set; }

        #region todo: internal

        public int ObjectId { get; set; }

        public string ObjectType { get; set; }

        #endregion todo: internal
    }
}