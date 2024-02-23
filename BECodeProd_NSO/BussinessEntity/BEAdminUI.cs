using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BEData.BusinessEntity
{
    public class BEAdminUI
    {
        public string UserId { get; set; }
        public string PU { get; set; }
        public string Role { get; set; }
        public string[] DuList { get; set; }
        public string[] MasterCustomerList { get; set; }
        public string[] ReportCodeList { get; set; }
        public string[] SDMMailIdlist { get; set; }
        public string IsAdmin { get; set; }
    }
}