using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using CtripServices.IntlFlight.DataContract;

namespace CtripServices.IntlFlight.SearchService.DataContract
{
    /// <summary>
    /// 国际机票航班查询响应实体
    /// </summary>
    /// <history>
    /// 	<date>2010-03-16</date>
    /// 	<programmer>zcchen</programmer>
    ///	    <document>国际机票查询接口改造(43786)</document>
    /// </history>
    [Serializable]
    [XmlRoot("FltIntlSearchFlightsResponse")]
    public class SearchResponse
    {
        private int recorderCount = 0;
        private List<ShoppingResultInfo> shoppingResults = new List<ShoppingResultInfo>();

        /// <summary>
        /// 所有结果集总数量
        /// </summary>
        public int RecorderCount
        {
            get { return recorderCount; }
            set { recorderCount = value; }
        }

        /// <summary>
        /// 有效航路集合
        /// </summary>
        public List<ShoppingResultInfo> ShoppingResults
        {
            get { return shoppingResults; }
            set { shoppingResults = value; }
        }
    }

    /// <summary>
    /// 航路信息
    /// </summary>
    /// <history>
    /// 	<date>2010-03-16</date>
    /// 	<programmer>zcchen</programmer>
    ///	    <document>国际机票查询接口改造(43786)</document>
    /// </history>
    [Serializable]
    public class ShoppingResultInfo
    {
        private List<FlightsInfo> flightInfos = new List<FlightsInfo>();
        private List<PolicyInfo> policyInfos = new List<PolicyInfo>();

        /// <summary>
        /// 航路中所包含航班集合
        /// </summary>
        public List<FlightsInfo> FlightInfos
        {
            get { return flightInfos; }
            set { flightInfos = value; }
        }

        /// <summary>
        /// 航路中包含运价政策集合
        /// </summary>
        public List<PolicyInfo> PolicyInfos
        {
            get { return policyInfos; }
            set { policyInfos = value; }
        }
    }

    /// <summary>
    /// 航班信息
    /// </summary>
    /// <history>
    /// 	<date>2010-03-16</date>
    /// 	<programmer>zcchen</programmer>
    ///	    <document>国际机票查询接口改造(43786)</document>
    /// </history>
    [Serializable]
    public class FlightsInfo
    {
        private int segmentInfoNo = 1;
        private List<Flight> flights = new List<Flight>();

        /// <summary>
        /// 航段数
        /// </summary>
        public int SegmentInfoNo
        {
            get { return segmentInfoNo; }
            set { segmentInfoNo = value; }
        }

        /// <summary>
        /// 航班集合
        /// </summary>
        public List<Flight> Flights
        {
            get { return flights; }
            set { flights = value; }
        }
    }

    /// <summary>
    /// 航班信息
    /// </summary>
    /// <history>
    /// 	<date>2010-03-16</date>
    /// 	<programmer>zcchen</programmer>
    ///	    <document>国际机票查询接口改造(43786)</document>
    /// </history>
    [Serializable]
    public class Flight
    {
        private int no = 1;
        private string airline = String.Empty;
        private string airlineCode = String.Empty;
        private string airLineName = String.Empty;
        private string flightNo = String.Empty;
        private string dCity = String.Empty;
        private int dCityID = 0;
        private string dCityName = String.Empty;
        private string dCityNameEng = String.Empty;
        private string aCity = String.Empty;
        private string aCityName = String.Empty;
        private string aCityNameEng = String.Empty;
        private int aCityID = 0;
        private string dPort = String.Empty;
        private string dPortName = String.Empty;
        private string dPortNameEng = String.Empty;
        private string aPort = String.Empty;
        private string aPortName = String.Empty;
        private string aPortNameEng = String.Empty;
        private DateTime? effectDate;
        private string dTime = String.Empty;
        private string aTime = String.Empty;
        private string dTimeString = String.Empty;
        private string aTimeString = String.Empty;
        private string craftType = String.Empty;
        private string mealType = String.Empty;
        private bool isShared = false;
        private string carrier = String.Empty;
        private string carrierFlightNo = String.Empty;

        private DateTime? departDate;
        private DateTime? arrivalDate;

        private bool isADDON = false;
        private int nextday = 0;
        private string groupID = String.Empty;
        private int journeyTime = 0;

        private List<Stop> stops = new List<Stop>();

        /// <summary>
        /// 航班序号
        /// </summary>
        public int No
        {
            get { return no; }
            set { no = value; }
        }

        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline
        {
            get { return airline; }
            set { airline = value; }
        }

        /// <summary>
        /// 航空公司编号
        /// </summary>
        public string AirlineCode
        {
            get { return airlineCode; }
            set { airlineCode = value; }
        }

        /// <summary>
        /// 航空公司名称
        /// </summary>
        public string AirLineName
        {
            get { return airLineName; }
            set { airLineName = value; }
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
        /// 出发城市代码
        /// </summary>
        public string DCity
        {
            get { return dCity; }
            set { dCity = value; }
        }

        /// <summary>
        /// 出发城市ID
        /// </summary>
        public int DCityID
        {
            get { return dCityID; }
            set { dCityID = value; }
        }

        /// <summary>
        /// 出发城市中文名称
        /// </summary>
        public string DCityName
        {
            get { return dCityName; }
            set { dCityName = value; }
        }

        /// <summary>
        /// 出发城市英文名称
        /// </summary>
        public string DCityNameEng
        {
            get { return dCityNameEng; }
            set { dCityNameEng = value; }
        }

        /// <summary>
        /// 到达城市代码
        /// </summary>
        public string ACity
        {
            get { return aCity; }
            set { aCity = value; }
        }

        /// <summary>
        /// 到达城市中文代码
        /// </summary>
        public string ACityName
        {
            get { return aCityName; }
            set { aCityName = value; }
        }

        /// <summary>
        /// 到达城市英文名称
        /// </summary>
        public string ACityNameEng
        {
            get { return aCityNameEng; }
            set { aCityNameEng = value; }
        }

        /// <summary>
        /// 到达城市ID
        /// </summary>
        public int ACityID
        {
            get { return aCityID; }
            set { aCityID = value; }
        }

        /// <summary>
        /// 出发机场三字码
        /// </summary>
        public string DPort
        {
            get { return dPort; }
            set { dPort = value; }
        }

        /// <summary>
        /// 出发机场中文名称
        /// </summary>
        public string DPortName
        {
            get { return dPortName; }
            set { dPortName = value; }
        }

        /// <summary>
        /// 出发机场英文名称
        /// </summary>
        public string DPortNameEng
        {
            get { return dPortNameEng; }
            set { dPortNameEng = value; }
        }

        /// <summary>
        /// 到达机场三字码
        /// </summary>
        public string APort
        {
            get { return aPort; }
            set { aPort = value; }
        }

        /// <summary>
        /// 到达机场中文名称
        /// </summary>
        public string APortName
        {
            get { return aPortName; }
            set { aPortName = value; }
        }

        /// <summary>
        /// 到达机场英文名称
        /// </summary>
        public string APortNameEng
        {
            get { return aPortNameEng; }
            set { aPortNameEng = value; }
        }

        /// <summary>
        /// 出发日期
        /// </summary>
        public DateTime? EffectDate
        {
            get { return effectDate; }
            set { effectDate = value; }
        }

        /// <summary>
        /// 航班出发时间
        /// </summary>
        public string DTime
        {
            get { return dTime; }
            set { dTime = value; }
        }

        /// <summary>
        /// 航班到达时间
        /// </summary>
        public string ATime
        {
            get { return aTime; }
            set { aTime = value; }
        }

        /// <summary>
        /// 航班出发时间
        /// </summary>
        public string DTimeString
        {
            get { return dTimeString; }
            set { dTimeString = value; }
        }

        /// <summary>
        /// 航班到达时间
        /// </summary>
        public string ATimeString
        {
            get { return aTimeString; }
            set { aTimeString = value; }
        }

        /// <summary>
        /// 出发日期时间
        /// </summary>
        public DateTime? DepartDate
        {
            get { return departDate; }
            set { departDate = value; }
        }

        /// <summary>
        /// 到达日期时间
        /// </summary>
        public DateTime? ArrivalDate
        {
            get { return arrivalDate; }
            set { arrivalDate = value; }
        }

        /// <summary>
        /// 机型
        /// </summary>
        public string CraftType
        {
            get { return craftType; }
            set { craftType = value; }
        }

        /// <summary>
        /// 餐食种类
        /// </summary>
        public string MealType
        {
            get { return mealType; }
            set { mealType = value; }
        }

        /// <summary>
        /// 是否共享航班
        /// </summary>
        public bool IsShared
        {
            get { return isShared; }
            set { isShared = value; }
        }

        /// <summary>
        /// 实际承运人
        /// </summary>
        public string Carrier
        {
            get { return carrier; }
            set { carrier = value; }
        }

        /// <summary>
        /// 实际承运航班号
        /// </summary>
        public string CarrierFlightNo
        {
            get { return carrierFlightNo; }
            set { carrierFlightNo = value; }
        }

        /// <summary>
        /// 是否ADDON
        /// </summary>
        public bool IsADDON
        {
            get { return isADDON; }
            set { isADDON = value; }
        }

        /// <summary>
        /// 航班是否跨天
        /// </summary>
        public int Nextday
        {
            get { return nextday; }
            set { nextday = value; }
        }

        /// <summary>
        /// 组Id,供联程用
        ///     非空时为联程但同一GroupId才可联程
        /// </summary>
        public string GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }

        /// <summary>
        /// 经停列表
        /// </summary>
        public List<Stop> Stops
        {
            get { return stops; }
            set { stops = value; }
        }

        /// <summary>
        /// 飞行时长
        /// </summary>
        public int JourneyTime
        {
            get { return journeyTime; }
            set { journeyTime = value; }
        }
    }

    /// <summary>
    /// 经停信息
    /// </summary>
    /// <history>
    /// 	<date>2010-03-16</date>
    /// 	<programmer>zcchen</programmer>
    ///	    <document>国际机票查询接口改造(43786)</document>
    /// </history>
    [Serializable]
    public class Stop
    {
        private string airport = String.Empty;

        /// <summary>
        /// 经停机场
        /// </summary>
        public string Airport
        {
            get { return airport; }
            set { airport = value; }
        }

        /// <summary>
        /// 经停时间
        /// </summary>
        public int Duration { get; set; }
    }

    /// <summary>
    /// 运价政策信息
    /// </summary>
    /// <history>
    /// 	<date>2010-03-16</date>
    /// 	<programmer>zcchen</programmer>
    ///	    <document>国际机票查询接口改造(43786)</document>
    /// </history>
    [Serializable]
    public class PolicyInfo
    {
        private string shoppingInfoID = String.Empty;
        private string computeExpression = String.Empty;
        private Eligibility eligibility = Eligibility.ALL;
        private int printTicketDay = 0;
        private int beforeFlyDate = 0;
        private int adjustday1 = 0;
        private int adjustday2 = 0;
        private bool sdss = false;
        private DateTime zwcp = default(DateTime);
        private bool isOpen = false;
        private int leastPerson = 0;
        private bool isSpecial = false;
        private string ticketRemark = String.Empty;
        private int recommend = 0;
        private DateTime startDate = default(DateTime);
        private DateTime expiryDate = default(DateTime);
        private TicketType ticketType = TicketType.BSPET;
        private string allowCPType = "0001";
        private DateTime lastPrintDate = default(DateTime);
        private DateTime fdzwcp = default(DateTime);
        private int aDDONPID = 0;
        private string fareID = String.Empty;
        private string noSalesStr = String.Empty;
        private bool isIssueNeeded = false;
        private string ownerAirline = String.Empty;
        private int giftNum = 0;
        private string giftDesc = String.Empty;
        private string giftType = String.Empty;
        private int ticketSetTravelCount;
        private string remarkFareId = string.Empty;

        private List<FlightBaseInfo> flightBaseInfos = new List<FlightBaseInfo>();
        private List<PriceInfo> priceInfos = new List<PriceInfo>();

        /// <summary>
        /// 政策航班组合ID
        ///     下一程查询时需带入的参数
        ///     与FltIntlSearchFlightsRequest.shoppingInfoID对应
        /// </summary>
        public string ShoppingInfoID
        {
            get { return shoppingInfoID; }
            set { shoppingInfoID = value; }
        }

        /// <summary>
        /// 运价横式
        /// </summary>
        public string ComputeExpression
        {
            get { return computeExpression; }
            set { computeExpression = value; }
        }

        /// <summary>
        /// 乘客资质
        /// </summary>
        public Eligibility Eligibility
        {
            get { return eligibility; }
            set { eligibility = value; }
        }

        /// <summary>
        /// 预订后出票时间
        /// </summary>
        public int PrintTicketDay
        {
            get { return printTicketDay; }
            set { printTicketDay = value; }
        }

        /// <summary>
        /// 起飞前出票时间
        /// </summary>
        public int BeforeFlyDate
        {
            get { return beforeFlyDate; }
            set { beforeFlyDate = value; }
        }

        /// <summary>
        /// 最短停留停期时间
        /// </summary>
        public int Adjustday1
        {
            get { return adjustday1; }
            set { adjustday1 = value; }
        }

        /// <summary>
        /// 最长停留停期时间
        /// </summary>
        public int Adjustday2
        {
            get { return adjustday2; }
            set { adjustday2 = value; }
        }

        /// <summary>
        /// 是否随订随售
        /// </summary>
        public bool SDSS
        {
            get { return sdss; }
            set { sdss = value; }
        }

        /// <summary>
        /// 最晚出票
        /// </summary>
        public DateTime ZWCP
        {
            get { return zwcp; }
            set { zwcp = value; }
        }

        /// <summary>
        /// 是否Open
        /// </summary>
        public bool IsOpen
        {
            get { return isOpen; }
            set { isOpen = value; }
        }

        /// <summary>
        /// 最小出行人数
        /// </summary>
        public int LeastPerson
        {
            get { return leastPerson; }
            set { leastPerson = value; }
        }

        /// <summary>
        /// 是否特价
        /// </summary>
        public bool IsSpecial
        {
            get { return isSpecial; }
            set { isSpecial = value; }
        }

        /// <summary>
        /// 出票备注
        /// </summary>
        public string TicketRemark
        {
            get { return ticketRemark; }
            set { ticketRemark = value; }
        }

        /// <summary>
        /// 推荐等级
        ///     99为特推
        /// </summary>
        public int Recommend
        {
            get { return recommend; }
            set { recommend = value; }
        }

        /// <summary>
        /// 旅行日期时间段开始时间
        /// </summary>
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        /// <summary>
        /// 旅行日期时间段结束时间
        /// </summary>
        public DateTime ExpiryDate
        {
            get { return expiryDate; }
            set { expiryDate = value; }
        }

        /// <summary>
        /// 票种
        ///     默认：TicketType.BSPET
        /// </summary>
        public TicketType TicketType
        {
            get { return ticketType; }
            set { ticketType = value; }
        }

        /// <summary>
        /// 票种
        /// 本本票票1=本票纸质票(AirlineTicket),2=bsp纸质票(BSPTicket),3=本票电子票(Eticket),4=bsp电子票(BSPET)
        /// </summary>
        public string AllowCPType
        {
            get { return allowCPType; }
            set { allowCPType = value; }
        }

        /// <summary>
        /// 最晚出票
        /// </summary>
        public DateTime LastPrintDate
        {
            get { return lastPrintDate; }
            set { lastPrintDate = value; }
        }

        /// <summary>
        /// 法定最晚出票
        /// </summary>
        public DateTime FDZWCP
        {
            get { return fdzwcp; }
            set { fdzwcp = value; }
        }

        /// <summary>
        /// ADDON政策ID
        /// </summary>
        public int ADDONPID
        {
            get { return aDDONPID; }
            set { aDDONPID = value; }
        }

        /// <summary>
        /// 运价ID
        /// </summary>
        public string FareID
        {
            get { return fareID; }
            set { fareID = value; }
        }

        /// <summary>
        /// 退改签组合FareID
        /// </summary>
        public string RemarkFareId
        {
            get { return remarkFareId; }
            set { remarkFareId = value; }
        }

        /// <summary>
        /// 禁售/只售航班信息
        /// </summary>
        public string NoSalesStr
        {
            get { return noSalesStr; }
            set { noSalesStr = value; }
        }

        /// <summary>
        /// 是否应该立即出票
        /// </summary>
        public bool IsIssueNeeded
        {
            get { return isIssueNeeded; }
            set { isIssueNeeded = value; }
        }

        /// <summary>
        /// 所属(出票)航空公司
        /// </summary>
        public string OwnerAirline
        {
            get { return ownerAirline; }
            set { ownerAirline = value; }
        }

        /// <summary>
        /// 礼盒数量
        /// </summary>
        public int GiftNum
        {
            get { return giftNum; }
            set { giftNum = value; }
        }

        /// <summary>
        /// 礼盒描述
        /// </summary>
        public string GiftDesc
        {
            get { return giftDesc; }
            set { giftDesc = value; }
        }

        /// <summary>
        /// 礼盒类型
        /// </summary>
        public string GiftType
        {
            get { return giftType; }
            set { giftType = value; }
        }

        /// <summary>
        /// 往返程套票次数
        /// </summary>
        public int TicketSetTravelCount
        {
            get { return ticketSetTravelCount; }
            set { ticketSetTravelCount = value; }
        }

        /// <summary>
        /// 航班基本信息列表
        /// </summary>
        public List<FlightBaseInfo> FlightBaseInfos
        {
            get { return flightBaseInfos; }
            set { flightBaseInfos = value; }
        }

        /// <summary>
        /// 价格信息列表
        /// </summary>
        public List<PriceInfo> PriceInfos
        {
            get { return priceInfos; }
            set { priceInfos = value; }
        }
    }

    /// <summary>
    /// 航班基本信息
    /// </summary>
    /// <history>
    /// 	<date>2010-03-16</date>
    /// 	<programmer>zcchen</programmer>
    ///	    <document>国际机票查询接口改造(43786)</document>
    /// </history>
    [Serializable]
    public class FlightBaseInfo
    {
        private string subClass = String.Empty;
        private SeatGrade classGrade = SeatGrade.Y;
        private string schedule = String.Empty;
        private string baggage = String.Empty;
        private string baggageEng = String.Empty;
        private int quantity = 0;

        /// <summary>
        /// 与航班匹配的子舱位
        /// </summary>
        public string SubClass
        {
            get { return subClass; }
            set { subClass = value; }
        }

        /// <summary>
        /// 与航班匹配的大舱位
        ///     默认：SeatGrade.Y
        /// </summary>
        public SeatGrade ClassGrade
        {
            get { return classGrade; }
            set { classGrade = value; }
        }

        /// <summary>
        /// 班期
        /// </summary>
        public string Schedule
        {
            get { return schedule; }
            set { schedule = value; }
        }

        /// <summary>
        /// 中文行李额
        /// </summary>
        public string Baggage
        {
            get { return baggage; }
            set { baggage = value; }
        }

        /// <summary>
        /// 英文行李额
        /// </summary>
        public string BaggageEng
        {
            get { return baggageEng; }
            set { baggageEng = value; }
        }

        /// <summary>
        /// 舱位剩余数量
        ///     数量小于9表示舱位紧张
        /// </summary>
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
    }

    /// <summary>
    /// 机票票台价格列表
    /// </summary>
    /// <history>
    /// 	<date>2010-03-16</date>
    /// 	<programmer>zcchen</programmer>
    ///	    <document>国际机票查询接口改造(43786)</document>
    /// </history>
    [Serializable]
    public class PriceInfo
    {
        private int agentID = 0;
        private int agentCityID = 0;
        private string agentCityCode = String.Empty;
        private decimal price = 0;
        private decimal salesPrice = 0;
        private decimal tax = 0;
        private bool isContainOil = false;
        private decimal fuelCharge = 0;
        private decimal floorPrice = 0;
        private decimal exchange = 0;
        private decimal surcharge = 0;
        private decimal salesRate = 0;
        private decimal floorRate = 0;
        private string currency = String.Empty;
        private int profitType = 0;
        private decimal oldCost;
        private decimal oldOilFee;
        private decimal oldPrice;
        private decimal oldTax;
        private decimal oldSalePrice;
        private string insuranceRemark = string.Empty;

        private List<InsuranceInfo> insuranceInfos = new List<InsuranceInfo>();

        /// <summary>
        /// 票台ID
        /// </summary>
        public int AgentID
        {
            get { return agentID; }
            set { agentID = value; }
        }

        /// <summary>
        /// 出票城市ID
        /// </summary>
        public int AgentCityID
        {
            get { return agentCityID; }
            set { agentCityID = value; }
        }

        /// <summary>
        /// 出票城市Code
        /// </summary>
        public string AgentCityCode
        {
            get { return agentCityCode; }
            set { agentCityCode = value; }
        }

        /// <summary>
        /// 携程卖价
        /// </summary>
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        /// <summary>
        /// 航空公司销售价
        /// </summary>
        public decimal SalesPrice
        {
            get { return salesPrice; }
            set { salesPrice = value; }
        }

        /// <summary>
        /// 税
        /// </summary>
        public decimal Tax
        {
            get { return tax; }
            set { tax = value; }
        }

        /// <summary>
        /// 是否包含燃油
        /// </summary>
        public bool IsContainOil
        {
            get { return isContainOil; }
            set { isContainOil = value; }
        }

        /// <summary>
        /// 燃油费
        /// </summary>
        public decimal FuelCharge
        {
            get { return fuelCharge; }
            set { fuelCharge = value; }
        }

        /// <summary>
        /// 结算价
        /// </summary>
        public decimal FloorPrice
        {
            get { return floorPrice; }
            set { floorPrice = value; }
        }

        /// <summary>
        /// 汇率
        /// </summary>
        public decimal Exchange
        {
            get { return exchange; }
            set { exchange = value; }
        }

        /// <summary>
        /// 费用合计
        /// </summary>
        public decimal Surcharge
        {
            get { return surcharge; }
            set { surcharge = value; }
        }

        /// <summary>
        /// 卖价扣率
        /// </summary>
        public decimal SalesRate
        {
            get { return salesRate; }
            set { salesRate = value; }
        }

        /// <summary>
        /// 底价扣率
        /// </summary>
        public decimal FloorRate
        {
            get { return floorRate; }
            set { floorRate = value; }
        }

        /// <summary>
        /// 币种
        /// </summary>
        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }

        /// <summary>
        /// 利润类型
        /// </summary>
        public int ProfitType
        {
            get { return profitType; }
            set { profitType = value; }
        }

        /// <summary>
        /// 原始结算价
        /// </summary>
        public decimal OldCost
        {
            get { return oldCost; }
            set { oldCost = value; }
        }

        /// <summary>
        /// 原始燃油费
        /// </summary>
        public decimal OldOilFee
        {
            get { return oldOilFee; }
            set { oldOilFee = value; }
        }

        /// <summary>
        /// 原始携程卖价
        /// </summary>
        public decimal OldPrice
        {
            get { return oldPrice; }
            set { oldPrice = value; }
        }

        /// <summary>
        /// 原始航空公司卖价
        /// </summary>
        public decimal OldSalePrice
        {
            get { return oldSalePrice; }
            set { oldSalePrice = value; }
        }

        /// <summary>
        /// 原始税
        /// </summary>
        public decimal OldTax
        {
            get { return oldTax; }
            set { oldTax = value; }
        }

        /// <summary>
        /// 保险信息
        /// </summary>
        public List<InsuranceInfo> InsuranceInfos
        {
            get { return insuranceInfos; }
            set { insuranceInfos = value; }
        }

        private decimal _ServerFee;

        /// <summary>
        /// 商旅服务费
        /// </summary>
        public decimal ServerFee
        {
            get { return _ServerFee; }
            set { _ServerFee = value; }
        }

        /// <summary>
        /// 保险备注说明
        /// </summary>
        public string InsuranceRemark
        {
            get { return insuranceRemark; }
            set { insuranceRemark = value; }
        }
    }

    /// <summary>
    /// 保险信息
    /// </summary>
    /// <history>
    /// 	<date>2010-03-16</date>
    /// 	<programmer>zcchen</programmer>
    ///	    <document>国际机票查询接口改造(43786)</document>
    /// </history>
    [Serializable]
    public class InsuranceInfo
    {
        private string type = String.Empty;
        private int count = 0;

        /// <summary>
        /// 保险品种
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
    }
}