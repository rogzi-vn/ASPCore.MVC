// https://stackoverflow.com/questions/51621103/submit-a-form-using-ajax-in-asp-net-core-mvc
$('form.auto-dwd-from').each(function () {
    bindFormAjax(this);
});

function bindFormAjax(f) {
    var callback = $(f).attr('f-callback').valueOf();
    var _dataType = $(f).attr('f-data-type').valueOf();

    $(f).submit(function (e) {
        e.preventDefault();
        var crrModal = $(this).children('div.modal');
        var formAction = $(this).attr("action");
        var method = $(this).attr("method");

        var fdata = new FormData(f);

        var token = $('input[name="__RequestVerificationToken"]').val();

        //console.log(JSON.stringify(Object.fromEntries(fdata)));

        if (_dataType === 'json') {
            $.ajax({
                type: method,
                url: formAction,
                dataType: _dataType,
                beforeSend: function (request) {
                    request.setRequestHeader("RequestVerificationToken", token);
                },
                data: JSON.stringify(Object.fromEntries(fdata)),
                contentType: 'application/json',
                complete: function (res) {
                    if (res.status >= 200 && res.status <= 299) {
                        try {
                            if (res.responseJSON.success) {
                                crrModal.modal('hide');
                            }
                        } catch (e) {
                            crrModal.modal('hide');
                        }
                    }
                    eval(`${callback}(res)`);
                }
            });
        } else {
            $.ajax({
                type: method,
                url: formAction,
                beforeSend: function (request) {
                    request.setRequestHeader("RequestVerificationToken", token);
                },
                data: fdata,
                processData: false,
                contentType: false,
                complete: function (res) {
                    if (res.status >= 200 && res.status <= 299) {
                        try {
                            if (res.responseJSON.success) {
                                $(this).children('div.modal').modal('hide');
                            }
                        } catch (e) {
                            $(this).children('div.modal').modal('hide');
                        }
                    }
                    eval(`${callback}(res)`);
                }
            });
        }
    });
}