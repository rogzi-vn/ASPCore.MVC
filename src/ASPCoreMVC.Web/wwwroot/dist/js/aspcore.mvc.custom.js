var audio = new Audio('/dist/audios/light-notify.mp3');
/* ==================================== */
function showToast(type, message, noSound = false) {
    new Noty({
        theme: 'nest',
        timeout: 3000,
        layout: 'bottomRight',
        text: message,
        type: type,
    }).show();
    if (!noSound) audio.play();
}
/* ==================================== */

/* ==================================== */
function generateDisplayName(firstName = '', lastName = '') {
    firstName = firstName
        .split(' ')
        .filter((x) => x != null && x.length > 0)
        .map((x) => x.charAt(0).toUpperCase() + x.slice(1))
        .join(' ');
    lastName = lastName
        .split(' ')
        .filter((x) => x != null && x.length > 0)
        .map((x) => x.charAt(0).toUpperCase() + x.slice(1))
        .join(' ');
    var displayNames = [
        `${firstName}`.trim(),
        `${firstName} ${lastName}`.trim(),
        `${lastName} ${firstName}`.trim(),
        `${lastName}`.trim(),
    ].filter((x) => x != null && x.length > 0);
    return [...new Set(displayNames)];
}
/* ==================================== */

/* ==================================== */
function getExtension(filename) {
    var parts = filename.split('.');
    return parts[parts.length - 1];
}
function isImage(filename) {
    var ext = getExtension(filename);
    switch (ext.toLowerCase()) {
        case 'jpg':
        case 'gif':
        case 'bmp':
        case 'png':
            //etc
            return true;
    }
    return false;
}

function isAudio(filename) {
    var ext = getExtension(filename);
    switch (ext.toLowerCase()) {
        case 'mp3':
        case 'mp4':
        case 'aif':
            //etc
            return true;
    }
    return false;
}

function isVideo(filename) {
    var ext = getExtension(filename);
    switch (ext.toLowerCase()) {
        case 'm4v':
        case 'avi':
        case 'mpg':
        case 'mp4':
            // etc
            return true;
    }
    return false;
}
/* ==================================== */

$('.datepicker')
    .datepicker({
        autoclose: true,
        todayHighlight: true,
    })
    .datepicker('update', new Date());

$('.db-table').DataTable({
    columnDefs: [
        {
            targets: 'no-sort',
            orderable: false,
        },
    ],
});

/* ================ CK EDITOR ==================== */
function bindEditor(e, isUserEditor = false) {
    var fileBrower = "";
    if (isUserEditor) {
        fileBrower = '/ckeditor-browser-user';
    } else {
        fileBrower = '/ckeditor-browser';
    }
    var height = 300;
    var isMint = false;
    try {
        height = parseInt($(e).attr('editor-height').valueOf());
    } catch (ex) { }
    try {
        isMint = $(e).attr('editor-mint').valueOf();
    } catch (ex) { }
    var _conf = {
        height: height,
        extraPlugins: [
            'videoembed',
            'confighelper',
            'image2',
            'tableresize',
            'table',
            'preview',
            'imageresize',
        ],
        filebrowserBrowseUrl: fileBrower,
    };
    if (isMint) {
        _conf.toolbar = [
            { name: 'basicstyles', items: ['Bold', 'Italic'] },
            { name: 'paragraph', items: ['NumberedList', 'BulletedList'] },
            { name: 'tools', items: ['Maximize', '-', 'About'] },
        ];
    }
    return CKEDITOR.replace(e, _conf);
}
$('.editor').each(function () {
    bindEditor(this);
});

$('.editor-auto-value').each(function () {
    var ck = bindEditor(this);
    var textArea = $(this);
    ck.on('change', function () {
        textArea.val(ck.getData());
    });
});

function bindCKEditor(className, isUserEditor = false) {
    $(`.${className}`).each(function () {
        var thisId = $(this).attr('id').valueOf();
        if (thisId) {
            var editor = bindEditor(this, isUserEditor);
            editor.on('change', function () {
                $(`#${thisId}`).val(editor.getData());
            });
        }
    });
}

// https://gasparesganga.com/labs/jquery-loading-overlay/
/* ================ LOADING OVERLAY ==================== */
$.LoadingOverlaySetup({
    background: '#00000030',
    image: '/dist/img/loading.svg',
    imageAnimation: '3s rotate_right',
    imageColor: '#e74c3c',
    maxSize: 40,
});

$(document).ajaxSend(function (event, jqxhr, settings) {
    if (
        settings.url != "/alert-center/sync" &&
        settings.url != "/message-center/sync"
    ) {
        $.LoadingOverlay('show');
    }
});
$(document).ajaxComplete(function (event, jqxhr, settings) {
    $.LoadingOverlay('hide');
});

const nativeFetch = window.fetch;
window.fetch = function (...args) {
    var x = nativeFetch.apply(window, args);
    if (
        args[0].indexOf("/api/app/notification/count-unread-notification") < 0 &&
        args[0].indexOf("/api/app/user-message/count-unread-message") < 0 &&
        args[0].indexOf("/alert-center/sync") < 0 &&
        args[0].indexOf("/message-center/sync") < 0 &&
        args[0].indexOf(location.origin + "/notification-hub") < 0 &&
        args[0].indexOf("/render-message") < 0 &&
        args[0].indexOf("/api/app/user-message") < 0
    ) {
        $.LoadingOverlay('show');
        x.then(function () {
            $.LoadingOverlay('hide');
        }).catch(function () {
            $.LoadingOverlay('hide');
        });
    }
    return x;
}

$(".zzz").click(event => {
    $.LoadingOverlay('show');
});

$(window).bind('beforeunload', function () {
    $.LoadingOverlay('show');
});

/* ============== SWEET ALERT 2 ============ */
function deleteConfirm(
    deleteUrl,
    onSuccess,
    deletedMsg = 'Your record has been deleted.'
) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        showLoaderOnConfirm: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!',
        preConfirm: () => {
            var token = $('input[name="__RequestVerificationToken"]').val();
            return fetch(decodeURI(deleteUrl), {
                method: 'DELETE',
                headers: {
                    RequestVerificationToken: token,
                    accept: '*/*',
                },
            })
                .then((response) => {
                    if (!response.ok) {
                        var response = JSON.parse(res.responseText);
                        showToast('error', response.error.message);
                    }
                    return response;
                })
                .catch((error) => {
                    Swal.showValidationMessage(`Request failed: ${error}`);
                });
        },
    }).then((result) => {
        if (result.isConfirmed) {
            onSuccess(result);
            Swal.fire('Deleted!', deletedMsg, 'success');
        }
    });
}

function putConfirm(
    confirmUrl,
    onSuccess,
    confirmedMsg = 'Your record state has been changed.',
    textMsg = 'Your record state will be change.',
) {
    Swal.fire({
        title: 'Are you sure?',
        text: textMsg,
        icon: 'warning',
        showCancelButton: true,
        showLoaderOnConfirm: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, change it!',
        preConfirm: () => {
            var token = $('input[name="__RequestVerificationToken"]').val();
            return fetch(decodeURI(confirmUrl), {
                method: 'PUT',
                headers: {
                    RequestVerificationToken: token,
                    accept: '*/*',
                },
            })
                .then((response) => {
                    if (!response.ok) {
                        var response = JSON.parse(res.responseText);
                        showToast('error', response.error.message);
                    }
                    return response;
                })
                .catch((error) => {
                    Swal.showValidationMessage(`Request failed: ${error}`);
                });
        },
    }).then((result) => {
        if (result.isConfirmed) {
            onSuccess(result);
            Swal.fire('State changed!', confirmedMsg, 'success');
        }
    });
}
/* ============== SWEET ALERT 2 ============ */

function pd(event) {
    event.preventDefault();
    event.stopPropagation();
}

$(".clickable-row").click(function () {
    window.location = $(this).data("href");
});

function startExam(path) {
    if (path) {
        window.location = path;
    }
    $.LoadingOverlay('show');
}

function loadExamTipsHelper(event, type, id, testable = false) {
    pd(event);
    $("#raw-empty-modal-content")
        .load(`/exam/helper/tips/${type}?id=${id}&testable=${testable}`, function () {
            $("#raw-empty-modal").modal('show');
        });
}

function showUserManual(event, _title, _contentUrl, confirmText = "OK") {
    pd(event);
    $.ajax({
        url: _contentUrl, success: function (data) {
            Swal.fire({
                title: '<strong>' + _title + '</strong>',
                width: '500px',
                html: data,
                confirmButtonText: confirmText
            });
        }
    });
}

$(function () {
    $('[data-toggle="tooltip"]').tooltip()
});


//======================= RUN VIDEO/AUDIO ====================//
function injectSource(id, uri, isLoop = false) {
    //var mimeType = mime.getType(uri);

    var video = document.getElementById(id);
    video.src = uri;
    video.loop = isLoop;
    return video;
}
//======================================================//
$(".prevent-default").each(function () {
    $(this).click(function (event) {
        event.preventDefault();
        event.stopPropagation();
    });
});
//======================================================//

function drawTagCloud(selector, texts = []) {
    $(selector).html("");
    var fill = d3.scale.category20();

    var w = $(selector).width();

    var layout = d3.layout.cloud()
        .size([w, w])
        .words(texts.map(function (d) {
            return { text: d, size: 10 + Math.random() * 90 };
        }))
        .padding(5)
        .rotate(function () { return ~~(Math.random() * 2) * 90; })
        .font("Impact")
        .fontSize(function (d) { return d.size; })
        .on("end", draw);

    layout.start();

    function draw(words) {
        d3.select(selector).append("svg")
            .attr("width", layout.size()[0])
            .attr("height", layout.size()[1])
            .append("g")
            .attr("transform", "translate(" + layout.size()[0] / 2 + "," + layout.size()[1] / 2 + ")")
            .selectAll("text")
            .data(words)
            .enter().append("text")
            .style("font-size", function (d) { return d.size + "px"; })
            .style("font-family", "Impact")
            .style("fill", function (d, i) { return fill(i); })
            .attr("text-anchor", "middle")
            .attr("transform", function (d) {
                return "translate(" + [d.x, d.y] + ")rotate(" + d.rotate + ")";
            })
            .text(function (d) { return d.text; });
    }
}

function onSreenSizeChange(changeCallback) {
    $(window).on('resize', function () {
        var win = $(this); //this = window
        if (win.height() < 600) { changeCallback(); }
        if (win.height() >= 600) { changeCallback(); }
        if (win.height() >= 768) { changeCallback(); }
        if (win.height() >= 992) { changeCallback(); }
        if (win.width() >= 1280) { changeCallback(); }
    });
}

function playAudio(url) {
    new Audio(url).play();
}

function makeSameSize() {
    setTimeout(() => {
        var maxWidth = 0;
        $('.same-size').each(function () {
            if ($(this).width() > maxWidth) {
                maxWidth = $(this).width();
            }
        });
        $(".same-size").width(maxWidth);
    }, 200);
}

function fetchTreeJs(url, element, f, selectedNotes = [], disables = []) {
    fetch(url)
        .then(r => r.json())
        .then(r => {
            initTreeJs(element, r, f, selectedNotes, disables);
        });
}
function initTreeJs(element, data, f, selectedNotes = [], disables = []) {
    let tree = new Tree(element, {
        // root data
        data: data,
        loaded: function () {
            if (selectedNotes) {
                var cloneSN = [...selectedNotes];
                cloneSN.forEach(item => {
                    var haveChild = false;
                    selectedNotes.forEach(i => {
                        if (i.indexOf(item) === 0 && i.length > item.length) {
                            haveChild = true;
                        }
                    });
                    if (haveChild)
                        selectedNotes = selectedNotes.filter(x => x != item);
                });
            }
            // pre-selected nodes
            this.values = selectedNotes;
            this.disables = disables;
        },
        onChange: function () {
            f(this.selectedNodes, this.values);
        }

    });
}
// Init audio player
function initAudioPlayer(s = '.audio-player') {
    try {
        GreenAudioPlayer.init({
            selector: s, // inits Green Audio Player on each audio container that has class "player"
            stopOthersOnPlay: true
        });
    } catch (_) { }
}
$(document).ready(function () {
    initAudioPlayer();
});

// Auto resize input
function resizeInput() {
    var len = $(this).val().length;
    if (len < 5)
        len = 5;
    $(this).attr('size', len - 1);
}

$('.auto-sizing')
    // event handler
    .keyup(resizeInput)
    // resize on page load
    .each(resizeInput);

// Seconds extract
function secondsExtract(duration) {
    // Hours, minutes and seconds
    var hrs = ~~(duration / 3600);
    var mins = ~~((duration % 3600) / 60);
    var secs = ~~duration % 60;

    // Output like "1:01" or "4:03:59" or "123:03:59"
    var ret = "";

    if (hrs > 0) {
        ret += "" + hrs + ":" + (mins < 10 ? "0" : "");
    } else {
        ret += (mins < 10 ? "0" : "");
    }

    ret += "" + mins + ":" + (secs < 10 ? "0" : "");
    ret += "" + secs;
    return ret;
}
// Func
function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
function numberWithoutCommas(x) {
    return x.toString().replace(/,/g, '');
}

$("textarea.auto-resize").each(function () {
    this.setAttribute("style", "height:" + (this.scrollHeight) + "px;overflow-y:hidden;");
}).on("input", function () {
    this.style.height = "auto";
    this.style.height = (this.scrollHeight) + "px";
});