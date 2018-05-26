
$('.PhoneNumber').on('input propertychange', function () {

    pnValue = $(this).val().replace(/\D/g, '');
    if (pnValue.length > 0) {
        pnValue = "(" + pnValue;

        if (pnValue.length > 3) {
            pnValue = pnValue.slice(0, 4) + ") " + pnValue.slice(4)

            if (pnValue.length > 8) {
                pnValue = pnValue.slice(0, 9) + "-" + pnValue.slice(9)

                if (pnValue.length > 14) {
                    pnValue = pnValue.slice(0, 14)
                }
            }
        }
    }

    $(this).val(pnValue);

});

// beg hide remove phone button / show verify remove phone button
$("#removePhone").click(function (e) {

    document.getElementById("newPhoneNumber").value = "";
    document.getElementById("newPhoneNumber").disabled = "disabled";
    document.getElementById("removePhone").classList.add("hidden");
    document.getElementById("phoneSave").classList.add("disabled");
    document.getElementById("verifyRemovePhone").classList.remove("hidden");

})
// end hide remove phone button / show verify remove phone button

