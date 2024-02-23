using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BEData.BusinessEntity
{
    public class BEDataUI
    {
        public string CustomerCode { get; set; }
        public string DU { get; set; }
        public string DM { get; set; }
        public string NativeCurrency { get; set; }

        public string DMMonth1 { get; set; }
        public string DMMonth2 { get; set; }
        public string DMMonth3 { get; set; }
        public string DMQCur { get; set; }
        //public string DMQNext { get; set; }
        public string DMQPrev { get; set; }

        public string SDMMailID { get; set; }

        public string SDMMonth1 { get; set; }
        public string SDMMonth2 { get; set; }
        public string SDMMonth3 { get; set; }
        public string SDMQCur { get; set; }
        //public string SDMQNext { get; set; }
        public string SDMQPrev { get; set; }



        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string ActualM1 { get; set; }
        public string ActualM2 { get; set; }
        public string ActualM3 { get; set; }
        public string IsApproved { get; set; }
        public string GuidanceConversionRate { get; set; }
        public string CurrentConversionRate { get; set; }


    }
}