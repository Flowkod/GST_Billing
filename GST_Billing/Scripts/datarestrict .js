
    $(document).ready(function () {
        //Disable full page
        $("body").bind("contextmenu", function (e) {
            return false;
        });

    });
$(document).ready(function () {
    //Disable full page
    $('body').bind('select cut copy paste', function (e) {
        e.preventDefault();
    });
});
$(document).ready(function () {
    //Disable cut copy paste
    $('body').bind('select cut copy paste', function (e) {
        e.preventDefault();
    });

});