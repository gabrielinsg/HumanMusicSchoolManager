$(document).ready(function () {
    $(".folders").click(function () {
        var verifica = $(this).attr("aria-expanded");
        if (verifica === "true") {
            $(".folders i").removeClass("fa-folder-open");
            $(".folders i").addClass("fa-folder");
        }
        else
        {
            $(".folders i").removeClass("fa-folder-open");
            $(".folders i").addClass("fa-folder");
            $(this).children("i").removeClass("fa-folder");
            $(this).children("i").addClass("fa-folder-open");
        }
    });

    $(document).click(function () {
        $(".folders i").removeClass("fa-folder-open");
        $(".folders i").addClass("fa-folder");
    });


});