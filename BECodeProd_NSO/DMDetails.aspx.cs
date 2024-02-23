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





    public partial class DMDetails : BasePage
    {

        private BEDL service = new BEDL();
        List<string> lstFinMapping = new List<string>();
        Logger logger = new Logger();
        public string fileName = "BEData.DMDetails.cs";
        BEDL objbe = new BEDL();
        public DateTime dateTime = DateTime.Today;
        decimal DMMonth1_total = default(decimal);
        decimal DMMonth2_total = default(decimal);
        decimal DMMonth3_total = default(decimal);

        decimal DMQCur_total = default(decimal);
        //decimal DMQNext_total = default(decimal);
        decimal BK1_total = default(decimal);

        decimal BK2_total = default(decimal);
        decimal BK3_total = default(decimal);
        decimal BK4_total = default(decimal);
        decimal BK_total = default(decimal);
        decimal Diff = default(decimal);
        decimal DMBE = default(decimal);
        decimal DMVolBE = default(decimal);
        decimal VolOn1 = default(decimal);
        decimal VolOn2 = default(decimal);
        decimal VolOn3 = default(decimal);
        decimal VolOff1 = default(decimal);
        decimal VolOff2 = default(decimal);
        decimal VolOff3 = default(decimal);
        decimal VolOnTotal = default(decimal);
        decimal VolOffTotal = default(decimal);
        decimal VolTotal = default(decimal);
        decimal AlconOn1 = default(decimal);
        decimal AlconOn2 = default(decimal);
        decimal AlconOn3 = default(decimal);
        decimal AlconOff1 = default(decimal);
        decimal AlconOff2 = default(decimal);
        decimal AlconOff3 = default(decimal);
        decimal AlconOnTotal = default(decimal);
        decimal AlconOffTotal = default(decimal);
        decimal AlconTotal = default(decimal);
        decimal PBSOn1 = default(decimal);
        decimal PBSOn2 = default(decimal);
        decimal PBSOn3 = default(decimal);
        decimal PBSOff1 = default(decimal);
        decimal PBSOff2 = default(decimal);
        decimal PBSOff3 = default(decimal);
        decimal PBSOnTotal = default(decimal);
        decimal PBSOffTotal = default(decimal);
        decimal PBSTotal = default(decimal);



        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            if (!Page.IsPostBack)
            {
                string beID = Request.QueryString["ID"] + "";

                string data = "select * from [EAS_BEData_SDM_NSO] where [intBEId]=" + beID + "";
                DataSet dsdata = service.GetDataSet(data);
                DataTable dtData = dsdata.Tables[0];
                string qtr = dtData.Rows[0]["txtQuarter"].ToString();
                string Nc = dtData.Rows[0]["txtNativeCurrency"].ToString();
                //string PU = dtData.Rows[0]["txtPU"].ToString();
                string offering = dtData.Rows[0]["Service Offering Code"].ToString();
                string MCC = dtData.Rows[0]["txtMasterClientCode"].ToString();
                string year = dtData.Rows[0]["txtFYYR"].ToString();

                string OtherCurrencies = "select distinct txtNativeCurrency from EAS_BEData_SDM_NSO where [Service Offering Code] ='" + offering + "' and txtMasterClientCode='" + MCC + "' and txtNativeCurrency not in ('" + Nc + "')";
                DataSet curr = service.GetDataSet(OtherCurrencies);
                DataTable othercurr = curr.Tables[0];
                if(othercurr.Rows.Count>0)
                {
                     lblCurr.Visible=true;
                lblCurr.Text = "Note: Other Currencies ";
                for (int i = 0; i < othercurr.Rows.Count; i++)
                {
                    lblCurr.Text += othercurr.Rows[i][0].ToString() +  ",";
                
                }
                lblCurr.Text += " exist for Service Offering Code - " + offering + ", master customer - " + MCC ;
                }
                else
                {
                    lblCurr.Visible = true;
                    lblCurr.Text = "Service Offering Code - " + offering + ", master customer - " + MCC + " , Native Currency - " + Nc ;
                }

                string serviceLine = dtData.Rows[0]["txtServiceLine"].ToString();
                string alcon = "Exec EAS_SP_GetAlcon_Digital '" + MCC + "','" + serviceLine + "','" + qtr + "','" + Nc + "','" + offering + "'";
                DataSet dsAlcon = service.GetDataSet(alcon);
                DataTable dtAlcon = dsAlcon.Tables[0];
                if (dtAlcon.Rows.Count > 0)
                {
                    foreach (DataRow row in dtAlcon.Rows)
                    {
                        AlconOn1 += GetDecimalCellValue(row, "M1Onsite");
                        AlconOn2 += GetDecimalCellValue(row, "M2Onsite");
                        AlconOn3 += GetDecimalCellValue(row, "M3Onsite");
                        AlconOff1 += GetDecimalCellValue(row, "M1Offsite");
                        AlconOff2 += GetDecimalCellValue(row, "M2Offsite");
                        AlconOff3 += GetDecimalCellValue(row, "M3Offsite");

                        AlconOnTotal += GetDecimalCellValue(row, "TotalOnsite");
                        AlconOffTotal += GetDecimalCellValue(row, "TotalOffsite");
                        AlconTotal += GetDecimalCellValue(row, "TotalVol");
                    }
                    gvAlcon.DataSource = dtAlcon;
                    gvAlcon.DataBind();
                }
                string PBS = "Exec EAS_SP_GetPBS_Digital '" + MCC + "','" + serviceLine + "','" + qtr + "','" + Nc + "','" + offering + "'";
                DataSet dsPBS = service.GetDataSet(PBS);
                DataTable dtPBS = dsPBS.Tables[0];
                if (dtPBS.Rows.Count > 0)
                {
                    foreach (DataRow row in dtPBS.Rows)
                    {


                        PBSOn1 += GetDecimalCellValue(row, "M1Onsite");
                        PBSOn2 += GetDecimalCellValue(row, "M2Onsite");
                        PBSOn3 += GetDecimalCellValue(row, "M3Onsite");
                        PBSOff1 += GetDecimalCellValue(row, "M1Offsite");
                        PBSOff2 += GetDecimalCellValue(row, "M2Offsite");
                        PBSOff3 += GetDecimalCellValue(row, "M3Offsite");

                        PBSOnTotal += GetDecimalCellValue(row, "TotalOnsite");
                        PBSOffTotal += GetDecimalCellValue(row, "TotalOffsite");
                        PBSTotal += GetDecimalCellValue(row, "TotalVol");
                    }
                    gvPBS.DataSource = dtPBS;
                    gvPBS.DataBind();
                }
                 //string id = Request.QueryString["b eID"] + "";
                //string MCC = Request.QueryString["mcc"] + "";
                //string Nc = Request.QueryString["nc"] + "";
               
                //string qtr = Request.QueryString["qtr"] + "";
                //string qtr1 = qtr.Trim();
             
                //string year = Request.QueryString["FYyear"] + "";
                //string PU = Request.QueryString["Pu"] + "";
               
                //string serviceLine = Request.QueryString["ServiceLine"] + "";

               // string cmdtext = "SELECT [txtMasterClientCode],[txtNativeCurrency],[txtDMMailId],round([fltDMMonth1BE],2) as [fltDMMonth1BE],round([fltDMMonth2BE],2) as [fltDMMonth2BE],round([fltDMMonth3BE],2) as [fltDMMonth3BE],round([fltDMQuarterBE],2) as  [fltDMQuarterBE],round([fltBK1],2) as [fltBK1],round([fltBK2],2) as [fltBK2],round([fltBK3],2) as [fltBK3] ,round([fltBK4],2) as [fltBK4],round([fltDMMonth1onsite],2) as [fltDMMonth1onsite],round([fltDMMonth1offsite],2) as [fltDMMonth1offsite],round([fltDMMonth2onsite],2) as [fltDMMonth2onsite],round([fltDMMonth2offsite],2) as  [fltDMMonth2offsite],round(fltDMMonth3onsite,2) as [fltDMMonth3onsite],round([fltDMMonth3offsite],2) as [fltDMMonth3offsite],round([fltDMTotalonsite],2) as [fltDMTotalonsite] ,round([fltDMTotaloffsite],2) as [fltDMTotaloffsite],round([fltDmTotalVolume],2) as [fltDmTotalVolume]   FROM [EAS_BEData_DM] where [txtPU]='" + PU + "' and [txtMasterClientCode]='" + MCC + "' and [txtNativeCurrency]='" + Nc + "' and [txtQuarter]='" + qtr + "' and [txtFYYR]='" + year + "' and [txtServiceLine]='" + serviceLine + "'";
                    // string cmdtext = "SELECT [txtMasterClientCode],[txtNativeCurrency],round([fltDMMonth1BE],2) as [fltDMMonth1BE],round([fltDMMonth2BE],2) as [fltDMMonth2BE],round([fltDMMonth3BE],2) as [fltDMMonth3BE],round([fltDMQuarterBE],2) as  [fltDMQuarterBE],round([fltDMBK1],2) as [fltDMBK1],round([fltDMBK2],2) as [fltDMBK2],round([fltDMBK3],2) as [fltDMBK3] ,round([fltDMBK4],2) as [fltDMBK4] FROM [EAS_BEData_SDM_NSO_DM] where [intBEId]=" + id + "";
              //  DataSet ds = service.GetDataSet(cmdtext);
               // string cmdtextVol = "SELECT [txtMasterClientCode],[txtDMMailId],round([fltDMMonth1BE],2) as [fltDMMonth1BE],round([fltDMMonth2BE],2) as [fltDMMonth2BE],round([fltDMMonth3BE],2) as [fltDMMonth3BE],round([fltDMQuarterBE],2) as  [fltDMQuarterBE],round([fltBK1],2) as [fltBK1],round([fltBK2],2) as [fltBK2],round([fltBK3],2) as [fltBK3] ,round([fltBK4],2) as [fltBK4],round([fltDMMonth1onsite],2) as [fltDMMonth1onsite],round([fltDMMonth1offsite],2) as [fltDMMonth1offsite],round([fltDMMonth2onsite],2) as [fltDMMonth2onsite],round([fltDMMonth2offsite],2) as  [fltDMMonth2offsite],round(fltDMMonth3onsite,2) as [fltDMMonth3onsite],round([fltDMMonth3offsite],2) as [fltDMMonth3offsite],round([fltDMTotalonsite],2) as [fltDMTotalonsite] ,round([fltDMTotaloffsite],2) as [fltDMTotaloffsite],round([fltDmTotalVolume],2) as [fltDmTotalVolume]    FROM [EAS_BEData_DM] where [txtPU]='" + PU + "' and [txtMasterClientCode]='" + MCC + "' and [txtQuarter]='" + qtr + "' and [txtFYYR]='" + year + "' and [txtServiceLine]='" + serviceLine + "'";

                string cmdtext = "SELECT [txtMasterClientCode],[txtNativeCurrency],[txtDMMailId],round(sum([fltDMMonth1BE]),2) as [fltDMMonth1BE],round(sum([fltDMMonth2BE]),2) as [fltDMMonth2BE],round(sum([fltDMMonth3BE]),2) as [fltDMMonth3BE],round(sum([fltDMQuarterBE]),2) as  [fltDMQuarterBE],round(sum([fltBK1]),2) as [fltBK1],round(sum([fltBK2]),2) as [fltBK2],round(sum([fltBK3]),2) as [fltBK3] ,round(sum([fltBK4]),2) as [fltBK4],[txtDMBERemarks] FROM [EAS_BEData_DM_NSO] where [Service Offering Code]='" + offering + "' and [txtMasterClientCode]='" + MCC + "' and [txtNativeCurrency]='" + Nc + "' and [txtQuarter]='" + qtr + "' and [txtFYYR]='" + year + "' and [txtServiceLine]='" + serviceLine + "' group by txtDMMailId,txtMasterClientCode,txtNativeCurrency,[txtDMBERemarks]";
                // string cmdtext = "SELECT [txtMasterClientCode],[txtNativeCurrency],round([fltDMMonth1BE],2) as [fltDMMonth1BE],round([fltDMMonth2BE],2) as [fltDMMonth2BE],round([fltDMMonth3BE],2) as [fltDMMonth3BE],round([fltDMQuarterBE],2) as  [fltDMQuarterBE],round([fltDMBK1],2) as [fltDMBK1],round([fltDMBK2],2) as [fltDMBK2],round([fltDMBK3],2) as [fltDMBK3] ,round([fltDMBK4],2) as [fltDMBK4] FROM [EAS_BEData_SDM_NSO_DM] where [intBEId]=" + id + "";
                DataSet ds = service.GetDataSet(cmdtext);
                string cmdtextVol = "SELECT [txtMasterClientCode],[txtNativeCurrency],[txtDMMailId],round(sum([fltDMMonth1onsite]),2) as [fltDMMonth1onsite],round(sum([fltDMMonth1offsite]),2) as [fltDMMonth1offsite],round(sum([fltDMMonth2onsite]),2) as [fltDMMonth2onsite],round(sum([fltDMMonth2offsite]),2) as  [fltDMMonth2offsite],round(sum(fltDMMonth3onsite),2) as [fltDMMonth3onsite],round(sum([fltDMMonth3offsite]),2) as [fltDMMonth3offsite],round(sum([fltDMTotalonsite]),2) as [fltDMTotalonsite] ,round(sum([fltDMTotaloffsite]),2) as [fltDMTotaloffsite],round(sum([fltDmTotalVolume]),2) as [fltDmTotalVolume]   FROM [EAS_BEData_DM_NSO] where [Service Offering Code]='" + offering + "' and [txtMasterClientCode]='" + MCC + "' and [txtQuarter]='" + qtr + "' and [txtFYYR]='" + year + "' and [txtServiceLine]='" + serviceLine + "' and [txtNativeCurrency]='" + Nc + "' group by txtDMMailId,txtMasterClientCode,txtNativeCurrency";


                // string cmdtext = "SELECT [txtMasterClientCode],[txtNativeCurrency],round([fltDMMonth1BE],2) as [fltDMMonth1BE],round([fltDMMonth2BE],2) as [fltDMMonth2BE],round([fltDMMonth3BE],2) as [fltDMMonth3BE],round([fltDMQuarterBE],2) as  [fltDMQuarterBE],round([fltDMBK1],2) as [fltDMBK1],round([fltDMBK2],2) as [fltDMBK2],round([fltDMBK3],2) as [fltDMBK3] ,round([fltDMBK4],2) as [fltDMBK4] FROM [EAS_BEData_SDM_NSO_NSO_DM] where [intBEId]=" + id + "";
                DataSet dsVol = service.GetDataSet(cmdtextVol);

                string rtbrText = "Exec EAS_SP_GetRtbr_Digital '" + beID + "'";
                DataSet dsrtbr = service.GetDataSet(rtbrText);
                DataTable dtrtbr = dsrtbr.Tables[0];
                if (dtrtbr.Rows.Count > 0)
                {
                    gvrtbr.DataSource = dtrtbr;
                    gvrtbr.DataBind();
                }


                if(ds.Tables[0].Rows.Count>0)
                {
                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    DMMonth1_total += GetDecimalCellValue(row, "fltDMMonth1BE");
                    DMMonth2_total += GetDecimalCellValue(row, "fltDMMonth2BE");
                    DMMonth3_total += GetDecimalCellValue(row, "fltDMMonth3BE");
                    DMQCur_total += GetDecimalCellValue(row, "fltDMQuarterBE");
                    BK1_total += GetDecimalCellValue(row, "fltBK1");
                    BK2_total += GetDecimalCellValue(row, "fltBK2");
                    BK3_total += GetDecimalCellValue(row, "fltBK3");
                    BK4_total += GetDecimalCellValue(row, "fltBK4");
                }
                grdBEDMView.DataSource = ds.Tables[0];
                grdBEDMView.DataBind();
               }

                if (dsVol.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsVol.Tables[0].Rows)
                    {
                        VolOn1 += GetDecimalCellValue(row, "fltDMMonth1onsite");
                        VolOn2 += GetDecimalCellValue(row, "fltDMMonth2onsite");
                        VolOn3 += GetDecimalCellValue(row, "fltDMMonth3onsite");
                        VolOff1 += GetDecimalCellValue(row, "fltDMMonth1offsite");
                        VolOff2 += GetDecimalCellValue(row, "fltDMMonth2offsite");
                        VolOff3 += GetDecimalCellValue(row, "fltDMMonth3offsite");

                        VolOnTotal += GetDecimalCellValue(row, "fltDMTotalonsite");
                        VolOffTotal += GetDecimalCellValue(row, "fltDMTotaloffsite");
                        VolTotal += GetDecimalCellValue(row, "fltDmTotalVolume");


                    }

                    grdBEDMViewVol.DataSource = dsVol.Tables[0];
                    grdBEDMViewVol.DataBind();
                }

            }

        }
        private decimal GetDecimalCellValue(DataRow row, string columnName)
        {
            decimal returnValue = default(decimal);

            string value = (row[columnName] + "").Length == 0 ? "0" : row[columnName] + "";
            returnValue = Convert.ToDecimal(value);


            return returnValue;
        }
        protected void grdBEDMView_RowCreated(object sender, GridViewRowEventArgs e)
        {


            //try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView objGridView = (GridView)sender;


                    GridViewRow objgridviewrow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    TableCell objtablecell = new TableCell();


                    AddMergedCells(objgridviewrow, objtablecell, 12, "DM Digital Revenue Projections", "#c41502");
                    


                    objGridView.Controls[0].Controls.AddAt(0, objgridviewrow);


                    int year = DateTime.Today.Year;

                    DateTime todaydate = dateTime;
                    int nxtyr;

                 
                    string qtr = Session["quarter"] + "";


                    var row = e.Row;

                    int currentMonth = DateTime.Now.Month;// dateTime.Month; //DateTime.Now.Month;
                    int currentYear = dateTime.Year; //DateTime.Now.Year;
                    string currentQuarter = qtr;

                    currentYear = currentYear - 2000;
                    string _CurrentQ = string.Empty;
                    //string _NextQ = string.Empty;

                    _CurrentQ = Session["currqtr"] + "";

                    //dm--dmmailid column is removed



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

                    string mon1 = " " + _month1 + "'" + currentYear + " ";
                    string mon2 = " " + _month2 + "'" + currentYear + " ";
                    string mon3 = " " + _month3 + "'" + currentYear + " ";


                    row.Cells[3].Text = " " + _month1 + "'" + currentYear + " ";
                    row.Cells[4].Text = " " + _month2 + "'" + currentYear + " ";
                    row.Cells[5].Text = " " + _month3 + "'" + currentYear + " ";
                    row.Cells[6].Text = "" + _CurrentQ + " BE";

                
                 


                    string constt = "";


                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {





                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {


                    for (int i = 0; i < 11; i++)
                    {
                        e.Row.Cells[i].CssClass = "FooterTotal";
                        // e.Row.Cells[i].ForeColor = System.Drawing.Color.White;
                        e.Row.Cells[i].ForeColor = System.Drawing.Color.White;
                        //e.Row.Cells[i].BackColor = System.Drawing.Color.FromName("#CC0000");
                        //e.Row.Cells[i].Attributes.Add("class", "footerBox");
                        e.Row.Cells[i].BackColor = System.Drawing.Color.FromArgb(51, 51, 51);
                    }

                    //TODO:18/12 dm mailid removed
                    e.Row.Cells[3].Text = DMMonth1_total + "";
                    e.Row.Cells[4].Text = DMMonth2_total + "";
                    e.Row.Cells[5].Text = DMMonth3_total + "";
                    e.Row.Cells[6].Text = DMQCur_total + "";
                    //e.Row.Cells[10].Text = DMQNext_total + ""; //TODO
                    //e.Row.Cells[5].Text = DMQPrev_total + "";
                    e.Row.Cells[7].Text = BK1_total + "";
                    e.Row.Cells[8].Text = BK2_total + "";
                    e.Row.Cells[9].Text = BK3_total + "";
                    e.Row.Cells[10].Text = BK4_total + "";
                 

                }



            }





        }
        protected void gvrtbr_RowCreated(object sender, GridViewRowEventArgs e)
        {


            //try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {

                    GridView objGridView = (GridView)sender;


                    GridViewRow objgridviewrow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    TableCell objtablecell = new TableCell();

                   
                    AddMergedCells(objgridviewrow, objtablecell, 2, "", "#c41502");
                    AddMergedCells(objgridviewrow, objtablecell, 4, "RTBR/FinPulse  (NC)", "#c41502");

                    AddMergedCells(objgridviewrow, objtablecell, 4, "RTBR/FinPulse  (USD)", "#c41502");

                    


                    objGridView.Controls[0].Controls.AddAt(0, objgridviewrow);


                    int year = DateTime.Today.Year;

                    DateTime todaydate = dateTime;
                    int nxtyr;

                 
                    string qtr = Session["quarter"] + "";


                    var row = e.Row;

                    int currentMonth = DateTime.Now.Month;// dateTime.Month; //DateTime.Now.Month;
                    int currentYear = dateTime.Year; //DateTime.Now.Year;
                    string currentQuarter = qtr;

                    currentYear = currentYear - 2000;
                    string _CurrentQ = string.Empty;
                    //string _NextQ = string.Empty;

                    _CurrentQ = Session["currqtr"] + "";

                    //dm--dmmailid column is removed



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

                    string mon1 = " " + _month1 + "'" + currentYear + " ";
                    string mon2 = " " + _month2 + "'" + currentYear + " ";
                    string mon3 = " " + _month3 + "'" + currentYear + " ";


                    row.Cells[2].Text = " " + _month1 + "'" + currentYear + " ";
                    row.Cells[3].Text = " " + _month2 + "'" + currentYear + " ";
                    row.Cells[4].Text = " " + _month3 + "'" + currentYear + " ";
                    row.Cells[5].Text = "" + _CurrentQ + " BE";

                    row.Cells[6].Text = " " + _month1 + "'" + currentYear + " ";
                    row.Cells[7].Text = " " + _month2 + "'" + currentYear + " ";
                    row.Cells[8].Text = " " + _month3 + "'" + currentYear + " ";
                    row.Cells[9].Text = "" + _CurrentQ + " BE";





                    string constt = "";


                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {





                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {

                   

                }



            }





        }
        protected void grdBEDMViewVol_RowCreated(object sender, GridViewRowEventArgs e)
        {


            //try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridViewRow objgridviewrow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    TableCell objtablecell = new TableCell();

                    GridView objGridView = (GridView)sender;
                    AddMergedCells(objgridviewrow, objtablecell, 12, "DM Digital Volume Projections", "#c41502");
                    objGridView.Controls[0].Controls.AddAt(0, objgridviewrow);

                    int year = DateTime.Today.Year;

                    DateTime todaydate = dateTime;
                    int nxtyr;

                  
                    string qtr = Session["quarter"] + "";


                    var row = e.Row;

                    int currentMonth = DateTime.Now.Month;// dateTime.Month; //DateTime.Now.Month;
                    int currentYear = dateTime.Year; //DateTime.Now.Year;
                    string currentQuarter = qtr;

                    currentYear = currentYear - 2000;
                    string _CurrentQ = string.Empty;
                    //string _NextQ = string.Empty;

                    _CurrentQ = Session["currqtr"] + "";

                    //dm--dmmailid column is removed



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

               

                    string mon1On = " " + _month1 + " On";
                    string mon2On = " " + _month2 + " On";
                    string mon3On = " " + _month3 + " On";

                    string mon1Off = " " + _month1 + " Off";
                    string mon2Off = " " + _month2 + " Off";
                    string mon3Off = " " + _month3 + " Off";


                    row.Cells[3].Text = " " + mon1On + "";
                    row.Cells[5].Text = " " + mon2On + "";
                    row.Cells[7].Text = " " + mon3On + "";
                    row.Cells[4].Text = " " + mon1Off + "";
                    row.Cells[6].Text = " " + mon2Off + "";
                    row.Cells[8].Text = " " + mon3Off + "";
                    row.Cells[9].Text = " " + _CurrentQ + " On";
                    row.Cells[10].Text = " " + _CurrentQ + " Off";
                    row.Cells[11].Text = " " + _CurrentQ + " Total volume";



                    string constt = "";


                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {





                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        e.Row.Cells[i].CssClass = "FooterTotal";
                        // e.Row.Cells[i].ForeColor = System.Drawing.Color.White;
                        e.Row.Cells[i].ForeColor = System.Drawing.Color.White;
                        //e.Row.Cells[i].BackColor = System.Drawing.Color.FromName("#CC0000");
                        //e.Row.Cells[i].Attributes.Add("class", "footerBox");
                        e.Row.Cells[i].BackColor = System.Drawing.Color.FromArgb(51, 51, 51);
                       
                    }


                    e.Row.Cells[3].Text = VolOn1 + "";
                    e.Row.Cells[4].Text = VolOff1 + "";
                    e.Row.Cells[5].Text = VolOn2 + "";
                    e.Row.Cells[6].Text = VolOff2 + "";
                    e.Row.Cells[7].Text = VolOn3 + "";
                    e.Row.Cells[8].Text = VolOff3 + "";
                    e.Row.Cells[9].Text = VolOnTotal + "";
                    e.Row.Cells[10].Text = VolOffTotal + "";
                    e.Row.Cells[11].Text = VolTotal + "";


                }



            }





        }
        protected void gvPBS_RowCreated(object sender, GridViewRowEventArgs e)
        {


            //try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridViewRow objgridviewrow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    TableCell objtablecell = new TableCell();

                    GridView objGridView = (GridView)sender;
                    AddMergedCells(objgridviewrow, objtablecell, 12, "Billed Months/PBS Volume", "#c41502");
                    objGridView.Controls[0].Controls.AddAt(0, objgridviewrow);

                    int year = DateTime.Today.Year;

                    DateTime todaydate = dateTime;
                    int nxtyr;


                    string qtr = Session["quarter"] + "";


                    var row = e.Row;

                    int currentMonth = DateTime.Now.Month;// dateTime.Month; //DateTime.Now.Month;
                    int currentYear = dateTime.Year; //DateTime.Now.Year;
                    string currentQuarter = qtr;

                    currentYear = currentYear - 2000;
                    string _CurrentQ = string.Empty;
                    //string _NextQ = string.Empty;

                    _CurrentQ = Session["currqtr"] + "";

                    //dm--dmmailid column is removed



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



                    string mon1On = " " + _month1 + " On";
                    string mon2On = " " + _month2 + " On";
                    string mon3On = " " + _month3 + " On";

                    string mon1Off = " " + _month1 + " Off";
                    string mon2Off = " " + _month2 + " Off";
                    string mon3Off = " " + _month3 + " Off";


                    row.Cells[3].Text = " " + mon1On + "";
                    row.Cells[5].Text = " " + mon2On + "";
                    row.Cells[7].Text = " " + mon3On + "";
                    row.Cells[4].Text = " " + mon1Off + "";
                    row.Cells[6].Text = " " + mon2Off + "";
                    row.Cells[8].Text = " " + mon3Off + "";
                    row.Cells[9].Text = " " + _CurrentQ + " On";
                    row.Cells[10].Text = " " + _CurrentQ + "Off";
                    row.Cells[11].Text = " " + _CurrentQ + " Total volume";



                    string constt = "";


                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {





                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        e.Row.Cells[i].CssClass = "FooterTotal";
                        // e.Row.Cells[i].ForeColor = System.Drawing.Color.White;
                        e.Row.Cells[i].ForeColor = System.Drawing.Color.White;
                        //e.Row.Cells[i].BackColor = System.Drawing.Color.FromName("#CC0000");
                        //e.Row.Cells[i].Attributes.Add("class", "footerBox");
                        e.Row.Cells[i].BackColor = System.Drawing.Color.FromArgb(51, 51, 51);

                    }


                    e.Row.Cells[3].Text = PBSOn1 + "";
                    e.Row.Cells[4].Text = PBSOff1 + "";
                    e.Row.Cells[5].Text = PBSOn2 + "";
                    e.Row.Cells[6].Text = PBSOff2 + "";
                    e.Row.Cells[7].Text = PBSOn3 + "";
                    e.Row.Cells[8].Text = PBSOff3 + "";
                    e.Row.Cells[9].Text = PBSOnTotal + "";
                    e.Row.Cells[10].Text = PBSOffTotal + "";
                    e.Row.Cells[11].Text = PBSTotal + "";


                }



            }





        }
        protected void gvAlcon_RowCreated(object sender, GridViewRowEventArgs e)
        {


            //try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridViewRow objgridviewrow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    TableCell objtablecell = new TableCell();
                    GridView objGridView = (GridView)sender;

                    AddMergedCells(objgridviewrow, objtablecell, 12, "Billed Months/ALCON Volume", "#c41502");
                   
                    objGridView.Controls[0].Controls.AddAt(0, objgridviewrow);

                    int year = DateTime.Today.Year;

                    DateTime todaydate = dateTime;
                    int nxtyr;


                    string qtr = Session["quarter"] + "";


                    var row = e.Row;

                    int currentMonth = DateTime.Now.Month;// dateTime.Month; //DateTime.Now.Month;
                    int currentYear = dateTime.Year; //DateTime.Now.Year;
                    string currentQuarter = qtr;

                    currentYear = currentYear - 2000;
                    string _CurrentQ = string.Empty;
                    //string _NextQ = string.Empty;

                    _CurrentQ = Session["currqtr"] + "";

                    //dm--dmmailid column is removed



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



                    string mon1On = " " + _month1 + " On";
                    string mon2On = " " + _month2 + " On";
                    string mon3On = " " + _month3 + " On";

                    string mon1Off = " " + _month1 + " Off";
                    string mon2Off = " " + _month2 + " Off";
                    string mon3Off = " " + _month3 + " Off";


                    row.Cells[3].Text = " " + mon1On + "";
                    row.Cells[5].Text = " " + mon2On + "";
                    row.Cells[7].Text = " " + mon3On + "";
                    row.Cells[4].Text = " " + mon1Off + "";
                    row.Cells[6].Text = " " + mon2Off + "";
                    row.Cells[8].Text = " " + mon3Off + "";
                    row.Cells[9].Text = " " + _CurrentQ + " On";
                    row.Cells[10].Text = " " + _CurrentQ + " Off";
                    row.Cells[11].Text = " " + _CurrentQ + " Total volume";



                    string constt = "";


                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {





                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        e.Row.Cells[i].CssClass = "FooterTotal";
                        // e.Row.Cells[i].ForeColor = System.Drawing.Color.White;
                        e.Row.Cells[i].ForeColor = System.Drawing.Color.White;
                        //e.Row.Cells[i].BackColor = System.Drawing.Color.FromName("#CC0000");
                        //e.Row.Cells[i].Attributes.Add("class", "footerBox");
                        e.Row.Cells[i].BackColor = System.Drawing.Color.FromArgb(51, 51, 51);

                    }


                    e.Row.Cells[3].Text = AlconOn1 + "";
                    e.Row.Cells[4].Text = AlconOff1 + "";
                    e.Row.Cells[5].Text = AlconOn2 + "";
                    e.Row.Cells[6].Text = AlconOff2 + "";
                    e.Row.Cells[7].Text = AlconOn3 + "";
                    e.Row.Cells[8].Text = AlconOff3 + "";
                    e.Row.Cells[9].Text = AlconOnTotal + "";
                    e.Row.Cells[10].Text = AlconOffTotal + "";
                    e.Row.Cells[11].Text = AlconTotal + "";


                }



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
                //objtablecell.Style.Add("background-color", backcolor);
                //objtablecell.Style.Add("border-bottom-color", "#878484");// "#c41502");

                objtablecell.HorizontalAlign = HorizontalAlign.Center;
                // objtablecell.BorderColor = System.Drawing.Color.FromName("#c41502");//("#525252");
                objtablecell.BorderColor = System.Drawing.Color.DarkSlateGray;
               // objtablecell.ForeColor = System.Drawing.Color.FromName("#ffcb8b");
                objtablecell.Attributes.Add("class", "GridHeader");
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


    }
