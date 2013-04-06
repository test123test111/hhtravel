using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CtripServices.IntlFlight.DataContract
{
    /// <summary>
    /// 行程类型
    /// </summary>
    [Serializable]
    public enum TripType
    {
        /// <summary>
        /// 单程
        /// </summary>
        OW = 1,

        /// <summary>
        /// 往返
        /// </summary>
        RT = 2,

        /// <summary>
        /// 多程
        /// </summary>
        MT = 4
    }

    /// <summary>
    /// 乘客类型
    /// </summary>
    [Serializable]
    public enum PassengerType
    {
        /// <summary>
        /// 成人
        /// </summary>
        ADT = 1,

        /// <summary>
        /// 婴儿
        /// </summary>
        INF = 2,

        /// <summary>
        /// 儿童
        /// </summary>
        CHD = 4
    }

    /// <summary>
    /// 销售渠道
    /// </summary>
    [Serializable]
    public enum SalesType
    {
        /// <summary>
        /// 网上
        /// </summary>
        Online = 1,

        /// <summary>
        /// 英文
        /// </summary>
        English = 2,

        /// <summary>
        /// 人工
        /// </summary>
        Manual = 4,

        /// <summary>
        /// 无线
        /// </summary>
        Wireless = 8,

        /// <summary>
        /// 度假
        /// </summary>
        Holiday = 16,

        /// <summary>
        /// 商旅
        /// </summary>
        Trade = 32,

        /// <summary>
        /// 分销联盟
        /// </summary>
        DistributionAlliance = 64
    }

    /// <summary>
    /// 运价类型
    /// </summary>
    [Serializable]
    [Flags]
    public enum FareType
    {
        /// <summary>
        /// 所有
        /// </summary>
        All = 0,

        /// <summary>
        /// 最低价
        /// </summary>
        LowestPrice = 1,

        /// <summary>
        /// 直飞
        /// </summary>
        Direct = 2,

        /// <summary>
        /// 套票
        /// </summary>
        TicketSet = 4,

        /// <summary>
        /// Open程
        /// </summary>
        Open = 8,

        /// <summary>
        /// 特惠
        /// </summary>
        Special = 16,

        /// <summary>
        /// 推荐
        /// </summary>
        Recommended = 32
    }

    /// <summary>
    /// 乘客资质
    /// </summary>
    [Serializable]
    public enum Eligibility
    {
        /// <summary>
        /// 所有
        /// </summary>
        ALL,

        /// <summary>
        /// 普通/成人
        /// </summary>
        NOR,

        /// <summary>
        /// 学生
        /// </summary>
        STU,

        /// <summary>
        /// 移民
        /// </summary>
        EMI,

        /// <summary>
        /// 海员
        /// </summary>
        SEA,

        /// <summary>
        /// 青年
        /// </summary>
        YTH,

        /// <summary>
        /// 劳工
        /// </summary>
        LBR,

        /// <summary>
        /// 老年人
        /// </summary>
        SRC,

        /// <summary>
        /// 探亲访友
        /// </summary>
        VFR,

        /// <summary>
        /// 军人
        /// </summary>
        MIL,

        /// <summary>
        /// 成人陪伴儿童
        /// </summary>
        CNN,

        /// <summary>
        /// 无座婴儿
        /// </summary>
        INF
    }

    /// <summary>
    /// 商旅用户支出类型
    /// </summary>
    [Serializable]
    public enum BusinessType
    {
        /// <summary>
        /// 自费
        /// </summary>
        OWN = 0,

        /// <summary>
        /// 公费
        /// </summary>
        PUB = 1
    }

    /// <summary>
    /// 舱位等级
    /// </summary>
    [Serializable]
    public enum SeatGrade
    {
        /// <summary>
        /// 经济舱
        /// </summary>
        Y,

        /// <summary>
        /// 超经舱
        /// </summary>
        S,

        /// <summary>
        /// 公务舱
        /// </summary>
        C,

        /// <summary>
        /// 头等舱
        /// </summary>
        F
    }

    /// <summary>
    /// 舱位等级
    /// </summary>
    [Serializable]
    public enum ResultMode
    {
        /// <summary>
        /// 所有价格
        /// </summary>
        All = 0,

        /// <summary>
        /// 航班最低价
        /// </summary>
        LowestPrice = 1,

        /// <summary>
        /// 航班更多价格
        /// </summary>
        MorePrice = 2
    }

    /// <summary>
    /// 排序字段
    /// </summary>
    [Serializable]
    public enum OrderBy
    {
        /// <summary>
        /// 价格
        /// </summary>
        Price = 1,

        /// <summary>
        /// 起飞时间
        /// </summary>
        DTime = 2,

        /// <summary>
        /// 到达时间
        /// </summary>
        Atime = 4
    }

    /// <summary>
    /// 排序方向
    /// </summary>
    [Serializable]
    public enum Direction
    {
        /// <summary>
        /// 升序
        /// </summary>
        Asc = 0,

        /// <summary>
        /// 降序
        /// </summary>
        Desc = 1
    }

    /// <summary>
    /// 票种
    /// </summary>
    [Serializable]
    public enum TicketType
    {
        /// <summary>
        /// 本票纸质票
        /// </summary>
        AirlineTicket = 1,

        /// <summary>
        /// bsp纸质票
        /// </summary>
        BSPTicket = 2,

        /// <summary>
        /// 本票电子票
        /// </summary>
        Eticket = 4,

        /// <summary>
        /// bsp电子票
        /// </summary>
        BSPET = 8
    }

    //add by kluo 订单接口增加语种标识 枚举
    [Serializable]
    public enum cLanguageVersion
    {
        /// <summary>
        /// 中文
        /// </summary>
        CN,

        /// <summary>
        /// 英文
        /// </summary>
        EN,

        /// <summary>
        /// 日文
        /// </summary>
        JP,

        /// <summary>
        /// 韩文
        /// </summary>
        KR,

        /// <summary>
        /// 德文
        /// </summary>
        DE,

        /// <summary>
        /// 法文
        /// </summary>
        FR,

        /// <summary>
        /// 西班牙
        /// </summary>
        ES,

        /// <summary>
        /// 俄文
        /// </summary>
        RU
    }
}