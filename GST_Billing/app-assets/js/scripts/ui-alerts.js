/*
 * UI - Alerts
 */

$(".card-alert .close").click(function () {
    $(this)
        .closest(".card-alert")
        .fadeOut("slow");
});

function setAlert(msg) {
    setTimeout(function () {
        $("body").append('<div class="card-alert card gradient-45deg-green-teal"><div class="card-content white-text"><p><i class="material-icons">check</i> SUCCESS : <span id="TopSuccessAlert">' + msg +'</span> </p></div><button type="button" class="close white-text" data-dismiss="alert" aria-label="Close"><span aria-hidden="true"><i class="material-icons">clear</i></span></button></div>')
    }, 3000);
}
