$(".form-update-corrent-question").submit(e => {
    e.preventDefault();
    var f = $(e.currentTarget);
    console.log(f);
    var fdata = new FormData(f[0]);

    var token = $('input[name="__RequestVerificationToken"]').val();

    var data = JSON.stringify(Object.fromEntries(fdata));

    Swal.fire({
        title: 'Are you sure?',
        text: `Do you want to update the resulting content for this answer`,
        icon: 'warning',
        showCancelButton: true,
        showLoaderOnConfirm: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, update!',
        preConfirm: () => {
            return fetch(decodeURI(f.attr('action')), {
                method: f.attr('method'),
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
            Swal.fire('SUCCESS', "Update results for successful answers", 'success');
            // Reload user list
            location.reload();
        }
    });
});

$("#instructor-comment").on("input propertychange", function () {
    var res = $(this).val();
    if (res.length > 0) {
        $("#instructor-comment-submit").removeClass("d-none");
    } else {
        $("#instructor-comment-submit").addClass("d-none");
    }
});

$("#instructor-comment-submit").click(event => {
    var token = $('input[name="__RequestVerificationToken"]').val();

    var examLogId = $("#hidden-exam-log-id").val();

    var valuer = $("#instructor-comment").val();

    if (valuer.length <= 0) {
        showToast('error', "Please enter content for comments");
        return;
    }

    Swal.fire({
        title: 'Are you sure?',
        text: `Would you like to update your comments for this test`,
        icon: 'warning',
        showCancelButton: true,
        showLoaderOnConfirm: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, update!',
        preConfirm: () => {
            return fetch(decodeURI(`/api/app/exam-log/instructor-comments/${examLogId}?comments=${encodeURI(valuer)}`), {
                method: 'PUT',
                headers: {
                    RequestVerificationToken: token,
                    accept: 'text/plain',
                    'content-type': 'application/json'
                }
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
            Swal.fire('SUCCESS', "Update results for successful answers", 'success');
            // Reload user list
            location.reload();
        }
    });
})