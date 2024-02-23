using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BEData.BusinessEntity;
using BEData;
using System.Configuration;



    public partial class AdminSite : MasterPage
    {
      
        private BEDL service = new BEDL();
        public string fileName = "BEData.SiteMaster";
        Logger logger = new Logger();
        string accessDenied = "alert('Access denied'); return false;";
        protected void Page_Load(object sender, EventArgs e)
        {
            
            return;

            try
            {

                string status = service.CheckStatus();

                if (status != "Active")
                {
                    Response.Redirect("Process.htm");
                }
                if (!Page.IsPostBack)
                {
                    string machineUser = string.Empty;
                    string[] machineUsers = HttpContext.Current.User.Identity.Name.Split('\\');
                    if (machineUsers.Length == 2)
                        machineUser = machineUsers[1];
                    string role = service.GetUserRole(machineUser);

                    


                    if (role + "" != "Admin" && role + "" != "PnA")
                        if (service.IsApplicationOffline())
                            Response.Redirect("Maintenance.aspx");

                    string roleAdmin = Session["Role"] + "";
                    if (roleAdmin.ToString() == role.ToString())
                    {
                        if (roleAdmin.ToLower() == "pna" || roleAdmin.ToLower() == "pna-csi" || roleAdmin.ToLower() == "pna-pps" || roleAdmin.ToLower() == "admin")
                        {
                            string MenuAccessCode = service.GetMenuCode(roleAdmin);

                            List<string> lstMenuAccessCodes = new List<string>();
                            if (MenuAccessCode.Length > 0)
                                lstMenuAccessCodes = MenuAccessCode.Split(',').Select(k => k.Trim()).Distinct(StringComparer.InvariantCultureIgnoreCase).ToList();

                            //string[] ary = { "A01", "A04", "A004", "A9" };
                            string[] ary = lstMenuAccessCodes.ToArray();

                            List<MenuAttributes> lstMenuAttrributes = new List<MenuAttributes>();
                            lstMenuAttrributes = GetMenuAttributes();

                            Func<string, MenuItem> func = k =>
                            {
                                var temp = lstMenuAttrributes.Single(k1 => k1.key == k);
                                return new MenuItem() { Text = temp.Text, NavigateUrl = temp.URL, Value = temp.key };

                            };



                            List<MenuValue> lstMenuValue = new List<MenuValue>();
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A01", Level3 = "A1", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A01", Level3 = "A2", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A01", Level3 = "A3", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A02", Level3 = "A4", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A02", Level3 = "A5", Level4 = "" });
                            //lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A03", Level3 = "A6", Level4 = "" });
                            //lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A03", Level3 = "A7", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A03", Level3 = "A8", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A004", Level4 = "A9" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A004", Level4 = "A10" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A11", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A12", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A13", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A14", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A15", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A16", Level4 = "" });

                            lstMenuValue = lstMenuValue.Where(k => ary.Contains(k.Level1) || ary.Contains(k.Level2) || ary.Contains(k.Level3) || ary.Contains(k.Level4)).ToList();


                            string[] distinctLevel1 = lstMenuValue.Select(k => k.Level1).Distinct().ToArray();

                            foreach (string item in distinctLevel1)
                                MenuAdmin.Items.Add(func(item));

                            for (int i = 0; i < distinctLevel1.Length; i++)
                            {
                                string key = distinctLevel1[i];
                                string[] _distinctLevel2 = lstMenuValue.Where(k => k.Level1 == key).Select(k => k.Level2).Distinct().ToArray();
                                for (int j = 0; j < _distinctLevel2.Length; j++)
                                    //if (ary.Contains(_distinctLevel2[j]))
                                    // if (MenuAdmin.Items.Count > distinctLevel1.Length) 
                                    MenuAdmin.Items[i].ChildItems.Add(func(_distinctLevel2[j]));
                            }

                            string[] distinctLevel2 = lstMenuValue.Select(k => k.Level2).Distinct().ToArray();
                            for (int i = 0; i < distinctLevel2.Length; i++)
                            {
                                string key = distinctLevel2[i];
                                string[] _distinctLevel3 = lstMenuValue.Where(k => k.Level2 == key).Select(k => k.Level3).Distinct().ToArray();
                                for (int j = 0; j < _distinctLevel3.Length; j++)
                                    if (ary.Contains(_distinctLevel3[j]))
                                        if (MenuAdmin.Items[0].ChildItems.Count >= distinctLevel2.Length)
                                            MenuAdmin.Items[0].ChildItems[i].ChildItems.Add(func(_distinctLevel3[j]));

                            }
                            foreach (MenuItem item in MenuAdmin.Items)
                                foreach (MenuItem item0 in item.ChildItems)
                                    foreach (MenuItem item1 in item0.ChildItems)
                                        if (item1.Value == "A004")
                                        {
                                            if (ary.Contains("A9"))
                                                item1.ChildItems.Add(func("A9"));
                                            if (ary.Contains("A10"))
                                                item1.ChildItems.Add(func("A10"));
                                        }

                        }
                    }
                    else
                    {
                        if (roleAdmin.ToLower() == "admin" || roleAdmin.ToLower() == "pna" || roleAdmin.ToLower() == "pna-bits" || roleAdmin.ToLower() == "pna-csi")
                        {
                             string MenuAccessCode = service.GetMenuCode(roleAdmin);

                            List<string> lstMenuAccessCodes = new List<string>();
                            if (MenuAccessCode.Length > 0)
                                lstMenuAccessCodes = MenuAccessCode.Split(',').Select(k => k.Trim()).Distinct(StringComparer.InvariantCultureIgnoreCase).ToList();

                            //string[] ary = { "A01", "A04", "A004", "A9" };
                            string[] ary = lstMenuAccessCodes.ToArray();

                            List<MenuAttributes> lstMenuAttrributes = new List<MenuAttributes>();
                            lstMenuAttrributes = GetMenuAttributes();

                            Func<string, MenuItem> func = k =>
                            {
                                var temp = lstMenuAttrributes.Single(k1 => k1.key == k);
                                return new MenuItem() { Text = temp.Text, NavigateUrl = temp.URL, Value = temp.key };

                            };



                            List<MenuValue> lstMenuValue = new List<MenuValue>();
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A01", Level3 = "A1", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A01", Level3 = "A2", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A01", Level3 = "A3", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A02", Level3 = "A4", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A02", Level3 = "A5", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A03", Level3 = "A6", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A03", Level3 = "A7", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A03", Level3 = "A8", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A004", Level4 = "A9" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A004", Level4 = "A10" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A11", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A12", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A13", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A14", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A15", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A16", Level4 = "" });

                            lstMenuValue = lstMenuValue.Where(k => ary.Contains(k.Level1) || ary.Contains(k.Level2) || ary.Contains(k.Level3) || ary.Contains(k.Level4)).ToList();


                            string[] distinctLevel1 = lstMenuValue.Select(k => k.Level1).Distinct().ToArray();

                            foreach (string item in distinctLevel1)
                                MenuAdmin.Items.Add(func(item));

                            for (int i = 0; i < distinctLevel1.Length; i++)
                            {
                                string key = distinctLevel1[i];
                                string[] _distinctLevel2 = lstMenuValue.Where(k => k.Level1 == key).Select(k => k.Level2).Distinct().ToArray();
                                for (int j = 0; j < _distinctLevel2.Length; j++)
                                    //if (ary.Contains(_distinctLevel2[j]))
                                    // if (MenuAdmin.Items.Count > distinctLevel1.Length) 
                                    MenuAdmin.Items[i].ChildItems.Add(func(_distinctLevel2[j]));
                            }

                            string[] distinctLevel2 = lstMenuValue.Select(k => k.Level2).Distinct().ToArray();
                            for (int i = 0; i < distinctLevel2.Length; i++)
                            {
                                string key = distinctLevel2[i];
                                string[] _distinctLevel3 = lstMenuValue.Where(k => k.Level2 == key).Select(k => k.Level3).Distinct().ToArray();
                                for (int j = 0; j < _distinctLevel3.Length; j++)
                                    if (ary.Contains(_distinctLevel3[j]))
                                        if (MenuAdmin.Items[0].ChildItems.Count >= distinctLevel2.Length)
                                            MenuAdmin.Items[0].ChildItems[i].ChildItems.Add(func(_distinctLevel3[j]));

                            }
                            foreach (MenuItem item in MenuAdmin.Items)
                                foreach (MenuItem item0 in item.ChildItems)
                                    foreach (MenuItem item1 in item0.ChildItems)
                                        if (item1.Value == "A004")
                                        {
                                            if (ary.Contains("A9"))
                                                item1.ChildItems.Add(func("A9"));
                                            if (ary.Contains("A10"))
                                                item1.ChildItems.Add(func("A10"));
                                        }

                        }
                        else if (role.ToLower() == "admin" || role.ToLower() == "pna" || role.ToLower() == "pna-bits" || role.ToLower() == "pna-csi")
                        {
                            string MenuAccessCode = service.GetMenuCode(role);

                            List<string> lstMenuAccessCodes = new List<string>();
                            if (MenuAccessCode.Length > 0)
                                lstMenuAccessCodes = MenuAccessCode.Split(',').Select(k => k.Trim()).Distinct(StringComparer.InvariantCultureIgnoreCase).ToList();

                            //string[] ary = { "A01", "A04", "A004", "A9" };
                            string[] ary = lstMenuAccessCodes.ToArray();

                            List<MenuAttributes> lstMenuAttrributes = new List<MenuAttributes>();
                            lstMenuAttrributes = GetMenuAttributes();

                            Func<string, MenuItem> func = k =>
                            {
                                var temp = lstMenuAttrributes.Single(k1 => k1.key == k);
                                return new MenuItem() { Text = temp.Text, NavigateUrl = temp.URL, Value = temp.key };

                            };



                            List<MenuValue> lstMenuValue = new List<MenuValue>();
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A01", Level3 = "A1", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A01", Level3 = "A2", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A01", Level3 = "A3", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A02", Level3 = "A4", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A02", Level3 = "A5", Level4 = "" });
                            //lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A03", Level3 = "A6", Level4 = "" });
                            //lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A03", Level3 = "A7", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A03", Level3 = "A8", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A004", Level4 = "A9" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A004", Level4 = "A10" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A11", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A12", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A13", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A14", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A15", Level4 = "" });
                            lstMenuValue.Add(new MenuValue() { Level1 = "A0", Level2 = "A04", Level3 = "A16", Level4 = "" });

                            lstMenuValue = lstMenuValue.Where(k => ary.Contains(k.Level1) || ary.Contains(k.Level2) || ary.Contains(k.Level3) || ary.Contains(k.Level4)).ToList();


                            string[] distinctLevel1 = lstMenuValue.Select(k => k.Level1).Distinct().ToArray();

                            foreach (string item in distinctLevel1)
                                MenuAdmin.Items.Add(func(item));

                            for (int i = 0; i < distinctLevel1.Length; i++)
                            {
                                string key = distinctLevel1[i];
                                string[] _distinctLevel2 = lstMenuValue.Where(k => k.Level1 == key).Select(k => k.Level2).Distinct().ToArray();
                                for (int j = 0; j < _distinctLevel2.Length; j++)
                                    //if (ary.Contains(_distinctLevel2[j]))
                                    // if (MenuAdmin.Items.Count > distinctLevel1.Length) 
                                    MenuAdmin.Items[i].ChildItems.Add(func(_distinctLevel2[j]));
                            }

                            string[] distinctLevel2 = lstMenuValue.Select(k => k.Level2).Distinct().ToArray();
                            for (int i = 0; i < distinctLevel2.Length; i++)
                            {
                                string key = distinctLevel2[i];
                                string[] _distinctLevel3 = lstMenuValue.Where(k => k.Level2 == key).Select(k => k.Level3).Distinct().ToArray();
                                for (int j = 0; j < _distinctLevel3.Length; j++)
                                    if (ary.Contains(_distinctLevel3[j]))
                                        if (MenuAdmin.Items[0].ChildItems.Count >= distinctLevel2.Length)
                                            MenuAdmin.Items[0].ChildItems[i].ChildItems.Add(func(_distinctLevel3[j]));

                            }
                            foreach (MenuItem item in MenuAdmin.Items)
                                foreach (MenuItem item0 in item.ChildItems)
                                    foreach (MenuItem item1 in item0.ChildItems)
                                        if (item1.Value == "A004")
                                        {
                                            if (ary.Contains("A9"))
                                                item1.ChildItems.Add(func("A9"));
                                            if (ary.Contains("A10"))
                                                item1.ChildItems.Add(func("A10"));
                                        }

                        }
                    }
                    //Menu access control 

                    string userId = Session["UserID"].ToString();
                    List<ApplnAccess> lstAccess = service.GetAccess(userId);

                    if (lstAccess.Count > 0)
                    {
                        //demand
                        //var demandAcess = lstAccess.FirstOrDefault(k => k.Appln.ToLower() == "demand");
                        //if (demandAcess != null)
                        //    //if (demandAcess.Access.ToLowerTrim() == "n")
                            //    btnExpense.OnClientClick = accessDenied;


                        //visa
                        //var visaAcess = lstAccess.FirstOrDefault(k => k.Appln.ToLower() == "visa");
                        //if (visaAcess != null)
                            //if (visaAcess.Access.ToLowerTrim() == "n")
                            //    //btnVisa.OnClientClick = accessDenied;

                        //expense
                        //var expenseAcess = lstAccess.FirstOrDefault(k => k.Appln.ToLower() == "expense");
                        //if (expenseAcess != null)
                            //if (expenseAcess.Access.ToLowerTrim() == "n")
                            //    btnExpense.OnClientClick = accessDenied;

                        //be
                        var beAcess = lstAccess.FirstOrDefault(k => k.Appln.ToLower() == "be");
                        if (beAcess != null)
                            if (beAcess.Access.ToLowerTrim() == "n")
                                btnBE.OnClientClick = accessDenied;

                        //upload
                        //var UploadAcess = lstAccess.FirstOrDefault(k => k.Appln.ToLower() == "upload");
                        //if (UploadAcess != null)
                        //    if (UploadAcess.Access.ToLowerTrim() == "n")
                        //        btnupload.OnClientClick = accessDenied;

                    }
                    //
                    //hypAdmin.Visible = false;
                    hypSwitchUser.Visible = false;
                    string isAdmin = Session["IsAdmin"] + "";
                    if (Session["Role"].ToString().ToLower() == "others")
                    {
                        lblWelcome.Text = Session["UserID"].ToString();
                    }

                    

                    if (Session["Role"].ToString().ToLower() == "anchor - r")
                    {
                        lblWelcome.Text = "( Read - Only )";
                    }
                    else
                    {
                        ////TODO : CSI change
                        //if (Session["RoleSDM"].ToString().ToLowerTrim() != "")
                        //{ lblWelcome.Text = "( " + Session["RoleSDM"] + " )"; }
                        //else
                        //    if (isAdmin.ToLowerTrim() == "y")
                        //    {
                        //        if (Session["Role"].ToString().ToLower() == "pna-bits")
                        //            lblWelcome.Text = " ( Admin - BITS )";
                        //        else if (Session["Role"].ToString().ToLower() == "pna-csi")
                        //            lblWelcome.Text = "( Admin - CSI )";
                        //        else if (Session["Role"].ToString().ToLower() == "pna-pps")
                        //            lblWelcome.Text = "( Admin - PPS )";
                        //    }
                        //    else
                                lblWelcome.Text = "( " + Session["Role"] + " )";
                    }

                    if (Session["Role"].ToString() == "Anchor")
                    {
                        lblWelcome.Text = "( Account Anchor )";
                    }

                    //string role = service.GetUserRole("sridevi_srirangan");
                    if (role.ToLower().Trim() == "admin" || isAdmin.ToLowerTrim() == "y")
                    {
                        //TODO: commented the below
                        //lnkbtnBEAdmin.Visible = true;
                        //lnkbtnBENSOAdmin.Visible = true;
                        hypSwitchUser.Visible = true;
                    }


                    //btnBE.Visible = false;
                    //btnDemand.Visible = false;
                    //btnExpense.Visible = false;
                    //btnupload.Visible = false;
                    //btnVisa.Visible = false;
                    if (role.ToLower() == "admin")
                    {
                     
                        lnkMasterCustomerDM.Visible = true;
                        lnkMasterCustomerSDM.Visible = true;
                    }
                    else{
                        lnkMasterCustomerDM.Visible = false;
                        lnkMasterCustomerSDM.Visible = false;
                }
                }

                
            }
            catch (Exception ex)
            {
                if ((ex.Message + "").Contains("Thread was being aborted."))
                    logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                else
                {
                    logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }
            }
        }
        protected void hypSignOut_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
        protected void hypAdmin_Click(object sender, EventArgs e)
        {
            Response.Redirect("BEAdmin.aspx");

        }

        protected void lnkbtnBENSOAdmin_Click(object sender, EventArgs e)
        {
            Response.Redirect("NSOAdminPage.aspx");
        }

        protected void hypSwitchUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
        protected void btnDemand_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://BLRKEC72629S:9876/Login.aspx?site=demand");
        }
        protected void btnVisa_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://BLRKEC72629S:9876/Login.aspx?site=demand");
        }
        protected void btnExpense_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://BLRKEC72629S:7777/Login.aspx?site=expense");
        }
        protected void btnBE_Click(object sender, EventArgs e)
        {
            return;
        }

        
        protected void btnupload_Click(object sender, EventArgs e)
        {
            
            //Response.Redirect("/beapp/Summary/BeReportSummary.aspx?#menu1");
                //Response.Redirect("/Summary/BeReportSummary.aspx");

            string Report = ConfigurationManager.AppSettings["Report"].ToString();
            Response.Redirect(Report);
        
        }

        protected void btnSummary_Click(object sender, EventArgs e)
        {
            string Summary = ConfigurationManager.AppSettings["Summary"].ToString();
            Response.Redirect(Summary);
            //Response.Redirect("/beapp/Summary/BeReportSummary.aspx?#home");

        }
        private List<MenuAttributes> GetMenuAttributes()
        {
            List<MenuAttributes> lstMenuAttrributes = new List<MenuAttributes>();
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A0", Text = "Admin", URL = "~/BEAdmin.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A01", Text = "Freezing and Delegation", URL = "" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A1", Text = "Application Freeze", URL = "javascript:PopUpFreeze();" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A2", Text = "Monthly Freeze", URL = "~/MasterSetting.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A3", Text = "Delegation", URL = "~/DelegatePage.aspx" });

            lstMenuAttrributes.Add(new MenuAttributes() { key = "A02", Text = "Master Data", URL = "" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A4", Text = "Client Code Portfolio", URL = "~/ClientCodePortfolioScreen0.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A5", Text = "Portfolio", URL = "~/BEPortfolioAdmin.aspx" });

            lstMenuAttrributes.Add(new MenuAttributes() { key = "A03", Text = "Exchange Rates", URL = "" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A6", Text = " Daily Conversion", URL = "~/ConvRateScreen.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A7", Text = "Monthly Conversion", URL = "~/ExhangeRateUpdate.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A8", Text = "Push Exchange Rates", URL = "~/ExchangeRates.aspx" });

            lstMenuAttrributes.Add(new MenuAttributes() { key = "A04", Text = "Maintenance", URL = "" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A004", Text = "Audit Log", URL = "" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A9", Text = "View", URL = "~/AuditLog.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A10", Text = "Delete", URL = "~/AuditLogDelete.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A11", Text = "Change DM MailId", URL = "~/ChangeDMName.aspx"});
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A12", Text = "Deletion/Updation Of Data", URL = "~/MCCDMSDMChange.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A13", Text = "Data Sync Prod-> Dev", URL = "~/DataSync.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A14", Text = "E-Mail Alert Settings", URL = "~/BEMailAlertSettings.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A15", Text = "Menu Access", URL = "~/BEAdminMenu.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A16", Text = "User", URL = "~/BEAdmin.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "", Text = "", URL = "#" });
            return lstMenuAttrributes;

        }
        public class MenuValue
        {
            public string Level1 { get; set; }
            public string Level2 { get; set; }
            public string Level3 { get; set; }
            public string Level4 { get; set; }
        }
        public class MenuAttributes
        {
            public string key { get; set; }
            public string Text { get; set; }
            public string URL { get; set; }
        }
    }
    
