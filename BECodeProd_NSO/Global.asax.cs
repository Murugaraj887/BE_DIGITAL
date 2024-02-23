using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace BECodeProd
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }
        private void insertTestData(string value)
        {
            string connectionString = @"Data Source=nebula\mssqlserver1;Initial Catalog=EAS_Prod;Persist Security Info=True;User ID=nebula_sql;Password=python@123";
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.CommandText = "insert into [tblDeleteTest] (name,Value) values ( '" + DateTime.Now.ToString() + "', '" + value + "')";
            cmd.CommandType = System.Data.CommandType.Text;
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            insertTestData("Session_Start-" + Session.SessionID);

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            insertTestData("Session_End-" + Session.SessionID);

           //Server.Transfer("SessionTimeout2.aspx");
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}