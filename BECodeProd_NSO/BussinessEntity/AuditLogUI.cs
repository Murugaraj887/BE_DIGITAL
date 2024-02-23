using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BEData.BusinessEntity
{
    public class AuditLogUI
    {
        public int id { get; set; }
        public string quarter { get; set; }
        public bool revenueDM { get; set; }
        public bool revenueSDM { get; set; }
        public bool volumeDM { get; set; }
        public bool volumeSDM { get; set; }
    }
}