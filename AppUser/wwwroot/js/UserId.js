

$("#UserName").focus(function (e) {
    jQuery('#unCheckList').show();
    jQuery('#unRequirements').slideDown();
    usernameVerification();
});
$("#UserName").blur(function (e) {
    jQuery('#unRequirements').slideUp();
    jQuery('#unCheckList').hide();
    usernameVerification();
});
document.getElementById("UserName").addEventListener("input", usernameVerification);
function usernameVerification() {

    var unStatus = document.getElementById("unStatus");
    var unValue = document.getElementById("UserName").value;
    var unCharactersOk = document.getElementById("unCharactersOk");
    var unLengthOk = document.getElementById("unLengthOk")
    var unLength = unValue.length;

    if (unLength > 0) {
        if (unValue.match(/[^A-Za-z\d]/)) {
            unCharactersOk.classList.remove("glyphicon-check");
            unCharactersOk.classList.add("glyphicon-remove-sign");
            unCharactersOk.style.color = "red";
            unLengthOk.classList.remove("glyphicon-check");
            unLengthOk.classList.add("glyphicon-remove-sign");
            unLengthOk.style.color = "red";
            unStatus.textContent = "User Name Needs Work!";
            unStatus.style.color = "red";
        }
        else {
            unCharactersOk.classList.remove("glyphicon-remove-sign");
            unCharactersOk.classList.add("glyphicon-check");
            unCharactersOk.style.color = "green";
            if (unLength >= 5 && unLength <= 20) {
                unLengthOk.classList.remove("glyphicon-remove-sign");
                unLengthOk.classList.add("glyphicon-check");
                unLengthOk.style.color = "green";
                unStatus.textContent = "User Name Approved!";
                unStatus.style.color = "green";
                document.getElementById("UserName").style.borderColor = "green";
            }
            else {
                unLengthOk.classList.remove("glyphicon-check");
                unLengthOk.classList.add("glyphicon-remove-sign");
                unLengthOk.style.color = "red";
                unStatus.textContent = "User Name Needs Work!";
                unStatus.style.color = "red";
                document.getElementById("UserName").style.borderColor = "red";
            }
        }
    }
    else {
        unCharactersOk.classList.remove("glyphicon-check");
        unCharactersOk.classList.add("glyphicon-remove-sign");
        unCharactersOk.style.color = "red";
        unLengthOk.classList.remove("glyphicon-check");
        unLengthOk.classList.add("glyphicon-remove-sign");
        unLengthOk.style.color = "red";
        unStatus.textContent = "";
    }
}
