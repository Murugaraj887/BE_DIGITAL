using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using ExpenseAPP.Helper;



    public class KeyValueInt
    {
        public int ID { get; set; }
        public string Text { get; set; }

    }

    public class KeyValueString
    {
        public string ID { get; set; }
        public string Text { get; set; }

    }


    public class ExpTypePLExtCatMap
    {
        [SQLInfo("pl")]
        public string PL { get; set; }
        [SQLInfo("ExpCategory")]
        public string Cat { get; set; }
    }


    public class RoleHomePage
    {
        public string Role { get; set; }
        public string HomePage { get; set; }
        public int Value { get; set; }
    }

   
    public class DHSDMMAP
    {
          [SQLInfo("txtdhmailid")]
        public string DH { get; set; }
          [SQLInfo("txtSDMMailid")]
        public string SDM { get; set; }
    }
    //public class ApplnAccess
    //{
    //    public string Appln { get; set; }
    //    public string Access { get; set; }
    //}

    public class PUDMMapping
    {
          //[SQLInfo("txtdmmailid")]
        public string DM { get; set; }
        //  [SQLInfo("txtPu")]
        public string PU { get; set; }
    }
