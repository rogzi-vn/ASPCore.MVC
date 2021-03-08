// http://jsfiddle.net/martyk/Kzf62/85/
$(document).ready(function () {
    var tb = $(".drag-drop");
    $("tbody.connectedSortable")
        .sortable({
            connectWith: ".connectedSortable",
            items: "> tr",
            appendTo: tb,
            helper: "clone",
            zIndex: 10,
            start: function () { tb.addClass("dragging") },
            stop: function () {
                tb.removeClass("dragging");
                eval(`${tb.data("moved")}()`);
            }
        }).disableSelection();


    tb.droppable({
        accept: ".connectedSortable tr",
        hoverClass: "ui-state-hover",
        over: function (event, ui) {
            var sizes = [];
            $(".drag-drop thead tr th").each(function () {
                var p = $(this);
                sizes.push(p.width());
            });
            $("tr.ui-sortable-helper").width($(".drag-drop thead tr").width());
            $("tr.ui-sortable-helper td").each(function (index) {
                $(this).width(sizes[index]);
            });
        },
        drop: function (event, ui) {
            return false;
        }
    });

});