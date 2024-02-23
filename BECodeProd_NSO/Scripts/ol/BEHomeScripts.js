

function PressfloatOnly(evt, thisobj) {

    var charCode = (evt.which) ? evt.which : event.keyCode

    var textboxValue = thisobj.value + "";

    if (charCode == 17 || charCode == 67)
        return true;
    if (charCode == 17 || charCode == 86)
        return true;
    if (charCode == 17 || charCode == 88)
        return true;

    if (charCode == 190 || charCode == 110) {
        var contains = textboxValue.indexOf(".") != -1;
        if (contains)
            return false;
    }

    if (charCode == 37 || charCode == 39) return true;  // allow arrows

    if (charCode == 46) return true; //delete

    if (charCode == 190 || charCode == 110) return true; // period or dot


    if (charCode == 35 || charCode == 36) return true; // home, end 


    if (charCode == 8 || charCode == 9) return true; // backspace , tab


    //            var temp = parseFloat(textboxValue);
    //            temp = temp.toFixed(2);
    //            if (temp > 99999999999.99) {
    //                
    //            return false; }




    if (charCode > 47 && charCode < 58) return true; //0-9

    if (charCode > 95 && charCode < 106) return true; //0-9



    return false;
}



function PressReadOnly(evt, thisobj) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 37 || charCode == 39) return true;  // allow arrows 
    if (charCode == 35 || charCode == 36) return true; // home, end 
    if (charCode == 9) return true; // backspace , tab 

    return false;
}



function PressIntOnly(evt, thisobj) {

    var charCode = (evt.which) ? evt.which : event.keyCode

    var textboxValue = thisobj.value + "";

    if (charCode == 17 || charCode == 67)
        return true;
    if (charCode == 17 || charCode == 86)
        return true;
    if (charCode == 17 || charCode == 88)
        return true;


    if (charCode == 37 || charCode == 39) return true;  // allow arrows

    if (charCode == 46) return true; //delete

    if (charCode == 190 || charCode == 110) return false; // period or dot


    if (charCode == 35 || charCode == 36) return true; // home, end 


    if (charCode == 8 || charCode == 9) return true; // backspace , tab



    if (charCode > 47 && charCode < 58) return true; //0-9

    if (charCode > 95 && charCode < 106) return true; //0-9



    return false;
}