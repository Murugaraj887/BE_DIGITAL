using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BEData.BusinessEntity;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using BEData;

    public partial class BEAdmin : BasePage
    {

        private BEDL service = new BEDL();
        Logger logger = new Logger();
        string fileName = "BEData.BEAdmin";
        string[] reportCodes;
        string[] SUTypeIsReadOnly;
        string[] SUTypeIsReadOnlyMachineUserId;
        protected void Page_Load(object sender, EventArgs e)
        {
            //TODO:Check if needed
            base.ValidateSession();

            try
            {
                int puIndex = (Request.QueryString["Offering"] + "").Length == 0 ? 0 : Convert.ToInt32(Request.QueryString["Offering"] + "");
                int ccIndex = (Request.QueryString["cc"] + "").Length == 0 ? 0 : Convert.ToInt32(Request.QueryString["cc"] + "");

                
                if (!Page.IsPostBack)
                {
                    lstReportList.Visible = true;
                    lstReporttobeAdded.Visible = true;
                    //string isValidEntry = Session["Login"] + "";
                    //if (!isValidEntry.Equals("1"))
                    //Response.Redirect("UnAuthorised.aspx");

                    string MachineUserid = UserIdentity.CurrentUser;
                    Session["UserID"] = MachineUserid;

                    string userid = Session["UserID"] + "";
                    //string MachineUserid = HttpContext.Current.User.Identity.Name;                   
                    //string[] userids = MachineUserid.Split('\\');
                    //if (userids.Length == 2)
                    //{
                    //    MachineUserid = userids[1];
                    //}
                    Session["MachineUserid"] = MachineUserid;
                    //LoadComboBox();                   
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

        public void RefreshCombo()
        {
                lstOffering.DataSource = null;
                lstOffering.Items.Clear();
                lstOfferingDestination.Items.Clear();

                lstMCCSource.DataSource = null;
                lstMCCSource.Items.Clear();
                lstMCCDestination.Items.Clear();

                lstReportList.DataSource = null;
                lstReportList.Items.Clear();
                lstReporttobeAdded.Items.Clear();                                                  
        }

        static List<BEAdminUI> lstMapping = new List<BEAdminUI>();

        private void LoadComboBox()
        {
            try
            {
                lstMapping = service.GetBEPUMappingSU(rdbSU.SelectedItem.Text);
                lstOffering.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstOffering.DataBind();
                //lstOffering.Items.Insert(0, "ALL");

                lstMapping = service.GetSDMMappingAll(txtUserID.Text, ddlAccessLevel.SelectedItem.Text, rdbSU.SelectedValue);
                lstSDMSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstSDMSource.DataBind();
                
                reportCodes = service.GetAllRepCodes(txtUserID.Text);
                lstReportList.DataSource = reportCodes;
                lstReportList.DataBind();

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
            //ddlPu.Items.Insert(0, "ALL");
        }

        private void LoadComboBoxAll()
        {
            try
            {
                lstMapping = service.GetBEPUMapping(txtUserID.Text, rdbSU.SelectedValue);
                lstOfferingDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstOfferingDestination.DataBind();
                //lstOfferingDestination.Items.Insert(0, "ALL");

                string SU = rdbSU.SelectedValue;
                lstMapping = service.GetBEPUMappingAll(SU, txtUserID.Text);
                lstOffering.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstOffering.DataBind();
               
                //lstOffering.Items.Insert(0, "ALL");

                lstMapping = service.GetSDMMappingAll(txtUserID.Text,ddlAccessLevel.SelectedItem.Text, rdbSU.SelectedValue);
                lstSDMSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstSDMSource.DataBind();

                lstMapping = service.GetSDMMapping(txtUserID.Text, rdbSU.SelectedValue); 
                lstSDMDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstSDMDestination.DataBind();

                reportCodes = service.GetReportCodes(txtUserID.Text);
                lstReporttobeAdded.DataSource = reportCodes;
                lstReporttobeAdded.DataBind();

                reportCodes = service.GetAllRepCodes(txtUserID.Text);
                lstReportList.DataSource = reportCodes;
                lstReportList.DataBind();

               
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            rdbSU.ClearSelection();
            chkSU.ClearSelection();
            //if (ddlRole.SelectedItem.Text == "PnA")
            //{
            //    divSu1.Attributes.Add("style", "display:none");
            //    divSu2.Attributes.Add("style", "display:block");
            //}
            //else
            //{
                divSu1.Attributes.Add("style", "display:block");
                //divSu2.Attributes.Add("style", "display:none");
            //}
            if (txtUserID.Text.Length > 0)
            {

                lstReporttobeAdded.Items.Clear();
                SUTypeIsReadOnly = service.GetSUTypeIsReadOnly(txtUserID.Text);
                SUTypeIsReadOnlyMachineUserId = service.GetSUTypeIsReadOnly(Session["MachineUserid"].ToString());
                ////rdbSU.Items.Insert(0,ListItem(SUTypeIsReadOnly,"0"));
                //rdbSU.SelectedItem.Text = SUTypeIsReadOnly[0].ToString();
                //rdbType.SelectedItem.Text = SUTypeIsReadOnly[1];
                //rdbIsReadOnly.SelectedItem.Text=SUTypeIsReadOnly[2];

                //   RefreshCombo();
                divForButtons.Visible = true;
                divForddlRole.Visible = true;
                try
                {
                    if (!string.IsNullOrWhiteSpace(txtUserID.Text))
                    {
                        ClearAllFields();

                        int result;
                        result = service.verifyUserId(txtUserID.Text, Session["MachineUserid"].ToString());

                        string MachineUserIdRole = service.GetMachineRole(Session["MachineUserid"].ToString());

                        if (MachineUserIdRole.ToLower() == "pna")
                        {
                            divSu1.Attributes.Add("style", "display:block");
                            //divSu2.Attributes.Add("style", "display:block");

                            string SUValue = SUTypeIsReadOnlyMachineUserId[0].ToString();

                            string[] arr = SUValue.Split(',');

                            for(int count =0;count<arr.Length;count++)
                            {
                                if (arr[count].ToString() == "ORC")
                              {
                                if (result == 0)
                                {
                                    lblError.Text = "New userId";
                                    lblError.ForeColor = System.Drawing.Color.Red;
                                    ddlRole.Items.Clear();
                                    //ddlRole.Items.Add("Admin");                            
                                    ddlRole.Items.Add("PnA");
                                    ddlRole.Items.Add("Anchor");
                                    ddlRole.Items.Add("SDM");
                                    ddlRole.Items.Add("DM");
                                    //ddlRole.Items.Add("UH");
                                    ddlRole.Items.Add("SOH");
                                    ddlRole.Items.Add("DH");

                                    if (ddlRole.SelectedValue == "Anchor")
                                    {
                                        divDetails.Visible = true;

                                        rdbSU.Items.Clear();
                                        rdbSU.Items.Add("ORC");
                                        //rdbSU.Items.Add("SAP");
                                        rdbSU.SelectedIndex = 0;

                                        divForOfferings.Visible = true;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForSDM.Visible = false;
                                        divForAccessLevel.Visible = false;
                                    }
                                    else if (ddlRole.SelectedValue == "Admin" || ddlRole.SelectedValue == "UH")
                                    {
                                        divDetails.Visible = false;
                                        rdbSU.Items.Clear();
                                        rdbSU.Items.Add("ORC");
                                        //rdbSU.Items.Add("SAP");
                                        rdbSU.Items.Add("All");
                                        rdbSU.SelectedIndex = 0;
                                        divForOfferings.Visible = false;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForSDM.Visible = false;
                                        divForAccessLevel.Visible = false;
                                    }
                                    else
                                    {
                                        divDetails.Visible = false;

                                        rdbSU.Items.Clear();
                                        rdbSU.Items.Add("ORC");
                                        //rdbSU.Items.Add("SAP");
                                        rdbSU.SelectedIndex = 0;

                                        divForOfferings.Visible = false;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForSDM.Visible = false;
                                        divForAccessLevel.Visible = false;

                                        
                                    }
                                    LoadComboBox();
                                }
                                else if (result == 1)
                                {
                                    lblError.Text = "UserId already exists";
                                    lblError.ForeColor = System.Drawing.Color.Red;
                                    DataSet ds = new DataSet();
                                    ddlRole.Items.Clear();
                                    ddlRole.Items.Add("PnA");
                                    ddlRole.Items.Add("Anchor");
                                    ddlRole.Items.Add("SDM");
                                    ddlRole.Items.Add("DM");
                                    //ddlRole.Items.Add("UH");
                                    ddlRole.Items.Add("SOH");
                                    ddlRole.Items.Add("DH");

                                    string userrole = service.GetUserRole(txtUserID.Text);
                                    ddlRole.SelectedValue = userrole;



                                    if (ddlRole.SelectedValue == "Anchor")
                                    {
                                        rdbType.SelectedValue = SUTypeIsReadOnly[1];
                                        ddlAccessLevel.SelectedValue = SUTypeIsReadOnly[3];
                                        rdbIsReadOnly.SelectedValue = SUTypeIsReadOnly[2];
                                        divDetails.Visible = true;
                                        rdbSU.Items.Clear();
                                        rdbSU.Items.Add("ORC");
                                        //rdbSU.Items.Add("SAP");
                                        rdbSU.SelectedValue = SUTypeIsReadOnly[0].ToString();
                                        divForReports.Visible = true;
                                        divForAccessLevel.Visible = true;

                                        if (ddlAccessLevel.SelectedItem.Text == "Offering")
                                        {
                                            divForOfferings.Visible = true;
                                            divForMcc.Visible = false;
                                            divForSDM.Visible = false;

                                        }
                                        else if (ddlAccessLevel.SelectedItem.Text == "MCC")
                                        {
                                            divForMcc.Visible = true;
                                            divForOfferings.Visible = false;
                                            divForSDM.Visible = false;
                                            lstMapping = service.GetMccMapping(txtUserID.Text, rdbSU.SelectedValue);
                                            lstMCCDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                                            lstMCCDestination.DataBind();

                                            DataSet dsmccall = service.GetMccMappingAll(txtUserID.Text, rdbSU.SelectedValue);
                                            //lstMCCSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();                                
                                            lstMCCSource.DataSource = dsmccall.Tables[0];
                                            lstMCCSource.DataTextField = "MCCNSO";
                                            lstMCCSource.DataValueField = "MCCNSO";
                                            lstMCCSource.DataBind();

                                        }
                                        else
                                        {
                                            divForMcc.Visible = false;
                                            divForOfferings.Visible = false;
                                            divForSDM.Visible = true;

                                            lstMapping = service.GetSDMMappingAll(txtUserID.Text, ddlAccessLevel.SelectedItem.Text, rdbSU.SelectedValue);
                                            lstSDMSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                                            lstSDMSource.DataBind();

                                            lstMapping = service.GetSDMMapping(txtUserID.Text, rdbSU.SelectedValue);
                                            lstSDMDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                                            lstSDMDestination.DataBind();
                                        }
                                    }
                                    else if (ddlRole.SelectedValue == "Admin" || ddlRole.SelectedValue == "UH")
                                    {

                                        divDetails.Visible = false;

                                        rdbSU.Items.Clear();
                                        if (ddlRole.SelectedValue != "UH")
                                        {
                                            rdbSU.Items.Add("ORC");
                                            //rdbSU.Items.Add("SAP");
                                            rdbSU.Items.Add("All");
                                            // rdbSU.SelectedIndex = 0;
                                            rdbSU.SelectedValue = SUTypeIsReadOnly[0].ToString();
                                        }
                                        divForOfferings.Visible = false;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForAccessLevel.Visible = false;
                                    }
                                    else
                                    {
                                        divDetails.Visible = false;

                                        rdbSU.Items.Clear();
                                        rdbSU.Items.Add("ORC");
                                        //rdbSU.Items.Add("SAP");
                                        // rdbSU.SelectedIndex = 0;
                                        
                                        divForOfferings.Visible = false;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForAccessLevel.Visible = false;

                                        if (ddlRole.SelectedItem.Text == "PnA")
                                        {
                                           //chkSU.Items[0].Selected = true;
                                            rdbSU.Items[0].Selected = true;
                                        }
                                        else if (ddlRole.SelectedItem.Text == "DH" || ddlRole.SelectedItem.Text == "UH" || ddlRole.SelectedItem.Text == "SOH")
                                        {}
                                        else
                                        {
                                            rdbSU.SelectedValue = SUTypeIsReadOnly[0].ToString();
                                        }
                                    }
                                    LoadComboBoxAll();
                                }
                                else
                                {
                                    lblError.Text = "Sorry! you do not have access for this User!";
                                    lblError.ForeColor = System.Drawing.Color.Red;
                                    divForButtons.Visible = false;
                                    txtUserID.Enabled = false;
                                }
                            }

                                else if (arr[count].ToString() == "ECAS")
                            {
                                if (result == 0)
                                {
                                    lblError.Text = "New userId";
                                    lblError.ForeColor = System.Drawing.Color.Red;
                                    ddlRole.Items.Clear();
                                    //ddlRole.Items.Add("Admin");                            
                                    ddlRole.Items.Add("PnA");
                                    ddlRole.Items.Add("Anchor");
                                    ddlRole.Items.Add("SDM");
                                    ddlRole.Items.Add("DM");
                                    //ddlRole.Items.Add("UH");
                                    ddlRole.Items.Add("SOH");
                                    ddlRole.Items.Add("DH");

                                    if (ddlRole.SelectedValue == "Anchor")
                                    {
                                        divDetails.Visible = true;

                                        rdbSU.Items.Clear();
                                        rdbSU.Items.Add("ECAS");
                                        //rdbSU.Items.Add("SAP");
                                        rdbSU.SelectedIndex = 0;

                                        divForOfferings.Visible = true;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForSDM.Visible = false;
                                        divForAccessLevel.Visible = false;
                                    }
                                    else if (ddlRole.SelectedValue == "Admin" || ddlRole.SelectedValue == "UH")
                                    {
                                        divDetails.Visible = false;

                                        rdbSU.Items.Clear();
                                        rdbSU.Items.Add("ECAS");
                                        //rdbSU.Items.Add("SAP");
                                        rdbSU.Items.Add("All");
                                        rdbSU.SelectedIndex = 0;

                                        divForOfferings.Visible = false;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForSDM.Visible = false;
                                        divForAccessLevel.Visible = false;
                                    }
                                    else
                                    {
                                        divDetails.Visible = false;

                                        rdbSU.Items.Clear();
                                        rdbSU.Items.Add("ECAS");
                                        //rdbSU.Items.Add("SAP");
                                        rdbSU.SelectedIndex = 0;

                                        divForOfferings.Visible = false;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForSDM.Visible = false;
                                        divForAccessLevel.Visible = false;
                                    }
                                    LoadComboBox();
                                }
                                else if (result == 1)
                                {
                                    lblError.Text = "UserId already exists";
                                    lblError.ForeColor = System.Drawing.Color.Red;
                                    DataSet ds = new DataSet();
                                    ddlRole.Items.Clear();
                                    ddlRole.Items.Add("PnA");
                                    ddlRole.Items.Add("Anchor");
                                    ddlRole.Items.Add("SDM");
                                    ddlRole.Items.Add("DM");
                                    //ddlRole.Items.Add("UH");
                                    ddlRole.Items.Add("SOH");
                                    ddlRole.Items.Add("DH");

                                    string userrole = service.GetUserRole(txtUserID.Text);
                                    ddlRole.SelectedValue = userrole;



                                    if (ddlRole.SelectedValue == "Anchor")
                                    {
                                        rdbType.SelectedValue = SUTypeIsReadOnly[1];
                                        ddlAccessLevel.SelectedValue = SUTypeIsReadOnly[3];
                                        rdbIsReadOnly.SelectedValue = SUTypeIsReadOnly[2];
                                        divDetails.Visible = true;
                                        rdbSU.Items.Clear();
                                        rdbSU.Items.Add("ECAS");
                                        //rdbSU.Items.Add("SAP");
                                        rdbSU.SelectedValue = SUTypeIsReadOnly[0].ToString();
                                        divForReports.Visible = true;
                                        divForAccessLevel.Visible = true;

                                        if (ddlAccessLevel.SelectedItem.Text == "Offering")
                                        {
                                            divForOfferings.Visible = true;
                                            divForMcc.Visible = false;
                                            divForSDM.Visible = false;

                                        }
                                        else if (ddlAccessLevel.SelectedItem.Text == "MCC")
                                        {
                                            divForMcc.Visible = true;
                                            divForOfferings.Visible = false;
                                            divForSDM.Visible = false;
                                            lstMapping = service.GetMccMapping(txtUserID.Text, rdbSU.SelectedValue);
                                            lstMCCDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                                            lstMCCDestination.DataBind();

                                            DataSet dsmccall = service.GetMccMappingAll(txtUserID.Text, rdbSU.SelectedValue);
                                            //lstMCCSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();                                
                                            lstMCCSource.DataSource = dsmccall.Tables[0];
                                            lstMCCSource.DataTextField = "MCCNSO";
                                            lstMCCSource.DataValueField = "MCCNSO";
                                            lstMCCSource.DataBind();

                                        }
                                        else
                                        {
                                            divForMcc.Visible = false;
                                            divForOfferings.Visible = false;
                                            divForSDM.Visible = true;

                                            lstMapping = service.GetSDMMappingAll(txtUserID.Text, ddlAccessLevel.SelectedItem.Text, rdbSU.SelectedValue);
                                            lstSDMSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                                            lstSDMSource.DataBind();

                                            lstMapping = service.GetSDMMapping(txtUserID.Text, rdbSU.SelectedValue);
                                            lstSDMDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                                            lstSDMDestination.DataBind();
                                        }
                                    }
                                    else if (ddlRole.SelectedValue == "Admin" || ddlRole.SelectedValue == "UH")
                                    {

                                        divDetails.Visible = false;

                                        rdbSU.Items.Clear();
                                        rdbSU.Items.Add("ECAS");
                                        //rdbSU.Items.Add("SAP");
                                        rdbSU.Items.Add("All");
                                        // rdbSU.SelectedIndex = 0;
                                        if (ddlRole.SelectedValue != "UH")
                                        {
                                            rdbSU.SelectedValue = SUTypeIsReadOnly[0].ToString();
                                        }
                                        divForOfferings.Visible = false;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForAccessLevel.Visible = false;
                                    }
                                    else
                                    {
                                        divDetails.Visible = false;

                                        rdbSU.Items.Clear();
                                        rdbSU.Items.Add("ECAS");
                                        //rdbSU.Items.Add("SAP");
                                        // rdbSU.SelectedIndex = 0;
                                        divForOfferings.Visible = false;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForAccessLevel.Visible = false;

                                        if (ddlRole.SelectedItem.Text == "PnA")
                                        {
                                            rdbSU.Items[2].Selected = true;
                                        }
                                        else if (ddlRole.SelectedItem.Text == "DH" || ddlRole.SelectedItem.Text == "UH" || ddlRole.SelectedItem.Text == "SOH")
                                        { }
                                        else
                                        {
                                            rdbSU.SelectedValue = SUTypeIsReadOnly[0].ToString();
                                        }
                                    }
                                    LoadComboBoxAll();
                                }
                                else
                                {
                                    lblError.Text = "Sorry! you do not have access for this User!";
                                    lblError.ForeColor = System.Drawing.Color.Red;
                                    divForButtons.Visible = false;
                                    txtUserID.Enabled = false;
                                }
                            }

                                else if (arr[count].ToString() == "SAP")
                            {
                                if (result == 0)
                                {
                                    lblError.Text = "New userId";
                                    lblError.ForeColor = System.Drawing.Color.Red;
                                    ddlRole.Items.Clear();
                                    //ddlRole.Items.Add("Admin");                            
                                    ddlRole.Items.Add("PnA");
                                    ddlRole.Items.Add("Anchor");
                                    ddlRole.Items.Add("SDM");
                                    ddlRole.Items.Add("DM");
                                    //ddlRole.Items.Add("UH");
                                    ddlRole.Items.Add("SOH");
                                    ddlRole.Items.Add("DH");

                                    if (ddlRole.SelectedValue == "Anchor")
                                    {
                                        divDetails.Visible = true;

                                        rdbSU.Items.Clear();
                                        //rdbSU.Items.Add("ORC");
                                        rdbSU.Items.Add("SAP");
                                        rdbSU.SelectedIndex = 0;

                                        divForOfferings.Visible = true;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForSDM.Visible = false;
                                        divForAccessLevel.Visible = false;
                                    }
                                    else if (ddlRole.SelectedValue == "Admin" || ddlRole.SelectedValue == "UH")
                                    {
                                        divDetails.Visible = false;

                                        rdbSU.Items.Clear();
                                        //rdbSU.Items.Add("ORC");
                                        rdbSU.Items.Add("SAP");
                                        //rdbSU.Items.Add("All");
                                        rdbSU.SelectedIndex = 0;

                                        divForOfferings.Visible = false;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForSDM.Visible = false;
                                        divForAccessLevel.Visible = false;
                                    }
                                    else
                                    {
                                        divDetails.Visible = false;

                                        rdbSU.Items.Clear();
                                        //rdbSU.Items.Add("ORC");
                                        rdbSU.Items.Add("SAP");
                                        rdbSU.SelectedIndex = 0;

                                        divForOfferings.Visible = false;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForSDM.Visible = false;
                                        divForAccessLevel.Visible = false;

                                       
                                    }
                                    LoadComboBox();
                                }
                                else if (result == 1)
                                {
                                    lblError.Text = "UserId already exists";
                                    lblError.ForeColor = System.Drawing.Color.Red;
                                    DataSet ds = new DataSet();
                                    ddlRole.Items.Clear();
                                    ddlRole.Items.Add("PnA");
                                    ddlRole.Items.Add("Anchor");
                                    ddlRole.Items.Add("SDM");
                                    ddlRole.Items.Add("DM");
                                    //ddlRole.Items.Add("UH");
                                    ddlRole.Items.Add("SOH");
                                    ddlRole.Items.Add("DH");

                                    string userrole = service.GetUserRole(txtUserID.Text);
                                    ddlRole.SelectedValue = userrole;



                                    if (ddlRole.SelectedValue == "Anchor")
                                    {
                                        rdbType.SelectedValue = SUTypeIsReadOnly[1];
                                        ddlAccessLevel.SelectedValue = SUTypeIsReadOnly[3];
                                        rdbIsReadOnly.SelectedValue = SUTypeIsReadOnly[2];
                                        divDetails.Visible = true;
                                        rdbSU.Items.Clear();
                                        //rdbSU.Items.Add("ORC");
                                        rdbSU.Items.Add("SAP");
                                        rdbSU.SelectedValue = SUTypeIsReadOnly[0].ToString();
                                        divForReports.Visible = true;
                                        divForAccessLevel.Visible = true;

                                        if (ddlAccessLevel.SelectedItem.Text == "Offering")
                                        {
                                            divForOfferings.Visible = true;
                                            divForMcc.Visible = false;
                                            divForSDM.Visible = false;

                                        }
                                        else if (ddlAccessLevel.SelectedItem.Text == "MCC")
                                        {
                                            divForMcc.Visible = true;
                                            divForOfferings.Visible = false;
                                            divForSDM.Visible = false;
                                            lstMapping = service.GetMccMapping(txtUserID.Text, rdbSU.SelectedValue);
                                            lstMCCDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                                            lstMCCDestination.DataBind();

                                            DataSet dsmccall = service.GetMccMappingAll(txtUserID.Text, rdbSU.SelectedValue);
                                            //lstMCCSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();                                
                                            lstMCCSource.DataSource = dsmccall.Tables[0];
                                            lstMCCSource.DataTextField = "MCCNSO";
                                            lstMCCSource.DataValueField = "MCCNSO";
                                            lstMCCSource.DataBind();

                                        }
                                        else
                                        {
                                            divForMcc.Visible = false;
                                            divForOfferings.Visible = false;
                                            divForSDM.Visible = true;

                                            lstMapping = service.GetSDMMappingAll(txtUserID.Text, ddlAccessLevel.SelectedItem.Text, rdbSU.SelectedValue);
                                            lstSDMSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                                            lstSDMSource.DataBind();

                                            lstMapping = service.GetSDMMapping(txtUserID.Text, rdbSU.SelectedValue);
                                            lstSDMDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                                            lstSDMDestination.DataBind();
                                        }
                                    }
                                    else if (ddlRole.SelectedValue == "Admin" || ddlRole.SelectedValue == "UH")
                                    {

                                        divDetails.Visible = false;

                                        rdbSU.Items.Clear();
                                        //rdbSU.Items.Add("ORC");
                                        rdbSU.Items.Add("SAP");
                                        //rdbSU.Items.Add("All");
                                        // rdbSU.SelectedIndex = 0;
                                        if (ddlRole.SelectedValue != "UH")
                                        {
                                            rdbSU.SelectedValue = SUTypeIsReadOnly[0].ToString();
                                        }
                                        divForOfferings.Visible = false;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForAccessLevel.Visible = false;
                                    }
                                    else
                                    {
                                        divDetails.Visible = false;

                                        rdbSU.Items.Clear();
                                        //rdbSU.Items.Add("ORC");
                                        rdbSU.Items.Add("SAP");
                                        // rdbSU.SelectedIndex = 0;
                                        divForOfferings.Visible = false;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForAccessLevel.Visible = false;

                                        if (ddlRole.SelectedItem.Text == "PnA")
                                        {
                                            rdbSU.Items[1].Selected = true;
                                        }
                                        else if (ddlRole.SelectedItem.Text == "DH" || ddlRole.SelectedItem.Text == "UH" || ddlRole.SelectedItem.Text == "SOH")
                                        { }
                                        else
                                        {
                                            rdbSU.SelectedValue = SUTypeIsReadOnly[0].ToString();
                                        }
                                    }
                                    LoadComboBoxAll();
                                }
                                else
                                {
                                    lblError.Text = "Sorry! you do not have access for this User!";
                                    lblError.ForeColor = System.Drawing.Color.Red;
                                    divForButtons.Visible = false;
                                    txtUserID.Enabled = false;
                                }
                            }
                                else if (arr[count].ToString() == "EAIS")
                            {
                                if (result == 0)
                                {
                                    lblError.Text = "New userId";
                                    lblError.ForeColor = System.Drawing.Color.Red;
                                    ddlRole.Items.Clear();
                                    //ddlRole.Items.Add("Admin");                            
                                    ddlRole.Items.Add("PnA");
                                    ddlRole.Items.Add("Anchor");
                                    ddlRole.Items.Add("SDM");
                                    ddlRole.Items.Add("DM");
                                    //ddlRole.Items.Add("UH");
                                    ddlRole.Items.Add("SOH");
                                    ddlRole.Items.Add("DH");

                                    if (ddlRole.SelectedValue == "Anchor")
                                    {
                                        divDetails.Visible = true;

                                        rdbSU.Items.Clear();
                                        //rdbSU.Items.Add("ORC");
                                        rdbSU.Items.Add("EAIS");
                                        rdbSU.SelectedIndex = 0;

                                        divForOfferings.Visible = true;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForSDM.Visible = false;
                                        divForAccessLevel.Visible = false;
                                    }
                                    else if (ddlRole.SelectedValue == "Admin" || ddlRole.SelectedValue == "UH")
                                    {
                                        divDetails.Visible = false;

                                        rdbSU.Items.Clear();
                                        //rdbSU.Items.Add("ORC");
                                        rdbSU.Items.Add("EAIS");
                                        rdbSU.Items.Add("All");
                                        rdbSU.SelectedIndex = 0;

                                        divForOfferings.Visible = false;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForSDM.Visible = false;
                                        divForAccessLevel.Visible = false;
                                    }
                                    else
                                    {
                                        divDetails.Visible = false;

                                        rdbSU.Items.Clear();
                                        //rdbSU.Items.Add("ORC");
                                        rdbSU.Items.Add("EAIS");
                                        rdbSU.SelectedIndex = 0;

                                        divForOfferings.Visible = false;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForSDM.Visible = false;
                                        divForAccessLevel.Visible = false;
                                    }
                                    LoadComboBox();
                                }
                                else if (result == 1)
                                {
                                    lblError.Text = "UserId already exists";
                                    lblError.ForeColor = System.Drawing.Color.Red;
                                    DataSet ds = new DataSet();
                                    ddlRole.Items.Clear();
                                    ddlRole.Items.Add("PnA");
                                    ddlRole.Items.Add("Anchor");
                                    ddlRole.Items.Add("SDM");
                                    ddlRole.Items.Add("DM");
                                    //ddlRole.Items.Add("UH");
                                    ddlRole.Items.Add("SOH");
                                    ddlRole.Items.Add("DH");

                                    string userrole = service.GetUserRole(txtUserID.Text);
                                    ddlRole.SelectedValue = userrole;



                                    if (ddlRole.SelectedValue == "Anchor")
                                    {
                                        rdbType.SelectedValue = SUTypeIsReadOnly[1];
                                        ddlAccessLevel.SelectedValue = SUTypeIsReadOnly[3];
                                        rdbIsReadOnly.SelectedValue = SUTypeIsReadOnly[2];
                                        divDetails.Visible = true;
                                        rdbSU.Items.Clear();
                                        //rdbSU.Items.Add("ORC");
                                        rdbSU.Items.Add("EAIS");
                                        rdbSU.SelectedValue = SUTypeIsReadOnly[0].ToString();
                                        divForReports.Visible = true;
                                        divForAccessLevel.Visible = true;

                                        if (ddlAccessLevel.SelectedItem.Text == "Offering")
                                        {
                                            divForOfferings.Visible = true;
                                            divForMcc.Visible = false;
                                            divForSDM.Visible = false;

                                        }
                                        else if (ddlAccessLevel.SelectedItem.Text == "MCC")
                                        {
                                            divForMcc.Visible = true;
                                            divForOfferings.Visible = false;
                                            divForSDM.Visible = false;
                                            lstMapping = service.GetMccMapping(txtUserID.Text, rdbSU.SelectedValue);
                                            lstMCCDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                                            lstMCCDestination.DataBind();

                                            DataSet dsmccall = service.GetMccMappingAll(txtUserID.Text, rdbSU.SelectedValue);
                                            //lstMCCSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();                                
                                            lstMCCSource.DataSource = dsmccall.Tables[0];
                                            lstMCCSource.DataTextField = "MCCNSO";
                                            lstMCCSource.DataValueField = "MCCNSO";
                                            lstMCCSource.DataBind();

                                        }
                                        else
                                        {
                                            divForMcc.Visible = false;
                                            divForOfferings.Visible = false;
                                            divForSDM.Visible = true;

                                            lstMapping = service.GetSDMMappingAll(txtUserID.Text, ddlAccessLevel.SelectedItem.Text, rdbSU.SelectedValue);
                                            lstSDMSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                                            lstSDMSource.DataBind();

                                            lstMapping = service.GetSDMMapping(txtUserID.Text, rdbSU.SelectedValue);
                                            lstSDMDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                                            lstSDMDestination.DataBind();
                                        }
                                    }
                                    else if (ddlRole.SelectedValue == "Admin" || ddlRole.SelectedValue == "UH")
                                    {

                                        divDetails.Visible = false;

                                        rdbSU.Items.Clear();
                                        //rdbSU.Items.Add("ORC");
                                        rdbSU.Items.Add("EAIS");
                                        rdbSU.Items.Add("All");
                                        // rdbSU.SelectedIndex = 0;
                                        if (ddlRole.SelectedValue != "UH")
                                        {
                                            rdbSU.SelectedValue = SUTypeIsReadOnly[0].ToString();
                                        }
                                        divForOfferings.Visible = false;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForAccessLevel.Visible = false;
                                    }
                                    else
                                    {
                                        divDetails.Visible = false;

                                        rdbSU.Items.Clear();
                                        //rdbSU.Items.Add("ORC");
                                        rdbSU.Items.Add("EAIS");
                                        // rdbSU.SelectedIndex = 0;
                                        divForOfferings.Visible = false;
                                        divForReports.Visible = true;
                                        divForMcc.Visible = false;
                                        divForAccessLevel.Visible = false;

                                        if (ddlRole.SelectedItem.Text == "PnA")
                                        {
                                            rdbSU.Items[3].Selected = true;
                                        }
                                        else if (ddlRole.SelectedItem.Text == "DH" || ddlRole.SelectedItem.Text == "UH" || ddlRole.SelectedItem.Text == "SOH")
                                        { }
                                        else
                                        {
                                            rdbSU.SelectedValue = SUTypeIsReadOnly[0].ToString();
                                        }
                                    }
                                    LoadComboBoxAll();
                                }
                                else
                                {
                                    lblError.Text = "Sorry! you do not have access for this User!";
                                    lblError.ForeColor = System.Drawing.Color.Red;
                                    divForButtons.Visible = false;
                                    txtUserID.Enabled = false;
                                }
                            }

                        }

                        }
                        else
                        {
                            if (result == 0)
                            {
                                lblError.Text = "New userId";
                                lblError.ForeColor = System.Drawing.Color.Red;
                                // DataSet ds = new DataSet();
                                //lstMapping = service.GetAllRole();
                                //ddlRole.DataSource = lstMapping.Select(k => k.Role).Distinct().ToList();
                                //ddlRole.DataBind();
                                ddlRole.Items.Clear();
                                ddlRole.Items.Add("Admin");
                                ddlRole.Items.Add("Anchor");
                                ddlRole.Items.Add("PnA");
                                ddlRole.Items.Add("SDM");
                                ddlRole.Items.Add("DM");
                                ddlRole.Items.Add("UH");
                                ddlRole.Items.Add("SOH");
                                ddlRole.Items.Add("DH");

                                if (ddlRole.SelectedValue == "Anchor")
                                {
                                    divDetails.Visible = true;

                                    rdbSU.Items.Clear();
                                    rdbSU.Items.Add("ORC");
                                    rdbSU.Items.Add("SAP");
                                    //rdbSU.Items.Add("ECAS");
                                    //rdbSU.Items.Add("EAIS");
                                    rdbSU.SelectedIndex = 0;

                                    divForOfferings.Visible = true;
                                    divForReports.Visible = true;
                                    divForMcc.Visible = false;
                                    divForSDM.Visible = false;
                                    divForAccessLevel.Visible = false;
                                }
                                else if (ddlRole.SelectedValue == "Admin" || ddlRole.SelectedValue == "UH")
                                {
                                    divDetails.Visible = false;

                                    rdbSU.Items.Clear();
                                    rdbSU.Items.Add("ORC");
                                    rdbSU.Items.Add("SAP");
                                    //rdbSU.Items.Add("ECAS");
                                    //rdbSU.Items.Add("EAIS");
                                    //rdbSU.Items.Add("All");
                                    rdbSU.SelectedIndex = 0;

                                    divForOfferings.Visible = false;
                                    divForReports.Visible = true;
                                    divForMcc.Visible = false;
                                    divForSDM.Visible = false;
                                    divForAccessLevel.Visible = false;
                                }
                                else
                                {
                                    divDetails.Visible = false;

                                    rdbSU.Items.Clear();
                                    rdbSU.Items.Add("ORC");
                                    rdbSU.Items.Add("SAP");
                                    //rdbSU.Items.Add("ECAS");
                                    //rdbSU.Items.Add("EAIS");
                                    rdbSU.SelectedIndex = 0;

                                    divForOfferings.Visible = false;
                                    divForReports.Visible = true;
                                    divForMcc.Visible = false;
                                    divForSDM.Visible = false;
                                    divForAccessLevel.Visible = false;
                                }
                                LoadComboBox();
                            }
                            else if (result == 1)
                            {
                                lblError.Text = "UserId already exists";
                                lblError.ForeColor = System.Drawing.Color.Red;
                                DataSet ds = new DataSet();
                                //lstMapping = service.GetAllRole();
                                //ddlRole.DataSource = lstMapping.Select(k => k.Role).Distinct().ToList();
                                //ddlRole.DataBind();
                                ddlRole.Items.Clear();
                                ddlRole.Items.Add("Admin");
                                ddlRole.Items.Add("Anchor");
                                ddlRole.Items.Add("PnA");
                                ddlRole.Items.Add("SDM");
                                ddlRole.Items.Add("DM");
                                ddlRole.Items.Add("UH");
                                ddlRole.Items.Add("SOH");
                                ddlRole.Items.Add("DH");

                                string userrole = service.GetUserRole(txtUserID.Text);
                                ddlRole.SelectedValue = userrole;


                                if (ddlRole.SelectedValue == "Anchor")
                                {
                                    //string cmdType = "select distinct txtDMorSDM from BEUserAccess where txtUserId='" + txtUserID.Text + "'";
                                    //DataSet DSType = service.GetDataSet(cmdType);
                                    //DataTable DTType = DSType.Tables[0];
                                    //string Type = DTType.Rows[0][0].ToString();
                                    //rdbType.SelectedValue = Type;
                                    rdbType.SelectedValue = SUTypeIsReadOnly[1];
                                    ddlAccessLevel.SelectedValue = SUTypeIsReadOnly[3];

                                    //string cmdIsReadOnly = "select distinct txtisReadOnly from BEUserAccess where txtUserId='" + txtUserID.Text + "'";
                                    //DataSet DSIsReadOnly = service.GetDataSet(cmdIsReadOnly);
                                    //DataTable DTIsReadOnly = DSIsReadOnly.Tables[0];
                                    //string UserIsReadOnly = DTIsReadOnly.Rows[0][0].ToString();
                                    //rdbIsReadOnly.SelectedValue = UserIsReadOnly;
                                    rdbIsReadOnly.SelectedValue = SUTypeIsReadOnly[2];

                                    divDetails.Visible = true;

                                    rdbSU.Items.Clear();
                                    rdbSU.Items.Add("ORC");
                                    rdbSU.Items.Add("SAP");
                                    //rdbSU.Items.Add("ECAS");
                                    //rdbSU.Items.Add("EAIS");
                                    // rdbSU.SelectedIndex = 0;

                                    //divForMcc.Visible = true;
                                    //divForOfferings.Visible = true;

                                    //string cmdUserSU = "select distinct txtServiceLine from BEUserAccess where txtUserId='" + txtUserID.Text + "'";
                                    //DataSet DSSU = service.GetDataSet(cmdUserSU);
                                    //DataTable DTsU = DSSU.Tables[0];
                                    //string USERSU = DTsU.Rows[0][0].ToString();
                                    //rdbSU.SelectedValue = USERSU;
                                    rdbSU.SelectedValue = SUTypeIsReadOnly[0].ToString();
                                    divForReports.Visible = true;
                                    divForAccessLevel.Visible = true;

                                    if (ddlAccessLevel.SelectedItem.Text == "Offering")
                                    {
                                        divForOfferings.Visible = true;
                                        divForMcc.Visible = false;
                                        divForSDM.Visible = false;

                                    }
                                    else if (ddlAccessLevel.SelectedItem.Text == "MCC")
                                    {
                                        divForMcc.Visible = true;
                                        divForOfferings.Visible = false;
                                        divForSDM.Visible = false;
                                        lstMapping = service.GetMccMapping(txtUserID.Text, rdbSU.SelectedValue);
                                        lstMCCDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                                        lstMCCDestination.DataBind();

                                        DataSet dsmccall = service.GetMccMappingAll(txtUserID.Text, rdbSU.SelectedValue);
                                        //lstMCCSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();                                
                                        lstMCCSource.DataSource = dsmccall.Tables[0];
                                        lstMCCSource.DataTextField = "MCCNSO";
                                        lstMCCSource.DataValueField = "MCCNSO";
                                        lstMCCSource.DataBind();

                                    }
                                    else
                                    {
                                        divForMcc.Visible = false;
                                        divForOfferings.Visible = false;
                                        divForSDM.Visible = true;

                                        lstMapping = service.GetSDMMappingAll(txtUserID.Text, ddlAccessLevel.SelectedItem.Text, rdbSU.SelectedValue);
                                        lstSDMSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                                        lstSDMSource.DataBind();

                                        lstMapping = service.GetSDMMapping(txtUserID.Text, rdbSU.SelectedValue);
                                        lstSDMDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                                        lstSDMDestination.DataBind();
                                    }
                                }
                                else if (ddlRole.SelectedValue == "Admin" || ddlRole.SelectedValue == "UH")
                                {

                                    divDetails.Visible = false;

                                    rdbSU.Items.Clear();
                                    rdbSU.Items.Add("ORC");
                                    rdbSU.Items.Add("SAP");
                                    //rdbSU.Items.Add("ECAS");
                                    //rdbSU.Items.Add("EAIS");
                                    //rdbSU.Items.Add("All");
                                    // rdbSU.SelectedIndex = 0;
                                    if (ddlRole.SelectedValue != "UH")
                                    {
                                        rdbSU.SelectedValue = SUTypeIsReadOnly[0].ToString();
                                    }
                                    divForOfferings.Visible = false;
                                    divForReports.Visible = true;
                                    divForMcc.Visible = false;
                                    divForAccessLevel.Visible = false;
                                }
                                else
                                {
                                    divDetails.Visible = false;

                                    rdbSU.Items.Clear();
                                    rdbSU.Items.Add("ORC");
                                    rdbSU.Items.Add("SAP");
                                    //rdbSU.Items.Add("ECAS");
                                    //rdbSU.Items.Add("EAIS");
                                    // rdbSU.SelectedIndex = 0;
                                  
                                    divForOfferings.Visible = false;
                                    divForReports.Visible = true;
                                    divForMcc.Visible = false;
                                    divForSDM.Visible = false;
                                    divForAccessLevel.Visible = false;

                                    if (ddlRole.SelectedItem.Text == "PnA")
                                    {
                                        divSu1.Attributes.Add("style", "display:block");
                                        //divSu2.Attributes.Add("style", "display:block");

                                        string selSu = SUTypeIsReadOnly[0].ToString();

                                        string[] arr1 = selSu.Split(',');

                                        for (int l = 0; l < arr1.Length; l++)
                                        {
                                            //chkSU.SelectedValue = arr1[l].ToString();

                                            if (arr1[l].ToString() == "ORC")
                                            {
                                                rdbSU.Items[0].Selected = true;
                                            }
                                            else if (arr1[l].ToString() == "SAP")
                                            {
                                                rdbSU.Items[1].Selected = true;
                                            }
                                            else if (arr1[l].ToString() == "ECAS")
                                            {
                                                rdbSU.Items[2].Selected = true;
                                            }
                                            else if (arr1[l].ToString() == "EAIS")
                                            {
                                                rdbSU.Items[3].Selected = true;
                                            }

                                        }
                                    }
                                    else if (ddlRole.SelectedItem.Text == "DH" || ddlRole.SelectedItem.Text == "UH" || ddlRole.SelectedItem.Text == "SOH")
                                    { }
                                    else
                                    {
                                        
                                        rdbSU.SelectedValue = SUTypeIsReadOnly[0].ToString();
                                    }
                                }
                                LoadComboBoxAll();
                            }
                        }
                    }
                    else
                    {
                        lblError.Text = "Please Enter UserName";
                        lblError.ForeColor = System.Drawing.Color.Red;
                    }
                    HideServiceLine();
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
            else
            {
                lblError.Text = "Please Enter UserName";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void PopulateFields(BEAdminUI beui)
        {

            try
            {
                ddlRole.Text = beui.Role;

                if (beui.IsAdmin.Trim() == "Y" && beui.Role == "PNA-BITS")
                {
                    ddlRole.Text = "Admin-BITS";
                }
                if (beui.IsAdmin.Trim() == "Y" && beui.Role == "PNA-CSI")
                {
                    ddlRole.Text = "Admin-CSI";
                }
                if (beui.IsAdmin.Trim() == "Y" && beui.Role == "PNA-PPS")
                {
                    ddlRole.Text = "Admin-PPS";
                }

                if (beui.PU == "ALL" || beui.PU == "BITS" || beui.PU == "CSI" || beui.PU == "PPS")
                {
                    LoadComboBoxAll();
                  //  ddlPu.Enabled = false;
                    //lstDUSource.Items.Add("ALL");
                }
                else
                {
                  //  ddlPu.Enabled = true;
                   // ddlPu.Text = beui.PU;
                    LoadComboBox();
                }


                //if (ddlRole.Text.ToLower() == "admin" || ddlRole.Text.ToLower() == "anchor" || ddlRole.Text.ToLower() == "anchor - r")
                //{
                //    ddlPu.DataSource = null;
                //    ddlPu.Items.Clear();
                //    ddlPu.Items.Add("ALL");
                //    ddlPu.Enabled = false;
                //    lstDUDestination.Items.Clear();
                //    lstDUDestination.Items.Add("ALL");
                //    lstDUSource.Items.Clear();
                //}

                //TODO:12/10 client code selection not req
                //LoadClentCodeSource();
                if (ddlRole.Text.ToLower() == "dh" || ddlRole.Text.ToLower() == "sdm")
                {
                    lstMCCDestination.Items.Clear();
                    lstMCCDestination.Items.Add("All");
                    lstMCCSource.Items.Clear();
                    //lstReporttobeAdded.DataSource = beui.ReportCodeList;
                    //lstReporttobeAdded.DataBind();

                }
                else if (ddlRole.Text.ToLower() == "admin" || ddlRole.Text.ToLower() == "anchor" || ddlRole.Text.ToLower() == "anchor - r")
                //else if (ddlRole.Text.ToLower() == "admin" || ddlRole.Text.ToLower() == "anchor" || ddlRole.Text.ToLower() == "anchor - r")
                {
                   // ddlPu.DataSource = null;
                   // ddlPu.Items.Clear();
                   // ddlPu.Items.Add("ALL");
                  //  ddlPu.Enabled = false;
                    lstMCCDestination.Items.Clear();
                    lstMCCDestination.Items.Add("ALL");
                    lstMCCSource.Items.Clear();
                }

                //else if (ddlRole.Text.ToLower() == "pna-pps" || ddlRole.Text.ToLower() == "admin-pps")
                ////else if (ddlRole.Text.ToLower() == "admin" || ddlRole.Text.ToLower() == "anchor" || ddlRole.Text.ToLower() == "anchor - r")
                //{
                //    ddlPu.DataSource = null;
                //    ddlPu.Items.Clear();
                //    ddlPu.Items.Add("PPS");
                //    ddlPu.Enabled = false;
                //    lstDUDestination.Items.Clear();
                //    lstDUDestination.Items.Add("ALL");
                //    lstDUSource.Items.Clear();
                //}

                //else if (ddlRole.Text.ToLower() == "delegate")
                //{

                //    ddlPu.DataSource = null;

                //    ddlPu.Items.Clear();

                //    ddlPu.Items.Add("ALL");

                //    ddlPu.Enabled = false;

                //    lstDUDestination.Items.Clear();

                //    lstDUDestination.Items.Add("ALL");

                //    lstDUSource.Items.Clear();

                //    lstReportList.Items.Clear();

                //    lstReporttobeAdded.Items.Clear();

                //}



                //else if (ddlRole.Text.ToLower() == "pna-bits" || ddlRole.Text.ToLower() == "admin-bits")
                ////else if (ddlRole.Text.ToLower() == "admin" || ddlRole.Text.ToLower() == "anchor" || ddlRole.Text.ToLower() == "anchor - r")
                //{
                //    ddlPu.DataSource = null;
                //    ddlPu.Items.Clear();
                //    ddlPu.Items.Add("BITS");
                //    ddlPu.Enabled = false;
                //    lstDUDestination.Items.Clear();
                //    lstDUDestination.Items.Add("ALL");
                //    lstDUSource.Items.Clear();
                //}

                //else if (ddlRole.Text.ToLower() == "pna-csi" || ddlRole.Text.ToLower() == "admin-csi")
                ////else if (ddlRole.Text.ToLower() == "admin" || ddlRole.Text.ToLower() == "anchor" || ddlRole.Text.ToLower() == "anchor - r")
                //{
                //    ddlPu.DataSource = null;
                //    ddlPu.Items.Clear();
                //    ddlPu.Items.Add("CSI");
                //    ddlPu.Enabled = false;
                //    lstDUDestination.Items.Clear();
                //    lstDUDestination.Items.Add("ALL");
                //    lstDUSource.Items.Clear();
                //}
                else
                {
                    lstMCCDestination.DataSource = beui.DuList;
                    lstMCCDestination.DataBind();
                    //lstReporttobeAdded.DataSource = beui.ReportCodeList;
                    //lstReporttobeAdded.DataBind();
                    LoadDuSource();
                }

                if (beui.ReportCodeList != null)
                {
                    beui.ReportCodeList = PopulateDestinationReportCodes(beui.ReportCodeList);
                    lstReporttobeAdded.DataSource = beui.ReportCodeList;
                    lstReporttobeAdded.DataBind();
                    if (beui.ReportCodeList != null)
                    {
                        for (int i = 0; i < beui.ReportCodeList.Length; i++)
                        {
                            lstReportList.Items.Remove(beui.ReportCodeList[i]);
                        }
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

        private string[] PopulateDestinationReportCodes(string[] _ReportCodes)
        {
            string[] ReportCodes = _ReportCodes.Length > 0 ? _ReportCodes[0].Split(',') : null;

            string[] returnValues = default(string[]);
            try
            {
                if (ReportCodes != null)
                {
                    string[] accessOutCodes = new string[ReportCodes.Length];
                    Hashtable hshReports = new Hashtable();
                    for (int i = 0; i < reportCodes.Length; i++)
                    {
                        string[] str = reportCodes[i].Split('|');
                        if (str.Length == 2)
                            hshReports.Add(str[0], str[1]);
                    }
                    for (int j = 0; j < ReportCodes.Length; j++)
                    {

                        if (hshReports.ContainsKey(ReportCodes[j].ToString()))
                            accessOutCodes[j] = ReportCodes[j].Trim() + "|" + hshReports[ReportCodes[j]].ToString();
                    }
                    returnValues = accessOutCodes.All(k => k == null) ? null : accessOutCodes;

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
            return returnValues;
        }

        #region clientCodepopulate
        //TODO:12/10 client code selection not req
        //private void PopulateFields1(BEClientAdminUI beclientui)
        //{

        //    try
        //    {

        //        //if (beui.PU == "ALL")
        //        //{
        //        //    LoadComboBoxAll();
        //        //    ddlPu.Enabled = false;
        //        //}
        //        //else
        //        //{
        //        //    ddlPu.Text = beui.PU;
        //        //    LoadComboBox();
        //        //}
        //        ///LoadDuSource();
        //        ///

        //        lstClientCodeSource.Items.Clear();
        //        lstClientCodeDest.DataSource = beclientui.ClientCodeList.Distinct();
        //        lstClientCodeDest.DataBind();

        //        string Pu = ddlPu.Text;
        //        List<string> derivedClientCode = new List<string>();
        //        var temp1 = service.GetBEClientCodeForPU(Pu);
        //        derivedClientCode.AddRange(temp1);
        //        if (lstClientCodeDest.Items.Count > 0)
        //        {

        //            var itemsInDestination = lstClientCodeDest.Items.Cast<ListItem>().Select(k => k.Value).ToList();

        //            derivedClientCode.RemoveAll(k => itemsInDestination.Contains(k));

        //        }


        //        if (ddlPu.Text == "ALL")
        //        {
        //            lstClientCodeDest.Items.Clear();
        //            lstClientCodeSource.Items.Clear();
        //            lstClientCodeDest.Items.Add("All");
        //        }
        //        else
        //        {
        //            if (ddlRole.Text.ToLower() == "dh" || ddlRole.Text.ToLower() == "sdm")
        //            {
        //                lstClientCodeDest.Items.Clear();
        //                lstClientCodeSource.Items.Clear();
        //                lstClientCodeDest.Items.Add("All");
        //            }
        //            else
        //            {
        //                lstClientCodeSource.Items.Clear();
        //                lstClientCodeSource.DataSource = derivedClientCode;
        //                lstClientCodeSource.DataBind();
        //            }
        //        }

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
        #endregion
        private void LoadDuSource()
        {
            try
            {
                string Pu;
                Pu = lstOffering.Text;
                List<string> derivedDUS = new List<string>();

                //TODO:21/9 funciton calling changed
                var temp = service.GetBEDMForPU(Pu);
                derivedDUS.AddRange(temp);

                if (lstMCCDestination.Items.Count > 0)
                {

                    var itemsInDestination = lstMCCDestination.Items.Cast<ListItem>().Select(k => k.Value).ToList();

                    derivedDUS.RemoveAll(k => itemsInDestination.Contains(k));

                }

                //if (ddlPu.Text == "ALL")
                //{
                //    lstDUSource.Items.Clear();
                //    //lstDUSource.Items.Add("ALL");
                //}
                else
                {
                    lstMCCSource.Items.Clear();
                    lstMCCSource.DataSource = derivedDUS;
                    lstMCCSource.DataBind();
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

        #region clientcode
        //TODO:12/10 client code selection not req
        //private void LoadClentCodeSource()
        //{
        //    try
        //    {
        //        string Pu;
        //        Pu = ddlPu.Text;
        //        List<string> derivedClientCode = new List<string>();

        //        var temp = service.GetBEClientCodeForPU(Pu);
        //        derivedClientCode.AddRange(temp);

        //        if (ddlPu.Text == "ALL")
        //        {
        //            lstClientCodeSource.Items.Clear();
        //            if (lstClientCodeDest.Items.Count == 0)
        //            {
        //                lstClientCodeSource.Items.Add("ALL");
        //            }
        //            //lstDUSource.Items.Add("ALL");
        //            //lstClientCodeSource.Items.Add("ALL");
        //            //lstClientCodeDest.Items.Add("ALL");
        //        }
        //        else
        //        {
        //            if (ddlRole.Text.ToLower() == "dh" || ddlRole.Text.ToLower() == "sdm")
        //            {

        //                lstClientCodeSource.Items.Clear();

        //            }
        //            else
        //            {
        //                lstClientCodeSource.Items.Clear();
        //                lstClientCodeSource.DataSource = derivedClientCode;
        //                lstClientCodeSource.DataBind();
        //            }
        //        }


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

        #endregion

        private void ClearAllFields()
        {
            try
            {
                //lstMCCDestination.Items.Clear();
                //lstMCCSource.Items.Clear();
                lblError.Text = string.Empty;
                lblSuccess.Text = string.Empty;

                //TODO:12/10 client code selection not req
                //lstClientCodeDest.Items.Clear();
                //lstClientCodeSource.Items.Clear();
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


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateFields())
                {                   
                    string du = "";
                    string MCC="";
                    string SDM = "";
                    string repcode = "";
                    string ClientCode = "";
                    BEAdminUI beui = new BEAdminUI();
                    BEClientAdminUI beclientui = new BEClientAdminUI();
                    beui.Role = ddlRole.Text;
                    //beui.PU = lstOfferingDestination.Text;
                    //beclientui.PU = lstOfferingDestination.SelectedItem.Text;

                    //if (lstOfferingDestination.Items != null && lstOfferingDestination.Items.Count > 0)
                    //{
                    //    //    if (ddlRole.Text.ToLower() == "dh" || ddlRole.Text.ToLower() == "sdm")
                    //    //    {
                    //    //        string[] dulist = service.GetBEDMForPU(lstOffering.Text.Trim());
                    //    //        beui.DuList = dulist;
                    //    //        int icount = dulist.Length;
                    //    //        if (icount > 0)
                    //    //        {
                    //    //            for (int j = 0; j < icount; j++)
                    //    //            {
                    //    //                if (j == 0)
                    //    //                    du = dulist[0].ToString().Trim();
                    //    //                else
                    //    //                    du = du + "," + dulist[j].ToString().Trim();
                    //    //            }
                    //    //        }
                    //    //    }
                    //    //    else
                    //    //    {
                    //    int icount = lstOfferingDestination.Items.Count;
                    //    string[] dulist = new string[icount];

                    //    for (int j = 0; j < icount; j++)
                    //    {
                    //        //du = "";
                    //        dulist[j] = lstOfferingDestination.Items[j].Text.Trim();
                    //        if (j == 0)
                    //        {
                    //            du = lstOfferingDestination.Items[j].Text.Trim();
                    //        }
                    //        else
                    //        {
                    //            du = du + "," + lstOfferingDestination.Items[j].Text.Trim();
                    //        }
                    //    }
                    //    beui.DuList = dulist;
                    //    //    }
                    //}
                    if (lstReporttobeAdded.Items != null && lstReporttobeAdded.Items.Count > 0)
                    {
                        int icount = lstReporttobeAdded.Items.Count;
                        string[] reportList = new string[icount];
                        for (int j = 0; j < icount; j++)
                        {
                            reportList[j] = lstReporttobeAdded.Items[j].Text.Split('|')[0];
                        }
                        beui.ReportCodeList = reportList;

                        ArrayList list = new ArrayList();

                        int icountrepcode = lstReporttobeAdded.Items.Count;
                        string[] reportListAdd = new string[icount];
                        for (int i = 0; i < icountrepcode; i++)
                        {
                            if (i == 0)
                            {
                                repcode = reportList[i];
                            }
                            else
                            {
                                repcode = repcode + "," + reportList[i];
                            }

                            //if (ddlRole.SelectedItem.Text != "Admin")
                            //{
                            //    if (reportList[i].IndexOf("A", 0) == 1)
                            //    {
                            //        list.Add(reportList[i]);
                            //    }
                            //}
                        }
                    }

                    

                    #region clientcode
                    //TODO:12/10 client code selection not req
                    //if (lstClientCodeDest.Items != null && lstClientCodeDest.Items.Count > 0)
                    //{
                    //    if (ddlRole.Text.ToLower() == "dh" || ddlRole.Text.ToLower() == "sdm")
                    //    {

                    //        string[] clientcodelist = service.GetBEClientCodeForPU(ddlPu.Text.Trim());
                    //        int icount = clientcodelist.Length;
                    //        if(icount > 0)
                    //        {
                    //        for (int j = 0; j < icount; j++)
                    //        {
                    //            if (j == 0)
                    //            {
                    //                ClientCode = clientcodelist[j].ToString().Trim();
                    //            }
                    //            else
                    //            {
                    //                ClientCode = ClientCode + "," + clientcodelist[j].ToString().Trim();
                    //            }
                    //        }
                    //        }
                    //        beclientui.ClientCodeList = clientcodelist;
                    //    }
                    //    else
                    //    {
                    //        int icount = lstClientCodeDest.Items.Count;
                    //        string[] ClientCodelist = new string[icount];

                    //        for (int j = 0; j < icount; j++)
                    //        {
                    //            //du = "";
                    //            ClientCodelist[j] = lstClientCodeDest.Items[j].Text.Trim();
                    //            if (j == 0)
                    //            {
                    //                ClientCode = lstClientCodeDest.Items[j].Text.Trim();
                    //            }
                    //            else
                    //            {
                    //                ClientCode = ClientCode + "," + lstClientCodeDest.Items[j].Text.Trim();
                    //            }
                    //        }
                    //        beclientui.ClientCodeList = ClientCodelist;
                    //    }

                    //}

                    #endregion

                    string Su = string .Empty;
                    string DMorSDM = rdbType.SelectedItem.Text;
                    string IsReadOnly = rdbIsReadOnly.SelectedItem.Value;
                    string AccessLevel = ddlAccessLevel.SelectedItem.Text;                   

                    beui.UserId = txtUserID.Text.Trim();
                    beclientui.UserId = txtUserID.Text.Trim();

                    if (beui.Role == "Anchor")
                    {
                        Su = rdbSU.SelectedItem.Text;
                        if (ddlAccessLevel.SelectedItem.Text == "Offering")
                        {
                            //string[] MCClist = new string[] { "All" };
                            //beui.MasterCustomerList = MCClist;

                            if (lstOfferingDestination.Items != null && lstOfferingDestination.Items.Count > 0)
                            {
                               
                                int icount = lstOfferingDestination.Items.Count;
                                string[] dulist = new string[icount];
                               
                                for (int j = 0; j < icount; j++)
                                {
                                    
                                    dulist[j] = lstOfferingDestination.Items[j].Text.Trim();
                                    if (j == 0)
                                    {
                                        du = lstOfferingDestination.Items[j].Text.Trim();
                                    }
                                    else
                                    {
                                        du = du + "," + lstOfferingDestination.Items[j].Text.Trim();
                                    }
                                }
                                beui.DuList = dulist;
                                
                            }

                            string[] MCClist = new string[] { "All" };
                            beui.MasterCustomerList = MCClist;

                            int icnt = MCClist.Length;
                            if (icnt > 0)
                            {
                                for (int j = 0; j < icnt; j++)
                                {
                                    if (j == 0)
                                        MCC = MCClist[0].ToString().Trim();
                                    else
                                        MCC = MCC + "," + MCClist[j].ToString().Trim();
                                }
                            }

                            service.InsertUserAccess(beui,MCC, du, repcode, Su, DMorSDM, IsReadOnly, AccessLevel);
                        }
                        else if (ddlAccessLevel.SelectedItem.Text == "MCC")
                        {
                            if (lstMCCDestination.Items != null && lstMCCDestination.Items.Count > 0)
                            {
                                int icount = lstMCCDestination.Items.Count;
                                string[] MCClist = new string[icount];

                                for (int j = 0; j < icount; j++)
                                {

                                    string a = lstMCCDestination.Items[j].Text.Trim();
                                    string[] a1 = a.Split('|');

                                    MCClist[j] = a1[0];
                                    if (j == 0)
                                    {
                                       
                                        MCC = a1[0];
                                    }
                                    else
                                    {
                                        
                                        MCC = MCC + "," + a1[0];
                                    }
                                }
                                beui.MasterCustomerList = MCClist;
                            }


                            //string[] PUlist = service.GetPUForMCC(MCC);
                            //beui.DuList = PUlist;
                            //int icnt = PUlist.Length;
                            //if (icnt > 0)
                            //{
                            //    for (int j = 0; j < icnt; j++)
                            //    {
                            //        if (j == 0)
                            //            du = PUlist[0].ToString().Trim();
                            //        else
                            //            du = du + "," + PUlist[j].ToString().Trim();
                            //    }
                            //}

                            // New logic
                            List<string> lstNSOMCC = new List<string>();
                            for (int i = 0; i < lstMCCDestination.Items.Count; i++)
                            { 
                                string text = lstMCCDestination.Items[i].Text.Trim();
                                lstNSOMCC.Add(text);
                            }
                            MCC = string.Join(",", lstNSOMCC);



                            service.InsertUserAccess(beui, MCC, du, repcode, Su, DMorSDM, IsReadOnly, AccessLevel);
                        }
                        else
                        {
                            string[] MCClist = new string[] { "All" };
                            beui.MasterCustomerList = MCClist;

                            int icnt = MCClist.Length;
                            if (icnt > 0)
                            {
                                for (int j = 0; j < icnt; j++)
                                {
                                    if (j == 0)
                                        MCC = MCClist[0].ToString().Trim();
                                    else
                                        MCC = MCC + "," + MCClist[j].ToString().Trim();
                                }
                            }

                            if (lstSDMDestination.Items != null && lstSDMDestination.Items.Count > 0)
                            {
                                int icount = lstSDMDestination.Items.Count;
                                string[] dulist = new string[icount];

                                for (int j = 0; j < icount; j++)
                                {

                                    dulist[j] = lstSDMDestination.Items[j].Text.Trim();
                                    if (j == 0)
                                    {
                                        du = lstSDMDestination.Items[j].Text.Trim();
                                    }
                                    else
                                    {
                                        du = du + "," + lstSDMDestination.Items[j].Text.Trim();
                                    }
                                }
                                beui.DuList = dulist;
                            }

                            service.InsertUserAccess(beui, MCC, du, repcode, Su, DMorSDM, IsReadOnly, AccessLevel);
                        }
                    }
                    else
                    {
                        AccessLevel = "Null";
                        DMorSDM = "Null";
                        IsReadOnly = "0";

                        string[] MCClist = new string[] { "All" };
                        beui.MasterCustomerList = MCClist;

                        int icnt = MCClist.Length;
                        if (icnt > 0)
                        {
                            for (int j = 0; j < icnt; j++)
                            {
                                if (j == 0)
                                    MCC = MCClist[0].ToString().Trim();
                                else
                                    MCC = MCC + "," + MCClist[j].ToString().Trim();
                            }
                        }

                        string[] PUlist = new string[] { "All" };
                        beui.DuList = PUlist;

                        int cnt = PUlist.Length;
                        if (cnt > 0)
                        {
                            for (int j = 0; j < cnt; j++)
                            {
                                if (j == 0)
                                    du = PUlist[0].ToString().Trim();
                                else
                                    du = du + "," + PUlist[j].ToString().Trim();
                            }
                        }

                        System.Text.StringBuilder str = new System.Text.StringBuilder();



                        if (ddlRole.SelectedItem.Text == "PnA")
                        {
                            Su = string.Empty;
                            Su = rdbSU.SelectedItem.Text;
                        }
                        else
                        {
                            
                            if (ddlRole.SelectedItem.Text == "UH" || ddlRole.SelectedItem.Text == "DH" || ddlRole.SelectedItem.Text == "SOH")
                            {
                                Su = "NA";
                            }
                            else
                            {
                                Su = rdbSU.SelectedItem.Text;
                            }
                        }
                      
                        service.InsertUserAccess(beui,MCC, du, repcode, Su, DMorSDM, IsReadOnly, AccessLevel);
                    }
                    

                    //TODO:12/10 client code selection not req
                    //                    service.InsertClientCodeList(beclientui, ClientCode);

                    ClearAllFields();
                    txtUserID.Text = string.Empty;
                    lblSuccess.Text = "Saved Successfully";
                    lblSuccess.ForeColor = System.Drawing.Color.Green;
                   // divDetails.Visible = false;

                }
                //btnSearch_Click(null,null);
                
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

        private bool ValidateFields()
        {
            bool isvalid = true;
            try
            {

                lblError.Text = string.Empty;

                if (string.IsNullOrWhiteSpace(txtUserID.Text))
                {
                    lblError.Text = lblError.Text + "Please Enter UserId. ";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    isvalid = false;
                }
                if (string.IsNullOrWhiteSpace(ddlRole.Text))
                {
                    lblError.Text = lblError.Text + "Please select Role. ";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    isvalid = false;
                }
                //if (lstReporttobeAdded.Items.Count == 0)
                //{                    
                //        lblError.Text = lblError.Text + "Please select Reports. ";
                //        isvalid = false;                  
                //}              
                if (ddlRole.SelectedValue == "Anchor")
                {
                    if (ddlAccessLevel.SelectedItem.Text == "Offering")
                    {
                        if (lstOfferingDestination == null)
                        {
                            lblError.Text = lblError.Text + "Please select Offering. ";
                            lblError.ForeColor = System.Drawing.Color.Red;
                            isvalid = false;
                        }
                        else if (lstOfferingDestination.Items.Count == 0)
                        {
                            lblError.Text = lblError.Text + "Please select Offerings. ";
                            lblError.ForeColor = System.Drawing.Color.Red;
                            isvalid = false;
                        }
                    }
                    else if (ddlAccessLevel.SelectedItem.Text == "MCC")
                    {
                        if (lstMCCDestination == null)
                        {
                            lblError.Text = lblError.Text + "Please select MCCs. ";
                            lblError.ForeColor = System.Drawing.Color.Red;
                            isvalid = false;
                        }
                        else if (lstMCCDestination.Items.Count == 0)
                        {
                            lblError.Text = lblError.Text + "Please select MCCs. ";
                            lblError.ForeColor = System.Drawing.Color.Red;
                            isvalid = false;
                        }
                    }

                    else
                    {
                        if (lstSDMDestination == null)
                        {
                            lblError.Text = lblError.Text + "Please select SDMs. ";
                            lblError.ForeColor = System.Drawing.Color.Red;
                            isvalid = false;
                        }
                        else if (lstSDMDestination.Items.Count == 0)
                        {
                            lblError.Text = lblError.Text + "Please select SDMs. ";
                            lblError.ForeColor = System.Drawing.Color.Red;
                            isvalid = false;
                        }
                    }
                    
                }               
                //TODO:12/10 client code selection not req
                //if (lstClientCodeDest == null)
                //{
                //    lblError.Text = lblError.Text + "Please select ClientCode. ";
                //    isvalid = false;
                //}
                //else if (lstClientCodeDest.Items.Count == 0)
                //{
                //    lblError.Text = lblError.Text + "Please select ClientCode. ";
                //    isvalid = false;
                //}

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
            return isvalid;

        }



        protected void btnApprove_Click(object sender, EventArgs e)
        {

        }


        protected void btnMCCAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string value = lstMCCSource.SelectedValue;
                if (!string.IsNullOrEmpty(value))
                {
                    lstMCCSource.Items.Remove(value);
                    lstMCCDestination.Items.Add(value);
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

        protected void btnMCCRemove_Click(object sender, EventArgs e)
        {
            try
            {
                string value = lstMCCDestination.SelectedValue;
                if (!string.IsNullOrEmpty(value))
                {
                    lstMCCSource.Items.Add(value);
                    lstMCCDestination.Items.Remove(value);
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

        protected void btnMCCAddAll_Click(object sender, EventArgs e)
        {

            try
            {
                int count = lstMCCSource.Items.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        lstMCCDestination.Items.Add(lstMCCSource.Items[i].Value);

                    }

                    lstMCCSource.Items.Clear();
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

        protected void btnMCCRemoveAll_Click(object sender, EventArgs e)
        {

            try
            {
                int count = lstMCCDestination.Items.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        lstMCCSource.Items.Add(lstMCCDestination.Items[i].Value);

                    }

                    lstMCCDestination.Items.Clear();
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


        //Ganapathy July13

        #region clientCodeallselect
        //TODO:12/10 client code selection not req
        //protected void btnClentCodeAll_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int count = lstClientCodeSource.Items.Count;
        //        if (count > 0)
        //        {
        //            for (int i = 0; i < count; i++)
        //            {
        //                lstClientCodeDest.Items.Add(lstClientCodeSource.Items[i].Value);

        //            }

        //            lstClientCodeSource.Items.Clear();
        //        }
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

        //protected void btnClientAdd_Click(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        string value = lstClientCodeSource.SelectedValue;
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            lstClientCodeSource.Items.Remove(value);
        //            lstClientCodeDest.Items.Add(value);
        //        }
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

        //protected void btnClientRemove_Click(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        string value = lstClientCodeDest.SelectedValue;
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            lstClientCodeSource.Items.Add(value);
        //            lstClientCodeDest.Items.Remove(value);
        //        }
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

        //protected void btnClientCodeRemoveAll_Click(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        int count = lstClientCodeDest.Items.Count;
        //        if (count > 0)
        //        {
        //            for (int i = 0; i < count; i++)
        //            {
        //                lstClientCodeSource.Items.Add(lstClientCodeDest.Items[i].Value);

        //            }

        //            lstClientCodeDest.Items.Clear();
        //        }
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


        #endregion

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            rdbSU.ClearSelection();
            chkSU.ClearSelection();
            divSu1.Attributes.Add("style", "display:block");
            //divSu2.Attributes.Add("style", "display:none");
            SUTypeIsReadOnlyMachineUserId = service.GetSUTypeIsReadOnly(Session["MachineUserid"].ToString());
           // SUTypeIsReadOnly = service.GetSUTypeIsReadOnly(txtUserID.Text);
            if (SUTypeIsReadOnlyMachineUserId[0].ToString() == "ORC")
            {
                if (ddlRole.SelectedItem.Text == "Admin" || ddlRole.SelectedItem.Text == "UH")
                {
                    rdbSU.Items.Clear();
                    rdbSU.Items.Add("ORC");
                    //rdbSU.Items.Add("SAP");
                    rdbSU.Items.Add("All");
                    rdbSU.SelectedIndex = 0;
                    rdbSU.Items[1].Enabled = true;
                }

                else
                {
                    rdbSU.Items.Clear();
                    rdbSU.Items.Add("ORC");
                    //rdbSU.Items.Add("SAP");
                    rdbSU.SelectedIndex = 0;
                }

                if (ddlRole.SelectedItem.Text == "Anchor")
                {
                    divDetails.Visible = true;
                    divForAccessLevel.Visible = true;
                    if (ddlAccessLevel.SelectedItem.Text == "Offering")
                    {
                        divForOfferings.Visible = true;
                        divForMcc.Visible = false;
                        divForSDM.Visible = false;

                        string SU = rdbSU.SelectedItem.Text;
                        lstMapping = service.GetBEPUMappingAll(SU, txtUserID.Text);
                        lstOffering.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstOffering.DataBind();

                        lstMapping = service.GetBEPUMapping(txtUserID.Text, SU);
                        lstOfferingDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstOfferingDestination.DataBind();
                    }
                    else if (ddlAccessLevel.SelectedItem.Text == "MCC")
                    {
                        divForOfferings.Visible = false;
                        divForMcc.Visible = true;
                        divForSDM.Visible = false;

                        DataSet dsmccall = service.GetMccMappingAll(txtUserID.Text, rdbSU.SelectedValue);
                        //lstMCCSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstMCCSource.DataSource = dsmccall.Tables[0];
                        lstMCCSource.DataTextField = "MCCNSO";
                        lstMCCSource.DataValueField = "MCCNSO";
                        lstMCCSource.DataBind();
                    }
                    else
                    {
                        divForOfferings.Visible = false;
                        divForMcc.Visible = false;
                        divForSDM.Visible = true;

                        lstMapping = service.GetSDMMappingAll(txtUserID.Text, ddlAccessLevel.SelectedItem.Text, rdbSU.SelectedValue);
                        lstSDMSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstSDMSource.DataBind();

                        lstMapping = service.GetSDMMapping(txtUserID.Text, rdbSU.SelectedValue);
                        lstSDMDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstSDMDestination.DataBind();
                    }
                }
                else
                {
                    divForMcc.Visible = false;
                    divForOfferings.Visible = false;
                    divForAccessLevel.Visible = false;
                    divDetails.Visible = false;
                    divForSDM.Visible = false;
                }
            }
            else if (SUTypeIsReadOnlyMachineUserId[0].ToString() == "SAP")
            {
                if (ddlRole.SelectedItem.Text == "Admin" || ddlRole.SelectedItem.Text == "UH")
                {
                    rdbSU.Items.Clear();
                    //rdbSU.Items.Add("ORC");
                    rdbSU.Items.Add("SAP");
                    //rdbSU.Items.Add("All");
                    rdbSU.SelectedIndex = 0;
                    rdbSU.Items[1].Enabled = true;
                }

                else
                {
                    rdbSU.Items.Clear();
                    //rdbSU.Items.Add("ORC");
                    rdbSU.Items.Add("SAP");
                    rdbSU.SelectedIndex = 0;
                }

                if (ddlRole.SelectedItem.Text == "Anchor")
                {
                    divDetails.Visible = true;
                    divForAccessLevel.Visible = true;
                    if (ddlAccessLevel.SelectedItem.Text == "Offering")
                    {
                        divForOfferings.Visible = true;
                        divForMcc.Visible = false;
                        divForSDM.Visible = false;

                        string SU = rdbSU.SelectedItem.Text;
                        lstMapping = service.GetBEPUMappingAll(SU, txtUserID.Text);
                        lstOffering.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstOffering.DataBind();

                        lstMapping = service.GetBEPUMapping(txtUserID.Text, SU);
                        lstOfferingDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstOfferingDestination.DataBind();
                    }
                    else if (ddlAccessLevel.SelectedItem.Text == "MCC")
                    {
                        divForOfferings.Visible = false;
                        divForMcc.Visible = true;
                        divForSDM.Visible = false;

                        DataSet dsmccall = service.GetMccMappingAll(txtUserID.Text, rdbSU.SelectedValue);
                        //lstMCCSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstMCCSource.DataSource = dsmccall.Tables[0];
                        lstMCCSource.DataTextField = "MCCNSO";
                        lstMCCSource.DataValueField = "MCCNSO";
                        lstMCCSource.DataBind();
                    }
                    else
                    {
                        divForOfferings.Visible = false;
                        divForMcc.Visible = false;
                        divForSDM.Visible = true;

                        lstMapping = service.GetSDMMappingAll(txtUserID.Text, ddlAccessLevel.SelectedItem.Text, rdbSU.SelectedValue);
                        lstSDMSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstSDMSource.DataBind();

                        lstMapping = service.GetSDMMapping(txtUserID.Text, rdbSU.SelectedValue);
                        lstSDMDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstSDMDestination.DataBind();
                    }
                }
                else
                {
                    divForMcc.Visible = false;
                    divForOfferings.Visible = false;
                    divForAccessLevel.Visible = false;
                    divDetails.Visible = false;
                    divForSDM.Visible = false;
                }
            }
            if (SUTypeIsReadOnlyMachineUserId[0].ToString() == "ECAS")
            {
                if (ddlRole.SelectedItem.Text == "Admin" || ddlRole.SelectedItem.Text == "UH")
                {
                    rdbSU.Items.Clear();
                    rdbSU.Items.Add("ECAS");
                    //rdbSU.Items.Add("SAP");
                    rdbSU.Items.Add("All");
                    rdbSU.SelectedIndex = 0;
                    rdbSU.Items[1].Enabled = true;
                }

                else
                {
                    rdbSU.Items.Clear();
                    rdbSU.Items.Add("ECAS");
                    //rdbSU.Items.Add("SAP");
                    rdbSU.SelectedIndex = 0;
                }

                if (ddlRole.SelectedItem.Text == "Anchor")
                {
                    divDetails.Visible = true;
                    divForAccessLevel.Visible = true;
                    if (ddlAccessLevel.SelectedItem.Text == "Offering")
                    {
                        divForOfferings.Visible = true;
                        divForMcc.Visible = false;
                        divForSDM.Visible = false;

                        string SU = rdbSU.SelectedItem.Text;
                        lstMapping = service.GetBEPUMappingAll(SU, txtUserID.Text);
                        lstOffering.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstOffering.DataBind();

                        lstMapping = service.GetBEPUMapping(txtUserID.Text, SU);
                        lstOfferingDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstOfferingDestination.DataBind();
                    }
                    else if (ddlAccessLevel.SelectedItem.Text == "MCC")
                    {
                        divForOfferings.Visible = false;
                        divForMcc.Visible = true;
                        divForSDM.Visible = false;

                        DataSet dsmccall = service.GetMccMappingAll(txtUserID.Text, rdbSU.SelectedValue);
                        //lstMCCSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstMCCSource.DataSource = dsmccall.Tables[0];
                        lstMCCSource.DataTextField = "MCCNSO";
                        lstMCCSource.DataValueField = "MCCNSO";
                        lstMCCSource.DataBind();
                    }
                    else
                    {
                        divForOfferings.Visible = false;
                        divForMcc.Visible = false;
                        divForSDM.Visible = true;

                        lstMapping = service.GetSDMMappingAll(txtUserID.Text, ddlAccessLevel.SelectedItem.Text, rdbSU.SelectedValue);
                        lstSDMSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstSDMSource.DataBind();

                        lstMapping = service.GetSDMMapping(txtUserID.Text, rdbSU.SelectedValue);
                        lstSDMDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstSDMDestination.DataBind();
                    }
                }
                else
                {
                    divForMcc.Visible = false;
                    divForOfferings.Visible = false;
                    divForAccessLevel.Visible = false;
                    divDetails.Visible = false;
                    divForSDM.Visible = false;
                }
            }

            if (SUTypeIsReadOnlyMachineUserId[0].ToString() == "EAIS")
            {
                if (ddlRole.SelectedItem.Text == "Admin" || ddlRole.SelectedItem.Text == "UH")
                {
                    rdbSU.Items.Clear();
                    rdbSU.Items.Add("EAIS");
                    //rdbSU.Items.Add("SAP");
                    rdbSU.Items.Add("All");
                    rdbSU.SelectedIndex = 0;
                    rdbSU.Items[1].Enabled = true;
                }

                else
                {
                    rdbSU.Items.Clear();
                    rdbSU.Items.Add("EAIS");
                    //rdbSU.Items.Add("SAP");
                    rdbSU.SelectedIndex = 0;
                }

                if (ddlRole.SelectedItem.Text == "Anchor")
                {
                    divDetails.Visible = true;
                    divForAccessLevel.Visible = true;
                    if (ddlAccessLevel.SelectedItem.Text == "Offering")
                    {
                        divForOfferings.Visible = true;
                        divForMcc.Visible = false;
                        divForSDM.Visible = false;

                        string SU = rdbSU.SelectedItem.Text;
                        lstMapping = service.GetBEPUMappingAll(SU, txtUserID.Text);
                        lstOffering.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstOffering.DataBind();

                        lstMapping = service.GetBEPUMapping(txtUserID.Text, SU);
                        lstOfferingDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstOfferingDestination.DataBind();
                    }
                    else if (ddlAccessLevel.SelectedItem.Text == "MCC")
                    {
                        divForOfferings.Visible = false;
                        divForMcc.Visible = true;
                        divForSDM.Visible = false;

                        DataSet dsmccall = service.GetMccMappingAll(txtUserID.Text, rdbSU.SelectedValue);
                        //lstMCCSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstMCCSource.DataSource = dsmccall.Tables[0];
                        lstMCCSource.DataTextField = "MCCNSO";
                        lstMCCSource.DataValueField = "MCCNSO";
                        lstMCCSource.DataBind();
                    }
                    else
                    {
                        divForOfferings.Visible = false;
                        divForMcc.Visible = false;
                        divForSDM.Visible = true;

                        lstMapping = service.GetSDMMappingAll(txtUserID.Text, ddlAccessLevel.SelectedItem.Text, rdbSU.SelectedValue);
                        lstSDMSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstSDMSource.DataBind();

                        lstMapping = service.GetSDMMapping(txtUserID.Text, rdbSU.SelectedValue);
                        lstSDMDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstSDMDestination.DataBind();
                    }
                }
                else
                {
                    divForMcc.Visible = false;
                    divForOfferings.Visible = false;
                    divForAccessLevel.Visible = false;
                    divDetails.Visible = false;
                    divForSDM.Visible = false;
                }
            }

           // else
            {
                if (ddlRole.SelectedItem.Text == "Admin" || ddlRole.SelectedItem.Text == "UH")
                {
                    rdbSU.Items.Clear();
                    //rdbSU.Items.Add("ORC");
                    rdbSU.Items.Add("SAP");
                    //rdbSU.Items.Add("ECAS");
                    //rdbSU.Items.Add("EAIS");
                    //rdbSU.Items.Add("All");
                    rdbSU.SelectedIndex = 0;
                    rdbSU.Items[1].Enabled = true;
                }

                else
                {
                    rdbSU.Items.Clear();
                    //rdbSU.Items.Add("ORC");
                    rdbSU.Items.Add("SAP");
                    //rdbSU.Items.Add("ECAS");
                    //rdbSU.Items.Add("EAIS");
                    rdbSU.SelectedIndex = 0;

                    if (ddlRole.SelectedItem.Text == "PnA")
                    {
                        divSu1.Attributes.Add("style", "display:block");
                        //divSu2.Attributes.Add("style", "display:none");
                    }
                }

                if (ddlRole.SelectedItem.Text == "Anchor")
                {
                    divDetails.Visible = true;
                    divForAccessLevel.Visible = true;
                    if (ddlAccessLevel.SelectedItem.Text == "Offering")
                    {
                        divForOfferings.Visible = true;
                        divForMcc.Visible = false;
                        divForSDM.Visible = false;

                        string SU = rdbSU.SelectedItem.Text;
                        lstMapping = service.GetBEPUMappingAll(SU, txtUserID.Text);
                        lstOffering.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstOffering.DataBind();

                        lstMapping = service.GetBEPUMapping(txtUserID.Text, SU);
                        lstOfferingDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstOfferingDestination.DataBind();
                    }
                    else if (ddlAccessLevel.SelectedItem.Text == "MCC")
                    {
                        divForOfferings.Visible = false;
                        divForMcc.Visible = true;
                        divForSDM.Visible = false;

                        DataSet dsmccall = service.GetMccMappingAll(txtUserID.Text, rdbSU.SelectedValue);
                        //lstMCCSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstMCCSource.DataSource = dsmccall.Tables[0];
                        lstMCCSource.DataTextField = "MCCNSO";
                        lstMCCSource.DataValueField = "MCCNSO";
                        lstMCCSource.DataBind();
                    }
                    else
                    {
                        divForOfferings.Visible = false;
                        divForMcc.Visible = false;
                        divForSDM.Visible = true;

                        lstMapping = service.GetSDMMappingAll(txtUserID.Text, ddlAccessLevel.SelectedItem.Text, rdbSU.SelectedValue);
                        lstSDMSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstSDMSource.DataBind();

                        lstMapping = service.GetSDMMapping(txtUserID.Text, rdbSU.SelectedValue);
                        lstSDMDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                        lstSDMDestination.DataBind();
                    }
                }
                else
                {
                    divForMcc.Visible = false;
                    divForOfferings.Visible = false;
                    divForAccessLevel.Visible = false;
                    divDetails.Visible = false;
                    divForSDM.Visible = false;
                }
            }

            HideServiceLine();
        }

        private void HideServiceLine()
        {
            if (ddlRole.Items.Count > 0)
            {

                if (ddlRole.SelectedItem.Text == "UH" || ddlRole.SelectedItem.Text == "DH" || ddlRole.SelectedItem.Text == "SOH")
                {
                    tdSU.Attributes.Add("style", "display:none");
                }
                else
                {
                    tdSU.Attributes.Add("style", "display:block");
                }
            }
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool isSuccess;
                bool isclientsuccess;
                if (!string.IsNullOrWhiteSpace(txtUserID.Text))
                {
                    DataSet ds = new DataSet();
                    ds = service.GetRoleForUser(txtUserID.Text.Trim());

                    isSuccess = service.DeleteBEUserAccess(txtUserID.Text.Trim());
                    isclientsuccess = service.DeleteBEClientUserAccess(txtUserID.Text.Trim());
                    ClearAllFields();
                   
                    if (ds.Tables[0].Rows[0]["Role"].ToString() == "Anchor")
                    {
                        if (isSuccess && isclientsuccess)
                        {
                            lblSuccess.Text = "Succesfully deleted access for " + txtUserID.Text;
                            lblSuccess.ForeColor = System.Drawing.Color.Green;
                            lblSuccess.Visible = true;
                        }
                        else
                        {
                            lblError.Text = "No record exists for user " + txtUserID.Text;
                            lblError.ForeColor = System.Drawing.Color.Red;
                            lblError.Visible = true;
                        }
                    }
                    else
                    {
                        if (isSuccess)
                        {
                            lblSuccess.Text = "Succesfully deleted access for " + txtUserID.Text;
                            lblSuccess.ForeColor = System.Drawing.Color.Green;
                            lblSuccess.Visible = true;
                        }
                        else
                        {
                            lblError.Text = "No record exists for user " + txtUserID.Text;
                            lblError.ForeColor = System.Drawing.Color.Red;
                            lblError.Visible = true;
                        }
                    }
                    
                    txtUserID.Text = string.Empty;
                }
                else
                {
                    lblError.Text = "Please Enter UserID";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    lblError.Visible = true;
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

        protected void btnReportAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string value = lstReportList.SelectedValue;
                if (!string.IsNullOrEmpty(value))
                {
                    //string[] reportcode = value.Split('|');
                    //if (reportcode.Length == 2)
                    //{
                    //    //lstSourceReports.Items.Remove(value);
                    //    if(lstDestinationReports.Items.FindByText(reportcode[0]) == null)
                    //        lstDestinationReports.Items.Add(reportcode[0]);
                    //}
                    lstReporttobeAdded.Items.Add(value);
                    lstReportList.Items.Remove(value);
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

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                string value = lstReporttobeAdded.SelectedValue;
                if (!string.IsNullOrEmpty(value))
                {
                    lstReportList.Items.Add(value);
                    lstReporttobeAdded.Items.Remove(value);
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

        protected void btnAddAllReport_Click(object sender, EventArgs e)
        {
            try
            {
                int count = lstReportList.Items.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        lstReporttobeAdded.Items.Add(lstReportList.Items[i].Value);

                    }

                    lstReportList.Items.Clear();
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

        protected void btnRemoveAllReports_Click(object sender, EventArgs e)
        {
            try
            {
                int count = lstReporttobeAdded.Items.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        lstReportList.Items.Add(lstReporttobeAdded.Items[i].Value);

                    }

                    lstReporttobeAdded.Items.Clear();
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

        protected void btnPUAddAll_Click(object sender, EventArgs e)
        {
            try
            {
                int count = lstOffering.Items.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {                       
                        lstOfferingDestination.Items.Add(lstOffering.Items[i].Value);
                    }

                    lstOffering.Items.Clear();
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

        protected void btnPUAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string value = lstOffering.SelectedItem.Text;
                if (!string.IsNullOrEmpty(value))
                {                                       
                    lstOfferingDestination.Items.Add(value);
                    lstOffering.Items.Remove(value);
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

        protected void btnPURemove_Click(object sender, EventArgs e)
        {
            try
            {
                string value = lstOfferingDestination.SelectedValue;
                if (!string.IsNullOrEmpty(value))
                {                    
                    lstOffering.Items.Add(value);
                    lstOfferingDestination.Items.Remove(value);
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

        protected void btnPURemoveAll_Click(object sender, EventArgs e)
        {
            try
            {
                int count = lstOfferingDestination.Items.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {                        
                        lstOffering.Items.Add(lstOfferingDestination.Items[i].Value);
                    }                   
                    lstOfferingDestination.Items.Clear();
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

        protected void btnSDMAddAll_Click(object sender, EventArgs e)
        {
            try
            {
                int count = lstSDMSource.Items.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        lstSDMDestination.Items.Add(lstSDMSource.Items[i].Value);
                    }

                    lstSDMSource.Items.Clear();
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

        protected void btnSDMAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string value = lstSDMSource.SelectedItem.Text;
                if (!string.IsNullOrEmpty(value))
                {
                    lstSDMDestination.Items.Add(value);
                    lstSDMSource.Items.Remove(value);
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

        protected void btnSDMRemove_Click(object sender, EventArgs e)
        {
            try
            {
                string value = lstSDMDestination.SelectedValue;
                if (!string.IsNullOrEmpty(value))
                {
                    lstSDMSource.Items.Add(value);
                    lstSDMDestination.Items.Remove(value);
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

        protected void btnSDMRemoveAll_Click(object sender, EventArgs e)
        {
            try
            {
                int count = lstSDMDestination.Items.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        lstSDMSource.Items.Add(lstSDMDestination.Items[i].Value);
                    }
                    lstSDMDestination.Items.Clear();
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

        protected void rdbSU_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstOfferingDestination.Items.Clear(); 

            if (ddlAccessLevel.SelectedItem.Text.ToString() == "Offering")
            {
                string SU = rdbSU.SelectedItem.Text;
                lstMapping = service.GetBEPUMappingAll(SU, txtUserID.Text);
                lstOffering.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstOffering.DataBind();

                lstMapping = service.GetBEPUMapping(txtUserID.Text, SU);
                lstOfferingDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstOfferingDestination.DataBind();
            }
            else if (ddlAccessLevel.SelectedItem.Text.ToString() == "MCC")
            {
                lstMapping = service.GetMccMapping(txtUserID.Text, rdbSU.SelectedValue);
                lstMCCDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstMCCDestination.DataBind();

                DataSet dsmccall = service.GetMccMappingAll(txtUserID.Text, rdbSU.SelectedValue);
                //lstMCCSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstMCCSource.DataSource = dsmccall.Tables[0];
                lstMCCSource.DataTextField = "MCCNSO";
                lstMCCSource.DataValueField = "MCCNSO";
                lstMCCSource.DataBind();
            }
            else
            {
                lstMapping = service.GetSDMMappingAll(txtUserID.Text, ddlAccessLevel.SelectedItem.Text, rdbSU.SelectedValue);
                lstSDMSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstSDMSource.DataBind();

                lstMapping = service.GetSDMMapping(txtUserID.Text, rdbSU.SelectedValue);
                lstSDMDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstSDMDestination.DataBind();
            }
        }

        protected void lstOfferingDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void ddlAccessLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAccessLevel.SelectedItem.Text == "Offering")
            {
                divForOfferings.Visible = true;
                divForMcc.Visible = false;
                divForSDM.Visible = false;
                string SU = rdbSU.SelectedItem.Text; 

                lstMapping = service.GetBEPUMappingAll(SU, txtUserID.Text);
                lstOffering.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstOffering.DataBind();

                lstMapping = service.GetBEPUMapping(txtUserID.Text, SU);
                lstOfferingDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstOfferingDestination.DataBind();
            }
            else if (ddlAccessLevel.SelectedItem.Text == "MCC")
            {
                divForMcc.Visible = true;
                divForOfferings.Visible = false;
                divForSDM.Visible = false;
                lstMapping = service.GetMccMapping(txtUserID.Text, rdbSU.SelectedValue);
                lstMCCDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstMCCDestination.DataBind();

                DataSet dsmccall = service.GetMccMappingAll(txtUserID.Text, rdbSU.SelectedValue);
                //lstMCCSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstMCCSource.DataSource = dsmccall.Tables[0];
                lstMCCSource.DataTextField = "MCCNSO";
                lstMCCSource.DataValueField = "MCCNSO";
                lstMCCSource.DataBind();
            }
            else
            {
                divForOfferings.Visible = false;
                divForMcc.Visible = false;
                divForSDM.Visible = true;

                lstMapping = service.GetSDMMappingAll(txtUserID.Text, ddlAccessLevel.SelectedItem.Text,rdbSU.SelectedValue);
                lstSDMSource.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstSDMSource.DataBind();

                lstMapping = service.GetSDMMapping(txtUserID.Text, rdbSU.SelectedValue);
                lstSDMDestination.DataSource = lstMapping.Select(k => k.PU).Distinct().ToList();
                lstSDMDestination.DataBind();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtUserID.Enabled = true;
            txtUserID.Text = "";
            //divForButtons.Visible = true;
        }
        
        
    }
