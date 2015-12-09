$(document).ready(function () {
    $('#lang-select a').click(function () {
        $('#lang-select div').fadeToggle(200);
    });
});

$(document).click(function (event) {
    if (!$(event.target).closest('#lang-select a').length) {
        if ($("#lang-select div").is(":visible"))
            $('#lang-select div').fadeToggle(200);
    }
});