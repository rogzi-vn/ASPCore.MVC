function syncAlertCenter() {
    $("#message-center-content").load("/message-center/sync", function () {
        var total = $("#alert-center-count").val();
        bindNotifyClick();

    })
}

var previousNewCount = 0;
function syncCountMessage() {
    fetch("/api/app/user-message/count-unread-message")
        .then(r => r.text())
        .then(data => {
            var count = data;
            if (count > 0) {
                $("#message-center-badge").removeClass("d-none");
                var countZ = count;
                if (count > 5) {
                    countZ = "5+";
                } else {
                    countZ = count.toString();
                }
                $("#message-center-badge").html(countZ);
            } else {
                $("#message-center-badge").addClass("d-none");
            }
        });
}

function bindNotifyClick() {
    $(".message-item").click(event => {
        alert('clicked');
        //var parent = $(event.currentTarget);
        //var id = parent.attr('id');
        //var href = parent.attr('c-href');

        //var token = $('input[name="__RequestVerificationToken"]').val();
        //// fetch seen
        //fetch(`/api/app/notification/${id}/notification-seen-state`, {
        //    method: 'PUT',
        //    headers: {
        //        RequestVerificationToken: token,
        //        accept: 'text/plain',
        //        'content-type': 'application/json'
        //    },
        //}).then(r => r.text())
        //    .then(data => {
        //        if (href && href != "#") {
        //            if (!href.startsWith("javascript:")) {
        //                window.location = href;
        //            } else {
        //                eval(href.replace("javascript:", ""));
        //            }
        //        }
        //    });
    });
}

syncCountMessage();

bindNotifyClick();

$("#messagesDropdown").click(syncAlertCenter);