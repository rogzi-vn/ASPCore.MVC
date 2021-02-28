var isHaveOtherModal = false;
function fmSetLink($url, id = "", callback) {
    $(`#${id}`).val($url);
    $("#modalSelectFile").modal("hide");
    if (callback) {
        eval(`${callback}($url)`);
    }
}

function initBrowseFile() {
    $(".btn-choose-file").click(e => {
        var extensions = "";
        var callbackfName = "";
        if (typeof $(e.target).data("extensions") !== typeof undefined) {
            extensions = "&extensions=" + $(e.target).data("extensions");
        }
        if (typeof $(e.target).data("callback") !== typeof undefined) {
            callbackfName = "&callback=" + $(e.target).data("callback");
        }
        if (typeof $(e.target).data("for") !== typeof undefined) {
            $("#modalSelectFile .modal-body").html("");
            isHaveOtherModal = $('body').hasClass('modal-open');
            $("#modalSelectFile").modal("show");

            $('#modalSelectFile').on('hidden.bs.modal', function () {
                // Nếu trước đó có một modal khác thì vẫn giữ lại class modal-open
                if (isHaveOtherModal) {
                    $('body').addClass('modal-open');
                }
            })
            setTimeout(() => {
                $("#modalSelectFile .modal-body").html(
                    '<iframe src="/manager/files/explore?id=' +
                    $(e.target).data("for") +
                    extensions +
                    callbackfName +
                    '" frameborder="0" style="width:100%;height:100%"></iframe>'
                );
            }, 450);
        }
    });

}

$(document).ready(function () {
    initBrowseFile();
});