using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BEData.BusinessEntity
{
    public class UPdateRevenueBE
    {
        public int BEID { get; set; }
        public decimal DMfltMonth1BE { get; set; }
        public decimal @DMfltMonth2BE { get; set; }
        public decimal @DMfltMonth3BE { get; set; }
        //public decimal @DMfltNextQuarterBE { get; set; }

        //TODO:24/9 previous commented
        //public decimal @DMfltPrevQuarterBE { get; set; }
        
        public string Remarks { get; set; }
        public string SDMRem { get; set; }
        public decimal @SDMfltMonth1BE { get; set; }
        public decimal @SDMfltMonth2BE { get; set; }
        public decimal @SDMfltMonth3BE { get; set; }
        //public decimal @SDMfltNextQuarterBE { get; set; }

        //public decimal @ActualMonth1BE { get; set; }
        //public decimal @ActualMonth2BE { get; set; }
        //public decimal @ActualtMonth3BE { get; set; }


    }

    public class UPdateBEVolume
    {
        public string @txtUserId { get; set; }
        public string @Role { get; set; }
        public int @intBEId { get; set; }
        public decimal @fltDMEffortMonth1Onsite { get; set; }
        public decimal @fltDMEffortMonth1OffShore { get; set; }
        public decimal @fltDMEffortMonth2Onsite { get; set; }
        public decimal @fltDMEffortMonth2Offshore { get; set; }
        public decimal @fltDMEffortMonth3Onsite { get; set; }
        public decimal @fltDMEffortMonth3Offshore { get; set; }
        public decimal @fltSDMEffortMonth1Onsite { get; set; }
        public decimal @fltSDMEffortMonth1OffShore { get; set; }
        public decimal @fltSDMEffortMonth2Onsite { get; set; }
        public decimal @fltSDMEffortMonth2Offshore { get; set; }
        public decimal @fltSDMEffortMonth3Onsite { get; set; }
        public decimal @fltSDMEffortMonth3Offshore { get; set; }
        public string @DMRemarks { get; set; }
        public string @SDMRemarks { get; set; }





    }

    public class ApproveBEVolume
    {


        public int @intBEId { get; set; }
        public decimal @fltDMEffortMonth1Onsite { get; set; }
        public decimal @fltDMEffortMonth1OffShore { get; set; }
        public decimal @fltDMEffortMonth2Onsite { get; set; }
        public decimal @fltDMEffortMonth2Offshore { get; set; }
        public decimal @fltDMEffortMonth3Onsite { get; set; }
        public decimal @fltDMEffortMonth3Offshore { get; set; }
        public decimal @fltSDMEffortMonth1Onsite { get; set; }
        public decimal @fltSDMEffortMonth1OffShore { get; set; }
        public decimal @fltSDMEffortMonth2Onsite { get; set; }
        public decimal @fltSDMEffortMonth2Offshore { get; set; }
        public decimal @fltSDMEffortMonth3Onsite { get; set; }
        public decimal @fltSDMEffortMonth3Offshore { get; set; }







    }

    public class UpdateExcelData
    {
        public int BEID { get; set; }

        public string @txtUserId { get; set; }
        public string @Role { get; set; }

        public decimal @DMfltMonth1BE { get; set; }
        public decimal @DMfltMonth2BE { get; set; }
        public decimal @DMfltMonth3BE { get; set; }

        public decimal @SDMfltMonth1BE { get; set; }
        public decimal @SDMfltMonth2BE { get; set; }
        public decimal @SDMfltMonth3BE { get; set; }

        public decimal @fltDMEffortMonth1Onsite { get; set; }
        public decimal @fltDMEffortMonth1OffShore { get; set; }
        public decimal @fltDMEffortMonth2Onsite { get; set; }
        public decimal @fltDMEffortMonth2Offshore { get; set; }
        public decimal @fltDMEffortMonth3Onsite { get; set; }
        public decimal @fltDMEffortMonth3Offshore { get; set; }

        public decimal @fltSDMEffortMonth1Onsite { get; set; }
        public decimal @fltSDMEffortMonth1OffShore { get; set; }
        public decimal @fltSDMEffortMonth2Onsite { get; set; }
        public decimal @fltSDMEffortMonth2Offshore { get; set; }
        public decimal @fltSDMEffortMonth3Onsite { get; set; }
        public decimal @fltSDMEffortMonth3Offshore { get; set; }
    }
}