using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BEData.BusinessEntity
{
    public class ClientCodePortfolio
    {
        public string txtMasterClientCode { get; set; }
        public string txtMasterCustomerName { get; set; }
        public string txtClientCode { get; set; }
        public string txtClientName { get; set; }
        public string txtPortfolio { get; set; }
        public string txtDivision { get; set; }
        public string txtVertical { get; set; }
        public string txtRHMailId { get; set; }
        public string txtSDMMailId { get; set; }
        public string txtDHMailId { get; set; }
        public string txtBITSCSIHMailId { get; set; }
        public string txtUHMailId { get; set; }
        public string txtPU { get; set; }
        public string txtFAPortfolio { get; set; }
        public int intmccid { get; set; }
        public string isActive { get; set; }
        public string txtMCOName { get; set; }
        public string txtServiceline { get; set; }
        public string txtunit { get; set; }
    }
}