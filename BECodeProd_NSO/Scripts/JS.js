


function openWindow(type) {
    ClearSaveMessage();
    if (type == "MCOBEData") {
        window.open('MCOBEData.aspx');
    }
    else if (type == "RTBRData") {
        window.open('http://www.bing.com/');
    }
    else if (type == "Opportunity") {
        window.open('Opportunity.aspx');
    }
    else if (type == 'ReportBEBaseData') {
        window.open('ReportBEBaseData.aspx');
    }
    else if (type == 'BEReportData') {
        window.open('BEReportData.aspx');
    }
    else if (type == 'BEReportForINPIPE') {
        window.open('ReportForINPIPEUpdate.aspx');
    }
}
         
    