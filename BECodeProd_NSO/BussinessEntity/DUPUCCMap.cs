using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BEData.BusinessEntity
{
    public class DUPUCCMap
    {
        public string PU { get; set; }


        public string NSOCOde { get; set; }
        public string DU { get; set; }
        public string CustomerCode { get; set; }
    }

    public class Region
    {
        public string Reg { get; set; }
    }

    public class NSOCodeDescMapping
    {
        public string NSOCode { get; set; }
        public string NSODesc { get; set; }
    }
}