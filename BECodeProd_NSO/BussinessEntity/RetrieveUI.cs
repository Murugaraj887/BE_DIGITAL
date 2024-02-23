using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BEData.BusinessEntity
{
    public class RetrieveUI
    {
        public int intSNo { get; set; }
        public string txtSBUCode { get; set; }
        public string txtDMMailId { get; set; }
        public string txtSDMMailId { get; set; }
        public string txtDHMailId { get; set; }
        public string txtBUCode { get; set; }
        public string txtUpdatedBy { get; set; }
        public string txtUpdateDt { get; set; }
        public string txtBITSCSIHMailId { get; set; }
        public string txtUHMailId { get; set; }
        public string txtVertical { get; set; }
        public string txtPortfolio { get; set; }
        public List<string> lstPU { get; set; }
    }
}