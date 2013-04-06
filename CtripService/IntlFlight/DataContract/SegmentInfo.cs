using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CtripServices.IntlFlight.DataContract
{
    /// <summary>
    /// 航段信息
    /// </summary>
    /// <history>
    /// 	<date>2010-03-16</date>
    /// 	<programmer>zcchen</programmer>
    ///	    <document>国际机票查询接口改造(43786)</document>
    /// </history>
    [Serializable]
    public class SegmentInfo
    {
        private const string DEFAULT_TIMEPERIOD = "All";

        private string dCode = String.Empty;
        private string aCode = String.Empty;
        private string dAirport = String.Empty;
        private string aAirport = String.Empty;
        //private DateTime dDate = default(DateTime);
        private string timePeriod = String.Empty;

        /// <summary>
        /// 出发城市码
        /// </summary>
        public string DCode
        {
            get { return dCode; }
            set { dCode = value; }
        }

        /// <summary>
        /// 到达城市码
        /// </summary>
        public string ACode
        {
            get { return aCode; }
            set { aCode = value; }
        }

        /// <summary>
        /// 出发机场
        /// </summary>
        public string DAirport
        {
            get { return dAirport; }
            set { dAirport = value; }
        }

        /// <summary>
        /// 到达机场
        /// </summary>
        public string AAirport
        {
            get { return aAirport; }
            set { aAirport = value; }
        }

        /// <summary>
        /// 出发日期
        /// </summary>
        public string DDate { get; set; }

        /// <summary>
        /// 起飞时间段
        ///  暂不用,默认0000-2400
        /// </summary>
        public string TimePeriod
        {
            get
            {
                if (timePeriod == null || timePeriod.Trim().Length == 0)
                    return DEFAULT_TIMEPERIOD;
                return timePeriod;
            }
            set { timePeriod = value; }
        }
    }
}