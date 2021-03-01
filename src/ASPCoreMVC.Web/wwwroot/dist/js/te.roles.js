const vtBaseAPI = "/api/identity/roles";
const vtCreateModalPartial = "/manager/roles/create";
const vtUpdateModalPartial = "/manager/roles/update";
var ceVtModalId = 'ce-vt';

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
    $("#role-collection").load(`/manager/roles/display?p=${p}&filter=${filter}`, function () {
        // Init search action
        initSearchVt();
        // Init edit action
        initEditVt();
        // initAddEvent
        initAddEvent();
        // Init tooltips
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    });
}

function syncDefVt() {
    syncVt('1', '');
}

function initAddEvent() {
    $("#add-role-btn").click(() => showVtModal());
}

function initSearchVt() {
    $("#search-vt-btn").click(function () {
        var inp = $("#search-vt-inp").val();
        syncVt('1', inp);
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
    // Hiển thị hộp thoại
    $(`#${ceVtModalId}`).modal('hide');
}