const __BaseAPI = "/api/app/app-user";

var __defInput = {
    p: 1,
    filter: ''
};
function syncData(p = __defInput.p, filter = __defInput.filter) {
    __defInput = {
        p: p,
        filter: filter
    };

    $("#user-collection").load(`/manager/users/display?p=${p}&filter=${filter}`, function () {
        // Init search action
        __initSearch();
        // Init action button
        initActionButton();
        // Init user profile view able
        initLoadUserProfileTrigger();
        // Init tooltips
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    });
}

function syncDefaultData() {
    syncData('1', '');
}

function __initSearch() {
    $("#__search_button").click(function () {
        var inp = $("#__search_input").val();
        syncData('1', encodeURI(inp));
    });
}

function initActionButton() {
    $(".__action_button").click(event => {
        var p = $(event.currentTarget);
    });
}