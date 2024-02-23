using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BEData;
using System.IO;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;
using System.Web.Services;
using System.Collections;
using System.Reflection;
namespace BECodeProd
{
    public class ColumnTypes
    {
        public static string DATE = "date";
        public static string FLOAT = "float";
        public static string INT = "int";
        public static string TEXT = "text";
        public static string LIST = "list";


    }
    public enum EnterOnly { Number, decimalpoint, Text }

    public partial class SubConHome : System.Web.UI.Page
    {
        List<MasterEntity> lstPagerDataSource = new List<MasterEntity>();


        Logger logger = new Logger();
        private string fileName = "BEData.SubConHome.cs";

        static BEDL service = new BEDL();

        List<ExpenseTemplateData> lstSettings = new List<ExpenseTemplateData>();
        List<ExpenseTemplateData> lstMiscSettings = new List<ExpenseTemplateData>();

        string commaseperatedColumns = "";
        string currentTemplate = "";
        string userID = "";
        List<PUDM> lstPUDM = new List<PUDM>();

        int currentWorkFlowStage = 0;
        string role = "";
        static string expenseType = "";
        static string quareter = "";
        string from = "";
        static string mcc = "";
        string _tempexptype = "";
        [System.Web.Services.WebMethod]
        public static string GridData(MasterEntity Enity)
        {

            service.UpdateMasterData(Enity);
            return "success";
        }


        [System.Web.Services.WebMethod]
        public static void Data(List<object> enitys)
        {

            var firstElement = enitys.First();

            var result = (from l in enitys
                          select l).FirstOrDefault();

            //List<object> entries = new List<object>();
            //entries.Add("zero");
            //entries.Add("one");
            //entries.Add("two");

            //Dictionary<object, object> numberedEntries = new Dictionary<object, object>();
            //int i = 0;
            //enitys.ForEach(x => numberedEntries.Add(i++, x));
            //foreach (KeyValuePair<object, object> pair in numberedEntries)
            //{
            //    Console.WriteLine(pair.Key + ": " + pair.Value);

            //    object[] ar = pair.Value;
            //}


            var t = ((IEnumerable)firstElement).Cast<dynamic>().ToList();
            MasterEntity xyz = new MasterEntity();
            foreach (object r in t)
            {
                string i = r.ToString();

                i = i.Replace("[", "").Replace("]", "");

                string[] data = i.Split(',');

                string Key = data[0].ToString().Trim();
                string Value = data[1].ToString().Trim();

                PropertyInfo prop = xyz.GetType().GetProperty(Key, BindingFlags.Public | BindingFlags.Instance);
                if (null != prop && prop.CanWrite)
                {
                    string S = prop.PropertyType.ToString();
                    if (prop.PropertyType.ToString().Contains("Double"))
                    {
                        prop.SetValue(xyz, Convert.ToDouble(Value), null);
                    }
                    else if (prop.PropertyType.ToString().Contains("DateTime"))
                    {
                        prop.SetValue(xyz, Convert.ToDateTime(Value), null);
                    }
                    else if (prop.PropertyType.ToString().Contains("Int"))
                    {
                        prop.SetValue(xyz, Convert.ToInt32(Value), null);
                    }
                    else
                    {
                        prop.SetValue(xyz, Value, null);
                    }
                }
            }
            GridData(xyz);

        }

        //[WebMethod]
        //public static void MyMethod(double BEDownside, double BEUpside, string BUCode,string PUCode,string ClientCode, string CreatedBy, string )
        //{
        //    try
        //    {

        //         MasterEntity masterData = new MasterEntity()
        //    {

        //        BEDownside = BEDownside,
        //        BEUpside = BEUpside,
        //        BUCode = BUCode,
        //        PUCode = PUCode,
        //        ClientCode = ClientCode,
        //        CreatedBy = CreatedBy,
        //        CreatedOn = CreatedOn,
        //        CurrQtr = CurrQtr,
        //        DUCode = DUCode,
        //        ExpCategory = ExpCategory,
        //        ExpenseDate = ExpenseDate,
        //        ExpType = ExpType,
        //        FieldDate1 = FieldDate1,
        //        FieldDate2 = FieldDate2,
        //        FieldDate3 = FieldDate3,
        //        FieldDate4 = FieldDate4,
        //        FieldDate5 = FieldDate5,
        //        FieldDate6 = FieldDate6,
        //        FieldDate7 = FieldDate7,
        //        FieldDate8 = FieldDate8,
        //        FieldList1 = FieldList1,
        //        FieldList2 = FieldList2,
        //        FieldList3 = FieldList3,
        //        FieldList4 = FieldList4,
        //        FieldList5 = FieldList5,
        //        FieldList6 = FieldList6,
        //        FieldList7 = FieldList7,
        //        FieldList8 = FieldList8,
        //        FieldList9 = FieldList9,
        //        FieldList10 = FieldList10,
        //        Fieldtxt1 = Fieldtxt1,
        //        Fieldtxt2 = Fieldtxt2,
        //        Fieldtxt3 = Fieldtxt3,
        //        Fieldtxt4 = Fieldtxt4,
        //        Fieldtxt5 = Fieldtxt5,
        //        Fieldtxt6 = Fieldtxt6,
        //        Fieldtxt7 = Fieldtxt7,
        //        Fieldtxt8 = Fieldtxt8,
        //        Fieldtxt9 = Fieldtxt9,
        //        Fieldtxt10 = Fieldtxt10,
        //        Fieldtxt11 = Fieldtxt11,
        //        Fieldtxt12 = Fieldtxt12,
        //        Fieldtxt13 = Fieldtxt13,
        //        Fieldtxt14 = Fieldtxt14,
        //        Fieldtxt15 = Fieldtxt15,
        //        Fieldtxt16 = Fieldtxt16,
        //        Fieldtxt17 = Fieldtxt17,
        //        Fieldtxt18 = Fieldtxt18,
        //        FutQtrBE = FutQtrBE,
        //        intExpId = _intExpId,
        //        IsBudgetedinPBS = IsBudgetedinPBS,
        //        IsCustomerRecoverable = IsCustomerRecoverable,
        //        ItemName = ItemName,
        //        JustificationRemarks = JustificationRemarks,
        //        ModifiedBy = ModifiedBy,
        //        ModifiedOn = ModifiedOn,
        //        NumberofItems = NumberofItems,
        //        Priority = Priority,
        //        ProjOppCode = ProjOppCode,
        //        Status = Status,
        //        UnitCost = UnitCost,
        //        DMMailId = hdnfldddlDM.Value



        //    };



        //        service.UpdateMasterData(
        //        //Do here server event  
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //} 




        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Page.IsPostBack)
                {
                    //expenseType = ddlExpenseType.Text;
                    //quareter = ddlQuarter.Text;

                    //_tempexptype = expenseType.Split(',').FirstOrDefault() + "";
                    //from = "Requirement";
                    //mcc = ddlMCC.Text;
                    //currentTemplate = (service.GetTemplateID(_tempexptype) + "").Trim();
                    LoadData();


                }
                else
                {
                    if (Request.QueryString.Count == 0)
                    {
                        LoadData();
                    }
                    else
                    {
                        SearchData();
                    }
                    BindGrid();


                    ExpenseType();
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

        private void ExpenseType()
        {
            //if (ddlExpenseType.Text == "Subcons - Existing")
            //{
            //    btnCopyRow.Visible = false;
            //    btnDelete.Visible = false;
            //}
            //else
            //{
            //    btnCopyRow.Visible = true;
            //    btnDelete.Visible = true;
            //}
            string qtr = ddlQuarter.Text.ToLower();

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

            //ViewEditButton.Text = (currqtr2.ToLower() == qtr /*&& Session[Constants.IsFreezed].ToString() == "0"*/) ? "Edit" : "View";
            int qc = Convert.ToInt32(currqtr2.Substring(1, 1));
            int q = Convert.ToInt32(qtr.Substring(1, 1));
            int yc = Convert.ToInt32(currqtr2.Substring(3, 2));
            int y = Convert.ToInt32(qtr.Substring(3, 2));
            if (ddlExpenseType.Text == "Subcons - Existing")
            {

                btnAddNew.Visible = false;
                btnCopyRow.Visible = false;
                btnDelete.Visible = false;

            }
            else
            {
                if (y <= yc)
                {
                    if (q < qc)
                    {

                        btnAddNew.Visible = false;
                        btnCopyRow.Visible = false;
                        btnDelete.Visible = false;
                    }
                    else
                    {
                        btnAddNew.Visible = true;
                        btnCopyRow.Visible = true;
                        btnDelete.Visible = true;
                    }
                }
                else
                {
                    btnAddNew.Visible = true;
                    btnCopyRow.Visible = true;
                    btnDelete.Visible = true;
                }
            }

        }

        private void LoadData()
        {
            //string pucode = "ORC";// Request.QueryString["pu"] + "";
            //string ducode = "Abhishek_Goyal";// Request.QueryString["du"] + "";
            expenseType = /*"Subcons - New"; */"Subcons - Existing";// Request.QueryString["exptype"] + "";
            quareter = "Q3'16";// Request.QueryString["quarter"] + "";
            // Session["Qtr"] = quareter;
            _tempexptype = expenseType.Split(',').FirstOrDefault() + "";
            from = "Requirement";// Request.QueryString["from"] + ""; // from requirement screen or finalisation screen 
            mcc = "ALL";
            currentTemplate = (service.GetTemplateID(_tempexptype) + "").Trim();
        }

        private void SearchData()
        {
            //string pucode = Request.QueryString["pu"] + "";
            //string ducode = Request.QueryString["du"] + "";
            expenseType = Request.QueryString["exptype"] + "";
            quareter = Request.QueryString["quarter"] + "";
            // Session["Qtr"] = quareter;
            _tempexptype = expenseType.Split(',').FirstOrDefault() + "";
            from = Request.QueryString["from"] + ""; // from requirement screen or finalisation screen
            mcc = Request.QueryString["mcc"] + "";
            currentTemplate = (service.GetTemplateID(_tempexptype) + "").Trim();
        }

        private void BindGrid()
        {
            //topnav.Visible = false;
            //List<ExpenseTemplateData> lstSettings = new List<ExpenseTemplateData>();
            btnSave.Visible = false;
            // onload
            currentWorkFlowStage = Convert.ToInt32(File.ReadAllText(Server.MapPath("/Storage/") + "WorkflowStage.txt").Trim());
            //TODO: testing 
            role = Session["Role"] + "";
            role = role.ToLower().Contains("pna") ? "PNA" : role;
            role = role.ToLower();

            //if (role == "sdm" || role == "dh" || role.Contains("pna") || role == "admin")
            //{

            //}
            //else
            //{
            //    //menuDistribution.Attributes.Add("onclick", "alert('Access Denied'); return false;");
            //}

            string machineUserID = HttpContext.Current.User.Identity.Name;

            string[] userids = machineUserID.Split('\\');
            if (userids.Length == 2)
                machineUserID = userids[1];

            bool isMachineUserisAdmin = service.GetRolee(machineUserID) == "Admin";



            btnCopyRow.ToolTip = "";
            btnDelete.ToolTip = "";
            btnCopyRow.Enabled = true;
            btnDelete.Enabled = true;
            btnAddNew.Enabled = true;
            hypAddOtherExpenses.Enabled = true;
            bool isFreezed = Session[Constants.IsFreezed] + "" == "1" ? true : false;
            if (isFreezed)
            {
                if (!isMachineUserisAdmin)
                {
                    btnCopyRow.Enabled = false;
                    btnDelete.Enabled = false;
                    btnAddNew.Enabled = false;
                    hypAddOtherExpenses.Enabled = false;
                    btnCopyRow.ToolTip = Session[Constants.FreezedText] + "";
                    btnDelete.ToolTip = Session[Constants.FreezedText] + "";
                    btnAddNew.ToolTip = Session[Constants.FreezedText] + "";
                    hypAddOtherExpenses.ToolTip = Session[Constants.FreezedText] + "";
                }
            }

            ////string pucode = "ORC";// Request.QueryString["pu"] + "";
            ////string ducode = "Abhishek_Goyal";// Request.QueryString["du"] + "";
            //string expenseType = /*"Subcons - New"; */"CCD - Communications - One Time Cost";// Request.QueryString["exptype"] + "";
            //string quareter = "Q3'16";// Request.QueryString["quarter"] + "";
            //// Session["Qtr"] = quareter;
            //string _tempexptype = expenseType.Split(',').FirstOrDefault() + "";
            //string from = "Requirement";// Request.QueryString["from"] + ""; // from requirement screen or finalisation screen 
            //string mcc = "None";
            //currentTemplate = (service.GetTemplateID(_tempexptype) + "").Trim();

            userID = Session["UserID"] + "";

            string message = Request.QueryString["Message"] + "";

            if (message.Length > 0)
                hdnfldMessage.Value = message;



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
            //ddlQuarter.Items.Add(currqtr2);
            //ddlQuarter.Items.Add(lastqtr2);

            //ddlQuarter.Items.Add(nextQtrPlus1);
            //ddlQuarter.Items.Add(nextQtrPlus2);
            //ddlQuarter.Items.Add("All");

            LoadCombobox(/*pucode, ducode,*/mcc, expenseType, quareter);

            if (expenseType == "Subcons - Existing")
            {
                btnAddNew.Visible = false;
            }
            else
            {
                btnAddNew.Visible = true;
            }

            lnkExportExcel.OnClientClick = "return ChkDataIsPresent();";



            //string _expenseType = expenseType.Replace("&", "%26");
            //_expenseType = _expenseType.Replace(" ", "%20");
            //hdnPass.Value = string.Format("&PU={0}&DU={1}&ExpenseType={2}&Quarter={3}&CurrentTemplate={4}", pucode, ducode, _expenseType, quareter, currentTemplate);
            //var pass = string.Format("'{0}','{1}','{2}','{3}','{4}'", pucode, ducode, _expenseType, quareter, currentTemplate);
            //hypAddOtherExpenses.OnClientClick = "return  PopUpOtherExpenses(" + pass + ")";
            ////btnAddNew.OnClientClick= "return PopUpAddNew()";


            //string UserSU = Session["UserSU"] + "";

            lstSettings = service.GetExpenseTemplateData().Where(k => k.ExpTemplateId.ToLower().Trim() == currentTemplate.ToLower() && k.IsActive).ToList();
            lstMiscSettings = service.GetExpenseTemplateData().Where(k => k.ExpTemplateId.ToLower().Trim() == "misc" && k.IsActive).ToList();


            List<ExpenseColumns> lstAllColumns = service.GetExpenseColumnsEntity();

            Action<string> process = (k1) =>
            {
                string contorlname = lstAllColumns.SingleOrDefault(k => k.ColumnName == k1).ControlName;
                if (!contorlname.StartsWith("ddl"))  // for now removing the ddl from validation ... in furtue to be added 
                    commaseperatedColumns += contorlname + ",";
            };
            commaseperatedColumns = commaseperatedColumns.Trim().TrimEnd(',').TrimStart(',');

            lstSettings.Where(k => k.IsMandatory).Select(k => k.ColumnName).ToList().ForEach(process);
            // btnSave.Attributes.Add("onclick", " return ValidateCheckBoxes('" + commaseperatedColumns + "');");


            // columns ordering 
            DataControlFieldCollection gvdcfCollection = grdBEData.Columns.CloneFields();
            //Response.Write(grdBEData.Columns[0].HeaderText);
            DataControlField colSelect = grdBEData.Columns[0];
            DataControlField colEidt = grdBEData.Columns[1];

            //DataControlField colSDMStatus = grdBEData.Columns[62]; // earlier 55 ,, incrementing +1 to the below
            //DataControlField colSDMApprovedAmount = grdBEData.Columns[63];
            //DataControlField colDHStatus = grdBEData.Columns[64];
            //DataControlField colDHApprovedAmount = grdBEData.Columns[65];
            //DataControlField colPNAStatus = grdBEData.Columns[66];
            //DataControlField colPNAApprovedAmount = grdBEData.Columns[67];


            DataControlField colTotalAmount = grdBEData.Columns[68];

            DataControlField colDm = grdBEData.Columns[69];

            DataControlField colhidden = grdBEData.Columns[70];

            //DataControlField colSelect = grdBEData.Columns[0]; 
            //DataControlField colEidt = grdBEData.Columns[0]; 
            //DataControlField colTotalAmount = grdBEData.Columns[0]; 
            //DataControlField colDm = grdBEData.Columns[0]; 
            //DataControlField colhidden = grdBEData.Columns[0];  

            //for (int i = 0; i < grdBEData.Columns.Count; i++)
            //{
            //    if (grdBEData.Columns[i].HeaderText == "Select")
            //    {
            //        colSelect = grdBEData.Columns[i];
            //    }
            //    if (grdBEData.Columns[i].HeaderText == "Edit")
            //    {
            //       colEidt = grdBEData.Columns[i];
            //    }
            //    if (grdBEData.Columns[i].HeaderText == "Ask (k$)")
            //    {
            //         colTotalAmount = grdBEData.Columns[i];
            //    }
            //    if (grdBEData.Columns[i].HeaderText == "DM")
            //    {
            //        colDm = grdBEData.Columns[i];
            //    }
            //    else
            //    {
            //        colDm = grdBEData.Columns[69];
            //    }
            //    if (grdBEData.Columns[i].HeaderText == "")
            //    {
            //        colhidden = grdBEData.Columns[i];
            //    }
            //    else
            //    {
            //        colhidden = grdBEData.Columns[70];
            //    }
            //}

            grdBEData.Columns.Clear();

            var templateData = lstSettings;
            var usedTopColumns = templateData.Where(k => k.ExpTemplateId.ToLower().Trim() == currentTemplate.ToLower().Trim() && k.IsActive).Where(k => k.ColumnPosition != null);
            var usedBottomColumns = templateData.Where(k => k.ExpTemplateId.ToLower().Trim() == currentTemplate.ToLower().Trim() && k.IsActive).Where(k => k.ColumnPosition == null);
            List<ExpenseTemplateData> lstOrdering = usedTopColumns.OrderBy(k => k.ColumnPosition).ToList();
            lstOrdering.AddRange(usedBottomColumns.OrderBy(k => k.ColumnPosition).ToList());


            // grdBEData.Columns.Add(colEidt);


            grdBEData.Columns.Add(colSelect);
            grdBEData.Columns.Add(colEidt);
            grdBEData.Columns.Add(colTotalAmount);
            grdBEData.Columns.Add(colDm);

            var columns = gvdcfCollection.Cast<DataControlField>().ToList();
            System.Text.StringBuilder ss = new System.Text.StringBuilder();
            ss.Append(colTotalAmount);
            ss.Append("," + colDm);
            foreach (var item in lstOrdering)
            {
                var col = columns.SingleOrDefault(k => k.HeaderText == item.ColumnName);
                if (col != null)
                    ss.Append("," + col);
                grdBEData.Columns.Add(col);
            }
            hdnCol.Value = ss.ToString();




            string commaSeperatedDateColumns = "";

            Action<string> processDateColumns = (k1) =>
            {
                string contorlname = lstAllColumns.SingleOrDefault(k => k.ColumnName == k1).ControlName;
                commaSeperatedDateColumns += contorlname + ",";
            };
            commaSeperatedDateColumns = commaSeperatedDateColumns.Trim().TrimEnd(',').TrimStart(',');
            lstSettings.Where(k => k.ColumnType.ToLower() == "date" && k.IsEditable == true).Select(k => k.ColumnName).ToList().ForEach(processDateColumns);



            commaseperatedColumns = "";
            Action<string> processCol = (k1) =>
            {
                string contorlname = lstAllColumns.SingleOrDefault(k => k.ColumnName == k1).ControlName;
                // if (contorlname.StartsWith("ddl"))  // for now removing the ddl from validation ... in furtue to be added 
                commaseperatedColumns += contorlname + ",";
            };
            commaseperatedColumns = commaseperatedColumns.Trim().TrimEnd(',').TrimStart(',');
            lstSettings.Where(k => k.IsMandatory).Select(k => k.ColumnName).ToList().ForEach(processCol);


            string commaseperatedCtColumns = "";
            Action<string> processCol1 = (k1) =>
            {
                string contorlname = lstAllColumns.SingleOrDefault(k => k.ColumnName == k1).ControlName;
                // if (contorlname.StartsWith("ddl"))  // for now removing the ddl from validation ... in furtue to be added 
                commaseperatedCtColumns += contorlname + ",";
            };
            commaseperatedCtColumns = commaseperatedCtColumns.Trim().TrimEnd(',').TrimStart(',');
            lstSettings.OrderBy(k => k.ColumnPosition).Select(k => k.ColumnName).ToList().ForEach(processCol1);

            hdnCt.Value = "txtTotalAmount" + "," + "txtDM" + "," + commaseperatedCtColumns;

            hdnDateCol.Value = commaSeperatedDateColumns;
            hdnMandCol.Value = commaseperatedColumns;








            int staticheight = 150;
            int variableheight = lstOrdering.Count / 2;
            hdnpopupHeight.Value = (staticheight + (variableheight * 26.2)).ToString();

            Amounts objAmounts = new Amounts();

            grdBEData.Columns.Add(colhidden);


            List<MasterEntity> lstDataSoruce = new List<MasterEntity>();

            lstDataSoruce = service.GetExpenseMasterData(/*pucode, ducode,*/userID, mcc, expenseType, quareter/*, cat.Cat, cat.PL, status*/);
            lstDataSoruce = lstDataSoruce == null ? new List<MasterEntity>() : lstDataSoruce;

            int index = 0;
            int.TryParse(Request.QueryString["pagerindex"] + "", out index);
            grdBEData.PageIndex = index;
            grdBEData.DataSource = lstDataSoruce;
            grdBEData.DataBind();
            ViewState["Data"] = lstDataSoruce;
            if (from.ToLower() == "requirement")
            {
                trowFilter.Visible = true;
            }

        }

        public void VisibilityControl(bool save, bool add, bool copy, bool delete, bool other)
        {
            btnSave.Visible = save;
            btnAddNew.Visible = add;
            btnCopyRow.Visible = copy;
            btnDelete.Visible = delete;
            hypAddOtherExpenses.Visible = other;
        }








        private void RemoveUnwantedColumns(GridView grdBEData, List<ExpenseTemplateData> lstSettings)
        {
            try
            {
                for (int i = 0; i < grdBEData.Columns.Count; i++)
                {
                    var currentHeaderText = grdBEData.Columns[i].HeaderText;
                    ExpenseTemplateData temp = lstSettings.SingleOrDefault(k => k.ColumnName.ToLower().Trim() == currentHeaderText.Trim().ToLower());
                    if (temp == null)
                        grdBEData.Columns[i].Visible = false;

                }
                grdBEData.Columns[0].Visible = true;  // check  box field 
                grdBEData.Columns[1].Visible = true;  // hyperlink edit 
                grdBEData.Columns[54].Visible = true; // hidden field 
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
            //string userSU = Session["UserSU"] + "";
            string currentTemplate = (service.GetTemplateID(ddlExpenseType.Text) + "").Trim();
            List<ExpenseTemplateData> lstSettings = new List<ExpenseTemplateData>();
            lstSettings = service.GetExpenseTemplateData().Where(k => k.ExpTemplateId.ToLower().Trim() == currentTemplate.ToLower() && k.IsActive).ToList();

            var temp = lstSettings.SingleOrDefault(k => k.ColumnName == columnName);
            return temp;
        }



        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //expenseType = ddlExpenseType.Text;
            //quareter = ddlQuarter.Text;
            //// Session["Qtr"] = quareter;
            //_tempexptype = expenseType.Split(',').FirstOrDefault() + "";
            //from = "Requirement"; // from requirement screen or finalisation screen
            //mcc = ddlMCC.Text;
            //currentTemplate = (service.GetTemplateID(_tempexptype) + "").Trim();
            //BindGrid();
            string copyID = hdnCopiedID.Value + "";
            int coun = grdBEData.Rows.Count;


            //foreach (GridViewRow item in grdBEData.Rows)
            //{
            //    if ((item.Cells[0].FindControl("chkRow") as CheckBox).Checked)
            //    {

            //    }
            //}
            int rowIndex = 0;
            for (int i = 1; i <= grdBEData.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)grdBEData.Rows[rowIndex].Cells[0].FindControl("chkRow");
                TextBox txt = (TextBox)grdBEData.Rows[rowIndex].Cells[2].FindControl("txtClientCode");
                string t = txt.Text;
                rowIndex++;
            }

            //string qurt = ddlQuarter.Text;
            //string _expenseType = expenseType.Replace("&", "%26");
            //_expenseType = _expenseType.Replace(" ", "%20");
            //string urlFormat = "SubConHome.aspx?mcc={0}&exptype={1}&quarter={2}&Message=Row(s) deleted successfully !";
            //string NavigateUrl = string.Format(urlFormat,/* pucode, ducode,*/mcc, _expenseType, qurt);
            //Response.Redirect(NavigateUrl);

            //string[] IDs = copyID.Split(',');
            //bool deleted = false;

            //if (IDs.Length > 0)
            //{
            //    foreach (string s in IDs)
            //    {
            //        var canproceed = s.Trim().Length > 0;

            //        if (canproceed)
            //        {
            //            int id = Convert.ToInt32(s);

            //        }
            //    }
            //}

        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //string pucode = ddlPU.Text;
            //string ducode = hdnfldddlDM.Value;
            string expenseType = ddlExpenseType.Text;
            string qurt = ddlQuarter.Text;
            string mcc = ddlMCC.Text;

            try
            {

                string copyID = hdnCopiedID.Value + "";

                string[] IDs = copyID.Split(',');
                bool deleted = false;

                if (IDs.Length > 0)
                {
                    foreach (string s in IDs)
                    {
                        var canproceed = s.Trim().Length > 0;

                        if (canproceed)
                        {
                            int id = Convert.ToInt32(s);
                            service.DeleteMasterData(id);
                            deleted = true;
                        }
                    }
                }
                if (deleted)
                {
                    //divMessage.Visible = true;
                    //divMessage.InnerText = "Row(s) deleted successfully !";
                    // Button1_Click1(null, null);
                    string _expenseType = expenseType.Replace("&", "%26");
                    _expenseType = _expenseType.Replace(" ", "%20");
                    string urlFormat = "SubConHome.aspx?mcc={0}&exptype={1}&quarter={2}&Message=Row(s) deleted successfully !";
                    string NavigateUrl = string.Format(urlFormat,/* pucode, ducode,*/mcc, _expenseType, qurt);
                    Response.Redirect(NavigateUrl);
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

        private void LoadCombobox(/*string pucode, string ducode,*/string mcc, string expenseType, string quareter)
        {


            try
            {

                lstPUDM = service.GetPUDMMapping(userID);


                string csvpudm = string.Empty;
                foreach (var item in lstPUDM)
                    csvpudm += item.PU + "," + item.DM + "|";



                csvpudm = csvpudm.Trim().TrimEnd('|').TrimStart('|');
                hdnfldDMCSV.Value = csvpudm;
                ////////////////////////////////////
                //ddlPU.Items.Clear();
                //var pus = lstPUDM.Select(k => k.PU).Distinct().ToList();
                //foreach (string rh in pus)
                //    ddlPU.Items.Add(rh);

                //ddlPU.Text = pucode;



                //ddlDM.Items.Clear();
                //var DMs = lstPUDM.Where(k => k.PU == pucode).Select(k => k.DM).Distinct().OrderBy(k => k).ToList();
                //foreach (string rh in DMs)
                //    ddlDM.Items.Add(rh);

                //ddlDM.Text = ducode;
                //hdnfldddlDM.Value = ddlDM.Text;
                ////////////////////////////////////////////
                //DateTime todaydate = DateTime.Now;
                //string lastqtr2 = "";
                //string currqtr2 = "";
                //int year = todaydate.Year - 2000;
                //int nextyear = year + 1;
                //string cmdstrquarter = "SELECT [strquarter]   FROM [ExpConfig]";
                //DataSet ds = service.GetDataSet(cmdstrquarter);
                //DataTable dt = ds.Tables[0];
                //string strquarter = dt.Rows[0][0].ToString();
                //if (strquarter == "Q1")
                //{
                //    lastqtr2 = "Q4'" + year;
                //    currqtr2 = "Q1'" + nextyear;
                //}

                //if (strquarter == "Q2")
                //{
                //    lastqtr2 = "Q1'" + nextyear;
                //    currqtr2 = "Q2'" + nextyear;
                //}
                //if (strquarter == "Q3")
                //{
                //    lastqtr2 = "Q2'" + nextyear;
                //    currqtr2 = "Q3'" + nextyear;
                //}
                //if (strquarter == "Q4")
                //{
                //    lastqtr2 = "Q3'" + nextyear;
                //    currqtr2 = "Q4'" + nextyear;
                //}

                //if (quareter.ToLower() == "current")
                //{
                //    ddlQuarter.Text = currqtr2;
                //}
                //else
                //{
                //    ddlQuarter.Text = lastqtr2;
                //}

                string PrevQtr = DateUtility.GetQuarter("prev");
                Session["PreviousQuarter"] = PrevQtr;

                // string PrevQtr = DateUtility.GetQuarter("prev");
                string currentQtr = DateUtility.GetQuarter("current");
                string nextQtr = DateUtility.GetQuarter("next");
                string nextQtrPlus1 = DateUtility.GetQuarter("next1");

                ddlQuarter.Text = currentQtr;
                ddlQuarter.Items.Insert(0, PrevQtr);
                ddlQuarter.Items.Insert(1, currentQtr);
                ddlQuarter.Items.Insert(2, nextQtr);
                ddlQuarter.Items.Insert(3, nextQtrPlus1);
                ddlQuarter.Text = quareter;

                List<string> lstItems = service.GetSpDDLItems("spBEExpGetExpenseType");

                if (lstItems != null && lstItems.Count > 1)
                    lstItems.Remove("Expenses - Existing");
                ddlExpenseType.DataSource = lstItems;
                ddlExpenseType.DataBind();

                ddlExpenseType.Text = expenseType;

                List<string> lstMCC = service.GetSpDDLItems("spBEExpClientCodePortfolioList", userID);

                ddlMCC.DataSource = lstMCC;
                ddlMCC.DataBind();
                ddlMCC.Text = mcc;
                //ddlMCC.Text = MasterClientCode;
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





        protected void grdBEData_RowCreated(object sender, GridViewRowEventArgs e)
        {


            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    string qtr = ddlQuarter.Text.ToLower();
                    var ViewEditButton = e.Row.Cells[1];
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

                    //ViewEditButton.Text = (currqtr2.ToLower() == qtr /*&& Session[Constants.IsFreezed].ToString() == "0"*/) ? "Edit" : "View";
                    int qc = Convert.ToInt32(currqtr2.Substring(1, 1));
                    int q = Convert.ToInt32(qtr.Substring(1, 1));
                    int yc = Convert.ToInt32(currqtr2.Substring(3, 2));
                    int y = Convert.ToInt32(qtr.Substring(3, 2));
                    if (ddlExpenseType.Text == "Subcons - Existing")
                    {
                        if (yc <= y)
                        {
                            if (q < qc)
                            {
                                ViewEditButton.Text = "VIEW";
                                btnAddNew.Visible = false;
                                btnCopyRow.Visible = false;
                                btnDelete.Visible = false;
                                hdnCtrl.Value = "VIEW";
                            }
                            else
                            {
                                ViewEditButton.Text = "SAVE";
                                hdnCtrl.Value = "SAVE";
                            }
                        }
                        else
                        {
                            ViewEditButton.Text = "SAVE";
                            hdnCtrl.Value = "SAVE";
                        }
                    }
                    else
                    {
                        if (yc <= y)
                        {
                            if (q < qc)
                            {
                                ViewEditButton.Text = "VIEW";
                                btnAddNew.Visible = false;
                                btnCopyRow.Visible = false;
                                btnDelete.Visible = false;
                                hdnCtrl.Value = "VIEW";
                            }
                            else
                            {
                                ViewEditButton.Text = "Edit";
                                hdnCtrl.Value = "Edit";
                            }
                        }
                        else
                        {
                            ViewEditButton.Text = "Edit";
                            hdnCtrl.Value = "Edit";
                        }
                    }

                    for (int i = 0; i < grdBEData.Columns.Count; i++)
                    {
                        var currentHeaderText = e.Row.Cells[i].Text;
                        ExpenseTemplateData temp = lstSettings.SingleOrDefault(k => k.ColumnName.ToLower().Trim() == currentHeaderText.Trim().ToLower());
                        if (temp != null)
                            e.Row.Cells[i].Text = temp.DisplayText;
                        else
                        {
                            ExpenseTemplateData tempMisc = lstMiscSettings.SingleOrDefault(k => k.ColumnName.ToLower().Trim() == currentHeaderText.Trim().ToLower());
                            if (tempMisc != null)
                                e.Row.Cells[i].Text = tempMisc.DisplayText;

                        }

                    }

                }




                if (e.Row.RowType == DataControlRowType.DataRow)
                {
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

                    //TODO: Changes Phase 2

                    var ViewEditButton = e.Row.Cells[1].Controls.OfType<LinkButton>().FirstOrDefault();
                    string qtr = ddlQuarter.Text.ToLower();
                    //if (ViewEditButton != null)
                    //    ViewEditButton.Text = (currqtr2.ToLower() == qtr /*&& Session[Constants.IsFreezed].ToString() == "0"*/) ? "Edit" : "View";

                    int qc = Convert.ToInt32(currqtr2.Substring(1, 1));
                    int q = Convert.ToInt32(qtr.Substring(1, 1));
                    int yc = Convert.ToInt32(currqtr2.Substring(3, 2));
                    int y = Convert.ToInt32(qtr.Substring(3, 2));
                    if (ddlExpenseType.Text == "Subcons - Existing")
                    {
                        if (yc <= y)
                        {
                            if (q < qc)
                            {
                                ViewEditButton.Text = "VIEW";
                                btnAddNew.Visible = false;
                                btnCopyRow.Visible = false;
                                btnDelete.Visible = false;
                                hdnCtrl.Value = "VIEW";
                            }
                            else
                            {
                                ViewEditButton.Text = "SAVE";
                                hdnCtrl.Value = "SAVE";
                            }
                        }
                        else
                        {
                            ViewEditButton.Text = "SAVE";
                            hdnCtrl.Value = "SAVE";
                        }
                    }
                    else
                    {
                        if (yc <= y)
                        {
                            if (q < qc)
                            {
                                ViewEditButton.Text = "VIEW";
                                btnAddNew.Visible = false;
                                btnCopyRow.Visible = false;
                                btnDelete.Visible = false;
                                hdnCtrl.Value = "VIEW";
                            }
                            else
                            {
                                ViewEditButton.Text = "Edit";
                                hdnCtrl.Value = "Edit";
                            }
                        }
                        else
                        {
                            ViewEditButton.Text = "Edit";
                            hdnCtrl.Value = "Edit";
                        }
                    }




                    //List<ExpenseColumns> lstAllColumns = service.GetExpenseColumnsEntity();
                    //string commaSeperatedDateColumns = "";
                    //Action<string> processDateColumns = (k1) =>
                    //{
                    //    string contorlname = lstAllColumns.SingleOrDefault(k => k.ColumnName == k1).ControlName;
                    //    commaSeperatedDateColumns += contorlname + ",";
                    //};
                    //commaSeperatedDateColumns = commaSeperatedDateColumns.Trim().TrimEnd(',').TrimStart(',');
                    //lstSettings.Where(k => k.ColumnType.ToLower() == "date").Select(k => k.ColumnName).ToList().ForEach(processDateColumns);commaseperatedColumns = "";
                    //Action<string> process = (k1) =>
                    //{
                    //    string contorlname = lstAllColumns.SingleOrDefault(k => k.ColumnName == k1).ControlName;
                    //    // if (contorlname.StartsWith("ddl"))  // for now removing the ddl from validation ... in furtue to be added 
                    //    commaseperatedColumns += contorlname + ",";
                    //};
                    //commaseperatedColumns = commaseperatedColumns.Trim().TrimEnd(',').TrimStart(',');
                    //lstSettings.Where(k => k.IsMandatory).Select(k => k.ColumnName).ToList().ForEach(process);
                    //ViewEditButton.Attributes.Add("onclick", " return ValidateMandatoryColumns('" + commaseperatedColumns + "', '" + commaSeperatedDateColumns + "');");






                    SetRowValue(e.Row, "txtClientCode", "ClientCode", DataBinder.Eval(e.Row.DataItem, "ClientCode"));
                    SetRowValue(e.Row, "hdnfld", "intExpId", DataBinder.Eval(e.Row.DataItem, "intExpId"));
                    SetRowValue(e.Row, "ddlPUCode", "PUCode", DataBinder.Eval(e.Row.DataItem, "PUCode"));
                    SetRowValue(e.Row, "ddlBUCode", "BUCode", DataBinder.Eval(e.Row.DataItem, "BUCode"));
                    SetRowValue(e.Row, "ddlDUCode", "DUCode", DataBinder.Eval(e.Row.DataItem, "DUCode"));
                    SetRowValue(e.Row, "ClientCode", "ClientCode", DataBinder.Eval(e.Row.DataItem, "ClientCode"));
                    SetRowValue(e.Row, "ddlExpType", "ExpType", DataBinder.Eval(e.Row.DataItem, "ExpType"));
                    SetRowValue(e.Row, "ddlExpCategory", "ExpCategory", DataBinder.Eval(e.Row.DataItem, "ExpCategory"));
                    SetRowValue(e.Row, "txtItemName", "ItemName", DataBinder.Eval(e.Row.DataItem, "ItemName"));
                    SetRowValue(e.Row, "ddlPriority", "Priority", DataBinder.Eval(e.Row.DataItem, "Priority"));
                    SetRowValue(e.Row, "txtNumberofItems", "NumberofItems", DataBinder.Eval(e.Row.DataItem, "NumberofItems"));
                    SetRowValue(e.Row, "txtUnitCost", "UnitCost", DataBinder.Eval(e.Row.DataItem, "UnitCost"));
                    SetRowValue(e.Row, "dtpExpenseDate", "ExpenseDate", DataBinder.Eval(e.Row.DataItem, "ExpenseDate"));
                    SetRowValue(e.Row, "txtJustificationRemarks", "JustificationRemarks", DataBinder.Eval(e.Row.DataItem, "JustificationRemarks"));
                    SetRowValue(e.Row, "ddlIsCustomerRecoverable", "IsCustomerRecoverable", DataBinder.Eval(e.Row.DataItem, "IsCustomerRecoverable"));
                    SetRowValue(e.Row, "txtProjOppCode", "ProjOppCode", DataBinder.Eval(e.Row.DataItem, "ProjOppCode"));
                    SetRowValue(e.Row, "ddlIsBudgetedinPBS", "IsBudgetedinPBS", DataBinder.Eval(e.Row.DataItem, "IsBudgetedinPBS"));
                    SetRowValue(e.Row, "txtBEUpside", "BEUpside", DataBinder.Eval(e.Row.DataItem, "BEUpside"));
                    SetRowValue(e.Row, "txtBEDownside", "BEDownside", DataBinder.Eval(e.Row.DataItem, "BEDownside"));
                    SetRowValue(e.Row, "txtCurrQtr", "CurrQtr", DataBinder.Eval(e.Row.DataItem, "CurrQtr"));
                    SetRowValue(e.Row, "txtFutQtrBE", "FutQtrBE", DataBinder.Eval(e.Row.DataItem, "FutQtrBE"));
                    SetRowValue(e.Row, "ddlStatus", "Status", DataBinder.Eval(e.Row.DataItem, "Status"));
                    SetRowValue(e.Row, "dtpFieldDate1", "FieldDate1", DataBinder.Eval(e.Row.DataItem, "FieldDate1"));
                    SetRowValue(e.Row, "dtpFieldDate2", "FieldDate2", DataBinder.Eval(e.Row.DataItem, "FieldDate2"));
                    SetRowValue(e.Row, "dtpFieldDate3", "FieldDate3", DataBinder.Eval(e.Row.DataItem, "FieldDate3"));
                    SetRowValue(e.Row, "dtpFieldDate4", "FieldDate4", DataBinder.Eval(e.Row.DataItem, "FieldDate4"));
                    SetRowValue(e.Row, "dtpFieldDate5", "FieldDate5", DataBinder.Eval(e.Row.DataItem, "FieldDate5"));
                    SetRowValue(e.Row, "dtpFieldDate6", "FieldDate6", DataBinder.Eval(e.Row.DataItem, "FieldDate6"));
                    SetRowValue(e.Row, "dtpFieldDate7", "FieldDate7", DataBinder.Eval(e.Row.DataItem, "FieldDate7"));
                    SetRowValue(e.Row, "dtpFieldDate8", "FieldDate8", DataBinder.Eval(e.Row.DataItem, "FieldDate8"));
                    SetRowValue(e.Row, "txtCreatedBy", "CreatedBy", DataBinder.Eval(e.Row.DataItem, "CreatedBy"));
                    SetRowValue(e.Row, "txtCreatedOn", "CreatedOn", DataBinder.Eval(e.Row.DataItem, "CreatedOn"));
                    SetRowValue(e.Row, "txtModifiedBy", "ModifiedBy", DataBinder.Eval(e.Row.DataItem, "ModifiedBy"));
                    SetRowValue(e.Row, "txtModifiedOn", "ModifiedOn", DataBinder.Eval(e.Row.DataItem, "ModifiedOn"));
                    SetRowValue(e.Row, "txtFieldtxt1", "Fieldtxt1", DataBinder.Eval(e.Row.DataItem, "Fieldtxt1"));
                    SetRowValue(e.Row, "txtFieldtxt2", "Fieldtxt2", DataBinder.Eval(e.Row.DataItem, "Fieldtxt2"));
                    SetRowValue(e.Row, "txtFieldtxt3", "Fieldtxt3", DataBinder.Eval(e.Row.DataItem, "Fieldtxt3"));
                    SetRowValue(e.Row, "txtFieldtxt4", "Fieldtxt4", DataBinder.Eval(e.Row.DataItem, "Fieldtxt4"));
                    SetRowValue(e.Row, "txtFieldtxt5", "Fieldtxt5", DataBinder.Eval(e.Row.DataItem, "Fieldtxt5"));
                    SetRowValue(e.Row, "txtFieldtxt6", "Fieldtxt6", DataBinder.Eval(e.Row.DataItem, "Fieldtxt6"));
                    SetRowValue(e.Row, "txtFieldtxt7", "Fieldtxt7", DataBinder.Eval(e.Row.DataItem, "Fieldtxt7"));
                    SetRowValue(e.Row, "txtFieldtxt8", "Fieldtxt8", DataBinder.Eval(e.Row.DataItem, "Fieldtxt8"));
                    SetRowValue(e.Row, "txtFieldtxt9", "Fieldtxt9", DataBinder.Eval(e.Row.DataItem, "Fieldtxt9"));
                    SetRowValue(e.Row, "txtFieldtxt10", "Fieldtxt10", DataBinder.Eval(e.Row.DataItem, "Fieldtxt10"));
                    SetRowValue(e.Row, "txtFieldtxt11", "Fieldtxt11", DataBinder.Eval(e.Row.DataItem, "Fieldtxt11"));
                    SetRowValue(e.Row, "txtFieldtxt12", "Fieldtxt12", DataBinder.Eval(e.Row.DataItem, "Fieldtxt12"));
                    SetRowValue(e.Row, "txtFieldtxt13", "Fieldtxt13", DataBinder.Eval(e.Row.DataItem, "Fieldtxt13"));
                    SetRowValue(e.Row, "txtFieldtxt14", "Fieldtxt14", DataBinder.Eval(e.Row.DataItem, "Fieldtxt14"));
                    SetRowValue(e.Row, "txtFieldtxt15", "Fieldtxt15", DataBinder.Eval(e.Row.DataItem, "Fieldtxt15"));
                    SetRowValue(e.Row, "txtFieldtxt16", "Fieldtxt16", DataBinder.Eval(e.Row.DataItem, "Fieldtxt16"));
                    SetRowValue(e.Row, "txtFieldtxt17", "Fieldtxt17", DataBinder.Eval(e.Row.DataItem, "Fieldtxt17"));
                    SetRowValue(e.Row, "txtFieldtxt18", "Fieldtxt18", DataBinder.Eval(e.Row.DataItem, "Fieldtxt18"));
                    SetRowValue(e.Row, "ddlFieldList1", "FieldList1", DataBinder.Eval(e.Row.DataItem, "FieldList1"));
                    SetRowValue(e.Row, "ddlFieldList2", "FieldList2", DataBinder.Eval(e.Row.DataItem, "FieldList2"));
                    SetRowValue(e.Row, "ddlFieldList3", "FieldList3", DataBinder.Eval(e.Row.DataItem, "FieldList3"));
                    SetRowValue(e.Row, "ddlFieldList4", "FieldList4", DataBinder.Eval(e.Row.DataItem, "FieldList4"));
                    SetRowValue(e.Row, "ddlFieldList5", "FieldList5", DataBinder.Eval(e.Row.DataItem, "FieldList5"));
                    SetRowValue(e.Row, "ddlFieldList6", "FieldList6", DataBinder.Eval(e.Row.DataItem, "FieldList6"));
                    SetRowValue(e.Row, "ddlFieldList7", "FieldList7", DataBinder.Eval(e.Row.DataItem, "FieldList7"));
                    SetRowValue(e.Row, "ddlFieldList8", "FieldList8", DataBinder.Eval(e.Row.DataItem, "FieldList8"));
                    SetRowValue(e.Row, "ddlFieldList9", "FieldList9", DataBinder.Eval(e.Row.DataItem, "FieldList9"));
                    SetRowValue(e.Row, "ddlFieldList10", "FieldList10", DataBinder.Eval(e.Row.DataItem, "FieldList10"));

                    // newly added columns


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



        protected void SetRowValue(GridViewRow row, string controlName, string columnName, object value)
        {
            try
            {
                string columnType = "int";

                List<string> listValues = new List<string>();
                int width = 150;
                //bool k =false;




                string[] columnstobeRemoved = new string[] { "PUCode", "ExpType", "ExpCategory", "DUCode", "BUCode" };
                if (columnstobeRemoved.Contains(columnName))
                    return;
                //if (columnName != "intExpId")
                //{
                //    ExpenseTemplateData ss = new ExpenseTemplateData();
                //    ss = GetCurrentColumnSetting(columnName);

                //    if (ss != null)
                //    {
                //        k = ss.IsEditable;
                //    }
                //}
                if (columnName == "intExpId")
                {
                    HiddenField hdnfld = row.FindControl("hdnfld") as HiddenField;
                    // hdnfld.Value = value + "";
                    //return;
                }
                ExpenseTemplateData settings;


                //string[] arryMiscColumns = new string[] { "SDMStatus", "DHStatus", "PNAStatus", "SDMApprovedAmount", "DHApprovedAmount", "PNAApprovedAmount" };

                //if (arryMiscColumns.Contains(columnName))
                //    settings = GetMiscColumnSettings(columnName);
                //else
                settings = GetCurrentColumnSetting(columnName);
                if (settings == null)
                    return;
                bool isThisColumnEditable;
                if (ddlExpenseType.Text == "Subcons - Existing")
                {
                    isThisColumnEditable = settings.IsEditable;
                }
                else
                {
                    isThisColumnEditable = false;
                }
                if (settings != null)
                    width = settings.Width;
                string enteronly = "";
                enteronly = settings.ColumnType == "int" ? "number" : settings.ColumnType == "float" ? "decimal" : "text";

                if (controlName == "ddlPriority" || controlName == "ClientCode" || controlName == "ddlFieldList2" || controlName == "ddlFieldList6" || controlName == "ddlFieldList7" || controlName == "ddlFieldList9")
                {
                    if (settings != null)
                    {
                        columnType = settings.ColumnType;

                        if ((settings.ListValues + "").Trim().Length != 0)
                        {
                            if (settings.ListValues.Trim().ToLower() != "null")
                                listValues = settings.ListValues.Trim().Split(',').ToList();
                        }
                        // check for sp 
                        if ((settings.spName + "").Trim().Length != 0)
                        {
                            if (settings.spName.Trim().ToLower() != "null")
                                listValues = service.GetSpDDLItems(settings.spName.Trim(), userID);
                        }
                    }
                }

                if (columnType == ColumnTypes.INT || columnType == ColumnTypes.FLOAT || columnType == ColumnTypes.TEXT)
                {
                    TextBox txtbox = row.FindControl(controlName) as TextBox;

                    if (txtbox != null)
                    {
                        txtbox.Style.Add("text-align", settings.Allignment.ToLower());

                        txtbox.Width = width;


                        if (isThisColumnEditable == false)
                        {
                            txtbox.ReadOnly = true;
                        }

                        int digits = 0;
                        if (enteronly == "decimal")
                        {
                            txtbox.Attributes.Add("onKeydown", "return PressfloatOnly(event,this)"); digits = 15;
                            txtbox.Attributes.Add("onblur", "ValidateDigits(this," + digits + ");");
                        }

                        if (enteronly == "number")
                        {
                            txtbox.Attributes.Add("onKeydown", "return PressIntOnly(event,this)"); digits = 6;
                            txtbox.Attributes.Add("onblur", "ValidateDigits(this," + digits + ");");
                        }

                    }
                }

                if (columnType == ColumnTypes.DATE)
                {
                    TextBox txtbox = row.FindControl(controlName) as TextBox;
                    if (txtbox != null)
                    {
                        txtbox.Style.Add("text-align", settings.Allignment.ToLower());
                        txtbox.Width = width;
                        txtbox.Font.Name = "Calibri";
                        txtbox.Font.Size = 8;
                        if (isThisColumnEditable == false)
                        {
                            txtbox.ReadOnly = true;
                        }
                    }


                }

                if (columnType == ColumnTypes.LIST)
                {
                    DropDownList ddl = row.FindControl(controlName) as DropDownList;
                    if (ddl != null)
                    {
                        ddl.Style.Add("text-align", settings.Allignment.ToLower());
                        if (columnName == "ClientCode")
                        {
                            ddl.Width = 100;
                        }
                        else
                            ddl.Width = width;
                        ddl.CssClass = "Label";
                        ddl.DataSource = listValues;
                        ddl.DataBind();

                        if (value == null || (value + "").Trim() == "")
                        {
                            ddl.SelectedIndex = 0;
                            //ddl.Items.Insert(0, "");
                        }
                        else
                        {
                            if ((value + "").ToLower() == "null")
                                ddl.SelectedIndex = 0;
                            else
                            {
                                if (listValues.Count == 0)
                                    ddl.Items.Add((value + "").Trim());
                                else
                                    ddl.Text = (value + "").Trim();
                            }


                        }
                        if (isThisColumnEditable == false)
                        {
                            ddl.Attributes.Add("disabled", "disabled");
                        }
                        // case for phase 1 and phase 2 
                        //if (currentWorkFlowStage == 1 || currentWorkFlowStage == 2)
                        //{
                        //    MakeDDLReadOnly(value, width, settings, ddl, "");
                        //}                    
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

        private void MakeDDLReadOnly(object value, int width, ExpenseTemplateData settings, DropDownList ddl, string id)
        {
            var cell = ddl.Parent;
            cell.Controls.Clear();
            var temptextbox = new TextBox();
            if ((id + "") != "")
                temptextbox.ID = id;
            temptextbox.CssClass = "Label";
            temptextbox.Text = value + "";

            temptextbox.Style.Add("text-align", settings.Allignment.ToLower());
            temptextbox.Width = width;
            temptextbox.Attributes.Add("onKeydown", "return PressReadOnly(event,this)");
            cell.Controls.Add(temptextbox);
        }




        protected object GetRowValue(GridViewRow row, string controlName, string columnType)
        {
            object returnValue = default(object);
            try
            {

                if (columnType.ToLower() == ColumnTypes.INT)
                {
                    TextBox txtbox = row.FindControl(controlName) as TextBox;
                    if (txtbox == null)
                        returnValue = null;
                    else
                        try
                        {
                            returnValue = Convert.ToInt32(txtbox.Text.Trim());
                        }
                        catch (Exception)
                        {

                            returnValue = null;
                        }
                }


                if (columnType.ToLower() == ColumnTypes.FLOAT)
                {
                    TextBox txtbox = row.FindControl(controlName) as TextBox;
                    if (txtbox == null)
                        returnValue = null;
                    else
                        try
                        {
                            returnValue = Convert.ToDouble(txtbox.Text.Trim());
                        }
                        catch (Exception)
                        {

                            returnValue = null;
                        }
                }

                if (columnType.ToLower() == ColumnTypes.LIST)
                {
                    DropDownList ddl = row.FindControl(controlName) as DropDownList;
                    if (ddl == null)
                        returnValue = null;
                    else
                        returnValue = Convert.ToString(ddl.Text.Trim());
                }

                if (columnType.ToLower() == ColumnTypes.TEXT)
                {
                    TextBox txtbox = row.FindControl(controlName) as TextBox;
                    if (txtbox == null)
                        returnValue = null;
                    else
                        returnValue = Convert.ToString(txtbox.Text.Trim());
                }

                if (columnType.ToLower() == ColumnTypes.DATE)
                {
                    TextBox txtdate = row.FindControl(controlName) as TextBox;
                    if (txtdate == null)
                        returnValue = null;
                    else
                        try
                        {
                            returnValue = Convert.ToDateTime(txtdate.Text.Trim());
                        }
                        catch (Exception)
                        {

                            returnValue = null;
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
            return returnValue;
        }



        public string GetCurrentQuarter()
        {


            string strquarter = "";

            DateTime todaydate = DateTime.Now;
            int year = todaydate.Year - 2000;
            int nextyear = year + 1;
            if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)
                strquarter = "Q1'" + year;
            else if (todaydate.Month == 4 || todaydate.Month == 5 || todaydate.Month == 6)
                strquarter = "Q2'" + nextyear;
            else if (todaydate.Month == 7 || todaydate.Month == 8 || todaydate.Month == 9)
                strquarter = "Q3'" + nextyear;
            else
                strquarter = "Q4'" + nextyear;
            return strquarter;


        }




        protected void btnCopyRow_Click(object sender, EventArgs e)
        {
            //string pucode = ddlPU.Text;
            //string ducode = hdnfldddlDM.Value;
            string expenseType = ddlExpenseType.Text;
            string qurt = ddlQuarter.Text;
            string mcc = ddlMCC.Text;
            //string currentNextAllQtr = DateUtility.GetQuarter("current") == qurt ? "current" : DateUtility.GetQuarter("next") == qurt ? "next" :
            //  DateUtility.GetQuarter("next1") == qurt ? "next1" : "next2";

            try
            {

                string copyID = hdnCopiedID.Value + "";

                string[] IDs = copyID.Split(',');

                if (IDs.Length > 0)
                {
                    foreach (string s in IDs)
                    {
                        var canproceed = s.Trim().Length > 0;

                        if (canproceed)
                        {
                            int id = Convert.ToInt32(s);
                            // copy sp needs to be done...
                            // call the copy sp and reload the page .. simple  :)
                            service.CopyRowMasterData(id);
                        }
                    }
                }

                string _expenseType = expenseType.Replace("&", "%26");
                //_expenseType = _expenseType.Replace(" ", "%20");
                string urlFormat = "SubConHome.aspx?mcc={0}&exptype={1}&quarter={2}&Message=Row(s) copied successfully !";
                string NavigateUrl = string.Format(urlFormat,/* pucode, ducode,*/mcc, _expenseType, qurt);
                Response.Redirect(NavigateUrl);

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

        //protected void btnAddNew_Click(object sender, EventArgs e)
        //{


        //}

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    // Page_Load(sender, e);
        //}

        //protected void Button1_Click1(object sender, EventArgs e)
        //{
        //    //string pucode = ddlPU.Text;
        //    //string ducode = hdnfldddlDM.Value;
        //    string expenseType = ddlExpenseType.Text;
        //    string qurt = "Current";
        //    string mcc = ddlMCC.Text;
        //    //string currentNextAllQtr = DateUtility.GetQuarter("current") == qurt ? "current" : DateUtility.GetQuarter("next") == qurt ? "next" :
        //    //  DateUtility.GetQuarter("next1") == qurt ? "next1" : "next2";



        //    string _expenseType = expenseType.Replace("&", "%26");
        //    _expenseType = _expenseType.Replace(" ", "%20");
        //    string urlFormat = "SubConHome.aspx?mcc={0}&exptype={1}&quarter={2}";
        //    string NavigateUrl = string.Format(urlFormat,/* pucode, ducode,*/mcc, _expenseType, qurt);
        //    Response.Redirect(NavigateUrl);
        //}

        #region Export Region

        protected void lnkExportExcel_Click(object sender, EventArgs e)
        {

            try
            {
                string from = "Requirement";
                if (from.ToLower() == "requirement")
                {

                    //string pucode = ddlPU.Text;
                    //string ducode = hdnfldddlDM.Value;
                    string expenseType = ddlExpenseType.Text;
                    string qurt = ddlQuarter.Text;
                    string mcc = ddlMCC.Text;
                    string userID = Session["UserID"] + "";
                    //string currentNextAllQtr = DateUtility.GetQuarter("current") == qurt ? "current" : DateUtility.GetQuarter("next") == qurt ? "next" :
                    //  DateUtility.GetQuarter("next1") == qurt ? "next1" : "next2";

                    List<MasterEntity> lstDataSoruce = new List<MasterEntity>();
                    ExpTypePLExtCatMap cat = service.GetExpTypePLExtCatMap(expenseType);
                    cat = cat == null ? new ExpTypePLExtCatMap() : cat;

                    string status = Request.QueryString["status"] + "";

                    status = status == "" ? "None" : status;

                    lstDataSoruce = service.GetExpenseMasterData(/*pucode, ducode,*/userID, mcc, expenseType, qurt/*, cat.Cat, cat.PL, status*/);

                    var table = ToDataTable(lstDataSoruce);

                    DoExpenseReportFromRequirement(table);
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

        public DataTable ToDataTable<T>(IList<T> data)
        {

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value; table.Rows.Add(row);
            }
            return table;



        }

        private void DoExpenseReportFromRequirement(DataTable table)
        {

            string currentTemplate = (service.GetTemplateID(ddlExpenseType.Text) + "").Trim();
            List<ExpenseTemplateData> lstSettings = new List<ExpenseTemplateData>();
            //string userSU = Session["UserSU"] + "";
            lstSettings = service.GetExpenseTemplateData().Where(k => k.ExpTemplateId.ToLower().Trim() == currentTemplate.ToLower() && k.IsActive).ToList();

            // var grid = GridView1;
            var templateData = lstSettings;
            var usedTopColumns = templateData.Where(k => k.ExpTemplateId.ToLower().Trim() == currentTemplate.ToLower().Trim() && k.IsActive).Where(k => k.ColumnPosition != null);
            var usedBottomColumns = templateData.Where(k => k.ExpTemplateId.ToLower().Trim() == currentTemplate.ToLower().Trim() && k.IsActive).Where(k => k.ColumnPosition == null);
            List<ExpenseTemplateData> lstOrdering = usedTopColumns.OrderBy(k => k.ColumnPosition).ToList();
            lstOrdering.AddRange(usedBottomColumns.OrderBy(k => k.ColumnPosition).ToList());



            // header text changing 
            List<string> columnsTobeRemoved = new List<string>();

            foreach (DataColumn column in table.Columns)
            {
                var item = lstOrdering.SingleOrDefault(k => k.ColumnName == column.ColumnName);
                if (item != null) // column is present 
                {
                    column.ColumnName = item.DisplayText;
                }
                else
                {
                    columnsTobeRemoved.Add(column.ColumnName);

                }
            }


            // unwanted column removal..
            foreach (string item in columnsTobeRemoved)
            {
                table.Columns.Remove(item);
            }

            // column ordering..


            //  removal of unwanted columns 
            int i = 0;
            try
            {
                foreach (ExpenseTemplateData data in lstOrdering)
                {


                    var column = table.Columns.Cast<DataColumn>().SingleOrDefault(k => k.ColumnName == data.DisplayText);
                    if (column != null)
                    {
                        column.SetOrdinal(i);
                        i++;
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
            DoExpenseReport(table);
        }


        private void DoExpenseReport(DataTable table)
        {
            try
            {
                System.Web.UI.WebControls.DataGrid grid = new System.Web.UI.WebControls.DataGrid();

                grid.HeaderStyle.Font.Bold = true;
                grid.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(141, 180, 226);
                grid.DataSource = table;
                grid.DataBind();

                string folder = "ExcelOperations";
                var MyDir = new DirectoryInfo(Server.MapPath(folder));


                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "" + ddlExpenseType.Text + ".xls") != null)
                    System.IO.File.Delete(MyDir.FullName + "\\" + ddlExpenseType.Text + ".xls");

                FileInfo file = new FileInfo(MyDir.FullName + "\\" + ddlExpenseType.Text + ".xls");

                using (StreamWriter sw = new StreamWriter(MyDir.FullName + "\\" + ddlExpenseType.Text + ".xls"))
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        grid.RenderControl(hw);
                    }

                }


                bool forceDownload = true;


                string path = MyDir.FullName + "\\" + ddlExpenseType.Text + ".xls";
                string name = "" + ddlExpenseType.Text + "" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls";
                string ext = Path.GetExtension(path);
                string type = "";
                // set known types based on file extension  
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
                            //type = "application/xls";
                            // type= "application/vnd.ms-excel";
                            break;
                    }
                }
                if (forceDownload)
                {
                    Response.AppendHeader("content-disposition",
                        "attachment; filename=" + name);
                }
                Response.Clear();
                if (type != "")
                    Response.ContentType = type;





                Response.WriteFile(path);

                Response.End();


                // HttpContext.Current.ApplicationInstance.CompleteRequest();


                //Server.Transfer(path);



            }
            catch (Exception ex)
            {

                GC.Collect();
                if (!ex.Message.Contains("Thread was being aborted"))
                {
                    logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }

            }



        }


        #endregion

        protected void grdBEData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            grdBEData_RowCreated(sender, e);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            string existingquery = Request.QueryString + "";

            //string pu = ddlPU.Text;
            //string du = ddlDM.Text;
            //du = hdnfldddlDM.Value;

            //if (du.Length == 0)
            //    du = ddlDM.Text;

            string exptyp = ddlExpenseType.Text;
            string qurt = ddlQuarter.Text;
            string mcc = ddlMCC.Text;

            //string currentNextAllQtr =  qurt.ToLower() == DateUtility.GetCurrentQuarter("current").ToLower() ? "current" : "next";

            string _expenseType = exptyp.Replace("&", "%26");
            _expenseType = _expenseType.Replace(" ", "%20");

            //string currentNextAllQtr = DateUtility.GetQuarter("current") == qurt ? "current" : DateUtility.GetQuarter("next") == qurt ? "next" :
            //    DateUtility.GetQuarter("next1") == qurt ? "next1" : "next2";


            //string currentNextAllQtr = qurt;

            string urlFormat = "SubConHome.aspx?mcc={0}&exptype={1}&quarter={2}&from={3}";
            string NavigateUrl = string.Format(urlFormat,/* pu, du,*/mcc, _expenseType, qurt, "Requirement");
            Response.Redirect(NavigateUrl);

            //List<MasterEntity> lstDataSoruce = new List<MasterEntity>();

            //lstDataSoruce = service.GetExpenseMasterData(/*pucode, ducode,*/mcc, exptyp, qurt/*, cat.Cat, cat.PL, status*/);
            //lstDataSoruce = lstDataSoruce == null ? new List<MasterEntity>() : lstDataSoruce;

            //int index = 0;
            //int.TryParse(Request.QueryString["pagerindex"] + "", out index);
            //grdBEData.PageIndex = index;
            //grdBEData.DataSource = lstDataSoruce;
            //grdBEData.DataBind();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string expHead = Request.QueryString["exphead"] + "";
            string dh = Request.QueryString["dh"] + "";
            string sdmlist = Request.QueryString["sdmlist"] + "";
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


            string qtr = currqtr2;
            userID = Session["UserID"] + "";
            Amounts objAmounts = new Amounts();
            bool canProceed = false;
            objAmounts = service.GetAllAmounts(userID, sdmlist, dh, qtr, expHead);

            string dhamount1 = objAmounts.DHAmount1.ToString();
            string shamount2 = objAmounts.SHAmount2.ToString();
            string askedamount3 = objAmounts.AskedAmount3.ToString();
            string finalisedamount4 = objAmounts.FinalisedAmount4.ToString();


            if (dhamount1 == hdnfld1DHallocated.Value)
                if (shamount2 == hdnfld2shortlistableamount.Value)
                    if (askedamount3 == hdnfld3askedamount.Value)
                        if (finalisedamount4 == hdnfld4finalisedamount.Value)
                            canProceed = true;

            if (!canProceed) // invalid data present 
            {

                string message = "&Message=Data mismatch occured, pls cross check the data. !";
                string NavigateUrl = "SubConHome.aspx?" + Request.QueryString;

                if (NavigateUrl.Contains("&Message=Data"))
                    NavigateUrl = NavigateUrl.Substring(0, NavigateUrl.IndexOf("&Message=Data"));



                Response.Redirect(NavigateUrl + message);
            }
            else // safe data 
            {
                string sdmdhpna = btnSave.OnClientClick.ToLower().Contains("sdm") ? "sdm" : btnSave.OnClientClick.ToLower().Contains("dh") ? "dh" : "pna";
                if (sdmdhpna == "sdm")
                {
                    string csv = hdnfldSDMDHPhase1Data.Value;
                    string[] array = csv.Split('&');
                    foreach (string item in array)
                    {

                        string[] innerarray = item.Split('|');
                        if (innerarray.Length == 4)
                        {
                            int id = Convert.ToInt32(innerarray[0]);
                            string sdmstatus = innerarray[1];
                            double appamount = 0;
                            double.TryParse(innerarray[2], out appamount);
                            string prioriry = innerarray[3];
                            service.UpdateSDMPhase1(id, prioriry, sdmstatus, appamount);

                        }

                    }
                }
                if (sdmdhpna == "dh")
                {
                    string csv = hdnfldDHPhase2Data.Value;
                    string[] array = csv.Split('&');
                    foreach (string item in array)
                    {

                        string[] innerarray = item.Split('|');
                        if (innerarray.Length == 4)
                        {
                            int id = Convert.ToInt32(innerarray[0]);
                            string sdmstatus = innerarray[1];
                            double appamount = 0;
                            double.TryParse(innerarray[2], out appamount);
                            string prioriry = innerarray[3];
                            service.UpdateDHPhase1(id, prioriry, sdmstatus, appamount);
                        }

                    }
                } if (sdmdhpna == "pna")
                {
                    string csv = hdnfldPNAPhase2Data.Value;
                    string[] array = csv.Split('&');
                    foreach (string item in array)
                    {

                        string[] innerarray = item.Split('|');
                        if (innerarray.Length == 3)
                        {
                            int id = Convert.ToInt32(innerarray[0]);
                            string sdmstatus = innerarray[1];
                            double appamount = 0;
                            double.TryParse(innerarray[2], out appamount);

                            service.UpdatePNAPhase2(id, sdmstatus, appamount);
                        }

                    }
                }



                string message = "&Message=Row(s) saved successfully !";
                string NavigateUrl = "SubConHome.aspx?" + Request.QueryString + "";

                if (NavigateUrl.Contains("&Message=Row(s)"))
                    NavigateUrl = NavigateUrl.Substring(0, NavigateUrl.IndexOf("Message=Row(s)"));


                Response.Redirect(NavigateUrl + message);
            }


        }

        protected void grdBEData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string expenseType = ddlExpenseType.Text;
            string qurt = ddlQuarter.Text;
            string mcc = ddlMCC.Text;
            string temp = "&pagerindex=" + e.NewPageIndex;
            string urlFormat = "SubConHome.aspx?mcc={0}&exptype={1}&quarter={2}";
            string NavigateUrl = string.Format(urlFormat,/* pucode, ducode,*/mcc, expenseType, qurt);

            if (NavigateUrl.Contains("&pagerindex="))
                NavigateUrl = NavigateUrl.Substring(0, NavigateUrl.IndexOf("&pagerindex="));

            if (NavigateUrl.Contains("&Message=Row(s)"))
                NavigateUrl = NavigateUrl.Substring(0, NavigateUrl.IndexOf("Message=Row(s)"));

            if (NavigateUrl.Contains("&Message=Data"))
                NavigateUrl = NavigateUrl.Substring(0, NavigateUrl.IndexOf("&Message=Data"));



            Response.Redirect(NavigateUrl + temp);


        }

        protected void btnAddNew_Click1(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "dpExcept", "dpExcept();", true);
        }

        protected void hypAddOtherExpenses_Click(object sender, EventArgs e)
        {

        }
    }
}