using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



    public class ExpenseTemplateData
    {
        public string ExpTemplateId { get; set; }
        public string ColumnName { get; set; }
        public string DisplayText { get; set; }
        public string ColumnType { get; set; }
        public bool IsMandatory { get; set; }
        public string ListValues { get; set; }
        public string spName { get; set; }
        public string updatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int? ColumnPosition { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int Width { get; set; }
        public bool IsEditable { get; set; }
        public string Allignment { get; set; }
        public string SU { get; set; }
    }


    public class PUDM
    {
        [SQLInfo("txtpu")]
        public string PU { get; set; }
        [SQLInfo("txtdm")]
        public string DM { get; set; }
    }

