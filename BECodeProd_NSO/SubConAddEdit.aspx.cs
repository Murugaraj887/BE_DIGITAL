using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BEData;
using BEData.BusinessEntity;
using System.Data;
using BECodeProd;





public partial class SubConAddEdit : BasePage
{



    string globalDateTimeFormat = "MM/dd/yyyy";
    BEDL service = new BEDL();

    Logger logger = new Logger();
    string fileName = "BEData.SubConAddEdit.cs";

    bool isAddNew = true;
    string commaseperatedColumns = "";

    List<ExpenseTemplateData> lstSettings = new List<ExpenseTemplateData>();
    MasterEntity dataEntity = new MasterEntity();


    // very carefull in this object  (important ) * n
    List<string> lstVisaTypes = new List<string>();


    protected void Page_Load(object sender, EventArgs e)
    {
        base.ValidateSession();

        try
        {
            if (Page.IsPostBack)
            {
                // during postback :(
            }
            else
            {
                // onload

                List<VisaCountry> lstVisa = new List<VisaCountry>();
                lstVisa = service.GetVisaCountryMapping();

                string csvVisa = string.Empty;
                foreach (var item in lstVisa)
                    csvVisa += item.Country + "," + item.VisaType + "|";

                csvVisa = csvVisa.Trim().TrimEnd('|').TrimStart('|');
                hdnfldVisaWalaCSV.Value = csvVisa;


                string pucode = string.Empty;
                string dMcode = string.Empty;
                string ExpenseType = string.Empty;
                string quareter = string.Empty;
                string currentTemplate = string.Empty;

                lstVisaTypes = new List<string>();


                hypadd.OnClientClick = " return isValidClick();";

                #region Freezing


                string machineUserID = HttpContext.Current.User.Identity.Name;

                string[] userids = machineUserID.Split('\\');
                if (userids.Length == 2)
                    machineUserID = userids[1];

                bool isMachineUserisAdmin = service.GetRolee(machineUserID) == "Admin";


                btnSave.Enabled = true;
                btnSave.ToolTip = "";
                //bool isFreezed = Session[Constants.IsFreezed] + "" == "1" ? true : false;
                //if (isFreezed)
                //{
                //    if (!isMachineUserisAdmin)
                //    {
                //        btnSave.Enabled = false;
                //        btnSave.ToolTip = Session[Constants.FreezedText] + "";
                //    }
                //}
                #endregion

              


                string userID = Session["UserID"] + "";
                string mode = Request.QueryString["Mode"] + "";
                // LoadCombobox();


                btnSave.Visible = true;

                if (mode == "OtherExp")
                {
                    btnSave.Visible = false;
                    Divminddetails.Visible = true;
                    DivAddEditInfo.Visible = false;
                    hdnfldddlDM.Value = ddlDM.Text;
                    LoadCombobox(pucode,dMcode,ExpenseType, quareter);


                }
                else if (mode == "AddEdit")
                {


                    hypadd.Visible = false;
                    hypRefresh.Visible = false;
                    ddlExpenseType.Enabled = false;

                    //pucode = Request.QueryString["PU"] + "";
                    //dMcode = Request.QueryString["DU"] + "";
                    ExpenseType = Request.QueryString["ExpenseType"] + "";
                    quareter = "Current";
                    isAddNew = Request.QueryString["IsAddNew"] + "" == "1";






                    int Expid = Convert.ToInt32(Request.QueryString["Expid"] + "");
                    hdnfldKey.Value = Expid + "";

                    if (Expid != 0)
                    {
                        MasterEntity editItemData = service.GetEditExpenseData(Expid);
                        pucode = editItemData.PUCode;
                        dMcode = editItemData.DMMailId;
                        hdnfldddlDM.Value = dMcode;
                        ExpenseType = editItemData.ExpType;
                        //quareter = editItemData.CurrQtr;
                        quareter = "Current";
                    }


                    currentTemplate = (service.GetTemplateID(ExpenseType) + "").Trim();

                    

                    LoadCombobox( pucode,dMcode,ExpenseType, quareter);

                    Process(ExpenseType, quareter, Expid, isAddNew, currentTemplate);

                    ViewAddEdit(Expid);

                }
                bool isFreezed = Session[Constants.IsFreezed] + "" == "1" ? true : false;
                if (isFreezed)
                {
                    if (!isMachineUserisAdmin)
                    {
                        btnSave.Enabled = false;
                        btnSave.ToolTip = Session[Constants.FreezedText] + "";
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

    private void ViewAddEdit(int Expid)
    {
        bool isView = Request.QueryString["view"] + "" == "1";
        btnSave.Visible = !isView;

        DateTime todaydate = DateTime.Now;
        string lastqtr2 = "";
        string currqtr2 = "";
        int year = todaydate.Year - 2000;
        int nextyear = year + 1;
        string cmdstrquarter = "SELECT [strquarter]   FROM [ExpConfig]";
        DataSet ds = service.GetDataSet(cmdstrquarter);
        DataTable dt = ds.Tables[0];
        string strquarter = dt.Rows[0][0].ToString();
        if (strquarter == "Q1")
        {
            lastqtr2 = "Q4'" + year;
            currqtr2 = "Q1'" + nextyear;
        }

        if (strquarter == "Q2")
        {
            lastqtr2 = "Q1'" + nextyear;
            currqtr2 = "Q2'" + nextyear;
        }
        if (strquarter == "Q3")
        {
            lastqtr2 = "Q2'" + nextyear;
            currqtr2 = "Q3'" + nextyear;
        }
        if (strquarter == "Q4")
        {
            lastqtr2 = "Q3'" + nextyear;
            currqtr2 = "Q4'" + nextyear;
        }
        if (isView)
        {
            ddlQuarter.Items.Clear();
            string currentQtr = currqtr2;
            //ddlQuarter.Items.Add(currentQtr);

            currentQtr = DateUtility.GetQuarter("current");
            string nextQtr = DateUtility.GetQuarter("next");
            string nextQtrPlus1 = DateUtility.GetQuarter("next1");


            ddlQuarter.Items.Insert(0, currentQtr);
            ddlQuarter.Items.Insert(1, nextQtr);
            ddlQuarter.Items.Insert(2, nextQtrPlus1);
            //ddlQuarter.Text = currentQtr;

            ddlQuarter.Items.Insert(0, (service.GetEditExpenseData(Expid)).CurrQtr);
            ddlQuarter.Enabled = false;
            ddlExpenseType.Enabled = false;
            ddlPU.Enabled = false;
            ddlDM.Enabled = false;

            //foreach (TextBox dr in this.Page.Form.Controls.OfType<TextBox>())
            //{
            //    dr.Enabled = false;
            //}

            //foreach (DropDownList dr in this.Page.Form.Controls.OfType<DropDownList>())
            //{
            //    dr.Enabled = false;
            //}

        }
        else
        {
            ddlQuarter.Enabled = false;
            ddlExpenseType.Enabled = false;
            ddlPU.Enabled = false;
            ddlDM.Enabled = false;
        }
        if (isAddNew)
        {
            ddlQuarter.Enabled = true;
            ddlPU.Enabled = true;
            ddlDM.Enabled = true;
        }
    }

    //[System.Web.Services.WebMethod]
    //public static string GetCurrentTime(string name)
    //{
    //    return "Hello " + name + Environment.NewLine + "The Current Time is: "
    //        + DateTime.Now.ToString();
    //}


    //[System.Web.Services.WebMethod]
    //public static string PopulateDMServer(string value)
    //{
    //    string returnValue = "";

    //    string pu = value;
    //    if (pu.ToLower() == "all")
    //    {
    //        var DMs = lstPUDM.Select(k => k.DM).Distinct().OrderBy(k => k).ToList();
    //        foreach (string dm in DMs)
    //            returnValue += dm + ",";
    //    }
    //    else
    //    {

    //        var DMs = lstPUDM.Where(k => k.PU == pu).Select(k => k.DM).Distinct().OrderBy(k => k).ToList();
    //        foreach (string dm in DMs)
    //            returnValue += dm + ",";
    //    }
    //    returnValue = returnValue.Trim().TrimEnd(',').TrimStart(',');
    //    return returnValue;
    //}


    //[System.Web.Services.WebMethod]
    //public static string PopulateVisaTypeServer(string country, string visawp)
    //{
    //    string returnValue = "";
    //    visawp = visawp.ToLower().StartsWith("visa") ? "visa" : "wp";
    //    List<VisaCountry> lstVisa = new List<VisaCountry>();
    //    lstVisa = new ExpenseDL().GetVisaCountryMapping();
    //    country = country.ToLower();
    //    //var result = lstVisa.Where(k => k.VisaWP.ToLower() == visawp).Where(k => k.Country.ToLower() == country).Select(k => k.VisaType).ToList();
    //    var result = lstVisa.Where(k => k.Country.ToLower() == country).Select(k => k.VisaType).ToList();

    //    foreach (string item in result)
    //        returnValue += item + ",";

    //    returnValue = returnValue.Trim().TrimEnd(',').TrimStart(',');

    //    return returnValue;
    //}




    public void Process(string ExpenseType, string quareter, int _Expid, bool isAddNew, string currentTemplate)
    {
        try
        {
            string userID = Session["UserID"] + "";
            currentTemplate = currentTemplate.Trim();
            btnSave.Visible = true;
            if (quareter.ToLower() == "previous" || quareter.ToLower() == "last" || quareter.ToLower() == "total")
                btnSave.Visible = false;
            bool isView = Request.QueryString["view"] + "" == "1";



            if (!isAddNew)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "dpExcept", "dpExcept();", true);
                // edit operation 
                dataEntity = service.GetEditExpenseData(_Expid);

                var EntityProperties = typeof(MasterEntity).GetProperties();

                foreach (Control ctrl in divHiddenFieldControls.Controls)
                {
                    if (ctrl is HiddenField)
                    {
                        HiddenField hiddenField = ctrl as HiddenField;
                        string hidenID = hiddenField.ID.Replace("hdnfld", "");//

                        hidenID = hidenID.Substring(3);
                        // .Replace("int", "").Replace("txt", "").Replace("lst", "").Replace("dtp", "");

                        var temp = EntityProperties.SingleOrDefault(k => (k.Name) == hidenID);
                        if (temp != null)
                            hiddenField.Value = temp.GetValue(dataEntity, null) + "";
                    }
                   
                }
                ddlQuarter.Items.Insert(0, dataEntity.CurrQtr);
            }
            else
            {
                // add new operation 
                ClientScript.RegisterStartupScript(this.GetType(), "dpExcept", "dpExcept();", true);


            }

            //string userSU = Session["UserSU"] + "";

            lstSettings = service.GetExpenseTemplateData().Where(k => k.ExpTemplateId.ToLower().Trim() == currentTemplate.ToLower().Trim() && k.IsActive).ToList();

           
            if (ddlPU.Text == "ECSPPS")
                lstSettings.Where(k => k.ColumnName == "FieldList10").ToList().ForEach(k => k.IsMandatory = true);
            else
                lstSettings.Where(k => k.ColumnName == "FieldList10").ToList().ForEach(k => k.IsEditable = false);


            List<ExpenseColumns> lstAllColumns = service.GetExpenseColumnsEntity();



            string commaSeperatedDateColumns = "";
            Action<string> processDateColumns = (k1) =>
            {
                string contorlname = lstAllColumns.SingleOrDefault(k => k.ColumnName == k1).ControlName;
                commaSeperatedDateColumns += contorlname + ",";
            };
            commaSeperatedDateColumns = commaSeperatedDateColumns.Trim().TrimEnd(',').TrimStart(',');
            lstSettings.Where(k => k.ColumnType.ToLower() == "date").Select(k => k.ColumnName).ToList().ForEach(processDateColumns);


            commaseperatedColumns = "";
            Action<string> process = (k1) =>
            {
                string contorlname = lstAllColumns.SingleOrDefault(k => k.ColumnName == k1).ControlName;
                // if (contorlname.StartsWith("ddl"))  // for now removing the ddl from validation ... in furtue to be added 
                commaseperatedColumns += contorlname + ",";
            };
            commaseperatedColumns = commaseperatedColumns.Trim().TrimEnd(',').TrimStart(',');

            lstSettings.Where(k => k.IsMandatory).Select(k => k.ColumnName).ToList().ForEach(process);

            btnSave.Attributes.Add("onclick", " return ValidateMandatoryColumns('" + commaseperatedColumns + "', '" + commaSeperatedDateColumns + "');");




            var templateData = lstSettings;
            var usedTopColumns = templateData.Where(k => k.ExpTemplateId.ToLower().Trim() == currentTemplate.ToLower().Trim() && k.IsActive).Where(k => k.ColumnPosition != null);
            var usedBottomColumns = templateData.Where(k => k.ExpTemplateId.ToLower().Trim() == currentTemplate.ToLower().Trim() && k.IsActive).Where(k => k.ColumnPosition == null);
            List<ExpenseTemplateData> lstOrdering = usedTopColumns.OrderBy(k => k.ColumnPosition).ToList();
            lstOrdering.AddRange(usedBottomColumns.OrderBy(k => k.ColumnPosition).ToList());


            Table table = new Table() { BorderColor = System.Drawing.Color.FromName("#CCCCCC"), BackColor = System.Drawing.Color.FromName("#b4b4b4"), BorderWidth = 0, CellPadding = 2, CellSpacing = 1 };

            bool isodd = false;

            isodd = lstOrdering.Count % 2 != 0;
            TableRow row = new TableRow();
            int i = 0;
            foreach (var item in lstOrdering)
            {


                // content one 
                string displayTexxt = item.DisplayText;
                if (item.IsMandatory)
                {
                    displayTexxt = displayTexxt + " <font size='2' color='red'>*</font>";
                }
                TableCell cell0 = new TableCell() { CssClass = "FormLabel", Text = displayTexxt };  //  column name 

                string columnType = item.ColumnType;


                string enteronly = "";
                List<string> listValues = new List<string>();
                int width = 150;

                string value = "";


                ExpenseTemplateData settings = GetCurrentColumnSetting(item.ColumnName);
                if (settings == null)
                    return;
                bool isThisColumnEditable;
                if (isView)
                {
                    isThisColumnEditable = false;
                }
                else
                {
                    isThisColumnEditable = settings.IsEditable;
                }
                if (settings != null)
                {
                    columnType = settings.ColumnType;
                    enteronly = settings.ColumnType == "int" ? "number" : settings.ColumnType == "float" ? "decimal" : "text";
                    width = settings.Width;
                    if ((settings.ListValues + "").Trim().Length != 0)
                    {
                        if (settings.ListValues.Trim().ToLower() != "null")
                            listValues = settings.ListValues.Trim().Split(',').ToList();
                    }



                    // check for sp 
                    if ((settings.spName + "").Trim().Length != 0)
                    {
                        if ((settings.spName + "").Trim() == "Custom")
                            listValues = lstVisaTypes;
                        else if ((settings.spName + "").Trim() == "Client")
                        {
                            string cmd = "Exec [spBEExpClientCodePortfolioList] '" + userID + "'";
                            DataSet ds = service.GetDataSet(cmd);
                            List<string> lstempCollection = new List<string>();
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                {
                                    string du = ds.Tables[0].Rows[j][0].ToString(); // 0th column must be the output
                                    lstempCollection.Add(du);
                                }
                            }
                            listValues = lstempCollection;
                        }
                        else if (settings.spName.Trim().ToLower() != "null")
                            listValues = service.GetSpDDLItems(settings.spName.Trim(), userID);
                    }

                }





                TableCell cell1 = new TableCell() { CssClass = "FormControls" }; // control value 

                if (columnType == ColumnTypes.INT || columnType == ColumnTypes.FLOAT || columnType == ColumnTypes.TEXT)
                {
                    TextBox txtbox = new TextBox() { ID = lstAllColumns.Single(k => k.ColumnName == item.ColumnName).ControlName };

                    if (txtbox != null)
                    {


                        txtbox.Style.Add("text-align", settings.Allignment.ToLower());
                        txtbox.Font.Name = "Verdana";
                        txtbox.Font.Size = 8;
                        txtbox.Text = value + "";
                        txtbox.Width = width;

                        txtbox.CssClass = "TextBox";
                        int digits = 0;

                        if (enteronly == "decimal")
                        { txtbox.Attributes.Add("onKeydown", "return PressfloatOnly(event,this)"); digits = 15; }

                        if (enteronly == "number")
                        { txtbox.Attributes.Add("onKeydown", "return PressIntOnly(event,this)"); digits = 6; }


                        //  txtbox.Attributes.Add("onClick", "return PressIntOnly(event,this)");





                        if (isAddNew)
                        {
                            // add new 
                        }
                        else
                        {
                            // edit 
                            string txtID = txtbox.ID;
                            txtID = txtID.Substring(3);
                            txtbox.Text = typeof(MasterEntity).GetProperties().SingleOrDefault(k => txtID == (k.Name)).GetValue(dataEntity, null) + "";
                            txtbox.ToolTip = txtbox.Text;

                        }


                        if (!isThisColumnEditable)
                        {
                            txtbox.CssClass = "Label";
                            txtbox.ReadOnly = true;
                            //txtbox.Attributes.Add("onKeydown", "return PressReadOnly(event,this)");
                        }


                        // updation of the hiddenfield 
                        var hiddenfldName = "'hdnfld" + txtbox.ID + "'";


                        txtbox.Attributes.Add("onblur", "UpdateReleventHiddenField(this," + hiddenfldName + "); ValidateDigits(this," + digits + ");");






                    }
                    cell1.Controls.Add(txtbox);

                }


                if (columnType == ColumnTypes.DATE)
                {
                    //Image image = new Image() { ImageUrl = "~/Images/calendar.gif" };
                    TextBox txtbox = new TextBox() { ID = lstAllColumns.Single(k => k.ColumnName == item.ColumnName).ControlName };

                    if (txtbox != null)
                    {
                        txtbox.Style.Add("text-align", settings.Allignment.ToLower());
                        txtbox.Width = width;
                        // txtbox.CssClass = "DatepickerInput";
                        //txtbox.Attributes.Add("onclick", "fnShowCalendarFrmDate(this)");
                        txtbox.Font.Name = "Verdana";
                        txtbox.Font.Size = 8;



                        string temp = string.IsNullOrEmpty(value) ? null : Convert.ToDateTime(value).ToString(globalDateTimeFormat);
                        txtbox.Text = temp;


                        if (isAddNew)
                        {
                            // add new 
                        }
                        else
                        {
                            // edit 
                            txtbox.Text = typeof(MasterEntity).GetProperties().SingleOrDefault(k => txtbox.ID.Contains(k.Name)).GetValue(dataEntity, null) + "";
                            temp = string.IsNullOrEmpty(txtbox.Text) ? null : Convert.ToDateTime(txtbox.Text).ToString(globalDateTimeFormat);
                            txtbox.Text = temp;
                            txtbox.ToolTip = txtbox.Text;

                        }



                        // updation of the hiddenfield 
                        var hiddenfldName = "'hdnfld" + txtbox.ID + "'";

                        txtbox.Attributes.Add("onchange", "UpdateReleventHiddenField(this," + hiddenfldName + ")");



                        //image.Attributes.Add("onmouseover", "this.style.cursor='hand'");

                        //image.Attributes.Add("onclick", "fnShowCalendarFrmDateNew('" + txtbox.ClientID + "'," + hiddenfldName + ")");


                        txtbox.Attributes.Add("onKeydown", "return PressReadOnly(event,this)");
                        if (!isThisColumnEditable)
                        {
                            txtbox.CssClass = "Label";

                            //image.Attributes.Remove("onmouseover");
                            //image.Attributes.Remove("onKeydown");
                            //image.Attributes.Remove("onclick");

                        }


                    }

                    cell1.Controls.Add(txtbox);
                    Label lbl = new Label() { Text = " " };
                    cell1.Controls.Add(lbl);
                    // cell1.Controls.Add(image);
                }

                if (columnType == ColumnTypes.LIST)
                {
                    DropDownList ddl = new DropDownList() { ID = lstAllColumns.Single(k => k.ColumnName == item.ColumnName).ControlName };
                    if (ddl != null)
                    {
                        ddl.Style.Add("text-align", settings.Allignment.ToLower());
                        // value = typeof(MasterEntity).GetProperties().SingleOrDefault(k => ddl.ID.Contains(k.Name)).GetValue(dataEntity, null) + "";
                        value = typeof(MasterEntity).GetProperties().SingleOrDefault(k => ddl.ID == "ddl" + k.Name).GetValue(dataEntity, null) + "";  // jun 13  2013 karthik
                        ddl.Width = width;
                        ddl.Font.Name = "Verdana";
                        ddl.Font.Size = 8;
                        ddl.CssClass = "TextBox";
                        ddl.DataSource = listValues;
                        ddl.DataBind();
                        Action<ListItem> actionToAddToolTip = k => k.Attributes["title"] = k.Text;
                        ddl.Items.OfType<ListItem>().ToList().ForEach(actionToAddToolTip);



                        if (value == null)
                        {
                            if (isAddNew)
                            {

                                //"ddlFieldList3"
                                var hdnfld = divHiddenFieldControls.Controls.OfType<HiddenField>().SingleOrDefault(k => k.ID == "hdnfld" + ddl.ID);
                                if (hdnfld != null)
                                    hdnfld.Value = ddl.Items.Count > 0 ? ddl.Items[0].Text : null;

                                //hdnfldddlFieldList1.Value = ddl.Items[0].Text;

                            }
                            else
                            {
                                ddl.SelectedIndex = -1;
                                ddl.Items.Insert(0, "");
                            }
                        }
                        else
                        {
                            if ((value + "").ToLower() == "null")
                            {
                                if (isAddNew)
                                {
                                    //"ddlFieldList3"
                                    var hdnfld = divHiddenFieldControls.Controls.OfType<HiddenField>().SingleOrDefault(k => k.ID == "hdnfld" + ddl.ID);
                                    if (hdnfld != null)
                                        hdnfld.Value = ddl.Items.Count > 0 ? ddl.Items[0].Text : null;

                                    //hdnfldddlFieldList1.Value = ddl.Items[0].Text;

                                }
                                else
                                {
                                    ddl.SelectedIndex = -1;
                                    ddl.Items.Insert(0, "");
                                }
                            }
                            else if ((value + "").Length == 0)
                            {

                                if (isAddNew)
                                {
                                    //"ddlFieldList3"
                                    var hdnfld = divHiddenFieldControls.Controls.OfType<HiddenField>().SingleOrDefault(k => k.ID == "hdnfld" + ddl.ID);
                                    if (hdnfld != null)
                                        hdnfld.Value = ddl.Items.Count > 0 ? ddl.Items[0].Text : null;

                                    //hdnfldddlFieldList1.Value = ddl.Items[0].Text;
                                }
                                else
                                {
                                    ddl.SelectedIndex = -1;
                                    ddl.Items.Insert(0, "");
                                }
                            }
                            else
                            {
                                if (listValues.Count == 0)
                                    ddl.Items.Add((value + "").Trim());
                                else
                                {
                                    var temp = ddl.Items.Cast<ListItem>().FirstOrDefault(k => (k.Text + "").ToLower() == (value + "").Trim().ToLower());
                                    if (temp != null)
                                    {

                                        ddl.Text = (temp.Text + "").Trim();
                                        ddl.ToolTip = ddl.Text;
                                    }
                                    else
                                    {
                                        //Priority
                                        //if (item.ColumnName.ToLowerTrim().Contains("priority"))
                                        //{
                                        //    ddl.Items.Insert(0, value + "");
                                        //    ddl.ToolTip = value + "";
                                        //}

                                    }
                                }
                            }
                        }

                        if (isAddNew)
                        {
                            // add new 

                            ddl.ToolTip = ddl.Text;


                        }
                        else
                        {
                            // edit 
                            // ddl.Text = typeof(MasterEntity).GetProperties().SingleOrDefault(k => ddl.ID.Contains(k.Name)).GetValue(dataEntity, null) + "";
                            ddl.Text = typeof(MasterEntity).GetProperties().SingleOrDefault(k => ddl.ID == "ddl" + k.Name).GetValue(dataEntity, null) + "";  // jun 13  2013 karthik
                            ddl.ToolTip = ddl.Text;


                        }



                        ddl.Enabled = isThisColumnEditable;

                        // updation of the hiddenfield 
                        var hiddenfldName = "'hdnfld" + ddl.ID + "'";
                        ddl.Attributes.Add("onchange", "UpdateReleventHiddenField(this," + hiddenfldName + ")");


                        // code for visa - country -  visatype mapper, cascade  purpose
                        string __ct = "";
                        if (ddlExpenseType.Text.ToLower().StartsWith("visa") || ddlExpenseType.Text.ToLower().StartsWith("wp"))
                            __ct = "visa";
                        if (__ct == "visa")
                        {
                            __ct = ddlExpenseType.Text.ToLower().StartsWith("visa") ? "visa" : "wp";
                            //if (__ct == "visa")
                            //{
                            if (ddl.ID == "ddlFieldList1")
                            {
                                ddl.Attributes.Add("onchange", "UpdateReleventHiddenFieldForVisa(this," + hiddenfldName + ")");
                                string _country = ddl.Items.Count > 0 ? ddl.Text : "";



                                List<VisaCountry> lstVisa = new List<VisaCountry>();
                                lstVisa = service.GetVisaCountryMapping();
                                _country = _country.ToLower();
                                var result = lstVisa.Where(k => k.Country.ToLower() == _country).Select(k => k.VisaType).ToList();
                                // var result = lstVisa.Where(k => k.VisaWP.ToLower() == __ct).Where(k => k.Country.ToLower() == _country).Select(k => k.VisaType).ToList();
                                lstVisaTypes = result;

                                //}
                            }
                        }



                    }
                    cell1.Controls.Add(ddl);
                }



                row.Cells.Add(cell0);
                row.Cells.Add(cell1);

                if (row.Cells.Count == 4)
                {
                    table.Rows.Add(row);
                    row = new TableRow();
                }

                // adding last value 
                i++;
                if (isodd)
                {

                    if (lstOrdering.Count == i)
                    {

                        TableCell cellempty0 = new TableCell() { CssClass = "FormControls" }; // control value   // content one 
                        TableCell cellempty1 = new TableCell() { CssClass = "FormControls" };  //  column name  FormLabel
                        row.Cells.Add(cellempty0);
                        row.Cells.Add(cellempty1);
                        table.Rows.Add(row);

                        row = new TableRow();
                    }
                }



            }

            DynamicControlsHolder.Controls.Add(table);



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

    public ExpenseTemplateData GetCurrentColumnSetting(string columnName)
    {
        ExpenseTemplateData temp = new ExpenseTemplateData();
        try
        {
            temp = lstSettings.SingleOrDefault(k => k.ColumnName == columnName);

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
        return temp;
    }

    public string GetHiddenStringValue(string value)
    {

        string returnValue = null;
        returnValue = (value + "").Length == 0 ? null : value.Trim();
        return returnValue;

    }

    public Nullable<DateTime> GetHiddenDateValue(string value)
    {

        Nullable<DateTime> returnValue = null;
        returnValue = (value + "").Length == 0 ? returnValue : Convert.ToDateTime(value.Trim());
        return returnValue;



    }


    //public string GetCurrentQuarter()
    //{
    //    
    //    string strquarter = quareter;
    //    return DateUtility.GetQuarter(strquarter.ToLower());
    //}



    protected void btnSave_Click(object sender, EventArgs e)
    {


        #region Initialisation


        int _intExpId = hdnfldKey.Value.Length == 0 ? 0 : Convert.ToInt32(hdnfldKey.Value);
        string ClientCode = null;
        string ItemName = null;
        Double? NumberofItems = default(double);
        double UnitCost = default(double);
        string JustificationRemarks = null;
        string ProjOppCode = null;
        double BEUpside = default(double);
        double BEDownside = default(double);
        string CurrQtr = null;
        double FutQtrBE = default(double);
        string Fieldtxt1 = null;
        string Fieldtxt2 = null;
        string Fieldtxt3 = null;
        string Fieldtxt4 = null;
        string Fieldtxt5 = null;
        string Fieldtxt6 = null;
        string Fieldtxt7 = null;
        string Fieldtxt8 = null;
        string Fieldtxt9 = null;
        string Fieldtxt10 = null;
        string Fieldtxt11 = null;
        string Fieldtxt12 = null;
        string Fieldtxt13 = null;
        string Fieldtxt14 = null;
        string Fieldtxt15 = null;
        string Fieldtxt16 = null;
        string Fieldtxt17 = null;
        string Fieldtxt18 = null;
        string PUCode = null;
        string BUCode = null;
        string DUCode = null;
        string ExpType = null;
        string ExpCategory = null;
        string Priority = null;
        string IsCustomerRecoverable = null;
        string IsBudgetedinPBS = null;
        string Status = null;
        string FieldList1 = null;
        string FieldList2 = null;
        string FieldList3 = null;
        string FieldList4 = null;
        string FieldList5 = null;
        string FieldList6 = null;
        string FieldList7 = null;
        string FieldList8 = null;
        string FieldList9 = null;
        string FieldList10 = null;
        DateTime? ExpenseDate = null;
        DateTime? FieldDate1 = null;
        DateTime? FieldDate2 = null;
        DateTime? FieldDate3 = null;
        DateTime? FieldDate4 = null;
        DateTime? FieldDate5 = null;
        DateTime? FieldDate6 = null;
        DateTime? FieldDate7 = null;
        DateTime? FieldDate8 = null;
        string CreatedBy = null;
        DateTime? CreatedOn = null;
        string ModifiedBy = null;
        DateTime? ModifiedOn = null;
        string DMMailID = null;

        #endregion

        Nullable<double> nullabledouble = null;


        try
        {
            ClientCode = hdnfldddlClientCode.Value;
            ItemName = hdnfldtxtItemName.Value;
            NumberofItems = hdnfldtxtNumberofItems.Value.Length == 0 ? 0 : Convert.ToDouble(hdnfldtxtNumberofItems.Value);
            UnitCost = hdnfldtxtUnitCost.Value.Length == 0 ? 0 : Convert.ToDouble(hdnfldtxtUnitCost.Value);
            JustificationRemarks = GetHiddenStringValue(hdnfldtxtJustificationRemarks.Value); // (string)GetRowValue(row, "txtJustificationRemarks", ColumnTypes.TEXT);
            ProjOppCode = GetHiddenStringValue(hdnfldtxtProjOppCode.Value); //(string)GetRowValue(row, "txtProjOppCode", ColumnTypes.TEXT);
            BEUpside = hdnfldtxtBEUpside.Value.Length == 0 ? 0 : Convert.ToDouble(hdnfldtxtBEUpside.Value);  //(double)GetRowValue(row, "txtBEUpside", ColumnTypes.FLOAT);
            BEDownside = hdnfldtxtBEDownside.Value.Length == 0 ? 0 : Convert.ToDouble(hdnfldtxtBEDownside.Value);// (double)GetRowValue(row, "txtBEDownside", ColumnTypes.FLOAT);


            // CurrQtr = GetCurrentQuarter();
            CurrQtr = ddlQuarter.Text;

            FutQtrBE = hdnfldtxtFutQtrBE.Value.Length == 0 ? 0 : Convert.ToDouble(hdnfldtxtFutQtrBE.Value);// (double)GetRowValue(row, "txtFutQtrBE", ColumnTypes.FLOAT);
            //FutQtrBE = GetHiddenStringValue(hdnfldtxtFutQtrBE.Value);
            Fieldtxt1 = GetHiddenStringValue(hdnfldtxtFieldtxt1.Value);
            Fieldtxt2 = GetHiddenStringValue(hdnfldtxtFieldtxt2.Value);
            Fieldtxt3 = GetHiddenStringValue(hdnfldtxtFieldtxt3.Value);
            Fieldtxt4 = GetHiddenStringValue(hdnfldtxtFieldtxt4.Value);
            Fieldtxt5 = GetHiddenStringValue(hdnfldtxtFieldtxt5.Value);
            Fieldtxt6 = GetHiddenStringValue(hdnfldtxtFieldtxt6.Value);
            Fieldtxt7 = GetHiddenStringValue(hdnfldtxtFieldtxt7.Value);
            Fieldtxt8 = GetHiddenStringValue(hdnfldtxtFieldtxt8.Value);
            Fieldtxt9 = GetHiddenStringValue(hdnfldtxtFieldtxt9.Value);
            Fieldtxt10 = GetHiddenStringValue(hdnfldtxtFieldtxt10.Value);
            Fieldtxt11 = GetHiddenStringValue(hdnfldtxtFieldtxt11.Value);
            Fieldtxt12 = GetHiddenStringValue(hdnfldtxtFieldtxt12.Value);

            Fieldtxt12 = GetHiddenStringValue(hdnfldtxtFieldtxt12.Value);
            Fieldtxt13 = GetHiddenStringValue(hdnfldtxtFieldtxt13.Value);
            Fieldtxt14 = GetHiddenStringValue(hdnfldtxtFieldtxt14.Value);
            Fieldtxt15 = GetHiddenStringValue(hdnfldtxtFieldtxt15.Value);
            Fieldtxt16 = GetHiddenStringValue(hdnfldtxtFieldtxt16.Value);
            Fieldtxt17 = GetHiddenStringValue(hdnfldtxtFieldtxt17.Value);
            Fieldtxt18 = GetHiddenStringValue(hdnfldtxtFieldtxt18.Value);

            //PUCode = (string)GetRowValue(row, "ddlPUCode", ColumnTypes.LIST);
            // BUCode = (string)GetRowValue(row, "ddlBUCode", ColumnTypes.LIST);
            //DUCode = (string)GetRowValue(row, "ddlDUCode", ColumnTypes.LIST);
            //ExpType = (string)GetRowValue(row, "ddlExpType", ColumnTypes.LIST);
            ExpCategory = GetHiddenStringValue(hdnfldddlExpCategory.Value);// (string)GetRowValue(row, "ddlExpCategory", ColumnTypes.LIST);

            //PUCode = pucode;

            //DUCode = dMcode;
            //ExpType = ExpenseType;

            PUCode = ddlPU.Text;
            ExpType = ddlExpenseType.Text;
            DUCode = hdnfldddlDM.Value;





            Priority = GetHiddenStringValue(hdnfldddlPriority.Value);// (string)GetRowValue(row, "ddlPriority", ColumnTypes.LIST);
            IsCustomerRecoverable = GetHiddenStringValue(hdnfldddlIsCustomerRecoverable.Value);// (string)GetRowValue(row, "ddlIsCustomerRecoverable", ColumnTypes.LIST);
            IsBudgetedinPBS = GetHiddenStringValue(hdnfldddlIsBudgetedinPBS.Value);// (string)GetRowValue(row, "ddlIsBudgetedinPBS", ColumnTypes.LIST);
            Status = GetHiddenStringValue(hdnfldddlStatus.Value); // (string)GetRowValue(row, "ddlStatus", ColumnTypes.LIST);
            FieldList1 = GetHiddenStringValue(hdnfldddlFieldList1.Value);// (string)GetRowValue(row, "ddlFieldList1", ColumnTypes.LIST);
            FieldList2 = GetHiddenStringValue(hdnfldddlFieldList2.Value);//(string)GetRowValue(row, "ddlFieldList2", ColumnTypes.LIST);
            FieldList3 = GetHiddenStringValue(hdnfldddlFieldList3.Value);//(string)GetRowValue(row, "ddlFieldList3", ColumnTypes.LIST);
            FieldList4 = GetHiddenStringValue(hdnfldddlFieldList4.Value);//(string)GetRowValue(row, "ddlFieldList4", ColumnTypes.LIST);
            FieldList5 = GetHiddenStringValue(hdnfldddlFieldList5.Value);//(string)GetRowValue(row, "ddlFieldList5", ColumnTypes.LIST);
            FieldList6 = GetHiddenStringValue(hdnfldddlFieldList6.Value);//(string)GetRowValue(row, "ddlFieldList6", ColumnTypes.LIST);
            FieldList7 = GetHiddenStringValue(hdnfldddlFieldList7.Value);//(string)GetRowValue(row, "ddlFieldList7", ColumnTypes.LIST);
            FieldList8 = GetHiddenStringValue(hdnfldddlFieldList8.Value);//(string)GetRowValue(row, "ddlFieldList8", ColumnTypes.LIST);
            FieldList9 = GetHiddenStringValue(hdnfldddlFieldList9.Value);//(string)GetRowValue(row, "ddlFieldList8", ColumnTypes.LIST);
            FieldList10 = GetHiddenStringValue(hdnfldddlFieldList10.Value);//(string)GetRowValue(row, "ddlFieldList8", ColumnTypes.LIST);
            ExpenseDate = GetHiddenDateValue(hdnflddtpExpenseDate.Value);// (DateTime?)GetRowValue(row, "dtpExpenseDates", ColumnTypes.DATE);
            FieldDate1 = GetHiddenDateValue(hdnflddtpFieldDate1.Value);// (DateTime?)GetRowValue(row, "dtpFieldDate1", ColumnTypes.DATE);
            FieldDate2 = GetHiddenDateValue(hdnflddtpFieldDate2.Value);// (DateTime?)GetRowValue(row, "dtpFieldDate2", ColumnTypes.DATE);
            FieldDate3 = GetHiddenDateValue(hdnflddtpFieldDate3.Value);// (DateTime?)GetRowValue(row, "dtpFieldDate3", ColumnTypes.DATE);
            FieldDate4 = GetHiddenDateValue(hdnflddtpFieldDate4.Value);// (DateTime?)GetRowValue(row, "dtpFieldDate4", ColumnTypes.DATE);
            FieldDate5 = GetHiddenDateValue(hdnflddtpFieldDate5.Value);// (DateTime?)GetRowValue(row, "dtpFieldDate5", ColumnTypes.DATE);
            FieldDate6 = GetHiddenDateValue(hdnflddtpFieldDate6.Value);// (DateTime?)GetRowValue(row, "dtpFieldDate6", ColumnTypes.DATE);
            FieldDate7 = GetHiddenDateValue(hdnflddtpFieldDate7.Value);// (DateTime?)GetRowValue(row, "dtpFieldDate7", ColumnTypes.DATE);
            FieldDate8 = GetHiddenDateValue(hdnflddtpFieldDate8.Value);// (DateTime?)GetRowValue(row, "dtpFieldDate8", ColumnTypes.DATE);
            CreatedBy = Session["UserID"] + "";
            CreatedOn = DateTime.Now;
            ModifiedBy = Session["UserID"] + "";
            // DMMailID = Session[Constants.DMMAILID] + "";
            DMMailID = hdnfldddlDM.Value;

            ModifiedOn = DateTime.Now;
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



        MasterEntity masterData = new MasterEntity()
        {

            BEDownside = BEDownside,
            BEUpside = BEUpside,
            BUCode = BUCode,
            PUCode = PUCode,
            ClientCode = ClientCode,
            CreatedBy = CreatedBy,
            CreatedOn = CreatedOn,
            CurrQtr = CurrQtr,
            DUCode = DUCode,
            ExpCategory = ExpCategory,
            ExpenseDate = ExpenseDate,
            ExpType = ExpType,
            FieldDate1 = FieldDate1,
            FieldDate2 = FieldDate2,
            FieldDate3 = FieldDate3,
            FieldDate4 = FieldDate4,
            FieldDate5 = FieldDate5,
            FieldDate6 = FieldDate6,
            FieldDate7 = FieldDate7,
            FieldDate8 = FieldDate8,
            FieldList1 = FieldList1,
            FieldList2 = FieldList2,
            FieldList3 = FieldList3,
            FieldList4 = FieldList4,
            FieldList5 = FieldList5,
            FieldList6 = FieldList6,
            FieldList7 = FieldList7,
            FieldList8 = FieldList8,
            FieldList9 = FieldList9,
            FieldList10 = FieldList10,
            Fieldtxt1 = Fieldtxt1,
            Fieldtxt2 = Fieldtxt2,
            Fieldtxt3 = Fieldtxt3,
            Fieldtxt4 = Fieldtxt4,
            Fieldtxt5 = Fieldtxt5,
            Fieldtxt6 = Fieldtxt6,
            Fieldtxt7 = Fieldtxt7,
            Fieldtxt8 = Fieldtxt8,
            Fieldtxt9 = Fieldtxt9,
            Fieldtxt10 = Fieldtxt10,
            Fieldtxt11 = Fieldtxt11,
            Fieldtxt12 = Fieldtxt12,
            Fieldtxt13 = Fieldtxt13,
            Fieldtxt14 = Fieldtxt14,
            Fieldtxt15 = Fieldtxt15,
            Fieldtxt16 = Fieldtxt16,
            Fieldtxt17 = Fieldtxt17,
            Fieldtxt18 = Fieldtxt18,
            FutQtrBE = FutQtrBE,
            intExpId = _intExpId,
            IsBudgetedinPBS = IsBudgetedinPBS,
            IsCustomerRecoverable = IsCustomerRecoverable,
            ItemName = ItemName,
            JustificationRemarks = JustificationRemarks,
            ModifiedBy = ModifiedBy,
            ModifiedOn = ModifiedOn,
            NumberofItems = NumberofItems,
            Priority = Priority,
            ProjOppCode = ProjOppCode,
            Status = Status,
            UnitCost = UnitCost,
            DMMailId = hdnfldddlDM.Value



        };

        masterData.PUCode = ddlPU.Text;
        masterData.ExpType = ddlExpenseType.Text;
        masterData.DMMailId = hdnfldddlDM.Value;

        string machineUser = string.Empty;
        string[] machineUsers = HttpContext.Current.User.Identity.Name.Split('\\');
        if (machineUsers.Length == 2)
            machineUser = machineUsers[1];

        try
        {
            // if (IsBudgetedinPBS=="--Select--" || Status=="--Select--" || IsCustomerRecoverable=="--Select--" || Priority=="--Select--" || FieldList1 == "--Select--" || FieldList2 == "--Select--" || FieldList3 == "--Select--" || FieldList4 == "--Select--" || FieldList5 == "--Select--" || FieldList6 == "--Select--" || FieldList7 == "--Select--" || FieldList8 == "--Select--" || FieldList9 == "--Select--" || FieldList10 == "--Select--" )
            // {
            //     Response.Write(@" <script type=""text/javascript""> alert('Please select appropriate values in the drop down(s)');FieldList3.focus(); return false;    </script>");
            ////   Response.Write(@" <script type=""text/javascript"">   </script>");
            // }
            // else

            // {
            if (ClientCode == "COE")
            {
                string CMDTEXT = "SELECT distinct [txtUserid] from [CoEOwners] where [txtUserid]='" + Session["UserID"] + "'";
                DataSet DS = service.GetDataSet(CMDTEXT);
                DataTable DT = DS.Tables[0];
                if (DT.Rows.Count == 0)
                {
                    Response.Write(@" <script type=""text/javascript""> alert('You donot have permission to add budget under COE!'); window.opener.document.getElementById('MainContent_Button1').click(); window.close();   </script>");
                }
                else
                {

                    if (_intExpId > 0)
                    {
                        // update operation
                        masterData.ModifiedBy = machineUser;
                        masterData.ModifiedOn = DateTime.Now;
                        masterData.intExpId = _intExpId;
                        service.UpdateMasterData(masterData);

                    }
                    else
                    {
                        // insert operation
                        masterData.CreatedBy = machineUser;
                        masterData.CreatedOn = DateTime.Now;
                        masterData.ModifiedBy = null;
                        masterData.ModifiedOn = null;
                        masterData.intExpId = 0;


                        service.InsertMasterData(masterData);

                    }
                }
            }
            else
            {

                if (_intExpId > 0)
                {
                    // update operation
                    masterData.ModifiedBy = machineUser;
                    masterData.ModifiedOn = DateTime.Now;
                    masterData.intExpId = _intExpId;
                    service.UpdateMasterData(masterData);

                }
                else
                {
                    // insert operation
                    masterData.CreatedBy = machineUser;
                    masterData.CreatedOn = DateTime.Now;
                    masterData.ModifiedBy = null;
                    masterData.ModifiedOn = null;
                    masterData.intExpId = 0;


                    service.InsertMasterData(masterData);

                }
            }
            Response.Write(@" <script type=""text/javascript""> alert('saved successfully !');  window.close();   </script>");

        }

        //}
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

    private void LoadCombobox(string pucode, string ducode, string ExpenseType, string quareter)
    {
        bool isAddother = false;
        isAddother = (/*pucode + ducode +*/ ExpenseType + quareter).Length == 0;

        try
        {

            string userID = Session["UserID"] + "";

            List<PUDM> lstPUDM = new List<PUDM>();
            lstPUDM = service.GetPUDMMapping(userID);


            string csvpudm = string.Empty;
            foreach (var item in lstPUDM)
                csvpudm += item.PU + "," + item.DM + "|";



            csvpudm = csvpudm.Trim().TrimEnd('|').TrimStart('|');
            hdnfldDMCSV.Value = csvpudm;



            ddlPU.Items.Clear();
            var pus = lstPUDM.Select(k => k.PU.Trim()).Distinct().ToList();
            foreach (string rh in pus)
                ddlPU.Items.Add(rh);

            if (!isAddother)
                ddlPU.Text = pucode;



            ddlDM.Items.Clear();
            var DMs = lstPUDM.Where(k => k.PU == ddlPU.Text).Select(k => k.DM).Distinct().OrderBy(k => k).ToList();
            foreach (string rh in DMs)
                ddlDM.Items.Add(rh);

            if (isAddother)
            {
                ddlDM.SelectedIndex = 0;
            }
            else
            {
                ddlDM.Text = "";
                ddlDM.Text = ducode;


            }

            hdnfldddlDM.Value = ddlDM.Text;
            DateTime todaydate = DateTime.Now;
            string lastqtr2 = "";
            string currqtr2 = "";
            int year = todaydate.Year - 2000;
            int nextyear = year + 1;
            string cmdstrquarter = "SELECT [strquarter]   FROM [ExpConfig]";
            DataSet ds = service.GetDataSet(cmdstrquarter);
            DataTable dt = ds.Tables[0];
            string strquarter = dt.Rows[0][0].ToString();
            if (strquarter == "Q1")
            {
                lastqtr2 = "Q4'" + year;
                currqtr2 = "Q1'" + nextyear;
            }

            if (strquarter == "Q2")
            {
                lastqtr2 = "Q1'" + nextyear;
                currqtr2 = "Q2'" + nextyear;
            }
            if (strquarter == "Q3")
            {
                lastqtr2 = "Q2'" + nextyear;
                currqtr2 = "Q3'" + nextyear;
            }
            if (strquarter == "Q4")
            {
                lastqtr2 = "Q3'" + nextyear;
                currqtr2 = "Q4'" + nextyear;
            }
            //string currentQtr = DateUtility.GetQuarter("current");
            //string nextQtr = DateUtility.GetQuarter("next");
            //string nextQtrPlus1 = DateUtility.GetQuarter("next1");
            //string nextQtrPlus2 = DateUtility.GetQuarter("next2");
            ddlQuarter.Items.Clear();
            //int currentWorkFlowStage = Convert.ToInt32(File.ReadAllText(Server.MapPath("/Storage/") + "WorkflowStage.txt").Trim());
            //if (currentWorkFlowStage != 1)
            string currentQtr = currqtr2;
            //ddlQuarter.Items.Add(currentQtr);
           
            currentQtr = DateUtility.GetQuarter("current");
            string nextQtr = DateUtility.GetQuarter("next");
            string nextQtrPlus1 = DateUtility.GetQuarter("next1");

           
            ddlQuarter.Items.Insert(0, currentQtr);
            ddlQuarter.Items.Insert(1, nextQtr);
            ddlQuarter.Items.Insert(2, nextQtrPlus1);
            ddlQuarter.Text = currentQtr;

            //ddlQuarter.Items.Add(nextQtrPlus1);
            //ddlQuarter.Items.Add(nextQtrPlus2);


            if (isAddother)
            {
                ddlQuarter.Items.Clear();
                //Uncomment it once Q3'14 upload is done...
                ddlQuarter.Items.Add(currqtr2);
                //ddlQuarter.Items.Add("Q4'14");
                //ddlQuarter.Text = DateUtility.GetQuarter(quareter);
                //ddlQuarter.Text = currqtr2;
            }

            List<string> lstItems = service.GetSpDDLItems("spBEExpGetExpenseType");
            if (lstItems != null && lstItems.Count > 1)
                lstItems.Remove("Expenses - Existing");
            ddlExpenseType.DataSource = lstItems;
            ddlExpenseType.DataBind();

            if (!isAddother)
                ddlExpenseType.Text = ExpenseType;

            ddlQuarter.Enabled = true;
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

    protected void hypadd_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "dpExcept", "dpExcept();", true);
        int size = 0;
        try
        {
            ddlDM.Enabled = false;
            ddlPU.Enabled = false;
            ddlExpenseType.Enabled = false;
            ddlQuarter.Enabled = false;

            string dMcode = ddlDM.Text;
            string pucode = ddlPU.Text;
            dMcode = hdnfldddlDM.Value;



            string pu = ddlPU.Text;
            ddlDM.Items.Clear();
            string userID = Session["UserID"] + "";

            List<PUDM> lstPUDM = new List<PUDM>();
            lstPUDM = service.GetPUDMMapping(userID);

            var DMs = lstPUDM.Where(k => k.PU == pu).Select(k => k.DM).Distinct().OrderBy(k => k).ToList();
            foreach (string rh in DMs)
                ddlDM.Items.Add(rh);

            ddlDM.Text = dMcode;


            string ExpenseType = ddlExpenseType.Text;
            string qurt = ddlQuarter.Text;
            string currentTemplate = service.GetTemplateID(ExpenseType);

            //TODO: quarter related task
            // quareter = currentNextQtr.ToLower() == DateUtility.GetQuarter("current").ToLower() ? "current" : "next";
            string currentNextAllQtr = DateUtility.GetQuarter("current") == qurt ? "current" : DateUtility.GetQuarter("next") == qurt ? "next" :
            DateUtility.GetQuarter("next1") == qurt ? "next1" : "next2";



            Process( ExpenseType, currentNextAllQtr, 0, true, currentTemplate);
            //Process(pucode, dMcode, ExpenseType, currentNextQtr, 0, true, currentTemplate);


            //string userSU = Session["UserSU"] + "";
            var count = service.GetExpenseTemplateData().Where(k => k.ExpTemplateId.ToLower().Trim() == currentTemplate.ToLower().Trim() && k.IsActive).ToList().Count;

            int staticheight = 220;
            int variableheight = count / 2;
            size = (staticheight + (variableheight * 26));
            // size = count * 32;

            btnSave.Visible = true;
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
        Response.Write(@" <script type=""text/javascript""> window.resizeTo(700," + size + ") </script>");
        Response.Write(@" <script type=""text/javascript""> window.focus() </script>");





    }

    protected void hypRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            ddlDM.Enabled = true;
            ddlPU.Enabled = true;
            ddlExpenseType.Enabled = true;
            //  ddlQuarter.Enabled = true;
            ddlQuarter.Enabled = false;
            DynamicControlsHolder.Controls.Clear();
            btnSave.Visible = false;


            string pu = ddlPU.Text;
            ddlDM.Items.Clear();
            string userID = Session["UserID"] + "";

            List<PUDM> lstPUDM = new List<PUDM>();
            lstPUDM = service.GetPUDMMapping(userID);
            var DMs = lstPUDM.Where(k => k.PU == pu).Select(k => k.DM).Distinct().OrderBy(k => k).ToList();
            foreach (string rh in DMs)
                ddlDM.Items.Add(rh);

            hdnfldddlDM.Value = ddlDM.Text;


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
        Response.Write(@" <script type=""text/javascript""> window.resizeTo(700,175) </script>");
    }




}