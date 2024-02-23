using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;


    public class GenericUtility
    {

        public static List<T> DataTableToList<T>(DataTable dt) where T : class, new()
        {
            List<T> lstItems = new List<T>();
            if (dt != null && dt.Rows.Count > 0)
                foreach (DataRow row in dt.Rows)
                    lstItems.Add(ConvertDataRowToGenericType<T>(row));
            else
                lstItems = null;
            return lstItems;
        }



        private static T ConvertDataRowToGenericType<T>(DataRow row) where T : class,new()
        {
            Type entityType = typeof(T);
            T objEntity = new T();
            foreach (DataColumn column in row.Table.Columns)
            {
                object value = row[column.ColumnName];
                if (value == DBNull.Value) value = null;
                PropertyInfo property = entityType.GetProperty(column.ColumnName, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);
                try
                {
                    if (property != null && property.CanWrite)
                        property.SetValue(objEntity, value, null);
                    else
                    {
                        property = entityType.GetProperties().Where(k => k.GetCustomAttributes(typeof(SQLInfoAttribute), true).Length > 0).SingleOrDefault(k => ((SQLInfoAttribute)k.GetCustomAttributes(typeof(SQLInfoAttribute), true)[0]).ColumnName == column.ColumnName);
                        if (property != null)
                            property.SetValue(objEntity, value, null);
                    }
                }
                catch (Exception ex)
                {
                    string exceptionMessageInfo_Names = string.Format("SQL Column Name:= {0} \n BusinessType Property Name:= {1}", column.ColumnName, property != null ? property.Name : string.Empty);
                    string exceptionMessageInfo_DataTypes = string.Format("SQL Column Type:= {0} \n BusinessType Property Type:= {1}", column.DataType, property != null ? property.PropertyType.Name : string.Empty);
                    throw new Exception("Error Message := " + ex.Message + Environment.NewLine + exceptionMessageInfo_Names +
                    Environment.NewLine + exceptionMessageInfo_DataTypes);
                }
            }
            return objEntity;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SQLInfoAttribute : Attribute
    {
        public string ColumnName;
        public SQLInfoAttribute(string columnName) { ColumnName = columnName; }

    }



