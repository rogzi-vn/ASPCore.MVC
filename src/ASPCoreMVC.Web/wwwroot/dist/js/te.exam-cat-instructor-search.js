const __BaseAPI = "/api/app/exam-cat-instruct";

var __defInput = {
    catId: null,
    p: 1,
    filter: ''
};
function syncData(catId = __defInput.catId, p = __defInput.p, filter = __defInput.filter) {
    __defInput = {
        catId: catId,
        p: p,
        filter: filter
    };
    if (!catId)
        return;
    $("#add-instructor-panel").load(`/manager/exam-cate-instructors/search-instructor/${catId}?p=${p}&filter=${filter}`, function () {
        // Init search action
        __initSearch();
        // Init action button
        initActionButton();
        // Init tooltips
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    });
}

function syncDefaultData() {
    syncData(__defInput.catId, '1', '');
}

function __initSearch() {
    $("#__search_button").click(function () {
        var inp = $("#__search_input").val();
        syncData(__defInput.catId, '1', encodeURI(inp));
    });
}

function initActionButton() {
    $(".__action_button").click(event => {
        var p = $(event.currentTarget);
        var uId = p.data('uid');
        var ecId = p.data('ecid');
        var uName = p.data('uname');
        var ecName = p.data('ecname');
        if (uId && ecId && uName && ecName) {
            confirmAddInstructor(uName, ecName, ecId, uId);
        }
    });
}

function confirmAddInstructor(uName, ecName, ecId, uId) {
    var data = JSON.stringify({
        userId: uId,
        examCategoryId: ecId
    });
    Swal.fire({
        title: 'Are you sure?',
        text: `Maked ${uName} as Instructor of ${ecName} Exam`,
        icon: 'warning',
        showCancelButton: true,
        showLoaderOnConfirm: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, make it!',
        preConfirm: () => {
            var token = $('input[name="__RequestVerificationToken"]').val();
            return fetch(decodeURI('/api/app/exam-cat-instruct'), {
                method: 'POST',
                headers: {
                    RequestVerificationToken: token,
                    accept: 'text/plain',
                    'content-type': 'application/json'
                },
                body: data
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
            Swal.fire('SUCCESS', "Add new instructor success!", 'success');
            // Reload user list
            syncData();
            // Reload instructor list
            syncVt();
        }
    });
}