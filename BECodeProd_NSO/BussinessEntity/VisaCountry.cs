using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using ExpenseAPP.Helper;


    public class VisaCountry
    {

        [SQLInfo("Visa/Wp Type")]
        public string VisaWP { get; set; }
        [SQLInfo("Country")]
        public string Country { get; set; }
        [SQLInfo("Visa Type")]
        public string VisaType { get; set; }
    }
