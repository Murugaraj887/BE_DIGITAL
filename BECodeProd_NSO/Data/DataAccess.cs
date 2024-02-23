using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Data.SqlClient;
using BEData ;


public class DataAccess
{
    private string strConn;
    private SqlConnection objConn;
    //private SqlDataAdapter objDataAdapter;
    //private SqlCommand sqlCmd;
    Logger logger = new Logger();

    string fileName = "DataLayer.DataAccess";
    public DataAccess()
    {

        try
        {
            // strConn = "Data Source=apac-ops;Initial Catalog=DemandCaptureDev;Persist Security Info=True;User ID=WBUser;Password=cmed@123";
            strConn = System.Configuration.ConfigurationManager.AppSettings["DemandCaptureConnectionString"];
        }
        catch (Exception ex)
        {


            throw ex;
        }
    }
    public void GetConnection()
    {
        try
        {
            objConn = new SqlConnection(strConn);
            objConn.Open();
        }
        catch (Exception ex)
        {


            throw ex;
        }
    }
    public void CloseConnection()
    {
        try
        {
            if (objConn.State == ConnectionState.Open)
                objConn.Close();
        }
        catch (Exception ex)
        {


            throw ex;
        }

    }
    //Testing Purpose
    public void ExecuteSP1(string strSPName, ref DataSet ds, SqlCommand sqlCmd)
    {
        //SqlCommand sqlCmd;
        SqlDataAdapter sqlAdapter;
        DataTable tbl = new DataTable();

        try
        {
            string objs = @"Data Source=nebula\mssqlserver1;Initial Catalog=EAS_Test;Persist Security Info=True;User ID=nebula_sql;Password=python@123";

            sqlCmd.Connection = new SqlConnection(objs);
            //sqlCmd.CommandTimeout = 500;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = strSPName;
            sqlCmd.CommandTimeout = int.MaxValue;
            //foreach (SqlParameter objParam in objParamColl)
            //{
            //    sqlCmd.Parameters.Add(objParam);
            //}

            sqlAdapter = new SqlDataAdapter(sqlCmd);
            sqlAdapter.Fill(ds);
            sqlCmd.Dispose();
        }
        catch (Exception ex)
        {


            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw ex;
        }
        finally
        {
            if (objConn != null)
                if (objConn.State == ConnectionState.Open)
                    objConn.Close();
        }
    }

    public void ExecuteSP(string strSPName, ref DataSet ds, SqlCommand sqlCmd)
    {
        //SqlCommand sqlCmd;
        SqlDataAdapter sqlAdapter;
        DataTable tbl = new DataTable();

        try
        {


            sqlCmd.Connection = objConn;
            sqlCmd.CommandTimeout = int.MaxValue;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = strSPName;
            sqlCmd.CommandTimeout = int.MaxValue;
            //foreach (SqlParameter objParam in objParamColl)
            //{
            //    sqlCmd.Parameters.Add(objParam);
            //}

            sqlAdapter = new SqlDataAdapter(sqlCmd);
            sqlAdapter.Fill(ds);
            sqlCmd.Dispose();
        }
        catch (Exception ex)
        {


            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw ex;
        }
        finally
        {
            if (objConn != null)
                if (objConn.State == ConnectionState.Open)
                    objConn.Close();
        }
    }

    //Specifically for BE Trends Report
    public void ExecuteSPTrends(string strSPName, ref DataSet ds, SqlCommand sqlCmd)
    {
        //SqlCommand sqlCmd;
        SqlDataAdapter sqlAdapter;
        DataTable tbl = new DataTable();

        try
        {
            sqlCmd.Connection = objConn;
            sqlCmd.CommandTimeout = int.MaxValue;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = strSPName;
            //foreach (SqlParameter objParam in objParamColl)
            //{
            //    sqlCmd.Parameters.Add(objParam);
            //}

            sqlAdapter = new SqlDataAdapter(sqlCmd);
            sqlAdapter.Fill(ds);
            sqlCmd.Dispose();
        }
        catch (Exception ex)
        {


            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw ex;
        }
        finally
        {
            if (objConn != null)
                if (objConn.State == ConnectionState.Open)
                    objConn.Close();
        }
    }

    public void ExecuteSP(string strSPName, SqlCommand sqlCmd)
    {
        //SqlCommand sqlCmd;
        try
        {
            sqlCmd.Connection = objConn;
            sqlCmd.CommandTimeout = int.MaxValue;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = strSPName;
            //foreach (SqlParameter objParam in objParamColl)
            //{
            //    sqlCmd.Parameters.Add(objParam);
            //}
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Dispose();
        }
        catch (Exception ex)
        {


            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw ex;
        }
        finally
        {
            if (objConn != null)
                if (objConn.State == ConnectionState.Open)
                    objConn.Close();
        }
    }

    public void ExecuteSP(string strSPName, ref DataSet ds)
    {
        SqlCommand sqlCmd = new SqlCommand();
        SqlDataAdapter sqlAdapter;
        DataTable tbl = new DataTable();

        try
        {
            sqlCmd.Connection = objConn;
            sqlCmd.CommandTimeout = int.MaxValue;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = strSPName;
            sqlAdapter = new SqlDataAdapter(sqlCmd);
            sqlAdapter.Fill(ds);
            sqlCmd.Dispose();
        }
        catch (Exception ex)
        {


            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw ex;
        }
        finally
        {
            if (objConn != null)
                if (objConn.State == ConnectionState.Open)
                    objConn.Close();
        }
    }
    //VISA function added:
    public bool Execute_SP(SqlCommand sqlCmd)
    {
        bool returnValue = false;
        try
        {
            GetConnection();
            sqlCmd.Connection = objConn;
            sqlCmd.CommandTimeout = int.MaxValue;
            sqlCmd.CommandType = CommandType.StoredProcedure;

            //foreach (SqlParameter objParam in objParamColl)
            //{
            //    sqlCmd.Parameters.Add(objParam);
            //}
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Dispose();
            returnValue = true;
        }
        catch (Exception ex)
        {
            returnValue = false;
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

        }
        finally
        {
            if (objConn != null)
                if (objConn.State == ConnectionState.Open)
                    objConn.Close();
        }
        return returnValue;
    }


    public DataTable ExecuteSP(SqlCommand sqlCmd)
    {
        //SqlCommand sqlCmd;
        SqlDataAdapter sqlAdapter;
        DataTable tbl = new DataTable();

        try
        {
            GetConnection();
            sqlCmd.Connection = objConn;
            sqlCmd.CommandTimeout = int.MaxValue;
            // sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlAdapter = new SqlDataAdapter(sqlCmd);
            sqlAdapter.Fill(tbl);
            sqlCmd.Dispose();
        }
        catch (Exception ex)
        {
            logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw ex;
        }
        finally
        {
            if (objConn != null)
                if (objConn.State == ConnectionState.Open)
                    objConn.Close();
        }
        return tbl;
    }


}

