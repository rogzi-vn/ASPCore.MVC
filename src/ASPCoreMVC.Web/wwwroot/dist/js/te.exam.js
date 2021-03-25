// Không cho phép nhấn phím F5
function disableF5(e) {
    if ((e.which || e.keyCode) == 116) {
        e.preventDefault();
        showToast('warning', 'Please do not refresh the page while the test is in progress');
    }
};
$(document).on("keydown", disableF5);

$(".write-ans").each(function () {
    var id = $(this).attr('id');
    if (id) {
        var affectedId = $(this).data('affect-id');
        if (affectedId) {
            var editor = bindEditor(this, true);
            editor.on('change', function () {
                $(`#${affectedId}`).val(editor.getData());
            });
        }
    }
});

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
var countDown;
function stopExamCountDown() {
    if (countDown) {
        clearInterval(countDown);
    }
}
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

function initRecorderButton() {
    $("button[id^='start-recorder-']").click(function (event) {
        // Process for start button
        var e = $(event.currentTarget);
        var affectId = e.attr("id").replace("start-recorder-", "");
        e.addClass("d-none");

        // Process for finish button
        $(`#finish-recorder-${affectId}`).removeClass("d-none");

        // Show analyser
        $(`#viz-${affectId}`).children("#analyser").removeClass("d-none");
        // Hide sound wave
        $(`#wavedisplay-${affectId}`).addClass('d-none');

        // Hide audio player
        $(`#audio-player-${affectId}`).addClass('d-none');
        $(`#audio-player-${affectId}`).removeClass('green-audio-player');
        $(`#audio-player-${affectId}`).html("");

        // Proccess code
        // start recording
        if (!audioRecorder)
            return;
        audioRecorder.clear();
        audioRecorder.record();
    });

    $("button[id^='finish-recorder-']").click(function (event) {
        // Process for start button
        var e = $(event.currentTarget);
        var affectId = e.attr("id").replace("finish-recorder-", "");
        e.addClass("d-none");

        // Process for finish button
        $(`#start-recorder-${affectId}`).removeClass("d-none");

        // Hide analyser
        $(`#viz-${affectId}`).children("#analyser").addClass("d-none");
        // Show sound wave
        $(`#wavedisplay-${affectId}`).removeClass('d-none');
        // Show audio play  
        $(`#audio-player-${affectId}`).removeClass('d-none');
        $(`#audio-player-${affectId}`).addClass('my-3 mb-4');

        // Proccess code
        // stop recording
        audioRecorder.stop();
        audioRecorder.getBuffers(function (cb) {
            getBufferFor(cb, affectId);
        });
    });
}

var recorderFiles = [];

function getBufferFor(buffers, affectId) {
    var canvas = document.getElementById(`wavedisplay-${affectId}`);

    drawBuffer(canvas.width, canvas.height, canvas.getContext('2d'), buffers[0]);

    audioRecorder.exportWAV(function (blob) {
        createAudioForListenRecorderAgain(affectId, blob);
    });

    var file = new File(buffers, `user-recorder-[${affectId}].wav`);
    var index = recorderFiles.indexOf(file);
    if (index > -1) {
        recorderFiles.splice(index, 1);
    }
    recorderFiles.push(file);
}

function isRecorderExistFor(affectedId) {
    var e = $(`#${affectedId}`);
    var dataType = e.data("ans-type");
    var isTrue = false;
    recorderFiles.forEach(any => {
        if (dataType == "RecorderAnswer") {
            console.log(any);
            if (any.name == `user-recorder-[${affectedId}].wav`) {
                isTrue = true;
            }
        }
    });
    return isTrue;
}

function createAudioForListenRecorderAgain(affectedId, blob) {
    var resStore = $(`#${affectedId}`);
    var value = resStore.val();
    if (value) {
        // Remove old audio
        deleteOldFile(value, function () {
            postNewAudioFile(affectedId, blob);
        });
    } else {
        postNewAudioFile(affectedId, blob);
    }
}

function postNewAudioFile(affectedId, blob) {
    var f = new FormData();
    f.append("fname", `${affectedId}.wav`);
    f.append("audio", blob);
    var token = $('input[name="__RequestVerificationToken"]').val();
    fetch("/exams/dfdfdfadaddafdaf/audio", {
        method: 'POST',
        headers: {
            RequestVerificationToken: token
        },
        body: f
    }).then(r => r.json()).then(data => {
        if (data.success && data.data) {
            var uploadedPath = `/resources${data.data.path}`;
            $(`#${affectedId}`).val(uploadedPath);

            var audio = document.createElement("audio");

            // Allow us to control the audio
            audio.controls = "true";

            const url = uploadedPath;

            // Set the src and start loading the audio from the file
            audio.src = url;
            $(`#audio-player-${affectedId}`).html(audio);
            initAudioPlayer(`#audio-player-${affectedId}`);
        }
    }).catch(e => {
    });
}

function deleteOldFile(fileName, complete) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    fetch(decodeURI(`/api/app/app-file/owner-file-remove?fileName=` + encodeURI(fileName)), {
        method: 'DELETE',
        headers: {
            RequestVerificationToken: token,
            accept: '*/*',
        },
    })
        .then((response) => {
            complete();
        })
        .catch((error) => {
            complete();
        });
}

function endProcess(logId) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var jsonData = {
        logId: logId,
        answers: collectAnswers()
    };
    //console.log(jsonData);
    stopExamCountDown();
    return fetch(decodeURI(`/api/app/exam-log/result-processing`), {
        method: 'POST',
        headers: {
            RequestVerificationToken: token,
            accept: '*/*',
            "Content-Type": 'application/json',
        },
        body: JSON.stringify(jsonData)
    }).then((response) => {
        if (!response.ok) {
            var response = JSON.parse(res.responseText);
            showToast('error', response.error.message);
        }
        return response;
    });
}

$("#complete-exam-button").click(function () {
    var logId = $("#exam-log-id").val();
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        showLoaderOnConfirm: true,
        confirmButtonColor: '#1cc88a',
        cancelButtonColor: '#4e73df',
        confirmButtonText: 'Yes, complete exam!',
        preConfirm: () => {
            return endProcess(logId).catch((error) => {
                Swal.showValidationMessage(`Request failed: ${error}`);
            });
        },
    }).then((result) => {
        if (result.isConfirmed) {
            if (result.value.status >= 200 && result.value.status <= 299) {
                $("body").html("");
                window.location.href = `/exams/review/${logId}`;
            }
        }
    });
});

function collectAnswers() {
    var answers = [];
    $(".question-answered").each(function () {
        var answer = $(this).val();
        var questionId = $(this).data("q-id");
        var ans = {
            questionId,
            answer
        };
        answers.push(ans);
    });
    return answers;
}
