function loadUsers() {
    var projectId = $('select#ddlProject').val();
    var loadProjectUsersUrl = $('#LoadProjectUsersUrl').val();
    $.ajax({
        url: loadProjectUsersUrl,
        type: 'POST',
        data: JSON.stringify({ id: projectId }),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            $('select#ddlAssignee').html("");
            $('select#ddlAssignee').append("<option value=''></option>");
            $.each(data, function (key, Users) {
                $('select#ddlAssignee').append(
                    '<option value="' + Users.Id + '">'
                    + Users.Email +
                    '</option>');
            });
        },
    });
}
$(function () {
    $('select#ddlProject').change(function () {
        loadUsers();
    });
    $(document).ready(function () {
        if ($('select#ddlAssignee').val() == "") {
            loadUsers();
        }
    });
});