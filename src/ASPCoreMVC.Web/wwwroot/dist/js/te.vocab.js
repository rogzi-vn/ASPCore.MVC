const vtBaseAPI = "/api/app/vocabulary";

var defVt = {
    p: 1,
    filter: ''
};
function syncVt(p = defVt.p, filter = defVt.filter) {
    defVt = {
        p: p,
        filter: filter
    };
    $("#vocabs").load(`/dictionary/vocabularies/search?p=${p}&filter=${filter}`, function () {
        // Init search action
        initSearchVt();
        // Init Audio player
        initAudioPlayer();
        // Init tooltips
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    });
}

function syncDefVt() {
    syncVt('1', '');
}

function initSearchVt() {
    $("#search-vt-btn").click(function () {
        var inp = $("#search-vt-inp").val();
        syncVt('1', inp);
    });
}


function vtSynced(res) {
    syncVt();
}

function initAudioPlayer() {
    $(".voice-speaker").click(event => {
        p = $(event.currentTarget);
        url = p.data('audio');
        if (url)
            playAudio(url);
        else {
            showToast('info', "This word haven't any pronouce audio");
        }
    });
}