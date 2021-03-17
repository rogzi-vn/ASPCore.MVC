const vtBaseAPI = "/api/app/exam-cat-instruct";

var defVt = {
    catId: null,
    p: 1,
    filter: ''
};
function syncVt(catId = defVt.catId, p = defVt.p, filter = defVt.filter) {
    defVt = {
        catId: catId,
        p: p,
        filter: filter
    };
    if (!catId)
        return;
    $("#instructors").load(`/manager/exam-cate-instructors/display/${catId}?p=${p}&filter=${filter}`, function () {
        // Init search action
        initSearchVt();
        // Remove confirm
        initRemoveAction();
        // initLoadUserProfileTrigger
        initLoadUserProfileTrigger();
        // Init tooltips
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    });
}

function syncDefVt() {
    syncVt(defVt.catId, '1', '');
}

function initSearchVt() {
    $("#search-vt-btn").click(function () {
        var inp = $("#search-vt-inp").val();
        syncVt(defVt.catId, '1', encodeURI(inp));
    });
}



function initRemoveAction() {
    $(".__remove_instructor").click(event => {
        pd(event);
        var p = $(event.currentTarget);
        var id = p.data('id');
        var uName = p.data('uname');
        var ecName = p.data('ecname');
        if (id && uName && ecName) {
            confirmRemoveInstructor(uName, ecName, id);
        }
    });
}

function confirmRemoveInstructor(uName, ecName, id) {
    Swal.fire({
        title: 'Are you sure?',
        text: `Remove ${uName} from ${ecName} Exam`,
        icon: 'warning',
        showCancelButton: true,
        showLoaderOnConfirm: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: `Yes, remove ${uName}!`,
        preConfirm: () => {
            var token = $('input[name="__RequestVerificationToken"]').val();
            return fetch(decodeURI(`/api/app/exam-cat-instruct/${id}`), {
                method: 'DELETE',
                headers: {
                    RequestVerificationToken: token,
                    accept: '*/*'
                }
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
            Swal.fire('SUCCESS', `Remove ${uName} from ${ecName} success`, 'success');
            // Reload user list
            syncData();
            // Reload instructor list
            syncVt();
        }
    });
}