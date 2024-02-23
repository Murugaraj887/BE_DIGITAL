function showOppResult() {
    var item = document.getElementsByName("divOppResult");
    if (item) {
        item.style.display = "block";
    }
}

function hideOppResult() {
    var item = document.getElementsByName("divOppResult");
    if (item) {
        item.style.display = "none";
    }
}

function showProjSearchResult() {
    var item = document.getElementsByName("divProjResult");
    if (item) {
        item.style.display = "block";
    }
}

function hideProjSearchResult() {
    var item = document.getElementsByName("divProjResult");
    if (item) {
        item.style.display = "none";
    }
}


function isInteger(s) {
    var i;
    for (i = 0; i < s.length; i++) {
        // Check that current character is number.
        var c = s.charAt(i);
        if (c == ".")
            continue;
        if (((c < "0") || (c > "9")))
            return false;
    }
    return true;
}

function normal(id) {
    var button = document.getElementById(id);
   button.style.cssText = 'cursor: pointer; cursor: hand; BORDER-RIGHT-WIDTH: 0px; BACKGROUND-COLOR: #f8df9c;   BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px;    HEIGHT: 20px; COLOR: #c41502; BORDER-LEFT-WIDTH: 0px; FONT-WEIGHT: bold'
   
}
function over(id) {
    var button = document.getElementById(id);
    button.style.cssText = 'cursor: pointer; cursor: hand; BORDER-RIGHT-WIDTH: 0px; BACKGROUND-COLOR: #f8df9c;   BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px;    HEIGHT: 20px; COLOR: #c41502; BORDER-LEFT-WIDTH: 0px; FONT-WEIGHT: bold'
    
}

function overSelected(id) {
    var button = document.getElementById(id);
    button.style.cssText = 'cursor: pointer; cursor: text; BORDER-RIGHT-WIDTH: 0px; BACKGROUND-COLOR: #C41502;    BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px;    HEIGHT: 20px; COLOR: White; BORDER-LEFT-WIDTH: 0px; FONT-WEIGHT: bold'
}
function openWindow(type) {
    if (type == "Quarter") {
        window.open('OpportunityList.aspx?Type=Quarter');
    }
    else if (type == "Supply") {
        window.open('http://vchnpnacmex-01/SupplyStatusTool/Forms/Home.aspx');
    }
}