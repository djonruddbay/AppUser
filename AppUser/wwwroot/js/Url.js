
//remove all spaces from url
$('.Url').on('input propertychange', function () {

    urlValue = $(this).val().replace(/\s/g, '');
    $(this).val(urlValue);
});
