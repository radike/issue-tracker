$(function () {
    $('#SearchTitle').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/en-US/Home/AutoCompleteSearch',
                dataType: "json",
                contentType: 'application/json, charset=utf-8',
                data: {
                    query: $("#SearchTitle").val()
                },
                success: function (data) {
                    response(data);
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        },
        select: function (event, ui) {
            window.location = "/en-US/Issues/Details/" + ui.item.Code;
        },
        minLength: 2
    }).data("ui-autocomplete")._renderItem = function (ul, item) {
        return $("<li></li>").data("ui-autocomplete-item", item)
            .append("<a href='/en-US/Issues/Details/" + item.Code + "'>" + item.Code + ":" + item.Title + "</a>")
            .appendTo(ul);
    };
});