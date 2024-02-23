using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BEData.BusinessEntity;
using BEData;

//namespace BEData
//{
    public partial class editPortfolio : BasePage
    {


        BEDL objbe = new BEDL();
        static List<ClientCodePortfolio> lstccp = new List<ClientCodePortfolio>();
        public string fileName = "BEData.editPortfolio";
        Logger logger = new Logger();
        string userid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            userid = Session["UserID"] + "";
            if (!Page.IsPostBack)
            {


                int MCCID = Convert.ToInt32(Request.QueryString["MCCID"] + "");
                ClientCodePortfolio ccp = new ClientCodePortfolio();

                ccp = objbe.GetAllCCPFields(MCCID);
                string txtPU = ccp.txtPU.Trim();
                string isactive = ccp.isActive.Trim();
                string su = "";
                ddlPU.DataSource = objbe.FetchPuForClientCodePortfolio(userid,su);
                ddlPU.DataBind();
                ddlPU.SelectedIndex = ddlPU.Items.IndexOf(ddlPU.Items.FindByText(txtPU));
                ddlisActive.SelectedIndex = ddlisActive.Items.IndexOf(ddlisActive.Items.FindByText(isactive));
                //ddlPU.SelectedItem.Text = txtPU;
                txtBITSCSI.Text = ccp.txtBITSCSIHMailId;
                txtClientCode.Text = ccp.txtClientCode;
                txtClientName.Text = ccp.txtClientName;
                txtDH.Text = ccp.txtDHMailId;
                txtDivision.Text = ccp.txtDivision;
                txtMasterclientcode.Text = ccp.txtMasterClientCode;
                txtMcname.Text = ccp.txtMasterCustomerName;
                txtPortfolio.Text = ccp.txtPortfolio;
                txtRH.Text = ccp.txtRHMailId;
                txtSDM.Text = ccp.txtSDMMailId;
                txtUH.Text = ccp.txtUHMailId;
                txtVertical.Text = ccp.txtVertical;
                txtFaportfolio.Text = ccp.txtFAPortfolio;
                TXTMCONAME.Text = ccp.txtMCOName;
                lblSU.Text = ccp.txtServiceline;
                lblUnit.Text = ccp.txtunit;
                


            }

        }

        protected void btnEditSave_Click(object sender, EventArgs e)
        {
            try
            {

                int MCCID = Convert.ToInt32(Request.QueryString["MCCID"] + "");
                ClientCodePortfolio ccp = new ClientCodePortfolio();
                ccp.txtBITSCSIHMailId = txtBITSCSI.Text;
                ccp.txtClientCode = txtClientCode.Text;
                ccp.txtClientName = txtClientName.Text;
                ccp.txtDHMailId = txtDH.Text;
                ccp.txtDivision = txtDivision.Text;
                ccp.txtMasterClientCode = txtMasterclientcode.Text;
                ccp.txtMasterCustomerName = txtMcname.Text;
                ccp.txtPortfolio = txtPortfolio.Text;
                ccp.txtPU = ddlPU.SelectedValue;
                ccp.txtRHMailId = txtRH.Text;
                ccp.txtSDMMailId = txtSDM.Text;
                ccp.txtUHMailId = txtUH.Text;
                ccp.txtVertical = txtVertical.Text;
                ccp.isActive = ddlisActive.SelectedValue;
                ccp.txtMCOName = TXTMCONAME.Text;
                ccp.txtServiceline = lblSU.Text;
                ccp.txtunit = lblUnit.Text;
                string updatedby = Session["userid"].ToString();
                ccp.txtFAPortfolio = txtFaportfolio.Text;
                objbe.EditClientCode(MCCID, ccp, updatedby);

                lblMsg.ForeColor = System.Drawing.Color.Green;
                //lblpopupInfo.Text = "Data Saved Successfully";
                lblMsg.Visible = true;
                Response.Write(@" <script type=""text/javascript""> alert('Updated successfully !'); window.opener.document.getElementById('MainContent_btnSearch').click(); window.close();   </script>");

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
//}