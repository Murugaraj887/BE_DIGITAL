using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using ExpenseAPP.Helper;


    [AttributeUsage(AttributeTargets.Property)]
    public class PropData : Attribute
    {
        public string data;
        public PropData(string Data) { data = Data; }

    }

    [Serializable]
    public class MasterEntity
    {

        public int intExpId { get; set; }

        public string ClientCode { get; set; }

        public string ItemName { get; set; }

        public double? NumberofItems { get; set; }

        public double UnitCost { get; set; }

        public string JustificationRemarks { get; set; }

        public string ProjOppCode { get; set; }

        public double BEUpside { get; set; }

        public double BEDownside { get; set; }

        public string CurrQtr { get; set; }
        public double FutQtrBE { get; set; }
        public string Fieldtxt1 { get; set; }
        public string Fieldtxt2 { get; set; }
        public string Fieldtxt3 { get; set; }
        public string Fieldtxt4 { get; set; }
        public string Fieldtxt5 { get; set; }
        public string Fieldtxt6 { get; set; }
        public string Fieldtxt7 { get; set; }
        public string Fieldtxt8 { get; set; }
        public string Fieldtxt9 { get; set; }
        public string Fieldtxt10 { get; set; }
        public string Fieldtxt11 { get; set; }
        public string Fieldtxt12 { get; set; }
        public string Fieldtxt13 { get; set; }
        public string Fieldtxt14 { get; set; }
        public string Fieldtxt15 { get; set; }
        public string Fieldtxt16 { get; set; }
        public string Fieldtxt17 { get; set; }
        public string Fieldtxt18 { get; set; }
        public string PUCode { get; set; }
        public string BUCode { get; set; }
        public string DUCode { get; set; }
        public string ExpType { get; set; }
        public string ExpCategory { get; set; }
        public string Priority { get; set; }
        public string IsCustomerRecoverable { get; set; }
        public string IsBudgetedinPBS { get; set; }
        public string Status { get; set; }
        public string FieldList1 { get; set; }
        public string FieldList2 { get; set; }
        public string FieldList3 { get; set; }
        public string FieldList4 { get; set; }
        public string FieldList5 { get; set; }
        public string FieldList6 { get; set; }
        public string FieldList7 { get; set; }
        public string FieldList8 { get; set; }
        public string FieldList9 { get; set; }
        public string FieldList10 { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public DateTime? FieldDate1 { get; set; }
        public DateTime? FieldDate2 { get; set; }
        public DateTime? FieldDate3 { get; set; }
        public DateTime? FieldDate4 { get; set; }
        public DateTime? FieldDate5 { get; set; }
        public DateTime? FieldDate6 { get; set; }
        public DateTime? FieldDate7 { get; set; }
        public DateTime? FieldDate8 { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string DMMailId { get; set; }

        public string pl { get; set; }



        // newly added columns 1
        public double TotalAmt { get; set; }



        // newly added columns 2
        public string SDMStatus { get; set; }
        public double SDMApprovedAmount { get; set; }
        public string DHStatus { get; set; }
        public double DHApprovedAmount { get; set; }
        public string PNAStatus { get; set; }
        public double PNAApprovedAmount { get; set; }




    }
