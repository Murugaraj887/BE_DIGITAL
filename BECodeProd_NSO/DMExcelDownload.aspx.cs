using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

namespace BECodeProd
{
    public partial class DMExcelDownload : BasePage
    {
        public DateTime dateTime = DateTime.Today;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            if (Request.QueryString["Key"] == "BulkData")
            {

                if (Session["BulkExcel"] != null)
                {

                    string filename = Session["BulkExcel"].ToString();
                    Session["BulkExcel"] = null;
                    string folder = "ExcelOperations";
                    var MyDir = new DirectoryInfo(Server.MapPath(folder));
                    string sPath = Server.MapPath("ExcelOperations\\");
                    if (MyDir.GetFiles().SingleOrDefault(k => k.Name == filename) != null)
                    {
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                        Response.TransmitFile(sPath + filename);
                        Response.End();
                    }
                }

               
            }
            else if (Request.QueryString["Key"] == "GirdDownload")
            {
                if (Session["FileName"] != null)
                {

                    string name = Session["FileName"].ToString();
                    Session["FileName"] = null;
                    Response.ContentType = "application/vnd.ms-excel";

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + name);

                    Response.Charset = String.Empty;
                    StringWriter excelWriter = new StringWriter();
                    HtmlTextWriter myHtmlTextWriter = new HtmlTextWriter(excelWriter);
                    gvDMExcel.RenderControl(myHtmlTextWriter);
                    Response.Write(excelWriter.ToString());
                    Response.End();
                }
            }
            else
            {

                if (Session["Excel"] != null)
                {
                    string name = Session["Excel"].ToString();

                    DataSet ds = new DataSet();
                    ds = (DataSet)Session["ExcelData"];
                    Session["Excel"] = null;
                    Session["ExcelData"] = null;

                    gvDMExcel.Visible = true;
                    gvDMExcel.DataSource = ds.Tables[0];
                    gvDMExcel.DataBind();

                    Response.ContentType = "application/vnd.ms-excel";

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + name);

                    Response.Charset = String.Empty;
                    StringWriter excelWriter = new StringWriter();
                    HtmlTextWriter myHtmlTextWriter = new HtmlTextWriter(excelWriter);
                    gvDMExcel.RenderControl(myHtmlTextWriter);
                    Response.Write(excelWriter.ToString());
                    Response.End();
                }
            }

            
            
        }

       

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
    }
}