
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BEData.BusinessEntity;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;




    public partial class AddMasterCustomer : BasePage
    {
        const string _MachineUserid = "MachineUserid";
        const string _Role = "Role";
        const string _UserID = "UserID";
        const string _RadioButtonSelected = "RadioButtonSelected";
        const string _Year = "Year";
        bool dummy = false;

        private BEDL service = new BEDL();
       
        
        protected void Page_Load(object sender, EventArgs e)
        {
             //if(Session.Count ==0)
             //{
             //    Response.ClearContent();
             //    Response.Write("Your Session has been expired.");
             //    divMain.Visible = false;
             //    return;
             //}
            base.ValidateSession();


            if (!IsPostBack)
            {

                //SetValueToSession(_Role, "DM");
                //SetValueToSession(_RadioButtonSelected, "0"); // DM
                //SetValueToSession(_UserID, "karthik_mahalingam01");
                //SetValueToSession(_Year, "2020-21");

                 LoadScreen();
               
                
            }           
        }
        protected string GetValueFromSession(string key)
        {
            string value = Session[key] + "";
            return value;
        }
        protected void SetValueToSession(string key, string value)
        {
            Session[key] = value;
           
        }
        protected void LoadScreen()
        {

            string MachineUserid = UserIdentity.CurrentUser;
             
            SetValueToSession(_MachineUserid, MachineUserid);
             
            string role = GetValueFromSession(_Role);  // GetValueFromSession(_Role);
            string userid = GetValueFromSession(_UserID);  // GetValueFromSession(_userid);
            int rdbValue = Convert.ToInt32(GetValueFromSession(_RadioButtonSelected));
            BindQuarter();
            BindSL(userid);
            ddlMasterCustomerCode.Items.Add("--Select--");
            ddlNativeCurrency.Items.Add("--Select--");
            ddlNSO.Items.Add("--Select--");
            if (role == "DM" || role == "SDM" )
            {  
               
                ddlNSO.Enabled = false;
                ddlMasterCustomerCode.Enabled = false;
                dummy = false;
                lblDMorSDM.Text = role;
                lbl1.Text = userid;
                ddlSDMorDM.Visible = false;

            }
            
            else if (role == "Anchor")
            {
                
                if (rdbValue == 0)
                    lblDMorSDM.Text = "DM";               
                else
                    lblDMorSDM.Text = "SDM";
               
              
                lblmsg.Visible = false;
                lbl1.Visible = false;
                
               
                ddlMasterCustomerCode.Items.Add("New Account");
                
                ddlNSO.Enabled = false;
                ddlMasterCustomerCode.Enabled = false;
                dummy = false;
                ddlSDMorDM.Enabled = false;
                ddlSDMorDM.Items.Add("--Select--");
            }
            else if (role == "Admin")
            {

                if (Request.QueryString["Type"] == "DM")
                {
                    lblDMorSDM.Visible = true;
                    lblDMorSDM.Text = "DM";
                    lbl1.Visible = false;
                }
                else if (Request.QueryString["Type"] == "SDM")
                {
                    lblDMorSDM.Visible = true;
                    lblDMorSDM.Text = "SDM";
                    lbl1.Visible = false;
                }
                else
                {
                    if (rdbValue == 0)
                    {
                        lblDMorSDM.Text = "DM";
                    }
                    else
                    {
                        lblDMorSDM.Text = "SDM";
                    }
                    //ServiceLine
                    lblmsg.Visible = false;
                    lbl1.Visible = false;
                }

              
                ddlServiceLine.Items.Clear(); 
                ddlServiceLine.Items.Add("--Select--");
               // ddlServiceLine.Items.Add("ORC");
                ddlServiceLine.Items.Add("SAP");
               // ddlServiceLine.Items.Add("ECAS");
               // ddlServiceLine.Items.Add("EAIS");
               
                
                ddlMasterCustomerCode.Items.Add("New Account");               
                ddlMasterCustomerCode.Enabled = false;
                dummy = false;
                ddlSDMorDM.Enabled = false;
                //Quarter              
                
                ddlSDMorDM.Items.Add("--Select--");
            }              
        }
        protected void BindQuarter() {
            ddlQuarter.Items.Clear();
            string currentQtr = DateUtility.GetQuarter("current");
            string nextQtr = DateUtility.GetQuarter("next");
            string nextQtrPlus1 = DateUtility.GetQuarter("next1");
            ddlQuarter.Text = currentQtr;

            ddlQuarter.Items.Insert(0, currentQtr);
            ddlQuarter.Items.Insert(1, nextQtr);
            ddlQuarter.Items.Insert(2, nextQtrPlus1);

            ddlQuarter.Text = currentQtr;
        }
        protected void BindSL(string userid) {
            ddlServiceLine.Items.Clear();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string query = " select Distinct txtServiceLine from BEUserAccess_NSO where txtUserId='" + userid + "'";
            ds = new DataSet();
            ds = service.GetDataSet(query);
            dt = ds.Tables[0]; 
            ddlServiceLine.Items.Add("--Select--"); 
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlServiceLine.Items.Add(dt.Rows[i]["txtServiceLine"].ToString());
            }
        }



        string Yes;
        string js = "";
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string Role = GetValueFromSession(_Role); 
            //string userid = HttpContext.Current.User.Identity.Name;
            ////string userid = "Gopinathreddy_p";
            //string[] userids = userid.Split('\\');
            //if (userids.Length == 2)
            //{
            //    userid = userids[1];
            //}
            if (Role == "DM" || Role == "SDM")
            {
                if (ddlServiceLine.SelectedItem.Text == "--Select--" || ddlNSO.SelectedItem.Text == "--Select--" || ddlMasterCustomerCode.SelectedItem.Text == "--Select--" || ddlNativeCurrency.SelectedItem.Text == "--Select--")
                {
                    lblmsg.Visible = true;
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Please select all the mandatory fields before adding";
                }
                else
                {
                    if (Role == "DM")
                    {
                        string ServiceLine = ddlServiceLine.SelectedItem.Text;
                        string nso = ddlNSO.SelectedItem.Text;
                        string MCC = ddlMasterCustomerCode.SelectedItem.Text;
                        string NC = ddlNativeCurrency.SelectedItem.Text;
                        string Quarter = ddlQuarter.Text.Remove(2).Trim();
                        string FYqtr = ddlQuarter.Text.Remove(0, 3);
                        string SDMorDM = GetValueFromSession(_UserID); 
                        string Fyear = GetValueFromSession(_Year);
                        string UserIdMachine = GetValueFromSession(_MachineUserid);
                        int rdbValue = Convert.ToInt32(GetValueFromSession(_RadioButtonSelected));

                        int result = service.AddMasterCustomer(ServiceLine, MCC, nso, NC, Quarter, Fyear, SDMorDM, Role, rdbValue, UserIdMachine);
                        if (result == -1)
                        {
                            lblmsg.Visible = true;
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Records already present";
                        }
                        else
                        {
                            Yes = "AddYes";
                        lblmsg.Visible = true;
                        lblmsg.ForeColor = Color.Green;
                        lblmsg.Text = "Records Saved..";
                        //string url = "DMView.aspx?Yes=" + Yes + "&qtr=" + Quarter + "&Year=" + FYqtr;

                        //    js += "window.opener.location.href='" + url + "';";
                        //    js += "window.close();";

                        //    ClientScript.RegisterStartupScript(this.GetType(), "redirect", js, true);
                        }
                    }
                    else if (Role == "SDM")
                    {
                        string ServiceLine = ddlServiceLine.SelectedItem.Text;
                        string PU = ddlNSO.SelectedItem.Text;
                        string MCC = ddlMasterCustomerCode.SelectedItem.Text;
                        string NC = ddlNativeCurrency.SelectedItem.Text;
                        string Quarter = ddlQuarter.Text.Remove(2).Trim();
                        string SDMorDM = GetValueFromSession(_UserID);
                        string Fyear = GetValueFromSession(_Year);
                        int rdbValue = Convert.ToInt32(GetValueFromSession(_RadioButtonSelected));
                        string FYqtr = ddlQuarter.Text.Remove(0, 3);
                        string UserIdMachine = GetValueFromSession(_MachineUserid);
                        int result = service.AddMasterCustomer(ServiceLine, MCC,PU, NC, Quarter, Fyear, SDMorDM, Role, rdbValue, UserIdMachine);
                        if (result == -1)
                        {
                            lblmsg.Visible = true;
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Records already present";
                        }
                        else
                        {
                            //Yes = "AddYes";

                            lblmsg.Visible = true;
                            lblmsg.ForeColor = Color.Green;
                            lblmsg.Text = "Records saved";

                            //string url = "SDMView.aspx?Yes=" + Yes + "&qtr=" + Quarter +"&Year=" + FYqtr;

                            //js += "window.opener.location.href='" + url + "';";
                            //js += "window.close();";

                            //ClientScript.RegisterStartupScript(this.GetType(), "redirect", js, true);
                        }
                    }
                }
                
            
            }
            else if (Role == "Anchor")
            {
                if (ddlServiceLine.SelectedItem.Text == "--Select--" || ddlNSO.SelectedItem.Text == "--Select--" || ddlMasterCustomerCode.SelectedItem.Text == "--Select--" || ddlNativeCurrency.SelectedItem.Text == "--Select--" || ddlSDMorDM.SelectedItem.Text == "--Select--")
               {
                lblmsg.Visible = true;
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Please select all the mandatory fields before adding";
               }
                else if (Role == "Anchor")
                {
                    string ServiceLine = ddlServiceLine.SelectedItem.Text;
                    string PU = ddlNSO.SelectedItem.Text;
                    string MCC = ddlMasterCustomerCode.SelectedItem.Text;
                    string NC = ddlNativeCurrency.SelectedItem.Text;
                    string Quarter = ddlQuarter.Text.Remove(2).Trim();
                    string SDMorDM = ddlSDMorDM.SelectedItem.Text;
                    string FYqtr = ddlQuarter.Text.Remove(0,3);
                    string Fyear = GetValueFromSession(_Year);
                    int rdbValue = Convert.ToInt32(GetValueFromSession(_RadioButtonSelected));
                    string UserIdMachine = GetValueFromSession(_MachineUserid);

                    int result = service.AddMasterCustomer(ServiceLine, MCC, PU, NC, Quarter, Fyear, SDMorDM, Role, rdbValue, UserIdMachine); ;
                    if (result == -1)
                    {
                        //Alert("Records Already Present");
                        lblmsg.Visible = true;
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "Records already present";
                    }
                    else
                    {
                        //Alert("Records Have Been Added");
                        if (rdbValue == 1)
                        {
                        //  Yes = "AddYes";


                        //  string url = "SDMView.aspx?Yes=" + Yes + "&qtr=" + Quarter + "&Year=" + FYqtr;
                        //// string finalurl= url.Replace("'", "\'");
                        //  js += "window.opener.location.href='" + url + "';";
                        //  js += "window.close();";

                        //  ClientScript.RegisterStartupScript(this.GetType(), "redirect", js, true);

                        lblmsg.Visible = true;
                        lblmsg.ForeColor = Color.Green;
                        lblmsg.Text = "Records saved";

                    }
                        else if (rdbValue == 0)
                        {
                        // Yes = "AddYes";


                        // string url = "DMView.aspx?Yes=" + Yes + "&qtr=" + Quarter + "&Year=" + FYqtr;
                        //// string finalurl = url.Replace("'", "\'");
                        // js += "window.opener.location.href='" + url + "';";

                        // js += "window.close();";

                        // ClientScript.RegisterStartupScript(this.GetType(), "redirect", js, true);

                        lblmsg.Visible = true;
                        lblmsg.ForeColor = Color.Green;
                        lblmsg.Text = "Records saved";
                    }

                    }
                }
            }
            else if (Role == "Admin")
            {
                if (ddlServiceLine.SelectedItem.Text == "--Select--" || ddlNSO.SelectedItem.Text == "--Select--" || ddlMasterCustomerCode.SelectedItem.Text == "--Select--" || ddlNativeCurrency.SelectedItem.Text == "--Select--" || ddlSDMorDM.SelectedItem.Text == "--Select--")
                {
                    lblmsg.Visible = true;
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Please select all the mandatory fields before adding";
                }
                else
                {
                    if (Role == "Admin")
                    {
                        string ServiceLine = ddlServiceLine.SelectedItem.Text;
                        string PU = ddlNSO.SelectedItem.Text;
                        string MCC = ddlMasterCustomerCode.SelectedItem.Text;
                        string NC = ddlNativeCurrency.SelectedItem.Text;
                        string Quarter = ddlQuarter.Text.Remove(2).Trim();
                        string SDMorDM = ddlSDMorDM.SelectedItem.Text;
                        string FYqtr = ddlQuarter.Text.Remove(0, 3);

                        string Fyear = "";
                        int rdbValue =0;
                        string UserIdMachine = GetValueFromSession(_MachineUserid);
                        if (Request.QueryString["Type"] != null)
                        {
                            int tempyear = Convert.ToInt32(ddlQuarter.Text.Remove(0, 3)) + 2000 - 1;
                            Fyear = string.Format("{0}-{1}", tempyear, (tempyear - 2000 + 1));

                            if (Request.QueryString["Type"] == "DM")
                            {
                                rdbValue=0;
                            }
                            else if (Request.QueryString["Type"] == "SDM")
                            {
                                rdbValue=1;
                            }
                        }
                        else {
                           Fyear = GetValueFromSession(_Year);
                            rdbValue = Convert.ToInt32(GetValueFromSession(_RadioButtonSelected));
                        }
                        
                        int result = service.AddMasterCustomer(ServiceLine, MCC,PU, NC, Quarter, Fyear, SDMorDM, Role, rdbValue, UserIdMachine);
                        if (result == -1)
                        {
                            //Alert("Records Already Present");
                            lblmsg.Visible = true;
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Records already present";
                        }
                        else
                        {
                            
                                lblmsg.Visible = true;
                                lblmsg.ForeColor = Color.Green;
                                lblmsg.Text = "Records Saved..";
                            

                        }
                    }
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {            
            string Role = GetValueFromSession(_Role);
            if (Role == "DM")
            {
             string url="DMView.aspx";
             js += "window.opener.location.href='" + url + "';";
             js += "window.close();";

             ClientScript.RegisterStartupScript(this.GetType(), "redirect", js, true);
            }
            else if (Role == "SDM")
            {
                string url ="SDMView.aspx";
                js += "window.opener.location.href='" + url + "';";
                js += "window.close();";

                ClientScript.RegisterStartupScript(this.GetType(), "redirect", js, true);
            }
            else if (Role == "Anchor")
            {
                int rdbValue = Convert.ToInt32(GetValueFromSession(_RadioButtonSelected));
                if (rdbValue == 0)
                {
                   string url ="DMView.aspx";
                   js += "window.opener.location.href='" + url + "';";
                   js += "window.close();";

                   ClientScript.RegisterStartupScript(this.GetType(), "redirect", js, true);
                }
                else
                {
                    string url = "SDMView.aspx";
                    js += "window.opener.location.href='" + url + "';";
                    js += "window.close();";

                    ClientScript.RegisterStartupScript(this.GetType(), "redirect", js, true);
                }
            }
            else if (Role == "Admin")
            {
                int rdbValue = Convert.ToInt32(GetValueFromSession(_RadioButtonSelected));
                if (rdbValue == 0)
                {
                    string url = "DMView.aspx";
                    js += "window.opener.location.href='" + url + "';";
                    js += "window.close();";

                    ClientScript.RegisterStartupScript(this.GetType(), "redirect", js, true);
                }
                else
                {
                    string url = "SDMView.aspx";
                    js += "window.opener.location.href='" + url + "';";
                    js += "window.close();";

                    ClientScript.RegisterStartupScript(this.GetType(), "redirect", js, true);
                }
            }
        }

        private void Alert(string message)
        {

            try
            {
                Page page = HttpContext.Current.CurrentHandler as Page;

                // string script = string.Format("alert('{0}');", message);

                if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
                {

                    // page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);

                    page.RegisterClientScriptBlock("alert", "<script type=\"text/javascript\">alert('" + message + "');</script>");

                }
            }
            catch (Exception ex)
            {

                //if ((ex.Message + "").Contains("Thread was being aborted."))
                //    logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                //else
                //{
                //    logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                //}
            }

        }


        private void LoadNSO()
        {
            ddlNSO.Items.Clear();
            ddlNSO.Enabled = true;
            ddlNSO.Items.Clear();
            ddlNSO.Items.Add("--Select--");
            DataTable dt1 = new DataTable();
            string UserId = GetValueFromSession(_UserID);
            dt1 = service.Get_NSO(UserId, ddlMasterCustomerCode.SelectedItem.Text, ddlServiceLine.SelectedItem.Text);
            ddlNSO.DataSource = dt1;
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                ddlNSO.Items.Add(dt1.Rows[i][0].ToString());
            }
        }

        protected void ddlMasterCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
        {
        LoadNSO();
        return;

            //NativeCurrency();

            lblmsg.Text = "";

            ddlNativeCurrency.Items.Add("--Select--");
            ddlSDMorDM.Items.Add("--Select--");

            string role = GetValueFromSession(_Role);
            string userid = GetValueFromSession(_UserID);
            int rdbValue = Convert.ToInt32(GetValueFromSession(_RadioButtonSelected));
            if (ddlMasterCustomerCode.SelectedItem.Text == "--Select--")
            { 
                if (role == "DM" || role == "SDM")
                {
                    lbl1.Enabled = true;
                }
                else
                {
                    ddlSDMorDM.Enabled = false;
                }
            }
            else
            {
              
                ddlSDMorDM.Enabled = false;
                 
            }
        }

        protected void ddlNSO_SelectedIndexChanged(object sender, EventArgs e)
        {
            BIND_SDM_DM();
        }


        private void NativeCurrency()
        {
            string mcc = ddlMasterCustomerCode.SelectedItem.Text;
            string[] userids = mcc.Split('_');
            if (userids.Length >= 2)
            {
                mcc = userids[0];
            }

            lblmsg.Text = "";
            dummy = true;
           
            //if (ddlMasterCustomerCode.SelectedItem.Value == "--Select--")
            //{
            //    if (Role == "Anchor")
            //    {
            //        //ddlSDMorDM.Items.Clear();
            //        //ddlSDMorDM.Items.Add("--Select--");
            //        ddlSDMorDM.SelectedIndex = 0;
            //        ddlSDMorDM.Enabled = false;


            //        ddlNativeCurrency.Items.Clear();
            //        ddlNativeCurrency.Items.Add("--Select--");
            //        dummy = false;
            //    }
            //    else
            //    {
            //        ddlNativeCurrency.Items.Clear();
            //        ddlNativeCurrency.Items.Add("--Select--");
            //        dummy = false;
            //    }
            //}
            //else
            //{
                dummy = true;
                string sl = ddlServiceLine.SelectedItem.Text;
                string _Mcc = ddlMasterCustomerCode.SelectedItem.Text;
                //string nso = ddlNSO.SelectedItem.Text;
                string format = " select distinct  [Currency Code] from AlconPBS  ";
                //string query = string.Format(format, sl, _Mcc, nso);
                ddlNativeCurrency.Items.Clear();
                ddlNativeCurrency.Items.Add("--Select--");
              
                DataSet dsNC = service.GetDataSet(format);
                DataTable dtNC = dsNC.Tables[0];
                for (int i = 0; i < dtNC.Rows.Count; i++) 
                    ddlNativeCurrency.Items.Add(dtNC.Rows[i][0].ToString());

            
        }

        private void BIND_SDM_DM()
        {
            string Role = GetValueFromSession(_Role);
            if (Role == "Anchor")
            {
                int rdbValue = Convert.ToInt32(GetValueFromSession(_RadioButtonSelected));
                if (rdbValue == 0)
                {
                    ddlSDMorDM.Items.Clear();
                    ddlSDMorDM.Items.Add("--Select--");
                    DataSet ds4 = new DataSet();
                    DataTable dt4 = new DataTable();
                    string GetDM = "select distinct  txtDMMailId  from EAS_BEData_DM_NSO where txtDMMailId <> '' and ISNUMERIC( txtDMMailId ) = 0 ";

                    ds4 = service.GetDataSet(GetDM);
                    dt4 = ds4.Tables[0];
                    ddlSDMorDM.DataSource = dt4;
                    for (int i = 0; i < dt4.Rows.Count; i++)
                    {
                        ddlSDMorDM.Items.Add(dt4.Rows[i]["txtDMMailId"].ToString());
                    }
                    lblDMorSDM.Visible = true;
                    lblDMorSDM.Text = "DM";
                    lbl1.Visible = false;

                }
                else
                {
                    ddlSDMorDM.Items.Clear();
                    ddlSDMorDM.Items.Add("--Select--");
                    DataSet ds4 = new DataSet();
                    DataTable dt4 = new DataTable();
                    string GetSDM;

                    GetSDM = "select distinct   txtSDMMailId  from DigitalOfferingSDMMapping where txtOfferingCode='" + ddlNSO.SelectedItem.Text + "'  ";
                    //}
                    ds4 = service.GetDataSet(GetSDM);
                    dt4 = ds4.Tables[0];
                    ddlSDMorDM.DataSource = dt4;
                    for (int i = 0; i < dt4.Rows.Count; i++)
                    {
                        ddlSDMorDM.Items.Add(dt4.Rows[i]["txtSDMMailId"].ToString());
                    }
                    lblDMorSDM.Visible = true;
                    lblDMorSDM.Text = "SDM";
                    lbl1.Visible = false;
                }
            }

            else if (Role == "Admin")
            {
                int rdbValue = Convert.ToInt32(GetValueFromSession(_RadioButtonSelected));

                if (Request.QueryString["Type"] == "DM")
                {
                    BindDM();
                }
                else if (Request.QueryString["Type"] == "SDM")
                {
                    BindSDM();
                }
                else
                {
                    if (rdbValue == 0)
                    {
                        BindDM();
                    }
                    else
                    {
                        BindSDM();
                    }
                }

                //}
            }
        }

        private void BindSDM()
        {
            ddlSDMorDM.Items.Clear();
            ddlSDMorDM.Items.Add("--Select--");
            DataSet ds4 = new DataSet();
            DataTable dt4 = new DataTable();
            string querySDM;
            querySDM = "select  txtSDMMailId  from DigitalOfferingSDMMapping where txtOfferingCode = '" + ddlNSO.SelectedItem.Text + "'";
            //}
            ds4 = service.GetDataSet(querySDM);
            dt4 = ds4.Tables[0];
            ddlSDMorDM.DataSource = dt4;
            for (int i = 0; i < dt4.Rows.Count; i++)
            {
                ddlSDMorDM.Items.Add(dt4.Rows[i]["txtSDMMailId"].ToString());
            }
            lblDMorSDM.Visible = true;
            lblDMorSDM.Text = "SDM";
            lbl1.Visible = false;
        }

        private void BindDM()
        {
            ddlSDMorDM.Items.Clear();
            ddlSDMorDM.Items.Add("--Select--");
            string sl = ddlServiceLine.SelectedItem.Text;
            DataSet ds4 = new DataSet();
            DataTable dt4 = new DataTable();
            string GetDM = "select distinct txtDMMailId from EAS_BEData_DM_NSO where txtDMMailId <>'' and txtServiceLine='"+sl+"'";

            ds4 = service.GetDataSet(GetDM);
            dt4 = ds4.Tables[0];
            ddlSDMorDM.DataSource = dt4;
            for (int i = 0; i < dt4.Rows.Count; i++)
            {
                ddlSDMorDM.Items.Add(dt4.Rows[i]["txtDMMailId"].ToString());
            }
            lblDMorSDM.Visible = true;
            lblDMorSDM.Text = "DM";
            lbl1.Visible = false;
        }

        protected void ddlServiceLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblmsg.Text = "";
            ddlMasterCustomerCode.Items.Clear();
            ddlNativeCurrency.Items.Clear();
            ddlNSO.Items.Clear();
            ddlSDMorDM.Items.Clear();
            ddlMasterCustomerCode.Items.Add("--Select--"); 
            ddlNativeCurrency.Items.Add("--Select--");
            ddlSDMorDM.Items.Add("--Select--");
            ddlNSO.Items.Add("--Select--");

            string role = GetValueFromSession(_Role);
            string userid = GetValueFromSession(_UserID);
            int rdbValue = Convert.ToInt32(GetValueFromSession(_RadioButtonSelected));
            if (ddlServiceLine.SelectedItem.Text == "--Select--")
            {
                ddlMasterCustomerCode.Enabled = false;
                dummy = false;
                if (role == "DM" || role == "SDM")
                {
                    lbl1.Enabled = true;
                }
                else
                {
                    ddlSDMorDM.Enabled = false;
                }
                ddlNSO.Enabled = false;
            }
            else
            {
                ddlNSO.Enabled = true;
                ddlMasterCustomerCode.Enabled = true;
                ddlSDMorDM.Enabled = true;
                if (role == "Anchor")
                {

                    ddlMasterCustomerCode.Items.Clear();
                    ddlMasterCustomerCode.Items.Add("--Select--");
                    ddlSDMorDM.Enabled = false;

                    DataTable dt1 = new DataTable();
                    string UserId = GetValueFromSession(_UserID);
                    dt1 = service.GetMCC(UserId);

                    ddlMasterCustomerCode.DataSource = dt1;
                List<string> lstMCCTemp = dt1.Rows.OfType<DataRow>().Select(k => k["CustCode"].ToString()).ToList().Distinct().ToList();

                //for (int i = 0; i < dt1.Rows.Count; i++)
                //{
                //    ddlMasterCustomerCode.Items.Add(dt1.Rows[i]["CustCode"].ToString());
                //}
                for (int i = 0; i < lstMCCTemp.Count; i++)
                {
                    ddlMasterCustomerCode.Items.Add(lstMCCTemp[i]);
                }
            }
                if (role == "SDM" || role == "DM" || role == "Admin")
                {
                    ddlMasterCustomerCode.Items.Clear();
                    ddlMasterCustomerCode.Items.Add("--Select--");                    
                    DataSet ds1 = new DataSet();
                    DataTable dt1 = new DataTable();
                    string query = "select distinct txtMasterClientCode from DemClientCodePortfolio where txtServiceline='" + ddlServiceLine.SelectedItem.Text + "' and IsActive='Y' ";
                    string adj = role == "Admin" ? "and txtMasterClientCode not like '%_Adj'" : "";
                    query = query + adj;
                    ds1 = new DataSet();
                    ds1 = service.GetDataSet(query);
                    dt1 = ds1.Tables[0];
                    ddlMasterCustomerCode.DataSource = dt1;
                    for (int i = 0; i < dt1.Rows.Count; i++) 
                        ddlMasterCustomerCode.Items.Add(dt1.Rows[i]["txtMasterClientCode"].ToString());
                    
                }
            }
            NativeCurrency();
            //LoadNSO();
        }

        // PU 
        protected void ddlNativeCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlNativeCurrency.SelectedItem.Text == "--Select--")
            {
                ddlSDMorDM.SelectedIndex = 0;
                ddlSDMorDM.Enabled = false;
                lblmsg.Text = "";
            }
            else {
                ddlSDMorDM.SelectedIndex = 0;
                ddlSDMorDM.Enabled = true;
                lblmsg.Text = "";
            }
        }

        protected void ddlQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblmsg.Text = "";
        }

        protected void ddlSDMorDM_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblmsg.Text = "";
        }
    }
