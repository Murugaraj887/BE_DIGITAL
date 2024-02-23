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
using Microsoft.Office;
using Microsoft.Office.Core;

//Manasa



    public partial class ReportAlconPBS : BasePage
    {


        List<string> lstFinMapping = new List<string>();
        Logger logger = new Logger();
        public string fileName = "BEData.ReportAlconPBS.cs";
        BEDL objbe = new BEDL();
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            if (Page.IsPostBack)
            { }
            else
            {


                //onload
                string isValidEntry = "1";
                    //Session["Login"].ToString();
                if (!isValidEntry.Equals("1"))
                    Response.Redirect("UnAuthorised.aspx");

                string userID = Session["UserID"] + "";
            //string userID = "kshitij.gaurav";
            List<string> lstSU = objbe.GetSUForuser(userID);
            ddlSU.DataSource = lstSU.Select(k => k.ToString()).Distinct().ToList();
            ddlSU.DataBind();
            ddlSU.Items.Insert(0, "ALL");
            //ddlSU.Items.Insert(0, "ALL");

            string su = ddlSU.SelectedValue;


                ddlNSO.DataTextField = "newOffering";
                ddlNSO.DataValueField = "newOffering";
                ddlNSO.DataSource = objbe.GetPUAlconReport(userID);
                ddlNSO.DataBind();
                ddlNSO.Items.Insert(0, "ALL");
                string pu = ddlNSO.SelectedValue;

                //if (pu.ToLowerTrim() == "all")
                //{
                //    ddlPU.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { pu += k + ","; });
                //    pu = pu.Replace("ALLALL,", string.Empty);
                //    pu = pu.Trim().TrimEnd(',').TrimStart(',');
                //}


                List<string> lstCustomerCode = objbe.GetCustomerCodeForPUVol(userID, su);
                ddlMastercode.DataSource = lstCustomerCode.Select(k => k.ToString()).Distinct().ToList();
                ddlMastercode.DataBind();
                ddlMastercode.Items.Insert(0, "ALL");

                lbldisplayRTBR.Text = lbldisplayRTBR.Text + objbe.GetAlconDumpDate();
            }
        }

        protected void btnreport_Click(object sender, EventArgs e)
        {
            //DoReport();
           //return;
            try
            {
                string su = ddlSU.SelectedItem.Text;
                string NSO = ddlNSO.SelectedItem.Text;
                string userid = Session["UserId"].ToString();
                //string userid = "kshitij.gaurav";
                string customerCode = string.Empty;
                List<string> lstAlcon = new List<string>();

                customerCode = ddlMastercode.Text;

                //if (customerCode.ToLowerTrim() == "all")
                //{
                //    ddlMastercode.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { customerCode += k + ","; });
                //    customerCode = customerCode.Replace("ALLALL,", string.Empty);
                //    customerCode = customerCode.Trim().TrimEnd(',').TrimStart(',');
                //}
                //string PU = string.Empty;
                //PU = ddlPU.Text;
                //if (PU.ToLowerTrim() == "all")
                //{
                //    ddlPU.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { PU += k + ","; });
                //    PU = PU.Replace("ALLALL,", string.Empty);
                //    PU = PU.Trim().TrimEnd(',').TrimStart(',');
                //}

                string alconname = "Alcon_PBS_Dump";
                DataTable dtAlcon = objbe.GetAlconPBSData(customerCode, userid, su,NSO);
                if (dtAlcon == null || dtAlcon.Rows.Count == 0)
                {
                    lbl.Text = "";
                    Session["key"] = null;
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
                    string message = "alert('No Data to download!')";
                    ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                    return;
                }
                string[] strArry = { "Master Project Code", "Project starts", "Project ends", "Customer Unit", "Project SubUnit", "NSO Code", "Customer SubUnit", "Master Customer", "Customer", "Contract Code", "TBB Enabled", "Service Offering", "Status", "Version", "PM", "DM", "ChildPU", "ChildCompany", "Month", "PBS Onsite Effort", "PBS Offshore Effort", "Total PBS Effort", "ALCON Onsite Effort", "ALCON Offshore Effort", "Total ALCON Effort", "PBS-ALCON Effort", "ProgramCode", "PBS non-Bill Onsite Effort", "PBS non-Bill Offshore Effort", "Group Master Project", "DumpDate", "Practice Line", "Service Line Code", "Credit Subunit", "Region Code" };
                lstAlcon = strArry.ToList();

                if (dtAlcon != null)
                {
                    if (dtAlcon.Rows.Count > 0)
                    {
                        for (int i = 0; i < lstAlcon.Count; i++)
                            dtAlcon.Columns[lstAlcon[i]].SetOrdinal(i);
                    }
                }
              //  dtAlcon.Columns["txtMcc"].ColumnName = "Master Customer Code";
               
                //return;
                string folder = @"ExcelOperations\DownloadFiles";
                var MyDir = new DirectoryInfo(Server.MapPath(folder));

                string name = "AlconPBS_" + userid + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";

                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == name) != null)
                    System.IO.File.Delete(MyDir.FullName  +"\\" + name);
                FileInfo file = new FileInfo(MyDir.FullName +"\\"+ name);
                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws;
                int rowcount = dtAlcon.Rows.Count;
                int colcount = dtAlcon.Columns.Count;

                ws = pck.Workbook.Worksheets.Add(alconname);
                ws.Cells["A1"].LoadFromDataTable(dtAlcon, true);
                ws.Cells[1, 1, 1, 36].Style.Font.Bold = true;
                var fill = ws.Cells[1, 1, 1, 36].Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                ws.Cells[1, 1, 1, 36].AutoFitColumns();
                ws.Cells[1, 1, rowcount + 1, colcount + 1].Style.Font.Name = "calibri";
                ws.Cells[1, 1, rowcount + 1, colcount + 1].Style.Font.Size = 9;


                pck.SaveAs(file);                
                pck.Dispose();
                ReleaseObject(pck);
                ReleaseObject(ws);              
                //DownloadReport(dtAlcon, name);
                GenerateReport(dtAlcon,name);
            }

            finally
            { }
        }
        private void DownloadReport(DataTable table, string name)
        {
            string userid = Session["UserId"].ToString();
            System.Web.UI.WebControls.DataGrid grid = new System.Web.UI.WebControls.DataGrid();

            grid.HeaderStyle.Font.Bold = true;
            grid.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(141, 180, 226);
            grid.DataSource = table;
            grid.DataBind();

            //string role = objbe.GetUserRole(userid);
            //string currentQtr = DateUtility.GetQuarter("current");
            Excel.Application oExcel;
            Excel.Workbook oBook = default(Excel.Workbook);
            VBIDE.VBComponent oModule;
            try
            {
                //Adding permission to excel file//
                string folder = @"ExcelOperations\DownloadFiles";
                var MyDir = new DirectoryInfo(Server.MapPath(folder));
                oExcel = new Excel.Application();
                oBook = oExcel.Workbooks.
                    Open(MyDir + "\\" + name, 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                Microsoft.Office.Interop.Excel.Sheets WRss = oBook.Sheets;

                oBook.Activate();
                oBook.Permission.Enabled = true;
                oBook.Permission.RemoveAll();
                string strExpiryDate = DateTime.Now.AddDays(60).Date.ToString();
                DateTime dtTempDate = Convert.ToDateTime(strExpiryDate);
                DateTime dtExpireDate = new DateTime(dtTempDate.Year, dtTempDate.Month, dtTempDate.Day);
                UserPermission userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionChange);
                //userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionEdit);
                //userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionExtract);
                userper.ExpirationDate = dtExpireDate;          

                oBook.Save();
                oBook.Close(false);
                oExcel.Quit();

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oExcel);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBook);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRss);

                /////////////////////////////////////
                Session["key"] = name;

                loading.Style.Add("visibility", "visible");
                lbl.Text = "Downloaded";
                up.Update();

                iframe.Attributes.Add("src", "Download.aspx");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myStopFunction", "myStopFunction()", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "isvaliduploadClose", "isvaliduploadClose()", true);
            }
            catch(Exception ex)
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

        void GenerateReport(DataTable table, string fname)
        {

            System.Web.UI.WebControls.DataGrid grid = new System.Web.UI.WebControls.DataGrid();

            grid.HeaderStyle.Font.Bold = true;
            grid.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(141, 180, 226);
            grid.DataSource = table;
            grid.DataBind();

            Microsoft.Office.Interop.Excel.Application oExcel;
            Microsoft.Office.Interop.Excel.Workbook oBook = default(Microsoft.Office.Interop.Excel.Workbook);
            //try
            {
                string folder = @"ExcelOperations\DownloadFiles";
                var myDir = new DirectoryInfo(Server.MapPath(folder));

                //instance of excel
                oExcel = new Microsoft.Office.Interop.Excel.Application();

                oBook = oExcel.Workbooks.
                    Open(myDir.FullName + "\\" + fname + "", 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Microsoft.Office.Interop.Excel.Sheets WRss = oBook.Sheets;
                string filename = "";
               

            
              
                //oBook.Permission.Enabled = true;
                //oBook.Permission.RemoveAll();
                //string strExpiryDate = DateTime.Now.AddDays(60).Date.ToString();
                //DateTime dtTempDate = Convert.ToDateTime(strExpiryDate);
                //DateTime dtExpireDate = new DateTime(dtTempDate.Year, dtTempDate.Month, dtTempDate.Day);
                //UserPermission userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionChange);
                //userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionEdit);
                //userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionExtract);
                //userper.ExpirationDate = dtExpireDate;
                /////////////////////////////////////

                oBook.Save();
                oBook.Close(false);
                oExcel.Quit();

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oExcel);
                oExcel = null;
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

        private void DownloadFileAlcon()
        {
            Excel.Application oExcel;
            Excel.Workbook oBook = default(Excel.Workbook);
            VBIDE.VBComponent oModule;
            try
            {
                bool forceDownload = true;
                string folder = @"ExcelOperations\DownloadFiles";
                var MyDir = new DirectoryInfo(Server.MapPath(folder));


                String sCode;
                Object oMissing = System.Reflection.Missing.Value;

                oExcel = new Excel.Application();


                oBook = oExcel.Workbooks.
                    Open(MyDir.FullName + "\\Alcon_PBS_Dump.xlsx", 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);


                oBook.Save();


                oBook.Close();
                oExcel.Quit();
                oExcel = null;
                oModule = null;
                oBook = null;

                GC.Collect();




            
            }
            catch (Exception ex)
            {
                if ((ex.Message + "").Contains("Thread was being aborted."))
                {

                    GC.Collect();
                    logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }
                else
                {

                    GC.Collect();
                    logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }
            }
        }

        protected void btnhidden_Click(object sender, ImageClickEventArgs e)
        {

            //bool forceDownload = true;
            //string folder = "ExcelOperations";
            //var MyDir = new DirectoryInfo(Server.MapPath(folder));

            //string path = MyDir.FullName + "\\Alcon_PBS_Dump.xlsx";

            //string name = "Alcon_PBS_Dump" + ".xlsx";
            //string ext = Path.GetExtension(path);
            //string type = "";

            //// set known types based on file extension  
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
            //            type = "application/vnd.ms-excel";
            //            break;
            //        case ".xlsx":
            //            type = "application/vnd.ms-excel.12";
            //            break;
            //    }
            //}

            //if (forceDownload)
            //{
            //    Response.AppendHeader("content-disposition",
            //        "attachment; filename=" + name);
            //}
            //if (type != "")
            //    Response.ContentType = type;
            //Response.WriteFile(path);

            //Response.Flush();
            //Response.End();


            bool forceDownload = true;
            //string path = MapPath(fname);
            string folder = @"ExcelOperations\DownloadFiles";
            var MyDir = new DirectoryInfo(Server.MapPath(folder));

            string path = MyDir.FullName + "\\Alcon_PBS_Dump.xlsx";
            //string name = "Revenue_Volume_BE_Dump" + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xls" xlsx ;
            string name = "SDMVolComparisonReport" + ".xlsx";
            string ext = Path.GetExtension(path);
            string type = "";

            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=Alcon_PBS_Dump.xlsx");

            Response.WriteFile(path);

            Response.Flush();
            Response.End();


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

        protected void ddlSU_SelectedIndexChanged(object sender, EventArgs e)
        {
            string userID = Session["UserID"].ToString();

            string su = ddlSU.SelectedValue;


            ddlNSO.DataTextField = "newOffering";
            ddlNSO.DataValueField = "newOffering";
            ddlNSO.DataSource = objbe.RTBRGetPUList(userID, su);
            ddlNSO.DataBind();
            ddlNSO.Items.Insert(0, "ALL");
            string NSO = ddlNSO.SelectedValue;

            //if (pu.ToLowerTrim() == "all")
            //{
            //    ddlPU.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { pu += k + ","; });
            //    pu = pu.Replace("ALLALL,", string.Empty);
            //    pu = pu.Trim().TrimEnd(',').TrimStart(',');
            //}


            //List<string> lstCustomerCode = objbe.GetCustomerCodeForPUVol(userID,su);
            //ddlMastercode.DataSource = lstCustomerCode.Select(k => k.ToString()).Distinct().ToList();

            ddlMastercode.DataTextField = "txtmcc";
            ddlMastercode.DataValueField = "txtmcc";
            ddlMastercode.DataSource = objbe.GetCustomerCodeAlcon(userID, su);
            ddlMastercode.DataBind();
            ddlMastercode.Items.Insert(0, "ALL");
        }

        protected void ddlNSO_SelectedIndexChanged(object sender, EventArgs e)
        {
            string userID = Session["UserID"].ToString();
            string NSO = ddlNSO.SelectedValue;
            string SU = ddlSU.SelectedItem.Text;
            //List<string> lstCustomerCode = objbe.GetCustomerCodeForPUVol(userID, pu);
            //ddlMastercode.DataSource = lstCustomerCode.Select(k => k.ToString()).Distinct().ToList();
            //ddlMastercode.DataBind();
            //ddlMastercode.Items.Insert(0, "ALL");

            ddlMastercode.DataTextField = "txtmcc";
            ddlMastercode.DataValueField = "txtmcc";
            ddlMastercode.DataSource = objbe.RTBRGetCustomerListForSUMCC(userID, SU, NSO);
            ddlMastercode.DataBind();
            ddlMastercode.Items.Insert(0, "ALL");
        }
        
    }


