using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

 
    public partial class BasePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ValidateSession()
        {
           // return;

            if (Session.Count == 0)
            {
                if (this.Page.AppRelativeVirtualPath == "~/Summary/BEReportSummary.aspx")
                    Response.Redirect("~/SessionTimeOut2.aspx");
                else
                    Response.Redirect("SessionTimeOut2.aspx");
            }

            
        }


        private void insertTestData()
        {
            string connectionString = @"Data Source=nebula\mssqlserver1;Initial Catalog=EAS_Prod;Persist Security Info=True;User ID=nebula_sql;Password=python@123";
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.CommandText = "insert into [tblDeleteTest] (name) values ( '"+DateTime.Now.ToString()+"')";
            cmd.CommandType = System.Data.CommandType.Text;
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }
    }
 