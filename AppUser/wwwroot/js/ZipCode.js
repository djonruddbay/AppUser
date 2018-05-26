
$('.ZipCode').on('input propertychange', function () {

    zcValue = $(this).val().replace(/\D/g, '');
    if (zcValue.length > 0) {

        if (zcValue.length > 5) {
            zcValue = zcValue.slice(0, 5) + "-" + zcValue.slice(5)
        }
        if (zcValue.length > 10) {
            zcValue = zcValue.slice(0, 10)
        }
    }

    $(this).val(zcValue);

});


