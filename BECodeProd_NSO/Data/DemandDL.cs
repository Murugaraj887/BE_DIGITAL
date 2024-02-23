using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Xml;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using BEData.BusinessEntity;
using System.Collections.Generic;
using BEData;
using System.Linq;


public class BEDL
{
    static string G_connStr = ConfigurationManager.AppSettings["DemandCaptureConnectionString"].ToString();
    //VISA: added to avoid adding the constants class
    public const string DateFormat = "MM/dd/yyyy";
    public const string DateFormat1 = "dd MMM yyyy";
    public string fileName = "BEDL.BEData";

    DataAccess objData = new DataAccess();

    Logger logger = new Logger();

    public DataTable GetDataLoad_Instruction(int Instructionid)
    {
        SqlCommand cmd = new SqlCommand("select Instructions from [Data_Load_Help_Details] where SortOrder=" + Instructionid);
        cmd.CommandTimeout = int.MaxValue;
        SqlConnection G_DBConnection = new SqlConnection(G_connStr);
        cmd.Connection = G_DBConnection;
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        try
        {
            G_DBConnection.Open();
            da.Fill(ds);
            G_DBConnection.Close();
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            /*
            lblErrMsg.Text = ex.Message;
            lblErrMsg.Visible = true;
             */
        }
        finally
        {
            if (G_DBConnection.State != ConnectionState.Closed)
                G_DBConnection.Close();
        }
        return null;
    }

    public void uploadDM(string userid, string CurrQtr, DataTable dt)
    {
        SqlConnection con = new SqlConnection(G_connStr);

        SqlCommand cmd = new SqlCommand("[sp_BulkUpdate_EAS_BEData_DM]");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = con;
        cmd.Parameters.AddWithValue("@userid", userid);
        cmd.Parameters.AddWithValue("@CurrQtr", CurrQtr);
        cmd.Parameters.AddWithValue("@tblCustomers", dt);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
    }

    public int FreezingPreviousMonthBE()
    {
        SqlConnection con = new SqlConnection(G_connStr);
        con.Open();
        SqlCommand cmd = new SqlCommand("FreezingPreviousMonthBE_Date_NSO");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = con;

        cmd.Parameters.Add("@Day", SqlDbType.SmallInt);
        cmd.Parameters["@Day"].Size = 50;
        cmd.Parameters["@Day"].Direction = ParameterDirection.Output;
        cmd.ExecuteNonQuery();

        int Day = Convert.ToInt32(cmd.Parameters["@Day"].Value);
        con.Close();
        return Day;
    }

    public DataSet GetServiceOffering(string UserId, string Qtr)
    {
        SqlConnection con = new SqlConnection(G_connStr);

        SqlCommand cmd = new SqlCommand("sp_GetServiceOffering");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = con;
        cmd.Parameters.AddWithValue("@UserId", UserId);
        cmd.Parameters.AddWithValue("@Qtr", Qtr);
        SqlDataAdapter sdr = new SqlDataAdapter(cmd);
        DataSet dt = new DataSet();
        sdr.Fill(dt);
        return dt;
    }

    public DataSet GetServiceOffering_Panaya(string UserId, string Qtr)
    {
        SqlConnection con = new SqlConnection(G_connStr);
        SqlCommand cmd = new SqlCommand("sp_GetServiceOffering_panaya");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = con;
        cmd.Parameters.AddWithValue("@UserId", UserId);
        cmd.Parameters.AddWithValue("@Qtr", Qtr);
        SqlDataAdapter sdr = new SqlDataAdapter(cmd);
        DataSet dt = new DataSet();
        sdr.Fill(dt);
        return dt;
    }

    public bool NewServiceOffering(string userid)
    {
        SqlConnection con = new SqlConnection(G_connStr);
        con.Open();
        SqlCommand cmd = new SqlCommand("sp_checkServiceOfferingList");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = con;
        cmd.Parameters.AddWithValue("@UserId", userid);
        cmd.Parameters.Add("@Valid", SqlDbType.VarChar);
        cmd.Parameters["@Valid"].Size = 50;
        cmd.Parameters["@Valid"].Direction = ParameterDirection.Output;
        cmd.ExecuteNonQuery();

        bool Valid = Convert.ToBoolean(cmd.Parameters["@Valid"].Value);
        con.Close();
        return Valid;
    }

    public void UpdateDataLoadTracker(String FileName, String PackageName, string UserId, String date)
    {


        SqlConnection G_DBConnection = new SqlConnection(G_connStr);
        //cmd.Connection = G_DBConnection;
        SqlCommand cmd = new SqlCommand("SPROC_UpdateDataLoadTracker", G_DBConnection);
        cmd.CommandTimeout = int.MaxValue;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@FileName", FileName);
        cmd.Parameters.AddWithValue("@PackageName", PackageName);
        cmd.Parameters.AddWithValue("@UserId", UserId);
        cmd.Parameters.AddWithValue("@timestamp", date);
        try
        {
            G_DBConnection.Open();
            cmd.ExecuteNonQuery();
            G_DBConnection.Close();

        }
        catch (Exception ex)
        {
        }
        finally
        {
            if (G_DBConnection.State != ConnectionState.Closed)
                G_DBConnection.Close();
        }
    }

    public void uploadSDM(string userid, string CurrQtr, DataTable dt)
    {
        SqlConnection con = new SqlConnection(G_connStr);

        SqlCommand cmd = new SqlCommand("[sp_BulkUpdate_EAS_BEData_SDM]");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = con;
        cmd.Parameters.AddWithValue("@userid", userid);
        cmd.Parameters.AddWithValue("@CurrQtr", CurrQtr);
        cmd.Parameters.AddWithValue("@tblCustomers", dt);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
    }

    public DataSet GetDMBEDataExcel_bulk_DM(string PU, string customerCode, string userid, string quarter, string year, string role, string MachineUserId)
    {


        DataSet ds = new DataSet();
        DataSet dsreturn = new DataSet();
        SqlCommand objCommand;
        List<DataTable> retTable = new List<DataTable>();

        try
        {


            objCommand = new SqlCommand();
            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@MasterClientCode";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = customerCode;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@UserId";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = userid;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@Quarter";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = quarter;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@FYYR";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = year;

            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.ParameterName = "@PU";
            objParamStatus.Direction = ParameterDirection.Input;
            objParamStatus.SqlDbType = SqlDbType.VarChar;
            objParamStatus.Value = PU;

            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@Role";
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.VarChar;
            objParamStatus5.Value = role;

            SqlParameter objParamStatus6 = new SqlParameter();
            objParamStatus6.ParameterName = "@MachineUserId";
            objParamStatus6.Direction = ParameterDirection.Input;
            objParamStatus6.SqlDbType = SqlDbType.VarChar;
            objParamStatus6.Value = MachineUserId;


            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            objParamColl.Add(objParamStatus);

            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);

            objParamColl.Add(objParamStatus4);
            objParamColl.Add(objParamStatus5);
            objParamColl.Add(objParamStatus6);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_Fetch_BEData_DM_ImportExcel_download", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                //dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
                //dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
                //dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
                //dt.Columns["txtQuarterName"].ColumnName = "Current Quarter Name";
                //dt.Columns["txtYear"].ColumnName = "Year";

                ////BE
                //dt.Columns["DMfltMonth1BE"].ColumnName = "DMMonth1";
                //dt.Columns["DMfltMonth2BE"].ColumnName = "DMMonth2";
                //dt.Columns["DMfltMonth3BE"].ColumnName = "DMMonth3";
                //dt.Columns["DMfltCurrentQuarterBE"].ColumnName = "DMQCur";
                //dt.Columns["SDMBK1"].ColumnName = "SDMBK1";
                //dt.Columns["SDMBK2"].ColumnName = "SDMBK2";
                //dt.Columns["SDMBK3"].ColumnName = "SDMBK3";
                //dt.Columns["SDMBK4"].ColumnName = "SDMBK4";
                //dt.Columns["txtSDMBERemarks"].ColumnName = "SDMBERemarks";

                ////Vol

                //dt.Columns["txtOnsiteValueM1"].ColumnName = "OnsiteValueM1";
                //dt.Columns["txtOffshoreValueM1"].ColumnName = "OffshoreValueM1";
                //dt.Columns["txtOnsiteValueM2"].ColumnName = "OnsiteValueM2";
                //dt.Columns["txtOffshoreValueM2"].ColumnName = "OffshoreValueM2";
                //dt.Columns["txtOnsiteValueM3"].ColumnName = "OnsiteValueM3";
                //dt.Columns["txtOffshoreValueM3"].ColumnName = "OffshoreValueM3";

                //dt.Columns["txtTotalOnsiteValue"].ColumnName = "TotalOnsiteValue";
                //dt.Columns["txtTotalOffshoreValue"].ColumnName = "TotalOffshoreValue";
                //dt.Columns["txtGrandTotalValue"].ColumnName = "GrandTotalValue";
                //dt.Columns["txtSDMVolumeRemarks"].ColumnName = "SDMVolumeRemarks";

                //dt.Columns["dtDMUpdatedDate"].ColumnName = "dtDMUpdatedDate";



                dsreturn.Tables.Add(dt.Copy());
                //dsreturn.Tables.Add(ds.Tables[1].Copy());

                return dsreturn;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataSet();
    }

    public DataSet GetDMBEDataExcel_bulk_DM_NSO(string customerCode, string userid, string quarter, string year, string role, string MachineUserId)
    {


        DataSet ds = new DataSet();
        DataSet dsreturn = new DataSet();
        SqlCommand objCommand;
        List<DataTable> retTable = new List<DataTable>();

        try
        {


            objCommand = new SqlCommand();
            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@MasterClientCode";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = customerCode;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@UserId";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = userid;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@Quarter";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = quarter;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@FYYR";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = year;

            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@Role";
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.VarChar;
            objParamStatus5.Value = role;

            SqlParameter objParamStatus6 = new SqlParameter();
            objParamStatus6.ParameterName = "@MachineUserId";
            objParamStatus6.Direction = ParameterDirection.Input;
            objParamStatus6.SqlDbType = SqlDbType.VarChar;
            objParamStatus6.Value = MachineUserId;


            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);
            objParamColl.Add(objParamStatus4);
            objParamColl.Add(objParamStatus5);
            objParamColl.Add(objParamStatus6);
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_Fetch_BEData_DM_ImportExcel_download_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                dsreturn.Tables.Add(dt.Copy());
                //dsreturn.Tables.Add(ds.Tables[1].Copy());

                return dsreturn;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataSet();
    }

    public DataSet GetDMBEDataExcel_bulk_SDM_NSO(string customerCode, string userid, string quarter, string year, string role, string Machineuserid)
    {


        DataSet ds = new DataSet();
        DataSet dsreturn = new DataSet();
        SqlCommand objCommand;
        List<DataTable> retTable = new List<DataTable>();

        try
        {


            objCommand = new SqlCommand();
            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@MasterClientCode";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = customerCode;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@UserId";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = userid;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@Quarter";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = quarter;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@FYYR";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = year;



            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@Role";
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.VarChar;
            objParamStatus5.Value = role;

            SqlParameter objParamStatus6 = new SqlParameter();
            objParamStatus6.ParameterName = "@MachineUserId";
            objParamStatus6.Direction = ParameterDirection.Input;
            objParamStatus6.SqlDbType = SqlDbType.VarChar;
            objParamStatus6.Value = Machineuserid;


            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;




            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);

            objParamColl.Add(objParamStatus4);
            objParamColl.Add(objParamStatus5);
            objParamColl.Add(objParamStatus6);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_Fetch_BEData_SDM_ImportExcel_download_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                //dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
                //dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
                //dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
                //dt.Columns["txtQuarterName"].ColumnName = "Current Quarter Name";
                //dt.Columns["txtYear"].ColumnName = "Year";

                ////BE
                //dt.Columns["DMfltMonth1BE"].ColumnName = "DMMonth1";
                //dt.Columns["DMfltMonth2BE"].ColumnName = "DMMonth2";
                //dt.Columns["DMfltMonth3BE"].ColumnName = "DMMonth3";
                //dt.Columns["DMfltCurrentQuarterBE"].ColumnName = "DMQCur";
                //dt.Columns["SDMBK1"].ColumnName = "SDMBK1";
                //dt.Columns["SDMBK2"].ColumnName = "SDMBK2";
                //dt.Columns["SDMBK3"].ColumnName = "SDMBK3";
                //dt.Columns["SDMBK4"].ColumnName = "SDMBK4";
                //dt.Columns["txtSDMBERemarks"].ColumnName = "SDMBERemarks";

                ////Vol

                //dt.Columns["txtOnsiteValueM1"].ColumnName = "OnsiteValueM1";
                //dt.Columns["txtOffshoreValueM1"].ColumnName = "OffshoreValueM1";
                //dt.Columns["txtOnsiteValueM2"].ColumnName = "OnsiteValueM2";
                //dt.Columns["txtOffshoreValueM2"].ColumnName = "OffshoreValueM2";
                //dt.Columns["txtOnsiteValueM3"].ColumnName = "OnsiteValueM3";
                //dt.Columns["txtOffshoreValueM3"].ColumnName = "OffshoreValueM3";

                //dt.Columns["txtTotalOnsiteValue"].ColumnName = "TotalOnsiteValue";
                //dt.Columns["txtTotalOffshoreValue"].ColumnName = "TotalOffshoreValue";
                //dt.Columns["txtGrandTotalValue"].ColumnName = "GrandTotalValue";
                //dt.Columns["txtSDMVolumeRemarks"].ColumnName = "SDMVolumeRemarks";

                //dt.Columns["dtDMUpdatedDate"].ColumnName = "dtDMUpdatedDate";



                dsreturn.Tables.Add(dt.Copy());
                //dsreturn.Tables.Add(ds.Tables[1].Copy());

                return dsreturn;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataSet();
    }


    public DataSet GetDMBEDataExcel_bulk_SDM(string PU, string customerCode, string userid, string quarter, string year, string role, string Machineuserid)
    {


        DataSet ds = new DataSet();
        DataSet dsreturn = new DataSet();
        SqlCommand objCommand;
        List<DataTable> retTable = new List<DataTable>();

        try
        {


            objCommand = new SqlCommand();
            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@MasterClientCode";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = customerCode;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@UserId";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = userid;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@Quarter";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = quarter;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@FYYR";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = year;

            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.ParameterName = "@PU";
            objParamStatus.Direction = ParameterDirection.Input;
            objParamStatus.SqlDbType = SqlDbType.VarChar;
            objParamStatus.Value = PU;

            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@Role";
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.VarChar;
            objParamStatus5.Value = role;

            SqlParameter objParamStatus6 = new SqlParameter();
            objParamStatus6.ParameterName = "@MachineUserId";
            objParamStatus6.Direction = ParameterDirection.Input;
            objParamStatus6.SqlDbType = SqlDbType.VarChar;
            objParamStatus6.Value = Machineuserid;


            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            objParamColl.Add(objParamStatus);

            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);

            objParamColl.Add(objParamStatus4);
            objParamColl.Add(objParamStatus5);
            objParamColl.Add(objParamStatus6);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_Fetch_BEData_SDM_ImportExcel_download_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                //dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
                //dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
                //dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
                //dt.Columns["txtQuarterName"].ColumnName = "Current Quarter Name";
                //dt.Columns["txtYear"].ColumnName = "Year";

                ////BE
                //dt.Columns["DMfltMonth1BE"].ColumnName = "DMMonth1";
                //dt.Columns["DMfltMonth2BE"].ColumnName = "DMMonth2";
                //dt.Columns["DMfltMonth3BE"].ColumnName = "DMMonth3";
                //dt.Columns["DMfltCurrentQuarterBE"].ColumnName = "DMQCur";
                //dt.Columns["SDMBK1"].ColumnName = "SDMBK1";
                //dt.Columns["SDMBK2"].ColumnName = "SDMBK2";
                //dt.Columns["SDMBK3"].ColumnName = "SDMBK3";
                //dt.Columns["SDMBK4"].ColumnName = "SDMBK4";
                //dt.Columns["txtSDMBERemarks"].ColumnName = "SDMBERemarks";

                ////Vol

                //dt.Columns["txtOnsiteValueM1"].ColumnName = "OnsiteValueM1";
                //dt.Columns["txtOffshoreValueM1"].ColumnName = "OffshoreValueM1";
                //dt.Columns["txtOnsiteValueM2"].ColumnName = "OnsiteValueM2";
                //dt.Columns["txtOffshoreValueM2"].ColumnName = "OffshoreValueM2";
                //dt.Columns["txtOnsiteValueM3"].ColumnName = "OnsiteValueM3";
                //dt.Columns["txtOffshoreValueM3"].ColumnName = "OffshoreValueM3";

                //dt.Columns["txtTotalOnsiteValue"].ColumnName = "TotalOnsiteValue";
                //dt.Columns["txtTotalOffshoreValue"].ColumnName = "TotalOffshoreValue";
                //dt.Columns["txtGrandTotalValue"].ColumnName = "GrandTotalValue";
                //dt.Columns["txtSDMVolumeRemarks"].ColumnName = "SDMVolumeRemarks";

                //dt.Columns["dtDMUpdatedDate"].ColumnName = "dtDMUpdatedDate";



                dsreturn.Tables.Add(dt.Copy());
                //dsreturn.Tables.Add(ds.Tables[1].Copy());

                return dsreturn;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataSet();
    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //EXPENSE
    public string GetRolee(string userID)
    {
        DataSet ds = new DataSet();

        SqlCommand objCommand;

        string role = "";

        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@userID";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = userID;

            objCommand = new SqlCommand();

            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spExpReturnRole", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    role = ds.Tables[0].Rows[i]["Role"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return role;
    }

    public DataSet GetBEReport_New(string SU, string userid)
    {
        //var date = date1.Date;
        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;
            SqlParameter sqlparam1, sqlparam2;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@SU";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = SU;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@Userid";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = userid;
            objParamColl.Add(sqlparam2);



            objData.ExecuteSP("dbo.EAS_BE_Data_Rprt", ref  dsCurrConv, objCommand);

            return dsCurrConv;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }


    public string GetTemplateID(string ExpenseType)
    {
        DataSet ds = new DataSet();

        SqlCommand objCommand;

        string returnValue = string.Empty;

        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@ExpType";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = ExpenseType;

            objCommand = new SqlCommand();

            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spExpExpTemplateId", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                    returnValue = ds.Tables[0].Rows[0]["ExpTemplateID"] + "";


        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return returnValue;
    }


    public List<ExpenseTemplateData> GetExpenseTemplateData()
    {
        // su = "EAS";
        List<ExpenseTemplateData> lstreturnData = new List<ExpenseTemplateData>();

        DataSet ds = new DataSet();




        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEExpGetTemplateDataFilter", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                lstreturnData = GenericUtility.DataTableToList<ExpenseTemplateData>(dt);
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        return lstreturnData;

    }

    public List<ExpenseColumns> GetExpenseColumnsEntity()
    {
        List<ExpenseColumns> lstreturnData = new List<ExpenseColumns>();

        DataSet ds = new DataSet();




        try
        {


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spExpGetAllColumns", ref ds);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                lstreturnData = GenericUtility.DataTableToList<ExpenseColumns>(dt);
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        return lstreturnData;

    }

    public ExpTypePLExtCatMap GetExpTypePLExtCatMap(string expType)
    {
        List<ExpTypePLExtCatMap> lstreturnData = new List<ExpTypePLExtCatMap>();

        DataSet ds = new DataSet();

        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        SqlParameter sqlparam1;

        objCommand = new SqlCommand();
        objParamColl = objCommand.Parameters;



        sqlparam1 = new SqlParameter();
        sqlparam1.ParameterName = "@exptype";
        sqlparam1.Direction = ParameterDirection.Input;
        sqlparam1.Value = expType;
        objParamColl.Add(sqlparam1);



        try
        {


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spExpGetTypePl", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                lstreturnData = GenericUtility.DataTableToList<ExpTypePLExtCatMap>(dt);
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        return lstreturnData.FirstOrDefault();

    }

    public List<MasterEntity> GetExpenseMasterData(/*string pu, string du,*/string userid, string mcc, string ExpenseType, string quarter/*, string cat, string pl, string status*/)
    {
        List<MasterEntity> lstreturnData = new List<MasterEntity>();

        DataSet ds = new DataSet();

        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        SqlParameter /*sqlparam1, sqlparam2,*/ sqlparam3, sqlparam4,/* sqlparam5, sqlparam6, sqlparam7,*/ sqlparam8, sqlparam9;

        objCommand = new SqlCommand();
        objParamColl = objCommand.Parameters;

        //sqlparam1 = new SqlParameter();
        //sqlparam1.ParameterName = "@pucode";
        //sqlparam1.Direction = ParameterDirection.Input;
        //sqlparam1.SqlDbType = SqlDbType.NVarChar;
        //sqlparam1.Value = pu;
        //objParamColl.Add(sqlparam1);

        //sqlparam2 = new SqlParameter();
        //sqlparam2.ParameterName = "@dmMailId";
        //sqlparam2.Direction = ParameterDirection.Input;
        //sqlparam2.SqlDbType = SqlDbType.NVarChar;
        //sqlparam2.Value = du;
        //objParamColl.Add(sqlparam2);


        sqlparam3 = new SqlParameter();
        sqlparam3.ParameterName = "@exptype";
        sqlparam3.Direction = ParameterDirection.Input;
        sqlparam3.SqlDbType = SqlDbType.NVarChar;
        sqlparam3.Value = ExpenseType;
        objParamColl.Add(sqlparam3);

        sqlparam4 = new SqlParameter();
        sqlparam4.ParameterName = "@Qtryear";
        sqlparam4.Direction = ParameterDirection.Input;
        sqlparam4.SqlDbType = SqlDbType.NVarChar;
        sqlparam4.Value = quarter;
        objParamColl.Add(sqlparam4);

        //sqlparam5 = new SqlParameter();
        //sqlparam5.ParameterName = "@expcat";
        //sqlparam5.Direction = ParameterDirection.Input;
        //sqlparam5.SqlDbType = SqlDbType.NVarChar;
        //sqlparam5.Value = cat;
        //objParamColl.Add(sqlparam5);

        //sqlparam6 = new SqlParameter();
        //sqlparam6.ParameterName = "@pl";
        //sqlparam6.Direction = ParameterDirection.Input;
        //sqlparam6.SqlDbType = SqlDbType.NVarChar;
        //sqlparam6.Value = pl;
        //objParamColl.Add(sqlparam6);

        //sqlparam7 = new SqlParameter();
        //sqlparam7.ParameterName = "@status";
        //sqlparam7.Direction = ParameterDirection.Input;
        //sqlparam7.SqlDbType = SqlDbType.NVarChar;
        //sqlparam7.Value = status;
        //objParamColl.Add(sqlparam7);

        sqlparam8 = new SqlParameter();
        sqlparam8.ParameterName = "@MCC";
        sqlparam8.Direction = ParameterDirection.Input;
        sqlparam8.SqlDbType = SqlDbType.NVarChar;
        sqlparam8.Value = mcc;
        objParamColl.Add(sqlparam8);

        sqlparam9 = new SqlParameter();
        sqlparam9.ParameterName = "@userid";
        sqlparam9.Direction = ParameterDirection.Input;
        sqlparam9.SqlDbType = SqlDbType.NVarChar;
        sqlparam9.Value = userid;
        objParamColl.Add(sqlparam9);
        try
        {


            objData = new DataAccess();
            objData.GetConnection();

            objData.ExecuteSP("spBEExpDashboardDetails", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                lstreturnData = GenericUtility.DataTableToList<MasterEntity>(dt);
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        lstreturnData = lstreturnData == null ? new List<MasterEntity>() : lstreturnData;
        lstreturnData = lstreturnData.Distinct(new MasterEntityEqualityComparer()).ToList();
        return lstreturnData;

    }


    public DataTable GetExpenseMasterData1(/*string pu, string du,*/string userid, string mcc, string ExpenseType, string quarter/*, string cat, string pl, string status*/)
    {


        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        SqlParameter /*sqlparam1, sqlparam2,*/ sqlparam3, sqlparam4,/* sqlparam5, sqlparam6, sqlparam7,*/ sqlparam8, sqlparam9;

        objCommand = new SqlCommand();
        objParamColl = objCommand.Parameters;

        //sqlparam1 = new SqlParameter();
        //sqlparam1.ParameterName = "@pucode";
        //sqlparam1.Direction = ParameterDirection.Input;
        //sqlparam1.SqlDbType = SqlDbType.NVarChar;
        //sqlparam1.Value = pu;
        //objParamColl.Add(sqlparam1);

        //sqlparam2 = new SqlParameter();
        //sqlparam2.ParameterName = "@dmMailId";
        //sqlparam2.Direction = ParameterDirection.Input;
        //sqlparam2.SqlDbType = SqlDbType.NVarChar;
        //sqlparam2.Value = du;
        //objParamColl.Add(sqlparam2);


        sqlparam3 = new SqlParameter();
        sqlparam3.ParameterName = "@exptype";
        sqlparam3.Direction = ParameterDirection.Input;
        sqlparam3.SqlDbType = SqlDbType.NVarChar;
        sqlparam3.Value = ExpenseType;
        objParamColl.Add(sqlparam3);

        sqlparam4 = new SqlParameter();
        sqlparam4.ParameterName = "@Qtryear";
        sqlparam4.Direction = ParameterDirection.Input;
        sqlparam4.SqlDbType = SqlDbType.NVarChar;
        sqlparam4.Value = quarter;
        objParamColl.Add(sqlparam4);

        //sqlparam5 = new SqlParameter();
        //sqlparam5.ParameterName = "@expcat";
        //sqlparam5.Direction = ParameterDirection.Input;
        //sqlparam5.SqlDbType = SqlDbType.NVarChar;
        //sqlparam5.Value = cat;
        //objParamColl.Add(sqlparam5);

        //sqlparam6 = new SqlParameter();
        //sqlparam6.ParameterName = "@pl";
        //sqlparam6.Direction = ParameterDirection.Input;
        //sqlparam6.SqlDbType = SqlDbType.NVarChar;
        //sqlparam6.Value = pl;
        //objParamColl.Add(sqlparam6);

        //sqlparam7 = new SqlParameter();
        //sqlparam7.ParameterName = "@status";
        //sqlparam7.Direction = ParameterDirection.Input;
        //sqlparam7.SqlDbType = SqlDbType.NVarChar;
        //sqlparam7.Value = status;
        //objParamColl.Add(sqlparam7);

        sqlparam8 = new SqlParameter();
        sqlparam8.ParameterName = "@MCC";
        sqlparam8.Direction = ParameterDirection.Input;
        sqlparam8.SqlDbType = SqlDbType.NVarChar;
        sqlparam8.Value = mcc;
        objParamColl.Add(sqlparam8);

        sqlparam9 = new SqlParameter();
        sqlparam9.ParameterName = "@userid";
        sqlparam9.Direction = ParameterDirection.Input;
        sqlparam9.SqlDbType = SqlDbType.NVarChar;
        sqlparam9.Value = userid;
        objParamColl.Add(sqlparam9);
        try
        {


            objData = new DataAccess();
            objData.GetConnection();

            objData.ExecuteSP("spBEExpDashboardDetails", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {

                dt = ds.Tables[0];

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        return dt;

    }


    internal Amounts GetAllAmounts(string userID, string sdmlist, string dh, string quarter, string expHead)
    {

        Amounts returnData = new Amounts();

        DataSet ds = new DataSet();

        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5;

        objCommand = new SqlCommand();
        objParamColl = objCommand.Parameters;


        sqlparam1 = new SqlParameter();
        sqlparam1.ParameterName = "@txtUserId";
        sqlparam1.Direction = ParameterDirection.Input;
        sqlparam1.SqlDbType = SqlDbType.NVarChar;
        sqlparam1.Value = userID;
        objParamColl.Add(sqlparam1);

        sqlparam2 = new SqlParameter();
        sqlparam2.ParameterName = "@exphead";
        sqlparam2.Direction = ParameterDirection.Input;
        sqlparam2.SqlDbType = SqlDbType.NVarChar;
        sqlparam2.Value = expHead;
        objParamColl.Add(sqlparam2);

        sqlparam3 = new SqlParameter();
        sqlparam3.ParameterName = "@sdmlist";
        sqlparam3.Direction = ParameterDirection.Input;
        sqlparam3.SqlDbType = SqlDbType.NVarChar;
        sqlparam3.Value = sdmlist;
        objParamColl.Add(sqlparam3);

        sqlparam4 = new SqlParameter();
        sqlparam4.ParameterName = "@dh";
        sqlparam4.Direction = ParameterDirection.Input;
        sqlparam4.SqlDbType = SqlDbType.NVarChar;
        sqlparam4.Value = dh;
        objParamColl.Add(sqlparam4);

        sqlparam5 = new SqlParameter();
        sqlparam5.ParameterName = "@Qtr";
        sqlparam5.Direction = ParameterDirection.Input;
        sqlparam5.SqlDbType = SqlDbType.NVarChar;
        sqlparam5.Value = quarter;
        objParamColl.Add(sqlparam5);




        try
        {


            objData = new DataAccess();
            objData.GetConnection();

            objData.ExecuteSP("spExpGetAllAmt", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    var datasource = GenericUtility.DataTableToList<Amounts>(dt);
                    if (datasource != null && datasource.Count == 1)
                        returnData = datasource[0];

                }

        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        return returnData;

    }

    public class MasterEntityEqualityComparer : IEqualityComparer<MasterEntity>
    {

        public bool Equals(MasterEntity x, MasterEntity y)
        {
            return x.intExpId == y.intExpId;
        }

        public int GetHashCode(MasterEntity obj)
        {
            return 0;
        }
    }

    public void DeleteMasterData(int intExpID)
    {






        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@intExpID";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.Int;

            sqlparam1.Value = intExpID;
            objParamColl.Add(sqlparam1);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEExpDeleteMasterData", objCommand);

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

    }

    internal List<PUDM> GetPUDMMapping(string userID)
    {
        List<PUDM> lstreturnData = new List<PUDM>();

        DataSet ds = new DataSet();

        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        SqlParameter sqlparam1;

        objCommand = new SqlCommand();
        objParamColl = objCommand.Parameters;

        sqlparam1 = new SqlParameter();
        sqlparam1.ParameterName = "@userid";
        sqlparam1.Direction = ParameterDirection.Input;
        sqlparam1.SqlDbType = SqlDbType.NVarChar;

        sqlparam1.Value = userID;
        objParamColl.Add(sqlparam1);




        try
        {


            objData = new DataAccess();
            objData.GetConnection();

            objData.ExecuteSP("spBEExpGetPuDMList", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                lstreturnData = GenericUtility.DataTableToList<PUDM>(dt);
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        lstreturnData = lstreturnData == null ? new List<PUDM>() : lstreturnData;
        return lstreturnData;

    }

    public List<string> GetSpDDLItems(string spName)
    {
        DataSet ds = new DataSet();

        SqlCommand objCommand;

        List<string> lstempCollection = new List<string>();

        try
        {


            objCommand = new SqlCommand();



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP(spName, ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string du = ds.Tables[0].Rows[i][0].ToString(); // 0th column must be the output
                    lstempCollection.Add(du);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection;
    }

    public List<string> GetSpDDLItems(string spName, string value)
    {
        DataSet ds = new DataSet();

        List<string> lstempCollection = new List<string>();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();

            SqlCommand objCommand;
            SqlParameterCollection objParamColl;
            SqlParameter sqlparam1;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@userid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.NVarChar;
            sqlparam1.Value = value;
            objParamColl.Add(sqlparam1);
            objData.ExecuteSP(spName, ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string du = ds.Tables[0].Rows[i][0].ToString(); // 0th column must be the output
                    lstempCollection.Add(du);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection;
    }

    public void CopyRowMasterData(int intExpID)
    {






        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@intExpID";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.Int;

            sqlparam1.Value = intExpID;
            objParamColl.Add(sqlparam1);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEExpCopyRowMasterData", objCommand);

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

    }

    public void UpdateSDMPhase1(int expid, string priority, string sdmstatus, double sdmapprovedamount)
    {

        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@intExpId";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.Value = expid;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@Priority";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.Value = priority;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@SDMStatus";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.Value = sdmstatus;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@SDMApprovedAmt";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.Value = sdmapprovedamount;
            objParamColl.Add(sqlparam4);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spUpdateSDMPhaseI", objCommand);
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }



    }

    public void UpdateDHPhase1(int expid, string priority, string dhstatus, double dhapprovedamount)
    {

        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@intExpId";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.Value = expid;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@Priority";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.Value = priority;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@DHStatus";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.Value = dhstatus;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@DHApprovedAmt";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.Value = dhapprovedamount;
            objParamColl.Add(sqlparam4);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spUpdateDHPhaseI", objCommand);
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }



    }

    public void UpdatePNAPhase2(int expid, string pnastatus, double pnaapprovedamount)
    {

        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@intExpId";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.Value = expid;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@PNAStatus";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.Value = pnastatus;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@PNAApprovedAmount";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.Value = pnaapprovedamount;
            objParamColl.Add(sqlparam3);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spUpdatePNAStatusPhaseII", objCommand);
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }



    }

    internal List<VisaCountry> GetVisaCountryMapping()
    {
        List<VisaCountry> lstreturnData = new List<VisaCountry>();

        DataSet ds = new DataSet();
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        objCommand = new SqlCommand();
        objParamColl = objCommand.Parameters;

        try
        {

            objData = new DataAccess();
            objData.GetConnection();

            objData.ExecuteSP("spExpCountryVisaWpType", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                lstreturnData = GenericUtility.DataTableToList<VisaCountry>(dt);
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        lstreturnData = lstreturnData == null ? new List<VisaCountry>() : lstreturnData;
        return lstreturnData;

    }

    public MasterEntity GetEditExpenseData(int expid)
    {
        MasterEntity lstreturnData = new MasterEntity();

        DataSet ds = new DataSet();

        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        SqlParameter sqlparam1;

        objCommand = new SqlCommand();
        objParamColl = objCommand.Parameters;

        sqlparam1 = new SqlParameter();
        sqlparam1.ParameterName = "@intexpid";
        sqlparam1.Direction = ParameterDirection.Input;
        sqlparam1.SqlDbType = SqlDbType.Int;

        sqlparam1.Value = expid;
        objParamColl.Add(sqlparam1);





        try
        {


            objData = new DataAccess();
            objData.GetConnection();

            objData.ExecuteSP("spBEExpGetMasterData", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                var temp = GenericUtility.DataTableToList<MasterEntity>(dt);
                if (temp != null)
                    if (temp.Count > 0)
                        lstreturnData = temp[0];
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        return lstreturnData;

    }

    public void InsertMasterData(MasterEntity data)
    {

        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        objCommand = new SqlCommand();
        objParamColl = objCommand.Parameters;

        try
        {


            SqlParameter sqlparamPU = new SqlParameter();
            sqlparamPU.ParameterName = "@PU";
            sqlparamPU.Direction = ParameterDirection.Input;
            sqlparamPU.SqlDbType = SqlDbType.NVarChar;
            sqlparamPU.Value = data.PUCode;
            objParamColl.Add(sqlparamPU);

            SqlParameter sqlparamBU = new SqlParameter();
            sqlparamBU.ParameterName = "@BU";
            sqlparamBU.Direction = ParameterDirection.Input;
            sqlparamBU.SqlDbType = SqlDbType.NVarChar;
            sqlparamBU.Value = data.BUCode;
            objParamColl.Add(sqlparamBU);

            //SqlParameter sqlparamDUCode = new SqlParameter();
            //sqlparamDUCode.ParameterName = "@DUCode";
            //sqlparamDUCode.Direction = ParameterDirection.Input;
            //sqlparamDUCode.SqlDbType = SqlDbType.NVarChar;
            //sqlparamDUCode.Value = data.DUCode;
            //objParamColl.Add(sqlparamDUCode);

            SqlParameter sqlparamDUCode = new SqlParameter();
            sqlparamDUCode.ParameterName = "@DmMailId";
            sqlparamDUCode.Direction = ParameterDirection.Input;
            sqlparamDUCode.SqlDbType = SqlDbType.NVarChar;
            sqlparamDUCode.Value = data.DMMailId;
            objParamColl.Add(sqlparamDUCode);


            SqlParameter sqlparamClientCode = new SqlParameter();
            sqlparamClientCode.ParameterName = "@ClientCode";
            sqlparamClientCode.Direction = ParameterDirection.Input;
            sqlparamClientCode.SqlDbType = SqlDbType.NVarChar;
            sqlparamClientCode.Value = data.ClientCode;
            objParamColl.Add(sqlparamClientCode);

            SqlParameter sqlparamExpType = new SqlParameter();
            sqlparamExpType.ParameterName = "@ExpType";
            sqlparamExpType.Direction = ParameterDirection.Input;
            sqlparamExpType.SqlDbType = SqlDbType.NVarChar;
            sqlparamExpType.Value = data.ExpType;
            objParamColl.Add(sqlparamExpType);

            SqlParameter sqlparamExpCategory = new SqlParameter();
            sqlparamExpCategory.ParameterName = "@ExpCategory";
            sqlparamExpCategory.Direction = ParameterDirection.Input;
            sqlparamExpCategory.SqlDbType = SqlDbType.NVarChar;
            sqlparamExpCategory.Value = data.ExpCategory;
            objParamColl.Add(sqlparamExpCategory);

            SqlParameter sqlparamItemName = new SqlParameter();
            sqlparamItemName.ParameterName = "@ItemName";
            sqlparamItemName.Direction = ParameterDirection.Input;
            sqlparamItemName.SqlDbType = SqlDbType.NVarChar;
            sqlparamItemName.Value = data.ItemName;
            objParamColl.Add(sqlparamItemName);

            SqlParameter sqlparamPriority = new SqlParameter();
            sqlparamPriority.ParameterName = "@Priority";
            sqlparamPriority.Direction = ParameterDirection.Input;
            sqlparamPriority.SqlDbType = SqlDbType.NVarChar;
            sqlparamPriority.Value = data.Priority;
            objParamColl.Add(sqlparamPriority);

            SqlParameter sqlparamNumberofItems = new SqlParameter();
            sqlparamNumberofItems.ParameterName = "@NumberofItems";
            sqlparamNumberofItems.Direction = ParameterDirection.Input;
            sqlparamNumberofItems.SqlDbType = SqlDbType.Float;
            sqlparamNumberofItems.Value = data.NumberofItems;
            objParamColl.Add(sqlparamNumberofItems);

            SqlParameter sqlparamUnitCost = new SqlParameter();
            sqlparamUnitCost.ParameterName = "@UnitCost";
            sqlparamUnitCost.Direction = ParameterDirection.Input;
            sqlparamUnitCost.SqlDbType = SqlDbType.Float;
            sqlparamUnitCost.Value = data.UnitCost;
            objParamColl.Add(sqlparamUnitCost);

            SqlParameter sqlparamExpenseDate = new SqlParameter();
            sqlparamExpenseDate.ParameterName = "@ExpenseDate";
            sqlparamExpenseDate.Direction = ParameterDirection.Input;
            sqlparamExpenseDate.SqlDbType = SqlDbType.NVarChar;
            sqlparamExpenseDate.Value = data.ExpenseDate;
            objParamColl.Add(sqlparamExpenseDate);

            SqlParameter sqlparamJustificationRemarks = new SqlParameter();
            sqlparamJustificationRemarks.ParameterName = "@JustificationRemarks";
            sqlparamJustificationRemarks.Direction = ParameterDirection.Input;
            sqlparamJustificationRemarks.SqlDbType = SqlDbType.NVarChar;
            sqlparamJustificationRemarks.Value = data.JustificationRemarks;
            objParamColl.Add(sqlparamJustificationRemarks);

            SqlParameter sqlparamIsCustomerRecoverable = new SqlParameter();
            sqlparamIsCustomerRecoverable.ParameterName = "@IsCustomerRecoverable";
            sqlparamIsCustomerRecoverable.Direction = ParameterDirection.Input;
            sqlparamIsCustomerRecoverable.SqlDbType = SqlDbType.NVarChar;
            sqlparamIsCustomerRecoverable.Value = data.IsCustomerRecoverable;
            objParamColl.Add(sqlparamIsCustomerRecoverable);

            SqlParameter sqlparamProjOppCode = new SqlParameter();
            sqlparamProjOppCode.ParameterName = "@ProjOppCode";
            sqlparamProjOppCode.Direction = ParameterDirection.Input;
            sqlparamProjOppCode.SqlDbType = SqlDbType.NVarChar;
            sqlparamProjOppCode.Value = data.ProjOppCode;
            objParamColl.Add(sqlparamProjOppCode);

            SqlParameter sqlparamIsBudgetedinPBS = new SqlParameter();
            sqlparamIsBudgetedinPBS.ParameterName = "@IsBudgetedinPBS";
            sqlparamIsBudgetedinPBS.Direction = ParameterDirection.Input;
            sqlparamIsBudgetedinPBS.SqlDbType = SqlDbType.NVarChar;
            sqlparamIsBudgetedinPBS.Value = data.IsBudgetedinPBS;
            objParamColl.Add(sqlparamIsBudgetedinPBS);

            SqlParameter sqlparamBEUpside = new SqlParameter();
            sqlparamBEUpside.ParameterName = "@BEUpside";
            sqlparamBEUpside.Direction = ParameterDirection.Input;
            sqlparamBEUpside.SqlDbType = SqlDbType.NVarChar;
            sqlparamBEUpside.Value = data.BEUpside;
            objParamColl.Add(sqlparamBEUpside);

            SqlParameter sqlparamBEDownside = new SqlParameter();
            sqlparamBEDownside.ParameterName = "@BEDownside";
            sqlparamBEDownside.Direction = ParameterDirection.Input;
            sqlparamBEDownside.SqlDbType = SqlDbType.NVarChar;
            sqlparamBEDownside.Value = data.BEDownside;
            objParamColl.Add(sqlparamBEDownside);

            SqlParameter sqlparamCurrQtr = new SqlParameter();
            sqlparamCurrQtr.ParameterName = "@CurrQtr";
            sqlparamCurrQtr.Direction = ParameterDirection.Input;
            sqlparamCurrQtr.SqlDbType = SqlDbType.NVarChar;
            sqlparamCurrQtr.Value = data.CurrQtr;
            objParamColl.Add(sqlparamCurrQtr);

            SqlParameter sqlparamFutQtrBE = new SqlParameter();
            sqlparamFutQtrBE.ParameterName = "@FutQtrBE";
            sqlparamFutQtrBE.Direction = ParameterDirection.Input;
            sqlparamFutQtrBE.SqlDbType = SqlDbType.NVarChar;
            sqlparamFutQtrBE.Value = data.FutQtrBE;
            objParamColl.Add(sqlparamFutQtrBE);

            SqlParameter sqlparamStatus = new SqlParameter();
            sqlparamStatus.ParameterName = "@Status";
            sqlparamStatus.Direction = ParameterDirection.Input;
            sqlparamStatus.SqlDbType = SqlDbType.NVarChar;
            sqlparamStatus.Value = data.Status;
            objParamColl.Add(sqlparamStatus);

            SqlParameter sqlparamFieldDate1 = new SqlParameter();
            sqlparamFieldDate1.ParameterName = "@FieldDate1";
            sqlparamFieldDate1.Direction = ParameterDirection.Input;
            sqlparamFieldDate1.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldDate1.Value = data.FieldDate1;
            objParamColl.Add(sqlparamFieldDate1);

            SqlParameter sqlparamFieldDate2 = new SqlParameter();
            sqlparamFieldDate2.ParameterName = "@FieldDate2";
            sqlparamFieldDate2.Direction = ParameterDirection.Input;
            sqlparamFieldDate2.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldDate2.Value = data.FieldDate2;
            objParamColl.Add(sqlparamFieldDate2);

            SqlParameter sqlparamFieldDate3 = new SqlParameter();
            sqlparamFieldDate3.ParameterName = "@FieldDate3";
            sqlparamFieldDate3.Direction = ParameterDirection.Input;
            sqlparamFieldDate3.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldDate3.Value = data.FieldDate3;
            objParamColl.Add(sqlparamFieldDate3);

            SqlParameter sqlparamFieldDate4 = new SqlParameter();
            sqlparamFieldDate4.ParameterName = "@FieldDate4";
            sqlparamFieldDate4.Direction = ParameterDirection.Input;
            sqlparamFieldDate4.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldDate4.Value = data.FieldDate4;
            objParamColl.Add(sqlparamFieldDate4);

            SqlParameter sqlparamFieldDate5 = new SqlParameter();
            sqlparamFieldDate5.ParameterName = "@FieldDate5";
            sqlparamFieldDate5.Direction = ParameterDirection.Input;
            sqlparamFieldDate5.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldDate5.Value = data.FieldDate5;
            objParamColl.Add(sqlparamFieldDate5);

            SqlParameter sqlparamFieldDate6 = new SqlParameter();
            sqlparamFieldDate6.ParameterName = "@FieldDate6";
            sqlparamFieldDate6.Direction = ParameterDirection.Input;
            sqlparamFieldDate6.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldDate6.Value = data.FieldDate6;
            objParamColl.Add(sqlparamFieldDate6);

            SqlParameter sqlparamFieldDate7 = new SqlParameter();
            sqlparamFieldDate7.ParameterName = "@FieldDate7";
            sqlparamFieldDate7.Direction = ParameterDirection.Input;
            sqlparamFieldDate7.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldDate7.Value = data.FieldDate7;
            objParamColl.Add(sqlparamFieldDate7);

            SqlParameter sqlparamFieldDate8 = new SqlParameter();
            sqlparamFieldDate8.ParameterName = "@FieldDate8";
            sqlparamFieldDate8.Direction = ParameterDirection.Input;
            sqlparamFieldDate8.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldDate8.Value = data.FieldDate8;
            objParamColl.Add(sqlparamFieldDate8);

            SqlParameter sqlparamCreatedBy = new SqlParameter();
            sqlparamCreatedBy.ParameterName = "@CreatedBy";
            sqlparamCreatedBy.Direction = ParameterDirection.Input;
            sqlparamCreatedBy.SqlDbType = SqlDbType.NVarChar;
            sqlparamCreatedBy.Value = data.CreatedBy;
            objParamColl.Add(sqlparamCreatedBy);

            SqlParameter sqlparamCreatedOn = new SqlParameter();
            sqlparamCreatedOn.ParameterName = "@CreatedOn";
            sqlparamCreatedOn.Direction = ParameterDirection.Input;
            sqlparamCreatedOn.SqlDbType = SqlDbType.NVarChar;
            sqlparamCreatedOn.Value = data.CreatedOn;
            objParamColl.Add(sqlparamCreatedOn);

            SqlParameter sqlparamModifiedBy = new SqlParameter();
            sqlparamModifiedBy.ParameterName = "@ModifiedBy";
            sqlparamModifiedBy.Direction = ParameterDirection.Input;
            sqlparamModifiedBy.SqlDbType = SqlDbType.NVarChar;
            sqlparamModifiedBy.Value = data.ModifiedBy;
            objParamColl.Add(sqlparamModifiedBy);

            SqlParameter sqlparamModifiedOn = new SqlParameter();
            sqlparamModifiedOn.ParameterName = "@ModifiedOn";
            sqlparamModifiedOn.Direction = ParameterDirection.Input;
            sqlparamModifiedOn.SqlDbType = SqlDbType.NVarChar;
            sqlparamModifiedOn.Value = data.ModifiedOn;
            objParamColl.Add(sqlparamModifiedOn);

            SqlParameter sqlparamFieldtxt1 = new SqlParameter();
            sqlparamFieldtxt1.ParameterName = "@Fieldtxt1";
            sqlparamFieldtxt1.Direction = ParameterDirection.Input;
            sqlparamFieldtxt1.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt1.Value = data.Fieldtxt1;
            objParamColl.Add(sqlparamFieldtxt1);

            SqlParameter sqlparamFieldtxt2 = new SqlParameter();
            sqlparamFieldtxt2.ParameterName = "@Fieldtxt2";
            sqlparamFieldtxt2.Direction = ParameterDirection.Input;
            sqlparamFieldtxt2.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt2.Value = data.Fieldtxt2;
            objParamColl.Add(sqlparamFieldtxt2);

            SqlParameter sqlparamFieldtxt3 = new SqlParameter();
            sqlparamFieldtxt3.ParameterName = "@Fieldtxt3";
            sqlparamFieldtxt3.Direction = ParameterDirection.Input;
            sqlparamFieldtxt3.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt3.Value = data.Fieldtxt3;
            objParamColl.Add(sqlparamFieldtxt3);

            SqlParameter sqlparamFieldtxt4 = new SqlParameter();
            sqlparamFieldtxt4.ParameterName = "@Fieldtxt4";
            sqlparamFieldtxt4.Direction = ParameterDirection.Input;
            sqlparamFieldtxt4.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt4.Value = data.Fieldtxt4;
            objParamColl.Add(sqlparamFieldtxt4);

            SqlParameter sqlparamFieldtxt5 = new SqlParameter();
            sqlparamFieldtxt5.ParameterName = "@Fieldtxt5";
            sqlparamFieldtxt5.Direction = ParameterDirection.Input;
            sqlparamFieldtxt5.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt5.Value = data.Fieldtxt5;
            objParamColl.Add(sqlparamFieldtxt5);

            SqlParameter sqlparamFieldtxt6 = new SqlParameter();
            sqlparamFieldtxt6.ParameterName = "@Fieldtxt6";
            sqlparamFieldtxt6.Direction = ParameterDirection.Input;
            sqlparamFieldtxt6.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt6.Value = data.Fieldtxt6;
            objParamColl.Add(sqlparamFieldtxt6);

            SqlParameter sqlparamFieldtxt7 = new SqlParameter();
            sqlparamFieldtxt7.ParameterName = "@Fieldtxt7";
            sqlparamFieldtxt7.Direction = ParameterDirection.Input;
            sqlparamFieldtxt7.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt7.Value = data.Fieldtxt7;
            objParamColl.Add(sqlparamFieldtxt7);

            SqlParameter sqlparamFieldtxt8 = new SqlParameter();
            sqlparamFieldtxt8.ParameterName = "@Fieldtxt8";
            sqlparamFieldtxt8.Direction = ParameterDirection.Input;
            sqlparamFieldtxt8.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt8.Value = data.Fieldtxt8;
            objParamColl.Add(sqlparamFieldtxt8);

            SqlParameter sqlparamFieldtxt9 = new SqlParameter();
            sqlparamFieldtxt9.ParameterName = "@Fieldtxt9";
            sqlparamFieldtxt9.Direction = ParameterDirection.Input;
            sqlparamFieldtxt9.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt9.Value = data.Fieldtxt9;
            objParamColl.Add(sqlparamFieldtxt9);

            SqlParameter sqlparamFieldtxt10 = new SqlParameter();
            sqlparamFieldtxt10.ParameterName = "@Fieldtxt10";
            sqlparamFieldtxt10.Direction = ParameterDirection.Input;
            sqlparamFieldtxt10.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt10.Value = data.Fieldtxt10;
            objParamColl.Add(sqlparamFieldtxt10);

            SqlParameter sqlparamFieldtxt11 = new SqlParameter();
            sqlparamFieldtxt11.ParameterName = "@Fieldtxt11";
            sqlparamFieldtxt11.Direction = ParameterDirection.Input;
            sqlparamFieldtxt11.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt11.Value = data.Fieldtxt11;
            objParamColl.Add(sqlparamFieldtxt11);

            SqlParameter sqlparamFieldtxt12 = new SqlParameter();
            sqlparamFieldtxt12.ParameterName = "@Fieldtxt12";
            sqlparamFieldtxt12.Direction = ParameterDirection.Input;
            sqlparamFieldtxt12.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt12.Value = data.Fieldtxt12;
            objParamColl.Add(sqlparamFieldtxt12);

            SqlParameter sqlparamFieldtxt13 = new SqlParameter();
            sqlparamFieldtxt13.ParameterName = "@Fieldtxt13";
            sqlparamFieldtxt13.Direction = ParameterDirection.Input;
            sqlparamFieldtxt13.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt13.Value = data.Fieldtxt13;
            objParamColl.Add(sqlparamFieldtxt13);

            SqlParameter sqlparamFieldtxt14 = new SqlParameter();
            sqlparamFieldtxt14.ParameterName = "@Fieldtxt14";
            sqlparamFieldtxt14.Direction = ParameterDirection.Input;
            sqlparamFieldtxt14.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt14.Value = data.Fieldtxt14;
            objParamColl.Add(sqlparamFieldtxt14);


            SqlParameter sqlparamFieldtxt15 = new SqlParameter();
            sqlparamFieldtxt15.ParameterName = "@Fieldtxt15";
            sqlparamFieldtxt15.Direction = ParameterDirection.Input;
            sqlparamFieldtxt15.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt15.Value = data.Fieldtxt15;
            objParamColl.Add(sqlparamFieldtxt15);


            SqlParameter sqlparamFieldtxt16 = new SqlParameter();
            sqlparamFieldtxt16.ParameterName = "@Fieldtxt16";
            sqlparamFieldtxt16.Direction = ParameterDirection.Input;
            sqlparamFieldtxt16.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt16.Value = data.Fieldtxt16;
            objParamColl.Add(sqlparamFieldtxt16);


            SqlParameter sqlparamFieldtxt17 = new SqlParameter();
            sqlparamFieldtxt17.ParameterName = "@Fieldtxt17";
            sqlparamFieldtxt17.Direction = ParameterDirection.Input;
            sqlparamFieldtxt17.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt17.Value = data.Fieldtxt17;
            objParamColl.Add(sqlparamFieldtxt17);


            SqlParameter sqlparamFieldtxt18 = new SqlParameter();
            sqlparamFieldtxt18.ParameterName = "@Fieldtxt18";
            sqlparamFieldtxt18.Direction = ParameterDirection.Input;
            sqlparamFieldtxt18.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt18.Value = data.Fieldtxt18;
            objParamColl.Add(sqlparamFieldtxt18);




            SqlParameter sqlparamFieldList1 = new SqlParameter();
            sqlparamFieldList1.ParameterName = "@FieldList1";
            sqlparamFieldList1.Direction = ParameterDirection.Input;
            sqlparamFieldList1.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList1.Value = data.FieldList1;
            objParamColl.Add(sqlparamFieldList1);

            SqlParameter sqlparamFieldList2 = new SqlParameter();
            sqlparamFieldList2.ParameterName = "@FieldList2";
            sqlparamFieldList2.Direction = ParameterDirection.Input;
            sqlparamFieldList2.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList2.Value = data.FieldList2;
            objParamColl.Add(sqlparamFieldList2);

            SqlParameter sqlparamFieldList3 = new SqlParameter();
            sqlparamFieldList3.ParameterName = "@FieldList3";
            sqlparamFieldList3.Direction = ParameterDirection.Input;
            sqlparamFieldList3.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList3.Value = data.FieldList3;
            objParamColl.Add(sqlparamFieldList3);

            SqlParameter sqlparamFieldList4 = new SqlParameter();
            sqlparamFieldList4.ParameterName = "@FieldList4";
            sqlparamFieldList4.Direction = ParameterDirection.Input;
            sqlparamFieldList4.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList4.Value = data.FieldList4;
            objParamColl.Add(sqlparamFieldList4);

            SqlParameter sqlparamFieldList5 = new SqlParameter();
            sqlparamFieldList5.ParameterName = "@FieldList5";
            sqlparamFieldList5.Direction = ParameterDirection.Input;
            sqlparamFieldList5.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList5.Value = data.FieldList5;
            objParamColl.Add(sqlparamFieldList5);

            SqlParameter sqlparamFieldList6 = new SqlParameter();
            sqlparamFieldList6.ParameterName = "@FieldList6";
            sqlparamFieldList6.Direction = ParameterDirection.Input;
            sqlparamFieldList6.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList6.Value = data.FieldList6;
            objParamColl.Add(sqlparamFieldList6);

            SqlParameter sqlparamFieldList7 = new SqlParameter();
            sqlparamFieldList7.ParameterName = "@FieldList7";
            sqlparamFieldList7.Direction = ParameterDirection.Input;
            sqlparamFieldList7.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList7.Value = data.FieldList7;
            objParamColl.Add(sqlparamFieldList7);

            SqlParameter sqlparamFieldList8 = new SqlParameter();
            sqlparamFieldList8.ParameterName = "@FieldList8";
            sqlparamFieldList8.Direction = ParameterDirection.Input;
            sqlparamFieldList8.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList8.Value = data.FieldList8;
            objParamColl.Add(sqlparamFieldList8);


            SqlParameter sqlparamFieldList9 = new SqlParameter();
            sqlparamFieldList9.ParameterName = "@FieldList9";
            sqlparamFieldList9.Direction = ParameterDirection.Input;
            sqlparamFieldList9.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList9.Value = data.FieldList9;
            objParamColl.Add(sqlparamFieldList9);

            SqlParameter sqlparamFieldList10 = new SqlParameter();
            sqlparamFieldList10.ParameterName = "@FieldList10";
            sqlparamFieldList10.Direction = ParameterDirection.Input;
            sqlparamFieldList10.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList10.Value = data.FieldList10;
            objParamColl.Add(sqlparamFieldList10);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEExpInsertMasterData ", objCommand);
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

    }

    public void UpdateMasterData(MasterEntity data)
    {

        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        objCommand = new SqlCommand();
        objParamColl = objCommand.Parameters;
        try
        {



            SqlParameter sqlparamintExpId = new SqlParameter();
            sqlparamintExpId.ParameterName = "@intExpId";
            sqlparamintExpId.Direction = ParameterDirection.Input;
            sqlparamintExpId.SqlDbType = SqlDbType.Int;
            sqlparamintExpId.Value = data.intExpId;
            objParamColl.Add(sqlparamintExpId);


            SqlParameter sqlparamPU = new SqlParameter();
            sqlparamPU.ParameterName = "@PU";
            sqlparamPU.Direction = ParameterDirection.Input;
            sqlparamPU.SqlDbType = SqlDbType.NVarChar;
            sqlparamPU.Value = data.PUCode;
            objParamColl.Add(sqlparamPU);

            SqlParameter sqlparamBU = new SqlParameter();
            sqlparamBU.ParameterName = "@BU";
            sqlparamBU.Direction = ParameterDirection.Input;
            sqlparamBU.SqlDbType = SqlDbType.NVarChar;
            sqlparamBU.Value = data.BUCode;
            objParamColl.Add(sqlparamBU);

            //SqlParameter sqlparamDUCode = new SqlParameter();
            //sqlparamDUCode.ParameterName = "@DUCode";
            //sqlparamDUCode.Direction = ParameterDirection.Input;
            //sqlparamDUCode.SqlDbType = SqlDbType.NVarChar;
            //sqlparamDUCode.Value = data.DUCode;
            //objParamColl.Add(sqlparamDUCode);

            SqlParameter sqlparamDUCode = new SqlParameter();
            sqlparamDUCode.ParameterName = "@DmMailId";
            sqlparamDUCode.Direction = ParameterDirection.Input;
            sqlparamDUCode.SqlDbType = SqlDbType.NVarChar;
            sqlparamDUCode.Value = data.DMMailId;
            objParamColl.Add(sqlparamDUCode);

            SqlParameter sqlparamClientCode = new SqlParameter();
            sqlparamClientCode.ParameterName = "@ClientCode";
            sqlparamClientCode.Direction = ParameterDirection.Input;
            sqlparamClientCode.SqlDbType = SqlDbType.NVarChar;
            sqlparamClientCode.Value = data.ClientCode;
            objParamColl.Add(sqlparamClientCode);

            SqlParameter sqlparamExpType = new SqlParameter();
            sqlparamExpType.ParameterName = "@ExpType";
            sqlparamExpType.Direction = ParameterDirection.Input;
            sqlparamExpType.SqlDbType = SqlDbType.NVarChar;
            sqlparamExpType.Value = data.ExpType;
            objParamColl.Add(sqlparamExpType);

            SqlParameter sqlparamExpCategory = new SqlParameter();
            sqlparamExpCategory.ParameterName = "@ExpCategory";
            sqlparamExpCategory.Direction = ParameterDirection.Input;
            sqlparamExpCategory.SqlDbType = SqlDbType.NVarChar;
            sqlparamExpCategory.Value = data.ExpCategory;
            objParamColl.Add(sqlparamExpCategory);

            SqlParameter sqlparamItemName = new SqlParameter();
            sqlparamItemName.ParameterName = "@ItemName";
            sqlparamItemName.Direction = ParameterDirection.Input;
            sqlparamItemName.SqlDbType = SqlDbType.NVarChar;
            sqlparamItemName.Value = data.ItemName;
            objParamColl.Add(sqlparamItemName);

            SqlParameter sqlparamPriority = new SqlParameter();
            sqlparamPriority.ParameterName = "@Priority";
            sqlparamPriority.Direction = ParameterDirection.Input;
            sqlparamPriority.SqlDbType = SqlDbType.NVarChar;
            sqlparamPriority.Value = data.Priority;
            objParamColl.Add(sqlparamPriority);

            SqlParameter sqlparamNumberofItems = new SqlParameter();
            sqlparamNumberofItems.ParameterName = "@NumberofItems";
            sqlparamNumberofItems.Direction = ParameterDirection.Input;
            sqlparamNumberofItems.SqlDbType = SqlDbType.NVarChar;
            sqlparamNumberofItems.Value = data.NumberofItems;
            objParamColl.Add(sqlparamNumberofItems);

            SqlParameter sqlparamUnitCost = new SqlParameter();
            sqlparamUnitCost.ParameterName = "@UnitCost";
            sqlparamUnitCost.Direction = ParameterDirection.Input;
            sqlparamUnitCost.SqlDbType = SqlDbType.NVarChar;
            sqlparamUnitCost.Value = data.UnitCost;
            objParamColl.Add(sqlparamUnitCost);

            SqlParameter sqlparamExpenseDate = new SqlParameter();
            sqlparamExpenseDate.ParameterName = "@ExpenseDate";
            sqlparamExpenseDate.Direction = ParameterDirection.Input;
            sqlparamExpenseDate.SqlDbType = SqlDbType.NVarChar;
            sqlparamExpenseDate.Value = data.ExpenseDate;
            objParamColl.Add(sqlparamExpenseDate);

            SqlParameter sqlparamJustificationRemarks = new SqlParameter();
            sqlparamJustificationRemarks.ParameterName = "@JustificationRemarks";
            sqlparamJustificationRemarks.Direction = ParameterDirection.Input;
            sqlparamJustificationRemarks.SqlDbType = SqlDbType.NVarChar;
            sqlparamJustificationRemarks.Value = data.JustificationRemarks;
            objParamColl.Add(sqlparamJustificationRemarks);

            SqlParameter sqlparamIsCustomerRecoverable = new SqlParameter();
            sqlparamIsCustomerRecoverable.ParameterName = "@IsCustomerRecoverable";
            sqlparamIsCustomerRecoverable.Direction = ParameterDirection.Input;
            sqlparamIsCustomerRecoverable.SqlDbType = SqlDbType.NVarChar;
            sqlparamIsCustomerRecoverable.Value = data.IsCustomerRecoverable;
            objParamColl.Add(sqlparamIsCustomerRecoverable);

            SqlParameter sqlparamProjOppCode = new SqlParameter();
            sqlparamProjOppCode.ParameterName = "@ProjOppCode";
            sqlparamProjOppCode.Direction = ParameterDirection.Input;
            sqlparamProjOppCode.SqlDbType = SqlDbType.NVarChar;
            sqlparamProjOppCode.Value = data.ProjOppCode;
            objParamColl.Add(sqlparamProjOppCode);

            SqlParameter sqlparamIsBudgetedinPBS = new SqlParameter();
            sqlparamIsBudgetedinPBS.ParameterName = "@IsBudgetedinPBS";
            sqlparamIsBudgetedinPBS.Direction = ParameterDirection.Input;
            sqlparamIsBudgetedinPBS.SqlDbType = SqlDbType.NVarChar;
            sqlparamIsBudgetedinPBS.Value = data.IsBudgetedinPBS;
            objParamColl.Add(sqlparamIsBudgetedinPBS);

            SqlParameter sqlparamBEUpside = new SqlParameter();
            sqlparamBEUpside.ParameterName = "@BEUpside";
            sqlparamBEUpside.Direction = ParameterDirection.Input;
            sqlparamBEUpside.SqlDbType = SqlDbType.NVarChar;
            sqlparamBEUpside.Value = data.BEUpside;
            objParamColl.Add(sqlparamBEUpside);

            SqlParameter sqlparamBEDownside = new SqlParameter();
            sqlparamBEDownside.ParameterName = "@BEDownside";
            sqlparamBEDownside.Direction = ParameterDirection.Input;
            sqlparamBEDownside.SqlDbType = SqlDbType.NVarChar;
            sqlparamBEDownside.Value = data.BEDownside;
            objParamColl.Add(sqlparamBEDownside);

            SqlParameter sqlparamCurrQtr = new SqlParameter();
            sqlparamCurrQtr.ParameterName = "@CurrQtr";
            sqlparamCurrQtr.Direction = ParameterDirection.Input;
            sqlparamCurrQtr.SqlDbType = SqlDbType.NVarChar;
            sqlparamCurrQtr.Value = data.CurrQtr;
            objParamColl.Add(sqlparamCurrQtr);

            SqlParameter sqlparamFutQtrBE = new SqlParameter();
            sqlparamFutQtrBE.ParameterName = "@FutQtrBE";
            sqlparamFutQtrBE.Direction = ParameterDirection.Input;
            sqlparamFutQtrBE.SqlDbType = SqlDbType.NVarChar;
            sqlparamFutQtrBE.Value = data.FutQtrBE;
            objParamColl.Add(sqlparamFutQtrBE);

            SqlParameter sqlparamStatus = new SqlParameter();
            sqlparamStatus.ParameterName = "@Status";
            sqlparamStatus.Direction = ParameterDirection.Input;
            sqlparamStatus.SqlDbType = SqlDbType.NVarChar;
            sqlparamStatus.Value = data.Status;
            objParamColl.Add(sqlparamStatus);

            SqlParameter sqlparamFieldDate1 = new SqlParameter();
            sqlparamFieldDate1.ParameterName = "@FieldDate1";
            sqlparamFieldDate1.Direction = ParameterDirection.Input;
            sqlparamFieldDate1.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldDate1.Value = data.FieldDate1;
            objParamColl.Add(sqlparamFieldDate1);

            SqlParameter sqlparamFieldDate2 = new SqlParameter();
            sqlparamFieldDate2.ParameterName = "@FieldDate2";
            sqlparamFieldDate2.Direction = ParameterDirection.Input;
            sqlparamFieldDate2.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldDate2.Value = data.FieldDate2;
            objParamColl.Add(sqlparamFieldDate2);

            SqlParameter sqlparamFieldDate3 = new SqlParameter();
            sqlparamFieldDate3.ParameterName = "@FieldDate3";
            sqlparamFieldDate3.Direction = ParameterDirection.Input;
            sqlparamFieldDate3.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldDate3.Value = data.FieldDate3;
            objParamColl.Add(sqlparamFieldDate3);

            SqlParameter sqlparamFieldDate4 = new SqlParameter();
            sqlparamFieldDate4.ParameterName = "@FieldDate4";
            sqlparamFieldDate4.Direction = ParameterDirection.Input;
            sqlparamFieldDate4.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldDate4.Value = data.FieldDate4;
            objParamColl.Add(sqlparamFieldDate4);

            SqlParameter sqlparamFieldDate5 = new SqlParameter();
            sqlparamFieldDate5.ParameterName = "@FieldDate5";
            sqlparamFieldDate5.Direction = ParameterDirection.Input;
            sqlparamFieldDate5.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldDate5.Value = data.FieldDate5;
            objParamColl.Add(sqlparamFieldDate5);

            SqlParameter sqlparamFieldDate6 = new SqlParameter();
            sqlparamFieldDate6.ParameterName = "@FieldDate6";
            sqlparamFieldDate6.Direction = ParameterDirection.Input;
            sqlparamFieldDate6.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldDate6.Value = data.FieldDate6;
            objParamColl.Add(sqlparamFieldDate6);

            SqlParameter sqlparamFieldDate7 = new SqlParameter();
            sqlparamFieldDate7.ParameterName = "@FieldDate7";
            sqlparamFieldDate7.Direction = ParameterDirection.Input;
            sqlparamFieldDate7.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldDate7.Value = data.FieldDate7;
            objParamColl.Add(sqlparamFieldDate7);

            SqlParameter sqlparamFieldDate8 = new SqlParameter();
            sqlparamFieldDate8.ParameterName = "@FieldDate8";
            sqlparamFieldDate8.Direction = ParameterDirection.Input;
            sqlparamFieldDate8.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldDate8.Value = data.FieldDate8;
            objParamColl.Add(sqlparamFieldDate8);

            SqlParameter sqlparamCreatedBy = new SqlParameter();
            sqlparamCreatedBy.ParameterName = "@CreatedBy";
            sqlparamCreatedBy.Direction = ParameterDirection.Input;
            sqlparamCreatedBy.SqlDbType = SqlDbType.NVarChar;
            sqlparamCreatedBy.Value = data.CreatedBy;
            objParamColl.Add(sqlparamCreatedBy);

            SqlParameter sqlparamCreatedOn = new SqlParameter();
            sqlparamCreatedOn.ParameterName = "@CreatedOn";
            sqlparamCreatedOn.Direction = ParameterDirection.Input;
            sqlparamCreatedOn.SqlDbType = SqlDbType.NVarChar;
            sqlparamCreatedOn.Value = data.CreatedOn;
            objParamColl.Add(sqlparamCreatedOn);

            SqlParameter sqlparamModifiedBy = new SqlParameter();
            sqlparamModifiedBy.ParameterName = "@ModifiedBy";
            sqlparamModifiedBy.Direction = ParameterDirection.Input;
            sqlparamModifiedBy.SqlDbType = SqlDbType.NVarChar;
            sqlparamModifiedBy.Value = data.ModifiedBy;
            objParamColl.Add(sqlparamModifiedBy);

            SqlParameter sqlparamModifiedOn = new SqlParameter();
            sqlparamModifiedOn.ParameterName = "@ModifiedOn";
            sqlparamModifiedOn.Direction = ParameterDirection.Input;
            sqlparamModifiedOn.SqlDbType = SqlDbType.NVarChar;
            sqlparamModifiedOn.Value = data.ModifiedOn;
            objParamColl.Add(sqlparamModifiedOn);

            SqlParameter sqlparamFieldtxt1 = new SqlParameter();
            sqlparamFieldtxt1.ParameterName = "@Fieldtxt1";
            sqlparamFieldtxt1.Direction = ParameterDirection.Input;
            sqlparamFieldtxt1.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt1.Value = data.Fieldtxt1;
            objParamColl.Add(sqlparamFieldtxt1);

            SqlParameter sqlparamFieldtxt2 = new SqlParameter();
            sqlparamFieldtxt2.ParameterName = "@Fieldtxt2";
            sqlparamFieldtxt2.Direction = ParameterDirection.Input;
            sqlparamFieldtxt2.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt2.Value = data.Fieldtxt2;
            objParamColl.Add(sqlparamFieldtxt2);

            SqlParameter sqlparamFieldtxt3 = new SqlParameter();
            sqlparamFieldtxt3.ParameterName = "@Fieldtxt3";
            sqlparamFieldtxt3.Direction = ParameterDirection.Input;
            sqlparamFieldtxt3.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt3.Value = data.Fieldtxt3;
            objParamColl.Add(sqlparamFieldtxt3);

            SqlParameter sqlparamFieldtxt4 = new SqlParameter();
            sqlparamFieldtxt4.ParameterName = "@Fieldtxt4";
            sqlparamFieldtxt4.Direction = ParameterDirection.Input;
            sqlparamFieldtxt4.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt4.Value = data.Fieldtxt4;
            objParamColl.Add(sqlparamFieldtxt4);

            SqlParameter sqlparamFieldtxt5 = new SqlParameter();
            sqlparamFieldtxt5.ParameterName = "@Fieldtxt5";
            sqlparamFieldtxt5.Direction = ParameterDirection.Input;
            sqlparamFieldtxt5.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt5.Value = data.Fieldtxt5;
            objParamColl.Add(sqlparamFieldtxt5);

            SqlParameter sqlparamFieldtxt6 = new SqlParameter();
            sqlparamFieldtxt6.ParameterName = "@Fieldtxt6";
            sqlparamFieldtxt6.Direction = ParameterDirection.Input;
            sqlparamFieldtxt6.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt6.Value = data.Fieldtxt6;
            objParamColl.Add(sqlparamFieldtxt6);

            SqlParameter sqlparamFieldtxt7 = new SqlParameter();
            sqlparamFieldtxt7.ParameterName = "@Fieldtxt7";
            sqlparamFieldtxt7.Direction = ParameterDirection.Input;
            sqlparamFieldtxt7.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt7.Value = data.Fieldtxt7;
            objParamColl.Add(sqlparamFieldtxt7);

            SqlParameter sqlparamFieldtxt8 = new SqlParameter();
            sqlparamFieldtxt8.ParameterName = "@Fieldtxt8";
            sqlparamFieldtxt8.Direction = ParameterDirection.Input;
            sqlparamFieldtxt8.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt8.Value = data.Fieldtxt8;
            objParamColl.Add(sqlparamFieldtxt8);

            SqlParameter sqlparamFieldtxt9 = new SqlParameter();
            sqlparamFieldtxt9.ParameterName = "@Fieldtxt9";
            sqlparamFieldtxt9.Direction = ParameterDirection.Input;
            sqlparamFieldtxt9.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt9.Value = data.Fieldtxt9;
            objParamColl.Add(sqlparamFieldtxt9);

            SqlParameter sqlparamFieldtxt10 = new SqlParameter();
            sqlparamFieldtxt10.ParameterName = "@Fieldtxt10";
            sqlparamFieldtxt10.Direction = ParameterDirection.Input;
            sqlparamFieldtxt10.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt10.Value = data.Fieldtxt10;
            objParamColl.Add(sqlparamFieldtxt10);

            SqlParameter sqlparamFieldtxt11 = new SqlParameter();
            sqlparamFieldtxt11.ParameterName = "@Fieldtxt11";
            sqlparamFieldtxt11.Direction = ParameterDirection.Input;
            sqlparamFieldtxt11.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt11.Value = data.Fieldtxt11;
            objParamColl.Add(sqlparamFieldtxt11);

            SqlParameter sqlparamFieldtxt12 = new SqlParameter();
            sqlparamFieldtxt12.ParameterName = "@Fieldtxt12";
            sqlparamFieldtxt12.Direction = ParameterDirection.Input;
            sqlparamFieldtxt12.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt12.Value = data.Fieldtxt12;
            objParamColl.Add(sqlparamFieldtxt12);

            SqlParameter sqlparamFieldtxt13 = new SqlParameter();
            sqlparamFieldtxt13.ParameterName = "@Fieldtxt13";
            sqlparamFieldtxt13.Direction = ParameterDirection.Input;
            sqlparamFieldtxt13.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt13.Value = data.Fieldtxt13;
            objParamColl.Add(sqlparamFieldtxt13);

            SqlParameter sqlparamFieldtxt14 = new SqlParameter();
            sqlparamFieldtxt14.ParameterName = "@Fieldtxt14";
            sqlparamFieldtxt14.Direction = ParameterDirection.Input;
            sqlparamFieldtxt14.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt14.Value = data.Fieldtxt14;
            objParamColl.Add(sqlparamFieldtxt14);


            SqlParameter sqlparamFieldtxt15 = new SqlParameter();
            sqlparamFieldtxt15.ParameterName = "@Fieldtxt15";
            sqlparamFieldtxt15.Direction = ParameterDirection.Input;
            sqlparamFieldtxt15.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt15.Value = data.Fieldtxt15;
            objParamColl.Add(sqlparamFieldtxt15);


            SqlParameter sqlparamFieldtxt16 = new SqlParameter();
            sqlparamFieldtxt16.ParameterName = "@Fieldtxt16";
            sqlparamFieldtxt16.Direction = ParameterDirection.Input;
            sqlparamFieldtxt16.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt16.Value = data.Fieldtxt16;
            objParamColl.Add(sqlparamFieldtxt16);


            SqlParameter sqlparamFieldtxt17 = new SqlParameter();
            sqlparamFieldtxt17.ParameterName = "@Fieldtxt17";
            sqlparamFieldtxt17.Direction = ParameterDirection.Input;
            sqlparamFieldtxt17.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt17.Value = data.Fieldtxt17;
            objParamColl.Add(sqlparamFieldtxt17);


            SqlParameter sqlparamFieldtxt18 = new SqlParameter();
            sqlparamFieldtxt18.ParameterName = "@Fieldtxt18";
            sqlparamFieldtxt18.Direction = ParameterDirection.Input;
            sqlparamFieldtxt18.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldtxt18.Value = data.Fieldtxt18;
            objParamColl.Add(sqlparamFieldtxt18);



            SqlParameter sqlparamFieldList1 = new SqlParameter();
            sqlparamFieldList1.ParameterName = "@FieldList1";
            sqlparamFieldList1.Direction = ParameterDirection.Input;
            sqlparamFieldList1.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList1.Value = data.FieldList1;
            objParamColl.Add(sqlparamFieldList1);

            SqlParameter sqlparamFieldList2 = new SqlParameter();
            sqlparamFieldList2.ParameterName = "@FieldList2";
            sqlparamFieldList2.Direction = ParameterDirection.Input;
            sqlparamFieldList2.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList2.Value = data.FieldList2;
            objParamColl.Add(sqlparamFieldList2);

            SqlParameter sqlparamFieldList3 = new SqlParameter();
            sqlparamFieldList3.ParameterName = "@FieldList3";
            sqlparamFieldList3.Direction = ParameterDirection.Input;
            sqlparamFieldList3.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList3.Value = data.FieldList3;
            objParamColl.Add(sqlparamFieldList3);

            SqlParameter sqlparamFieldList4 = new SqlParameter();
            sqlparamFieldList4.ParameterName = "@FieldList4";
            sqlparamFieldList4.Direction = ParameterDirection.Input;
            sqlparamFieldList4.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList4.Value = data.FieldList4;
            objParamColl.Add(sqlparamFieldList4);

            SqlParameter sqlparamFieldList5 = new SqlParameter();
            sqlparamFieldList5.ParameterName = "@FieldList5";
            sqlparamFieldList5.Direction = ParameterDirection.Input;
            sqlparamFieldList5.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList5.Value = data.FieldList5;
            objParamColl.Add(sqlparamFieldList5);

            SqlParameter sqlparamFieldList6 = new SqlParameter();
            sqlparamFieldList6.ParameterName = "@FieldList6";
            sqlparamFieldList6.Direction = ParameterDirection.Input;
            sqlparamFieldList6.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList6.Value = data.FieldList6;
            objParamColl.Add(sqlparamFieldList6);

            SqlParameter sqlparamFieldList7 = new SqlParameter();
            sqlparamFieldList7.ParameterName = "@FieldList7";
            sqlparamFieldList7.Direction = ParameterDirection.Input;
            sqlparamFieldList7.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList7.Value = data.FieldList7;
            objParamColl.Add(sqlparamFieldList7);

            SqlParameter sqlparamFieldList8 = new SqlParameter();
            sqlparamFieldList8.ParameterName = "@FieldList8";
            sqlparamFieldList8.Direction = ParameterDirection.Input;
            sqlparamFieldList8.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList8.Value = data.FieldList8;
            objParamColl.Add(sqlparamFieldList8);

            SqlParameter sqlparamFieldList9 = new SqlParameter();
            sqlparamFieldList9.ParameterName = "@FieldList9";
            sqlparamFieldList9.Direction = ParameterDirection.Input;
            sqlparamFieldList9.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList9.Value = data.FieldList9;
            objParamColl.Add(sqlparamFieldList9);

            SqlParameter sqlparamFieldList10 = new SqlParameter();
            sqlparamFieldList10.ParameterName = "@FieldList10";
            sqlparamFieldList10.Direction = ParameterDirection.Input;
            sqlparamFieldList10.SqlDbType = SqlDbType.NVarChar;
            sqlparamFieldList10.Value = data.FieldList10;
            objParamColl.Add(sqlparamFieldList10);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEExpUpdateMasterData ", objCommand);
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////

    public DataTable GetAlconPBSData(string customerCode, string userid, string su, string NSO)
    {


        DataSet ds = new DataSet();
        SqlParameter objParm, objParm1, objParam2, objParam3;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        string reportCode = string.Empty;
        DataTable dtAlcon = null;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@customercode";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = customerCode;

            objParm1 = new SqlParameter();
            objParm1.ParameterName = "@su";
            objParm1.Direction = ParameterDirection.Input;
            objParm1.SqlDbType = SqlDbType.VarChar;
            objParm1.Value = su;


            objParam2 = new SqlParameter();
            objParam2.ParameterName = "@userid";
            objParam2.Direction = ParameterDirection.Input;
            objParam2.SqlDbType = SqlDbType.VarChar;
            objParam2.Value = userid;


            objParam3 = new SqlParameter();
            objParam3.ParameterName = "@newOffering";
            objParam3.Direction = ParameterDirection.Input;
            objParam3.SqlDbType = SqlDbType.VarChar;
            objParam3.Value = NSO;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);
            objParamColl.Add(objParm1);
            objParamColl.Add(objParam2);
            objParamColl.Add(objParam3);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("[EAS_SP_BeFetchAlconPBS_SU_NSO]", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                dtAlcon = ds.Tables[0];
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return dtAlcon;
    }

    public int UpdateActExchangeRate(string type, string qtr, string year, int m1, int m2, int m3)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();

        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@qtr";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@year";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@Month1";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.Float;
            sqlparam3.Value = m1;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@Month2";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.Float;
            sqlparam4.Value = m2;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@Month3";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.Float;
            sqlparam5.Value = m3;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@Type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);

            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.Direction = ParameterDirection.ReturnValue;
            objParamStatus.SqlDbType = SqlDbType.Int;
            objParamStatus.ParameterName = "ReturnValue";
            objParamColl.Add(objParamStatus);


            objData.ExecuteSP("dbo.spUpdateActExchangeRate", ref  ds, objCommand);

            int updatestatus = (int)objCommand.Parameters["ReturnValue"].Value;
            return updatestatus;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable FetchMonthlyActRates(string qtr, string year)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();

        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@qtr";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@year";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);


            objData.ExecuteSP("dbo.spBeGetMonthlyRates", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable FetchMonthlyActRate(string qtr, string year, int m1, int m2, int m3)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();

        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@qtr";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@year";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@Month1";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.Float;
            sqlparam3.Value = m1;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@Month2";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.Float;
            sqlparam4.Value = m2;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@Month3";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.Float;
            sqlparam5.Value = m3;
            objParamColl.Add(sqlparam5);


            objData.ExecuteSP("dbo.spFetchMonthlyActRate", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }


    public DataTable FetchActMonths(string qtr, string year)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();

        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@qtr";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@year";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);


            objData.ExecuteSP("dbo.BEPortalConfig", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }
    public DataTable FetchFinpulseYear()
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            objData.ExecuteSP("dbo.EAS_SP_GetFinpulseYear", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        //return new DataTable();
    }

    public int AlconDumpCountDev()
    {
        int count = 0;
        DataSet ds = new DataSet();

        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.EAS_SP_AlconDumpCount", ref ds);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                count = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL"].ToString());
            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return count;
    }

    public int FinPulseDumpCountDev()
    {
        int count = 0;
        DataSet ds = new DataSet();

        try
        {
            //ObjData = new DataAccess();FinPulseDumpCountDev
            objData.GetConnection();
            objData.ExecuteSP("dbo.EAS_SP_FinpulseDumpCount", ref ds);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                count = Convert.ToInt32(ds.Tables[0].Rows[0]["Total"].ToString());
            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return count;
    }

    public List<string> GetAllPUs(string userid)
    {
        DataSet ds = new DataSet();
        List<string> allpus = new List<string>();

        SqlCommand objCommand;
        try
        {

            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@txtUserid";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userid;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;



            objParamColl.Add(objParamUserId);
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEAllPU", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataTable dt = new DataTable();
                    allpus.Add(ds.Tables[0].Rows[i]["txtPU"] + "");

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allpus;

    }

    public DataSet GetDataSet(string cmdText)
    {
        SqlCommand cmd = new SqlCommand(cmdText);
        //cmd.CommandTimeout = 60;
        cmd.CommandTimeout = int.MaxValue;
        SqlConnection G_DBConnection = new SqlConnection(G_connStr);
        cmd.Connection = G_DBConnection;
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        try
        {
            G_DBConnection.Open();
            da.Fill(ds);
            G_DBConnection.Close();
            return ds;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            /*
            lblErrMsg.Text = ex.Message;
            lblErrMsg.Visible = true;
             */
        }
        finally
        {
            if (G_DBConnection.State != ConnectionState.Closed)
                G_DBConnection.Close();
        }
        return null;
    }
    public int AddMasterCustomer(string ServiceLine, string MCC, string nso, string NC, string Quarter, string Fyyear, string SDMorDM, string role, int rdbvalue, string MachineUserID)
    {

        //TODO:12/10 is all DMs commented
        //string _IsAllDU = (IsAllDU + string.Empty).ToLower() == "yes" ? "Y" : "N";
        DataSet ds = new DataSet();
        SqlParameter objParm1;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        try
        {
            objParm1 = new SqlParameter();
            objParm1.ParameterName = "@MachineUserId";
            objParm1.Direction = ParameterDirection.Input;
            objParm1.Size = 50;
            objParm1.SqlDbType = SqlDbType.VarChar;
            objParm1.Value = MachineUserID.TrimEnd().TrimStart();

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@MasterCustomerCode";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.Size = 50;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = MCC.TrimEnd().TrimStart();

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@ServiceLine";
            objParamStatus3.Size = 50;
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = ServiceLine.TrimEnd().TrimStart();

            SqlParameter objParmPU = new SqlParameter();
            objParmPU.ParameterName = "@NSO";
            objParmPU.Direction = ParameterDirection.Input;
            objParmPU.Size = 50;
            objParmPU.SqlDbType = SqlDbType.VarChar;
            objParmPU.Value = nso.TrimEnd().TrimStart();

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@NC";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = NC.TrimEnd().TrimStart();

            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@Quarter";
            objParamStatus5.Size = 50;
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.NVarChar;
            objParamStatus5.Value = Quarter.TrimEnd().TrimStart();

            SqlParameter objParamStatus10 = new SqlParameter();
            objParamStatus10.ParameterName = "@FYyear";
            objParamStatus10.Size = 50;
            objParamStatus10.Direction = ParameterDirection.Input;
            objParamStatus10.SqlDbType = SqlDbType.NVarChar;
            objParamStatus10.Value = Fyyear.TrimEnd().TrimStart();


            string s = SDMorDM;
            string ss = SDMorDM.TrimStart().TrimEnd();
            SqlParameter objParamStatus6 = new SqlParameter();
            objParamStatus6.ParameterName = "@SDMorDM";
            objParamStatus6.Size = 50;
            objParamStatus6.Direction = ParameterDirection.Input;
            objParamStatus6.SqlDbType = SqlDbType.VarChar;
            objParamStatus6.Value = SDMorDM.TrimStart().TrimEnd();

            SqlParameter objParamStatus8 = new SqlParameter();
            objParamStatus8.ParameterName = "@role";
            objParamStatus8.Size = 10;
            objParamStatus8.Direction = ParameterDirection.Input;
            objParamStatus8.SqlDbType = SqlDbType.NChar;
            objParamStatus8.Value = role.TrimEnd().TrimStart();

            SqlParameter objParamStatus9 = new SqlParameter();
            objParamStatus9.ParameterName = "@rdbvalue";
            objParamStatus9.Size = 10;
            objParamStatus9.Direction = ParameterDirection.Input;
            objParamStatus9.SqlDbType = SqlDbType.Int;
            objParamStatus9.Value = rdbvalue;

            SqlParameter objParamStatus7 = new SqlParameter();
            objParamStatus7.Direction = ParameterDirection.ReturnValue;
            objParamStatus7.SqlDbType = SqlDbType.Int;
            objParamStatus7.ParameterName = "ReturnValue";
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            objParamColl.AddRange(new SqlParameter[] { objParm1, objParamStatus2, objParamStatus3, objParamStatus4, objParamStatus5, objParamStatus10, objParamStatus6, objParamStatus7, objParamStatus8, objParamStatus9, objParmPU });
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_AddMasterCustomer_NSO", ref ds, objCommand);

            int updatestatus = (int)objCommand.Parameters["ReturnValue"].Value;
            return updatestatus;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {

            objData.CloseConnection();
        }



    }

    public int AddMasterCustomerNSO(string ServiceLine, string MCC, string NC, string Quarter, string Fyyear, string SDMorDM, string role, int rdbvalue, string MachineUserID, string NewServiceOffering)
    {

        //TODO:12/10 is all DMs commented
        //string _IsAllDU = (IsAllDU + string.Empty).ToLower() == "yes" ? "Y" : "N";
        DataSet ds = new DataSet();
        SqlParameter objParm1;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        try
        {
            objParm1 = new SqlParameter();
            objParm1.ParameterName = "@MachineUserId";
            objParm1.Direction = ParameterDirection.Input;
            objParm1.Size = 50;
            objParm1.SqlDbType = SqlDbType.VarChar;
            objParm1.Value = MachineUserID.TrimEnd().TrimStart();

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@MasterCustomerCode";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.Size = 50;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = MCC.TrimEnd().TrimStart();

            SqlParameter objNOS = new SqlParameter();
            objNOS.ParameterName = "@NSO";
            objNOS.Direction = ParameterDirection.Input;
            objNOS.Size = 100;
            objNOS.SqlDbType = SqlDbType.VarChar;
            objNOS.Value = NewServiceOffering.TrimEnd().TrimStart();

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@ServiceLine";
            objParamStatus3.Size = 50;
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = ServiceLine.TrimEnd().TrimStart();



            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@NC";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = NC.TrimEnd().TrimStart();

            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@Quarter";
            objParamStatus5.Size = 50;
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.NVarChar;
            objParamStatus5.Value = Quarter.TrimEnd().TrimStart();

            SqlParameter objParamStatus10 = new SqlParameter();
            objParamStatus10.ParameterName = "@FYyear";
            objParamStatus10.Size = 50;
            objParamStatus10.Direction = ParameterDirection.Input;
            objParamStatus10.SqlDbType = SqlDbType.NVarChar;
            objParamStatus10.Value = Fyyear.TrimEnd().TrimStart();


            string s = SDMorDM;
            string ss = SDMorDM.TrimStart().TrimEnd();
            SqlParameter objParamStatus6 = new SqlParameter();
            objParamStatus6.ParameterName = "@SDMorDM";
            objParamStatus6.Size = 50;
            objParamStatus6.Direction = ParameterDirection.Input;
            objParamStatus6.SqlDbType = SqlDbType.VarChar;
            objParamStatus6.Value = SDMorDM.TrimStart().TrimEnd();

            SqlParameter objParamStatus8 = new SqlParameter();
            objParamStatus8.ParameterName = "@role";
            objParamStatus8.Size = 10;
            objParamStatus8.Direction = ParameterDirection.Input;
            objParamStatus8.SqlDbType = SqlDbType.NChar;
            objParamStatus8.Value = role.TrimEnd().TrimStart();

            SqlParameter objParamStatus9 = new SqlParameter();
            objParamStatus9.ParameterName = "@rdbvalue";
            objParamStatus9.Size = 10;
            objParamStatus9.Direction = ParameterDirection.Input;
            objParamStatus9.SqlDbType = SqlDbType.Int;
            objParamStatus9.Value = rdbvalue;

            SqlParameter objParamStatus7 = new SqlParameter();
            objParamStatus7.Direction = ParameterDirection.ReturnValue;
            objParamStatus7.SqlDbType = SqlDbType.Int;
            objParamStatus7.ParameterName = "ReturnValue";
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            objParamColl.AddRange(new SqlParameter[] { objParm1, objParamStatus2, objParamStatus3, objParamStatus4, objParamStatus5, objParamStatus10, objParamStatus6, objParamStatus7, objParamStatus8, objParamStatus9, objNOS });
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_AddMasterCustomer_NSO", ref ds, objCommand);

            int updatestatus = (int)objCommand.Parameters["ReturnValue"].Value;
            return updatestatus;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {

            objData.CloseConnection();
        }



    }

    public DataTable GetMCC(string userID)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        try
        {
            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@UserId";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userID;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objParamUserId);
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_GetPU_CustCode_NSO_Test", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                return ds.Tables[0];
            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return ds.Tables[0];
    }

    public DataSet GetNewServiceOffering(string ServiceLine, string UserId)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        try
        {
            DataTable dt = new DataTable();
            SqlParameter objParamServiceLine = new SqlParameter();
            objParamServiceLine.ParameterName = "@ServiceLine";
            objParamServiceLine.Direction = ParameterDirection.Input;
            objParamServiceLine.SqlDbType = SqlDbType.VarChar;
            objParamServiceLine.Value = ServiceLine;

            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@UserId";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = UserId;

            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objParamServiceLine);
            objParamColl.Add(objParamUserId);
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("Sp_Get_NewServiceOffering", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {

                return ds;
            }
            else
            {
                return ds;
            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

    }

    public DataSet FetchMCC_NSO(string UserId, string SL)
    {
        DataSet ds = new DataSet();
        try
        {

            using (SqlConnection conn = new SqlConnection(G_connStr))
            {

                SqlCommand objCommand;
                SqlParameterCollection objParamColl;
                SqlDataAdapter sqlAdapter;
                SqlParameter objParm = new SqlParameter();
                objParm.ParameterName = "@UserId";
                objParm.Direction = ParameterDirection.Input;
                objParm.SqlDbType = SqlDbType.VarChar;
                objParm.Value = UserId;

                SqlParameter objParm1 = new SqlParameter();
                objParm1.ParameterName = "@ServiceLine";
                objParm1.Direction = ParameterDirection.Input;
                objParm1.SqlDbType = SqlDbType.VarChar;
                objParm1.Value = SL;

                objCommand = new SqlCommand();
                objParamColl = objCommand.Parameters;
                objParamColl.Add(objParm);
                objParamColl.Add(objParm1);
                objCommand.Connection = conn;
                objCommand.CommandTimeout = 500;
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "SP_FetchMCC_NSO";

                sqlAdapter = new SqlDataAdapter(objCommand);
                sqlAdapter.Fill(ds);

                sqlAdapter.Dispose();
            }


            return ds;



        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

    }

    public DataTable Get_NSO(string userID, string MCC, string SL)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        try
        {
            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@UserId";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userID;

            SqlParameter objParamMCC = new SqlParameter();
            objParamMCC.ParameterName = "@MCC";
            objParamMCC.Direction = ParameterDirection.Input;
            objParamMCC.SqlDbType = SqlDbType.VarChar;
            objParamMCC.Value = MCC;

            SqlParameter objParamSL = new SqlParameter();
            objParamSL.ParameterName = "@SL";
            objParamSL.Direction = ParameterDirection.Input;
            objParamSL.SqlDbType = SqlDbType.VarChar;
            objParamSL.Value = SL;

            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objParamUserId);
            objParamColl.Add(objParamMCC);
            objParamColl.Add(objParamSL);
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_GetNSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                return ds.Tables[0];
            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return ds.Tables[0];
    }


    public string GetUserAccessRole(string userID)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;


        try
        {


            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@txtUserId";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userID;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;



            objParamColl.Add(objParamUserId);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_spBeReturnRole", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                return ds.Tables[0].Rows[0]["Role"] + "";




            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return "";
    }



    public DataSet GetNotDMBEDataExcelNSO(string customerCode, string userid, string quarter, string year, string role)
    {


        DataSet ds = new DataSet();
        DataSet dsreturn = new DataSet();
        SqlCommand objCommand;
        List<DataTable> retTable = new List<DataTable>();
        string _CurrentQ = string.Empty;
        //string _NextQ = string.Empty;
        //  ddlQuarter.SelectedIndex = 1;
        //_CurrentQ = Session["currqtr"] + "";
        //string currentQuarter = Session["quarter"] + "";

        try
        {


            objCommand = new SqlCommand();
            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@MasterClientCode";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = customerCode;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@UserId";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = userid;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@Quarter";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = quarter;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@FYYR";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = year;




            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@Role";
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.VarChar;
            objParamStatus5.Value = role;




            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;




            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);

            objParamColl.Add(objParamStatus4);
            objParamColl.Add(objParamStatus5);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_Fetch_BEData_SDM_ImportExcel_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {

                return ds;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataSet();
    }

    public DataSet GetNotDMBEDataExcel(string NSO, string customerCode, string userid, string quarter, string year, string role)
    {


        DataSet ds = new DataSet();
        DataSet dsreturn = new DataSet();
        SqlCommand objCommand;
        List<DataTable> retTable = new List<DataTable>();
        string _CurrentQ = string.Empty;
        //string _NextQ = string.Empty;
        //  ddlQuarter.SelectedIndex = 1;
        //_CurrentQ = Session["currqtr"] + "";
        //string currentQuarter = Session["quarter"] + "";

        try
        {


            objCommand = new SqlCommand();
            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@MasterClientCode";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = customerCode;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@UserId";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = userid;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@Quarter";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = quarter;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@FYYR";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = year;

            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.ParameterName = "@NewOffering";
            objParamStatus.Direction = ParameterDirection.Input;
            objParamStatus.SqlDbType = SqlDbType.VarChar;
            objParamStatus.Value = NSO;


            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@Role";
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.VarChar;
            objParamStatus5.Value = role;




            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            objParamColl.Add(objParamStatus);

            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);

            objParamColl.Add(objParamStatus4);
            objParamColl.Add(objParamStatus5);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("[EAS_SP_Fetch_BEData_SDM_ImportExcel_NSO]", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {

                return ds;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataSet();
    }





    public DataTable GetFinpulseDetails(string userid, string mcc, string year, string su, string nso)
    {
        DataSet ds = new DataSet();
        SqlParameter objParm, objParm3, objParm4, objParm6, objParm5;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@userid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = userid;

            //objParm1 = new SqlParameter();

            //objParm1.ParameterName = "@SU";

            //objParm1.Direction = ParameterDirection.Input;

            //objParm1.SqlDbType = SqlDbType.NVarChar;

            //objParm1.Value = su;

            //objParm2 = new SqlParameter();

            //objParm2.ParameterName = "@YearMonth";

            //objParm2.Direction = ParameterDirection.Input;

            //objParm2.SqlDbType = SqlDbType.VarChar;

            //objParm2.Value = yearmonth;

            objParm3 = new SqlParameter();
            objParm3.ParameterName = "@mcc";
            objParm3.Direction = ParameterDirection.Input;
            objParm3.SqlDbType = SqlDbType.NVarChar;
            objParm3.Value = mcc;

            objParm4 = new SqlParameter();
            objParm4.ParameterName = "@txtYear";
            objParm4.Direction = ParameterDirection.Input;
            objParm4.SqlDbType = SqlDbType.VarChar;
            objParm4.Value = year;

            objParm5 = new SqlParameter();
            objParm5.ParameterName = "@su";
            objParm5.Direction = ParameterDirection.Input;
            objParm5.SqlDbType = SqlDbType.VarChar;
            objParm5.Value = su;

            objParm6 = new SqlParameter();
            objParm6.ParameterName = "@newOffering";
            objParm6.Direction = ParameterDirection.Input;
            objParm6.SqlDbType = SqlDbType.VarChar;
            objParm6.Value = nso;

            objCommand = new SqlCommand();

            objParamColl = objCommand.Parameters;

            objParamColl.AddRange(new SqlParameter[] { objParm, objParm3, objParm4, objParm5, objParm6 });

            objData = new DataAccess();

            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_BeFinData_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                return dt;
            }

        }

        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            throw;

        }

        finally
        {

            objData.CloseConnection();

        }



        return new DataTable();

    }




    public DataTable GetBETrendsReport(string qtr, string year, string pu, string sdm, string Acc, string dh)
    {
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);


            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtSDM";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = sdm;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@Accounts";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = Acc;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@DH";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = dh;
            objParamColl.Add(sqlparam6);
            //objCommand.CommandTimeout = 1000000000;



            objData.ExecuteSPTrends("dbo.spBETrendsTemp_Daily", ref  ds, objCommand);

            return ds.Tables[0]; ;


        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }


    public DataTable GetBEBITSVarianceReport()
    {
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            objData.ExecuteSP("dbo.spBEBITSVarianceTemp", ref  ds, objCommand);

            return ds.Tables[0]; ;

        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public void DeleteZeroBEDM(string ClientCode, string NativeCurrency, string DMMailId, string Qtr, string Year)
    {

        DataSet ds = new DataSet();
        SqlParameter objParmClienCode;
        SqlParameter objParmNativeCurrency;
        SqlParameter objParmDmMail;
        SqlParameter objParmQtr;
        SqlParameter objParmYear;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        try
        {
            objParmClienCode = new SqlParameter();
            objParmClienCode.ParameterName = "@ClientCode";
            objParmClienCode.Direction = ParameterDirection.Input;
            objParmClienCode.SqlDbType = SqlDbType.NVarChar;
            objParmClienCode.Value = ClientCode;

            objParmNativeCurrency = new SqlParameter();
            objParmNativeCurrency.ParameterName = "@NativeCurrency";
            objParmNativeCurrency.Direction = ParameterDirection.Input;
            objParmNativeCurrency.SqlDbType = SqlDbType.NVarChar;
            objParmNativeCurrency.Value = NativeCurrency;

            objParmDmMail = new SqlParameter();
            objParmDmMail.ParameterName = "@DMMailId";
            objParmDmMail.Direction = ParameterDirection.Input;
            objParmDmMail.SqlDbType = SqlDbType.NVarChar;
            objParmDmMail.Value = DMMailId;

            objParmQtr = new SqlParameter();
            objParmQtr.ParameterName = "@Qtr";
            objParmQtr.Direction = ParameterDirection.Input;
            objParmQtr.SqlDbType = SqlDbType.NVarChar;
            objParmQtr.Value = Qtr;

            objParmYear = new SqlParameter();
            objParmYear.ParameterName = "@Year";
            objParmYear.Direction = ParameterDirection.Input;
            objParmYear.SqlDbType = SqlDbType.NVarChar;
            objParmYear.Value = Year;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.AddRange(new SqlParameter[] { objParmClienCode, objParmNativeCurrency, objParmDmMail, objParmQtr, objParmYear });

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEDeleteZeroBEDM", ref ds, objCommand);
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }



    public void DeleteZeroBESDM(string ClientCode, string NativeCurrency, string SDMMailId, string Qtr, string Year)
    {

        DataSet ds = new DataSet();
        SqlParameter objParmClienCode;
        SqlParameter objParmNativeCurrency;
        SqlParameter objParmSDmMail;
        SqlParameter objParmQtr;
        SqlParameter objParmYear;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        try
        {
            objParmClienCode = new SqlParameter();
            objParmClienCode.ParameterName = "@ClientCode";
            objParmClienCode.Direction = ParameterDirection.Input;
            objParmClienCode.SqlDbType = SqlDbType.VarChar;
            objParmClienCode.Value = ClientCode;

            objParmNativeCurrency = new SqlParameter();
            objParmNativeCurrency.ParameterName = "@NativeCurrency";
            objParmNativeCurrency.Direction = ParameterDirection.Input;
            objParmNativeCurrency.SqlDbType = SqlDbType.NVarChar;
            objParmNativeCurrency.Value = NativeCurrency;

            objParmSDmMail = new SqlParameter();
            objParmSDmMail.ParameterName = "@SDMMailId";
            objParmSDmMail.Direction = ParameterDirection.Input;
            objParmSDmMail.SqlDbType = SqlDbType.VarChar;
            objParmSDmMail.Value = SDMMailId;

            objParmQtr = new SqlParameter();
            objParmQtr.ParameterName = "@Qtr";
            objParmQtr.Direction = ParameterDirection.Input;
            objParmQtr.SqlDbType = SqlDbType.NVarChar;
            objParmQtr.Value = Qtr;

            objParmYear = new SqlParameter();
            objParmYear.ParameterName = "@Year";
            objParmYear.Direction = ParameterDirection.Input;
            objParmYear.SqlDbType = SqlDbType.NVarChar;
            objParmYear.Value = Year;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.AddRange(new SqlParameter[] { objParmClienCode, objParmNativeCurrency, objParmSDmMail, objParmQtr, objParmYear });

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEDeleteZeroBESDM", ref ds, objCommand);
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    //public string GetReportCode(string userID)
    //{


    //    DataSet ds = new DataSet();
    //    SqlParameter objParm;
    //    SqlCommand objCommand;
    //    SqlParameterCollection objParamColl;
    //    string reportCode = string.Empty;

    //    try
    //    {
    //        objParm = new SqlParameter();
    //        objParm.ParameterName = "@userID";
    //        objParm.Direction = ParameterDirection.Input;
    //        objParm.SqlDbType = SqlDbType.VarChar;
    //        objParm.Value = userID;

    //        objCommand = new SqlCommand();
    //        objParamColl = objCommand.Parameters;
    //        objParamColl.Add(objParm);

    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("spBEGetReportCode", ref ds, objCommand);
    //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
    //        {
    //            reportCode = ds.Tables[0].Rows[0]["txtReportCode"] + "";
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }


    //    return reportCode;
    //}







    //   public DataTable GetAlconPBSData(string customerCode, string PU, string userid)
    //{


    //    DataSet ds = new DataSet();
    //    SqlParameter objParm, objParm1, objParam2;
    //    SqlCommand objCommand;
    //    SqlParameterCollection objParamColl;
    //    string reportCode = string.Empty;
    //    DataTable dtAlcon = null;
    //    try
    //    {
    //        objParm = new SqlParameter();
    //        objParm.ParameterName = "@customercode";
    //        objParm.Direction = ParameterDirection.Input;
    //        objParm.SqlDbType = SqlDbType.VarChar;
    //        objParm.Value = customerCode;

    //        objParm1 = new SqlParameter();
    //        objParm1.ParameterName = "@pu";
    //        objParm1.Direction = ParameterDirection.Input;
    //        objParm1.SqlDbType = SqlDbType.VarChar;
    //        objParm1.Value = PU;


    //        objParam2 = new SqlParameter();
    //        objParam2.ParameterName = "@userid";
    //        objParam2.Direction = ParameterDirection.Input;
    //        objParam2.SqlDbType = SqlDbType.VarChar;
    //        objParam2.Value = userid;


    //        objCommand = new SqlCommand();
    //        objParamColl = objCommand.Parameters;
    //        objParamColl.Add(objParm);
    //        objParamColl.Add(objParm1);
    //        objParamColl.Add(objParam2);
    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("spBeFetchAlconPBS", ref ds, objCommand);
    //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
    //        {
    //            dtAlcon = ds.Tables[0];
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }


    //    return dtAlcon;
    //}

    public List<string> GetAllBEfromPortfolio()
    {
        DataSet ds = new DataSet();
        List<string> allbes = new List<string>();


        try
        {


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spGetAllBE", ref ds);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataTable dt = new DataTable();
                    allbes.Add(ds.Tables[0].Rows[i]["txtBEType"] + "");

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allbes;

    }
    public List<string> GetAllBERegions()
    {
        DataSet ds = new DataSet();
        List<string> allbes = new List<string>();


        try
        {


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEGetAllRegions", ref ds);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataTable dt = new DataTable();
                    allbes.Add(ds.Tables[0].Rows[i]["txtRegion"] + "");

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allbes;

    }

    public List<string> GetAllBEYearMonthFin(string year)
    {
        DataSet ds = new DataSet();
        List<string> allbes = new List<string>();
        SqlCommand objCommand;


        try
        {


            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@txtYear";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = year;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;



            objParamColl.Add(objParamUserId);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_BEGetYearMonthsFin", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataTable dt = new DataTable();
                    allbes.Add(ds.Tables[0].Rows[i]["YearMonth"] + "");

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allbes;

    }



    public List<string> GetAllBEYearFin()
    {
        DataSet ds = new DataSet();
        List<string> allbes = new List<string>();


        try
        {


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_BEFinpulseGetYear", ref ds);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataTable dt = new DataTable();
                    allbes.Add(ds.Tables[0].Rows[i]["txtYear"] + "");

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allbes;

    }



    public List<string> GetAllFinYearForRTBR()
    {
        DataSet ds = new DataSet();
        List<string> allbes = new List<string>();


        try
        {


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEGetRTBRFinYear", ref ds);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataTable dt = new DataTable();
                    allbes.Add(ds.Tables[0].Rows[i]["Fin Year End"] + "");

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allbes;

    }




    //public List<KeyValues> GetAllUsers()
    //{
    //    DataSet ds = new DataSet();
    //    List<KeyValues> allUsers = new List<KeyValues>();


    //    try
    //    {

    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("spBEFetchUserList", ref ds);
    //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //                allUsers.Add(new KeyValues() { Key = ds.Tables[0].Rows[i]["txtUserId"] + "", Value = ds.Tables[0].Rows[i]["txtflag"] + "" });

    //    }
    //    catch (Exception ex)
    //    {
    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;

    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }


    //    return allUsers;

    //}
    public DataSet GetAllUsers(string userid)
    {
        DataSet ds = new DataSet();
        List<KeyValueValue> allUsers = new List<KeyValueValue>();
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        SqlCommand objCommand;

        try
        {

            objData = new DataAccess();


            objParm = new SqlParameter();
            objParm.ParameterName = "@userid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = userid;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);
            objData.GetConnection();
            objData.ExecuteSP("spBEFetchUserList_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                return ds;
            }

            //   allUsers.Add(new KeyValueValue() { Key = ds.Tables[0].Rows[i]["txtUserId"] + "", Value1 = ds.Tables[0].Rows[i]["txtflag"] + "", Value2 = ds.Tables[0].Rows[i]["txtdel"] + "" });
            //  allUsers.Add(ds.Tables[0].Rows[0][i].ToString());
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;

        }
        finally
        {
            objData.CloseConnection();
        }


        return ds;

    }

    public List<ApplnAccess> GetAccess(string userId)
    {
        DataSet ds = new DataSet();
        List<ApplnAccess> allport = new List<ApplnAccess>();
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        SqlCommand objCommand;
        try
        {

            objData = new DataAccess();

            objParm = new SqlParameter();
            objParm.ParameterName = "@txtuserid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = userId;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData.GetConnection();
            objData.ExecuteSP("spCheckAccessAppln", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    allport.Add(new ApplnAccess() { Appln = ds.Tables[0].Rows[i]["txtappln"] + "", Access = ds.Tables[0].Rows[i]["txtaccess"] + "" });
                    // allport.Add(ds.Tables[0].Rows[i]["txtPortfolio"] + "");

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allport;

    }

    //public List<string> GetAllPortfolio()
    //{
    //    DataSet ds = new DataSet();
    //    List<string> allport = new List<string>();


    //    try
    //    {

    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("spBEFetchAllPortfolio", ref ds);
    //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
    //        {
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {
    //                DataTable dt = new DataTable();
    //                allport.Add(ds.Tables[0].Rows[i]["txtPortfolio"] + "");

    //            }


    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }


    //    return allport;

    //}



    public List<RTBRFooter> GetRTBRFooter(string PU, string customerCode, string qtr, string year, string userid)
    {
        DataSet ds = new DataSet();
        List<RTBRFooter> RTBRCollection = new List<RTBRFooter>();
        SqlParameter objParm, objParm1, objparm2, objParm3, objParm4;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        RTBRFooter RTBR;
        try
        {

            objParm = new SqlParameter();
            objParm.ParameterName = "@PU";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = PU;

            objParm1 = new SqlParameter();
            objParm1.ParameterName = "@txtCustomerCode";
            objParm1.Direction = ParameterDirection.Input;
            objParm1.SqlDbType = SqlDbType.NVarChar;
            objParm1.Value = customerCode;

            objparm2 = new SqlParameter();
            objparm2.ParameterName = "@txtQuarterName";
            objparm2.Direction = ParameterDirection.Input;
            objparm2.SqlDbType = SqlDbType.NVarChar;
            objparm2.Value = qtr;

            objParm3 = new SqlParameter();
            objParm3.ParameterName = "@txtYear";
            objParm3.Direction = ParameterDirection.Input;
            objParm3.SqlDbType = SqlDbType.NVarChar;
            objParm3.Value = year;

            objParm4 = new SqlParameter();
            objParm4.ParameterName = "@txtUserId";
            objParm4.Direction = ParameterDirection.Input;
            objParm4.SqlDbType = SqlDbType.NVarChar;
            objParm4.Value = userid;



            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.AddRange(new SqlParameter[] { objParm, objParm1, objparm2, objParm3, objParm4 });

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEGetFooterTotal", ref ds, objCommand);


            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataTable dt = new DataTable();
                    RTBR = new RTBRFooter();
                    RTBR.ActualM1 = ds.Tables[0].Rows[i]["RTBR/FINM1"].ToString().Trim();
                    RTBR.ActualM2 = ds.Tables[0].Rows[i]["RTBR/FINM2"].ToString().Trim();
                    RTBR.ActualM3 = ds.Tables[0].Rows[i]["RTBR/FINM3"].ToString().Trim();
                    RTBR.totalActual = ds.Tables[0].Rows[i]["RTBR/FINMTotal"].ToString().Trim();
                    RTBRCollection.Add(RTBR);
                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return RTBRCollection;

    }

    public void DeletePortfolio(int AdminID)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@AdminId";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.Int;
            objParm.Value = AdminID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEDeletePortfolio", ref ds, objCommand);
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }


    public int updateAdminAllDUsList(string PU, string MasterCustomerCode, string Portfolio, string Anchors,
            string Region, string RevDMorSDM, string VolDMorSDM, int AdminNo, string BE)
    {

        //TODO:12/10 is all DMs commented
        //string _IsAllDU = (IsAllDU + string.Empty).ToLower() == "yes" ? "Y" : "N";
        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtPU";
            objParm.Direction = ParameterDirection.Input;
            objParm.Size = 50;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = PU;

            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@MasterCustomerCode";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.Size = 50;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = MasterCustomerCode;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@txtPortfolio";
            objParamStatus2.Size = 50;
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = Portfolio;

            //SqlParameter objParamStatus3 = new SqlParameter();
            //objParamStatus3.ParameterName = "@txtCurrency";
            //objParamStatus3.Direction = ParameterDirection.Input;
            //objParamStatus3.Size = 50;
            //objParamStatus3.SqlDbType = SqlDbType.VarChar;
            //objParamStatus3.Value = Currency;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@txtAnchors";

            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = Anchors;


            //SqlParameter objParamStatus5 = new SqlParameter();
            //objParamStatus5.ParameterName = "@IsAllDU";
            //objParamStatus5.Direction = ParameterDirection.Input;
            //objParamStatus5.Size = 10;
            //objParamStatus5.SqlDbType = SqlDbType.NChar;
            //objParamStatus5.Value = _IsAllDU;

            SqlParameter objParamStatus6 = new SqlParameter();
            objParamStatus6.ParameterName = "@txtRegion";
            objParamStatus6.Size = 50;
            objParamStatus6.Direction = ParameterDirection.Input;
            objParamStatus6.SqlDbType = SqlDbType.NVarChar;
            objParamStatus6.Value = Region;

            SqlParameter objParamStatus7 = new SqlParameter();
            objParamStatus7.ParameterName = "@RevDMorSDM";
            objParamStatus7.Size = 10;
            objParamStatus7.Direction = ParameterDirection.Input;
            objParamStatus7.SqlDbType = SqlDbType.NChar;
            objParamStatus7.Value = RevDMorSDM;

            SqlParameter objParamStatus8 = new SqlParameter();
            objParamStatus8.ParameterName = "@VolDMorSDM";
            objParamStatus8.Direction = ParameterDirection.Input;
            objParamStatus8.Size = 10;
            objParamStatus8.SqlDbType = SqlDbType.NChar;
            objParamStatus8.Value = VolDMorSDM;

            SqlParameter objParamStatus9 = new SqlParameter();
            objParamStatus9.ParameterName = "@intAdminNo";
            objParamStatus9.Direction = ParameterDirection.Input;
            objParamStatus9.SqlDbType = SqlDbType.Int;
            objParamStatus9.Value = AdminNo;

            SqlParameter objParamStatus11 = new SqlParameter();
            objParamStatus11.ParameterName = "@BE";
            objParamStatus11.Direction = ParameterDirection.Input;
            objParamStatus11.SqlDbType = SqlDbType.VarChar;
            objParamStatus11.Value = BE;

            SqlParameter objParamStatus10 = new SqlParameter();
            objParamStatus10.Direction = ParameterDirection.ReturnValue;
            objParamStatus10.SqlDbType = SqlDbType.Int;
            objParamStatus10.ParameterName = "ReturnValue";


            //SqlParameter objParamStatus11 = new SqlParameter();
            //objParamStatus11.ParameterName = "@txtDMMailId";
            //objParamStatus11.Direction = ParameterDirection.Input;
            //objParamStatus11.SqlDbType = SqlDbType.VarChar;
            //objParamStatus11.Value = txtDMMailId;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            //objParamColl.AddRange(new SqlParameter[]{ objParm,objParamStatus1,objParamStatus2,objParamStatus3,objParamStatus4,objParamStatus5,objParamStatus6,objParamStatus7,objParamStatus8,
            //                    objParamStatus9,objParamStatus10});


            //objParamColl.AddRange(new SqlParameter[]{ objParm,objParamStatus1,objParamStatus2,objParamStatus3,objParamStatus4,objParamStatus5,objParamStatus6,objParamStatus7,objParamStatus8,
            //                    objParamStatus9,objParamStatus10,objParamStatus11});


            objParamColl.AddRange(new SqlParameter[]{ objParm,objParamStatus1,objParamStatus2,objParamStatus4,objParamStatus6,objParamStatus7,objParamStatus8,
                                objParamStatus9,objParamStatus10,objParamStatus11});
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEUpdatePortfolio", ref ds, objCommand);

            int updatestatus = (int)objCommand.Parameters["ReturnValue"].Value;
            return updatestatus;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {

            objData.CloseConnection();
        }




    }






    public List<AdminAllDU> getAdminAllDUsList(string portfolio, string userID, string be)
    {
        //string userid = Session["UserID"] + "";
        DataSet ds = new DataSet();
        SqlParameter objParm, objParmbe, objparamuser;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        List<AdminAllDU> lstempCollection = new List<AdminAllDU>();
        List<DUPUCCMap> lstDUPU = new List<DUPUCCMap>();
        DUPUCCMap puobj = new DUPUCCMap();
        AdminAllDU empCollection;

        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtPortfolio";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = portfolio;

            objParmbe = new SqlParameter();
            objParmbe.ParameterName = "@txtBEtype";
            objParmbe.Direction = ParameterDirection.Input;
            objParmbe.SqlDbType = SqlDbType.VarChar;

            objparamuser = new SqlParameter();
            objparamuser.ParameterName = "@txtUserId";
            objparamuser.Direction = ParameterDirection.Input;
            objparamuser.SqlDbType = SqlDbType.VarChar;
            objparamuser.Value = userID;

            if (be.ToLowerTrim() == "")
                objParmbe.Value = "none";
            else
                objParmbe.Value = be;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);
            objParamColl.Add(objParmbe);
            objParamColl.Add(objparamuser);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEfetchAdminAllDUs", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new AdminAllDU();

                    empCollection.portFolio = ds.Tables[0].Rows[i]["txtPortFolio"].ToString().Trim();
                    empCollection.txtPU = ds.Tables[0].Rows[i]["txtPU"].ToString().Trim();
                    empCollection.masterCustomerCode = ds.Tables[0].Rows[i]["txtMasterCustomerCode"].ToString().Trim();
                    empCollection.Region = ds.Tables[0].Rows[i]["txtRegion"].ToString().Trim();
                    // empCollection.Currency = ds.Tables[0].Rows[i]["txtCurrency"].ToString().Trim();


                    // empCollection.isAllDU = ds.Tables[0].Rows[i]["IsAllDU"] == DBNull.Value ? "No" : ds.Tables[0].Rows[i]["IsAllDU"].ToString().Trim() == null ? "No" : ds.Tables[0].Rows[i]["IsAllDU"].ToString().Trim() == "Y" ? "Yes" : "No";
                    // empCollection.txtDMMailId = ds.Tables[0].Rows[i]["txtDMMailId"].ToString().Trim();

                    empCollection.RevDMorSDM = ds.Tables[0].Rows[i]["RevDMorSDM"] == DBNull.Value ? "SDM" : ds.Tables[0].Rows[i]["RevDMorSDM"].ToString().Trim() == null ? "SDM" : ds.Tables[0].Rows[i]["RevDMorSDM"].ToString().Trim();
                    empCollection.VolDMorSDM = ds.Tables[0].Rows[i]["VolDMorSDM"] == DBNull.Value ? "SDM" : ds.Tables[0].Rows[i]["VolDMorSDM"].ToString().Trim() == null ? "SDM" : ds.Tables[0].Rows[i]["VolDMorSDM"].ToString().Trim();
                    empCollection.lstportFolio = GetAllPortfolio(userID);
                    //empCollection.lstCurrency = GetAllCurrency();
                    empCollection.anchors = ds.Tables[0].Rows[i]["txtAnchors"].ToString().Trim();
                    empCollection.AdminNo = Convert.ToInt32(ds.Tables[0].Rows[i]["intAdminNo"].ToString().Trim());
                    empCollection.Betype = ds.Tables[0].Rows[i]["txtBEtype"] == DBNull.Value ? "" : ds.Tables[0].Rows[i]["txtBEtype"].ToString().Trim() == null ? "" : ds.Tables[0].Rows[i]["txtBEtype"].ToString().Trim();
                    empCollection.updatedBy = ds.Tables[0].Rows[i]["txtUpdatedBy"] == DBNull.Value ? "" : ds.Tables[0].Rows[i]["txtUpdatedBy"].ToString().Trim() == null ? "" : ds.Tables[0].Rows[i]["txtUpdatedBy"].ToString().Trim();
                    empCollection.lstPU = GetAllPUs(userID);
                    empCollection.lstBE = GetAllBEfromPortfolio();
                    lstempCollection.Add(empCollection);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }


    public List<string> GetAllCurrency()
    {





        DataSet ds = new DataSet();



        List<string> lstempCollection = new List<string>();

        try
        {


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEFetchCurrencyList", ref ds);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    string tmp = ds.Tables[0].Rows[i]["txtCurrency"].ToString().Trim();

                    lstempCollection.Add(tmp);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;






    }


    public List<BEAdminUI> GetBEPUMapping()
    {

        DataSet ds = new DataSet();
        //SqlParameter objParm;
        SqlCommand objCommand;
        //SqlParameterCollection objParamColl;
        List<BEAdminUI> lstempCollection = new List<BEAdminUI>();
        BEAdminUI empCollection;
        try
        {
            //objParm = new SqlParameter();
            //objParm.ParameterName = "@txtUserId";
            //objParm.Direction = ParameterDirection.Input;
            //objParm.SqlDbType = SqlDbType.VarChar;
            //objParm.Value = userID;

            objCommand = new SqlCommand();
            //objParamColl = objCommand.Parameters;
            //objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spGetAllPUFORBE", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new BEAdminUI();

                    // empCollection.DU = ds.Tables[0].Rows[i]["txtDU"].ToString();
                    empCollection.PU = ds.Tables[0].Rows[i]["txtEmpPU"].ToString();
                    // empCollection.CustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                    lstempCollection.Add(empCollection);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }


    public List<BEAdminUI> GetBEPUMappingforDH(string userid)
    {

        DataSet ds = new DataSet();
        //SqlParameter objParm;
        SqlCommand objCommand;
        //SqlParameterCollection objParamColl;
        List<BEAdminUI> lstempCollection = new List<BEAdminUI>();
        BEAdminUI empCollection;
        try
        {
            //objParm = new SqlParameter();
            //objParm.ParameterName = "@txtUserId";
            //objParm.Direction = ParameterDirection.Input;
            //objParm.SqlDbType = SqlDbType.VarChar;
            //objParm.Value = userID;

            objCommand = new SqlCommand();
            //objParamColl = objCommand.Parameters;
            //objParamColl.Add(objParm);
            SqlParameterCollection objParamColl;
            SqlParameter objParmUserId;

            objParmUserId = new SqlParameter();
            objParmUserId.ParameterName = "@txtUserId";
            objParmUserId.Direction = ParameterDirection.Input;
            objParmUserId.SqlDbType = SqlDbType.NVarChar;
            objParmUserId.Value = userid;
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParmUserId);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEGetPUFORDH", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new BEAdminUI();
                    // empCollection.DU = ds.Tables[0].Rows[i]["txtDU"].ToString();
                    empCollection.PU = ds.Tables[0].Rows[i]["txtPU"].ToString();
                    // empCollection.CustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();
                    lstempCollection.Add(empCollection);
                }
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }

    //Ganapathy 03 july
    //public BEAdminUI GetBEAdminList(string userID)
    //{

    //    BEAdminUI beui = null;

    //    DataSet obj = new DataSet();
    //    SqlCommand objCommand;
    //    SqlParameterCollection objParamColl;
    //    SqlParameter objParmUserId;

    //    try
    //    {
    //        objParmUserId = new SqlParameter();
    //        objParmUserId.ParameterName = "@txtUserId";
    //        objParmUserId.Direction = ParameterDirection.Input;
    //        objParmUserId.SqlDbType = SqlDbType.VarChar;
    //        objParmUserId.Value = userID;
    //        objCommand = new SqlCommand();
    //        objParamColl = objCommand.Parameters;
    //        objParamColl.Add(objParmUserId);

    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("dbo.spGetBEAdminUserAccess", ref obj, objCommand);
    //        if (obj != null && obj.Tables != null && obj.Tables.Count > 0)
    //        {
    //            if (obj.Tables[0] != null && obj.Tables[0].Rows.Count > 0)
    //            {
    //                //beui = new BEAdminUI();
    //                beui = new BEAdminUI();

    //                beui.PU = obj.Tables[0].Rows[0]["txtPU"].ToString();
    //                beui.Role = obj.Tables[0].Rows[0]["txtRole"].ToString();
    //                beui.UserId = obj.Tables[0].Rows[0]["txtUserId"].ToString();

    //                if (obj.Tables[0] != null && obj.Tables[0].Rows.Count > 0)
    //                {

    //                    List<string> lstTemp = new List<string>();
    //                    //string[] DUList = new string[];
    //                    for (int j = 0; j < obj.Tables[0].Rows.Count; j++)
    //                    {
    //                        string DUList = string.Empty;
    //                        if (j == 0)
    //                            DUList = obj.Tables[0].Rows[j]["txtDMMailId"].ToString();
    //                        else
    //                            DUList = DUList + obj.Tables[0].Rows[j]["txtDMMailId"].ToString();

    //                        lstTemp.Add(DUList);
    //                    }
    //                    beui.DuList = lstTemp.ToArray();

    //                }


    //            }
    //            //if (obj.Tables[1] != null && obj.Tables[1].Rows.Count > 0)
    //            //{

    //            //    List<string> lstTemp1 = new List<string>();
    //            //    //string[] DUList = new string[];
    //            //    for (int j = 0; j < obj.Tables[1].Rows.Count; j++)
    //            //    {
    //            //        string ClientCodeList = string.Empty;
    //            //        if (j == 0)
    //            //            ClientCodeList = obj.Tables[1].Rows[j]["txtMasterClientCode"].ToString().ToUpper();
    //            //        else
    //            //            ClientCodeList = ClientCodeList + obj.Tables[1].Rows[j]["txtMasterClientCode"].ToString().ToUpper();

    //            //        lstTemp1.Add(ClientCodeList);
    //            //    }
    //            //    beui.ClientCodeList = lstTemp1.ToArray();

    //            //}
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }
    //    return beui;
    //}


    //ganapathy July16

    //public BEAdminUI GetBEAdminList(string userID)
    //{

    //    BEAdminUI beui = null;

    //    DataSet obj = new DataSet();
    //    SqlCommand objCommand;
    //    SqlParameterCollection objParamColl;
    //    SqlParameter objParmUserId;

    //    try
    //    {
    //        objParmUserId = new SqlParameter();
    //        objParmUserId.ParameterName = "@txtUserId";
    //        objParmUserId.Direction = ParameterDirection.Input;
    //        objParmUserId.SqlDbType = SqlDbType.VarChar;
    //        objParmUserId.Value = userID;
    //        objCommand = new SqlCommand();
    //        objParamColl = objCommand.Parameters;
    //        objParamColl.Add(objParmUserId);

    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("dbo.spGetBEAdminUserAccess", ref obj, objCommand);
    //        if (obj != null && obj.Tables != null && obj.Tables.Count > 0)
    //        {
    //            if (obj.Tables[0] != null && obj.Tables[0].Rows.Count > 0)
    //            {
    //                //beui = new BEAdminUI();
    //                beui = new BEAdminUI();

    //                beui.PU = obj.Tables[0].Rows[0]["txtPU"].ToString();
    //                beui.Role = obj.Tables[0].Rows[0]["txtRole"].ToString();
    //                beui.UserId = obj.Tables[0].Rows[0]["txtUserId"].ToString();
    //                beui.IsAdmin = obj.Tables[0].Rows[0]["isAdmin"].ToString();

    //                if (obj.Tables[0] != null && obj.Tables[0].Rows.Count > 0)
    //                {

    //                    List<string> lstTemp = new List<string>();
    //                    //string[] DUList = new string[];
    //                    for (int j = 0; j < obj.Tables[0].Rows.Count; j++)
    //                    {
    //                        string DUList = string.Empty;
    //                        if (j == 0)
    //                            DUList = obj.Tables[0].Rows[j]["txtDMMailId"].ToString();
    //                        else
    //                            DUList = DUList + obj.Tables[0].Rows[j]["txtDMMailId"].ToString();

    //                        lstTemp.Add(DUList);
    //                    }
    //                    beui.DuList = lstTemp.ToArray();

    //                }

    //                if (obj.Tables[0] != null && obj.Tables[0].Rows.Count > 0)
    //                {

    //                    List<string> lstRepTemp = new List<string>();
    //                    //string[] DUList = new string[];
    //                    for (int j = 0; j < obj.Tables[0].Rows.Count; j++)
    //                    {
    //                        string ReportCodeList = string.Empty;
    //                        if (j == 0)
    //                            ReportCodeList = obj.Tables[0].Rows[j]["txtReportCode"].ToString();
    //                        else
    //                            ReportCodeList = ReportCodeList + obj.Tables[0].Rows[j]["txtReportCode"].ToString();

    //                        lstRepTemp.Add(ReportCodeList);
    //                    }
    //                    beui.ReportCodeList = lstRepTemp.ToArray();

    //                }


    //            }
    //            //if (obj.Tables[1] != null && obj.Tables[1].Rows.Count > 0)
    //            //{

    //            //    List<string> lstTemp1 = new List<string>();
    //            //    //string[] DUList = new string[];
    //            //    for (int j = 0; j < obj.Tables[1].Rows.Count; j++)
    //            //    {
    //            //        string ClientCodeList = string.Empty;
    //            //        if (j == 0)
    //            //            ClientCodeList = obj.Tables[1].Rows[j]["txtMasterClientCode"].ToString().ToUpper();
    //            //        else
    //            //            ClientCodeList = ClientCodeList + obj.Tables[1].Rows[j]["txtMasterClientCode"].ToString().ToUpper();

    //            //        lstTemp1.Add(ClientCodeList);
    //            //    }
    //            //    beui.ClientCodeList = lstTemp1.ToArray();

    //            //}
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }
    //    return beui;
    //}


    public BEAdminUI GetBEAdminList(string userID, string loggeduserid, out int ret)
    {

        BEAdminUI beui = null;

        DataSet obj = new DataSet();
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        SqlParameter objParmUserId, objParmUserId1, objParmUserId2;

        try
        {
            objParmUserId = new SqlParameter();
            objParmUserId.ParameterName = "@txtUserId";
            objParmUserId.Direction = ParameterDirection.Input;
            objParmUserId.SqlDbType = SqlDbType.VarChar;
            objParmUserId.Value = userID;
            //objCommand = new SqlCommand();
            //objParamColl = objCommand.Parameters;



            objParmUserId1 = new SqlParameter();
            objParmUserId1.ParameterName = "@txtloggeduser";
            objParmUserId1.Direction = ParameterDirection.Input;
            objParmUserId1.SqlDbType = SqlDbType.VarChar;
            objParmUserId1.Value = loggeduserid;



            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParmUserId);
            objParamColl.Add(objParmUserId1);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spGetBEAdminUserAccess", ref obj, objCommand);

            if (obj != null && obj.Tables != null && obj.Tables.Count > 0)
            {
                if (obj.Tables[0] != null && obj.Tables[0].Rows.Count > 0)
                {
                    //beui = new BEAdminUI();
                    beui = new BEAdminUI();

                    beui.PU = obj.Tables[0].Rows[0]["txtPU"].ToString();
                    beui.Role = obj.Tables[0].Rows[0]["txtRole"].ToString();
                    beui.UserId = obj.Tables[0].Rows[0]["txtUserId"].ToString();
                    beui.IsAdmin = obj.Tables[0].Rows[0]["isAdmin"].ToString();

                    if (obj.Tables[0] != null && obj.Tables[0].Rows.Count > 0)
                    {

                        List<string> lstTemp = new List<string>();
                        //string[] DUList = new string[];
                        for (int j = 0; j < obj.Tables[0].Rows.Count; j++)
                        {
                            string DUList = string.Empty;
                            if (j == 0)
                                DUList = obj.Tables[0].Rows[j]["txtDMMailId"].ToString();
                            else
                                DUList = DUList + obj.Tables[0].Rows[j]["txtDMMailId"].ToString();

                            lstTemp.Add(DUList);
                        }
                        beui.DuList = lstTemp.ToArray();

                    }

                    if (obj.Tables[0] != null && obj.Tables[0].Rows.Count > 0)
                    {

                        List<string> lstRepTemp = new List<string>();
                        //string[] DUList = new string[];
                        for (int j = 0; j < obj.Tables[0].Rows.Count; j++)
                        {
                            string ReportCodeList = string.Empty;
                            if (j == 0)
                                ReportCodeList = obj.Tables[0].Rows[j]["txtReportCode"].ToString();
                            else
                                ReportCodeList = ReportCodeList + obj.Tables[0].Rows[j]["txtReportCode"].ToString();

                            lstRepTemp.Add(ReportCodeList);
                        }
                        beui.ReportCodeList = lstRepTemp.ToArray();

                    }




                }

                //if (obj.Tables[1] != null && obj.Tables[1].Rows.Count > 0)
                //{

                //    List<string> lstTemp1 = new List<string>();
                //    //string[] DUList = new string[];
                //    for (int j = 0; j < obj.Tables[1].Rows.Count; j++)
                //    {
                //        string ClientCodeList = string.Empty;
                //        if (j == 0)
                //            ClientCodeList = obj.Tables[1].Rows[j]["txtMasterClientCode"].ToString().ToUpper();
                //        else
                //            ClientCodeList = ClientCodeList + obj.Tables[1].Rows[j]["txtMasterClientCode"].ToString().ToUpper();

                //        lstTemp1.Add(ClientCodeList);
                //    }
                //    beui.ClientCodeList = lstTemp1.ToArray();

                //}
            }

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        ret = Convert.ToInt32(obj.Tables[1].Rows[0]["ret"]);
        return beui;
    }

    public string[] GetAllMenuCodes()
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<string> lstempCollection = new List<string>();

        try
        {

            objCommand = new SqlCommand();

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEGetAllMenuCodes", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string ReportCode = ds.Tables[0].Rows[i]["txtMenuCode"].ToString().Trim() + "|" + ds.Tables[0].Rows[i]["txtMenuName"].ToString().Trim();
                    //string ReportCode = ds.Tables[0].Rows[i]["ReportName"].ToString().Trim();
                    lstempCollection.Add(ReportCode);
                }

            }
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection.ToArray();
    }

    public BEAdminMenuUI GetBEAdminMenuList(string Role)
    {

        BEAdminMenuUI beui = null;

        DataSet obj = new DataSet();
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        SqlParameter objParmUserId;

        try
        {
            objParmUserId = new SqlParameter();
            objParmUserId.ParameterName = "@Role";
            objParmUserId.Direction = ParameterDirection.Input;
            objParmUserId.SqlDbType = SqlDbType.VarChar;
            objParmUserId.Value = Role;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParmUserId);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spGetBEAdminMenuUserAccess", ref obj, objCommand);
            if (obj != null && obj.Tables != null && obj.Tables.Count > 0)
            {
                if (obj.Tables[0] != null && obj.Tables[0].Rows.Count > 0)
                {
                    //beui = new BEAdminUI();
                    beui = new BEAdminMenuUI();


                    beui.Role = obj.Tables[0].Rows[0]["Role"].ToString();


                    if (obj.Tables[0] != null && obj.Tables[0].Rows.Count > 0)
                    {

                        List<string> lstRepTemp = new List<string>();
                        //string[] DUList = new string[];
                        for (int j = 0; j < obj.Tables[0].Rows.Count; j++)
                        {
                            string ReportCodeList = string.Empty;
                            if (j == 0)
                                ReportCodeList = obj.Tables[0].Rows[j]["MenuCode"].ToString();
                            else
                                ReportCodeList = ReportCodeList + obj.Tables[0].Rows[j]["MenuCode"].ToString();

                            lstRepTemp.Add(ReportCodeList);
                        }

                        //if (lstRepTemp.Contains("A004"))
                        //{
                        //    lstRepTemp.Remove("A004");
                        //    if (lstRepTemp.Contains("A04"))
                        //    {
                        //        lstRepTemp.Remove("A04");
                        //    }
                        //}
                        beui.ReportCodeList = lstRepTemp.ToArray();

                    }

                }
            }

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return beui;
    }

    public void InsertMenuUserAccess(BEAdminMenuUI objAccess, string repcode)
    {

        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        // SqlParameter objParmUserId;
        SqlParameter objParmRole;
        SqlParameter objParmReportCodes;

        try
        {

            //objParmUserId = new SqlParameter();
            //objParmUserId.ParameterName = "@txtUserId";
            //objParmUserId.Direction = ParameterDirection.Input;
            //objParmUserId.SqlDbType = SqlDbType.VarChar;
            //objParmUserId.Value = objAccess.UserId;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            //objParamColl.Add(objParmUserId);


            objParmRole = new SqlParameter();
            objParmRole.ParameterName = "@txtRole";
            objParmRole.Direction = ParameterDirection.Input;
            objParmRole.SqlDbType = SqlDbType.VarChar;
            objParmRole.Value = objAccess.Role;
            objParamColl.Add(objParmRole);


            objParmReportCodes = new SqlParameter();
            objParmReportCodes.ParameterName = "@txtMenuCode";
            objParmReportCodes.Direction = ParameterDirection.Input;
            objParmReportCodes.SqlDbType = SqlDbType.VarChar;
            objParmReportCodes.Value = repcode;
            //objParmReportCodes.Value = objAccess.ReportCodeList;
            objParamColl.Add(objParmReportCodes);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spInsertBEMenuUserAccess", objCommand);

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public string GetIsAdmin(string userID)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;


        try
        {


            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@txtUserId";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userID;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;



            objParamColl.Add(objParamUserId);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBeIsAdmin_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                if (ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0].Rows[0]["isAdmin"] + "";
                else
                    return "N";



            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return "N";
    }

    /// <summary>
    /// to get all the portfolio  based on the su 
    /// portfolio screen
    /// </summary>
    /// <returns></returns>
    public List<string> GetPortfolioFromSU(string su)
    {
        DataSet ds = new DataSet();
        List<string> allpus = new List<string>();

        SqlCommand objCommand;
        try
        {

            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@txtsu";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = su;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;



            objParamColl.Add(objParamUserId);
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEGetPortfolioFromSU", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataTable dt = new DataTable();
                    allpus.Add(ds.Tables[0].Rows[i]["txtPortfolio"] + "");

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allpus;

    }

    public BEClientAdminUI GetBEClientAdminList(string userID)
    {

        BEClientAdminUI beclientui = null;

        DataSet obj = new DataSet();
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        SqlParameter objParmUserId;

        try
        {
            objParmUserId = new SqlParameter();
            objParmUserId.ParameterName = "@txtUserId";
            objParmUserId.Direction = ParameterDirection.Input;
            objParmUserId.SqlDbType = SqlDbType.VarChar;
            objParmUserId.Value = userID;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParmUserId);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spGetBEClientAdminUserAccess", ref obj, objCommand);
            if (obj != null && obj.Tables != null && obj.Tables.Count > 0)
            {
                if (obj.Tables[0] != null && obj.Tables[0].Rows.Count > 0)
                {
                    //beui = new BEAdminUI();
                    beclientui = new BEClientAdminUI();

                    beclientui.PU = obj.Tables[0].Rows[0]["txtPU"].ToString();
                    //beclientui.Role = obj.Tables[0].Rows[0]["txtRole"].ToString();


                    if (obj.Tables[0] != null && obj.Tables[0].Rows.Count > 0)
                    {

                        List<string> lstTemp = new List<string>();
                        //string[] DUList = new string[];
                        for (int j = 0; j < obj.Tables[0].Rows.Count; j++)
                        {
                            string ClientList = string.Empty;
                            if (j == 0)
                                ClientList = obj.Tables[0].Rows[j]["txtMasterClientCode"].ToString().ToUpper();
                            else
                                ClientList = ClientList + obj.Tables[0].Rows[j]["txtMasterClientCode"].ToString().ToUpper();

                            lstTemp.Add(ClientList);
                        }
                        beclientui.ClientCodeList = lstTemp.ToArray();

                    }

                }

            }

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return beclientui;
    }



    //Ganapathy July 04 2012
    public bool DeleteBEUserAccess(string userId)
    {
        bool isDataExist = false;
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        SqlParameter objParmUserId;

        try
        {

            objParmUserId = new SqlParameter();
            objParmUserId.ParameterName = "@txtUserId";
            objParmUserId.Direction = ParameterDirection.Input;
            objParmUserId.SqlDbType = SqlDbType.VarChar;
            objParmUserId.Value = userId;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParmUserId);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.EAS_SP_DeleteBEUserAccess_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                int count = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL"].ToString());
                if (count > 0)
                    isDataExist = true;
            }

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return isDataExist;
    }


    //ganapathy July16
    public bool DeleteBEClientUserAccess(string userId)
    {
        bool isDataExist = false;
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        SqlParameter objParmUserId;

        try
        {

            objParmUserId = new SqlParameter();
            objParmUserId.ParameterName = "@txtUserId";
            objParmUserId.Direction = ParameterDirection.Input;
            objParmUserId.SqlDbType = SqlDbType.VarChar;
            objParmUserId.Value = userId;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParmUserId);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.EAS_SP_DeleteBEClientUserAccess_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                int count = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL"].ToString());
                if (count > 0)
                    isDataExist = true;
            }

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return isDataExist;
    }

    //public void InsertUserAccess(BEAdminUI objAccess, string du)
    //{

    //    SqlCommand objCommand;
    //    SqlParameterCollection objParamColl;
    //    SqlParameter objParmPU;
    //    SqlParameter objParmDuList;
    //    SqlParameter objParmUserId;
    //    SqlParameter objParmRole;

    //    try
    //    {

    //        objParmUserId = new SqlParameter();
    //        objParmUserId.ParameterName = "@txtUserId";
    //        objParmUserId.Direction = ParameterDirection.Input;
    //        objParmUserId.SqlDbType = SqlDbType.VarChar;
    //        objParmUserId.Value = objAccess.UserId;
    //        objCommand = new SqlCommand();
    //        objParamColl = objCommand.Parameters;
    //        objParamColl.Add(objParmUserId);

    //        objParmPU = new SqlParameter();
    //        objParmPU.ParameterName = "@txtPU";
    //        objParmPU.Direction = ParameterDirection.Input;
    //        objParmPU.SqlDbType = SqlDbType.VarChar;
    //        objParmPU.Value = objAccess.PU;
    //        objParamColl.Add(objParmPU);

    //        objParmDuList = new SqlParameter();
    //        objParmDuList.ParameterName = "@txtDUList";
    //        objParmDuList.Direction = ParameterDirection.Input;
    //        objParmDuList.SqlDbType = SqlDbType.VarChar;
    //        objParmDuList.Value = du;
    //        objParamColl.Add(objParmDuList);

    //        objParmRole = new SqlParameter();
    //        objParmRole.ParameterName = "@txtRole";
    //        objParmRole.Direction = ParameterDirection.Input;
    //        objParmRole.SqlDbType = SqlDbType.VarChar;
    //        objParmRole.Value = objAccess.Role;
    //        objParamColl.Add(objParmRole);

    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("dbo.spInsertBEUserAccess", objCommand);

    //    }
    //    catch (Exception ex)
    //    {
    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }
    //}



    //Ganapathy July13

    //public void InsertUserAccess(BEAdminUI objAccess, string du, string repcode)
    //{

    //    SqlCommand objCommand;
    //    SqlParameterCollection objParamColl;
    //    SqlParameter objParmPU;
    //    SqlParameter objParmDuList;
    //    SqlParameter objParmUserId;
    //    SqlParameter objParmRole;
    //    SqlParameter objParmReportCodes;

    //    try
    //    {

    //        objParmUserId = new SqlParameter();
    //        objParmUserId.ParameterName = "@txtUserId";
    //        objParmUserId.Direction = ParameterDirection.Input;
    //        objParmUserId.SqlDbType = SqlDbType.VarChar;
    //        objParmUserId.Value = objAccess.UserId;
    //        objCommand = new SqlCommand();
    //        objParamColl = objCommand.Parameters;
    //        objParamColl.Add(objParmUserId);

    //        objParmPU = new SqlParameter();
    //        objParmPU.ParameterName = "@txtPU";
    //        objParmPU.Direction = ParameterDirection.Input;
    //        objParmPU.SqlDbType = SqlDbType.VarChar;
    //        objParmPU.Value = objAccess.PU;
    //        objParamColl.Add(objParmPU);

    //        objParmDuList = new SqlParameter();
    //        objParmDuList.ParameterName = "@txtDUList";
    //        objParmDuList.Direction = ParameterDirection.Input;
    //        objParmDuList.SqlDbType = SqlDbType.VarChar;
    //        objParmDuList.Value = du;
    //        objParamColl.Add(objParmDuList);

    //        objParmRole = new SqlParameter();
    //        objParmRole.ParameterName = "@txtRole";
    //        objParmRole.Direction = ParameterDirection.Input;
    //        objParmRole.SqlDbType = SqlDbType.VarChar;
    //        objParmRole.Value = objAccess.Role;
    //        objParamColl.Add(objParmRole);


    //        objParmReportCodes = new SqlParameter();
    //        objParmReportCodes.ParameterName = "@txtReportCode";
    //        objParmReportCodes.Direction = ParameterDirection.Input;
    //        objParmReportCodes.SqlDbType = SqlDbType.VarChar;
    //        objParmReportCodes.Value = repcode;
    //        //objParmReportCodes.Value = objAccess.ReportCodeList;
    //        objParamColl.Add(objParmReportCodes);

    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("dbo.spInsertBEUserAccess", objCommand);

    //    }
    //    catch (Exception ex)
    //    {
    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }
    //}


    public string[] GetAllReportCodes(string userid)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<string> lstempCollection = new List<string>();

        SqlParameter objParm;
        SqlParameterCollection objParamColl;

        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtuserid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = userid;

            objCommand = new SqlCommand();

            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);




            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEGetAllReportCodes", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string ReportCode = ds.Tables[0].Rows[i]["ReportCode"].ToString().Trim() + "|" + ds.Tables[0].Rows[i]["ReportName"].ToString().Trim();
                    //string ReportCode = ds.Tables[0].Rows[i]["ReportName"].ToString().Trim();
                    lstempCollection.Add(ReportCode);
                }

            }
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection.ToArray();
    }

    //public string[] GetAllReportCodes()
    //{
    //    DataSet ds = new DataSet();
    //    SqlCommand objCommand;
    //    List<string> lstempCollection = new List<string>();



    //    try
    //    {

    //        objCommand = new SqlCommand();

    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("dbo.spBEGetAllReportCodes", ref ds, objCommand);
    //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
    //        {
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {
    //                string ReportCode = ds.Tables[0].Rows[i]["ReportCode"].ToString().Trim() + "|" + ds.Tables[0].Rows[i]["ReportName"].ToString().Trim();
    //                //string ReportCode = ds.Tables[0].Rows[i]["ReportName"].ToString().Trim();
    //                lstempCollection.Add(ReportCode);
    //            }

    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        throw e;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }
    //    return lstempCollection.ToArray();
    //}


    public void InsertClientCodeList(BEClientAdminUI objAccess, string ClientCode)
    {

        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        SqlParameter objParmPU;
        SqlParameter objParmClientCodeList;
        SqlParameter objParmUserId;


        try
        {

            objParmUserId = new SqlParameter();
            objParmUserId.ParameterName = "@txtUserId";
            objParmUserId.Direction = ParameterDirection.Input;
            objParmUserId.SqlDbType = SqlDbType.VarChar;
            objParmUserId.Value = objAccess.UserId;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParmUserId);

            objParmPU = new SqlParameter();
            objParmPU.ParameterName = "@txtPU";
            objParmPU.Direction = ParameterDirection.Input;
            objParmPU.SqlDbType = SqlDbType.VarChar;
            objParmPU.Value = objAccess.PU;
            objParamColl.Add(objParmPU);

            objParmClientCodeList = new SqlParameter();
            objParmClientCodeList.ParameterName = "@txtMasterClientCode";
            objParmClientCodeList.Direction = ParameterDirection.Input;
            objParmClientCodeList.SqlDbType = SqlDbType.VarChar;
            objParmClientCodeList.Value = ClientCode;
            objParamColl.Add(objParmClientCodeList);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spInsertBEClientCode", objCommand);

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }


    //Ganapathy 04july
    public string[] GetBEDUForPU(string PU)
    {
        DataSet ds = new DataSet();

        SqlCommand objCommand;

        List<string> lstempCollection = new List<string>();
        //lstempCollection.Add("ALL");
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtPU";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = PU;

            objCommand = new SqlCommand();

            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.SpBEGetDUForPU", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string du = ds.Tables[0].Rows[i]["txtDU"].ToString();
                    lstempCollection.Add(du);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection.ToArray();
    }

    //TODO:21/9 For DMMailid function changed
    public string[] GetBEDMForPU(string PU)
    {
        DataSet ds = new DataSet();

        SqlCommand objCommand;

        List<string> lstempCollection = new List<string>();
        //lstempCollection.Add("ALL");
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtPU";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = PU;

            objCommand = new SqlCommand();

            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.SpBEGetDMForNSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string du = ds.Tables[0].Rows[i]["txtDMMailId"].ToString();
                    lstempCollection.Add(du);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection.ToArray();
    }

    public string[] GetBEClientCodeForPU(string PU)
    {
        DataSet ds = new DataSet();

        SqlCommand objCommand;

        List<string> lstempCollection = new List<string>();
        //lstempCollection.Add("ALL");
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtPU";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = PU;

            objCommand = new SqlCommand();

            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.SpBEGetClientCodeForPU", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string du = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();
                    lstempCollection.Add(du);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection.ToArray();
    }

    public bool GetBEAdminScreenAccess(string userID)
    {
        bool isAdmin = false;
        DataSet ds = new DataSet();
        try
        {
            SqlParameterCollection objParamColl;
            objData = new DataAccess();
            objData.GetConnection();

            SqlParameter objParm = new SqlParameter();
            objParm.ParameterName = "@txtUserId";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Size = 50;
            objParm.Value = userID;
            var objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData.ExecuteSP("dbo.spBEGetAdminScreenAccess", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //string adminAccess = ds.Tables[0].Rows[i]["AdminAcess"].ToString();
                    isAdmin = true;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return isAdmin;
    }


    public DataTable GetMCOList(string userid)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;


        try
        {


            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@UserId";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userid;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            objParamColl.Add(objParamUserId);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEFetchMCODetails", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                return dt;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        return new DataTable();
    }


    public bool CanAbleTOAddNewCode(string userID, string txtMasterClientCode)
    {
        bool canDO = false;
        DataSet ds = new DataSet();
        try
        {
            SqlParameterCollection objParamColl;
            objData = new DataAccess();
            objData.GetConnection();

            SqlParameter objParm = new SqlParameter();
            objParm.ParameterName = "@txtUserId";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Size = 50;
            objParm.Value = userID;

            SqlParameter objParm1 = new SqlParameter();
            objParm1.ParameterName = "@txtMasterClientCode";
            objParm1.Direction = ParameterDirection.Input;
            objParm1.SqlDbType = SqlDbType.NVarChar;
            objParm1.Value = txtMasterClientCode;


            //SqlParameter objParm2 = new SqlParameter();
            //objParm2.ParameterName = "@txtquarter";
            //objParm2.Direction = ParameterDirection.Input;
            //objParm2.SqlDbType = SqlDbType.NVarChar;
            //objParm2.Size = 10;
            //objParm2.Value = qtr;

            var objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);
            objParamColl.Add(objParm1);
            //  objParamColl.Add(objParm2);

            objData.ExecuteSP("dbo.spBEChkClientAccess ", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                string adminAccess = ds.Tables[0].Rows[0]["Flag"].ToString();
                canDO = adminAccess == "0" ? false : true;

            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return canDO;
    }





    public DataTable GetBEData(string PU, string customerCode, string dm, string quarter, string year)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;


        try
        {

            objCommand = new SqlCommand();
            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.ParameterName = "@txtCustomerCode";
            objParamStatus.Direction = ParameterDirection.Input;
            objParamStatus.SqlDbType = SqlDbType.VarChar;
            objParamStatus.Value = customerCode;

            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@txtUserId";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = dm;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@txtQuarterName";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = quarter;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@txtYear";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = year;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@PU";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = PU;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            objParamColl.Add(objParamStatus);

            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);
            objParamColl.Add(objParamStatus4);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEFetchData", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
                //dt.Columns["txtDU"].ColumnName = "DU";
                dt.Columns["txtDMMailId"].ColumnName = "DM";
                dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
                dt.Columns["DMfltMonth1BE"].ColumnName = "DMMonth1";
                dt.Columns["DMfltMonth2BE"].ColumnName = "DMMonth2";
                dt.Columns["DMfltMonth3BE"].ColumnName = "DMMonth3";
                dt.Columns["DMfltCurrentQuarterBE"].ColumnName = "DMQCur";
                //dt.Columns["DMfltNextQuarterBE"].ColumnName = "DMQNext"; //TODO
                dt.Columns["fltPrevQtrBE"].ColumnName = "DMQPrev";
                dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
                dt.Columns["SDMfltMonth1BE"].ColumnName = "SDMMonth1";
                dt.Columns["SDMfltMonth2BE"].ColumnName = "SDMMonth2";
                dt.Columns["SDMfltMonth3BE"].ColumnName = "SDMMonth3";
                dt.Columns["SDMfltCurrentQuarterBE"].ColumnName = "SDMQCur";
                //dt.Columns["SDMfltNextQuarterBE"].ColumnName = "SDMQNext"; //TODO

                //dt.Columns[""].ColumnName = "SDMQPrev";
                // dt.Columns["txtLastUpdatedBy"].ColumnName = "LastModifiedBy";

                dt.Columns["fltActualsMonth1"].ColumnName = "ActualM1";
                dt.Columns["fltActualsMonth2"].ColumnName = "ActualM2";
                dt.Columns["fltActualsMonth3"].ColumnName = "ActualM3";
                dt.Columns["txtIsApproved"].ColumnName = "IsApproved";
                //dt.Columns["fltGuidanceConvRate"].ColumnName = "GuidanceConversionRate";
                //dt.Columns["fltCurrentConvRate"].ColumnName = "CurrentConversionRate";

                dt.Columns["dtDMUpdatedDate"].ColumnName = "DMLastModifiedOn";
                dt.Columns["dtSDMUpdatedDate"].ColumnName = "SDMLastModifiedOn";
                //dt.Columns["dtLastUpdatedDate"].ColumnName = "SDMLastModifiedOn";

                // dt.Columns["dtLastUpdatedDate"].ColumnName = "SDMLastModifiedOn";


                dt.Columns["intBEId"].ColumnName = "BEID";
                dt.Columns["txtRemarks"].ColumnName = "Remarks";
                dt.Columns["fltActualstotal"].ColumnName = "totalRTBR";
                dt.Columns["txtSDMRemarks"].ColumnName = "SDMRemarks";

                return dt;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataTable();
    }

    //public DataTable GetBEDataExcel(string customerCode, string dm, string PU, string quarter, string year)
    //{


    //    DataSet ds = new DataSet();

    //    SqlCommand objCommand;


    //    try
    //    {

    //        objCommand = new SqlCommand();
    //        SqlParameter objParamStatus = new SqlParameter();
    //        objParamStatus.ParameterName = "@txtCustomerCode";
    //        objParamStatus.Direction = ParameterDirection.Input;
    //        objParamStatus.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus.Value = customerCode;

    //        SqlParameter objParamStatus1 = new SqlParameter();
    //        objParamStatus1.ParameterName = "@txtUserId";
    //        objParamStatus1.Direction = ParameterDirection.Input;
    //        objParamStatus1.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus1.Value = dm;

    //        SqlParameter objParamStatus2 = new SqlParameter();
    //        objParamStatus2.ParameterName = "@txtQuarterName";
    //        objParamStatus2.Direction = ParameterDirection.Input;
    //        objParamStatus2.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus2.Value = quarter;

    //        SqlParameter objParamStatus3 = new SqlParameter();
    //        objParamStatus3.ParameterName = "@txtYear";
    //        objParamStatus3.Direction = ParameterDirection.Input;
    //        objParamStatus3.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus3.Value = year;

    //        SqlParameter objParamStatus4 = new SqlParameter();
    //        objParamStatus4.ParameterName = "@PU";
    //        objParamStatus4.Direction = ParameterDirection.Input;
    //        objParamStatus4.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus4.Value = PU;
    //        objCommand = new SqlCommand();
    //        SqlParameterCollection objParamColl = objCommand.Parameters;


    //        objParamColl.Add(objParamStatus);


    //        objParamColl.Add(objParamStatus1);
    //        objParamColl.Add(objParamStatus2);
    //        objParamColl.Add(objParamStatus3);
    //        objParamColl.Add(objParamStatus4);



    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("spBEVolData", ref ds, objCommand);
    //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
    //        {
    //            DataTable dt = new DataTable();
    //            dt = ds.Tables[0];
    //            dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
    //            //dt.Columns["txtDU"].ColumnName = "DU";
    //            dt.Columns["txtDMMailId"].ColumnName = "DM";
    //            dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
    //            dt.Columns["DMfltMonth1BE"].ColumnName = "DMMonth1";
    //            dt.Columns["DMfltMonth2BE"].ColumnName = "DMMonth2";
    //            dt.Columns["DMfltMonth3BE"].ColumnName = "DMMonth3";
    //            dt.Columns["DMfltCurrentQuarterBE"].ColumnName = "DMQCur";
    //            //dt.Columns["DMfltNextQuarterBE"].ColumnName = "DMQNext";
    //            dt.Columns["fltPrevQtrBE"].ColumnName = "DMQPrev";
    //            dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
    //            dt.Columns["SDMfltMonth1BE"].ColumnName = "SDMMonth1";
    //            dt.Columns["SDMfltMonth2BE"].ColumnName = "SDMMonth2";
    //            dt.Columns["SDMfltMonth3BE"].ColumnName = "SDMMonth3";
    //            dt.Columns["SDMfltCurrentQuarterBE"].ColumnName = "SDMQCur";
    //            //dt.Columns["SDMfltNextQuarterBE"].ColumnName = "SDMQNext";
    //            //dt.Columns[""].ColumnName = "SDMQPrev";
    //            // dt.Columns["txtLastUpdatedBy"].ColumnName = "LastModifiedBy";

    //            dt.Columns["fltActualsMonth1"].ColumnName = "ActualM1";
    //            dt.Columns["fltActualsMonth2"].ColumnName = "ActualM2";
    //            dt.Columns["fltActualsMonth3"].ColumnName = "ActualM3";
    //            dt.Columns["txtIsApproved"].ColumnName = "IsApproved";
    //            //dt.Columns["fltGuidanceConvRate"].ColumnName = "GuidanceConversionRate";
    //            //dt.Columns["fltCurrentConvRate"].ColumnName = "CurrentConversionRate";

    //            dt.Columns["dtDMUpdatedDate"].ColumnName = "DMLastModifiedOn";
    //            dt.Columns["dtSDMUpdatedDate"].ColumnName = "SDMLastModifiedOn";
    //            //dt.Columns["dtLastUpdatedDate"].ColumnName = "SDMLastModifiedOn";

    //            // dt.Columns["dtLastUpdatedDate"].ColumnName = "SDMLastModifiedOn";


    //            dt.Columns["intBEId"].ColumnName = "BEID";
    //            dt.Columns["txtRemarks"].ColumnName = "Remarks";
    //            dt.Columns["fltActualstotal"].ColumnName = "totalRTBR";
    //            //chandan 17 aug
    //            dt.Columns["fltPrevQtrOnsiteEffort"].ColumnName = "LastQON";
    //            dt.Columns["fltPrevQtrOffshoreEffort"].ColumnName = "LastQOFF";
    //            dt.Columns["fltPrevQtrTotalEffort"].ColumnName = "LastQTotal";

    //            dt.Columns["DMfltMonth1EffortOnsite"].ColumnName = ("DMMonth1ON");
    //            dt.Columns["DMfltMonth2EffortOnsite"].ColumnName = ("DMMonth2ON");
    //            dt.Columns["DMfltMonth3EffortOnsite"].ColumnName = ("DMMonth3ON");
    //            dt.Columns["DMfltMonth1EffortOffShore"].ColumnName = ("DMMonth1OFF");
    //            dt.Columns["DMfltMonth2EffortOffShore"].ColumnName = ("DMMonth2OFF");
    //            dt.Columns["DMfltMonth3EffortOffShore"].ColumnName = ("DMMonth3OFF");

    //            dt.Columns["SDMfltMonth1EffortOnsite"].ColumnName = ("SDMMonth1ON");
    //            dt.Columns["SDMfltMonth2EffortOnsite"].ColumnName = ("SDMMonth2ON");
    //            dt.Columns["SDMfltMonth3EffortOnsite"].ColumnName = ("SDMMonth3ON");
    //            dt.Columns["SDMfltMonth1EffortOffShore"].ColumnName = ("SDMMonth1OFF");
    //            dt.Columns["SDMfltMonth2EffortOffShore"].ColumnName = ("SDMMonth2OFF");
    //            dt.Columns["SDMfltMonth3EffortOffShore"].ColumnName = ("SDMMonth3OFF");


    //            dt.Columns["DMfltTotalOnsite"].ColumnName = ("DMTotalON");
    //            dt.Columns["DMfltTotalOffShore"].ColumnName = ("DMTotalOFF");
    //            dt.Columns["SDMfltTotalOnsite"].ColumnName = ("SDMTotalON");
    //            dt.Columns["SDMfltTotalOffShore"].ColumnName = ("SDMTotalOFF");
    //            dt.Columns["DMfltGrandTotal"].ColumnName = ("DMGrandTotal");
    //            dt.Columns["SDMfltGrandTotal"].ColumnName = ("SDMGrandTotal");
    //            dt.Columns["txtCurrentQuarterName"].ColumnName = ("txtCurrentQuarterName");
    //            dt.Columns["txtYear"].ColumnName = ("txtYear");

    //            //dt.Columns.Add("LastQON");
    //            //dt.Columns.Add("LastQOFF");
    //            //dt.Columns.Add("LastQTotal");


    //            return dt;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }


    //    return new DataTable();
    //}


    //public DataTable GetBEVolDataExcel(string customerCode, string dm, string PU, string quarter, string year)
    //{


    //    DataSet ds = new DataSet();

    //    SqlCommand objCommand;


    //    try
    //    {

    //        objCommand = new SqlCommand();
    //        SqlParameter objParamStatus = new SqlParameter();
    //        objParamStatus.ParameterName = "@txtCustomerCode";
    //        objParamStatus.Direction = ParameterDirection.Input;
    //        objParamStatus.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus.Value = customerCode;

    //        SqlParameter objParamStatus1 = new SqlParameter();
    //        objParamStatus1.ParameterName = "@txtUserId";
    //        objParamStatus1.Direction = ParameterDirection.Input;
    //        objParamStatus1.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus1.Value = dm;

    //        SqlParameter objParamStatus2 = new SqlParameter();
    //        objParamStatus2.ParameterName = "@txtQuarterName";
    //        objParamStatus2.Direction = ParameterDirection.Input;
    //        objParamStatus2.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus2.Value = quarter;

    //        SqlParameter objParamStatus3 = new SqlParameter();
    //        objParamStatus3.ParameterName = "@txtYear";
    //        objParamStatus3.Direction = ParameterDirection.Input;
    //        objParamStatus3.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus3.Value = year;

    //        SqlParameter objParamStatus4 = new SqlParameter();
    //        objParamStatus4.ParameterName = "@PU";
    //        objParamStatus4.Direction = ParameterDirection.Input;
    //        objParamStatus4.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus4.Value = PU;
    //        objCommand = new SqlCommand();
    //        SqlParameterCollection objParamColl = objCommand.Parameters;


    //        objParamColl.Add(objParamStatus);


    //        objParamColl.Add(objParamStatus1);
    //        objParamColl.Add(objParamStatus2);
    //        objParamColl.Add(objParamStatus3);
    //        objParamColl.Add(objParamStatus4);



    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("spBEVolDataBase", ref ds, objCommand);
    //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
    //        {
    //            DataTable dt = new DataTable();
    //            dt = ds.Tables[0];
    //            dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
    //            //dt.Columns["txtDU"].ColumnName = "DU";
    //            dt.Columns["txtDMMailId"].ColumnName = "DM";
    //            dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
    //            dt.Columns["DMfltMonth1BE"].ColumnName = "DMMonth1";
    //            dt.Columns["DMfltMonth2BE"].ColumnName = "DMMonth2";
    //            dt.Columns["DMfltMonth3BE"].ColumnName = "DMMonth3";
    //            dt.Columns["DMfltCurrentQuarterBE"].ColumnName = "DMQCur";
    //            //dt.Columns["DMfltNextQuarterBE"].ColumnName = "DMQNext";
    //            dt.Columns["fltPrevQtrBE"].ColumnName = "DMQPrev";
    //            dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
    //            dt.Columns["SDMfltMonth1BE"].ColumnName = "SDMMonth1";
    //            dt.Columns["SDMfltMonth2BE"].ColumnName = "SDMMonth2";
    //            dt.Columns["SDMfltMonth3BE"].ColumnName = "SDMMonth3";
    //            dt.Columns["SDMfltCurrentQuarterBE"].ColumnName = "SDMQCur";
    //            //dt.Columns["SDMfltNextQuarterBE"].ColumnName = "SDMQNext";
    //            //dt.Columns[""].ColumnName = "SDMQPrev";
    //            // dt.Columns["txtLastUpdatedBy"].ColumnName = "LastModifiedBy";

    //            dt.Columns["fltActualsMonth1"].ColumnName = "ActualM1";
    //            dt.Columns["fltActualsMonth2"].ColumnName = "ActualM2";
    //            dt.Columns["fltActualsMonth3"].ColumnName = "ActualM3";
    //            dt.Columns["txtIsApproved"].ColumnName = "IsApproved";
    //            //dt.Columns["fltGuidanceConvRate"].ColumnName = "GuidanceConversionRate";
    //            //dt.Columns["fltCurrentConvRate"].ColumnName = "CurrentConversionRate";

    //            dt.Columns["dtDMUpdatedDate"].ColumnName = "DMLastModifiedOn";
    //            dt.Columns["dtSDMUpdatedDate"].ColumnName = "SDMLastModifiedOn";
    //            //dt.Columns["dtLastUpdatedDate"].ColumnName = "SDMLastModifiedOn";

    //            // dt.Columns["dtLastUpdatedDate"].ColumnName = "SDMLastModifiedOn";


    //            dt.Columns["intBEId"].ColumnName = "BEID";
    //            dt.Columns["txtRemarks"].ColumnName = "Remarks";
    //            dt.Columns["fltActualstotal"].ColumnName = "totalRTBR";
    //            //chandan 17 aug
    //            dt.Columns["fltPrevQtrOnsiteEffort"].ColumnName = "LastQON";
    //            dt.Columns["fltPrevQtrOffshoreEffort"].ColumnName = "LastQOFF";
    //            dt.Columns["fltPrevQtrTotalEffort"].ColumnName = "LastQTotal";

    //            dt.Columns["DMfltMonth1EffortOnsite"].ColumnName = ("DMMonth1ON");
    //            dt.Columns["DMfltMonth2EffortOnsite"].ColumnName = ("DMMonth2ON");
    //            dt.Columns["DMfltMonth3EffortOnsite"].ColumnName = ("DMMonth3ON");
    //            dt.Columns["DMfltMonth1EffortOffShore"].ColumnName = ("DMMonth1OFF");
    //            dt.Columns["DMfltMonth2EffortOffShore"].ColumnName = ("DMMonth2OFF");
    //            dt.Columns["DMfltMonth3EffortOffShore"].ColumnName = ("DMMonth3OFF");

    //            dt.Columns["SDMfltMonth1EffortOnsite"].ColumnName = ("SDMMonth1ON");
    //            dt.Columns["SDMfltMonth2EffortOnsite"].ColumnName = ("SDMMonth2ON");
    //            dt.Columns["SDMfltMonth3EffortOnsite"].ColumnName = ("SDMMonth3ON");
    //            dt.Columns["SDMfltMonth1EffortOffShore"].ColumnName = ("SDMMonth1OFF");
    //            dt.Columns["SDMfltMonth2EffortOffShore"].ColumnName = ("SDMMonth2OFF");
    //            dt.Columns["SDMfltMonth3EffortOffShore"].ColumnName = ("SDMMonth3OFF");


    //            dt.Columns["DMfltTotalOnsite"].ColumnName = ("DMTotalON");
    //            dt.Columns["DMfltTotalOffShore"].ColumnName = ("DMTotalOFF");
    //            dt.Columns["SDMfltTotalOnsite"].ColumnName = ("SDMTotalON");
    //            dt.Columns["SDMfltTotalOffShore"].ColumnName = ("SDMTotalOFF");
    //            dt.Columns["DMfltGrandTotal"].ColumnName = ("DMGrandTotal");
    //            dt.Columns["SDMfltGrandTotal"].ColumnName = ("SDMGrandTotal");
    //            dt.Columns["txtCurrentQuarterName"].ColumnName = ("txtCurrentQuarterName");
    //            dt.Columns["txtYear"].ColumnName = ("txtYear");

    //            //dt.Columns.Add("LastQON");
    //            //dt.Columns.Add("LastQOFF");
    //            //dt.Columns.Add("LastQTotal");


    //            return dt;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }


    //    return new DataTable();
    //}








    public DataTable GetBERevDataExcel(string customerCode, string dm, string PU, string quarter, string year)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;


        try
        {

            objCommand = new SqlCommand();
            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.ParameterName = "@txtCustomerCode";
            objParamStatus.Direction = ParameterDirection.Input;
            objParamStatus.SqlDbType = SqlDbType.VarChar;
            objParamStatus.Value = customerCode;

            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@txtUserId";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = dm;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@txtQuarterName";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = quarter;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@txtYear";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = year;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@PU";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = PU;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            objParamColl.Add(objParamStatus);


            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);
            objParamColl.Add(objParamStatus4);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEVolDataBase", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
                dt.Columns["txtPU"].ColumnName = "PU";
                dt.Columns["txtDMMailId"].ColumnName = "DM";
                dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
                dt.Columns["DMfltMonth1BE"].ColumnName = "DMMonth1";
                dt.Columns["DMfltMonth2BE"].ColumnName = "DMMonth2";
                dt.Columns["DMfltMonth3BE"].ColumnName = "DMMonth3";
                dt.Columns["DMfltCurrentQuarterBE"].ColumnName = "DMQCur";
                dt.Columns["txtDHMailId"].ColumnName = "DHMailID";
                //  dt.Columns["fltPrevQtrBE"].ColumnName = "DMQPrev";
                dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
                dt.Columns["SDMfltMonth1BE"].ColumnName = "SDMMonth1";
                dt.Columns["SDMfltMonth2BE"].ColumnName = "SDMMonth2";
                dt.Columns["SDMfltMonth3BE"].ColumnName = "SDMMonth3";
                dt.Columns["SDMfltCurrentQuarterBE"].ColumnName = "SDMQCur";
                //dt.Columns["SDMfltNextQuarterBE"].ColumnName = "SDMQNext";
                //dt.Columns[""].ColumnName = "SDMQPrev";
                // dt.Columns["txtLastUpdatedBy"].ColumnName = "LastModifiedBy";

                dt.Columns["fltActualsMonth1"].ColumnName = "ActualM1";
                dt.Columns["fltActualsMonth2"].ColumnName = "ActualM2";
                dt.Columns["fltActualsMonth3"].ColumnName = "ActualM3";
                dt.Columns["txtIsApproved"].ColumnName = "IsApproved";
                //dt.Columns["fltGuidanceConvRate"].ColumnName = "GuidanceConversionRate";
                //dt.Columns["fltCurrentConvRate"].ColumnName = "CurrentConversionRate";

                dt.Columns["dtDMUpdatedDate"].ColumnName = "DMLastModifiedOn";
                dt.Columns["dtSDMUpdatedDate"].ColumnName = "SDMLastModifiedOn";
                //dt.Columns["dtLastUpdatedDate"].ColumnName = "SDMLastModifiedOn";

                // dt.Columns["dtLastUpdatedDate"].ColumnName = "SDMLastModifiedOn";


                dt.Columns["intBEId"].ColumnName = "BEID";
                dt.Columns["txtRemarks"].ColumnName = "DMRemarks";
                dt.Columns["txtSDMRemarks"].ColumnName = "SDMRemarks";
                dt.Columns["fltActualstotal"].ColumnName = "totalRTBR";
                //chandan 17 aug
                //dt.Columns["fltPrevQtrOnsiteEffort"].ColumnName = "LastQON";
                //dt.Columns["fltPrevQtrOffshoreEffort"].ColumnName = "LastQOFF";
                //dt.Columns["fltPrevQtrTotalEffort"].ColumnName = "LastQTotal";

                dt.Columns["txtCurrentQuarterName"].ColumnName = ("txtCurrentQuarterName");
                dt.Columns["txtYear"].ColumnName = ("txtYear");

                return dt;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataTable();
    }


    //public DataTable GetBEVolDataExcel(string customerCode, string dm, string PU, string quarter, string year)
    //{


    //    DataSet ds = new DataSet();

    //    SqlCommand objCommand;


    //    try
    //    {

    //        objCommand = new SqlCommand();
    //        SqlParameter objParamStatus = new SqlParameter();
    //        objParamStatus.ParameterName = "@txtCustomerCode";
    //        objParamStatus.Direction = ParameterDirection.Input;
    //        objParamStatus.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus.Value = customerCode;

    //        SqlParameter objParamStatus1 = new SqlParameter();
    //        objParamStatus1.ParameterName = "@txtUserId";
    //        objParamStatus1.Direction = ParameterDirection.Input;
    //        objParamStatus1.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus1.Value = dm;

    //        SqlParameter objParamStatus2 = new SqlParameter();
    //        objParamStatus2.ParameterName = "@txtQuarterName";
    //        objParamStatus2.Direction = ParameterDirection.Input;
    //        objParamStatus2.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus2.Value = quarter;

    //        SqlParameter objParamStatus3 = new SqlParameter();
    //        objParamStatus3.ParameterName = "@txtYear";
    //        objParamStatus3.Direction = ParameterDirection.Input;
    //        objParamStatus3.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus3.Value = year;

    //        SqlParameter objParamStatus4 = new SqlParameter();
    //        objParamStatus4.ParameterName = "@PU";
    //        objParamStatus4.Direction = ParameterDirection.Input;
    //        objParamStatus4.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus4.Value = PU;
    //        objCommand = new SqlCommand();
    //        SqlParameterCollection objParamColl = objCommand.Parameters;


    //        objParamColl.Add(objParamStatus);


    //        objParamColl.Add(objParamStatus1);
    //        objParamColl.Add(objParamStatus2);
    //        objParamColl.Add(objParamStatus3);
    //        objParamColl.Add(objParamStatus4);



    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("spBEVolData", ref ds, objCommand);
    //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
    //        {
    //            DataTable dt = new DataTable();
    //            dt = ds.Tables[0];
    //            dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
    //            dt.Columns["txtPU"].ColumnName = "PU";
    //            dt.Columns["txtDMMailId"].ColumnName = "DM";
    //            //dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
    //            //dt.Columns["DMfltMonth1BE"].ColumnName = "DMMonth1";
    //            //dt.Columns["DMfltMonth2BE"].ColumnName = "DMMonth2";
    //            //dt.Columns["DMfltMonth3BE"].ColumnName = "DMMonth3";
    //            //dt.Columns["DMfltCurrentQuarterBE"].ColumnName = "DMQCur";
    //            //dt.Columns["DMfltNextQuarterBE"].ColumnName = "DMQNext";
    //            dt.Columns["txtDHMailId"].ColumnName = "DHMailId";
    //            dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
    //            //dt.Columns["SDMfltMonth1BE"].ColumnName = "SDMMonth1";
    //            //dt.Columns["SDMfltMonth2BE"].ColumnName = "SDMMonth2";
    //            //dt.Columns["SDMfltMonth3BE"].ColumnName = "SDMMonth3";
    //            //dt.Columns["SDMfltCurrentQuarterBE"].ColumnName = "SDMQCur";
    //            //dt.Columns["SDMfltNextQuarterBE"].ColumnName = "SDMQNext";
    //            //dt.Columns[""].ColumnName = "SDMQPrev";
    //            // dt.Columns["txtLastUpdatedBy"].ColumnName = "LastModifiedBy";

    //            //dt.Columns["fltActualsMonth1"].ColumnName = "ActualM1";
    //            //dt.Columns["fltActualsMonth2"].ColumnName = "ActualM2";
    //            //dt.Columns["fltActualsMonth3"].ColumnName = "ActualM3";
    //            dt.Columns["txtIsApproved"].ColumnName = "IsApproved";
    //            //dt.Columns["fltGuidanceConvRate"].ColumnName = "GuidanceConversionRate";
    //            //dt.Columns["fltCurrentConvRate"].ColumnName = "CurrentConversionRate";

    //            dt.Columns["dtDMUpdatedDate"].ColumnName = "DMLastModifiedOn";
    //            dt.Columns["dtSDMUpdatedDate"].ColumnName = "SDMLastModifiedOn";
    //            //dt.Columns["dtLastUpdatedDate"].ColumnName = "SDMLastModifiedOn";

    //            // dt.Columns["dtLastUpdatedDate"].ColumnName = "SDMLastModifiedOn";


    //            dt.Columns["intBEId"].ColumnName = "BEID";
    //            dt.Columns["txtRemarks"].ColumnName = "DMRemarks";
    //            dt.Columns["txtRemarksSDM"].ColumnName = "SDMRemarks";
    //            // dt.Columns["fltActualstotal"].ColumnName = "totalRTBR";
    //            //chandan 17 aug
    //            //dt.Columns["fltPrevQtrOnsiteEffort"].ColumnName = "LastQON";
    //            //dt.Columns["fltPrevQtrOffshoreEffort"].ColumnName = "LastQOFF";
    //            //dt.Columns["fltPrevQtrTotalEffort"].ColumnName = "LastQTotal";

    //            dt.Columns["DMfltMonth1EffortOnsite"].ColumnName = ("DMMonth1ON");
    //            dt.Columns["DMfltMonth2EffortOnsite"].ColumnName = ("DMMonth2ON");
    //            dt.Columns["DMfltMonth3EffortOnsite"].ColumnName = ("DMMonth3ON");
    //            dt.Columns["DMfltMonth1EffortOffShore"].ColumnName = ("DMMonth1OFF");
    //            dt.Columns["DMfltMonth2EffortOffShore"].ColumnName = ("DMMonth2OFF");
    //            dt.Columns["DMfltMonth3EffortOffShore"].ColumnName = ("DMMonth3OFF");

    //            dt.Columns["SDMfltMonth1EffortOnsite"].ColumnName = ("SDMMonth1ON");
    //            dt.Columns["SDMfltMonth2EffortOnsite"].ColumnName = ("SDMMonth2ON");
    //            dt.Columns["SDMfltMonth3EffortOnsite"].ColumnName = ("SDMMonth3ON");
    //            dt.Columns["SDMfltMonth1EffortOffShore"].ColumnName = ("SDMMonth1OFF");
    //            dt.Columns["SDMfltMonth2EffortOffShore"].ColumnName = ("SDMMonth2OFF");
    //            dt.Columns["SDMfltMonth3EffortOffShore"].ColumnName = ("SDMMonth3OFF");


    //            dt.Columns["DMfltTotalOnsite"].ColumnName = ("DMTotalON");
    //            dt.Columns["DMfltTotalOffShore"].ColumnName = ("DMTotalOFF");
    //            dt.Columns["SDMfltTotalOnsite"].ColumnName = ("SDMTotalON");
    //            dt.Columns["SDMfltTotalOffShore"].ColumnName = ("SDMTotalOFF");
    //            dt.Columns["DMfltGrandTotal"].ColumnName = ("DMGrandTotal");
    //            dt.Columns["SDMfltGrandTotal"].ColumnName = ("SDMGrandTotal");
    //            dt.Columns["txtCurrentQuarterName"].ColumnName = ("txtCurrentQuarterName");
    //            dt.Columns["txtYear"].ColumnName = ("txtYear");

    //            //dt.Columns.Add("LastQON");
    //            //dt.Columns.Add("LastQOFF");
    //            //dt.Columns.Add("LastQTotal");


    //            return dt;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }


    //    return new DataTable();
    //}






    public DataTable GetBEVolume(string PU, string customerCode, string userID, string quarter, string year, string type)
    {

        {


            DataSet ds = new DataSet();

            SqlCommand objCommand;


            try
            {

                objCommand = new SqlCommand();
                SqlParameter objParamStatus = new SqlParameter();
                objParamStatus.ParameterName = "@txtCustomerCode";
                objParamStatus.Direction = ParameterDirection.Input;
                objParamStatus.SqlDbType = SqlDbType.VarChar;
                objParamStatus.Value = customerCode;

                SqlParameter objParamStatus1 = new SqlParameter();
                objParamStatus1.ParameterName = "@txtUserId";
                objParamStatus1.Direction = ParameterDirection.Input;
                objParamStatus1.SqlDbType = SqlDbType.VarChar;
                objParamStatus1.Value = userID;

                SqlParameter objParamStatus2 = new SqlParameter();
                objParamStatus2.ParameterName = "@txtQuarterName";
                objParamStatus2.Direction = ParameterDirection.Input;
                objParamStatus2.SqlDbType = SqlDbType.VarChar;
                objParamStatus2.Value = quarter;

                SqlParameter objParamStatus3 = new SqlParameter();
                objParamStatus3.ParameterName = "@txtYear";
                objParamStatus3.Direction = ParameterDirection.Input;
                objParamStatus3.SqlDbType = SqlDbType.VarChar;
                objParamStatus3.Value = year;

                SqlParameter objParamStatus4 = new SqlParameter();
                objParamStatus4.ParameterName = "@PU";
                objParamStatus4.Direction = ParameterDirection.Input;
                objParamStatus4.SqlDbType = SqlDbType.VarChar;
                objParamStatus4.Value = PU;

                SqlParameter objParamStatus5 = new SqlParameter();
                objParamStatus5.ParameterName = "@type";
                objParamStatus5.Direction = ParameterDirection.Input;
                objParamStatus5.SqlDbType = SqlDbType.VarChar;
                objParamStatus5.Value = type;

                objCommand = new SqlCommand();
                SqlParameterCollection objParamColl = objCommand.Parameters;


                objParamColl.Add(objParamStatus);
                objParamColl.Add(objParamStatus1);
                objParamColl.Add(objParamStatus2);
                objParamColl.Add(objParamStatus3);
                objParamColl.Add(objParamStatus4);
                objParamColl.Add(objParamStatus5);


                objData = new DataAccess();
                objData.GetConnection();
                objData.ExecuteSP("spBEFetchVolSDM", ref ds, objCommand);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";

                    //dt.Columns["txtDMMailId"].ColumnName = "DM";


                    dt.Columns["DMfltMonth1EffortOnsite"].ColumnName = ("DMMonth1ON");
                    dt.Columns["DMfltMonth2EffortOnsite"].ColumnName = ("DMMonth2ON");
                    dt.Columns["DMfltMonth3EffortOnsite"].ColumnName = ("DMMonth3ON");
                    dt.Columns["DMfltMonth1EffortOffShore"].ColumnName = ("DMMonth1OFF");
                    dt.Columns["DMfltMonth2EffortOffShore"].ColumnName = ("DMMonth2OFF");
                    dt.Columns["DMfltMonth3EffortOffShore"].ColumnName = ("DMMonth3OFF");

                    dt.Columns["SDMfltMonth1EffortOnsite"].ColumnName = ("SDMMonth1ON");
                    dt.Columns["SDMfltMonth2EffortOnsite"].ColumnName = ("SDMMonth2ON");
                    dt.Columns["SDMfltMonth3EffortOnsite"].ColumnName = ("SDMMonth3ON");
                    dt.Columns["SDMfltMonth1EffortOffShore"].ColumnName = ("SDMMonth1OFF");
                    dt.Columns["SDMfltMonth2EffortOffShore"].ColumnName = ("SDMMonth2OFF");
                    dt.Columns["SDMfltMonth3EffortOffShore"].ColumnName = ("SDMMonth3OFF");

                    dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";

                    dt.Columns["DMfltTotalOnsite"].ColumnName = ("DMTotalON");
                    dt.Columns["DMfltTotalOffShore"].ColumnName = ("DMTotalOFF");
                    dt.Columns["SDMfltTotalOnsite"].ColumnName = ("SDMTotalON");
                    dt.Columns["SDMfltTotalOffShore"].ColumnName = ("SDMTotalOFF");
                    dt.Columns["DMfltGrandTotal"].ColumnName = ("DMGrandTotal");
                    dt.Columns["SDMfltGrandTotal"].ColumnName = ("SDMGrandTotal");

                    dt.Columns["txtOnsiteValueM1"].ColumnName = ("RTBRMonth1ON");
                    dt.Columns["txtOffshoreValueM1"].ColumnName = ("RTBRMonth1OFF");
                    dt.Columns["txtOnsiteValueM2"].ColumnName = ("RTBRMonth2ON");
                    dt.Columns["txtOffshoreValueM2"].ColumnName = ("RTBRMonth2OFF");
                    dt.Columns["txtOnsiteValueM3"].ColumnName = ("RTBRMonth3ON");
                    dt.Columns["txtOffshoreValueM3"].ColumnName = ("RTBRMonth3OFF");

                    dt.Columns["txtTotalOnsiteValue"].ColumnName = ("RTBRTotalON");
                    dt.Columns["txtTotalOffshoreValue"].ColumnName = ("RTBRTotalOFF");
                    dt.Columns["txtGrandTotalValue"].ColumnName = ("RTBRGrandTotal");


                    //dt.Columns["dtDMUpdatedDate"].ColumnName = "DMLastModifiedOn";

                    dt.Columns["dtSDMUpdatedDate"].ColumnName = "SDMLastModifiedOn";


                    dt.Columns["intBEId"].ColumnName = "BEID";

                    //dt.Columns["txtRemarks"].ColumnName = "DMRemarks";

                    dt.Columns["txtRemarksSDM"].ColumnName = "SDMRemarks";
                    dt.Columns["txtPU"].ColumnName = "PU";

                    return dt;
                }
            }
            catch (Exception ex)
            {

                logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                objData.CloseConnection();
            }


            return new DataTable();
        }




    }


    //ganapathy ----For Fetching the BE DM data


    public DataTable GetBEVolumeDM(string PU, string customerCode, string userID, string quarter, string year)
    {

        {
            DataSet ds = new DataSet();
            SqlCommand objCommand;

            try
            {
                objCommand = new SqlCommand();
                SqlParameter objParamStatus = new SqlParameter();
                objParamStatus.ParameterName = "@txtCustomerCode";
                objParamStatus.Direction = ParameterDirection.Input;
                objParamStatus.SqlDbType = SqlDbType.VarChar;
                objParamStatus.Value = customerCode;

                SqlParameter objParamStatus1 = new SqlParameter();
                objParamStatus1.ParameterName = "@txtUserId";
                objParamStatus1.Direction = ParameterDirection.Input;
                objParamStatus1.SqlDbType = SqlDbType.VarChar;
                objParamStatus1.Value = userID;

                SqlParameter objParamStatus2 = new SqlParameter();
                objParamStatus2.ParameterName = "@txtQuarterName";
                objParamStatus2.Direction = ParameterDirection.Input;
                objParamStatus2.SqlDbType = SqlDbType.VarChar;
                objParamStatus2.Value = quarter;

                SqlParameter objParamStatus3 = new SqlParameter();
                objParamStatus3.ParameterName = "@txtYear";
                objParamStatus3.Direction = ParameterDirection.Input;
                objParamStatus3.SqlDbType = SqlDbType.VarChar;
                objParamStatus3.Value = year;

                SqlParameter objParamStatus4 = new SqlParameter();
                objParamStatus4.ParameterName = "@PU";
                objParamStatus4.Direction = ParameterDirection.Input;
                objParamStatus4.SqlDbType = SqlDbType.VarChar;
                objParamStatus4.Value = PU;
                objCommand = new SqlCommand();
                SqlParameterCollection objParamColl = objCommand.Parameters;


                objParamColl.Add(objParamStatus);
                objParamColl.Add(objParamStatus1);
                objParamColl.Add(objParamStatus2);
                objParamColl.Add(objParamStatus3);
                objParamColl.Add(objParamStatus4);



                objData = new DataAccess();
                objData.GetConnection();
                objData.ExecuteSP("spBEFetchVolDM_RTBR", ref ds, objCommand);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
                    //dt.Columns["txtDU"].ColumnName = "DU";
                    dt.Columns["txtDMMailId"].ColumnName = "DM";


                    dt.Columns["DMfltMonth1EffortOnsite"].ColumnName = ("DMMonth1ON");
                    dt.Columns["DMfltMonth2EffortOnsite"].ColumnName = ("DMMonth2ON");
                    dt.Columns["DMfltMonth3EffortOnsite"].ColumnName = ("DMMonth3ON");
                    dt.Columns["DMfltMonth1EffortOffShore"].ColumnName = ("DMMonth1OFF");
                    dt.Columns["DMfltMonth2EffortOffShore"].ColumnName = ("DMMonth2OFF");
                    dt.Columns["DMfltMonth3EffortOffShore"].ColumnName = ("DMMonth3OFF");

                    //dt.Columns["SDMfltMonth1EffortOnsite"].ColumnName = ("SDMMonth1ON");
                    //dt.Columns["SDMfltMonth2EffortOnsite"].ColumnName = ("SDMMonth2ON");
                    //dt.Columns["SDMfltMonth3EffortOnsite"].ColumnName = ("SDMMonth3ON");
                    //dt.Columns["SDMfltMonth1EffortOffShore"].ColumnName = ("SDMMonth1OFF");
                    //dt.Columns["SDMfltMonth2EffortOffShore"].ColumnName = ("SDMMonth2OFF");
                    //dt.Columns["SDMfltMonth3EffortOffShore"].ColumnName = ("SDMMonth3OFF");

                    //dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";

                    dt.Columns["DMfltTotalOnsite"].ColumnName = ("DMTotalON");
                    dt.Columns["DMfltTotalOffShore"].ColumnName = ("DMTotalOFF");
                    //dt.Columns["SDMfltTotalOnsite"].ColumnName = ("SDMTotalON");
                    //dt.Columns["SDMfltTotalOffShore"].ColumnName = ("SDMTotalOFF");
                    dt.Columns["DMfltGrandTotal"].ColumnName = ("DMGrandTotal");
                    //dt.Columns["SDMfltGrandTotal"].ColumnName = ("SDMGrandTotal");

                    dt.Columns["txtOnsiteValueM1"].ColumnName = ("RTBRMonth1ON");
                    dt.Columns["txtOffshoreValueM1"].ColumnName = ("RTBRMonth1OFF");
                    dt.Columns["txtOnsiteValueM2"].ColumnName = ("RTBRMonth2ON");
                    dt.Columns["txtOffshoreValueM2"].ColumnName = ("RTBRMonth2OFF");
                    dt.Columns["txtOnsiteValueM3"].ColumnName = ("RTBRMonth3ON");
                    dt.Columns["txtOffshoreValueM3"].ColumnName = ("RTBRMonth3OFF");

                    dt.Columns["txtTotalOnsiteValue"].ColumnName = ("RTBRTotalON");
                    dt.Columns["txtTotalOffshoreValue"].ColumnName = ("RTBRTotalOFF");
                    dt.Columns["txtGrandTotalValue"].ColumnName = ("RTBRGrandTotal");

                    dt.Columns["dtDMUpdatedDate"].ColumnName = "DMLastModifiedOn";
                    //dt.Columns["dtSDMUpdatedDate"].ColumnName = "SDMLastModifiedOn";


                    dt.Columns["intBEId"].ColumnName = "BEID";
                    dt.Columns["txtRemarks"].ColumnName = "DMRemarks";

                    dt.Columns["txtPU"].ColumnName = "PU";


                    return dt;
                }
            }
            catch (Exception ex)
            {

                logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                objData.CloseConnection();
            }


            return new DataTable();
        }

    }







    //TODO:20/12 commented
    //public DataTable GetBEVolume(string PU, string customerCode, string userID, string quarter, string year)
    //{

    //    {


    //        DataSet ds = new DataSet();

    //        SqlCommand objCommand;


    //        try
    //        {

    //            objCommand = new SqlCommand();
    //            SqlParameter objParamStatus = new SqlParameter();
    //            objParamStatus.ParameterName = "@txtCustomerCode";
    //            objParamStatus.Direction = ParameterDirection.Input;
    //            objParamStatus.SqlDbType = SqlDbType.VarChar;
    //            objParamStatus.Value = customerCode;

    //            SqlParameter objParamStatus1 = new SqlParameter();
    //            objParamStatus1.ParameterName = "@txtUserId";
    //            objParamStatus1.Direction = ParameterDirection.Input;
    //            objParamStatus1.SqlDbType = SqlDbType.VarChar;
    //            objParamStatus1.Value = userID;

    //            SqlParameter objParamStatus2 = new SqlParameter();
    //            objParamStatus2.ParameterName = "@txtQuarterName";
    //            objParamStatus2.Direction = ParameterDirection.Input;
    //            objParamStatus2.SqlDbType = SqlDbType.VarChar;
    //            objParamStatus2.Value = quarter;

    //            SqlParameter objParamStatus3 = new SqlParameter();
    //            objParamStatus3.ParameterName = "@txtYear";
    //            objParamStatus3.Direction = ParameterDirection.Input;
    //            objParamStatus3.SqlDbType = SqlDbType.VarChar;
    //            objParamStatus3.Value = year;

    //            SqlParameter objParamStatus4 = new SqlParameter();
    //            objParamStatus4.ParameterName = "@PU";
    //            objParamStatus4.Direction = ParameterDirection.Input;
    //            objParamStatus4.SqlDbType = SqlDbType.VarChar;
    //            objParamStatus4.Value = PU;
    //            objCommand = new SqlCommand();
    //            SqlParameterCollection objParamColl = objCommand.Parameters;


    //            objParamColl.Add(objParamStatus);
    //            objParamColl.Add(objParamStatus1);
    //            objParamColl.Add(objParamStatus2);
    //            objParamColl.Add(objParamStatus3);
    //            objParamColl.Add(objParamStatus4);



    //            objData = new DataAccess();
    //            objData.GetConnection();
    //            objData.ExecuteSP("spBEFetchVolume", ref ds, objCommand);
    //            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
    //            {
    //                DataTable dt = new DataTable();
    //                dt = ds.Tables[0];
    //                dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
    //                //dt.Columns["txtDU"].ColumnName = "DU";
    //                dt.Columns["txtDMMailId"].ColumnName = "DM";
    //                //TODO: du and currency commented
    //                //dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
    //                //chandan 17 aug

    //                //TODO:25/9 previous qtr details not to be displayed
    //                //dt.Columns["fltPrevQtrOnsiteEffort"].ColumnName = "LastQON";
    //                //dt.Columns["fltPrevQtrOffshoreEffort"].ColumnName = "LastQOFF";
    //                //dt.Columns["fltPrevQtrTotalEffort"].ColumnName = "LastQTotal";

    //                dt.Columns["DMfltMonth1EffortOnsite"].ColumnName = ("DMMonth1ON");
    //                dt.Columns["DMfltMonth2EffortOnsite"].ColumnName = ("DMMonth2ON");
    //                dt.Columns["DMfltMonth3EffortOnsite"].ColumnName = ("DMMonth3ON");
    //                dt.Columns["DMfltMonth1EffortOffShore"].ColumnName = ("DMMonth1OFF");
    //                dt.Columns["DMfltMonth2EffortOffShore"].ColumnName = ("DMMonth2OFF");
    //                dt.Columns["DMfltMonth3EffortOffShore"].ColumnName = ("DMMonth3OFF");

    //                dt.Columns["SDMfltMonth1EffortOnsite"].ColumnName = ("SDMMonth1ON");
    //                dt.Columns["SDMfltMonth2EffortOnsite"].ColumnName = ("SDMMonth2ON");
    //                dt.Columns["SDMfltMonth3EffortOnsite"].ColumnName = ("SDMMonth3ON");
    //                dt.Columns["SDMfltMonth1EffortOffShore"].ColumnName = ("SDMMonth1OFF");
    //                dt.Columns["SDMfltMonth2EffortOffShore"].ColumnName = ("SDMMonth2OFF");
    //                dt.Columns["SDMfltMonth3EffortOffShore"].ColumnName = ("SDMMonth3OFF");

    //                dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";

    //                dt.Columns["DMfltTotalOnsite"].ColumnName = ("DMTotalON");
    //                dt.Columns["DMfltTotalOffShore"].ColumnName = ("DMTotalOFF");
    //                dt.Columns["SDMfltTotalOnsite"].ColumnName = ("SDMTotalON");
    //                dt.Columns["SDMfltTotalOffShore"].ColumnName = ("SDMTotalOFF");
    //                dt.Columns["DMfltGrandTotal"].ColumnName = ("DMGrandTotal");
    //                dt.Columns["SDMfltGrandTotal"].ColumnName = ("SDMGrandTotal");

    //                //dt.Columns.Add("DMTotalON");
    //                //dt.Columns.Add("DMTotalOFF");
    //                //dt.Columns.Add("SDMTotalON");
    //                //dt.Columns.Add("SDMTotalOFF");
    //                //dt.Columns.Add("DMGrandTotal");
    //                //dt.Columns.Add("SDMGrandTotal");

    //                //dt.Columns.Add("LastQON") ;
    //                //dt.Columns.Add("LastQOFF");
    //                //dt.Columns.Add("LastQTotal");


    //                dt.Columns["dtDMUpdatedDate"].ColumnName = "DMLastModifiedOn";
    //                dt.Columns["dtSDMUpdatedDate"].ColumnName = "SDMLastModifiedOn";


    //                dt.Columns["intBEId"].ColumnName = "BEID";
    //                dt.Columns["txtRemarks"].ColumnName = "DMRemarks";

    //                dt.Columns["txtRemarksSDM"].ColumnName = "SDMRemarks";


    //                return dt;
    //            }
    //        }
    //        catch (Exception ex)
    //        {

    //            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //            throw;
    //        }
    //        finally
    //        {
    //            objData.CloseConnection();
    //        }


    //        return new DataTable();
    //    }




    //}

    public string CheckStatus()
    {
        SqlConnection con = new SqlConnection(G_connStr);
        con.Open();
        SqlCommand cmd = new SqlCommand("sp_Application_Status", con);
        SqlParameter ApplicationName = new SqlParameter("@ApplicationName", SqlDbType.VarChar);
        ApplicationName.Value = "BE App";
        SqlParameter outId = new SqlParameter("@Status", SqlDbType.VarChar) { Direction = ParameterDirection.Output };
        outId.Size = 50;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(ApplicationName);
        cmd.Parameters.Add(outId);
        cmd.ExecuteNonQuery();
        con.Close();
        return outId.Value.ToString();
    }
    public string UserPageAccessNSO(string UserId)
    {
        SqlConnection con = new SqlConnection(G_connStr);
        con.Open();
        SqlCommand cmd = new SqlCommand("spAccessNSO", con);
        SqlParameter ParamUserId = new SqlParameter("@UserId", SqlDbType.VarChar);
        ParamUserId.Value = UserId;
        SqlParameter outId = new SqlParameter("@PageAccess", SqlDbType.VarChar) { Direction = ParameterDirection.Output };
        outId.Size = 50;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(ParamUserId);
        cmd.Parameters.Add(outId);
        cmd.ExecuteNonQuery();
        con.Close();
        return outId.Value.ToString();
    }



    public void AddNewItem(out int BEID, string beType, string customerCode, string pu, string currency, string year, string currentQuarter, string createdBy, string role, string dm, string region)
    {
        currency = currency.Trim();
        BEID = 0;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        SqlParameter objintBEID;
        SqlParameter objtxtCreatedBy;
        SqlParameter objtxtYear;
        SqlParameter objtxtCurrentQuarterName;
        SqlParameter objtxtNativeCurrency;
        SqlParameter objtxtPU, objBeType;
        SqlParameter objtxtMasterClientCode, objRole, objDM, objtxtregion;

        objCommand = new SqlCommand();

        objintBEID = new SqlParameter();
        objintBEID.ParameterName = "@intBEID";
        objintBEID.Direction = ParameterDirection.Output;
        objintBEID.SqlDbType = SqlDbType.Int;
        //objintBEID.Size = 30;
        objintBEID.Value = BEID;

        objParamColl = objCommand.Parameters;
        objParamColl.Add(objintBEID);



        objDM = new SqlParameter();
        objDM.ParameterName = "@txtDM";
        objDM.Direction = ParameterDirection.Input;
        objDM.SqlDbType = SqlDbType.NVarChar;
        objDM.Size = 30;

        objParamColl = objCommand.Parameters;
        objDM.Value = dm;
        objParamColl.Add(objDM);


        objtxtCreatedBy = new SqlParameter();
        objtxtCreatedBy.ParameterName = "@txtUserId";
        objtxtCreatedBy.Direction = ParameterDirection.Input;
        objtxtCreatedBy.SqlDbType = SqlDbType.NVarChar;
        objtxtCreatedBy.Size = 50;
        objtxtCreatedBy.Value = createdBy;
        objParamColl.Add(objtxtCreatedBy);

        objtxtYear = new SqlParameter();
        objtxtYear.ParameterName = "@txtYear";
        objtxtYear.Direction = ParameterDirection.Input;
        objtxtYear.SqlDbType = SqlDbType.NVarChar;
        objtxtYear.Size = 50;
        objtxtYear.Value = year;
        objParamColl.Add(objtxtYear);

        objtxtCurrentQuarterName = new SqlParameter();
        objtxtCurrentQuarterName.ParameterName = "@txtCurrentQuarterName";
        objtxtCurrentQuarterName.Direction = ParameterDirection.Input;
        objtxtCurrentQuarterName.SqlDbType = SqlDbType.NVarChar;
        objtxtCurrentQuarterName.Size = 10;
        objtxtCurrentQuarterName.Value = currentQuarter;
        objParamColl.Add(objtxtCurrentQuarterName);

        objtxtNativeCurrency = new SqlParameter();
        objtxtNativeCurrency.ParameterName = "@txtNativeCurrency";
        objtxtNativeCurrency.Direction = ParameterDirection.Input;
        objtxtNativeCurrency.SqlDbType = SqlDbType.NVarChar;
        objtxtNativeCurrency.Size = 50;
        objtxtNativeCurrency.Value = currency;
        objParamColl.Add(objtxtNativeCurrency);


        objtxtPU = new SqlParameter();
        objtxtPU.ParameterName = "@txtPU";
        objtxtPU.Direction = ParameterDirection.Input;
        objtxtPU.SqlDbType = SqlDbType.NVarChar;
        objtxtPU.Size = 50;
        objtxtPU.Value = pu;
        objParamColl.Add(objtxtPU);

        objRole = new SqlParameter();
        objRole.ParameterName = "@txtRole";
        objRole.Direction = ParameterDirection.Input;
        objRole.SqlDbType = SqlDbType.NVarChar;
        objRole.Size = 50;
        objRole.Value = role;
        objParamColl.Add(objRole);



        objtxtMasterClientCode = new SqlParameter();
        objtxtMasterClientCode.ParameterName = "@txtMasterClientCode";
        objtxtMasterClientCode.Direction = ParameterDirection.Input;
        objtxtMasterClientCode.SqlDbType = SqlDbType.NVarChar;
        objtxtMasterClientCode.Size = 50;
        objtxtMasterClientCode.Value = customerCode;
        objParamColl.Add(objtxtMasterClientCode);


        objBeType = new SqlParameter();
        objBeType.ParameterName = "@txtBeType";
        objBeType.Direction = ParameterDirection.Input;
        objBeType.SqlDbType = SqlDbType.NVarChar;
        objBeType.Value = beType;
        objParamColl.Add(objBeType);


        objtxtregion = new SqlParameter();
        objtxtregion.ParameterName = "@region";
        objtxtregion.Direction = ParameterDirection.Input;
        objtxtregion.SqlDbType = SqlDbType.NVarChar;
        objtxtregion.Size = 50;
        objtxtregion.Value = region;
        objParamColl.Add(objtxtregion);

        objData = new DataAccess();
        objData.GetConnection();
        objData.ExecuteSP("dbo.spBEInsertData", objCommand);
        if (objParamColl["@intBEID"] != null)
        {
            BEID = Convert.ToInt32(objParamColl["@intBEID"].Value);
        }


    }


    public void AddNewVolumeItem(out int BEID, string customerCode, string pu, string year, string currentQuarter, string createdBy, string role, string dm)
    {

        BEID = 0;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        SqlParameter objintBEID;
        SqlParameter objtxtCreatedBy;
        SqlParameter objtxtYear;
        SqlParameter objtxtCurrentQuarterName;

        SqlParameter objtxtPU, objRemarks;
        SqlParameter objtxtMasterClientCode, objRole, objDm;

        objCommand = new SqlCommand();

        objintBEID = new SqlParameter();
        objintBEID.ParameterName = "@intBEID";
        objintBEID.Direction = ParameterDirection.Output;
        objintBEID.SqlDbType = SqlDbType.Int;
        //objintBEID.Size = 30;
        objintBEID.Value = BEID;

        objParamColl = objCommand.Parameters;
        objParamColl.Add(objintBEID);



        objDm = new SqlParameter();
        objDm.ParameterName = "@txtDM";
        objDm.Direction = ParameterDirection.Input;
        objDm.SqlDbType = SqlDbType.NVarChar;
        objDm.Size = 30;

        objParamColl = objCommand.Parameters;
        objDm.Value = dm;
        objParamColl.Add(objDm);


        objtxtCreatedBy = new SqlParameter();
        objtxtCreatedBy.ParameterName = "@txtUserId";
        objtxtCreatedBy.Direction = ParameterDirection.Input;
        objtxtCreatedBy.SqlDbType = SqlDbType.NVarChar;
        objtxtCreatedBy.Size = 50;
        objtxtCreatedBy.Value = createdBy;
        objParamColl.Add(objtxtCreatedBy);

        objtxtYear = new SqlParameter();
        objtxtYear.ParameterName = "@txtYear";
        objtxtYear.Direction = ParameterDirection.Input;
        objtxtYear.SqlDbType = SqlDbType.NVarChar;
        objtxtYear.Size = 50;
        objtxtYear.Value = year;
        objParamColl.Add(objtxtYear);

        objtxtCurrentQuarterName = new SqlParameter();
        objtxtCurrentQuarterName.ParameterName = "@txtCurrentQuarterName";
        objtxtCurrentQuarterName.Direction = ParameterDirection.Input;
        objtxtCurrentQuarterName.SqlDbType = SqlDbType.NVarChar;
        objtxtCurrentQuarterName.Size = 10;
        objtxtCurrentQuarterName.Value = currentQuarter;
        objParamColl.Add(objtxtCurrentQuarterName);

        //objtxtNativeCurrency = new SqlParameter();
        //objtxtNativeCurrency.ParameterName = "@txtNativeCurrency";
        //objtxtNativeCurrency.Direction = ParameterDirection.Input;
        //objtxtNativeCurrency.SqlDbType = SqlDbType.NVarChar;
        //objtxtNativeCurrency.Size = 50;
        //objtxtNativeCurrency.Value = currency;
        //objParamColl.Add(objtxtNativeCurrency);


        objtxtPU = new SqlParameter();
        objtxtPU.ParameterName = "@txtPU";
        objtxtPU.Direction = ParameterDirection.Input;
        objtxtPU.SqlDbType = SqlDbType.NVarChar;
        objtxtPU.Size = 50;
        objtxtPU.Value = pu;
        objParamColl.Add(objtxtPU);

        objRole = new SqlParameter();
        objRole.ParameterName = "@txtRole";
        objRole.Direction = ParameterDirection.Input;
        objRole.SqlDbType = SqlDbType.NVarChar;
        objRole.Size = 50;
        objRole.Value = role;
        objParamColl.Add(objRole);



        objtxtMasterClientCode = new SqlParameter();
        objtxtMasterClientCode.ParameterName = "@txtMasterClientCode";
        objtxtMasterClientCode.Direction = ParameterDirection.Input;
        objtxtMasterClientCode.SqlDbType = SqlDbType.NVarChar;
        objtxtMasterClientCode.Size = 50;
        objtxtMasterClientCode.Value = customerCode;
        objParamColl.Add(objtxtMasterClientCode);

        //objRemarks = new SqlParameter();
        //objRemarks.ParameterName = "@txtRemarks";
        //objRemarks.Direction = ParameterDirection.Input;
        //objRemarks.SqlDbType = SqlDbType.NVarChar;
        //objRemarks.Size = 50;
        //objRemarks.Value = remarks;
        //objParamColl.Add(objRemarks);




        objData = new DataAccess();
        objData.GetConnection();
        objData.ExecuteSP("dbo.spBEInsertDataVolume", objCommand);
        if (objParamColl["@intBEID"] != null)
        {
            BEID = Convert.ToInt32(objParamColl["@intBEID"].Value);
        }


    }


    public string GetUserRole(string userID)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;


        try
        {


            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@txtUserId";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userID;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;



            objParamColl.Add(objParamUserId);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBeReturnRole_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                return ds.Tables[0].Rows[0]["Role"] + "";




            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return "";
    }

    public string GetUserRoleNSO(string userID)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;


        try
        {


            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@txtUserId";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userID;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;



            objParamColl.Add(objParamUserId);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBeReturnRole_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                return ds.Tables[0].Rows[0]["Role"] + "";




            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return "";
    }



    public List<DUPUCCMap> GetPU(string userID, string SU)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm, objParm1;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<DUPUCCMap> lstempCollection = new List<DUPUCCMap>();
        DUPUCCMap empCollection;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtUserId";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = userID;

            objParm1 = new SqlParameter();
            objParm1.ParameterName = "@txtSU";
            objParm1.Direction = ParameterDirection.Input;
            objParm1.SqlDbType = SqlDbType.VarChar;
            objParm1.Value = SU;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            objParamColl.Add(objParm);
            objParamColl.Add(objParm1);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEPUListFromSU", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new DUPUCCMap();

                    // empCollection.DU = ds.Tables[0].Rows[i]["txtDU"].ToString();
                    empCollection.PU = ds.Tables[0].Rows[i]["txtPU"].ToString();
                    // empCollection.CustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                    lstempCollection.Add(empCollection);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }

    public List<DUPUCCMap> GetPUfromSU(string SU)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm, objParm1;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<DUPUCCMap> lstempCollection = new List<DUPUCCMap>();
        DUPUCCMap empCollection;
        try
        {


            objParm1 = new SqlParameter();
            objParm1.ParameterName = "@txtSU";
            objParm1.Direction = ParameterDirection.Input;
            objParm1.SqlDbType = SqlDbType.VarChar;
            objParm1.Value = SU;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            // objParamColl.Add(objParm);
            objParamColl.Add(objParm1);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spGetPUBeReportfromSU", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new DUPUCCMap();

                    // empCollection.DU = ds.Tables[0].Rows[i]["txtDU"].ToString();
                    empCollection.PU = ds.Tables[0].Rows[i]["txtPU"].ToString();
                    // empCollection.CustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                    lstempCollection.Add(empCollection);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }

    public List<DUPUCCMap> GetMapping(string userID)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<DUPUCCMap> lstempCollection = new List<DUPUCCMap>();
        DUPUCCMap empCollection;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtUserId";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = userID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_spBENSOListForDropDown", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new DUPUCCMap();

                    // empCollection.DU = ds.Tables[0].Rows[i]["txtDU"].ToString();
                    empCollection.PU = ds.Tables[0].Rows[i]["NSO"].ToString();
                    empCollection.NSOCOde = ds.Tables[0].Rows[i]["NSOCode"].ToString();
                    // empCollection.CustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                    lstempCollection.Add(empCollection);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }

    public DataSet GetMapping_1(string userID)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<DUPUCCMap> lstempCollection = new List<DUPUCCMap>();
        DUPUCCMap empCollection;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtUserId";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = userID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_spBENSOListForDropDown", ref ds, objCommand);
            return ds;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


      
    }


    public List<NSOCodeDescMapping> GetNSOCodeDescMapping()
    {

        DataSet ds = new DataSet();
        SqlParameter objParm;
        
        SqlParameterCollection objParamColl;
        List<NSOCodeDescMapping> lstMapping = new List<NSOCodeDescMapping>();

        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand =  new SqlCommand("select distinct  txtOfferingCode as NSOCode , txtOfferingCodeDesc as NSODesc   from DigitalOfferingSDMMapping ");


            var dt = objData.ExecuteSP(objCommand);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var map = new NSOCodeDescMapping();
                map.NSOCode = dt.Rows[i]["NSOCode"].ToString();
                map.NSODesc = dt.Rows[i]["NSODesc"].ToString();
                lstMapping.Add(map);
            }


        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstMapping;
    }


    public void UpdateRevenueGridItems(List<UPdateRevenueBE> lstItems, string userID, string role, string quartername)
    {
        foreach (var item in lstItems)
        {
            SqlCommand objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            SqlParameter param1, param2, param3, param5, param9, param10, param11, param13, param14, param15, param16, param17;

            param1 = new SqlParameter();
            param1.ParameterName = "@intBEId";
            param1.Direction = ParameterDirection.Input;
            param1.SqlDbType = SqlDbType.NVarChar;
            param1.Size = 50;
            param1.Value = item.BEID; ;
            objParamColl.Add(param1);

            param13 = new SqlParameter();
            param13.ParameterName = "@txtUserId";
            param13.Direction = ParameterDirection.Input;
            param13.SqlDbType = SqlDbType.NVarChar;
            param13.Size = 50;
            param13.Value = userID;
            objParamColl.Add(param13);

            param14 = new SqlParameter();
            param14.ParameterName = "@Role";
            param14.Direction = ParameterDirection.Input;
            param14.SqlDbType = SqlDbType.NVarChar;
            param14.Size = 50;
            param14.Value = role;
            objParamColl.Add(param14);



            param2 = new SqlParameter();
            param2.ParameterName = "@DMfltMonth1BE";
            param2.Direction = ParameterDirection.Input;
            param2.SqlDbType = SqlDbType.Float;
            param2.Value = item.DMfltMonth1BE; ;
            objParamColl.Add(param2);

            param3 = new SqlParameter();
            param3.ParameterName = "@DMfltMonth2BE";
            param3.Direction = ParameterDirection.Input;
            param3.SqlDbType = SqlDbType.Float;
            param3.Value = item.DMfltMonth2BE; ;
            objParamColl.Add(param3);




            param5 = new SqlParameter();
            param5.ParameterName = "@DMfltMonth3BE";
            param5.Direction = ParameterDirection.Input;
            param5.SqlDbType = SqlDbType.Float;
            param5.Value = item.DMfltMonth3BE; ;
            objParamColl.Add(param5);

            //TODO
            //param6 = new SqlParameter();
            //param6.ParameterName = "@DMfltNextQuarterBE";
            //param6.Direction = ParameterDirection.Input;
            //param6.SqlDbType = SqlDbType.Float;
            //param6.Value = item.DMfltNextQuarterBE; ;
            //objParamColl.Add(param6);



            //TODO:24/9 previous qtr removed
            //param8 = new SqlParameter();
            //param8.ParameterName = "@DMfltPrevQuarterBE";
            //param8.Direction = ParameterDirection.Input;
            //param8.SqlDbType = SqlDbType.Float;
            //param8.Value = item.DMfltPrevQuarterBE; ;
            //objParamColl.Add(param8);


            param9 = new SqlParameter();
            param9.ParameterName = "@SDMfltMonth1BE";
            param9.Direction = ParameterDirection.Input;
            param9.SqlDbType = SqlDbType.Float;
            param9.Value = item.SDMfltMonth1BE; ;
            objParamColl.Add(param9);

            param10 = new SqlParameter();
            param10.ParameterName = "@SDMfltMonth2BE";
            param10.Direction = ParameterDirection.Input;
            param10.SqlDbType = SqlDbType.Float;
            param10.Value = item.SDMfltMonth2BE; ;
            objParamColl.Add(param10);

            param11 = new SqlParameter();
            param11.ParameterName = "@SDMfltMonth3BE";
            param11.Direction = ParameterDirection.Input;
            param11.SqlDbType = SqlDbType.Float;
            param11.Value = item.SDMfltMonth3BE; ;
            objParamColl.Add(param11);

            //TODO
            //param12 = new SqlParameter();
            //param12.ParameterName = "@SDMfltNextQuarterBE";
            //param12.Direction = ParameterDirection.Input;
            //param12.SqlDbType = SqlDbType.Float;
            //param12.Value = item.SDMfltNextQuarterBE; ;
            //objParamColl.Add(param12);


            param15 = new SqlParameter();
            param15.ParameterName = "@txtRemarks";
            param15.Direction = ParameterDirection.Input;
            param15.SqlDbType = SqlDbType.NVarChar;
            //  param15.Size = 201;
            param15.Value = item.Remarks;
            objParamColl.Add(param15);

            param17 = new SqlParameter();
            param17.ParameterName = "@txtSDMRemarks";
            param17.Direction = ParameterDirection.Input;
            param17.SqlDbType = SqlDbType.NVarChar;
            //  param15.Size = 201;
            param17.Value = item.SDMRem;
            objParamColl.Add(param17);

            param16 = new SqlParameter();
            param16.ParameterName = "@QuarterName";
            param16.Direction = ParameterDirection.Input;
            param16.SqlDbType = SqlDbType.NVarChar;
            param16.Value = quartername;
            objParamColl.Add(param16);

            objParamColl = objCommand.Parameters;


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEUpdateData", objCommand);

        }
    }

    public void UpdateVolumeGridItems(List<UPdateBEVolume> lstItems, string userID, string role, string quartername)
    {
        foreach (var item in lstItems)
        {
            SqlCommand objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            SqlParameter param1, param2, param3, param5, param6, param8, param9, param10, param11, param12, param13, param14, param15,
                param16, param17, param18, param19, param20;




            param1 = new SqlParameter();
            param1.ParameterName = "@intBEId";
            param1.Direction = ParameterDirection.Input;
            param1.SqlDbType = SqlDbType.NVarChar;
            param1.Size = 50;
            param1.Value = item.intBEId; ;
            objParamColl.Add(param1);

            param13 = new SqlParameter();
            param13.ParameterName = "@txtUserId";
            param13.Direction = ParameterDirection.Input;
            param13.SqlDbType = SqlDbType.NVarChar;
            param13.Size = 50;
            param13.Value = userID;
            objParamColl.Add(param13);

            param14 = new SqlParameter();
            param14.ParameterName = "@Role";
            param14.Direction = ParameterDirection.Input;
            param14.SqlDbType = SqlDbType.NVarChar;
            param14.Size = 50;
            param14.Value = role;
            objParamColl.Add(param14);




            param2 = new SqlParameter();
            param2.ParameterName = "@fltDMEffortMonth1Onsite";
            param2.Direction = ParameterDirection.Input;
            param2.SqlDbType = SqlDbType.Float;
            param2.Value = item.fltDMEffortMonth1Onsite; ;
            objParamColl.Add(param2);

            param3 = new SqlParameter();
            param3.ParameterName = "@fltDMEffortMonth1OffShore";
            param3.Direction = ParameterDirection.Input;
            param3.SqlDbType = SqlDbType.Float;
            param3.Value = item.fltDMEffortMonth1OffShore; ;
            objParamColl.Add(param3);




            param5 = new SqlParameter();
            param5.ParameterName = "@fltDMEffortMonth2Onsite";
            param5.Direction = ParameterDirection.Input;
            param5.SqlDbType = SqlDbType.Float;
            param5.Value = item.fltDMEffortMonth2Onsite; ;
            objParamColl.Add(param5);


            param6 = new SqlParameter();
            param6.ParameterName = "@fltDMEffortMonth2Offshore";
            param6.Direction = ParameterDirection.Input;
            param6.SqlDbType = SqlDbType.Float;
            param6.Value = item.fltDMEffortMonth2Offshore;
            objParamColl.Add(param6);




            param8 = new SqlParameter();
            param8.ParameterName = "@fltDMEffortMonth3Onsite";
            param8.Direction = ParameterDirection.Input;
            param8.SqlDbType = SqlDbType.Float;
            param8.Value = item.fltDMEffortMonth3Onsite;
            objParamColl.Add(param8);


            param9 = new SqlParameter();
            param9.ParameterName = "@fltDMEffortMonth3Offshore";
            param9.Direction = ParameterDirection.Input;
            param9.SqlDbType = SqlDbType.Float;
            param9.Value = item.fltDMEffortMonth3Offshore;
            objParamColl.Add(param9);

            param10 = new SqlParameter();
            param10.ParameterName = "@fltSDMEffortMonth1Onsite";
            param10.Direction = ParameterDirection.Input;
            param10.SqlDbType = SqlDbType.Float;
            param10.Value = item.fltSDMEffortMonth1Onsite; ;
            objParamColl.Add(param10);

            param11 = new SqlParameter();
            param11.ParameterName = "@fltSDMEffortMonth1OffShore";
            param11.Direction = ParameterDirection.Input;
            param11.SqlDbType = SqlDbType.Float;
            param11.Value = item.fltSDMEffortMonth1OffShore; ;
            objParamColl.Add(param11);

            param12 = new SqlParameter();
            param12.ParameterName = "@fltSDMEffortMonth2Onsite";
            param12.Direction = ParameterDirection.Input;
            param12.SqlDbType = SqlDbType.Float;
            param12.Value = item.fltSDMEffortMonth2Onsite; ;
            objParamColl.Add(param12);


            param18 = new SqlParameter();
            param18.ParameterName = "@fltSDMEffortMonth2Offshore";
            param18.Direction = ParameterDirection.Input;
            param18.SqlDbType = SqlDbType.Float;
            param18.Value = item.fltSDMEffortMonth2Offshore; ;
            objParamColl.Add(param18);

            param16 = new SqlParameter();
            param16.ParameterName = "@fltSDMEffortMonth3Onsite";
            param16.Direction = ParameterDirection.Input;
            param16.SqlDbType = SqlDbType.Float;
            param16.Value = item.fltSDMEffortMonth3Onsite; ;
            objParamColl.Add(param16);

            param17 = new SqlParameter();
            param17.ParameterName = "@fltSDMEffortMonth3Offshore";
            param17.Direction = ParameterDirection.Input;
            param17.SqlDbType = SqlDbType.Float;
            param17.Value = item.fltSDMEffortMonth3Offshore; ;
            objParamColl.Add(param17);


            param15 = new SqlParameter();
            param15.ParameterName = "@RemarksDM";
            param15.Direction = ParameterDirection.Input;
            param15.SqlDbType = SqlDbType.NVarChar;
            param15.Value = item.DMRemarks;
            objParamColl.Add(param15);

            param20 = new SqlParameter();
            param20.ParameterName = "@RemarksSDM";
            param20.Direction = ParameterDirection.Input;
            param20.SqlDbType = SqlDbType.NVarChar;
            param20.Value = item.SDMRemarks;
            objParamColl.Add(param20);

            param19 = new SqlParameter();
            param19.ParameterName = "@QuarterName";
            param19.Direction = ParameterDirection.Input;
            param19.SqlDbType = SqlDbType.NVarChar;
            param19.Value = quartername;
            objParamColl.Add(param19);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBESaveVolume", objCommand);

        }
    }


    public void UpdateVolumeGridItemsDM(List<UPdateBEVolume> lstItems, string userID, string role, string quartername)
    {
        foreach (var item in lstItems)
        {
            SqlCommand objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            SqlParameter param1, param2, param3, param5, param6, param8, param9, param10, param11, param12, param13, param14, param15,
              param16, param17, param18, param19, param20;

            param1 = new SqlParameter();
            param1.ParameterName = "@intBEId";
            param1.Direction = ParameterDirection.Input;
            param1.SqlDbType = SqlDbType.NVarChar;
            param1.Size = 50;
            param1.Value = item.intBEId; ;
            objParamColl.Add(param1);

            param13 = new SqlParameter();
            param13.ParameterName = "@txtUserId";
            param13.Direction = ParameterDirection.Input;
            param13.SqlDbType = SqlDbType.NVarChar;
            param13.Size = 50;
            param13.Value = userID;
            objParamColl.Add(param13);

            param14 = new SqlParameter();
            param14.ParameterName = "@Role";
            param14.Direction = ParameterDirection.Input;
            param14.SqlDbType = SqlDbType.NVarChar;
            param14.Size = 50;
            param14.Value = role;
            objParamColl.Add(param14);




            param2 = new SqlParameter();
            param2.ParameterName = "@fltDMEffortMonth1Onsite";
            param2.Direction = ParameterDirection.Input;
            param2.SqlDbType = SqlDbType.Float;
            param2.Value = item.fltDMEffortMonth1Onsite; ;
            objParamColl.Add(param2);

            param3 = new SqlParameter();
            param3.ParameterName = "@fltDMEffortMonth1OffShore";
            param3.Direction = ParameterDirection.Input;
            param3.SqlDbType = SqlDbType.Float;
            param3.Value = item.fltDMEffortMonth1OffShore; ;
            objParamColl.Add(param3);




            param5 = new SqlParameter();
            param5.ParameterName = "@fltDMEffortMonth2Onsite";
            param5.Direction = ParameterDirection.Input;
            param5.SqlDbType = SqlDbType.Float;
            param5.Value = item.fltDMEffortMonth2Onsite; ;
            objParamColl.Add(param5);


            param6 = new SqlParameter();
            param6.ParameterName = "@fltDMEffortMonth2Offshore";
            param6.Direction = ParameterDirection.Input;
            param6.SqlDbType = SqlDbType.Float;
            param6.Value = item.fltDMEffortMonth2Offshore;
            objParamColl.Add(param6);




            param8 = new SqlParameter();
            param8.ParameterName = "@fltDMEffortMonth3Onsite";
            param8.Direction = ParameterDirection.Input;
            param8.SqlDbType = SqlDbType.Float;
            param8.Value = item.fltDMEffortMonth3Onsite;
            objParamColl.Add(param8);


            param9 = new SqlParameter();
            param9.ParameterName = "@fltDMEffortMonth3Offshore";
            param9.Direction = ParameterDirection.Input;
            param9.SqlDbType = SqlDbType.Float;
            param9.Value = item.fltDMEffortMonth3Offshore;
            objParamColl.Add(param9);

            param10 = new SqlParameter();
            param10.ParameterName = "@fltSDMEffortMonth1Onsite";
            param10.Direction = ParameterDirection.Input;
            param10.SqlDbType = SqlDbType.Float;
            param10.Value = item.fltSDMEffortMonth1Onsite; ;
            objParamColl.Add(param10);

            param11 = new SqlParameter();
            param11.ParameterName = "@fltSDMEffortMonth1OffShore";
            param11.Direction = ParameterDirection.Input;
            param11.SqlDbType = SqlDbType.Float;
            param11.Value = item.fltSDMEffortMonth1OffShore; ;
            objParamColl.Add(param11);

            param12 = new SqlParameter();
            param12.ParameterName = "@fltSDMEffortMonth2Onsite";
            param12.Direction = ParameterDirection.Input;
            param12.SqlDbType = SqlDbType.Float;
            param12.Value = item.fltSDMEffortMonth2Onsite; ;
            objParamColl.Add(param12);


            param18 = new SqlParameter();
            param18.ParameterName = "@fltSDMEffortMonth2Offshore";
            param18.Direction = ParameterDirection.Input;
            param18.SqlDbType = SqlDbType.Float;
            param18.Value = item.fltSDMEffortMonth2Offshore; ;
            objParamColl.Add(param18);

            param16 = new SqlParameter();
            param16.ParameterName = "@fltSDMEffortMonth3Onsite";
            param16.Direction = ParameterDirection.Input;
            param16.SqlDbType = SqlDbType.Float;
            param16.Value = item.fltSDMEffortMonth3Onsite; ;
            objParamColl.Add(param16);

            param17 = new SqlParameter();
            param17.ParameterName = "@fltSDMEffortMonth3Offshore";
            param17.Direction = ParameterDirection.Input;
            param17.SqlDbType = SqlDbType.Float;
            param17.Value = item.fltSDMEffortMonth3Offshore; ;
            objParamColl.Add(param17);


            param15 = new SqlParameter();
            param15.ParameterName = "@RemarksDM";
            param15.Direction = ParameterDirection.Input;
            param15.SqlDbType = SqlDbType.NVarChar;
            param15.Value = item.DMRemarks;
            objParamColl.Add(param15);

            param20 = new SqlParameter();
            param20.ParameterName = "@RemarksSDM";
            param20.Direction = ParameterDirection.Input;
            param20.SqlDbType = SqlDbType.NVarChar;
            param20.Value = item.SDMRemarks;
            objParamColl.Add(param20);

            param19 = new SqlParameter();
            param19.ParameterName = "@QuarterName";
            param19.Direction = ParameterDirection.Input;
            param19.SqlDbType = SqlDbType.NVarChar;
            param19.Value = quartername;
            objParamColl.Add(param19);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBESaveVolume", objCommand);

        }
    }



    //TODO:20/12
    //public void UpdateVolumeGridItems(List<UPdateBEVolume> lstItems, string userID, string role, string quartername)
    //{
    //    foreach (var item in lstItems)
    //    {
    //        SqlCommand objCommand = new SqlCommand();
    //        SqlParameterCollection objParamColl = objCommand.Parameters;


    //        SqlParameter param1, param2, param3, param5, param6, param8, param9, param10, param11, param12, param13, param14, param15,
    //            param16, param17, param18, param19, param20;




    //        param1 = new SqlParameter();
    //        param1.ParameterName = "@intBEId";
    //        param1.Direction = ParameterDirection.Input;
    //        param1.SqlDbType = SqlDbType.NVarChar;
    //        param1.Size = 50;
    //        param1.Value = item.intBEId; ;
    //        objParamColl.Add(param1);

    //        param13 = new SqlParameter();
    //        param13.ParameterName = "@txtUserId";
    //        param13.Direction = ParameterDirection.Input;
    //        param13.SqlDbType = SqlDbType.NVarChar;
    //        param13.Size = 50;
    //        param13.Value = userID;
    //        objParamColl.Add(param13);

    //        param14 = new SqlParameter();
    //        param14.ParameterName = "@Role";
    //        param14.Direction = ParameterDirection.Input;
    //        param14.SqlDbType = SqlDbType.NVarChar;
    //        param14.Size = 50;
    //        param14.Value = role;
    //        objParamColl.Add(param14);




    //        param2 = new SqlParameter();
    //        param2.ParameterName = "@fltDMEffortMonth1Onsite";
    //        param2.Direction = ParameterDirection.Input;
    //        param2.SqlDbType = SqlDbType.Float;
    //        param2.Value = item.fltDMEffortMonth1Onsite; ;
    //        objParamColl.Add(param2);

    //        param3 = new SqlParameter();
    //        param3.ParameterName = "@fltDMEffortMonth1OffShore";
    //        param3.Direction = ParameterDirection.Input;
    //        param3.SqlDbType = SqlDbType.Float;
    //        param3.Value = item.fltDMEffortMonth1OffShore; ;
    //        objParamColl.Add(param3);




    //        param5 = new SqlParameter();
    //        param5.ParameterName = "@fltDMEffortMonth2Onsite";
    //        param5.Direction = ParameterDirection.Input;
    //        param5.SqlDbType = SqlDbType.Float;
    //        param5.Value = item.fltDMEffortMonth2Onsite; ;
    //        objParamColl.Add(param5);


    //        param6 = new SqlParameter();
    //        param6.ParameterName = "@fltDMEffortMonth2Offshore";
    //        param6.Direction = ParameterDirection.Input;
    //        param6.SqlDbType = SqlDbType.Float;
    //        param6.Value = item.fltDMEffortMonth2Offshore;
    //        objParamColl.Add(param6);




    //        param8 = new SqlParameter();
    //        param8.ParameterName = "@fltDMEffortMonth3Onsite";
    //        param8.Direction = ParameterDirection.Input;
    //        param8.SqlDbType = SqlDbType.Float;
    //        param8.Value = item.fltDMEffortMonth3Onsite;
    //        objParamColl.Add(param8);


    //        param9 = new SqlParameter();
    //        param9.ParameterName = "@fltDMEffortMonth3Offshore";
    //        param9.Direction = ParameterDirection.Input;
    //        param9.SqlDbType = SqlDbType.Float;
    //        param9.Value = item.fltDMEffortMonth3Offshore;
    //        objParamColl.Add(param9);

    //        param10 = new SqlParameter();
    //        param10.ParameterName = "@fltSDMEffortMonth1Onsite";
    //        param10.Direction = ParameterDirection.Input;
    //        param10.SqlDbType = SqlDbType.Float;
    //        param10.Value = item.fltSDMEffortMonth1Onsite; ;
    //        objParamColl.Add(param10);

    //        param11 = new SqlParameter();
    //        param11.ParameterName = "@fltSDMEffortMonth1OffShore";
    //        param11.Direction = ParameterDirection.Input;
    //        param11.SqlDbType = SqlDbType.Float;
    //        param11.Value = item.fltSDMEffortMonth1OffShore; ;
    //        objParamColl.Add(param11);

    //        param12 = new SqlParameter();
    //        param12.ParameterName = "@fltSDMEffortMonth2Onsite";
    //        param12.Direction = ParameterDirection.Input;
    //        param12.SqlDbType = SqlDbType.Float;
    //        param12.Value = item.fltSDMEffortMonth2Onsite; ;
    //        objParamColl.Add(param12);


    //        param18 = new SqlParameter();
    //        param18.ParameterName = "@fltSDMEffortMonth2Offshore";
    //        param18.Direction = ParameterDirection.Input;
    //        param18.SqlDbType = SqlDbType.Float;
    //        param18.Value = item.fltSDMEffortMonth2Offshore; ;
    //        objParamColl.Add(param18);

    //        param16 = new SqlParameter();
    //        param16.ParameterName = "@fltSDMEffortMonth3Onsite";
    //        param16.Direction = ParameterDirection.Input;
    //        param16.SqlDbType = SqlDbType.Float;
    //        param16.Value = item.fltSDMEffortMonth3Onsite; ;
    //        objParamColl.Add(param16);

    //        param17 = new SqlParameter();
    //        param17.ParameterName = "@fltSDMEffortMonth3Offshore";
    //        param17.Direction = ParameterDirection.Input;
    //        param17.SqlDbType = SqlDbType.Float;
    //        param17.Value = item.fltSDMEffortMonth3Offshore; ;
    //        objParamColl.Add(param17);


    //        param15 = new SqlParameter();
    //        param15.ParameterName = "@RemarksDM";
    //        param15.Direction = ParameterDirection.Input;
    //        param15.SqlDbType = SqlDbType.NVarChar;
    //        param15.Value = item.DMRemarks;
    //        objParamColl.Add(param15);

    //        param20 = new SqlParameter();
    //        param20.ParameterName = "@RemarksSDM";
    //        param20.Direction = ParameterDirection.Input;
    //        param20.SqlDbType = SqlDbType.NVarChar;
    //        param20.Value = item.SDMRemarks;
    //        objParamColl.Add(param20);

    //        param19 = new SqlParameter();
    //        param19.ParameterName = "@QuarterName";
    //        param19.Direction = ParameterDirection.Input;
    //        param19.SqlDbType = SqlDbType.NVarChar;
    //        param19.Value = quartername;
    //        objParamColl.Add(param19);

    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("dbo.spBESaveVolume", objCommand);

    //    }
    //}



    public void UpdateExcelData(List<UpdateExcelData> lstItems)
    {
        foreach (var item in lstItems)
        {
            SqlCommand objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            SqlParameter param1, param2, param3, param5;
            //param6, param8, param9, param10, param11, param12, param13, param14, param15;

            param1 = new SqlParameter();
            param1.ParameterName = "@intBEId";
            param1.Direction = ParameterDirection.Input;
            param1.SqlDbType = SqlDbType.Int;
            //param1.Size = 50;
            param1.Value = item.BEID;
            objParamColl.Add(param1);

            //param13 = new SqlParameter();
            //param13.ParameterName = "@txtUserId";
            //param13.Direction = ParameterDirection.Input;
            //param13.SqlDbType = SqlDbType.NVarChar;
            //param13.Size = 50;
            //param13.Value = userID;
            //objParamColl.Add(param13);

            //param14 = new SqlParameter();
            //param14.ParameterName = "@Role";
            //param14.Direction = ParameterDirection.Input;
            //param14.SqlDbType = SqlDbType.NVarChar;
            //param14.Size = 50;
            //param14.Value = role;
            //objParamColl.Add(param14);

            param2 = new SqlParameter();
            param2.ParameterName = "@DMfltMonth1BE";
            param2.Direction = ParameterDirection.Input;
            param2.SqlDbType = SqlDbType.Float;
            param2.Value = item.DMfltMonth1BE; ;
            objParamColl.Add(param2);

            param3 = new SqlParameter();
            param3.ParameterName = "@DMfltMonth2BE";
            param3.Direction = ParameterDirection.Input;
            param3.SqlDbType = SqlDbType.Float;
            param3.Value = item.DMfltMonth2BE; ;
            objParamColl.Add(param3);

            param5 = new SqlParameter();
            param5.ParameterName = "@DMfltMonth3BE";
            param5.Direction = ParameterDirection.Input;
            param5.SqlDbType = SqlDbType.Float;
            param5.Value = item.DMfltMonth3BE; ;
            objParamColl.Add(param5);


            //param6 = new SqlParameter();
            //param6.ParameterName = "@fltDMEffortMonth1Onsite";
            //param6.Direction = ParameterDirection.Input;
            //param6.SqlDbType = SqlDbType.Float;
            //param6.Value = item.fltDMEffortMonth1Onsite; ;
            //objParamColl.Add(param6);

            //param8 = new SqlParameter();
            //param8.ParameterName = "@fltDMEffortMonth1OffShore";
            //param8.Direction = ParameterDirection.Input;
            //param8.SqlDbType = SqlDbType.Float;
            //param8.Value = item.fltDMEffortMonth1OffShore; ;
            //objParamColl.Add(param8);

            //param9 = new SqlParameter();
            //param9.ParameterName = "@fltDMEffortMonth2Onsite";
            //param9.Direction = ParameterDirection.Input;
            //param9.SqlDbType = SqlDbType.Float;
            //param9.Value = item.fltDMEffortMonth2Onsite; ;
            //objParamColl.Add(param9);


            //param10 = new SqlParameter();
            //param10.ParameterName = "@fltDMEffortMonth2Offshore";
            //param10.Direction = ParameterDirection.Input;
            //param10.SqlDbType = SqlDbType.Float;
            //param10.Value = item.fltDMEffortMonth2Offshore;
            //objParamColl.Add(param10);


            //param11 = new SqlParameter();
            //param11.ParameterName = "@fltDMEffortMonth3Onsite";
            //param11.Direction = ParameterDirection.Input;
            //param11.SqlDbType = SqlDbType.Float;
            //param11.Value = item.fltDMEffortMonth3Onsite;
            //objParamColl.Add(param11);


            //param12 = new SqlParameter();
            //param12.ParameterName = "@fltDMEffortMonth3Offshore";
            //param12.Direction = ParameterDirection.Input;
            //param12.SqlDbType = SqlDbType.Float;
            //param12.Value = item.fltDMEffortMonth3Offshore;
            //objParamColl.Add(param12);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEUploadBEExcelData", objCommand);

        }
    }

    public DataTable GetOpportunityList(string userid)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;


        try
        {


            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@UserId";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userid;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;



            objParamColl.Add(objParamUserId);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEFetchOpplist", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];



                return dt;
            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataTable();
    }




    public void ApproveGridItems(List<UPdateRevenueBE> lstItems)
    {
        foreach (var item in lstItems)
        {
            SqlCommand objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            SqlParameter param1, param2, param3, param5, param9, param10, param11;

            param1 = new SqlParameter();
            param1.ParameterName = "@intBEId";
            param1.Direction = ParameterDirection.Input;
            param1.SqlDbType = SqlDbType.NVarChar;
            param1.Size = 50;
            param1.Value = item.BEID; ;
            objParamColl.Add(param1);



            param2 = new SqlParameter();
            param2.ParameterName = "@DMfltMonth1BE";
            param2.Direction = ParameterDirection.Input;
            param2.SqlDbType = SqlDbType.Float;
            param2.Value = item.DMfltMonth1BE; ;
            objParamColl.Add(param2);

            param3 = new SqlParameter();
            param3.ParameterName = "@DMfltMonth2BE";
            param3.Direction = ParameterDirection.Input;
            param3.SqlDbType = SqlDbType.Float;
            param3.Value = item.DMfltMonth2BE; ;
            objParamColl.Add(param3);




            param5 = new SqlParameter();
            param5.ParameterName = "@DMfltMonth3BE";
            param5.Direction = ParameterDirection.Input;
            param5.SqlDbType = SqlDbType.Float;
            param5.Value = item.DMfltMonth3BE; ;
            objParamColl.Add(param5);

            //TODO
            //param6 = new SqlParameter();
            //param6.ParameterName = "@DMfltNextQuarterBE";
            //param6.Direction = ParameterDirection.Input;
            //param6.SqlDbType = SqlDbType.Float;
            //param6.Value = item.DMfltNextQuarterBE; ;
            //objParamColl.Add(param6);



            //TODO:24/9 previous qtr commented
            //param8 = new SqlParameter();
            //param8.ParameterName = "@DMfltPrevQuarterBE";
            //param8.Direction = ParameterDirection.Input;
            //param8.SqlDbType = SqlDbType.Float;
            //param8.Value = item.DMfltPrevQuarterBE; ;
            //objParamColl.Add(param8);


            param9 = new SqlParameter();
            param9.ParameterName = "@SDMfltMonth1BE";
            param9.Direction = ParameterDirection.Input;
            param9.SqlDbType = SqlDbType.Float;
            param9.Value = item.SDMfltMonth1BE; ;
            objParamColl.Add(param9);

            param10 = new SqlParameter();
            param10.ParameterName = "@SDMfltMonth2BE";
            param10.Direction = ParameterDirection.Input;
            param10.SqlDbType = SqlDbType.Float;
            param10.Value = item.SDMfltMonth2BE; ;
            objParamColl.Add(param10);

            param11 = new SqlParameter();
            param11.ParameterName = "@SDMfltMonth3BE";
            param11.Direction = ParameterDirection.Input;
            param11.SqlDbType = SqlDbType.Float;
            param11.Value = item.SDMfltMonth3BE; ;
            objParamColl.Add(param11);

            //TODO
            //param12 = new SqlParameter();
            //param12.ParameterName = "@SDMfltNextQuarterBE";
            //param12.Direction = ParameterDirection.Input;
            //param12.SqlDbType = SqlDbType.Float;
            //param12.Value = item.SDMfltNextQuarterBE; ;
            //objParamColl.Add(param12);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEApprove", objCommand);

        }
    }


    public void ApproveVolumeGridItems(List<ApproveBEVolume> lstItems)
    {
        foreach (var item in lstItems)
        {
            SqlCommand objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            SqlParameter param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13;

            param1 = new SqlParameter();
            param1.ParameterName = "@intBEId";
            param1.Direction = ParameterDirection.Input;
            param1.SqlDbType = SqlDbType.Int;
            param1.Value = item.intBEId;
            objParamColl.Add(param1);



            param2 = new SqlParameter();
            param2.ParameterName = "@fltDMEffortMonth1Onsite";
            param2.Direction = ParameterDirection.Input;
            param2.SqlDbType = SqlDbType.Float;
            param2.Value = item.fltDMEffortMonth1Onsite;
            objParamColl.Add(param2);

            param3 = new SqlParameter();
            param3.ParameterName = "@fltDMEffortMonth1Offshore";
            param3.Direction = ParameterDirection.Input;
            param3.SqlDbType = SqlDbType.Float;
            param3.Value = item.fltDMEffortMonth1OffShore;
            objParamColl.Add(param3);




            param4 = new SqlParameter();
            param4.ParameterName = "@fltDMEffortMonth2Onsite";
            param4.Direction = ParameterDirection.Input;
            param4.SqlDbType = SqlDbType.Float;
            param4.Value = item.fltDMEffortMonth2Onsite;
            objParamColl.Add(param4);

            //TODO
            param5 = new SqlParameter();
            param5.ParameterName = "@fltDMEffortMonth2Offshore";
            param5.Direction = ParameterDirection.Input;
            param5.SqlDbType = SqlDbType.Float;
            param5.Value = item.fltDMEffortMonth2Offshore;
            objParamColl.Add(param5);

            param6 = new SqlParameter();
            param6.ParameterName = "@fltDMEffortMonth3Onsite";
            param6.Direction = ParameterDirection.Input;
            param6.SqlDbType = SqlDbType.Float;
            param6.Value = item.fltDMEffortMonth3Onsite;
            objParamColl.Add(param6);

            param7 = new SqlParameter();
            param7.ParameterName = "@fltDMEffortMonth3Offshore";
            param7.Direction = ParameterDirection.Input;
            param7.SqlDbType = SqlDbType.Float;
            param7.Value = item.fltDMEffortMonth3Offshore;
            objParamColl.Add(param7);

            param8 = new SqlParameter();
            param8.ParameterName = "@fltSDMEffortMonth1Onsite";
            param8.Direction = ParameterDirection.Input;
            param8.SqlDbType = SqlDbType.Float;
            param8.Value = item.fltSDMEffortMonth1Onsite; ;
            objParamColl.Add(param8);


            param9 = new SqlParameter();
            param9.ParameterName = "@fltSDMEffortMonth1Offshore";
            param9.Direction = ParameterDirection.Input;
            param9.SqlDbType = SqlDbType.Float;
            param9.Value = item.fltSDMEffortMonth1OffShore; ;
            objParamColl.Add(param9);

            param10 = new SqlParameter();
            param10.ParameterName = "@fltSDMEffortMonth2Offshore";
            param10.Direction = ParameterDirection.Input;
            param10.SqlDbType = SqlDbType.Float;
            param10.Value = item.fltSDMEffortMonth2Offshore;
            objParamColl.Add(param10);

            param11 = new SqlParameter();
            param11.ParameterName = "@fltSDMEffortMonth2Onsite";
            param11.Direction = ParameterDirection.Input;
            param11.SqlDbType = SqlDbType.Float;
            param11.Value = item.fltSDMEffortMonth2Onsite; ;
            objParamColl.Add(param11);


            param12 = new SqlParameter();
            param12.ParameterName = "@fltSDMEffortMonth3Offshore";
            param12.Direction = ParameterDirection.Input;
            param12.SqlDbType = SqlDbType.Float;
            param12.Value = item.fltSDMEffortMonth3Offshore;
            objParamColl.Add(param12);

            param13 = new SqlParameter();
            param13.ParameterName = "@fltSDMEffortMonth3Onsite";
            param13.Direction = ParameterDirection.Input;
            param13.SqlDbType = SqlDbType.Float;
            param13.Value = item.fltSDMEffortMonth3Onsite;
            objParamColl.Add(param13);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEApproveVolume", objCommand);

        }
    }



    public List<string> GetCustomerCode()
    {

        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<string> lstempCollection = new List<string>();

        try
        {

            objCommand = new SqlCommand();

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEFetchCustomerListforDropDown", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    string tmp = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                    lstempCollection.Add(tmp);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }

    //public List<string> GetCustomerCodeForBEtype(string Betype)
    //{


    //    DataSet ds = new DataSet();

    //    SqlCommand objCommand;

    //    List<string> lstempCollection = new List<string>();

    //    try
    //    {

    //        objCommand = new SqlCommand();


    //        SqlParameter objBE = new SqlParameter();
    //        objBE.ParameterName = "@BEType";
    //        objBE.Direction = ParameterDirection.Input;
    //        objBE.SqlDbType = SqlDbType.VarChar;
    //        objBE.Value = Betype;


    //        objCommand = new SqlCommand();
    //        SqlParameterCollection objParamColl = objCommand.Parameters;
    //        objParamColl.Add(objBE);

    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        // objData.ExecuteSP("spBEDUListForDropDown", ref ds, objCommand);
    //        objData.ExecuteSP("spGetCustomerCodeForBEtype", ref ds, objCommand);

    //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
    //        {
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {
    //                // empCollection = new DUPUCCMap();
    //                string tmp = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

    //                lstempCollection.Add(tmp);
    //            }

    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }


    //    return lstempCollection;
    //}
    public List<string> GetDUPOPUP(string userID, string pu)
    {


        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;

        List<string> lstempCollection = new List<string>();

        try
        {

            objCommand = new SqlCommand();


            SqlParameter objPU = new SqlParameter();
            objPU.ParameterName = "@txtUserID";
            objPU.Direction = ParameterDirection.Input;
            objPU.SqlDbType = SqlDbType.VarChar;
            objPU.Value = userID;


            //SqlParameter objCC = new SqlParameter();
            //objCC.ParameterName = "@txtMasterCustomerCode";
            //objCC.Direction = ParameterDirection.Input;
            //objCC.SqlDbType = SqlDbType.VarChar;
            //objCC.Value = customerCode;

            SqlParameter objpu = new SqlParameter();
            objpu.ParameterName = "@txtPU";
            objpu.Direction = ParameterDirection.Input;
            objpu.SqlDbType = SqlDbType.VarChar;
            objpu.Value = pu;





            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objPU);
            //objParamColl.Add(objCC);
            objParamColl.Add(objpu);






            objData = new DataAccess();
            objData.GetConnection();
            // objData.ExecuteSP("spBEDUListForDropDown", ref ds, objCommand);
            objData.ExecuteSP("spBEDMListForDropDown", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    string tmp = ds.Tables[0].Rows[i]["txtDMMailId"].ToString();

                    lstempCollection.Add(tmp);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }

    public List<string> GetDMMailList(string userID, string pu, string role)
    {


        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;

        List<string> lstempCollection = new List<string>();

        try
        {

            objCommand = new SqlCommand();


            SqlParameter objPU = new SqlParameter();
            objPU.ParameterName = "@txtUserID";
            objPU.Direction = ParameterDirection.Input;
            objPU.SqlDbType = SqlDbType.VarChar;
            objPU.Value = userID;



            SqlParameter objpu = new SqlParameter();
            objpu.ParameterName = "@txtPU";
            objpu.Direction = ParameterDirection.Input;
            objpu.SqlDbType = SqlDbType.VarChar;
            objpu.Value = pu;

            SqlParameter objrole = new SqlParameter();
            objrole.ParameterName = "@txtRole";
            objrole.Direction = ParameterDirection.Input;
            objrole.SqlDbType = SqlDbType.VarChar;
            objrole.Value = role;

            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objPU);

            objParamColl.Add(objpu);
            objParamColl.Add(objrole);


            objData = new DataAccess();
            objData.GetConnection();
            // objData.ExecuteSP("spBEDUListForDropDown", ref ds, objCommand);
            objData.ExecuteSP("spBEDMListForRevenuePop", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    string tmp = ds.Tables[0].Rows[i]["txtDMMailId"].ToString();

                    lstempCollection.Add(tmp);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }

    public List<string> GetCurrency(string pu)
    {





        DataSet ds = new DataSet();

        SqlCommand objCommand;

        List<string> lstempCollection = new List<string>();

        try
        {

            objCommand = new SqlCommand();


            SqlParameter objPU = new SqlParameter();
            objPU.ParameterName = "@txtPU";
            objPU.Direction = ParameterDirection.Input;
            objPU.SqlDbType = SqlDbType.NVarChar;
            objPU.Value = pu;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objPU);






            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEFetchCurrencyListforDropDown", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    string tmp = ds.Tables[0].Rows[i]["txtCurrency"].ToString();

                    lstempCollection.Add(tmp);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw; logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;






    }

    public void UpdateBEVolume(int beid, double DMEffortMonth1OffShore, double DMEffortMonth1Onsite, double DMEffortMonth2OffShore, double DMEffortMonth2Onsite,
        double DMEffortMonth3OffShore, double DMEffortMonth3Onsite, string dm, string DMRem)
    {

        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8, sqlparam9;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@BEID";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.Value = beid;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@DMEffortMonth1Onsite";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.Value = DMEffortMonth1Onsite;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "DMEffortMonth1OffShore";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.Value = DMEffortMonth1OffShore;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@DMEffortMonth2Onsite";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.Value = DMEffortMonth2Onsite;
            objParamColl.Add(sqlparam4);


            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@DMEffortMonth2Offshore";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.Value = DMEffortMonth2OffShore;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@DMEffortMonth3Onsite";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.Value = DMEffortMonth3Onsite;
            objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@DMEffortMonth3Offshore";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.Value = DMEffortMonth3OffShore;
            objParamColl.Add(sqlparam7);

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@DM";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.Value = dm;
            objParamColl.Add(sqlparam8);

            sqlparam9 = new SqlParameter();
            sqlparam9.ParameterName = "@DMRemarks";
            sqlparam9.Direction = ParameterDirection.Input;
            sqlparam9.Value = DMRem;
            objParamColl.Add(sqlparam9);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEVolUploadBE", objCommand);
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }



    }
    public void UpdateBEDAtaDeleteIT(int beid, double dmmonht1, double dmmonth2, double dmmonth3, string dmrem, string dm)
    {

        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@Beid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.Value = beid;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@DmMnth1";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.Value = dmmonht1;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@DmMnth2";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.Value = dmmonth2;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@DmMnth3";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.Value = dmmonth3;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@DMrem";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.Value = dmrem;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@DM";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.Value = dm;
            objParamColl.Add(sqlparam6);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEUploadBEData1", objCommand);
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }



    }

    /// <summary>
    /// TODO : 7 nov Curd Screen
    /// </summary>
    /// <returns>DataTable</returns>
    public DataTable FetchCurrencyConversion(DateTime date)
    {
        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@dateparam";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = date;
            objParamColl.Add(sqlparam1);
            objData.ExecuteSP("dbo.spBEFetchCurrencyConversion", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        //return new DataTable();
    }

    /// <summary>
    /// TODO : 7 nov Curd Screen
    /// </summary>
    /// <returns>DataTable</returns>
    public void InsertCurrencyConversion(string NativeCurrency, string BaselineCurrency, double CurrConvRate, DateTime dt)
    {
        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam4, sqlparam5;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@NativeCurr";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = NativeCurrency;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@BaseCurr";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = BaselineCurrency;
            objParamColl.Add(sqlparam2);


            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@CurrCon";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.Float;
            sqlparam4.Value = CurrConvRate;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@dt";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.DateTime;
            sqlparam5.Value = dt;
            objParamColl.Add(sqlparam5);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEInserCurrencyConversion", objCommand);
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }
    public int AddNewCurrConvRate(string NativeCurrency, string BaselineCurrency, double CurrConvRate, DateTime dt)
    {
        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam4, sqlparam5, sqlparam6;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@NativeCurr";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = NativeCurrency;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@BaseCurr";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = BaselineCurrency;
            objParamColl.Add(sqlparam2);



            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@CurrCon";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.Float;
            sqlparam4.Value = CurrConvRate;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@dt";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.DateTime;
            sqlparam5.Value = dt;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@retValue";
            sqlparam6.Direction = ParameterDirection.ReturnValue;
            sqlparam6.SqlDbType = SqlDbType.Int;
            objParamColl.Add(sqlparam6);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEAddNewCurrencyConversion", objCommand);

            var ret = Convert.ToInt32(sqlparam6.Value);

            return ret;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }
    /// <summary>
    /// TODO : 7 nov Curd Screen
    /// </summary>
    /// <returns>DataTable</returns>
    public DataTable FetchCurrencyConversionDate()
    {
        DataTable dtCurrConv = new DataTable();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            sqlcmd.CommandText = "spBEFetchCurrencyConversionDates";
            sqlcmd.CommandType = CommandType.StoredProcedure;
            dtCurrConv = objData.ExecuteSP(sqlcmd);

            return dtCurrConv;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }


    /// <summary>
    /// TODO : 27 nov Monthly Conversion
    /// </summary>
    /// <returns>DataTable</returns>
    public DataTable FetchMonthYearForMonthlyConversion(string monthoryear)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@monthoryear";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = monthoryear;
            objParamColl.Add(sqlparam1);
            objData.ExecuteSP("dbo.EAS_SP_BEGetMonthYear", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        //return new DataTable();
    }


    public DataTable FetchYearForMonthlyConversion()
    {
        DataTable dtCurrConv = new DataTable();
        SqlCommand sqlcmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            sqlcmd.CommandText = "EAS_SP_BEGetYear";
            sqlcmd.CommandType = CommandType.StoredProcedure;
            dtCurrConv = objData.ExecuteSP(sqlcmd);
            return dtCurrConv;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public DataTable FetchMonthForMonthlyConversion(string year)
    {
        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;
            SqlParameter sqlparam1;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtyr";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = year;
            objParamColl.Add(sqlparam1);
            objData.ExecuteSP("dbo.[EAS_SP_BEGetMonth]", ref  dsCurrConv, objCommand);
            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    /// <summary>
    /// TODO : 27 nov Monthly Conversion
    /// </summary>
    /// <returns>DataTable</returns>
    public DataTable FetchMonthlyCurrencyConversion(string month, string year)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@month";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = month;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@year";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);

            objData.ExecuteSP("dbo.EAS_SP_FetchMonthlyCurrencyConversion", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        //return new DataTable();
    }

    /// <summary>
    /// TODO : 26 nov monthly currency conversion
    /// </summary>
    /// <param name="lstItems"></param>
    /// <param name="userID"></param>
    /// <param name="role"></param>
    /// <param name="quartername"></param>
    public void UpdateMonthlyCurrencyConversion(List<MonthlyCurrencyConversion> lstItems)
    {
        foreach (var item in lstItems)
        {
            SqlCommand objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            SqlParameter param1, param2, param4, param5, param6, param7, param8;

            param1 = new SqlParameter();
            param1.ParameterName = "@nativeCurr";
            param1.Direction = ParameterDirection.Input;
            param1.SqlDbType = SqlDbType.VarChar;
            param1.Size = 50;
            param1.Value = item.nativeCurrency; ;
            objParamColl.Add(param1);

            param2 = new SqlParameter();
            param2.ParameterName = "@guidrate";
            param2.Direction = ParameterDirection.Input;
            param2.SqlDbType = SqlDbType.Float;
            param2.Value = item.guidanceRate; ;
            objParamColl.Add(param2);

            //param3 = new SqlParameter();
            //param3.ParameterName = "@actualrate";
            //param3.Direction = ParameterDirection.Input;
            //param3.SqlDbType = SqlDbType.Float;
            //param3.Value = item.actualRate; ;
            //objParamColl.Add(param3);

            param6 = new SqlParameter();
            param6.ParameterName = "@actualratemonth1";
            param6.Direction = ParameterDirection.Input;
            param6.SqlDbType = SqlDbType.Float;
            param6.Value = item.actualRateMonth1; ;
            objParamColl.Add(param6);


            param7 = new SqlParameter();
            param7.ParameterName = "@actualratemonth2";
            param7.Direction = ParameterDirection.Input;
            param7.SqlDbType = SqlDbType.Float;
            param7.Value = item.actualRateMonth2; ;
            objParamColl.Add(param7);


            param8 = new SqlParameter();
            param8.ParameterName = "@actualratemonth3";
            param8.Direction = ParameterDirection.Input;
            param8.SqlDbType = SqlDbType.Float;
            param8.Value = item.actualRateMonth3; ;
            objParamColl.Add(param8);

            param4 = new SqlParameter();
            param4.ParameterName = "@month";
            param4.Direction = ParameterDirection.Input;
            param4.SqlDbType = SqlDbType.VarChar;
            param4.Value = item.month; ;
            objParamColl.Add(param4);

            param5 = new SqlParameter();
            param5.ParameterName = "@year";
            param5.Direction = ParameterDirection.Input;
            param5.SqlDbType = SqlDbType.VarChar;
            param5.Value = item.year; ;
            objParamColl.Add(param5);

            objParamColl = objCommand.Parameters;

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.EAS_SP_BEUpdateMonthlyCurrencyConversion", objCommand);

        }
    }

    public void DeleteMonthlyCurrencyConversion(List<MonthlyCurrencyConversion> lstItems)
    {
        foreach (var item in lstItems)
        {
            SqlCommand objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            SqlParameter param1, param2, param4, param5, param6, param7, param8;

            param1 = new SqlParameter();
            param1.ParameterName = "@nativeCurr";
            param1.Direction = ParameterDirection.Input;
            param1.SqlDbType = SqlDbType.VarChar;
            param1.Size = 50;
            param1.Value = item.nativeCurrency; ;
            objParamColl.Add(param1);

            param4 = new SqlParameter();
            param4.ParameterName = "@month";
            param4.Direction = ParameterDirection.Input;
            param4.SqlDbType = SqlDbType.VarChar;
            param4.Value = item.month; ;
            objParamColl.Add(param4);

            param5 = new SqlParameter();
            param5.ParameterName = "@year";
            param5.Direction = ParameterDirection.Input;
            param5.SqlDbType = SqlDbType.VarChar;
            param5.Value = item.year; ;
            objParamColl.Add(param5);

            objParamColl = objCommand.Parameters;

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.[EAS_SP_BEDeleteMonthlyCurrencyConversion]", objCommand);

        }
    }

    /// <summary>
    /// TODO : 26 nov monthly currency conversion screen
    /// </summary>
    /// <returns></returns>
    public DataTable GetPopUpYear()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("year", typeof(string));
        string year = DateTime.Today.Year.ToString();
        string prevyear = (Convert.ToInt32(year) - 1).ToString();
        string nextyear = (Convert.ToInt32(year) + 1).ToString();
        dt.Rows.Add(prevyear);
        dt.Rows.Add(year);
        dt.Rows.Add(nextyear);
        return dt;
    }

    public int AddNewMonthlyConvRate(string month, string year)
    {
        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam6;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@month";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = month;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@year";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@retValue";
            sqlparam6.Direction = ParameterDirection.ReturnValue;
            sqlparam6.SqlDbType = SqlDbType.Int;
            objParamColl.Add(sqlparam6);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.EAS_SP_BEAddNewMonthlyCurrConv", objCommand);

            var ret = Convert.ToInt32(sqlparam6.Value);

            return ret;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public int AddNewMonthlyConvRateNew(string month, string year, string Currency)
    {
        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam6;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@month";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = month;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@year";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@Currency";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = Currency;
            objParamColl.Add(sqlparam3);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@retValue";
            sqlparam6.Direction = ParameterDirection.ReturnValue;
            sqlparam6.SqlDbType = SqlDbType.Int;
            objParamColl.Add(sqlparam6);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.EAS_SP_BEAddNewMonthlyCurrConvNew", objCommand);

            var ret = Convert.ToInt32(sqlparam6.Value);

            return ret;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }
    public List<RetrieveUI> RetreiveDetails(string txtSBUCode, string txtSDMMailId, string UserID)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<RetrieveUI> objList = new List<RetrieveUI>();
        RetrieveUI objRetrieve = null;
        objCommand = new SqlCommand();
        SqlParameter objSBUCode = new SqlParameter();
        objSBUCode.Direction = ParameterDirection.Input;
        objCommand.Parameters.Add(objSBUCode);
        objSBUCode.SqlDbType = SqlDbType.VarChar;
        objSBUCode.ParameterName = "@txtSBUCode";
        objSBUCode.Value = txtSBUCode;

        SqlParameter objSDMMailId = new SqlParameter();
        objSDMMailId.Direction = ParameterDirection.Input;
        objCommand.Parameters.Add(objSDMMailId);
        objSDMMailId.SqlDbType = SqlDbType.VarChar;
        objSDMMailId.ParameterName = "@txtSDMMailId";
        objSDMMailId.Value = txtSDMMailId;

        SqlParameterCollection objParameterCollection = objCommand.Parameters;
        //objParameterCollection.Add(objSBUCode);
        // objParameterCollection.Add(objSDMMailId);
        objRetrieve = new RetrieveUI();

        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEDmSdmMapRetrieve", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objRetrieve = new RetrieveUI();
                    objRetrieve.intSNo = Convert.ToInt16(ds.Tables[0].Rows[i]["intSNo"]);
                    objRetrieve.txtSBUCode = ds.Tables[0].Rows[i]["txtSBUCode"] + "";
                    objRetrieve.txtDMMailId = ds.Tables[0].Rows[i]["txtDMMailId"] + "";
                    objRetrieve.txtSDMMailId = ds.Tables[0].Rows[i]["txtSDMMailId"] + "";
                    objRetrieve.txtDHMailId = ds.Tables[0].Rows[i]["txtDHMailId"] + "";
                    objRetrieve.txtBUCode = ds.Tables[0].Rows[i]["txtBUCode"] + "";
                    objRetrieve.txtUpdatedBy = ds.Tables[0].Rows[i]["txtUpdatedBy"] + "";
                    objRetrieve.txtUpdateDt = ds.Tables[0].Rows[i]["txtUpdateDt"] + "";
                    objRetrieve.txtBITSCSIHMailId = ds.Tables[0].Rows[i]["txtBITSCSIHMailId"] + "";
                    objRetrieve.txtUHMailId = ds.Tables[0].Rows[i]["txtUHMailId"] + "";
                    objRetrieve.txtVertical = ds.Tables[0].Rows[i]["txtVertical"] + "";
                    objRetrieve.txtPortfolio = ds.Tables[0].Rows[i]["txtPortfolio"] + "";
                    objRetrieve.lstPU = GetAllPUs(UserID);
                    objList.Add(objRetrieve);
                }
            }
        }
        catch (Exception ex)
        {
            //Logger.LogErrorToServer(App_Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return objList;
    }
    public List<string> GetSUDmSdmMap(string txtUserId)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;

        List<string> lstempCollection = new List<string>();
        try
        {

            objCommand = new SqlCommand();
            SqlParameter objSU = new SqlParameter();
            objSU.ParameterName = "@txtUserID";
            objSU.Direction = ParameterDirection.Input;
            objSU.SqlDbType = SqlDbType.VarChar;
            objSU.Value = txtUserId;

            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objSU);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEDMSDMMAPGetSU", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string quarter = string.Empty;

                    quarter = ds.Tables[0].Rows[i]["txtSU"].ToString();
                    lstempCollection.Add(quarter);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection;
    }


    public int UpdateDetails(int intSNo, string txtSBUCode, string txtDMMailId, string txtSDMMailId, string txtDHMailId, string txtBUCode, string txtUpdatedBy, string txtBITSCSIHMailId, string txtUHMailId, string txtVertical, string txtPortfolio)
    {
        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            SqlParameter objParamSNo = new SqlParameter();
            objParamSNo.ParameterName = "@intSNo";
            objParamSNo.Direction = ParameterDirection.Input;
            objParamSNo.SqlDbType = SqlDbType.Int;
            objParamSNo.Value = intSNo;
            objParamColl.Add(objParamSNo);

            SqlParameter objParamSBUCode = new SqlParameter();
            objParamSBUCode.ParameterName = "@txtSBUCode";
            objParamSBUCode.Direction = ParameterDirection.Input;
            objParamSBUCode.SqlDbType = SqlDbType.NVarChar;
            objParamSBUCode.Value = txtSBUCode;
            objParamColl.Add(objParamSBUCode);

            SqlParameter objParamDMMailId = new SqlParameter();
            objParamDMMailId.ParameterName = "@txtDMMailId";
            objParamDMMailId.Direction = ParameterDirection.Input;
            objParamDMMailId.SqlDbType = SqlDbType.NVarChar;
            objParamDMMailId.Value = txtDMMailId;
            objParamColl.Add(objParamDMMailId);

            SqlParameter objParamSDMMailId = new SqlParameter();
            objParamSDMMailId.ParameterName = "@txtSDMMailId";
            objParamSDMMailId.Direction = ParameterDirection.Input;
            objParamSDMMailId.SqlDbType = SqlDbType.NVarChar;
            objParamSDMMailId.Value = txtSDMMailId;
            objParamColl.Add(objParamSDMMailId);

            SqlParameter objParamDHMailId = new SqlParameter();
            objParamDHMailId.ParameterName = "@txtDHMailId";
            objParamDHMailId.Direction = ParameterDirection.Input;
            objParamDHMailId.SqlDbType = SqlDbType.NVarChar;
            objParamDHMailId.Value = txtDHMailId;
            objParamColl.Add(objParamDHMailId);

            SqlParameter objParamBUCode = new SqlParameter();
            objParamBUCode.ParameterName = "@txtBUCode";
            objParamBUCode.Direction = ParameterDirection.Input;
            objParamBUCode.SqlDbType = SqlDbType.NVarChar;
            objParamBUCode.Value = txtBUCode;
            objParamColl.Add(objParamBUCode);

            SqlParameter objParamUpdatedBy = new SqlParameter();
            objParamUpdatedBy.ParameterName = "@txtUpdatedBy";
            objParamUpdatedBy.Direction = ParameterDirection.Input;
            objParamUpdatedBy.SqlDbType = SqlDbType.NVarChar;
            objParamUpdatedBy.Value = txtUpdatedBy;
            objParamColl.Add(objParamUpdatedBy);

            SqlParameter objParamBITSCISHMailId = new SqlParameter();
            objParamBITSCISHMailId.ParameterName = "@txtBITSCSIHMailId";
            objParamBITSCISHMailId.Direction = ParameterDirection.Input;
            objParamBITSCISHMailId.SqlDbType = SqlDbType.NVarChar;
            objParamBITSCISHMailId.Value = txtBITSCSIHMailId;
            objParamColl.Add(objParamBITSCISHMailId);

            SqlParameter objParamUHMailId = new SqlParameter();
            objParamUHMailId.ParameterName = "@txtUHMailId";
            objParamUHMailId.Direction = ParameterDirection.Input;
            objParamUHMailId.SqlDbType = SqlDbType.NVarChar;
            objParamUHMailId.Value = txtUHMailId;
            objParamColl.Add(objParamUHMailId);

            SqlParameter objParamVertical = new SqlParameter();
            objParamVertical.ParameterName = "@txtVertical";
            objParamVertical.Direction = ParameterDirection.Input;
            objParamVertical.SqlDbType = SqlDbType.NVarChar;
            objParamVertical.Value = txtVertical;
            objParamColl.Add(objParamVertical);

            SqlParameter objParamPortfolio = new SqlParameter();
            objParamPortfolio.ParameterName = "@txtPortfolio";
            objParamPortfolio.Direction = ParameterDirection.Input;
            objParamPortfolio.SqlDbType = SqlDbType.NVarChar;
            objParamPortfolio.Value = txtPortfolio;
            objParamColl.Add(objParamPortfolio);

            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.Direction = ParameterDirection.ReturnValue;
            objParamStatus.SqlDbType = SqlDbType.Int;
            objParamStatus.ParameterName = "ReturnValue";
            objParamColl.Add(objParamStatus);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEDmSdmMapUpdateDetails", objCommand);

            int updatestatus = (int)objCommand.Parameters["ReturnValue"].Value;
            return updatestatus;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }
    /// <summary>
    /// Shobana(dont delete)
    /// </summary>
    //public List<RetrieveUI> RetreiveDetails(string txtSBUCode, string txtSDMMailId)
    //{

    //    DataSet ds = new DataSet();
    //    SqlCommand objCommand;
    //    List<RetrieveUI> objList = new List<RetrieveUI>();
    //    RetrieveUI objRetrieve = null;

    //    objCommand = new SqlCommand();
    //    SqlParameter objSBUCode = new SqlParameter();
    //    objSBUCode.Direction = ParameterDirection.Input;
    //    objCommand.Parameters.Add(objSBUCode);
    //    objSBUCode.SqlDbType = SqlDbType.VarChar;
    //    objSBUCode.ParameterName = "@txtSBUCode";
    //    objSBUCode.Value = txtSBUCode;

    //    SqlParameter objSDMMailId = new SqlParameter();
    //    objSDMMailId.Direction = ParameterDirection.Input;
    //    objCommand.Parameters.Add(objSDMMailId);
    //    objSDMMailId.SqlDbType = SqlDbType.VarChar;
    //    objSDMMailId.ParameterName = "@txtSDMMailId";
    //    objSDMMailId.Value = txtSDMMailId;

    //    SqlParameterCollection objParameterCollection = objCommand.Parameters;
    //    //objParameterCollection.Add(objSBUCode);
    //    // objParameterCollection.Add(objSDMMailId);
    //    objRetrieve = new RetrieveUI();

    //    try
    //    {

    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("dbo.spBEDmSdmMapRetrieve", ref ds, objCommand);

    //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
    //        {
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {
    //                objRetrieve = new RetrieveUI();
    //                objRetrieve.intSNo = Convert.ToInt16(ds.Tables[0].Rows[i]["intSNo"]);
    //                objRetrieve.txtSBUCode = ds.Tables[0].Rows[i]["txtSBUCode"] + "";
    //                objRetrieve.txtDMMailId = ds.Tables[0].Rows[i]["txtDMMailId"] + "";
    //                objRetrieve.txtSDMMailId = ds.Tables[0].Rows[i]["txtSDMMailId"] + "";
    //                objRetrieve.txtDHMailId = ds.Tables[0].Rows[i]["txtDHMailId"] + "";
    //                objRetrieve.txtBUCode = ds.Tables[0].Rows[i]["txtBUCode"] + "";
    //                objRetrieve.txtUpdatedBy = ds.Tables[0].Rows[i]["txtUpdatedBy"] + "";
    //                objRetrieve.txtUpdateDt = ds.Tables[0].Rows[i]["txtUpdateDt"] + "";
    //                objRetrieve.txtBITSCSIHMailId = ds.Tables[0].Rows[i]["txtBITSCSIHMailId"] + "";
    //                objRetrieve.txtUHMailId = ds.Tables[0].Rows[i]["txtUHMailId"] + "";
    //                objRetrieve.txtVertical = ds.Tables[0].Rows[i]["txtVertical"] + "";
    //                objRetrieve.txtPortfolio = ds.Tables[0].Rows[i]["txtPortfolio"] + "";
    //                objList.Add(objRetrieve);
    //            }

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //Logger.LogErrorToServer(App_Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }
    //    return objList;
    //}


    /// <summary>
    /// Shobana(dont delete)
    /// </summary>
    /// 

    public int DeleteDetails(int intSNo)
    {
        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            SqlParameter objParamSNo = new SqlParameter();
            objParamSNo.ParameterName = "@intSNo";
            objParamSNo.Direction = ParameterDirection.Input;
            objParamSNo.SqlDbType = SqlDbType.Int;
            objParamSNo.Value = intSNo;
            objParamColl.Add(objParamSNo);

            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.Direction = ParameterDirection.ReturnValue;
            objParamStatus.SqlDbType = SqlDbType.Int;
            objParamStatus.ParameterName = "ReturnValue";
            objParamColl.Add(objParamStatus);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEDmSdmMapDeleteDetails", objCommand);

            int updatestatus = (int)objCommand.Parameters["ReturnValue"].Value;
            return updatestatus;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public List<RetrieveUI> RetreiveDetailsALL(string txtSBUCode, string UserID)
    {

        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<RetrieveUI> objList = new List<RetrieveUI>();
        RetrieveUI objRetrieve = null;

        objCommand = new SqlCommand();
        SqlParameter objSBUCode = new SqlParameter();
        objSBUCode.Direction = ParameterDirection.Input;
        objCommand.Parameters.Add(objSBUCode);
        objSBUCode.SqlDbType = SqlDbType.VarChar;
        objSBUCode.ParameterName = "@txtSBUCode";
        objSBUCode.Value = txtSBUCode;

        SqlParameterCollection objParameterCollection = objCommand.Parameters;
        //objParameterCollection.Add(objSBUCode);
        // objParameterCollection.Add(objSDMMailId);
        objRetrieve = new RetrieveUI();

        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEDmSdmMapGetAllDetails", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objRetrieve = new RetrieveUI();
                    objRetrieve.intSNo = Convert.ToInt16(ds.Tables[0].Rows[i]["intSNo"]);
                    objRetrieve.txtSBUCode = ds.Tables[0].Rows[i]["txtSBUCode"] + "";
                    objRetrieve.txtDMMailId = ds.Tables[0].Rows[i]["txtDMMailId"] + "";
                    objRetrieve.txtSDMMailId = ds.Tables[0].Rows[i]["txtSDMMailId"] + "";
                    objRetrieve.txtDHMailId = ds.Tables[0].Rows[i]["txtDHMailId"] + "";
                    objRetrieve.txtBUCode = ds.Tables[0].Rows[i]["txtBUCode"] + "";
                    objRetrieve.txtUpdatedBy = ds.Tables[0].Rows[i]["txtUpdatedBy"] + "";
                    objRetrieve.txtUpdateDt = ds.Tables[0].Rows[i]["txtUpdateDt"] + "";
                    objRetrieve.txtBITSCSIHMailId = ds.Tables[0].Rows[i]["txtBITSCSIHMailId"] + "";
                    objRetrieve.txtUHMailId = ds.Tables[0].Rows[i]["txtUHMailId"] + "";
                    objRetrieve.txtVertical = ds.Tables[0].Rows[i]["txtVertical"] + "";
                    objRetrieve.txtPortfolio = ds.Tables[0].Rows[i]["txtPortfolio"] + "";
                    objRetrieve.lstPU = GetAllPUs(UserID);
                    objList.Add(objRetrieve);
                }
            }
        }
        catch (Exception ex)
        {
            //Logger.LogErrorToServer(App_Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return objList;
    }

    //shobana(dont delete)
    public List<string> GetSBUCode()
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<string> SBUCode = new List<string>();

        try
        {
            objCommand = new SqlCommand();
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEDmSdmMapGetSBUCode", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string DMName = string.Empty;

                    DMName = ds.Tables[0].Rows[i]["txtSBUCode"].ToString();
                    SBUCode.Add(DMName);
                }

            }
        }
        catch (Exception ex)
        {
            //Logger.LogErrorToServer(App_Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return SBUCode;
    }
    //Shobana(dont delete)
    public List<string> GetSDMMailId(string txtSBUCode)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;

        List<string> lstempCollection = new List<string>();
        try
        {

            objCommand = new SqlCommand();
            SqlParameter objSBUCode = new SqlParameter();
            objSBUCode.ParameterName = "@txtSBUCode";
            objSBUCode.Direction = ParameterDirection.Input;
            objSBUCode.SqlDbType = SqlDbType.NVarChar;
            objSBUCode.Value = txtSBUCode;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objSBUCode);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spSampleSDMMailId", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string DMName = string.Empty;

                    DMName = ds.Tables[0].Rows[i]["txtSDMMailId"].ToString();
                    lstempCollection.Add(DMName);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection;
    }

    public int InsertDelegateUser(string fromuser, string touser, DateTime fromdate, DateTime todate)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam6;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@fromuser";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = fromuser;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@touser";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = touser;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@fromdate";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.DateTime;
            sqlparam3.Value = fromdate;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@todate";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.DateTime;
            sqlparam4.Value = todate;
            objParamColl.Add(sqlparam4);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@retValue";
            sqlparam6.Direction = ParameterDirection.ReturnValue;
            sqlparam6.SqlDbType = SqlDbType.Int;
            objParamColl.Add(sqlparam6);

            objData.ExecuteSP("dbo.spBEInsertDelegateUser", ref  dsCurrConv, objCommand);

            int ret = Convert.ToInt32(sqlparam6.Value);

            return ret;


        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        //return new DataTable();
    }

    public DataTable FetchPuForClientCodePortfolio(string userid, string su)
    {

        DataSet dsCurrConv = new DataSet();

        SqlCommand sqlcmd = new SqlCommand();

        //SqlDataAdapter daCurrConv = new SqlDataAdapter();

        try
        {

            objData = new DataAccess();

            objData.GetConnection();

            SqlCommand objCommand;

            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam6;

            objCommand = new SqlCommand();

            objParamColl = objCommand.Parameters;





            sqlparam1 = new SqlParameter();

            sqlparam1.ParameterName = "@txtUserId";

            sqlparam1.Direction = ParameterDirection.Input;

            sqlparam1.SqlDbType = SqlDbType.VarChar;

            sqlparam1.Value = userid;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();

            sqlparam2.ParameterName = "@su";

            sqlparam2.Direction = ParameterDirection.Input;

            sqlparam2.SqlDbType = SqlDbType.VarChar;

            sqlparam2.Value = su;

            objParamColl.Add(sqlparam2);

            objData.ExecuteSP("dbo.EAS_SP_BEDemGetPuclientcode", ref dsCurrConv, objCommand);

            return dsCurrConv.Tables[0];

        }

        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            throw;

        }

        finally
        {

            objData.CloseConnection();

        }

    }


    public DataTable FetchMasterClientCodeForCCP(string txtsdm, string pu)
    {
        DataSet dsCcp = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtsdm";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = txtsdm;
            objParamColl.Add(sqlparam1);



            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtPU";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = pu;
            objParamColl.Add(sqlparam2);

            objData.ExecuteSP("dbo.EAS_SP_BEDemGetClientCode", ref  dsCcp, objCommand);

            return dsCcp.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }


    public DataTable FetchClientcodeportfolio(string mcc, string pu, string sdm)
    {

        try
        {
            DataSet ds = new DataSet();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtMcc";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = mcc;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtPU";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = pu;
            objParamColl.Add(sqlparam2);


            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtsdm";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = sdm;
            objParamColl.Add(sqlparam3);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.EAS_SP_BEDemFetchClientcodeportfolio", ref ds, objCommand);

            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }


    public void DeleteClientCodeportfolio(List<ClientCodePortfolio> lstItems)
    {
        foreach (var item in lstItems)
        {
            SqlCommand objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            SqlParameter param1;

            param1 = new SqlParameter();
            param1.ParameterName = "@mccid";
            param1.Direction = ParameterDirection.Input;
            param1.SqlDbType = SqlDbType.Int;
            param1.Value = item.intmccid;
            objParamColl.Add(param1);




            objParamColl = objCommand.Parameters;

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spDemDeleteClientCodePortfolio", objCommand);

        }
    }


    public int AddNewClientCode(ClientCodePortfolio item, string userid)
    {
        try
        {


            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8, sqlparam9,
                sqlparam10, sqlparam11, sqlparam12, sqlparam13, sqlparam14, sqlparam15, sqlparam16, sqlparam17, sqlparam18, sqlparam19, sqlparam20;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtMasterClientCode";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = item.txtMasterClientCode;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtClientCode";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = item.txtClientCode;
            objParamColl.Add(sqlparam2);


            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = item.txtPU;
            objParamColl.Add(sqlparam3);


            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtBITSCSIHMailId";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = item.txtBITSCSIHMailId;
            objParamColl.Add(sqlparam4);


            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@txtClientName";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = item.txtClientName;
            objParamColl.Add(sqlparam5);


            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@txtDHMailId";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = item.txtDHMailId;
            objParamColl.Add(sqlparam6);


            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@txtDivision";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = item.txtDivision;
            objParamColl.Add(sqlparam7);


            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@txtMasterCustomerName";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.SqlDbType = SqlDbType.VarChar;
            sqlparam8.Value = item.txtMasterCustomerName;
            objParamColl.Add(sqlparam8);


            sqlparam9 = new SqlParameter();
            sqlparam9.ParameterName = "@txtPortfolio";
            sqlparam9.Direction = ParameterDirection.Input;
            sqlparam9.SqlDbType = SqlDbType.VarChar;
            sqlparam9.Value = item.txtPortfolio;
            objParamColl.Add(sqlparam9);

            sqlparam10 = new SqlParameter();
            sqlparam10.ParameterName = "@txtRHMailId";
            sqlparam10.Direction = ParameterDirection.Input;
            sqlparam10.SqlDbType = SqlDbType.VarChar;
            sqlparam10.Value = item.txtRHMailId;
            objParamColl.Add(sqlparam10);


            sqlparam11 = new SqlParameter();
            sqlparam11.ParameterName = "@txtSDMMailId";
            sqlparam11.Direction = ParameterDirection.Input;
            sqlparam11.SqlDbType = SqlDbType.VarChar;
            sqlparam11.Value = item.txtSDMMailId;
            objParamColl.Add(sqlparam11);


            sqlparam12 = new SqlParameter();
            sqlparam12.ParameterName = "@txtUHMailId";
            sqlparam12.Direction = ParameterDirection.Input;
            sqlparam12.SqlDbType = SqlDbType.VarChar;
            sqlparam12.Value = item.txtUHMailId;
            objParamColl.Add(sqlparam12);

            sqlparam13 = new SqlParameter();
            sqlparam13.ParameterName = "@txtVertical";
            sqlparam13.Direction = ParameterDirection.Input;
            sqlparam13.SqlDbType = SqlDbType.VarChar;
            sqlparam13.Value = item.txtVertical;
            objParamColl.Add(sqlparam13);

            sqlparam14 = new SqlParameter();
            sqlparam14.ParameterName = "@txtUpdatedBy";
            sqlparam14.Direction = ParameterDirection.Input;
            sqlparam14.SqlDbType = SqlDbType.VarChar;
            sqlparam14.Value = userid;
            objParamColl.Add(sqlparam14);


            sqlparam15 = new SqlParameter();
            sqlparam15.ParameterName = "@txtFAPortfolio";
            sqlparam15.Direction = ParameterDirection.Input;
            sqlparam15.SqlDbType = SqlDbType.VarChar;
            sqlparam15.Value = item.txtFAPortfolio;
            objParamColl.Add(sqlparam15);


            sqlparam17 = new SqlParameter();
            sqlparam17.ParameterName = "@isActive";
            sqlparam17.Direction = ParameterDirection.Input;
            sqlparam17.SqlDbType = SqlDbType.VarChar;
            sqlparam17.Value = item.isActive;
            objParamColl.Add(sqlparam17);





            sqlparam18 = new SqlParameter();
            sqlparam18.ParameterName = "@ServiceLine";
            sqlparam18.Direction = ParameterDirection.Input;
            sqlparam18.SqlDbType = SqlDbType.VarChar;
            sqlparam18.Value = item.txtServiceline;
            objParamColl.Add(sqlparam18);

            sqlparam19 = new SqlParameter();
            sqlparam19.ParameterName = "@Unit";
            sqlparam19.Direction = ParameterDirection.Input;
            sqlparam19.SqlDbType = SqlDbType.VarChar;
            sqlparam19.Value = item.txtunit;
            objParamColl.Add(sqlparam19);

            sqlparam20 = new SqlParameter();
            sqlparam20.ParameterName = "@Mconame";
            sqlparam20.Direction = ParameterDirection.Input;
            sqlparam20.SqlDbType = SqlDbType.VarChar;
            sqlparam20.Value = item.txtMCOName;
            objParamColl.Add(sqlparam20);





            sqlparam16 = new SqlParameter();
            sqlparam16.ParameterName = "@ret";
            sqlparam16.Direction = ParameterDirection.ReturnValue;
            sqlparam16.SqlDbType = SqlDbType.Int;
            objParamColl.Add(sqlparam16);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spDemAddNewClientCode", objCommand);

            var ret = Convert.ToInt32(sqlparam16.Value);



            return ret;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public void EditClientCode(int mccid, ClientCodePortfolio item, string userid)
    {
        try
        {


            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8, sqlparam9,
                sqlparam10, sqlparam11, sqlparam12, sqlparam13, sqlparam14, sqlparam15, sqlparam16, sqlparam17, sqlparam18, sqlparam19;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam15 = new SqlParameter();
            sqlparam15.ParameterName = "@id";
            sqlparam15.Direction = ParameterDirection.Input;
            sqlparam15.SqlDbType = SqlDbType.Int;
            sqlparam15.Value = mccid;
            objParamColl.Add(sqlparam15);

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtMasterClientCode";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = item.txtMasterClientCode.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtClientCode";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = item.txtClientCode.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam2);


            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = item.txtPU.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam3);


            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtBITSCSIHMailId";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = item.txtBITSCSIHMailId.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam4);


            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@txtClientName";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = item.txtClientName.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam5);


            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@txtDHMailId";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = item.txtDHMailId.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam6);


            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@txtDivision";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = item.txtDivision.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam7);


            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@txtMasterCustomerName";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.SqlDbType = SqlDbType.VarChar;
            sqlparam8.Value = item.txtMasterCustomerName.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam8);


            sqlparam9 = new SqlParameter();
            sqlparam9.ParameterName = "@txtPortfolio";
            sqlparam9.Direction = ParameterDirection.Input;
            sqlparam9.SqlDbType = SqlDbType.VarChar;
            sqlparam9.Value = item.txtPortfolio.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam9);

            sqlparam10 = new SqlParameter();
            sqlparam10.ParameterName = "@txtRHMailId";
            sqlparam10.Direction = ParameterDirection.Input;
            sqlparam10.SqlDbType = SqlDbType.VarChar;
            sqlparam10.Value = item.txtRHMailId.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam10);


            sqlparam11 = new SqlParameter();
            sqlparam11.ParameterName = "@txtSDMMailId";
            sqlparam11.Direction = ParameterDirection.Input;
            sqlparam11.SqlDbType = SqlDbType.VarChar;
            sqlparam11.Value = item.txtSDMMailId.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam11);


            sqlparam12 = new SqlParameter();
            sqlparam12.ParameterName = "@txtUHMailId";
            sqlparam12.Direction = ParameterDirection.Input;
            sqlparam12.SqlDbType = SqlDbType.VarChar;
            sqlparam12.Value = item.txtUHMailId.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam12);

            sqlparam13 = new SqlParameter();
            sqlparam13.ParameterName = "@txtVertical";
            sqlparam13.Direction = ParameterDirection.Input;
            sqlparam13.SqlDbType = SqlDbType.VarChar;
            sqlparam13.Value = item.txtVertical.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam13);

            sqlparam14 = new SqlParameter();
            sqlparam14.ParameterName = "@txtUpdatedBy";
            sqlparam14.Direction = ParameterDirection.Input;
            sqlparam14.SqlDbType = SqlDbType.VarChar;
            sqlparam14.Value = userid.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam14);



            sqlparam15 = new SqlParameter();
            sqlparam15.ParameterName = "@txtFAPortfolio";
            sqlparam15.Direction = ParameterDirection.Input;
            sqlparam15.SqlDbType = SqlDbType.VarChar;
            sqlparam15.Value = item.txtFAPortfolio.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam15);


            sqlparam16 = new SqlParameter();
            sqlparam16.ParameterName = "@isActive";
            sqlparam16.Direction = ParameterDirection.Input;
            sqlparam16.SqlDbType = SqlDbType.VarChar;
            sqlparam16.Value = item.isActive.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam16);

            sqlparam17 = new SqlParameter();
            sqlparam17.ParameterName = "@Mconame";
            sqlparam17.Direction = ParameterDirection.Input;
            sqlparam17.SqlDbType = SqlDbType.VarChar;
            sqlparam17.Value = item.txtMCOName.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam17);

            sqlparam18 = new SqlParameter();
            sqlparam18.ParameterName = "@ServiceLine";
            sqlparam18.Direction = ParameterDirection.Input;
            sqlparam18.SqlDbType = SqlDbType.VarChar;
            sqlparam18.Value = item.txtServiceline.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam18);

            sqlparam19 = new SqlParameter();
            sqlparam19.ParameterName = "@Unit";
            sqlparam19.Direction = ParameterDirection.Input;
            sqlparam19.SqlDbType = SqlDbType.VarChar;
            sqlparam19.Value = item.txtunit.TrimStart().TrimEnd();
            objParamColl.Add(sqlparam19);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spUpdateClientCodePortfolio", objCommand);





        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public ClientCodePortfolio GetAllCCPFields(int mccid)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        ClientCodePortfolio empCollection = new ClientCodePortfolio();


        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@intmccid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.Int;
            objParm.Value = mccid;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spDemGetCCPFields", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new ClientCodePortfolio();

                    empCollection.txtBITSCSIHMailId = ds.Tables[0].Rows[i]["txtBITSCSIHMailId"].ToString().Trim();
                    empCollection.txtPU = ds.Tables[0].Rows[i]["txtPU"].ToString().Trim();
                    empCollection.txtClientCode = ds.Tables[0].Rows[i]["txtClientCode"].ToString().Trim();
                    empCollection.txtClientName = ds.Tables[0].Rows[i]["txtClientName"].ToString().Trim();
                    empCollection.txtDHMailId = ds.Tables[0].Rows[i]["txtDHMailId"].ToString().Trim();


                    empCollection.txtDivision = ds.Tables[0].Rows[i]["txtDivision"].ToString().Trim();
                    empCollection.txtMasterClientCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString().Trim();
                    empCollection.txtMasterCustomerName = ds.Tables[0].Rows[i]["txtMasterCustomerName"].ToString().Trim();
                    empCollection.txtPortfolio = ds.Tables[0].Rows[i]["txtPortfolio"].ToString().Trim();
                    empCollection.txtRHMailId = ds.Tables[0].Rows[i]["txtRHMailId"].ToString().Trim();
                    empCollection.txtSDMMailId = ds.Tables[0].Rows[i]["txtSDMMailId"].ToString().Trim();
                    empCollection.txtUHMailId = ds.Tables[0].Rows[i]["txtUHMailId"].ToString().Trim();
                    empCollection.txtVertical = ds.Tables[0].Rows[i]["txtVertical"].ToString().Trim();
                    empCollection.txtFAPortfolio = ds.Tables[0].Rows[i]["txtFAPortfolio"].ToString().Trim();
                    empCollection.isActive = ds.Tables[0].Rows[i]["isActive"].ToString().Trim();
                    empCollection.txtMCOName = ds.Tables[0].Rows[i]["txtMCOName"].ToString().Trim();
                    empCollection.txtServiceline = ds.Tables[0].Rows[i]["txtServiceline"].ToString().Trim();
                    empCollection.txtunit = ds.Tables[0].Rows[i]["txtunit"].ToString().Trim();


                }



            }

            return empCollection;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }



    }

    public List<string> FetchSDMForCCP(string txtPU)
    {
        DataSet dsCcp = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        List<string> lstSDM = new List<string>();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            string sdm;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtPU";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = txtPU;
            objParamColl.Add(sqlparam1);
            objData.ExecuteSP("dbo.EAS_SP_BEDemGetSDMListCCP", ref  dsCcp, objCommand);


            if (dsCcp != null && dsCcp.Tables != null && dsCcp.Tables.Count > 0)
            {
                for (int i = 0; i < dsCcp.Tables[0].Rows.Count; i++)
                {


                    // empCollection.DU = ds.Tables[0].Rows[i]["txtDU"].ToString();
                    sdm = dsCcp.Tables[0].Rows[i]["txtSDMMailid"].ToString().Trim();
                    // empCollection.CustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                    lstSDM.Add(sdm);
                }

            }
            return lstSDM;

        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    //added on 07Oct2020
    public string get_FA_MaxDate()
    {
        SqlCommand cmd = new SqlCommand("select FORMAT( max(exratedate),'ddMMMyyyy') as FA_Date from BEExchangeRates_FA where month(ExRatedate)  = 3 and Day(exratedate) = 31");
        cmd.CommandTimeout = int.MaxValue;
        SqlConnection G_DBConnection = new SqlConnection(G_connStr);
        cmd.Connection = G_DBConnection;
        G_DBConnection.Open();
        string FA_Rate = cmd.ExecuteScalar().ToString();
        G_DBConnection.Close();
        return FA_Rate;
    }
    //Done
    //TODO:11/12 excel download
    public DataTable GetRTBRDetails(string userid, string year, string BEtype, string nso, string Mcc, string su)
    {
        DataSet ds = new DataSet();

        SqlParameter objParm, objParm1, objParm3, objParm4, objParm5, objParm6;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        try
        {

            objParm = new SqlParameter();
            objParm.ParameterName = "@userid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = userid;

            objParm1 = new SqlParameter();
            objParm1.ParameterName = "@year";
            objParm1.Direction = ParameterDirection.Input;
            objParm1.SqlDbType = SqlDbType.NVarChar;
            objParm1.Value = year;

            //objparm2 = new SqlParameter();
            //objparm2.ParameterName = "@SU";
            //objparm2.Direction = ParameterDirection.Input;
            //objparm2.SqlDbType = SqlDbType.NVarChar;
            //objparm2.Value = SU;

            objParm3 = new SqlParameter();
            objParm3.ParameterName = "@type";
            objParm3.Direction = ParameterDirection.Input;
            objParm3.SqlDbType = SqlDbType.NVarChar;
            objParm3.Value = BEtype;


            objParm5 = new SqlParameter();
            objParm5.ParameterName = "@Mcc";
            objParm5.Direction = ParameterDirection.Input;
            objParm5.SqlDbType = SqlDbType.NVarChar;
            objParm5.Value = Mcc;

            objParm6 = new SqlParameter();
            objParm6.ParameterName = "@su";
            objParm6.Direction = ParameterDirection.Input;
            objParm6.SqlDbType = SqlDbType.NVarChar;
            objParm6.Value = su;

            objParm4 = new SqlParameter();
            objParm4.ParameterName = "@newOffering";
            objParm4.Direction = ParameterDirection.Input;
            objParm4.SqlDbType = SqlDbType.NVarChar;
            objParm4.Value = nso;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.AddRange(new SqlParameter[] { objParm, objParm1, objParm3, objParm5, objParm6, objParm4 });

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("[spBeRtbrUSDNC_V1_NSO]", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {



                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                return dt;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataTable();

    }


    public string GetRTBRDumpDate()
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;

        string tmp = string.Empty;

        try
        {

            objCommand = new SqlCommand();



            objData = new DataAccess();
            objData.GetConnection();
            // objData.ExecuteSP("spBEDUListForDropDown", ref ds, objCommand);
            objData.ExecuteSP("spGetRTBRDumpDate", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    tmp = ds.Tables[0].Rows[i]["dumpdate"] == DBNull.Value ? "" : Convert.ToDateTime(ds.Tables[0].Rows[i]["dumpdate"]).ToString("dd-MMM-yyyy HH:mm IST");

                    //ds.Tables[0].Rows[i]["IsAllDU"] == DBNull.Value ? "No" : ds.Tables[0].Rows[i]["IsAllDU"].ToString().Trim() == null ? "No" : ds.Tables[0].Rows[i]["IsAllDU"].ToString().Trim() == "Y" ? "Yes" : "No";

                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return tmp;
    }



    public List<string> GetSUForuser(string userid)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;

        List<string> lstempCollection = new List<string>();

        try
        {

            objCommand = new SqlCommand();


            SqlParameter objBE = new SqlParameter();
            objBE.ParameterName = "@userid";
            objBE.Direction = ParameterDirection.Input;
            objBE.SqlDbType = SqlDbType.VarChar;
            objBE.Value = userid;


            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objBE);

            objData = new DataAccess();
            objData.GetConnection();
            // objData.ExecuteSP("spBEDUListForDropDown", ref ds, objCommand);
            objData.ExecuteSP("[spBEGetSU_dummy_NSO]", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    string tmp = ds.Tables[0].Rows[i]["SU"].ToString();

                    lstempCollection.Add(tmp);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }

    public List<string> GetNSOForuser(string userid)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<string> lstempCollection = new List<string>();
        try
        {
            objCommand = new SqlCommand();
            SqlParameter objBE = new SqlParameter();
            objBE.ParameterName = "@txtUserId";
            objBE.Direction = ParameterDirection.Input;
            objBE.SqlDbType = SqlDbType.VarChar;
            objBE.Value = userid;


            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objBE);

            objData = new DataAccess();
            objData.GetConnection();
            // objData.ExecuteSP("spBEDUListForDropDown", ref ds, objCommand);
           // objData.ExecuteSP("[spBEPUList]", ref ds, objCommand);
            objData.ExecuteSP("EAS_spBENSOListForDropDown_1", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    //string tmp = ds.Tables[0].Rows[i]["PU"].ToString();
                    string tmp = ds.Tables[0].Rows[i]["NSO"].ToString();

                    lstempCollection.Add(tmp);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }

    public DataTable GetFinPulseDetails(string userid, string SU)
    {
        DataSet ds = new DataSet();

        SqlParameter objParm, objParm1;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        try
        {

            objParm = new SqlParameter();
            objParm.ParameterName = "@userid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = userid;

            objParm1 = new SqlParameter();
            objParm1.ParameterName = "@SU";
            objParm1.Direction = ParameterDirection.Input;
            objParm1.SqlDbType = SqlDbType.NVarChar;
            objParm1.Value = SU;




            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.AddRange(new SqlParameter[] { objParm, objParm1 });

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBeFinData", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {



                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                return dt;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataTable();

    }

    /// <summary>
    /// 13 nov : Exchange Rate Screen
    /// </summary>
    /// <returns></returns>
    public DataTable FetchExchangeRates(string type)
    {
        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@type";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = type;
            objParamColl.Add(sqlparam1);

            objData.ExecuteSP("dbo.spBEExchangeRateCalc", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        //return new DataTable();
    }


    public void InsertExchangeRates(DateTime dt)
    {
        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@dt";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = dt;
            objParamColl.Add(sqlparam1);

            objData.ExecuteSP("dbo.spBESaveExchageRatetoDailyConv", ref  dsCurrConv, objCommand);


        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        //return new DataTable();
    }

    //public DataSet GetDMBEData(string PU, string customerCode, string dm, string quarter, string year, string currency)
    //{


    //    DataSet ds = new DataSet();
    //    DataSet dsreturn = new DataSet();

    //    SqlCommand objCommand;



    //    try
    //    {

    //        objCommand = new SqlCommand();
    //        SqlParameter objParamStatus = new SqlParameter();
    //        objParamStatus.ParameterName = "@txtCustomerCode";
    //        objParamStatus.Direction = ParameterDirection.Input;
    //        objParamStatus.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus.Value = customerCode;

    //        SqlParameter objParamStatus1 = new SqlParameter();
    //        objParamStatus1.ParameterName = "@txtUserId";
    //        objParamStatus1.Direction = ParameterDirection.Input;
    //        objParamStatus1.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus1.Value = dm;

    //        SqlParameter objParamStatus2 = new SqlParameter();
    //        objParamStatus2.ParameterName = "@txtQuarterName";
    //        objParamStatus2.Direction = ParameterDirection.Input;
    //        objParamStatus2.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus2.Value = quarter;

    //        SqlParameter objParamStatus3 = new SqlParameter();
    //        objParamStatus3.ParameterName = "@txtYear";
    //        objParamStatus3.Direction = ParameterDirection.Input;
    //        objParamStatus3.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus3.Value = year;

    //        SqlParameter objParamStatus4 = new SqlParameter();
    //        objParamStatus4.ParameterName = "@PU";
    //        objParamStatus4.Direction = ParameterDirection.Input;
    //        objParamStatus4.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus4.Value = PU;

    //        SqlParameter objParamStatus5 = new SqlParameter();
    //        objParamStatus5.ParameterName = "@txtCurrency";
    //        objParamStatus5.Direction = ParameterDirection.Input;
    //        objParamStatus5.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus5.Value = currency;

    //        objCommand = new SqlCommand();
    //        SqlParameterCollection objParamColl = objCommand.Parameters;


    //        objParamColl.Add(objParamStatus);

    //        objParamColl.Add(objParamStatus1);
    //        objParamColl.Add(objParamStatus2);
    //        objParamColl.Add(objParamStatus3);
    //        objParamColl.Add(objParamStatus4);
    //        objParamColl.Add(objParamStatus5);
    //        DataTable dttemp1 = null;

    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("spBEFetchRevDataDM", ref ds, objCommand);
    //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
    //        {
    //            DataTable dt = new DataTable();
    //            dt = ds.Tables[0];


    //            dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
    //            //dt.Columns["txtDU"].ColumnName = "DU";
    //            dt.Columns["txtDMMailId"].ColumnName = "DM";
    //            dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
    //            dt.Columns["DMfltMonth1BE"].ColumnName = "DMMonth1";
    //            dt.Columns["DMfltMonth2BE"].ColumnName = "DMMonth2";
    //            dt.Columns["DMfltMonth3BE"].ColumnName = "DMMonth3";
    //            dt.Columns["DMfltCurrentQuarterBE"].ColumnName = "DMQCur";
    //            dt.Columns["txtDHMailid"].ColumnName = "DHMailId";
    //            dt.Columns["txtPU"].ColumnName = "PU";
    //            dt.Columns["txtQuarterName"].ColumnName = "Current Quarter Name";
    //            dt.Columns["txtYear"].ColumnName = "Year";
    //            //dt.Columns["DMfltNextQuarterBE"].ColumnName = "DMQNext"; //TODO
    //            // dt.Columns["fltPrevQtrBE"].ColumnName = "DMQPrev";
    //            //TODO:SDM section removed
    //            //dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
    //            //dt.Columns["SDMfltMonth1BE"].ColumnName = "SDMMonth1";
    //            //dt.Columns["SDMfltMonth2BE"].ColumnName = "SDMMonth2";
    //            //dt.Columns["SDMfltMonth3BE"].ColumnName = "SDMMonth3";
    //            //dt.Columns["SDMfltCurrentQuarterBE"].ColumnName = "SDMQCur";
    //            //dt.Columns["SDMfltNextQuarterBE"].ColumnName = "SDMQNext"; //TODO

    //            //dt.Columns[""].ColumnName = "SDMQPrev";
    //            // dt.Columns["txtLastUpdatedBy"].ColumnName = "LastModifiedBy";

    //            dt.Columns["FinRTBRM1"].ColumnName = "ActualM1";
    //            dt.Columns["FinRTBRM2"].ColumnName = "ActualM2";
    //            dt.Columns["FinRTBRM3"].ColumnName = "ActualM3";
    //            dt.Columns["dtDMUpdatedDate"].ColumnName = "DMLastModifiedOn";

    //            //dt.Columns["dtSDMUpdatedDate"].ColumnName = "SDMLastModifiedOn";
    //            //dt.Columns["dtLastUpdatedDate"].ColumnName = "SDMLastModifiedOn";

    //            // dt.Columns["dtLastUpdatedDate"].ColumnName = "SDMLastModifiedOn";


    //            dt.Columns["intBEId"].ColumnName = "BEID";
    //            dt.Columns["txtRemarks"].ColumnName = "Remarks";
    //            dt.Columns["FinRTBRTotal"].ColumnName = "totalRTBR";
    //            // dt.Columns["txtSDMRemarks"].ColumnName = "SDMRemarks";
    //            dttemp1 = dt;
    //            dsreturn.Tables.Add(dttemp1.Copy());

    //            DataTable dt1 = new DataTable();
    //            dt1 = ds.Tables[1];
    //            dsreturn.Tables.Add(dt1.Copy());
    //            return dsreturn;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }


    //    return new DataSet();
    //}

    //public DataTable GetAlconPBSData(string customerCode)
    //{


    //    DataSet ds = new DataSet();
    //    SqlParameter objParm;
    //    SqlCommand objCommand;
    //    SqlParameterCollection objParamColl;
    //    string reportCode = string.Empty;
    //    DataTable dtAlcon = null;
    //    try
    //    {
    //        objParm = new SqlParameter();
    //        objParm.ParameterName = "@customercode";
    //        objParm.Direction = ParameterDirection.Input;
    //        objParm.SqlDbType = SqlDbType.VarChar;
    //        objParm.Value = customerCode;

    //        objCommand = new SqlCommand();
    //        objParamColl = objCommand.Parameters;
    //        objParamColl.Add(objParm);

    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("spBeFetchAlconPBS", ref ds, objCommand);
    //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
    //        {
    //            dtAlcon = ds.Tables[0];
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }


    //    return dtAlcon;
    //}

    public DataSet GetNotDMBEDataNSO(string NewServiceOffering, string customerCode, string userid, string quarter, string year, string Role)
    {
        DataSet ds = new DataSet();
        DataSet dsreturn = new DataSet();
        SqlCommand objCommand;
        List<DataTable> retTable = new List<DataTable>();
        try
        {
            objCommand = new SqlCommand();
            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@MasterClientCode";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = customerCode;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@UserId";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = userid;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@Quarter";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = quarter;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@FYYR";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = year;

            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@Role";
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.VarChar;
            objParamStatus5.Value = Role;

            SqlParameter objParamStatus6 = new SqlParameter();
            objParamStatus6.ParameterName = "@NewOffering";
            objParamStatus6.Direction = ParameterDirection.Input;
            objParamStatus6.SqlDbType = SqlDbType.VarChar;
            objParamStatus6.Value = NewServiceOffering;

            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;

            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);

            objParamColl.Add(objParamStatus4);
            objParamColl.Add(objParamStatus5);
            objParamColl.Add(objParamStatus6);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_Fetch_BEData_SDM_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                // dt.Columns["txtMasterClientCode"].ColumnName = month1 +"CustomerCode";
                //dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
                //dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
                //dt.Columns["txtQuarterName"].ColumnName = "Current Quarter Name";
                //dt.Columns["txtYear"].ColumnName = "Year";

                ////BE
                //dt.Columns["DMfltMonth1BE"].ColumnName = "DMMonth1";
                //dt.Columns["DMfltMonth2BE"].ColumnName = "DMMonth2";
                //dt.Columns["DMfltMonth3BE"].ColumnName = "DMMonth3";
                //dt.Columns["DMfltCurrentQuarterBE"].ColumnName = "DMQCur";
                //dt.Columns["SDMBK1"].ColumnName = "SDMBK1";
                //dt.Columns["SDMBK2"].ColumnName = "SDMBK2";
                //dt.Columns["SDMBK3"].ColumnName = "SDMBK3";
                //dt.Columns["SDMBK4"].ColumnName = "SDMBK4";
                //dt.Columns["txtSDMBERemarks"].ColumnName = "SDMBERemarks";

                ////Vol

                //dt.Columns["txtOnsiteValueM1"].ColumnName = "OnsiteValueM1";
                //dt.Columns["txtOffshoreValueM1"].ColumnName = "OffshoreValueM1";
                //dt.Columns["txtOnsiteValueM2"].ColumnName = "OnsiteValueM2";
                //dt.Columns["txtOffshoreValueM2"].ColumnName = "OffshoreValueM2";
                //dt.Columns["txtOnsiteValueM3"].ColumnName = "OnsiteValueM3";
                //dt.Columns["txtOffshoreValueM3"].ColumnName = "OffshoreValueM3";

                //dt.Columns["txtTotalOnsiteValue"].ColumnName = "TotalOnsiteValue";
                //dt.Columns["txtTotalOffshoreValue"].ColumnName = "TotalOffshoreValue";
                //dt.Columns["txtGrandTotalValue"].ColumnName = "GrandTotalValue";
                //dt.Columns["txtSDMVolumeRemarks"].ColumnName = "SDMVolumeRemarks";

                //dt.Columns["dtDMUpdatedDate"].ColumnName = "dtDMUpdatedDate";



                dsreturn.Tables.Add(dt.Copy());
                //dsreturn.Tables.Add(ds.Tables[1].Copy());

                return dsreturn;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataSet();
    }


    public DataSet GetNotDMBEData(string sl, string customerCode, string userid, string quarter, string year, string Role)
    {


        DataSet ds = new DataSet();
        DataSet dsreturn = new DataSet();
        SqlCommand objCommand;
        List<DataTable> retTable = new List<DataTable>();

        try
        {


            objCommand = new SqlCommand();
            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@MasterClientCode";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = customerCode;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@UserId";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = userid;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@Quarter";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = quarter;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@FYYR";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = year;

            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.ParameterName = "@PU";
            objParamStatus.Direction = ParameterDirection.Input;
            objParamStatus.SqlDbType = SqlDbType.VarChar;
            objParamStatus.Value = sl;

            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@Role";
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.VarChar;
            objParamStatus5.Value = Role;







            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            objParamColl.Add(objParamStatus);

            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);

            objParamColl.Add(objParamStatus4);
            objParamColl.Add(objParamStatus5);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_Fetch_BEData_SDM_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                // dt.Columns["txtMasterClientCode"].ColumnName = month1 +"CustomerCode";
                //dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
                //dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
                //dt.Columns["txtQuarterName"].ColumnName = "Current Quarter Name";
                //dt.Columns["txtYear"].ColumnName = "Year";

                ////BE
                //dt.Columns["DMfltMonth1BE"].ColumnName = "DMMonth1";
                //dt.Columns["DMfltMonth2BE"].ColumnName = "DMMonth2";
                //dt.Columns["DMfltMonth3BE"].ColumnName = "DMMonth3";
                //dt.Columns["DMfltCurrentQuarterBE"].ColumnName = "DMQCur";
                //dt.Columns["SDMBK1"].ColumnName = "SDMBK1";
                //dt.Columns["SDMBK2"].ColumnName = "SDMBK2";
                //dt.Columns["SDMBK3"].ColumnName = "SDMBK3";
                //dt.Columns["SDMBK4"].ColumnName = "SDMBK4";
                //dt.Columns["txtSDMBERemarks"].ColumnName = "SDMBERemarks";

                ////Vol

                //dt.Columns["txtOnsiteValueM1"].ColumnName = "OnsiteValueM1";
                //dt.Columns["txtOffshoreValueM1"].ColumnName = "OffshoreValueM1";
                //dt.Columns["txtOnsiteValueM2"].ColumnName = "OnsiteValueM2";
                //dt.Columns["txtOffshoreValueM2"].ColumnName = "OffshoreValueM2";
                //dt.Columns["txtOnsiteValueM3"].ColumnName = "OnsiteValueM3";
                //dt.Columns["txtOffshoreValueM3"].ColumnName = "OffshoreValueM3";

                //dt.Columns["txtTotalOnsiteValue"].ColumnName = "TotalOnsiteValue";
                //dt.Columns["txtTotalOffshoreValue"].ColumnName = "TotalOffshoreValue";
                //dt.Columns["txtGrandTotalValue"].ColumnName = "GrandTotalValue";
                //dt.Columns["txtSDMVolumeRemarks"].ColumnName = "SDMVolumeRemarks";

                //dt.Columns["dtDMUpdatedDate"].ColumnName = "dtDMUpdatedDate";



                dsreturn.Tables.Add(dt.Copy());
                //dsreturn.Tables.Add(ds.Tables[1].Copy());

                return dsreturn;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataSet();
    }

    public DataTable GetBEVolDataExcel(string customerCode, string dm, string PU, string quarter, string year, string type)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;


        try
        {

            objCommand = new SqlCommand();
            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.ParameterName = "@txtCustomerCode";
            objParamStatus.Direction = ParameterDirection.Input;
            objParamStatus.SqlDbType = SqlDbType.VarChar;
            objParamStatus.Value = customerCode;

            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@txtUserId";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = dm;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@txtQuarterName";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = quarter;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@txtYear";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = year;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@PU";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = PU;

            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@type";
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.VarChar;
            objParamStatus5.Value = type;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            objParamColl.Add(objParamStatus);


            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);
            objParamColl.Add(objParamStatus4);
            objParamColl.Add(objParamStatus5);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEFetchVolSDM", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
                dt.Columns["txtPU"].ColumnName = "PU";
                //dt.Columns["txtDMMailId"].ColumnName = "DM";

                //dt.Columns["txtDHMailId"].ColumnName = "DHMailId";
                dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";

                //dt.Columns["txtIsApproved"].ColumnName = "IsApproved";


                //dt.Columns["dtDMUpdatedDate"].ColumnName = "DMLastModifiedOn";
                dt.Columns["dtSDMUpdatedDate"].ColumnName = "SDMLastModifiedOn";


                dt.Columns["intBEId"].ColumnName = "BEID";
                //dt.Columns["txtRemarks"].ColumnName = "DMRemarks";
                dt.Columns["txtRemarksSDM"].ColumnName = "SDMRemarks";



                dt.Columns["DMfltMonth1EffortOnsite"].ColumnName = ("DMMonth1ON");
                dt.Columns["DMfltMonth2EffortOnsite"].ColumnName = ("DMMonth2ON");
                dt.Columns["DMfltMonth3EffortOnsite"].ColumnName = ("DMMonth3ON");
                dt.Columns["DMfltMonth1EffortOffShore"].ColumnName = ("DMMonth1OFF");
                dt.Columns["DMfltMonth2EffortOffShore"].ColumnName = ("DMMonth2OFF");
                dt.Columns["DMfltMonth3EffortOffShore"].ColumnName = ("DMMonth3OFF");

                dt.Columns["SDMfltMonth1EffortOnsite"].ColumnName = ("SDMMonth1ON");
                dt.Columns["SDMfltMonth2EffortOnsite"].ColumnName = ("SDMMonth2ON");
                dt.Columns["SDMfltMonth3EffortOnsite"].ColumnName = ("SDMMonth3ON");
                dt.Columns["SDMfltMonth1EffortOffShore"].ColumnName = ("SDMMonth1OFF");
                dt.Columns["SDMfltMonth2EffortOffShore"].ColumnName = ("SDMMonth2OFF");
                dt.Columns["SDMfltMonth3EffortOffShore"].ColumnName = ("SDMMonth3OFF");


                dt.Columns["DMfltTotalOnsite"].ColumnName = ("DMTotalON");
                dt.Columns["DMfltTotalOffShore"].ColumnName = ("DMTotalOFF");
                dt.Columns["SDMfltTotalOnsite"].ColumnName = ("SDMTotalON");
                dt.Columns["SDMfltTotalOffShore"].ColumnName = ("SDMTotalOFF");
                dt.Columns["DMfltGrandTotal"].ColumnName = ("DMGrandTotal");
                dt.Columns["SDMfltGrandTotal"].ColumnName = ("SDMGrandTotal");

                dt.Columns["txtOnsiteValueM1"].ColumnName = ("RTBRMonth1ON");
                dt.Columns["txtOffshoreValueM1"].ColumnName = ("RTBRMonth1OFF");
                dt.Columns["txtOnsiteValueM2"].ColumnName = ("RTBRMonth2ON");
                dt.Columns["txtOffshoreValueM2"].ColumnName = ("RTBRMonth2OFF");
                dt.Columns["txtOnsiteValueM3"].ColumnName = ("RTBRMonth3ON");
                dt.Columns["txtOffshoreValueM3"].ColumnName = ("RTBRMonth3OFF");
                dt.Columns["txtTotalOnsiteValue"].ColumnName = ("RTBRTotalON");
                dt.Columns["txtTotalOffshoreValue"].ColumnName = ("RTBRTotalOFF");
                dt.Columns["txtGrandTotalValue"].ColumnName = ("RTBRGrandTotal");

                dt.Columns["txtQuarterName"].ColumnName = ("txtCurrentQuarterName");
                dt.Columns["txtYear"].ColumnName = ("txtYear");

                return dt;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataTable();
    }


    public DataTable GetBEVolDataExcelDM(string customerCode, string dm, string PU, string quarter, string year)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;


        try
        {

            objCommand = new SqlCommand();
            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.ParameterName = "@txtCustomerCode";
            objParamStatus.Direction = ParameterDirection.Input;
            objParamStatus.SqlDbType = SqlDbType.VarChar;
            objParamStatus.Value = customerCode;

            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@txtUserId";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = dm;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@txtQuarterName";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = quarter;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@txtYear";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = year;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@PU";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = PU;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            objParamColl.Add(objParamStatus);


            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);
            objParamColl.Add(objParamStatus4);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEFetchVolDM_RTBR", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
                dt.Columns["txtPU"].ColumnName = "PU";
                dt.Columns["txtDMMailId"].ColumnName = "DM";

                dt.Columns["txtDHMailId"].ColumnName = "DHMailId";
                //dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";

                //dt.Columns["txtIsApproved"].ColumnName = "IsApproved";


                dt.Columns["dtDMUpdatedDate"].ColumnName = "DMLastModifiedOn";
                //dt.Columns["dtSDMUpdatedDate"].ColumnName = "SDMLastModifiedOn";


                dt.Columns["intBEId"].ColumnName = "BEID";
                dt.Columns["txtRemarks"].ColumnName = "DMRemarks";
                //dt.Columns["txtRemarksSDM"].ColumnName = "SDMRemarks";



                dt.Columns["DMfltMonth1EffortOnsite"].ColumnName = ("DMMonth1ON");
                dt.Columns["DMfltMonth2EffortOnsite"].ColumnName = ("DMMonth2ON");
                dt.Columns["DMfltMonth3EffortOnsite"].ColumnName = ("DMMonth3ON");
                dt.Columns["DMfltMonth1EffortOffShore"].ColumnName = ("DMMonth1OFF");
                dt.Columns["DMfltMonth2EffortOffShore"].ColumnName = ("DMMonth2OFF");
                dt.Columns["DMfltMonth3EffortOffShore"].ColumnName = ("DMMonth3OFF");

                //dt.Columns["SDMfltMonth1EffortOnsite"].ColumnName = ("SDMMonth1ON");
                //dt.Columns["SDMfltMonth2EffortOnsite"].ColumnName = ("SDMMonth2ON");
                //dt.Columns["SDMfltMonth3EffortOnsite"].ColumnName = ("SDMMonth3ON");
                //dt.Columns["SDMfltMonth1EffortOffShore"].ColumnName = ("SDMMonth1OFF");
                //dt.Columns["SDMfltMonth2EffortOffShore"].ColumnName = ("SDMMonth2OFF");
                //dt.Columns["SDMfltMonth3EffortOffShore"].ColumnName = ("SDMMonth3OFF");


                dt.Columns["DMfltTotalOnsite"].ColumnName = ("DMTotalON");
                dt.Columns["DMfltTotalOffShore"].ColumnName = ("DMTotalOFF");
                //dt.Columns["SDMfltTotalOnsite"].ColumnName = ("SDMTotalON");
                //dt.Columns["SDMfltTotalOffShore"].ColumnName = ("SDMTotalOFF");
                dt.Columns["DMfltGrandTotal"].ColumnName = ("DMGrandTotal");

                dt.Columns["txtOnsiteValueM1"].ColumnName = ("RTBRMonth1ON");
                dt.Columns["txtOffshoreValueM1"].ColumnName = ("RTBRMonth1OFF");
                dt.Columns["txtOnsiteValueM2"].ColumnName = ("RTBRMonth2ON");
                dt.Columns["txtOffshoreValueM2"].ColumnName = ("RTBRMonth2OFF");
                dt.Columns["txtOnsiteValueM3"].ColumnName = ("RTBRMonth3ON");
                dt.Columns["txtOffshoreValueM3"].ColumnName = ("RTBRMonth3OFF");
                dt.Columns["txtTotalOnsiteValue"].ColumnName = ("RTBRTotalON");
                dt.Columns["txtTotalOffshoreValue"].ColumnName = ("RTBRTotalOFF");
                dt.Columns["txtGrandTotalValue"].ColumnName = ("RTBRGrandTotal");

                //dt.Columns["SDMfltGrandTotal"].ColumnName = ("SDMGrandTotal");
                dt.Columns["txtQuarterName"].ColumnName = ("txtCurrentQuarterName");
                dt.Columns["txtYear"].ColumnName = ("txtYear");

                return dt;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataTable();
    }

    public DataTable GetDineshReport()
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();

            SqlCommand objcmd;
            SqlParameterCollection objparamcol;

            objData.ExecuteSP("dbo.spBeDineshReportNew", ref ds, cmd);


            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetInpipeReport(string pu, int no, string type)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtPU";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = pu;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@intno";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.Int;
            sqlparam2.Value = no;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@type";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = type;
            objParamColl.Add(sqlparam3);
            objData.ExecuteSP("dbo.spBeINPIPENew", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    //DELEGATE :To poulate the from user & to user dropdownlist
    public List<string> GetDelegateUserId()
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<string> userId = new List<string>();

        try
        {
            objCommand = new SqlCommand();
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEDelegateUserId", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string id = string.Empty;

                    id = ds.Tables[0].Rows[i]["txtUserId"].ToString();
                    userId.Add(id);
                }

            }
        }
        catch (Exception ex)
        {
            //Logger.LogErrorToServer(App_Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return userId;
    }

    //DELEGATE :To populate the grid view
    public List<DelegateUI> GetDelegateDetails()
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<DelegateUI> objDelegate = new List<DelegateUI>();
        DelegateUI objDel = null;
        try
        {
            objCommand = new SqlCommand();
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEDelegateDetails", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objDel = new DelegateUI();

                    objDel.SNo = Convert.ToInt16(ds.Tables[0].Rows[i]["SNo"]);
                    objDel.txtFromUser = ds.Tables[0].Rows[i]["txtFromUser"].ToString();
                    objDel.txtToUser = ds.Tables[0].Rows[i]["txtToUser"].ToString();

                    //objDemandDetails.Startdate = Convert.ToDateTime(ds.Tables[0].Rows[i]["dtStartDate"]).ToString("dd MMM yyyy");

                    objDel.dtFromDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["dtFromDate"]).ToString("MM/dd/yyyy");
                    objDel.dtToDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["dtToDate"]).ToString("MM/dd/yyyy");
                    objDel.lstUserId = GetDelegateUserId();

                    objDelegate.Add(objDel);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return objDelegate;
    }

    //DelegatePage :For updating the date
    public int UpdateDelegatePageDetails(int SNo, string dtFromDate, string dtToDate)
    {
        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            SqlParameter objParamSNo = new SqlParameter();
            objParamSNo.ParameterName = "@SNo";
            objParamSNo.Direction = ParameterDirection.Input;
            objParamSNo.SqlDbType = SqlDbType.Int;
            objParamSNo.Value = SNo;
            objParamColl.Add(objParamSNo);

            SqlParameter objParamFromDate = new SqlParameter();
            objParamFromDate.ParameterName = "@dtFromDate";
            objParamFromDate.Direction = ParameterDirection.Input;
            objParamFromDate.SqlDbType = SqlDbType.Date;
            objParamFromDate.Value = dtFromDate;
            objParamColl.Add(objParamFromDate);

            SqlParameter objParamToDate = new SqlParameter();
            objParamToDate.ParameterName = "@dtToDate";
            objParamToDate.Direction = ParameterDirection.Input;
            objParamToDate.SqlDbType = SqlDbType.Date;
            objParamToDate.Value = dtToDate;
            objParamColl.Add(objParamToDate);

            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.Direction = ParameterDirection.ReturnValue;
            objParamStatus.SqlDbType = SqlDbType.Int;
            objParamStatus.ParameterName = "ReturnValue";
            objParamColl.Add(objParamStatus);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.[spBEDelegatePageUpdateDetails]", objCommand);

            int updatestatus = (int)objCommand.Parameters["ReturnValue"].Value;
            return updatestatus;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }
    //DELEGATE: For adding a new row
    public int UpdateDelegateDetails(int SNo, string txtFromUser, string txtToUser, string dtFromDate, string dtToDate)
    {
        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            SqlParameter objParamSNo = new SqlParameter();
            objParamSNo.ParameterName = "@SNo";
            objParamSNo.Direction = ParameterDirection.Input;
            objParamSNo.SqlDbType = SqlDbType.Int;
            objParamSNo.Value = SNo;
            objParamColl.Add(objParamSNo);

            SqlParameter objParamFromUser = new SqlParameter();
            objParamFromUser.ParameterName = "@txtFromUser";
            objParamFromUser.Direction = ParameterDirection.Input;
            objParamFromUser.SqlDbType = SqlDbType.NVarChar;
            objParamFromUser.Value = txtFromUser;
            objParamColl.Add(objParamFromUser);

            SqlParameter objParamToUser = new SqlParameter();
            objParamToUser.ParameterName = "@txtToUser";
            objParamToUser.Direction = ParameterDirection.Input;
            objParamToUser.SqlDbType = SqlDbType.NVarChar;
            objParamToUser.Value = txtToUser;
            objParamColl.Add(objParamToUser);

            SqlParameter objParamFromDate = new SqlParameter();
            objParamFromDate.ParameterName = "@dtFromDate";
            objParamFromDate.Direction = ParameterDirection.Input;
            objParamFromDate.SqlDbType = SqlDbType.DateTime;
            objParamFromDate.Value = dtFromDate;
            objParamColl.Add(objParamFromDate);

            SqlParameter objParamToDate = new SqlParameter();
            objParamToDate.ParameterName = "@dtToDate";
            objParamToDate.Direction = ParameterDirection.Input;
            objParamToDate.SqlDbType = SqlDbType.DateTime;
            objParamToDate.Value = dtToDate;
            objParamColl.Add(objParamToDate);

            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.Direction = ParameterDirection.ReturnValue;
            objParamStatus.SqlDbType = SqlDbType.Int;
            objParamStatus.ParameterName = "ReturnValue";
            objParamColl.Add(objParamStatus);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.[spBEDelegateUpdateDetails]", objCommand);

            int updatestatus = (int)objCommand.Parameters["ReturnValue"].Value;
            return updatestatus;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public DataTable GetAllParametersReport(string qtr, string year, string dh, string pu, string userid, string type, DateTime date)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtdh";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = dh;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@datedd";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.DateTime;
            sqlparam7.Value = date;
            objParamColl.Add(sqlparam7);

            objData.ExecuteSP("dbo.spBEAllParameters_date", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }
    public DataTable GetComparisonReport()
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();

            objData.ExecuteSP("dbo.spBEComparisionReport", ref ds, cmd);

            return ds.Tables[0];
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }
    //public DataTable GetBEReport(string qtr, string year)
    //{

    //    DataSet dsCurrConv = new DataSet();
    //    SqlCommand sqlcmd = new SqlCommand();
    //    //SqlDataAdapter daCurrConv = new SqlDataAdapter();
    //    try
    //    {
    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        SqlCommand objCommand;
    //        SqlParameterCollection objParamColl;

    //        SqlParameter sqlparam1, sqlparam2;

    //        objCommand = new SqlCommand();
    //        objParamColl = objCommand.Parameters;



    //        sqlparam1 = new SqlParameter();
    //        sqlparam1.ParameterName = "@txtCurQuarterName";
    //        sqlparam1.Direction = ParameterDirection.Input;
    //        sqlparam1.SqlDbType = SqlDbType.VarChar;
    //        sqlparam1.Value = qtr;
    //        objParamColl.Add(sqlparam1);

    //        sqlparam2 = new SqlParameter();
    //        sqlparam2.ParameterName = "@finyear";
    //        sqlparam2.Direction = ParameterDirection.Input;
    //        sqlparam2.SqlDbType = SqlDbType.VarChar;
    //        sqlparam2.Value = year;
    //        objParamColl.Add(sqlparam2);

    //        objData.ExecuteSP("dbo.spBE_Report", ref  dsCurrConv, objCommand);

    //        return dsCurrConv.Tables[0]; ;
    //    }
    //    catch (Exception ex)
    //    {

    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }


    //}
    public DataTable GetBEReport(string qtr, string year, string pu, string dh, string userid, string type)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtdh";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = dh;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);

            objData.ExecuteSP("dbo.spBE_Report", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetBEReportCSI(string qtr, string year, string pu, string userid, string type, DateTime date)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam5, sqlparam6, sqlparam7;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@datedd";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.DateTime;
            sqlparam7.Value = date;
            objParamColl.Add(sqlparam7);

            objData.ExecuteSP("dbo.spBeReportForCSI", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetBEReportQtrYear(string type, string qtr)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@type";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = type;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@qtr";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = qtr;
            objParamColl.Add(sqlparam2);
            objData.ExecuteSP("dbo.EAS_SP_BEQtrYearAdmin", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }


    //public DataTable GetBEReportQtrYear(string type)
    //{

    //    DataSet ds = new DataSet();
    //    SqlCommand cmd = new SqlCommand();
    //    try
    //    {

    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        SqlCommand objCommand;
    //        SqlParameterCollection objParamColl;

    //        SqlParameter sqlparam1;

    //        objCommand = new SqlCommand();
    //        objParamColl = objCommand.Parameters;



    //        sqlparam1 = new SqlParameter();
    //        sqlparam1.ParameterName = "@type";
    //        sqlparam1.Direction = ParameterDirection.Input;
    //        sqlparam1.SqlDbType = SqlDbType.VarChar;
    //        sqlparam1.Value = type;
    //        objParamColl.Add(sqlparam1);
    //        objData.ExecuteSP("dbo.spBEQtrYearAdmin", ref  ds, objCommand);

    //        return ds.Tables[0]; ;
    //    }
    //    catch (Exception ex)
    //    {

    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }


    //}


    public DataTable GetExchangeActQtr()
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            objData.ExecuteSP("dbo.spBeGetQuarterExchangeRate", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }



    public DataTable GetExchangeActYear()
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            objData.ExecuteSP("dbo.spBeGetYearExchangeRate", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

    }


    public DataTable FetchMonthlyActRate(string qtr, string year)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();

        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@qtr";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);


            objData.ExecuteSP("dbo.spBEReportActAndCurrRate", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }
    public List<DMDetailsPopUp> GetBEPopUpDMValuesRevforDMView(int BEID)
    {
        DataSet ds = new DataSet();
        List<DMDetailsPopUp> allport = new List<DMDetailsPopUp>();
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        SqlCommand objCommand;
        try
        {

            objData = new DataAccess();

            objParm = new SqlParameter();
            objParm.ParameterName = "@beid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = BEID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData.GetConnection();
            objData.ExecuteSP("spBEPopUpDMValuesRevforDMView", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    allport.Add(new DMDetailsPopUp()
                    {
                        txtCustomerCode = ds.Tables[0].Rows[i]["txtCustomerCode"] + "",
                        txtCurrency = ds.Tables[0].Rows[i]["txtNativeCurrency"] + "",
                        txtDMMailId = ds.Tables[0].Rows[i]["txtDMMailId"] + "",
                        DMMonth1 = ds.Tables[0].Rows[i]["DMfltMonth1BE"] + "",
                        DMMonth2 = ds.Tables[0].Rows[i]["DMfltMonth2BE"] + "",
                        DMMonth3 = ds.Tables[0].Rows[i]["DMfltMonth3BE"] + "",
                        PU = ds.Tables[0].Rows[i]["txtPU"] + "",
                        total = ds.Tables[0].Rows[i]["Total"] + ""
                    });
                    // allport.Add(ds.Tables[0].Rows[i]["txtPortfolio"] + "");

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allport;

    }

    public List<DMDetailsPopUp> GetBEPopUpDMValuesRevforSDMView(int BEID)
    {
        DataSet ds = new DataSet();
        List<DMDetailsPopUp> allport = new List<DMDetailsPopUp>();
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        SqlCommand objCommand;
        try
        {

            objData = new DataAccess();

            objParm = new SqlParameter();
            objParm.ParameterName = "@beid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = BEID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData.GetConnection();
            objData.ExecuteSP("spBEPopUpDMValuesRevforSDMView", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    allport.Add(new DMDetailsPopUp()
                    {
                        txtCustomerCode = ds.Tables[0].Rows[i]["txtCustomerCode"] + "",
                        txtCurrency = ds.Tables[0].Rows[i]["txtNativeCurrency"] + "",
                        txtDMMailId = ds.Tables[0].Rows[i]["txtDMMailId"] + "",
                        DMMonth1 = ds.Tables[0].Rows[i]["DMfltMonth1BE"] + "",
                        DMMonth2 = ds.Tables[0].Rows[i]["DMfltMonth2BE"] + "",
                        DMMonth3 = ds.Tables[0].Rows[i]["DMfltMonth3BE"] + "",
                        PU = ds.Tables[0].Rows[i]["txtPU"] + "",
                        total = ds.Tables[0].Rows[i]["Total"] + ""
                    });

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allport;

    }

    //public DataTable GetBEPopUpDMValuesRevforSDMTotalView(string BEID)
    //{
    //    DataSet ds = new DataSet();

    //    SqlParameter objParm;
    //    SqlParameterCollection objParamColl;
    //    SqlCommand objCommand;
    //    //string sdmttotal = string.Empty;
    //    //string sdmMailId = string.Empty;
    //    //List<sdmDetailsUI> sdmDetails = new List<sdmDetailsUI>;
    //    //sdmDetailsUI objSDM = new sdmDetailsUI();
    //    try
    //    {
    //        //sdmDetails = new List<sdmDetailsUI>();
    //        objData = new DataAccess();

    //        objParm = new SqlParameter();
    //        objParm.ParameterName = "@beid";
    //        objParm.Direction = ParameterDirection.Input;
    //        objParm.SqlDbType = SqlDbType.NVarChar;
    //        objParm.Value = BEID;

    //        objCommand = new SqlCommand();
    //        objParamColl = objCommand.Parameters;
    //        objParamColl.Add(objParm);

    //        objData.GetConnection();
    //        objData.ExecuteSP("spBEPopUpSDMValueRevforDMView", ref ds, objCommand);

    //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
    //        {
    //            DataTable dt = new DataTable();
    //            dt = ds.Tables[0];
    //            return dt;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }
    //    return new DataTable();
    //}

    public List<DMDetailsPopUp> GetBEPopUpDMValuesVolforSDMView(int BEID)
    {
        DataSet ds = new DataSet();
        List<DMDetailsPopUp> allport = new List<DMDetailsPopUp>();
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        SqlCommand objCommand;
        try
        {

            objData = new DataAccess();

            objParm = new SqlParameter();
            objParm.ParameterName = "@beid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = BEID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData.GetConnection();
            objData.ExecuteSP("spBEPopUpDMValuesVolforSDMView", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    allport.Add(new DMDetailsPopUp() { txtCustomerCode = ds.Tables[0].Rows[i]["txtCustomerCode"] + "", txtDMMailId = ds.Tables[0].Rows[i]["txtDMMailId"] + "", txtPU = ds.Tables[0].Rows[i]["txtPU"] + "", DMMonth1Onsite = ds.Tables[0].Rows[i]["fltDMEffortMonth1Onsite"] + "", DMMonth2Onsite = ds.Tables[0].Rows[i]["fltDMEffortMonth2Onsite"] + "", DMMonth3Onsite = ds.Tables[0].Rows[i]["fltDMEffortMonth3Onsite"] + "", DMMonth1Offshore = ds.Tables[0].Rows[i]["fltDMEffortMonth1OffShore"] + "", DMMonth2Offshore = ds.Tables[0].Rows[i]["fltDMEffortMonth2Offshore"] + "", DMMonth3Offshore = ds.Tables[0].Rows[i]["fltDMEffortMonth3Offshore"] + "", DMOnsiteOffshoreTotal = ds.Tables[0].Rows[i]["Total"] + "" });
                    // allport.Add(ds.Tables[0].Rows[i]["txtPortfolio"] + "");

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allport;

    }





    public List<DMDetailsPopUp> GetBEPopUpDMValuesVolforDMView(int BEID)
    {
        DataSet ds = new DataSet();
        List<DMDetailsPopUp> allport = new List<DMDetailsPopUp>();
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        SqlCommand objCommand;
        try
        {

            objData = new DataAccess();

            objParm = new SqlParameter();
            objParm.ParameterName = "@beid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = BEID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData.GetConnection();
            objData.ExecuteSP("spBEPopUpDMValuesVolforDMView", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    allport.Add(new DMDetailsPopUp() { txtCustomerCode = ds.Tables[0].Rows[i]["txtCustomerCode"] + "", txtDMMailId = ds.Tables[0].Rows[i]["txtDMMailId"] + "", txtPU = ds.Tables[0].Rows[i]["txtPU"] + "", DMMonth1Onsite = ds.Tables[0].Rows[i]["fltDMEffortMonth1Onsite"] + "", DMMonth2Onsite = ds.Tables[0].Rows[i]["fltDMEffortMonth2Onsite"] + "", DMMonth3Onsite = ds.Tables[0].Rows[i]["fltDMEffortMonth3Onsite"] + "", DMMonth1Offshore = ds.Tables[0].Rows[i]["fltDMEffortMonth1OffShore"] + "", DMMonth2Offshore = ds.Tables[0].Rows[i]["fltDMEffortMonth2Offshore"] + "", DMMonth3Offshore = ds.Tables[0].Rows[i]["fltDMEffortMonth3Offshore"] + "", DMOnsiteOffshoreTotal = ds.Tables[0].Rows[i]["Total"] + "" });
                    //allport.Add(new DMDetailsPopUp() { txtCustomerCode = ds.Tables[0].Rows[i]["txtCustomerCode"] + "", txtCurrency = ds.Tables[0].Rows[i]["txtNativeCurrency"] + "", txtDMMailId = ds.Tables[0].Rows[i]["txtDMMailId"] + "", DMMonth1 = ds.Tables[0].Rows[i]["DMfltMonth1BE"] + "", DMMonth2 = ds.Tables[0].Rows[i]["DMfltMonth2BE"] + "", DMMonth3 = ds.Tables[0].Rows[i]["DMfltMonth3BE"] + "" });
                    // allport.Add(ds.Tables[0].Rows[i]["txtPortfolio"] + "");

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allport;

    }




    public string GetBEPopUpDMValuesVolforSDMTotalView(int BEID)
    {
        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        SqlCommand objCommand;
        string sdmttotal = string.Empty;
        try
        {

            objData = new DataAccess();

            objParm = new SqlParameter();
            objParm.ParameterName = "@beid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = BEID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData.GetConnection();
            objData.ExecuteSP("spBEPopUpSDMValueVolforDMView", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    sdmttotal = ds.Tables[0].Rows[0]["SDMTotal"] + "";
                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return sdmttotal;

    }

    public List<string> GetCustomerCodeForPUVol(string userid, string su)
    {

        DataSet ds = new DataSet();
        SqlParameter objParam1, objParam2;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<string> lstCustomerCode = new List<string>();
        try
        {

            //objParm = new SqlParameter();
            //objParm.ParameterName = "@PU";
            //objParm.Direction = ParameterDirection.Input;
            //objParm.SqlDbType = SqlDbType.VarChar;
            //objParm.Value = pu;

            objParam1 = new SqlParameter();
            objParam1.ParameterName = "@userid";
            objParam1.Direction = ParameterDirection.Input;
            objParam1.SqlDbType = SqlDbType.VarChar;
            objParam1.Value = userid;

            objParam2 = new SqlParameter();
            objParam2.ParameterName = "@type";
            objParam2.Direction = ParameterDirection.Input;
            objParam2.SqlDbType = SqlDbType.VarChar;
            objParam2.Value = su;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            // objParamColl.Add(objParm);
            objParamColl.Add(objParam1);
            objParamColl.Add(objParam2);


            objData = new DataAccess();
            objData.GetConnection();
            //objData.ExecuteSP("EAS_SP_BeGetCustomerListAlcon", ref ds, objCommand);
            objData.ExecuteSP("spBeGetCustomerListForRTBR_dummy_nso", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    string customerCode = string.Empty;

                    customerCode = ds.Tables[0].Rows[i]["txtmcc"].ToString();
                    lstCustomerCode.Add(customerCode);

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        return lstCustomerCode;
    }

    public DataTable GetCustomerCodeAlcon(string userid, string su)
    {

        DataSet ds = new DataSet();
        SqlParameter objParam1, objParam2;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<string> lstCustomerCode = new List<string>();
        try
        {

            //objParm = new SqlParameter();
            //objParm.ParameterName = "@PU";
            //objParm.Direction = ParameterDirection.Input;
            //objParm.SqlDbType = SqlDbType.VarChar;
            //objParm.Value = pu;

            objParam1 = new SqlParameter();
            objParam1.ParameterName = "@userid";
            objParam1.Direction = ParameterDirection.Input;
            objParam1.SqlDbType = SqlDbType.VarChar;
            objParam1.Value = userid;

            objParam2 = new SqlParameter();
            objParam2.ParameterName = "@type";
            objParam2.Direction = ParameterDirection.Input;
            objParam2.SqlDbType = SqlDbType.VarChar;
            objParam2.Value = su;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            // objParamColl.Add(objParm);
            objParamColl.Add(objParam1);
            objParamColl.Add(objParam2);


            objData = new DataAccess();
            objData.GetConnection();
            //objData.ExecuteSP("EAS_SP_BeGetCustomerListAlcon", ref ds, objCommand);
            objData.ExecuteSP("spBeGetCustomerListForRTBR_dummy", ref ds, objCommand);

            return ds.Tables[0];
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }


    public List<string> GetCustomerCodeForPU(string userid, string PU)
    {

        DataSet ds = new DataSet();
        SqlParameter objParam1, objParam2;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<string> lstCustomerCode = new List<string>();
        try
        {

            //objParm = new SqlParameter();
            //objParm.ParameterName = "@PU";
            //objParm.Direction = ParameterDirection.Input;
            //objParm.SqlDbType = SqlDbType.VarChar;
            //objParm.Value = pu;

            objParam1 = new SqlParameter();
            objParam1.ParameterName = "@userid";
            objParam1.Direction = ParameterDirection.Input;
            objParam1.SqlDbType = SqlDbType.VarChar;
            objParam1.Value = userid;

            objParam2 = new SqlParameter();
            objParam2.ParameterName = "@SU";
            objParam2.Direction = ParameterDirection.Input;
            objParam2.SqlDbType = SqlDbType.VarChar;
            objParam2.Value = PU;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            // objParamColl.Add(objParm);
            objParamColl.Add(objParam1);
            objParamColl.Add(objParam2);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_BeGetCustomerListAlcon", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    string customerCode = string.Empty;

                    customerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();
                    lstCustomerCode.Add(customerCode);

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        return lstCustomerCode;
    }

    public DataTable GetSUBeReport(string userid)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@userid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = userid;
            objParamColl.Add(sqlparam1);
            objData.ExecuteSP("dbo.EAS_SP_BEGetSU_NSO", ref  ds, objCommand);

            return ds.Tables[0];
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }



    public DataTable GetSDMMailID(string userid)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            //sqlparam1 = new SqlParameter();
            //sqlparam1.ParameterName = "@userid";
            //sqlparam1.Direction = ParameterDirection.Input;
            //sqlparam1.SqlDbType = SqlDbType.VarChar;
            //sqlparam1.Value = userid;
            //objParamColl.Add(sqlparam1);
            objData.ExecuteSP("dbo.spBEGetSU", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

    }

    public DataTable GetPUAlconReport(string userid)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm, objParm1;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        try
        {

            objParm = new SqlParameter();
            objParm.ParameterName = "@UserId";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = userid;

            //objParm1 = new SqlParameter();
            //objParm1.ParameterName = "@txtSU";
            //objParm1.Direction = ParameterDirection.Input;
            //objParm1.SqlDbType = SqlDbType.VarChar;
            //objParm1.Value = su;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            objParamColl.Add(objParm);
            //objParamColl.Add(objParm1);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBENSOList", ref ds, objCommand);
            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetQuarterYearDineshReport(string type, string qtr)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@type";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = type;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@qtr";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = qtr;
            objParamColl.Add(sqlparam2);
            objData.ExecuteSP("dbo.spBeGetQuarterYearForDineshRep", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }
    public DataTable GetDineshReport(string qtr, string year, string pu, string zero, string type)
    {
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtQtr";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtFinYear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);


            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@iszero";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = zero;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@type";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = type;
            objParamColl.Add(sqlparam5);


            objData.ExecuteSP("dbo.spBeDineshReportNew", ref  ds, objCommand);

            return ds.Tables[0]; ;


        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetDineshReportSum(string qtr, string year, string pu, string zero, string type)
    {
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtQtr";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtFinYear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);


            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@iszero";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = zero;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@type";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = type;
            objParamColl.Add(sqlparam5);


            objData.ExecuteSP("dbo.spBeDineshReportNew_Pivot", ref  ds, objCommand);

            return ds.Tables[0]; ;


        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetInpipeReport(string pu, int no)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtPU";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = pu;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@intno";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.Int;
            sqlparam2.Value = no;
            objParamColl.Add(sqlparam2);
            objData.ExecuteSP("dbo.spBeINPIPENew", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetPUInpipeReport(string userid, string su)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@userid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = userid;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@su";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = su;
            objParamColl.Add(sqlparam2);
            objData.ExecuteSP("dbo.spGetPUInpipeReport", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }
    public DataTable GetDHFromSuBeReport(string userid, string su)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@userid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = userid;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@su";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = su;
            objParamColl.Add(sqlparam2);
            objData.ExecuteSP("dbo.spBeGetDHBeReport", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }


    public DataTable GetSDMFromSuBeReport(string userid, string su)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@userid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = userid;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@su";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = su;
            objParamColl.Add(sqlparam2);
            objData.ExecuteSP("dbo.spBeGetSDMBeReport", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetPUBeReport(string dh)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@dh";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = dh;
            objParamColl.Add(sqlparam1);
            objData.ExecuteSP("dbo.spGetPUBeReport", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    //public DataTable GetBEReportQtrYear(string type, string qtr)
    //{

    //    DataSet ds = new DataSet();
    //    SqlCommand cmd = new SqlCommand();
    //    try
    //    {

    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        SqlCommand objCommand;
    //        SqlParameterCollection objParamColl;

    //        SqlParameter sqlparam1, sqlparam2;

    //        objCommand = new SqlCommand();
    //        objParamColl = objCommand.Parameters;



    //        sqlparam1 = new SqlParameter();
    //        sqlparam1.ParameterName = "@type";
    //        sqlparam1.Direction = ParameterDirection.Input;
    //        sqlparam1.SqlDbType = SqlDbType.VarChar;
    //        sqlparam1.Value = type;
    //        objParamColl.Add(sqlparam1);

    //        sqlparam2 = new SqlParameter();
    //        sqlparam2.ParameterName = "@qtr";
    //        sqlparam2.Direction = ParameterDirection.Input;
    //        sqlparam2.SqlDbType = SqlDbType.VarChar;
    //        sqlparam2.Value = qtr;
    //        objParamColl.Add(sqlparam2);
    //        objData.ExecuteSP("dbo.spBEQtrYearAdmin", ref  ds, objCommand);

    //        return ds.Tables[0]; ;
    //    }
    //    catch (Exception ex)
    //    {

    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }


    //}

    public DataTable GetAllParametersReport(string qtr, string year, string dh, string pu, string userid, string type)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtdh";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = dh;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);

            objData.ExecuteSP("dbo.spBEAllParameters", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }
    public DataTable GetComparisonReport(string qtr, string year, string dh, string pu, string userid, string type, string paramName, string paramvalue)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtdh";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = dh;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@typeofReport";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = paramName;
            objParamColl.Add(sqlparam7);

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@valuefortype";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.SqlDbType = SqlDbType.VarChar;
            sqlparam8.Value = paramvalue;
            objParamColl.Add(sqlparam8);

            objData.ExecuteSP("dbo.spBEComparisionReport", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable ComparisonParameterValues(string paramName)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;




            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@paramName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = paramName;
            objParamColl.Add(sqlparam1);


            objData.ExecuteSP("dbo.spGetComparisonParameter", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public List<DMDetailsPopUp> GetBEPopUpDMValuesRevforDMView(string BEID)
    {
        DataSet ds = new DataSet();
        List<DMDetailsPopUp> allport = new List<DMDetailsPopUp>();
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        SqlCommand objCommand;
        try
        {

            objData = new DataAccess();

            objParm = new SqlParameter();
            objParm.ParameterName = "@beid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = BEID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData.GetConnection();
            objData.ExecuteSP("spBEPopUpDMValuesRevforDMView", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    allport.Add(new DMDetailsPopUp()
                    {
                        txtCustomerCode = ds.Tables[0].Rows[i]["txtCustomerCode"] + "",
                        txtCurrency = ds.Tables[0].Rows[i]["txtNativeCurrency"] + "",
                        txtDMMailId = ds.Tables[0].Rows[i]["txtDMMailId"] + "",
                        DMMonth1 = ds.Tables[0].Rows[i]["DMfltMonth1BE"] + "",
                        DMMonth2 = ds.Tables[0].Rows[i]["DMfltMonth2BE"] + "",
                        DMMonth3 = ds.Tables[0].Rows[i]["DMfltMonth3BE"] + "",
                        PU = ds.Tables[0].Rows[i]["txtPU"] + "",
                        total = ds.Tables[0].Rows[i]["Total"] + ""
                    });
                    // allport.Add(ds.Tables[0].Rows[i]["txtPortfolio"] + "");

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allport;

    }

    public List<DMDetailsPopUp> GetBEPopUpDMValuesRevforSDMView(string BEID)
    {
        DataSet ds = new DataSet();
        List<DMDetailsPopUp> allport = new List<DMDetailsPopUp>();
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        SqlCommand objCommand;
        try
        {

            objData = new DataAccess();

            objParm = new SqlParameter();
            objParm.ParameterName = "@beid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = BEID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData.GetConnection();
            objData.ExecuteSP("spBEPopUpDMValuesRevforSDMView", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    allport.Add(new DMDetailsPopUp()
                    {
                        txtCustomerCode = ds.Tables[0].Rows[i]["txtCustomerCode"] + "",
                        txtCurrency = ds.Tables[0].Rows[i]["txtNativeCurrency"] + "",
                        txtDMMailId = ds.Tables[0].Rows[i]["txtDMMailId"] + "",
                        DMMonth1 = ds.Tables[0].Rows[i]["DMfltMonth1BE"] + "",
                        DMMonth2 = ds.Tables[0].Rows[i]["DMfltMonth2BE"] + "",
                        DMMonth3 = ds.Tables[0].Rows[i]["DMfltMonth3BE"] + "",
                        PU = ds.Tables[0].Rows[i]["txtPU"] + "",
                        total = ds.Tables[0].Rows[i]["Total"] + ""
                    });

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allport;

    }

    public DataTable GetBEPopUpDMValuesRevforSDMTotalView(string BEID)
    {
        DataSet ds = new DataSet();

        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        SqlCommand objCommand;
        //string sdmttotal = string.Empty;
        //string sdmMailId = string.Empty;
        //List<sdmDetailsUI> sdmDetails = new List<sdmDetailsUI>;
        //sdmDetailsUI objSDM = new sdmDetailsUI();
        try
        {
            //sdmDetails = new List<sdmDetailsUI>();
            objData = new DataAccess();

            objParm = new SqlParameter();
            objParm.ParameterName = "@beid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = BEID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData.GetConnection();
            objData.ExecuteSP("spBEPopUpSDMValueRevforDMView", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                return dt;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return new DataTable();
    }

    public List<DMDetailsPopUp> GetBEPopUpDMValuesVolforSDMView(string BEID)
    {
        DataSet ds = new DataSet();
        List<DMDetailsPopUp> allport = new List<DMDetailsPopUp>();
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        SqlCommand objCommand;
        try
        {

            objData = new DataAccess();

            objParm = new SqlParameter();
            objParm.ParameterName = "@beid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = BEID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData.GetConnection();
            objData.ExecuteSP("EAS_spBEPopUpDMValuesVolforDMView", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    allport.Add(new DMDetailsPopUp() { txtCustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"] + "", txtDMMailId = ds.Tables[0].Rows[i]["txtDMMailId"] + "", txtPU = ds.Tables[0].Rows[i]["txtPU"] + "", DMMonth1Onsite = ds.Tables[0].Rows[i]["fltDMMonth1onsite"] + "", DMMonth2Onsite = ds.Tables[0].Rows[i]["fltDMMonth2onsite"] + "", DMMonth3Onsite = ds.Tables[0].Rows[i]["fltDMMonth3onsite"] + "", DMMonth1Offshore = ds.Tables[0].Rows[i]["fltDMMonth1offsite"] + "", DMMonth2Offshore = ds.Tables[0].Rows[i]["fltDMMonth2offsite"] + "", DMMonth3Offshore = ds.Tables[0].Rows[i]["fltDMMonth3offsite"] + "", DMTotalOffshore = ds.Tables[0].Rows[i]["fltDMMonthTotaloffsite"] + "", DMTotalOnsite = ds.Tables[0].Rows[i]["fltDMMonthTotalonsite"] + "", DMOnsiteOffshoreTotal = ds.Tables[0].Rows[i]["fltDMMonthTotalVol"] + "" });                    // allport.Add(ds.Tables[0].Rows[i]["txtPortfolio"] + "");

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allport;

    }





    public List<DMDetailsPopUp> GetBEPopUpDMValuesVolforDMView(string BEID)
    {
        DataSet ds = new DataSet();
        List<DMDetailsPopUp> allport = new List<DMDetailsPopUp>();
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        SqlCommand objCommand;
        try
        {

            objData = new DataAccess();

            objParm = new SqlParameter();
            objParm.ParameterName = "@beid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = BEID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData.GetConnection();
            objData.ExecuteSP("spBEPopUpDMValuesVolforDMView", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    allport.Add(new DMDetailsPopUp() { txtCustomerCode = ds.Tables[0].Rows[i]["txtCustomerCode"] + "", txtDMMailId = ds.Tables[0].Rows[i]["txtDMMailId"] + "", txtPU = ds.Tables[0].Rows[i]["txtPU"] + "", DMMonth1Onsite = ds.Tables[0].Rows[i]["fltDMEffortMonth1Onsite"] + "", DMMonth2Onsite = ds.Tables[0].Rows[i]["fltDMEffortMonth2Onsite"] + "", DMMonth3Onsite = ds.Tables[0].Rows[i]["fltDMEffortMonth3Onsite"] + "", DMMonth1Offshore = ds.Tables[0].Rows[i]["fltDMEffortMonth1OffShore"] + "", DMMonth2Offshore = ds.Tables[0].Rows[i]["fltDMEffortMonth2Offshore"] + "", DMMonth3Offshore = ds.Tables[0].Rows[i]["fltDMEffortMonth3Offshore"] + "", DMOnsiteOffshoreTotal = ds.Tables[0].Rows[i]["Total"] + "" });
                    //allport.Add(new DMDetailsPopUp() { txtCustomerCode = ds.Tables[0].Rows[i]["txtCustomerCode"] + "", txtCurrency = ds.Tables[0].Rows[i]["txtNativeCurrency"] + "", txtDMMailId = ds.Tables[0].Rows[i]["txtDMMailId"] + "", DMMonth1 = ds.Tables[0].Rows[i]["DMfltMonth1BE"] + "", DMMonth2 = ds.Tables[0].Rows[i]["DMfltMonth2BE"] + "", DMMonth3 = ds.Tables[0].Rows[i]["DMfltMonth3BE"] + "" });
                    // allport.Add(ds.Tables[0].Rows[i]["txtPortfolio"] + "");

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allport;

    }




    public DataTable GetBEPopUpDMValuesVolforSDMTotalView(string BEID)
    {
        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        SqlCommand objCommand;
        string sdmttotal = string.Empty;
        try
        {
            DataTable dt = new DataTable();
            objData = new DataAccess();

            objParm = new SqlParameter();
            objParm.ParameterName = "@beid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = BEID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData.GetConnection();
            objData.ExecuteSP("spBEPopUpSDMValueVolforDMView", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                return dt;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataTable();

    }
    public DateTime GetInpipeDate()
    {

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand("select MAX(dtfrozendate) as dtfrozendate from BeRevSDMWeekly");
            objParamColl = objCommand.Parameters;

            dt = objData.ExecuteSP(objCommand);

            string datetime = dt.Rows[0][0].ToString();


            DateTime date = Convert.ToDateTime(datetime);
            return date;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetMCOReport(string type, string pu, string userid, int no)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam4, sqlparam5;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@type";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = type;
            objParamColl.Add(sqlparam1);





            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@userid";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = userid;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@PU";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = pu;
            objParamColl.Add(sqlparam5);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@intno";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.Int;
            sqlparam2.Value = no;
            objParamColl.Add(sqlparam2);

            objData.ExecuteSP("dbo.spBeMCOUSDNC", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }
    public string GetMCODumpDate()
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;

        string tmp = string.Empty;

        try
        {

            objCommand = new SqlCommand();
            objData = new DataAccess();
            objData.GetConnection();
            // objData.ExecuteSP("spBEDUListForDropDown", ref ds, objCommand);
            objData.ExecuteSP("spGetMCODumpDate", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    tmp = Convert.ToDateTime(ds.Tables[0].Rows[i]["DumpDate"]).ToString("dd-MMM-yyyy hh:mm IST");
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return tmp;
    }

    public string GetAlconDumpDate()
    {
        DataSet ds = new DataSet();

        SqlCommand objCommand;

        string tmp = string.Empty;

        try
        {

            objCommand = new SqlCommand();
            objData = new DataAccess();
            objData.GetConnection();
            // objData.ExecuteSP("spBEDUListForDropDown", ref ds, objCommand);
            objData.ExecuteSP("EAS_SP_GetAlconDumpDate", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    tmp = Convert.ToDateTime(ds.Tables[0].Rows[i]["DumpDate"]).ToString("dd-MMM-yyyy HH:mm IST");
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return tmp;
    }



    public DataTable RTBRGetCustomerList(string userid, string type)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;




            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@userid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = userid;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@type";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = type;
            objParamColl.Add(sqlparam2);





            objData.ExecuteSP("spBeGetCustomerListForRTBR_dummy_NSO", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }


    public DataTable RTBRGetPUList(string userid, string type)
    {
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;
            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@userid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = userid;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@type";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = type;
            objParamColl.Add(sqlparam2);

            objData.ExecuteSP("spBeGetPUListForRTBRReport_NSO", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public DataTable RTBRGetCustomerListForSUMCC(string userid, string SU, string NSO)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;
            SqlParameter sqlparam1, sqlparam2, sqlparam3;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@userid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = userid;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@SU";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = SU;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@newOffering";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = NSO;
            objParamColl.Add(sqlparam3);

            objData.ExecuteSP("spBeGetCustomerListForSU_nso", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

    }

    public DataTable TrendsGetCustomerList(string userid, string pu)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;




            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@userid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = userid;
            objParamColl.Add(sqlparam1);




            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@pu";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);


            objData.ExecuteSP("dbo.spBeGetCustomerListForTrends", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetRTBRAllPuReport(string mcc, string year, string type)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;




            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@year";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.NVarChar;
            sqlparam1.Value = year;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@type";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = type;
            objParamColl.Add(sqlparam2);


            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@MCC";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = mcc;
            objParamColl.Add(sqlparam3);


            objData.ExecuteSP("dbo.spBeRtbrUSDNCAllPu", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }
    public DataSet GetBEVolComparison(string qtr, string year)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        try
        {
            objCommand = new SqlCommand();
            objData = new DataAccess();

            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@Quarter";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.NVarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@Year";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);

            objData.GetConnection();
            objData.ExecuteSP("spBEDelBEVolComparison", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                return ds;
            }

        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return new DataSet();

    }

    public DataTable SearchMcc(string Mcc, string userid)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;




            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@Mcc";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = Mcc;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtuserid";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = userid;
            objParamColl.Add(sqlparam2);


            objData.ExecuteSP("dbo.EAS_SP_BEGetCCPFields_Mcc", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }


    //public DataSet GetDMSDM_MccDMSDMChange(string type, string pu, string mcc, string qtr, string year)
    //{

    //         DataSet ds = new DataSet();
    //    DataSet dsreturn = new DataSet();
    //    SqlCommand objCommand;
    //    List<DataTable> retTable = new List<DataTable>();

    //    try
    //    {


    //        objCommand = new SqlCommand();
    //        SqlParameter objParamStatus1 = new SqlParameter();
    //        objParamStatus1.ParameterName = "@MasterClientCode";
    //        objParamStatus1.Direction = ParameterDirection.Input;
    //        objParamStatus1.SqlDbType = SqlDbType.VarChar;
    //       // objParamStatus1.Value = customerCode;

    //        SqlParameter objParamStatus2 = new SqlParameter();
    //        objParamStatus2.ParameterName = "@UserId";
    //        objParamStatus2.Direction = ParameterDirection.Input;
    //        objParamStatus2.SqlDbType = SqlDbType.VarChar;
    //       // objParamStatus2.Value = userid;

    //        SqlParameter objParamStatus3 = new SqlParameter();
    //        objParamStatus3.ParameterName = "@Quarter";
    //        objParamStatus3.Direction = ParameterDirection.Input;
    //        objParamStatus3.SqlDbType = SqlDbType.VarChar;
    //       // objParamStatus3.Value = quarter;

    //        SqlParameter objParamStatus4 = new SqlParameter();
    //        objParamStatus4.ParameterName = "@FYYR";
    //        objParamStatus4.Direction = ParameterDirection.Input;
    //        objParamStatus4.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus4.Value = year;

    //        SqlParameter objParamStatus = new SqlParameter();
    //        objParamStatus.ParameterName = "@PU";
    //        objParamStatus.Direction = ParameterDirection.Input;
    //        objParamStatus.SqlDbType = SqlDbType.VarChar;
    //      //  objParamStatus.Value = PU;






    //        objCommand = new SqlCommand();
    //        SqlParameterCollection objParamColl = objCommand.Parameters;


    //        objParamColl.Add(objParamStatus);

    //        objParamColl.Add(objParamStatus1);
    //        objParamColl.Add(objParamStatus2);
    //        objParamColl.Add(objParamStatus3);

    //        objParamColl.Add(objParamStatus4);


    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("EAS_SP_Fetch_BEData_DM", ref ds, objCommand);
    //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
    //        {
    //            DataTable dt = new DataTable();
    //            dt = ds.Tables[0];
    //            //dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
    //            //dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
    //            //dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
    //            //dt.Columns["txtQuarterName"].ColumnName = "Current Quarter Name";
    //            //dt.Columns["txtYear"].ColumnName = "Year";

    //            ////BE
    //            //dt.Columns["DMfltMonth1BE"].ColumnName = "DMMonth1";
    //            //dt.Columns["DMfltMonth2BE"].ColumnName = "DMMonth2";
    //            //dt.Columns["DMfltMonth3BE"].ColumnName = "DMMonth3";
    //            //dt.Columns["DMfltCurrentQuarterBE"].ColumnName = "DMQCur";
    //            //dt.Columns["SDMBK1"].ColumnName = "SDMBK1";
    //            //dt.Columns["SDMBK2"].ColumnName = "SDMBK2";
    //            //dt.Columns["SDMBK3"].ColumnName = "SDMBK3";
    //            //dt.Columns["SDMBK4"].ColumnName = "SDMBK4";
    //            //dt.Columns["txtSDMBERemarks"].ColumnName = "SDMBERemarks";

    //            ////Vol

    //            //dt.Columns["txtOnsiteValueM1"].ColumnName = "OnsiteValueM1";
    //            //dt.Columns["txtOffshoreValueM1"].ColumnName = "OffshoreValueM1";
    //            //dt.Columns["txtOnsiteValueM2"].ColumnName = "OnsiteValueM2";
    //            //dt.Columns["txtOffshoreValueM2"].ColumnName = "OffshoreValueM2";
    //            //dt.Columns["txtOnsiteValueM3"].ColumnName = "OnsiteValueM3";
    //            //dt.Columns["txtOffshoreValueM3"].ColumnName = "OffshoreValueM3";

    //            //dt.Columns["txtTotalOnsiteValue"].ColumnName = "TotalOnsiteValue";
    //            //dt.Columns["txtTotalOffshoreValue"].ColumnName = "TotalOffshoreValue";
    //            //dt.Columns["txtGrandTotalValue"].ColumnName = "GrandTotalValue";
    //            //dt.Columns["txtSDMVolumeRemarks"].ColumnName = "SDMVolumeRemarks";

    //            //dt.Columns["dtDMUpdatedDate"].ColumnName = "dtDMUpdatedDate";



    //            dsreturn.Tables.Add(dt.Copy());
    //            //dsreturn.Tables.Add(ds.Tables[1].Copy());

    //            return dsreturn;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }


    //}

    public DataTable GetPU_MccDMSDMChange()
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            //SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;




            objData.ExecuteSP("dbo.spBEGetPU_MCCDMSDMChange", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetMCC_MccDMSDMChange(string pu, string qtr, string year)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;




            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@pu";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = pu;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtQtr";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = qtr;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtYear";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = year;
            objParamColl.Add(sqlparam3);

            objData.ExecuteSP("dbo.spGetMCC_MCCDMSDMChange", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }


    public void GetCurrentField(string type, string dmsdm, string pu, string mcc, string qtr, string year, out string current)
    {

        current = "";

        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7;

        objCommand = new SqlCommand();
        objParamColl = objCommand.Parameters;




        sqlparam1 = new SqlParameter();
        sqlparam1.ParameterName = "@type";
        sqlparam1.Direction = ParameterDirection.Input;
        sqlparam1.SqlDbType = SqlDbType.VarChar;
        sqlparam1.Value = type;
        objParamColl.Add(sqlparam1);

        sqlparam2 = new SqlParameter();
        sqlparam2.ParameterName = "@dmsdm";
        sqlparam2.Direction = ParameterDirection.Input;
        sqlparam2.SqlDbType = SqlDbType.VarChar;
        sqlparam2.Value = dmsdm;
        objParamColl.Add(sqlparam2);


        sqlparam3 = new SqlParameter();
        sqlparam3.ParameterName = "@pu";
        sqlparam3.Direction = ParameterDirection.Input;
        sqlparam3.SqlDbType = SqlDbType.VarChar;
        sqlparam3.Value = pu;
        objParamColl.Add(sqlparam3);


        sqlparam4 = new SqlParameter();
        sqlparam4.ParameterName = "@txtQtr";
        sqlparam4.Direction = ParameterDirection.Input;
        sqlparam4.SqlDbType = SqlDbType.VarChar;
        sqlparam4.Value = qtr;
        objParamColl.Add(sqlparam4);

        sqlparam5 = new SqlParameter();
        sqlparam5.ParameterName = "@mcc";
        sqlparam5.Direction = ParameterDirection.Input;
        sqlparam5.SqlDbType = SqlDbType.VarChar;
        sqlparam5.Value = mcc;
        objParamColl.Add(sqlparam5);

        sqlparam6 = new SqlParameter();
        sqlparam6.ParameterName = "@txtYear";
        sqlparam6.Direction = ParameterDirection.Input;
        sqlparam6.SqlDbType = SqlDbType.VarChar;
        sqlparam6.Value = year;
        objParamColl.Add(sqlparam6);

        sqlparam7 = new SqlParameter();
        sqlparam7.ParameterName = "@current";
        sqlparam7.Direction = ParameterDirection.Output;
        sqlparam7.SqlDbType = SqlDbType.VarChar;
        sqlparam7.Size = 100;
        objParamColl.Add(sqlparam7);


        objData = new DataAccess();
        objData.GetConnection();
        objData.ExecuteSP("dbo.spBeGetFields_MCCdmsdmchange", objCommand);

        string output = objParamColl["@current"].Value.ToString();

        current = output;

    }


    public int UpdateMccdmsdmChange(string type, string dmsdm, string pu, string mcc, string qtr, string year, string change, DateTime weeklydate, DateTime dailydate)
    {



        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8, sqlparam9, sqlparam10;

        objCommand = new SqlCommand();
        objParamColl = objCommand.Parameters;




        sqlparam1 = new SqlParameter();
        sqlparam1.ParameterName = "@type";
        sqlparam1.Direction = ParameterDirection.Input;
        sqlparam1.SqlDbType = SqlDbType.VarChar;
        sqlparam1.Value = type;
        objParamColl.Add(sqlparam1);

        sqlparam2 = new SqlParameter();
        sqlparam2.ParameterName = "@dmsdm";
        sqlparam2.Direction = ParameterDirection.Input;
        sqlparam2.SqlDbType = SqlDbType.VarChar;
        sqlparam2.Value = dmsdm;
        objParamColl.Add(sqlparam2);


        sqlparam3 = new SqlParameter();
        sqlparam3.ParameterName = "@pu";
        sqlparam3.Direction = ParameterDirection.Input;
        sqlparam3.SqlDbType = SqlDbType.VarChar;
        sqlparam3.Value = pu;
        objParamColl.Add(sqlparam3);


        sqlparam4 = new SqlParameter();
        sqlparam4.ParameterName = "@txtQtr";
        sqlparam4.Direction = ParameterDirection.Input;
        sqlparam4.SqlDbType = SqlDbType.VarChar;
        sqlparam4.Value = qtr;
        objParamColl.Add(sqlparam4);

        sqlparam5 = new SqlParameter();
        sqlparam5.ParameterName = "@mcc";
        sqlparam5.Direction = ParameterDirection.Input;
        sqlparam5.SqlDbType = SqlDbType.VarChar;
        sqlparam5.Value = mcc;
        objParamColl.Add(sqlparam5);

        sqlparam6 = new SqlParameter();
        sqlparam6.ParameterName = "@txtYear";
        sqlparam6.Direction = ParameterDirection.Input;
        sqlparam6.SqlDbType = SqlDbType.VarChar;
        sqlparam6.Value = year;
        objParamColl.Add(sqlparam6);

        sqlparam7 = new SqlParameter();
        sqlparam7.ParameterName = "@change";
        sqlparam7.Direction = ParameterDirection.Input;
        sqlparam7.SqlDbType = SqlDbType.VarChar;
        sqlparam7.Value = change;
        objParamColl.Add(sqlparam7);



        sqlparam9 = new SqlParameter();
        sqlparam9.ParameterName = "@dtweeklydate";
        sqlparam9.Direction = ParameterDirection.Input;
        sqlparam9.SqlDbType = SqlDbType.DateTime;
        sqlparam9.Value = weeklydate;
        objParamColl.Add(sqlparam9);


        sqlparam10 = new SqlParameter();
        sqlparam10.ParameterName = "@dtdailydate";
        sqlparam10.Direction = ParameterDirection.Input;
        sqlparam10.SqlDbType = SqlDbType.DateTime;
        sqlparam10.Value = dailydate;
        objParamColl.Add(sqlparam10);


        sqlparam8 = new SqlParameter();
        sqlparam8.ParameterName = "@ret";
        sqlparam8.Direction = ParameterDirection.ReturnValue;
        sqlparam8.SqlDbType = SqlDbType.Int;
        objParamColl.Add(sqlparam8);

        objData = new DataAccess();
        objData.GetConnection();
        objData.ExecuteSP("dbo.spMCCDMSDMChange", objCommand);


        var ret = Convert.ToInt32(sqlparam8.Value);
        return ret;




    }

    public List<BEMonthlyFreeze> GetMonthlyFreezeDetails()
    {
        DataSet ds = new DataSet();
        List<BEMonthlyFreeze> monthlyFreeze = new List<BEMonthlyFreeze>();


        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEGetFreezeDetails", ref ds);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    monthlyFreeze.Add(new BEMonthlyFreeze()
                    {
                        Year = ds.Tables[0].Rows[i]["txtFinyear"].ToString().Trim(),
                        Quarter = ds.Tables[0].Rows[i]["txtQuarterName"].ToString().Trim(),
                        Month1 = ds.Tables[0].Rows[i]["isMon1freeze"].ToString().ToLowerTrim() == "y" ? true : false,
                        Month2 = ds.Tables[0].Rows[i]["isMon2freeze"].ToString().ToLowerTrim() == "y" ? true : false,
                        Month3 = ds.Tables[0].Rows[i]["isMon3freeze"].ToString().ToLowerTrim() == "y" ? true : false
                    });

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;

        }
        finally
        {
            objData.CloseConnection();
        }


        return monthlyFreeze;
    }

    public void UpdateMonthlyFreezedetails(BEMonthlyFreeze be)
    {
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtFinyear";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = be.Year;

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtQuartername";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = be.Quarter;

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@isMon1";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = be.Month1 == true ? "Y" : "N";

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@isMon2";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = be.Month2 == true ? "Y" : "N";

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@isMon3";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = be.Month3 == true ? "Y" : "N";

            objParamColl.Add(sqlparam1);
            objParamColl.Add(sqlparam2);
            objParamColl.Add(sqlparam3);
            objParamColl.Add(sqlparam4);
            objParamColl.Add(sqlparam5);

            objData.ExecuteSP("dbo.spBESaveMonthFreeze", ref  ds, objCommand);


        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }


    public DataTable GetNewBusinessSummary(string qtr, string year, string pu, string dh, string userid, string type, string weekdate)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtdh";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = dh;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);


            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@dtdate";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = weekdate;
            objParamColl.Add(sqlparam7);
            objData.ExecuteSP("dbo.spBENewBusinessSummary", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }


    public DataTable GetBESummary(string qtr, string year, string pu, string userid, string type, float range, string weekdate)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);



            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);


            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@range";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.Float;
            sqlparam7.Value = range;
            objParamColl.Add(sqlparam7);

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@dtdate";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.SqlDbType = SqlDbType.VarChar;
            sqlparam8.Value = weekdate;
            objParamColl.Add(sqlparam8);

            objData.ExecuteSP("dbo.spBeSummary", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }


    public DataTable DownloadCCP(string userid)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtuserid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = userid;
            objParamColl.Add(sqlparam1);


            objData.ExecuteSP("dbo.EAS_SP_BEDemClientCodePortfolioDownload", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    //code for download user details

    public DataSet DownloadUserDetails(string userid)
    {

        DataSet dsUserDetails = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {





            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtuserid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = userid;
            objParamColl.Add(sqlparam1);


            objData.ExecuteSP("dbo.SP_UserDetails", ref  dsUserDetails, objCommand);

            return dsUserDetails;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    //code for download users ends

    public List<string> GetWeeklyDates()
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;

        string tmp = string.Empty;
        List<string> lstdate = new List<string>();
        DataTable dt = new DataTable();
        try
        {


            SqlParameterCollection objParamColl;


            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;





            objData = new DataAccess();
            objData.GetConnection();
            // objData.ExecuteSP("spBEDUListForDropDown", ref ds, objCommand);
            objData.ExecuteSP("EAS_SP_BeGetWeeklyFrozendates", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    tmp = Convert.ToDateTime(ds.Tables[0].Rows[i]["dtdate"]).ToString("dd-MMM-yyyy");
                    lstdate.Add(tmp);


                }

            }

            return lstdate;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        return lstdate;

    }



    public int WeeklyDatesDelUpdate(string dtdeldate, string dtupdate, string type, string qtr, string yr)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@dltdate";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = dtdeldate;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@upddate";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = dtupdate;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@type";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = type;
            objParamColl.Add(sqlparam3);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@txtQtr";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = qtr;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@txtyear";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = yr;
            objParamColl.Add(sqlparam6);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@ret";
            sqlparam4.Direction = ParameterDirection.ReturnValue;
            sqlparam4.SqlDbType = SqlDbType.Int;
            objParamColl.Add(sqlparam4);

            objData.ExecuteSP("dbo.EAS_SP_DeleteFromWeeklyTables", ref  dsCurrConv, objCommand);



            var ret = Convert.ToInt32(sqlparam4.Value);

            return ret;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public List<string> GetWeeklyDateSum(string year, string qtr)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        List<string> lstempCollection = new List<string>();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtyear";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = year;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtQtr";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = qtr;
            objParamColl.Add(sqlparam2);

            objData.ExecuteSP("dbo.EAS_SP_BeGetDateandSum", ref  dsCurrConv, objCommand);


            if (dsCurrConv != null && dsCurrConv.Tables != null && dsCurrConv.Tables.Count > 0)
            {
                for (int i = 0; i < dsCurrConv.Tables[0].Rows.Count; i++)
                {
                    string DMName = string.Empty;

                    DMName = dsCurrConv.Tables[0].Rows[i]["txtDateSum"].ToString();
                    lstempCollection.Add(DMName);
                }

            }






        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection;

    }

    public DataTable GetQuarterYearWeekly(string type, string qtr)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@type";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = type;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@qtr";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = qtr;
            objParamColl.Add(sqlparam2);

            objData.ExecuteSP("dbo.EAS_SP_BeGetQtrYearWeekly", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

    }

    public string GetDelegatedExistenceInUserAccess(string userID)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;


        try
        {


            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@txtUserID";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userID;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;



            objParamColl.Add(objParamUserId);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEDelegateRoleinUserAccess", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                return ds.Tables[0].Rows[0]["Existence"] + "";
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return "";
    }

    //26/3

    public string GetUserdelegate(string userid)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        string customerCode = string.Empty;
        try
        {


            objParm = new SqlParameter();
            objParm.ParameterName = "@txtuserid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = userid;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEchkDelegationLogin", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {



                    customerCode = ds.Tables[0].Rows[i]["txtToUser"].ToString();


                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        return customerCode;
    }

    public string GetUserPU(string userID)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;


        try
        {


            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@txtuserid";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userID;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;



            objParamColl.Add(objParamUserId);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEGetPULogin", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                return ds.Tables[0].Rows[0]["txtPU"] + "";




            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return "";
    }

    public string GetUserFromdelegate(string userid)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        string customerCode = string.Empty;
        try
        {


            objParm = new SqlParameter();
            objParm.ParameterName = "@txtuserid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = userid;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEchkfromDelegate", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {



                    customerCode = ds.Tables[0].Rows[i]["txtFromUser"].ToString();


                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

        return customerCode;
    }

    public List<string> GetCustomerCodeDropDownNSO(string userid)
    {

        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            List<string> lstCustomerCode = new List<string>();

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@userid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = userid;
            objParamColl.Add(sqlparam1);



            DataSet ds = new DataSet();

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("eas_spGetCustomerCodeForBEtype_NSO", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    string tmp = ds.Tables[0].Rows[i]["txtMasterClientCOde"].ToString();

                    lstCustomerCode.Add(tmp);
                }

            }

            return lstCustomerCode;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public List<string> GetCustomerCodeDropDown(string userid, string nso)
    {

        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            List<string> lstCustomerCode = new List<string>();

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@userid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = userid;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@nso";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = nso;
            objParamColl.Add(sqlparam2);

            DataSet ds = new DataSet();

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("eas_spGetCustomerCodeForBEtype_NSO", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    string tmp = ds.Tables[0].Rows[i][0].ToString();

                    lstCustomerCode.Add(tmp);
                }

            }

            return lstCustomerCode;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public List<string> GetCustomerCodeDropDown_DH(string userid, string pu)
    {

        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            List<string> lstCustomerCode = new List<string>();

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@userid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = userid;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@pu";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = pu;
            objParamColl.Add(sqlparam2);

            DataSet ds = new DataSet();

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("eas_spGetCustomerCodeForBEtype", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    string tmp = ds.Tables[0].Rows[i]["txtmcc"].ToString();

                    lstCustomerCode.Add(tmp);
                }

            }

            return lstCustomerCode;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public List<string> GetCustomerCodeForBEtype(string Betype, string PU, string userID)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;

        List<string> lstempCollection = new List<string>();

        try
        {

            objCommand = new SqlCommand();


            SqlParameter objBE = new SqlParameter();
            objBE.ParameterName = "@BEType";
            objBE.Direction = ParameterDirection.Input;
            objBE.SqlDbType = SqlDbType.VarChar;
            objBE.Value = Betype;

            SqlParameter objBE1 = new SqlParameter();
            objBE1.ParameterName = "@pu";
            objBE1.Direction = ParameterDirection.Input;
            objBE1.SqlDbType = SqlDbType.VarChar;
            objBE1.Value = PU;

            SqlParameter objBE2 = new SqlParameter();
            objBE2.ParameterName = "@userid";
            objBE2.Direction = ParameterDirection.Input;
            objBE2.SqlDbType = SqlDbType.VarChar;
            objBE2.Value = userID;

            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objBE);
            objParamColl.Add(objBE1);
            objParamColl.Add(objBE2);

            objData = new DataAccess();
            objData.GetConnection();
            // objData.ExecuteSP("spBEDUListForDropDown", ref ds, objCommand);
            objData.ExecuteSP("eas_spGetCustomerCodeForBEtype", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    string tmp = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                    lstempCollection.Add(tmp);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }

    //public DataSet GetUpdateDMBEData(int id,Double M1, Double M2, Double M3, Double BK1, Double BK2, Double BK3, Double BK4,Double Volon1,Double Volon2,Double Volon3,Double Voloff1,Double Voloff2,Double Voloff3,string remarks)
    //{


    //    DataSet ds = new DataSet();
    //    DataSet dsreturn = new DataSet();
    //    SqlCommand objCommand;
    //    List<DataTable> retTable = new List<DataTable>();

    //    try
    //    {


    //        objCommand = new SqlCommand();
    //        SqlParameter objParamStatus1 = new SqlParameter();
    //        objParamStatus1.ParameterName = "@intBEId";
    //        objParamStatus1.Direction = ParameterDirection.Input;
    //        objParamStatus1.SqlDbType = SqlDbType.Int;
    //        objParamStatus1.Value = id;

    //        SqlParameter objParamStatus2 = new SqlParameter();
    //        objParamStatus2.ParameterName = "@fltMonth1BE";
    //        objParamStatus2.Direction = ParameterDirection.Input;
    //        objParamStatus2.SqlDbType = SqlDbType.Decimal;
    //        objParamStatus2.Value = M1;


    //        SqlParameter objParamStatus3 = new SqlParameter();
    //        objParamStatus3.ParameterName = "@fltMonth2BE";
    //        objParamStatus3.Direction = ParameterDirection.Input;
    //        objParamStatus3.SqlDbType = SqlDbType.Decimal;
    //        objParamStatus3.Value = M2;

    //        SqlParameter objParamStatus4 = new SqlParameter();
    //        objParamStatus4.ParameterName = "@fltMonth3BE";
    //        objParamStatus4.Direction = ParameterDirection.Input;
    //        objParamStatus4.SqlDbType = SqlDbType.Decimal;
    //        objParamStatus4.Value = M1;

    //        SqlParameter objParamStatus3 = new SqlParameter();
    //        objParamStatus3.ParameterName = "@Quarter";
    //        objParamStatus3.Direction = ParameterDirection.Input;
    //        objParamStatus3.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus3.Value = quarter;

    //        SqlParameter objParamStatus4 = new SqlParameter();
    //        objParamStatus4.ParameterName = "@FYYR";
    //        objParamStatus4.Direction = ParameterDirection.Input;
    //        objParamStatus4.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus4.Value = year;

    //        SqlParameter objParamStatus = new SqlParameter();
    //        objParamStatus.ParameterName = "@PU";
    //        objParamStatus.Direction = ParameterDirection.Input;
    //        objParamStatus.SqlDbType = SqlDbType.VarChar;
    //        objParamStatus.Value = PU;






    //        objCommand = new SqlCommand();
    //        SqlParameterCollection objParamColl = objCommand.Parameters;


    //        objParamColl.Add(objParamStatus);

    //        objParamColl.Add(objParamStatus1);
    //        objParamColl.Add(objParamStatus2);
    //        objParamColl.Add(objParamStatus3);

    //        objParamColl.Add(objParamStatus4);


    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        objData.ExecuteSP("EAS_SP_Fetch_BEData_DM", ref ds, objCommand);
    //        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
    //        {
    //            DataTable dt = new DataTable();
    //            dt = ds.Tables[0];



    //            dsreturn.Tables.Add(dt.Copy());


    //            return dsreturn;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }


    //    return new DataSet();
    //}


    public DataSet GetDMBEData(string NSO, string customerCode, string userid, string quarter, string year, string role)
    {


        DataSet ds = new DataSet();
        DataSet dsreturn = new DataSet();
        SqlCommand objCommand;
        List<DataTable> retTable = new List<DataTable>();

        try
        {


            objCommand = new SqlCommand();
            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@MasterClientCode";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = customerCode;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@UserId";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = userid;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@Quarter";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = quarter;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@FYYR";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = year;

            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.ParameterName = "@NewOffering";
            objParamStatus.Direction = ParameterDirection.Input;
            objParamStatus.SqlDbType = SqlDbType.VarChar;
            objParamStatus.Value = NSO;

            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@role";
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.VarChar;
            objParamStatus5.Value = role;






            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            objParamColl.Add(objParamStatus);

            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);

            objParamColl.Add(objParamStatus4);
            objParamColl.Add(objParamStatus5);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_Fetch_BEData_DM_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                //dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
                //dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
                //dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
                //dt.Columns["txtQuarterName"].ColumnName = "Current Quarter Name";
                //dt.Columns["txtYear"].ColumnName = "Year";

                ////BE
                //dt.Columns["DMfltMonth1BE"].ColumnName = "DMMonth1";
                //dt.Columns["DMfltMonth2BE"].ColumnName = "DMMonth2";
                //dt.Columns["DMfltMonth3BE"].ColumnName = "DMMonth3";
                //dt.Columns["DMfltCurrentQuarterBE"].ColumnName = "DMQCur";
                //dt.Columns["SDMBK1"].ColumnName = "SDMBK1";
                //dt.Columns["SDMBK2"].ColumnName = "SDMBK2";
                //dt.Columns["SDMBK3"].ColumnName = "SDMBK3";
                //dt.Columns["SDMBK4"].ColumnName = "SDMBK4";
                //dt.Columns["txtSDMBERemarks"].ColumnName = "SDMBERemarks";

                ////Vol

                //dt.Columns["txtOnsiteValueM1"].ColumnName = "OnsiteValueM1";
                //dt.Columns["txtOffshoreValueM1"].ColumnName = "OffshoreValueM1";
                //dt.Columns["txtOnsiteValueM2"].ColumnName = "OnsiteValueM2";
                //dt.Columns["txtOffshoreValueM2"].ColumnName = "OffshoreValueM2";
                //dt.Columns["txtOnsiteValueM3"].ColumnName = "OnsiteValueM3";
                //dt.Columns["txtOffshoreValueM3"].ColumnName = "OffshoreValueM3";

                //dt.Columns["txtTotalOnsiteValue"].ColumnName = "TotalOnsiteValue";
                //dt.Columns["txtTotalOffshoreValue"].ColumnName = "TotalOffshoreValue";
                //dt.Columns["txtGrandTotalValue"].ColumnName = "GrandTotalValue";
                //dt.Columns["txtSDMVolumeRemarks"].ColumnName = "SDMVolumeRemarks";

                //dt.Columns["dtDMUpdatedDate"].ColumnName = "dtDMUpdatedDate";



                dsreturn.Tables.Add(dt.Copy());
                //dsreturn.Tables.Add(ds.Tables[1].Copy());

                return dsreturn;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataSet();
    }

    public DataSet GetDMBEDataNSO(string customerCode, string userid, string quarter, string year, string role, string NewServiceOffering)
    {


        DataSet ds = new DataSet();
        DataSet dsreturn = new DataSet();
        SqlCommand objCommand;
        List<DataTable> retTable = new List<DataTable>();

        try
        {


            objCommand = new SqlCommand();
            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@MasterClientCode";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = customerCode;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@UserId";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = userid;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@Quarter";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = quarter;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@FYYR";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = year;

            //SqlParameter objParamStatus = new SqlParameter();
            //objParamStatus.ParameterName = "@PU";
            //objParamStatus.Direction = ParameterDirection.Input;
            //objParamStatus.SqlDbType = SqlDbType.VarChar;
            //objParamStatus.Value = PU;

            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@role";
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.VarChar;
            objParamStatus5.Value = role;

            SqlParameter objParamStatus6 = new SqlParameter();
            objParamStatus6.ParameterName = "@NewOffering";
            objParamStatus6.Direction = ParameterDirection.Input;
            objParamStatus6.SqlDbType = SqlDbType.VarChar;
            objParamStatus6.Value = NewServiceOffering;






            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            //objParamColl.Add(objParamStatus);

            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);

            objParamColl.Add(objParamStatus4);
            objParamColl.Add(objParamStatus5);
            objParamColl.Add(objParamStatus6);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_Fetch_BEData_DM_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                //dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
                //dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
                //dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
                //dt.Columns["txtQuarterName"].ColumnName = "Current Quarter Name";
                //dt.Columns["txtYear"].ColumnName = "Year";

                ////BE
                //dt.Columns["DMfltMonth1BE"].ColumnName = "DMMonth1";
                //dt.Columns["DMfltMonth2BE"].ColumnName = "DMMonth2";
                //dt.Columns["DMfltMonth3BE"].ColumnName = "DMMonth3";
                //dt.Columns["DMfltCurrentQuarterBE"].ColumnName = "DMQCur";
                //dt.Columns["SDMBK1"].ColumnName = "SDMBK1";
                //dt.Columns["SDMBK2"].ColumnName = "SDMBK2";
                //dt.Columns["SDMBK3"].ColumnName = "SDMBK3";
                //dt.Columns["SDMBK4"].ColumnName = "SDMBK4";
                //dt.Columns["txtSDMBERemarks"].ColumnName = "SDMBERemarks";

                ////Vol

                //dt.Columns["txtOnsiteValueM1"].ColumnName = "OnsiteValueM1";
                //dt.Columns["txtOffshoreValueM1"].ColumnName = "OffshoreValueM1";
                //dt.Columns["txtOnsiteValueM2"].ColumnName = "OnsiteValueM2";
                //dt.Columns["txtOffshoreValueM2"].ColumnName = "OffshoreValueM2";
                //dt.Columns["txtOnsiteValueM3"].ColumnName = "OnsiteValueM3";
                //dt.Columns["txtOffshoreValueM3"].ColumnName = "OffshoreValueM3";

                //dt.Columns["txtTotalOnsiteValue"].ColumnName = "TotalOnsiteValue";
                //dt.Columns["txtTotalOffshoreValue"].ColumnName = "TotalOffshoreValue";
                //dt.Columns["txtGrandTotalValue"].ColumnName = "GrandTotalValue";
                //dt.Columns["txtSDMVolumeRemarks"].ColumnName = "SDMVolumeRemarks";

                //dt.Columns["dtDMUpdatedDate"].ColumnName = "dtDMUpdatedDate";



                dsreturn.Tables.Add(dt.Copy());
                //dsreturn.Tables.Add(ds.Tables[1].Copy());

                return dsreturn;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataSet();
    }
    public DataSet GetDMBEDataExcel(string NSO, string customerCode, string userid, string quarter, string year, string role)
    {


        DataSet ds = new DataSet();
        DataSet dsreturn = new DataSet();
        SqlCommand objCommand;
        List<DataTable> retTable = new List<DataTable>();

        try
        {


            objCommand = new SqlCommand();
            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@MasterClientCode";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = customerCode;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@UserId";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = userid;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@Quarter";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = quarter;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@FYYR";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = year;

            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.ParameterName = "@NewOffering";
            objParamStatus.Direction = ParameterDirection.Input;
            objParamStatus.SqlDbType = SqlDbType.VarChar;
            objParamStatus.Value = NSO;

            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@Role";
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.VarChar;
            objParamStatus5.Value = role;




            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            objParamColl.Add(objParamStatus);

            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);

            objParamColl.Add(objParamStatus4);
            objParamColl.Add(objParamStatus5);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_Fetch_BEData_DM_ImportExcel_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                //dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
                //dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
                //dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
                //dt.Columns["txtQuarterName"].ColumnName = "Current Quarter Name";
                //dt.Columns["txtYear"].ColumnName = "Year";

                ////BE
                //dt.Columns["DMfltMonth1BE"].ColumnName = "DMMonth1";
                //dt.Columns["DMfltMonth2BE"].ColumnName = "DMMonth2";
                //dt.Columns["DMfltMonth3BE"].ColumnName = "DMMonth3";
                //dt.Columns["DMfltCurrentQuarterBE"].ColumnName = "DMQCur";
                //dt.Columns["SDMBK1"].ColumnName = "SDMBK1";
                //dt.Columns["SDMBK2"].ColumnName = "SDMBK2";
                //dt.Columns["SDMBK3"].ColumnName = "SDMBK3";
                //dt.Columns["SDMBK4"].ColumnName = "SDMBK4";
                //dt.Columns["txtSDMBERemarks"].ColumnName = "SDMBERemarks";

                ////Vol

                //dt.Columns["txtOnsiteValueM1"].ColumnName = "OnsiteValueM1";
                //dt.Columns["txtOffshoreValueM1"].ColumnName = "OffshoreValueM1";
                //dt.Columns["txtOnsiteValueM2"].ColumnName = "OnsiteValueM2";
                //dt.Columns["txtOffshoreValueM2"].ColumnName = "OffshoreValueM2";
                //dt.Columns["txtOnsiteValueM3"].ColumnName = "OnsiteValueM3";
                //dt.Columns["txtOffshoreValueM3"].ColumnName = "OffshoreValueM3";

                //dt.Columns["txtTotalOnsiteValue"].ColumnName = "TotalOnsiteValue";
                //dt.Columns["txtTotalOffshoreValue"].ColumnName = "TotalOffshoreValue";
                //dt.Columns["txtGrandTotalValue"].ColumnName = "GrandTotalValue";
                //dt.Columns["txtSDMVolumeRemarks"].ColumnName = "SDMVolumeRemarks";

                //dt.Columns["dtDMUpdatedDate"].ColumnName = "dtDMUpdatedDate";



                dsreturn.Tables.Add(dt.Copy());
                //dsreturn.Tables.Add(ds.Tables[1].Copy());

                return dsreturn;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataSet();
    }

    public DataSet GetDMBEDataExcelNSO(string customerCode, string userid, string quarter, string year, string role)
    {


        DataSet ds = new DataSet();
        DataSet dsreturn = new DataSet();
        SqlCommand objCommand;
        List<DataTable> retTable = new List<DataTable>();

        try
        {


            objCommand = new SqlCommand();
            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@MasterClientCode";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = customerCode;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@UserId";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = userid;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@Quarter";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = quarter;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@FYYR";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = year;

            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@Role";
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.VarChar;
            objParamStatus5.Value = role;

            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;

            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);

            objParamColl.Add(objParamStatus4);
            objParamColl.Add(objParamStatus5);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_Fetch_BEData_DM_ImportExcel_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                dsreturn.Tables.Add(dt.Copy());
                return dsreturn;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataSet();
    }

    public DataSet Get_NewServiceOffering(string UserId)
    {


        DataSet ds = new DataSet();
        DataSet dsreturn = new DataSet();

        SqlCommand objCommand;



        try
        {

            objCommand = new SqlCommand();
            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.ParameterName = "@userid";
            objParamStatus.Direction = ParameterDirection.Input;
            objParamStatus.SqlDbType = SqlDbType.VarChar;
            objParamStatus.Value = UserId;



            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            objParamColl.Add(objParamStatus);



            DataTable dttemp1 = null;

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("eas_spGet_NSO", ref ds, objCommand);

            return ds;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataSet();
    }

    public DataSet GetDHBEData(string PU, string customerCode, string dm, string quarter, string year, string currency, string type)
    {


        DataSet ds = new DataSet();
        DataSet dsreturn = new DataSet();

        SqlCommand objCommand;



        try
        {

            objCommand = new SqlCommand();
            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.ParameterName = "@txtCustomerCode";
            objParamStatus.Direction = ParameterDirection.Input;
            objParamStatus.SqlDbType = SqlDbType.VarChar;
            objParamStatus.Value = customerCode;

            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@txtUserId";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = dm;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@txtQuarterName";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = quarter;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@txtYear";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = year;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@PU";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = PU;

            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@txtCurrency";
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.VarChar;
            objParamStatus5.Value = currency;

            SqlParameter objParamStatus6 = new SqlParameter();

            objParamStatus6.ParameterName = "@type";

            objParamStatus6.Direction = ParameterDirection.Input;

            objParamStatus6.SqlDbType = SqlDbType.VarChar;

            objParamStatus6.Value = type;


            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            objParamColl.Add(objParamStatus);

            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);
            objParamColl.Add(objParamStatus4);
            objParamColl.Add(objParamStatus5);
            objParamColl.Add(objParamStatus6);
            DataTable dttemp1 = null;

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEFetchRevDataDH_superset", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];


                dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
                //dt.Columns["txtDU"].ColumnName = "DU";
                dt.Columns["txtDHMailId"].ColumnName = "DH";
                dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
                dt.Columns["fltDHBE"].ColumnName = "DHBE";
                dt.Columns["fltInpipeDHBE"].ColumnName = "InpipeDHBE";
                dt.Columns["fltDMBE"].ColumnName = "DMBE";
                dt.Columns["fltSDMBE"].ColumnName = "SDMBE";
                dt.Columns["fltInpipeMCOBE"].ColumnName = "MCOBE";

                dt.Columns["fltPrevRTBR"].ColumnName = "PrevRTBR";
                dt.Columns["fltPrev2RTBR"].ColumnName = "Prev2RTBR";
                //dt.Columns["txtDHMailid"].ColumnName = "DHMailId";
                dt.Columns["txtPU"].ColumnName = "PU";
                //dt.Columns["txtQuarterName"].ColumnName = "Current Quarter Name";
                //dt.Columns["txtYear"].ColumnName = "Year";
                //dt.Columns["DMfltNextQuarterBE"].ColumnName = "DMQNext"; //TODO
                // dt.Columns["fltPrevQtrBE"].ColumnName = "DMQPrev";
                //TODO:SDM section removed
                //dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
                //dt.Columns["SDMfltMonth1BE"].ColumnName = "SDMMonth1";
                //dt.Columns["SDMfltMonth2BE"].ColumnName = "SDMMonth2";
                //dt.Columns["SDMfltMonth3BE"].ColumnName = "SDMMonth3";
                //dt.Columns["SDMfltCurrentQuarterBE"].ColumnName = "SDMQCur";
                //dt.Columns["SDMfltNextQuarterBE"].ColumnName = "SDMQNext"; //TODO

                //dt.Columns[""].ColumnName = "SDMQPrev";
                dt.Columns["txtDHUpdatedBy"].ColumnName = "DHUpdatedBy";

                //dt.Columns["FinRTBRM1"].ColumnName = "ActualM1";
                //dt.Columns["FinRTBRM2"].ColumnName = "ActualM2";
                //dt.Columns["FinRTBRM3"].ColumnName = "ActualM3";
                dt.Columns["dtDHUpdatedDate"].ColumnName = "DHUpdatedDate";

                //dt.Columns["dtSDMUpdatedDate"].ColumnName = "SDMLastModifiedOn";
                //dt.Columns["dtLastUpdatedDate"].ColumnName = "SDMLastModifiedOn";

                // dt.Columns["dtLastUpdatedDate"].ColumnName = "SDMLastModifiedOn";


                dt.Columns["intBEId"].ColumnName = "BEID";
                //dt.Columns["txtRemarks"].ColumnName = "Remarks";
                //dt.Columns["FinRTBRTotal"].ColumnName = "totalRTBR";
                // dt.Columns["txtSDMRemarks"].ColumnName = "SDMRemarks";

                dt.Columns["fltRTBR"].ColumnName = "RTBR";
                dttemp1 = dt;
                dsreturn.Tables.Add(dttemp1.Copy());

                //DataTable dt1 = new DataTable();
                //dt1 = ds.Tables[1];
                //dsreturn.Tables.Add(dt1.Copy());
                return dsreturn;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataSet();
    }
    public void UpdateSDMBEDAtaDeleteIT(int beid, double sdmmonht1, double sdmmonth2, double sdmmonth3, string sdmrem, string sdm)
    {

        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@Beid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.Value = beid;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@sdmMnth1";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.Value = sdmmonht1;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@sdmMnth2";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.Value = sdmmonth2;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@sdmMnth3";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.Value = sdmmonth3;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@sdMrem";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.Value = sdmrem;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@SDM";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.Value = sdm;
            objParamColl.Add(sqlparam6);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEUploadSDMBEData1", objCommand);
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }



    }

    public void UpdateBEVolumeSDM(int beid, double SDMEffortMonth1OffShore, double SDMEffortMonth1Onsite, double SDMEffortMonth2OffShore, double SDMEffortMonth2Onsite,
     double SDMEffortMonth3OffShore, double SDMEffortMonth3Onsite, string Sdm, string SDMRem)
    {

        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8, sqlparam9;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@BEID";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.Value = beid;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@SDMEffortMonth1Onsite";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.Value = SDMEffortMonth1Onsite;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "SDMEffortMonth1OffShore";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.Value = SDMEffortMonth1OffShore;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@SDMEffortMonth2Onsite";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.Value = SDMEffortMonth2Onsite;
            objParamColl.Add(sqlparam4);


            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@SDMEffortMonth2Offshore";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.Value = SDMEffortMonth2OffShore;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@SDMEffortMonth3Onsite";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.Value = SDMEffortMonth3Onsite;
            objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@SDMEffortMonth3Offshore";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.Value = SDMEffortMonth3OffShore;
            objParamColl.Add(sqlparam7);

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@SDM";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.Value = Sdm;
            objParamColl.Add(sqlparam8);

            sqlparam9 = new SqlParameter();
            sqlparam9.ParameterName = "@SDMRemarks";
            sqlparam9.Direction = ParameterDirection.Input;
            sqlparam9.Value = SDMRem;
            objParamColl.Add(sqlparam9);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBESDMVolUploadBE", objCommand);
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }



    }
    public List<string> GetDatesForDropDown(string qtr, string year)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        string tmp = string.Empty;
        List<string> lstdate = new List<string>();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtQtr";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);
            objData.ExecuteSP("dbo.EAS_SP_GetDatesforDropDown", ref  ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    tmp = Convert.ToDateTime(ds.Tables[0].Rows[i]["dtdate"]).ToString("dd-MMM-yyyy");
                    lstdate.Add(tmp);


                }

            }

            return lstdate;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }
    public List<string> GetWeeklyDatesForInpipe()
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;

        string tmp = string.Empty;
        List<string> lstdate = new List<string>();
        DataTable dt = new DataTable();
        try
        {


            SqlParameterCollection objParamColl;


            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;





            objData = new DataAccess();
            objData.GetConnection();
            // objData.ExecuteSP("spBEDUListForDropDown", ref ds, objCommand);
            objData.ExecuteSP("spBeGetWeeklyFrozendates_Inpipe", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    tmp = Convert.ToDateTime(ds.Tables[0].Rows[i]["dtdate"]).ToString("dd-MMM-yyyy");
                    lstdate.Add(tmp);


                }

            }

            return lstdate;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }
    public DataTable GetDineshReportSum(string qtr, string year, string pu, string zero, string type, string weekdate)
    {
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtQtr";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtFinYear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);


            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@iszero";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = zero;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@type";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = type;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@dtdate";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = weekdate;
            objParamColl.Add(sqlparam6);

            objData.ExecuteSP("dbo.spBeDineshReportNew_Pivot", ref  ds, objCommand);

            return ds.Tables[0]; ;


        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }
    public DataTable GetDineshReport(string qtr, string year, string pu, string zero, string type, string weekdate)
    {
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtQtr";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtFinYear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);


            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@iszero";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = zero;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@type";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = type;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@dtdate";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = weekdate;
            objParamColl.Add(sqlparam6);

            objData.ExecuteSP("dbo.spBeDineshReportNew", ref  ds, objCommand);

            return ds.Tables[0]; ;


        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }
    public DataTable GetInpipeReport(string pu, int no, string type, string weekdate)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtPU";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = pu;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@intno";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.Int;
            sqlparam2.Value = no;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@type";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = type;
            objParamColl.Add(sqlparam3);



            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@dtdate";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = weekdate;
            objParamColl.Add(sqlparam4);
            objData.ExecuteSP("dbo.spBeINPIPENew", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }
    public DataTable GetBEReport(string qtr, string year, string pu, string dh, string userid, string type, DateTime date)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtdh";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = dh;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@datedd";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.DateTime;
            sqlparam7.Value = date;
            objParamColl.Add(sqlparam7);

            objData.ExecuteSP("dbo.spBE_Report_date", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetBEReport(string qtr, string year, string pu, string dh, string userid, string type, DateTime date, string OnOffYes)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtdh";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = dh;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@datedd";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.DateTime;
            sqlparam7.Value = date;
            objParamColl.Add(sqlparam7);

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@paramonoff";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.SqlDbType = SqlDbType.VarChar;
            sqlparam8.Value = OnOffYes;
            objParamColl.Add(sqlparam8);

            objData.ExecuteSP("dbo.spBE_Report_date_Vol_new1", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    //public DataTable GetBEReportforsplit(string qtr, string year, string pu, string dh, string userid, string type, DateTime date, string OnOffYes)
    //{

    //    DataSet dsCurrConv = new DataSet();
    //    SqlCommand sqlcmd = new SqlCommand();
    //    //SqlDataAdapter daCurrConv = new SqlDataAdapter();
    //    try
    //    {
    //        objData = new DataAccess();
    //        objData.GetConnection();
    //        SqlCommand objCommand;
    //        SqlParameterCollection objParamColl;

    //        SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8;

    //        objCommand = new SqlCommand();
    //        objParamColl = objCommand.Parameters;



    //        sqlparam1 = new SqlParameter();
    //        sqlparam1.ParameterName = "@txtCurQuarterName";
    //        sqlparam1.Direction = ParameterDirection.Input;
    //        sqlparam1.SqlDbType = SqlDbType.VarChar;
    //        sqlparam1.Value = qtr;
    //        objParamColl.Add(sqlparam1);

    //        sqlparam2 = new SqlParameter();
    //        sqlparam2.ParameterName = "@finyear";
    //        sqlparam2.Direction = ParameterDirection.Input;
    //        sqlparam2.SqlDbType = SqlDbType.VarChar;
    //        sqlparam2.Value = year;
    //        objParamColl.Add(sqlparam2);



    //        sqlparam3 = new SqlParameter();
    //        sqlparam3.ParameterName = "@txtPU";
    //        sqlparam3.Direction = ParameterDirection.Input;
    //        sqlparam3.SqlDbType = SqlDbType.VarChar;
    //        sqlparam3.Value = pu;
    //        objParamColl.Add(sqlparam3);

    //        sqlparam4 = new SqlParameter();
    //        sqlparam4.ParameterName = "@txtdh";
    //        sqlparam4.Direction = ParameterDirection.Input;
    //        sqlparam4.SqlDbType = SqlDbType.VarChar;
    //        sqlparam4.Value = dh;
    //        objParamColl.Add(sqlparam4);

    //        sqlparam5 = new SqlParameter();
    //        sqlparam5.ParameterName = "@userid";
    //        sqlparam5.Direction = ParameterDirection.Input;
    //        sqlparam5.SqlDbType = SqlDbType.VarChar;
    //        sqlparam5.Value = userid;
    //        objParamColl.Add(sqlparam5);

    //        sqlparam6 = new SqlParameter();
    //        sqlparam6.ParameterName = "@type";
    //        sqlparam6.Direction = ParameterDirection.Input;
    //        sqlparam6.SqlDbType = SqlDbType.VarChar;
    //        sqlparam6.Value = type;
    //        objParamColl.Add(sqlparam6);

    //        sqlparam7 = new SqlParameter();
    //        sqlparam7.ParameterName = "@datedd";
    //        sqlparam7.Direction = ParameterDirection.Input;
    //        sqlparam7.SqlDbType = SqlDbType.DateTime;
    //        sqlparam7.Value = date;
    //        objParamColl.Add(sqlparam7);

    //        sqlparam8 = new SqlParameter();
    //        sqlparam8.ParameterName = "@paramonoff";
    //        sqlparam8.Direction = ParameterDirection.Input;
    //        sqlparam8.SqlDbType = SqlDbType.VarChar;
    //        sqlparam8.Value = OnOffYes;
    //        objParamColl.Add(sqlparam8);

    //        objData.ExecuteSP("dbo.spBE_Report_date_Vol_newsplit", ref  dsCurrConv, objCommand);

    //        return dsCurrConv.Tables[0]; ;
    //    }
    //    catch (Exception ex)
    //    {

    //        logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
    //        throw;
    //    }
    //    finally
    //    {
    //        objData.CloseConnection();
    //    }


    //}

    public DataTable GetBEReportforsplit(string qtr, string year, string userid, string SU)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@Quarter";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@FYYR";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            //sqlparam3 = new SqlParameter();
            //sqlparam3.ParameterName = "@txtPU";
            //sqlparam3.Direction = ParameterDirection.Input;
            //sqlparam3.SqlDbType = SqlDbType.VarChar;
            //sqlparam3.Value = pu;
            //objParamColl.Add(sqlparam3);

            //sqlparam4 = new SqlParameter();
            //sqlparam4.ParameterName = "@txtdh";
            //sqlparam4.Direction = ParameterDirection.Input;
            //sqlparam4.SqlDbType = SqlDbType.VarChar;
            //sqlparam4.Value = dh;
            //objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@txtuserid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            //sqlparam6 = new SqlParameter();
            //sqlparam6.ParameterName = "@type";
            //sqlparam6.Direction = ParameterDirection.Input;
            //sqlparam6.SqlDbType = SqlDbType.VarChar;
            //sqlparam6.Value = type;
            //objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@SU";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = SU;
            objParamColl.Add(sqlparam7);

            //sqlparam8 = new SqlParameter();
            //sqlparam8.ParameterName = "@paramonoff";
            //sqlparam8.Direction = ParameterDirection.Input;
            //sqlparam8.SqlDbType = SqlDbType.VarChar;
            //sqlparam8.Value = OnOffYes;
            //objParamColl.Add(sqlparam8);

            //objData.ExecuteSP("dbo.spBE_Report_date_Vol_newsplit", ref  dsCurrConv, objCommand);
            objData.ExecuteSP("dbo.EAS_SP_BEreport_ForGLN", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetBEReportforDMsplit(string qtr, string year, string userid, string SU, string PU)
    {
        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@Quarter";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@FYYR";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@PU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = PU;
            objParamColl.Add(sqlparam3);

            //sqlparam4 = new SqlParameter();
            //sqlparam4.ParameterName = "@txtdh";
            //sqlparam4.Direction = ParameterDirection.Input;
            //sqlparam4.SqlDbType = SqlDbType.VarChar;
            //sqlparam4.Value = dh;
            //objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@txtuserid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            //sqlparam6 = new SqlParameter();
            //sqlparam6.ParameterName = "@type";
            //sqlparam6.Direction = ParameterDirection.Input;
            //sqlparam6.SqlDbType = SqlDbType.VarChar;
            //sqlparam6.Value = type;
            //objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@SU";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = SU;
            objParamColl.Add(sqlparam7);

            //sqlparam8 = new SqlParameter();
            //sqlparam8.ParameterName = "@paramonoff";
            //sqlparam8.Direction = ParameterDirection.Input;
            //sqlparam8.SqlDbType = SqlDbType.VarChar;
            //sqlparam8.Value = OnOffYes;
            //objParamColl.Add(sqlparam8);

            //objData.ExecuteSP("dbo.spBE_Report_date_Vol_newsplit", ref  dsCurrConv, objCommand);
            objData.ExecuteSP("dbo.EAS_SP_DMBEreport", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }




    public DataSet EAS_SP_BEReport_Sales()
    {
        //var date = date1.Date;
        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            objCommand = new SqlCommand();
            objData.ExecuteSP("dbo.EAS_SP_BEReport_Sales", ref  dsCurrConv, objCommand);

            return dsCurrConv;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public DataSet EAS_SP_BEReport_Sales_Current(string userid, string SU, string flag)
    {
        //var date = date1.Date;
        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;
            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8, sqlparam9, sqlparam10;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;


            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@txtuserid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@SU";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = SU;
            objParamColl.Add(sqlparam7);



            sqlparam10 = new SqlParameter();
            sqlparam10.ParameterName = "@flag";
            sqlparam10.Direction = ParameterDirection.Input;
            sqlparam10.SqlDbType = SqlDbType.VarChar;
            sqlparam10.Value = flag;
            objParamColl.Add(sqlparam10);

            objData.ExecuteSP("dbo.EAS_SP_BEReport_Sales_BE_Summary_Current", ref  dsCurrConv, objCommand);

            return dsCurrConv;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public DataSet EAS_SP_BEReport_Sales_Future(string userid, string SU, string flag)
    {
        //var date = date1.Date;
        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;
            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8, sqlparam9, sqlparam10;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;


            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@txtuserid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@SU";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = SU;
            objParamColl.Add(sqlparam7);



            sqlparam10 = new SqlParameter();
            sqlparam10.ParameterName = "@flag";
            sqlparam10.Direction = ParameterDirection.Input;
            sqlparam10.SqlDbType = SqlDbType.VarChar;
            sqlparam10.Value = flag;
            objParamColl.Add(sqlparam10);

            objData.ExecuteSP("dbo.EAS_SP_BEReport_Sales_BE_Summary_Future", ref  dsCurrConv, objCommand);

            return dsCurrConv;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public DataSet GetBEReportRevenueMomentum(string qtr, string year, string userid, string SU, string Type, string date, string fetchingType, string nso)
    {
        //var date = date1.Date;
        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;
            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8, sqlparam9, sqlparam10;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@Quarter";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@FYYR";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@txtuserid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@SU";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = SU;
            objParamColl.Add(sqlparam7);

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@Type";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.SqlDbType = SqlDbType.VarChar;
            sqlparam8.Value = Type;
            objParamColl.Add(sqlparam8);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@DtSelectedDate";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = date;
            objParamColl.Add(sqlparam3);

            sqlparam9 = new SqlParameter();
            sqlparam9.ParameterName = "@FetchingType";
            sqlparam9.Direction = ParameterDirection.Input;
            sqlparam9.SqlDbType = SqlDbType.VarChar;
            sqlparam9.Value = fetchingType;
            objParamColl.Add(sqlparam9);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@NSOcode";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = nso;
            objParamColl.Add(sqlparam4);

            if (SU.ToLower() == "ecas")
                objData.ExecuteSP("dbo.EAS_SP_BEReport_RevMom_working_NSO_ecas", ref  dsCurrConv, objCommand);
            else
                objData.ExecuteSP("dbo.EAS_SP_BEReport_RevMom_working_NSO_Manasa", ref  dsCurrConv, objCommand);

            return dsCurrConv;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public DataTable GetSDMVolComparisonReport(string qtr, string year, string dh, string pu, string userid, string type, string paramName, string paramvalue, DateTime date)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8, sqlparam9;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtdh";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = dh;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@typeofReport";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = paramName;
            objParamColl.Add(sqlparam7);

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@valuefortype";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.SqlDbType = SqlDbType.VarChar;
            sqlparam8.Value = paramvalue;
            objParamColl.Add(sqlparam8);

            sqlparam9 = new SqlParameter();
            sqlparam9.ParameterName = "@datedd";
            sqlparam9.Direction = ParameterDirection.Input;
            sqlparam9.SqlDbType = SqlDbType.DateTime;
            sqlparam9.Value = date;
            objParamColl.Add(sqlparam9);

            objData.ExecuteSP("dbo.spBEVolComparisionReport_date", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetDMVolComparisonReport(string qtr, string year, string dh, string pu, string userid, string type, string paramName, string paramvalue, DateTime date)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8, sqlparam9;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtdh";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = dh;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@typeofReport";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = paramName;
            objParamColl.Add(sqlparam7);

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@valuefortype";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.SqlDbType = SqlDbType.VarChar;
            sqlparam8.Value = paramvalue;
            objParamColl.Add(sqlparam8);

            sqlparam9 = new SqlParameter();
            sqlparam9.ParameterName = "@datedd";
            sqlparam9.Direction = ParameterDirection.Input;
            sqlparam9.SqlDbType = SqlDbType.DateTime;
            sqlparam9.Value = date;
            objParamColl.Add(sqlparam9);

            objData.ExecuteSP("dbo.spBEVolComparisionReportDM_date", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

    }

    public DataTable GetDMRevComparisonReport(string qtr, string year, string dh, string pu, string userid, string type, string paramName, string paramvalue, DateTime date)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8, sqlparam9;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtdh";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = dh;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@typeofReport";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = paramName;
            objParamColl.Add(sqlparam7);

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@valuefortype";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.SqlDbType = SqlDbType.VarChar;
            sqlparam8.Value = paramvalue;
            objParamColl.Add(sqlparam8);

            sqlparam9 = new SqlParameter();
            sqlparam9.ParameterName = "@datedd";
            sqlparam9.Direction = ParameterDirection.Input;
            sqlparam9.SqlDbType = SqlDbType.DateTime;
            sqlparam9.Value = date;
            objParamColl.Add(sqlparam9);

            objData.ExecuteSP("dbo.spBEComparisionReportDM_date", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetComparisonReport(string qtr, string year, string dh, string pu, string userid, string type, string paramName, string paramvalue, DateTime date)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8, sqlparam9;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtdh";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = dh;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@typeofReport";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = paramName;
            objParamColl.Add(sqlparam7);

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@valuefortype";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.SqlDbType = SqlDbType.VarChar;
            sqlparam8.Value = paramvalue;
            objParamColl.Add(sqlparam8);

            sqlparam9 = new SqlParameter();
            sqlparam9.ParameterName = "@datedd";
            sqlparam9.Direction = ParameterDirection.Input;
            sqlparam9.SqlDbType = SqlDbType.DateTime;
            sqlparam9.Value = date;
            objParamColl.Add(sqlparam9);

            objData.ExecuteSP("dbo.spBEComparisionReport_date", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public List<string> GetCopyDataFutureFinancialYear(string fromQuarter, int no)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<string> finYear = new List<string>();

        try
        {
            objCommand = new SqlCommand();
            SqlParameter objYear = new SqlParameter();
            objYear.ParameterName = "@txtYear";
            objYear.Direction = ParameterDirection.Input;
            objYear.SqlDbType = SqlDbType.VarChar;
            objYear.Value = fromQuarter;

            SqlParameter objNo = new SqlParameter();
            objNo.ParameterName = "@intNo";
            objNo.Direction = ParameterDirection.Input;
            objNo.SqlDbType = SqlDbType.VarChar;
            objNo.Value = no;


            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objYear);
            objParamColl.Add(objNo);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBECopyDataFutureFinancialYear", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string year = string.Empty;

                    year = ds.Tables[0].Rows[i]["txtFutureYear"].ToString();
                    finYear.Add(year);
                }

            }
        }
        catch (Exception ex)
        {
            //Logger.LogErrorToServer(App_Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return finYear;
    }


    //Copy Data- For fetchingfetching The quarters
    public List<string> GetCopyDataQuarter(string txtFinancialYear, int val)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;

        List<string> lstempCollection = new List<string>();
        try
        {

            objCommand = new SqlCommand();
            SqlParameter objPU = new SqlParameter();
            objPU.ParameterName = "@txtFinancialYear";
            objPU.Direction = ParameterDirection.Input;
            objPU.SqlDbType = SqlDbType.VarChar;
            objPU.Value = txtFinancialYear;

            objCommand = new SqlCommand();
            SqlParameter objVal = new SqlParameter();
            objVal.ParameterName = "@int";
            objVal.Direction = ParameterDirection.Input;
            objVal.SqlDbType = SqlDbType.Int;
            objVal.Value = val;

            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objPU);
            objParamColl.Add(objVal);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("[EAS_SP_BECopyDataQuarters]", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string quarter = string.Empty;

                    quarter = ds.Tables[0].Rows[i]["txtQuarter"].ToString();
                    lstempCollection.Add(quarter);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection;
    }

    //Copy Data- For Copying the data and getting the count of values that goe added
    public int GetCopyData(string txtFromQuarter, string txtFromFinancialYear, string txtToQuarter, string txtToFinancialYear)
    {
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtFromQuarter";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.Value = txtFromQuarter;

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtFromFinancialYear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.Value = txtFromFinancialYear;

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtToQuarter";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.Value = txtToQuarter;

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtToFinancialYear";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.Value = txtToFinancialYear;

            sqlparam5 = new SqlParameter();
            sqlparam5.Direction = ParameterDirection.ReturnValue;
            sqlparam5.ParameterName = "ReturnValue";

            objParamColl.Add(sqlparam1);
            objParamColl.Add(sqlparam2);
            objParamColl.Add(sqlparam3);
            objParamColl.Add(sqlparam4);
            objParamColl.Add(sqlparam5);

            objData.ExecuteSP("dbo.spBECopyData", ref  ds, objCommand);

            int count = (int)objCommand.Parameters["ReturnValue"].Value;
            return count;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }


    //Copy Data- For fetching the Financial year
    public List<string> GetCopyDataFinancialYear(int val)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<string> finYear = new List<string>();

        try
        {
            objCommand = new SqlCommand();
            SqlParameter objPU = new SqlParameter();
            objPU.ParameterName = "@int";
            objPU.Direction = ParameterDirection.Input;
            objPU.SqlDbType = SqlDbType.Int;
            objPU.Value = val;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objPU);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.EAS_SP_BECopyDataFinancialYear", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string year = string.Empty;

                    year = ds.Tables[0].Rows[i]["finYear"].ToString();
                    finYear.Add(year);
                }

            }
        }
        catch (Exception ex)
        {
            //Logger.LogErrorToServer(App_Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return finYear;
    }

    public List<string> GetAuditSUs()
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<string> su = new List<string>();

        try
        {
            objCommand = new SqlCommand();
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEFetchSU", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string id = string.Empty;

                    id = ds.Tables[0].Rows[i]["SU"].ToString();
                    su.Add(id);
                }

            }
        }
        catch (Exception ex)
        {
            //Logger.LogErrorToServer(App_Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return su;
    }

    public List<string> GetAuditCustomerCode(string txtPU)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;

        List<string> lstempCollection = new List<string>();
        try
        {

            objCommand = new SqlCommand();
            SqlParameter objPU = new SqlParameter();
            objPU.ParameterName = "@txtPU";
            objPU.Direction = ParameterDirection.Input;
            objPU.SqlDbType = SqlDbType.VarChar;
            objPU.Value = txtPU;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objPU);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEAuditFetchMCCs", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string MCC = string.Empty;

                    MCC = ds.Tables[0].Rows[i]["txtMCC"].ToString();
                    lstempCollection.Add(MCC);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection;
    }

    //Audit- For loading Action Type Drop Down List
    public List<string> GetActionType()
    {
        DataSet ds = new DataSet();
        //SqlParameter objParm;
        SqlCommand objCommand;
        //SqlParameterCollection objParamColl;
        List<string> lstActionType = new List<string>();

        try
        {
            objCommand = new SqlCommand();
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEAuditActionType", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string tmp = ds.Tables[0].Rows[i]["txtActionType"].ToString();
                    lstActionType.Add(tmp);
                }

            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstActionType;
    }

    //Audit- For loading BE Type Drop Down List
    public List<string> GetBEType()
    {
        DataSet ds = new DataSet();
        //SqlParameter objParm;
        SqlCommand objCommand;
        //SqlParameterCollection objParamColl;
        List<string> lstBEType = new List<string>();

        try
        {
            objCommand = new SqlCommand();
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEAuditBEType", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string tmp = ds.Tables[0].Rows[i]["txtBEType"].ToString();
                    lstBEType.Add(tmp);
                }

            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstBEType;
    }

    public List<AuditUI> FetchAuditDetails(string txtRevVol, string txtDmSdm, string txtPU, string txtMCC, string txtQuarter, string txtFinancialYear, string txtBeType, string txtActionType, DateTime startDate, DateTime endDate)
    {

        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<AuditUI> objList = new List<AuditUI>();
        AuditUI objAudit = null;

        objCommand = new SqlCommand();
        SqlParameter objRevVol = new SqlParameter();
        objRevVol.Direction = ParameterDirection.Input;
        objCommand.Parameters.Add(objRevVol);
        objRevVol.SqlDbType = SqlDbType.VarChar;
        objRevVol.ParameterName = "@txtRevVol";
        objRevVol.Value = txtRevVol;

        SqlParameter objDmSdm = new SqlParameter();
        objDmSdm.Direction = ParameterDirection.Input;
        objCommand.Parameters.Add(objDmSdm);
        objDmSdm.SqlDbType = SqlDbType.VarChar;
        objDmSdm.ParameterName = "@txtDmSdm";
        objDmSdm.Value = txtDmSdm;

        SqlParameter objPU = new SqlParameter();
        objPU.Direction = ParameterDirection.Input;
        objCommand.Parameters.Add(objPU);
        objPU.SqlDbType = SqlDbType.VarChar;
        objPU.Size = 8000;
        objPU.ParameterName = "@txtPU";
        objPU.Value = txtPU;

        SqlParameter objMCC = new SqlParameter();
        objMCC.SqlDbType = SqlDbType.VarChar;
        objMCC.Size = 8000;
        objCommand.Parameters.Add(objMCC);
        objMCC.ParameterName = "@txtMCC";
        objMCC.Value = txtMCC;

        SqlParameter objQuarter = new SqlParameter();
        objQuarter.Direction = ParameterDirection.Input;
        objCommand.Parameters.Add(objQuarter);
        objQuarter.SqlDbType = SqlDbType.VarChar;
        objQuarter.ParameterName = "@txtQuarter";
        objQuarter.Value = txtQuarter;

        SqlParameter objFinancialYear = new SqlParameter();
        objFinancialYear.Direction = ParameterDirection.Input;
        objCommand.Parameters.Add(objFinancialYear);
        objFinancialYear.SqlDbType = SqlDbType.VarChar;
        objFinancialYear.ParameterName = "@txtYear";
        objFinancialYear.Value = txtFinancialYear;

        SqlParameter objBEType = new SqlParameter();
        objBEType.Direction = ParameterDirection.Input;
        objCommand.Parameters.Add(objBEType);
        objBEType.SqlDbType = SqlDbType.VarChar;
        objBEType.ParameterName = "@txtBeType";
        objBEType.Value = txtBeType;

        SqlParameter objActionType = new SqlParameter();
        objActionType.Direction = ParameterDirection.Input;
        objCommand.Parameters.Add(objActionType);
        objActionType.SqlDbType = SqlDbType.VarChar;
        objActionType.ParameterName = "@txtActionType";
        objActionType.Value = txtActionType;

        SqlParameter objStartDate = new SqlParameter();
        objStartDate.Direction = ParameterDirection.Input;
        objCommand.Parameters.Add(objStartDate);
        objStartDate.SqlDbType = SqlDbType.DateTime;
        objStartDate.ParameterName = "@dtFromDate";
        objStartDate.Value = startDate;

        SqlParameter objEndDate = new SqlParameter();
        objEndDate.Direction = ParameterDirection.Input;
        objCommand.Parameters.Add(objEndDate);
        objEndDate.SqlDbType = SqlDbType.DateTime;
        objEndDate.ParameterName = "@dtToDate";
        objEndDate.Value = endDate;


        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEAuditFetchDetails", ref ds, objCommand);

            if (txtRevVol == "Revenue")
            {
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objAudit = new AuditUI();
                        objAudit.intHide = Convert.ToInt16(ds.Tables[0].Rows[i]["intHide"]);
                        objAudit.intBEId = Convert.ToInt16(ds.Tables[0].Rows[i]["intBEIdTable"]);
                        objAudit.txtMasterClientCode = ds.Tables[0].Rows[i]["txtMasterClientCodeTable"] + "";
                        objAudit.txtMasterClientName = ds.Tables[0].Rows[i]["txtMasterClientNameTable"] + "";
                        objAudit.txtPU = ds.Tables[0].Rows[i]["txtPUTable"] + "";
                        objAudit.txtDMMailId = ds.Tables[0].Rows[i]["txtDMMailIdTable"] + "";
                        objAudit.txtNativeCurrency = ds.Tables[0].Rows[i]["txtNativeCurrencyTable"] + "";
                        objAudit.DMfltMonth1BE = ds.Tables[0].Rows[i]["DMfltMonth1BETable"] + "";
                        objAudit.DMfltMonth2BE = ds.Tables[0].Rows[i]["DMfltMonth2BETable"] + "";
                        objAudit.DMfltMonth3BE = ds.Tables[0].Rows[i]["DMfltMonth3BETable"] + "";
                        objAudit.txtCurrentQuarterName = ds.Tables[0].Rows[i]["txtCurrentQuarterNameTable"] + "";
                        objAudit.txtYear = ds.Tables[0].Rows[i]["txtYearTable"] + "";
                        objAudit.txtCreatedBy = ds.Tables[0].Rows[i]["txtCreatedByTable"] + "";
                        objAudit.txtCreatedDate = ConvertToDateTimeDDMMYYYY(ds.Tables[0].Rows[i], "dtCreatedDateTable");
                        objAudit.txtDMUpdatedby = ds.Tables[0].Rows[i]["txtDMUpdatedByTable"] + "";
                        // objAudit.txtDMUpdatedDate = string.Format("{0:dd/MM/yyyy hh:mm:ss tt}", (ds.Tables[0].Rows[i]["dtDMUpdatedDateTable"] + ""));
                        objAudit.txtDMUpdatedDate = ConvertToDateTimeDDMMYYYY(ds.Tables[0].Rows[i], "dtDMUpdatedDateTable");
                        objAudit.txtRemarks = ds.Tables[0].Rows[i]["txtRemarksTable"] + "";
                        objAudit.txtBeType = ds.Tables[0].Rows[i]["txtBeTypeTable"] + "";
                        objAudit.dtDumpDate = ds.Tables[0].Rows[i]["dtDumpDateTable"] + "" == "" ? "" : Convert.ToDateTime(ds.Tables[0].Rows[i]["dtDumpDateTable"] + "").ToString("dd/MM/yyyy hh:mm:ss tt");
                        objAudit.txtActionType = ds.Tables[0].Rows[i]["txtActionTypeTable"] + "";
                        objAudit.txtIsChanged = ds.Tables[0].Rows[i]["txtIsChangedTable"] + "";
                        objList.Add(objAudit);
                    }

                }
            }
            else
            {
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objAudit = new AuditUI();
                        objAudit.intHide = Convert.ToInt16(ds.Tables[0].Rows[i]["intHide"]);
                        objAudit.intBEId = Convert.ToInt16(ds.Tables[0].Rows[i]["intBEIdTable"]);
                        objAudit.txtMasterClientCode = ds.Tables[0].Rows[i]["txtMasterClientCodeTable"] + "";
                        objAudit.txtMasterClientName = ds.Tables[0].Rows[i]["txtMasterClientNameTable"] + "";
                        objAudit.txtPU = ds.Tables[0].Rows[i]["txtPUTable"] + "";
                        objAudit.txtDMMailId = ds.Tables[0].Rows[i]["txtDMMailIdTable"] + "";
                        objAudit.txtCurrentQuarterName = ds.Tables[0].Rows[i]["txtCurrentQuarterNameTable"] + "";
                        objAudit.txtYear = ds.Tables[0].Rows[i]["txtYearTable"] + "";
                        objAudit.txtCreatedBy = ds.Tables[0].Rows[i]["txtCreatedByTable"] + "";
                        // objAudit.txtCreatedDate = string.Format("{0:dd/MM/yyyy hh:mm:ss tt}", (ds.Tables[0].Rows[i]["dtCreatedDateTable"] + ""));
                        objAudit.txtCreatedDate = ConvertToDateTimeDDMMYYYY(ds.Tables[0].Rows[i], "dtCreatedDateTable");
                        objAudit.txtDMUpdatedby = ds.Tables[0].Rows[i]["txtDMUpdatedByTable"] + "";
                        objAudit.txtDMUpdatedDate = ConvertToDateTimeDDMMYYYY(ds.Tables[0].Rows[i], "dtDMUpdatedDateTable");
                        objAudit.txtRemarks = ds.Tables[0].Rows[i]["txtRemarksTable"] + "";
                        objAudit.fltDMEffortMonth1Onsite = ds.Tables[0].Rows[i]["fltDMEffortMonth1OnsiteTable"] + "";
                        objAudit.fltDMEffortMonth1OffShore = ds.Tables[0].Rows[i]["fltDMEffortMonth1OffshoreTable"] + "";
                        objAudit.fltDMEffortMonth2Onsite = ds.Tables[0].Rows[i]["fltDMEffortMonth2OnsiteTable"] + "";
                        objAudit.fltDMEffortMonth2OffShore = ds.Tables[0].Rows[i]["fltDMEffortMonth2OffshoreTable"] + "";
                        objAudit.fltDMEffortMonth3Onsite = ds.Tables[0].Rows[i]["fltDMEffortMonth3OnsiteTable"] + "";
                        objAudit.fltDMEffortMonth3OffShore = ds.Tables[0].Rows[i]["fltDMEffortMonth3OffshoreTable"] + "";
                        objAudit.txtBeType = ds.Tables[0].Rows[i]["txtBeTypeTable"] + "";
                        //objAudit.dtDumpDate = ds.Tables[0].Rows[i]["dtDumpDateTable"] + "" == "" ? "" : Convert.ToDateTime(ds.Tables[0].Rows[i]["dtDumpDateTable"] + "").ToString("MM/dd/yyyy hh:mm:ss tt");
                        //objAudit.dtDumpDate = ds.Tables[0].Rows[i]["dtDumpDateTable"] + "" == "" ? datenull : Convert.ToDateTime(ds.Tables[0].Rows[i]["dtDumpDateTable"] + "");
                        objAudit.txtActionType = ds.Tables[0].Rows[i]["txtActionTypeTable"] + "";
                        objList.Add(objAudit);
                    }

                }
            }
        }

        catch (Exception ex)
        {
            // Logger.LogErrorToServer(App_Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        objData.CloseConnection();
        return objList;
    }

    public string ConvertToDateTimeDDMMYYYY(DataRow row, string colunName)
    {
        string returnValue = "";
        string value = (row[colunName] + "").Trim();
        DateTime dateTime;
        bool isValid = DateTime.TryParse(value, out dateTime);
        if (isValid)
            returnValue = dateTime.ToString("dd/MM/yyyy hh:mm:ss tt");
        return returnValue;

    }

    // Audit(Delete)- For fetching the financial year and quarter for the grid view 
    public List<AuditLogUI> GetFinYearQuarter()
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;

        List<AuditLogUI> objList = new List<AuditLogUI>();
        try
        {
            objCommand = new SqlCommand();
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEAuditFetchQuarters", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    AuditLogUI objAudit = new AuditLogUI();
                    //objAudit.id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString());
                    objAudit.quarter = ds.Tables[0].Rows[i]["quarter"].ToString();
                    objList.Add(objAudit);
                }

            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return objList;
    }

    //Audit(Delete)- For deleting the details
    public int DeleteAuditDetails(string txtPU, string txtMCC, AuditLogUI log, string userID)
    {
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8, sqlparam9;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtPU";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.Value = txtPU;

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtMCC";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.Value = txtMCC;

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtfinYear";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.Value = log.quarter;

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@isRevenueDM";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.Value = log.revenueDM == true ? "Y" : "N";

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@isRevenueSDM";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.Value = log.revenueSDM == true ? "Y" : "N";

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@isVolumeDM";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.Value = log.volumeDM == true ? "Y" : "N";

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@isVolumeSDM";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.Value = log.volumeSDM == true ? "Y" : "N";

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@txtUserId";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.Value = userID;

            sqlparam9 = new SqlParameter();
            sqlparam9.Direction = ParameterDirection.ReturnValue;
            sqlparam9.ParameterName = "ReturnValue";

            objParamColl.Add(sqlparam1);
            objParamColl.Add(sqlparam2);
            objParamColl.Add(sqlparam3);
            objParamColl.Add(sqlparam4);
            objParamColl.Add(sqlparam5);
            objParamColl.Add(sqlparam6);
            objParamColl.Add(sqlparam7);
            objParamColl.Add(sqlparam8);
            objParamColl.Add(sqlparam9);

            objData.ExecuteSP("dbo.spBEAuditDelete", ref  ds, objCommand);

            int updatestatus = (int)objCommand.Parameters["ReturnValue"].Value;
            return updatestatus;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    //Audit(Delete)- For populating the PU dropdownlist
    public List<string> GetAuditPUs()
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<string> pu = new List<string>();

        try
        {
            objCommand = new SqlCommand();
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEAuditFetchPUs", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string id = string.Empty;

                    id = ds.Tables[0].Rows[i]["txtPU"].ToString();
                    pu.Add(id);
                }

            }
        }
        catch (Exception ex)
        {
            //Logger.LogErrorToServer(App_Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return pu;
    }

    internal bool IsDeleted(string quarter, string revenuevolume, string dmsdm, string year)
    {
        bool isDataExist = false;
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;


        try
        {

            var objParmquar = new SqlParameter();
            objParmquar.ParameterName = "@quarter";
            objParmquar.Direction = ParameterDirection.Input;
            objParmquar.SqlDbType = SqlDbType.VarChar;
            objParmquar.Value = quarter;

            var objParmRevVol = new SqlParameter();
            objParmRevVol.ParameterName = "@revvol";
            objParmRevVol.Direction = ParameterDirection.Input;
            objParmRevVol.SqlDbType = SqlDbType.VarChar;
            objParmRevVol.Value = revenuevolume;


            var objParmdmsdm = new SqlParameter();
            objParmdmsdm.ParameterName = "@dmsdm";
            objParmdmsdm.Direction = ParameterDirection.Input;
            objParmdmsdm.SqlDbType = SqlDbType.VarChar;
            objParmdmsdm.Value = dmsdm;

            var objParmyear = new SqlParameter();
            objParmyear.ParameterName = "@year";
            objParmyear.Direction = ParameterDirection.Input;
            objParmyear.SqlDbType = SqlDbType.VarChar;
            objParmyear.Value = year;




            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParmRevVol);
            objParamColl.Add(objParmquar);
            objParamColl.Add(objParmdmsdm);
            objParamColl.Add(objParmyear);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEAuditIsDeleted", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                int count = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL"].ToString());
                if (count > 0)
                    isDataExist = true;
            }

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return isDataExist;
    }


    public DataTable GetWeeklyDatesMccDmSdm(string qtr, string year, string type, DateTime weeklydate, DateTime dailydate)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        string tmp = string.Empty;
        List<string> lstdate = new List<string>();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtQtr";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);


            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@type";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = type;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@date";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.DateTime;
            sqlparam4.Value = weeklydate;
            objParamColl.Add(sqlparam4);


            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@dtdailydate";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.DateTime;
            sqlparam5.Value = dailydate;
            objParamColl.Add(sqlparam5);

            objData.ExecuteSP("dbo.EAS_SP_BEGetDates_MccDmSdm", ref  ds, objCommand);

            //if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            //{
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        // empCollection = new DUPUCCMap();
            //        tmp = Convert.ToDateTime(ds.Tables[0].Rows[i]["dtdate"]).ToString("dd-MMM-yyyy");
            //        lstdate.Add(tmp);


            //    }

            //}

            return ds.Tables[0];
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    //Portfolio - For adding new portfolio name 
    public int AddNewPortfolio(string portfolio, string txtsu)
    {
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtPortfolio";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.Value = portfolio;

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtSU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.Value = txtsu;

            sqlparam2 = new SqlParameter();
            sqlparam2.Direction = ParameterDirection.ReturnValue;
            sqlparam2.ParameterName = "ReturnValue";

            objParamColl.Add(sqlparam1);
            objParamColl.Add(sqlparam2);
            objParamColl.Add(sqlparam3);
            objData.ExecuteSP("dbo.spBEPortfolioAddNewPortfolio", ref  ds, objCommand);

            int count = (int)objCommand.Parameters["ReturnValue"].Value;
            return count;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public int EditPortfolio(string prevPortfolio, string newPortfolio, string SU)
    {
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtPrevPortfolio";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.Value = prevPortfolio;

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtNewPortfolio";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.Value = newPortfolio;


            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtSU";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.Value = SU;

            sqlparam3 = new SqlParameter();
            sqlparam3.Direction = ParameterDirection.ReturnValue;
            sqlparam3.ParameterName = "ReturnValue";

            objParamColl.Add(sqlparam1);
            objParamColl.Add(sqlparam2);
            objParamColl.Add(sqlparam3);
            objParamColl.Add(sqlparam4);

            objData.ExecuteSP("dbo.spBEPortfolioEditPortfolio", ref  ds, objCommand);

            int count = (int)objCommand.Parameters["ReturnValue"].Value;
            return count;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }


    public List<string> GetAllPortfolio(string userId)
    {
        DataSet ds = new DataSet();
        List<string> allport = new List<string>();
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        SqlCommand objCommand;

        try
        {

            objData = new DataAccess();

            objParm = new SqlParameter();
            objParm.ParameterName = "@txtuserid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.NVarChar;
            objParm.Value = userId;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData.GetConnection();
            objData.ExecuteSP("spBEFetchAllPortfolio", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataTable dt = new DataTable();
                    allport.Add(ds.Tables[0].Rows[i]["txtPortfolio"] + "");

                }


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return allport;

    }

    public DataSet GetDataSyncResult(string tableName, string year, string quarter)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;


        try
        {

            objCommand = new SqlCommand();
            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.ParameterName = "@TableNames";
            objParamStatus.Direction = ParameterDirection.Input;
            objParamStatus.SqlDbType = SqlDbType.VarChar;
            objParamStatus.Value = tableName;

            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@Year";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = year;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@Quarter";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = quarter;


            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            objParamColl.Add(objParamStatus);

            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);




            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("[EAS_SP_BEProdDevTablesSync]", ref ds, objCommand);



        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return ds;
    }

    public int SendMail()
    {

        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        int value;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@Status";
            objParm.Direction = ParameterDirection.Output;
            objParm.Size = 5;
            objParm.SqlDbType = SqlDbType.Int;


            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;


            objParamColl.AddRange(new SqlParameter[] { objParm });
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBESendEmail", objCommand);

            value = Convert.ToInt32(objCommand.Parameters[0].Value);


        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {

            objData.CloseConnection();

        }


        return value;

    }

    public List<MailAlert> getMailSetGrdList(string portfolio, string UserId)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        List<MailAlert> lstempMailColl = new List<MailAlert>();
        //List<DUPUCCMap> lstDUPU = new List<DUPUCCMap>();
        //DUPUCCMap puobj = new DUPUCCMap();
        MailAlert empCollection;

        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtPortfolio";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = portfolio;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEfetchMailSetGrd", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new MailAlert();

                    empCollection.portFolio = ds.Tables[0].Rows[i]["txtPortFolio"].ToString().Trim();
                    empCollection.txtPU = ds.Tables[0].Rows[i]["txtPU"].ToString().Trim();
                    empCollection.masterCustomerCode = ds.Tables[0].Rows[i]["txtMasterCustomerCode"].ToString().Trim();
                    empCollection.SendTo = ds.Tables[0].Rows[i]["txtSendTo"].ToString().Trim();
                    empCollection.SendCC = ds.Tables[0].Rows[i]["txtSendCC"].ToString().Trim();
                    empCollection.OnDMRev = ds.Tables[0].Rows[i]["txtOnDMRev"].ToString().Trim();
                    empCollection.OnDMVol = ds.Tables[0].Rows[i]["txtONDMVol"].ToString().Trim();
                    empCollection.OnsDMRev = ds.Tables[0].Rows[i]["txtOnSDMRev"].ToString().Trim();
                    empCollection.OnSDMVol = ds.Tables[0].Rows[i]["txtOnSDMVol"].ToString().Trim();
                    empCollection.Update = ds.Tables[0].Rows[i]["txtUpdateAlert"].ToString().Trim();
                    empCollection.Insert = ds.Tables[0].Rows[i]["txtInsertAlert"].ToString().Trim();
                    empCollection.Delete = ds.Tables[0].Rows[i]["txtDeleteAlert"].ToString().Trim();
                    empCollection.lstportFolio = GetAllPortfolio(UserId);
                    empCollection.AdminNo = Convert.ToInt32(ds.Tables[0].Rows[i]["intAdminNo"].ToString().Trim());
                    empCollection.updatedBy = ds.Tables[0].Rows[i]["txtUpdatedBy"] == DBNull.Value ? "" : ds.Tables[0].Rows[i]["txtUpdatedBy"].ToString().Trim() == null ? "" : ds.Tables[0].Rows[i]["txtUpdatedBy"].ToString().Trim();
                    empCollection.lstPU = GetAllPUs(UserId);
                    lstempMailColl.Add(empCollection);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempMailColl;
    }

    public string GetMenuCode(string userID)
    {


        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        string reportCode = string.Empty;


        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@Role";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = userID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEGetMenuCode", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                reportCode = ds.Tables[0].Rows[0]["MenuCode"] + "";
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return reportCode;
    }

    //Audit(View & Delete)- For populating the SU dropdownlist
    public List<string> GetAuditSUs(string txtUserID)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<string> su = new List<string>();
        try
        {
            objCommand = new SqlCommand();
            SqlParameter objUserID = new SqlParameter();
            objUserID.Direction = ParameterDirection.Input;
            objUserID.SqlDbType = SqlDbType.VarChar;
            objUserID.ParameterName = "@txtUserID";
            objUserID.Value = txtUserID;
            objCommand.Parameters.Add(objUserID);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEFetchSU", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string id = string.Empty;

                    id = ds.Tables[0].Rows[i]["txtSU"].ToString();
                    su.Add(id);
                }

            }
        }
        catch (Exception ex)
        {
            //Logger.LogErrorToServer(App_Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return su;
    }

    public void SaveMailAlertDetails(int AdminNo, string DMRev, string SDMrev, string DMVol, string SDMVol, string Upd, string Ins, string Del, string To, string CC, string UpdtBy)
    {


        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@AdminNo";
            objParm.Direction = ParameterDirection.Input;
            objParm.Size = 50;
            objParm.SqlDbType = SqlDbType.Int;
            objParm.Value = AdminNo;

            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@SDMRev";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.Size = 50;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = SDMrev;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@SDMVol";
            objParamStatus2.Size = 50;
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = SDMVol;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@DMRev";
            objParamStatus4.Size = 50;
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = DMRev;

            SqlParameter objParamStatus6 = new SqlParameter();
            objParamStatus6.ParameterName = "@DMVol";
            objParamStatus6.Size = 50;
            objParamStatus6.Direction = ParameterDirection.Input;
            objParamStatus6.SqlDbType = SqlDbType.NVarChar;
            objParamStatus6.Value = DMVol;

            SqlParameter objParamStatus7 = new SqlParameter();
            objParamStatus7.ParameterName = "@insert";
            objParamStatus7.Size = 10;
            objParamStatus7.Direction = ParameterDirection.Input;
            objParamStatus7.SqlDbType = SqlDbType.NChar;
            objParamStatus7.Value = Ins;

            SqlParameter objParamStatus8 = new SqlParameter();
            objParamStatus8.ParameterName = "@update";
            objParamStatus8.Direction = ParameterDirection.Input;
            objParamStatus8.Size = 10;
            objParamStatus8.SqlDbType = SqlDbType.NChar;
            objParamStatus8.Value = Upd;

            SqlParameter objParamStatus9 = new SqlParameter();
            objParamStatus9.ParameterName = "@delete";
            objParamStatus9.Size = 10;
            objParamStatus9.Direction = ParameterDirection.Input;
            objParamStatus9.SqlDbType = SqlDbType.VarChar;
            objParamStatus9.Value = Del;

            SqlParameter objParamStatus11 = new SqlParameter();
            objParamStatus11.ParameterName = "@to";
            objParamStatus11.Size = 80;
            objParamStatus11.Direction = ParameterDirection.Input;
            objParamStatus11.SqlDbType = SqlDbType.VarChar;
            objParamStatus11.Value = To;

            SqlParameter objParamStatus12 = new SqlParameter();
            objParamStatus12.ParameterName = "@cc";
            objParamStatus12.Size = 80;
            objParamStatus12.Direction = ParameterDirection.Input;
            objParamStatus12.SqlDbType = SqlDbType.VarChar;
            objParamStatus12.Value = CC;

            SqlParameter objParamStatus13 = new SqlParameter();
            objParamStatus13.ParameterName = "@UpdatedBy";
            objParamStatus13.Size = 80;
            objParamStatus13.Direction = ParameterDirection.Input;
            objParamStatus13.SqlDbType = SqlDbType.VarChar;
            objParamStatus13.Value = UpdtBy;




            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;


            objParamColl.AddRange(new SqlParameter[]{ objParm,objParamStatus1,objParamStatus2,objParamStatus4,objParamStatus6,objParamStatus7,objParamStatus8,
                            objParamStatus9,objParamStatus11,objParamStatus12,objParamStatus13});
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("BESavetoBEMailAlerts", objCommand);


        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {

            objData.CloseConnection();
        }




    }

    public List<BEAdminUI> GetBERoleMapping(string userid)
    {

        DataSet ds = new DataSet();
        SqlParameter sqlparam1;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<BEAdminUI> lstempCollection = new List<BEAdminUI>();
        BEAdminUI empCollection;
        try
        {


            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtuserid";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = userid;
            objParamColl.Add(sqlparam1);


            objData = new DataAccess();
            objData.GetConnection();



            objData.ExecuteSP("spBEGetRoles", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new BEAdminUI();

                    // empCollection.DU = ds.Tables[0].Rows[i]["txtDU"].ToString();
                    empCollection.Role = ds.Tables[0].Rows[i]["txtRole"].ToString();
                    // empCollection.CustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                    lstempCollection.Add(empCollection);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }

    public string GetDelegatedUserRole(string userID)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;


        try
        {


            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@userID";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userID;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;



            objParamColl.Add(objParamUserId);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEUserIDValidation", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                return ds.Tables[0].Rows[0]["role"] + "";




            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return "";
    }


    public DataTable GetBEReport(string qtr, string year, string pu, string dh, string userid, string type, DateTime date, string OnOffYes, string month)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8, sqlparam9;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtdh";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = dh;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@datedd";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.DateTime;
            sqlparam7.Value = date;
            objParamColl.Add(sqlparam7);

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@paramonoff";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.SqlDbType = SqlDbType.VarChar;
            sqlparam8.Value = OnOffYes;
            objParamColl.Add(sqlparam8);

            sqlparam9 = new SqlParameter();
            sqlparam9.ParameterName = "@month";
            sqlparam9.Direction = ParameterDirection.Input;
            sqlparam9.SqlDbType = SqlDbType.VarChar;
            sqlparam9.Value = month;
            objParamColl.Add(sqlparam9);

            objData.ExecuteSP("dbo.spBE_Report_date_Vol_month", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }


    public List<string> GetMonthsForBEReport(string Quarter, string Year, DateTime date)
    {
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();

        List<string> month = new List<string>();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = Quarter;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = Year;
            objParamColl.Add(sqlparam2);


            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@date";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.DateTime;
            sqlparam3.Value = date;
            objParamColl.Add(sqlparam3);

            objData.ExecuteSP("dbo.spBeGetMonths", ref  ds, objCommand);
            //return ds.Tables[0];

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string id = string.Empty;

                    id = ds.Tables[0].Rows[i]["Month"].ToString();
                    month.Add(id);
                }

            }

        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return month;

    }

    public DataTable FetchReports(string userID, string type)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;

        try
        {
            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@userid";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userID;

            SqlParameter objParamType = new SqlParameter();
            objParamType.ParameterName = "@type";
            objParamType.Direction = ParameterDirection.Input;
            objParamType.SqlDbType = SqlDbType.VarChar;
            objParamType.Value = type;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;



            objParamColl.Add(objParamUserId);
            objParamColl.Add(objParamType);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEFetchReports_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                return ds.Tables[0];
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataTable();
    }



    internal List<object> GetMaintenanceDetails()
    {
        List<object> lstReturnValue = new List<object>();


        DataSet ds = new DataSet();

        SqlCommand objCommand = new SqlCommand();


        try
        {


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEGetMaintenanceDetails", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                lstReturnValue.Add(ds.Tables[0].Rows[0][0]);
                lstReturnValue.Add(ds.Tables[0].Rows[0][1]);

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstReturnValue;
    }

    internal void SaveMaintenanceDetails(bool isOffline, string message)
    {

        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        // SqlParameter objParmUserId;
        SqlParameter objParmMessage;
        SqlParameter objParamIsOffline;

        try
        {


            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            objParmMessage = new SqlParameter();
            objParmMessage.ParameterName = "@message";
            objParmMessage.Direction = ParameterDirection.Input;
            objParmMessage.Value = message;
            objParamColl.Add(objParmMessage);

            objParamIsOffline = new SqlParameter();
            objParamIsOffline.ParameterName = "@isoffline";
            objParamIsOffline.Direction = ParameterDirection.Input;
            objParamIsOffline.SqlDbType = SqlDbType.Bit;
            objParamIsOffline.Value = isOffline;
            objParamColl.Add(objParamIsOffline);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEUpdateMaintenanceDetails", objCommand);

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    internal bool IsApplicationOffline()
    {
        bool returnValue = false;


        DataSet ds = new DataSet();

        SqlCommand objCommand = new SqlCommand();


        try
        {


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEGetMaintenanceDetails", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                returnValue = Convert.ToBoolean(ds.Tables[0].Rows[0][0]);


            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return returnValue;
    }


    public string GetFinpulseDumpDate()
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;

        string tmp = string.Empty;

        try
        {

            objCommand = new SqlCommand();



            objData = new DataAccess();
            objData.GetConnection();
            // objData.ExecuteSP("spBEDUListForDropDown", ref ds, objCommand);
            objData.ExecuteSP("EAS_SP_BEGetFinpulseDumpDate", ref ds, objCommand);

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // empCollection = new DUPUCCMap();
                    tmp = ds.Tables[0].Rows[i]["dumpdate"] == DBNull.Value ? "" : Convert.ToDateTime(ds.Tables[0].Rows[i]["dumpdate"]).ToString("dd-MMM-yyyy hh:mm IST");

                    //ds.Tables[0].Rows[i]["IsAllDU"] == DBNull.Value ? "No" : ds.Tables[0].Rows[i]["IsAllDU"].ToString().Trim() == null ? "No" : ds.Tables[0].Rows[i]["IsAllDU"].ToString().Trim() == "Y" ? "Yes" : "No";

                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return tmp;
    }



    public string GetDelegatedDMSDM(string userID)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;


        try
        {


            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@txtUserID";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userID;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;



            objParamColl.Add(objParamUserId);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEDMSDMDelegatedValidation", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                return ds.Tables[0].Rows[0]["exist"] + "";
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return "";
    }


    public string GetDelegatedUserRoleDMSDM(string userID)
    {


        DataSet ds = new DataSet();

        SqlCommand objCommand;


        try
        {


            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@txtUserID";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userID;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;



            objParamColl.Add(objParamUserId);



            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEDMSDMCheckDelegatedValidation", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                return ds.Tables[0].Rows[0]["role"] + "";




            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return "";
    }

    public DataTable GetVerical_Prtfolio(string userid, string type)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtUserId";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = userid;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txttype";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = type;
            objParamColl.Add(sqlparam2);



            objData.ExecuteSP("dbo.spBEPopulateVertical_Portfolio", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

    }

    public DataTable GetSDMVolComparison_vertical(string qtr, string year, string dh, string pu, string userid, string type, string vertical, string portfolio, DateTime date)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8, sqlparam9;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtdh";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = dh;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@vertical";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = vertical;
            objParamColl.Add(sqlparam7);

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@portfolio";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.SqlDbType = SqlDbType.VarChar;
            sqlparam8.Value = portfolio;
            objParamColl.Add(sqlparam8);

            sqlparam9 = new SqlParameter();
            sqlparam9.ParameterName = "@datedd";
            sqlparam9.Direction = ParameterDirection.Input;
            sqlparam9.SqlDbType = SqlDbType.DateTime;
            sqlparam9.Value = date;
            objParamColl.Add(sqlparam9);

            objData.ExecuteSP("dbo.spBEVolComparisionReport_Vertical", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }
    public DataTable GetDMVolComparison_vertical(string qtr, string year, string dh, string pu, string userid, string type, string vertical, string portfolio, DateTime date)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8, sqlparam9;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtdh";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = dh;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@vertical";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = vertical;
            objParamColl.Add(sqlparam7);

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@portfolio";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.SqlDbType = SqlDbType.VarChar;
            sqlparam8.Value = portfolio;
            objParamColl.Add(sqlparam8);

            sqlparam9 = new SqlParameter();
            sqlparam9.ParameterName = "@datedd";
            sqlparam9.Direction = ParameterDirection.Input;
            sqlparam9.SqlDbType = SqlDbType.DateTime;
            sqlparam9.Value = date;
            objParamColl.Add(sqlparam9);

            objData.ExecuteSP("dbo.spBEVolComparisionReportDM_Vertical", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }

    }

    public DataTable GetQtrYearForDump(string type, string quarter)
    {

        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        SqlParameter objParmPU;
        SqlParameter objParmUserId;
        DataSet dsDump = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();

        try
        {

            objParmUserId = new SqlParameter();
            objParmUserId.ParameterName = "@type";
            objParmUserId.Direction = ParameterDirection.Input;
            objParmUserId.SqlDbType = SqlDbType.VarChar;
            objParmUserId.Value = type;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParmUserId);

            objParmPU = new SqlParameter();
            objParmPU.ParameterName = "@qtr";
            objParmPU.Direction = ParameterDirection.Input;
            objParmPU.SqlDbType = SqlDbType.VarChar;
            objParmPU.Value = quarter;
            objParamColl.Add(objParmPU);






            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEGetQuarterYearForDump", ref  dsDump, objCommand);

            return dsDump.Tables[0];

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public DataTable GetBEVolumeForDump(string PU, string customerCode, string userID, string quarter, string year, string type)
    {

        {


            DataSet ds = new DataSet();

            SqlCommand objCommand;


            try
            {

                objCommand = new SqlCommand();
                SqlParameter objParamStatus = new SqlParameter();
                objParamStatus.ParameterName = "@txtCustomerCode";
                objParamStatus.Direction = ParameterDirection.Input;
                objParamStatus.SqlDbType = SqlDbType.VarChar;
                objParamStatus.Value = customerCode;

                SqlParameter objParamStatus1 = new SqlParameter();
                objParamStatus1.ParameterName = "@txtUserId";
                objParamStatus1.Direction = ParameterDirection.Input;
                objParamStatus1.SqlDbType = SqlDbType.VarChar;
                objParamStatus1.Value = userID;

                SqlParameter objParamStatus2 = new SqlParameter();
                objParamStatus2.ParameterName = "@txtQuarterName";
                objParamStatus2.Direction = ParameterDirection.Input;
                objParamStatus2.SqlDbType = SqlDbType.VarChar;
                objParamStatus2.Value = quarter;

                SqlParameter objParamStatus3 = new SqlParameter();
                objParamStatus3.ParameterName = "@txtYear";
                objParamStatus3.Direction = ParameterDirection.Input;
                objParamStatus3.SqlDbType = SqlDbType.VarChar;
                objParamStatus3.Value = year;

                SqlParameter objParamStatus4 = new SqlParameter();
                objParamStatus4.ParameterName = "@PU";
                objParamStatus4.Direction = ParameterDirection.Input;
                objParamStatus4.SqlDbType = SqlDbType.VarChar;
                objParamStatus4.Value = PU;

                SqlParameter objParamStatus5 = new SqlParameter();
                objParamStatus5.ParameterName = "@type";
                objParamStatus5.Direction = ParameterDirection.Input;
                objParamStatus5.SqlDbType = SqlDbType.VarChar;
                objParamStatus5.Value = type;
                objCommand = new SqlCommand();
                SqlParameterCollection objParamColl = objCommand.Parameters;


                objParamColl.Add(objParamStatus);


                objParamColl.Add(objParamStatus1);
                objParamColl.Add(objParamStatus2);
                objParamColl.Add(objParamStatus3);
                objParamColl.Add(objParamStatus4);
                objParamColl.Add(objParamStatus5);


                objData = new DataAccess();
                objData.GetConnection();
                objData.ExecuteSP("spBEFetchVolSDM_Report", ref ds, objCommand);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
                    dt.Columns["txtPU"].ColumnName = "PU";
                    //dt.Columns["txtDMMailId"].ColumnName = "DM";

                    //dt.Columns["txtDHMailId"].ColumnName = "DHMailId";
                    dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";

                    //dt.Columns["txtIsApproved"].ColumnName = "IsApproved";


                    //dt.Columns["dtDMUpdatedDate"].ColumnName = "DMLastModifiedOn";
                    dt.Columns["dtSDMUpdatedDate"].ColumnName = "SDMLastModifiedOn";


                    dt.Columns["intBEId"].ColumnName = "BEID";
                    //dt.Columns["txtRemarks"].ColumnName = "DMRemarks";
                    dt.Columns["txtRemarksSDM"].ColumnName = "SDMRemarks";



                    dt.Columns["DMfltMonth1EffortOnsite"].ColumnName = ("DMMonth1ON");
                    dt.Columns["DMfltMonth2EffortOnsite"].ColumnName = ("DMMonth2ON");
                    dt.Columns["DMfltMonth3EffortOnsite"].ColumnName = ("DMMonth3ON");
                    dt.Columns["DMfltMonth1EffortOffShore"].ColumnName = ("DMMonth1OFF");
                    dt.Columns["DMfltMonth2EffortOffShore"].ColumnName = ("DMMonth2OFF");
                    dt.Columns["DMfltMonth3EffortOffShore"].ColumnName = ("DMMonth3OFF");

                    dt.Columns["SDMfltMonth1EffortOnsite"].ColumnName = ("SDMMonth1ON");
                    dt.Columns["SDMfltMonth2EffortOnsite"].ColumnName = ("SDMMonth2ON");
                    dt.Columns["SDMfltMonth3EffortOnsite"].ColumnName = ("SDMMonth3ON");
                    dt.Columns["SDMfltMonth1EffortOffShore"].ColumnName = ("SDMMonth1OFF");
                    dt.Columns["SDMfltMonth2EffortOffShore"].ColumnName = ("SDMMonth2OFF");
                    dt.Columns["SDMfltMonth3EffortOffShore"].ColumnName = ("SDMMonth3OFF");


                    dt.Columns["DMfltTotalOnsite"].ColumnName = ("DMTotalON");
                    dt.Columns["DMfltTotalOffShore"].ColumnName = ("DMTotalOFF");
                    dt.Columns["SDMfltTotalOnsite"].ColumnName = ("SDMTotalON");
                    dt.Columns["SDMfltTotalOffShore"].ColumnName = ("SDMTotalOFF");
                    dt.Columns["DMfltGrandTotal"].ColumnName = ("DMGrandTotal");
                    dt.Columns["SDMfltGrandTotal"].ColumnName = ("SDMGrandTotal");

                    dt.Columns["txtOnsiteValueM1"].ColumnName = ("RTBRMonth1ON");
                    dt.Columns["txtOffshoreValueM1"].ColumnName = ("RTBRMonth1OFF");
                    dt.Columns["txtOnsiteValueM2"].ColumnName = ("RTBRMonth2ON");
                    dt.Columns["txtOffshoreValueM2"].ColumnName = ("RTBRMonth2OFF");
                    dt.Columns["txtOnsiteValueM3"].ColumnName = ("RTBRMonth3ON");
                    dt.Columns["txtOffshoreValueM3"].ColumnName = ("RTBRMonth3OFF");
                    dt.Columns["txtTotalOnsiteValue"].ColumnName = ("RTBRTotalON");
                    dt.Columns["txtTotalOffshoreValue"].ColumnName = ("RTBRTotalOFF");
                    dt.Columns["txtGrandTotalValue"].ColumnName = ("RTBRGrandTotal");

                    dt.Columns["txtQuarterName"].ColumnName = ("txtCurrentQuarterName");
                    dt.Columns["txtYear"].ColumnName = ("txtYear");

                    return dt;
                }
            }
            catch (Exception ex)
            {

                logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                objData.CloseConnection();
            }


            return new DataTable();
        }




    }

    public DataSet GetRevenueDataForDump(string PU, string customerCode, string dm, string quarter, string year, string currency, string type)
    {


        DataSet ds = new DataSet();
        DataSet dsreturn = new DataSet();
        SqlCommand objCommand;
        List<DataTable> retTable = new List<DataTable>();

        try
        {

            objCommand = new SqlCommand();
            SqlParameter objParamStatus = new SqlParameter();
            objParamStatus.ParameterName = "@txtCustomerCode";
            objParamStatus.Direction = ParameterDirection.Input;
            objParamStatus.SqlDbType = SqlDbType.VarChar;
            objParamStatus.Value = customerCode;

            SqlParameter objParamStatus1 = new SqlParameter();
            objParamStatus1.ParameterName = "@txtUserId";
            objParamStatus1.Direction = ParameterDirection.Input;
            objParamStatus1.SqlDbType = SqlDbType.VarChar;
            objParamStatus1.Value = dm;

            SqlParameter objParamStatus2 = new SqlParameter();
            objParamStatus2.ParameterName = "@txtQuarterName";
            objParamStatus2.Direction = ParameterDirection.Input;
            objParamStatus2.SqlDbType = SqlDbType.VarChar;
            objParamStatus2.Value = quarter;

            SqlParameter objParamStatus3 = new SqlParameter();
            objParamStatus3.ParameterName = "@txtYear";
            objParamStatus3.Direction = ParameterDirection.Input;
            objParamStatus3.SqlDbType = SqlDbType.VarChar;
            objParamStatus3.Value = year;

            SqlParameter objParamStatus4 = new SqlParameter();
            objParamStatus4.ParameterName = "@PU";
            objParamStatus4.Direction = ParameterDirection.Input;
            objParamStatus4.SqlDbType = SqlDbType.VarChar;
            objParamStatus4.Value = PU;

            SqlParameter objParamStatus5 = new SqlParameter();
            objParamStatus5.ParameterName = "@txtCurrency";
            objParamStatus5.Direction = ParameterDirection.Input;
            objParamStatus5.SqlDbType = SqlDbType.VarChar;
            objParamStatus5.Value = currency;

            SqlParameter objParamStatus6 = new SqlParameter();

            objParamStatus6.ParameterName = "@type";

            objParamStatus6.Direction = ParameterDirection.Input;

            objParamStatus6.SqlDbType = SqlDbType.VarChar;

            objParamStatus6.Value = type;


            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;


            objParamColl.Add(objParamStatus);

            objParamColl.Add(objParamStatus1);
            objParamColl.Add(objParamStatus2);
            objParamColl.Add(objParamStatus3);
            objParamColl.Add(objParamStatus4);
            objParamColl.Add(objParamStatus5);
            objParamColl.Add(objParamStatus6);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEFetchRevDataSDM_Report", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
                //dt.Columns["txtDU"].ColumnName = "DU";
                //dt.Columns["txtDMMailId"].ColumnName = "DM";
                dt.Columns["txtNativeCurrency"].ColumnName = "NativeCurrency";
                dt.Columns["DMfltMonth1BE"].ColumnName = "DMMonth1";
                dt.Columns["DMfltMonth2BE"].ColumnName = "DMMonth2";
                dt.Columns["DMfltMonth3BE"].ColumnName = "DMMonth3";
                dt.Columns["DMfltCurrentQtrBE"].ColumnName = "DMQCur";
                dt.Columns["txtPU"].ColumnName = "PU";
                dt.Columns["txtQuarterName"].ColumnName = "Current Quarter Name";
                dt.Columns["txtYear"].ColumnName = "Year";
                dt.Columns["txtDHMailid"].ColumnName = "DHMailId";
                //dt.Columns["DMfltNextQuarterBE"].ColumnName = "DMQNext"; //TODO
                //dt.Columns["fltPrevQtrBE"].ColumnName = "DMQPrev";
                dt.Columns["txtSDMMailId"].ColumnName = "SDMMailID";
                dt.Columns["SDMfltMonth1BE"].ColumnName = "SDMMonth1";
                dt.Columns["SDMfltMonth2BE"].ColumnName = "SDMMonth2";
                dt.Columns["SDMfltMonth3BE"].ColumnName = "SDMMonth3";
                dt.Columns["SDMfltCurrentQuarterBE"].ColumnName = "SDMQCur";
                //dt.Columns["SDMfltNextQuarterBE"].ColumnName = "SDMQNext"; //TODO

                //dt.Columns[""].ColumnName = "SDMQPrev";
                // dt.Columns["txtLastUpdatedBy"].ColumnName = "LastModifiedBy";

                dt.Columns["FinRTBRM1"].ColumnName = "ActualM1";
                dt.Columns["FinRTBRM2"].ColumnName = "ActualM2";
                dt.Columns["FinRTBRM3"].ColumnName = "ActualM3";
                //dt.Columns["txtIsApproved"].ColumnName = "IsApproved";
                //dt.Columns["fltGuidanceConvRate"].ColumnName = "GuidanceConversionRate";
                //dt.Columns["fltCurrentConvRate"].ColumnName = "CurrentConversionRate";

                // dt.Columns["dtDMUpdatedDate"].ColumnName = "DMLastModifiedOn";

                dt.Columns["dtSDMUpdatedDate"].ColumnName = "SDMLastModifiedOn";
                //dt.Columns["dtLastUpdatedDate"].ColumnName = "SDMLastModifiedOn";

                // dt.Columns["dtLastUpdatedDate"].ColumnName = "SDMLastModifiedOn";


                dt.Columns["intBEId"].ColumnName = "BEID";
                // dt.Columns["txtRemarks"].ColumnName = "Remarks";
                dt.Columns["FinRTBRTotal"].ColumnName = "totalRTBR";
                dt.Columns["txtSDMRemarks"].ColumnName = "SDMRemarks";

                dsreturn.Tables.Add(dt.Copy());


                return dsreturn;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataSet();
    }

    public DataTable GetRevenueReasons(string pu, string mcc, string nc, string quarter, string year)
    {

        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        SqlParameter objParmPU;
        SqlParameter objParmmcc;
        SqlParameter objParmnc;
        SqlParameter objParmquarter;
        SqlParameter objParmyear;
        DataSet dsDump = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();

        try
        {
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            objParmmcc = new SqlParameter();
            objParmmcc.ParameterName = "@txtMCCode";
            objParmmcc.Direction = ParameterDirection.Input;
            objParmmcc.SqlDbType = SqlDbType.VarChar;
            objParmmcc.Value = mcc;
            objParamColl.Add(objParmmcc);

            objParmPU = new SqlParameter();
            objParmPU.ParameterName = "@txtPU";
            objParmPU.Direction = ParameterDirection.Input;
            objParmPU.SqlDbType = SqlDbType.VarChar;
            objParmPU.Value = pu;
            objParamColl.Add(objParmPU);

            objParmnc = new SqlParameter();
            objParmnc.ParameterName = "@txtNC";
            objParmnc.Direction = ParameterDirection.Input;
            objParmnc.SqlDbType = SqlDbType.VarChar;
            objParmnc.Value = nc;
            objParamColl.Add(objParmnc);

            objParmquarter = new SqlParameter();
            objParmquarter.ParameterName = "@txtQuarter";
            objParmquarter.Direction = ParameterDirection.Input;
            objParmquarter.SqlDbType = SqlDbType.VarChar;
            objParmquarter.Value = quarter;
            objParamColl.Add(objParmquarter);

            objParmyear = new SqlParameter();
            objParmyear.ParameterName = "@txtyear";
            objParmyear.Direction = ParameterDirection.Input;
            objParmyear.SqlDbType = SqlDbType.VarChar;
            objParmyear.Value = year;
            objParamColl.Add(objParmyear);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEFetchReasons", ref  dsDump, objCommand);

            return dsDump.Tables[0];

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public DataTable GetVolumeReasons(string pu, string mcc, string quarter, string year)
    {

        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        SqlParameter objParmPU;
        SqlParameter objParmmcc;
        SqlParameter objParmquarter;
        SqlParameter objParmyear;
        DataSet dsDump = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();

        try
        {
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            objParmmcc = new SqlParameter();
            objParmmcc.ParameterName = "@txtMCCode";
            objParmmcc.Direction = ParameterDirection.Input;
            objParmmcc.SqlDbType = SqlDbType.VarChar;
            objParmmcc.Value = mcc;
            objParamColl.Add(objParmmcc);

            objParmPU = new SqlParameter();
            objParmPU.ParameterName = "@txtPU";
            objParmPU.Direction = ParameterDirection.Input;
            objParmPU.SqlDbType = SqlDbType.VarChar;
            objParmPU.Value = pu;
            objParamColl.Add(objParmPU);


            objParmquarter = new SqlParameter();
            objParmquarter.ParameterName = "@txtQuarter";
            objParmquarter.Direction = ParameterDirection.Input;
            objParmquarter.SqlDbType = SqlDbType.VarChar;
            objParmquarter.Value = quarter;
            objParamColl.Add(objParmquarter);

            objParmyear = new SqlParameter();
            objParmyear.ParameterName = "@txtyear";
            objParmyear.Direction = ParameterDirection.Input;
            objParmyear.SqlDbType = SqlDbType.VarChar;
            objParmyear.Value = year;
            objParamColl.Add(objParmyear);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEFetchReasonsVol", ref  dsDump, objCommand);

            return dsDump.Tables[0];

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public void InsertReasons(string pu, string mcc, string nc, string quarter, string year, string fieldtype, double budget, double projectloss,
        double projectclosure, double newbiz, double extn, double highprob, double others, string reason, string updatedby, string type)
    {

        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        SqlParameter objParmPU;
        SqlParameter objParmmcc;
        SqlParameter objParmnc;
        SqlParameter objParmquarter;
        SqlParameter objParmyear, objParam1, objParam2, objParam3, objParam4, objParam5, objParam6, objParam7, objParam8, objParam9, objParam10, objParam11;
        DataSet dsDump = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();

        try
        {
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            objParmmcc = new SqlParameter();
            objParmmcc.ParameterName = "@mcc";
            objParmmcc.Direction = ParameterDirection.Input;
            objParmmcc.SqlDbType = SqlDbType.VarChar;
            objParmmcc.Value = mcc;
            objParamColl.Add(objParmmcc);

            objParmPU = new SqlParameter();
            objParmPU.ParameterName = "@pu";
            objParmPU.Direction = ParameterDirection.Input;
            objParmPU.SqlDbType = SqlDbType.VarChar;
            objParmPU.Value = pu;
            objParamColl.Add(objParmPU);

            objParmnc = new SqlParameter();
            objParmnc.ParameterName = "@nc";
            objParmnc.Direction = ParameterDirection.Input;
            objParmnc.SqlDbType = SqlDbType.VarChar;
            objParmnc.Value = nc;
            objParamColl.Add(objParmnc);

            objParmquarter = new SqlParameter();
            objParmquarter.ParameterName = "@qtr";
            objParmquarter.Direction = ParameterDirection.Input;
            objParmquarter.SqlDbType = SqlDbType.VarChar;
            objParmquarter.Value = quarter;
            objParamColl.Add(objParmquarter);

            objParmyear = new SqlParameter();
            objParmyear.ParameterName = "@year";
            objParmyear.Direction = ParameterDirection.Input;
            objParmyear.SqlDbType = SqlDbType.VarChar;
            objParmyear.Value = year;
            objParamColl.Add(objParmyear);

            objParam1 = new SqlParameter();
            objParam1.ParameterName = "@fieldtype";
            objParam1.Direction = ParameterDirection.Input;
            objParam1.SqlDbType = SqlDbType.VarChar;
            objParam1.Value = fieldtype;
            objParamColl.Add(objParam1);

            objParam2 = new SqlParameter();
            objParam2.ParameterName = "@budgetcutrampdown";
            objParam2.Direction = ParameterDirection.Input;
            objParam2.SqlDbType = SqlDbType.Float;
            objParam2.Value = budget;
            objParamColl.Add(objParam2);

            objParam3 = new SqlParameter();
            objParam3.ParameterName = "@projectloss";
            objParam3.Direction = ParameterDirection.Input;
            objParam3.SqlDbType = SqlDbType.Float;
            objParam3.Value = projectloss;
            objParamColl.Add(objParam3);

            objParam4 = new SqlParameter();
            objParam4.ParameterName = "@expectedprojectclosure";
            objParam4.Direction = ParameterDirection.Input;
            objParam4.SqlDbType = SqlDbType.Float;
            objParam4.Value = projectclosure;
            objParamColl.Add(objParam4);

            objParam5 = new SqlParameter();
            objParam5.ParameterName = "@wonnewbiz";
            objParam5.Direction = ParameterDirection.Input;
            objParam5.SqlDbType = SqlDbType.Float;
            objParam5.Value = newbiz;
            objParamColl.Add(objParam5);

            objParam6 = new SqlParameter();
            objParam6.ParameterName = "@knownextn";
            objParam6.Direction = ParameterDirection.Input;
            objParam6.SqlDbType = SqlDbType.Float;
            objParam6.Value = extn;
            objParamColl.Add(objParam6);

            objParam7 = new SqlParameter();
            objParam7.ParameterName = "@probability";
            objParam7.Direction = ParameterDirection.Input;
            objParam7.SqlDbType = SqlDbType.Float;
            objParam7.Value = highprob;
            objParamColl.Add(objParam7);

            objParam9 = new SqlParameter();
            objParam9.ParameterName = "@others";
            objParam9.Direction = ParameterDirection.Input;
            objParam9.SqlDbType = SqlDbType.Float;
            objParam9.Value = others;
            objParamColl.Add(objParam9);

            objParam8 = new SqlParameter();
            objParam8.ParameterName = "@reason";
            objParam8.Direction = ParameterDirection.Input;
            objParam8.SqlDbType = SqlDbType.VarChar;
            objParam8.Value = reason;
            objParamColl.Add(objParam8);

            objParam10 = new SqlParameter();
            objParam10.ParameterName = "@updatedBy";
            objParam10.Direction = ParameterDirection.Input;
            objParam10.SqlDbType = SqlDbType.VarChar;
            objParam10.Value = updatedby;
            objParamColl.Add(objParam10);

            objParam11 = new SqlParameter();
            objParam11.ParameterName = "@type";
            objParam11.Direction = ParameterDirection.Input;
            objParam11.SqlDbType = SqlDbType.VarChar;
            objParam11.Value = type;
            objParamColl.Add(objParam11);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEInsertReasons", objCommand);



        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public int SaveBERevenueDH(int beid, decimal DHBE)
    {
        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam6;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@intBEID";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.Int;
            sqlparam1.Value = beid;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@fltDHBE";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = DHBE;
            objParamColl.Add(sqlparam2);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@retValue";
            sqlparam6.Direction = ParameterDirection.ReturnValue;
            sqlparam6.SqlDbType = SqlDbType.Int;
            objParamColl.Add(sqlparam6);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBERevenueDH", objCommand);

            var ret = Convert.ToInt32(sqlparam6.Value);

            return ret;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public int SaveBERevenueDH(int beid, decimal DHBE, string MCCode, string PU, string qtr, string year, string DH, string userid)
    {
        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam6, sqlparam3, sqlparam4, sqlparam5, sqlparam7, sqlparam8, sqlparam10;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@intBEID";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.Int;
            sqlparam1.Value = beid;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@fltDHBE";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = DHBE;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtmcc";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = MCCode;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtPU";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = PU;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@txtdh";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = DH;
            objParamColl.Add(sqlparam5);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@txtqtr";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = qtr;
            objParamColl.Add(sqlparam7);

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@year";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.SqlDbType = SqlDbType.VarChar;
            sqlparam8.Value = year;
            objParamColl.Add(sqlparam8);

            sqlparam10 = new SqlParameter();
            sqlparam10.ParameterName = "@UserId";
            sqlparam10.Direction = ParameterDirection.Input;
            sqlparam10.SqlDbType = SqlDbType.VarChar;
            sqlparam10.Value = userid;
            objParamColl.Add(sqlparam10);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@retValue";
            sqlparam6.Direction = ParameterDirection.ReturnValue;
            sqlparam6.SqlDbType = SqlDbType.Int;
            objParamColl.Add(sqlparam6);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBERevenueDH_superset", objCommand);

            var ret = Convert.ToInt32(sqlparam6.Value);

            return ret;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public int SaveBEVolumeDH(int beid, decimal DHOnsite, decimal DHOffshore)
    {
        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@intBEID";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.Int;
            sqlparam1.Value = beid;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@fltOnsite";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = DHOnsite;
            objParamColl.Add(sqlparam2);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@fltOffshore";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = DHOffshore;
            objParamColl.Add(sqlparam2);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@retValue";
            sqlparam4.Direction = ParameterDirection.ReturnValue;
            sqlparam4.SqlDbType = SqlDbType.Int;
            objParamColl.Add(sqlparam4);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEVolumeDH", objCommand);

            var ret = Convert.ToInt32(sqlparam4.Value);

            return ret;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public int SaveBEVolumeDH(int beid, decimal DHOnsite, decimal DHOffshore, string MCCode, string PU, string qtr, string year, string DH, string userid)
    {
        try
        {
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam6, sqlparam3, sqlparam4, sqlparam5, sqlparam7, sqlparam8, sqlparam10, sqlparam11;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@intBEID";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.Int;
            sqlparam1.Value = beid;
            objParamColl.Add(sqlparam1);


            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@fltOnsite";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = DHOnsite;
            objParamColl.Add(sqlparam2);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@fltOffshore";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = DHOffshore;
            objParamColl.Add(sqlparam6);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtmcc";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = MCCode;
            objParamColl.Add(sqlparam3);

            sqlparam11 = new SqlParameter();
            sqlparam11.ParameterName = "@txtPU";
            sqlparam11.Direction = ParameterDirection.Input;
            sqlparam11.SqlDbType = SqlDbType.VarChar;
            sqlparam11.Value = PU;
            objParamColl.Add(sqlparam11);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@txtdh";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = DH;
            objParamColl.Add(sqlparam5);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@txtqtr";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.VarChar;
            sqlparam7.Value = qtr;
            objParamColl.Add(sqlparam7);

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@year";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.SqlDbType = SqlDbType.VarChar;
            sqlparam8.Value = year;
            objParamColl.Add(sqlparam8);

            sqlparam10 = new SqlParameter();
            sqlparam10.ParameterName = "@UserId";
            sqlparam10.Direction = ParameterDirection.Input;
            sqlparam10.SqlDbType = SqlDbType.VarChar;
            sqlparam10.Value = userid;
            objParamColl.Add(sqlparam10);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@retValue";
            sqlparam4.Direction = ParameterDirection.ReturnValue;
            sqlparam4.SqlDbType = SqlDbType.Int;
            objParamColl.Add(sqlparam4);


            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.spBEVolumeDH_superset", objCommand);

            var ret = Convert.ToInt32(sqlparam4.Value);

            return ret;
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public DataTable GetDHVolume(string PU, string customerCode, string userID, string quarter, string year, string type)
    {

        {


            DataSet ds = new DataSet();

            SqlCommand objCommand;


            try
            {

                objCommand = new SqlCommand();
                SqlParameter objParamStatus = new SqlParameter();
                objParamStatus.ParameterName = "@txtCustomerCode";
                objParamStatus.Direction = ParameterDirection.Input;
                objParamStatus.SqlDbType = SqlDbType.VarChar;
                objParamStatus.Value = customerCode;

                SqlParameter objParamStatus1 = new SqlParameter();
                objParamStatus1.ParameterName = "@txtUserId";
                objParamStatus1.Direction = ParameterDirection.Input;
                objParamStatus1.SqlDbType = SqlDbType.VarChar;
                objParamStatus1.Value = userID;

                SqlParameter objParamStatus2 = new SqlParameter();
                objParamStatus2.ParameterName = "@txtQuarterName";
                objParamStatus2.Direction = ParameterDirection.Input;
                objParamStatus2.SqlDbType = SqlDbType.VarChar;
                objParamStatus2.Value = quarter;

                SqlParameter objParamStatus3 = new SqlParameter();
                objParamStatus3.ParameterName = "@txtYear";
                objParamStatus3.Direction = ParameterDirection.Input;
                objParamStatus3.SqlDbType = SqlDbType.VarChar;
                objParamStatus3.Value = year;

                SqlParameter objParamStatus4 = new SqlParameter();
                objParamStatus4.ParameterName = "@PU";
                objParamStatus4.Direction = ParameterDirection.Input;
                objParamStatus4.SqlDbType = SqlDbType.VarChar;
                objParamStatus4.Value = PU;

                SqlParameter objParamStatus5 = new SqlParameter();
                objParamStatus5.ParameterName = "@type";
                objParamStatus5.Direction = ParameterDirection.Input;
                objParamStatus5.SqlDbType = SqlDbType.VarChar;
                objParamStatus5.Value = type;

                objCommand = new SqlCommand();
                SqlParameterCollection objParamColl = objCommand.Parameters;


                objParamColl.Add(objParamStatus);
                objParamColl.Add(objParamStatus1);
                objParamColl.Add(objParamStatus2);
                objParamColl.Add(objParamStatus3);
                objParamColl.Add(objParamStatus4);
                objParamColl.Add(objParamStatus5);


                objData = new DataAccess();
                objData.GetConnection();
                objData.ExecuteSP("spBEFetchVolDH_superset", ref ds, objCommand);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    dt.Columns["txtMasterClientCode"].ColumnName = "CustomerCode";
                    dt.Columns["txtDHMailId"].ColumnName = "DH";
                    dt.Columns["DHfltOnsite"].ColumnName = "DHON";
                    dt.Columns["DHfltOffshore"].ColumnName = "DHOFF";
                    dt.Columns["DMfltTotalOnsite"].ColumnName = "DMON";
                    dt.Columns["DMfltTotalOffshore"].ColumnName = "DMOFF";
                    dt.Columns["SDMfltTotalOnsite"].ColumnName = "SDMON";
                    dt.Columns["SDMfltTotalOffshore"].ColumnName = "SDMOFF";
                    dt.Columns["txtTotalActualOnsite"].ColumnName = "ActualON";
                    dt.Columns["txtTotalActualOffshore"].ColumnName = "ActualOFF";
                    dt.Columns["txtprevTotalActualOnsite"].ColumnName = "prevON";
                    dt.Columns["txtprevTotalActualOffshore"].ColumnName = "prevOFF";
                    dt.Columns["txtprev1TotalActualOnsite"].ColumnName = "prev1ON";
                    dt.Columns["txtprev1TotalActualOffshore"].ColumnName = "prev1OFF";
                    dt.Columns["txtDHUpdatedBy"].ColumnName = "DHUpdatedBy";
                    dt.Columns["dtDHUpdatedDate"].ColumnName = "DHUpdatedDate";
                    dt.Columns["txtPU"].ColumnName = "PU";
                    dt.Columns["intBEId"].ColumnName = "BEID";

                    return dt;
                }
            }
            catch (Exception ex)
            {

                logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                throw;
            }
            finally
            {
                objData.CloseConnection();
            }


            return new DataTable();
        }




    }

    public DataTable GetPUBaliReport()
    {
        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            objData.ExecuteSP("dbo.spBEBaliFetchPU", ref  ds, objCommand);

            return ds.Tables[0]; ;

        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }

    public DataTable GetBaliReportQtrYear(string type, string qtr)
    {

        DataSet ds = new DataSet();
        SqlCommand cmd = new SqlCommand();
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@type";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = type;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@qtr";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = qtr;
            objParamColl.Add(sqlparam2);
            objData.ExecuteSP("dbo.[spBEBaliReportQtrYear]", ref  ds, objCommand);

            return ds.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetBalisReport(string qtr, string year, string pu)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;

            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@txtYear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);

            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@PU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);


            objData.ExecuteSP("dbo.BEBalisReport", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public List<Region> GetRegion()
    {

        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;

        objCommand = new SqlCommand();
        objParamColl = objCommand.Parameters;

        List<Region> lstRegion = new List<Region>();
        Region region;
        try
        {

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEFetchRegion", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    region = new Region();

                    // empCollection.DU = ds.Tables[0].Rows[i]["txtDU"].ToString();
                    region.Reg = ds.Tables[0].Rows[i]["Division"].ToString();
                    // empCollection.CustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                    lstRegion.Add(region);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstRegion;
    }

    public DataTable GetWeeklyRevData(string quarter, string year, string region)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;

        try
        {

            objCommand = new SqlCommand();
            SqlParameter objParam1 = new SqlParameter();
            objParam1.ParameterName = "@qtr";
            objParam1.Direction = ParameterDirection.Input;
            objParam1.SqlDbType = SqlDbType.VarChar;
            objParam1.Value = quarter;

            SqlParameter objParam2 = new SqlParameter();
            objParam2.ParameterName = "@year";
            objParam2.Direction = ParameterDirection.Input;
            objParam2.SqlDbType = SqlDbType.VarChar;
            objParam2.Value = year;

            SqlParameter objParam3 = new SqlParameter();
            objParam3.ParameterName = "@region";
            objParam3.Direction = ParameterDirection.Input;
            objParam3.SqlDbType = SqlDbType.VarChar;
            objParam3.Value = region;

            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;

            objParamColl.Add(objParam1);
            objParamColl.Add(objParam2);
            objParamColl.Add(objParam3);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEWeeklyRevenueReport", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                dt.Columns["txtPU"].ColumnName = "PU";
                dt.Columns["txtmasterclientcode"].ColumnName = "MasterClientCode";
                dt.Columns["txtRegion"].ColumnName = "Region";
                dt.Columns["fltLastUpdatedValue"].ColumnName = "LastUpdatedValue";
                dt.Columns["fltprevQtrAct"].ColumnName = "PrevQtrActuals";
                dt.Columns["fltDMBE"].ColumnName = "DMBE";
                dt.Columns["fltprevSDMBE"].ColumnName = "PrevSDMBE";
                dt.Columns["fltSDMBE"].ColumnName = "SDMBE";
                dt.Columns["fltRTBR"].ColumnName = ("RTBR");
                dt.Columns["fltMCOBE"].ColumnName = ("MCOBE");
                dt.Columns["fltDHBE"].ColumnName = ("DHBE");
                dt.Columns["fltDHPortalBE"].ColumnName = ("DHPortalBE");
                dt.Columns["dtSDMUpdatedDate"].ColumnName = ("SDMUpdatedDate");
                dt.Columns["dtDMUpdatedDate"].ColumnName = ("DMUpdatedDate");
                dt.Columns["dtDHUpdatedDate"].ColumnName = ("DHUpdatedDate");

                return dt;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataTable();
    }

    public DataTable GetWeeklyVolData(string quarter, string year, string region)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;

        try
        {

            objCommand = new SqlCommand();
            SqlParameter objParam1 = new SqlParameter();
            objParam1.ParameterName = "@qtr";
            objParam1.Direction = ParameterDirection.Input;
            objParam1.SqlDbType = SqlDbType.VarChar;
            objParam1.Value = quarter;

            SqlParameter objParam2 = new SqlParameter();
            objParam2.ParameterName = "@year";
            objParam2.Direction = ParameterDirection.Input;
            objParam2.SqlDbType = SqlDbType.VarChar;
            objParam2.Value = year;

            SqlParameter objParam3 = new SqlParameter();
            objParam3.ParameterName = "@region";
            objParam3.Direction = ParameterDirection.Input;
            objParam3.SqlDbType = SqlDbType.VarChar;
            objParam3.Value = region;

            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;

            objParamColl.Add(objParam1);
            objParamColl.Add(objParam2);
            objParamColl.Add(objParam3);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEWeeklyVolumeReport", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                dt.Columns["PU"].ColumnName = "PU";
                dt.Columns["MasterClientCode"].ColumnName = "MasterClientCode";
                dt.Columns["Region"].ColumnName = "Region";
                dt.Columns["LastUpdatedOnValue"].ColumnName = "LastUpdatedOnValue";
                dt.Columns["LastUpdatedOffValue"].ColumnName = "LastUpdatedOffValue";
                dt.Columns["PrevQtrActVolOn"].ColumnName = "PrevQtrActVolOn";
                dt.Columns["PrevQtrActVolOff"].ColumnName = "PrevQtrActVolOff";
                dt.Columns["DMOnVal"].ColumnName = "DMOnVal";
                dt.Columns["DMOffVal"].ColumnName = "DMOffVal";
                dt.Columns["PrevSDMOnVal"].ColumnName = "PrevSDMOnVal";
                dt.Columns["PrevSDMOffVal"].ColumnName = "PrevSDMOffVal";
                dt.Columns["SDMOnVal"].ColumnName = "SDMOnVal";
                dt.Columns["SDMOffVal"].ColumnName = "SDMOffVal";
                dt.Columns["DHOnVal"].ColumnName = ("DHOnVal");
                dt.Columns["DHOffVal"].ColumnName = ("DHOffVal");
                dt.Columns["PBSOnVal"].ColumnName = ("PBSOnVal");
                dt.Columns["PBSOffVal"].ColumnName = ("PBSOffVal");
                dt.Columns["ALCONOnVal"].ColumnName = ("ALCONOnVal");
                dt.Columns["ALCONOffVal"].ColumnName = ("ALCONOffVal");
                dt.Columns["DMUpdatedDate"].ColumnName = ("DMUpdatedDate");
                dt.Columns["SDMUpdatedDate"].ColumnName = ("SDMUpdatedDate");
                dt.Columns["DHUpdatedDate"].ColumnName = ("DHUpdatedDate");

                return dt;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataTable();
    }
    public DataTable GetMondayData(string quarter, string year)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;

        try
        {

            objCommand = new SqlCommand();
            SqlParameter objParam1 = new SqlParameter();
            objParam1.ParameterName = "@qtr";
            objParam1.Direction = ParameterDirection.Input;
            objParam1.SqlDbType = SqlDbType.VarChar;
            objParam1.Value = quarter;

            SqlParameter objParam2 = new SqlParameter();
            objParam2.ParameterName = "@year";
            objParam2.Direction = ParameterDirection.Input;
            objParam2.SqlDbType = SqlDbType.VarChar;
            objParam2.Value = year;

            //SqlParameter objParam3 = new SqlParameter();
            //objParam3.ParameterName = "@region";
            //objParam3.Direction = ParameterDirection.Input;
            //objParam3.SqlDbType = SqlDbType.VarChar;
            //objParam3.Value = region;

            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;

            objParamColl.Add(objParam1);
            objParamColl.Add(objParam2);
            //objParamColl.Add(objParam3);

            objData = new DataAccess();
            objData.GetConnection();
            //objData.ExecuteSP("spBEWeeklyRevenueReport", ref ds, objCommand);
            objData.ExecuteSP("spGetRU_BE_VOLData", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                dt.Columns["Account"].ColumnName = "Account";
                dt.Columns["ServiceLine"].ColumnName = "ServiceLine";
                dt.Columns["DM"].ColumnName = ("DM");
                dt.Columns["SDM"].ColumnName = "SDM";
                dt.Columns["NativeCurrency"].ColumnName = "Native Currency";
                dt.Columns["QuarterName"].ColumnName = "QuarterName";
                dt.Columns["Year"].ColumnName = "Year";
                dt.Columns["DM BE"].ColumnName = "DM BE";
                dt.Columns["SDM BE"].ColumnName = "SDM BE";
                dt.Columns["DM Vol Offshore"].ColumnName = "DM Vol Offshore";
                dt.Columns["DM Vol Onsite"].ColumnName = "DM Vol Onsite";
                dt.Columns["DM Vol"].ColumnName = "DM Vol";
                dt.Columns["SDM Vol Offshore"].ColumnName = "SDM Vol Offshore";
                dt.Columns["SDM Vol Onsite"].ColumnName = "SDM Vol Onsite";
                dt.Columns["SDM Vol"].ColumnName = "SDM Vol";


                return dt;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataTable();
    }

    public DataTable GetBEReportforsplit_Admin(string qtr, string year, string pu, string dh, string userid, string type, DateTime date, string OnOffYes)
    {

        DataSet dsCurrConv = new DataSet();
        SqlCommand sqlcmd = new SqlCommand();
        //SqlDataAdapter daCurrConv = new SqlDataAdapter();
        try
        {
            objData = new DataAccess();
            objData.GetConnection();
            SqlCommand objCommand;
            SqlParameterCollection objParamColl;

            SqlParameter sqlparam1, sqlparam2, sqlparam3, sqlparam4, sqlparam5, sqlparam6, sqlparam7, sqlparam8;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;



            sqlparam1 = new SqlParameter();
            sqlparam1.ParameterName = "@txtCurQuarterName";
            sqlparam1.Direction = ParameterDirection.Input;
            sqlparam1.SqlDbType = SqlDbType.VarChar;
            sqlparam1.Value = qtr;
            objParamColl.Add(sqlparam1);

            sqlparam2 = new SqlParameter();
            sqlparam2.ParameterName = "@finyear";
            sqlparam2.Direction = ParameterDirection.Input;
            sqlparam2.SqlDbType = SqlDbType.VarChar;
            sqlparam2.Value = year;
            objParamColl.Add(sqlparam2);



            sqlparam3 = new SqlParameter();
            sqlparam3.ParameterName = "@txtPU";
            sqlparam3.Direction = ParameterDirection.Input;
            sqlparam3.SqlDbType = SqlDbType.VarChar;
            sqlparam3.Value = pu;
            objParamColl.Add(sqlparam3);

            sqlparam4 = new SqlParameter();
            sqlparam4.ParameterName = "@txtdh";
            sqlparam4.Direction = ParameterDirection.Input;
            sqlparam4.SqlDbType = SqlDbType.VarChar;
            sqlparam4.Value = dh;
            objParamColl.Add(sqlparam4);

            sqlparam5 = new SqlParameter();
            sqlparam5.ParameterName = "@userid";
            sqlparam5.Direction = ParameterDirection.Input;
            sqlparam5.SqlDbType = SqlDbType.VarChar;
            sqlparam5.Value = userid;
            objParamColl.Add(sqlparam5);

            sqlparam6 = new SqlParameter();
            sqlparam6.ParameterName = "@type";
            sqlparam6.Direction = ParameterDirection.Input;
            sqlparam6.SqlDbType = SqlDbType.VarChar;
            sqlparam6.Value = type;
            objParamColl.Add(sqlparam6);

            sqlparam7 = new SqlParameter();
            sqlparam7.ParameterName = "@datedd";
            sqlparam7.Direction = ParameterDirection.Input;
            sqlparam7.SqlDbType = SqlDbType.DateTime;
            sqlparam7.Value = date;
            objParamColl.Add(sqlparam7);

            sqlparam8 = new SqlParameter();
            sqlparam8.ParameterName = "@paramonoff";
            sqlparam8.Direction = ParameterDirection.Input;
            sqlparam8.SqlDbType = SqlDbType.VarChar;
            sqlparam8.Value = OnOffYes;
            objParamColl.Add(sqlparam8);

            objData.ExecuteSP("dbo.spBE_Report_date_Vol_newsplit_Admin", ref  dsCurrConv, objCommand);

            return dsCurrConv.Tables[0]; ;
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


    }

    public DataTable GetWeeklyRevDataTue(string quarter, string year)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;

        try
        {

            objCommand = new SqlCommand();
            SqlParameter objParam1 = new SqlParameter();
            objParam1.ParameterName = "@txtCurQuarterName";
            objParam1.Direction = ParameterDirection.Input;
            objParam1.SqlDbType = SqlDbType.VarChar;
            objParam1.Value = quarter;

            SqlParameter objParam2 = new SqlParameter();
            objParam2.ParameterName = "@finyear";
            objParam2.Direction = ParameterDirection.Input;
            objParam2.SqlDbType = SqlDbType.VarChar;
            objParam2.Value = year;

            //SqlParameter objParam3 = new SqlParameter();
            //objParam3.ParameterName = "@region";
            //objParam3.Direction = ParameterDirection.Input;
            //objParam3.SqlDbType = SqlDbType.VarChar;
            //objParam3.Value = region;

            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;

            objParamColl.Add(objParam1);
            objParamColl.Add(objParam2);
            //objParamColl.Add(objParam3);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBERaviReport_New1_Enhanc", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                dt.Columns["txtPU"].ColumnName = "PU";
                dt.Columns["txtMCC"].ColumnName = "MasterClientCode";
                dt.Columns["fltprevQtrRev"].ColumnName = "PrevQtrRevenue";
                dt.Columns["fltdhbecurwk"].ColumnName = "DHBECurWeek";
                dt.Columns["fltdhbeinclstqtr"].ColumnName = "DHBEIncLastQtr";
                dt.Columns["fltdhbelstwk"].ColumnName = "DHBELastWeek";
                dt.Columns["fltdhbechglstwk"].ColumnName = "DHBEChgLastWeek";
                dt.Columns["fltsdmbecurwk"].ColumnName = ("SDMBECurWeek");
                //
                dt.Columns["fltsdmbemon1"].ColumnName = ("SDMBEMon1");
                dt.Columns["fltsdmbemon2"].ColumnName = ("SDMBEMon2");
                dt.Columns["fltsdmbemon3"].ColumnName = ("SDMBEMon3");
                dt.Columns["fltdmbemon1"].ColumnName = ("DMBEMon1");
                dt.Columns["fltdmbemon2"].ColumnName = ("DMBEMon2");
                dt.Columns["fltdmbemon3"].ColumnName = ("DMBEMon3");
                //
                dt.Columns["fltdhsdmdif"].ColumnName = ("DHSDMDiff");
                dt.Columns["fltsdmbelstwk"].ColumnName = ("SDMBELastWeek");
                dt.Columns["fltsdmchglstwk"].ColumnName = ("SDMChgLastWeek");
                dt.Columns["fltrtbrcurwk"].ColumnName = ("RTBRCurrWeek");
                dt.Columns["fltdhrtbrdiff"].ColumnName = ("DHRTBRDiff");
                dt.Columns["fltrtbrlstwk"].ColumnName = ("RTBRLastWeek");
                dt.Columns["fltrtbrchglstwk"].ColumnName = ("RTBRChgLastWeek");
                dt.Columns["fltmcobecurwk"].ColumnName = ("MCOBECurWeek");
                dt.Columns["fltdhmcodiff"].ColumnName = ("DHMCODiff");

                return dt;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataTable();
    }

    public DataTable GetWeeklyVolDataTue(string quarter, string year)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;

        try
        {

            objCommand = new SqlCommand();
            SqlParameter objParam1 = new SqlParameter();
            objParam1.ParameterName = "@qtr";
            objParam1.Direction = ParameterDirection.Input;
            objParam1.SqlDbType = SqlDbType.VarChar;
            objParam1.Value = quarter;

            SqlParameter objParam2 = new SqlParameter();
            objParam2.ParameterName = "@year";
            objParam2.Direction = ParameterDirection.Input;
            objParam2.SqlDbType = SqlDbType.VarChar;
            objParam2.Value = year;

            //SqlParameter objParam3 = new SqlParameter();
            //objParam3.ParameterName = "@region";
            //objParam3.Direction = ParameterDirection.Input;
            //objParam3.SqlDbType = SqlDbType.VarChar;
            //objParam3.Value = region;

            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;

            objParamColl.Add(objParam1);
            objParamColl.Add(objParam2);
            //objParamColl.Add(objParam3);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("[spBERaviReport_vol]", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                dt.Columns["txtPU"].ColumnName = "PU";
                dt.Columns["txtMCC"].ColumnName = "MasterClientCode";
                dt.Columns["fltprevQtrVol"].ColumnName = "PrevQtrVolume";
                dt.Columns["fltSdmVolCurWk"].ColumnName = "SDMVolCurWeek";
                //dt.Columns["fltSdmVolInclstQtr"].ColumnName = "SDMVolIncLastQtr";
                dt.Columns["fltSdmVolLstWk"].ColumnName = "SDMVOLLastWeek";
                dt.Columns["fltSdmChgLstWk"].ColumnName = "SDMChgLastWeek";
                dt.Columns["fltAlconVolCurWk"].ColumnName = ("ALCONVolCurWeek");
                dt.Columns["fltSDMAlconDiff"].ColumnName = ("SDMALCONDiff");
                dt.Columns["fltPBSVolCurWk"].ColumnName = ("PBSVOLCurWeek");
                dt.Columns["fltSDMPBSdiff"].ColumnName = ("SDMPBSDiff");
                dt.Columns["fltAlconVolm1"].ColumnName = ("AlconVolM1");
                dt.Columns["fltAlconVolm2"].ColumnName = ("AlconVolM2");
                dt.Columns["fltAlconVolm3"].ColumnName = ("AlconVolM3");
                dt.Columns["fltAlconAvgm1m2"].ColumnName = ("AlconAvgM1M2");
                dt.Columns["fltPBSVolm1"].ColumnName = ("PBSVolM1");
                dt.Columns["fltPBSVolm2"].ColumnName = ("PBSVolM2");
                dt.Columns["fltPBSVolm3"].ColumnName = ("PBSVolM3");
                dt.Columns["fltPBSAvgm1m2"].ColumnName = ("PBSAvgM1M2");

                return dt;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataTable();
    }

    public DataTable GetMondayCallData(string quarter, string year)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;

        try
        {

            objCommand = new SqlCommand();
            SqlParameter objParam1 = new SqlParameter();
            objParam1.ParameterName = "@Qtr";
            objParam1.Direction = ParameterDirection.Input;
            objParam1.SqlDbType = SqlDbType.VarChar;
            objParam1.Value = quarter;

            SqlParameter objParam2 = new SqlParameter();
            objParam2.ParameterName = "@Year";
            objParam2.Direction = ParameterDirection.Input;
            objParam2.SqlDbType = SqlDbType.VarChar;
            objParam2.Value = year;

            //SqlParameter objParam3 = new SqlParameter();
            //objParam3.ParameterName = "@region";
            //objParam3.Direction = ParameterDirection.Input;
            //objParam3.SqlDbType = SqlDbType.VarChar;
            //objParam3.Value = region;

            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;

            objParamColl.Add(objParam1);
            objParamColl.Add(objParam2);
            //objParamColl.Add(objParam3);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBeRavisReport", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                dt.Columns["PU"].ColumnName = "PU";
                dt.Columns["MasterCustomerCode"].ColumnName = "MasterCustomerCode";

                dt.Columns["NextQtrM1Actual"].ColumnName = "NextQtrM1Actual";
                dt.Columns["NextQtrM1SDMBE"].ColumnName = "NextQtrM1SDMBE";
                dt.Columns["NextQtrM2Actual"].ColumnName = "NextQtrM2Actual";
                dt.Columns["NextQtrM2SDMBE"].ColumnName = "NextQtrM2SDMBE";
                dt.Columns["NextQtrM3Actual"].ColumnName = "NextQtrM3Actual";
                dt.Columns["NextQtrM3SDMBE"].ColumnName = "NextQtrM3SDMBE";
                dt.Columns["NextQtrTotalSDMBE"].ColumnName = ("NextQtrTotalSDMBE");

                dt.Columns["NextM1ActVol"].ColumnName = ("NextM1ActVol");
                dt.Columns["NextQtrM1SDMVol"].ColumnName = ("NextQtrM1SDMVol");
                dt.Columns["NextM2ActVol"].ColumnName = ("NextM2ActVol");
                dt.Columns["NextQtrM2SDMVol"].ColumnName = ("NextQtrM2SDMVol");
                dt.Columns["NextM3ActVol"].ColumnName = ("NextM3ActVol");
                dt.Columns["NextQtrM3SDMVol"].ColumnName = ("NextQtrM3SDMVol");
                dt.Columns["NextQtrTotalSDMVol"].ColumnName = ("NextQtrTotalSDMVol");

                dt.Columns["CurrentM1Act"].ColumnName = ("CurrentM1Act");
                dt.Columns["CurrentM1SDMBE"].ColumnName = ("CurrentM1SDMBE");
                dt.Columns["CurrentM2Act"].ColumnName = ("CurrentM2Act");
                dt.Columns["CurrentM2SDMBE"].ColumnName = ("CurrentM2SDMBE");
                dt.Columns["CurrentM3Act"].ColumnName = ("CurrentM3Act");
                dt.Columns["CurrentM3SDMBE"].ColumnName = ("CurrentM3SDMBE");
                dt.Columns["CurrentQtrTotalSDMBE"].ColumnName = ("CurrentQtrTotalSDMBE");

                dt.Columns["CurrentM1ActVol"].ColumnName = ("CurrentM1ActVol");
                dt.Columns["CurrentM1SDMVol"].ColumnName = ("CurrentM1SDMVol");
                dt.Columns["CurrentM2ActVol"].ColumnName = ("CurrentM2ActVol");
                dt.Columns["CurrentM2SDMVol"].ColumnName = ("CurrentM2SDMVol");
                dt.Columns["CurrentM3ActVol"].ColumnName = ("CurrentM3ActVol");
                dt.Columns["CurrentM3SDMVol"].ColumnName = ("CurrentM3SDMVol");
                dt.Columns["CurrentQtrTotalSDMVol"].ColumnName = ("CurrentQtrTotalSDMVol");

                dt.Columns["NextQtrM1ActStatus"].ColumnName = ("NextQtrM1ActStatus");
                dt.Columns["NextQtrM2ActStatus"].ColumnName = ("NextQtrM2ActStatus");
                dt.Columns["NextQtrM3ActStatus"].ColumnName = ("NextQtrM3ActStatus");

                dt.Columns["CurrentQtrM1ActStatus"].ColumnName = ("CurrentQtrM1ActStatus");
                dt.Columns["CurrentQtrM2ActStatus"].ColumnName = ("CurrentQtrM2ActStatus");
                dt.Columns["CurrentQtrM3ActStatus"].ColumnName = ("CurrentQtrM3ActStatus");

                return dt;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return new DataTable();
    }


    public void InsertUserAccess(BEAdminUI objAccess, string MCC, string du, string repcode, string SU, string DMorSDM, string IsReadOnly, string AccessLevel)
    {

        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        SqlParameter objParmPU;
        // SqlParameter objSDM;
        SqlParameter objParmMCCList;
        SqlParameter objParmUserId;
        SqlParameter objParmRole;
        SqlParameter objParmReportCodes;
        SqlParameter objParmSU;
        SqlParameter objParmDMorSDM;
        SqlParameter objParmIsReadOnly;
        SqlParameter objParmAccessLevel;
        try
        {

            objParmUserId = new SqlParameter();
            objParmUserId.ParameterName = "@txtUserId";
            objParmUserId.Direction = ParameterDirection.Input;
            objParmUserId.SqlDbType = SqlDbType.VarChar;
            objParmUserId.Value = objAccess.UserId;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParmUserId);

            objParmMCCList = new SqlParameter();
            objParmMCCList.ParameterName = "@txtMCCList";
            objParmMCCList.Direction = ParameterDirection.Input;
            objParmMCCList.SqlDbType = SqlDbType.VarChar;
            objParmMCCList.Value = MCC;
            objParamColl.Add(objParmMCCList);

            //objSDM = new SqlParameter();
            //objSDM.ParameterName = "@txtSDMList";
            //objSDM.Direction = ParameterDirection.Input;
            //objSDM.SqlDbType = SqlDbType.VarChar;
            //objSDM.Value = SDM;
            //objParamColl.Add(objSDM);

            objParmPU = new SqlParameter();
            objParmPU.ParameterName = "@txtPUList";
            objParmPU.Direction = ParameterDirection.Input;
            objParmPU.SqlDbType = SqlDbType.NVarChar;
            objParmPU.Value = du;
            objParamColl.Add(objParmPU);

            objParmRole = new SqlParameter();
            objParmRole.ParameterName = "@txtRole";
            objParmRole.Direction = ParameterDirection.Input;
            objParmRole.SqlDbType = SqlDbType.VarChar;
            objParmRole.Value = objAccess.Role;
            objParamColl.Add(objParmRole);


            objParmReportCodes = new SqlParameter();
            objParmReportCodes.ParameterName = "@txtReportCode";
            objParmReportCodes.Direction = ParameterDirection.Input;
            objParmReportCodes.SqlDbType = SqlDbType.VarChar;
            objParmReportCodes.Value = repcode;
            //objParmReportCodes.Value = objAccess.ReportCodeList;
            objParamColl.Add(objParmReportCodes);

            objParmSU = new SqlParameter();
            objParmSU.ParameterName = "@txtServiceLine";
            objParmSU.Direction = ParameterDirection.Input;
            objParmSU.SqlDbType = SqlDbType.VarChar;
            objParmSU.Value = SU;
            objParamColl.Add(objParmSU);

            objParmDMorSDM = new SqlParameter();
            objParmDMorSDM.ParameterName = "@txtDMorSDM";
            objParmDMorSDM.Direction = ParameterDirection.Input;
            objParmDMorSDM.SqlDbType = SqlDbType.VarChar;
            objParmDMorSDM.Value = DMorSDM;
            objParamColl.Add(objParmDMorSDM);

            objParmIsReadOnly = new SqlParameter();
            objParmIsReadOnly.ParameterName = "@txtisReadOnly";
            objParmIsReadOnly.Direction = ParameterDirection.Input;
            objParmIsReadOnly.SqlDbType = SqlDbType.VarChar;
            objParmIsReadOnly.Value = IsReadOnly;
            objParamColl.Add(objParmIsReadOnly);

            objParmAccessLevel = new SqlParameter();
            objParmAccessLevel.ParameterName = "@txtAccessLevel";
            objParmAccessLevel.Direction = ParameterDirection.Input;
            objParmAccessLevel.SqlDbType = SqlDbType.VarChar;
            objParmAccessLevel.Value = AccessLevel;
            objParamColl.Add(objParmAccessLevel);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.EAS_SP_InsertBEUserAccess_NSO", objCommand);

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
    }


    public List<BEAdminUI> GetBEPUMappingSU(string SU)
    {
        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<BEAdminUI> lstempCollection = new List<BEAdminUI>();
        BEAdminUI empCollection;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtSU";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = SU;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("[EAS_SP_GetAllNSOFORBE]", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new BEAdminUI();
                    empCollection.PU = ds.Tables[0].Rows[i][0].ToString();
                    lstempCollection.Add(empCollection);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection;
    }


    public string[] GetAllReportCodes()
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<string> lstempCollection = new List<string>();
        try
        {
            objCommand = new SqlCommand();

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.EAS_SP_BEGetAllReportCodes", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string ReportCode = ds.Tables[0].Rows[i]["ReportCode"].ToString().Trim() + "|" + ds.Tables[0].Rows[i]["ReportName"].ToString().Trim();
                    //string ReportCode = ds.Tables[0].Rows[i]["ReportName"].ToString().Trim();
                    lstempCollection.Add(ReportCode);
                }

            }
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection.ToArray();
    }



    public List<BEAdminUI> GetBEPUMapping(string userid, string SU)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlParameter objParm1;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<BEAdminUI> lstempCollection = new List<BEAdminUI>();
        BEAdminUI empCollection;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtUserId";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = userid;

            objParm1 = new SqlParameter();
            objParm1.ParameterName = "@SU";
            objParm1.Direction = ParameterDirection.Input;
            objParm1.SqlDbType = SqlDbType.VarChar;
            objParm1.Value = SU;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);
            objParamColl.Add(objParm1);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_GetNSOFORBE", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new BEAdminUI();

                    // empCollection.DU = ds.Tables[0].Rows[i]["txtDU"].ToString();
                    empCollection.PU = ds.Tables[0].Rows[i][0].ToString();
                    // empCollection.CustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                    lstempCollection.Add(empCollection);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }



    public List<BEAdminUI> GetBEPUMappingAll(string SU)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<BEAdminUI> lstempCollection = new List<BEAdminUI>();
        BEAdminUI empCollection;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtSU";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = SU;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_GetPUFORBEAll", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new BEAdminUI();

                    // empCollection.DU = ds.Tables[0].Rows[i]["txtDU"].ToString();
                    empCollection.PU = ds.Tables[0].Rows[i]["txtPU"].ToString();
                    // empCollection.CustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                    lstempCollection.Add(empCollection);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }
    public string GetMachineRole(string userID)
    {
        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        string role = string.Empty;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtuserid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = userID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_BEGetRoles_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                role = ds.Tables[0].Rows[0]["txtrole"] + "";
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return role;
    }

    public int verifyUserId(string UserId, string MachineUserId)
    {
        int count = 0;
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        try
        {
            SqlParameter sql = new SqlParameter();
            sql.ParameterName = "@txtuserid";
            sql.SqlDbType = SqlDbType.NVarChar;
            sql.Value = UserId;
            sql.Direction = ParameterDirection.Input;

            SqlParameter sql1 = new SqlParameter();
            sql1.ParameterName = "@txtMachineUserId";
            sql1.SqlDbType = SqlDbType.NVarChar;
            sql1.Value = MachineUserId;
            sql1.Direction = ParameterDirection.Input;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(sql);
            objParamColl.Add(sql1);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.[EAS_SP_VerifyUserId_NSO]", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                count = Convert.ToInt32(ds.Tables[0].Rows[0]["value"].ToString());
            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return count;

    }

    public List<BEAdminUI> GetAllRole()
    {
        DataSet ds = new DataSet();
        List<BEAdminUI> lstempCollection = new List<BEAdminUI>();
        BEAdminUI empcollection;
        //SqlCommand objCommand;
        try
        {
            //SqlParameter objParamUserId = new SqlParameter();
            //objParamUserId.ParameterName = "@txtUserId";
            //objParamUserId.Direction = ParameterDirection.Input;
            //objParamUserId.SqlDbType = SqlDbType.VarChar;
            //objParamUserId.Value = userID;
            //objCommand = new SqlCommand();
            //SqlParameterCollection objParamColl = objCommand.Parameters;
            //objParamColl.Add(objParamUserId);
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_BeReturnRole", ref ds);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empcollection = new BEAdminUI();
                    empcollection.Role = ds.Tables[0].Rows[i]["txtRole"].ToString();
                    lstempCollection.Add(empcollection);
                }
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection;
    }


    public List<BEAdminUI> GetRole(string userID)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<BEAdminUI> lstEmpCollection = new List<BEAdminUI>();
        BEAdminUI empcollection;
        try
        {
            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@txtUserId";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userID;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objParamUserId);
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBeReturnRole", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empcollection = new BEAdminUI();
                    empcollection.Role = ds.Tables[0].Rows[i]["Role"].ToString();
                    lstEmpCollection.Add(empcollection);
                }
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstEmpCollection;

    }

    public DataSet GetRoleForUser(string userID)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<BEAdminUI> lstEmpCollection = new List<BEAdminUI>();
        BEAdminUI empcollection;
        try
        {
            SqlParameter objParamUserId = new SqlParameter();
            objParamUserId.ParameterName = "@txtUserId";
            objParamUserId.Direction = ParameterDirection.Input;
            objParamUserId.SqlDbType = SqlDbType.VarChar;
            objParamUserId.Value = userID;
            objCommand = new SqlCommand();
            SqlParameterCollection objParamColl = objCommand.Parameters;
            objParamColl.Add(objParamUserId);
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBeReturnRole_NSO", ref ds, objCommand);
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return ds;

    }

    public List<BEAdminUI> GetMccMapping(string userid, string SU)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm1;
        SqlParameter objParm2;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<BEAdminUI> lstempCollection = new List<BEAdminUI>();
        BEAdminUI empCollection;
        try
        {
            objParm1 = new SqlParameter();
            objParm1.ParameterName = "@userid";
            objParm1.Direction = ParameterDirection.Input;
            objParm1.SqlDbType = SqlDbType.VarChar;
            objParm1.Value = userid;

            objParm2 = new SqlParameter();
            objParm2.ParameterName = "@SU";
            objParm2.Direction = ParameterDirection.Input;
            objParm2.SqlDbType = SqlDbType.VarChar;
            objParm2.Value = SU;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm1);
            objParamColl.Add(objParm2);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("[EAS_SP_BeGetMCCNSO]", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new BEAdminUI();

                    // empCollection.DU = ds.Tables[0].Rows[i]["txtDU"].ToString();
                    empCollection.PU = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();
                    // empCollection.CustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                    lstempCollection.Add(empCollection);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }

    public List<BEAdminUI> GetMccMappingAll()
    {

        DataSet ds = new DataSet();
        //SqlParameter objParm1;
        SqlCommand objCommand;
        //SqlParameterCollection objParamColl;
        List<BEAdminUI> lstempCollection = new List<BEAdminUI>();
        BEAdminUI empCollection;
        try
        {
            //objParm1 = new SqlParameter();
            //objParm1.ParameterName = "@userid";
            //objParm1.Direction = ParameterDirection.Input;
            //objParm1.SqlDbType = SqlDbType.VarChar;
            //objParm1.Value = userid;

            //objParm2 = new SqlParameter();
            //objParm2.ParameterName = "@SU";
            //objParm2.Direction = ParameterDirection.Input;
            //objParm2.SqlDbType = SqlDbType.VarChar;
            //objParm2.Value = SU;

            objCommand = new SqlCommand();
            //objParamColl = objCommand.Parameters;
            //objParamColl.Add(objParm1);
            // objParamColl.Add(objParm2);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("[EAS_SP_BeGetMCCPuAll]", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new BEAdminUI();

                    // empCollection.DU = ds.Tables[0].Rows[i]["txtDU"].ToString();
                    empCollection.PU = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();
                    // empCollection.CustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                    lstempCollection.Add(empCollection);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }

    public List<BEAdminUI> GetSDMMapping(string UserId, string AccessLevel)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm1;
        SqlParameter objParm2;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<BEAdminUI> lstempCollection = new List<BEAdminUI>();
        BEAdminUI empCollection;
        try
        {
            objParm1 = new SqlParameter();
            objParm1.ParameterName = "@txtUserId";
            objParm1.Direction = ParameterDirection.Input;
            objParm1.SqlDbType = SqlDbType.VarChar;
            objParm1.Value = UserId;

            objParm2 = new SqlParameter();
            objParm2.ParameterName = "@SU";
            objParm2.Direction = ParameterDirection.Input;
            objParm2.SqlDbType = SqlDbType.VarChar;
            objParm2.Value = AccessLevel;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm1);
            objParamColl.Add(objParm2);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("[EAS_SP_BEGetSDMMailId_NSO]", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new BEAdminUI();

                    // empCollection.DU = ds.Tables[0].Rows[i]["txtDU"].ToString();
                    empCollection.PU = ds.Tables[0].Rows[i]["txtSDMMailId"].ToString();
                    // empCollection.CustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                    lstempCollection.Add(empCollection);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }

    public List<BEAdminUI> GetSDMMappingAll(string UserID, string AccessLevel, string SU)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm1;
        SqlParameter objParm2;
        SqlParameter objParm3;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<BEAdminUI> lstempCollection = new List<BEAdminUI>();
        BEAdminUI empCollection;
        try
        {
            objParm1 = new SqlParameter();
            objParm1.ParameterName = "@txtUserId";
            objParm1.Direction = ParameterDirection.Input;
            objParm1.SqlDbType = SqlDbType.VarChar;
            objParm1.Value = UserID;

            objParm3 = new SqlParameter();
            objParm3.ParameterName = "@txtAccessLevel";
            objParm3.Direction = ParameterDirection.Input;
            objParm3.SqlDbType = SqlDbType.VarChar;
            objParm3.Value = AccessLevel;

            objParm2 = new SqlParameter();
            objParm2.ParameterName = "@SU";
            objParm2.Direction = ParameterDirection.Input;
            objParm2.SqlDbType = SqlDbType.VarChar;
            objParm2.Value = SU;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm1);
            objParamColl.Add(objParm2);
            objParamColl.Add(objParm3);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("[EAS_SP_BEGetSDMMailIdAll_NSO]", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new BEAdminUI();

                    // empCollection.DU = ds.Tables[0].Rows[i]["txtDU"].ToString();
                    empCollection.PU = ds.Tables[0].Rows[i]["txtSDMMailId"].ToString();
                    // empCollection.CustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                    lstempCollection.Add(empCollection);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;
    }

    public string GetReportCode(string userID)
    {


        DataSet ds = new DataSet();
        SqlParameter objParm;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        string reportCode = string.Empty;

        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@userID";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = userID;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("spBEGetReportCode_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                reportCode = ds.Tables[0].Rows[0]["txtReportCode"] + "";
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return reportCode;

    }

    public string[] GetReportCodes(string userid)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<string> lstempCollection = new List<string>();

        SqlParameter objParm;
        SqlParameterCollection objParamColl;

        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtuserid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = userid;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);
            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.EAS_SP_BEGetReportCodes_nso", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string ReportCode = ds.Tables[0].Rows[i]["ReportCode"].ToString().Trim() + "|" + ds.Tables[0].Rows[i]["ReportName"].ToString().Trim();
                    //string ReportCode = ds.Tables[0].Rows[i]["ReportName"].ToString().Trim();
                    lstempCollection.Add(ReportCode);
                }
            }
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection.ToArray();
    }


    public string[] GetSUTypeIsReadOnly(string userid)
    {
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<string> lstempCollection = new List<string>();

        SqlParameter objParm;
        SqlParameterCollection objParamColl;

        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtuserid";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = userid;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.EAS_SP_BEGetSUTypeIsReadOnly_NSO", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string txtServiceLine = ds.Tables[0].Rows[i]["txtServiceLine"].ToString().Trim();
                    string txtDMorSDM = ds.Tables[0].Rows[i]["txtDMorSDM"].ToString().Trim();
                    string txtisReadOnly = ds.Tables[0].Rows[i]["txtisReadOnly"].ToString().Trim();
                    string txtAnchorLevelAccess = ds.Tables[0].Rows[i]["Anchor Access Level"].ToString().Trim();
                    //string ReportCode = ds.Tables[0].Rows[i]["ReportName"].ToString().Trim();
                    lstempCollection.Add(txtServiceLine);
                    lstempCollection.Add(txtDMorSDM);
                    lstempCollection.Add(txtisReadOnly);
                    lstempCollection.Add(txtAnchorLevelAccess);
                }
            }
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection.ToArray();

    }


    public string[] GetPUForMCC(string PU)
    {
        DataSet ds = new DataSet();

        SqlCommand objCommand;

        List<string> lstempCollection = new List<string>();
        //lstempCollection.Add("ALL");
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtMcc";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = PU;

            objCommand = new SqlCommand();

            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.[EAS_SP_BEGetDMForNSO]", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string du = ds.Tables[0].Rows[i][0].ToString();
                    lstempCollection.Add(du);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection.ToArray();

    }


    public List<BEAdminUI> GetBEPUMappingAll(string SU, string UserId)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm, objParm1;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<BEAdminUI> lstempCollection = new List<BEAdminUI>();
        BEAdminUI empCollection;
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtSU";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = SU;

            objParm1 = new SqlParameter();
            objParm1.ParameterName = "@txtUserId";
            objParm1.Direction = ParameterDirection.Input;
            objParm1.SqlDbType = SqlDbType.VarChar;
            objParm1.Value = UserId;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);
            objParamColl.Add(objParm1);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_GetNSOFORBEAll", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    empCollection = new BEAdminUI();

                    // empCollection.DU = ds.Tables[0].Rows[i]["txtDU"].ToString();
                    empCollection.PU = ds.Tables[0].Rows[i]["Service Offering Code"].ToString();
                    // empCollection.CustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                    lstempCollection.Add(empCollection);
                }

            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        return lstempCollection;

    }



    public DataSet GetMccMappingAll(string userid, string SU)
    {

        DataSet ds = new DataSet();
        SqlParameter objParm1;
        SqlParameter objParm2;
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        List<BEAdminUI> lstempCollection = new List<BEAdminUI>();
        BEAdminUI empCollection;
        try
        {
            objParm1 = new SqlParameter();
            objParm1.ParameterName = "@txtUserId";
            objParm1.Direction = ParameterDirection.Input;
            objParm1.SqlDbType = SqlDbType.VarChar;
            objParm1.Value = userid;

            objParm2 = new SqlParameter();
            objParm2.ParameterName = "@SU";
            objParm2.Direction = ParameterDirection.Input;
            objParm2.SqlDbType = SqlDbType.VarChar;
            objParm2.Value = SU;

            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm1);
            objParamColl.Add(objParm2);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("EAS_SP_BeGetMCCNSOAll", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    empCollection = new BEAdminUI();

                //    // empCollection.DU = ds.Tables[0].Rows[i]["txtDU"].ToString();
                //    empCollection.PU = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();
                //    // empCollection.CustomerCode = ds.Tables[0].Rows[i]["txtMasterClientCode"].ToString();

                //    lstempCollection.Add(empCollection);
                //}
                return ds;
            }
        }
        catch (Exception ex)
        {

            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }


        //return lstempCollection;
        return ds;
    }


    public string[] GetAllRepCodes()
    {
        //SqlParameter objParm;
        //SqlParameterCollection objParamColl;
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<string> lstempCollection = new List<string>();
        try
        {
            //objParm = new SqlParameter();
            //objParm.ParameterName = "@txtUserId";
            //objParm.Direction = ParameterDirection.Input;
            //objParm.SqlDbType = SqlDbType.VarChar;
            //objParm.Value = userid;
            objCommand = new SqlCommand();
            //objParamColl = objCommand.Parameters;
            //objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.EAS_SP_BEGetAllRepCodes", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string ReportCode = ds.Tables[0].Rows[i]["ReportCode"].ToString().Trim() + "|" + ds.Tables[0].Rows[i]["ReportName"].ToString().Trim();
                    //string ReportCode = ds.Tables[0].Rows[i]["ReportName"].ToString().Trim();
                    lstempCollection.Add(ReportCode);
                }

            }
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection.ToArray();

    }


    public string[] GetAllRepCodes(string userid)
    {
        SqlParameter objParm;
        SqlParameterCollection objParamColl;
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        List<string> lstempCollection = new List<string>();
        try
        {
            objParm = new SqlParameter();
            objParm.ParameterName = "@txtUserId";
            objParm.Direction = ParameterDirection.Input;
            objParm.SqlDbType = SqlDbType.VarChar;
            objParm.Value = userid;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParm);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.EAS_SP_BEGetAllReportCodes_nso", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string ReportCode = ds.Tables[0].Rows[i]["ReportCode"].ToString().Trim() + "|" + ds.Tables[0].Rows[i]["ReportName"].ToString().Trim();
                    //string ReportCode = ds.Tables[0].Rows[i]["ReportName"].ToString().Trim();
                    lstempCollection.Add(ReportCode);
                }

            }
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            objData.CloseConnection();
        }
        return lstempCollection.ToArray();
    }


    public int Update_EAS_BEData_DM(string SU, string MCC, string OldDMMailId, string NewDMMailid)
    {
        int count = 0;
        DataSet ds = new DataSet();
        SqlCommand objCommand;
        SqlParameterCollection objParamColl;
        SqlParameter objParmSU, objParmMCC, objParmOldDMMailId, objParmNewDMMailid;
        try
        {
            objParmSU = new SqlParameter();
            objParmSU.ParameterName = "@txtSU";
            objParmSU.Direction = ParameterDirection.Input;
            objParmSU.SqlDbType = SqlDbType.VarChar;
            objParmSU.Value = SU;
            objCommand = new SqlCommand();
            objParamColl = objCommand.Parameters;
            objParamColl.Add(objParmSU);

            objParmMCC = new SqlParameter();
            objParmMCC.ParameterName = "@txtMCC";
            objParmMCC.Direction = ParameterDirection.Input;
            objParmMCC.SqlDbType = SqlDbType.VarChar;
            objParmMCC.Value = MCC;
            objParamColl.Add(objParmMCC);

            objParmOldDMMailId = new SqlParameter();
            objParmOldDMMailId.ParameterName = "@txtOldDMMailID";
            objParmOldDMMailId.Direction = ParameterDirection.Input;
            objParmOldDMMailId.SqlDbType = SqlDbType.NVarChar;
            objParmOldDMMailId.Value = OldDMMailId;
            objParamColl.Add(objParmOldDMMailId);

            objParmNewDMMailid = new SqlParameter();
            objParmNewDMMailid.ParameterName = "@txtNewDMMailiD";
            objParmNewDMMailid.Direction = ParameterDirection.Input;
            objParmNewDMMailid.SqlDbType = SqlDbType.VarChar;
            objParmNewDMMailid.Value = NewDMMailid;
            objParamColl.Add(objParmNewDMMailid);

            objData = new DataAccess();
            objData.GetConnection();
            objData.ExecuteSP("dbo.[EAS_SP_Update_EAS_BEData_DM]", ref ds, objCommand);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                count = Convert.ToInt32(ds.Tables[0].Rows[0]["value"].ToString());
            }

        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            objData.CloseConnection();
        }
        return count;
    }

}



