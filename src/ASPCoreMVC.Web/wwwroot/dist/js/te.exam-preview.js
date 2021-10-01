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

// Xử lý bổ xung khi câu hỏi của mathJax bị tràn
window.addEventListener('resize', function () {
    optimizeMathJaxAnswer();
});

function optimizeMathJaxAnswer() {
    // Lấy danh sách các khối câu hỏi
    $(".question-container").each(function () {
        const ansList = $(this).children(".row").children(".col-12");
        //console.log(`Số câu hỏi đếm được: ${ansList.length}`);
        if (ansList && ansList.length > 0) {
            // Đếm số cột của câu trả lời thuộc câu hỏi này
            let columnCount = 0;
            const matched = $(ansList[0]).attr("class").match(/col-lg-(\d+)/m)[1];
            if (matched && matched > 0) {
                columnCount = parseInt(matched);
            }
            //console.log(`Số cột: ${columnCount}`);

            // Kích thước của mỗi ô
            let maxColWidth = $(ansList[0]).width();
            //console.log(`Kích thước thực mỗi ô: ${maxColWidth}`);

            let isHaveMathJax = false;

            // Kích thước lớn nhất của câu trả lời
            let maxAnsWidth = 0;
            ansList.each(function () {
                var p = $(this);
                var mathFrame = p.children(".MathJax");
                //console.log(`Số lượng MathJax đếm được: ${mathFrame.length}`);
                if (mathFrame && mathFrame.length > 0) {
                    isHaveMathJax = true;
                    const crrWidth = $(mathFrame[0]).width();
                    if (crrWidth > maxAnsWidth) {
                        maxAnsWidth = crrWidth;
                    }
                }
            });

            //console.log(`Kích thước câu trả lời lớn nhất: ${maxAnsWidth}`);

            if (isHaveMathJax) {
                const overSizeRate = maxAnsWidth / maxColWidth;
                //console.log(`Tỉ lệ thay đổi kích thước: ${overSizeRate}`);
                if (overSizeRate > 1 && overSizeRate <= 2 && columnCount == 3) {
                    ansList.removeClass(`col-lg-${columnCount}`)
                        .addClass(`col-lg-${columnCount * 2}`);
                } else if (overSizeRate > 2 && columnCount < 12) {
                    ansList.removeClass(`col-lg-${columnCount}`)
                        .addClass(`col-lg-12`);
                }
            }
        }
    });
}

MathJax.Hub.Queue(function () {
    optimizeMathJaxAnswer();
});