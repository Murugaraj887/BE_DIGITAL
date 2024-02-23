using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BEData.BusinessEntity
{
    public class DelegateUI
    {
        public int SNo { get; set; }
        public string txtFromUser { get; set; }
        public string txtToUser { get; set; }
        public string dtFromDate { get; set; }
        public string dtToDate { get; set; }  
        public List<string> lstUserId { get; set; }
    }
}