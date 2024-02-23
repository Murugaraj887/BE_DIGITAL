using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BEData.BusinessEntity
{
    public class DMDetailsPopUp
    {
        public string txtCustomerCode { get; set; }
        public string txtCurrency { get; set; }
        public string txtDMMailId { get; set; }
        public string DMMonth1 { get; set; }
        public string DMMonth2 { get; set; }
        public string DMMonth3 { get; set; }
        public string PU { get; set; }
        public string total { get; set; }

        public string DMMonth1Onsite { get; set; }
        public string DMMonth2Onsite { get; set; }
        public string DMMonth3Onsite { get; set; }

        public string DMMonth1Offshore { get; set; }
        public string DMMonth2Offshore { get; set; }
        public string DMMonth3Offshore { get; set; }
        public string DMTotalOffshore { get; set; }
        public string DMTotalOnsite { get; set; }

       

        public string DMOnsiteOffshoreTotal { get; set; }
        public string txtPU { get; set; }
    }
}