using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

 
    public  class UserIdentity
    {

        public static string CurrentUser
        {
            get
            {
                string MachineUserid = HttpContext.Current.User.Identity.Name;
                // string userid = "Gopinathreddy_p";

                string[] userids = MachineUserid.Split('\\');
                if (userids.Length == 2)
                {
                    MachineUserid = userids[1];
                };

                return MachineUserid;
            }
        }
    }

 

 