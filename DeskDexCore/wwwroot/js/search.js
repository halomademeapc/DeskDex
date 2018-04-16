let currentSearch;

function loadSearchResults(term) {
    console.log("searching for " + term);
    if (currentSearch) {
        console.log("cancelling old request");
        currentSearch.abort();
    }
    var suggest = $("#searchSuggest");
    currentSearch = $.getJSON('/api/search/' + term, function (data) {
        console.log(data);

        suggest.empty();
        // populate results
        for (var i = 0; i < data.people.length; i++) {
            createDdlItem(data.people[i].display, data.people[i].link, suggest);
        }
        if (data.people.length > 0 && data.stations.length > 0) {
            createDivider(suggest);
        }
        for (var j = 0; j < data.stations.length; j++) {
            createDdlItem(data.stations[j].display, data.stations[j].link, suggest);
        }
    });
}

function createDivider(append) {
    var d = $(document.createElement('div'));
    d.addClass("dropdown-divider");
    append.append(d);
}

function createDdlItem(text, target, append) {
    var lnk = $(document.createElement('a'));
    lnk.addClass("dropdown-item");
    lnk.text(text);
    lnk.attr("href", target);
    append.append(lnk);
}

$(document).ready(function () {
    var clear = $("#searchClear");
    var search = $("#userSearch");

    search.on("input", function () {
        console.log("event triggered");
        var suggest = $("#searchSuggest");
        if ($(this).val() == "") {
            suggest.slideUp();
            clear.hide();
        } else {
            loadSearchResults($(this).val());
            clear.removeClass("d-none");
            suggest.slideDown();
            clear.show();
        }
    });

    clear.on("click", function () {
        console.log("clearing");
        search.val("");
        search.trigger("input");
    })
});