using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BEData.BusinessEntity;
using System.Data;
using System.IO;
using BEData;



    public partial class ClientCodePortfolioScreen0 : BasePage
    {


        BEDL objbe = new BEDL();
        public string fileName = "BEData.ClientCodePortfolioScreen0";
        Logger logger = new Logger();
        string userid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            lblMsg.Visible = false;
            //ModalPopupExtender2.TargetControlID 
            userid = Session["UserID"] + "";
            if (!Page.IsPostBack)
            {
                string su="";
                if (Session["Role"].ToString() == "Admin")
                {
                    btnAddNew.Visible = true;
                    btnDelete.Visible = true;
                }
                else
                {
                    btnAddNew.Visible = false;
                    btnDelete.Visible = false;
                }
                ddlPu.DataSource = objbe.FetchPuForClientCodePortfolio(userid,su);
                ddlPu.DataBind();
                ddlPu.Items.Insert(0, "--Select--");
             //   ddlPu.SelectedIndex = ddlPu.Items.IndexOf(ddlPu.Items.FindByText("ECSADM"));
                string PU = ddlPu.SelectedValue;

                List<string> sdmlist = new List<string>();
                sdmlist = objbe.FetchSDMForCCP(PU);
                //''.
                var sdm = sdmlist.Distinct().Select(k => new { txtSDMMailid = k }).ToList();
                ddlSDM.DataSource = sdm;
                ddlSDM.DataBind();
                ddlSDM.Items.Insert(0, "--Select--");
                //ddlSDM.Items.Insert(1, "All");



                //ddlSDM.DataSource = objbe.FetchSDMForCCP(PU);
                //ddlSDM.DataBind();
                //ddlSDM.Items.Insert(0, "--Select--");
                //ddlSDM.SelectedIndex = ddlSDM.Items.IndexOf(ddlSDM.Items.FindByText(sdm));
                string SDM = ddlSDM.SelectedValue;

                ddlMcc.DataSource = objbe.FetchMasterClientCodeForCCP(SDM, PU);
                ddlMcc.DataBind();
                ddlMcc.Items.Insert(0, "--Select--");
               // ddlMcc.Items.Insert(1, "All");

                string MCC = ddlMcc.SelectedValue;
                ddlSDM.Enabled = false;
                ddlMcc.Enabled = false;
                //grdClientCode.DataSource = objbe.FetchClientcodeportfolio(MCC, PU, SDM);
                //grdClientCode.DataBind();
                grdClientCode.AllowPaging = false;

                //btnCopy.Attributes["OnClick"] = "return PopUpCopy(this);";

            }

            //lnkbtnCopy.Attributes.Add("OnClick", "PopUpCopy(this);");
            
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {


            string pu = ddlPu.SelectedValue;
            string Mcc = ddlMcc.SelectedValue;
            string sdm = ddlSDM.SelectedValue;

            //ddlPu.DataSource = objbe.FetchPuForClientCodePortfolio();
            //ddlPu.DataBind();
            //ddlPu.SelectedIndex = ddlPu.Items.IndexOf(ddlPu.Items.FindByText(pu));


            //ddlSDM.DataSource = objbe.FetchSDMForCCP(pu);
            //ddlSDM.DataBind();
            //ddlSDM.Items.Insert(0, "--Select--");
            //ddlSDM.SelectedIndex = ddlSDM.Items.IndexOf(ddlSDM.Items.FindByText(sdm));
            //string SDM = ddlSDM.SelectedValue;

            //ddlMcc.DataSource = objbe.FetchMasterClientCodeForCCP(sdm, pu);
            //ddlMcc.DataBind();
            //ddlMcc.Items.Insert(0, "All");

            //ddlMcc.SelectedIndex = ddlMcc.Items.IndexOf(ddlMcc.Items.FindByText(Mcc));
            //string MCC = ddlMcc.SelectedValue;


            if (Mcc.ToLowerTrim() == "all")
            {
                ddlMcc.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { Mcc += k + ","; });
                Mcc = Mcc.Replace("ALLALL,", string.Empty);
                Mcc = Mcc.Trim().TrimEnd(',').TrimStart(',');
            }

            if (sdm.ToLowerTrim() == "all")
            {
                ddlSDM.Items.OfType<ListItem>().Select(k => k.Text).ToList().ForEach(k => { sdm += k + ","; });
                sdm = sdm.Replace("ALLALL,", string.Empty);
                sdm = sdm.Trim().TrimEnd(',').TrimStart(',');
            }

            grdClientCode.DataSource = objbe.FetchClientcodeportfolio(Mcc, pu, sdm);
            grdClientCode.DataBind();
            grdClientCode.Visible = true;

        }





        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                List<ClientCodePortfolio> lstUPdateItems = new List<ClientCodePortfolio>();

                DataTable dt = new DataTable();

                foreach (GridViewRow row in grdClientCode.Rows)
                {
                    if ((row.Cells[0].FindControl("chkRow") as CheckBox).Checked)
                    {
                        HiddenField mid = (HiddenField)row.FindControl("hdnfldID");
                        string mcid = mid.Value;
                        int mccid = Convert.ToInt32(mcid);
                 //string cc = row.Cells[4].Text.Trim();
                        //string pu = row.Cells[14].Text.Trim();

                        lstUPdateItems.Add(new ClientCodePortfolio()
                        {
                           intmccid=mccid

                        });
                    }
                }
                if (lstUPdateItems.Count > 0)
                {

                    objbe.DeleteClientCodeportfolio(lstUPdateItems);

                }
                if (lstUPdateItems.Count > 0)
                {
                    lblMsg.Text = "Master Customer Code Deleted Successfully!!";
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    lblMsg.Visible = true;
                    string su = "";
                    ddlPu.DataSource = objbe.FetchPuForClientCodePortfolio(userid,su);
                    ddlPu.DataBind();
                    ddlPu.SelectedIndex = ddlPu.Items.IndexOf(ddlPu.Items.FindByText("ECSADM"));
                    string PU = ddlPu.SelectedValue;


                    List<string> sdmlist = new List<string>();
                    sdmlist = objbe.FetchSDMForCCP(PU);
                    //''.
                    var sdm = sdmlist.Distinct().Select(k => new { txtSDMMailid = k }).ToList();
                    ddlSDM.DataSource = sdm;
                    ddlSDM.DataBind();
                    ddlSDM.Items.Insert(0, "--Select--");
                    string SDM = ddlSDM.SelectedValue;

                 
                    
                    ddlMcc.DataSource = objbe.FetchMasterClientCodeForCCP(SDM, PU);
                    ddlMcc.DataBind();
                    ddlMcc.Items.Insert(0, "--Select--");
                    ddlMcc.Items.Insert(1, "All");
                    grdClientCode.Visible = false;
                }
                else
                {
                    lblMsg.Text = "Select a row to delete!!";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Visible = true;
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
        protected void ddlPu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPu.SelectedValue == "--Select--")
            {
                ddlSDM.Enabled = false;
                ddlMcc.Enabled = false;
            }
            else
            {
            ddlSDM.Enabled = true;
            }
            string PU = ddlPu.SelectedValue;
            List<string> sdmlist = new List<string>();
            sdmlist = objbe.FetchSDMForCCP(PU);
            //''.
            var sdm = sdmlist.Distinct().Select(k => new { txtSDMMailid = k }).ToList();
            ddlSDM.DataSource = sdm;
            ddlSDM.DataBind();
          //  ddlSDM.Items.Insert(0, "--Select--");
            ddlSDM.Items.Insert(0, "All");
            string SDM = ddlSDM.SelectedValue;
            ddlMcc.DataSource = objbe.FetchMasterClientCodeForCCP(SDM, PU);
            ddlMcc.DataBind();
            ddlMcc.Items.Insert(0, "All");
            grdClientCode.Visible = false;

        }
        //protected void grdClientCode_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        //{

        //    grdClientCode.PageIndex = e.NewPageIndex;
        //    grdClientCode.DataBind();
        //}
        protected void grdClientCode_PageIndexChanged(object sender, EventArgs e)
        {

        }
        protected void grdClientCode_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            //string PU = ddlPu.SelectedValue;
            //string MCC = ddlMcc.SelectedValue;
            //string sdm = ddlSDM.SelectedValue;
            //grdClientCode.DataSource = objbe.FetchClientcodeportfolio(MCC, PU, sdm);
            //grdClientCode.PageIndex = e.NewPageIndex;
            //grdClientCode.DataBind();
        }
        protected void ddlSDM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSDM.SelectedValue == "--Select--")
            {
                ddlMcc.Enabled = false;
            }
            else
            {
                ddlMcc.Enabled = true;
            }
            string SDM = ddlSDM.SelectedValue;
            string PU = ddlPu.SelectedValue;
            ddlMcc.DataSource = objbe.FetchMasterClientCodeForCCP(SDM, PU);
            ddlMcc.DataBind();

            ddlMcc.DataSource = objbe.FetchMasterClientCodeForCCP(SDM, PU);
            ddlMcc.DataBind();

            ddlMcc.Items.Insert(0, "All");
            grdClientCode.Visible = false;
        }
        protected void ddlMcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdClientCode.Visible = false;
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            grdClientCode.Visible = false;
        }
        protected void lnkSearch_Click(object sender, ImageClickEventArgs e)
        {
            string txtMcc= txtSearch.Text;

            if (txtSearch.Text == "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "<script language='JavaScript'>alert('Enter Master Client Code to Search');</script>", false);

                return;
            }
            grdClientCode.DataSource = objbe.SearchMcc(txtMcc,userid);
            grdClientCode.DataBind();
            grdClientCode.AllowPaging = false;
            grdClientCode.Visible = true;
        }
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            //foreach (GridViewRow row in grdClientCode.Rows)
            //{
            //    if ((row.Cells[0].FindControl("chkRow") as CheckBox).Checked)
            //    {
            //        HiddenField mid = (HiddenField)row.FindControl("hdnfldID");
            //        string mcid = mid.Value;
            //        int mccid = Convert.ToInt32(mcid);
            //    }
            //}

           // btnCopy.Attributes["OnClick"] = "PopUpCopy(this);";            
        }

        protected void grdClientCode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

           
    }



