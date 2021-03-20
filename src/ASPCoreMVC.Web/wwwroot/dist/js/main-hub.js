"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/main-hub").build();

connection.start().then(function () {
    document.getElementById("send-message").disabled = false;
    //connection.invoke("SendWelcome").catch(function (err) {
    //    return console.error(err.toString());
    //});
}).catch(function (err) {
    return console.error(err.toString());
});

// Nhận tin nhắn chung
connection.on("ReceiveGlobalMessage", function (msgDTO) {
    console.log(msgDTO);
    $.ajax({
        type: 'post',
        url: "/render-message",
        data: JSON.stringify(msgDTO),
        contentType: 'application/json',
        complete: function (res) {
            if (res.status >= 200 && res.status <= 299) {
                $("#message-content").append(res.responseText);
            }
        }
    });
});

// Nhận tin chào mừng
connection.on("ReceiveWelcome", function (msg) {
    $("#message-content").append(`<div class="separator my-2 text-center small">${msg}</div>`);
});

// Nhận tin tạm biệt
connection.on("ReceiveBye", function (msg) {
    $("#message-content").append(`<div class="separator my-2 text-center small">${msg}</div>`);
});

document.getElementById("send-message").addEventListener("click", function (event) {
    var msg = $("#msg-value").val();
    msg = msg.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    $("#msg-value").val('');
    connection.invoke("SendGlobalMessage", msg).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});