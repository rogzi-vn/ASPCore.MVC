const vtBaseAPI = "/manager/roles";
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
    $("#add-role-btn").click(() => showCreateUpdateVocabularyModal());
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
            showCreateUpdateVocabularyModal(event, id);
        }
    });
}

function showCreateUpdateVocabularyModal(event, id) {
    if (!id) {
        var url = `${vtCreateModalPartial}`;
        $(`#${ceVtModalId}-content`).load(url, function () {
            $(`#${ceVtModalId} h5.modal-title`).html(cVtT);
            // Xử lý sự kiện sau khi load xong
            $(`#${ceVtModalId}`)
                .parent()
                .attr('action', `${vtBaseAPI}/create`)
                .attr('method', 'POST');
            // Hiển thị hộp thoại
            $(`#${ceVtModalId}`).modal('show');
            $(`#${ceVtModalId}-del-no-sub`).addClass("d-none");
            // permisison tree
            fetchTreeJs("/user-permissions/treejs-data", "#permisisons-tree", (nodes, values) => {
                var selectedPermissionName = nodes.map((item) => { return item["id"]; });
                $(`#${ceVtModalId}`)
                    .parent()
                    .attr('action', `${vtBaseAPI}/create?permissions=${selectedPermissionName}`);
            });
            // bind editor
            bindCKEditor("editor-vt");
        });
    } else {
        pd(event);
        var url = `${vtUpdateModalPartial}/${id}`;
        $(`#${ceVtModalId}-content`).load(url, function () {
            var currentRoleName = $("#role-name").val();

            $(`#${ceVtModalId} h5.modal-title`).html(uVtT);

            if (currentRoleName == "admin") {
                // Xử lý sự kiện sau khi load xong
                $(`#${ceVtModalId}`)
                    .parent()
                    .attr('action', '')
                    .attr('method', 'GET');
                $("#ce-vt-del-no-sub").hide();
                $("#ce-vt button.btn.btn-primary").addClass("d-none");
                $("#ce-vt button.btn.btn-primary").removeClass("d-inline-block");
            } else {
                // Xử lý sự kiện sau khi load xong
                $(`#${ceVtModalId}`)
                    .parent()
                    .attr('action', `${vtBaseAPI}/update/${id}`)
                    .attr('method', 'PUT');

                $("#ce-vt-del-no-sub").show();
                $("#ce-vt button.btn.btn-primary").removeClass("d-none");
                $("#ce-vt button.btn.btn-primary").addClass("d-inline-block");

                // selected permissions
                var currentRolePermissions = $("#current-role-permissions").val();
                // permisison tree
                fetchTreeJs("/user-permissions/treejs-data", "#permisisons-tree", (nodes, values) => {
                    var selectedPermissionName = nodes.map((item) => { return item["id"]; });
                    $(`#${ceVtModalId}`)
                        .parent()
                        .attr('action', `${vtBaseAPI}/update/${id}?permissions=${selectedPermissionName}`);
                }, currentRolePermissions.split(","));
            }

            if (currentRoleName.toLocaleLowerCase() != "admin") {
                // Cho phép xóa
                $(`#${ceVtModalId}-del-no-sub`).removeClass("d-none");
                // delete options
                $(`#${ceVtModalId}-del-no-sub`).click(function () {
                    // Ẩn hộp thoại
                    $(`#${ceVtModalId}`).modal('hide');
                    deleteConfirm(`${vtBaseAPI}/delete/${id}`, function (res) {
                        //if (id == selectedQuestionGroup) {
                        //    $(`#remove - selected - question - group - btn`).click();
                        //}
                        syncVt();
                    });
                });
            }
            // Hiển thị hộp thoại
            $(`#${ceVtModalId}`).modal('show');
            // bind editor
            bindCKEditor("editor-vt");
        });
    }
}
function vtSynced(res) {
    if (res.status >= 200 && res.status <= 299) {
        try {
            if (res.responseJSON.success) {
                syncVt();
                // Hiển thị hộp thoại
                $(`#${ceVtModalId}`).modal('hide');
            }
        } catch (e) {

        }
    } else {
        try {
            res.responseJSON.error.validationErrors.forEach(item => {
                //console.log(item);
                showToast('error', item.message);
            });
        } catch (ez) {
        }
    }

}