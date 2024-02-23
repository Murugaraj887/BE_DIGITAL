using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BEData.BusinessEntity
{
    public class AuditUI
    {
        public int intHide { get; set; }
        public int intBEId { get; set; }
        public string txtMasterClientCode { get; set; }
        public string txtMasterClientName { get; set; }
        public string txtPU { get; set; }
        public string txtDMMailId { get; set; }
        public string txtNativeCurrency { get; set;}
        public string DMfltMonth1BE { get; set; }
        public string DMfltMonth2BE { get; set; }
        public string DMfltMonth3BE { get; set; }
        public string txtCurrentQuarterName { get; set; }
        public string txtYear { get; set; }
        public string txtCreatedBy { get; set; }
        public string txtCreatedDate { get; set; }
        public string txtDMUpdatedby { get; set; }
        public string txtDMUpdatedDate { get; set; }
        public string txtRemarks { get; set; }
        public string txtBeType { get; set; }
        public string dtDumpDate { get; set; }
        public string txtActionType { get; set; }
        public string txtIsChanged { get; set; }

        public string fltDMEffortMonth1Onsite { get; set; }
        public string fltDMEffortMonth1OffShore { get; set; }
        public string fltDMEffortMonth2Onsite { get; set; }
        public string fltDMEffortMonth2OffShore { get; set; }
        public string fltDMEffortMonth3Onsite { get; set; }
        public string fltDMEffortMonth3OffShore { get; set; }
     
    }
}