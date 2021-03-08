const vtBaseAPI = "/api/app/vocabulary";
const vtCreateModalPartial = "/manager/vocabularies/create";
const vtUpdateModalPartial = "/manager/vocabularies/update";
var ceVtModalId = 'ce-vt';

var isConfirmable = true;

var cVtT = "";
var uVtT = "";

var defVt = {
    p: 1,
    filter: '',
    vcId: null,
    wcId: null
};
function syncVt(p = defVt.p, filter = defVt.filter, vcId = defVt.vcId, wcId = defVt.wcId) {
    defVt = {
        p: p,
        filter: filter
    };
    $("#vocabulary-topics").load(`/manager/vocabularies/display?p=${p}&filter=${filter}&topicId=${vcId}&wcId=${wcId}`, function () {
        // After grammar category loaded
        $("#add-grammar-category-btn").click(showVtModal);
        // Init search action
        initSearchVt();
        // Init edit action
        initEditVt();
        // Init confrim action
        if (isConfirmable) {
            initConfirm();
        }
        // Init tooltips
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    });
}

function syncDefVt() {
    syncVt('1', '', null, null);
}

function initConfirm() {
    $(".csb").click(event => {
        var e = $(event.currentTarget);
        var href = e.data("cref");
        if (href) {
            putConfirm(href, function () {
                syncVt();
            });
        }
    });
}

function initSearchVt() {
    $("#search-vt-btn").click(function () {
        var inp = $("#search-vt-inp").val();
        syncVt('1', encodeURI(inp));
    });
}

function initEditVt() {
    $(".vt-edit").click(event => {
        pd(event);
        var obj = $(event.currentTarget);
        var id = obj.data('id');
        if (id) {
            showVtModal(event, id);
        }
    });
}

function showVtModal(event, id) {
    if (!id) {
        var url = `${vtCreateModalPartial}`;
        $(`#${ceVtModalId}-content`).load(url, function () {
            $(`#${ceVtModalId} h5.modal-title`).html(cVtT);
            // Xử lý sự kiện sau khi load xong
            $(`#${ceVtModalId}`)
                .parent()
                .attr('action', `${vtBaseAPI}`)
                .attr('method', 'POST');
            // Hiển thị hộp thoại
            $(`#${ceVtModalId}`).modal('show');
            $(`#${ceVtModalId}-del-no-sub`).addClass("d-none");
            // Load exists audio
            loadAudio();
            // Init file browser
            initBrowseFile();
            // Init selector
            $('.bb-selector').selectpicker('refresh');
            // bind editor
            bindCKEditor("editor-vt");
        });
    } else {
        pd(event);
        var url = `${vtUpdateModalPartial}/${id}`;
        $(`#${ceVtModalId}-content`).load(url, function () {
            $(`#${ceVtModalId} h5.modal-title`).html(uVtT);
            // Xử lý sự kiện sau khi load xong
            $(`#${ceVtModalId}`)
                .parent()
                .attr('action', `${vtBaseAPI}/${id}`)
                .attr('method', 'PUT');
            // Cho phép xóa
            $(`#${ceVtModalId}-del-no-sub`).removeClass("d-none");
            $(`#${ceVtModalId}-del-no-sub`).click(function () {
                // Ẩn hộp thoại
                $(`#${ceVtModalId}`).modal('hide');
                deleteConfirm(`${vtBaseAPI}/${id}`, function (res) {
                    //if (id == selectedQuestionGroup) {
                    //    $(`#remove-selected-question-group-btn`).click();
                    //}
                    syncVt();
                });
            });
            // Load exists audio
            loadAudio();
            // Init file browser
            initBrowseFile();
            // Init selector
            $('.bb-selector').selectpicker('refresh');
            // Hiển thị hộp thoại
            $(`#${ceVtModalId}`).modal('show');
            // bind editor
            bindCKEditor("editor-vt");
        });
    }
}
function vtSynced(res) {
    syncVt();
}


// Nếu dạng câu hỏi là âm thanh
function loadAudio(path) {
    // Lấy đường dẫn âm thanh mặc định trước đó
    var defaultAudioPath = $("#default-media-path").val();
    if (!path) {
        // Nếu đường dẫn hiện tại không có, gắn đường dẫn cũ.
        path = defaultAudioPath;
    }
    //else if (path != defaultAudioPath) {
    //    // Nếu đường dẫn hiện tại mới hơn, cập nhật vào nơi lưu trữ
    //    $("#media-path-hidden").val(path);
    //}
    if (path) {
        const audio = document.createElement("audio");

        // Clean up the URL Object after we are done with it
        audio.addEventListener("load", () => {
            URL.revokeObjectURL(path);
        });

        // Thay thế cho thông báo
        $('#selected-audio').html(audio);

        // Allow us to control the audio
        audio.controls = "true";

        // Set the src and start loading the audio from the file
        audio.src = path;

        if (path === defaultAudioPath) {
            // Ẩn nút reset
            $("#set-default-audio").hide();
        } else {
            if (defaultAudioPath)
                // Hiển thị nút reset
                $("#set-default-audio").show();
        }
    } else {
        // Ẩn nút reset
        $("#set-default-audio").hide();

        // Đưa về giá trị mặc định
        $("#media-path-hidden").val(defaultAudioPath);

        // Thay thế cho thông báo
        $('#selected-audio').html('<small class="form-text text-danger">No audio availabe.</small>' +
            '<small class="form-text text-info">Select and play audio test here.</small>');
    }
}
// Sự kiện khi âm thanh được chọn
function audioSelectedEvent(url) {
    loadAudio(url);
}