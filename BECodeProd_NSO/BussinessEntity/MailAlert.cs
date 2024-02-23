using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BEData.BusinessEntity
{
    public class MailAlert
    {
        public string txtPU { get; set; }
        public string masterCustomerCode { get; set; }
        public string portFolio { get; set; }
        public string updatedBy { get; set; }
        public string SendTo { get; set; }
        public string SendCC { get; set; }
        public string anchors { get; set; }
        public string OnDMRev { get; set; }
        public string OnsDMRev { get; set; }
        public string OnDMVol { get; set; }
        public string OnSDMVol { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
        public int AdminNo { get; set; }
        public List<string> lstportFolio { get; set; }
        public List<string> lstPU { get; set; }

    }
}