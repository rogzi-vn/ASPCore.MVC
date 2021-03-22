"use strict";

var notifyAudio = new Audio('/dist/audios/messenger.mp3');

var notificationHubConnection = new signalR.HubConnectionBuilder().withUrl("/notification-hub").build();

notificationHubConnection.start({
    pingInterval: 3000
}).then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});


// Nhận thông báo khi có tin nhắn đến
notificationHubConnection.on("NotyHaveNewMsgReciver", function (msgDTO) {
    if (!location.pathname.startsWith('/discussions') || (location.pathname.startsWith('/discussions') && currentRoomId != msgDTO.messGroupId)) {
        showNewMessageNotification(msgDTO);
        $(`#${msgDTO.messGroupId}-popup-latest-msg`).html(msgDTO.message);
    }
});

function showNewMessageNotification(msgDTO) {
    if (msgDTO && !msgDTO.photo) {
        msgDTO.photo = defaultImage;
    }
    notifyAudio.play();
    syncCountMessage();
    new Noty({
        theme: 'nest',
        timeout: 2500,
        layout: 'bottomRight',
        text: '<div class="card border-left-primary shadow py-0 w-100 parent-nopadding">\n' +
            '    <div class="card-body p-2">\n' +
            '        <div class="d-flex">\n' +
            '            <div class="col-auto">\n' +
            '                <div style="z-index: 10;">\n' +
            `                    <div class="rounded-circle round-size-avt-sm" style="background-image: url(${msgDTO.photo});"></div>\n` +
            '                </div>\n' +
            '            </div>\n' +
            '            <div class="col p-0">\n' +
            '                <div class="text-xs font-weight-bold text-primary text-truncate text-uppercase mb-1">\n' +
            `                    ${msgDTO.displayName}\n` +
            '                </div>\n' +
            '                <div class="mb-0 text-gray-800 text-truncate">\n' +
            `                    ${msgDTO.message}\n` +
            '                </div>\n' +
            '            </div>\n' +
            '        </div>\n' +
            '    </div>\n' +
            '</div>\n',
        type: 'new_msg_noty',
    }).show();
}