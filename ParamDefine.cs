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
         
        /// //////////////////////////////////////////////用户列表开始/////////////////////////////////////////////////////////////// 
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

        /// //////////////////////////////////////////////用户列表结束///////////////////////////////////////////////////////////////
   

        /// /////////////////////////////////////////////套餐权限 开始///////////////////////////////////////////////////////////////
        
        /// /////////////////////////////////////////////套餐权限 结束///////////////////////////////////////////////////////////////
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
            /// </summary>
            public int Type { get; set; }
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
        /// //////////////////////////////////////////////套餐的可续费套餐开始///////////////////////////////////////////////////////////////
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
        /// //////////////////////////////////////////////套餐的可续费套餐结束///////////////////////////////////////////////////////////////
        /// //////////////////////////////////////////////XXXX开始///////////////////////////////////////////////////////////////
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
        /// //////////////////////////////////////////////XXXX结束///////////////////////////////////////////////////////////////
    }
}
