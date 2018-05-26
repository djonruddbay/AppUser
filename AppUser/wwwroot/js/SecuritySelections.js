
//begin Change Color and Font When Security Selections Are Made
$("#securityColorAnswer").click(function (e) {
    var colorSelected = document.getElementById("securityColorAnswer").value;
    if (colorSelected != "") {
        document.getElementById("securityColorAnswer").style.color = colorSelected;
        document.getElementById("securityColorAnswer").style.fontWeight = 600;
    }
})

$("#securitySymbolAnswer").click(function (e) {
    var symbolSelected = document.getElementById("securitySymbolAnswer").value;
    if (symbolSelected != "") {
        document.getElementById("securitySymbolAnswer").style.color = "black";
        document.getElementById("securitySymbolAnswer").style.fontWeight = 600;
    }
})
//end Change Color and Font When Security Selections Are Made

// begin Keep selected security selections selected during page verification
$(document).ready(function () {
    var color = document.getElementById("selectedSecurityColor").value;
    var symbol = document.getElementById("selectedSecuritySymbol").value;

    if (color != "") {
        $('[name=SecurityColorAnswer]').val(color);
        var colorSelected = document.getElementById("securityColorAnswer").value;
        document.getElementById("securityColorAnswer").style.color = colorSelected
    }

    if (symbol != "") {
        $('[name=SecuritySymbolAnswer]').val(symbol);
    }
});
// end Keep selected security selections selected during page verification




