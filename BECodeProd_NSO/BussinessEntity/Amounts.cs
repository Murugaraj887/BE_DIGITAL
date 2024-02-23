using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using ExpenseAPP.Helper;

    public class Amounts
    {
          [SQLInfo("fltdhamt")]
        public double DHAmount1 { get; set; }
           [SQLInfo("fltshamt")]
        public double SHAmount2 { get; set; }
           [SQLInfo("fltaskedamt")]
        public double AskedAmount3 { get; set; }
           [SQLInfo("fltfinalisedamt")]
        public double FinalisedAmount4 { get; set; }

         
    }
