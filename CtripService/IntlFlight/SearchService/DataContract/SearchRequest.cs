using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using CtripServices.IntlFlight.DataContract;

namespace CtripServices.IntlFlight.SearchService.DataContract
{
    /// <summary>
    /// 国际机票航班查询请求实体
    /// </summary>
    /// <history>
    /// 	<date>2010-03-16</date>
    /// 	<programmer>zcchen</programmer>
    ///	    <document>国际机票查询接口改造(43786)</document>
    /// </history>
    [Serializable]
    [XmlRoot("FltIntlSearchFlightsRequest")]
    public class SearchRequest
    {
        private TripType tripType = TripType.OW;
        private PassengerType passengerType = PassengerType.ADT;
        private int passengerCount = 0;
        private Eligibility eligibility = Eligibility.ALL;
        private string businessID = String.Empty;
        private BusinessType businessType = BusinessType.OWN;
        private string airline = String.Empty;
        private SeatGrade classGrade = SeatGrade.Y;
        private SalesType salesType = SalesType.Online;
        private string fareIds = String.Empty;
        private FareType fareType = FareType.All;
        private string agentID = String.Empty;
        private ResultMode resultMode = ResultMode.All;
        private OrderBy orderBy = OrderBy.Price;
        private Direction direction = Direction.Asc;
        private string shoppingInfoID = string.Empty;
        private List<SegmentInfo> segmentInfos = new List<SegmentInfo>();
        private List<Routing> routings = new List<Routing>();
        private bool isCompress = false;
        private bool isDirect = false;

        /// <summary>
        /// 行程类型
        ///     默认:单程
        /// </summary>
        public TripType TripType
        {
            get { return tripType; }
            set { tripType = value; }
        }

        /// <summary>
        /// 乘客类型
        ///     默认:全部
        /// </summary>
        public PassengerType PassengerType
        {
            get { return passengerType; }
            set { passengerType = value; }
        }

        /// <summary>
        /// 乘客数量
        ///     默认:全部
        /// </summary>
        public int PassengerCount
        {
            get { return passengerCount; }
            set { passengerCount = value; }
        }

        /// <summary>
        /// 乘客资质
        ///     默认:全部
        /// </summary>
        public Eligibility Eligibility
        {
            get { return eligibility; }
            set { eligibility = value; }
        }

        /// <summary>
        /// 商旅客户编号
        /// </summary>
        public string BusinessID
        {
            get { return businessID; }
            set { businessID = value; }
        }

        /// <summary>
        /// 商旅用户支出类型
        ///     默认:自费
        /// </summary>
        public BusinessType BusinessType
        {
            get { return businessType; }
            set { businessType = value; }
        }

        /// <summary>
        /// 舱空公司二字码
        ///     默认:所有航空公司
        /// </summary>
        public string Airline
        {
            get { return airline; }
            set { airline = value; }
        }

        /// <summary>
        /// 舱位等级
        ///     默认:Y(查询全部)
        /// </summary>
        public SeatGrade SeatGrade
        {
            get { return classGrade; }
            set { classGrade = value; }
        }

        /// <summary>
        /// 销售渠道
        ///     默认:网上销售
        /// </summary>
        public SalesType SalesType
        {
            get { return salesType; }
            set { salesType = value; }
        }

        /// <summary>
        /// 指定运价ID
        /// </summary>
        public string FareIds
        {
            get { return fareIds; }
            set { fareIds = value; }
        }

        /// <summary>
        /// 运价类型
        ///     默认:所有
        /// </summary>
        public FareType FareType
        {
            get { return fareType; }
            set { fareType = value; }
        }

        /// <summary>
        /// 票台编号
        /// </summary>
        public string AgentID
        {
            get { return agentID; }
            set { agentID = value; }
        }

        /// <summary>
        /// 结果模式
        ///     默认:所有价格
        /// </summary>
        public ResultMode ResultMode
        {
            get { return resultMode; }
            set { resultMode = value; }
        }

        /// <summary>
        /// 排序字段
        ///     默认:价格
        /// </summary>
        public OrderBy OrderBy
        {
            get { return orderBy; }
            set { orderBy = value; }
        }

        /// <summary>
        /// 排序方向
        ///     默认:从低到高
        /// </summary>
        public Direction Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        /// <summary>
        /// 已选航班信息
        ///     查询下一程航班信息时必须带入的参数
        /// </summary>
        public string ShoppingInfoID
        {
            get { return shoppingInfoID; }
            set { shoppingInfoID = value; }
        }

        /// <summary>
        /// 航段信息
        /// </summary>
        public List<SegmentInfo> SegmentInfos
        {
            get { return segmentInfos; }
            set { segmentInfos = value; }
        }

        /// <summary>
        /// 选定航路信息
        /// </summary>
        public List<Routing> Routings
        {
            get { return routings; }
            set { routings = value; }
        }

        /// <summary>
        /// 是否压缩返回报文
        ///  默认:不压缩
        /// </summary>
        public bool IsCompress
        {
            get { return isCompress; }
            set { isCompress = value; }
        }

        /// <summary>
        /// 是否查询直飞数据
        ///  默认:false(返回所有数据), true(只返回直飞的数据), 默认为false(直飞有可能有经停)
        /// </summary>
        public bool IsDirect
        {
            get { return isDirect; }
            set { isDirect = value; }
        }
    }
}