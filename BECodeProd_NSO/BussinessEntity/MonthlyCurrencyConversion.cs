using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BEData.BusinessEntity
{
    public class MonthlyCurrencyConversion
    {
        public string nativeCurrency { get; set; }
        public double guidanceRate { get; set; }
        public double BenchMarkrate { get; set; }
        public double QtyAvgFARate { get; set; }
        public double actualRateMonth1 { get; set; }
        public double actualRateMonth2 { get; set; }
        public double actualRateMonth3 { get; set; }
        public string month { get; set; }
        public string year { get; set; }
    }
}