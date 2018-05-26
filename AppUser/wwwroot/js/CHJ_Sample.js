
// begin - search input with "x" to clear (handles text, number, url)

$('.has-clear input[type="text"]').on('input propertychange', function () {
    var $this = $(this);
    var visible = Boolean($this.val());
    $this.siblings('.form-control-clear').toggleClass('hidden', !visible);
}).trigger('propertychange');
$('.has-clear input[type="number"]').on('input propertychange', function () {
    var $this = $(this);
    var visible = Boolean($this.val());
    $this.siblings('.form-control-clear').toggleClass('hidden', !visible);
}).trigger('propertychange');
$('.has-clear input[type="url"]').on('input propertychange', function () {
    var $this = $(this);
    var visible = Boolean($this.val());
    $this.siblings('.form-control-clear').toggleClass('hidden', !visible);
}).trigger('propertychange');
$('.has-clear input[type="date"]').on('input propertychange', function () {
    var $this = $(this);
    var visible = Boolean($this.val());
    $this.siblings('.form-control-clear').toggleClass('hidden', !visible);
}).trigger('propertychange');

$('.form-control-clear').click(function () {
    $(this).siblings('input[type="text"]').val('')
        .trigger('propertychange').focus();
});
$('.form-control-clear').click(function () {
    $(this).siblings('input[type="number"]').val('')
        .trigger('propertychange').focus();
});
$('.form-control-clear').click(function () {
    $(this).siblings('input[type="url"]').val('')
        .trigger('propertychange').focus();
});
$('.form-control-clear').click(function () {
    $(this).siblings('input[type="date"]').val('')
        .trigger('propertychange').focus();
});
$('.form-control-clear').hover(function () {
    var $this = $(this);
    $this.toggleClass('glyphicon-remove glyphicon-remove-sign');
});

// end - search input with "x" to clear


// begin disable copy and paste

$('.NoCopy').bind('copy', function (e) {
    e.preventDefault();
});
$('.NoPaste').bind('paste', function (e) {
    e.preventDefault();
});

// end disable copy and paste


/* Begin ToolTips */

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

/* End ToolTips */

/* Begin integer only input */
$('.integerOnly').on('input propertychange', function () {

    intValue = $(this).val().replace(/\D/g, '');
    $(this).val(intValue);

});
/* End integer only input */

/* begin show image not available when image not has error */
document.getElementById("imgProfileImage").addEventListener("error", imageNotAvailable);
function imageNotAvailable() {
    document.getElementById("imgProfileImage").style.display = 'none';
    document.getElementById("imgNotAvailable").style.display = 'inline';
}
/* end show image not available when image not has error */