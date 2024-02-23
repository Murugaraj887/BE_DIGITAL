using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


    public partial class AnchorLogin : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidateSession();

            string isValidEntry = Session["Login"] + "";
            if (!isValidEntry.Equals("1"))
                Response.Redirect("UnAuthorised.aspx");
            Session["RadioButtonSelected"] = rdbRole.SelectedValue;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
           
            
             if (rdbRole.SelectedValue == "0")
            {
                Response.Redirect("DMView.aspx");
            }
             if (rdbRole.SelectedValue == "1")
             {
                 Response.Redirect("SDMView.aspx");
             }
            else
            {
                Alert("Please Select a Radio Button");
            }
        }

        private void Alert(string message)
        {

            try
            {
                Page page = HttpContext.Current.CurrentHandler as Page;

                // string script = string.Format("alert('{0}');", message);

                if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
                {

                    // page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);

                    page.RegisterClientScriptBlock("alert", "<script type=\"text/javascript\">alert('" + message + "');</script>");

                }
            }
            catch (Exception ex)
            {

                
            }

        }
    }
