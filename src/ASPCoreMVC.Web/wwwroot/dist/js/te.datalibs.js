var editQuestionPath = '';
var currentSkillPartId = '';
var createQuestionGroupModalId = '';
var createQuestionGroupTitle = "";
var updateQuestionGroupTitle = "";

//========================= START QUESTION CLICK =========================//
function syncQuestionClickable() {
    $(".q-item-in-list").click(e => {
        var crr = $(e.currentTarget);
        var id = crr.data('id');
        if (id) {
            window.location = `${editQuestionPath}/${id}`;
        }
    });
}
//========================= END QUESTION CLICK =========================//

//========================= START QUESTION =========================//
var questionQueries = {
    skillPartId: null,
    p: '1',
    filter: '',
    groupId: ''
}
function syncDefaultQuestions() {
    questionQueries.filter = "";
    syncQuestions();
}
function syncQuestions(skillPartId = questionQueries.skillPartId, p = questionQueries.p, filter = questionQueries.filter, groupId = questionQueries.groupId) {
    questionQueries = {
        skillPartId: skillPartId,
        p: p,
        filter: filter,
        groupId: groupId
    }
    $("#questions").load(`/manager/exam-data-libraries/${skillPartId}/questions?p=${p}&filter=${filter}&groupId=${groupId}`,
        function () {
            $("#search-questions-btn").click(function () {
                var inp = $("#search-questions-inp").val();
                if (!inp) {
                    inp = "";
                }
                syncQuestions(questionQueries.skillPartId, '1', encodeURI(inp), questionQueries.groupId);
            });

            if (!selectedQuestionGroup) {
                $("#remove-selected-question-group-btn").attr('disabled', '');
            }
            $("#remove-selected-question-group-btn").click(function () {
                selectedQuestionGroup = "";
                $(".question-group-item").removeClass("active");
                syncQuestions(questionQueries.skillPartId, '1', '', '');
                $("#remove-selected-question-group-btn").attr('disabled', '');
            });

            indeQuestionPreview();
            syncQuestionClickable();

            $(function () {
                $('[data-toggle="tooltip"]').tooltip()
            });
        });
}
//========================= END QUESTION =========================//

//========================= START QUESTION GROUP =========================//
var selectedQuestionGroup = "";
var questionGroupQueries = {
    skillPartId: null,
    p: '1',
    filter: ''
}
function syncDefaultQuestionGroups() {
    questionGroupQueries.filter = "";
    syncQuestionGroups();
}
function syncQuestionGroups(skillPartId = questionGroupQueries.skillPartId, p = questionGroupQueries.p, filter = questionGroupQueries.filter) {
    questionGroupQueries = {
        skillPartId: skillPartId,
        p: p,
        filter: filter
    }
    $("#question-groups").load(`/manager/exam-data-libraries/${skillPartId}/question-groups?p=${p}&filter=${filter}`,
        function () {
            $("#search-question-groups-btn").click(function () {
                var inp = $("#search-question-groups-inp").val();
                if (!inp) {
                    inp = "";
                }
                syncQuestionGroups(questionGroupQueries.skillPartId, '1', encodeURI(inp));
            });

            $(".question-group-item").click(event => {
                var id = $(event.currentTarget).data('id');
                if (id) {
                    selectedQuestionGroup = id;
                    $("#remove-selected-question-group-btn").removeAttr('disabled');
                    $(".question-group-item").removeClass("active");
                    $(event.currentTarget).addClass("active");
                    syncQuestions(questionQueries.skillPartId, '1', '', id);
                }
            });

            $(".question-group-item").each(function () {
                if ($(this).data('id') == selectedQuestionGroup) {
                    $(this).addClass("active");
                }
            });

            $("#add-question-group-btn").click(showCreUpdQgModal);

            $(function () {
                $('[data-toggle="tooltip"]').tooltip()
            });

        });
}
//========================= END QUESTION GROUP =========================//

//========================= START QUESTION GROUP CREATE=========================//
function showCreUpdQgModal(event, id) {
    if (!id) {
        var url = `/manager/question-groups/${currentSkillPartId}/create`;
        $(`#${createQuestionGroupModalId}-content`).load(url, function () {
            $(`#${createQuestionGroupModalId} h5.modal-title`).html(createQuestionGroupTitle);
            // Xử lý sự kiện sau khi load xong
            $(`#${createQuestionGroupModalId}`)
                .parent()
                .attr('action', `/api/app/exam-question-group`)
                .attr('method', 'POST');
            // Hiển thị hộp thoại
            $(`#${createQuestionGroupModalId}`).modal('show');
            $(`#${createQuestionGroupModalId}-del-no-sub`).addClass("d-none");
        });
    } else {
        pd(event);
        var url = `/manager/question-groups/update/${id}`;
        $(`#${createQuestionGroupModalId}-content`).load(url, function () {
            $(`#${createQuestionGroupModalId} h5.modal-title`).html(updateQuestionGroupTitle);
            // Xử lý sự kiện sau khi load xong
            $(`#${createQuestionGroupModalId}`)
                .parent()
                .attr('action', `/api/app/exam-question-group/${id}`)
                .attr('method', 'PUT');
            // Cho phép xóa
            $(`#${createQuestionGroupModalId}-del-no-sub`).removeClass("d-none");
            $(`#${createQuestionGroupModalId}-del-no-sub`).click(function () {
                // Ẩn hộp thoại
                $(`#${createQuestionGroupModalId}`).modal('hide');
                deleteConfirm(`/api/app/exam-question-group/${id}`, function (res) {
                    if (id == selectedQuestionGroup) {
                        $(`#remove-selected-question-group-btn`).click();
                    }
                    syncQuestionGroups();
                });
            });
            // Hiển thị hộp thoại
            $(`#${createQuestionGroupModalId}`).modal('show');
        });
    }
}
function questionGroupSynced(res) {
    syncQuestionGroups();
}
//========================= END QUESTION GROUP CREATE=========================//

//========================= START QUESTION PREVIEW =========================//
function indeQuestionPreview() {
    $(".q-preview").click(e => {
        var c = $(e.currentTarget);
        var id = c.data('id');
        if (id) {
            showQuestionPreview(id);
        }
    });
}
function showQuestionPreview(id) {
    $("#preview-question-content").load(`/manager/exam-data-libraries/questions/${id}/preview`,
        function () {
            // Cho phép xóa
            $("#preview-question-del-no-sub").click(function () {
                // Ẩn hộp thoại
                $("#preview-question").modal('hide');
                deleteConfirm(`/api/app/exam-data-library/${id}`, function (res) {
                    syncQuestions();
                });
            });
            // Hiển thị hộp thoại
            $("#preview-question").modal('show');
            // equation initialize
            MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
        });
}
//========================= END QUESTION PREVIEW =========================//