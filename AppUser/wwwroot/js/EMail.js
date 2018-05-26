
$("#UserEmail").focus(function (e) {
    userEmailVerification();
});

$("#UserEmail").blur(function (e) {
    userEmailVerification();
});

document.getElementById("UserEmail").addEventListener("input", userEmailVerification);
function userEmailVerification() {

    var emStatus = document.getElementById("emStatus"); 
    var emValue = document.getElementById('UserEmail').value;
    var input = document.createElement('input');
    input.type = 'email';
    input.value = emValue;

    if (emValue.length > 0) {
        if (input.checkValidity()) {
            emStatus.textContent = "E-Mail Approved!";
            emStatus.style.color = "green";
            document.getElementById("ConfirmEmail").disabled = false;
            document.getElementById("UserEmail").style.borderColor = "green";
        } else {
            emStatus.textContent = "E-Mail Needs Work!";
            emStatus.style.color = "red";
            document.getElementById("ConfirmEmail").disabled = true;
            document.getElementById("UserEmail").style.borderColor = "red";
        }
    }
    else {
        emStatus.textContent = "";
        document.getElementById("ConfirmEmail").disabled = true;
    }
    emConfirmation();
}
// end user email verification


// begin email confirmation
$("#ConfirmEmail").focus(function (e) {
    emConfirmation();
});

$("#ConfirmEmail").blur(function (e) {
    emConfirmation();
});

document.getElementById("ConfirmEmail").addEventListener("input", emConfirmation);
function emConfirmation() {
    var emValue = document.getElementById("UserEmail").value;
    var emConfirm = document.getElementById("ConfirmEmail").value;
    var emConfirmStatus = document.getElementById("emConfirmStatus");
    if (emConfirm.length > 0) {
        if (emValue === emConfirm) {
            emConfirmStatus.textContent = "E-Mail Confirmed!";
            emConfirmStatus.style.color = "green";
            document.getElementById("ConfirmEmail").style.borderColor = "green";
        }
        else {
            emConfirmStatus.textContent = "E-Mail Not Confirmed!";
            emConfirmStatus.style.color = "red";
            document.getElementById("ConfirmEmail").style.borderColor = "red";
        }
    } else {
        emConfirmStatus.textContent = "";
    }
}



