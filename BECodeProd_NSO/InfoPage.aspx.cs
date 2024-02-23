using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BECodeProd
{
    public partial class InfoPage : BasePage
    {
        private BEDL service = new BEDL();
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();
            string cmdtext = "SELECT  [Code] as [File],[Year],[Quarter],[Month],[Value] as [Loaded Date] FROM [BEPortalConfig] where [Year]='" + Session["Year"].ToString() + "' and [Quarter]='" + Session["CurrentQuarter"].ToString() + "'";
            DataSet ds = service.GetDataSet(cmdtext);
            DataTable dt = ds.Tables[0];
            gvInfo.DataSource = dt;
            gvInfo.DataBind();
        }
    }
}