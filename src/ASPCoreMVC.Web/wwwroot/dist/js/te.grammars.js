var ceGcModalId = '';
var ceGgModalId = '';

var createGrammarCategoryTitle = "";
var updateGrammarCategoryTitle = "";

var cgt = "";
var ugt = "";

//====================== Grammar Groups =====================//
var defGc = {
    p: 1,
    filter: ''
};
function syncGrammarCategories(p = defGc.p, filter = defGc.filter) {
    defGc = {
        p: p,
        filter: filter
    };
    $("#grammar-cats").load(`/manager/grammar-categories/display?p=${p}&filter=${filter}`, function () {
        // After grammar category loaded
        $("#add-grammar-category-btn").click(showCreateGrammarGroup);
        // Init search action
        initSearchGc();
        // Init gc clicked event
        initClickedGc();
    });
}

function syncDefaultGc() {
    syncGrammarCategories('1', '');
}

function initSearchGc() {
    $("#search-gc-btn").click(function () {
        var inp = $("#search-gc-inp").val();
        syncGrammarCategories('1', inp);
    });
}

function initClickedGc() {
    $(".grammar-category-item").click(event => {
        var s = $(event.currentTarget);
        var id = s.data('id');
        if (id) {
            $(".grammar-category-item").removeClass('active');
            s.addClass("active");
            syncGrammar(id, '1', '');
        }
    });
}

function showCreateGrammarGroup(event, id) {
    if (!id) {
        var url = `/manager/grammar-categories/create`;
        $(`#${ceGcModalId}-content`).load(url, function () {
            $(`#${ceGcModalId} h5.modal-title`).html(createGrammarCategoryTitle);
            // Xử lý sự kiện sau khi load xong
            $(`#${ceGcModalId}`)
                .parent()
                .attr('action', `/api/app/grammar-category`)
                .attr('method', 'POST');
            // Hiển thị hộp thoại
            $(`#${ceGcModalId}`).modal('show');
            $(`#${ceGcModalId}-del-no-sub`).addClass("d-none");
            // bind editor
            bindCKEditor("about-gc");
        });
    } else {
        pd(event);
        var url = `/manager/grammar-categories/update/${id}`;
        $(`#${ceGcModalId}-content`).load(url, function () {
            $(`#${ceGcModalId} h5.modal-title`).html(updateGrammarCategoryTitle);
            // Xử lý sự kiện sau khi load xong
            $(`#${ceGcModalId}`)
                .parent()
                .attr('action', `/api/app/grammar-category/${id}`)
                .attr('method', 'PUT');
            // Cho phép xóa
            $(`#${ceGcModalId}-del-no-sub`).removeClass("d-none");
            $(`#${ceGcModalId}-del-no-sub`).click(function () {
                // Ẩn hộp thoại
                $(`#${ceGcModalId}`).modal('hide');
                deleteConfirm(`/api/app/grammar-category/${id}`, function (res) {
                    //if (id == selectedQuestionGroup) {
                    //    $(`#remove-selected-question-group-btn`).click();
                    //}
                    syncGrammarCategories();
                });
            });
            // Hiển thị hộp thoại
            $(`#${ceGcModalId}`).modal('show');
            // bind editor
            bindCKEditor("about-gc");
        });
    }
}
function gcSynced(res) {
    syncGrammarCategories();
}
//============================== Grammar =========================//
function show2GrammarButtons() {
    $("#add-grammar-btn").show();
    $("#remove-selected-gc-btn").show();
}

function hide2GrammarButtons() {
    $("#add-grammar-btn").hide();
    $("#remove-selected-gc-btn").hide();
}

function sync2GrammarButtons() {
    if (defGg.gcId) {
        show2GrammarButtons();
    } else {
        hide2GrammarButtons();
    }
}

function initRemveSelectedGc() {
    $("#remove-selected-gc-btn").click(function () {
        syncGrammar(null, '1', '');
        $(".grammar-category-item").removeClass('active');
    });
}

var defGg = {
    gcId: null,
    p: 1,
    filter: ''
}

function initEditBtn() {
    $(".gg-edit").click(event => {
        pd(event);
        var id = $(event.currentTarget).data('id');
        if (id) {
            showCreateGrammar(event, id);
        }
    });
}

function initSearchBtn() {
    $("#search-gg-btn").click(event => {
        var inp = $("#search-gg-inp").val();
        syncGrammar(defGg.gcId, '1', inp);
    });
}

function syncDefaultGrammars() {
    syncGrammar(defGg.gcId, '1', '');
}

function syncGrammar(gcId = defGg.gcId, p = defGg.p, filter = defGg.filter) {
    defGg = {
        gcId: gcId,
        p: p,
        filter: filter
    }
    $("#grammars").load(`/manager/grammars/display?gcId=${gcId}&p=${p}&filter=${filter}`, function () {
        sync2GrammarButtons();
        // After grammar loaded
        $("#add-grammar-btn").click(showCreateGrammar);
        // Init show all grammars button
        initRemveSelectedGc();
        // Init edit event
        initEditBtn();
        // Init search button
        initSearchBtn();
    });
}

function showCreateGrammar(event, id) {
    if (!id) {
        var url = `/manager/grammars/${defGg.gcId}/create`;
        $(`#${ceGgModalId}-content`).load(url, function () {
            $(`#${ceGgModalId} h5.modal-title`).html(cgt);
            // Xử lý sự kiện sau khi load xong
            $(`#${ceGgModalId}`)
                .parent()
                .attr('action', `/api/app/grammar`)
                .attr('method', 'POST');
            // Hiển thị hộp thoại
            $(`#${ceGgModalId}`).modal('show');
            $(`#${ceGgModalId}-del-no-sub`).addClass("d-none");
            // bind editor
            bindCKEditor("editor-gg");
        });
    } else {
        pd(event);
        var url = `/manager/grammars/update/${id}`;
        $(`#${ceGgModalId}-content`).load(url, function () {
            $(`#${ceGgModalId} h5.modal-title`).html(ugt);
            // Xử lý sự kiện sau khi load xong
            $(`#${ceGgModalId}`)
                .parent()
                .attr('action', `/api/app/grammar/${id}`)
                .attr('method', 'PUT');
            // Cho phép xóa
            $(`#${ceGgModalId}-del-no-sub`).removeClass("d-none");
            $(`#${ceGgModalId}-del-no-sub`).click(function () {
                // Ẩn hộp thoại
                $(`#${ceGgModalId}`).modal('hide');
                deleteConfirm(`/api/app/grammar/${id}`, function (res) {
                    //if (id == selectedQuestionGroup) {
                    //    $(`#remove-selected-question-group-btn`).click();
                    //}
                    syncGrammar();
                });
            });
            // Hiển thị hộp thoại
            $(`#${ceGgModalId}`).modal('show');
            // bind editor
            bindCKEditor("editor-gg");
        });
    }
}
function ggSynced(res) {
    syncGrammar();
}