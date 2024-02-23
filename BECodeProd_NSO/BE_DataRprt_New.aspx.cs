using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VBIDE = Microsoft.Vbe.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.IO;
using BEData;
using System.Data;
using System.Globalization;
using System.Data.SqlClient;
using System.Drawing;
using Microsoft.Office.Core;
using System.Runtime.InteropServices;

namespace BECodeProd
{
    public partial class BE_DataRprt_New : BasePage
    {
        string connString = System.Configuration.ConfigurationManager.AppSettings["DemandCaptureConnectionString"];
        Logger logger = new Logger();
        public string fileName = "BEData.BEReport.cs";
        BEDL objbe = new BEDL();
        int mon = DateTime.Now.Month;
        string curqtr = string.Empty;
        public DateTime dateTime = DateTime.Now;
        string actyesm1 = string.Empty;
        string actyesm2 = string.Empty;
        string pactyesm3 = string.Empty;

        public int month = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();


            if (mon == 4 || mon == 5 || mon == 6)
                curqtr = "Q1";
            else if (mon == 7 || mon == 8 || mon == 9)
                curqtr = "Q2";
            else if (mon == 10 || mon == 11 || mon == 12)
                curqtr = "Q3";
            else
                curqtr = "Q4";


            if (Page.IsPostBack)
            { }
            else
            {


                //if (rdbtnlst.SelectedValue == "As Of Today")
                //{
                //    ddlDate.Enabled = false;
                //}
                //else
                //{
                //    ddlDate.Enabled = true;
                //}

                
                //onload
                string isValidEntry = Session["Login"] + "";
                if (!isValidEntry.Equals("1"))
                    Response.Redirect("UnAuthorised.aspx");

                string userID = Session["UserID"] + "";


                //ddlSU.DataTextField = "SU";
                //ddlSU.DataValueField = "SU";
                //ddlSU.DataSource = objbe.GetSUBeReport(userID);
                //ddlSU.DataBind();
                //if (Session["Role"].ToString() == "Admin")
                //    ddlSU.Items.Insert(0, "ALL");

                //string userid = HttpContext.Current.User.Identity.Name;
                //string[] userids = userid.Split('\\');
                //if (userids.Length == 2)
                //{
                //    userid = userids[1];
                //}
                //userID = "pvljanaki";
                Session["userid"] = userID;
                SqlCommand sql = new SqlCommand("EXEC dbo.[spBEGetSU_dummy] '" + userID + "'");
                DataSet ds1 = new DataSet();
                ds1 = GetDataSet(sql);
                DataTable dt0 = new DataTable();
                dt0 = ds1.Tables[0];

                SqlCommand sqlrole = new SqlCommand("select distinct txtrole from beuseraccess where txtuserid='" + userID + "'");
                DataSet dsrole = new DataSet();
                dsrole = GetDataSet(sqlrole);
                DataTable dtrole = new DataTable();
                dtrole = dsrole.Tables[0];


                if (dtrole.Rows[0][0].ToString() == "Admin" || dtrole.Rows[0][0].ToString() == "UH")
                {
                    ddlSU.Items.Insert(0, "All");
                    ddlSU.Items.Insert(1, "EAIS");
                    ddlSU.Items.Insert(2, "ECAS");
                    ddlSU.Items.Insert(3, "ORC");
                    ddlSU.Items.Insert(4, "SAP");
                   
                    
                }
                else
                {
                    ddlSU.DataSource = dt0;
                    ddlSU.DataValueField = "SU";
                    ddlSU.DataTextField = "SU";
                    ddlSU.DataBind();
                }                             
            }




                ddlQtr.DataTextField = "txtqtr";
                ddlQtr.DataValueField = "txtqtr";
                ddlQtr.DataSource = objbe.GetBEReportQtrYear("Qtr", "0");
                ddlQtr.DataBind();

               
                ddlQtr.SelectedIndex = ddlQtr.Items.IndexOf(ddlQtr.Items.FindByValue(curqtr));

                string qtr = ddlQtr.SelectedValue.Trim();

                ddlYear.DataTextField = "txtyear";
                ddlYear.DataValueField = "txtyear";
                ddlYear.DataSource = objbe.GetBEReportQtrYear("Year", qtr);
                ddlYear.DataBind();

                string year = ddlYear.SelectedValue.ToString().Trim();

             

            }

        public DataSet GetDataSet(SqlCommand cmd)
        {
            DataSet dsPU = new DataSet();

            using (SqlConnection conn = new SqlConnection())
            {

                conn.ConnectionString = connString;
                cmd.Connection = conn;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                conn.Open();
                da.Fill(dsPU, "Table");

                return dsPU;
            }
        }

        protected void btnreport_Click(object sender, EventArgs e)
        {
           

                var qtr = ddlQtr.SelectedValue.ToString().Trim();
                var year = ddlYear.SelectedValue.ToString();
                var date = DateTime.Now.ToString();
                //string date = date1.ToString("yyyy-MM-dd");                               
                var userid = Session["UserId"] + "";
                var SU = ddlSU.SelectedItem.Text;
                //userid = "pvljanki";
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();
              
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                DataTable dt4 = new DataTable();
                DataTable dtReadMe = new DataTable();
             
                var tblBE_data = dt1;
                var tblSDM_View = dt2;
                var tblPU_View = dt3;
                var tblBE_data_reported = dt4;
                var tblReadme = dtReadMe;
                string sht1 = string.Empty;
                ds1 = objbe.GetBEReport_New(SU,userid);
               

      
                       

                        dt1 = ds1.Tables[0];
                        dt2 = ds1.Tables[1];
                        dt3 = ds1.Tables[2];
                        dt4 = ds1.Tables[3];
                        dtReadMe=ds1.Tables[4];
                        

                        tblBE_data = dt3;
                        tblSDM_View =dt2;
                        tblPU_View = dt1;
                        tblBE_data_reported = dt4;
                        tblReadme=dtReadMe;

                        if (tblBE_data == null || tblBE_data.Rows.Count == 0)
                    {
                        lbl.Text = "";
                        Session["key"] = null;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
                        return;
                    }
                    string folder = "ExcelOperations";
                    var MyDir = new DirectoryInfo(Server.MapPath(folder));

                    string name1 = "BEReport_" + Session["UserId"] + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
                    Session["NAME"] = name1.ToString();
                    string name = Session["NAME"].ToString();




                    FileInfo file = new FileInfo(MyDir.FullName + "\\" + name);
                    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == name) != null)
                        System.IO.File.Delete(MyDir.FullName + "\\" + name);

                  
                    ExcelPackage pck = new ExcelPackage();

                    //Create the worksheet
                    // ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Revenue_Volume_BE_Dump");


                    ExcelWorksheet ws;
                    ExcelWorksheet ws1;
                    ExcelWorksheet ws2;
                    ExcelWorksheet ws3;
                    ExcelWorksheet ws4;

                    // string yr = Convert.ToString(dateTime.Year);

                    //int tempyear = Convert.ToInt32(ddlQtr.Text.Remove(0, 3)) + 2000 - 1;
                    //string yr = string.Format("{0}-{1}", tempyear, (tempyear - 2000 + 1));

                    //string sht = ddlQtr.Text + ddlYear.Text.Substring(5, 2) + "_" + "BE_Details";
                    string sht = "BE_Data";
                    int row = tblBE_data.Rows.Count;
                    int col = tblBE_data.Columns.Count;

       
                    sht1 = "SDM_View";
                    if (Session["Role"].ToString() == "DM")
                    sht1 = "DM_View";

                    if (Session["Role"].ToString() == "Anchor")
                    {

                        if (Session["SDMorDM"].ToString() == "DM")
                        {
                            sht1 = "DM_View";
                        }
                        if (Session["SDMorDM"].ToString() == "SDM")
                        {
                            sht1 = "SDM_View";
                        }
                        if (Session["SDMorDM"].ToString() == "All")
                        {
                            sht1 = "SDM_View";
                        }
                    }

                    
                    int row1 = tblSDM_View.Rows.Count;
                    int col1 = tblSDM_View.Columns.Count;

                    string sht2 = "PU_View";
                    int row2 = tblPU_View.Rows.Count;
                    int col2 = tblPU_View.Columns.Count;


                    string sht3 = "BE_Data_ReportedCurrency";
                    int row3 = tblBE_data_reported.Rows.Count;
                    int col3 = tblBE_data_reported.Columns.Count;
                

                    string Readmesheet = "Read Me";
                    int rowReadme = tblReadme.Rows.Count;
                    int colReadme = tblReadme.Columns.Count; 
                    //Create the worksheet
                    // if (tableBEREV.Rows.Count > 0)
                    {

                        ws4 = pck.Workbook.Worksheets.Add(Readmesheet);
                        ws4.Cells["C5"].LoadFromDataTable(tblReadme, true);
                        var fillReadme = ws4.Cells[5, 3, 5, colReadme+2].Style.Fill;
                        fillReadme.PatternType = ExcelFillStyle.Solid;
                        fillReadme.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        ws4.Cells[5, 3, rowReadme+5, colReadme+2].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws4.Cells[5, 3, rowReadme+5, colReadme+2].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws4.Cells[5, 3, rowReadme+5, colReadme+2].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws4.Cells[5, 3, rowReadme+5, colReadme+2].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws4.Cells[5, 3, rowReadme + 5, colReadme+2].Style.Font.Bold = true;
                        ws4.Cells[5, 3, rowReadme + 5, colReadme+2].AutoFitColumns();
                        ws4.Cells[5, 3, rowReadme + 5, colReadme+2].Style.Font.Size = 9;
                        ws4.View.ShowGridLines = false;
                        ws4.Cells[5, 3, 5, colReadme + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        ws4.Cells[6, 3, rowReadme + 5, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;

                        var DMorSDM = ws4.Cells[7, 4].Value;
                        Session["DMorSDM"] = DMorSDM;


                        ws2 = pck.Workbook.Worksheets.Add(sht2);
                        ws2.Cells["A1"].LoadFromDataTable(tblPU_View, true);
                        //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                        var fill2 = ws2.Cells[1, 1, 1, col2].Style.Fill;
                        fill2.PatternType = ExcelFillStyle.Solid;
                        fill2.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);



                        var fillcolor = ws2.Cells[2, 2, 4, col2].Style.Fill;
                        fillcolor.PatternType = ExcelFillStyle.Solid;
                        fillcolor.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 230, 153));

                        var fillcolornew = ws2.Cells[16, 2, 18, col2].Style.Fill;
                        fillcolornew.PatternType = ExcelFillStyle.Solid;
                        fillcolornew.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 230, 153));

                        var fillcolor1 = ws2.Cells[5, 2, 15, col2].Style.Fill;
                        fillcolor1.PatternType = ExcelFillStyle.Solid;
                        fillcolor1.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(217, 225, 242));

                        var fillcolor2 = ws2.Cells[19, 2, 33, col2].Style.Fill;
                        fillcolor2.PatternType = ExcelFillStyle.Solid;
                        fillcolor2.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(217, 225, 242));

                        //var fillcolor3 = ws2.Cells[2, 17, row2 + 1, 26].Style.Fill;
                        //fillcolor3.PatternType = ExcelFillStyle.Solid;
                        //fillcolor3.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(217, 225, 242));
                        var fillcolor4 = ws2.Cells[2, 1, row2 + 1, 1].Style.Fill;
                        fillcolor4.PatternType = ExcelFillStyle.Solid;
                        fillcolor4.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(198, 224, 180));
                        ws2.Cells[1, 1, 1, col2].Style.Font.Bold = true;
                        ws2.Cells[1, 1, row2 + 1, col2].AutoFitColumns();
                        ws2.Cells[1, 1, row2 + 1, col2].Style.Font.Size = 9;

                        ws2.Cells[1, 1, 1, col2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        ws2.View.ShowGridLines = false;                        
                        ws2.Cells[1, 1, row2 + 1, col2].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws2.Cells[1, 1, row2 + 1, col2].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws2.Cells[1, 1, row2 + 1, col2].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws2.Cells[1, 1, row2 + 1, col2].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;                      
                        ws2.Cells[1, 1, row2 + 1, col2].Style.Numberformat.Format = "#,###.00";

                        ws1 = pck.Workbook.Worksheets.Add(sht1);
                        ws1.Cells["A1"].LoadFromDataTable(tblSDM_View, true);
                        //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                        var fill1 = ws1.Cells[1, 1, 1, col1].Style.Fill;
                        fill1.PatternType = ExcelFillStyle.Solid;
                        fill1.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);


                        var fillcolor5 = ws1.Cells[2, 2, 4, col1].Style.Fill;
                        fillcolor5.PatternType = ExcelFillStyle.Solid;
                        fillcolor5.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 230, 153));

                        var fillcolor6 = ws1.Cells[16, 2, 18, col1].Style.Fill;
                        fillcolor6.PatternType = ExcelFillStyle.Solid;
                        fillcolor6.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 230, 153));

                        var fillcolor7 = ws1.Cells[5, 2, 15, col1].Style.Fill;
                        fillcolor7.PatternType = ExcelFillStyle.Solid;
                        fillcolor7.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(217, 225, 242));

                        var fillcolor8 = ws1.Cells[19, 2, 33, col1].Style.Fill;
                        fillcolor8.PatternType = ExcelFillStyle.Solid;
                        fillcolor8.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(217, 225, 242));

                        var fillcolor9 = ws1.Cells[2, 1, row1 + 1, 1].Style.Fill;
                        fillcolor9.PatternType = ExcelFillStyle.Solid;
                        fillcolor9.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(198, 224, 180));


                        ws1.Cells[1, 1, 1, col1].Style.Font.Bold = true;
                        ws1.Cells[1, 1, row1+1, col1].AutoFitColumns();
                        ws1.Cells[1, 1, row1+1, col1].Style.Font.Size = 9;

                        ws1.Cells[1, 1, 1, col1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        ws1.View.ShowGridLines = false;
                        ws1.Cells[1, 1, row1 + 1, col1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws1.Cells[1, 1, row1 + 1, col1].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws1.Cells[1, 1, row1 + 1, col1].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws1.Cells[1, 1, row1 + 1, col1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws1.Cells[1, 1, row1 + 1, col1].Style.Numberformat.Format = "#,###.00";

                        ws = pck.Workbook.Worksheets.Add(sht); 
                        ws.Cells["A1"].LoadFromDataTable(tblBE_data, true);
                        //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                        var fill = ws.Cells[1, 1, 1, col].Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                        var fillcolor10 = ws.Cells[2, 6, row + 1, 8].Style.Fill;
                        fillcolor10.PatternType = ExcelFillStyle.Solid;
                        fillcolor10.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 230, 153));
                        var fillcolor11 = ws.Cells[2, 9, row + 1, 20].Style.Fill;
                        fillcolor11.PatternType = ExcelFillStyle.Solid;
                        fillcolor11.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(217, 225, 242));
                        var fillcolor12 = ws.Cells[2, 21, row + 1, 23].Style.Fill;
                        fillcolor12.PatternType = ExcelFillStyle.Solid;
                        fillcolor12.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 230, 153));
                        var fillcolor13 = ws.Cells[2, 24, row + 1, 38].Style.Fill;
                        fillcolor13.PatternType = ExcelFillStyle.Solid;
                        fillcolor13.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(217, 225, 242));
                        var fillcolor14 = ws.Cells[2, 1, row + 1, 5].Style.Fill;
                        fillcolor14.PatternType = ExcelFillStyle.Solid;
                        fillcolor14.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(198, 224, 180));
                        ws.Cells[1, 1, 1, col].Style.Font.Bold = true;
                        ws.Cells[1, 1, row+1, col].AutoFitColumns();
                        ws.Cells[1, 1, row+1, col].Style.Font.Size = 9;

                        ws.Cells[1, 1, 1, 32].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        ws.View.ShowGridLines = false;
                        ws.Cells[1, 1, row + 1, col].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws.Cells[1, 1, row + 1, col].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws.Cells[1, 1, row + 1, col].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws.Cells[1, 1, row + 1, col].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws.Cells[1, 1, row + 1, col].Style.Numberformat.Format = "#,###.00";

                        ws3 = pck.Workbook.Worksheets.Add(sht3);
                        ws3.Cells["A1"].LoadFromDataTable(tblBE_data_reported, true);
                        //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                        var fill3 = ws3.Cells[1, 1, 1, col3].Style.Fill;
                        fill3.PatternType = ExcelFillStyle.Solid;
                        fill3.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                        var fillcolor15 = ws3.Cells[2, 6, row3 + 1, 8].Style.Fill;
                        fillcolor15.PatternType = ExcelFillStyle.Solid;
                        fillcolor15.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 230, 153));
                        var fillcolor16 = ws3.Cells[2, 9, row3 + 1, 20].Style.Fill;
                        fillcolor16.PatternType = ExcelFillStyle.Solid;
                        fillcolor16.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(217, 225, 242));
                        var fillcolor17 = ws3.Cells[2, 21, row3 + 1, 23].Style.Fill;
                        fillcolor17.PatternType = ExcelFillStyle.Solid;
                        fillcolor17.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 230, 153));
                        var fillcolor18 = ws3.Cells[2, 24, row3 + 1, 37].Style.Fill;
                        fillcolor18.PatternType = ExcelFillStyle.Solid;
                        fillcolor18.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(217, 225, 242));
                        var fillcolor19 = ws3.Cells[2, 1, row3 + 1, 5].Style.Fill;
                        fillcolor19.PatternType = ExcelFillStyle.Solid;
                        fillcolor19.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(198, 224, 180));
                        ws3.Cells[1, 1, 1, col3].Style.Font.Bold = true;
                        ws3.Cells[1, 1, row3+1, col3].AutoFitColumns();
                        ws3.Cells[1, 1, row3+1, col3].Style.Font.Size = 9;

                        ws3.Cells[1, 1, 1, 32].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        ws3.View.ShowGridLines = false;
                        ws3.Cells[1, 1, row3 + 1, col3].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws3.Cells[1, 1, row3 + 1, col3].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws3.Cells[1, 1, row3 + 1, col3].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws3.Cells[1, 1, row3 + 1, col3].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        ws3.Cells[1, 1, row3 + 1, col3].Style.Numberformat.Format = "#,###.00";

                    }

                    pck.SaveAs(file);
                    pck.Dispose();
                    ReleaseObject(pck);
                    ReleaseObject(ws);
                    ReleaseObject(ws1);
                    ReleaseObject(ws2);
                    ReleaseObject(ws3);
                    GenerateReport(name);
                  //  DownloadFileBEReport_Full(name);
            
        }


        void GenerateReport(string fname)
        {

            string UserId = Session["UserId"].ToString();
            Microsoft.Office.Interop.Excel.Application oExcel;
            Microsoft.Office.Interop.Excel.Workbook oBook = default(Microsoft.Office.Interop.Excel.Workbook);
            //try
            {
                string folder = "ExcelOperations";
                var myDir = new DirectoryInfo(Server.MapPath(folder));

                //instance of excel
                oExcel = new Microsoft.Office.Interop.Excel.Application();

                oBook = oExcel.Workbooks.
                    Open(myDir.FullName + "\\" + fname + "", 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Microsoft.Office.Interop.Excel.Sheets WRss = oBook.Sheets;
                string filename = "";
                filename = "BEReport" + "_" + UserId + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + "IST.xlsx";

                if (myDir.GetFiles().SingleOrDefault(k => k.Name == filename) != null)
                    System.IO.File.Delete(myDir.FullName + "\\" + filename);

                String excelFile1 = "~\\ExcelOperations\\" + filename;
                String destPath = Server.MapPath(excelFile1);

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
                /////////////////////////////////////

                oBook.SaveCopyAs(destPath);
                oBook.Close(false);
                oExcel.Quit();

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oExcel);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBook);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(WRss);
                GC.Collect();


                DownloadFile(filename);

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

        }
        

        private void DownloadFileBEReport_Full(string filename)
        {
            Excel.Application oExcel;
            Excel.Workbook oBook = default(Excel.Workbook);
            VBIDE.VBComponent oModule;
            try
            {
              
                string folder = "ExcelOperations";
                var MyDir = new DirectoryInfo(Server.MapPath(folder));

                string filename1 = "";
                filename1 = "BEReport_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + "IST.xlsx";

                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == filename1) != null)
                    System.IO.File.Delete(MyDir.FullName + "\\" + filename1);

                String excelFile1 = "~\\ExcelOperations\\" + filename1;
                String destPath = Server.MapPath(excelFile1);


                oExcel = new Excel.Application();                
                oBook = oExcel.Workbooks.
                    Open(MyDir.FullName + "\\" + filename, 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                oBook.Permission.Enabled = true;
                oBook.Permission.RemoveAll();
                string strExpiryDate = DateTime.Now.AddDays(60).Date.ToString();
                DateTime dtTempDate = Convert.ToDateTime(strExpiryDate);
                DateTime dtExpireDate = new DateTime(dtTempDate.Year, dtTempDate.Month, dtTempDate.Day);
                UserPermission userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionChange);
                //userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionEdit);
                //userper = oBook.Permission.Add("Everyone", MsoPermission.msoPermissionExtract);
                userper.ExpirationDate = dtExpireDate;
                /////////////////////////////////////
                string path = MyDir.FullName + "\\" + filename;

                oBook.SaveCopyAs(destPath);
                oBook.Close(false);
                oExcel.Quit();

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oExcel);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBook);
           
                GC.Collect();

                Session["key"] = filename1;                
                loading.Style.Add("visibility", "visible");
                lbl.Text = "Downloaded";
                up.Update();
                iframe.Attributes.Add("src", "Download.aspx");
                ClientScript.RegisterStartupScript(this.GetType(), "myStopFunction", "myStopFunction();", true);
                ClientScript.RegisterStartupScript(this.GetType(), "isvaliduploadClose", "isvaliduploadClose();", true);            

            }

            catch (Exception ex)
            {
                if ((ex.Message + "").Contains("Thread was being aborted."))
                {
                   
                }
                else
                {
                   
                }
           
            }
        }


        private void DownloadFileBEReport_Partial()
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
                    Open(Session["FullfileName"].ToString(), 0, false, 5, "", "", true,
                    Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                oModule = oBook.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);


                sCode = "sub BEReportMacro()\r\n" +
                    System.IO.File.ReadAllText(MyDir.FullName + "\\NewSheetsMacro_updated.txt") +
                        "\nend sub";

                oModule.CodeModule.AddFromString(sCode);

                oExcel.GetType().InvokeMember("Run",
                                System.Reflection.BindingFlags.Default |
                                System.Reflection.BindingFlags.InvokeMethod,
                                null, oExcel, new string[] { "BEReportMacro" });

                string finalname = Session["fileName"] + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
                //string finalname = "RevenueMomentum_rupali03_07Aug2015_1052.xlsx";
                if (MyDir.GetFiles().SingleOrDefault(k => k.Name == finalname) != null)
                    System.IO.File.Delete(MyDir.FullName + "\\" + finalname);

                oBook.SaveCopyAs(MyDir.FullName + "\\" + finalname);
                Session["key"] = finalname;

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
                loading.Style.Add("visibility", "visible");
                lbl.Text = "Downloaded";
                up.Update();
                iframe.Attributes.Add("src", "Download.aspx");

                ClientScript.RegisterStartupScript(this.GetType(), "myStopFunction", "myStopFunction();", true);
                ClientScript.RegisterStartupScript(this.GetType(), "isvaliduploadClose", "isvaliduploadClose();", true);
                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.AppendHeader("content-disposition", "attachment;  filename=" + finalname);

                //Response.WriteFile(path);

                //Response.Flush();
                //Response.End();



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
                    //Response.AppendHeader("content-disposition",
                    //    "attachment; filename=" + finalname);
                }
                //if (type != "")
                //    Response.ContentType = type;
                //Response.WriteFile(path);
                //Response.End();


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
                //loading.Style.Add("visibility", "hidden");
            }
        }


        protected void ddlSU_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mon == 4 || mon == 5 || mon == 6)
                curqtr = "Q1";
            else if (mon == 7 || mon == 8 || mon == 9)
                curqtr = "Q2";
            else if (mon == 10 || mon == 11 || mon == 12)
                curqtr = "Q3";
            else
                curqtr = "Q4";

            string userID = Session["UserID"] + "";

            string su = ddlSU.SelectedValue;

    

            if (su.ToLowerTrim() == "all")
            {
                ddlSU.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { su += k + ","; });
                su = su.Replace("ALLALL,", string.Empty);
                su = su.Trim().TrimEnd(',').TrimStart(',');
            }

            ddlQtr.SelectedIndex = ddlQtr.Items.IndexOf(ddlQtr.Items.FindByValue(curqtr));

            //ddlDH.DataTextField = "txtDHMailid";
            //ddlDH.DataValueField = "txtDHMailid";
            //ddlDH.DataSource = objbe.GetDHFromSuBeReport(userID, su);
            //ddlDH.DataBind();
            //ddlDH.Items.Insert(0, "ALL");
            //string dh = ddlDH.SelectedValue;

            //if (dh.ToLowerTrim() == "all")
            //{
            //    ddlDH.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { dh += k + ","; });
            //    dh = dh.Replace("ALLALL,", string.Empty);
            //    dh = dh.Trim().TrimEnd(',').TrimStart(',');
            //}

            //dh = dh.Trim();
            //ddlPU.DataTextField = "txtSBUCode";
            //ddlPU.DataValueField = "txtSBUCode";
            //ddlPU.DataSource = objbe.GetPUBeReport(dh);
            //ddlPU.DataBind();
            //ddlPU.Items.Insert(0, "ALL");

            //string pu = ddlPU.SelectedValue;

            //if (pu.ToLowerTrim() == "all")
            //{
            //    ddlPU.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { pu += k + ","; });
            //    pu = pu.Replace("ALLALL,", string.Empty);
            //    pu = pu.Trim().TrimEnd(',').TrimStart(',');
            //}
            //pu = pu.Trim();
            //hdnfldFlag.Value = "0";

            //ddlQtr.DataTextField = "txtqtr";
            //ddlQtr.DataValueField = "txtqtr";
            //ddlQtr.DataSource = objbe.GetBEReportQtrYear("Qtr", "0");
            //ddlQtr.DataBind();
            //ddlQtr.SelectedIndex = ddlQtr.Items.IndexOf(ddlQtr.Items.FindByValue(curqtr));

            //string qtr = ddlQtr.SelectedValue.Trim();
            //ddlYear.DataTextField = "txtyear";
            //ddlYear.DataValueField = "txtyear";
            //ddlYear.DataSource = objbe.GetBEReportQtrYear("Year", qtr);
            //ddlYear.DataBind();
        }

        protected void ddlPU_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mon == 4 || mon == 5 || mon == 6)
                curqtr = "Q1";
            else if (mon == 7 || mon == 8 || mon == 9)
                curqtr = "Q2";
            else if (mon == 10 || mon == 11 || mon == 12)
                curqtr = "Q3";
            else
                curqtr = "Q4";

            //string dh = ddlDH.SelectedValue;

            //if (dh.ToLowerTrim() == "all")
            //{
            //    ddlDH.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { dh += k + ","; });
            //    dh = dh.Replace("ALLALL,", string.Empty);
            //    dh = dh.Trim().TrimEnd(',').TrimStart(',');
            //}
            //dh = dh.Trim();

            //ddlPU.DataTextField = "txtSBUCode";
            //ddlPU.DataValueField = "txtSBUCode";
            //ddlPU.DataSource = objbe.GetPUBeReport(dh);
            //ddlPU.DataBind();
            //ddlPU.Items.Insert(0, "ALL");


            //string pu = ddlPU.SelectedValue;

            //if (pu.ToLowerTrim() == "all")
            //{
            //    ddlPU.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { pu += k + ","; });
            //    pu = pu.Replace("ALLALL,", string.Empty);
            //    pu = pu.Trim().TrimEnd(',').TrimStart(',');
            //}
            //pu = pu.Trim();
            //hdnfldFlag.Value = "0";
            //ddlQtr.DataTextField = "txtqtr";
            //ddlQtr.DataValueField = "txtqtr";
            //ddlQtr.DataSource = objbe.GetBEReportQtrYear("Qtr", "0");
            //ddlQtr.DataBind();

            //ddlQtr.SelectedIndex = ddlQtr.Items.IndexOf(ddlQtr.Items.FindByValue(curqtr));

            //string qtr = ddlQtr.SelectedValue.Trim();

            //ddlYear.DataTextField = "txtyear";
            //ddlYear.DataValueField = "txtyear";
            //ddlYear.DataSource = objbe.GetBEReportQtrYear("Year", qtr);
            //ddlYear.DataBind();
        }

        protected void ddlQtr_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qtr = ddlQtr.SelectedValue;
            ddlYear.DataTextField = "txtyear";
            ddlYear.DataValueField = "txtyear";
            ddlYear.DataSource = objbe.GetBEReportQtrYear("Year", qtr);
            ddlYear.DataBind();

            //string qtr = ddlQtr.SelectedValue;
            string yr = ddlYear.SelectedValue;

            //ddlDate.DataSource = objbe.GetDatesForDropDown(qtr, yr);
            //ddlDate.DataBind();
            //hdnfldFlag.Value = "0";
            GG.Update();

        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qtr = ddlQtr.SelectedValue;
            string yr = ddlYear.SelectedValue;

            //ddlDate.DataSource = objbe.GetDatesForDropDown(qtr, yr);
            //ddlDate.DataBind();

            // hdnfldFlag.Value = "0";


        }
    }
}