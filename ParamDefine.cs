using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckRenewalPkg
{
    class ParamDefine
    {
        public static string UserTreeDefault = @"{""error"":0,""reason"":"""",""result"":[{""id"":1,""parentId"":0,""name"":""运营中心(2620782)"",""nodeLevel"":1,""hasChildren"":false},
{""id"":1703,""parentId"":1184,""name"":""章益鹏浙D1211W(1)"",""nodeLevel"":11,""hasChildren"":false},
{""id"":1705,""parentId"":1184,""name"":""朱海滨鲁p1h838(1)"",""nodeLevel"":11,""hasChildren"":false}]}";

        #region 用户列表开始
        public class UserTreeResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string parentId { get; set; }
            /// <summary>
            /// 深圳超通互动管理中心(2261)
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int nodeLevel { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string hasChildren { get; set; }
        }

        public class UserTree
        {
            /// <summary>
            /// 
            /// </summary>
            public int error { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<UserTreeResultItem> result { get; set; }
        }
        #endregion

        #region 套餐权限
        public class PackageListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string ID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Data { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string PackageType { get; set; }
            /// <summary>
            /// 48G 12个月(每月4G)
            /// </summary>
            public string PackageName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string UnitPrice { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string BackPrice { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string TopLevel { get; set; }
            /// <summary>
            /// //0-移动电信， 3-年，2-月，1-叠加，4-加油
            public int Type { get; set; }
            /// </summary>
            /// <summary>
            //显示单价
            public string ShowUnitPrice { get; set; }
            /// </summary>
        }

        public class HoldList
        {
            /// <summary>
            /// 
            /// </summary>
            public int WXPayId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int HoldId { get; set; }
            /// <summary>
            /// 0_M_深圳酷比
            /// </summary>
            public string HoldName { get; set; }
            /// <summary>
            /// 0_厂家_M
            /// </summary>
            public string ParentHoldName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string HoldExp { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string PayEE { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string PayKEY { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string WxPayUrl { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string PayNOTIFY_URL { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string MCHID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string PayIP { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<PackageListItem> PackageList { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<PackageListItem> YDPackageList { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<PackageListItem> DXPackageList { get; set; }
        }

        public class HoldRenewalsList
        {
            /// <summary>
            /// 
            /// </summary>
            public int error { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<HoldList> result { get; set; }
        }
        #endregion 
        /// //////////////////////////////////////////////套餐的可续费套餐开始///////////////////////////////////////////////////////////////
        #region 套餐的可续费套餐开始
        public class RenewalsPackageItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string PackageCode { get; set; }
            /// <summary>
            /// 180G 12个月(每月15G)
            /// </summary>
            public string PackageName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string UnitPrice { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string BackPrice { get; set; }
        }

        public class RenewalsPackage
        {
            /// <summary>
            /// 
            /// </summary>
            public int error { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<RenewalsPackageItem> result { get; set; }
        }
        #endregion
        /// //////////////////////////////////////////////套餐的可续费套餐结束///////////////////////////////////////////////////////////////
        #region 返利数据
        public class BackMoneyResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string iccid { get; set; }
            /// <summary>
            /// 48G 12个月(每月4G)
            /// </summary>
            public string packageName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double backPrice { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double renewalsPrice { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string renewalsTime { get; set; }
        }

        public class BackMoney
        {
            /// <summary>
            /// 
            /// </summary>
            public int error { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<BackMoneyResultItem> result { get; set; }
        }
        #endregion
        /// //////////////////////////////////////////////XXXX结束///////////////////////////////////////////////////////////////
        /// 
        /// 
        #region 续费汇总- 脱网卡-激活卡----
        /// 
        public class RenewalsTotalResult
        {
            /// <summary>
            /// 
            /// </summary>
            public int allCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int ltAllCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int ltActivatedCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int ltStopCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int ltOutCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int twiceRenewalsCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double renewalsAmount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int renewalsCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int ltRenewalsCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double backAmount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int lastDayAllCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int lastDayActivatedCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int lastDayStopCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int lastDayOutCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int lastDayTwiceRenewalsCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double lastDayRenewalsAmount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int lastDayRenewalsCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double lastDayBackAmount { get; set; }
        }

        public class RenewalsTotal
        {
            /// <summary>
            /// 
            /// </summary>
            public int error { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public RenewalsTotalResult result { get; set; }
        }
        /// 
        #endregion

        #region 月续费汇总
        ///////////////////////////////////////月续费汇总/////////////////////////////////////////////////////////
        public class MonthRenewalsTotalResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string date { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string addSimCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string simCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string times { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double amount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double usage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string stopTimes { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string stopAmount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string stopDays { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string firstTimes { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string firstAmount { get; set; }
        }

        public class MonthRenewalsTotal
        {
            /// <summary>
            /// 
            /// </summary>
            public string error { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<MonthRenewalsTotalResultItem> result { get; set; }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion






        #region 用量数据
        /////////////////////////////////////////////用量数据////////////////////////////////////////////////////
        public class SimStatisticsListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string sourceType { get; set; }
            /// <summary>
            /// 全部
            /// </summary>
            public string sourceName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string allNum { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string stockNum { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string testableNum { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string canactivateNum { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string activatedNum { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string stopedNum { get; set; }
        }

        public class SourceListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string sourceType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sourceName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string amountUsage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string validCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string validAvgUsage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string iAmountUsage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string iValidCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string iValidAvgUsage { get; set; }
        }

        public class DayStatisticsListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string statDay { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double amountUsage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string validCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double validAvgUsage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string iAmountUsage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string iValidCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string iValidAvgUsage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<SourceListItem> sourceList { get; set; }
        } 
  

        public class MonthStatisticsListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string statMonth { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string amountUsage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<SourceListItem> sourceList { get; set; }
        }

        public class FlowReportResult
        {
            /// <summary>
            /// 
            /// </summary>
            public string holdId { get; set; }
            /// <summary>
            /// M上海函夏贸易AL
            /// </summary>
            public string holdName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string updateTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<SimStatisticsListItem> simStatisticsList { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<DayStatisticsListItem> dayStatisticsList { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<MonthStatisticsListItem> monthStatisticsList { get; set; }
        }

        public class FlowReportRoot
        {
            /// <summary>
            /// 
            /// </summary>
            public int error { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public FlowReportResult result { get; set; }
        }
        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////老续费 begin//////////////////////////////////////////////////////////////
        #region 老续费
        public class OldRenewalsRootResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public int SummSort { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string totalDay { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int renewalsTimes { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double renewalsAmount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double MA5 { get; set; }
        }

        public class OldRenewalsRoot
        {
            /// <summary>
            /// 
            /// </summary>
            public int error { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<OldRenewalsRootResultItem> result { get; set; }
        }
        #endregion
        ///////////////////////////////////////////老续费 end //////////////////////////////////////////////////////////////




        ///////////////////////////////////////////续费明细的汇总 begin //////////////////////////////////////////////////////////////

        #region 续费明细的汇总
        public class RenewalsOrderSumTotalItem
        {
            /// <summary>
            /// 30M(首年激活套餐)
            /// </summary>
            public string packageName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int times { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double usage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double amount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double backPrice { get; set; }
        }

        public class RenewalsOrderSumResult
        {
            /// <summary>
            /// 
            /// </summary>
            public List<RenewalsOrderSumTotalItem> Total { get; set; }
            /// <summary>
            /// 
            /// </summary>
           // public List<object> Order { get; set; }
        }

        public class RenewalsOrderSum
        {
            /// <summary>
            /// 
            /// </summary>
            public string error { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public RenewalsOrderSumResult result { get; set; }
        }

        #endregion
        ///////////////////////////////////////////续费明细的汇总 end //////////////////////////////////////////////////////////////



        #region 套餐列表-分组列表
        public class SltPackageTypeItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string Selected { get; set; }
            /// <summary>
            /// 全部
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Value { get; set; }
        }

        public class SltDeviceTypeItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string Selected { get; set; }
            /// <summary>
            /// 全部
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Value { get; set; }
        }

        public class SltRatePlonTypeItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string Selected { get; set; }
            /// <summary>
            /// 全部
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Value { get; set; }
        }

        public class SltCustomerTypeItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string Selected { get; set; }
            /// <summary>
            /// 全部
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Value { get; set; }
        }

        public class SltSourceTypeItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string Selected { get; set; }
            /// <summary>
            /// 全部
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Value { get; set; }
        }

        public class SltCardTypeItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string Selected { get; set; }
            /// <summary>
            /// 全部
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Value { get; set; }
        }

        public class PkgListDetailResult
        {
            /// <summary>
            /// 
            /// </summary>
            public List<SltPackageTypeItem> sltPackageType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<SltDeviceTypeItem> sltDeviceType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<SltRatePlonTypeItem> sltRatePlonType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<SltCustomerTypeItem> sltCustomerType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<SltSourceTypeItem> sltSourceType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<SltCardTypeItem> sltCardType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string sltTagType { get; set; }
        }

        public class PkgListDetailRoot
        {
            /// <summary>
            /// 
            /// </summary>
            public int error { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public PkgListDetailResult result { get; set; }
        }
        #endregion


        #region 账号下套餐分布
        public class PkgDistributionResultItem
        {
            /// <summary>
            /// 1GB_(一年有效)
            /// </summary>
            public string groupByName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string groupByValue { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string groupByValue2 { get; set; }
        }

        public class PkgDistributionRoot
        {
            /// <summary>
            /// 
            /// </summary>
            public int error { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<PkgDistributionResultItem> result { get; set; }
        }
        #endregion

        #region 搜索卡列表
        public class SearchSimListDetail
        {
            /// <summary>
            /// 
            /// </summary>
            public string indexNo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string simId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string guid { get; set; }
            /// <summary>
            /// 成都乐富易
            /// </summary>
            public string holdName { get; set; }
            /// <summary>
            /// 36M一年
            /// </summary>
            public string package { get; set; }
            /// <summary>
            /// 否
            /// </summary>
            public string isUsageReset { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string amountUsageData { get; set; }
            /// <summary>
            /// 可激活
            /// </summary>
            public string simState { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string lastActiveTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string expireTime { get; set; }
            /// <summary>
            /// 365天
            /// </summary>
            public string oddTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string monthUsageData { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string flowLeftValue { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string totalMonthUsageFlow { get; set; }
        }

          public class SearchSimListRoot
          {
              public List<SearchSimListDetail> result { get; set; }
          }
        
        #endregion

        #region 卡的可续费套餐
        public class SimRenewalsPkgListResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public int packageId { get; set; }
            /// <summary>
            /// 4GB(一年有效)
            /// </summary>
            public string packageName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string price { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string origPrice { get; set; }
            /// <summary>
            /// 4096MB，流量不清零，一年有效，全国通用，总流量用完即停机，可累加年套餐。
            /// </summary>
            public string packageInfo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int isAddPackage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int isUsageReset { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int addUsageForYear { get; set; }
        }

        public class SimRenewalsPkgListRoot
        {
            /// <summary>
            /// 
            /// </summary>
            public int error { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<SimRenewalsPkgListResultItem> result { get; set; }
        }
        #endregion



        #region 激活停用数汇总

        public class ActivateCountResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string date { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string activatedCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string deactivatedCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string firstActivatedCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string lastDeactivatedCount { get; set; }
        }

        public class ActivateCountRoot
        {
            /// <summary>
            /// 
            /// </summary>
            public int error { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<ActivateCountResultItem> result { get; set; }
        }
        #endregion

        #region 续费用量汇总
        public class RenewalsUsageResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string date { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string amount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string usage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string times { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string stopTimes { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string firstTimes { get; set; }
        }

        public class RenewalsUsageRoot
        {
            /// <summary>
            /// 
            /// </summary>
            public int error { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<RenewalsUsageResultItem> result { get; set; }
        }
        #endregion


        #region 搜索用户
        public class HoldLikeNameResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string holdId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
        }

        public class HoldLikeNameRoot
        {
            /// <summary>
            /// 
            /// </summary>
            public int error { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string reason { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<HoldLikeNameResultItem> result { get; set; }
        }
        #endregion

    }
}
