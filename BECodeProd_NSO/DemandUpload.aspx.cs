using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Microsoft.SqlServer.Dts.Runtime;
using System.Data.OleDb;
using Microsoft.SqlServer.Dts;
//using Microsoft.Office.Core;
using OfficeOpenXml;
using BEData.BusinessEntity;
using System.Diagnostics;
using BEData;
using System.Configuration;

using System.Text;







    public partial class DemandUpload : BasePage
    {
        Package pkg;
        DTSExecResult pkgresult;
        public DateTime dateTime = DateTime.Today;
        //App_Logger logger = new App_Logger();
        Logger logger = new Logger();
    
        private BEDL service = new BEDL();
        string fileName = "BEData.DemandUpload.cs";
        //Controller.ServiceController ctrlr = new Controller.ServiceController();
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            if (!Page.IsPostBack)
            {
                lblError.Visible = false;
                lblSuccess.Visible = false;
                string userid = HttpContext.Current.User.Identity.Name;
                // userid = "karthik_mahalingam01";
                string[] userids = userid.Split('\\');
                if (userids.Length == 2)
                {
                    userid = userids[1];
                }
                //userid = "Srinivas_Manjunath";
                Session["UserID"] = userid;


                DateTime todaydate = DateTime.Now;
                //string curyear = Convert.ToString(todaydate.Year - 2000);
                string curyear = Convert.ToString(todaydate.Year);
                string nextyear = Convert.ToString(todaydate.Year + 1);
                int curyr = Convert.ToInt32(todaydate.Year);
                //string yr=



                int tempyear = Convert.ToInt32(curyear.Substring(2, 2)) + 2000 - 1;
                string year = string.Format("{0}-{1}", tempyear, (tempyear - 2000 + 1));

                int tempyearnxt = Convert.ToInt32(nextyear.Substring(2, 2)) + 2000 - 1;
                string nxtyear = string.Format("{0}-{1}", tempyearnxt, (tempyearnxt - 2000 + 1));

                string yearmonth = string.Empty;

                string strquarter = string.Empty;
                string nextquarter = string.Empty;
                //int nextyear = year + 1;
                if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)
                {
                    strquarter = "Q4";
                    nextquarter = "Q1";
                }
                else if (todaydate.Month == 4 || todaydate.Month == 5 || todaydate.Month == 6)
                {
                    strquarter = "Q1";
                    nextquarter = "Q2";
                }
                else if (todaydate.Month == 7 || todaydate.Month == 8 || todaydate.Month == 9)
                {
                    strquarter = "Q2";
                    nextquarter = "Q3";
                }
                else
                {
                    strquarter = "Q3";
                    nextquarter = "Q4";
                }

                //drpQtr.Text = strquarter;
                //drpQtrProd.Text = strquarter;

                //drpYear.Items.Insert(0, year);
                //drpYear.Items.Insert(1, nxtyear);

                //drpYearProd.Items.Insert(0, year);
                //drpYearProd.Items.Insert(1, nxtyear);


                //drpSU.DataSource = service.FetchSUforFinpulse();
                //drpSU.DataTextField = "SU";
                //drpSU.DataValueField = "SU";
                //drpSU.DataBind();

                drpYer.DataSource = service.FetchFinpulseYear();
                drpYer.DataTextField = "Year";
                drpYer.DataValueField = "Year";
                drpYer.DataBind();
               
            }
        }

        Application app = new Application();

        public void LoadPackage(string ExcelPath, string DTSPath)
        {

            string pkgpath = "";
            string folder = "ExcelOperations";
            var MyDir = new DirectoryInfo(Server.MapPath(folder));

            int counterror = 0;
            string ExecStatus = "";
           
            pkg = app.LoadPackage(System.Configuration.ConfigurationManager.AppSettings["ImportData"] + DTSPath, null);

            for (int i = 0; i < pkg.Connections.Count; i++)
            {
                if (pkg.Connections[i].CreationName == "OLEDB")
                {
                    string temp = pkg.Connections[i].ConnectionString;
                    temp = temp + "Password=cmed@123;";
                    pkg.Connections[i].ConnectionString = temp;
                }
            }

            pkgresult = pkg.Execute();
            counterror = pkg.Errors.Count;
            ExecStatus = pkg.ExecutionStatus.ToString();

            if (pkgresult.ToString() == "Success")
            {
                lblSuccess.Text = "";
                lblSuccess.Text = "Data Uploaded Successfully";
                lblSuccess.Visible = true;
                lblError.Visible = false;
            }
            else
            {
                lblError.Text = "";
                lblError.Text = "Data Upload was NOT SUCCESSFULL";
                lblError.Visible = true;
                lblSuccess.Visible = false;
                for (int i = 0; i < pkg.Errors.Count; i++)
                {
                    lblError.Text = lblError.Text + i.ToString() + "." + pkg.Errors[i].Description + "~";
                }
            }


            if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "Cimba.xlsx") == null)
                System.IO.File.Delete(MyDir.FullName + "\\Cimba.xlsx");
            if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "EmployeeReport.xlsx") == null)
                System.IO.File.Delete(MyDir.FullName + "\\EmployeeReport.xlsx");
            if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "OMS.xlsx") == null)
                System.IO.File.Delete(MyDir.FullName + "\\OMS.xlsx");
        }


        #region Demand Portion Commented

        //protected void btnEmpUload_Click(object sender, EventArgs e)
        //{
        //    System.Data.DataTable dtExcel = new System.Data.DataTable();

        //    dtExcel.TableName = "MyExcelData";

        //    string folder = "ExcelOperations";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "EmployeeReport.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\EmployeeReport.xlsx");

        //    string path = MyDir.FullName + "\\EmployeeReport.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {


        //        string FileName = UlEmp.FileName;

        //        if (UlEmp.HasFile)
        //        {

        //            //System.Data.DataTable dtExcel = new System.Data.DataTable();

        //            //dtExcel.TableName = "MyExcelData";

        //            //string folder = "ExcelOperations";
        //            //var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //            //if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "EmployeeReport.xlsx") != null)
        //                //System.IO.File.Delete(MyDir.FullName + "\\EmployeeReport.xlsx");
        //            if (FileName.Contains(".xls"))
        //            {
        //                //string path = MyDir.FullName + "\\EmployeeReport.xlsx";// + FileName;
        //                //string filename = Path.GetFileName(FileUpload1.FileName);
        //                UlEmp.SaveAs(path);

        //                //string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //                //OleDbConnection con = new OleDbConnection(SourceConstr);

        //                string query = "Select * from [Sheet1$]";


        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("Sheet1$"))
        //                {

        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    bool isSuccess;

        //                    isSuccess = service.DeleteEmployeeDetailsDump();

        //                    //if (isSuccess)
        //                    //{

        //                        data.Fill(dtExcel);

        //                        int noOfRows = dtExcel.Rows.Count;
        //                        int rowsupdated = 0;

        //                        foreach (DataRow row in dtExcel.Rows)
        //                        {
        //                            Nullable<int> nullableInt = null;
        //                            Nullable<DateTime> nullableDate = null;
        //                            Nullable<double> nullableDouble = null;

        //                            Nullable<int> intEmpId = row["Emp No"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Emp No"]);
        //                            string txtEmpName = row["Emp Name"] == DBNull.Value ? "" : Convert.ToString(row["Emp Name"]);

        //                            string txtEmpMailId = row["Emp Mail ID"] == DBNull.Value ? "" : Convert.ToString(row["Emp Mail ID"]);
        //                            string txtEmpPU = row["Emp PU"] == DBNull.Value ? "" : Convert.ToString(row["Emp PU"]);
        //                            string txtEmpDU = row["Emp DU"] == DBNull.Value ? "" : Convert.ToString(row["Emp DU"]);
        //                            string txtEmpSubUnit = row["Emp Sub Unit"] == DBNull.Value ? "" : Convert.ToString(row["Emp Sub Unit"]);
        //                            string txtEmpUnit = row["Emp Unit"] == DBNull.Value ? "" : Convert.ToString(row["Emp Unit"]);
        //                            string txtBaseLocation = row["Emp Base Location"] == DBNull.Value ? "" : Convert.ToString(row["Emp Base Location"]);
        //                            Nullable<int> intDUDMId = row["DU Head for Emp DU"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["DU Head for Emp DU"]);
        //                            string txtDUDMName = row["DU Head Name for Emp DU"] == DBNull.Value ? "" : Convert.ToString(row["DU Head Name for Emp DU"]);
        //                            Nullable<int> intReportingToEmpId = row["Reporting To Emp No"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Reporting To Emp No"]);
        //                            string txtReportingToMailId = row["Reporting To Mail Id"] == DBNull.Value ? "" : Convert.ToString(row["Reporting To Mail Id"]);
        //                            string txtMasterProject = row["Master Project Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Project Code"]);
        //                            string txtRole = row["Role Capability"] == DBNull.Value ? "" : Convert.ToString(row["Role Capability"]);
        //                            Nullable<double> fltEmpTotalExp = row["Total Exp in Years"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Total Exp in Years"]);
        //                            string txtOnsiteOffshore = row["Onsite/Offshore"] == DBNull.Value ? "" : Convert.ToString(row["Onsite/Offshore"]);
        //                            string txtTechnologyCode = row["Tech Code"] == DBNull.Value ? "" : Convert.ToString(row["Tech Code"]);
        //                            Nullable<int> intJobBand = row["Job Band"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Job Band"]);
        //                            string txtEmpBaseCity = row["Emp Base City"] == DBNull.Value ? "" : Convert.ToString(row["Emp Base City"]);
        //                            Nullable<DateTime> dtJoiningDate = row["Joining Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Joining Date"]);
        //                            string txtEmpCurrentLoc = row["Emp Current Location"] == DBNull.Value ? "" : Convert.ToString(row["Emp Current Location"]);
        //                            string txtEmpCurrentCity = row["Emp Current City"] == DBNull.Value ? "" : Convert.ToString(row["Emp Current City"]);
        //                            string txtProjectCode = row["Project Code"] == DBNull.Value ? "" : Convert.ToString(row["Project Code"]);
        //                            string txtCustomerCode = row["Customer Code"] == DBNull.Value ? "" : Convert.ToString(row["Customer Code"]);
        //                            Nullable<DateTime> dtFromDate = row["From Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["From Date"]);
        //                            Nullable<DateTime> dtToDate = row["To Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["To Date"]);
        //                            string txtAllocatedCountry = row["Allocated Country"] == DBNull.Value ? "" : Convert.ToString(row["Allocated Country"]);
        //                            string txtAllocatedState = row["Allocated State"] == DBNull.Value ? "" : Convert.ToString(row["Allocated State"]);
        //                            string txtAllocatedCity = row["Allocated City"] == DBNull.Value ? "" : Convert.ToString(row["Allocated City"]);
        //                            string txtProjectType = row["Project Type"] == DBNull.Value ? "" : Convert.ToString(row["Project Type"]);
        //                            string txtBillingType = row["Billing Type"] == DBNull.Value ? "" : Convert.ToString(row["Billing Type"]);
        //                            string txtReportingToEmpName = row["Reporting To Emp Name"] == DBNull.Value ? "" : Convert.ToString(row["Reporting To Emp Name"]);
        //                            string txtProjRemarks = row["Proj Remarks"] == DBNull.Value ? "" : Convert.ToString(row["Proj Remarks"]);
        //                            string txtUnitCode = row["Unit Code"] == DBNull.Value ? "" : Convert.ToString(row["Unit Code"]);
        //                            string txtEmpExpInfosys = row["Emp Exp In Infosys "] == DBNull.Value ? "" : Convert.ToString(row["Emp Exp In Infosys "]);
        //                            string txtEmpPrevExp = row["Emp Prev Exp "] == DBNull.Value ? "" : Convert.ToString(row["Emp Prev Exp "]);
        //                            string txtEmpTotExpMonths = row["Emp Total Exp"] == DBNull.Value ? "" : Convert.ToString(row["Emp Total Exp"]);
        //                            string txtApplCode = row["Appl Code"] == DBNull.Value ? "" : Convert.ToString(row["Appl Code"]);
        //                            string txtServiceCode = row["Service Code"] == DBNull.Value ? "" : Convert.ToString(row["Service Code"]);
        //                            string txtActivityCode = row["Activity Code"] == DBNull.Value ? "" : Convert.ToString(row["Activity Code"]);
        //                            string txtTechCategory = row["Tech Category"] == DBNull.Value ? "" : Convert.ToString(row["Tech Category"]);
        //                            string txtMarketingBranchCode = row["Marketing Branch Code"] == DBNull.Value ? "" : Convert.ToString(row["Marketing Branch Code"]);
        //                            string txtProjDUCode = row["Proj DU Code"] == DBNull.Value ? "" : Convert.ToString(row["Proj DU Code"]);
        //                            string txtProjDevCentreCode = row["Proj DevCentre Code"] == DBNull.Value ? "" : Convert.ToString(row["Proj DevCentre Code"]);
        //                            string txtHRRemarks = row["HR Remarks"] == DBNull.Value ? "" : Convert.ToString(row["HR Remarks"]);
        //                            Nullable<int> intPersonalBand = row["Personal Band"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Personal Band"]);
        //                            string txtCustPortfolio = row["Customer Portfolio"] == DBNull.Value ? "" : Convert.ToString(row["Customer Portfolio"]);
        //                            string txtPartTimeEmp = row["PartTimeEmployee"] == DBNull.Value ? "" : Convert.ToString(row["PartTimeEmployee"]);
        //                            string txtProgramCode = row["ProgramCode"] == DBNull.Value ? "" : Convert.ToString(row["ProgramCode"]);
        //                            string txtTrackCode = row["TrackCode"] == DBNull.Value ? "" : Convert.ToString(row["TrackCode"]);
        //                            string txtEmpCompany = row["EmployeeCompany"] == DBNull.Value ? "" : Convert.ToString(row["EmployeeCompany"]);
        //                            string txtEmpBU = row["Employee Budgeting Unit"] == DBNull.Value ? "" : Convert.ToString(row["Employee Budgeting Unit"]);
        //                            string txtProjBU = row["Project Budgeting Unit"] == DBNull.Value ? "" : Convert.ToString(row["Project Budgeting Unit"]);
        //                            string txtProjCompany = row["Project Company"] == DBNull.Value ? "" : Convert.ToString(row["Project Company"]);
        //                            string txtProjSourceComp = row["Project Source Company"] == DBNull.Value ? "" : Convert.ToString(row["Project Source Company"]);
        //                            string txtEmpAttribute = row["Employee Attribute"] == DBNull.Value ? "" : Convert.ToString(row["Employee Attribute"]);
        //                            Nullable<DateTime> dtEmpDOB = row["Emp Date Of Birth"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Emp Date Of Birth"]);
        //                            string txtEmpGender = row["Emp Gender"] == DBNull.Value ? "" : Convert.ToString(row["Emp Gender"]);
        //                            Nullable<int> intProjDUDMId = row["Project DM "] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Project DM "]);
        //                            string txtProjDUDMName = row["Project DM Name"] == DBNull.Value ? "" : Convert.ToString(row["Project DM Name"]);
        //                            Nullable<int> intProjectDUhead = row["Project DU Head"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Project DU Head"]);
        //                            string txtProjectDUheadName = row["Project DUHead Name"] == DBNull.Value ? "" : Convert.ToString(row["Project DUHead Name"]);
        //                            Nullable<int> intProjectSDM = row["Project SDM"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Project SDM"]);
        //                            string txtProjectSDMmailId = row["Project SDM MailId"] == DBNull.Value ? "" : Convert.ToString(row["Project SDM MailId"]);
        //                            string txtGroupMasterProjectCode = row["Group Master Project Code"] == DBNull.Value ? "" : Convert.ToString(row["Group Master Project Code"]);

        //                            service.InsertEmployeeDetailsDump(intEmpId, txtEmpName, txtEmpMailId, txtEmpPU, txtEmpDU,
        //                            txtEmpSubUnit, txtEmpUnit, txtBaseLocation, intDUDMId, txtDUDMName, intReportingToEmpId,
        //                            txtReportingToMailId, txtMasterProject, txtRole, fltEmpTotalExp, txtOnsiteOffshore,
        //                            txtTechnologyCode, intJobBand, txtEmpBaseCity, dtJoiningDate, txtEmpCurrentLoc,
        //                            txtEmpCurrentCity, txtProjectCode, txtCustomerCode, dtFromDate, dtToDate, txtAllocatedCountry,
        //                            txtAllocatedState, txtAllocatedCity, txtProjectType, txtBillingType, txtReportingToEmpName,
        //                            txtProjRemarks, txtUnitCode, txtEmpExpInfosys, txtEmpPrevExp, txtEmpTotExpMonths,
        //                            txtApplCode, txtServiceCode, txtActivityCode, txtTechCategory, txtMarketingBranchCode,
        //                            txtProjDUCode, txtProjDevCentreCode, txtHRRemarks, intPersonalBand, txtCustPortfolio,
        //                            txtPartTimeEmp, txtProgramCode, txtTrackCode, txtEmpCompany, txtEmpBU, txtProjBU, txtProjCompany,
        //                            txtProjSourceComp, txtEmpAttribute, dtEmpDOB, txtEmpGender, intProjDUDMId, txtProjDUDMName,
        //                            intProjectDUhead, txtProjectDUheadName, intProjectSDM, txtProjectSDMmailId, txtGroupMasterProjectCode);
        //                            rowsupdated++;
        //                            //}
        //                        }

        //                        Session["FileName"] = path;
        //                        string ExcelFilePath = Session["FileName"].ToString();
        //                        if (con.State.ToString().ToLower() == "open")
        //                            con.Close();
        //                        lblSuccess.Text = "Data Uploaded Successfully";
        //                        lblSuccess.Visible = true;
        //                        lblError.Visible = false;
        //                    //}
        //                }
        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename sheet to 'Sheet1'";
        //                    lblError.Visible = true;
        //                }
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}

        //protected void btnProjUload_Click(object sender, EventArgs e)
        //{

        //    System.Data.DataTable dtExcel = new System.Data.DataTable();

        //    dtExcel.TableName = "MyExcelData";

        //    string folder = "ExcelOperations";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "ProjectCode.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\ProjectCode.xlsx");

        //    string path = MyDir.FullName + "\\ProjectCode.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {
        //        string FileName = UlProj.FileName;

        //        if (UlProj.HasFile)
        //        {
        //            //System.Data.DataTable dtExcel = new System.Data.DataTable();

        //            //dtExcel.TableName = "MyExcelData";

        //            //string folder = "ExcelOperations";
        //            //var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //            //if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "ProjectCode.xlsx") != null)
        //                //System.IO.File.Delete(MyDir.FullName + "\\ProjectCode.xlsx");
        //            if (FileName.Contains(".xls"))
        //            {
        //                //string path = MyDir.FullName + "\\ProjectCode.xlsx";// + FileName;
        //                //string filename = Path.GetFileName(FileUpload1.FileName);
        //                UlProj.SaveAs(path);

        //                //string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //                //OleDbConnection con = new OleDbConnection(SourceConstr);

        //                string query = "Select * from [Sheet1$]";

        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("Sheet1$"))
        //                {


        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    bool isSuccess;

        //                    isSuccess = service.DeleteProjectDetailsDump();

        //                    //if (isSuccess)
        //                    //{

        //                        data.Fill(dtExcel);

        //                        int noOfRows = dtExcel.Rows.Count;
        //                        int rowsupdated = 0;

        //                        foreach (DataRow row in dtExcel.Rows)
        //                        {
        //                            Nullable<int> nullableInt = null;
        //                            Nullable<DateTime> nullableDate = null;

        //                            //string txtClientSubUnit = row[0] == DBNull.Value ? "" : Convert.ToString(row[0]);
        //                            //string txtSubUnit = row[1] == DBNull.Value ? "" : Convert.ToString(row[1]);
        //                            //string txtPU = row[2] == DBNull.Value ? "" : Convert.ToString(row[2]);
        //                            //string txtCustomerCode = row[3] == DBNull.Value ? "" : Convert.ToString(row[3]);
        //                            //Nullable<int> intLOENumber = row[4] == DBNull.Value ? nullableInt : Convert.ToInt32(row[4]);
        //                            //string txtMasterProjectCode = row[5] == DBNull.Value ? "" : Convert.ToString(row[5]);
        //                            //string txtChildProjectCode = row[6] == DBNull.Value ? "" : Convert.ToString(row[6]);
        //                            //string txtDescription = row[7] == DBNull.Value ? "" : Convert.ToString(row[7]);
        //                            //Nullable<DateTime> dtStartDate = row[8] == DBNull.Value ? nullableDate : Convert.ToDateTime(row[8]);
        //                            //Nullable<DateTime> dtEndDate = row[9] == DBNull.Value ? nullableDate : Convert.ToDateTime(row[9]);
        //                            //string txtProjectType = row[10] == DBNull.Value ? "" : Convert.ToString(row[10]);
        //                            //Nullable<DateTime> dtProjectCreatedOn = row[11] == DBNull.Value ? nullableDate : Convert.ToDateTime(row[11]);
        //                            //string txtDMMailID = row[12] == DBNull.Value ? "" : Convert.ToString(row[12]);
        //                            //string txtPMMailID = row[13] == DBNull.Value ? "" : Convert.ToString(row[13]);
        //                            //string txtDevelopmentCenter = row[14] == DBNull.Value ? "" : Convert.ToString(row[14]);
        //                            //string txtBU = row[15] == DBNull.Value ? "" : Convert.ToString(row[15]);
        //                            //string txtDU = row[16] == DBNull.Value ? "" : Convert.ToString(row[16]);
        //                            //string txtCreditSubUnit = row[17] == DBNull.Value ? "" : Convert.ToString(row[17]);
        //                            //string txtCountryCode = row[18] == DBNull.Value ? "" : Convert.ToString(row[18]);
        //                            //string txtProjectStateCode = row[19] == DBNull.Value ? "" : Convert.ToString(row[19]);
        //                            //Nullable<int> intNetworkNumber = row[20] == DBNull.Value ? nullableInt : Convert.ToInt32(row[20]);
        //                            //string txtTechnology = row[21] == DBNull.Value ? "" : Convert.ToString(row[21]);
        //                            //string txtServiceOffering = row[22] == DBNull.Value ? "" : Convert.ToString(row[22]);
        //                            //string txtSecondaryServiceCode = row[23] == DBNull.Value ? "" : Convert.ToString(row[23]);
        //                            //string txtCustomerServiceOffering = row[24] == DBNull.Value ? "" : Convert.ToString(row[24]);
        //                            //string txtCreatedBy = row[25] == DBNull.Value ? "" : Convert.ToString(row[25]);
        //                            //string txtReportingPU = row[26] == DBNull.Value ? "" : Convert.ToString(row[26]);
        //                            //string txtMappedProjectCode = row[27] == DBNull.Value ? "" : Convert.ToString(row[27]);
        //                            //string txtCreatedInSAP = row[28] == DBNull.Value ? "" : Convert.ToString(row[28]);
        //                            //string txtContractType = row[29] == DBNull.Value ? "" : Convert.ToString(row[29]);
        //                            //string txtTimeBasedBilling = row[30] == DBNull.Value ? "" : Convert.ToString(row[30]);
        //                            //string txtGroupMasterProjCode = row[31] == DBNull.Value ? "" : Convert.ToString(row[31]);

        //                            string txtClientSubUnit = row["ClientSubUnit"] == DBNull.Value ? "" : Convert.ToString(row["ClientSubUnit"]);
        //                            string txtSubUnit = row["SubUnit"] == DBNull.Value ? "" : Convert.ToString(row["SubUnit"]);
        //                            string txtPU = row["PU"] == DBNull.Value ? "" : Convert.ToString(row["PU"]);
        //                            string txtCustomerCode = row["CustomerCode"] == DBNull.Value ? "" : Convert.ToString(row["CustomerCode"]);
        //                            Nullable<int> intLOENumber = row["LOENumber"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["LOENumber"]);
        //                            string txtMasterProjectCode = row["MasterProjectCode"] == DBNull.Value ? "" : Convert.ToString(row["MasterProjectCode"]);
        //                            string txtChildProjectCode = row["ChildProjectCode"] == DBNull.Value ? "" : Convert.ToString(row["ChildProjectCode"]);
        //                            string txtDescription = row["Description"] == DBNull.Value ? "" : Convert.ToString(row["Description"]);
        //                            Nullable<DateTime> dtStartDate = row["StartDate"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["StartDate"]);
        //                            Nullable<DateTime> dtEndDate = row["EndDate"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["EndDate"]);
        //                            string txtProjectType = row["ProjectType"] == DBNull.Value ? "" : Convert.ToString(row["ProjectType"]);
        //                            Nullable<DateTime> dtProjectCreatedOn = row["ProjectCreatedOn"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["ProjectCreatedOn"]);
        //                            string txtDMMailID = row["DeliveryManagerMailID"] == DBNull.Value ? "" : Convert.ToString(row["DeliveryManagerMailID"]);
        //                            string txtPMMailID = row["ProjectManagerMailID"] == DBNull.Value ? "" : Convert.ToString(row["ProjectManagerMailID"]);
        //                            string txtDevelopmentCenter = row["DevelopmentCenter"] == DBNull.Value ? "" : Convert.ToString(row["DevelopmentCenter"]);
        //                            string txtBU = row["BU"] == DBNull.Value ? "" : Convert.ToString(row["BU"]);
        //                            string txtDU = row["DU"] == DBNull.Value ? "" : Convert.ToString(row["DU"]);
        //                            string txtCreditSubUnit = row["CreditSubUnit"] == DBNull.Value ? "" : Convert.ToString(row["CreditSubUnit"]);
        //                            string txtCountryCode = row["CountryCode"] == DBNull.Value ? "" : Convert.ToString(row["CountryCode"]);
        //                            string txtProjectStateCode = row["ProjectStateCode"] == DBNull.Value ? "" : Convert.ToString(row["ProjectStateCode"]);
        //                            Nullable<int> intNetworkNumber = row["NetworkNumber"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["NetworkNumber"]);
        //                            string txtTechnology = row["Technology"] == DBNull.Value ? "" : Convert.ToString(row["Technology"]);
        //                            string txtServiceOffering = row["ServiceOffering"] == DBNull.Value ? "" : Convert.ToString(row["ServiceOffering"]);
        //                            string txtSecondaryServiceCode = row["SecondaryServiceCode"] == DBNull.Value ? "" : Convert.ToString(row["SecondaryServiceCode"]);
        //                            string txtCustomerServiceOffering = row["CustomerServiceOffering"] == DBNull.Value ? "" : Convert.ToString(row["CustomerServiceOffering"]);
        //                            string txtCreatedBy = row["CreatedBy"] == DBNull.Value ? "" : Convert.ToString(row["CreatedBy"]);
        //                            string txtReportingPU = row["ReportingPU"] == DBNull.Value ? "" : Convert.ToString(row["ReportingPU"]);
        //                            string txtMappedProjectCode = row["MappedProjectCode"] == DBNull.Value ? "" : Convert.ToString(row["MappedProjectCode"]);
        //                            string txtCreatedInSAP = row["CreatedInSAP"] == DBNull.Value ? "" : Convert.ToString(row["CreatedInSAP"]);
        //                            string txtContractType = row["ContractType"] == DBNull.Value ? "" : Convert.ToString(row["ContractType"]);
        //                            string txtTimeBasedBilling = row["TimeBasedBilling"] == DBNull.Value ? "" : Convert.ToString(row["TimeBasedBilling"]);
        //                            string txtGroupMasterProjCode = row["GroupMasterProjectCode"] == DBNull.Value ? "" : Convert.ToString(row["GroupMasterProjectCode"]);


        //                            service.InsertProjectDetailsDump(txtClientSubUnit, txtSubUnit, txtPU, txtCustomerCode,
        //                            intLOENumber, txtMasterProjectCode, txtChildProjectCode, txtDescription,
        //                            dtStartDate, dtEndDate, txtProjectType, dtProjectCreatedOn, txtDMMailID, txtPMMailID,
        //                            txtDevelopmentCenter, txtBU, txtDU, txtCreditSubUnit, txtCountryCode, txtProjectStateCode,
        //                            intNetworkNumber, txtTechnology, txtServiceOffering, txtSecondaryServiceCode,
        //                            txtCustomerServiceOffering, txtCreatedBy, txtReportingPU, txtMappedProjectCode,
        //                            txtCreatedInSAP, txtContractType, txtTimeBasedBilling, txtGroupMasterProjCode);
        //                            rowsupdated++;
        //                            //}
        //                        }

        //                        Session["FileName"] = path;
        //                        string ExcelFilePath = Session["FileName"].ToString();
        //                        if (con.State.ToString().ToLower() == "open")
        //                            con.Close();
        //                        lblSuccess.Text = "Data Uploaded Successfully";
        //                        lblSuccess.Visible = true;
        //                        lblError.Visible = false;
        //                    //}
        //                }
        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename the sheet to 'Sheet1'";
        //                    lblError.Visible = true;
        //                }
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";
        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }

        //    }
        //}

        //protected void lnkExportExcel_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/SampleXLSX/Cimba.xlsx");
        //}

        //protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/SampleXLSX/EmployeeReport.xlsx");
        //}

        //protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/SampleXLSX/OMS.xlsx");
        //}

        #endregion

        private void DownloadFile(string FileName)
        {
            try
            {
                bool forceDownload = true;

                string folder = "ExcelOperations";
                var MyDir = new DirectoryInfo(Server.MapPath(folder));
                string path = MyDir.FullName + "\\" + FileName;
                string name = FileName + ".xlsx";
                string ext = Path.GetExtension(path);
                string type = "";


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
                            type = "application/vnd.ms-excel";
                            break;
                        case ".xlsx":
                            type = "application/vnd.ms-excel.12";
                            break;
                    }
                }

                if (forceDownload)
                {
                    Response.AppendHeader("content-disposition",
                        "attachment; filename=" + name);
                }
                if (type != "")
                    Response.ContentType = type;
                Response.WriteFile(path);

                Response.Flush();
                Response.End();

                if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "OpportunityDump.xlsx") == null)
                    System.IO.File.Delete(MyDir.FullName + "\\OpportunityDump.xlsx");
                if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "EmployeeDump.xlsx") == null)
                    System.IO.File.Delete(MyDir.FullName + "\\EmployeeDump.xlsx");
                if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "ProjectDump.xlsx") == null)
                    System.IO.File.Delete(MyDir.FullName + "\\ProjectDump.xlsx");
                if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "FinpulseDump.xlsx") == null)
                    System.IO.File.Delete(MyDir.FullName + "\\FinpulseDump.xlsx");
                if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "RTBRDump.xlsx") == null)
                    System.IO.File.Delete(MyDir.FullName + "\\RTBRDump.xlsx");
                if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "BEMCODump.xlsx") == null)
                    System.IO.File.Delete(MyDir.FullName + "\\BEMCODump.xlsx");

            }
            catch (Exception ex)
            {
                if ((ex.Message + "").Contains("Thread was being aborted."))
                    logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                else
                {
                    logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }
            }
        }


        //private void DownloadFileProd(string FileName)
        //{
        //    try
        //    {
        //        bool forceDownload = true;

        //        //string folder = "ExcelOperations";
        //        string folder = "ExcelOperationsProd";

        //        var MyDir = new DirectoryInfo(Server.MapPath(folder));
        //        string path = MyDir.FullName + "\\" + FileName;
        //        string name = FileName + ".xlsx";
        //        string ext = Path.GetExtension(path);
        //        string type = "";


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
        //                    type = "application/vnd.ms-excel";
        //                    break;
        //                case ".xlsx":
        //                    type = "application/vnd.ms-excel.12";
        //                    break;
        //            }
        //        }

        //        if (forceDownload)
        //        {
        //            Response.AppendHeader("content-disposition",
        //                "attachment; filename=" + name);
        //        }
        //        if (type != "")
        //            Response.ContentType = type;
        //        Response.WriteFile(path);

        //        Response.Flush();
        //        Response.End();

        //        //if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "OpportunityDump.xlsx") == null)
        //        //    System.IO.File.Delete(MyDir.FullName + "\\OpportunityDump.xlsx");
        //        //if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "EmployeeDump.xlsx") == null)
        //        //    System.IO.File.Delete(MyDir.FullName + "\\EmployeeDump.xlsx");
        //        //if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "ProjectDump.xlsx") == null)
        //        //    System.IO.File.Delete(MyDir.FullName + "\\ProjectDump.xlsx");

        //    }
        //    catch (Exception ex)
        //    {
        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}


        #region Demand Portion

        //protected void lnkDownloadBkUp_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        DataAccess dataAccess = new DataAccess();
        //        DataTable dt = new DataTable();
        //        dt = dataAccess.GetOppBkUp();

        //        System.Web.UI.WebControls.DataGrid grid = new System.Web.UI.WebControls.DataGrid();
        //        grid.HeaderStyle.Font.Bold = true;
        //        grid.DataSource = dt;

        //        grid.DataBind();


        //        string Filename = "OpportunityDump.xlsx";

        //        string folder = "ExcelOperations";
        //        var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //        if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "OpportunityDump.xlsx") == null)
        //            System.IO.File.Delete(MyDir.FullName + "\\OpportunityDump.xlsx");



        //        FileInfo file = new FileInfo(MyDir.FullName + "\\OpportunityDump.xlsx");
        //        using (ExcelPackage pck = new ExcelPackage(file))
        //        {
        //            //Create the worksheet
        //            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("OpportunityDump");

        //            //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
        //            ws.Cells["A1"].LoadFromDataTable(dt, true);
        //            pck.Save();
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("content-disposition", "attachment;  filename=OpportunityDump.xlsx");
        //            //Response.BinaryWrite(pck.GetAsByteArray());
        //        }


        //        DownloadFile(Filename);

        //    }
        //    catch (Exception ex)
        //    {

        //        //if ((ex.Message + "").Contains("Thread was being aborted."))
        //        //    logger.LogErrorToServer(Controller.App_Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        //else
        //        //{
        //        //    logger.LogErrorToServer(Controller.App_Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        //    throw ex;
        //        //}
        //    }
        //}

        //protected void ImageButton4_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        DataAccess dataAccess = new DataAccess();
        //        DataTable dt = new DataTable();
        //        dt = dataAccess.GetEmployeeBkUp();

        //        System.Web.UI.WebControls.DataGrid grid = new System.Web.UI.WebControls.DataGrid();
        //        grid.HeaderStyle.Font.Bold = true;
        //        grid.DataSource = dt;
        //        grid.DataBind();

        //        string Filename = "EmployeeDump.xlsx";

        //        //string folder = "ExcelOperations";
        //        string folder = "ExcelOperations";

        //        var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //        if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "EmployeeDump.xlsx") == null)
        //            System.IO.File.Delete(MyDir.FullName + "\\EmployeeDump.xlsx");

        //        FileInfo file = new FileInfo(MyDir.FullName + "\\EmployeeDump.xlsx");
        //        using (ExcelPackage pck = new ExcelPackage(file))
        //        {
        //            //Create the worksheet
        //            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");

        //            //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
        //            ws.Cells["A1"].LoadFromDataTable(dt, true);
        //            pck.Save();
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("content-disposition", "attachment;  filename=EmployeeDump.xlsx");
        //            //Response.BinaryWrite(pck.GetAsByteArray());
        //        }

        //        DownloadFile(Filename);
        //    }
        //    catch (Exception ex)
        //    {

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}

        //protected void ImageButton5_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        DataAccess dataAccess = new DataAccess();
        //        DataTable dt = new DataTable();
        //        dt = dataAccess.GetProjectBkUp();

        //        System.Web.UI.WebControls.DataGrid grid = new System.Web.UI.WebControls.DataGrid();
        //        grid.HeaderStyle.Font.Bold = true;
        //        grid.DataSource = dt;
        //        grid.DataBind();

        //        string Filename = "ProjectDump.xlsx";

        //        //string folder = "ExcelOperations";
        //        string folder = "ExcelOperations";

        //        var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //        if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "ProjectDump.xlsx") == null)
        //            System.IO.File.Delete(MyDir.FullName + "\\ProjectDump.xlsx");

        //        FileInfo file = new FileInfo(MyDir.FullName + "\\ProjectDump.xlsx");
        //        using (ExcelPackage pck = new ExcelPackage(file))
        //        {
        //            //Create the worksheet
        //            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");

        //            //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
        //            ws.Cells["A1"].LoadFromDataTable(dt, true);
        //            pck.Save();
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("content-disposition", "attachment;  filename=ProjectDump.xlsx");
        //            //Response.BinaryWrite(pck.GetAsByteArray());
        //        }

        //        DownloadFile(Filename);
        //    }
        //    catch (Exception ex)
        //    {

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}

        //protected void btnOppUload_Click(object sender, EventArgs e)
        //{


        //    System.Data.DataTable dtExcel = new System.Data.DataTable();

        //    dtExcel.TableName = "MyExcelData";

        //    string folder = "ExcelOperations";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "Cimba.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\Cimba.xlsx");

        //    string path = MyDir.FullName + "\\Cimba.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {

        //        string FileName = UlOpp.FileName;

        //        if (UlOpp.HasFile)
        //        {

        //            //string filename = Path.GetFileName(FileUpload1.FileName);
        //            UlOpp.SaveAs(path);


        //            if (FileName.Contains(".xls"))
        //            {

        //                string query = "Select * from [Sheet1$]";

        //                //StringBuilder query = new StringBuilder();
        //                //query.Append("SELECT * ").AppendFormat(" FROM [{0}$]", "FINAL");


        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("Sheet1$"))
        //                {

        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    bool isSuccess;

        //                    isSuccess = service.DeleteOpportunityDump();

        //                    //if (isSuccess)
        //                    //{

        //                        data.Fill(dtExcel);

        //                        int noOfRows = dtExcel.Rows.Count;
        //                        int rowsupdated = 0;

        //                        foreach (DataRow row in dtExcel.Rows)
        //                        {

        //                            Nullable<int> nullableInt = null;
        //                            Nullable<DateTime> nullableDate = null;
        //                            Nullable<double> nullableDouble = null;



        //                            //Nullable<int> intCRMOppId = row["SAP Opportunity ID"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["SAP Opportunity ID"]);
        //                            //Nullable<int> intOppId = row["CIMBA Opportunity ID"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["CIMBA Opportunity ID"]);
        //                            //string txtOppName = row["Opportunity Name"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Name"]);
        //                            //Nullable<int> intParentOppID = row["Parent Opportunity ID"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Parent Opportunity ID"]);
        //                            //string txtCompanyName = row["Child Account Name"] == DBNull.Value ? "" : Convert.ToString(row["Child Account Name"]);
        //                            //string txtAccCode = row["Child Account Code"] == DBNull.Value ? "" : Convert.ToString(row["Child Account Code"]);
        //                            //string txtMCCode = row["Master Customer Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Customer Code"]);
        //                            //string txtPrimaryMember = row["Primary Owner Name"] == DBNull.Value ? "" : Convert.ToString(row["Primary Owner Name"]);
        //                            //string txtSalesRegion = row["Child Sales Region Code"] == DBNull.Value ? "" : Convert.ToString(row["Child Sales Region Code"]);
        //                            //string txtOppOwner = row["Opportunity Owner Name"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Owner Name"]);
        //                            //string txtOppStage = row["Opportunity Stage"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Stage"]);
        //                            //string txtOppStatus = row["Opportunity Status"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Status"]);
        //                            //string txtNotes = row["Opportunity Comments"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Comments"]);
        //                            //Nullable<double> fltTotalEstimate = row["Opportunity Total Estimates"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Opportunity Total Estimates"]);
        //                            //string txtNativeCurrency = row["Opportunity Currency Code"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Currency Code"]);
        //                            //Nullable<double> fltTotalEstimateInKUSD = row["Opportunity Total Estimates In KUSD"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Opportunity Total Estimates In KUSD"]);
        //                            //Nullable<int> intProbability = row["Opportunity Probability"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Opportunity Probability"]);
        //                            //Nullable<DateTime> dtOppCreated = row["Created Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Created Date"]);
        //                            //Nullable<DateTime> dtLikelyStartDate = row["Opportunity Likely Start Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Opportunity Likely Start Date"]);
        //                            //Nullable<DateTime> dtLikelyEndDate = row["Opportunity Likely End Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Opportunity Likely End Date"]);
        //                            //string txtTransformational = row["Program Transformational?"] == DBNull.Value ? "" : Convert.ToString(row["Program Transformational?"]);
        //                            //string txtSGStagged = row["SGS Tagged(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["SGS Tagged(Y/N)"]);
        //                            //string txtItrac = row["Itrac(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["Itrac(Y/N)"]);
        //                            //string txtProposalSubmit = row["ProposalSubmit(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["ProposalSubmit(Y/N)"]);
        //                            //string txtProposalNo = row["Proposal ID"] == DBNull.Value ? "" : Convert.ToString(row["Proposal ID"]);
        //                            //string txtFlgTopOpp = row["Top Opportunity?"] == DBNull.Value ? "" : Convert.ToString(row["Top Opportunity?"]);
        //                            //string txtPU = row["Opportunity Primary PU Code"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Primary PU Code"]);
        //                            //string txtDummy = row["Dummy Opportunity?"] == DBNull.Value ? "" : Convert.ToString(row["Dummy Opportunity?"]);
        //                            //string txtStale = row["Is Opportunity Stale?"] == DBNull.Value ? "" : Convert.ToString(row["Is Opportunity Stale?"]);
        //                            //Nullable<DateTime> dtLastModifiedDate = row["Last Modified Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Last Modified Date"]);
        //                            //string txtProposalStatus = row["Stage Description"] == DBNull.Value ? "" : Convert.ToString(row["Stage Description"]);
        //                            //Nullable<double> fltProposalValue = row["Total in USD as on Proposal Date"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Total in USD as on Proposal Date"]);
        //                            //string txtPropAnchor = row["Proposal Anchor Name"] == DBNull.Value ? "" : Convert.ToString(row["Proposal Anchor Name"]);
        //                            //Nullable<DateTime> dtProposalCreatedDate = row["ProposalCreatedDate"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["ProposalCreatedDate"]);
        //                            //string txtSelling = row["Opportunity Classification"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Classification"]);
        //                            //string txtSellingInvolved = row["Selling Involved"] == DBNull.Value ? "" : Convert.ToString(row["Selling Involved"]);
        //                            //string txtContractType = row["Contract Code"] == DBNull.Value ? "" : Convert.ToString(row["Contract Code"]);
        //                            //Nullable<DateTime> dtOppClosedOn = row["Opportunity Closed On"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Opportunity Closed On"]);
        //                            //Nullable<DateTime> dtProposalSubmissionDate = row["Proposal Submitted On"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Proposal Submitted On"]);
        //                            //string txtCountry = row["Country Name"] == DBNull.Value ? "" : Convert.ToString(row["Country Name"]);
        //                            //string txtSolutionName = row["Opportunity Solution Name"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Solution Name"]);
        //                            //string txtAllianceName = row["Opportunity Alliance Name"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Alliance Name"]);
        //                            //string txtDPSLinked = row["DPS Linked?"] == DBNull.Value ? "" : Convert.ToString(row["DPS Linked?"]);



        //                            Nullable<int> intCRMOppId = row["CRMOppId"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["CRMOppId"]);
        //                            Nullable<int> intOppId = row["OppId"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["OppId"]);
        //                            string txtOppName = row["OppName"] == DBNull.Value ? "" : Convert.ToString(row["OppName"]);
        //                            Nullable<int> intParentOppID = row["ParentOppID"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["ParentOppID"]);
        //                            string txtCompanyName = row["CompanyName"] == DBNull.Value ? "" : Convert.ToString(row["CompanyName"]);
        //                            string txtAccCode = row["Acc Code"] == DBNull.Value ? "" : Convert.ToString(row["Acc Code"]);
        //                            string txtMCCode = row["MC Code"] == DBNull.Value ? "" : Convert.ToString(row["MC Code"]);
        //                            string txtPrimaryMember = row["PrimaryMember"] == DBNull.Value ? "" : Convert.ToString(row["PrimaryMember"]);
        //                            string txtSalesRegion = row["Sales Region"] == DBNull.Value ? "" : Convert.ToString(row["Sales Region"]);
        //                            string txtOppOwner = row["OppOwner"] == DBNull.Value ? "" : Convert.ToString(row["OppOwner"]);
        //                            string txtOppStage = row["OppStage"] == DBNull.Value ? "" : Convert.ToString(row["OppStage"]);
        //                            string txtOppStatus = row["OppStatus"] == DBNull.Value ? "" : Convert.ToString(row["OppStatus"]);
        //                            string txtNotes = row["Notes"] == DBNull.Value ? "" : Convert.ToString(row["Notes"]);
        //                            Nullable<double> fltTotalEstimate = row["Total Est('000')"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Total Est('000')"]);
        //                            string txtNativeCurrency = row["NativeCurrency"] == DBNull.Value ? "" : Convert.ToString(row["NativeCurrency"]);
        //                            Nullable<double> fltTotalEstimateInKUSD = row["Total Est ('000')inUSD"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Total Est ('000')inUSD"]);
        //                            Nullable<int> intProbability = row["Probability"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Probability"]);
        //                            Nullable<DateTime> dtOppCreated = row["OppCreated"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["OppCreated"]);
        //                            Nullable<DateTime> dtLikelyStartDate = row["LikelyStartDate"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["LikelyStartDate"]);
        //                            Nullable<DateTime> dtLikelyEndDate = row["LikelyEndDate"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["LikelyEndDate"]);
        //                            string txtTransformational = row["Transformational(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["Transformational(Y/N)"]);
        //                            string txtSGStagged = row["SGS tagged(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["SGS tagged(Y/N)"]);
        //                            string txtItrac = row["Itrac(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["Itrac(Y/N)"]);
        //                            string txtProposalSubmit = row["ProposalSubmit(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["ProposalSubmit(Y/N)"]);
        //                            string txtProposalNo = row["ProposalNo"] == DBNull.Value ? "" : Convert.ToString(row["ProposalNo"]);
        //                            string txtFlgTopOpp = row["FlgTopOpp"] == DBNull.Value ? "" : Convert.ToString(row["FlgTopOpp"]);
        //                            string txtPU = row["PU"] == DBNull.Value ? "" : Convert.ToString(row["PU"]);
        //                            string txtDummy = row["Dummy(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["Dummy(Y/N)"]);
        //                            string txtStale = row["Stale Y/N"] == DBNull.Value ? "" : Convert.ToString(row["Stale Y/N"]);
        //                            Nullable<DateTime> dtLastModifiedDate = row["Last Modified Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Last Modified Date"]);
        //                            string txtProposalStatus = row["ProposalStatus"] == DBNull.Value ? "" : Convert.ToString(row["ProposalStatus"]);
        //                            Nullable<double> fltProposalValue = row["ProposalValue(USD)"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["ProposalValue(USD)"]);
        //                            string txtPropAnchor = row["PropAnchor"] == DBNull.Value ? "" : Convert.ToString(row["PropAnchor"]);
        //                            Nullable<DateTime> dtProposalCreatedDate = row["ProposalCreatedDate"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["ProposalCreatedDate"]);
        //                            string txtSelling = row["Selling/Direct/Extension"] == DBNull.Value ? "" : Convert.ToString(row["Selling/Direct/Extension"]);
        //                            string txtSellingInvolved = row["SellingInvolved"] == DBNull.Value ? "" : Convert.ToString(row["SellingInvolved"]);
        //                            string txtContractType = row["ContractType"] == DBNull.Value ? "" : Convert.ToString(row["ContractType"]);
        //                            Nullable<DateTime> dtOppClosedOn = row["OppClosedOn"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["OppClosedOn"]);
        //                            Nullable<DateTime> dtProposalSubmissionDate = row["ProposalSubmissionDate"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["ProposalSubmissionDate"]);
        //                            string txtCountry = row["Country"] == DBNull.Value ? "" : Convert.ToString(row["Country"]);
        //                            string txtSolutionName = row["SolutionName"] == DBNull.Value ? "" : Convert.ToString(row["SolutionName"]);
        //                            string txtAllianceName = row["AllianceName"] == DBNull.Value ? "" : Convert.ToString(row["AllianceName"]);
        //                            string txtDPSLinked = row["DPS Linked"] == DBNull.Value ? "" : Convert.ToString(row["DPS Linked"]);


        //                            service.InsertOpportunityDump(intCRMOppId, intOppId, txtOppName, intParentOppID, txtCompanyName, txtAccCode,
        //                            txtMCCode, txtPrimaryMember, txtSalesRegion, txtOppOwner, txtOppStage, txtOppStatus, txtNotes, fltTotalEstimate,
        //                            txtNativeCurrency, fltTotalEstimateInKUSD, intProbability, dtOppCreated, dtLikelyStartDate, dtLikelyEndDate,
        //                            txtTransformational, txtSGStagged, txtItrac, txtProposalSubmit, txtProposalNo, txtFlgTopOpp, txtPU,
        //                            txtDummy, txtStale, dtLastModifiedDate, txtProposalStatus, fltProposalValue, txtPropAnchor, dtProposalCreatedDate,
        //                            txtSelling, txtSellingInvolved, txtContractType, dtOppClosedOn, dtProposalSubmissionDate, txtCountry,
        //                            txtSolutionName, txtAllianceName, txtDPSLinked);
        //                            rowsupdated++;
        //                            //}
        //                        }

        //                        Session["FileName"] = path;
        //                        string ExcelFilePath = Session["FileName"].ToString();
        //                        //LoadPackage();
        //                        //UpdateExcelData();

        //                        //lblSuccess.Text = "";
        //                        if (con.State.ToString().ToLower() == "open")
        //                            con.Close();
        //                        lblSuccess.Text = "Data Uploaded Successfully";
        //                        lblSuccess.Visible = true;
        //                        lblError.Visible = false;
        //                    //}
        //                }
        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename sheet to 'Sheet1'";
        //                    lblError.Visible = true;
        //                }
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";

        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}


        ////Code for Opportunity Upload - Production
        //protected void btnOppProd_Click(object sender, EventArgs e)
        //{


        //    System.Data.DataTable dtExcel = new System.Data.DataTable();

        //    dtExcel.TableName = "MyExcelData";

        //    string folder = "ExcelOperationsProd";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "Cimba.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\Cimba.xlsx");

        //    string path = MyDir.FullName + "\\Cimba.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {


        //        string FileName = UploadOppProd.FileName;

        //        if (UploadOppProd.HasFile)
        //        {
        //            //System.Data.DataTable dtExcel = new System.Data.DataTable();

        //            //dtExcel.TableName = "MyExcelData";

        //            //string folder = "ExcelOperations";
        //            //string folder = "ExcelOperationsProd";
        //            //var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //            //if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "Cimba.xlsx") != null)
        //                //System.IO.File.Delete(MyDir.FullName + "\\Cimba.xlsx");
        //            if (FileName.Contains(".xls"))
        //            {

        //                string query = "Select * from [Sheet1$]";

        //                //StringBuilder query = new StringBuilder();
        //                //query.Append("SELECT * ").AppendFormat(" FROM [{0}$]", "FINAL");


        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("Sheet1$"))
        //                {

        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    bool isSuccess;

        //                    isSuccess = service.DeleteOpportunityDumpProd();

        //                    if (isSuccess)
        //                    {

        //                        data.Fill(dtExcel);

        //                        int noOfRows = dtExcel.Rows.Count;
        //                        int rowsupdated = 0;

        //                        foreach (DataRow row in dtExcel.Rows)
        //                        {

        //                            Nullable<int> nullableInt = null;
        //                            Nullable<DateTime> nullableDate = null;
        //                            Nullable<double> nullableDouble = null;



        //                            //Nullable<int> intCRMOppId = row["SAP Opportunity ID"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["SAP Opportunity ID"]);
        //                            //Nullable<int> intOppId = row["CIMBA Opportunity ID"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["CIMBA Opportunity ID"]);
        //                            //string txtOppName = row["Opportunity Name"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Name"]);
        //                            //Nullable<int> intParentOppID = row["Parent Opportunity ID"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Parent Opportunity ID"]);
        //                            //string txtCompanyName = row["Child Account Name"] == DBNull.Value ? "" : Convert.ToString(row["Child Account Name"]);
        //                            //string txtAccCode = row["Child Account Code"] == DBNull.Value ? "" : Convert.ToString(row["Child Account Code"]);
        //                            //string txtMCCode = row["Master Customer Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Customer Code"]);
        //                            //string txtPrimaryMember = row["Primary Owner Name"] == DBNull.Value ? "" : Convert.ToString(row["Primary Owner Name"]);
        //                            //string txtSalesRegion = row["Child Sales Region Code"] == DBNull.Value ? "" : Convert.ToString(row["Child Sales Region Code"]);
        //                            //string txtOppOwner = row["Opportunity Owner Name"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Owner Name"]);
        //                            //string txtOppStage = row["Opportunity Stage"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Stage"]);
        //                            //string txtOppStatus = row["Opportunity Status"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Status"]);
        //                            //string txtNotes = row["Opportunity Comments"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Comments"]);
        //                            //Nullable<double> fltTotalEstimate = row["Opportunity Total Estimates"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Opportunity Total Estimates"]);
        //                            //string txtNativeCurrency = row["Opportunity Currency Code"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Currency Code"]);
        //                            //Nullable<double> fltTotalEstimateInKUSD = row["Opportunity Total Estimates In KUSD"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Opportunity Total Estimates In KUSD"]);
        //                            //Nullable<int> intProbability = row["Opportunity Probability"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Opportunity Probability"]);
        //                            //Nullable<DateTime> dtOppCreated = row["Created Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Created Date"]);
        //                            //Nullable<DateTime> dtLikelyStartDate = row["Opportunity Likely Start Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Opportunity Likely Start Date"]);
        //                            //Nullable<DateTime> dtLikelyEndDate = row["Opportunity Likely End Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Opportunity Likely End Date"]);
        //                            //string txtTransformational = row["Program Transformational?"] == DBNull.Value ? "" : Convert.ToString(row["Program Transformational?"]);
        //                            //string txtSGStagged = row["SGS Tagged(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["SGS Tagged(Y/N)"]);
        //                            //string txtItrac = row["Itrac(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["Itrac(Y/N)"]);
        //                            //string txtProposalSubmit = row["ProposalSubmit(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["ProposalSubmit(Y/N)"]);
        //                            //string txtProposalNo = row["Proposal ID"] == DBNull.Value ? "" : Convert.ToString(row["Proposal ID"]);
        //                            //string txtFlgTopOpp = row["Top Opportunity?"] == DBNull.Value ? "" : Convert.ToString(row["Top Opportunity?"]);
        //                            //string txtPU = row["Opportunity Primary PU Code"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Primary PU Code"]);
        //                            //string txtDummy = row["Dummy Opportunity?"] == DBNull.Value ? "" : Convert.ToString(row["Dummy Opportunity?"]);
        //                            //string txtStale = row["Is Opportunity Stale?"] == DBNull.Value ? "" : Convert.ToString(row["Is Opportunity Stale?"]);
        //                            //Nullable<DateTime> dtLastModifiedDate = row["Last Modified Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Last Modified Date"]);
        //                            //string txtProposalStatus = row["Stage Description"] == DBNull.Value ? "" : Convert.ToString(row["Stage Description"]);
        //                            //Nullable<double> fltProposalValue = row["Total in USD as on Proposal Date"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Total in USD as on Proposal Date"]);
        //                            //string txtPropAnchor = row["Proposal Anchor Name"] == DBNull.Value ? "" : Convert.ToString(row["Proposal Anchor Name"]);
        //                            //Nullable<DateTime> dtProposalCreatedDate = row["ProposalCreatedDate"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["ProposalCreatedDate"]);
        //                            //string txtSelling = row["Opportunity Classification"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Classification"]);
        //                            //string txtSellingInvolved = row["Selling Involved"] == DBNull.Value ? "" : Convert.ToString(row["Selling Involved"]);
        //                            //string txtContractType = row["Contract Code"] == DBNull.Value ? "" : Convert.ToString(row["Contract Code"]);
        //                            //Nullable<DateTime> dtOppClosedOn = row["Opportunity Closed On"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Opportunity Closed On"]);
        //                            //Nullable<DateTime> dtProposalSubmissionDate = row["Proposal Submitted On"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Proposal Submitted On"]);
        //                            //string txtCountry = row["Country Name"] == DBNull.Value ? "" : Convert.ToString(row["Country Name"]);
        //                            //string txtSolutionName = row["Opportunity Solution Name"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Solution Name"]);
        //                            //string txtAllianceName = row["Opportunity Alliance Name"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Alliance Name"]);
        //                            //string txtDPSLinked = row["DPS Linked?"] == DBNull.Value ? "" : Convert.ToString(row["DPS Linked?"]);



        //                            Nullable<int> intCRMOppId = row["CRMOppId"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["CRMOppId"]);
        //                            Nullable<int> intOppId = row["OppId"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["OppId"]);
        //                            string txtOppName = row["OppName"] == DBNull.Value ? "" : Convert.ToString(row["OppName"]);
        //                            Nullable<int> intParentOppID = row["ParentOppID"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["ParentOppID"]);
        //                            string txtCompanyName = row["CompanyName"] == DBNull.Value ? "" : Convert.ToString(row["CompanyName"]);
        //                            string txtAccCode = row["Acc Code"] == DBNull.Value ? "" : Convert.ToString(row["Acc Code"]);
        //                            string txtMCCode = row["MC Code"] == DBNull.Value ? "" : Convert.ToString(row["MC Code"]);
        //                            string txtPrimaryMember = row["PrimaryMember"] == DBNull.Value ? "" : Convert.ToString(row["PrimaryMember"]);
        //                            string txtSalesRegion = row["Sales Region"] == DBNull.Value ? "" : Convert.ToString(row["Sales Region"]);
        //                            string txtOppOwner = row["OppOwner"] == DBNull.Value ? "" : Convert.ToString(row["OppOwner"]);
        //                            string txtOppStage = row["OppStage"] == DBNull.Value ? "" : Convert.ToString(row["OppStage"]);
        //                            string txtOppStatus = row["OppStatus"] == DBNull.Value ? "" : Convert.ToString(row["OppStatus"]);
        //                            string txtNotes = row["Notes"] == DBNull.Value ? "" : Convert.ToString(row["Notes"]);
        //                            Nullable<double> fltTotalEstimate = row["Total Est('000')"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Total Est('000')"]);
        //                            string txtNativeCurrency = row["NativeCurrency"] == DBNull.Value ? "" : Convert.ToString(row["NativeCurrency"]);
        //                            Nullable<double> fltTotalEstimateInKUSD = row["Total Est ('000')inUSD"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Total Est ('000')inUSD"]);
        //                            Nullable<int> intProbability = row["Probability"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Probability"]);
        //                            Nullable<DateTime> dtOppCreated = row["OppCreated"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["OppCreated"]);
        //                            Nullable<DateTime> dtLikelyStartDate = row["LikelyStartDate"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["LikelyStartDate"]);
        //                            Nullable<DateTime> dtLikelyEndDate = row["LikelyEndDate"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["LikelyEndDate"]);
        //                            string txtTransformational = row["Transformational(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["Transformational(Y/N)"]);
        //                            string txtSGStagged = row["SGS tagged(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["SGS tagged(Y/N)"]);
        //                            string txtItrac = row["Itrac(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["Itrac(Y/N)"]);
        //                            string txtProposalSubmit = row["ProposalSubmit(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["ProposalSubmit(Y/N)"]);
        //                            string txtProposalNo = row["ProposalNo"] == DBNull.Value ? "" : Convert.ToString(row["ProposalNo"]);
        //                            string txtFlgTopOpp = row["FlgTopOpp"] == DBNull.Value ? "" : Convert.ToString(row["FlgTopOpp"]);
        //                            string txtPU = row["PU"] == DBNull.Value ? "" : Convert.ToString(row["PU"]);
        //                            string txtDummy = row["Dummy(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["Dummy(Y/N)"]);
        //                            string txtStale = row["Stale Y/N"] == DBNull.Value ? "" : Convert.ToString(row["Stale Y/N"]);
        //                            Nullable<DateTime> dtLastModifiedDate = row["Last Modified Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Last Modified Date"]);
        //                            string txtProposalStatus = row["ProposalStatus"] == DBNull.Value ? "" : Convert.ToString(row["ProposalStatus"]);
        //                            Nullable<double> fltProposalValue = row["ProposalValue(USD)"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["ProposalValue(USD)"]);
        //                            string txtPropAnchor = row["PropAnchor"] == DBNull.Value ? "" : Convert.ToString(row["PropAnchor"]);
        //                            Nullable<DateTime> dtProposalCreatedDate = row["ProposalCreatedDate"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["ProposalCreatedDate"]);
        //                            string txtSelling = row["Selling/Direct/Extension"] == DBNull.Value ? "" : Convert.ToString(row["Selling/Direct/Extension"]);
        //                            string txtSellingInvolved = row["SellingInvolved"] == DBNull.Value ? "" : Convert.ToString(row["SellingInvolved"]);
        //                            string txtContractType = row["ContractType"] == DBNull.Value ? "" : Convert.ToString(row["ContractType"]);
        //                            Nullable<DateTime> dtOppClosedOn = row["OppClosedOn"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["OppClosedOn"]);
        //                            Nullable<DateTime> dtProposalSubmissionDate = row["ProposalSubmissionDate"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["ProposalSubmissionDate"]);
        //                            string txtCountry = row["Country"] == DBNull.Value ? "" : Convert.ToString(row["Country"]);
        //                            string txtSolutionName = row["SolutionName"] == DBNull.Value ? "" : Convert.ToString(row["SolutionName"]);
        //                            string txtAllianceName = row["AllianceName"] == DBNull.Value ? "" : Convert.ToString(row["AllianceName"]);
        //                            string txtDPSLinked = row["DPS Linked"] == DBNull.Value ? "" : Convert.ToString(row["DPS Linked"]);


        //                            service.InsertOpportunityDumpProd(intCRMOppId, intOppId, txtOppName, intParentOppID, txtCompanyName, txtAccCode,
        //                            txtMCCode, txtPrimaryMember, txtSalesRegion, txtOppOwner, txtOppStage, txtOppStatus, txtNotes, fltTotalEstimate,
        //                            txtNativeCurrency, fltTotalEstimateInKUSD, intProbability, dtOppCreated, dtLikelyStartDate, dtLikelyEndDate,
        //                            txtTransformational, txtSGStagged, txtItrac, txtProposalSubmit, txtProposalNo, txtFlgTopOpp, txtPU,
        //                            txtDummy, txtStale, dtLastModifiedDate, txtProposalStatus, fltProposalValue, txtPropAnchor, dtProposalCreatedDate,
        //                            txtSelling, txtSellingInvolved, txtContractType, dtOppClosedOn, dtProposalSubmissionDate, txtCountry,
        //                            txtSolutionName, txtAllianceName, txtDPSLinked);
        //                            rowsupdated++;
        //                        }

        //                        Session["FileName"] = path;
        //                        string ExcelFilePath = Session["FileName"].ToString();
        //                        if (con.State.ToString().ToLower() == "open")
        //                            con.Close();
        //                        lblSuccess.Text = "Data Uploaded Successfully";
        //                        lblSuccess.Visible = true;
        //                        lblError.Visible = false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();

        //                    lblError.Text = "";
        //                    lblError.Text = "File does not contain proper Sheet Name";
        //                    lblError.Visible = true;
        //                }
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";

        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}

        //protected void ImgOppSamProd_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/SampleXLSX/Cimba.xlsx");
        //}


        //protected void ImgOppExcelBkUpProd_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        DataAccess dataAccess = new DataAccess();
        //        DataTable dt = new DataTable();
        //        dt = dataAccess.GetOppBkUpProd();

        //        System.Web.UI.WebControls.DataGrid grid = new System.Web.UI.WebControls.DataGrid();
        //        grid.HeaderStyle.Font.Bold = true;
        //        grid.DataSource = dt;

        //        grid.DataBind();


        //        string Filename = "OpportunityDump.xlsx";

        //        //string folder = "ExcelOperations";
        //        string folder = "ExcelOperationsProd";

        //        var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //        if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "OpportunityDump.xlsx") == null)
        //            System.IO.File.Delete(MyDir.FullName + "\\OpportunityDump.xlsx");



        //        FileInfo file = new FileInfo(MyDir.FullName + "\\OpportunityDump.xlsx");
        //        using (ExcelPackage pck = new ExcelPackage(file))
        //        {
        //            //Create the worksheet
        //            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");

        //            //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
        //            ws.Cells["A1"].LoadFromDataTable(dt, true);
        //            pck.Save();
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("content-disposition", "attachment;  filename=OpportunityDump.xlsx");
        //            //Response.BinaryWrite(pck.GetAsByteArray());
        //        }


        //        DownloadFileProd(Filename);

        //    }
        //    catch (Exception ex)
        //    {
        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}


        ////Code for Employee Upload- Production
        //protected void btnEmpLoadProd_Click(object sender, EventArgs e)
        //{

        //    System.Data.DataTable dtExcel = new System.Data.DataTable();

        //    dtExcel.TableName = "MyExcelData";

        //    string folder = "ExcelOperationsProd";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "EmployeeReport.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\EmployeeReport.xlsx");

        //    string path = MyDir.FullName + "\\EmployeeReport.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {


        //        string FileName = UploadEmpProd.FileName;

        //        if (UploadEmpProd.HasFile)
        //        {

        //            //System.Data.DataTable dtExcel = new System.Data.DataTable();

        //            //dtExcel.TableName = "MyExcelData";

        //            //string folder = "ExcelOperations";
        //            //string folder = "ExcelOperationsProd";

        //            //var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //            //if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "EmployeeReport.xlsx") != null)
        //                //System.IO.File.Delete(MyDir.FullName + "\\EmployeeReport.xlsx");
        //            if (FileName.Contains(".xls"))
        //            {
        //                //string path = MyDir.FullName + "\\EmployeeReport.xlsx";// + FileName;
        //                //string filename = Path.GetFileName(FileUpload1.FileName);
        //                UploadEmpProd.SaveAs(path);

        //                //string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //                //OleDbConnection con = new OleDbConnection(SourceConstr);

        //                string query = "Select * from [Sheet1$]";

        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("Sheet1$"))
        //                {

        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    bool isSuccess;

        //                    isSuccess = service.DeleteEmployeeDetailsDumpProd();

        //                    if (isSuccess)
        //                    {

        //                        data.Fill(dtExcel);

        //                        int noOfRows = dtExcel.Rows.Count;
        //                        int rowsupdated = 0;

        //                        foreach (DataRow row in dtExcel.Rows)
        //                        {
        //                            Nullable<int> nullableInt = null;
        //                            Nullable<DateTime> nullableDate = null;
        //                            Nullable<double> nullableDouble = null;

        //                            Nullable<int> intEmpId = row["Emp No"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Emp No"]);
        //                            string txtEmpName = row["Emp Name"] == DBNull.Value ? "" : Convert.ToString(row["Emp Name"]);

        //                            string txtEmpMailId = row["Emp Mail ID"] == DBNull.Value ? "" : Convert.ToString(row["Emp Mail ID"]);
        //                            string txtEmpPU = row["Emp PU"] == DBNull.Value ? "" : Convert.ToString(row["Emp PU"]);
        //                            string txtEmpDU = row["Emp DU"] == DBNull.Value ? "" : Convert.ToString(row["Emp DU"]);
        //                            string txtEmpSubUnit = row["Emp Sub Unit"] == DBNull.Value ? "" : Convert.ToString(row["Emp Sub Unit"]);
        //                            string txtEmpUnit = row["Emp Unit"] == DBNull.Value ? "" : Convert.ToString(row["Emp Unit"]);
        //                            string txtBaseLocation = row["Emp Base Location"] == DBNull.Value ? "" : Convert.ToString(row["Emp Base Location"]);
        //                            Nullable<int> intDUDMId = row["DU Head for Emp DU"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["DU Head for Emp DU"]);
        //                            string txtDUDMName = row["DU Head Name for Emp DU"] == DBNull.Value ? "" : Convert.ToString(row["DU Head Name for Emp DU"]);
        //                            Nullable<int> intReportingToEmpId = row["Reporting To Emp No"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Reporting To Emp No"]);
        //                            string txtReportingToMailId = row["Reporting To Mail Id"] == DBNull.Value ? "" : Convert.ToString(row["Reporting To Mail Id"]);
        //                            string txtMasterProject = row["Master Project Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Project Code"]);
        //                            string txtRole = row["Role Capability"] == DBNull.Value ? "" : Convert.ToString(row["Role Capability"]);
        //                            Nullable<double> fltEmpTotalExp = row["Total Exp in Years"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Total Exp in Years"]);
        //                            string txtOnsiteOffshore = row["Onsite/Offshore"] == DBNull.Value ? "" : Convert.ToString(row["Onsite/Offshore"]);
        //                            string txtTechnologyCode = row["Tech Code"] == DBNull.Value ? "" : Convert.ToString(row["Tech Code"]);
        //                            Nullable<int> intJobBand = row["Job Band"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Job Band"]);
        //                            string txtEmpBaseCity = row["Emp Base City"] == DBNull.Value ? "" : Convert.ToString(row["Emp Base City"]);
        //                            Nullable<DateTime> dtJoiningDate = row["Joining Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Joining Date"]);
        //                            string txtEmpCurrentLoc = row["Emp Current Location"] == DBNull.Value ? "" : Convert.ToString(row["Emp Current Location"]);
        //                            string txtEmpCurrentCity = row["Emp Current City"] == DBNull.Value ? "" : Convert.ToString(row["Emp Current City"]);
        //                            string txtProjectCode = row["Project Code"] == DBNull.Value ? "" : Convert.ToString(row["Project Code"]);
        //                            string txtCustomerCode = row["Customer Code"] == DBNull.Value ? "" : Convert.ToString(row["Customer Code"]);
        //                            Nullable<DateTime> dtFromDate = row["From Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["From Date"]);
        //                            Nullable<DateTime> dtToDate = row["To Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["To Date"]);
        //                            string txtAllocatedCountry = row["Allocated Country"] == DBNull.Value ? "" : Convert.ToString(row["Allocated Country"]);
        //                            string txtAllocatedState = row["Allocated State"] == DBNull.Value ? "" : Convert.ToString(row["Allocated State"]);
        //                            string txtAllocatedCity = row["Allocated City"] == DBNull.Value ? "" : Convert.ToString(row["Allocated City"]);
        //                            string txtProjectType = row["Project Type"] == DBNull.Value ? "" : Convert.ToString(row["Project Type"]);
        //                            string txtBillingType = row["Billing Type"] == DBNull.Value ? "" : Convert.ToString(row["Billing Type"]);
        //                            string txtReportingToEmpName = row["Reporting To Emp Name"] == DBNull.Value ? "" : Convert.ToString(row["Reporting To Emp Name"]);
        //                            string txtProjRemarks = row["Proj Remarks"] == DBNull.Value ? "" : Convert.ToString(row["Proj Remarks"]);
        //                            string txtUnitCode = row["Unit Code"] == DBNull.Value ? "" : Convert.ToString(row["Unit Code"]);
        //                            string txtEmpExpInfosys = row["Emp Exp In Infosys "] == DBNull.Value ? "" : Convert.ToString(row["Emp Exp In Infosys "]);
        //                            string txtEmpPrevExp = row["Emp Prev Exp "] == DBNull.Value ? "" : Convert.ToString(row["Emp Prev Exp "]);
        //                            string txtEmpTotExpMonths = row["Emp Total Exp"] == DBNull.Value ? "" : Convert.ToString(row["Emp Total Exp"]);
        //                            string txtApplCode = row["Appl Code"] == DBNull.Value ? "" : Convert.ToString(row["Appl Code"]);
        //                            string txtServiceCode = row["Service Code"] == DBNull.Value ? "" : Convert.ToString(row["Service Code"]);
        //                            string txtActivityCode = row["Activity Code"] == DBNull.Value ? "" : Convert.ToString(row["Activity Code"]);
        //                            string txtTechCategory = row["Tech Category"] == DBNull.Value ? "" : Convert.ToString(row["Tech Category"]);
        //                            string txtMarketingBranchCode = row["Marketing Branch Code"] == DBNull.Value ? "" : Convert.ToString(row["Marketing Branch Code"]);
        //                            string txtProjDUCode = row["Proj DU Code"] == DBNull.Value ? "" : Convert.ToString(row["Proj DU Code"]);
        //                            string txtProjDevCentreCode = row["Proj DevCentre Code"] == DBNull.Value ? "" : Convert.ToString(row["Proj DevCentre Code"]);
        //                            string txtHRRemarks = row["HR Remarks"] == DBNull.Value ? "" : Convert.ToString(row["HR Remarks"]);
        //                            Nullable<int> intPersonalBand = row["Personal Band"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Personal Band"]);
        //                            string txtCustPortfolio = row["Customer Portfolio"] == DBNull.Value ? "" : Convert.ToString(row["Customer Portfolio"]);
        //                            string txtPartTimeEmp = row["PartTimeEmployee"] == DBNull.Value ? "" : Convert.ToString(row["PartTimeEmployee"]);
        //                            string txtProgramCode = row["ProgramCode"] == DBNull.Value ? "" : Convert.ToString(row["ProgramCode"]);
        //                            string txtTrackCode = row["TrackCode"] == DBNull.Value ? "" : Convert.ToString(row["TrackCode"]);
        //                            string txtEmpCompany = row["EmployeeCompany"] == DBNull.Value ? "" : Convert.ToString(row["EmployeeCompany"]);
        //                            string txtEmpBU = row["Employee Budgeting Unit"] == DBNull.Value ? "" : Convert.ToString(row["Employee Budgeting Unit"]);
        //                            string txtProjBU = row["Project Budgeting Unit"] == DBNull.Value ? "" : Convert.ToString(row["Project Budgeting Unit"]);
        //                            string txtProjCompany = row["Project Company"] == DBNull.Value ? "" : Convert.ToString(row["Project Company"]);
        //                            string txtProjSourceComp = row["Project Source Company"] == DBNull.Value ? "" : Convert.ToString(row["Project Source Company"]);
        //                            string txtEmpAttribute = row["Employee Attribute"] == DBNull.Value ? "" : Convert.ToString(row["Employee Attribute"]);
        //                            Nullable<DateTime> dtEmpDOB = row["Emp Date Of Birth"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Emp Date Of Birth"]);
        //                            string txtEmpGender = row["Emp Gender"] == DBNull.Value ? "" : Convert.ToString(row["Emp Gender"]);
        //                            Nullable<int> intProjDUDMId = row["Project DM "] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Project DM "]);
        //                            string txtProjDUDMName = row["Project DM Name"] == DBNull.Value ? "" : Convert.ToString(row["Project DM Name"]);
        //                            Nullable<int> intProjectDUhead = row["Project DU Head"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Project DU Head"]);
        //                            string txtProjectDUheadName = row["Project DUHead Name"] == DBNull.Value ? "" : Convert.ToString(row["Project DUHead Name"]);
        //                            Nullable<int> intProjectSDM = row["Project SDM"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Project SDM"]);
        //                            string txtProjectSDMmailId = row["Project SDM MailId"] == DBNull.Value ? "" : Convert.ToString(row["Project SDM MailId"]);
        //                            string txtGroupMasterProjectCode = row["Group Master Project Code"] == DBNull.Value ? "" : Convert.ToString(row["Group Master Project Code"]);

        //                            service.InsertEmployeeDetailsDumpProd(intEmpId, txtEmpName, txtEmpMailId, txtEmpPU, txtEmpDU,
        //                            txtEmpSubUnit, txtEmpUnit, txtBaseLocation, intDUDMId, txtDUDMName, intReportingToEmpId,
        //                            txtReportingToMailId, txtMasterProject, txtRole, fltEmpTotalExp, txtOnsiteOffshore,
        //                            txtTechnologyCode, intJobBand, txtEmpBaseCity, dtJoiningDate, txtEmpCurrentLoc,
        //                            txtEmpCurrentCity, txtProjectCode, txtCustomerCode, dtFromDate, dtToDate, txtAllocatedCountry,
        //                            txtAllocatedState, txtAllocatedCity, txtProjectType, txtBillingType, txtReportingToEmpName,
        //                            txtProjRemarks, txtUnitCode, txtEmpExpInfosys, txtEmpPrevExp, txtEmpTotExpMonths,
        //                            txtApplCode, txtServiceCode, txtActivityCode, txtTechCategory, txtMarketingBranchCode,
        //                            txtProjDUCode, txtProjDevCentreCode, txtHRRemarks, intPersonalBand, txtCustPortfolio,
        //                            txtPartTimeEmp, txtProgramCode, txtTrackCode, txtEmpCompany, txtEmpBU, txtProjBU, txtProjCompany,
        //                            txtProjSourceComp, txtEmpAttribute, dtEmpDOB, txtEmpGender, intProjDUDMId, txtProjDUDMName,
        //                            intProjectDUhead, txtProjectDUheadName, intProjectSDM, txtProjectSDMmailId, txtGroupMasterProjectCode);
        //                            rowsupdated++;
        //                            //}
        //                        }

        //                        Session["FileName"] = path;
        //                        string ExcelFilePath = Session["FileName"].ToString();
        //                        if (con.State.ToString().ToLower() == "open")
        //                            con.Close();
        //                        lblSuccess.Text = "Data Uploaded Successfully";
        //                        lblSuccess.Visible = true;
        //                        lblError.Visible = false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "File does not contain proper Sheet Name";
        //                    lblError.Visible = true;
        //                }

        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}

        //protected void ImgEmpSampProd_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/SampleXLSX/EmployeeReport.xlsx");
        //}

        //protected void ImgEmpExcelBkupProd_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        DataAccess dataAccess = new DataAccess();
        //        DataTable dt = new DataTable();
        //        dt = dataAccess.GetEmployeeBkUpProd();

        //        //System.Web.UI.WebControls.DataGrid grid = new System.Web.UI.WebControls.DataGrid();
        //        //grid.HeaderStyle.Font.Bold = true;
        //        //grid.DataSource = dt;
        //        //grid.DataBind();

        //        //string Filename = "EmployeeDump.xls";

        //        //string folder = "ExcelOperations";
        //        //string folder = "ExcelOperationsProd";

        //        //var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //        //using (StreamWriter sw = new StreamWriter(MyDir.FullName + "\\" + Filename))
        //        //{
        //        //    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
        //        //    {
        //        //        grid.RenderControl(hw);
        //        //    }
        //        //}


        //        System.Web.UI.WebControls.DataGrid grid = new System.Web.UI.WebControls.DataGrid();
        //        grid.HeaderStyle.Font.Bold = true;
        //        grid.DataSource = dt;
        //        grid.DataBind();

        //        string Filename = "EmployeeDump.xlsx";

        //        //string folder = "ExcelOperations";
        //        string folder = "ExcelOperationsProd";

        //        var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //        if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "EmployeeDump.xlsx") == null)
        //            System.IO.File.Delete(MyDir.FullName + "\\EmployeeDump.xlsx");

        //        FileInfo file = new FileInfo(MyDir.FullName + "\\EmployeeDump.xlsx");
        //        using (ExcelPackage pck = new ExcelPackage(file))
        //        {
        //            //Create the worksheet
        //            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");

        //            //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
        //            ws.Cells["A1"].LoadFromDataTable(dt, true);
        //            pck.Save();
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("content-disposition", "attachment;  filename=EmployeeDump.xlsx");
        //            //Response.BinaryWrite(pck.GetAsByteArray());
        //        }

        //        DownloadFileProd(Filename);
        //    }

        //    catch (Exception ex)
        //    {

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}



        ////Code for Project Upload - Production
        //protected void btnProjloadProd_Click(object sender, EventArgs e)
        //{
        //    System.Data.DataTable dtExcel = new System.Data.DataTable();

        //    dtExcel.TableName = "MyExcelData";

        //    string folder = "ExcelOperationsProd";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "ProjectCode.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\ProjectCode.xlsx");

        //    string path = MyDir.FullName + "\\ProjectCode.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {
        //        string FileName = UploadProjProd.FileName;

        //        if (UploadProjProd.HasFile)
        //        {
        //            //System.Data.DataTable dtExcel = new System.Data.DataTable();

        //            //dtExcel.TableName = "MyExcelData";

        //            //string folder = "ExcelOperations";
        //            //string folder = "ExcelOperationsProd";


        //            //var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //            //if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "ProjectCode.xlsx") != null)
        //                //System.IO.File.Delete(MyDir.FullName + "\\ProjectCode.xlsx");
        //            if (FileName.Contains(".xls"))
        //            {
        //                //string path = MyDir.FullName + "\\ProjectCode.xlsx";// + FileName;
        //                //string filename = Path.GetFileName(FileUpload1.FileName);
        //                UploadProjProd.SaveAs(path);

        //                //string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //                //OleDbConnection con = new OleDbConnection(SourceConstr);

        //                string query = "Select * from [Sheet1$]";


        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("Sheet1$"))
        //                {

        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    bool isSuccess;

        //                    isSuccess = service.DeleteProjectDetailsDumpProd();

        //                    if (isSuccess)
        //                    {

        //                        data.Fill(dtExcel);

        //                        int noOfRows = dtExcel.Rows.Count;
        //                        int rowsupdated = 0;

        //                        foreach (DataRow row in dtExcel.Rows)
        //                        {
        //                            Nullable<int> nullableInt = null;
        //                            Nullable<DateTime> nullableDate = null;

        //                            string txtClientSubUnit = row["ClientSubUnit"] == DBNull.Value ? "" : Convert.ToString(row["ClientSubUnit"]);
        //                            string txtSubUnit = row["SubUnit"] == DBNull.Value ? "" : Convert.ToString(row["SubUnit"]);
        //                            string txtPU = row["PU"] == DBNull.Value ? "" : Convert.ToString(row["PU"]);
        //                            string txtCustomerCode = row["CustomerCode"] == DBNull.Value ? "" : Convert.ToString(row["CustomerCode"]);
        //                            Nullable<int> intLOENumber = row["LOENumber"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["LOENumber"]);
        //                            string txtMasterProjectCode = row["MasterProjectCode"] == DBNull.Value ? "" : Convert.ToString(row["MasterProjectCode"]);
        //                            string txtChildProjectCode = row["ChildProjectCode"] == DBNull.Value ? "" : Convert.ToString(row["ChildProjectCode"]);
        //                            string txtDescription = row["Description"] == DBNull.Value ? "" : Convert.ToString(row["Description"]);
        //                            Nullable<DateTime> dtStartDate = row["StartDate"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["StartDate"]);
        //                            Nullable<DateTime> dtEndDate = row["EndDate"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["EndDate"]);
        //                            string txtProjectType = row["ProjectType"] == DBNull.Value ? "" : Convert.ToString(row["ProjectType"]);
        //                            Nullable<DateTime> dtProjectCreatedOn = row["ProjectCreatedOn"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["ProjectCreatedOn"]);
        //                            string txtDMMailID = row["DeliveryManagerMailID"] == DBNull.Value ? "" : Convert.ToString(row["DeliveryManagerMailID"]);
        //                            string txtPMMailID = row["ProjectManagerMailID"] == DBNull.Value ? "" : Convert.ToString(row["ProjectManagerMailID"]);
        //                            string txtDevelopmentCenter = row["DevelopmentCenter"] == DBNull.Value ? "" : Convert.ToString(row["DevelopmentCenter"]);
        //                            string txtBU = row["BU"] == DBNull.Value ? "" : Convert.ToString(row["BU"]);
        //                            string txtDU = row["DU"] == DBNull.Value ? "" : Convert.ToString(row["DU"]);
        //                            string txtCreditSubUnit = row["CreditSubUnit"] == DBNull.Value ? "" : Convert.ToString(row["CreditSubUnit"]);
        //                            string txtCountryCode = row["CountryCode"] == DBNull.Value ? "" : Convert.ToString(row["CountryCode"]);
        //                            string txtProjectStateCode = row["ProjectStateCode"] == DBNull.Value ? "" : Convert.ToString(row["ProjectStateCode"]);
        //                            Nullable<int> intNetworkNumber = row["NetworkNumber"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["NetworkNumber"]);
        //                            string txtTechnology = row["Technology"] == DBNull.Value ? "" : Convert.ToString(row["Technology"]);
        //                            string txtServiceOffering = row["ServiceOffering"] == DBNull.Value ? "" : Convert.ToString(row["ServiceOffering"]);
        //                            string txtSecondaryServiceCode = row["SecondaryServiceCode"] == DBNull.Value ? "" : Convert.ToString(row["SecondaryServiceCode"]);
        //                            string txtCustomerServiceOffering = row["CustomerServiceOffering"] == DBNull.Value ? "" : Convert.ToString(row["CustomerServiceOffering"]);
        //                            string txtCreatedBy = row["CreatedBy"] == DBNull.Value ? "" : Convert.ToString(row["CreatedBy"]);
        //                            string txtReportingPU = row["ReportingPU"] == DBNull.Value ? "" : Convert.ToString(row["ReportingPU"]);
        //                            string txtMappedProjectCode = row["MappedProjectCode"] == DBNull.Value ? "" : Convert.ToString(row["MappedProjectCode"]);
        //                            string txtCreatedInSAP = row["CreatedInSAP"] == DBNull.Value ? "" : Convert.ToString(row["CreatedInSAP"]);
        //                            string txtContractType = row["ContractType"] == DBNull.Value ? "" : Convert.ToString(row["ContractType"]);
        //                            string txtTimeBasedBilling = row["TimeBasedBilling"] == DBNull.Value ? "" : Convert.ToString(row["TimeBasedBilling"]);
        //                            string txtGroupMasterProjCode = row["GroupMasterProjectCode"] == DBNull.Value ? "" : Convert.ToString(row["GroupMasterProjectCode"]);


        //                            service.InsertProjectDetailsDumpProd(txtClientSubUnit, txtSubUnit, txtPU, txtCustomerCode,
        //                            intLOENumber, txtMasterProjectCode, txtChildProjectCode, txtDescription,
        //                            dtStartDate, dtEndDate, txtProjectType, dtProjectCreatedOn, txtDMMailID, txtPMMailID,
        //                            txtDevelopmentCenter, txtBU, txtDU, txtCreditSubUnit, txtCountryCode, txtProjectStateCode,
        //                            intNetworkNumber, txtTechnology, txtServiceOffering, txtSecondaryServiceCode,
        //                            txtCustomerServiceOffering, txtCreatedBy, txtReportingPU, txtMappedProjectCode,
        //                            txtCreatedInSAP, txtContractType, txtTimeBasedBilling, txtGroupMasterProjCode);
        //                            rowsupdated++;
        //                            //}
        //                        }

        //                        Session["FileName"] = path;
        //                        string ExcelFilePath = Session["FileName"].ToString();
        //                        if (con.State.ToString().ToLower() == "open")
        //                            con.Close();
        //                        lblSuccess.Text = "Data Uploaded Successfully";
        //                        lblSuccess.Visible = true;
        //                        lblError.Visible = false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "File does not contain proper Sheet Name";
        //                    lblError.Visible = true;
        //                }
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";
        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}

        //protected void imgProjSampProd_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/SampleXLSX/ProjectCode.xlsx");
        //}

        //protected void ImgProjExcelBkupProd_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        DataAccess dataAccess = new DataAccess();
        //        DataTable dt = new DataTable();
        //        dt = dataAccess.GetProjectBkUpProd();

        //        //System.Web.UI.WebControls.DataGrid grid = new System.Web.UI.WebControls.DataGrid();
        //        //grid.HeaderStyle.Font.Bold = true;
        //        //grid.DataSource = dt;

        //        //grid.DataBind();


        //        //string Filename = "ProjectDump.xls";

        //        ////string folder = "ExcelOperations";
        //        //string folder = "ExcelOperationsProd";


        //        //var MyDir = new DirectoryInfo(Server.MapPath(folder));
        //        //using (StreamWriter sw = new StreamWriter(MyDir.FullName + "\\" + Filename))
        //        //{
        //        //    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
        //        //    {
        //        //        grid.RenderControl(hw);
        //        //    }
        //        //}



        //        System.Web.UI.WebControls.DataGrid grid = new System.Web.UI.WebControls.DataGrid();
        //        grid.HeaderStyle.Font.Bold = true;
        //        grid.DataSource = dt;
        //        grid.DataBind();

        //        string Filename = "ProjectDump.xlsx";

        //        //string folder = "ExcelOperations";
        //        string folder = "ExcelOperationsProd";

        //        var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //        if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "ProjectDump.xlsx") == null)
        //            System.IO.File.Delete(MyDir.FullName + "\\ProjectDump.xlsx");

        //        FileInfo file = new FileInfo(MyDir.FullName + "\\ProjectDump.xlsx");
        //        using (ExcelPackage pck = new ExcelPackage(file))
        //        {
        //            //Create the worksheet
        //            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");

        //            //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
        //            ws.Cells["A1"].LoadFromDataTable(dt, true);
        //            pck.Save();
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("content-disposition", "attachment;  filename=ProjectDump.xlsx");
        //            //Response.BinaryWrite(pck.GetAsByteArray());
        //        }

        //        DownloadFileProd(Filename);
        //    }
        //    catch (Exception ex)
        //    {

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}

        #endregion
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

                if ((ex.Message + "").Contains("Thread was being aborted."))
                    logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                else
                {
                    logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }
            }

        }
        //Dev code for FinPulse
        protected void btnfinpulUpload_Click(object sender, EventArgs e)
        {
         

            System.Data.DataTable dtExcel = new System.Data.DataTable();

            dtExcel.TableName = "MyExcelData";

            System.Data.DataTable dtExcelMonthFilter = new System.Data.DataTable();

            dtExcelMonthFilter.TableName = "MyExcelDataMonthFilter";

            string folder = "ExcelOperations";

            var MyDir = new DirectoryInfo(Server.MapPath(folder));

            if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "FinPulse.xlsx") != null)
                System.IO.File.Delete(MyDir.FullName + "\\FinPulse.xlsx");

            //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "4", "4");


            string path = MyDir.FullName + "\\FinPulse.xlsx";// + FileName;

            string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";


            //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "5", "5");

            OleDbConnection con = new OleDbConnection(SourceConstr);

       

            try
            {

                string FileName = FinPulseUpload.FileName;
                if (FinPulseUpload.HasFile)
                {

                    string fileExt = Path.GetExtension(FinPulseUpload.FileName);
                    if ((fileExt.Equals(".xlsx")) || (fileExt.Equals(".xls")) && FinPulseUpload.PostedFile.ContentLength != 0)
                    {
                        if (FileName == "FinPulse.xls" || FileName == "FinPulse.xlsx")
                        {

                            FinPulseUpload.SaveAs(path);

                         

                            string query = "Select * from [Finpulse$]";
                            OleDbDataAdapter data = new OleDbDataAdapter(query, con);
                            data.Fill(dtExcel);
                            int noOfRows = dtExcel.Rows.Count;
                            //Code to check if sheet is having proper name
                            con.Open();

                            DataTable worksheets = con.GetSchema("Tables");
                            string w = worksheets.Columns["TABLE_NAME"].ToString();
                            List<string> lstsheetNames = new List<string>();
                            Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

                            worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

                       


                            if (lstsheetNames.Contains("Finpulse$"))
                            {
                             

                                OleDbDataAdapter data1 = new OleDbDataAdapter(query, con);

                               
                                data1.Fill(dtExcelMonthFilter);
                                

                                dtExcelMonthFilter = dtExcelMonthFilter.DefaultView.ToTable(true, "YearMonth");

                                string yearfilter = "";

                                int icount = dtExcelMonthFilter.Rows.Count;

                                string[] YearMonthFilter = new string[icount];
                                for (int j = 0; j < icount; j++)
                                {
                                    if (j == 0)
                                    {
                                        yearfilter = dtExcelMonthFilter.Rows[j][0].ToString();
                                    }
                                    else
                                    {
                                        yearfilter = yearfilter + "," + dtExcelMonthFilter.Rows[j][0].ToString();
                                    }
                                }

                                string drpMonth = ddlMonth.SelectedItem.ToString();
                                string drpyear = drpYer.SelectedItem.ToString();
                                string drpyearmonth = drpyear + drpMonth;



                                if (drpyearmonth == yearfilter)
                                {
                                    // isSuccess = service.DeleteFinPulseDump(yearmonth);


                                    Application app = new Application();
                                    Package package = null;

                                    string folderpkg = "ETL";

                                    var MyDirpkg = new DirectoryInfo(Server.MapPath(folderpkg));

                                    //Load DTSX
                                    string pathpkg = @"D:\ETLFinPulse@\ETLFinPulse@\Package.dtsx";
                                    package = app.LoadPackage(pathpkg, null);

                                    //Global Package Variable
                                    Variables vars = package.Variables;
                                    vars["ServiceLine"].Value = ddlServiceline.SelectedItem.ToString();
                                    vars["YearMonth"].Value = drpyearmonth;
                                    vars["Year"].Value = drpYer.SelectedItem.ToString();


                                    //Specify Excel Connection From DTSX Connection Manager
                                   // package.Connections["SourceConnectionExcel"].ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + fileName + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";


                                    //Execute DTSX.
                                    Microsoft.SqlServer.Dts.Runtime.DTSExecResult results = package.Execute();
                                    if (results == Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure)
                                    {
                                        StringBuilder strError = new StringBuilder();
                                        foreach (Microsoft.SqlServer.Dts.Runtime.DtsError local_DtsError in package.Errors)
                                        {
                                            Console.WriteLine("Package Execution results: {0}", local_DtsError.Description.ToString());
                                            Console.WriteLine();
                                            strError.Append(local_DtsError.Description.ToString());
                                            strError.Append("-");
                                        }
                                        lblError.Text = strError.ToString();
                                        lblError.Visible = true;
                                        if (con.State.ToString().ToLower() == "open")
                                            con.Close();
                                    }
                                    else
                                    {
                                        lblSuccess.Visible = true;

                                        int cnt = service.FinPulseDumpCountDev();
                                        if (con.State.ToString().ToLower() == "open")
                                            con.Close();
                                        lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in Excel : " + fontRed + noOfRows + fontEnd +
                                            " -No. of records in the table after upload :" + fontRed + cnt;
                            
                                    }

                                }

                                else
                                {
                                    if (con.State.ToString().ToLower() == "open")
                                        con.Close();

                                    Alert("Data for the selected criterion does not exist in the Excel");
                                }
                            }
                            else
                            {
                                if (con.State.ToString().ToLower() == "open")
                                    con.Close();
                                lblError.Text = "";
                                lblError.Text = "Please rename the sheet to 'Finpulse'";
                                lblError.Visible = true;
                                lblSuccess.Visible = false;
                            }
                         
                        }

                         else
                        {
                            if (con.State.ToString().ToLower() == "open")
                                con.Close();
                            lblError.Text = "";
                            lblError.Text = "Please rename the Excel to 'FinPulse'";
                            lblError.Visible = true;
                            lblSuccess.Visible = false;
                        }

                    }

                    else
                    {
                        if (con.State.ToString().ToLower() == "open")
                            con.Close();
                        lblError.Text = "";
                        lblError.Text = "File is not in specified Format";
                        lblError.Visible = true;
                        lblSuccess.Visible = false;
                    }
                

                }

                else
                {
                    lblError.Text = "";
                    lblError.Text = "Please Select a File";
                    lblError.Visible = true;
                    lblSuccess.Visible = false;
                }


                        }
              
           
          
            catch (Exception ex)
            {
                if (con.State.ToString().ToLower() == "open")
                    con.Close();

                if ((ex.Message + "").Contains("Thread was being aborted."))
                    logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                else
                {
                    logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }
            }
        }


        protected void lnkDmMailId_Click(object sender, EventArgs e)
        {
            Response.Redirect("UpdateDMMailID.aspx");
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("UpdateDMMailIDProd.aspx");
        }


        string fontRed = "<font style=\"font-weight:bold\" color='red'>";
        string fontEnd = "</font>";

      



        private void Alert()
        {

            try
            {
                Page page = HttpContext.Current.CurrentHandler as Page;
                if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("onclick"))
                {
                    page.RegisterClientScriptBlock("onclick", "<script type=\"text/javascript\">PopUpMasterClientList();</script>");

                }
            }
            catch (Exception ex)
            {
                if ((ex.Message + "").Contains("Thread was being aborted."))
                    logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                else
                {
                    logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }
            }
        }




        //Code for Production - FinPulse
        //protected void btnfinpulUploadProd_Click(object sender, EventArgs e)
        //{
        //    int year = dateTime.Year;

        //    DateTime todaydate = DateTime.Now;
        //    int curyear = todaydate.Year - 2000;
        //    //string yearmonth1;
        //    //string yearmonth2;
        //    //string yearmonth3;
        //    string yearmonth = string.Empty;

        //    string strquarter;
        //    int nextyear = year + 1;
        //    if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)
        //        strquarter = "Q4";
        //    else if (todaydate.Month == 4 || todaydate.Month == 5 || todaydate.Month == 6)
        //        strquarter = "Q1";
        //    else if (todaydate.Month == 7 || todaydate.Month == 8 || todaydate.Month == 9)
        //        strquarter = "Q2";
        //    else
        //        strquarter = "Q3";

        //    if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3 || todaydate.Month == 4 || todaydate.Month == 5 || todaydate.Month == 6 || todaydate.Month == 7 || todaydate.Month == 8 || todaydate.Month == 9)
        //    {
        //        yearmonth = Convert.ToString(year) + "0" + Convert.ToString(todaydate.Month - 1);
        //    }
        //    else
        //    {
        //        yearmonth = Convert.ToString(year) + Convert.ToString(todaydate.Month - 1);
        //    }

        //    //if (strquarter == "Q4")
        //    //{
        //    //    yearmonth1 = Convert.ToString(year) + "0" + todaydate.Month;
        //    //    yearmonth2 = Convert.ToString(year) + "0" + todaydate.Month;
        //    //    yearmonth3 = Convert.ToString(year) + "0" + todaydate.Month;
        //    //}
        //    //if (strquarter == "Q1")
        //    //{
        //    //    yearmonth1 = Convert.ToString(year) + "0" + todaydate.Month;
        //    //    yearmonth2 = Convert.ToString(year) + "0" + todaydate.Month;
        //    //    yearmonth3 = Convert.ToString(year) + "0" + todaydate.Month;
        //    //}
        //    //if (strquarter == "Q2")
        //    //{
        //    //    yearmonth1 = Convert.ToString(year) + "0" + todaydate.Month;
        //    //    yearmonth2 = Convert.ToString(year) + "0" + todaydate.Month;
        //    //    yearmonth3 = Convert.ToString(year) + "0" + todaydate.Month;
        //    //}
        //    //if (strquarter == "Q3")
        //    //{
        //    //    yearmonth1 = Convert.ToString(year) +  todaydate.Month;
        //    //    yearmonth2 = Convert.ToString(year) +  todaydate.Month;
        //    //    yearmonth3 = Convert.ToString(year) +  todaydate.Month;
        //    //}

        //    System.Data.DataTable dtExcel = new System.Data.DataTable();

        //    dtExcel.TableName = "MyExcelData";

        //    System.Data.DataTable dtExcelMonthFilter = new System.Data.DataTable();

        //    dtExcelMonthFilter.TableName = "MyExcelDataMonthFilter";

        //    string folder = "ExcelOperationsProd";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));



        //    //Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
        //    //string fileName = MyDir.FullName + "\\FinPulse.xlsx";
        //    //Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
        //    //System.String DataFile = fileName;
        //    //app.DisplayAlerts = false;
        //    //Microsoft.Office.Interop.Excel.Workbook aBk = app.Workbooks.Open(DataFile);

        //    ///* Do things */

        //    //aBk.Close(true);


        //    //Microsoft.Office.Interop.Excel.Workbooks


        //    //Microsoft.Office.Interop.Excel.Workbook workbook = this.app.Workbooks.get_Item(fileName);
        //    //workbook.Close(false);


        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "FinPulse.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\FinPulse.xlsx");

        //    string path = MyDir.FullName + "\\FinPulse.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {
        //        string FileName = FinPulUploadProd.FileName;

        //        if (FinPulUploadProd.HasFile)
        //        {
        //            //System.Data.DataTable dtExcel = new System.Data.DataTable();

        //            //dtExcel.TableName = "MyExcelData";

        //            //string folder = "ExcelOperations";
        //            //var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //            //if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "ProjectCode.xlsx") != null)
        //            //System.IO.File.Delete(MyDir.FullName + "\\ProjectCode.xlsx");
        //            if (FileName.Contains(".xls"))
        //            {
        //                //string path = MyDir.FullName + "\\ProjectCode.xlsx";// + FileName;
        //                //string filename = Path.GetFileName(FileUpload1.FileName);
        //                FinPulUploadProd.SaveAs(path);

        //                //string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //                //OleDbConnection con = new OleDbConnection(SourceConstr);

        //                string query = "Select * from [Finpulse$]";

        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("Finpulse$"))
        //                {

        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    //bool isSuccess;

        //                    //bool isSuccess;
        //                    data.Fill(dtExcelMonthFilter);
        //                    //dtExcelMonthFilter.DefaultView.RowFilter = "[YearMonth]";

        //                    //DataTable dt = new DataTable();

        //                    dtExcelMonthFilter = dtExcelMonthFilter.DefaultView.ToTable(true, "YearMonth");

        //                    string yearfilter = "";

        //                    int icount = dtExcelMonthFilter.Rows.Count;
        //                    string[] YearMonthFilter = new string[icount];
        //                    for (int j = 0; j < icount; j++)
        //                    {
        //                        if (j == 0)
        //                        {
        //                            yearfilter = dtExcelMonthFilter.Rows[j][0].ToString();
        //                        }
        //                        else
        //                        {
        //                            yearfilter = yearfilter + "," + dtExcelMonthFilter.Rows[j][0].ToString();
        //                        }
        //                    }

        //                    //isSuccess = service.DeleteFinPulseDump(yearmonth);
        //                    service.DeleteFinPulseDumpProd(yearfilter, drpSUProd.Text.Trim(), drpYerProd.Text.Trim());

        //                    //if (isSuccess)
        //                    //{

        //                    data.Fill(dtExcel);
        //                    //dtExcel.Columns[YearMonth].
        //                    int noOfRows = dtExcel.Rows.Count;
        //                    int rowsupdated = 0;
        //                    //dtExcel.TableName["YearMonth"][0];

        //                    string yearmonth4 = string.Empty;

        //                    //dtExcel.DefaultView.RowFilter = "[YearMonth] IN ('" + yearmonth + "')";
        //                    //dtExcel = dtExcel.DefaultView.ToTable();

        //                    foreach (DataRow row in dtExcel.Rows)
        //                    {
        //                        Nullable<int> nullableInt = null;
        //                        Nullable<DateTime> nullableDate = null;
        //                        Nullable<double> nullableDouble = null;

        //                        string ProjectCode = row["ProjectCode"] == DBNull.Value ? "" : Convert.ToString(row["ProjectCode"]);
        //                        string MasterProject = row["MasterProject"] == DBNull.Value ? "" : Convert.ToString(row["MasterProject"]);
        //                        string ProjectGeography = row["ProjectGeography"] == DBNull.Value ? "" : Convert.ToString(row["ProjectGeography"]);
        //                        string DU = row["DU"] == DBNull.Value ? "" : Convert.ToString(row["DU"]);

        //                        Nullable<double> YearMonth = row["YearMonth"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["YearMonth"]);
        //                        Nullable<double> OnSiteRevenue = row["OnSiteRevenue"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OnSiteRevenue"]);

        //                        Nullable<double> OffShoreRevenue = row["OffShoreRevenue"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OffShoreRevenue"]);
        //                        Nullable<double> TotalRevenue = row["TotalRevenue"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["TotalRevenue"]);
        //                        Nullable<double> OnsiteCostofRevenueDirect = row["OnsiteCostofRevenueDirect"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OnsiteCostofRevenueDirect"]);
        //                        Nullable<double> OffShoreCostofRevenueDirect = row["OffShoreCostofRevenueDirect"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OffShoreCostofRevenueDirect"]);
        //                        Nullable<double> TotalCostofRevenueDirect = row["TotalCostofRevenueDirect"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["TotalCostofRevenueDirect"]);
        //                        Nullable<double> OnsiteProjectMargin = row["OnsiteProjectMargin"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OnsiteProjectMargin"]);
        //                        Nullable<double> OffshoreProjectMargin = row["OffshoreProjectMargin"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OffshoreProjectMargin"]);
        //                        Nullable<double> ProjectMargin = row["ProjectMargin"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["ProjectMargin"]);
        //                        Nullable<double> TotalCostofRevenueAllocated = row["TotalCostofRevenueAllocated"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["TotalCostofRevenueAllocated"]);
        //                        Nullable<double> PUDelyMargin = row["PUDelyMargin"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["PUDelyMargin"]);
        //                        Nullable<double> TotalCostofRevenueOtherCosts = row["TotalCostofRevenueOtherCosts"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["TotalCostofRevenueOtherCosts"]);
        //                        Nullable<double> GrossMargin = row["GrossMargin"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["GrossMargin"]);
        //                        Nullable<double> TotalSGADirect = row["TotalSGADirect"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["TotalSGADirect"]);
        //                        Nullable<double> TotalSellingAllocated = row["TotalSellingAllocated"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["TotalSellingAllocated"]);
        //                        Nullable<double> TotalGAAllocated = row["TotalGAAllocated"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["TotalGAAllocated"]);
        //                        Nullable<double> PBTBeforeInvestment = row["PBTBeforeInvestment"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["PBTBeforeInvestment"]);
        //                        Nullable<double> PBTAfterInvestmentForexLosses = row["PBTAfterInvestmentForexLosses"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["PBTAfterInvestmentForexLosses"]);
        //                        Nullable<double> TotalTaxesExcludingIndiaTax = row["TotalTaxesExcludingIndiaTax"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["TotalTaxesExcludingIndiaTax"]);
        //                        Nullable<double> PATBeforeIndiaTax = row["PATBeforeIndiaTax"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["PATBeforeIndiaTax"]);
        //                        Nullable<double> IndiaTax = row["IndiaTax"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["IndiaTax"]);
        //                        Nullable<double> PATAfterIndiaTax = row["PATAfterIndiaTax"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["PATAfterIndiaTax"]);
        //                        Nullable<double> TotalExpense = row["TotalExpense"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["TotalExpense"]);
        //                        Nullable<double> OnSiteBilledMonths = row["OnSiteBilledMonths"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OnSiteBilledMonths"]);
        //                        Nullable<double> OffShoreBilledMonths = row["OffShoreBilledMonths"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OffShoreBilledMonths"]);
        //                        Nullable<double> TotalBilledMonths = row["TotalBilledMonths"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["TotalBilledMonths"]);
        //                        Nullable<double> BenchMonths = row["BenchMonths"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["BenchMonths"]);

        //                        Nullable<double> TotalBillableMonths = row["TotalBillableMonths"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["TotalBillableMonths"]);
        //                        Nullable<double> RDMonths = row["RDMonths"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["RDMonths"]);
        //                        Nullable<double> SolutionMonths = row["SolutionMonths"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["SolutionMonths"]);
        //                        Nullable<double> OverheadMonths = row["OverheadMonths"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OverheadMonths"]);
        //                        Nullable<double> TrainingMonths = row["TrainingMonths"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["TrainingMonths"]);
        //                        Nullable<double> Leave = row["Leave"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Leave"]);
        //                        Nullable<double> OnsiteRDMonths = row["OnsiteRDMonths"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OnsiteRDMonths"]);
        //                        Nullable<double> OffshoreRDMonths = row["OffshoreRDMonths"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OffshoreRDMonths"]);
        //                        Nullable<double> OnsiteSolutionMonths = row["OnsiteSolutionMonths"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OnsiteSolutionMonths"]);
        //                        Nullable<double> OffshoreSolutionMonths = row["OffshoreSolutionMonths"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OffshoreSolutionMonths"]);
        //                        Nullable<double> OnsiteOverheads = row["OnsiteOverheads"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OnsiteOverheads"]);
        //                        Nullable<double> OffshoreOverheads = row["OffshoreOverheads"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OffshoreOverheads"]);
        //                        Nullable<double> OnsiteTraining = row["OnsiteTraining"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OnsiteTraining"]);

        //                        Nullable<double> OffshoreTraining = row["OffshoreTraining"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OffshoreTraining"]);
        //                        Nullable<double> OnsiteLeave = row["OnsiteLeave"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OnsiteLeave"]);
        //                        Nullable<double> OffshoreLeave = row["OffshoreLeave"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OffshoreLeave"]);
        //                        Nullable<double> Buffer = row["Buffer"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Buffer"]);
        //                        Nullable<double> OnsiteBuffer = row["OnsiteBuffer"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OnsiteBuffer"]);
        //                        Nullable<double> OffshoreBuffer = row["OffshoreBuffer"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OffshoreBuffer"]);
        //                        Nullable<double> TotalPersonMonths = row["TotalPersonMonths"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["TotalPersonMonths"]);
        //                        Nullable<double> Support = row["Support"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Support"]);
        //                        Nullable<double> OnsiteSupport = row["OnsiteSupport"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OnsiteSupport"]);
        //                        Nullable<double> OffshoreSupport = row["OffshoreSupport"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OffshoreSupport"]);

        //                        string ReportingPU = row["ReportingPU"] == DBNull.Value ? "" : Convert.ToString(row["ReportingPU"]);
        //                        string SBUPUGroup = row["SBUPUGroup"] == DBNull.Value ? "" : Convert.ToString(row["SBUPUGroup"]);
        //                        string SubUnit = row["SubUnit"] == DBNull.Value ? "" : Convert.ToString(row["SubUnit"]);
        //                        string Unit = row["Unit"] == DBNull.Value ? "" : Convert.ToString(row["Unit"]);
        //                        string Technology = row["Technology"] == DBNull.Value ? "" : Convert.ToString(row["Technology"]);
        //                        string ServiceOffering = row["ServiceOffering"] == DBNull.Value ? "" : Convert.ToString(row["ServiceOffering"]);
        //                        string ServiceOfferingGroup = row["ServiceOfferingGroup"] == DBNull.Value ? "" : Convert.ToString(row["ServiceOfferingGroup"]);
        //                        string ProjectType = row["ProjectType"] == DBNull.Value ? "" : Convert.ToString(row["ProjectType"]);
        //                        string ProductionInCharge = row["ProductionInCharge"] == DBNull.Value ? "" : Convert.ToString(row["ProductionInCharge"]);
        //                        string CustomerName = row["CustomerName"] == DBNull.Value ? "" : Convert.ToString(row["CustomerName"]);
        //                        string CustomerCode = row["CustomerCode"] == DBNull.Value ? "" : Convert.ToString(row["CustomerCode"]);
        //                        string MasterCustomerCode = row["MasterCustomerCode"] == DBNull.Value ? "" : Convert.ToString(row["MasterCustomerCode"]);
        //                        string CustomerPortfolio = row["CustomerPortfolio"] == DBNull.Value ? "" : Convert.ToString(row["CustomerPortfolio"]);
        //                        string ContractType = row["ContractType"] == DBNull.Value ? "" : Convert.ToString(row["ContractType"]);
        //                        string Location = row["Location"] == DBNull.Value ? "" : Convert.ToString(row["Location"]);



        //                        Nullable<double> OnSiteLT = row["OnSiteLT"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OnSiteLT"]);
        //                        Nullable<double> OnSiteST = row["OnSiteST"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OnSiteST"]);
        //                        Nullable<double> BenchON = row["BenchON"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["BenchON"]);
        //                        Nullable<double> BenchOFF = row["BenchOFF"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["BenchOFF"]);

        //                        string MasterCustIBU = row["MasterCustIBU"] == DBNull.Value ? "" : Convert.ToString(row["MasterCustIBU"]);

        //                        Nullable<double> InvestmentCost = row["InvestmentCost"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["InvestmentCost"]);
        //                        Nullable<double> OnsiteBilledTM = row["OnsiteBilledTM"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OnsiteBilledTM"]);
        //                        Nullable<double> OffShoreBilledTM = row["OffShoreBilledTM"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OffShoreBilledTM"]);
        //                        Nullable<double> OnsiteBenchTM = row["OnsiteBenchTM"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OnsiteBenchTM"]);
        //                        Nullable<double> OffshoreBenchTM = row["OffshoreBenchTM"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OffshoreBenchTM"]);
        //                        Nullable<double> OnsiteBufferTM = row["OnsiteBufferTM"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OnsiteBufferTM"]);
        //                        Nullable<double> OffshoreBufferTM = row["OffshoreBufferTM"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OffshoreBufferTM"]);
        //                        Nullable<double> OperationsCost = row["OperationsCost"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["OperationsCost"]);
        //                        Nullable<double> ForexIncomeLosses = row["ForexIncomeLosses"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["ForexIncomeLosses"]);

        //                        string ProgramCode = row["ProgramCode"] == DBNull.Value ? "" : Convert.ToString(row["ProgramCode"]);
        //                        string TrackCode = row["TrackCode"] == DBNull.Value ? "" : Convert.ToString(row["TrackCode"]);
        //                        string Region = row["Region"] == DBNull.Value ? "" : Convert.ToString(row["Region"]);
        //                        string ProjectCurrency = row["ProjectCurrency"] == DBNull.Value ? "" : Convert.ToString(row["ProjectCurrency"]);
        //                        string CustomerGeography = row["CustomerGeography"] == DBNull.Value ? "" : Convert.ToString(row["CustomerGeography"]);
        //                        string Company = row["Company"] == DBNull.Value ? "" : Convert.ToString(row["Company"]);
        //                        string ProjectPU = row["ProjectPU"] == DBNull.Value ? "" : Convert.ToString(row["ProjectPU"]);
        //                        string STPCategory = row["STPCategory"] == DBNull.Value ? "" : Convert.ToString(row["STPCategory"]);
        //                        string IndustryCode = row["IndustryCode"] == DBNull.Value ? "" : Convert.ToString(row["IndustryCode"]);
        //                        string IndustrySubCode = row["IndustrySubCode"] == DBNull.Value ? "" : Convert.ToString(row["IndustrySubCode"]);
        //                        string TBB = row["TBB"] == DBNull.Value ? "" : Convert.ToString(row["TBB"]);
        //                        string ProjectName = row["ProjectName"] == DBNull.Value ? "" : Convert.ToString(row["ProjectName"]);
        //                        string IBUVertical = row["IBUVertical"] == DBNull.Value ? "" : Convert.ToString(row["IBUVertical"]);
        //                        string BudgetingUnit = row["BudgetingUnit"] == DBNull.Value ? "" : Convert.ToString(row["BudgetingUnit"]);
        //                        string RegionGroup = row["RegionGroup"] == DBNull.Value ? "" : Convert.ToString(row["RegionGroup"]);
        //                        string ServiceLine = row["ServiceLine"] == DBNull.Value ? "" : Convert.ToString(row["ServiceLine"]);
        //                        string PracticeLine = row["PracticeLine"] == DBNull.Value ? "" : Convert.ToString(row["PracticeLine"]);
        //                        string DeliverySubUnit = row["DeliverySubUnit"] == DBNull.Value ? "" : Convert.ToString(row["DeliverySubUnit"]);
        //                        string GroupMasterProjectCode = row["GroupMasterProjectCode"] == DBNull.Value ? "" : Convert.ToString(row["GroupMasterProjectCode"]);




        //                        service.InsertFinPulseDumpProd(ProjectCode, MasterProject, ProjectGeography, DU,
        //                        YearMonth, OnSiteRevenue, OffShoreRevenue, TotalRevenue,
        //                        OnsiteCostofRevenueDirect, OffShoreCostofRevenueDirect, TotalCostofRevenueDirect, OnsiteProjectMargin,
        //                        OffshoreProjectMargin, ProjectMargin, TotalCostofRevenueAllocated, PUDelyMargin,
        //                        TotalCostofRevenueOtherCosts, GrossMargin, TotalSGADirect, TotalSellingAllocated,
        //                        TotalGAAllocated, PBTBeforeInvestment, PBTAfterInvestmentForexLosses, TotalTaxesExcludingIndiaTax,
        //                        PATBeforeIndiaTax, IndiaTax, PATAfterIndiaTax, TotalExpense,
        //                        OnSiteBilledMonths, OffShoreBilledMonths, TotalBilledMonths, BenchMonths,
        //                        TotalBillableMonths, RDMonths, SolutionMonths, OverheadMonths, TrainingMonths, Leave,
        //                        OnsiteRDMonths, OffshoreRDMonths, OnsiteSolutionMonths, OffshoreSolutionMonths, OnsiteOverheads,
        //                        OffshoreOverheads, OnsiteTraining, OffshoreTraining, OnsiteLeave,
        //                        OffshoreLeave, Buffer, OnsiteBuffer, OffshoreBuffer,
        //                        TotalPersonMonths, Support, OnsiteSupport, OffshoreSupport,
        //                        ReportingPU, SBUPUGroup, SubUnit, Unit,
        //                        Technology, ServiceOffering, ServiceOfferingGroup, ProjectType, ProductionInCharge, CustomerName,
        //                        CustomerCode, MasterCustomerCode, CustomerPortfolio, ContractType, Location, OnSiteLT,
        //                        OnSiteST, BenchON, BenchOFF, MasterCustIBU, InvestmentCost, OnsiteBilledTM,
        //                        OffShoreBilledTM, OnsiteBenchTM, OffshoreBenchTM, OnsiteBufferTM,
        //                        OffshoreBufferTM, OperationsCost, ForexIncomeLosses, ProgramCode,
        //                        TrackCode, Region, ProjectCurrency, CustomerGeography, Company, ProjectPU, STPCategory,
        //                        IndustryCode, IndustrySubCode, TBB, ProjectName, IBUVertical, BudgetingUnit, RegionGroup,
        //                        ServiceLine, PracticeLine, DeliverySubUnit, GroupMasterProjectCode, drpSUProd.Text, drpYerProd.Text);
        //                        rowsupdated++;
        //                        //}
        //                    }

        //                    Session["FileName"] = path;
        //                    string ExcelFilePath = Session["FileName"].ToString();

        //                    service.UpdateFinPulseYearMonthProd(drpYerProd.Text.Trim(), drpSUProd.Text.Trim());

        //                    //service.UpdateBaseDatafromFinPulProd();
        //                    lblSuccess.Text = "Data Uploaded Successfully";
        //                    lblSuccess.Visible = true;
        //                    lblError.Visible = false;
        //                    //}

        //                    //Code to check the count in Excel and Database
        //                    int cnt = 0;
        //                    cnt = service.FinPulseDumpCountProd();

        //                    lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in Excel : " + fontRed + noOfRows + fontEnd +
        //                        " -No. of records in the table after upload :" + fontRed + cnt;

        //                    //----------END

        //                    service.UpdateDmMailIdFinPulProd();

        //                    ClientScript.RegisterStartupScript(Page.GetType(), "PopUpMasterClientFinpulseProd", "javascript:PopUpMasterClientFinpulseProd();", true);
        //                }
        //                else
        //                {
        //                    if (con.State == ConnectionState.Open)
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename sheet to 'Finpulse'";
        //                    lblError.Visible = true;
        //                }
        //            }
        //            else
        //            {
        //                if (con.State == ConnectionState.Open)
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";
        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State == ConnectionState.Open)
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }

        //    }

        //    if (con.State == ConnectionState.Open)
        //        con.Close();

        //}


        // Code for Production - RTBR
        //protected void btnRtbrUploadProd_Click(object sender, EventArgs e)
        //{

        //    System.Data.DataTable dtExcel = new System.Data.DataTable();

        //    dtExcel.TableName = "MyExcelData";

        //    System.Data.DataTable dtExcel1 = new System.Data.DataTable();

        //    dtExcel1.TableName = "MyExcelData";

        //    string folder = "ExcelOperationsProd";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "RTBR.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\RTBR.xlsx");

        //    string path = MyDir.FullName + "\\RTBR.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {
        //        string FileName = RTBRUploadProd.FileName;

        //        if (RTBRUploadProd.HasFile)
        //        {
        //            //System.Data.DataTable dtExcel = new System.Data.DataTable();

        //            //dtExcel.TableName = "MyExcelData";

        //            //string folder = "ExcelOperations";
        //            //var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //            //if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "ProjectCode.xlsx") != null)
        //            //System.IO.File.Delete(MyDir.FullName + "\\ProjectCode.xlsx");
        //            if (FileName.Contains(".xls"))
        //            {
        //                //string path = MyDir.FullName + "\\ProjectCode.xlsx";// + FileName;
        //                //string filename = Path.GetFileName(FileUpload1.FileName);
        //                RTBRUploadProd.SaveAs(path);

        //                //string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //                //OleDbConnection con = new OleDbConnection(SourceConstr);

        //                string query = "Select * from [USD$]";
        //                string query1 = "Select * from [NC$]";

        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("USD$"))
        //                {


        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    bool isSuccess;

        //                    isSuccess = service.DeleteRTBRDumpProd();

        //                    //if (isSuccess)
        //                    //{

        //                    data.Fill(dtExcel);

        //                    int noOfRows = dtExcel.Rows.Count;
        //                    int rowsupdated = 0;

        //                    foreach (DataRow row in dtExcel.Rows)
        //                    {
        //                        Nullable<int> nullableInt = null;
        //                        Nullable<DateTime> nullableDate = null;
        //                        Nullable<double> nullableDouble = null;

        //                        string ProjectCode = row["Project Code"] == DBNull.Value ? "" : Convert.ToString(row["Project Code"]);
        //                        Nullable<DateTime> ProjectStartDate = row["Project Start Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Project Start Date"]);
        //                        Nullable<DateTime> ProjectEnddate = row["Project End date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Project End date"]);
        //                        string ProjectCurrency = row["Project Currency"] == DBNull.Value ? "" : Convert.ToString(row["Project Currency"]);
        //                        string ProjectType = row["Project Type"] == DBNull.Value ? "" : Convert.ToString(row["Project Type"]);
        //                        string TBBEnabled = row["TBB Enabled"] == DBNull.Value ? "" : Convert.ToString(row["TBB Enabled"]);
        //                        string ServiceOffering = row["Service Offering"] == DBNull.Value ? "" : Convert.ToString(row["Service Offering"]);
        //                        string ClientCode = row["Client Code"] == DBNull.Value ? "" : Convert.ToString(row["Client Code"]);
        //                        string MasterClientCode = row["Master Client Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Client Code"]);
        //                        string SUCode = row["SU Code"] == DBNull.Value ? "" : Convert.ToString(row["SU Code"]);
        //                        string IBUCode = row["IBU Code"] == DBNull.Value ? "" : Convert.ToString(row["IBU Code"]);
        //                        string CUCode = row["CU Code"] == DBNull.Value ? "" : Convert.ToString(row["CU Code"]);
        //                        string PMMailID = row["PM Mail ID"] == DBNull.Value ? "" : Convert.ToString(row["PM Mail ID"]);
        //                        string DMMailID = row["DM Mail ID"] == DBNull.Value ? "" : Convert.ToString(row["DM Mail ID"]);
        //                        string MasterPUCode = row["Master PU Code"] == DBNull.Value ? "" : Convert.ToString(row["Master PU Code"]);
        //                        string CustomerPortfolio = row["Customer Portfolio"] == DBNull.Value ? "" : Convert.ToString(row["Customer Portfolio"]);



        //                        Nullable<double> FinYearEnd = row["Fin Year End"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Fin Year End"]);

        //                        string ParticipatingPU = row["Participating PU"] == DBNull.Value ? "" : Convert.ToString(row["Participating PU"]);
        //                        string ParticipatingCompany = row["Participating Company"] == DBNull.Value ? "" : Convert.ToString(row["Participating Company"]);

        //                        Nullable<double> AprValue = row["Apr Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Apr Value"]);

        //                        Nullable<double> MayValue = row["May Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["May Value"]);
        //                        Nullable<double> JunValue = row["Jun Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Jun Value"]);
        //                        Nullable<double> Q1Total = row["Q1 Total"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Q1 Total"]);
        //                        Nullable<double> JulValue = row["Jul Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Jul Value"]);
        //                        Nullable<double> AugValue = row["Aug Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Aug Value"]);
        //                        Nullable<double> SepValue = row["Sep Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Sep Value"]);
        //                        Nullable<double> Q2Total = row["Q2 Total"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Q2 Total"]);
        //                        Nullable<double> OctValue = row["Oct Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Oct Value"]);
        //                        Nullable<double> NovValue = row["Nov Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Nov Value"]);
        //                        Nullable<double> DecValue = row["Dec Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Dec Value"]);
        //                        Nullable<double> Q3Total = row["Q3 Total"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Q3 Total"]);
        //                        Nullable<double> JanValue = row["Jan Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Jan Value"]);
        //                        Nullable<double> FebValue = row["Feb Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Feb Value"]);
        //                        Nullable<double> MarValue = row["Mar Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Mar Value"]);
        //                        Nullable<double> Q4Total = row["Q4 Total"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Q4 Total"]);
        //                        Nullable<double> AnnualRTBR = row["Annual RTBR"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Annual RTBR"]);

        //                        string CompanyCode = row["Company Code"] == DBNull.Value ? "" : Convert.ToString(row["Company Code"]);
        //                        string ConsultingInvolved = row["Consulting Involved?"] == DBNull.Value ? "" : Convert.ToString(row["Consulting Involved?"]);



        //                        service.InsertRTBRDumpProd(ProjectCode, ProjectStartDate, ProjectEnddate, ProjectCurrency,
        //                        ProjectType, TBBEnabled, ServiceOffering, ClientCode,
        //                        MasterClientCode, SUCode, IBUCode, CUCode, PMMailID, DMMailID, MasterPUCode, CustomerPortfolio,
        //                        FinYearEnd, ParticipatingPU, ParticipatingCompany, AprValue, MayValue, JunValue, Q1Total, JulValue,
        //                        AugValue, SepValue, Q2Total, OctValue, NovValue, DecValue, Q3Total, JanValue,
        //                        FebValue, MarValue, Q4Total, AnnualRTBR, CompanyCode, ConsultingInvolved);
        //                        rowsupdated++;
        //                    }
        //                    //}


        //                    //Sheet2
        //                    OleDbDataAdapter data1 = new OleDbDataAdapter(query1, con);

        //                    bool isSuccess1;

        //                    isSuccess1 = service.DeleteRTBRDumpProdNC();

        //                    //if (isSuccess)
        //                    //{

        //                    data1.Fill(dtExcel1);

        //                    int noOfRows1 = dtExcel1.Rows.Count;
        //                    int rowsupdated1 = 0;

        //                    foreach (DataRow row in dtExcel1.Rows)
        //                    {
        //                        Nullable<int> nullableInt = null;
        //                        Nullable<DateTime> nullableDate = null;
        //                        Nullable<double> nullableDouble = null;

        //                        string ProjectCode = row["Project Code"] == DBNull.Value ? "" : Convert.ToString(row["Project Code"]);
        //                        Nullable<DateTime> ProjectStartDate = row["Project Start Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Project Start Date"]);
        //                        Nullable<DateTime> ProjectEnddate = row["Project End date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Project End date"]);
        //                        string ProjectCurrency = row["Project Currency"] == DBNull.Value ? "" : Convert.ToString(row["Project Currency"]);
        //                        string ProjectType = row["Project Type"] == DBNull.Value ? "" : Convert.ToString(row["Project Type"]);
        //                        string TBBEnabled = row["TBB Enabled"] == DBNull.Value ? "" : Convert.ToString(row["TBB Enabled"]);
        //                        string ServiceOffering = row["Service Offering"] == DBNull.Value ? "" : Convert.ToString(row["Service Offering"]);
        //                        string ClientCode = row["Client Code"] == DBNull.Value ? "" : Convert.ToString(row["Client Code"]);
        //                        string MasterClientCode = row["Master Client Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Client Code"]);
        //                        string SUCode = row["SU Code"] == DBNull.Value ? "" : Convert.ToString(row["SU Code"]);
        //                        string IBUCode = row["IBU Code"] == DBNull.Value ? "" : Convert.ToString(row["IBU Code"]);
        //                        string CUCode = row["CU Code"] == DBNull.Value ? "" : Convert.ToString(row["CU Code"]);
        //                        string PMMailID = row["PM Mail ID"] == DBNull.Value ? "" : Convert.ToString(row["PM Mail ID"]);
        //                        string DMMailID = row["DM Mail ID"] == DBNull.Value ? "" : Convert.ToString(row["DM Mail ID"]);
        //                        string MasterPUCode = row["Master PU Code"] == DBNull.Value ? "" : Convert.ToString(row["Master PU Code"]);
        //                        string CustomerPortfolio = row["Customer Portfolio"] == DBNull.Value ? "" : Convert.ToString(row["Customer Portfolio"]);

        //                        Nullable<double> FinYearEnd = row["Fin Year End"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Fin Year End"]);

        //                        string ParticipatingPU = row["Participating PU"] == DBNull.Value ? "" : Convert.ToString(row["Participating PU"]);
        //                        string ParticipatingCompany = row["Participating Company"] == DBNull.Value ? "" : Convert.ToString(row["Participating Company"]);

        //                        Nullable<double> AprValue = row["Apr Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Apr Value"]);
        //                        Nullable<double> MayValue = row["May Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["May Value"]);
        //                        Nullable<double> JunValue = row["Jun Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Jun Value"]);
        //                        Nullable<double> Q1Total = row["Q1 Total"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Q1 Total"]);
        //                        Nullable<double> JulValue = row["Jul Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Jul Value"]);
        //                        Nullable<double> AugValue = row["Aug Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Aug Value"]);
        //                        Nullable<double> SepValue = row["Sep Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Sep Value"]);
        //                        Nullable<double> Q2Total = row["Q2 Total"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Q2 Total"]);
        //                        Nullable<double> OctValue = row["Oct Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Oct Value"]);
        //                        Nullable<double> NovValue = row["Nov Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Nov Value"]);
        //                        Nullable<double> DecValue = row["Dec Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Dec Value"]);
        //                        Nullable<double> Q3Total = row["Q3 Total"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Q3 Total"]);
        //                        Nullable<double> JanValue = row["Jan Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Jan Value"]);
        //                        Nullable<double> FebValue = row["Feb Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Feb Value"]);
        //                        Nullable<double> MarValue = row["Mar Value"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Mar Value"]);
        //                        Nullable<double> Q4Total = row["Q4 Total"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Q4 Total"]);
        //                        Nullable<double> AnnualRTBR = row["Annual RTBR"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Annual RTBR"]);

        //                        string CompanyCode = row["Company Code"] == DBNull.Value ? "" : Convert.ToString(row["Company Code"]);
        //                        string ConsultingInvolved = row["Consulting Involved?"] == DBNull.Value ? "" : Convert.ToString(row["Consulting Involved?"]);

        //                        service.InsertRTBRDumpProdNC(ProjectCode, ProjectStartDate, ProjectEnddate, ProjectCurrency,
        //                        ProjectType, TBBEnabled, ServiceOffering, ClientCode,
        //                        MasterClientCode, SUCode, IBUCode, CUCode, PMMailID, DMMailID, MasterPUCode, CustomerPortfolio,
        //                        FinYearEnd, ParticipatingPU, ParticipatingCompany, AprValue, MayValue, JunValue, Q1Total, JulValue,
        //                        AugValue, SepValue, Q2Total, OctValue, NovValue, DecValue, Q3Total, JanValue,
        //                        FebValue, MarValue, Q4Total, AnnualRTBR, CompanyCode, ConsultingInvolved);
        //                        rowsupdated1++;
        //                    }
        //                    //}




        //                    Session["FileName"] = path;
        //                    string ExcelFilePath = Session["FileName"].ToString();
        //                    //service.UpdateBaseDatafromRTBRProd();
        //                    //lblSuccess.Text = "Data Uploaded Successfully";




        //                    //Code to check the count in Excel and Database
        //                    int cnt = 0;
        //                    cnt = service.RTBRDumpCountProd();

        //                    lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in Excel : " + fontRed + noOfRows + fontEnd +
        //                        " -No. of records in the table after upload :" + fontRed + cnt;

        //                    //----------END

        //                    ClientScript.RegisterStartupScript(Page.GetType(), "PopUpMasterClientListProd", "javascript:PopUpMasterClientListProd();", true);
        //                    lblSuccess.Visible = true;
        //                    lblError.Visible = false;
        //                    //}
        //                }
        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename sheet to 'RTBR'";
        //                    lblError.Visible = true;
        //                }
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";
        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File ";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }

        //    }
        //}


        //Dev Code For BE MCO
        //protected void btnMCO_Click(object sender, EventArgs e)
        //{
        //    System.Data.DataTable dtExcel = new System.Data.DataTable();

        //    // Alert();

        //    dtExcel.TableName = "MyExcelData";


        //    System.Data.DataTable dtExcel1 = new System.Data.DataTable();

        //    dtExcel1.TableName = "MyExcelData";

        //    string folder = "ExcelOperations";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "BEMCO.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\BEMCO.xlsx");

        //    string path = MyDir.FullName + "\\BEMCO.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    string SourceConstrxls = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";



        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    OleDbConnection conxls = new OleDbConnection(SourceConstrxls);

        //    try
        //    {
        //        int noOfRows = 0;
        //        int noOfRows1 = 0;
        //        string FileName = MCOUpload.FileName;

        //        string FileName1 = MCOUploadBE.FileName;


        //        if (MCOUpload.FileName == "" || MCOUploadBE.FileName == "")
        //        {
        //            //lblError.Text = "";
        //            //lblError.Text = "Please Select both the MCOBE Delivery Share Report and DH BE Report and then upload";
        //            //lblError.Visible = true;
        //            //lblSuccess.Visible = false;

        //            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Please Select both the MCOBE Delivery Share Report and DH BE Report and then upload');</script>", false);
        //            return;

        //        }
        //        if (MCOUpload.HasFile)
        //        {

        //            if (FileName.Contains(".xls"))
        //            {

        //                MCOUpload.SaveAs(path);

        //                //string query = "Select * from [BE_MCO_USD$]";

        //                //Code to check if sheet is having proper name

        //                if (FileName.Contains(".xlsx"))
        //                    con.Open();
        //                else
        //                    conxls.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                //string staringqtr=string.Empty;
        //                //int curyr = Convert.ToInt32(drpYear.Text);
        //                //staringqtr = drpQtr.Text + "'" + (curyr - 2000);

        //                if (lstsheetNames.Contains("MCOBEDeliveryShareReport$"))
        //                {
        //                    string query = "Select * from [MCOBEDeliveryShareReport$]";
        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    bool isSuccess;

        //                    isSuccess = service.DeleteBEMCODump();

        //                    data.Fill(dtExcel);

        //                    dtExcel.DefaultView.RowFilter = "[Unit] IS NOT NULL";
        //                    dtExcel = dtExcel.DefaultView.ToTable();


        //                    noOfRows = dtExcel.Rows.Count;
        //                    int rowsupdated = 0;

        //                    foreach (DataRow row in dtExcel.Rows)
        //                    {
        //                        Nullable<int> nullableInt = null;
        //                        Nullable<DateTime> nullableDate = null;
        //                        Nullable<double> nullableDouble = null;

        //                        string Unit = row["Unit"] == DBNull.Value ? "" : Convert.ToString(row["Unit"]);
        //                        string MasterClientCode = row["Master Client Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Client Code"]);
        //                        string MasterClientName = row["Master Client Name"] == DBNull.Value ? "" : Convert.ToString(row["Master Client Name"]);
        //                        string PU = row[6] == DBNull.Value ? "" : Convert.ToString(row[6]);

        //                        Nullable<double> CurqtrMCO = row[7] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[7]);
        //                        Nullable<double> CurqtrDHBe = row[10] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[10]);
        //                        Nullable<double> NextqtrMCO = row[13] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[13]);
        //                        Nullable<double> NextqtrDHBE = row[16] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[16]);
        //                        Nullable<double> Nextqtr1MCO = row[19] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[19]);
        //                        Nullable<double> Nextqtr1DHBE = row[22] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[22]);
        //                        Nullable<double> Nextqtr2MCO = row[25] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[25]);
        //                        Nullable<double> Nextqtr2DHBE = row[28] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[28]);
        //                        string Qtr = drpQtr.Text;
        //                        string Year = drpYear.Text;

        //                        service.InsertBEMCODump(Unit, MasterClientCode, MasterClientName, PU,
        //                        CurqtrMCO, CurqtrDHBe, NextqtrMCO, NextqtrDHBE,
        //                        Nextqtr1MCO, Nextqtr1DHBE, Nextqtr2MCO, Nextqtr2DHBE, Qtr, Year);
        //                        rowsupdated++;
        //                    }


        //                    // service.InsertBEMCO(drpQtr.Text, drpYear.Text);


        //                }

        //                else if (lstsheetNames.Contains("DHBEReport$"))
        //                {
        //                    lblError.Text = "Please choose/select MCOBEDeliveryShareReport first and DHBEReport next";
        //                    lblError.Visible = true;
        //                }






        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename the sheet to MCOBEDeliveryShareReport";
        //                    lblError.Visible = true;
        //                }

        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                //lblSuccess.Text = "Data Uploaded Successfully";

        //                //ClientScript.RegisterStartupScript(Page.GetType(), "PopUpMasterClientList", "javascript:PopUpMasterClientList();", true);
        //                //lblSuccess.Visible = true;
        //                //lblError.Visible = false;
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";
        //        }

        //        /////////////////////////mco 


        //        if (MCOUploadBE.HasFile)
        //        {

        //            if (FileName1.Contains(".xls"))
        //            {

        //                MCOUploadBE.SaveAs(path);

        //                //string query = "Select * from [BE_MCO_USD$]";




        //                //Code to check if sheet is having proper name
        //                if (FileName1.Contains(".xlsx"))
        //                    con.Open();
        //                else
        //                    conxls.Open();
        //                DataTable worksheets1 = con.GetSchema("Tables");
        //                string w1 = worksheets1.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames1 = new List<string>();
        //                Action<DataRow> actionToGetSheetName1 = (k) => { lstsheetNames1.Add(k["TABLE_NAME"] + ""); };

        //                worksheets1.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName1);

        //                //string staringqtr=string.Empty;
        //                //int curyr = Convert.ToInt32(drpYear.Text);
        //                //staringqtr = drpQtr.Text + "'" + (curyr - 2000);

        //                if (lstsheetNames1.Contains("DHBEReport$"))
        //                {
        //                    string query1 = "Select * from [DHBEReport$]";
        //                    //OleDbDataAdapter data = new OleDbDataAdapter(query, con);



        //                    OleDbDataAdapter data1 = new OleDbDataAdapter(query1, con);

        //                    bool isSuccess1;

        //                    isSuccess1 = service.DeleteBEMCODumpNC();

        //                    data1.Fill(dtExcel1);

        //                    //int noOfRows1 = dtExcel1.Rows.Count;
        //                    dtExcel1.DefaultView.RowFilter = "[Unit] IS NOT NULL";
        //                    dtExcel1 = dtExcel1.DefaultView.ToTable();

        //                    noOfRows1 = dtExcel1.Rows.Count;
        //                    int rowsupdated1 = 0;

        //                    foreach (DataRow row in dtExcel1.Rows)
        //                    {
        //                        Nullable<int> nullableInt = null;
        //                        Nullable<DateTime> nullableDate = null;
        //                        Nullable<double> nullableDouble = null;

        //                        string Unit = row["Unit"] == DBNull.Value ? "" : Convert.ToString(row["Unit"]);
        //                        string MasterClientCode = row["Master Client Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Client Code"]);
        //                        string MasterClientName = row["Master Client Name"] == DBNull.Value ? "" : Convert.ToString(row["Master Client Name"]);
        //                        string PU = row[4] == DBNull.Value ? "" : Convert.ToString(row[4]);
        //                        string Currency = row[5] == DBNull.Value ? "" : Convert.ToString(row[5]);


        //                        Nullable<double> CurqtrDHBe = row[6] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[6]);
        //                        Nullable<double> NextqtrDHBE = row[10] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[10]);
        //                        Nullable<double> Nextqtr1DHBE = row[14] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[14]);
        //                        Nullable<double> Nextqtr2DHBE = row[18] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[18]);
        //                        Nullable<double> CurqtrDHBeUSD = row[7] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[7]);
        //                        Nullable<double> NextqtrDHBEUSD = row[11] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[11]);
        //                        Nullable<double> Nextqtr1DHBEUSD = row[15] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[15]);
        //                        Nullable<double> Nextqtr2DHBEUSD = row[19] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[19]);
        //                        string Qtr = drpQtr.Text;
        //                        string Year = drpYear.Text;

        //                        //string StartingQtr = staringqtr;

        //                        service.InsertBEMCODumpNC(Unit, MasterClientCode, MasterClientName, PU,
        //                        Currency, CurqtrDHBe, NextqtrDHBE, Nextqtr1DHBE,
        //                        Nextqtr2DHBE, Qtr, Year, CurqtrDHBeUSD, NextqtrDHBEUSD, Nextqtr1DHBEUSD, Nextqtr2DHBEUSD);
        //                        rowsupdated1++;
        //                    }
        //                    //}

        //                    Session["FileName"] = path;
        //                    string ExcelFilePath = Session["FileName"].ToString();






        //                    service.InsertBEMCONC(drpQtr.Text, drpYear.Text);
        //                    service.InsertBEMCO(drpQtr.Text, drpYear.Text);


        //                    //if (con.State.ToString().ToLower() == "open")
        //                    //    con.Close();
        //                    //lblSuccess.Text = "Data Uploaded Successfully";


        //                    ////ClientScript.RegisterStartupScript(Page.GetType(), "PopUpMasterClientList", "javascript:PopUpMasterClientList();", true);
        //                    //lblSuccess.Visible = true;
        //                    //lblError.Visible = false;


        //                }


        //                else if (lstsheetNames1.Contains("MCOBEDeliveryShareReport$"))
        //                {
        //                    lblError.Text = "Please choose/select MCOBEDeliveryShareReport first and DHBEReport next";
        //                    lblError.Visible = true;
        //                }



        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename the sheet to DHBEReport";
        //                    lblError.Visible = true;
        //                }

        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //            }

        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";


        //        }





        //        //if (MCOUpload.FileName == "" && MCOUploadBE.FileName == "")
        //        //{
        //        //    lblError.Text = "";
        //        //    lblError.Text = "Please Select a File";
        //        //    lblError.Visible = true;
        //        //    lblSuccess.Visible = false;
        //        //}
        //        if (dtExcel.Rows.Count > 0 || dtExcel1.Rows.Count > 0)
        //        {
        //            int cnt = 0;
        //            cnt = service.MCOCountDev();

        //            int count = 0;
        //            count = service.MCODumpCountDev();

        //            lblSuccess.Visible = true;
        //            lblError.Visible = false;

        //            lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in MCO BE Excel : " + fontRed + noOfRows + fontEnd + " , DH BE Excel : " +
        //                fontRed + noOfRows1 + fontEnd +
        //               " -No. of records in the table after upload :" + fontRed + count + fontEnd + " -No. or records in the main table after upload: " + fontRed + cnt;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }

        //    }
        //}


        //Production Code BE MCO
        //protected void btnMCOProd_Click(object sender, EventArgs e)
        //{
        //    System.Data.DataTable dtExcel = new System.Data.DataTable();

        //    // Alert();

        //    dtExcel.TableName = "MyExcelData";


        //    System.Data.DataTable dtExcel1 = new System.Data.DataTable();

        //    dtExcel1.TableName = "MyExcelData";

        //    string folder = "ExcelOperations";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "BEMCO.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\BEMCO.xlsx");

        //    string path = MyDir.FullName + "\\BEMCO.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";



        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {
        //        int noOfRows = 0;
        //        int noOfRows1 = 0;
        //        string FileName = MCOUploadProd.FileName;

        //        string FileName1 = MCOUploadBEProd.FileName;



        //        if (MCOUploadProd.FileName == "" || MCOUploadBEProd.FileName == "")
        //        {
        //            //lblError.Text = "";
        //            //lblError.Text = "Please Select both the MCOBE Delivery Share Report and DH BE Report and then upload";
        //            //lblError.Visible = true;
        //            //lblSuccess.Visible = false;

        //            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Please Select both the MCOBE Delivery Share Report and DH BE Report and then upload');</script>", false);
        //            return;

        //        }


        //        if (MCOUploadProd.HasFile)
        //        {

        //            if (FileName.Contains(".xls"))
        //            {

        //                MCOUploadProd.SaveAs(path);

        //                //string query = "Select * from [BE_MCO_USD$]";

        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                //string staringqtr=string.Empty;
        //                //int curyr = Convert.ToInt32(drpYear.Text);
        //                //staringqtr = drpQtr.Text + "'" + (curyr - 2000);

        //                if (lstsheetNames.Contains("MCOBEDeliveryShareReport$"))
        //                {
        //                    string query = "Select * from [MCOBEDeliveryShareReport$]";
        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    bool isSuccess;

        //                    isSuccess = service.DeleteBEMCODumpProd();

        //                    data.Fill(dtExcel);

        //                    dtExcel.DefaultView.RowFilter = "[Unit] IS NOT NULL";
        //                    dtExcel = dtExcel.DefaultView.ToTable();


        //                    noOfRows = dtExcel.Rows.Count;
        //                    int rowsupdated = 0;

        //                    foreach (DataRow row in dtExcel.Rows)
        //                    {
        //                        Nullable<int> nullableInt = null;
        //                        Nullable<DateTime> nullableDate = null;
        //                        Nullable<double> nullableDouble = null;

        //                        string Unit = row["Unit"] == DBNull.Value ? "" : Convert.ToString(row["Unit"]);
        //                        string MasterClientCode = row["Master Client Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Client Code"]);
        //                        string MasterClientName = row["Master Client Name"] == DBNull.Value ? "" : Convert.ToString(row["Master Client Name"]);
        //                        string PU = row[6] == DBNull.Value ? "" : Convert.ToString(row[6]);

        //                        Nullable<double> CurqtrMCO = row[7] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[7]);
        //                        Nullable<double> CurqtrDHBe = row[10] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[10]);
        //                        Nullable<double> NextqtrMCO = row[13] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[13]);
        //                        Nullable<double> NextqtrDHBE = row[16] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[16]);
        //                        Nullable<double> Nextqtr1MCO = row[19] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[19]);
        //                        Nullable<double> Nextqtr1DHBE = row[22] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[22]);
        //                        Nullable<double> Nextqtr2MCO = row[25] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[25]);
        //                        Nullable<double> Nextqtr2DHBE = row[28] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[28]);
        //                        string Qtr = drpQtr.Text;
        //                        string Year = drpYear.Text;

        //                        service.InsertBEMCODumpProd(Unit, MasterClientCode, MasterClientName, PU,
        //                        CurqtrMCO, CurqtrDHBe, NextqtrMCO, NextqtrDHBE,
        //                        Nextqtr1MCO, Nextqtr1DHBE, Nextqtr2MCO, Nextqtr2DHBE, Qtr, Year);
        //                        rowsupdated++;
        //                    }


        //                    // service.InsertBEMCOProd(drpQtr.Text, drpYear.Text);


        //                }


        //                else if (lstsheetNames.Contains("DHBEReport$"))
        //                {
        //                    lblError.Text = "Please choose/select MCOBEDeliveryShareReport first and DHBEReport next";
        //                    lblError.Visible = true;
        //                }






        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename the sheet to MCOBEDeliveryShareReport ";
        //                    lblError.Visible = true;
        //                }

        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                //lblSuccess.Text = "Data Uploaded Successfully";

        //                //ClientScript.RegisterStartupScript(Page.GetType(), "PopUpMasterClientList", "javascript:PopUpMasterClientList();", true);
        //                //lblSuccess.Visible = true;
        //                //lblError.Visible = false;
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";
        //        }

        //        /////////////////////////mco 


        //        if (MCOUploadBEProd.HasFile)
        //        {

        //            if (FileName1.Contains(".xls"))
        //            {

        //                MCOUploadBEProd.SaveAs(path);

        //                //string query = "Select * from [BE_MCO_USD$]";




        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets1 = con.GetSchema("Tables");
        //                string w1 = worksheets1.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames1 = new List<string>();
        //                Action<DataRow> actionToGetSheetName1 = (k) => { lstsheetNames1.Add(k["TABLE_NAME"] + ""); };

        //                worksheets1.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName1);

        //                //string staringqtr=string.Empty;
        //                //int curyr = Convert.ToInt32(drpYear.Text);
        //                //staringqtr = drpQtr.Text + "'" + (curyr - 2000);

        //                if (lstsheetNames1.Contains("DHBEReport$"))
        //                {
        //                    string query1 = "Select * from [DHBEReport$]";
        //                    //OleDbDataAdapter data = new OleDbDataAdapter(query, con);



        //                    OleDbDataAdapter data1 = new OleDbDataAdapter(query1, con);

        //                    bool isSuccess1;

        //                    isSuccess1 = service.DeleteBEMCODumpNCProd();

        //                    data1.Fill(dtExcel1);

        //                    //int noOfRows1 = dtExcel1.Rows.Count;
        //                    dtExcel1.DefaultView.RowFilter = "[Unit] IS NOT NULL";
        //                    dtExcel1 = dtExcel1.DefaultView.ToTable();

        //                    noOfRows1 = dtExcel1.Rows.Count;
        //                    int rowsupdated1 = 0;

        //                    foreach (DataRow row in dtExcel1.Rows)
        //                    {
        //                        Nullable<int> nullableInt = null;
        //                        Nullable<DateTime> nullableDate = null;
        //                        Nullable<double> nullableDouble = null;

        //                        string Unit = row["Unit"] == DBNull.Value ? "" : Convert.ToString(row["Unit"]);
        //                        string MasterClientCode = row["Master Client Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Client Code"]);
        //                        string MasterClientName = row["Master Client Name"] == DBNull.Value ? "" : Convert.ToString(row["Master Client Name"]);
        //                        string PU = row[4] == DBNull.Value ? "" : Convert.ToString(row[4]);
        //                        string Currency = row[5] == DBNull.Value ? "" : Convert.ToString(row[5]);


        //                        Nullable<double> CurqtrDHBe = row[6] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[6]);
        //                        Nullable<double> NextqtrDHBE = row[10] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[10]);
        //                        Nullable<double> Nextqtr1DHBE = row[14] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[14]);
        //                        Nullable<double> Nextqtr2DHBE = row[18] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[18]);
        //                        Nullable<double> CurqtrDHBeUSD = row[7] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[7]);
        //                        Nullable<double> NextqtrDHBEUSD = row[11] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[11]);
        //                        Nullable<double> Nextqtr1DHBEUSD = row[15] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[15]);
        //                        Nullable<double> Nextqtr2DHBEUSD = row[19] == DBNull.Value ? nullableDouble : Convert.ToDouble(row[19]);
        //                        string Qtr = drpQtr.Text;
        //                        string Year = drpYear.Text;

        //                        //string StartingQtr = staringqtr;

        //                        service.InsertBEMCODumpNCProd(Unit, MasterClientCode, MasterClientName, PU,
        //                        Currency, CurqtrDHBe, NextqtrDHBE, Nextqtr1DHBE,
        //                        Nextqtr2DHBE, Qtr, Year, CurqtrDHBeUSD, NextqtrDHBEUSD, Nextqtr1DHBEUSD, Nextqtr2DHBEUSD);
        //                        rowsupdated1++;
        //                    }
        //                    //}

        //                    Session["FileName"] = path;
        //                    string ExcelFilePath = Session["FileName"].ToString();






        //                    service.InsertBEMCONCProd(drpQtr.Text, drpYear.Text);
        //                    service.InsertBEMCOProd(drpQtr.Text, drpYear.Text);


        //                    //if (con.State.ToString().ToLower() == "open")
        //                    //    con.Close();
        //                    //lblSuccess.Text = "Data Uploaded Successfully";


        //                    ////ClientScript.RegisterStartupScript(Page.GetType(), "PopUpMasterClientList", "javascript:PopUpMasterClientList();", true);
        //                    //lblSuccess.Visible = true;
        //                    //lblError.Visible = false;


        //                }

        //                else if (lstsheetNames1.Contains("MCOBEDeliveryShareReport$"))
        //                {
        //                    lblError.Text = "Please choose/select MCOBEDeliveryShareReport first and DHBEReport next";
        //                    lblError.Visible = true;
        //                }


        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename the sheet to DHBEReport";
        //                    lblError.Visible = true;
        //                }

        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //            }

        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";


        //        }






        //        if (dtExcel.Rows.Count > 0 || dtExcel1.Rows.Count > 0)
        //        {
        //            int cnt = 0;
        //            cnt = service.MCOCountProd();

        //            int count = 0;
        //            count = service.MCODumpCountProd();

        //            lblSuccess.Visible = true;
        //            lblError.Visible = false;

        //            lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in MCO BE Excel : " + fontRed + noOfRows + fontEnd + " , DH BE Excel : " +
        //                fontRed + noOfRows1 + fontEnd +
        //               " -No. of records in the table after upload :" + fontRed + count + fontEnd + " -No. or records in the main table after upload: " + fontRed + cnt;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }

        //    }
        //}



        public void LoadPackage()
        {

            try
            {
                string pkgpath = "";

                int counterror = 0;
                string ExecStatus = "";

                pkg = app.LoadPackage(System.Configuration.ConfigurationManager.AppSettings["ImportData"] + "\\CimbaLoadDev.dtsx", null);

                for (int i = 0; i < pkg.Connections.Count; i++)
                {
                    if (pkg.Connections[i].CreationName == "OLEDB")
                    {
                        pkg.Connections[i].ConnectionString = pkg.Connections[i].ConnectionString + "Password=cmed@123;";
                    }
                }

                pkgresult = pkg.Execute();
                counterror = pkg.Errors.Count;

                ExecStatus = pkg.ExecutionStatus.ToString();

                if (pkgresult.ToString() == "Success")
                {
                    lblSuccess.Text = "";
                    lblSuccess.Text = "Data Uploaded Successfully";
                    lblSuccess.Visible = true;
                    lblError.Visible = false;
                }
                else
                {
                    lblError.Text = "";
                    lblError.Text = "Data Upload was NOT SUCCESSFULL";
                    lblError.Visible = true;
                    lblSuccess.Visible = false;
                    for (int i = 0; i < pkg.Errors.Count; i++)
                    {
                        lblError.Text = lblError.Text + i.ToString() + "." + pkg.Errors[i].Description + "~";
                    }
                }

            }
            catch (Exception ex)
            {
                if ((ex.Message + "").Contains("Thread was being aborted."))
                    logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                else
                {
                    logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (drpApplication.Text == "Development")
            {
                divDev.Visible = true;
            //    divProd.Visible = false;
                lblSuccess.Text = "";
                lblError.Text = "";
            }
            else if (drpApplication.Text == "Production")
            {
               // divProd.Visible = true;
                divDev.Visible = false;
                lblSuccess.Text = "";
                lblError.Text = "";
            }
        }

        //protected void ImageButton7_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        DataAccess dataAccess = new DataAccess();
        //        DataTable dt = new DataTable();
        //        dt = dataAccess.GetRTBRBkUp();

        //        System.Web.UI.WebControls.DataGrid grid = new System.Web.UI.WebControls.DataGrid();
        //        grid.HeaderStyle.Font.Bold = true;
        //        grid.DataSource = dt;

        //        grid.DataBind();


        //        string Filename = "RTBRDump.xlsx";

        //        string folder = "ExcelOperations";
        //        var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //        if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "RTBRDump.xlsx") == null)
        //            System.IO.File.Delete(MyDir.FullName + "\\RTBRDump.xlsx");



        //        FileInfo file = new FileInfo(MyDir.FullName + "\\RTBRDump.xlsx");
        //        using (ExcelPackage pck = new ExcelPackage(file))
        //        {
        //            //Create the worksheet
        //            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("RTBRDump");

        //            //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
        //            ws.Cells["A1"].LoadFromDataTable(dt, true);
        //            pck.Save();
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("content-disposition", "attachment;  filename=RTBRDump.xlsx");
        //            //Response.BinaryWrite(pck.GetAsByteArray());
        //        }


        //        DownloadFile(Filename);

        //    }
        //    catch (Exception ex)
        //    {
        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}



        //protected void ImgFinPulDump_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        DataAccess dataAccess = new DataAccess();
        //        DataTable dt = new DataTable();
        //        dt = dataAccess.GetFinpulseBkUp();

        //        System.Web.UI.WebControls.DataGrid grid = new System.Web.UI.WebControls.DataGrid();
        //        grid.HeaderStyle.Font.Bold = true;
        //        grid.DataSource = dt;

        //        grid.DataBind();


        //        string Filename = "FinpulseDump.xlsx";

        //        string folder = "ExcelOperations";
        //        var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //        if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "FinpulseDump.xlsx") == null)
        //            System.IO.File.Delete(MyDir.FullName + "\\FinpulseDump.xlsx");



        //        FileInfo file = new FileInfo(MyDir.FullName + "\\FinpulseDump.xlsx");
        //        using (ExcelPackage pck = new ExcelPackage(file))
        //        {
        //            //Create the worksheet
        //            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("FinpulseDump");

        //            //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
        //            ws.Cells["A1"].LoadFromDataTable(dt, true);
        //            pck.Save();
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("content-disposition", "attachment;  filename=FinpulseDump.xlsx");
        //            //Response.BinaryWrite(pck.GetAsByteArray());
        //        }


        //        DownloadFile(Filename);

        //    }
        //    catch (Exception ex)
        //    {
        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}

        protected void ImgDownLoadFinPul_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/SampleXLSX/Finpulse.xls");
        }

        //protected void ImageButton13_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        DataAccess dataAccess = new DataAccess();
        //        DataTable dt = new DataTable();
        //        dt = dataAccess.GetBEMCOBkUp();

        //        System.Web.UI.WebControls.DataGrid grid = new System.Web.UI.WebControls.DataGrid();
        //        grid.HeaderStyle.Font.Bold = true;
        //        grid.DataSource = dt;

        //        grid.DataBind();


        //        string Filename = "BEMCODump.xlsx";

        //        string folder = "ExcelOperations";
        //        var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //        if (MyDir.GetFiles().SingleOrDefault(k => k.FullName == "BEMCODump.xlsx") == null)
        //            System.IO.File.Delete(MyDir.FullName + "\\BEMCODump.xlsx");



        //        FileInfo file = new FileInfo(MyDir.FullName + "\\BEMCODump.xlsx");
        //        using (ExcelPackage pck = new ExcelPackage(file))
        //        {
        //            //Create the worksheet
        //            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("BEMCODump");

        //            //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
        //            ws.Cells["A1"].LoadFromDataTable(dt, true);
        //            pck.Save();
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("content-disposition", "attachment;  filename=BEMCODump.xlsx");
        //            //Response.BinaryWrite(pck.GetAsByteArray());
        //        }


        //        DownloadFile(Filename);

        //    }
        //    catch (Exception ex)
        //    {
        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }
        //    }
        //}

        protected void ImageButton6_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/SampleXLSX/RTBR.xlsx");
        }

        //protected void ImageButton12_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/SampleXLSX/BEMCO.xlsx");
        //}

        protected void ImageButton8_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/SampleXLSX/Finpulse.xls");
        }

        protected void ImageButton10_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/SampleXLSX/RTBR.xlsx");
        }

        //protected void ImageButton14_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/SampleXLSX/BEMCO.xlsx");
        //}

        protected void ImgbtnAlcon_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/SampleXLSX/Alcon.xlsx");
        }

        protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/SampleXLSX/Alcon.xlsx");
        }

        //Alcon Upload DEV
        protected void btnAlcon_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dtExcel = new System.Data.DataTable();

            // Alert();

            dtExcel.TableName = "MyExcelData";


            System.Data.DataTable dtExcel1 = new System.Data.DataTable();

            dtExcel1.TableName = "MyExcelData";

            string folder = "ExcelOperations";

            var MyDir = new DirectoryInfo(Server.MapPath(folder));

            if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "AlconPBS.xlsx") != null)
                System.IO.File.Delete(MyDir.FullName + "\\AlconPBS.xlsx");

         

            //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "4", "4");


            string path = MyDir.FullName + "\\AlconPBS.xlsx";// + FileName;

            string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";


            //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "5", "5");

            OleDbConnection con = new OleDbConnection(SourceConstr);

            //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "6", "6");

            try
            {
                //                string FileName =
                //Server.MapPath(System.IO.Path.GetFileName(FinPulseUpload.PostedFile.FileName.ToString()));
                //                FinPulseUpload.PostedFile.SaveAs(fileName);
                string FileName = AlconUpload.FileName;
                if (AlconUpload.HasFile)
                {

                    string fileExt = Path.GetExtension(AlconUpload.FileName);
                    if ((fileExt.Equals(".xlsx")) || (fileExt.Equals(".xls")) && AlconUpload.PostedFile.ContentLength != 0)
                    {
                        if (FileName == "AlconPBS.xlsx" || FileName == "AlconPBS.xls")
                        {

                            AlconUpload.SaveAs(path);



                            string query = "Select * from [AlconPBS$]";
                            
                            OleDbDataAdapter data = new OleDbDataAdapter(query, con);
                          
                            data.Fill(dtExcel);
                          
                            int noOfRows = dtExcel.Rows.Count;
                           
                            //Code to check if sheet is having proper name
                            con.Open();

                            DataTable worksheets = con.GetSchema("Tables");
                            string w = worksheets.Columns["TABLE_NAME"].ToString();
                            List<string> lstsheetNames = new List<string>();
                            Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

                            worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

                            //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "11", "11");


                            if (lstsheetNames.Contains("AlconPBS$"))
                            {
                                


                                Application app = new Application();
                                Package package = null;



                                //Load DTSX
                                package = app.LoadPackage(@"D:\ETLPBS@\ETLPBS@\Package.dtsx", null);

                                //Execute DTSX.
                                Microsoft.SqlServer.Dts.Runtime.DTSExecResult results = package.Execute();
                                if (results == Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure)
                                {
                                    StringBuilder strError = new StringBuilder();
                                    foreach (Microsoft.SqlServer.Dts.Runtime.DtsError local_DtsError in package.Errors)
                                    {
                                        Console.WriteLine("Package Execution results: {0}", local_DtsError.Description.ToString());
                                        Console.WriteLine();
                                        strError.Append(local_DtsError.Description.ToString());
                                        strError.Append("-");
                                    }
                                    lblError.Text = strError.ToString();
                                    lblError.Visible = true;
                                    if (con.State.ToString().ToLower() == "open")
                                        con.Close();
                                }

                            }
                            lblSuccess.Visible = true;
                            int cnt = 0;
                            cnt = service.AlconDumpCountDev();
                            if (con.State.ToString().ToLower() == "open")
                                con.Close();
                            lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in Excel : " + fontRed + noOfRows + fontEnd +
                                " -No. of records in the table after upload :" + fontRed + cnt;
                            
                        }

                        else
                        {
                            if (con.State.ToString().ToLower() == "open")
                                con.Close();
                            lblError.Text = "";
                            lblError.Text = "Please rename the sheet to 'AlconPBS'";
                            lblError.Visible = true;
                            lblSuccess.Visible = false;
                        }

                    }

                    else
                    {
                        if (con.State.ToString().ToLower() == "open")
                            con.Close();
                        lblError.Text = "";
                        lblError.Text = "File is not in specified Format(.xls or .xlsx)";
                        lblError.Visible = true;
                        lblSuccess.Visible = false;
                    }


                }

                else
                {
                    lblError.Text = "";
                    lblError.Text = "Please Select a File";
                    lblError.Visible = true;
                    lblSuccess.Visible = false;
                }


            }
            catch (Exception ex)
            {
                if (con.State.ToString().ToLower() == "open")
                    con.Close();

                if ((ex.Message + "").Contains("Thread was being aborted."))
                    logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                else
                {
                    logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }

            }

          
        }

        //Alcon Upload PROD
        //protected void btnAlconProd_Click(object sender, EventArgs e)
        //{
        //    System.Data.DataTable dtExcel = new System.Data.DataTable();
        //    dtExcel.TableName = "MyExcelData";

        //    string folder = "ExcelOperationsProd";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "Alcon.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\Alcon.xlsx");

        //    string path = MyDir.FullName + "\\Alcon.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {
        //        string FileName = AlconUploadProd.FileName;

        //        if (AlconUploadProd.HasFile)
        //        {
        //            if (FileName.Contains(".xls"))
        //            {

        //                AlconUploadProd.SaveAs(path);
        //                string query = "Select * from [Sheet1$]";

        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("Sheet1$"))
        //                {

        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    bool isSuccess;

        //                    isSuccess = service.DeleteAlconDumpProd();

        //                    data.Fill(dtExcel);


        //                    int noOfRows = dtExcel.Rows.Count;
        //                    int rowsupdated = 0;

        //                    foreach (DataRow row in dtExcel.Rows)
        //                    {
        //                        Nullable<int> nullableInt = null;
        //                        Nullable<DateTime> nullableDate = null;
        //                        Nullable<double> nullableDouble = null;

        //                        string MasterProjectCode = row["Master Project Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Project Code"]);
        //                        Nullable<DateTime> Projectstarts = row["Project starts"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Project starts"]);
        //                        Nullable<DateTime> Projectends = row["Project ends"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Project ends"]);
        //                        string CustomerUnit = row["Customer Unit"] == DBNull.Value ? "" : Convert.ToString(row["Customer Unit"]);
        //                        string ProjectSubUnit = row["Project SubUnit"] == DBNull.Value ? "" : Convert.ToString(row["Project SubUnit"]);
        //                        string PU = row["PU"] == DBNull.Value ? "" : Convert.ToString(row["PU"]);
        //                        string CustomerSubUnit = row["Customer SubUnit"] == DBNull.Value ? "" : Convert.ToString(row["Customer SubUnit"]);
        //                        string MasterCustomer = row["Master Customer"] == DBNull.Value ? "" : Convert.ToString(row["Master Customer"]);
        //                        string Customer = row["Customer"] == DBNull.Value ? "" : Convert.ToString(row["Customer"]);
        //                        string ContractCode = row["Contract Code"] == DBNull.Value ? "" : Convert.ToString(row["Contract Code"]);
        //                        string TBBEnabled = row["TBB Enabled"] == DBNull.Value ? "" : Convert.ToString(row["TBB Enabled"]);
        //                        string ServiceOffering = row["Service Offering"] == DBNull.Value ? "" : Convert.ToString(row["Service Offering"]);
        //                        string Status = row["Status"] == DBNull.Value ? "" : Convert.ToString(row["Status"]);
        //                        Nullable<double> Version = row["Version"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Version"]);
        //                        string PM = row["PM"] == DBNull.Value ? "" : Convert.ToString(row["PM"]);
        //                        string DM = row["DM"] == DBNull.Value ? "" : Convert.ToString(row["DM"]);

        //                        string ChildPU = row["ChildPU"] == DBNull.Value ? "" : Convert.ToString(row["ChildPU"]);

        //                        string ChildCompany = row["ChildCompany"] == DBNull.Value ? "" : Convert.ToString(row["ChildCompany"]);
        //                        string Month = row["Month"] == DBNull.Value ? "" : Convert.ToString(row["Month"]);

        //                        Nullable<double> PBSOnsiteEffort = row["PBS Onsite Effort"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["PBS Onsite Effort"]);
        //                        Nullable<double> PBSOffshoreEffort = row["PBS Offshore Effort"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["PBS Offshore Effort"]);
        //                        Nullable<double> TotalPBSEffort = row["Total PBS Effort"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Total PBS Effort"]);
        //                        Nullable<double> ALCONOnsiteEffort = row["ALCON Onsite Effort"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["ALCON Onsite Effort"]);
        //                        Nullable<double> ALCONOffshoreEffort = row["ALCON Offshore Effort"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["ALCON Offshore Effort"]);
        //                        Nullable<double> TotalALCONEffort = row["Total ALCON Effort"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Total ALCON Effort"]);
        //                        Nullable<double> PBSALCONEffort = row["PBS-ALCON Effort"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["PBS-ALCON Effort"]);
        //                        string ProgramCode = row["ProgramCode"] == DBNull.Value ? "" : Convert.ToString(row["ProgramCode"]);
        //                        Nullable<double> PBSnonBillOnsiteEffort = row["PBS non-Bill Onsite Effort"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["PBS non-Bill Onsite Effort"]);
        //                        Nullable<double> PBSnonBillOffshoreEffort = row["PBS non-Bill Offshore Effort"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["PBS non-Bill Offshore Effort"]);
        //                        string GroupMasterProject = row["Group Master Project"] == DBNull.Value ? "" : Convert.ToString(row["Group Master Project"]);


        //                        service.InsertAlconDumpProd(MasterProjectCode, Projectstarts, Projectends, CustomerUnit,
        //                        ProjectSubUnit, PU, CustomerSubUnit, MasterCustomer,
        //                        Customer, ContractCode, TBBEnabled, ServiceOffering, Status, Version, PM, DM,
        //                        ChildPU, ChildCompany, Month, PBSOnsiteEffort, PBSOffshoreEffort, TotalPBSEffort,
        //                        ALCONOnsiteEffort, ALCONOffshoreEffort, TotalALCONEffort, PBSALCONEffort,
        //                        ProgramCode, PBSnonBillOnsiteEffort, PBSnonBillOffshoreEffort, GroupMasterProject);
        //                        rowsupdated++;
        //                    }


        //                    Session["FileName"] = path;
        //                    string ExcelFilePath = Session["FileName"].ToString();
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblSuccess.Text = "Data Uploaded Successfully";
        //                    lblSuccess.Visible = true;
        //                    lblError.Visible = false;
        //                    //}

        //                    //Code to check the count in Excel and Database
        //                    int cnt = 0;
        //                    cnt = service.AlconDumpCountProd();

        //                    lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in Excel : " + fontRed + noOfRows + fontEnd +
        //                       " -No. of records in the table after upload :" + fontRed + cnt;

        //                    //----------END

        //                    ClientScript.RegisterStartupScript(Page.GetType(), "PopUpMasterClientAlconProd", "javascript:PopUpMasterClientAlconProd();", true);
        //                }
        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename the sheet to 'Sheet1'";
        //                    lblError.Visible = true;
        //                }
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";
        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }

        //    }
        //}

        //protected void btnTMUnbilled_Click(object sender, EventArgs e)
        //{

        //    System.Data.DataTable dtExcel = new System.Data.DataTable();
        //    dtExcel.TableName = "MyExcelData";

        //    string folder = "ExcelOperations";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "TMUnbilled.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\TMUnbilled.xlsx");

        //    string path = MyDir.FullName + "\\TMUnbilled.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {
        //        string FileName = TMUnbilledLoad.FileName;

        //        if (TMUnbilledLoad.HasFile)
        //        {
        //            if (FileName.Contains(".xls"))
        //            {

        //                TMUnbilledLoad.SaveAs(path);
        //                string query = "Select * from [Sheet1$]";

        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("Sheet1$"))
        //                {
        //                    string finyear = ddlTMYear.SelectedValue;
        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    bool isSuccess;

        //                    isSuccess = service.DeleteTMUnbilledDump(finyear);

        //                    data.Fill(dtExcel);


        //                    int noOfRows = dtExcel.Rows.Count;
        //                    int rowsupdated = 0;

        //                    foreach (DataRow row in dtExcel.Rows)
        //                    {
        //                        Nullable<int> nullableInt = null;
        //                        Nullable<double> nullableDouble = null;

        //                        Nullable<double> EmpNum = row["Employee Number"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Employee Number"]);
        //                        string EmployeeName = row["Employee Name"] == DBNull.Value ? "" : Convert.ToString(row["Employee Name"]);
        //                        string RoleCapability = row["Role Capability"] == DBNull.Value ? "" : Convert.ToString(row["Role Capability"]);
        //                        string ProjectCode = row["Project Code"] == DBNull.Value ? "" : Convert.ToString(row["Project Code"]);
        //                        string MappedProjectCode = row["Mapped Project Code"] == DBNull.Value ? "" : Convert.ToString(row["Mapped Project Code"]);

        //                        string ProjectDU = row["Project DU"] == DBNull.Value ? "" : Convert.ToString(row["Project DU"]);
        //                        string ProjectPU = row["Project PU"] == DBNull.Value ? "" : Convert.ToString(row["Project PU"]);
        //                        string SubUnit = row["SubUnit"] == DBNull.Value ? "" : Convert.ToString(row["SubUnit"]);
        //                        string Unit = row["Unit"] == DBNull.Value ? "" : Convert.ToString(row["Unit"]);
        //                        string CustomerCode = row["Customer Code"] == DBNull.Value ? "" : Convert.ToString(row["Customer Code"]);
        //                        string MasterCustomerCode = row["Master Customer Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Customer Code"]);
        //                        Nullable<double> Month = row["Month"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Month"]);
        //                        Nullable<double> Quarter = row["Quarter"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Quarter"]);
        //                        Nullable<double> OpeningBalance = row["Opening Balance"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Opening Balance"]);
        //                        Nullable<double> OpeningbalanceFuture = row["Opening balance - Future"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Opening balance - Future"]);
        //                        Nullable<double> OpeningbalanceCancelledInvoices = row["Opening balance - Cancelled Invoices"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Opening balance - Cancelled Invoices"]);
        //                        Nullable<double> BillableDays = row["Billable Days"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Billable Days"]);
        //                        Nullable<double> BilledDays = row["Billed Days"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Billed Days"]);
        //                        Nullable<double> ConfirmationNotInitiated = row["Confirmation Not Initiated"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Confirmation Not Initiated"]);
        //                        Nullable<double> Buffer = row["Buffer"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Buffer"]);
        //                        Nullable<double> Future = row["Future"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Future"]);
        //                        Nullable<double> Holiday = row["Holiday"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Holiday"]);
        //                        Nullable<double> Leave = row["Leave"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Leave"]);
        //                        Nullable<double> NonBillable = row["Non-Billable"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Non-Billable"]);
        //                        Nullable<double> RoundingOffError = row["Rounding Off Error"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Rounding Off Error"]);
        //                        Nullable<double> Training = row["Training"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Training"]);
        //                        Nullable<double> Travel = row["Travel"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Travel"]);
        //                        Nullable<double> FT = row["FT"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["FT"]);
        //                        Nullable<double> IncorrectAllocation = row["Incorrect Allocation"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Incorrect Allocation"]);
        //                        Nullable<double> BillingatActual = row["Billing at Actual "] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Billing at Actual "]);
        //                        Nullable<double> SystemOverbilling = row["System Overbilling"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["System Overbilling"]);
        //                        Nullable<double> UnderBilled = row["Under Billed"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Under Billed"]);
        //                        Nullable<double> PersonMonths = row["Person Months"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Person Months"]);

        //                        string BillingStatus = row["Billing Status"] == DBNull.Value ? "" : Convert.ToString(row["Billing Status"]);
        //                        string ConfirmationType = row["Confirmation Type"] == DBNull.Value ? "" : Convert.ToString(row["Confirmation Type"]);
        //                        string Activity = row["Activity"] == DBNull.Value ? "" : Convert.ToString(row["Activity"]);
        //                        string OnsiteOffshoreIndicator = row["Onsite Offshore Indicator"] == DBNull.Value ? "" : Convert.ToString(row["Onsite Offshore Indicator"]);
        //                        string Remarks = row["Remarks"] == DBNull.Value ? "" : Convert.ToString(row["Remarks"]);
        //                        string ProductionInCharge = row["Production In Charge"] == DBNull.Value ? "" : Convert.ToString(row["Production In Charge"]);
        //                        string ProjectLocation = row["Project Location"] == DBNull.Value ? "" : Convert.ToString(row["Project Location"]);
        //                        string ProjectClass = row["Project Class"] == DBNull.Value ? "" : Convert.ToString(row["Project Class"]);
        //                        string BillingType = row["Billing Type"] == DBNull.Value ? "" : Convert.ToString(row["Billing Type"]);
        //                        string CTMBillingtype = row["CTM Billing type"] == DBNull.Value ? "" : Convert.ToString(row["CTM Billing type"]);
        //                        string BudgetingUnit = row["Budgeting Unit"] == DBNull.Value ? "" : Convert.ToString(row["Budgeting Unit"]);
        //                        string EmployeePU = row["Employee PU"] == DBNull.Value ? "" : Convert.ToString(row["Employee PU"]);
        //                        string EmployeeDU = row["Employee DU"] == DBNull.Value ? "" : Convert.ToString(row["Employee DU"]);
        //                        string EmployeeLocation = row["Employee Location"] == DBNull.Value ? "" : Convert.ToString(row["Employee Location"]);
        //                        string PMMailId = row["PM Mail Id"] == DBNull.Value ? "" : Convert.ToString(row["PM Mail Id"]);
        //                        string EmployeeRole = row["EmployeeRole"] == DBNull.Value ? "" : Convert.ToString(row["EmployeeRole"]);

        //                        Nullable<double> JobLevel = (row["JobLevel"] + "").Trim() == "" ? nullableDouble : (row["JobLevel"] + "").ToLowerTrim() == "null" ? nullableDouble : Convert.ToDouble(row["JobLevel"]);

        //                        string MasterProjectCode = row["MasterProjectCode"] == DBNull.Value ? "" : Convert.ToString(row["MasterProjectCode"]);
        //                        string ReportingPU = row["ReportingPU"] == DBNull.Value ? "" : Convert.ToString(row["ReportingPU"]);
        //                        string Company = row["Company"] == DBNull.Value ? "" : Convert.ToString(row["Company"]);
        //                        string SourceCompany = row["Source Company"] == DBNull.Value ? "" : Convert.ToString(row["Source Company"]);
        //                        string ProjectCountryCode = row["Project Country Code"] == DBNull.Value ? "" : Convert.ToString(row["Project Country Code"]);
        //                        //string SDM = row["SDM"] == DBNull.Value ? "" : Convert.ToString(row["SDM"]);
        //                        //string DH = row["DH"] == DBNull.Value ? "" : Convert.ToString(row["DH"]);
        //                        //string FinYear = row["FinYear"] == DBNull.Value ? "" : Convert.ToString(row["FinYear"]);

        //                        string FinYear = ddlTMYear.SelectedValue;

        //                        service.InsertTMUnbilledDump(EmpNum, EmployeeName, RoleCapability, ProjectCode, MappedProjectCode, ProjectDU, ProjectPU, SubUnit, Unit, CustomerCode,
        //                            MasterCustomerCode, Month, Quarter, OpeningBalance, OpeningbalanceFuture, OpeningbalanceCancelledInvoices, BillableDays, BilledDays, ConfirmationNotInitiated
        //                            , Buffer, Future, Holiday, Leave, NonBillable, RoundingOffError, Training, Travel, FT, IncorrectAllocation, BillingatActual, SystemOverbilling, UnderBilled
        //                            , PersonMonths, BillingStatus, ConfirmationType, Activity, OnsiteOffshoreIndicator, Remarks, ProductionInCharge, ProjectLocation, ProjectClass,
        //                            BillingType, CTMBillingtype, BudgetingUnit, EmployeePU, EmployeeDU, EmployeeLocation, PMMailId, EmployeeRole, JobLevel, MasterProjectCode, ReportingPU,
        //                            Company, SourceCompany, ProjectCountryCode, FinYear);
        //                        rowsupdated++;
        //                    }


        //                    Session["FileName"] = path;
        //                    string ExcelFilePath = Session["FileName"].ToString();
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblSuccess.Text = "Data Uploaded Successfully";
        //                    lblSuccess.Visible = true;
        //                    lblError.Visible = false;
        //                    //}

        //                    //Code to check the count in Excel and Database
        //                    int cnt = 0;
        //                    cnt = service.TMUnbilledDumpCountDev();

        //                    lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in Excel : " + fontRed + noOfRows + fontEnd +
        //                       " -No. of records in the table after upload :" + fontRed + cnt;

        //                    //----------END


        //                }
        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename the sheet to 'Sheet1'";
        //                    lblError.Visible = true;
        //                }
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";
        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }

        //    }
        //}

        //protected void ImgbtnTMUnbilled_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/SampleXLSX/TMUnbilledExcel.xlsx");
        //}

        //protected void btnDemOppLoad_Click(object sender, EventArgs e)
        //{



        //    System.Data.DataTable dtExcel = new System.Data.DataTable();
        //    dtExcel.TableName = "MyExcelData";

        //    string folder = "ExcelOperations";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "DemOppDetails.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\DemOppDetails.xlsx");

        //    string path = MyDir.FullName + "\\DemOppDetails.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {
        //        string FileName = DemOppLoad.FileName;

        //        if (DemOppLoad.HasFile)
        //        {
        //            if (FileName.Contains(".xls"))
        //            {

        //                DemOppLoad.SaveAs(path);
        //                string query = "Select * from [Sheet1$]";

        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("Sheet1$"))
        //                {

        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    bool isSuccess;

        //                    isSuccess = service.DeleteDemOppDetails();

        //                    data.Fill(dtExcel);


        //                    int noOfRows = dtExcel.Rows.Count;
        //                    int rowsupdated = 0;

        //                    foreach (DataRow row in dtExcel.Rows)
        //                    {
        //                        Nullable<int> nullableInt = null;
        //                        Nullable<double> nullableDouble = null;
        //                        Nullable<DateTime> nullableDatetime = null;


        //                        Nullable<int> intCRMOppId = row["SAP Opportunity ID"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["SAP Opportunity ID"]);
        //                        Nullable<int> intOppId = row["CIMBA Opportunity ID"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["CIMBA Opportunity ID"]);
        //                        string txtOppName = row["Opportunity Name"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Name"]);


        //                        Nullable<int> intParentOppID = (row["Parent Opportunity ID"] + "").Trim() == "" ? nullableInt : (row["Parent Opportunity ID"] + "".Trim()) == "NULL" ? nullableInt : Convert.ToInt32(row["Parent Opportunity ID"]);
        //                        string txtCompanyName = row["Child Account Name"] == DBNull.Value ? "" : Convert.ToString(row["Child Account Name"]);
        //                        string txtAccCode = row["Child Account Code"] == DBNull.Value ? "" : Convert.ToString(row["Child Account Code"]);
        //                        string txtMCCode = row["Master Customer Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Customer Code"]);
        //                        string txtPrimaryMember = row["Primary Owner Name"] == DBNull.Value ? "" : Convert.ToString(row["Primary Owner Name"]);
        //                        string txtSalesRegion = row["Child Sales Region Code"] == DBNull.Value ? "" : Convert.ToString(row["Child Sales Region Code"]);
        //                        string txtOppOwner = row["Opportunity Owner Name"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Owner Name"]);
        //                        string txtOppStage = row["Opportunity Stage"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Stage"]);
        //                        string txtOppStatus = row["Opportunity Status"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Status"]);
        //                        string txtNotes = row["Opportunity Comments"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Comments"]);

        //                        Nullable<double> fltTotalEstimate = (row["Opportunity Total Estimates"] + "").Trim() == "" ? nullableDouble : (row["Opportunity Total Estimates"] + "").Trim() == "NULL" ? nullableDouble : Convert.ToDouble(row["Opportunity Total Estimates"]);
        //                        string txtNativeCurrency = row["Opportunity Currency Code"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Currency Code"]);
        //                        Nullable<double> fltTotalEstimateInKUSD = (row["Opportunity Total Estimates In KUSD"] + "").Trim() == "" ? nullableDouble : (row["Opportunity Total Estimates In KUSD"] + "").Trim() == "NULL" ? nullableDouble : Convert.ToDouble(row["Opportunity Total Estimates In KUSD"]);

        //                        Nullable<int> intProbability = (row["Opportunity Probability"] + "").Trim() == "" ? nullableInt : (row["Opportunity Probability"] + "").Trim() == "NULL" ? nullableInt : Convert.ToInt32(row["Opportunity Probability"]);

        //                        Nullable<DateTime> dtOppCreated = (row["Created Date"] + "").Trim() == "" ? nullableDatetime : (row["Created Date"] + "").Trim() == "NULL" ? nullableDatetime : Convert.ToDateTime(row["Created Date"]);
        //                        Nullable<DateTime> dtLikelyStartDate = (row["Opportunity Likely Start Date"] + "").Trim() == "" ? nullableDatetime : (row["Opportunity Likely Start Date"] + "").Trim() == "NULL" ? nullableDatetime : Convert.ToDateTime(row["Opportunity Likely Start Date"]);
        //                        Nullable<DateTime> dtLikelyEndDate = (row["Opportunity Likely End Date"] + "").Trim() == "" ? nullableDatetime : (row["Opportunity Likely End Date"] + "").Trim() == "NULL" ? nullableDatetime : Convert.ToDateTime(row["Opportunity Likely End Date"]);
        //                        string txtTransformational = row["Program Transformational?"] == DBNull.Value ? "" : Convert.ToString(row["Program Transformational?"]);
        //                        string txtSGStagged = row["SGS Tagged(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["SGS Tagged(Y/N)"]);
        //                        //  string txtItrac = row["txtItrac(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["txtItrac(Y/N)"]);
        //                        // string txtProposalSubmit = row["txtProposalSubmit(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["txtProposalSubmit(Y/N)"]);
        //                        string txtProposalNo = row["Proposal ID"] == DBNull.Value ? "" : Convert.ToString(row["Proposal ID"]);
        //                        string txtFlgTopOpp = row["Top Opportunity?"] == DBNull.Value ? "" : Convert.ToString(row["Top Opportunity?"]);
        //                        string txtPU = row["Opportunity Primary PU Code"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Primary PU Code"]);
        //                        string txtDummy = row["Dummy Opportunity?"] == DBNull.Value ? "" : Convert.ToString(row["Dummy Opportunity?"]);
        //                        string txtStale = row["Is Opportunity Stale?"] == DBNull.Value ? "" : Convert.ToString(row["Is Opportunity Stale?"]);
        //                        Nullable<DateTime> dtLastModifiedDate = (row["Last Modified Date"] + "").Trim() == "" ? nullableDatetime : (row["Last Modified Date"] + "").Trim() == "NULL" ? nullableDatetime : Convert.ToDateTime(row["Last Modified Date"]);
        //                        string txtProposalStatus = row["Stage Description"] == DBNull.Value ? "" : Convert.ToString(row["Stage Description"]);
        //                        Nullable<double> fltProposalValue = (row["Total in USD as on Proposal Date"] + "").Trim() == "" ? nullableDouble : (row["Total in USD as on Proposal Date"] + "").Trim() == "NULL" ? nullableDouble : Convert.ToDouble(row["Total in USD as on Proposal Date"]);
        //                        string txtPropAnchor = row["Proposal Anchor Name"] == DBNull.Value ? "" : Convert.ToString(row["Proposal Anchor Name"]);
        //                        // Nullable<DateTime> dtProposalCreatedDate = (row["dtProposalCreatedDate"] + "").Trim() == "" ? nullableDatetime : (row["dtProposalCreatedDate"] + "").Trim() == "NULL" ? nullableDatetime : Convert.ToDateTime(row["dtProposalCreatedDate"]);
        //                        string txtSellingDirectExtension = row["Opportunity Classification"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Classification"]);
        //                        string txtSellingInvolved = row["Selling Involved"] == DBNull.Value ? "" : Convert.ToString(row["Selling Involved"]);
        //                        string txtContractType = row["Contract Code"] == DBNull.Value ? "" : Convert.ToString(row["Contract Code"]);
        //                        Nullable<DateTime> dtOppClosedOn = (row["Opportunity Closed On"] + "").Trim() == "" ? nullableDatetime : (row["Opportunity Closed On"] + "").Trim() == "null" ? nullableDatetime : Convert.ToDateTime(row["Opportunity Closed On"]);
        //                        //  Nullable<DateTime> dtProposalSubmissionDate = (row["dtProposalSubmissionDate"]+"").Trim() == "" ? nullableDatetime : (row["dtProposalSubmissionDate"]+"").Trim() == "null" ? nullableDatetime :Convert.ToDateTime(row["dtProposalSubmissionDate"]);
        //                        string txtCountry = row["Country Name"] == DBNull.Value ? "" : Convert.ToString(row["Country Name"]);
        //                        string txtSolutionName = row["Opportunity Solution Name"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Solution Name"]);
        //                        string txtAllianceName = row["Opportunity Alliance Name"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Alliance Name"]);
        //                        string txtDPSLinked = row["DPS Linked?"] == DBNull.Value ? "" : Convert.ToString(row["DPS Linked?"]);






        //                        service.InsertDemOppDetails(intCRMOppId, intOppId, txtOppName, intParentOppID, txtCompanyName, txtAccCode, txtMCCode, txtPrimaryMember, txtSalesRegion
        //                            , txtOppOwner, txtOppStage, txtOppStatus, txtNotes, fltTotalEstimate, txtNativeCurrency, fltTotalEstimateInKUSD, intProbability, dtOppCreated, dtLikelyStartDate,
        //                            dtLikelyEndDate, txtTransformational, txtSGStagged, txtProposalNo, txtFlgTopOpp, txtPU, txtDummy, txtStale, dtLastModifiedDate,
        //                            txtProposalStatus, fltProposalValue, txtPropAnchor, txtSellingDirectExtension, txtSellingInvolved, txtContractType, dtOppClosedOn,
        //                             txtCountry, txtSolutionName, txtAllianceName, txtDPSLinked);

        //                        rowsupdated++;
        //                    }


        //                    Session["FileName"] = path;
        //                    string ExcelFilePath = Session["FileName"].ToString();
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblSuccess.Text = "Data Uploaded Successfully";
        //                    lblSuccess.Visible = true;
        //                    lblError.Visible = false;
        //                    //}

        //                    //Code to check the count in Excel and Database
        //                    int cnt = 0;
        //                    cnt = service.DemOppDetailsCountDev();

        //                    lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in Excel : " + fontRed + noOfRows + fontEnd +
        //                       " -No. of records in the table after upload :" + fontRed + cnt;

        //                    //----------END


        //                }
        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename the sheet to 'Sheet1'";
        //                    lblError.Visible = true;
        //                }
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";
        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }

        //    }

        //}

        //protected void ImgDemOppLoad_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/SampleXLSX/DemOppDetails.xlsx");
        //}






        /////////////////////PROD/////////////



        //protected void btnTMUnbilledProd_Click(object sender, EventArgs e)
        //{

        //    System.Data.DataTable dtExcel = new System.Data.DataTable();
        //    dtExcel.TableName = "MyExcelData";

        //    string folder = "ExcelOperations";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "TMUnbilled.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\TMUnbilled.xlsx");

        //    string path = MyDir.FullName + "\\TMUnbilled.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {
        //        string FileName = TMUnbilledLoadProd.FileName;

        //        if (TMUnbilledLoadProd.HasFile)
        //        {
        //            if (FileName.Contains(".xls"))
        //            {

        //                TMUnbilledLoadProd.SaveAs(path);
        //                string query = "Select * from [Sheet1$]";

        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("Sheet1$"))
        //                {
        //                    string finyear = ddlTMYearProd.SelectedValue;
        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    bool isSuccess;

        //                    isSuccess = service.DeleteTMUnbilledDumpProd(finyear);

        //                    data.Fill(dtExcel);


        //                    int noOfRows = dtExcel.Rows.Count;
        //                    int rowsupdated = 0;

        //                    foreach (DataRow row in dtExcel.Rows)
        //                    {
        //                        Nullable<int> nullableInt = null;
        //                        Nullable<double> nullableDouble = null;

        //                        Nullable<double> EmpNum = row["Employee Number"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Employee Number"]);
        //                        string EmployeeName = row["Employee Name"] == DBNull.Value ? "" : Convert.ToString(row["Employee Name"]);
        //                        string RoleCapability = row["Role Capability"] == DBNull.Value ? "" : Convert.ToString(row["Role Capability"]);
        //                        string ProjectCode = row["Project Code"] == DBNull.Value ? "" : Convert.ToString(row["Project Code"]);
        //                        string MappedProjectCode = row["Mapped Project Code"] == DBNull.Value ? "" : Convert.ToString(row["Mapped Project Code"]);

        //                        string ProjectDU = row["Project DU"] == DBNull.Value ? "" : Convert.ToString(row["Project DU"]);
        //                        string ProjectPU = row["Project PU"] == DBNull.Value ? "" : Convert.ToString(row["Project PU"]);
        //                        string SubUnit = row["SubUnit"] == DBNull.Value ? "" : Convert.ToString(row["SubUnit"]);
        //                        string Unit = row["Unit"] == DBNull.Value ? "" : Convert.ToString(row["Unit"]);
        //                        string CustomerCode = row["Customer Code"] == DBNull.Value ? "" : Convert.ToString(row["Customer Code"]);
        //                        string MasterCustomerCode = row["Master Customer Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Customer Code"]);
        //                        Nullable<double> Month = row["Month"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Month"]);
        //                        Nullable<double> Quarter = row["Quarter"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Quarter"]);
        //                        Nullable<double> OpeningBalance = row["Opening Balance"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Opening Balance"]);
        //                        Nullable<double> OpeningbalanceFuture = row["Opening balance - Future"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Opening balance - Future"]);
        //                        Nullable<double> OpeningbalanceCancelledInvoices = row["Opening balance - Cancelled Invoices"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Opening balance - Cancelled Invoices"]);
        //                        Nullable<double> BillableDays = row["Billable Days"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Billable Days"]);
        //                        Nullable<double> BilledDays = row["Billed Days"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Billed Days"]);
        //                        //Nullable<double> ConfirmationNotInitiated = row["Confirmation Not Initiated"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Confirmation Not Initiated"]);
        //                        Nullable<double> ConfirmationNotInitiated = (row["Confirmation Not Initiated"] + "").Trim() == "" ? nullableDouble : (row["Confirmation Not Initiated"] + "").ToLowerTrim() == "null" ? nullableDouble : Convert.ToDouble(row["Confirmation Not Initiated"]);
        //                        Nullable<double> Buffer = row["Buffer"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Buffer"]);
        //                        Nullable<double> Future = row["Future"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Future"]);
        //                        Nullable<double> Holiday = row["Holiday"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Holiday"]);
        //                        Nullable<double> Leave = row["Leave"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Leave"]);
        //                        Nullable<double> NonBillable = row["Non-Billable"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Non-Billable"]);
        //                        Nullable<double> RoundingOffError = row["Rounding Off Error"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Rounding Off Error"]);
        //                        Nullable<double> Training = row["Training"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Training"]);
        //                        Nullable<double> Travel = row["Travel"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Travel"]);
        //                        Nullable<double> FT = row["FT"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["FT"]);
        //                        Nullable<double> IncorrectAllocation = row["Incorrect Allocation"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Incorrect Allocation"]);
        //                        Nullable<double> BillingatActual = row["Billing at Actual "] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Billing at Actual "]);
        //                        Nullable<double> SystemOverbilling = row["System Overbilling"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["System Overbilling"]);
        //                        Nullable<double> UnderBilled = row["Under Billed"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Under Billed"]);
        //                        Nullable<double> PersonMonths = row["Person Months"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Person Months"]);

        //                        string BillingStatus = row["Billing Status"] == DBNull.Value ? "" : Convert.ToString(row["Billing Status"]);
        //                        string ConfirmationType = row["Confirmation Type"] == DBNull.Value ? "" : Convert.ToString(row["Confirmation Type"]);
        //                        string Activity = row["Activity"] == DBNull.Value ? "" : Convert.ToString(row["Activity"]);
        //                        string OnsiteOffshoreIndicator = row["Onsite Offshore Indicator"] == DBNull.Value ? "" : Convert.ToString(row["Onsite Offshore Indicator"]);
        //                        string Remarks = row["Remarks"] == DBNull.Value ? "" : Convert.ToString(row["Remarks"]);
        //                        string ProductionInCharge = row["Production In Charge"] == DBNull.Value ? "" : Convert.ToString(row["Production In Charge"]);
        //                        string ProjectLocation = row["Project Location"] == DBNull.Value ? "" : Convert.ToString(row["Project Location"]);
        //                        string ProjectClass = row["Project Class"] == DBNull.Value ? "" : Convert.ToString(row["Project Class"]);
        //                        string BillingType = row["Billing Type"] == DBNull.Value ? "" : Convert.ToString(row["Billing Type"]);
        //                        string CTMBillingtype = row["CTM Billing type"] == DBNull.Value ? "" : Convert.ToString(row["CTM Billing type"]);
        //                        string BudgetingUnit = row["Budgeting Unit"] == DBNull.Value ? "" : Convert.ToString(row["Budgeting Unit"]);
        //                        string EmployeePU = row["Employee PU"] == DBNull.Value ? "" : Convert.ToString(row["Employee PU"]);
        //                        string EmployeeDU = row["Employee DU"] == DBNull.Value ? "" : Convert.ToString(row["Employee DU"]);
        //                        string EmployeeLocation = row["Employee Location"] == DBNull.Value ? "" : Convert.ToString(row["Employee Location"]);
        //                        string PMMailId = row["PM Mail Id"] == DBNull.Value ? "" : Convert.ToString(row["PM Mail Id"]);
        //                        string EmployeeRole = row["EmployeeRole"] == DBNull.Value ? "" : Convert.ToString(row["EmployeeRole"]);

        //                        Nullable<double> JobLevel = (row["JobLevel"] + "").Trim() == "" ? nullableDouble : (row["JobLevel"] + "").ToLowerTrim() == "null" ? nullableDouble : Convert.ToDouble(row["JobLevel"]);

        //                        string MasterProjectCode = row["MasterProjectCode"] == DBNull.Value ? "" : Convert.ToString(row["MasterProjectCode"]);
        //                        string ReportingPU = row["ReportingPU"] == DBNull.Value ? "" : Convert.ToString(row["ReportingPU"]);
        //                        string Company = row["Company"] == DBNull.Value ? "" : Convert.ToString(row["Company"]);
        //                        string SourceCompany = row["Source Company"] == DBNull.Value ? "" : Convert.ToString(row["Source Company"]);
        //                        string ProjectCountryCode = row["Project Country Code"] == DBNull.Value ? "" : Convert.ToString(row["Project Country Code"]);
        //                        //string SDM = row["SDM"] == DBNull.Value ? "" : Convert.ToString(row["SDM"]);
        //                        //string DH = row["DH"] == DBNull.Value ? "" : Convert.ToString(row["DH"]);
        //                        string FinYear =ddlTMYearProd.SelectedValue;



        //                        service.InsertTMUnbilledDumpProd(EmpNum, EmployeeName, RoleCapability, ProjectCode, MappedProjectCode, ProjectDU, ProjectPU, SubUnit, Unit, CustomerCode,
        //                            MasterCustomerCode, Month, Quarter, OpeningBalance, OpeningbalanceFuture, OpeningbalanceCancelledInvoices, BillableDays, BilledDays, ConfirmationNotInitiated
        //                            , Buffer, Future, Holiday, Leave, NonBillable, RoundingOffError, Training, Travel, FT, IncorrectAllocation, BillingatActual, SystemOverbilling, UnderBilled
        //                            , PersonMonths, BillingStatus, ConfirmationType, Activity, OnsiteOffshoreIndicator, Remarks, ProductionInCharge, ProjectLocation, ProjectClass,
        //                            BillingType, CTMBillingtype, BudgetingUnit, EmployeePU, EmployeeDU, EmployeeLocation, PMMailId, EmployeeRole, JobLevel, MasterProjectCode, ReportingPU,
        //                            Company, SourceCompany, ProjectCountryCode,FinYear);
        //                        rowsupdated++;
        //                    }


        //                    Session["FileName"] = path;
        //                    string ExcelFilePath = Session["FileName"].ToString();
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblSuccess.Text = "Data Uploaded Successfully";
        //                    lblSuccess.Visible = true;
        //                    lblError.Visible = false;
        //                    //}

        //                    //Code to check the count in Excel and Database
        //                    int cnt = 0;
        //                    cnt = service.TMUnbilledDumpCountProd();

        //                    lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in Excel : " + fontRed + noOfRows + fontEnd +
        //                       " -No. of records in the table after upload :" + fontRed + cnt;

        //                    //----------END


        //                }
        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename the sheet to 'Sheet1'";
        //                    lblError.Visible = true;
        //                }
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";
        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }

        //    }
        //}

        //protected void ImgbtnTMUnbilledProd_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/SampleXLSX/TMUnbilledExcel.xlsx");
        //}

        //protected void btnDemOppLoadProd_Click(object sender, EventArgs e)
        //{



        //    System.Data.DataTable dtExcel = new System.Data.DataTable();
        //    dtExcel.TableName = "MyExcelData";

        //    string folder = "ExcelOperations";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "DemOppDetails.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\DemOppDetails.xlsx");

        //    string path = MyDir.FullName + "\\DemOppDetails.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {
        //        string FileName = DemOppLoadProd.FileName;

        //        if (DemOppLoadProd.HasFile)
        //        {
        //            if (FileName.Contains(".xls"))
        //            {

        //                DemOppLoadProd.SaveAs(path);
        //                string query = "Select * from [Sheet1$]";

        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("Sheet1$"))
        //                {

        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    bool isSuccess;

        //                    isSuccess = service.DeleteDemOppDetailsProd();

        //                    data.Fill(dtExcel);


        //                    int noOfRows = dtExcel.Rows.Count;
        //                    int rowsupdated = 0;

        //                    foreach (DataRow row in dtExcel.Rows)
        //                    {
        //                        Nullable<int> nullableInt = null;
        //                        Nullable<double> nullableDouble = null;
        //                        Nullable<DateTime> nullableDatetime = null;


        //                        Nullable<int> intCRMOppId = row["SAP Opportunity ID"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["SAP Opportunity ID"]);
        //                        Nullable<int> intOppId = row["CIMBA Opportunity ID"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["CIMBA Opportunity ID"]);
        //                        string txtOppName = row["Opportunity Name"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Name"]);


        //                        Nullable<int> intParentOppID = (row["Parent Opportunity ID"] + "").Trim() == "" ? nullableInt : (row["Parent Opportunity ID"] + "".Trim()) == "NULL" ? nullableInt : Convert.ToInt32(row["Parent Opportunity ID"]);
        //                        string txtCompanyName = row["Child Account Name"] == DBNull.Value ? "" : Convert.ToString(row["Child Account Name"]);
        //                        string txtAccCode = row["Child Account Code"] == DBNull.Value ? "" : Convert.ToString(row["Child Account Code"]);
        //                        string txtMCCode = row["Master Customer Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Customer Code"]);
        //                        string txtPrimaryMember = row["Primary Owner Name"] == DBNull.Value ? "" : Convert.ToString(row["Primary Owner Name"]);
        //                        string txtSalesRegion = row["Child Sales Region Code"] == DBNull.Value ? "" : Convert.ToString(row["Child Sales Region Code"]);
        //                        string txtOppOwner = row["Opportunity Owner Name"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Owner Name"]);
        //                        string txtOppStage = row["Opportunity Stage"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Stage"]);
        //                        string txtOppStatus = row["Opportunity Status"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Status"]);
        //                        string txtNotes = row["Opportunity Comments"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Comments"]);

        //                        Nullable<double> fltTotalEstimate = (row["Opportunity Total Estimates"] + "").Trim() == "" ? nullableDouble : (row["Opportunity Total Estimates"] + "").Trim() == "NULL" ? nullableDouble : Convert.ToDouble(row["Opportunity Total Estimates"]);
        //                        string txtNativeCurrency = row["Opportunity Currency Code"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Currency Code"]);
        //                        Nullable<double> fltTotalEstimateInKUSD = (row["Opportunity Total Estimates In KUSD"] + "").Trim() == "" ? nullableDouble : (row["Opportunity Total Estimates In KUSD"] + "").Trim() == "NULL" ? nullableDouble : Convert.ToDouble(row["Opportunity Total Estimates In KUSD"]);

        //                        Nullable<int> intProbability = (row["Opportunity Probability"] + "").Trim() == "" ? nullableInt : (row["Opportunity Probability"] + "").Trim() == "NULL" ? nullableInt : Convert.ToInt32(row["Opportunity Probability"]);

        //                        Nullable<DateTime> dtOppCreated = (row["Created Date"] + "").Trim() == "" ? nullableDatetime : (row["Created Date"] + "").Trim() == "NULL" ? nullableDatetime : Convert.ToDateTime(row["Created Date"]);
        //                        Nullable<DateTime> dtLikelyStartDate = (row["Opportunity Likely Start Date"] + "").Trim() == "" ? nullableDatetime : (row["Opportunity Likely Start Date"] + "").Trim() == "NULL" ? nullableDatetime : Convert.ToDateTime(row["Opportunity Likely Start Date"]);
        //                        Nullable<DateTime> dtLikelyEndDate = (row["Opportunity Likely End Date"] + "").Trim() == "" ? nullableDatetime : (row["Opportunity Likely End Date"] + "").Trim() == "NULL" ? nullableDatetime : Convert.ToDateTime(row["Opportunity Likely End Date"]);
        //                        string txtTransformational = row["Program Transformational?"] == DBNull.Value ? "" : Convert.ToString(row["Program Transformational?"]);
        //                        string txtSGStagged = row["SGS Tagged(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["SGS Tagged(Y/N)"]);
        //                        //  string txtItrac = row["txtItrac(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["txtItrac(Y/N)"]);
        //                        // string txtProposalSubmit = row["txtProposalSubmit(Y/N)"] == DBNull.Value ? "" : Convert.ToString(row["txtProposalSubmit(Y/N)"]);
        //                        string txtProposalNo = row["Proposal ID"] == DBNull.Value ? "" : Convert.ToString(row["Proposal ID"]);
        //                        string txtFlgTopOpp = row["Top Opportunity?"] == DBNull.Value ? "" : Convert.ToString(row["Top Opportunity?"]);
        //                        string txtPU = row["Opportunity Primary PU Code"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Primary PU Code"]);
        //                        string txtDummy = row["Dummy Opportunity?"] == DBNull.Value ? "" : Convert.ToString(row["Dummy Opportunity?"]);
        //                        string txtStale = row["Is Opportunity Stale?"] == DBNull.Value ? "" : Convert.ToString(row["Is Opportunity Stale?"]);
        //                        Nullable<DateTime> dtLastModifiedDate = (row["Last Modified Date"] + "").Trim() == "" ? nullableDatetime : (row["Last Modified Date"] + "").Trim() == "NULL" ? nullableDatetime : Convert.ToDateTime(row["Last Modified Date"]);
        //                        string txtProposalStatus = row["Stage Description"] == DBNull.Value ? "" : Convert.ToString(row["Stage Description"]);
        //                        Nullable<double> fltProposalValue = (row["Total in USD as on Proposal Date"] + "").Trim() == "" ? nullableDouble : (row["Total in USD as on Proposal Date"] + "").Trim() == "NULL" ? nullableDouble : Convert.ToDouble(row["Total in USD as on Proposal Date"]);
        //                        string txtPropAnchor = row["Proposal Anchor Name"] == DBNull.Value ? "" : Convert.ToString(row["Proposal Anchor Name"]);
        //                        // Nullable<DateTime> dtProposalCreatedDate = (row["dtProposalCreatedDate"] + "").Trim() == "" ? nullableDatetime : (row["dtProposalCreatedDate"] + "").Trim() == "NULL" ? nullableDatetime : Convert.ToDateTime(row["dtProposalCreatedDate"]);
        //                        string txtSellingDirectExtension = row["Opportunity Classification"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Classification"]);
        //                        string txtSellingInvolved = row["Selling Involved"] == DBNull.Value ? "" : Convert.ToString(row["Selling Involved"]);
        //                        string txtContractType = row["Contract Code"] == DBNull.Value ? "" : Convert.ToString(row["Contract Code"]);
        //                        Nullable<DateTime> dtOppClosedOn = (row["Opportunity Closed On"] + "").Trim() == "" ? nullableDatetime : (row["Opportunity Closed On"] + "").Trim() == "null" ? nullableDatetime : Convert.ToDateTime(row["Opportunity Closed On"]);
        //                        //  Nullable<DateTime> dtProposalSubmissionDate = (row["dtProposalSubmissionDate"]+"").Trim() == "" ? nullableDatetime : (row["dtProposalSubmissionDate"]+"").Trim() == "null" ? nullableDatetime :Convert.ToDateTime(row["dtProposalSubmissionDate"]);
        //                        string txtCountry = row["Country Name"] == DBNull.Value ? "" : Convert.ToString(row["Country Name"]);
        //                        string txtSolutionName = row["Opportunity Solution Name"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Solution Name"]);
        //                        string txtAllianceName = row["Opportunity Alliance Name"] == DBNull.Value ? "" : Convert.ToString(row["Opportunity Alliance Name"]);
        //                        string txtDPSLinked = row["DPS Linked?"] == DBNull.Value ? "" : Convert.ToString(row["DPS Linked?"]);






        //                        service.InsertDemOppDetailsProd(intCRMOppId, intOppId, txtOppName, intParentOppID, txtCompanyName, txtAccCode, txtMCCode, txtPrimaryMember, txtSalesRegion
        //                            , txtOppOwner, txtOppStage, txtOppStatus, txtNotes, fltTotalEstimate, txtNativeCurrency, fltTotalEstimateInKUSD, intProbability, dtOppCreated, dtLikelyStartDate,
        //                            dtLikelyEndDate, txtTransformational, txtSGStagged, txtProposalNo, txtFlgTopOpp, txtPU, txtDummy, txtStale, dtLastModifiedDate,
        //                            txtProposalStatus, fltProposalValue, txtPropAnchor, txtSellingDirectExtension, txtSellingInvolved, txtContractType, dtOppClosedOn,
        //                             txtCountry, txtSolutionName, txtAllianceName, txtDPSLinked);

        //                        rowsupdated++;
        //                    }


        //                    Session["FileName"] = path;
        //                    string ExcelFilePath = Session["FileName"].ToString();
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblSuccess.Text = "Data Uploaded Successfully";
        //                    lblSuccess.Visible = true;
        //                    lblError.Visible = false;
        //                    //}

        //                    //Code to check the count in Excel and Database
        //                    int cnt = 0;
        //                    cnt = service.DemOppDetailsCountProd();

        //                    lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in Excel : " + fontRed + noOfRows + fontEnd +
        //                       " -No. of records in the table after upload :" + fontRed + cnt;

        //                    //----------END


        //                }
        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename the sheet to 'Sheet1'";
        //                    lblError.Visible = true;
        //                }
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";
        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }

        //    }

        //}

        //protected void ImgDemOppLoadProd_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/SampleXLSX/DemOppDetails.xlsx");
        //}

        //protected void ImageBtnRevExp_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/SampleXLSX/RevenueExp.xlsx");
        //}


        //protected void ImgBtnSubcon_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/SampleXLSX/Subcon.xlsx");
        //}

        //Subcon Upload DEV
        //protected void btnSubconUpload_Click(object sender, EventArgs e)
        //{
        //    System.Data.DataTable dtExcel = new System.Data.DataTable();
        //    dtExcel.TableName = "MyExcelData";

        //    string folder = "ExcelOperations";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "Subcon.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\Subcon.xlsx");

        //    string path = MyDir.FullName + "\\Subcon.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {
        //        string FileName = SubconUpload.FileName;

        //        if (SubconUpload.HasFile)
        //        {
        //            if (FileName.Contains(".xls"))
        //            {

        //                SubconUpload.SaveAs(path);
        //                string query = "Select * from [Sheet1$]";

        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("Sheet1$"))
        //                {
        //                    service.DeleteSubconDumpDev();

        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);
        //                    data.Fill(dtExcel);


        //                    int noOfRows = dtExcel.Rows.Count;
        //                    int rowsupdated = 0;

        //                    foreach (DataRow row in dtExcel.Rows)
        //                    {
        //                        Nullable<int> nullableInt = null;
        //                        Nullable<DateTime> nullableDate = null;
        //                        Nullable<double> nullableDouble = null;

        //                        Nullable<int> empNo = row["Emp No"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Emp No"]);
        //                        string empName = row["Emp Name"] == DBNull.Value ? "" : Convert.ToString(row["Emp Name"]);
        //                        Nullable<DateTime> joinedDate = row["Joined Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Joined Date"]);
        //                        string sbuJoined = row["SBU Joined"] == DBNull.Value ? "" : Convert.ToString(row["SBU Joined"]);
        //                        string puCode = row["PU Code"] == DBNull.Value ? "" : Convert.ToString(row["PU Code"]);
        //                        string duCode = row["DU Code"] == DBNull.Value ? "" : Convert.ToString(row["DU Code"]);
        //                        string joinedLocation = row["Joined Location"] == DBNull.Value ? "" : Convert.ToString(row["Joined Location"]);
        //                        Nullable<double> poNumber = row["PO Number"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["PO Number"]);
        //                        string projectCode = row["Project Code"] == DBNull.Value ? "" : Convert.ToString(row["Project Code"]);
        //                        Nullable<DateTime> startDate = row["Start Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Start Date"]);
        //                        Nullable<DateTime> endDate = row["End Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["End Date"]);
        //                        string skills = row["Skills"] == DBNull.Value ? "" : Convert.ToString(row["Skills"]);
        //                        string yearsOfExp = row["Years of Exp#"] == DBNull.Value ? "" : Convert.ToString(row["Years of Exp#"]);
        //                        string vendorName = row["Vendor Name"] == DBNull.Value ? "" : Convert.ToString(row["Vendor Name"]);
        //                        Nullable<double> vendorRate = row["Vendor Rate"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Vendor Rate"]);
        //                        string vendorRateCurrency = row["Vendor Rate Currency"] == DBNull.Value ? "" : Convert.ToString(row["Vendor Rate Currency"]);
        //                        string vendorRateUOM = row["Vendor Rate UOM"] == DBNull.Value ? "" : Convert.ToString(row["Vendor Rate UOM"]);
        //                        Nullable<double> billingRate = row["Billing Rate"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Billing Rate"]);
        //                        string billingCurrency = row["Billing Currency"] == DBNull.Value ? "" : Convert.ToString(row["Billing Currency"]);
        //                        string billingUOM = row["Billing UOM"] == DBNull.Value ? "" : Convert.ToString(row["Billing UOM"]);
        //                        string margin = row["Margin"] == DBNull.Value ? "" : Convert.ToString(row["Margin"]);
        //                        string onsiteOffshore = row["Onsite/Offshore"] == DBNull.Value ? "" : Convert.ToString(row["Onsite/Offshore"]);
        //                        string GLCode = row["GL Code"] == DBNull.Value ? "" : Convert.ToString(row["GL Code"]);
        //                        string prodNonProd = row["PROD/NON-PROD"] == DBNull.Value ? "" : Convert.ToString(row["PROD/NON-PROD"]);
        //                        string country = row["Country"] == DBNull.Value ? "" : Convert.ToString(row["Country"]);
        //                        string usNonus = row["US/NON-US"] == DBNull.Value ? "" : Convert.ToString(row["US/NON-US"]);
        //                        Nullable<double> year = row["Year"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Year"]);
        //                        Nullable<double> month = row["Month"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Month"]);
        //                        Nullable<double> totalMonth = row["Total Months"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Total Months"]);
        //                        Nullable<double> age1 = row["Age <3 Months"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Age <3 Months"]);
        //                        Nullable<double> age2 = row["Age 3-6 Months"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Age 3-6 Months"]);
        //                        Nullable<double> age3 = row["Age 6 Months-1 Year"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Age 6 Months-1 Year"]);
        //                        Nullable<double> age4 = row["Age 1-3 Years"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Age 1-3 Years"]);
        //                        Nullable<double> age5 = row["Age 3-5 Years"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Age 3-5 Years"]);
        //                        Nullable<double> age6 = row["Age > 5 years"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Age > 5 years"]);

        //                        service.InsertSubconDumpDev(empNo, empName, joinedDate, sbuJoined, puCode, duCode, joinedLocation, poNumber,
        //                            projectCode, startDate, endDate, skills, yearsOfExp, vendorName, vendorRate, vendorRateCurrency, vendorRateUOM, billingRate,
        //                            billingCurrency, billingUOM, margin, onsiteOffshore, GLCode, prodNonProd, country, usNonus, year,
        //                            month, totalMonth, age1, age2, age3, age4, age5, age6);

        //                        rowsupdated++;

        //                    }


        //                    Session["FileName"] = path;
        //                    string ExcelFilePath = Session["FileName"].ToString();
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblSuccess.Text = "Data Uploaded Successfully";
        //                    lblSuccess.Visible = true;
        //                    lblError.Visible = false;

        //                    int count = 0;
        //                    count = service.SubconDumpCountDev();
        //                    lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in Excel : " + fontRed + noOfRows + fontEnd +
        //                       " -No. of records in the table after upload :" + fontRed + count;

        //                    // ClientScript.RegisterStartupScript(Page.GetType(), "PopUpMasterClientAlcon", "javascript:PopUpMasterClientAlcon();", true);
        //                }
        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename the sheet to 'Sheet1'";
        //                    lblError.Visible = true;
        //                }
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";
        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }

        //    }
        //}

        //Subcon Upload PROD
        //protected void btnSubconUploadProd_Click(object sender, EventArgs e)
        //{
        //    System.Data.DataTable dtExcel = new System.Data.DataTable();
        //    dtExcel.TableName = "MyExcelData";

        //    string folder = "ExcelOperationsProd";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "Subcon.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\Subcon.xlsx");

        //    string path = MyDir.FullName + "\\Subcon.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {
        //        string FileName = SubconUploadProd.FileName;

        //        if (SubconUploadProd.HasFile)
        //        {
        //            if (FileName.Contains(".xls"))
        //            {

        //                SubconUploadProd.SaveAs(path);
        //                string query = "Select * from [Sheet1$]";

        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("Sheet1$"))
        //                {

        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    bool isSuccess;

        //                    isSuccess = service.DeleteSubconDumpProd();

        //                    data.Fill(dtExcel);


        //                    int noOfRows = dtExcel.Rows.Count;
        //                    int rowsupdated = 0;

        //                    foreach (DataRow row in dtExcel.Rows)
        //                    {
        //                        Nullable<int> nullableInt = null;
        //                        Nullable<DateTime> nullableDate = null;
        //                        Nullable<double> nullableDouble = null;

        //                        Nullable<int> empNo = row["Emp No"] == DBNull.Value ? nullableInt : Convert.ToInt32(row["Emp No"]);
        //                        string empName = row["Emp Name"] == DBNull.Value ? "" : Convert.ToString(row["Emp Name"]);
        //                        Nullable<DateTime> joinedDate = row["Joined Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Joined Date"]);
        //                        string sbuJoined = row["SBU Joined"] == DBNull.Value ? "" : Convert.ToString(row["SBU Joined"]);
        //                        string puCode = row["PU Code"] == DBNull.Value ? "" : Convert.ToString(row["PU Code"]);
        //                        string duCode = row["DU Code"] == DBNull.Value ? "" : Convert.ToString(row["DU Code"]);
        //                        string joinedLocation = row["Joined Location"] == DBNull.Value ? "" : Convert.ToString(row["Joined Location"]);
        //                        Nullable<double> poNumber = row["PO Number"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["PO Number"]);
        //                        string projectCode = row["Project Code"] == DBNull.Value ? "" : Convert.ToString(row["Project Code"]);
        //                        Nullable<DateTime> startDate = row["Start Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["Start Date"]);
        //                        Nullable<DateTime> endDate = row["End Date"] == DBNull.Value ? nullableDate : Convert.ToDateTime(row["End Date"]);
        //                        string skills = row["Skills"] == DBNull.Value ? "" : Convert.ToString(row["Skills"]);
        //                        string yearsOfExp = row["Years of Exp#"] == DBNull.Value ? "" : Convert.ToString(row["Years of Exp#"]);
        //                        string vendorName = row["Vendor Name"] == DBNull.Value ? "" : Convert.ToString(row["Vendor Name"]);
        //                        Nullable<double> vendorRate = row["Vendor Rate"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Vendor Rate"]);
        //                        string vendorRateCurrency = row["Vendor Rate Currency"] == DBNull.Value ? "" : Convert.ToString(row["Vendor Rate Currency"]);
        //                        string vendorRateUOM = row["Vendor Rate UOM"] == DBNull.Value ? "" : Convert.ToString(row["Vendor Rate UOM"]);
        //                        Nullable<double> billingRate = row["Billing Rate"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Billing Rate"]);
        //                        string billingCurrency = row["Billing Currency"] == DBNull.Value ? "" : Convert.ToString(row["Billing Currency"]);
        //                        string billingUOM = row["Billing UOM"] == DBNull.Value ? "" : Convert.ToString(row["Billing UOM"]);
        //                        string margin = row["Margin"] == DBNull.Value ? "" : Convert.ToString(row["Margin"]);
        //                        string onsiteOffshore = row["Onsite/Offshore"] == DBNull.Value ? "" : Convert.ToString(row["Onsite/Offshore"]);
        //                        string GLCode = row["GL Code"] == DBNull.Value ? "" : Convert.ToString(row["GL Code"]);
        //                        string prodNonProd = row["PROD/NON-PROD"] == DBNull.Value ? "" : Convert.ToString(row["PROD/NON-PROD"]);
        //                        string country = row["Country"] == DBNull.Value ? "" : Convert.ToString(row["Country"]);
        //                        string usNonus = row["US/NON-US"] == DBNull.Value ? "" : Convert.ToString(row["US/NON-US"]);
        //                        Nullable<double> year = row["Year"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Year"]);
        //                        Nullable<double> month = row["Month"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Month"]);
        //                        Nullable<double> totalMonth = row["Total Months"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Total Months"]);
        //                        Nullable<double> age1 = row["Age <3 Months"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Age <3 Months"]);
        //                        Nullable<double> age2 = row["Age 3-6 Months"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Age 3-6 Months"]);
        //                        Nullable<double> age3 = row["Age 6 Months-1 Year"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Age 6 Months-1 Year"]);
        //                        Nullable<double> age4 = row["Age 1-3 Years"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Age 1-3 Years"]);
        //                        Nullable<double> age5 = row["Age 3-5 Years"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Age 3-5 Years"]);
        //                        Nullable<double> age6 = row["Age > 5 years"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Age > 5 years"]);

        //                        service.InsertSubconDumpProd(empNo, empName, joinedDate, sbuJoined, puCode, duCode, joinedLocation, poNumber,
        //                            projectCode, startDate, endDate, skills, yearsOfExp, vendorName, vendorRate, vendorRateCurrency, vendorRateUOM, billingRate,
        //                            billingCurrency, billingUOM, margin, onsiteOffshore, GLCode, prodNonProd, country, usNonus, year,
        //                            month, totalMonth, age1, age2, age3, age4, age5, age6);

        //                        rowsupdated++;
        //                    }


        //                    Session["FileName"] = path;
        //                    string ExcelFilePath = Session["FileName"].ToString();
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblSuccess.Text = "Data Uploaded Successfully";
        //                    lblSuccess.Visible = true;
        //                    lblError.Visible = false;
        //                    //}

        //                    //Code to check the count in Excel and Database
        //                    int cnt = 0;
        //                    cnt = service.SubconDumpCountProd();

        //                    lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in Excel : " + fontRed + noOfRows + fontEnd +
        //                       " -No. of records in the table after upload :" + fontRed + cnt;

        //                    //----------END

        //                    //ClientScript.RegisterStartupScript(Page.GetType(), "PopUpMasterClientAlconProd", "javascript:PopUpMasterClientAlconProd();", true);
        //                }
        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename the sheet to 'Sheet1'";
        //                    lblError.Visible = true;
        //                }
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";
        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }

        //    }
        //}

        //Revenue Exp Upload DEV
        //protected void btnRevExpDev_Click(object sender, EventArgs e)
        //{
        //    System.Data.DataTable dtExcel = new System.Data.DataTable();
        //    dtExcel.TableName = "MyExcelData";

        //    string folder = "ExcelOperations";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "RevenueExp.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\RevenueExp.xlsx");

        //    string path = MyDir.FullName + "\\RevenueExp.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {
        //        string FileName = RevenueUpload.FileName;

        //        if (RevenueUpload.HasFile)
        //        {
        //            if (FileName.Contains(".xls"))
        //            {
        //                string year = ddlYearRevenue.Text;

        //                RevenueUpload.SaveAs(path);
        //                string query = "Select * from [Sheet1$]";

        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("Sheet1$"))
        //                {

        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    service.DeleteRevExpDev(year);

        //                    data.Fill(dtExcel);


        //                    int noOfRows = dtExcel.Rows.Count;
        //                    int rowsupdated = 0;

        //                    foreach (DataRow row in dtExcel.Rows)
        //                    {
        //                        Nullable<double> nullableDouble = null;

        //                        string reportingPU = row["Reporting PU"] == DBNull.Value ? "" : Convert.ToString(row["Reporting PU"]);
        //                        string plGroup = row["PL Group"] == DBNull.Value ? "" : Convert.ToString(row["PL Group"]);
        //                        string classification = row["Classification"] == DBNull.Value ? "" : Convert.ToString(row["Classification"]);
        //                        string allocationType = row["Allocation Type"] == DBNull.Value ? "" : Convert.ToString(row["Allocation Type"]);
        //                        string budgetingUnitCode = row["Budgeting Unit Code"] == DBNull.Value ? "" : Convert.ToString(row["Budgeting Unit Code"]);
        //                        string subUnitCode = row["Sub Unit Code"] == DBNull.Value ? "" : Convert.ToString(row["Sub Unit Code"]);
        //                        string unitCode = row["Unit Code"] == DBNull.Value ? "" : Convert.ToString(row["Unit Code"]);
        //                        string puGroup = row["PU Group"] == DBNull.Value ? "" : Convert.ToString(row["PU Group"]);
        //                        Nullable<double> apr = row["Apr"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Apr"]);
        //                        Nullable<double> may = row["May"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["May"]);
        //                        Nullable<double> jun = row["Jun"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Jun"]);
        //                        Nullable<double> jul = row["Jul"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Jul"]);
        //                        Nullable<double> aug = row["Aug"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Aug"]);
        //                        Nullable<double> sep = row["Sep"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Sep"]);
        //                        Nullable<double> oct = row["Oct"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Oct"]);
        //                        Nullable<double> nov = row["Nov"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Nov"]);
        //                        Nullable<double> dec = row["Dec"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Dec"]);
        //                        Nullable<double> jan = row["Jan"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Jan"]);
        //                        Nullable<double> feb = row["Feb"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Feb"]);
        //                        Nullable<double> mar = row["Mar"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Mar"]);
        //                        Nullable<double> q1 = row["Q1"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Q1"]);
        //                        Nullable<double> q2 = row["Q2"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Q2"]);
        //                        Nullable<double> q3 = row["Q3"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Q3"]);
        //                        Nullable<double> q4 = row["Q4"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Q4"]);
        //                        Nullable<double> ytd = row["YTD"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["YTD"]);
        //                        Nullable<double> total = row["Total"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Total"]);
        //                        string projectType = row["Project Type"] == DBNull.Value ? "" : Convert.ToString(row["Project Type"]);
        //                        string accountHead = row["Account Head"] == DBNull.Value ? "" : Convert.ToString(row["Account Head"]);
        //                        string onsiteOffshore = row["Onsite/Offshore"] == DBNull.Value ? "" : Convert.ToString(row["Onsite/Offshore"]);
        //                        string customerCode = row["Customer Code"] == DBNull.Value ? "" : Convert.ToString(row["Customer Code"]);
        //                        string customerName = row["Customer Name"] == DBNull.Value ? "" : Convert.ToString(row["Customer Name"]);
        //                        string masterCustomerCode = row["Master Customer Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Customer Code"]);
        //                        string customerPortfolio = row["Customer Portfolio"] == DBNull.Value ? "" : Convert.ToString(row["Customer Portfolio"]);
        //                        string masterCustIBU = row["Master Customer IBU"] == DBNull.Value ? "" : Convert.ToString(row["Master Customer IBU"]);
        //                        string programCode = row["Program Code"] == DBNull.Value ? "" : Convert.ToString(row["Program Code"]);
        //                        string trackCode = row["Track Code"] == DBNull.Value ? "" : Convert.ToString(row["Track Code"]);
        //                        string regionCode = row["Region Code"] == DBNull.Value ? "" : Convert.ToString(row["Region Code"]);
        //                        string regionGroup = row["Region Group"] == DBNull.Value ? "" : Convert.ToString(row["Region Group"]);
        //                        string serviceLine = row["Service Line"] == DBNull.Value ? "" : Convert.ToString(row["Service Line"]);
        //                        string practiceLine = row["Practice Line"] == DBNull.Value ? "" : Convert.ToString(row["Practice Line"]);
        //                        string deliverySubUnit = row["Delivery Sub Unit"] == DBNull.Value ? "" : Convert.ToString(row["Delivery Sub Unit"]);
        //                        string projectPU = row["Project PU"] == DBNull.Value ? "" : Convert.ToString(row["Project PU"]);
        //                        string finYear = ddlYearRevenue.SelectedItem.ToString();


        //                        service.InsertRevExpDumpDev(reportingPU, plGroup, classification, allocationType, budgetingUnitCode, subUnitCode,
        //                                    unitCode, puGroup, apr, may, jun, jul, aug, sep, oct, nov, dec, jan, feb, mar, q1, q2, q3, q4, ytd,
        //                                    total, projectType, accountHead, onsiteOffshore, customerCode, customerName, masterCustomerCode,
        //                                    customerPortfolio, masterCustIBU, programCode, trackCode, regionCode, regionGroup, serviceLine,
        //                                    practiceLine, deliverySubUnit, projectPU, finYear);
        //                        rowsupdated++;
        //                    }


        //                    Session["FileName"] = path;
        //                    string ExcelFilePath = Session["FileName"].ToString();
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblSuccess.Text = "Data Uploaded Successfully";
        //                    lblSuccess.Visible = true;
        //                    lblError.Visible = false;
        //                    //}

        //                    //Code to check the count in Excel and Database
        //                    int cnt = 0;
        //                    cnt = service.RevExpDumpCountDev(year);

        //                    lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in Excel : " + fontRed + noOfRows + fontEnd +
        //                       " -No. of records in the table after upload :" + fontRed + cnt;

        //                    //----------END

        //                    // ClientScript.RegisterStartupScript(Page.GetType(), "PopUpMasterClientAlcon", "javascript:PopUpMasterClientAlcon();", true);
        //                }
        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename the sheet to 'Sheet1'";
        //                    lblError.Visible = true;
        //                }
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";
        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }

        //    }
        //}

        //Revenue Exp Upload Prod
        //protected void btnRevExpDevProd_Click(object sender, EventArgs e)
        //{
        //    System.Data.DataTable dtExcel = new System.Data.DataTable();
        //    dtExcel.TableName = "MyExcelData";

        //    string folder = "ExcelOperationsProd";

        //    var MyDir = new DirectoryInfo(Server.MapPath(folder));

        //    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "RevenueExp.xlsx") != null)
        //        System.IO.File.Delete(MyDir.FullName + "\\RevenueExp.xlsx");

        //    string path = MyDir.FullName + "\\RevenueExp.xlsx";// + FileName;

        //    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

        //    OleDbConnection con = new OleDbConnection(SourceConstr);

        //    try
        //    {
        //        string FileName = RevenueUploadProd.FileName;

        //        if (RevenueUploadProd.HasFile)
        //        {
        //            if (FileName.Contains(".xls"))
        //            {
        //                string year = ddlYearRevenueProd.Text;

        //                RevenueUploadProd.SaveAs(path);
        //                string query = "Select * from [Sheet1$]";

        //                //Code to check if sheet is having proper name
        //                con.Open();
        //                DataTable worksheets = con.GetSchema("Tables");
        //                string w = worksheets.Columns["TABLE_NAME"].ToString();
        //                List<string> lstsheetNames = new List<string>();
        //                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

        //                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

        //                if (lstsheetNames.Contains("Sheet1$"))
        //                {

        //                    OleDbDataAdapter data = new OleDbDataAdapter(query, con);

        //                    service.DeleteRevExpProd(year);

        //                    data.Fill(dtExcel);


        //                    int noOfRows = dtExcel.Rows.Count;
        //                    int rowsupdated = 0;

        //                    foreach (DataRow row in dtExcel.Rows)
        //                    {
        //                        Nullable<double> nullableDouble = null;

        //                        string reportingPU = row["Reporting PU"] == DBNull.Value ? "" : Convert.ToString(row["Reporting PU"]);
        //                        string plGroup = row["PL Group"] == DBNull.Value ? "" : Convert.ToString(row["PL Group"]);
        //                        string classification = row["Classification"] == DBNull.Value ? "" : Convert.ToString(row["Classification"]);
        //                        string allocationType = row["Allocation Type"] == DBNull.Value ? "" : Convert.ToString(row["Allocation Type"]);
        //                        string budgetingUnitCode = row["Budgeting Unit Code"] == DBNull.Value ? "" : Convert.ToString(row["Budgeting Unit Code"]);
        //                        string subUnitCode = row["Sub Unit Code"] == DBNull.Value ? "" : Convert.ToString(row["Sub Unit Code"]);
        //                        string unitCode = row["Unit Code"] == DBNull.Value ? "" : Convert.ToString(row["Unit Code"]);
        //                        string puGroup = row["PU Group"] == DBNull.Value ? "" : Convert.ToString(row["PU Group"]);
        //                        Nullable<double> apr = row["Apr"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Apr"]);
        //                        Nullable<double> may = row["May"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["May"]);
        //                        Nullable<double> jun = row["Jun"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Jun"]);
        //                        Nullable<double> jul = row["Jul"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Jul"]);
        //                        Nullable<double> aug = row["Aug"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Aug"]);
        //                        Nullable<double> sep = row["Sep"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Sep"]);
        //                        Nullable<double> oct = row["Oct"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Oct"]);
        //                        Nullable<double> nov = row["Nov"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Nov"]);
        //                        Nullable<double> dec = row["Dec"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Dec"]);
        //                        Nullable<double> jan = row["Jan"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Jan"]);
        //                        Nullable<double> feb = row["Feb"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Feb"]);
        //                        Nullable<double> mar = row["Mar"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Mar"]);
        //                        Nullable<double> q1 = row["Q1"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Q1"]);
        //                        Nullable<double> q2 = row["Q2"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Q2"]);
        //                        Nullable<double> q3 = row["Q3"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Q3"]);
        //                        Nullable<double> q4 = row["Q4"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Q4"]);
        //                        Nullable<double> ytd = row["YTD"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["YTD"]);
        //                        Nullable<double> total = row["Total"] == DBNull.Value ? nullableDouble : Convert.ToDouble(row["Total"]);
        //                        string projectType = row["Project Type"] == DBNull.Value ? "" : Convert.ToString(row["Project Type"]);
        //                        string accountHead = row["Account Head"] == DBNull.Value ? "" : Convert.ToString(row["Account Head"]);
        //                        string onsiteOffshore = row["Onsite/Offshore"] == DBNull.Value ? "" : Convert.ToString(row["Onsite/Offshore"]);
        //                        string customerCode = row["Customer Code"] == DBNull.Value ? "" : Convert.ToString(row["Customer Code"]);
        //                        string customerName = row["Customer Name"] == DBNull.Value ? "" : Convert.ToString(row["Customer Name"]);
        //                        string masterCustomerCode = row["Master Customer Code"] == DBNull.Value ? "" : Convert.ToString(row["Master Customer Code"]);
        //                        string customerPortfolio = row["Customer Portfolio"] == DBNull.Value ? "" : Convert.ToString(row["Customer Portfolio"]);
        //                        string masterCustIBU = row["Master Customer IBU"] == DBNull.Value ? "" : Convert.ToString(row["Master Customer IBU"]);
        //                        string programCode = row["Program Code"] == DBNull.Value ? "" : Convert.ToString(row["Program Code"]);
        //                        string trackCode = row["Track Code"] == DBNull.Value ? "" : Convert.ToString(row["Track Code"]);
        //                        string regionCode = row["Region Code"] == DBNull.Value ? "" : Convert.ToString(row["Region Code"]);
        //                        string regionGroup = row["Region Group"] == DBNull.Value ? "" : Convert.ToString(row["Region Group"]);
        //                        string serviceLine = row["Service Line"] == DBNull.Value ? "" : Convert.ToString(row["Service Line"]);
        //                        string practiceLine = row["Practice Line"] == DBNull.Value ? "" : Convert.ToString(row["Practice Line"]);
        //                        string deliverySubUnit = row["Delivery Sub Unit"] == DBNull.Value ? "" : Convert.ToString(row["Delivery Sub Unit"]);
        //                        string projectPU = row["Project PU"] == DBNull.Value ? "" : Convert.ToString(row["Project PU"]);
        //                        string finYear = ddlYearRevenueProd.SelectedItem.ToString();

        //                        service.InsertRevExpDumpProd(reportingPU, plGroup, classification, allocationType, budgetingUnitCode, subUnitCode,
        //                                    unitCode, puGroup, apr, may, jun, jul, aug, sep, oct, nov, dec, jan, feb, mar, q1, q2, q3, q4, ytd,
        //                                    total, projectType, accountHead, onsiteOffshore, customerCode, customerName, masterCustomerCode,
        //                                    customerPortfolio, masterCustIBU, programCode, trackCode, regionCode, regionGroup, serviceLine,
        //                                    practiceLine, deliverySubUnit, projectPU, finYear);
        //                        rowsupdated++;
        //                    }


        //                    Session["FileName"] = path;
        //                    string ExcelFilePath = Session["FileName"].ToString();
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblSuccess.Text = "Data Uploaded Successfully";
        //                    lblSuccess.Visible = true;
        //                    lblError.Visible = false;
        //                    //}

        //                    //Code to check the count in Excel and Database
        //                    int cnt = 0;
        //                    cnt = service.RevExpDumpCountDev(year);

        //                    lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in Excel : " + fontRed + noOfRows + fontEnd +
        //                       " -No. of records in the table after upload :" + fontRed + cnt;

        //                    //----------END

        //                    // ClientScript.RegisterStartupScript(Page.GetType(), "PopUpMasterClientAlcon", "javascript:PopUpMasterClientAlcon();", true);
        //                }
        //                else
        //                {
        //                    if (con.State.ToString().ToLower() == "open")
        //                        con.Close();
        //                    lblError.Text = "";
        //                    lblError.Text = "Please rename the sheet to 'Sheet1'";
        //                    lblError.Visible = true;
        //                }
        //            }
        //            else
        //            {
        //                if (con.State.ToString().ToLower() == "open")
        //                    con.Close();
        //                lblError.Text = "";
        //                lblError.Text = "File is not in specified Format";
        //                lblError.Visible = true;
        //            }
        //            //lblSuccess.Text = "Data Uploaded Successfully";
        //        }
        //        else
        //        {
        //            lblError.Text = "";
        //            lblError.Text = "Please Select a File";
        //            lblError.Visible = true;
        //            lblSuccess.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State.ToString().ToLower() == "open")
        //            con.Close();

        //        if ((ex.Message + "").Contains("Thread was being aborted."))
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //        else
        //        {
        //            logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        //            throw ex;
        //        }

        //    }
        //}

        protected void drpYer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnRtbrUpload_Click(object sender, EventArgs e)
        {

            System.Data.DataTable dtExcel = new System.Data.DataTable();

            // Alert();

            dtExcel.TableName = "MyExcelData";


            System.Data.DataTable dtExcel1 = new System.Data.DataTable();

            dtExcel1.TableName = "MyExcelData";

            string folder = "ExcelOperations";

            var MyDir = new DirectoryInfo(Server.MapPath(folder));

            if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "RTBR.xlsx") != null)
                System.IO.File.Delete(MyDir.FullName + "\\RTBR.xlsx");

            //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "4", "4");


            string path = MyDir.FullName + "\\RTBR.xlsx";// + FileName;

            string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";


            //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "5", "5");

            OleDbConnection con = new OleDbConnection(SourceConstr);

            //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "6", "6");

            try
            {
                //                string FileName =
                //Server.MapPath(System.IO.Path.GetFileName(FinPulseUpload.PostedFile.FileName.ToString()));
                //                FinPulseUpload.PostedFile.SaveAs(fileName);
                string FileName = RTBRUpload.FileName;
                if (RTBRUpload.HasFile)
                {

                    string fileExt = Path.GetExtension(RTBRUpload.FileName);
                    if ((fileExt.Equals(".xlsx")) || (fileExt.Equals(".xls")) && RTBRUpload.PostedFile.ContentLength != 0)
                    {
                        if (FileName == "RTBR.xlsx" || FileName == "RTBR.xls")
                        {

                            RTBRUpload.SaveAs(path);



                            string query = "Select * from [USD$]";
                            string query1 = "Select * from [NC$]";
                            OleDbDataAdapter data = new OleDbDataAdapter(query, con);
                            OleDbDataAdapter data1 = new OleDbDataAdapter(query1, con);
                            data.Fill(dtExcel);
                            data1.Fill(dtExcel1);
                            int noOfRows = dtExcel.Rows.Count;
                            int noOfRows1 = dtExcel1.Rows.Count;
                            //Code to check if sheet is having proper name
                            con.Open();

                            DataTable worksheets = con.GetSchema("Tables");
                            string w = worksheets.Columns["TABLE_NAME"].ToString();
                            List<string> lstsheetNames = new List<string>();
                            Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

                            worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

                            //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "11", "11");


                            if (lstsheetNames.Contains("NC$"))
                            {
                                //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "12", "12");

                              



                                    // isSuccess = service.DeleteFinPulseDump(yearmonth);


                                    Application app = new Application();
                                    Package package = null;
                                  


                                    //Load DTSX
                                    package = app.LoadPackage(@"D:\ETLRTBR@\ETLRTBR@\Package.dtsx", null);

                                    //Global Package Variable
                                    //Variables vars = package.Variables;
                                    //vars["ServiceLine"].Value = ddlUpload.SelectedItem.ToString();
                                 


                                    //Specify Excel Connection From DTSX Connection Manager
                                    // package.Connections["SourceConnectionExcel"].ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + fileName + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";


                                    //Execute DTSX.
                                    Microsoft.SqlServer.Dts.Runtime.DTSExecResult results = package.Execute();
                                    if (results == Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure)
                                    {
                                        StringBuilder strError = new StringBuilder();
                                        foreach (Microsoft.SqlServer.Dts.Runtime.DtsError local_DtsError in package.Errors)
                                        {
                                            Console.WriteLine("Package Execution results: {0}", local_DtsError.Description.ToString());
                                            Console.WriteLine();
                                            strError.Append(local_DtsError.Description.ToString());
                                            strError.Append("-");
                                        }
                                        lblError.Text = strError.ToString();
                                        lblError.Visible = true;
                                        if (con.State.ToString().ToLower() == "open")
                                            con.Close();
                                    }


                          else
                                    {
                                        lblSuccess.Visible = true;

                                        int cnt = service.FinPulseDumpCountDev();
                                        if (con.State.ToString().ToLower() == "open")
                                            con.Close();
                                        lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in Excel : " + fontRed + noOfRows + fontEnd +
                                            " -No. of records in the table after upload :" + fontRed + cnt;
                            
                                    }

                                }

                               
                            else
                            {
                                if (con.State.ToString().ToLower() == "open")
                                    con.Close();
                                lblError.Text = "";
                                lblError.Text = "Please rename the sheet to 'NC' and 'USD'";
                                lblError.Visible = true;
                                lblSuccess.Visible = false;
                            }
                         
                        }

                         else
                        {
                            if (con.State.ToString().ToLower() == "open")
                                con.Close();
                            lblError.Text = "";
                            lblError.Text = "Please rename the Excel to 'RTBR'";
                            lblError.Visible = true;
                            lblSuccess.Visible = false;
                        }

                    }

                    else
                    {
                        if (con.State.ToString().ToLower() == "open")
                            con.Close();
                        lblError.Text = "";
                        lblError.Text = "File is not in specified Format";
                        lblError.Visible = true;
                        lblSuccess.Visible = false;
                    }
                

                }

                else
                {
                    lblError.Text = "";
                    lblError.Text = "Please Select a File";
                    lblError.Visible = true;
                    lblSuccess.Visible = false;
                }


                        }
              
          
            catch (Exception ex)
            {
                if (con.State.ToString().ToLower() == "open")
                    con.Close();

                if ((ex.Message + "").Contains("Thread was being aborted."))
                    logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                else
                {
                    logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }

            }
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dtExcel = new System.Data.DataTable();

            // Alert();

            dtExcel.TableName = "MyExcelData";


            System.Data.DataTable dtExcel1 = new System.Data.DataTable();

            dtExcel1.TableName = "MyExcelData";

            string folder = "ExcelOperations";

            var MyDir = new DirectoryInfo(Server.MapPath(folder));

            if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "RTBR.xlsx") != null)
                System.IO.File.Delete(MyDir.FullName + "\\RTBR.xlsx");

            //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "4", "4");


            string path = MyDir.FullName + "\\RTBR.xlsx";// + FileName;

            string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";


            //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "5", "5");

            OleDbConnection con = new OleDbConnection(SourceConstr);

            //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "6", "6");

            try
            {
                //                string FileName =
                //Server.MapPath(System.IO.Path.GetFileName(FinPulseUpload.PostedFile.FileName.ToString()));
                //                FinPulseUpload.PostedFile.SaveAs(fileName);
                string FileName = RTBRUpload.FileName;
                if (RTBRUpload.HasFile)
                {

                    string fileExt = Path.GetExtension(RTBRUpload.FileName);
                    if ((fileExt.Equals(".xlsx")) || (fileExt.Equals(".xls")) && RTBRUpload.PostedFile.ContentLength != 0)
                    {
                        if (FileName == "RTBR.xlsx" || FileName == "RTBR.xls")
                        {

                            RTBRUpload.SaveAs(path);



                            
                            string query1 = "Select * from [NC$]";
                         
                            OleDbDataAdapter data1 = new OleDbDataAdapter(query1, con);
                          
                            data1.Fill(dtExcel1);
                       
                            int noOfRows1 = dtExcel1.Rows.Count;
                            //Code to check if sheet is having proper name
                            con.Open();

                            DataTable worksheets = con.GetSchema("Tables");
                            string w = worksheets.Columns["TABLE_NAME"].ToString();
                            List<string> lstsheetNames = new List<string>();
                            Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

                            worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

                            //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "11", "11");


                            if (lstsheetNames.Contains("NC$"))
                            {
                                //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "12", "12");





                                // isSuccess = service.DeleteFinPulseDump(yearmonth);


                                Application app = new Application();
                                Package package = null;



                                //Load DTSX
                                package = app.LoadPackage(@"D:\ETLRTBR@\ETLRTBR@\RTBRNCDump.dtsx", null);

                                //Global Package Variable
                                //Variables vars = package.Variables;
                                //vars["ServiceLine"].Value = ddlUpload.SelectedItem.ToString();



                                //Specify Excel Connection From DTSX Connection Manager
                                // package.Connections["SourceConnectionExcel"].ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + fileName + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";


                                //Execute DTSX.
                                Microsoft.SqlServer.Dts.Runtime.DTSExecResult results = package.Execute();
                                if (results == Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure)
                                {
                                    StringBuilder strError = new StringBuilder();
                                    foreach (Microsoft.SqlServer.Dts.Runtime.DtsError local_DtsError in package.Errors)
                                    {
                                        Console.WriteLine("Package Execution results: {0}", local_DtsError.Description.ToString());
                                        Console.WriteLine();
                                        strError.Append(local_DtsError.Description.ToString());
                                        strError.Append("-");
                                    }
                                    lblError.Text = strError.ToString();
                                    lblError.Visible = true;
                                    if (con.State.ToString().ToLower() == "open")
                                        con.Close();
                                }



                                else
                                {

                                    string cmdtext = "EXEC EAS_SP_RTBRNC_GetCount";
                                    DataSet ds = service.GetDataSet(cmdtext);
                                    DataTable dt1 = ds.Tables[0];
                                    DataTable dt2 = ds.Tables[1];
                                    gvAfterUplaod.Visible = true;
                                    gvBeforeUpload.Visible = true;
                                    gvAfterUplaod.DataSource = dt2;
                                    gvAfterUplaod.DataBind();
                                    gvBeforeUpload.DataSource = dt1;
                                    gvBeforeUpload.DataBind();
                                    if (con.State.ToString().ToLower() == "open")
                                        con.Close();

                                }

                            }


                            else
                            {
                                if (con.State.ToString().ToLower() == "open")
                                    con.Close();
                                lblError.Text = "";
                                lblError.Text = "Please rename the sheet to 'NC'";
                                lblError.Visible = true;
                                lblSuccess.Visible = false;
                            }

                        }

                        else
                        {
                            if (con.State.ToString().ToLower() == "open")
                                con.Close();
                            lblError.Text = "";
                            lblError.Text = "Please rename the Excel to 'RTBR'";
                            lblError.Visible = true;
                            lblSuccess.Visible = false;
                        }

                    }

                    else
                    {
                        if (con.State.ToString().ToLower() == "open")
                            con.Close();
                        lblError.Text = "";
                        lblError.Text = "File is not in specified Format";
                        lblError.Visible = true;
                        lblSuccess.Visible = false;
                    }


                }

                else
                {
                    lblError.Text = "";
                    lblError.Text = "Please Select a File";
                    lblError.Visible = true;
                    lblSuccess.Visible = false;
                }


            }
              
          

            catch (Exception ex)
            {
                if (con.State.ToString().ToLower() == "open")
                    con.Close();

                if ((ex.Message + "").Contains("Thread was being aborted."))
                    logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                else
                {
                    logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }

            }

        }

        protected void gvBeforeUpload_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView objGridView = (GridView)sender;


                GridViewRow objgridviewrow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell objtablecell = new TableCell();


                AddMergedCells(objgridviewrow, objtablecell, 12, "Before Uplaod", "#c41502");



                objGridView.Controls[0].Controls.AddAt(0, objgridviewrow);
            }
         }
        protected void gvAfterUplaod_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView objGridView = (GridView)sender;


                GridViewRow objgridviewrow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell objtablecell = new TableCell();


                AddMergedCells(objgridviewrow, objtablecell, 12, "After Uplaod", "#c41502");



                objGridView.Controls[0].Controls.AddAt(0, objgridviewrow);
            }
        }
            protected void AddMergedCells(GridViewRow objgridviewrow,
  TableCell objtablecell, int colspan, string celltext, string backcolor)
            {



                try
                {
                    objtablecell = new TableCell();
                    objtablecell.Text = celltext;
                    objtablecell.Font.Bold = true;
                    objtablecell.ColumnSpan = colspan;
                    objtablecell.Style.Add("background-color", backcolor);
                    objtablecell.Style.Add("border-bottom-color", "#878484");// "#c41502");

                    objtablecell.HorizontalAlign = HorizontalAlign.Center;
                    // objtablecell.BorderColor = System.Drawing.Color.FromName("#c41502");//("#525252");
                    objtablecell.BorderColor = System.Drawing.Color.DarkSlateGray;
                    objtablecell.ForeColor = System.Drawing.Color.FromName("#ffcb8b");

                    objgridviewrow.Cells.Add(objtablecell);
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

            protected void btnVerifyPBS_Click(object sender, EventArgs e)
            {
                System.Data.DataTable dtExcel = new System.Data.DataTable();

                // Alert();

                dtExcel.TableName = "MyExcelData";


                System.Data.DataTable dtExcel1 = new System.Data.DataTable();

                dtExcel1.TableName = "MyExcelData";

                string folder = "ExcelOperations";

                var MyDir = new DirectoryInfo(Server.MapPath(folder));

                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == "AlconPBS.xlsx") != null)
                    System.IO.File.Delete(MyDir.FullName + "\\AlconPBS.xlsx");



                //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "4", "4");


                string path = MyDir.FullName + "\\AlconPBS.xlsx";// + FileName;

                string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + path + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";


                //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "5", "5");

                OleDbConnection con = new OleDbConnection(SourceConstr);

                //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "6", "6");

                try
                {
                    //                string FileName =
                    //Server.MapPath(System.IO.Path.GetFileName(FinPulseUpload.PostedFile.FileName.ToString()));
                    //                FinPulseUpload.PostedFile.SaveAs(fileName);
                    string FileName = AlconUpload.FileName;
                    if (AlconUpload.HasFile)
                    {

                        string fileExt = Path.GetExtension(AlconUpload.FileName);
                        if ((fileExt.Equals(".xlsx")) || (fileExt.Equals(".xls")) && AlconUpload.PostedFile.ContentLength != 0)
                        {
                            if (FileName == "AlconPBS.xlsx" || FileName == "AlconPBS.xls")
                            {

                                AlconUpload.SaveAs(path);



                                string query = "Select * from [AlconPBS$]";

                                OleDbDataAdapter data = new OleDbDataAdapter(query, con);

                                data.Fill(dtExcel);

                                int noOfRows = dtExcel.Rows.Count;

                                //Code to check if sheet is having proper name
                                con.Open();

                                DataTable worksheets = con.GetSchema("Tables");
                                string w = worksheets.Columns["TABLE_NAME"].ToString();
                                List<string> lstsheetNames = new List<string>();
                                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

                                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);

                                //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "11", "11");


                                if (lstsheetNames.Contains("AlconPBS$"))
                                {



                                    Application app = new Application();
                                    Package package = null;



                                    //Load DTSX
                                    package = app.LoadPackage(@"D:\ETLPBS@\ETLPBS@\Package.dtsx", null);

                                    //Execute DTSX.
                                    Microsoft.SqlServer.Dts.Runtime.DTSExecResult results = package.Execute();
                                    if (results == Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure)
                                    {
                                        StringBuilder strError = new StringBuilder();
                                        foreach (Microsoft.SqlServer.Dts.Runtime.DtsError local_DtsError in package.Errors)
                                        {
                                            Console.WriteLine("Package Execution results: {0}", local_DtsError.Description.ToString());
                                            Console.WriteLine();
                                            strError.Append(local_DtsError.Description.ToString());
                                            strError.Append("-");
                                        }
                                        lblError.Text = strError.ToString();
                                        lblError.Visible = true;
                                        if (con.State.ToString().ToLower() == "open")
                                            con.Close();
                                    }

                                }
                                lblSuccess.Visible = true;
                                int cnt = 0;
                                cnt = service.AlconDumpCountDev();
                                if (con.State.ToString().ToLower() == "open")
                                    con.Close();
                                lblSuccess.Text = "Data Uploaded Successfully" + "</br>" + " No. of Records in Excel : " + fontRed + noOfRows + fontEnd +
                                    " -No. of records in the table after upload :" + fontRed + cnt;

                            }

                            else
                            {
                                if (con.State.ToString().ToLower() == "open")
                                    con.Close();
                                lblError.Text = "";
                                lblError.Text = "Please rename the sheet to 'AlconPBS'";
                                lblError.Visible = true;
                                lblSuccess.Visible = false;
                            }

                        }

                        else
                        {
                            if (con.State.ToString().ToLower() == "open")
                                con.Close();
                            lblError.Text = "";
                            lblError.Text = "File is not in specified Format(.xls or .xlsx)";
                            lblError.Visible = true;
                            lblSuccess.Visible = false;
                        }


                    }

                    else
                    {
                        lblError.Text = "";
                        lblError.Text = "Please Select a File";
                        lblError.Visible = true;
                        lblSuccess.Visible = false;
                    }


                }
                catch (Exception ex)
                {
                    if (con.State.ToString().ToLower() == "open")
                        con.Close();

                    if ((ex.Message + "").Contains("Thread was being aborted."))
                        logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    else
                    {
                        logger.LogErrorToServer(BEData.Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                        throw ex;
                    }

                }

          
            }


    }

