using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BEData.BusinessEntity
{
    
        
         public class BEMonthlyFreeze
        {
            public string Year { get; set; }
            public string Quarter { get; set; }
            public bool Month1 { get; set; }
            public bool Month2 { get; set; }
            public bool Month3 { get; set; }
        }
    
}