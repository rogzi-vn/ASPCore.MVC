const vtBaseAPI = "/api/app/user-note";
const vtCreateModalPartial = "/notes/create";
const vtUpdateModalPartial = "/notes/update";
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
    $("#note-cats").load(`/notes/display?p=${p}&filter=${filter}`, function () {
        // Init search action
        initSearchVt();
        // Init note clicked
        detailtNotes();
        // Init edit action
        initEditVt();
        // Init tooltips
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    });
}

function syncDefVt() {
    syncVt('1', '');
}

function detailtNotes() {
    $(".q-item-in-list").click(event => {
        var p = $(event.currentTarget);
        var id = p.data('id');
        if (id) {
            fetch(`${vtBaseAPI}/${id}`).then(res => res.json()).then(res => {
                if (res.success) {
                    $("#note-title").html(res.data.title);
                    $("#note-content").html(res.data.content);
                    showToast('success', res.message);
                } else {
                    if (res.message) {
                        showToast('error', res.message);
                    } else {
                        showToast('error', 'Can not show detail for this note');
                    }
                }
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
            bindCKEditor("editor-vt", true);
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
            bindCKEditor("editor-vt", true);
        });
    }
}
function vtSynced(res) {
    syncVt();
}