using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using BEData;
using VBIDE = Microsoft.Vbe.Interop;


public partial class RevenueMomentum : BasePage
    {
        public static string HiddenFieldEAS="";

        private BEDL service = new BEDL();
        Logger logger = new Logger();
        public DateTime dateTime = DateTime.Today;
        public string fileName = "Reports";
        static string yr = "";

        string PhysicalPath_Macro = "";
        string PhysicalPath_DownloadFiles = "";
        string PhysicalPath_Template = "";

        private string GetPathAndFileName(string path, string fileName)
        {
            return Path.Combine(path, fileName);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            

            base.ValidateSession();
            PhysicalPath_DownloadFiles = Server.MapPath("ExcelOperations\\DownloadFiles");
            PhysicalPath_Template = Server.MapPath("ExcelOperations\\Template");
            PhysicalPath_Macro = Server.MapPath("ExcelOperations\\Macro");

            if (!IsPostBack)
            {
                HiddenFieldEAS = Request.QueryString["Val"].ToString();

                string userID = Session["UserID"] + "";

                List<string> lstSU = service.GetSUForuser(userID);
                ddlSL.DataSource = lstSU.Select(k => k.ToString()).Distinct().ToList();
                ddlSL.DataBind();


                //if (HiddenFieldEAS == "1")
                //{
                //    ddlSL.Items.Insert(0, "All");
                //    ddlSL.Items.Insert(1, "EAIS");
                //    ddlSL.Items.Insert(2, "ECAS");
                //    ddlSL.Items.Insert(3, "ORC");
                //    ddlSL.Items.Insert(4, "SAP");
                   
                    
                //}
                //else
                //{
                //    ddlSL.Items.Insert(0, "All");
                //    ddlSL.Items.Insert(1, "ORC");
                //    ddlSL.Items.Insert(2, "SAP");
                //}

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                int currentYear = dateTime.Year; //DateTime.Now.Year;
                int year = DateTime.Today.Year;
                DateTime todaydate = dateTime;
                string strcurrent = "";
                string strcurrentNxt = "";
                yr = "";
                //if (todaydate.Month == 1 || todaydate.Month == 2 || todaydate.Month == 3)
                //{
                //    strcurrent = "Q4";
                //    strcurrentNxt = "Q1";
                //    yr = Convert.ToString(currentYear - 1) + '-' + (((currentYear - 1) - 2000) + 1).ToString();
                //}
                //else if (todaydate.Month == 4 || todaydate.Month == 5 || todaydate.Month == 6)
                //{
                //    strcurrent = "Q1";
                //    strcurrentNxt = "Q2";
                //    yr = Convert.ToString(currentYear) + '-' + ((currentYear - 2000) + 1).ToString();
                //}
                //else if (todaydate.Month == 7 || todaydate.Month == 8 || todaydate.Month == 9)
                //{
                //    strcurrent = "Q2";
                //    strcurrentNxt = "Q3";
                //    yr = Convert.ToString(currentYear) + '-' + ((currentYear - 2000) + 1).ToString();
                //}
                //else
                //{
                //    strcurrent = "Q3";
                //    strcurrentNxt = "Q4";
                //    yr = Convert.ToString(currentYear) + '-' + ((currentYear - 2000) + 1).ToString();
                //}

                string cmd = "select distinct dtFrozenDate as DumpDate from EAS_BEData_SDMWeekly_NSO where txtSnapshotQtr='" + strcurrent + "' order by dtFrozenDate desc";
                ds=service.GetDataSet(cmd);
                dt = ds.Tables[0];
                ddlBEWeeKDate.DataSource = dt;
                ddlBEWeeKDate.DataTextField = "DumpDate";
                ddlBEWeeKDate.DataValueField = "DumpDate";
                ddlBEWeeKDate.DataBind();
                //dateTime date=dateTime.today;
                var date1 = DateTime.Now;
                ddlBEWeeKDate.Items.Insert(0, date1.ToString());

               
                //ddlQuarter.Items.Insert(0, strcurrent);
                //ddlQuarter.Items.Insert(1, strcurrentNxt);


                ddlQuarter.Items.Clear();
                string currentQtr = DateUtility.GetQuarter("current");
                string nextQtr = DateUtility.GetQuarter("next");
                string nextQtrPlus1 = DateUtility.GetQuarter("next1");

                ddlQuarter.Items.Insert(0, currentQtr);
                ddlQuarter.Items.Insert(1, nextQtr);
                ddlQuarter.Items.Insert(2, nextQtrPlus1);
                ddlQuarter.Text = currentQtr;
 
            }
        }

        protected void btnreport_Click(object sender, EventArgs e)
        {

           

            int year = DateTime.Today.Year;
            DateTime todaydate = dateTime;
            string strcurrent = ddlQuarter.SelectedItem.Text;
            int currentYear = dateTime.Year; //DateTime.Now.Year;
            string yr = "";


            string finyr = strcurrent.Substring(3, 2);
            finyr = Convert.ToString(Convert.ToUInt32(finyr) - 1 + 2000) + '-' + (finyr);
            //if (strcurrent == "Q4")
            //{
            //    yr = "20" + Convert.ToString(currentYear - 2000 - 1) + '-' + (currentYear - 2000).ToString();
            //}
            //else if (strcurrent == "Q1")
            //{
            //    yr = Convert.ToString(currentYear) + '-' + ((currentYear - 2000) + 1).ToString();
            //}
            //else if (strcurrent == "Q2")
            //{
            //    yr = Convert.ToString(currentYear) + '-' + ((currentYear - 2000) + 1).ToString();
            //}
            //else
            //{
            //    yr = Convert.ToString(currentYear) + '-' + ((currentYear - 2000) + 1).ToString();
            //}

            string MachineUser = Session["MachineUser"].ToString();

            //string MachineUser = "glnrao";
            string MachineRole = Session["MachineRole"].ToString();
            
            string userID = Session["UserID"] + "";
            string currentQuarter = strcurrent.Substring(0, 2);
            string cmdtext = "select txtServiceLine from BEUserAccess_NSO where txtUserId='" + MachineUser + "'";
            DataSet ds = new DataSet();
            ds = service.GetDataSet(cmdtext);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            FileInfo file;
            try
            {
                var qtr = currentQuarter;
                var CurrYear = finyr;
                var userid = MachineUser;
                DataSet dsSL1 = new DataSet();
                DataSet dsSL2 = new DataSet();
                DataSet dsSL3 = new DataSet();
                DataSet dsSL4 = new DataSet();
                DataSet dsInpipeRev = new DataSet();
                DataSet dsInpipeVol = new DataSet();

                DataTable dt1SL1 = new DataTable();
                DataTable dt2SL1 = new DataTable();
                DataTable dt1SL2 = new DataTable();
                DataTable dt2SL2 = new DataTable();

                DataTable dt1SL3 = new DataTable();
                DataTable dt2SL3 = new DataTable();
                DataTable dt1SL4 = new DataTable();
                DataTable dt2SL4 = new DataTable();

                DataTable dt3SL1 = new DataTable();
                DataTable dt3SL2 = new DataTable();
                DataTable dt3SL3 = new DataTable();
                DataTable dt3SL4 = new DataTable();

                DataTable dtInpipeRev = new DataTable();
                DataTable dtInpipeVol = new DataTable();
            DataTable dtInpipeRev2 = new DataTable();
            DataTable dtInpipeVol2 = new DataTable();

            DataSet dsEAS = new DataSet();
                DataTable dtEAS = new DataTable();

                var tblComparisonReport1 = dt1SL1;
                var tblComparisonReport2 = dt2SL1;
                var tblComparisonReport3 = dt2SL2;
                var tblComparisonReport4 = dtEAS;
                var tblComparisonReport5 = dt1SL2;
                var tblComparisonReport6 = dt2SL3;
                var tblComparisonReport7 = dt2SL4;
                var tblComparisonReport8 = dt3SL1;
                var tblComparisonReport9 = dtInpipeRev;
                var tblComparisonReport10 = dtInpipeVol;

                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws;
                ExcelWorksheet ws1;
                ExcelWorksheet ws2;
                ExcelWorksheet ws3;
                ExcelWorksheet ws4;
                ExcelWorksheet ws5;
                ExcelWorksheet ws8;
                ExcelWorksheet ws9;
                ExcelWorksheet ws10;



            List<string> lstSL=new List<string>();

               foreach (ListItem item in ddlSL.Items)
               {
                   if (item.Selected == true)
                   {
                       lstSL.Add(item.Value.ToString());
                   }
               }

               if (MachineRole == "Admin" || MachineRole == "PnA" || MachineRole == "UH")
                {
                      if (lstSL.Count==4)
                        {
                            dsSL1 = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, "ORC", "RevenueMomentum", ddlBEWeeKDate.SelectedItem.Text, "Full","All");
                            dsSL2 = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, "SAP", "RevenueMomentum", ddlBEWeeKDate.SelectedItem.Text, "Full", "All");


                            dsSL3 = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, "ECAS", "RevenueMomentum", ddlBEWeeKDate.SelectedItem.Text, "Full", "All");
                            dsSL4 = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, "EAIS", "RevenueMomentum", ddlBEWeeKDate.SelectedItem.Text, "Full", "All");

                            dt1SL3 = dsSL3.Tables[0];
                            dt2SL3 = dsSL3.Tables[1];
                            dt1SL4 = dsSL4.Tables[0];
                            dt2SL4 = dsSL4.Tables[1];

                            dt3SL1 = dsSL1.Tables[2];

                            dt1SL1 = dsSL1.Tables[0];
                            dt2SL1 = dsSL1.Tables[1];
                            dt1SL2 = dsSL2.Tables[0];
                            dt2SL2 = dsSL2.Tables[1];

                            dtInpipeRev = dsSL1.Tables[3];
                            dtInpipeVol = dsSL1.Tables[4];

                    dt1SL1.Merge(dt1SL2);
                            dt1SL1.Merge(dt1SL3);
                            dt1SL1.Merge(dt1SL4);




                            


                            DataTable tbl_SL3, tbl_SL4;

                            tbl_SL3 = dt2SL3.Copy();
                            tbl_SL4 = dt2SL4.Copy();

                            DataTable left, right;
                            if (dt2SL1.Rows.Count > dt2SL2.Rows.Count)
                            { left = dt2SL1.Copy(); right = dt2SL2.Copy(); }
                            else
                            { left = dt2SL2.Copy(); right = dt2SL1.Copy(); }

                            for (int i = 0; i < left.Rows.Count; i++)
                            {
                                string main = left.Rows[i][0] + "";
                                var rows_temp = right.Select("Main = '" + main + "'");

                                var temp_SL3 = tbl_SL3.Select("Main = '" + main + "'");
                                var temp_SL4 = tbl_SL4.Select("Main = '" + main + "'");




                                if (rows_temp.Length == 1)
                                {
                                    DataRow row_temp = rows_temp[0];
                                    left.Rows[i][1] = Convert.ToDouble(left.Rows[i][1].ToString()) + Convert.ToDouble(row_temp[1].ToString()) ;  
                                    left.Rows[i][2] = Convert.ToDouble(left.Rows[i][2].ToString()) + Convert.ToDouble(row_temp[2].ToString()) ;  
                                    left.Rows[i][3] = Convert.ToDouble(left.Rows[i][3].ToString()) + Convert.ToDouble(row_temp[3].ToString()) ;  
                                    left.Rows[i][4] = Convert.ToDouble(left.Rows[i][4].ToString()) + Convert.ToDouble(row_temp[4].ToString()) ;  
                                    left.Rows[i][7] = Convert.ToDouble(left.Rows[i][7].ToString()) + Convert.ToDouble(row_temp[7].ToString()) ;  
                                    left.Rows[i][8] = Convert.ToDouble(left.Rows[i][8].ToString()) + Convert.ToDouble(row_temp[8].ToString()) ;
                                    left.Rows[i][9] = Convert.ToDouble(left.Rows[i][9].ToString()) + Convert.ToDouble(row_temp[9].ToString()) ;  
                                    left.Rows[i][10] = Convert.ToDouble(left.Rows[i][10].ToString()) + Convert.ToDouble(row_temp[10].ToString());
                                    left.Rows[i][13] = Convert.ToDouble(left.Rows[i][13].ToString()) + Convert.ToDouble(row_temp[13].ToString());
                                    left.Rows[i][14] = Convert.ToDouble(left.Rows[i][14].ToString()) + Convert.ToDouble(row_temp[14].ToString());
                                    left.Rows[i][15] = Convert.ToDouble(left.Rows[i][15].ToString()) + Convert.ToDouble(row_temp[15].ToString());
                                    left.Rows[i][16] = Convert.ToDouble(left.Rows[i][16].ToString()) + Convert.ToDouble(row_temp[16].ToString());

                                    left.Rows[i][19] = Convert.ToDouble(left.Rows[i][19].ToString()) + Convert.ToDouble(row_temp[19].ToString());
                                    left.Rows[i][20] = Convert.ToDouble(left.Rows[i][20].ToString()) + Convert.ToDouble(row_temp[20].ToString());
                                    left.Rows[i][21] = Convert.ToDouble(left.Rows[i][21].ToString()) + Convert.ToDouble(row_temp[21].ToString());
                                    left.Rows[i][22] = Convert.ToDouble(left.Rows[i][22].ToString()) + Convert.ToDouble(row_temp[22].ToString());

                                    left.Rows[i][25] = Convert.ToDouble(left.Rows[i][25].ToString()) + Convert.ToDouble(row_temp[25].ToString());
                                    left.Rows[i][26] = Convert.ToDouble(left.Rows[i][26].ToString()) + Convert.ToDouble(row_temp[26].ToString());
                                    left.Rows[i][27] = Convert.ToDouble(left.Rows[i][27].ToString()) + Convert.ToDouble(row_temp[27].ToString());
                                    left.Rows[i][28] = Convert.ToDouble(left.Rows[i][28].ToString()) + Convert.ToDouble(row_temp[28].ToString());

                                    left.Rows[i][31] = Convert.ToDouble(left.Rows[i][31].ToString()) + Convert.ToDouble(row_temp[31].ToString());
                                    left.Rows[i][32] = Convert.ToDouble(left.Rows[i][32].ToString()) + Convert.ToDouble(row_temp[32].ToString());
                                    left.Rows[i][33] = Convert.ToDouble(left.Rows[i][33].ToString()) + Convert.ToDouble(row_temp[33].ToString());
                                    left.Rows[i][34] = Convert.ToDouble(left.Rows[i][34].ToString()) + Convert.ToDouble(row_temp[34].ToString());
                                }

                                if (temp_SL3.Length == 1)
                                {
                                    DataRow row_SL3 = temp_SL3[0];
                                    left.Rows[i][1] = Convert.ToDouble(left.Rows[i][1].ToString()) + Convert.ToDouble(row_SL3[1].ToString());
                                    left.Rows[i][2] = Convert.ToDouble(left.Rows[i][2].ToString()) + Convert.ToDouble(row_SL3[2].ToString());
                                    left.Rows[i][3] = Convert.ToDouble(left.Rows[i][3].ToString()) + Convert.ToDouble(row_SL3[3].ToString());
                                    left.Rows[i][4] = Convert.ToDouble(left.Rows[i][4].ToString()) + Convert.ToDouble(row_SL3[4].ToString());
                                    left.Rows[i][7] = Convert.ToDouble(left.Rows[i][7].ToString()) + Convert.ToDouble(row_SL3[7].ToString());
                                    left.Rows[i][8] = Convert.ToDouble(left.Rows[i][8].ToString()) + Convert.ToDouble(row_SL3[8].ToString());
                                    left.Rows[i][9] = Convert.ToDouble(left.Rows[i][9].ToString()) + Convert.ToDouble(row_SL3[9].ToString());
                                    left.Rows[i][10] = Convert.ToDouble(left.Rows[i][10].ToString()) + Convert.ToDouble(row_SL3[10].ToString());
                                    left.Rows[i][13] = Convert.ToDouble(left.Rows[i][13].ToString()) + Convert.ToDouble(row_SL3[13].ToString());
                                    left.Rows[i][14] = Convert.ToDouble(left.Rows[i][14].ToString()) + Convert.ToDouble(row_SL3[14].ToString());
                                    left.Rows[i][15] = Convert.ToDouble(left.Rows[i][15].ToString()) + Convert.ToDouble(row_SL3[15].ToString());
                                    left.Rows[i][16] = Convert.ToDouble(left.Rows[i][16].ToString()) + Convert.ToDouble(row_SL3[16].ToString());

                                    left.Rows[i][19] = Convert.ToDouble(left.Rows[i][19].ToString()) + Convert.ToDouble(row_SL3[19].ToString());
                                    left.Rows[i][20] = Convert.ToDouble(left.Rows[i][20].ToString()) + Convert.ToDouble(row_SL3[20].ToString());
                                    left.Rows[i][21] = Convert.ToDouble(left.Rows[i][21].ToString()) + Convert.ToDouble(row_SL3[21].ToString());
                                    left.Rows[i][22] = Convert.ToDouble(left.Rows[i][22].ToString()) + Convert.ToDouble(row_SL3[22].ToString());
                                                                                                                        
                                    left.Rows[i][25] = Convert.ToDouble(left.Rows[i][25].ToString()) + Convert.ToDouble(row_SL3[25].ToString());
                                    left.Rows[i][26] = Convert.ToDouble(left.Rows[i][26].ToString()) + Convert.ToDouble(row_SL3[26].ToString());
                                    left.Rows[i][27] = Convert.ToDouble(left.Rows[i][27].ToString()) + Convert.ToDouble(row_SL3[27].ToString());
                                    left.Rows[i][28] = Convert.ToDouble(left.Rows[i][28].ToString()) + Convert.ToDouble(row_SL3[28].ToString());
                                                                                                                        
                                    left.Rows[i][31] = Convert.ToDouble(left.Rows[i][31].ToString()) + Convert.ToDouble(row_SL3[31].ToString());
                                    left.Rows[i][32] = Convert.ToDouble(left.Rows[i][32].ToString()) + Convert.ToDouble(row_SL3[32].ToString());
                                    left.Rows[i][33] = Convert.ToDouble(left.Rows[i][33].ToString()) + Convert.ToDouble(row_SL3[33].ToString());
                                    left.Rows[i][34] = Convert.ToDouble(left.Rows[i][34].ToString()) + Convert.ToDouble(row_SL3[34].ToString());
                                }

                                if (temp_SL4.Length == 1)
                                {
                                    DataRow row_SL4 = temp_SL4[0];
                                    left.Rows[i][1] = Convert.ToDouble(left.Rows[i][1].ToString()) + Convert.ToDouble(row_SL4[1].ToString());
                                    left.Rows[i][2] = Convert.ToDouble(left.Rows[i][2].ToString()) + Convert.ToDouble(row_SL4[2].ToString());
                                    left.Rows[i][3] = Convert.ToDouble(left.Rows[i][3].ToString()) + Convert.ToDouble(row_SL4[3].ToString());
                                    left.Rows[i][4] = Convert.ToDouble(left.Rows[i][4].ToString()) + Convert.ToDouble(row_SL4[4].ToString());
                                    left.Rows[i][7] = Convert.ToDouble(left.Rows[i][7].ToString()) + Convert.ToDouble(row_SL4[7].ToString());
                                    left.Rows[i][8] = Convert.ToDouble(left.Rows[i][8].ToString()) + Convert.ToDouble(row_SL4[8].ToString());
                                    left.Rows[i][9] = Convert.ToDouble(left.Rows[i][9].ToString()) + Convert.ToDouble(row_SL4[9].ToString());
                                    left.Rows[i][10] = Convert.ToDouble(left.Rows[i][10].ToString()) + Convert.ToDouble(row_SL4[10].ToString());
                                    left.Rows[i][13] = Convert.ToDouble(left.Rows[i][13].ToString()) + Convert.ToDouble(row_SL4[13].ToString());
                                    left.Rows[i][14] = Convert.ToDouble(left.Rows[i][14].ToString()) + Convert.ToDouble(row_SL4[14].ToString());
                                    left.Rows[i][15] = Convert.ToDouble(left.Rows[i][15].ToString()) + Convert.ToDouble(row_SL4[15].ToString());
                                    left.Rows[i][16] = Convert.ToDouble(left.Rows[i][16].ToString()) + Convert.ToDouble(row_SL4[16].ToString());
                                                                                                                        
                                    left.Rows[i][19] = Convert.ToDouble(left.Rows[i][19].ToString()) + Convert.ToDouble(row_SL4[19].ToString());
                                    left.Rows[i][20] = Convert.ToDouble(left.Rows[i][20].ToString()) + Convert.ToDouble(row_SL4[20].ToString());
                                    left.Rows[i][21] = Convert.ToDouble(left.Rows[i][21].ToString()) + Convert.ToDouble(row_SL4[21].ToString());
                                    left.Rows[i][22] = Convert.ToDouble(left.Rows[i][22].ToString()) + Convert.ToDouble(row_SL4[22].ToString());
                                                                                                                        
                                    left.Rows[i][25] = Convert.ToDouble(left.Rows[i][25].ToString()) + Convert.ToDouble(row_SL4[25].ToString());
                                    left.Rows[i][26] = Convert.ToDouble(left.Rows[i][26].ToString()) + Convert.ToDouble(row_SL4[26].ToString());
                                    left.Rows[i][27] = Convert.ToDouble(left.Rows[i][27].ToString()) + Convert.ToDouble(row_SL4[27].ToString());
                                    left.Rows[i][28] = Convert.ToDouble(left.Rows[i][28].ToString()) + Convert.ToDouble(row_SL4[28].ToString());
                                                                                                                        
                                    left.Rows[i][31] = Convert.ToDouble(left.Rows[i][31].ToString()) + Convert.ToDouble(row_SL4[31].ToString());
                                    left.Rows[i][32] = Convert.ToDouble(left.Rows[i][32].ToString()) + Convert.ToDouble(row_SL4[32].ToString());
                                    left.Rows[i][33] = Convert.ToDouble(left.Rows[i][33].ToString()) + Convert.ToDouble(row_SL4[33].ToString());
                                    left.Rows[i][34] = Convert.ToDouble(left.Rows[i][34].ToString()) + Convert.ToDouble(row_SL4[34].ToString());
                                }

                            }
                            left.AcceptChanges();
                            dtEAS = left;


                            
                            //}

                            tblComparisonReport1 = dt1SL1;
                            tblComparisonReport2 = dt2SL1;
                            tblComparisonReport3 = dt2SL2;
                            tblComparisonReport4 = dtEAS;
                            tblComparisonReport6 = dt2SL3;
                            tblComparisonReport7 = dt2SL4;
                            tblComparisonReport8 = dt3SL1;

                            if (tblComparisonReport1 == null || tblComparisonReport1.Rows.Count == 0 || tblComparisonReport2 == null || tblComparisonReport2.Rows.Count == 0 || tblComparisonReport3 == null || tblComparisonReport3.Rows.Count == 0 || tblComparisonReport4 == null || tblComparisonReport4.Rows.Count == 0 || tblComparisonReport7 == null || tblComparisonReport7.Rows.Count == 0 || tblComparisonReport6 == null || tblComparisonReport6.Rows.Count == 0)
                            {
                                lbl.Text = "";
                                Session["key"] = null;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
                                return;
                            }

                           // string folder = "ExcelOperations";
                            var MyDir = new DirectoryInfo(PhysicalPath_DownloadFiles);
                            string fileName = "RevenueMomentum_Digital_" + userID + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
                            file = new FileInfo(MyDir.FullName + "\\" + fileName);
                            if (MyDir.GetFiles().SingleOrDefault(k => k.Name == (fileName)) != null)
                                System.IO.File.Delete(MyDir.FullName + "\\" + fileName);

                            string sht = "Digital_BE_Data";
                            string sht1 = "ORC";
                            string sht2 = "SAP";
                            string sht3 = "EAS";
                            string sht4 = "ECAS";
                            string sht5 = "EAIS";
                            string sht6 = "Header";
                            string sht7 = "Inpipe Rev";
                            string sht8 = "Inpipe Vol";



                            int row = tblComparisonReport1.Rows.Count;
                            int col = tblComparisonReport1.Columns.Count;
                            int row1 = tblComparisonReport2.Rows.Count;
                            int col1 = tblComparisonReport2.Columns.Count;
                            int row2 = tblComparisonReport3.Rows.Count;
                            int col2 = tblComparisonReport3.Columns.Count;
                            int row3 = tblComparisonReport4.Rows.Count;
                            int col3 = tblComparisonReport4.Columns.Count;

                            int row4 = tblComparisonReport6.Rows.Count;
                            int col4 = tblComparisonReport6.Columns.Count;
                            int row5 = tblComparisonReport7.Rows.Count;
                            int col5 = tblComparisonReport7.Columns.Count;

                            int row8 = tblComparisonReport8.Rows.Count;
                            int col8 = tblComparisonReport8.Columns.Count;


                    int row9 = tblComparisonReport9.Rows.Count;
                    int col9 = tblComparisonReport9.Columns.Count;
                    int row10 = tblComparisonReport10.Rows.Count;
                    int col10 = tblComparisonReport10.Columns.Count;




                    ws = pck.Workbook.Worksheets.Add(sht);
                            ws.Cells["A1"].LoadFromDataTable(tblComparisonReport1, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill = ws.Cells[1, 1, 1, col].Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws.Cells[1, 1, 1, col].Style.Font.Bold = true;
                            ws.Cells[1, 1, row, col].AutoFitColumns();

                            ws1 = pck.Workbook.Worksheets.Add(sht1);
                            ws1.Cells["A1"].LoadFromDataTable(tblComparisonReport2, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill1 = ws1.Cells[1, 1, 1, col1].Style.Fill;
                            fill1.PatternType = ExcelFillStyle.Solid;
                            fill1.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws1.Cells[1, 1, 1, col1].Style.Font.Bold = true;
                            ws1.Cells[1, 1, row1, col1].AutoFitColumns();


                            ws2 = pck.Workbook.Worksheets.Add(sht2);
                            ws2.Cells["A1"].LoadFromDataTable(tblComparisonReport3, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill2 = ws2.Cells[1, 1, 1, col2].Style.Fill;
                            fill2.PatternType = ExcelFillStyle.Solid;
                            fill2.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws2.Cells[1, 1, 1, col2].Style.Font.Bold = true;
                            ws2.Cells[1, 1, row2, col2].AutoFitColumns();

                            ws3 = pck.Workbook.Worksheets.Add(sht3);
                            ws3.Cells["A1"].LoadFromDataTable(tblComparisonReport4, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill3 = ws2.Cells[1, 1, 1, col3].Style.Fill;
                            fill3.PatternType = ExcelFillStyle.Solid;
                            fill3.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws3.Cells[1, 1, 1, col3].Style.Font.Bold = true;
                            ws3.Cells[1, 1, row3, col3].AutoFitColumns();

                            ws4 = pck.Workbook.Worksheets.Add(sht4);
                            ws4.Cells["A1"].LoadFromDataTable(tblComparisonReport6, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill4 = ws4.Cells[1, 1, 1, col4].Style.Fill;
                            fill3.PatternType = ExcelFillStyle.Solid;
                            fill3.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws4.Cells[1, 1, 1, col4].Style.Font.Bold = true;
                            ws4.Cells[1, 1, row4, col4].AutoFitColumns();

                            ws5 = pck.Workbook.Worksheets.Add(sht5);
                            ws5.Cells["A1"].LoadFromDataTable(tblComparisonReport7, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill5 = ws5.Cells[1, 1, 1, col5].Style.Fill;
                            fill5.PatternType = ExcelFillStyle.Solid;
                            fill5.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws5.Cells[1, 1, 1, col5].Style.Font.Bold = true;
                            ws5.Cells[1, 1, row5, col5].AutoFitColumns();

                            ws8 = pck.Workbook.Worksheets.Add(sht6);
                            ws8.Cells["A1"].LoadFromDataTable(tblComparisonReport8, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill8 = ws8.Cells[1, 1, 1, col8].Style.Fill;
                            fill8.PatternType = ExcelFillStyle.Solid;
                            fill8.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws8.Cells[1, 1, 1, col8].Style.Font.Bold = true;
                            ws8.Cells[1, 1, row8, col8].AutoFitColumns();

                    ws9 = pck.Workbook.Worksheets.Add(sht7);
                    ws9.Cells["A1"].LoadFromDataTable(tblComparisonReport9, true);
                    //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                    var fill9 = ws9.Cells[1, 1, 1, col2].Style.Fill;
                    fill3.PatternType = ExcelFillStyle.Solid;
                    fill3.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    ws9.Cells[1, 1, 1, col2].Style.Font.Bold = true;
                    ws9.Cells[1, 1, row2, col2].AutoFitColumns();

                    ws10 = pck.Workbook.Worksheets.Add(sht8);
                    ws10.Cells["A1"].LoadFromDataTable(tblComparisonReport10, true);
                    //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                    var fill10 = ws10.Cells[1, 1, 1, col2].Style.Fill;
                    fill3.PatternType = ExcelFillStyle.Solid;
                    fill3.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    ws10.Cells[1, 1, 1, col2].Style.Font.Bold = true;
                    ws10.Cells[1, 1, row2, col2].AutoFitColumns();




                    pck.SaveAs(file);
                            pck.Dispose();
                            ReleaseObject(pck);
                            ReleaseObject(ws);
                            GenerateReport(fileName);
                        }



                        else if (lstSL.Count == 3)
                        {

                            var SL1 = lstSL[0];
                            var SL2 = lstSL[1];
                            var SL3 = lstSL[2];

                            dsSL1 = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, SL1, "RevenueMomentum", ddlBEWeeKDate.SelectedItem.Text, "Full", "All");
                            dsSL2 = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, SL2, "RevenueMomentum", ddlBEWeeKDate.SelectedItem.Text, "Full", "All");
                            dsSL3 = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, SL3, "RevenueMomentum", ddlBEWeeKDate.SelectedItem.Text, "Full", "All");
                            

                            dt1SL3 = dsSL3.Tables[0];
                            dt2SL3 = dsSL3.Tables[1];


                            dt3SL1 = dsSL1.Tables[2];

                            dt1SL1 = dsSL1.Tables[0];
                            dt2SL1 = dsSL1.Tables[1];
                            dt1SL2 = dsSL2.Tables[0];
                            dt2SL2 = dsSL2.Tables[1];

                            dt1SL1.Merge(dt1SL2);
                            dt1SL1.Merge(dt1SL3);

                            DataTable tbl_SL3;

                            tbl_SL3 = dt2SL3.Copy();


                            DataTable left, right;
                            if (dt2SL1.Rows.Count > dt2SL2.Rows.Count)
                            { left = dt2SL1.Copy(); right = dt2SL2.Copy(); }
                            else
                            { left = dt2SL2.Copy(); right = dt2SL1.Copy(); }

                            for (int i = 0; i < left.Rows.Count; i++)
                            {
                                string main = left.Rows[i][0] + "";
                                var rows_temp = right.Select("Main = '" + main + "'");

                                var temp_SL3 = tbl_SL3.Select("Main = '" + main + "'");


                                if (rows_temp.Length == 1)
                                {
                                    DataRow row_temp = rows_temp[0];
                                    left.Rows[i][1] = Convert.ToDouble(left.Rows[i][1].ToString()) + Convert.ToDouble(row_temp[1].ToString());
                                    left.Rows[i][2] = Convert.ToDouble(left.Rows[i][2].ToString()) + Convert.ToDouble(row_temp[2].ToString());
                                    left.Rows[i][3] = Convert.ToDouble(left.Rows[i][3].ToString()) + Convert.ToDouble(row_temp[3].ToString());
                                    left.Rows[i][4] = Convert.ToDouble(left.Rows[i][4].ToString()) + Convert.ToDouble(row_temp[4].ToString());
                                    left.Rows[i][7] = Convert.ToDouble(left.Rows[i][7].ToString()) + Convert.ToDouble(row_temp[7].ToString());
                                    left.Rows[i][8] = Convert.ToDouble(left.Rows[i][8].ToString()) + Convert.ToDouble(row_temp[8].ToString());
                                    left.Rows[i][9] = Convert.ToDouble(left.Rows[i][9].ToString()) + Convert.ToDouble(row_temp[9].ToString());
                                    left.Rows[i][10] = Convert.ToDouble(left.Rows[i][10].ToString()) + Convert.ToDouble(row_temp[10].ToString());
                                    left.Rows[i][13] = Convert.ToDouble(left.Rows[i][13].ToString()) + Convert.ToDouble(row_temp[13].ToString());
                                    left.Rows[i][14] = Convert.ToDouble(left.Rows[i][14].ToString()) + Convert.ToDouble(row_temp[14].ToString());
                                    left.Rows[i][15] = Convert.ToDouble(left.Rows[i][15].ToString()) + Convert.ToDouble(row_temp[15].ToString());
                                    left.Rows[i][16] = Convert.ToDouble(left.Rows[i][16].ToString()) + Convert.ToDouble(row_temp[16].ToString());

                                    left.Rows[i][19] = Convert.ToDouble(left.Rows[i][19].ToString()) + Convert.ToDouble(row_temp[19].ToString());
                                    left.Rows[i][20] = Convert.ToDouble(left.Rows[i][20].ToString()) + Convert.ToDouble(row_temp[20].ToString());
                                    left.Rows[i][21] = Convert.ToDouble(left.Rows[i][21].ToString()) + Convert.ToDouble(row_temp[21].ToString());
                                    left.Rows[i][22] = Convert.ToDouble(left.Rows[i][22].ToString()) + Convert.ToDouble(row_temp[22].ToString());

                                    left.Rows[i][25] = Convert.ToDouble(left.Rows[i][25].ToString()) + Convert.ToDouble(row_temp[25].ToString());
                                    left.Rows[i][26] = Convert.ToDouble(left.Rows[i][26].ToString()) + Convert.ToDouble(row_temp[26].ToString());
                                    left.Rows[i][27] = Convert.ToDouble(left.Rows[i][27].ToString()) + Convert.ToDouble(row_temp[27].ToString());
                                    left.Rows[i][28] = Convert.ToDouble(left.Rows[i][28].ToString()) + Convert.ToDouble(row_temp[28].ToString());

                                    left.Rows[i][31] = Convert.ToDouble(left.Rows[i][31].ToString()) + Convert.ToDouble(row_temp[31].ToString());
                                    left.Rows[i][32] = Convert.ToDouble(left.Rows[i][32].ToString()) + Convert.ToDouble(row_temp[32].ToString());
                                    left.Rows[i][33] = Convert.ToDouble(left.Rows[i][33].ToString()) + Convert.ToDouble(row_temp[33].ToString());
                                    left.Rows[i][34] = Convert.ToDouble(left.Rows[i][34].ToString()) + Convert.ToDouble(row_temp[34].ToString());
                                }

                                if (temp_SL3.Length == 1)
                                {
                                    DataRow row_SL3 = temp_SL3[0];
                                    left.Rows[i][1] = Convert.ToDouble(left.Rows[i][1].ToString()) + Convert.ToDouble(row_SL3[1].ToString());
                                    left.Rows[i][2] = Convert.ToDouble(left.Rows[i][2].ToString()) + Convert.ToDouble(row_SL3[2].ToString());
                                    left.Rows[i][3] = Convert.ToDouble(left.Rows[i][3].ToString()) + Convert.ToDouble(row_SL3[3].ToString());
                                    left.Rows[i][4] = Convert.ToDouble(left.Rows[i][4].ToString()) + Convert.ToDouble(row_SL3[4].ToString());
                                    left.Rows[i][7] = Convert.ToDouble(left.Rows[i][7].ToString()) + Convert.ToDouble(row_SL3[7].ToString());
                                    left.Rows[i][8] = Convert.ToDouble(left.Rows[i][8].ToString()) + Convert.ToDouble(row_SL3[8].ToString());
                                    left.Rows[i][9] = Convert.ToDouble(left.Rows[i][9].ToString()) + Convert.ToDouble(row_SL3[9].ToString());
                                    left.Rows[i][10] = Convert.ToDouble(left.Rows[i][10].ToString()) + Convert.ToDouble(row_SL3[10].ToString());
                                    left.Rows[i][13] = Convert.ToDouble(left.Rows[i][13].ToString()) + Convert.ToDouble(row_SL3[13].ToString());
                                    left.Rows[i][14] = Convert.ToDouble(left.Rows[i][14].ToString()) + Convert.ToDouble(row_SL3[14].ToString());
                                    left.Rows[i][15] = Convert.ToDouble(left.Rows[i][15].ToString()) + Convert.ToDouble(row_SL3[15].ToString());
                                    left.Rows[i][16] = Convert.ToDouble(left.Rows[i][16].ToString()) + Convert.ToDouble(row_SL3[16].ToString());

                                    left.Rows[i][19] = Convert.ToDouble(left.Rows[i][19].ToString()) + Convert.ToDouble(row_SL3[19].ToString());
                                    left.Rows[i][20] = Convert.ToDouble(left.Rows[i][20].ToString()) + Convert.ToDouble(row_SL3[20].ToString());
                                    left.Rows[i][21] = Convert.ToDouble(left.Rows[i][21].ToString()) + Convert.ToDouble(row_SL3[21].ToString());
                                    left.Rows[i][22] = Convert.ToDouble(left.Rows[i][22].ToString()) + Convert.ToDouble(row_SL3[22].ToString());

                                    left.Rows[i][25] = Convert.ToDouble(left.Rows[i][25].ToString()) + Convert.ToDouble(row_SL3[25].ToString());
                                    left.Rows[i][26] = Convert.ToDouble(left.Rows[i][26].ToString()) + Convert.ToDouble(row_SL3[26].ToString());
                                    left.Rows[i][27] = Convert.ToDouble(left.Rows[i][27].ToString()) + Convert.ToDouble(row_SL3[27].ToString());
                                    left.Rows[i][28] = Convert.ToDouble(left.Rows[i][28].ToString()) + Convert.ToDouble(row_SL3[28].ToString());

                                    left.Rows[i][31] = Convert.ToDouble(left.Rows[i][31].ToString()) + Convert.ToDouble(row_SL3[31].ToString());
                                    left.Rows[i][32] = Convert.ToDouble(left.Rows[i][32].ToString()) + Convert.ToDouble(row_SL3[32].ToString());
                                    left.Rows[i][33] = Convert.ToDouble(left.Rows[i][33].ToString()) + Convert.ToDouble(row_SL3[33].ToString());
                                    left.Rows[i][34] = Convert.ToDouble(left.Rows[i][34].ToString()) + Convert.ToDouble(row_SL3[34].ToString());
                                }

                                

                            }
                            left.AcceptChanges();
                            dtEAS = left;



                            //}

                            tblComparisonReport1 = dt1SL1;
                            tblComparisonReport2 = dt2SL1;
                            tblComparisonReport3 = dt2SL2;
                            tblComparisonReport4 = dtEAS;
                            tblComparisonReport6 = dt2SL3;
                            tblComparisonReport8 = dt3SL1;

                            if (tblComparisonReport1 == null || tblComparisonReport1.Rows.Count == 0 || tblComparisonReport2 == null || tblComparisonReport2.Rows.Count == 0 || tblComparisonReport3 == null || tblComparisonReport3.Rows.Count == 0 || tblComparisonReport4 == null || tblComparisonReport4.Rows.Count == 0 ||  tblComparisonReport6 == null || tblComparisonReport6.Rows.Count == 0)
                            {
                                lbl.Text = "";
                                Session["key"] = null;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
                                return;
                            }

                            // string folder = "ExcelOperations";
                            var MyDir = new DirectoryInfo(PhysicalPath_DownloadFiles);
                            string fileName = "RevenueMomentum_Digital_" + userID + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
                            file = new FileInfo(MyDir.FullName + "\\" + fileName);
                            if (MyDir.GetFiles().SingleOrDefault(k => k.Name == (fileName)) != null)
                                System.IO.File.Delete(MyDir.FullName + "\\" + fileName);

                            string sht = "Digital_BE_Data";
                            string sht1 = SL1;
                            string sht2 = SL2;
                            string sht3 = "EAS";
                            string sht4 = SL3;
                            string sht6 = "Header";
                    string sht7 = "";


                            int row = tblComparisonReport1.Rows.Count;
                            int col = tblComparisonReport1.Columns.Count;
                            int row1 = tblComparisonReport2.Rows.Count;
                            int col1 = tblComparisonReport2.Columns.Count;
                            int row2 = tblComparisonReport3.Rows.Count;
                            int col2 = tblComparisonReport3.Columns.Count;
                            int row3 = tblComparisonReport4.Rows.Count;
                            int col3 = tblComparisonReport4.Columns.Count;

                            int row4 = tblComparisonReport6.Rows.Count;
                            int col4 = tblComparisonReport6.Columns.Count;


                            int row8 = tblComparisonReport8.Rows.Count;
                            int col8 = tblComparisonReport8.Columns.Count;




                            ws = pck.Workbook.Worksheets.Add(sht);
                            ws.Cells["A1"].LoadFromDataTable(tblComparisonReport1, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill = ws.Cells[1, 1, 1, col].Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws.Cells[1, 1, 1, col].Style.Font.Bold = true;
                            ws.Cells[1, 1, row, col].AutoFitColumns();

                            ws1 = pck.Workbook.Worksheets.Add(sht1);
                            ws1.Cells["A1"].LoadFromDataTable(tblComparisonReport2, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill1 = ws1.Cells[1, 1, 1, col1].Style.Fill;
                            fill1.PatternType = ExcelFillStyle.Solid;
                            fill1.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws1.Cells[1, 1, 1, col1].Style.Font.Bold = true;
                            ws1.Cells[1, 1, row1, col1].AutoFitColumns();


                            ws2 = pck.Workbook.Worksheets.Add(sht2);
                            ws2.Cells["A1"].LoadFromDataTable(tblComparisonReport3, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill2 = ws2.Cells[1, 1, 1, col2].Style.Fill;
                            fill2.PatternType = ExcelFillStyle.Solid;
                            fill2.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws2.Cells[1, 1, 1, col2].Style.Font.Bold = true;
                            ws2.Cells[1, 1, row2, col2].AutoFitColumns();

                            ws3 = pck.Workbook.Worksheets.Add(sht3);
                            ws3.Cells["A1"].LoadFromDataTable(tblComparisonReport4, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill3 = ws2.Cells[1, 1, 1, col3].Style.Fill;
                            fill3.PatternType = ExcelFillStyle.Solid;
                            fill3.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws3.Cells[1, 1, 1, col3].Style.Font.Bold = true;
                            ws3.Cells[1, 1, row3, col3].AutoFitColumns();

                            ws4 = pck.Workbook.Worksheets.Add(sht4);
                            ws4.Cells["A1"].LoadFromDataTable(tblComparisonReport6, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill4 = ws4.Cells[1, 1, 1, col4].Style.Fill;
                            fill3.PatternType = ExcelFillStyle.Solid;
                            fill3.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws4.Cells[1, 1, 1, col4].Style.Font.Bold = true;
                            ws4.Cells[1, 1, row4, col4].AutoFitColumns();


                            ws8 = pck.Workbook.Worksheets.Add(sht6);
                            ws8.Cells["A1"].LoadFromDataTable(tblComparisonReport8, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill8 = ws8.Cells[1, 1, 1, col8].Style.Fill;
                            fill8.PatternType = ExcelFillStyle.Solid;
                            fill8.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws8.Cells[1, 1, 1, col8].Style.Font.Bold = true;
                            ws8.Cells[1, 1, row8, col8].AutoFitColumns();



                            pck.SaveAs(file);
                            pck.Dispose();
                            ReleaseObject(pck);
                            ReleaseObject(ws);
                            GenerateReport(fileName);
                        }


                        else if (lstSL.Count == 2)
                        {

                            var SL1 = lstSL[0];
                            var SL2 = lstSL[1];

                            dsSL1 = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, SL1, "RevenueMomentum", ddlBEWeeKDate.SelectedItem.Text, "Full", "All");
                            dsSL2 = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, SL2, "RevenueMomentum", ddlBEWeeKDate.SelectedItem.Text, "Full", "All");


                  

                            dt3SL1 = dsSL1.Tables[2];

                            dt1SL1 = dsSL1.Tables[0];
                            dt2SL1 = dsSL1.Tables[1];
                            dtInpipeRev = dsSL1.Tables[3];
                            dtInpipeVol = dsSL1.Tables[4];
                            dt1SL2 = dsSL2.Tables[0];
                            dt2SL2 = dsSL2.Tables[1];
                            dtInpipeRev2 = dsSL2.Tables[3];
                            dtInpipeVol2 = dsSL2.Tables[4];

                    dt1SL1.Merge(dt1SL2);
                    dtInpipeRev.Merge(dtInpipeRev2);
                    dtInpipeVol.Merge(dtInpipeVol2);
                    
                    DataTable left, right;
                            if (dt2SL1.Rows.Count > dt2SL2.Rows.Count)
                            { left = dt2SL1.Copy(); right = dt2SL2.Copy(); }
                            else
                            { left = dt2SL2.Copy(); right = dt2SL1.Copy(); }

                            for (int i = 0; i < left.Rows.Count; i++)
                            {
                                string main = left.Rows[i][0] + "";
                                var rows_temp = right.Select("Main = '" + main + "'");


                                if (rows_temp.Length == 1)
                                {
                                    DataRow row_temp = rows_temp[0];
                                    left.Rows[i][1] = Convert.ToDouble(left.Rows[i][1].ToString()) + Convert.ToDouble(row_temp[1].ToString());
                                    left.Rows[i][2] = Convert.ToDouble(left.Rows[i][2].ToString()) + Convert.ToDouble(row_temp[2].ToString());
                                    left.Rows[i][3] = Convert.ToDouble(left.Rows[i][3].ToString()) + Convert.ToDouble(row_temp[3].ToString());
                                    left.Rows[i][4] = Convert.ToDouble(left.Rows[i][4].ToString()) + Convert.ToDouble(row_temp[4].ToString());
                                    left.Rows[i][7] = Convert.ToDouble(left.Rows[i][7].ToString()) + Convert.ToDouble(row_temp[7].ToString());
                                    left.Rows[i][8] = Convert.ToDouble(left.Rows[i][8].ToString()) + Convert.ToDouble(row_temp[8].ToString());
                                    left.Rows[i][9] = Convert.ToDouble(left.Rows[i][9].ToString()) + Convert.ToDouble(row_temp[9].ToString());
                                    left.Rows[i][10] = Convert.ToDouble(left.Rows[i][10].ToString()) + Convert.ToDouble(row_temp[10].ToString());
                                    left.Rows[i][13] = Convert.ToDouble(left.Rows[i][13].ToString()) + Convert.ToDouble(row_temp[13].ToString());
                                    left.Rows[i][14] = Convert.ToDouble(left.Rows[i][14].ToString()) + Convert.ToDouble(row_temp[14].ToString());
                                    left.Rows[i][15] = Convert.ToDouble(left.Rows[i][15].ToString()) + Convert.ToDouble(row_temp[15].ToString());
                                    left.Rows[i][16] = Convert.ToDouble(left.Rows[i][16].ToString()) + Convert.ToDouble(row_temp[16].ToString());

                                    left.Rows[i][19] = Convert.ToDouble(left.Rows[i][19].ToString()) + Convert.ToDouble(row_temp[19].ToString());
                                    left.Rows[i][20] = Convert.ToDouble(left.Rows[i][20].ToString()) + Convert.ToDouble(row_temp[20].ToString());
                                    left.Rows[i][21] = Convert.ToDouble(left.Rows[i][21].ToString()) + Convert.ToDouble(row_temp[21].ToString());
                                    left.Rows[i][22] = Convert.ToDouble(left.Rows[i][22].ToString()) + Convert.ToDouble(row_temp[22].ToString());

                                    left.Rows[i][25] = Convert.ToDouble(left.Rows[i][25].ToString()) + Convert.ToDouble(row_temp[25].ToString());
                                    left.Rows[i][26] = Convert.ToDouble(left.Rows[i][26].ToString()) + Convert.ToDouble(row_temp[26].ToString());
                                    left.Rows[i][27] = Convert.ToDouble(left.Rows[i][27].ToString()) + Convert.ToDouble(row_temp[27].ToString());
                                    left.Rows[i][28] = Convert.ToDouble(left.Rows[i][28].ToString()) + Convert.ToDouble(row_temp[28].ToString());

                                    left.Rows[i][31] = Convert.ToDouble(left.Rows[i][31].ToString()) + Convert.ToDouble(row_temp[31].ToString());
                                    left.Rows[i][32] = Convert.ToDouble(left.Rows[i][32].ToString()) + Convert.ToDouble(row_temp[32].ToString());
                                    left.Rows[i][33] = Convert.ToDouble(left.Rows[i][33].ToString()) + Convert.ToDouble(row_temp[33].ToString());
                                    left.Rows[i][34] = Convert.ToDouble(left.Rows[i][34].ToString()) + Convert.ToDouble(row_temp[34].ToString());
                                }                               

                            }
                            left.AcceptChanges();
                            dtEAS = left;



                            //}

                            tblComparisonReport1 = dt1SL1;
                            tblComparisonReport2 = dt2SL1;
                            tblComparisonReport3 = dt2SL2;
                            tblComparisonReport4 = dtEAS;
                            tblComparisonReport8 = dt3SL1;

                    tblComparisonReport9 = dtInpipeRev;
                    tblComparisonReport10 = dtInpipeVol;

                    if (tblComparisonReport1 == null || tblComparisonReport1.Rows.Count == 0 || tblComparisonReport2 == null || tblComparisonReport2.Rows.Count == 0 || tblComparisonReport3 == null || tblComparisonReport3.Rows.Count == 0 || tblComparisonReport4 == null || tblComparisonReport4.Rows.Count == 0 )
                            {
                                lbl.Text = "";
                                Session["key"] = null;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
                                return;
                            }

                            // string folder = "ExcelOperations";
                            var MyDir = new DirectoryInfo(PhysicalPath_DownloadFiles);
                            string fileName = "RevenueMomentum_Digital_" + userID + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
                            file = new FileInfo(MyDir.FullName + "\\" + fileName);
                            if (MyDir.GetFiles().SingleOrDefault(k => k.Name == (fileName)) != null)
                                System.IO.File.Delete(MyDir.FullName + "\\" + fileName);

                            string sht = "Digital_BE_Data";
                            string sht1 = SL1;
                            string sht2 = SL2;
                            string sht3 = "EAS";
                            string sht6 = "Header";
                    string sht7 = "Inpipe_Rev";
                    string sht8 = "Inpipe_Vol";


                    int row = tblComparisonReport1.Rows.Count;
                            int col = tblComparisonReport1.Columns.Count;
                            int row1 = tblComparisonReport2.Rows.Count;
                            int col1 = tblComparisonReport2.Columns.Count;
                            int row2 = tblComparisonReport3.Rows.Count;
                            int col2 = tblComparisonReport3.Columns.Count;
                            int row3 = tblComparisonReport4.Rows.Count;
                            int col3 = tblComparisonReport4.Columns.Count;



                            int row8 = tblComparisonReport8.Rows.Count;
                            int col8 = tblComparisonReport8.Columns.Count;

                    int row9 = tblComparisonReport9.Rows.Count;
                    int col9 = tblComparisonReport9.Columns.Count;
                    int row10 = tblComparisonReport10.Rows.Count;
                    int col10 = tblComparisonReport10.Columns.Count;


                    ws = pck.Workbook.Worksheets.Add(sht);
                            ws.Cells["A1"].LoadFromDataTable(tblComparisonReport1, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill = ws.Cells[1, 1, 1, col].Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws.Cells[1, 1, 1, col].Style.Font.Bold = true;
                            ws.Cells[1, 1, row, col].AutoFitColumns();

                            ws1 = pck.Workbook.Worksheets.Add(sht1);
                            ws1.Cells["A1"].LoadFromDataTable(tblComparisonReport2, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill1 = ws1.Cells[1, 1, 1, col1].Style.Fill;
                            fill1.PatternType = ExcelFillStyle.Solid;
                            fill1.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws1.Cells[1, 1, 1, col1].Style.Font.Bold = true;
                            ws1.Cells[1, 1, row1, col1].AutoFitColumns();


                            ws2 = pck.Workbook.Worksheets.Add(sht2);
                            ws2.Cells["A1"].LoadFromDataTable(tblComparisonReport3, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill2 = ws2.Cells[1, 1, 1, col2].Style.Fill;
                            fill2.PatternType = ExcelFillStyle.Solid;
                            fill2.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws2.Cells[1, 1, 1, col2].Style.Font.Bold = true;
                            ws2.Cells[1, 1, row2, col2].AutoFitColumns();

                            ws3 = pck.Workbook.Worksheets.Add(sht3);
                            ws3.Cells["A1"].LoadFromDataTable(tblComparisonReport4, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill3 = ws2.Cells[1, 1, 1, col3].Style.Fill;
                            fill3.PatternType = ExcelFillStyle.Solid;
                            fill3.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws3.Cells[1, 1, 1, col3].Style.Font.Bold = true;
                            ws3.Cells[1, 1, row3, col3].AutoFitColumns();


                            ws8 = pck.Workbook.Worksheets.Add(sht6);
                            ws8.Cells["A1"].LoadFromDataTable(tblComparisonReport8, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill8 = ws8.Cells[1, 1, 1, col8].Style.Fill;
                            fill8.PatternType = ExcelFillStyle.Solid;
                            fill8.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws8.Cells[1, 1, 1, col8].Style.Font.Bold = true;
                            ws8.Cells[1, 1, row8, col8].AutoFitColumns();

                    ws9 = pck.Workbook.Worksheets.Add(sht7);
                    ws9.Cells["A1"].LoadFromDataTable(tblComparisonReport9, true);
                    //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                    var fill9 = ws9.Cells[1, 1, 1, col9].Style.Fill;
                    fill9.PatternType = ExcelFillStyle.Solid;
                    fill9.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    ws9.Cells[1, 1, 1, col9].Style.Font.Bold = true;
                    ws9.Cells[1, 1, row9, col9].AutoFitColumns();

                    ws10 = pck.Workbook.Worksheets.Add(sht8);
                    ws10.Cells["A1"].LoadFromDataTable(tblComparisonReport10, true);
                    //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                    var fill10 = ws10.Cells[1, 1, 1, col10].Style.Fill;
                    fill9.PatternType = ExcelFillStyle.Solid;
                    fill9.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    ws10.Cells[1, 1, 1, col10].Style.Font.Bold = true;
                    ws10.Cells[1, 1, row10, col10].AutoFitColumns();

                    pck.SaveAs(file);
                            pck.Dispose();
                            ReleaseObject(pck);
                            ReleaseObject(ws);
                            GenerateReport(fileName);
                        }


                        else if (lstSL.Count==1)
                        {
                            var SL1 = lstSL[0];

                            dsSL1 = service.GetBEReportRevenueMomentum(qtr, CurrYear, userid, SL1, "RevenueMomentum", ddlBEWeeKDate.SelectedItem.Text, "Full", "All");

                            dt3SL1 = dsSL1.Tables[2];
                            dt1SL1 = dsSL1.Tables[0];
                            dt2SL1 = dsSL1.Tables[1];
                    dtInpipeRev = dsSL1.Tables[3];
                    dtInpipeVol = dsSL1.Tables[4];

                    tblComparisonReport1 = dt1SL1;
                            tblComparisonReport2 = dt2SL1;
                            tblComparisonReport3 = dt3SL1;
                    tblComparisonReport4 = dtInpipeRev;
                    tblComparisonReport5 = dtInpipeVol;


                    if (tblComparisonReport1 == null || tblComparisonReport1.Rows.Count == 0 || tblComparisonReport2 == null || tblComparisonReport2.Rows.Count == 0)
                            {
                                lbl.Text = "";
                                Session["key"] = null;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('No Data to download!');</script>");
                                return;
                            }

                            //string folder = "ExcelOperations";
                            var MyDir = new DirectoryInfo(PhysicalPath_DownloadFiles);
                            string fileName = "RevenueMomentum_Digital_" + userID + "_" + DateTime.Now.ToString("ddMMMyyyy_HHmm") + ".xlsx";
                            file = new FileInfo(MyDir.FullName + "\\" + fileName);
                            if (MyDir.GetFiles().SingleOrDefault(k => k.Name == (fileName)) != null)
                                System.IO.File.Delete(MyDir.FullName + "\\" + fileName);

                            string sht = "Digital_BE_Data";
                            string sht1 = SL1;
                            string sht2 = "Header";


                            int row = tblComparisonReport1.Rows.Count;
                            int col = tblComparisonReport1.Columns.Count;
                            int row1 = tblComparisonReport2.Rows.Count;
                            int col1 = tblComparisonReport2.Columns.Count;
                            int row2 = tblComparisonReport3.Rows.Count;
                            int col2 = tblComparisonReport3.Columns.Count;
                    int row3 = tblComparisonReport4.Rows.Count;
                    int col3 = tblComparisonReport4.Columns.Count;
                    int row4 = tblComparisonReport5.Rows.Count;
                    int col4 = tblComparisonReport5.Columns.Count;




                    ws = pck.Workbook.Worksheets.Add(sht);
                            ws.Cells["A1"].LoadFromDataTable(tblComparisonReport1, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill = ws.Cells[1, 1, 1, col].Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws.Cells[1, 1, 1, col].Style.Font.Bold = true;
                            ws.Cells[1, 1, row, col].AutoFitColumns();

                            ws1 = pck.Workbook.Worksheets.Add(sht1);
                            ws1.Cells["A1"].LoadFromDataTable(tblComparisonReport2, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill1 = ws1.Cells[1, 1, 1, col1].Style.Fill;
                            fill1.PatternType = ExcelFillStyle.Solid;
                            fill1.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws1.Cells[1, 1, 1, col1].Style.Font.Bold = true;
                            ws1.Cells[1, 1, row1, col1].AutoFitColumns();

                            ws3 = pck.Workbook.Worksheets.Add(sht2);
                            ws3.Cells["A1"].LoadFromDataTable(tblComparisonReport3, true);
                            //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                            var fill3 = ws3.Cells[1, 1, 1, col2].Style.Fill;
                            fill3.PatternType = ExcelFillStyle.Solid;
                            fill3.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            ws3.Cells[1, 1, 1, col2].Style.Font.Bold = true;
                            ws3.Cells[1, 1, row2, col2].AutoFitColumns();
                    string SHT7 = "Inpipe_Rev";
                    ws4 = pck.Workbook.Worksheets.Add(SHT7);
                    ws4.Cells["A1"].LoadFromDataTable(tblComparisonReport4, true);
                    //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                    var fill4 = ws4.Cells[1, 1, 1, col3].Style.Fill;
                    fill4.PatternType = ExcelFillStyle.Solid;
                    fill4.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    ws4.Cells[1, 1, 1, col3].Style.Font.Bold = true;
                    ws4.Cells[1, 1, row3, col3].AutoFitColumns();

                    string SHT8 = "Inpipe_Vol";
                    ws5 = pck.Workbook.Worksheets.Add(SHT8);
                    ws5.Cells["A1"].LoadFromDataTable(tblComparisonReport5, true);
                    //ws.Cells[1, 1, 1, 38].Style.Font.Bold = true;
                    var fill5 = ws5.Cells[1, 1, 1, col4].Style.Fill;
                    fill5.PatternType = ExcelFillStyle.Solid;
                    fill5.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    ws5.Cells[1, 1, 1, col4].Style.Font.Bold = true;
                    ws5.Cells[1, 1, row4, col4].AutoFitColumns();


                    pck.SaveAs(file);
                            pck.Dispose();
                            ReleaseObject(pck);
                            ReleaseObject(ws);
                            GenerateReport(fileName);
                        }
    
                }
                
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "<script language=JavaScript>alert('You are not authorized to download the report!');</script>");
                    return;
                }




                //  hdnfldFlag.Value = "1";
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

        void GenerateReport(string fname)
        {


            Microsoft.Office.Interop.Excel.Application oExcel;
            Microsoft.Office.Interop.Excel.Workbook oBook = default(Microsoft.Office.Interop.Excel.Workbook);
            VBIDE.VBComponent oModule;
            //try
            {
                
              //  string folder = "ExcelOperations";
                var myDir = new DirectoryInfo(PhysicalPath_DownloadFiles);
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
                          GetVariableDeclaration("HiddenFieldEAS", MacroDataType.Integer, HiddenFieldEAS) +
                        System.IO.File.ReadAllText(PhysicalPath_Macro + "\\RevenueMomentumMacro_NSO.txt") +
                            "\nend sub";
                    oModule.CodeModule.AddFromString(sCode);
               
                oExcel.GetType().InvokeMember("Run",
                                System.Reflection.BindingFlags.Default |
                                System.Reflection.BindingFlags.InvokeMethod,
                                null, oExcel, new string[] { "Macro" });
              
                //Adding permission to excel file//

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


                Session["key"] = fname;
                //Session["data"] = table;
                loading.Style.Add("visibility", "visible");
                lbl.Text = "Downloaded";
                up.Update();

                iframe.Attributes.Add("src", "Download.aspx");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myStopFunction", "myStopFunction()", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "isvaliduploadClose", "isvaliduploadClose()", true);

            }

        }



 
    }
