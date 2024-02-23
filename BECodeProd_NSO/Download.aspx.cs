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
    public partial class Download : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

                if (Session["key"] != null)
                {
                    string fileName = Session["key"].ToString();

                    if (fileName.Contains("Americas_Sales_Dashboard"))
                    {
                        string folder = "BE_Sales";
                        var myDir = new DirectoryInfo(Server.MapPath(folder));

                        if (myDir.GetFiles().SingleOrDefault(k => k.Name == fileName) != null)
                        {
                            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                            Response.TransmitFile(myDir + @"\" + fileName);
                            Response.End();
                        }
                    }
                    else
                    {
                        string folder = @"ExcelOperations\DownloadFiles";
                        var MyDir = new DirectoryInfo(Server.MapPath(folder));


                        string sPath = Server.MapPath("ExcelOperations\\DownloadFiles\\");
                        Session["key"] = null;

                        if (MyDir.GetFiles().SingleOrDefault(k => k.Name == fileName) != null)
                        {
                            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                            Response.TransmitFile(sPath + fileName);
                            Response.End();
                        }
                    }
                }
        }
    }
}