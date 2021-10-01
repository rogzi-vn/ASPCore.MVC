function syncAlertCenter() {
    $("#alert-center-content").load("/alert-center/sync", function () {
        var total = $("#alert-center-count").val();
        $(".notify-item").click(event => {
            var parent = $(event.currentTarget);
            var id = parent.attr('id');
            var href = parent.attr('c-href');

            var token = $('input[name="__RequestVerificationToken"]').val();
            // fetch seen
            fetch(`/api/app/notification/${id}/notification-seen-state`, {
                method: 'PUT',
                headers: {
                    RequestVerificationToken: token,
                    accept: 'text/plain',
                    'content-type': 'application/json'
                },
            }).then(r => r.text())
                .then(data => {
                    if (href && href != "#") {
                        if (!href.startsWith("javascript:")) {
                            window.location = href;
                        } else {
                            eval(href.replace("javascript:", ""));
                        }
                    }
                });
        });
    })
}

const intervalTime = 60000 * 2;

function syncCountNotification() {
    fetch("/api/app/notification/count-unread-notification")
        .then(r => r.text())
        .then(data => {
            const count = data;
            if (count > 0) {
                $("#alert-center-badge").removeClass("d-none");
                let countZ;
                if (count > 5) {
                    countZ = "5+";
                } else {
                    countZ = count.toString();
                }
                $("#alert-center-badge").html(countZ);
            } else {
                $("#alert-center-badge").addClass("d-none");
            }
            setTimeout(syncCountNotification, intervalTime);
        })
        .catch((error) => {
            setTimeout(syncCountNotification, intervalTime);
        });
}

function markAllAsRead() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    fetch(`/api/app/notification/mark-all-as-read`, {
        method: 'POST',
        headers: {
            RequestVerificationToken: token,
            accept: 'text/plain',
            'content-type': 'application/json'
        },
    }).then(r => r.text())
        .then(data => {
            location.reload();
        });
}

syncCountNotification();

$("#alertsDropdown").click(syncAlertCenter);