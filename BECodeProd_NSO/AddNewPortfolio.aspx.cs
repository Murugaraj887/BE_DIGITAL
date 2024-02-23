using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BEData.BusinessEntity;
using BEData;
using System.Data;


    public partial class AddNewPortfolio : System.Web.UI.Page
    {
        BEDL objbe = new BEDL();
        public string fileName = "BEData.AddNewPortfolio";
        Logger logger = new Logger();
        string userid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            userid = Session["UserID"] + "";
            
            if (!Page.IsPostBack)
            {
                ddlPopupPu.Items.Clear();
                ddlPopupPu.Items.Insert(0, "--Select--");
                ddlVertical.Items.Clear();
                ddlVertical.Items.Insert(0, "--Select--");
                ddlDH.Items.Clear();
                ddlDH.Items.Insert(0, "--Select--");
                ddlSoh.Items.Clear();
                ddlSoh.Items.Insert(0, "--Select--");
                ddlUh.Items.Clear();
                ddlUh.Items.Insert(0, "--Select--");
              
                btnCancel.OnClientClick = " Reset(); ";
            }

            else
            {

            }
        }

        protected void btnSavepopup_click(object sender, EventArgs e)
        {
            ClientCodePortfolio ccp = new ClientCodePortfolio();
            ccp.txtMasterClientCode = txtpopupMCC.Text;
            ccp.txtMasterCustomerName = txtpopupMCName.Text;
            ccp.txtClientCode = txtpopupClientCode.Text;

            ccp.txtClientName = txtpopupClientName.Text;
            ccp.txtPortfolio = txtpopupPortfolio.Text;
            ccp.txtDivision = txtpopupDivision.Text;

            ccp.txtVertical = ddlVertical.SelectedValue;
            
            ccp.txtRHMailId = txtpopupRHMailid.Text;
            ccp.txtSDMMailId = txtpopupSDM.Text;

            ccp.txtDHMailId = ddlDH.SelectedValue;

            ccp.txtBITSCSIHMailId = ddlSoh.SelectedValue;
            ccp.txtUHMailId = ddlUh.SelectedValue;
            

            ccp.txtPU = ddlPopupPu.SelectedValue;
           

            ccp.txtMCOName = TXTMCONAME.Text;
            ccp.txtServiceline = ddlSU.SelectedValue;
            ccp.txtunit = lblUnit.Text;
          

            string updatedby = Session["userid"].ToString();
            ccp.txtFAPortfolio = txtFaPortfolio.Text;
            ccp.isActive = ddlisActive.SelectedValue;
            string PU = ddlPopupPu.SelectedValue;
            var ret = objbe.AddNewClientCode(ccp, updatedby);
            if (ret == -1)
            {
                lblpopupInfo.ForeColor = System.Drawing.Color.Red;
                lblpopupInfo.Text = "Master Client Code and PU Combination Already Exists";
                lblpopupInfo.Visible = true;
            }
            else if (ret == 0)
            {
                lblpopupInfo.ForeColor = System.Drawing.Color.Green;
                //lblpopupInfo.Text = "Data Saved Successfully";
                lblpopupInfo.Visible = true;
                Response.Write(@" <script type=""text/javascript""> alert('saved successfully !'); window.opener.document.getElementById('MainContent_btnSearch').click(); window.close();   </script>");


            }


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void ddlSU_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlSU.SelectedValue=="--Select--")
            {
                ddlPopupPu.Items.Clear();
                ddlPopupPu.Items.Insert(0, "--Select--");
                ddlVertical.Items.Clear();
                ddlVertical.Items.Insert(0, "--Select--");
                ddlDH.Items.Clear();
                ddlDH.Items.Insert(0, "--Select--");
                ddlSoh.Items.Clear();
                ddlSoh.Items.Insert(0, "--Select--");
                ddlUh.Items.Clear();
                ddlUh.Items.Insert(0, "--Select--");
                ddlVertical.Enabled = false;
                ddlPopupPu.Enabled = false;
                ddlDH.Enabled = false;
                ddlSoh.Enabled = false;
                ddlUh.Enabled = false;
            }
            else
            {
                ddlPopupPu.Items.Clear();
                ddlPopupPu.Items.Insert(0, "--Select--");
                ddlPopupPu.DataSource = objbe.FetchPuForClientCodePortfolio(userid,ddlSU.SelectedValue);
                ddlPopupPu.DataBind();
                ddlPopupPu.SelectedIndex = ddlPopupPu.Items.IndexOf(ddlPopupPu.Items.FindByText("ECSADM"));

                ddlVertical.Items.Clear();
                ddlVertical.Items.Insert(0, "--Select--");
                string cmdVertical = "select distinct dbo.udfTrim(txtVertical) as txtVertical from demclientcodePortfolio where txtVertical is not null and txtVertical<>'' and txtServiceline='" + ddlSU.SelectedValue + "'";
                DataSet dsVertical = objbe.GetDataSet(cmdVertical);
                DataTable dtVertical = dsVertical.Tables[0];
                ddlVertical.DataSource = dtVertical;
                ddlVertical.DataBind();

                ddlDH.Items.Clear();
                ddlDH.Items.Insert(0, "--Select--");
                string cmdDHMailid = "select distinct dbo.udfTrim(txtDHMailId) as txtDHMailId from demclientcodePortfolio where txtDHMailId is not null and txtDHMailId<>'' and txtServiceline='" + ddlSU.SelectedValue + "'";
                DataSet dsDHMailid = objbe.GetDataSet(cmdDHMailid);
                DataTable dtDHMailid = dsDHMailid.Tables[0];
                ddlDH.DataSource = dtDHMailid;
                ddlDH.DataBind();

                ddlSoh.Items.Clear();
                ddlSoh.Items.Insert(0, "--Select--");
                string cmdSOHMailid = "select distinct dbo.udfTrim(txtBITSCSIHMailId) as txtBITSCSIHMailId from demclientcodePortfolio where txtBITSCSIHMailId is not null and txtBITSCSIHMailId<>'' and txtServiceline='" + ddlSU.SelectedValue + "'";
                DataSet dsSOHMailid = objbe.GetDataSet(cmdSOHMailid);
                DataTable dtSOHMailid = dsSOHMailid.Tables[0];
                ddlSoh.DataSource = dtSOHMailid;
                ddlSoh.DataBind();

                ddlUh.Items.Clear();
                ddlUh.Items.Insert(0, "--Select--");
                string cmdUHMailid = "select distinct dbo.udfTrim(txtUHMailId) as txtUHMailId from demclientcodePortfolio where txtUHMailId is not null and txtUHMailId<>'' and txtServiceline='" + ddlSU.SelectedValue + "'";
                DataSet dsUHMailid = objbe.GetDataSet(cmdUHMailid);
                DataTable dtUHMailid = dsUHMailid.Tables[0];
                ddlUh.DataSource = dtUHMailid;
                ddlUh.DataBind();

            ddlVertical.Enabled=true;
                ddlPopupPu.Enabled=true;
                ddlDH.Enabled=true;
                ddlSoh.Enabled=true;
                ddlUh.Enabled=true;
            }
        }






    }
