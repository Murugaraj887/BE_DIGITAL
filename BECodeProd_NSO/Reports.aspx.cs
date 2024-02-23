using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Microsoft.SqlServer.Dts.Runtime;
using System.Data.OleDb;
//using Office = Microsoft.Office.Core;
using VBIDE = Microsoft.Vbe.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using BEData;
using System.Globalization;
using System.Web.UI.HtmlControls;



    public partial class Reports : BasePage
    {
        private BEDL service = new BEDL();
        Logger logger = new Logger();
        public string fileName = "Reports";
        int mon = DateTime.Now.Month;
        string curqtr = string.Empty;
        public DateTime dateTime = DateTime.Today;

        class Bullets { public string Name { get; set; } public string Code { get; set; } public string Value { get; set; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();
            if (Request.QueryString["flag"] != "N")
            {
                HtmlGenericControl sitemap = (HtmlGenericControl)Master.FindControl("SiteMap1");
                sitemap.Attributes.Add("style", "display:none");
            }
            if (Page.IsPostBack)
            { }
            else
            {
                //string userid = HttpContext.Current.User.Identity.Name;
                //// userid = "karthik_mahalingam01";
                //string[] userids = userid.Split('\\');
                //if (userids.Length == 2)
                //{
                //    userid = userids[1];
                //}
                //
                //
                string machineUser = string.Empty;
                string[] machineUsers = HttpContext.Current.User.Identity.Name.Split('\\');
                if (machineUsers.Length == 2)
                    machineUser = machineUsers[1];
                string machineRole = service.GetUserRole(machineUser); //Machine User
                string machineIsAdmin = service.GetIsAdmin(machineUser); //Machine User
                Session["MachineRole"] = machineRole;
                Session["MachineUser"] = machineUser;
                string loginUser = Session["UserID"] + ""; //selected item
                string loginRole = Session["Role"] + ""; //Selected item's role
                string loginIsAdmin = service.GetIsAdmin(loginUser);//selected item's is admin status

                if (machineRole.ToLower().Trim() == loginRole.ToLower().Trim())
                {
                    if ((machineRole.ToLower().Trim() == "pna-bits" || machineRole.ToLower().Trim() == "pna-csi" || machineRole.ToLower().Trim() == "pna-pps") && loginIsAdmin.ToLowerTrim() == "y")
                    {
                        MenuAdmin.Visible = false;
                    }
                    else if (machineRole.ToLower().Trim() == "admin")
                    {
                        MenuAdmin.Visible = false;
                    }
                }
                else
                {
                    if ((loginRole.ToLower().Trim() == "pna-bits" || loginRole.ToLower().Trim() == "pna-csi" || loginRole.ToLower().Trim() == "pna-pps") && loginIsAdmin.ToLowerTrim() == "y")
                    {
                        MenuAdmin.Visible = false;
                    }
                    else if (machineRole.ToLower().Trim() == "admin")
                    {
                        MenuAdmin.Visible = false;
                    }
                    else if ((machineRole.ToLower().Trim() == "pna-bits" || machineRole.ToLower().Trim() == "pna-csi" || machineRole.ToLower().Trim() == "pna-pps") && machineIsAdmin.ToLowerTrim() == "y")
                    {
                        MenuAdmin.Visible = false;
                    }
                }
                if (machineRole.ToLower().Trim() == loginRole.ToLower().Trim())
                {
                    if ((machineRole.ToLower().Trim() == "pna-bits" || machineRole.ToLower().Trim() == "pna-csi" || machineRole.ToLower().Trim() == "pna-pps") && loginIsAdmin.ToLowerTrim() == "y")
                    {
                        string MenuAccessCode = service.GetMenuCode(machineRole);

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
                    else if (machineRole.ToLower().Trim() == "admin")
                    {
                        string MenuAccessCode = service.GetMenuCode(machineRole);

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
                }
                else
                {
                    if ((loginRole.ToLower().Trim() == "pna-bits" || loginRole.ToLower().Trim() == "pna-csi" || loginRole.ToLower().Trim() == "pna-pps") && loginIsAdmin.ToLowerTrim() == "y")
                    {
                        string MenuAccessCode = service.GetMenuCode(loginRole);

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
                    else if (machineRole.ToLower().Trim() == "admin")
                    {
                        string MenuAccessCode = service.GetMenuCode(machineRole);

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
                    else if ((machineRole.ToLower().Trim() == "pna-bits" || machineRole.ToLower().Trim() == "pna-csi" || machineRole.ToLower().Trim() == "pna-pps") && machineIsAdmin.ToLowerTrim() == "y")
                    {
                        string MenuAccessCode = service.GetMenuCode(machineRole);

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
                }
                //
                //


                //onload
                string isValidEntry = Session["Login"] + "";
                if (!isValidEntry.Equals("1"))
                    Response.Redirect("UnAuthorised.aspx");

                string userid = Session["UserID"] + "";
                string role = Session["Role"] + "";
                string roleanchor = Session["RoleForAnchor"] + "";

                string reportAccessCode = service.GetReportCode(userid);
                List<string> lstReportAccessCodes = new List<string>();
                if (reportAccessCode.Length > 0)
                    lstReportAccessCodes = reportAccessCode.Split(',').Select(k => k.Trim()).Distinct(StringComparer.InvariantCultureIgnoreCase).ToList();

                List<Bullets> lstBEReports = new List<Bullets>();

                DataTable dtRevenue = service.FetchReports(userid, "Revenue");
                for (int i = 0; i < dtRevenue.Rows.Count; i++)
                {
                    string txtcode = dtRevenue.Rows[i][0].ToString();
                    string txtname = dtRevenue.Rows[i][1].ToString();
                    string txtvalue = dtRevenue.Rows[i][2].ToString();
                    lstBEReports.Add(new Bullets() { Code = txtcode, Name = txtname, Value = txtvalue });
                }

                


                List<Bullets> lstVolumereports = new List<Bullets>();
                DataTable dtVolume = service.FetchReports(userid, "Volume");
                for (int i = 0; i < dtVolume.Rows.Count; i++)
                {
                    string txtcode = dtVolume.Rows[i][0].ToString();
                    string txtname = dtVolume.Rows[i][1].ToString();
                    string txtvalue = dtVolume.Rows[i][2].ToString();
                    lstVolumereports.Add(new Bullets() { Code = txtcode, Name = txtname, Value = txtvalue });
                }


               

                bulletRevenue.DataSource = lstBEReports;
                bulletRevenue.DataTextField = "Name";
                bulletRevenue.DataValueField = "Value";
                bulletRevenue.DataBind();

                int count = bulletRevenue.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    ListItem currentItem = bulletRevenue.Items[i] as ListItem;

                    //if(currentItem.Text=="SDM BE Report")
                    string text = currentItem.Value.ToString();
                    //string action="CallPopUp("+"'"+text+"'"+"); return false;";
                    string action = "modal(this,'" + text + "'); return false;";
                    bulletRevenue.Items[i].Attributes["onclick"] = action;


                  



                }


                //if (role.ToLowerTrim() == "admin")
                //bulletReport.Items[2].Attributes["onclick"] = "PopUpFinRTBR(); return false;";
                //else
                //bulletReport.Items[1].Attributes["onclick"] = "PopUpFinRTBR(); return false;";
                bulletVolume.DataSource = lstVolumereports;
                bulletVolume.DataTextField = "Name";
                bulletVolume.DataValueField = "Value";
                bulletVolume.DataBind();

                int countvolume = bulletVolume.Items.Count;

                for (int i = 0; i < countvolume; i++)
                {
                    ListItem currentItem = bulletVolume.Items[i] as ListItem;
                    string text = currentItem.Value;
                    //string action = "CallPopUp(" + "'" + text + "'" + "); return false;";
                    string action = "modal(this,'" + text + "'); return false;";
                    bulletVolume.Items[i].Attributes["onclick"] = action;

                   



                }

             
                List<Bullets> lstAdmin = new List<Bullets>();
                if (Session["MachineRole"].ToString() == "Admin" || Session["MachineRole"].ToString() == "PnA" || Session["MachineRole"].ToString() == "UH")
                {
                DataTable dtAdmin = service.FetchReports(userid, "Admin");
                for (int i = 0; i < dtAdmin.Rows.Count; i++)
                {
                    string txtcode = dtAdmin.Rows[i][0].ToString();
                    string txtname = dtAdmin.Rows[i][1].ToString();
                    string txtvalue = dtAdmin.Rows[i][2].ToString();
                 
                    lstAdmin.Add(new Bullets() { Code = txtcode, Name = txtname, Value = txtvalue });
                }
                }
               

                bulletAdmin.DataSource = lstAdmin;
                bulletAdmin.DataTextField = "Name";
                bulletAdmin.DataValueField = "Value";
                bulletAdmin.DataBind();

                int countadminreport = bulletAdmin.Items.Count;
                for (int i = 0; i < countadminreport; i++)
                {

                    ListItem currentItem = bulletAdmin.Items[i] as ListItem;
                    string text = currentItem.Value;
                    //string action = "CallPopUp(" + "'" + text + "'" + "); return false;";
                    string action = "modal(this,'" + text + "'); return false;";
                    bulletAdmin.Items[i].Attributes["onclick"] = action;

                  
          

                }


                

               


                List<Bullets> lstMisc = new List<Bullets>();
                DataTable dtMisc = service.FetchReports(userid, "Miscellaneous");
                for (int i = 0; i < dtMisc.Rows.Count; i++)
                {
                    string txtcode = dtMisc.Rows[i][0].ToString();
                    string txtname = dtMisc.Rows[i][1].ToString();
                    string txtvalue = dtMisc.Rows[i][2].ToString();
                    lstMisc.Add(new Bullets() { Code = txtcode, Name = txtname, Value = txtvalue });
                }
             
                bulletMisc.DataSource = lstMisc;
                bulletMisc.DataTextField = "Name";
                bulletMisc.DataValueField = "Value";
                bulletMisc.DataBind();



                int countmiscreport = bulletMisc.Items.Count;
                for (int i = 0; i < countmiscreport; i++)
                {
                    ListItem currentItem = bulletMisc.Items[i] as ListItem;
                    string text = currentItem.Value;
                    //string action = "CallPopUp(" + "'" + text + "'" + "); return false;";
                    string action = "modal(this,'" + text + "'); return false;";
                    bulletMisc.Items[i].Attributes["onclick"] = action;
     

                }

                //AllParametersReport();
                if (bulletAdmin.Items.Count == 0)
                    tblAdmin.Visible = false;
                if (bulletRevenue.Items.Count == 0)
                    tblReport.Visible = false;
                if (bulletVolume.Items.Count == 0)
                    tblVariance.Visible = false;
                if (bulletMisc.Items.Count == 0)
                    tblMisc.Visible = false;


                bool adminMiscVisibilty = new string[] { "admin", "pna", "uh" }.Contains(loginRole.ToLower().Trim());
                tblAdmin.Visible = adminMiscVisibilty;
                tblMisc.Visible = adminMiscVisibilty;
            }

        

        }

      

        protected void btnCCP_Click(object sender, EventArgs e)
        {
            Response.Redirect("ClientCodePortfolioDump.aspx");
            //try
            //{
            //    string userid = Session["UserID"] + "";
            //    var tblCCPDownload = service.DownloadCCP(userid);


            //    string folder = "ExcelOperations";
            //    var MyDir = new DirectoryInfo(Server.MapPath(folder));


            //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "ClientCodePortfolioDump.xlsx") != null)
            //        System.IO.File.Delete(MyDir.FullName + "\\ClientCodePortfolioDump.xlsx");


            //    //using (StreamWriter sw = new StreamWriter(MyDir.FullName + "\\Revenue_Volume_BE_Dump.xls"))
            //    //{
            //    //    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            //    //    {
            //    //        grid1.RenderControl(hw);
            //    //        //Response.Write(sw.ToString());
            //    //        ////Response.End(); 
            //    //    }
            //    //}



            //    FileInfo file = new FileInfo(MyDir.FullName + "\\ClientCodePortfolioDump.xlsx");

            //    ExcelPackage pck = new ExcelPackage();

            //    //Create the worksheet
            //    // ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Revenue_Volume_BE_Dump");


            //    ExcelWorksheet ws;
            //    ExcelWorksheet ws1;

            //    int rowcount = tblCCPDownload.Rows.Count;
            //    int colcount = tblCCPDownload.Columns.Count;

            //    //Create the worksheet
            //    // if (tableBEREV.Rows.Count > 0)
            //    {
            //        ws = pck.Workbook.Worksheets.Add("ClientCodePortfolioDump");
            //        ws.Cells["A1"].LoadFromDataTable(tblCCPDownload, true);
            //        //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
            //        var fill = ws.Cells[1, 1, 1, colcount].Style.Fill;
            //        fill.PatternType = ExcelFillStyle.Solid;
            //        fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
            //        ws.Cells[1, 1, 1, colcount].Style.Font.Bold = true;
            //        ws.Cells[1, 1, rowcount, colcount].AutoFitColumns();
            //        //ws.Cells[
            //    }


            //    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
            //    //  ws.Cells["A1"].LoadFromDataTable(tableBEREV, true);
            //    pck.SaveAs(file);


            //    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    //Response.AddHeader("content-disposition", "attachment;  filename=DineshReport.xlsx");
            //    //Response.BinaryWrite(pck.GetAsByteArray());
            //    pck.Dispose();
            //    ws = null;
            //    pck = null;

            //    DownloadFile();
            //}

            //catch (Exception ex)
            //{

            //    if ((ex.Message + "").Contains("Thread was being aborted."))

            //        logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            //    else
            //    {
            //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            //        throw ex;
            //    }
            //}
        }


        //private void DownloadFile()
        //{
        //    Excel.Application oExcel;
        //    Excel.Workbook oBook = default(Excel.Workbook);
        //    VBIDE.VBComponent oModule;
        //    try
        //    {
        //        bool forceDownload = true;
        //        //string path = MapPath(fname);
        //        string folder = "ExcelOperations";
        //        var MyDir = new DirectoryInfo(Server.MapPath(folder));


        //        String sCode;
        //        Object oMissing = System.Reflection.Missing.Value;

        //        //Create an instance of Excel.
        //        oExcel = new Excel.Application();


        //        oBook = oExcel.Workbooks.
        //            Open(MyDir.FullName + "\\ClientCodePortfolioDump.xlsx", 0, false, 5, "", "", true,
        //            Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);



        //        //if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "DineshReport1.xlsx") != null)
        //        //    System.IO.File.Delete(MyDir.FullName + "\\DineshReport1.xlsx");

        //        //oBook.SaveCopyAs(MyDir.FullName + "\\DineshReport.xlsx");

        //        //if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "DECSBITSUtilOutput.xls") == null)
        //        //    System.IO.File.Delete(MyDir.FullName + "\\DECSBITSUtilOutput.xls");



        //        oBook.Save();


        //        oBook.Close();
        //        oExcel.Quit();
        //        oExcel = null;
        //        oModule = null;
        //        oBook = null;

        //        GC.Collect();




        //        string path = MyDir.FullName + "\\ClientCodePortfolioDump.xlsx";
        //        //string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
        //        string name = "ClientCodePortfolioDump" + ".xlsx";
        //        string ext = Path.GetExtension(path);
        //        string type = "";

        //        // set known types based on file extension  
        //        //if (ext != null)
        //        //{
        //        //    switch (ext.ToLower())
        //        //    {
        //        //        case ".htm":
        //        //        case ".html":
        //        //            type = "text/HTML";
        //        //            break;

        //        //        case ".txt":
        //        //            type = "text/plain";
        //        //            break;

        //        //        case ".csv":
        //        //        case ".xls":
        //        //            type = "application/vnd.ms-excel";
        //        //            break;
        //        //        case ".xlsx":
        //        //            type = "application/vnd.ms-excel.12";
        //        //            break;
        //        //    }
        //        //}

        //        //if (forceDownload)
        //        //{
        //        //    Response.AppendHeader("content-disposition",  "attachment; filename=" + name);
        //        //}
        //        //if (type != "")
        //        //    Response.ContentType = type;

        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;  filename=ClientCodePortfolioDump.xlsx");

        //        Response.WriteFile(path);

        //        Response.Flush();
        //        Response.End();

        //    }
        //    catch (Exception ex)
        //    {
        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //        {
        //            oModule = null;
        //            oBook = null;
        //            oExcel = null;
        //            GC.Collect();
        //            logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        }
        //        else
        //        {
        //            oModule = null;
        //            oBook = null;
        //            oExcel = null;
        //            GC.Collect();
        //            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}

        //code section for user details

        protected void btnUserDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserDetails.aspx");
            //try
            //{
            //    string userid = Session["UserID"] + "";

            //    //var tblUserDownload0 = service.DownloadUserDetails(userid).Tables[0];
            //    //var tblUserDownload1 = service.DownloadUserDetails(userid).Tables[1];


            //    string cmdtext = "EXEC dbo.SP_UserDetails '" + userid + "'";
            //    DataSet ds = new DataSet();
            //    ds = service.GetDataSet(cmdtext);
            //    DataTable dt0 = new DataTable();
            //    DataTable dt1 = new DataTable();
            //    dt0 = ds.Tables[0];
            //    dt1 = ds.Tables[1];

            //    var tblUserDownload0 = dt0;
            //    var tblUserDownload1 = dt1;


            //    string folder = "ExcelOperations";
            //    var MyDir = new DirectoryInfo(Server.MapPath(folder));


            //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "User Details.xlsx") != null)
            //        System.IO.File.Delete(MyDir.FullName + "\\User Details.xlsx");

            //    FileInfo file = new FileInfo(MyDir.FullName + "\\User Details.xlsx");

            //    ExcelPackage pck = new ExcelPackage();

            //    ExcelWorksheet ws;
            //    ExcelWorksheet ws1;

            //    int rowcountSheet0 = tblUserDownload0.Rows.Count;
            //    int colcountSheet0 = tblUserDownload0.Columns.Count;

            //    //Create the worksheet
            //    // if (tableBEREV.Rows.Count > 0)
            //    {
            //        ws = pck.Workbook.Worksheets.Add("User List");
            //        ws.Cells["A1"].LoadFromDataTable(tblUserDownload0, true);
            //        //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
            //        var fill = ws.Cells[1, 1, 1, colcountSheet0].Style.Fill;
            //        fill.PatternType = ExcelFillStyle.Solid;
            //        fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
            //        ws.Cells[1, 1, 1, colcountSheet0].Style.Font.Bold = true;
            //        ws.Cells[1, 1, rowcountSheet0, colcountSheet0].AutoFitColumns();
            //        //ws.Cells[
            //    }


            //    int rowcountSheet1 = tblUserDownload1.Rows.Count;
            //    int colcountSheet1 = tblUserDownload1.Columns.Count;

            //    //Create the worksheet
            //    // if (tableBEREV.Rows.Count > 0)
            //    {
            //        ws1 = pck.Workbook.Worksheets.Add("Anchor Access Details");
            //        ws1.Cells["A1"].LoadFromDataTable(tblUserDownload1, true);
            //        //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
            //        var fill = ws1.Cells[1, 1, 1, colcountSheet1].Style.Fill;
            //        fill.PatternType = ExcelFillStyle.Solid;
            //        fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
            //        ws1.Cells[1, 1, 1, colcountSheet1].Style.Font.Bold = true;
            //        ws1.Cells[1, 1, rowcountSheet1, colcountSheet1].AutoFitColumns();
            //        //ws.Cells[
            //    }

            //    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
            //    //  ws.Cells["A1"].LoadFromDataTable(tableBEREV, true);
            //    pck.SaveAs(file);


            //    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    //Response.AddHeader("content-disposition", "attachment;  filename=DineshReport.xlsx");
            //    //Response.BinaryWrite(pck.GetAsByteArray());
            //    pck.Dispose();
            //    ws = null;
            //    pck = null;

            //    DownloadFileUser();
            //}

            //catch (Exception ex)
            //{

            //    if ((ex.Message + "").Contains("Thread was being aborted."))

            //        logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            //    else
            //    {
            //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            //        throw ex;
            //    }
            //}
        }

        //code for user details ends

        //download user details

        //private void DownloadFileUser()
        //{
        //    Excel.Application oExcel;
        //    Excel.Workbook oBook = default(Excel.Workbook);
        //    VBIDE.VBComponent oModule;
        //    try
        //    {
        //        bool forceDownload = true;
        //        //string path = MapPath(fname);
        //        string folder = "ExcelOperations";
        //        var MyDir = new DirectoryInfo(Server.MapPath(folder));


        //        String sCode;
        //        Object oMissing = System.Reflection.Missing.Value;

        //        //Create an instance of Excel.
        //        oExcel = new Excel.Application();


        //        oBook = oExcel.Workbooks.
        //            Open(MyDir.FullName + "\\User Details.xlsx", 0, false, 5, "", "", true,
        //            Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);


        //        oBook.Save();


        //        oBook.Close();
        //        oExcel.Quit();
        //        oExcel = null;
        //        oModule = null;
        //        oBook = null;

        //        GC.Collect();




        //        string path = MyDir.FullName + "\\User Details.xlsx";         
        //        string name = "User Details" + ".xlsx";
        //        string ext = Path.GetExtension(path);
        //        string type = "";

        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;  filename=User Details.xlsx");

        //        Response.WriteFile(path);

        //        Response.Flush();
        //        Response.End();

        //    }
        //    catch (Exception ex)
        //    {
        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //        {
        //            oModule = null;
        //            oBook = null;
        //            oExcel = null;
        //            GC.Collect();
        //            logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        }
        //        else
        //        {
        //            oModule = null;
        //            oBook = null;
        //            oExcel = null;
        //            GC.Collect();
        //            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}

        //download user dtetails code ends


        //code for btnNewProjectList

        protected void btnNewProjectList_Click(object sender, EventArgs e)
        {
            //string monthName = "August";
            //int yy = 2015;

            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            var mn =datevalue.Month;
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mn);
            String yy = datevalue.Year.ToString();

            try
            {
                string userid = Session["UserID"] + "";

                string cmdtext = "EXEC dbo.EAS_NewProject_list '" + monthName + "','" + yy + "'";
                DataSet ds = new DataSet();
                ds = service.GetDataSet(cmdtext);
                DataTable dt0 = new DataTable();

                dt0 = ds.Tables[0];


                var tblProjectDownload0 = dt0;



                string folder = "ExcelOperations";
                var MyDir = new DirectoryInfo(Server.MapPath(folder));


                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "NewProjectDetails.xlsx") != null)
                    System.IO.File.Delete(MyDir.FullName + "\\NewProjectDetails.xlsx");

                FileInfo file = new FileInfo(MyDir.FullName + "\\NewProjectDetails.xlsx");

                ExcelPackage pck = new ExcelPackage();

                ExcelWorksheet ws;


                int rowcountSheet0 = tblProjectDownload0.Rows.Count;
                int colcountSheet0 = tblProjectDownload0.Columns.Count;

            //    //Create the worksheet
            //    // if (tableBEREV.Rows.Count > 0)
                {
                    ws = pck.Workbook.Worksheets.Add("Project List");
                    ws.Cells["A1"].LoadFromDataTable(tblProjectDownload0, true);
                    var fill = ws.Cells[1, 1, 1, colcountSheet0].Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    ws.Cells[1, 1, 1, colcountSheet0].Style.Font.Bold = true;
                    ws.Cells[1, 1, rowcountSheet0, colcountSheet0].AutoFitColumns();
                }
                
                pck.SaveAs(file);

                pck.Dispose();
                ws = null;
                pck = null;

                DownloadFileProject();
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

        private void DownloadFileProject()
        {
            Excel.Application oExcel;
            Excel.Workbook oBook = default(Excel.Workbook);
            VBIDE.VBComponent oModule;
            try
            {
                bool forceDownload = true;
                //string path = MapPath(fname);
                string folder = "ExcelOperations";
                var MyDir = new DirectoryInfo(Server.MapPath(folder));


                String sCode;
                Object oMissing = System.Reflection.Missing.Value;

                //Create an instance of Excel.
                oExcel = new Excel.Application();


                oBook = oExcel.Workbooks.
                    Open(MyDir.FullName + "\\NewProjectDetails.xlsx", 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);


                oBook.Save();


                oBook.Close();
                oExcel.Quit();
                oExcel = null;
                oModule = null;
                oBook = null;

                GC.Collect();




                string path = MyDir.FullName + "\\NewProjectDetails.xlsx";
                string name = "NewProjectDetails" + ".xlsx";
                string ext = Path.GetExtension(path);
                string type = "";

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=NewProjectDetails.xlsx");

                Response.WriteFile(path);

                Response.Flush();
                Response.End();

            }
            catch (Exception ex)
            {
                if ((ex.Message + "").Contains("Thread was being aborted."))
                {
                    oModule = null;
                    oBook = null;
                    oExcel = null;
                    GC.Collect();
                    logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }
                else
                {
                    oModule = null;
                    oBook = null;
                    oExcel = null;
                    GC.Collect();
                    logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }
            }
        }

        //end for btnNewProjectList
        
        private List<MenuAttributes> GetMenuAttributes()
        {
            List<MenuAttributes> lstMenuAttrributes = new List<MenuAttributes>();
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A0", Text = "Admin", URL = "" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A01", Text = "Freezing and Delegation", URL = "" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A1", Text = "Application Freeze", URL = "javascript:PopUpFreeze();" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A2", Text = "Monthly Freeze", URL = "~/MasterSetting.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A3", Text = "Delegation", URL = "~/DelegatePage.aspx" });

            lstMenuAttrributes.Add(new MenuAttributes() { key = "A02", Text = "Master Data", URL = "" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A4", Text = "Client Code Portfolio", URL = "~/ClientCodePortfolioScreen0.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A5", Text = "Portfolio", URL = "~/BEPortfolioAdmin.aspx" });

            lstMenuAttrributes.Add(new MenuAttributes() { key = "A03", Text = "Exchange Rates", URL = "" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A6", Text = " Daily Conversion", URL = "~/ConvRateScreen.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A7", Text = "Monthly Conversion", URL = "~/GuidanceAndActuals.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A8", Text = "Push Exchange Rates", URL = "~/ExchangeRate.aspx" });

            lstMenuAttrributes.Add(new MenuAttributes() { key = "A04", Text = "Maintenance", URL = "" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A004", Text = "Audit Log", URL = "" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A9", Text = "View", URL = "~/AuditLog.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A10", Text = "Delete", URL = "~/AuditLogDelete.aspx" });
            lstMenuAttrributes.Add(new MenuAttributes() { key = "A11", Text = "Change DM MailID", URL = "~/ChangeDMName.aspx" });
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

        //protected void RevenueMomemtum_Click1(object sender, EventArgs e)
        //{
        //    int year = DateTime.Today.Year;
        //    DateTime todaydate = dateTime;
        //    string strcurrent = "";                  
        //    if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)
        //        strcurrent = "Q4";
        //    else if (todaydate.Month == 4 || todaydate.Month == 5 || todaydate.Month == 6)
        //        strcurrent = "Q1";
        //    else if (todaydate.Month == 7 || todaydate.Month == 8 || todaydate.Month == 9)
        //        strcurrent = "Q2";
        //    else
        //        strcurrent = "Q3";

        //    string MachineUser = Session["MachineUser"].ToString();
        //    string MachineRole = Session["MachineRole"].ToString();
        //    int currentYear = dateTime.Year; //DateTime.Now.Year;           
        //    string yr = Convert.ToString(currentYear) + '-' + ((currentYear - 2000) + 1).ToString();
        //    string currentQuarter = strcurrent;
        //    string cmdtext = "select txtServiceLine from BEUserAccess where txtUserId='" + MachineUser + "'";
        //    DataSet ds = new DataSet();
        //    ds = service.GetDataSet(cmdtext);
        //    DataTable dt = new DataTable();
        //    dt = ds.Tables[0];
        //    try
        //    {
        //        var qtr = currentQuarter;
        //        var CurrYear = yr;
        //        var userid = MachineUser;
        //        DataSet dsORC = new DataSet();
        //        DataSet dsSAP = new DataSet();
        //        DataTable dt1ORC = new DataTable();
        //        DataTable dt2ORC = new DataTable();
        //        DataTable dt1SAP = new DataTable();
        //        DataTable dt2SAP = new DataTable();
        //        DataSet dsEAS = new DataSet();
        //        DataTable dtEAS = new DataTable();

        //        var tblComparisonReport1 = dt1ORC;
        //        var tblComparisonReport2 = dt2ORC;
        //        var tblComparisonReport3 = dt2SAP;
        //        var tblComparisonReport4 = dtEAS;
        //        var tblComparisonReport5 = dt1SAP;

        //        if (dt.Rows[0][0].ToString() == "All" && MachineRole == "Admin")
        //        {
        //            dsORC = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, "ORC", "RevenueMomentum");
        //            dsSAP = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, "SAP", "RevenueMomentum");
        //            dsEAS = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, "ORC", "RevenueMomentum");
        //            dt1ORC = dsORC.Tables[0];
        //            dt2ORC = dsORC.Tables[1];
        //            dt1SAP = dsSAP.Tables[0];
        //            dt2SAP = dsSAP.Tables[1];
        //            dt1ORC.Merge(dt1SAP);
        //            dtEAS = dsEAS.Tables[1];

                   
        //            for (int i = 0; i < dtEAS.Rows.Count; i++)
        //            {
        //                //for (int j = 0; j < dt2SAP.Rows.Count; j++)
        //                //{
        //                //    if (dt2ORC.Rows[i][0].ToString() == dt2SAP.Rows[j][0].ToString())
        //                //    {
        //                dtEAS.Rows[i][1] =Convert.ToString(Convert.ToDouble(dt2ORC.Rows[i][1].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][1].ToString()));
        //                dtEAS.Rows[i][2] = Convert.ToDouble(dt2ORC.Rows[i][2].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][2].ToString());
        //                dtEAS.Rows[i][3] = Convert.ToDouble(dt2ORC.Rows[i][3].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][3].ToString());
        //                dtEAS.Rows[i][4] = Convert.ToDouble(dt2ORC.Rows[i][4].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][4].ToString());
        //                dtEAS.Rows[i][7] = Convert.ToDouble(dt2ORC.Rows[i][7].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][7].ToString());
        //                dtEAS.Rows[i][8] = Convert.ToDouble(dt2ORC.Rows[i][8].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][8].ToString());
        //                dtEAS.Rows[i][9] = Convert.ToDouble(dt2ORC.Rows[i][9].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][9].ToString());
        //                dtEAS.Rows[i][10] = Convert.ToDouble(dt2ORC.Rows[i][10].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][10].ToString());
        //                dtEAS.Rows[i][13] = Convert.ToDouble(dt2ORC.Rows[i][13].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][13].ToString());
        //                dtEAS.Rows[i][14] = Convert.ToDouble(dt2ORC.Rows[i][14].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][14].ToString());
        //                dtEAS.Rows[i][15] = Convert.ToDouble(dt2ORC.Rows[i][15].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][15].ToString());
        //                dtEAS.Rows[i][16] = Convert.ToDouble(dt2ORC.Rows[i][16].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][16].ToString());

        //                dtEAS.Rows[i][19] = Convert.ToDouble(dt2ORC.Rows[i][19].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][19].ToString());
        //                dtEAS.Rows[i][20] = Convert.ToDouble(dt2ORC.Rows[i][20].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][20].ToString());
        //                dtEAS.Rows[i][21] = Convert.ToDouble(dt2ORC.Rows[i][21].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][21].ToString());
        //                dtEAS.Rows[i][22] = Convert.ToDouble(dt2ORC.Rows[i][22].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][22].ToString());

        //                dtEAS.Rows[i][25] = Convert.ToDouble(dt2ORC.Rows[i][25].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][25].ToString());
        //                dtEAS.Rows[i][26] = Convert.ToDouble(dt2ORC.Rows[i][26].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][26].ToString());
        //                dtEAS.Rows[i][27] = Convert.ToDouble(dt2ORC.Rows[i][27].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][27].ToString());
        //                dtEAS.Rows[i][28] = Convert.ToDouble(dt2ORC.Rows[i][28].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][28].ToString());

        //                dtEAS.Rows[i][31] = Convert.ToDouble(dt2ORC.Rows[i][31].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][31].ToString());
        //                dtEAS.Rows[i][32] = Convert.ToDouble(dt2ORC.Rows[i][32].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][32].ToString());
        //                dtEAS.Rows[i][33] = Convert.ToDouble(dt2ORC.Rows[i][33].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][33].ToString());
        //                dtEAS.Rows[i][34] = Convert.ToDouble(dt2ORC.Rows[i][34].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][34].ToString());
        //                //}
        //                //else
        //                //{
        //                //    Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('You are in else');</script>");
        //                //    return;
        //                //}
        //                //}
        //            }

        //            tblComparisonReport1 = dt1ORC;
        //            tblComparisonReport2 = dt2ORC;
        //            tblComparisonReport3 = dt2SAP;
        //            tblComparisonReport4 = dtEAS;

        //            if (tblComparisonReport1 == null || tblComparisonReport1.Rows.Count == 0 || tblComparisonReport2 == null || tblComparisonReport2.Rows.Count == 0 || tblComparisonReport3 == null || tblComparisonReport3.Rows.Count == 0 || tblComparisonReport4 == null || tblComparisonReport4.Rows.Count == 0)
        //            {
        //                Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
        //                return;
        //            }
        //        }
        //        else if (dt.Rows[0][0].ToString() == "All" && MachineRole == "UH")
        //        {
        //            dsORC = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, "ORC", "RevenueMomentum");
        //            dsSAP = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, "SAP", "RevenueMomentum");
        //            dsEAS = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, "ORC", "RevenueMomentum");
        //            dt1ORC = dsORC.Tables[0];
        //            dt2ORC = dsORC.Tables[1];
        //            dt1SAP = dsSAP.Tables[0];
        //            dt2SAP = dsSAP.Tables[1];
        //            dt1ORC.Merge(dt1SAP);
        //            dtEAS = dsEAS.Tables[1];

        //            for (int i = 0; i < dtEAS.Rows.Count; i++)
        //            {
        //                //for (int j = 0; j < dt2SAP.Rows.Count; j++)
        //                //{
        //                //    if (dt2ORC.Rows[i][0].ToString() == dt2SAP.Rows[j][0].ToString())
        //                //    {
                        
        //                dtEAS.Rows[i][1] = Convert.ToDouble(dt2ORC.Rows[i][1].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][1].ToString());
        //                dtEAS.Rows[i][2] = Convert.ToDouble(dt2ORC.Rows[i][2].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][2].ToString());
        //                dtEAS.Rows[i][3] = Convert.ToDouble(dt2ORC.Rows[i][3].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][3].ToString());
        //                dtEAS.Rows[i][4] = Convert.ToDouble(dt2ORC.Rows[i][4].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][4].ToString());
        //                dtEAS.Rows[i][7] = Convert.ToDouble(dt2ORC.Rows[i][7].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][7].ToString());
        //                dtEAS.Rows[i][8] = Convert.ToDouble(dt2ORC.Rows[i][8].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][8].ToString());
        //                dtEAS.Rows[i][9] = Convert.ToDouble(dt2ORC.Rows[i][9].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][9].ToString());
        //                dtEAS.Rows[i][10] = Convert.ToDouble(dt2ORC.Rows[i][10].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][10].ToString());
        //                dtEAS.Rows[i][13] = Convert.ToDouble(dt2ORC.Rows[i][13].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][13].ToString());
        //                dtEAS.Rows[i][14] = Convert.ToDouble(dt2ORC.Rows[i][14].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][14].ToString());
        //                dtEAS.Rows[i][15] = Convert.ToDouble(dt2ORC.Rows[i][15].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][15].ToString());
        //                dtEAS.Rows[i][16] = Convert.ToDouble(dt2ORC.Rows[i][16].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][16].ToString());

        //                dtEAS.Rows[i][19] = Convert.ToDouble(dt2ORC.Rows[i][19].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][19].ToString());
        //                dtEAS.Rows[i][20] = Convert.ToDouble(dt2ORC.Rows[i][20].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][20].ToString());
        //                dtEAS.Rows[i][21] = Convert.ToDouble(dt2ORC.Rows[i][21].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][21].ToString());
        //                dtEAS.Rows[i][22] = Convert.ToDouble(dt2ORC.Rows[i][22].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][22].ToString());

        //                dtEAS.Rows[i][25] = Convert.ToDouble(dt2ORC.Rows[i][25].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][25].ToString());
        //                dtEAS.Rows[i][26] = Convert.ToDouble(dt2ORC.Rows[i][26].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][26].ToString());
        //                dtEAS.Rows[i][27] = Convert.ToDouble(dt2ORC.Rows[i][27].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][27].ToString());
        //                dtEAS.Rows[i][28] = Convert.ToDouble(dt2ORC.Rows[i][28].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][28].ToString());

        //                dtEAS.Rows[i][31] = Convert.ToDouble(dt2ORC.Rows[i][31].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][31].ToString());
        //                dtEAS.Rows[i][32] = Convert.ToDouble(dt2ORC.Rows[i][32].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][32].ToString());
        //                dtEAS.Rows[i][33] = Convert.ToDouble(dt2ORC.Rows[i][33].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][33].ToString());
        //                dtEAS.Rows[i][34] = Convert.ToDouble(dt2ORC.Rows[i][34].ToString()) + Convert.ToDouble(dt2SAP.Rows[i][34].ToString());


        //                //}
        //                //else
        //                //{
        //                //    Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('You are in else');</script>");
        //                //    return;
        //                //}
        //                //}
        //            }
        //            tblComparisonReport1 = dt1ORC;
        //            tblComparisonReport2 = dt2ORC;
        //            tblComparisonReport3 = dt2SAP;
        //            tblComparisonReport4 = dtEAS;
        //            if (tblComparisonReport1 == null || tblComparisonReport1.Rows.Count == 0 || tblComparisonReport2 == null || tblComparisonReport2.Rows.Count == 0 || tblComparisonReport3 == null || tblComparisonReport3.Rows.Count == 0 || tblComparisonReport4 == null || tblComparisonReport4.Rows.Count == 0)
        //            {
        //                Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
        //                return;
        //            }
        //        }
        //        else if (dt.Rows[0][0].ToString() == "ORC" && MachineRole == "PnA")
        //        {
        //            dsORC = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, "ORC", "RevenueMomentum");
        //            dt1ORC = dsORC.Tables[0];
        //            dt2ORC = dsORC.Tables[1];
        //            tblComparisonReport1 = dt1ORC;
        //            tblComparisonReport2 = dt2ORC;
        //            if (tblComparisonReport1 == null || tblComparisonReport1.Rows.Count == 0 || tblComparisonReport2 == null || tblComparisonReport2.Rows.Count == 0)
        //            {
        //                Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
        //                return;
        //            }
        //        }
        //        else if (dt.Rows[0][0].ToString() == "SAP" && MachineRole == "PnA")
        //        {
        //            dsSAP = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, "SAP", "RevenueMomentum");
        //            dt1SAP = dsSAP.Tables[0];
        //            dt2SAP = dsSAP.Tables[1];
        //            tblComparisonReport5 = dt1SAP;
        //            tblComparisonReport3 = dt2SAP;
        //            if (tblComparisonReport3 == null || tblComparisonReport3.Rows.Count == 0 || tblComparisonReport5 == null || tblComparisonReport5.Rows.Count == 0)
        //            {
        //                Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('You are not authorized to download the report!');</script>");
        //            return;
        //        }


        //        string folder = "ExcelOperations";
        //        var MyDir = new DirectoryInfo(Server.MapPath(folder));
        //        string fileName = "RevenueMomentum_" + Session["UserId"] + "";
        //        Session["fileNameRev"] = fileName;
        //        FileInfo file = new FileInfo(MyDir.FullName + "\\" + fileName + ".xlsx");
        //        if (MyDir.GetFiles().SingleOrDefault(k => k.Name == (fileName + ".xlsx")) != null)
        //            System.IO.File.Delete(MyDir.FullName + "\\" + fileName + ".xlsx");
        //        Session["FullfileNameRev"] = file;
        //        ExcelPackage pck = new ExcelPackage();
        //        ExcelWorksheet ws;
        //        ExcelWorksheet ws1;
        //        ExcelWorksheet ws2;
        //        ExcelWorksheet ws3;
                
        //        string sht = "BE_Data";
        //        string sht1 = "ORC";
        //        string sht2 = "SAP";
        //        string sht3 = "EAS";
        //        int row = tblComparisonReport1.Rows.Count;
        //        int col = tblComparisonReport1.Columns.Count;
        //        int row1 = tblComparisonReport2.Rows.Count;
        //        int col1 = tblComparisonReport2.Columns.Count;
        //        int row2 = tblComparisonReport3.Rows.Count;
        //        int col2 = tblComparisonReport3.Columns.Count;
        //        int row3 = tblComparisonReport4.Rows.Count;
        //        int col3 = tblComparisonReport4.Columns.Count;
                
        //        {                
        //                ws = pck.Workbook.Worksheets.Add(sht);
        //                ws.Cells["A1"].LoadFromDataTable(tblComparisonReport1, true);
        //                //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
        //                var fill = ws.Cells[1, 1, 1, col].Style.Fill;
        //                fill.PatternType = ExcelFillStyle.Solid;
        //                fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
        //                ws.Cells[1, 1, 1, col].Style.Font.Bold = true;
        //                ws.Cells[1, 1, row, col].AutoFitColumns();
                                     
        //                ws1 = pck.Workbook.Worksheets.Add(sht1);
        //                ws1.Cells["A1"].LoadFromDataTable(tblComparisonReport2, true);
        //                //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
        //                var fill1 = ws1.Cells[1, 1, 1, col1].Style.Fill;
        //                fill1.PatternType = ExcelFillStyle.Solid;
        //                fill1.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
        //                ws1.Cells[1, 1, 1, col1].Style.Font.Bold = true;
        //                ws1.Cells[1, 1, row1, col1].AutoFitColumns();
                    

        //            ws2 = pck.Workbook.Worksheets.Add(sht2);
        //            ws2.Cells["A1"].LoadFromDataTable(tblComparisonReport3, true);
        //            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
        //            var fill2 = ws2.Cells[1, 1, 1, col2].Style.Fill;
        //            fill2.PatternType = ExcelFillStyle.Solid;
        //            fill2.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
        //            ws2.Cells[1, 1, 1, col2].Style.Font.Bold = true;
        //            ws2.Cells[1, 1, row2, col2].AutoFitColumns();

        //            ws3 = pck.Workbook.Worksheets.Add(sht3);
        //            ws3.Cells["A1"].LoadFromDataTable(tblComparisonReport4, true);
        //            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
        //            var fill3 = ws2.Cells[1, 1, 1, col3].Style.Fill;
        //            fill3.PatternType = ExcelFillStyle.Solid;
        //            fill3.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
        //            ws3.Cells[1, 1, 1, col3].Style.Font.Bold = true;
        //            ws3.Cells[1, 1, row3, col3].AutoFitColumns();

                    
        //        }
        //        pck.SaveAs(file);
        //        pck.Dispose();
        //        ws = null;
        //        ws1 = null;
        //        ws2 = null;
        //        ws3 = null;
        //        pck = null;

        //        DownloadFileBEReport();
        //      //  hdnfldFlag.Value = "1";
        //    }

        //    catch (Exception ex)
        //    {

        //        if ((ex.Message + "").Contains("Thread was being aborted."))

        //            logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}

        private void DownloadFileBEReport()
        {
            Excel.Application oExcel;
            Excel.Workbook oBook = default(Excel.Workbook);
            VBIDE.VBComponent oModule;
            try
            {
                bool forceDownload = true;
                //string path = MapPath(fname);
                string folder = "ExcelOperations";
                var MyDir = new DirectoryInfo(Server.MapPath(folder));


                String sCode;
                Object oMissing = System.Reflection.Missing.Value;

                //Create an instance of Excel.
                oExcel = new Excel.Application();


                oBook = oExcel.Workbooks.
                    Open(Session["FullfileNameRev"].ToString(), 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);


                sCode = "sub RevenueMomentumMacro()\r\n" +
                    System.IO.File.ReadAllText(MyDir.FullName + "\\RevenueMomentumMacro.txt") +
                        "\nend sub";

                oModule.CodeModule.AddFromString(sCode);

                oExcel.GetType().InvokeMember("Run",
                                System.Reflection.BindingFlags.Default |
                                System.Reflection.BindingFlags.InvokeMethod,
                                null, oExcel, new string[] { "RevenueMomentumMacro" });

                 string finalname = Session["fileNameRev"] + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
               // string finalname = "RevenueMomentum_rupali03_07Aug2015_1052.xlsx";
                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == finalname) != null)
                    System.IO.File.Delete(MyDir.FullName + "\\" + finalname);

                oBook.SaveCopyAs(MyDir.FullName + "\\" + finalname);


                oBook.Save();


                oBook.Close();
                oExcel.Quit();
                oExcel = null;
                oModule = null;
                oBook = null;

                GC.Collect();

                string year = Convert.ToString(dateTime.Year);

                string path = MyDir.FullName + "\\" + finalname;
                // string name = "RupaliExel_Test.xlsx";
                ////string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
                //if (ddlSU.Text == "ALL")
                //{
                //    name = "ECS" + "_" + ddlQtr.Text + year.Substring(2, 2) + "_" + "BEReport" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
                //}
                //else
                //{
                //    name = ddlSU.Text + "_" + ddlQtr.Text + year.Substring(2, 2) + "_" + "BEReport" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
                //}
                string ext = Path.GetExtension(path);
                string type = "";

                //string path = MyDir.FullName + "\\BEReport.xlsx";
                ////string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
                //string name = "BEReport" + ".xlsx";
                //string ext = Path.GetExtension(path);
                //string type = "";

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AppendHeader("content-disposition", "attachment;  filename=" + finalname);

                Response.WriteFile(path);

                Response.Flush();
                Response.End();


                if (ext != null)
                {
                    switch (ext.ToLower())
                    {
                        case ".htm":
                        case ".html":
                            type = "text/HTML";
                            break;

                        case ".txt":
                            type = "text/plain";
                            break;



                        case ".csv":
                        case ".xls":
                        case ".xlsx":
                            type = "Application/x-msexcel";
                            break;
                    }
                }
                if (forceDownload)
                {
                    Response.AppendHeader("content-disposition",
                        "attachment; filename=" + finalname);
                }
                if (type != "")
                    Response.ContentType = type;
                Response.WriteFile(path);
                Response.End();
               // loading.Visible = false;
              
            }

            catch (Exception ex)
            {
                if ((ex.Message + "").Contains("Thread was being aborted."))
                {
                    oModule = null;
                    oBook = null;
                    oExcel = null;
                    GC.Collect();
                    logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }
                else
                {
                    oModule = null;
                    oBook = null;
                    oExcel = null;
                    GC.Collect();
                    logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }
            }
        }




        //private void DownloadFileBEReport(string FileName)
        //{
        //    Excel.Application oExcel;
        //    Excel.Workbook oBook = default(Excel.Workbook);
         
        //    VBIDE.VBComponent oModule;
        //    try
        //    {
        //        bool forceDownload = true;
        //        string folder = "ExcelOperations";
        //        var MyDir = new DirectoryInfo(Server.MapPath(folder));
        //        String sCode;
        //        Object oMissing = System.Reflection.Missing.Value;
        //        oExcel = new Excel.Application();
        //        oBook = oExcel.Workbooks.
        //            Open(FileName, 0, false, 5, "", "", true,
        //            Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
        //        oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);
        //        sCode = "sub Macro1()\r\n" +
        //            System.IO.File.ReadAllText(MyDir.FullName + "\\Macro1.txt") +
        //                "\nend sub";
        //        oModule.CodeModule.AddFromString(sCode);
        //        oExcel.GetType().InvokeMember("Run",
        //                        System.Reflection.BindingFlags.Default |
        //                        System.Reflection.BindingFlags.InvokeMethod,
        //                        null, oExcel, new string[] { "Macro1" });

        //        string finalname = Session["fileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";

              
        //        if (MyDir.GetFiles().SingleOrDefault(k => k.Name == finalname) != null)
        //            System.IO.File.Delete(MyDir.FullName + "\\" + finalname);
        //        oBook.SaveCopyAs(MyDir.FullName + "\\" + finalname);
        //        oBook.Save();
        //        oBook.Close();
        //        oExcel.Quit();
        //        oExcel = null;
        //        oModule = null;
        //        oBook = null;
        //        GC.Collect();
       
        //        //string year = Convert.ToString(dateTime.Year);
               
                
        //        //string fileName = "RevenueMomentum_" + Session["UserId"] + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
        //        //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        //Response.AppendHeader("content-disposition", "attachment;  filename=" + finalname);
        //        //Response.WriteFile(path);
        //        //Response.Flush();
        //        //Response.End();
        //        //if (ext != null)
        //        //{
        //        //    switch (ext.ToLower())
        //        //    {
        //        //        case ".htm":
        //        //        case ".html":
        //        //            type = "text/HTML";
        //        //            break;
        //        //        case ".txt":
        //        //            type = "text/plain";
        //        //            break;
        //        //        case ".csv":
        //        //        case ".xls":
        //        //        case ".xlsx":
        //        //            type = "Application/x-msexcel";
        //        //            break;
        //        //    }
        //        //}
        //        //if (forceDownload)
        //        //{
        //        //    Response.AppendHeader("content-disposition",
        //        //        "attachment; filename=" + fileName);
        //        //}
        //        //if (type != "")
        //        //    Response.ContentType = type;
        //        //Response.WriteFile(path);
        //        //Response.End();
        //        //loading.Visible = false;
        //         finalname = "RevenueMomentum_rupali03_07Aug2015_1052.xlsx";
        //        string path = MyDir.FullName + "\\" + finalname;
        //                     //string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
              
        //      string  ext = Path.GetExtension(path);
        //      string type = "";


        //       Response.ContentType = "Application/x-msexcel";
        //        Response.AppendHeader("content-disposition", "attachment;  filename=" + finalname);

        //        Response.WriteFile(path);

        //        Response.Flush();
        //        Response.End();
        //        if (ext != null)
        //        {
        //            switch (ext.ToLower())
        //            {
        //                case ".htm":
        //                case ".html":
        //                    type = "text/HTML";
        //                    break;

        //                case ".txt":
        //                    type = "text/plain";
        //                    break;



        //                case ".csv":
        //                case ".xls":
        //                case ".xlsx":
        //                    type = "Application/x-msexcel";
        //                    break;
        //            }
        //        }
        //        if (forceDownload)
        //        {
        //            Response.AppendHeader("content-disposition",
        //                "attachment; filename=" + finalname);
        //        }
        //        if (type != "")
        //            Response.ContentType = type;
        //        Response.WriteFile(path);
        //        Response.End();
        //       // loading.Visible = false;
        //    }

        //    catch (Exception ex)
        //    {
        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //        {
        //            oModule = null;
        //            oBook = null;
        //            oExcel = null;
        //            GC.Collect();
        //            logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        }
        //        else
        //        {
        //            oModule = null;
        //            oBook = null;
        //            oExcel = null;
        //            GC.Collect();
        //            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}

    }
