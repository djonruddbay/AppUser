
$('.AccountNumber').on('input propertychange', function () {
    anValue = $(this).val().replace(/\D/g, '');
    if (anValue.length > 0) {

        if (anValue.length > 4) {
            anValue = anValue.slice(0, 4) + "-" + anValue.slice(4)
        }

        if (anValue.length > 7) {
            anValue = anValue.slice(0, 7) + "-" + anValue.slice(7)
        }
        if (anValue.length > 15) {
            anValue = anValue.slice(0, 15)
        }
    }

    $(this).val(anValue);
});
