using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BEData.BusinessEntity
{
    public class AdminAllDU
    {
        public string txtPU {get;set;}
        public string masterCustomerCode { get; set; }
        public string Region { get; set; }

        //TODO:12/10 is all DMs commented
    //      public string txtDMMailId { get; set; }
        // public string isAllDU { get; set; }
        public string Betype { get; set; }

       // public string Currency { get; set; }
        public string portFolio { get; set; }
        public string updatedBy { get; set; }
        public string RevDMorSDM { get; set; }
        public string VolDMorSDM { get; set; }
        public List<string> lstportFolio { get; set; }
        //public List<string> lstCurrency { get; set; }
        public List<string> lstPU  { get; set; }
        public List<string> lstBE { get; set; }
        public string anchors { get; set; }
        public int AdminNo { get; set; }
    }
}