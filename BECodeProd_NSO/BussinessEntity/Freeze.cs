using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BEData.BusinessEntity
{
    
        [Serializable]
        public class Freeze
        {
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string ModifiedBy { get; set; }
            public string ModifiedOn { get; set; }
        }
    
}