var defaultIds = [];
function collectionIds() {
    var tempIds = [];
    $("#skill-cate-table tbody tr").each(function () {
        var id = $(this).attr('id');
        if (id && tempIds.indexOf(id) < 0) {
            tempIds.push(id);
        }
    });
    return tempIds;
}
function onPositionChanged() {
    var tempIds = collectionIds();
    if (JSON.stringify(defaultIds) !== JSON.stringify(tempIds)) {
        // Cập nhật
        defaultIds = tempIds;
        // Xử lý cập nhật vị trí tại đây
        var token = $('input[name="__RequestVerificationToken"]').val();
        fetch("/api/app/skill-category/update-order", {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
                // 'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: JSON.stringify(tempIds),
        }).then(response => response.text()).then(data => {
            showToast('success', "Changed the sort order");
        }).catch(e => {
            window.location.reload();
        });
    }

}

// Lấy danh sách ID lần đầu
$(document).ready(function () {
    defaultIds = collectionIds();
});