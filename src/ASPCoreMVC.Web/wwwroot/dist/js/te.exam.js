$(document).ready(() => {
    $(".replace-count").each(function () {
        var count = 1;
        var startCount = $(this).data("start-count");
        if (startCount)
            count = startCount;
        var paragraph = $(this).html();
        paragraph = paragraph.replace(/{\$\$}/g, function () {
            return count++;
        });
        $(this).html(paragraph);
    });
});

function circleAlpha(element) {
    var affectId = element.data("affect-id");
    $(`div[data-affect-id="${affectId}"]`)
        .children(".c-able")
        .removeClass("rounded-circle border border-dark px-2");
    element.children(".c-able").addClass("rounded-circle border border-dark px-2");

    $(`#${affectId}`).val(element.attr('id'));
}

$(".text-ans").click(event => {
    var p = $(event.currentTarget);
    circleAlpha(p);
});
$(".img-ans").click(event => {
    var p = $(event.currentTarget);
    circleAlpha(p);
});
$(".fill-ans").change(function (event) {
    var p = $(event.currentTarget);
    var affectId = p.data("affect-id");
    $(`#${affectId}`).val(p.val());
});

// Count down
function startExamCountDown(timeInMinutes, onEnd) {
    var timeInSeconds = timeInMinutes * 60;
    var timeInSecondsFixed = timeInSeconds;
    var stateElement = $("#line-counter");
    var countDown = setInterval(function () {
        timeInSeconds = timeInSeconds - 1;
        if (timeInSeconds < 0) {
            clearInterval(countDown);
            if (onEnd) {
                onEnd();
            }
            return;
        }
        $("#counter-content").html(secondsExtract(timeInSeconds));

        if (timeInSeconds < timeInSecondsFixed * 0.3) {
            if (stateElement.hasClass("border-top-exam-success"))
                stateElement.removeClass("border-top-exam-success");

            if (stateElement.hasClass("border-top-exam-warning"))
                stateElement.removeClass("border-top-exam-warning");

            if (!stateElement.hasClass("border-top-exam-danger"))
                stateElement.addClass("border-top-exam-danger");
        } else if (timeInSeconds < timeInSecondsFixed * 0.6) {
            if (stateElement.hasClass("border-top-exam-success"))
                stateElement.removeClass("border-top-exam-success");

            if (!stateElement.hasClass("border-top-exam-warning"))
                stateElement.addClass("border-top-exam-warning");

        }
    }, 1000);
}