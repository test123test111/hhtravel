using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CtripServices.IntlFlight.DataContract
{
    /// <summary>
    /// 路由信息
    /// </summary>
    /// <history>
    /// 	<date>2010-03-16</date>
    /// 	<programmer>zcchen</programmer>
    ///	    <document>国际机票查询接口改造(43786)</document>
    /// </history>
    [Serializable]
    public class Routing
    {
        private string dCode = String.Empty;
        private string aCode = String.Empty;
        private string dAirport = String.Empty;
        private string aAirport = String.Empty;
        private string airline = String.Empty;
        private string seatClass = String.Empty;
        private string flightNo = String.Empty;
        private string operatorNo = String.Empty;
        private int segmentInfoNo = 1;
        private int no = 1;

        /// <summary>
        /// 出发城市
        /// </summary>
        public string DCode
        {
            get { return dCode; }
            set { dCode = value; }
        }

        /// <summary>
        /// 到达城市
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
        /// 舱空公司二字码
        /// </summary>
        public string Airline
        {
            get { return airline; }
            set { airline = value; }
        }

        /// <summary>
        /// 舱位代码
        /// </summary>
        public string SeatClass
        {
            get { return seatClass; }
            set { seatClass = value; }
        }

        /// <summary>
        /// 航班号
        /// </summary>
        public string FlightNo
        {
            get { return flightNo; }
            set { flightNo = value; }
        }

        /// <summary>
        /// 承运航班号
        /// </summary>
        public string OperatorNo
        {
            get { return operatorNo; }
            set { operatorNo = value; }
        }

        /// <summary>
        /// 航段数
        /// </summary>
        public int SegmentInfoNo
        {
            get { return segmentInfoNo; }
            set { segmentInfoNo = value; }
        }

        /// <summary>
        /// 航班序号
        /// </summary>
        public int No
        {
            get { return no; }
            set { no = value; }
        }
    }
}