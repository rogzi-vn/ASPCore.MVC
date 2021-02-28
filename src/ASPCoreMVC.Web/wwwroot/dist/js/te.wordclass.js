const wcBaseAPI = "/api/app/word-class";
const wcCreateModalPartial = "/manager/word-classes/create";
const wcUpdateModalPartial = "/manager/word-classes/update";
var ceWcModalId = 'ce-wc';

var cWcT = "";
var uWcT = "";

var defWc = {
    p: 1,
    filter: ''
};
function syncWc(p = defWc.p, filter = defWc.filter) {
    defWc = {
        p: p,
        filter: filter
    };
    $("#word-classes").load(`/manager/word-classes/display?p=${p}&filter=${filter}`, function () {
        // After grammar category loaded
        $("#add-grammar-category-btn").click(showWcModal);
        // Init search action
        initSearchWc();
        // Init edit action
        initEditWc();
    });
}

function syncDefWc() {
    syncWc('1', '');
}

function initSearchWc() {
    $("#search-wc-btn").click(function () {
        var inp = $("#search-wc-inp").val();
        syncWc('1', inp);
    });
}

function initEditWc() {
    $(".wc-edit").click(event => {
        pd(event);
        var obj = $(event.currentTarget);
        var id = obj.data('id');
        if (id) {
            showWcModal(event, id);
        }
    });
}

function showWcModal(event, id) {
    if (!id) {
        var url = `${wcCreateModalPartial}`;
        $(`#${ceWcModalId}-content`).load(url, function () {
            $(`#${ceWcModalId} h5.modal-title`).html(cWcT);
            // Xử lý sự kiện sau khi load xong
            $(`#${ceWcModalId}`)
                .parent()
                .attr('action', `${wcBaseAPI}`)
                .attr('method', 'POST');
            // Hiển thị hộp thoại
            $(`#${ceWcModalId}`).modal('show');
            $(`#${ceWcModalId}-del-no-sub`).addClass("d-none");
            // bind editor
            bindCKEditor("editor-wc");
        });
    } else {
        pd(event);
        var url = `${wcUpdateModalPartial}/${id}`;
        $(`#${ceWcModalId}-content`).load(url, function () {
            $(`#${ceWcModalId} h5.modal-title`).html(uWcT);
            // Xử lý sự kiện sau khi load xong
            $(`#${ceWcModalId}`)
                .parent()
                .attr('action', `${wcBaseAPI}/${id}`)
                .attr('method', 'PUT');
            // Cho phép xóa
            $(`#${ceWcModalId}-del-no-sub`).removeClass("d-none");
            $(`#${ceWcModalId}-del-no-sub`).click(function () {
                // Ẩn hộp thoại
                $(`#${ceWcModalId}`).modal('hide');
                deleteConfirm(`${wcBaseAPI}/${id}`, function (res) {
                    //if (id == selectedQuestionGroup) {
                    //    $(`#remove-selected-question-group-btn`).click();
                    //}
                    syncWc();
                });
            });
            // Hiển thị hộp thoại
            $(`#${ceWcModalId}`).modal('show');
            // bind editor
            bindCKEditor("editor-wc");
        });
    }
}
function wcSynced(res) {
    syncWc();
}