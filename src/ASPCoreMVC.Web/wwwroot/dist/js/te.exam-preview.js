$(".load-artical-async").each(function () {
    var p = $(this);
    var qcId = p.data("qc-id");
    p.load(`/api/app/render-exam/render-artical/${qcId}`, function () {
        var count = 1;
        var startCount = p.data("start-count");
        if (startCount)
            count = startCount;
        var paragraph = p.html();
        paragraph = paragraph.replace(/{\$\$}/g, function () {
            return count++;
        });
        p.html(paragraph);
        // equation initialize
        MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
    });
});


function startExamCountDown(timeInMinutes, onEnd) {
    var timeInSeconds = timeInMinutes * 60;
    var timeInSecondsFixed = timeInSeconds;
    var stateElement = $("#line-counter");
    countDown = setInterval(function () {
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

