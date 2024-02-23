using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BEData.BusinessEntity;
using System.Data;
using System.IO;
using VBIDE = Microsoft.Vbe.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using BEData;
using ExcelFordownload = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.Runtime.InteropServices;



class BulletsFin { public string Name { get; set; } public string Code { get; set; } public string Value { get; set; } }



public partial class ReportFinpulse : BasePage
{


    List<string> lstFinMapping = new List<string>();
    Logger logger = new Logger();
    public string fileName = "BEData.ReportFinpulse.cs";
    BEDL objbe = new BEDL();
    protected void Page_Load(object sender, EventArgs e)
    {
        base.ValidateSession();

        if (Page.IsPostBack)
        { }
        else
        {

            lbldisplayFin.Text = lbldisplayFin.Text + objbe.GetFinpulseDumpDate();
            //onload
            string isValidEntry = Session["Login"] + "";
            if (!isValidEntry.Equals("1"))
                Response.Redirect("UnAuthorised.aspx");


            string userID = Session["UserID"] + "";


            LoadComboBox(userID);
            loadMCCPU();
        }
    }



    private void LoadComboBox(string userID)
    {
        try
        {
            List<string> lstSU = objbe.GetSUForuser(userID);
            ddlSU.DataSource = lstSU.Select(k => k.ToString()).Distinct().ToList();
            ddlSU.DataBind();
            ddlSU.Items.Insert(0, "ALL");

            List<string> lstPU = objbe.GetNSOForuser(userID);
            ddlPU.DataSource = lstPU.Select(k => k.ToString()).Distinct().ToList();
            ddlPU.DataBind();
            ddlPU.Items.Insert(0, "ALL");

            List<string> lstYear = objbe.GetAllBEYearFin();
            ddlYear.DataSource = lstYear.Select(k => k.ToString()).Distinct().ToList();
            ddlYear.DataBind();

            string year = ddlYear.SelectedValue;



            //List<string> lstYearMonth = objbe.GetAllBEYearMonthFin(year);
            //int rowcount = lstYearMonth.Count;
            //ddlYearMonth.DataSource = lstYearMonth.Select(k => k.ToString()).Distinct().ToList();
            //ddlYearMonth.DataBind();

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
    protected void ddlSU_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadMCCPU();
    }

    protected void ddlPU_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadMCC();
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        string year = ddlYear.SelectedValue;
        List<string> lstYearMonth = objbe.GetAllBEYearMonthFin(year);
        int rowcount = lstYearMonth.Count;
        //ddlYearMonth.DataSource = lstYearMonth.Select(k => k.ToString()).Distinct().ToList();
        //ddlYearMonth.DataBind();
        //hdnfldFlag.Value = "0";


    }
    public void loadMCCPU()
    {
        string userID = Session["UserID"] + "";

        ddlPU.DataTextField = "newOffering";
        ddlPU.DataValueField = "newOffering";
        ddlPU.DataSource = objbe.RTBRGetPUList(userID, ddlSU.SelectedItem.Text);
        ddlPU.DataBind();
        ddlPU.Items.Insert(0, "ALL");

        //List<string> lstMapping = new List<string>();
        //lstMapping = objbe.GetCustomerCodeForPUVol(userID, ddlSU.SelectedItem.Text);
        //ddlMCC.DataSource = lstMapping.Select(k => k.ToString()).Distinct().ToList();
        //ddlMCC.DataBind();
        //ddlMCC.Items.Insert(0, "ALL");

        ddlMCC.DataTextField = "txtmcc";
        ddlMCC.DataValueField = "txtmcc";
        ddlMCC.DataSource = objbe.RTBRGetCustomerList(userID, ddlSU.SelectedItem.Text);
        ddlMCC.DataBind();
        ddlMCC.Items.Insert(0, "ALL");
    }

    public void loadMCC()
    {
        string userID = Session["UserID"] + "";
        string SU = ddlSU.SelectedItem.Text;
        string PU = ddlPU.SelectedItem.Text;

        ddlMCC.DataTextField = "txtmcc";
        ddlMCC.DataValueField = "txtmcc";
        ddlMCC.DataSource = objbe.RTBRGetCustomerListForSUMCC(userID, SU, PU);
        ddlMCC.DataBind();
        ddlMCC.Items.Insert(0, "ALL");
    }
    protected void btnreport_Click(object sender, EventArgs e)
    {
        string userID = Session["UserID"] + "";
        // DataTable dtRTBR,dtRTBR1,dtRTBR2,dtRTBR3;
        string dtFinexcel = string.Empty;

        DataTable dtFin = new DataTable();
        // dtRTBR = null; dtRTBR1 = null; dtRTBR2 = null; dtRTBR3 = null;
        string mcc = ddlMCC.Text;
        //if (mcc.ToLowerTrim() == "all")
        //{
        //    ddlMCC.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { mcc += k + ","; });
        //    mcc = mcc.Replace("ALLALL,", string.Empty);
        //    mcc = mcc.Trim().TrimEnd(',').TrimStart(',');
        //}


        string year = ddlYear.SelectedValue;
        string su = ddlSU.SelectedItem.Text;
        string pu = ddlPU.SelectedItem.Text;

        dtFin = objbe.GetFinpulseDetails(userID, mcc, year, su, pu);
        //dtFinexcel = "Finpulse_" + ddlYearMonth.Text;
        dtFinexcel = "Finpulse";
        if (dtFin == null || dtFin.Rows.Count == 0)
        {
            lbl.Text = "";
            Session["key"] = null;
            //Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
            string message = "alert('No Data to download!')";
            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
            return;
        }
        string[] strArryFin = { "ProjectCode", "MasterProject", "ProjectGeography", "DU", "YearMonth", "OnSiteRevenue", "OffShoreRevenue", "TotalRevenue","TotalRevenue31MarCurr", "OnsiteCostofRevenueDirect", "OffShoreCostofRevenueDirect", "TotalCostofRevenueDirect", "OnsiteProjectMargin", "OffshoreProjectMargin", "ProjectMargin", "TotalCostofRevenueAllocated", "PUDelyMargin", "TotalCostofRevenueOtherCosts", "GrossMargin", "TotalSGADirect", "TotalSellingAllocated", "TotalGAAllocated", "PBTBeforeInvestment", "PBTAfterInvestmentForexLosses", "TotalTaxesExcludingIndiaTax", "PATBeforeIndiaTax", "IndiaTax", "PATAfterIndiaTax", "TotalExpense", "OnSiteBilledMonths", "OffShoreBilledMonths", "TotalBilledMonths", "BenchMonths", "TotalBillableMonths", "RDMonths", "SolutionMonths", "OverheadMonths", "TrainingMonths", "Leave", "OnsiteRDMonths", "OffshoreRDMonths", "OnsiteSolutionMonths",
                                  "OffshoreSolutionMonths", "OnsiteOverheads", "OffshoreOverheads", "OnsiteTraining", "OffshoreTraining", "OnsiteLeave", "OffshoreLeave",
                                  "Buffer", "OnsiteBuffer", "OffshoreBuffer", "TotalPersonMonths", "Support", "OnsiteSupport", "OffshoreSupport", "ReportingPU", "SBUPUGroup", 
                                  "SubUnit", "Unit", "Technology", "ServiceOffering", "ServiceOfferingGroup", "ProjectType", "ProductionInCharge", "CustomerName",
                                  "CustomerCode", "MasterCustomerCode", "CustomerPortfolio", "ContractType", "Location", "OnSiteLT", "OnSiteST", "BenchON", "BenchOFF", 
                                  "MasterCustIBU", "InvestmentCost", "OnsiteBilledTM", "OffShoreBilledTM", "OnsiteBenchTM", "OffshoreBenchTM", "OnsiteBufferTM", 
                                  "OffshoreBufferTM", "OperationsCost", "ForexIncomeLosses", "ProgramCode", "TrackCode", "Region", "ProjectCurrency", "CustomerGeography",
                                  "Company", "ProjectPU", "STPCategory", "IndustryCode", "IndustrySubCode", "TBB", "ProjectName", "IBUVertical",
                                     "BudgetingUnit", "RegionGroup", "ServiceLine", "PracticeLine", "DeliverySubUnit", "GroupMasterProjectCode", "DMMailid","NewOfferingCode" };
        lstFinMapping = strArryFin.ToList();

        if (dtFin != null)
        {
            if (dtFin.Rows.Count > 0)
            {
                for (int i = 0; i < lstFinMapping.Count; i++)
                    dtFin.Columns[lstFinMapping[i]].SetOrdinal(i);

            }
        }
       
        string folder = @"ExcelOperations\DownloadFiles";
        var MyDir = new DirectoryInfo(Server.MapPath(folder));

        string Filename = "Finpulse" + "_" +userID+ "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + "IST.xlsx"; 

        if (MyDir.GetFiles().SingleOrDefault(k => k.Name == Filename) != null)
            System.IO.File.Delete(MyDir.FullName + "\\" + Filename);


        FileInfo file = new FileInfo(MyDir.FullName + "\\" + Filename);

   
        DataTable[] splittedtables = dtFin.AsEnumerable()
                                    .Select((row1, index) => new { row1, index })
                                    .GroupBy(x => x.index / 500)  // integer division, the fractional part is truncated
                                    .Select(g => g.Select(x => x.row1).CopyToDataTable())
                                    .ToArray();
        
        int Count = 1;
        ExcelPackage pck = new ExcelPackage();
        ExcelWorksheet ws;
        ws = pck.Workbook.Worksheets.Add("Data");
        bool flag1 = true;
        for (int i = 0; i < splittedtables.Length; i++)
        {

            string CellValue = "A" + Count;

            if (flag1)
            {
                ws.Cells[CellValue].LoadFromDataTable(splittedtables[i], true);
                Count++;
            }
            else
                ws.Cells[CellValue].LoadFromArrays(splittedtables[i].Rows.OfType<DataRow>().Select(k => k.ItemArray));

            Count = Count + splittedtables[i].Rows.Count;
            flag1 = false;
        }

        int rowcountSheet0 = dtFin.Rows.Count;
        int colcountSheet0 = dtFin.Columns.Count;

        ws.Cells[1, 1, 1, colcountSheet0].Style.Font.Bold = true;
        var fill = ws.Cells[1, 1, 1, colcountSheet0].Style.Fill;
        fill.PatternType = ExcelFillStyle.Solid;
        fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
        ws.Cells[1, 1, 1, colcountSheet0].AutoFitColumns();

        ws.Cells[1, 1, rowcountSheet0 + 1, colcountSheet0 + 1].Style.Font.Name = "calibri";
        ws.Cells[1, 1, rowcountSheet0 + 1, colcountSheet0 + 1].Style.Font.Size = 9;

        pck.SaveAs(file);
        pck.Dispose();
        ReleaseObject(pck);
        ReleaseObject(ws);
        GenerateReport(Filename);
    }

    enum MacroDataType
    {
        String, Integer

    }
    private string GetVariableDeclaration(string variableName, MacroDataType type, object value)
    {
        string formatString = "Dim {0} as {1} \n {0} =\"{2}\" \n";
        string formatNumber = "Dim {0} as {1} \n {0} ={2} \n";
        string returnValue = "";
        switch (type)
        {
            case MacroDataType.String:
                returnValue = string.Format(formatString, variableName, type.ToString(), value);
                break;
            case MacroDataType.Integer:
                returnValue = string.Format(formatNumber, variableName, type.ToString(), value);
                break;
            default:
                break;
        }
        return returnValue;
    }

    void GenerateReport(string fname)
    {
        string year = ddlYear.SelectedValue;
        string YR = "2020";
        

        Microsoft.Office.Interop.Excel.Application oExcel;
        Microsoft.Office.Interop.Excel.Workbook oBook = default(Microsoft.Office.Interop.Excel.Workbook);
        VBIDE.VBComponent oModule;
        //try
        {
            string folder = @"ExcelOperations\DownloadFiles";
            var myDir = new DirectoryInfo(Server.MapPath(folder));
            String sCode;
            Object oMissing = System.Reflection.Missing.Value;
            //instance of excel
            oExcel = new Microsoft.Office.Interop.Excel.Application();

            oBook = oExcel.Workbooks.
                Open(myDir.FullName + "\\" + fname + "", 0, false, 5, "", "", true,
                Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Sheets WRss = oBook.Sheets;
            //string filename = "";
            //filename = "Finpulse" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + "IST.xlsx";

            //if (myDir.GetFiles().SingleOrDefault(k => k.Name == filename) != null)
            //    System.IO.File.Delete(myDir.FullName + "\\" + filename);

            //String excelFile1 = "~\\ExcelOperations\\" + filename;
            //String destPath = Server.MapPath(excelFile1);

            string macroFolder = @"ExcelOperations\Macro";
            var myDir1 = new DirectoryInfo(Server.MapPath(macroFolder));
            oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);
            sCode = "sub Macro()\r\n" +
                    GetVariableDeclaration("YR", MacroDataType.String, YR) +
                System.IO.File.ReadAllText(myDir1.FullName + "\\FinpulseRepotMacro.txt") +
                    "\nend sub";
            oModule.CodeModule.AddFromString(sCode);
            oExcel.GetType().InvokeMember("Run",
                            System.Reflection.BindingFlags.Default |
                            System.Reflection.BindingFlags.InvokeMethod,
                            null, oExcel, new string[] { "Macro" });
            //Adding permission to excel file//
            //oBook.Activate();
            //oBook.Permission.Enabled = true;
            //oBook.Permission.RemoveAll();
            //string strExpiryDate = DateTime.Now.AddDays(60).Date.ToString();
            //DateTime dtTempDate = Convert.ToDateTime(strExpiryDate);
            //DateTime dtExpireDate = new DateTime(dtTempDate.Year, dtTempDate.Month, dtTempDate.Day);

            //UserPermission userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionChange);
            //userper.ExpirationDate = dtExpireDate;
            /////////////////////////////////////

            oBook.Save();
            oBook.Close(false);
            oExcel.Quit();

            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oExcel);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBook);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRss);
            GC.Collect();


            DownloadFile(fname);

        }

    }

    void GenerateReport_old(string fname)
    {


        Microsoft.Office.Interop.Excel.Application oExcel;
        Microsoft.Office.Interop.Excel.Workbook oBook = default(Microsoft.Office.Interop.Excel.Workbook);
        VBIDE.VBComponent oModule;
        //try
        {
            string folder = @"ExcelOperations\DownloadFiles";
            var myDir = new DirectoryInfo(Server.MapPath(folder));
            String sCode;
            Object oMissing = System.Reflection.Missing.Value;
            //instance of excel
            oExcel = new Microsoft.Office.Interop.Excel.Application();

            oBook = oExcel.Workbooks.
                Open(myDir.FullName + "\\" + fname + "", 0, false, 5, "", "", true,
                Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Sheets WRss = oBook.Sheets;
            //string filename = "";
            //filename = "Finpulse" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + "IST.xlsx";

            //if (myDir.GetFiles().SingleOrDefault(k => k.Name == filename) != null)
            //    System.IO.File.Delete(myDir.FullName + "\\" + filename);

            //String excelFile1 = "~\\ExcelOperations\\" + filename;
            //String destPath = Server.MapPath(excelFile1);

            oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);
            sCode = "sub Macro()\r\n" +
                System.IO.File.ReadAllText(myDir.FullName + "\\FinpulseRepotMacro.txt") +
                    "\nend sub";
            oModule.CodeModule.AddFromString(sCode);
            oExcel.GetType().InvokeMember("Run",
                            System.Reflection.BindingFlags.Default |
                            System.Reflection.BindingFlags.InvokeMethod,
                            null, oExcel, new string[] { "Macro" });
            //Adding permission to excel file//
            oBook.Activate();
            oBook.Permission.Enabled = true;
            oBook.Permission.RemoveAll();
            string strExpiryDate = DateTime.Now.AddDays(60).Date.ToString();
            DateTime dtTempDate = Convert.ToDateTime(strExpiryDate);
            DateTime dtExpireDate = new DateTime(dtTempDate.Year, dtTempDate.Month, dtTempDate.Day);
        
            UserPermission userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionChange);
            userper.ExpirationDate = dtExpireDate;
            /////////////////////////////////////

            oBook.Save();
            oBook.Close(false);
            oExcel.Quit();

            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oExcel);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBook);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRss);
            GC.Collect();


            DownloadFile(fname);

        }

    }

    private void DownloadFile(string FileNAme)
    {



        try
        {


            Session["Key"] = FileNAme;

            loading.Style.Add("visibility", "visible");
            lbl.Text = "Downloaded";
            up.Update();

            iframe.Attributes.Add("src", "Download.aspx");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myStopFunction", "myStopFunction()", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "isvaliduploadClose", "isvaliduploadClose()", true);





        }
        catch (Exception ex)
        {

        }
    }

    public void ReleaseObject(object o)
    {
        try
        {
            if (o != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
        }
        catch (Exception) { }
        finally { o = null; }
    }

    protected void btnhidden_Click(object sender, ImageClickEventArgs e)
    {
        //string name = "RTBR_Dump1" + userID + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls";

        string name = Session["NAME"].ToString();
        //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //Response.AddHeader("content-disposition", "attachment;  filename=" + name);



        string folder = @"ExcelOperations\DownloadFiles";
        var MyDir = new DirectoryInfo(Server.MapPath(folder));
        //string path = MyDir.FullName + "\\RTBR_Dump1.xlsx";
        string path = MyDir.FullName + "\\" + name;
        //string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
        //string name = "RTBR_Dump1" + ".xlsx";
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
                    type = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    type = "application/vnd.ms-excel.12";
                    break;
            }
        }


        Response.AppendHeader("content-disposition", "attachment;" + "filename=" + name + "; " + "creation-date=" + DateTime.Now.ToString("R").Replace(",", "") + "; " +
                "modification-date=" + DateTime.Now.ToString("R").Replace(",", "") + "; " +
                "read-date=" + DateTime.Now.ToString("R").Replace(",", ""));


        if (type != "")
            Response.ContentType = type;
        Response.WriteFile(path);

        Response.Flush();
        Response.End();
    }

    private void DownloadReport(DataTable table, string filename)
    {
        ExcelFordownload.Application oExcel;
        ExcelFordownload.Workbook oBook = default(Excel.Workbook);
        VBIDE.VBComponent oModule;
        try
        {
          
            string folder = @"ExcelOperations\DownloadFiles";
            var MyDir = new DirectoryInfo(Server.MapPath(folder));
            var userid = Session["UserId"];
            String sCode;
            Object oMissing = System.Reflection.Missing.Value;
            oExcel = new Excel.Application();
            FileInfo file1 = new FileInfo(MyDir.FullName + "\\" + filename);
            oBook = oExcel.Workbooks.Open(file1.ToString(), 0, false, 5, "", "", true,
                Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);
            sCode = "sub Macro()\r\n" +
                System.IO.File.ReadAllText(MyDir.FullName + "\\FinpulseRepotMacro.txt") +
                    "\nend sub";
            oModule.CodeModule.AddFromString(sCode);
            oExcel.GetType().InvokeMember("Run",
                            System.Reflection.BindingFlags.Default |
                            System.Reflection.BindingFlags.InvokeMethod,
                            null, oExcel, new string[] { "Macro" });
            
            /////////////////////////////////////

            //Adding permission to excel file//
            oBook.Permission.Enabled = true;
            oBook.Permission.RemoveAll();
            string strExpiryDate = DateTime.Now.AddDays(60).Date.ToString();
            DateTime dtTempDate = Convert.ToDateTime(strExpiryDate);
            DateTime dtExpireDate = new DateTime(dtTempDate.Year, dtTempDate.Month, dtTempDate.Day);
            UserPermission userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionChange);
            //userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionEdit);
            //userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionExtract);
            userper.ExpirationDate = dtExpireDate;

          
            oExcel.DisplayAlerts = false;
            oBook.Save();
            oBook.Close(0);
            oExcel.Quit();

            Marshal.FinalReleaseComObject(oModule);
            oModule = null;
            Marshal.FinalReleaseComObject(oBook);
            oBook = null;
            Marshal.FinalReleaseComObject(oExcel);
            oExcel = null;



            Session["key"] = filename;
            //Session["data"] = table;
            loading.Style.Add("visibility", "visible");
            lbl.Text = "Downloaded";
            up.Update();

            iframe.Attributes.Add("src", "Download.aspx");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myStopFunction", "myStopFunction()", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "isvaliduploadClose", "isvaliduploadClose()", true);
        }
        catch
        {
        }
    }
   



    //protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string year = ddlYear.SelectedValue;
    //    List<string> lstYearMonth = objbe.GetAllBEYearMonthFin(year);
    //    int rowcount = lstYearMonth.Count;
    //    ddlYearMonth.DataSource = lstYearMonth.Select(k => k.ToString()).Distinct().ToList();
    //    ddlYearMonth.DataBind();
    //    //hdnfldFlag.Value = "0";


    //}
}
