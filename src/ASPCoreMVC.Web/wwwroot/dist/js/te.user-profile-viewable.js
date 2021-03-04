var userProfileViewableModalId = "";

function loadUserProfileViewableModal(userId) {
    $(`#${userProfileViewableModalId}-content`).load(`/users/${userId}/profile-modal`, function () {
        // Show modal when content loaded
        $(`#${userProfileViewableModalId}`).modal('show');
        makeSameSize();
    });
}

function initLoadUserProfileTrigger() {
    $(".profile-viewable").click(event => {
        var p = $(event.currentTarget);
        var id = p.data('id');
        if (id) {
            loadUserProfileViewableModal(id);
        }
    });
}