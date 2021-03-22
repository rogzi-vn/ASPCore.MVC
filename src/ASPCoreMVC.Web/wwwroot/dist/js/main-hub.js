"use strict";

var chatBoxScrolling = false;
var connection = new signalR.HubConnectionBuilder().withUrl("/main-hub").build();
var currentRoomId = "";
var currentUserId = "";

connection.start({
    pingInterval: 3000
}).then(function () {
    document.getElementById("send-message").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

// Nhận tin nhắn chung
connection.on("ReceiveGlobalMessage", function (msgDTO) {
    if (currentRoomId && currentRoomId.length > 0)
        return;
    //console.log(msgDTO);
    $.ajax({
        type: 'post',
        url: "/render-message",
        data: JSON.stringify(msgDTO),
        contentType: 'application/json',
        complete: function (res) {
            if (res.status >= 200 && res.status <= 299) {
                $("#message-content").append(res.responseText);
                scrollEnd();
            }
        }
    });
});

// Nhận tin nhắn cá nhân
connection.on("ReceiveMessage", function (msgDTO) {
    if (msgDTO && $(`#${msgDTO.messGroupId}-latest-msg`).length > 0) {
        if (msgDTO) {
            $(`#${msgDTO.messGroupId}-latest-msg`).html(msgDTO.message);
            var m = new Date(msgDTO.creationTime);
            var dateString =
                ("0" + m.getHours()).slice(-2) + ":" +
                ("0" + m.getMinutes()).slice(-2) + ":" +
                ("0" + m.getSeconds()).slice(-2) + " " +
                ("0" + m.getDate()).slice(-2) + "/" +
                ("0" + (m.getMonth() + 1)).slice(-2) + "/" +
                m.getFullYear();
            $(`#${msgDTO.messGroupId}-latest-msg-time`).html(dateString);
            $(`#${msgDTO.messGroupId}-latest-msg-time`).removeClass("d-none");
            if (currentUserId != msgDTO.creatorID)
                $(`#${msgDTO.messGroupId}-latest-msg`).addClass("font-weight-bold");
        }
    } else {

    }
    onHaveNewMessage_ReloadList();
    if (currentRoomId && currentRoomId.length >= 32) {

        $.ajax({
            type: 'post',
            url: "/render-message",
            data: JSON.stringify(msgDTO),
            contentType: 'application/json',
            complete: function (res) {
                if (res.status >= 200 && res.status <= 299) {
                    $("#message-content").append(res.responseText);
                    scrollEnd();
                }
            }
        });

        connection.invoke("MessageSeen", msgDTO).catch(function (err) {
            return console.error(err.toString());
        });

        $(`#${msgDTO.messGroupId}-latest-msg`).removeClass("font-weight-bold");

    }
});

function ChangeRoom(roomId, roomName) {
    $("#room-name").html(roomName);
    $("#message-content").html("");
    loadPreviousMessage(roomId);
    currentRoomId = roomId;
    connection.invoke("ChangeRoom", roomId, roomName).catch(function (err) {
        return console.error(err.toString());
    });
}

// Nhận tin chào mừng
connection.on("ReceiveWelcome", function (msg) {
    if (currentRoomId && currentRoomId.length > 0)
        return;
    $("#message-content").append(`<div class="separator my-2 text-center small">${msg}</div>`);
});

// Cập nhật số người đang online
connection.on("UpdateGeneralOnlineCounter", function (msg) {
    $("#online-general-room-counter").html(msg);
});

// Nhận tin tạm biệt
connection.on("ReceiveBye", function (msg) {
    if (currentRoomId && currentRoomId.length > 0)
        return;
    $("#message-content").append(`<div class="separator my-2 text-center small">${msg}</div>`);
});

connection.on("OnNotification", function (type, msg) {
    showToast(type, msg);
});

document.getElementById("send-message").addEventListener("click", function (event) {
    pd(event);
    var msg = $("#msg-value").val();
    if (!msg || msg.length <= 0) {
        return;
    }
    msg = msg.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    $("#msg-value").val('');
    if (currentRoomId && currentRoomId.length > 0) {
        $(`#${currentRoomId}-latest-msg`).html(msg);
        connection.invoke("SendMessage", currentRoomId, msg).catch(function (err) {
            return console.error(err.toString());
        });
    } else {
        connection.invoke("SendGlobalMessage", msg).catch(function (err) {
            return console.error(err.toString());
        });
    }
});


function initRoomChangeable() {
    $(".room-changeable").click(event => {
        var p = $(event.currentTarget);
        var roomId = p.data("room-id");
        var roomName = p.data("room-name");
        if (roomId == currentRoomId) {
            showToast('warning', "You are current this room");
        } else {
            // Hỏi chuyển room
            Swal.fire({
                title: 'Change room?',
                text: `Do you want to change to room \"${roomName}\"!`,
                icon: 'warning',
                showCancelButton: true,
                showLoaderOnConfirm: true,
                confirmButtonColor: '#1cc88a',
                cancelButtonColor: '#4e73df',
                confirmButtonText: 'Yes, change it!',
            }).then((result) => {
                if (result.isConfirmed) {
                    isFirstScrollEnd = true;
                    ChangeRoom(roomId, roomName);
                }
            });
        }
        pd(event);
    });
}


function initAutoScroll() {
    $("#message-content").on('scroll', function () {
        var scrollTop = $(this).scrollTop();
        var scrollHeight = $(this).get(0).scrollHeight;
        if (scrollTop == 0 && scrollHeight >= 200) {
            var oldestMsg = $("#oldest-mgs-id").val();
            loadPreviousMessage(currentRoomId);
            if (oldestMsg) {
                $(this).scrollTop(200);
            }
        } else {
            chatBoxScrolling = false;
        }
        chatBoxScrolling = true;
    });
}

function scrollEnd() {
    $("#message-content").animate({
        scrollTop: $("#message-content").get(0).scrollHeight
    }, 1000);
}

initAutoScroll();