// begin toggle password visibility

$('.Password input[type="password"]').on('input propertychange', function () {
    var $this = $(this);
    var visible = Boolean($this.val());
    $this.siblings('.form-control-clear').toggleClass('hidden', !visible);
}).trigger('propertychange');

$('#Password + .glyphicon').hover(
    function () {
        var $this = $(this);
        $this.toggleClass('glyphicon-remove-sign glyphicon-eye-open');
        document.getElementById("Password").type = "text";
    }, function () {
        var $this = $(this);
        $this.toggleClass('glyphicon-eye-open glyphicon-remove-sign');
        document.getElementById("Password").type = "password";
    }
);
$('#ConfirmPassword + .glyphicon').hover(
    function () {
        var $this = $(this);
        $this.toggleClass('glyphicon-remove-sign glyphicon-eye-open');
        document.getElementById("ConfirmPassword").type = "text";
    }, function () {
        var $this = $(this);
        $this.toggleClass('glyphicon-eye-open glyphicon-remove-sign');
        document.getElementById("ConfirmPassword").type = "password";
    }
);
$('#OldPassword + .glyphicon').hover(
    function () {
        var $this = $(this);
        $this.toggleClass('glyphicon-remove-sign glyphicon-eye-open');
        document.getElementById("OldPassword").type = "text";
    }, function () {
        var $this = $(this);
        $this.toggleClass('glyphicon-eye-open glyphicon-remove-sign');
        document.getElementById("OldPassword").type = "password";
    }
);
// end toggle password visibility


// begin check requirements

$("#Password").focus(function (e) {
    jQuery('#pwCheckList').show();
    jQuery('#pwRequirements').slideDown();
    passwordVerification();
});
$("#Password").blur(function (e) {
    jQuery('#pwRequirements').slideUp();
    jQuery('#pwCheckList').hide();
    passwordVerification();
});

document.getElementById("Password").addEventListener("input", passwordVerification);
function passwordVerification() {

    var pwErrorCount = 0;
    var pwValue = document.getElementById("Password").value;
    var pwOriginalValue = pwValue;
    var badInputCount = pwValue.length;
    pwValue = pwValue.replace(/@/g, "");
    pwValue = pwValue.replace(/\[/g, "");
    pwValue = pwValue.replace(/]/g, "");
    pwValue = pwValue.replace(/\\/g, "");
    pwValue = pwValue.replace(/\//g, "");
    pwValue = pwValue.replace(/:/g, "");
    pwValue = pwValue.replace(/;/g, "");
    pwValue = pwValue.replace(/|/g, "");
    badInputCount = badInputCount - pwValue.length;

    // begin check bad characters
    var pwCharactersOk = document.getElementById("pwCharactersOk");
    if (badInputCount == 0) {
        pwCharactersOk.classList.remove("glyphicon-remove-sign");
        pwCharactersOk.classList.add("glyphicon-check");
        pwCharactersOk.style.color = "green";
    }
    else {
        pwCharactersOk.classList.remove("glyphicon-check");
        pwCharactersOk.classList.add("glyphicon-remove-sign");
        pwCharactersOk.style.color = "red";
        pwErrorCount++;
    }
    // end check bad characters

    // begin check password length
    var pwLengthOk = document.getElementById("pwLengthOk");
    var badInputCount = pwValue.length;
    pwValue = pwValue.replace(/\s/g, "");
    badInputCount = badInputCount - pwValue.length;

    if (badInputCount == 0 && pwErrorCount == 0 && pwValue.length >= 8) {
        pwLengthOk.classList.remove("glyphicon-remove-sign");
        pwLengthOk.classList.add("glyphicon-check");
        pwLengthOk.style.color = "green";
    }
    else {
        pwLengthOk.classList.remove("glyphicon-check");
        pwLengthOk.classList.add("glyphicon-remove-sign");
        pwLengthOk.style.color = "red";
        pwErrorCount++;
    }
    // end check password length

    // begin check uppercase
    var pwUpperOk = document.getElementById("pwUpperOk");
    if (pwValue.match(/[A-Z]/)) {
        pwUpperOk.classList.remove("glyphicon-remove-sign");
        pwUpperOk.classList.add("glyphicon-check");
        pwUpperOk.style.color = "green";
    }
    else {
        pwUpperOk.classList.remove("glyphicon-check");
        pwUpperOk.classList.add("glyphicon-remove-sign");
        pwUpperOk.style.color = "red";
        pwErrorCount++;
    }
    // end check lowercase

    //begin check lowercase
    var pwLowerOk = document.getElementById("pwLowerOk");
    if (pwValue.match(/[a-z]/)) {
        pwLowerOk.classList.remove("glyphicon-remove-sign");
        pwLowerOk.classList.add("glyphicon-check");
        pwLowerOk.style.color = "green";
    }
    else {
        pwLowerOk.classList.remove("glyphicon-check");
        pwLowerOk.classList.add("glyphicon-remove-sign");
        pwLowerOk.style.color = "red";
        pwErrorCount++;
    }
    // end check lowercase

    // begin check for digit
    var pwDigitOk = document.getElementById("pwDigitOk");
    if (pwValue.match(/\d/)) {
        pwDigitOk.classList.remove("glyphicon-remove-sign");
        pwDigitOk.classList.add("glyphicon-check");
        pwDigitOk.style.color = "green";
    } else {
        pwDigitOk.classList.remove("glyphicon-check");
        pwDigitOk.classList.add("glyphicon-remove-sign");
        pwDigitOk.style.color = "red";
        pwErrorCount++;
    }
    // end check for digit

    //begin check special
    var pwSpecialOk = document.getElementById("pwSpecialOk");
    if (pwValue.match(/[^A-Za-z\d]/)) {
        pwSpecialOk.classList.remove("glyphicon-remove-sign");
        pwSpecialOk.classList.add("glyphicon-check");
        pwSpecialOk.style.color = "green";
    }
    else {
        pwSpecialOk.classList.remove("glyphicon-check");
        pwSpecialOk.classList.add("glyphicon-remove-sign");
        pwSpecialOk.style.color = "red";
        pwErrorCount++;
    }
    // end check special

    // begin error check
    var pwStatus = document.getElementById("pwStatus");

    if (pwOriginalValue.length > 0) {
        if (pwErrorCount == 0) {
            pwStatus.textContent = "Password Approved!";
            pwStatus.style.color = "green";
            document.getElementById("ConfirmPassword").disabled = false;
            document.getElementById("Password").style.borderColor = "green";
        }
        else {
            pwStatus.textContent = "Password Needs Work!";
            pwStatus.style.color = "red";
            document.getElementById("ConfirmPassword").disabled = true;
            document.getElementById("Password").style.borderColor = "red";
        }
    }
    else {
        pwStatus.textContent = "";
        document.getElementById("ConfirmPassword").disabled = true;
    }
    passwordConfirmation();
    // end error check
}
// end password requirements

// begin password confirmation
$("#ConfirmPassword").focus(function (e) {
    passwordConfirmation();
});

$("#ConfirmPassword").blur(function (e) {
    passwordConfirmation();
});

document.getElementById("ConfirmPassword").addEventListener("input", passwordConfirmation);
function passwordConfirmation() {
    var pwValue = document.getElementById("Password").value;
    var pwConfirm = document.getElementById("ConfirmPassword").value;
    var pwConfirmStatus = document.getElementById("pwConfirmStatus");
    if (pwValue.length > 0 && pwConfirm.length > 0) {
        if (pwValue === pwConfirm) {
            pwConfirmStatus.textContent = "Password Confirmed!";
            pwConfirmStatus.style.color = "green";
            document.getElementById("ConfirmPassword").style.borderColor = "green";
        }
        else {
            pwConfirmStatus.textContent = "Password Not Confirmed!";
            document.getElementById("ConfirmPassword").style.borderColor = "red";
            pwConfirmStatus.style.color = "red";
        }
    } else {
        pwConfirmStatus.textContent = "";
    }
}

// end password confirmation