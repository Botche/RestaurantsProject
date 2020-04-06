$(document).ready(function () {

    $('input[data-val-required]').parent().children('label').addClass('required');
    $('textarea[data-val-required]').parent().children('label').addClass('required');
});