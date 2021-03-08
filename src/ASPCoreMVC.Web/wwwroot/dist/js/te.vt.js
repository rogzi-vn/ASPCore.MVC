const vtBaseAPI = "/api/app/vocabulary-topic";
const vtCreateModalPartial = "/manager/vocabulary-topics/create";
const vtUpdateModalPartial = "/manager/vocabulary-topics/update";
var ceVtModalId = 'ce-vt';

var isConfirmable = true;

var cVtT = "";
var uVtT = "";

var defVt = {
    p: 1,
    filter: ''
};
function syncVt(p = defVt.p, filter = defVt.filter) {
    defVt = {
        p: p,
        filter: filter
    };
    $("#vocabulary-topics").load(`/manager/vocabulary-topics/display?p=${p}&filter=${filter}`, function () {
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
    syncVt('1', '');
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