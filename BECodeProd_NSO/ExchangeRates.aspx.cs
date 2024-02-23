using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VBIDE = Microsoft.Vbe.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using BEData;
using BEData.BusinessEntity;
using System.Data.Common;
using System.Data.OleDb;
using Microsoft.SqlServer.Dts;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Diagnostics;
using Microsoft.SqlServer.Dts.Runtime;
using System.Web.UI.HtmlControls;
namespace BECodeProd
{
    public partial class ExchangeRates : BasePage
    {
        Logger logger = new Logger();
        public string fileName = "BEData.ExchangeRate.cs";
        BEDL objbe = new BEDL();
        public DateTime dateTime = DateTime.Today;
        string userID = "";
        DataTable dtExcelData = new DataTable();
        DataTable dtExcel2Data = new DataTable();
        DataTable dtExcel3Data = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            if (Request.QueryString["flag"] == "N")
            {
                HtmlGenericControl sitemap = (HtmlGenericControl)Master.FindControl("SiteMap1");
                sitemap.Attributes.Add("style", "display:none");
            }
            userID = Session["UserID"] + "";
            if (Page.IsPostBack)
            {
                if (DateTime.Now.DayOfWeek.Equals("Friday"))
                {
                    cbxExcp.Visible = true;
                }
                else
                {
                    cbxExcp.Visible = false;
                }
                if (cbxExcp.Checked == true)
                {
                    cbxExcp.Visible = true;
                    
                }
                else
                {
                    
                    if (DateTime.Now.DayOfWeek.Equals("Friday") && ddlType.SelectedIndex == 1)
                    {
                        
                        btnUpload.Enabled = false;
                    }
                    else
                    {
                        btnUpload.Enabled = true;
                    }
                }
                

                //ddlYear.AppendDataBoundItems = false;
            }
            else
            {
                txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");

                //onload
                string isValidEntry = Session["Login"] + "";
                if (!isValidEntry.Equals("1"))
                    Response.Redirect("UnAuthorised.aspx");


                ddlYear.AppendDataBoundItems = true;
                ddlQtr.AppendDataBoundItems = true;

                ddlYear.DataSource = objbe.GetExchangeActYear();
                ddlYear.DataTextField = "txtYear";
                ddlYear.DataValueField = "txtYear";
                ddlYear.DataBind();
                ddlQtr.DataSource = objbe.GetExchangeActQtr();
                ddlQtr.DataTextField = "txtQuarterName";
                ddlQtr.DataValueField = "txtQuarterName";
                ddlQtr.DataBind();

                ddlYear1.DataSource = objbe.GetExchangeActYear();
                ddlYear1.DataTextField = "txtYear";
                ddlYear1.DataValueField = "txtYear";
                ddlYear1.DataBind();
                ddlQtr1.DataSource = objbe.GetExchangeActQtr();
                ddlQtr1.DataTextField = "txtQuarterName";
                ddlQtr1.DataValueField = "txtQuarterName";
                ddlQtr1.DataBind();
                if (DateTime.Now.DayOfWeek.Equals("Friday"))
                {
                    cbxExcp.Visible = true;
                }
                else
                {
                    cbxExcp.Visible = false;
                }

                if (DateTime.Now.DayOfWeek.Equals("Friday") && ddlType.SelectedIndex == 1)
                {
                   
                    btnUpload.Enabled = false;
                }
                
            }
        }



        static bool IsValidSqlDateTimeNative(string someval,ref DateTime dt)
        {
            bool valid = false;

            

            if (DateTime.TryParseExact(someval, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                valid = true;
            }

            return valid;
        }

     


        public static bool IsDateTime(string txtDate)
        {
            DateTime tempDate;

            return DateTime.TryParse(txtDate, out tempDate) ? true : false;
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            int flag = 0;
            string machineUser = string.Empty;
            string[] machineUsers = HttpContext.Current.User.Identity.Name.Split('\\');
            if (machineUsers.Length == 2)
                machineUser = machineUsers[1];

            string date = DateTime.Now.ToString("dd-MM-yyyy");
            if (ddlYear1.SelectedIndex == 0 || ddlQtr1.SelectedIndex == 0 || ddlType.SelectedIndex == 0)
            {
                PopUp("Please select an UpdateType , Year , Quarter and Month !!! ");
            }
            else
            {
                DateTime uploadDate = DateTime.Now;

                bool isvaliddate = IsValidSqlDateTimeNative(txtdate.Text, ref uploadDate);
                               
                if (isvaliddate == true)
                {
                    
                    if (fuUpload.HasFile)
                    {
                        if (fuUpload.PostedFile.ContentLength != 0)
                        {
                            string fileExtension = Path.GetExtension(fuUpload.FileName);
                            if (fileExtension == ".xls")
                            {
                                string fileName = Path.GetFileName(fuUpload.PostedFile.FileName);
                                if (fileName.Contains("RTBR") == true)
                                {
                                    if (fileName.Contains("RTBR") == true && ddlType.SelectedValue == "Daily")
                                    {
                                        try
                                        {

                                            if (ddlYear1.SelectedIndex == 0 || ddlQtr1.SelectedIndex == 0 || ddlType.SelectedIndex == 0)
                                            {
                                                PopUp("Please select  year and Qtr");
                                            }
                                            else
                                            {

                                                int x, y, z;
                                                if (cbQuarter1.Items[0].Selected == true)
                                                {
                                                    x = 1;
                                                }
                                                else
                                                {
                                                    x = 0;
                                                }
                                                if (cbQuarter1.Items[1].Selected == true)
                                                {
                                                    y = 1;
                                                }
                                                else
                                                {
                                                    y = 0;
                                                }
                                                if (cbQuarter1.Items[2].Selected == true)
                                                {
                                                    z = 1;
                                                }
                                                else
                                                {
                                                    z = 0;
                                                }

                                                string path = string.Empty;
                                                if (fuUpload.HasFile)
                                                {
                                                 
                                                    path = string.Concat(Server.MapPath("~/ExcelOperations/" + fuUpload.FileName));

                                                    fuUpload.SaveAs(path);
                                                  
                                                    // Connection String to Excel Workbook
                                                    string conString = string.Empty;

                                                    string extension = Path.GetExtension(fuUpload.PostedFile.FileName);
                                                    if (extension == null)
                                                    {
                                                        PopUp("Please Upload a file!!");
                                                    }
                                                    else if (extension == ".xlsx")
                                                    {
                                                        PopUp("Please Upload a Excel 97-2003 file!!");


                                                    }
                                                    else if (extension == ".xls")
                                                    {
                                                        switch (extension)
                                                        {

                                                            case ".xls": //Excel 97-03

                                                                //conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                                                                //conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                                                                conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=yes;IMEX=1;\""; 
                                                                
                                                                break;

                                                            //case ".xlsx": //Excel 07 or higher

                                                            //    //conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                                                            //    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                                                            //    break;


                                                        }

                                                        conString = string.Format(conString, path);

                                                        if (extension == ".xls" /*|| extension == ".xlsx"*/)
                                                        {

                                                            using (OleDbConnection excel_con = new OleDbConnection(conString))
                                                            {

                                                                excel_con.Open();

                                                                string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();

                                                                DataTable worksheets = excel_con.GetSchema("Tables");
                                                                string w = worksheets.Columns["TABLE_NAME"].ToString();
                                                                List<string> lstsheetNames = new List<string>();
                                                                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

                                                                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);




                                                                if (lstsheetNames.Contains("NC$") && lstsheetNames.Contains("USD$"))
                                                                {



                                                                    //[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.
                                                                    if (lstsheetNames.Contains("NC$"))
                                                                    {
                                                                        dtExcel2Data.Columns.AddRange(new DataColumn[2] { new DataColumn("Project Currency", typeof(string)), new DataColumn("Annual RTBR", typeof(float)) });
                                                                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT *  FROM [NC$]", excel_con))
                                                                        {
                                                                            oda.Fill(dtExcel2Data);
                                                                        }
                                                                        excel_con.Close();
                                                                    }
                                                                    if (lstsheetNames.Contains("USD$"))
                                                                    {
                                                                        dtExcel3Data.Columns.AddRange(new DataColumn[2] { new DataColumn("Project Currency", typeof(string)), new DataColumn("Annual RTBR", typeof(float)) });
                                                                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT *  FROM [USD$]", excel_con))
                                                                        {
                                                                            oda.Fill(dtExcel3Data);
                                                                        }
                                                                        excel_con.Close();
                                                                    }
                                                                    string consString = ConfigurationManager.AppSettings["DemandCaptureConnectionString"].ToString();



                                                                    if (dtExcel2Data != null && dtExcel3Data != null)
                                                                    {


                                                                        //string SourceConstr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + path + "';Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";

                                                                        string SourceConstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=yes;IMEX=1;\""; 
                                                                        //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "5", "5");

                                                                        OleDbConnection con = new OleDbConnection(SourceConstr);

                                                                        Application app = new Application();
                                                                        Package package = null;

                                                                        string folderpkg = "ETL";

                                                                        var MyDirpkg = new DirectoryInfo(Server.MapPath(folderpkg));

                                                                        //Load DTSX
                                                                        string ExchangeRatesKey = ConfigurationManager.AppSettings["ExchangeRatesDaily"].ToString();
                                                                        string pathpkg = ExchangeRatesKey;
                                                                        //string pathpkg = @"D:\Exchange ETL Prod\ExchRatesUpload\ExchRatesUpload\ExchRatesDaily.dtsx";

                                                                        package = app.LoadPackage(pathpkg, null);
                                                                        Microsoft.SqlServer.Dts.Runtime.DTSExecResult results;

                                                                        //Specify Excel Connection From DTSX Connection Manager
                                                                        // package.Connections["SourceConnectionExcel"].ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + fileName + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";


                                                                        //Execute DTSX.
                                                                        results = package.Execute();
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
                                                                            //string cmd2 = "select distinct Dumpdate from ExchRatesDailyMain";
                                                                            //DataSet ds2 = objbe.GetDataSet(cmd2);
                                                                            //DataTable dt2 = ds2.Tables[0];
                                                                            string dbdt = DateTime.Now.ToString("yyyy-MM-dd");//dt2.Rows[0]["Dumpdate"].ToString();
                                                                            string apdt = DateTime.Now.ToString("yyyy-MM-dd");



                                                                            //int cnt = objbe.FinPulseDumpCountDev();
                                                                            if (con.State.ToString().ToLower() == "open")
                                                                                con.Close();
                                                                            if (apdt == dbdt)
                                                                            {
                                                                                flag = 1;
                                                                                PopUp("Data Uploaded Successfully as on " + date + " !!");
                                                                            }
                                                                            else
                                                                            {
                                                                                PopUp("Data Upload Unsuccessful " + date + " !!");
                                                                            }

                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        PopUp("Please select the correct excel to upload the data");
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    PopUp("Please change the name of the sheet to NC and USD");
                                                                }

                                                            }
                                                        }
                                                    }
                                                    //PopUp("Uploaded Successfully!!!");

                                                }
                                                else
                                                {
                                                    PopUp("Excel is empty");
                                                }



                                                int status = objbe.UpdateActExchangeRate(ddlType.SelectedValue, ddlQtr1.SelectedValue, ddlYear1.SelectedValue, x, y, z);
                                                if (status == 0)
                                                {
                                                    flag = 1;

                                                    PopUp("DATA Uploaded & Updated succesfully as on " + date + " !!");
                                                }
                                                else
                                                {
                                                    PopUp("Some Error");
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
                                    else
                                    {
                                        PopUp("RTBR file can be loaded only for the type Daily");
                                    }

                                }
                                else if (fileName.Contains("ExchRates_Upload") == true)
                                {
                                    if (fileName.CompareTo("ExchRates_Upload.xlsx") == 0 || fileName.CompareTo("ExchRates_Upload.xls") == 0)
                                    {
                                        try
                                        {

                                            if (ddlYear1.SelectedIndex == 0 || ddlQtr1.SelectedIndex == 0 || ddlType.SelectedIndex == 0)
                                            {
                                                PopUp("Please select  year and Qtr");
                                            }
                                            else
                                            {

                                                int x, y, z;
                                                if (cbQuarter1.Items[0].Selected == true)
                                                {
                                                    x = 1;
                                                }
                                                else
                                                {
                                                    x = 0;
                                                }
                                                if (cbQuarter1.Items[1].Selected == true)
                                                {
                                                    y = 1;
                                                }
                                                else
                                                {
                                                    y = 0;
                                                }
                                                if (cbQuarter1.Items[2].Selected == true)
                                                {
                                                    z = 1;
                                                }
                                                else
                                                {
                                                    z = 0;
                                                }

                                                string path = string.Empty;
                                                if (fuUpload.HasFile)
                                                {

                                                    path = string.Concat(Server.MapPath("~/ExcelOperations/" + fuUpload.FileName));

                                                    fuUpload.SaveAs(path);


                                                    // Connection String to Excel Workbook
                                                    string conString = string.Empty;

                                                    string extension = Path.GetExtension(fuUpload.PostedFile.FileName);
                                                    if (extension == null)
                                                    {
                                                        PopUp("Please Upload a file!!");
                                                    }
                                                    else if (extension == ".xlsx")
                                                    {
                                                        PopUp("Please Upload a Excel 97-2003 file!!");


                                                    }
                                                    else if (extension == ".xls")
                                                    {
                                                        switch (extension)
                                                        {

                                                            case ".xls": //Excel 97-03

                                                                //conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                                                               // conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                                                                conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=yes;IMEX=1;\""; 
                                                               break;

                                                            //case ".xlsx": //Excel 07 or higher

                                                            //    //conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                                                            //    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                                                            //    break;


                                                        }

                                                        conString = string.Format(conString, path);

                                                        if (extension == ".xls" /*|| extension == ".xlsx"*/)
                                                        {

                                                            using (OleDbConnection excel_con = new OleDbConnection(conString))
                                                            {

                                                                excel_con.Open();

                                                                string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();

                                                                DataTable worksheets = excel_con.GetSchema("Tables");
                                                                string w = worksheets.Columns["TABLE_NAME"].ToString();
                                                                List<string> lstsheetNames = new List<string>();
                                                                Action<DataRow> actionToGetSheetName = (k) => { lstsheetNames.Add(k["TABLE_NAME"] + ""); };

                                                                worksheets.Rows.OfType<DataRow>().ToList().ForEach(actionToGetSheetName);




                                                                if (lstsheetNames.Contains("ExchangeRates$"))
                                                                {



                                                                    //[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.

                                                                    dtExcelData.Columns.AddRange(new DataColumn[2] { new DataColumn("From", typeof(string)), 
                                     new DataColumn("USD", typeof(float)) });



                                                                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT *  FROM [ExchangeRates$]", excel_con))
                                                                    {

                                                                        oda.Fill(dtExcelData);

                                                                    }

                                                                    excel_con.Close();


                                                                    string consString = ConfigurationManager.AppSettings["DemandCaptureConnectionString"].ToString();

                                                                    if (dtExcelData != null)
                                                                    {


                                                                        //string SourceConstr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + path + "';Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";

                                                                        string SourceConstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=yes;IMEX=1;\""; 
                                                                        //logger.LogErrorToServer(BEData.Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, "5", "5");

                                                                        OleDbConnection con = new OleDbConnection(SourceConstr);

                                                                        Application app = new Application();
                                                                        Package package = null;

                                                                        string folderpkg = "ETL";

                                                                        var MyDirpkg = new DirectoryInfo(Server.MapPath(folderpkg));

                                                                        //Load DTSX
                                                                        string ExchangeRatesKey = ConfigurationManager.AppSettings["ExchangeRates"].ToString();
                                                                        string pathpkg = ExchangeRatesKey;
                                                                       // string pathpkg = @"D:\Exchange ETL Prod\ExchRatesUpload\ExchRatesUpload\ExchRates.dtsx";

                                                                        package = app.LoadPackage(pathpkg, null);
                                                                        Microsoft.SqlServer.Dts.Runtime.DTSExecResult results;

                                                                        //Specify Excel Connection From DTSX Connection Manager
                                                                        // package.Connections["SourceConnectionExcel"].ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + fileName + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";


                                                                        //Execute DTSX.
                                                                        results = package.Execute();
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
                                                                            string cmd2 = "select distinct Dumpdate from BECurrConvRate_Dump";
                                                                            DataSet ds2 = objbe.GetDataSet(cmd2);
                                                                            DataTable dt2 = ds2.Tables[0];
                                                                            string dbdt = dt2.Rows[0]["Dumpdate"].ToString();
                                                                            string apdt = DateTime.Now.ToString("yyyy-MM-dd");



                                                                            //int cnt = objbe.FinPulseDumpCountDev();
                                                                            if (con.State.ToString().ToLower() == "open")
                                                                                con.Close();
                                                                            if (apdt == dbdt)
                                                                            {
                                                                                flag = 1;
                                                                                PopUp("Data Uploaded Successfully as on " + date + " !!");
                                                                            }
                                                                            else
                                                                            {
                                                                                PopUp("Data Upload Unsuccessful " + date + " !!");
                                                                            }

                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        PopUp("Please select the correct excel to upload the data");
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    PopUp("Please change the name of the sheet to ExchangeRates");
                                                                }

                                                            }
                                                        }
                                                    }
                                                    //PopUp("Uploaded Successfully!!!");

                                                }
                                                else
                                                {
                                                    PopUp("Excel is empty");
                                                }



                                                int status = objbe.UpdateActExchangeRate(ddlType.SelectedValue, ddlQtr1.SelectedValue, ddlYear1.SelectedValue, x, y, z);
                                                if (status == 0)
                                                {
                                                    flag = 1;

                                                    PopUp("DATA Uploaded & Updated succesfully as on " + date + " !!");
                                                }
                                                else
                                                {
                                                    PopUp("Some Error");
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
                                    else
                                    {
                                        PopUp("Please Rename the file to ExchRates_Upload and remove extra columns and rows");
                                    }
                                }
                                else
                                {
                                    PopUp("Please verify the file to be loaded");
                                }
                            }
                            else
                            {
                                PopUp("Please Upload an excel file with .xls extension");
                            }
                        }
                        else
                        {
                            PopUp("Please select a file to upload!");
                        }
                    }
                    else
                    {
                        PopUp("Please select a file to upload!");
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Invalid Date";
                }


                if (flag == 1)
                {
                    objbe.UpdateDataLoadTracker("ExchangeRates", fuUpload.FileName, machineUser, uploadDate.ToString());
                }
            }

        }

        protected void PopUp(string msg)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "", "alert('" + msg + "');", true);
        } // EO PopUp()

        protected void dwldTemp_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlYear.SelectedIndex == 0 || ddlQtr.SelectedIndex == 0)
                {
                    PopUp("Please select the Year and the Quarter");
                }
                else
                {
                    DataTable dt = objbe.FetchMonthlyActRates(ddlQtr.SelectedValue, ddlYear.SelectedValue);
                    if (dt.Rows.Count < 0)
                        return;
                    var fpath = string.Empty;
                    if (Directory.Exists(Server.MapPath("ExcelOperations")) == false)
                        Directory.CreateDirectory(Server.MapPath("ExcelOperations"));

                    string user = Session["UserID"].ToString();
                    //fpath = Server.MapPath("ExcelOperations/ExchangeRates" + "_" + Session["UserID"].ToString() + "_" + DateTime.Now.ToString() + ".xls");
                    fpath = Server.MapPath("ExcelOperations/ExchangeRates" + "_" + DateTime.Now.ToString("ddMMM_HHmm") + "IST_" + Session["UserID"].ToString() + ".xls");
                    if (File.Exists(fpath) == false)
                        File.Create(fpath).Close();
                    else
                        File.Create(fpath).Close();
                    if (fpath.Trim() != string.Empty)
                        DataTableToExcel(dt, fpath, "");
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

        private void DataTableToExcel(DataTable dtExport, string path, string tableName)
        {
            try
            {
                if (path == string.Empty)
                    return;
                StreamWriter SWriter = new StreamWriter(path);
                string str = string.Empty;
                Int32 colspan = dtExport.Columns.Count;
                str += "<Table border=2><TR>";
                foreach (DataColumn DBCol in dtExport.Columns)
                {
                    str += "<TD bgcolor='808080'>" + DBCol.ColumnName + "</TD>";
                }
                str += "</TR>";

                foreach (DataRow DBCol in dtExport.Rows)
                {
                    str += "<TR>";
                    for (int i = 0; i < dtExport.Columns.Count; i++)
                    {
                        str += "<TD>" + DBCol[i].ToString() + "</TD>";
                    }
                    str += "</TR>";
                }


                SWriter.WriteLine(str);
                SWriter.Flush();
                SWriter.Close();
                if (path.Length > 5)
                    DownloadFile(path);
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

        private void DownloadFile(string FPath)
        {
            try
            {
                bool forceDownload = true;
                String strRequest = Request.QueryString["file"];
                FileInfo file = new FileInfo(FPath);

                //string ext = Path.GetExtension(FPath);
                //string type = "";
                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                ////Response.ContentType = "application/Excel";
                //Response.AppendHeader("content-disposition", "attachment;  filename=" + file.Name);
                //Response.WriteFile(file.FullName);
                //Response.Flush();
                //Response.Close();
                //Response.End();

                Response.Clear();
                Response.AppendHeader("content-disposition",
                        "attachment; filename=" + file.Name);
                Response.Charset = "";

                Response.ContentType = "application/vnd.xls";
                Response.WriteFile(FPath);
                //ClientScript.RegisterStartupScript(this.GetType(), "isvaliduploadClose", "isvaliduploadClose();", true);

                Response.Flush();

                Response.End();



                //try
                //{
                //    //Response.End();
                //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                //}
                //catch(Exception ex)
                //{
                //    Debug.WriteLine(ex.Message);
                //    Debug.WriteLine(ex.StackTrace);
                //    Debug.WriteLine(ex.InnerException.ToString());
                //}

                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                //if (ext != null)
                //{
                //    switch (ext.ToLower())
                //    {
                //        case ".htm":
                //        case ".html":
                //            type = "text/HTML";
                //            break;

                //        case ".txt":
                //            type = "text/plain";
                //            break;



                //        case ".csv":
                //        case ".xls":
                //        case ".xlsx":
                //            type = "Application/x-msexcel";
                //            break;
                //    }
                //}
                //if (forceDownload)
                //{
                //    Response.AppendHeader("content-disposition", "attachment; filename=" + file.Name);
                //}
                //if (type != "")
                //{
                //    Response.ContentType = type;
                //}

                //Response.WriteFile(file.FullName);
                //Response.End();
                //Response.AddHeader("content-disposition", "attachment; filename=" + file.Name);
                //Response.AddHeader("Content-Type", "application/Excel");
                //Response.ContentType = "application/vnd.xls";
                //Response.AddHeader("Content-Length", file.Length.ToString());
                //Response.WriteFile(file.FullName);
                //Response.End();
            }
            catch (Exception ex)
            {
                if ((ex.Message + "").Contains("Thread was being aborted."))
                {
                    //oModule = null;
                    //oBook = null;
                    //oExcel = null;
                    GC.Collect();
                    logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }
                else
                {
                    //oModule = null;
                    //oBook = null;
                    //oExcel = null;
                    GC.Collect();
                    logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }
            }
        }
        
        protected void ddlQtr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlYear.SelectedIndex == 0 || ddlQtr.SelectedIndex == 0)
            {
                gvData.Visible = false;
                PopUp("Please select the Year and the Quarter ");
            }
            else
            {
                gvData.Visible = true;
                BindGrid();

                string currentQuarter = ddlQtr.SelectedValue.ToString();
                string currentYear = ddlYear.SelectedValue.ToString();



            }


        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlYear.SelectedIndex == 0)
            {
                //ddlQtr.Visible = false;
                gvData.Visible = false;
                //cbQuarter.Visible = false;
                PopUp("Please select the Year and the Quarter ");
            }
            else
            {
                ddlQtr.Visible = true;

            }


        }

        private void BindGrid()
        {
            gvData.DataSource = objbe.FetchMonthlyActRates(ddlQtr.SelectedValue, ddlYear.SelectedValue);
            gvData.DataBind();
        }


        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void ddlYear1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlYear1.SelectedIndex == 0 || ddlType.SelectedIndex == 0)
            {
                //ddlQtr1.Visible = false;

                cbQuarter1.Visible = false;
                PopUp("Please select the Year and the Quarter and the Update Type");
            }
            else
            {
                ddlQtr1.Visible = true;

            }
        }

        protected void ddlQtr1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlYear1.SelectedIndex == 0 || ddlQtr1.SelectedIndex == 0 || ddlType.SelectedIndex == 0)
            {
                cbQuarter1.Visible = false;
                PopUp("Please select the Year and the Quarter and the Update Type");
            }
            else
            {

                cbQuarter1.Visible = true;


                string currentQuarter = ddlQtr1.SelectedValue.ToString();
                string currentYear = ddlYear1.SelectedValue.ToString();

                // Month1 / Month2 / Month3
                string _month1 = string.Empty;
                string _month2 = string.Empty;
                string _month3 = string.Empty;
                if (currentQuarter == "Q4")
                {
                    _month1 = "Jan";
                    _month2 = "Feb";
                    _month3 = "Mar";
                }
                else if (currentQuarter == "Q1")
                {
                    _month1 = "Apr";
                    _month2 = "May";
                    _month3 = "Jun";
                }
                else if (currentQuarter == "Q2")
                {
                    _month1 = "Jul";
                    _month2 = "Aug";
                    _month3 = "Sep";
                }
                else
                {
                    _month1 = "Oct";
                    _month2 = "Nov";
                    _month3 = "Dec";
                }
                if (ddlQtr1.SelectedIndex != 0)
                {
                    cbQuarter1.Visible = true;

                    cbQuarter1.Items.Clear();
                    cbQuarter1.Items.Add(_month1);
                    cbQuarter1.Items.Add(_month2);
                    cbQuarter1.Items.Add(_month3);
                }
                else
                {
                    cbQuarter1.Visible = false;
                    PopUp("Select a Quarter");

                }
                if (ddlType.SelectedValue.ToString() == "Daily" || ddlType.SelectedValue.ToString() == "Weekly" || ddlType.SelectedValue.ToString() == "Monthly")
                {
                    string cmd = "SELECT distinct [MONTH] as Month  FROM [BEPortalConfig] where [quarter]='" + currentQuarter + "' and [Year]='" + currentYear + "' and (Month is Not Null or Month != 'NA')";

                    DataSet ds = objbe.GetDataSet(cmd);
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {

                        for (int i = 0; i < cbQuarter1.Items.Count; i++)
                        {

                            cbQuarter1.Items[i].Selected = true;
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["Month"].ToString() != "NA")
                            {
                                cbQuarter1.Items.FindByValue(dt.Rows[i]["Month"].ToString()).Selected = false;
                            }

                        }

                    }
                }
                else
                {
                    cbQuarter1.Visible = false;
                }

            }
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Sortorder;
            cbxExcp.Checked = false;
            if (ddlType.SelectedValue.ToString() == "Weekly" || ddlType.SelectedValue.ToString() == "Daily")
            {
                ddlQtr1.Visible = true;
                ddlYear1.Visible = true;
                cbQuarter1.Visible = true;
                //fuUpload.Visible = true;
                fuUpload.Attributes.Add("style", "display:block");
                btnUpload.Visible = true;
                if (DateTime.Now.DayOfWeek.Equals("Friday"))
                {
                    btnUpload.Enabled = false;
                }
                Sortorder = 37;
            }
            else if (ddlType.SelectedValue.ToString() == "Monthly")
            {
                ddlQtr1.Visible = true;
                ddlYear1.Visible = true;
                cbQuarter1.Visible = true;
                //fuUpload.Visible = true;
                fuUpload.Attributes.Add("style", "display:block");
                btnUpload.Visible = true;
                Sortorder = 19;
            }
            else if (ddlType.SelectedValue.ToString() == "Quarterly")
            {
                ddlQtr1.Visible = true;
                ddlYear1.Visible = true;
                cbQuarter1.Visible = false;
                //fuUpload.Visible = true;
                fuUpload.Attributes.Add("style", "display:block");
                btnUpload.Visible = true;
                Sortorder = 19;
            }
            else
            {
                divInstruction.Visible = false;
                //ddlQtr1.Visible = false;
                //ddlYear1.Visible = false;
                cbQuarter1.Visible = false;
                ////fuUpload.Visible = false;
                //fuUpload.Attributes.Add("style", "display:none");
                //btnUpload.Visible = false;
                PopUp("Please select the Year and the Quarter and the Update Type");
                Sortorder = 0;
            }
            if (Sortorder == 19 || Sortorder == 37)
            {
                DataTable dtInstruction = new DataTable();
                dtInstruction = objbe.GetDataLoad_Instruction(Sortorder);
                ViewState["DataInstruction"] = dtInstruction;
                divInstruction.Visible = true;
                string str = dtInstruction.Rows[0][0].ToString();
                txtinstruction.Text = str;
            }
        }

        protected void lbDownload_Click(object sender, EventArgs e)
        {
            if (pnlDownload.Visible == false)
            {
                pnlDownload.Visible = true;
                if (upUpload.Visible == true)
                {
                    upUpload.Visible = false;
                }
            }
            else
            {
                pnlDownload.Visible = false;
            }
        }

        protected void lbUpload_Click(object sender, EventArgs e)
        {
            if (upUpload.Visible == false)
            {
                upUpload.Visible = true;
                if (pnlDownload.Visible == true)
                {
                    pnlDownload.Visible = false;
                }
            }
            else
            {
                upUpload.Visible = false;
            }
        }

        
    }
}