$('.nav-item').each(function () {
    var top = $(this);
    var navLink = $(this).find('a.nav-link:first');
    var collapse = $(this).find('div.collapse:first');

    var path = navLink.attr('href').valueOf();
    if (path && path != '#' &&
        path != 'javascript:;' &&
        path != 'javascript:void(0);' &&
        (path.indexOf('/') == 0 || path.length > 0)) {
        // Nếu đường dấn này là hiện tại
        if (path == location.pathname) {
            top.addClass('active');
        }
        return;
    } else {
        // Nếu không có đường dẫn và không có phần mở rộng -> quay lại
        if (collapse == undefined || collapse == null)
            return;
        $(this).find('a.collapse-item').each(function () {
            var p = $(this).attr('href').valueOf();
            if (p && p != '#' &&
                p != 'javascript:;' &&
                p != 'javascript:void(0);' &&
                (p.indexOf('/') == 0 || p.length > 0)) {
                // Nếu đường dấn này là hiện tại
                if (p == location.pathname) {
                    top.addClass('active');
                    $(this).addClass('active');
                    navLink.removeClass('collapsed');
                    navLink.attr('aria-expanded', 'true');

                    collapse.addClass('show');
                }
                return;
            }
        });
    }
});