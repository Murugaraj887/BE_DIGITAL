using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;

namespace BECodeProd
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        static string connStr = ConfigurationManager.AppSettings["DemandCaptureConnectionString"].ToString();



        [WebMethod]
        public void SaveInstruction(string instruction, string instructionId)
        {
            instruction = instruction.Replace(@"\\", @"\");
            SqlCommand cmd = new SqlCommand();
            SqlConnection con = new SqlConnection(connStr);
            cmd = new SqlCommand("Instruction_Add", con);
            cmd.Parameters.AddWithValue("@instruction", instruction);
            cmd.Parameters.AddWithValue("@instructionId", instructionId);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            con.Close();
        }

        [WebMethod]
        public string GetInstruction(string ApplicationName)
        {


            SqlCommand cmd = new SqlCommand();
            SqlConnection con = new SqlConnection(connStr);
            cmd = new SqlCommand("select InstructionText,sortorder from  Data_Load_Help_Details where Application ='"+ApplicationName+"'", con);
            //cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
            cmd.CommandType = CommandType.Text;
            con.Open();
            //SqlDataReader dr = cmd.ExecuteReader();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            con.Close();
            DataTable dt = new DataTable();
            sda.Fill(dt);
            string json = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return json;
        }

        [WebMethod]
        public string EnableEdit()
        {
            string userid = HttpContext.Current.User.Identity.Name;
            string[] userids = userid.Split('\\');
            if (userids.Length == 2)
            {
                userid = userids[1];
            }

            SqlCommand cmd = new SqlCommand();
            SqlConnection con = new SqlConnection(connStr);
            cmd = new SqlCommand("Instruction_GetRole", con);
            cmd.Parameters.AddWithValue("@UserId", userid);
            cmd.Parameters.Add("@Result", SqlDbType.VarChar, 30);
            cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            string Result = cmd.Parameters["@Result"].Value.ToString();
            return Result;
        }

        public string DataTableToJSONWithStringBuilder(System.Data.DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }  
    }
}
