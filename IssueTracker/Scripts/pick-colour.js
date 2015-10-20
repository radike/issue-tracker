$(document).ready(function(){

    $("#pick-colour span").each(function () {

        $(this).click(function () {
            $("#pick-colour-input").val($(this).text());
        });

    });

});