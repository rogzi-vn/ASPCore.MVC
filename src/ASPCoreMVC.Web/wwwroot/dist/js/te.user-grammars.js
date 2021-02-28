"use strict";
const vtBaseAPI = "/api/app/vocabulary-topic";
const vtCreateModalPartial = "/manager/vocabulary-topics/create";
const vtUpdateModalPartial = "/manager/vocabulary-topics/update";
var ceVtModalId = 'ce-vt';

var isConfirmable = true;

var cVtT = "";
var uVtT = "";

var defVt = {
    gcId: null,
    p: 1,
    filter: ''
};
function syncVt(gcId = defVt.gcId, p = defVt.p, filter = defVt.filter) {
    defVt = {
        gcId: gcId,
        p: p,
        filter: filter
    };
    $("#grammars-container").load(`/dictionary/grammars/display?p=${p}&filter=${filter}&gcId=${gcId}`, function () {
        // After grammar category loaded
        $("#add-grammar-category-btn").click(showVtModal);
        // Init search action
        initSearchVt();
        // Init edit action
        initEditVt();
        // Init view event
        initViewEvent();
        // Init tooltips
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    });
}

function syncDefVt() {
    syncVt(null, '1', '');
}

function initSearchVt() {
    $("#search-vt-btn").click(function () {
        var inp = $("#search-vt-inp").val();
        syncVt(null, '1', inp);
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

// Đếm số lượng ngữ pháp của mỗi mục
$('span[id^="gc-counting-"]').each(function () {
    var p = $(this);
    var id = p.attr('id').valueOf().replace("gc-counting-", "");
    var url = "";
    if (id && id != "") {
        url = `/api/app/grammar/count/${id}`;
    } else {
        url = `/api/app/grammar/count-all`;
    }
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            if (data.success) {
                p.html(data.data);
            }
        }
    });
});

// Đếm số lượng ngữ pháp của mỗi mục
$('div[id^="sync-grammar-"]').each(function () {
    var p = $(this);
    var id = p.attr('id').valueOf().replace("sync-grammar-", "");
    p.click(function () {
        if (id && id != "") {
            syncVt(id, '1', '');
        } else {
            syncVt(null, '1', '');
        }
    });
});

function initViewEvent() {
    $(".vt-detail").click(event => {
        var e = $(event.currentTarget);
        var id = e.data('id');
        if (id) {
            $("#grammar-detail").load(`/dictionary/grammars/${id}/detail`, function () {
                $("#grammar-detail").modal('show');
            });
        }
    });
}