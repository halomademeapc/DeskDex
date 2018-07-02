let urlChanged = false;
let activeStation = 0;
let activeFloor = 0;

$(document).ready(function () {
    //console.log("let's do this!");
    var fs = $("#floorSelect");

    fs.on("change", function () {
        var targetFloor = $("#floorSelect").val();
        resetZoom();
        loadMap(targetFloor);
    });

    $("#mapImage").on("dragstart", function (event) {
        //console.log("prevent drag");
        event.preventDefault();
    })

    setOverlay();
    $(window).resize(setOverlay);

    $(".zoomViewport").on("click", function () {
        hideDetails();
        resetZoom();
    });

    var defaultFloor = fs.data("default")
    // check querystring
    var urlParams = new URLSearchParams(window.location.search);
    if (urlParams.has("floor")) {
        defaultFloor = parseInt(urlParams.get("floor"));
    }

    if (defaultFloor > 0) {
        fs.val(defaultFloor);
    }
    loadMap(fs.val());

    $(".legendtoggle").on("click", function () {
        $(".workstyle-" + $(this).data("target")).fadeToggle();
    });

    // auto-refresh every 60 seconds
    window.setInterval(refreshData, 60000);
});

$(document).on('mousemove', function (e) {
    var ftt = $("#followTooltip");
    ftt.css({
        left: (e.pageX - (ftt.width() / 2)),
        top: (e.pageY - ftt.height() - 5)
    });
});

function setOverlay() {
    $("#overlays").width($("#mapContainer").width());
}

function loadMap(floor) {
    activeFloor = parseInt(floor);
    // set cookie to remember current floor
    document.cookie = "mapFloor=" + floor + ";";
    if (urlChanged) {
        window.history.pushState("", "", '/Map?floor=' + floor);
    } else {
        urlChanged = true;
    }

    var olDiv = $("#overlays");

    // call API to get desk locations on this floor
    $.getJSON('/api/floor/' + floor, function (data) {
        //console.log(data);
        // load image
        $("#mapImage").attr('src', data.floorImage);

        // check querystring
        var urlParams = new URLSearchParams(window.location.search);
        var targetStation = 0;
        if (urlParams.has("station")) {
            targetStation = parseInt(urlParams.get("station"));
            //console.log("targeting " + targetStation);
        }

        //clear existing elements
        olDiv.empty();

        // load stations
        for (var i = 0; i < data.stations.length; i++) {
            var thisStation = createStation(data.stations[i]);
            //console.log(thisStation);
            olDiv.append(thisStation);
            if (targetStation > 0 && thisStation.data("stationID") == targetStation) {
                thisStation.addClass("queriedStation");
                //console.log("zooming to " + thisStation.data("stationID"));
            }
        }

        //$(".queriedStation").zoomTo();
        setTimeout(function () {
            $(".queriedStation").trigger("click");
        }, 600);
    });
}

function createStation(stationViewModel) {
    var stationDiv = $(document.createElement('div'));
    var size = [(stationViewModel.x2 - stationViewModel.x1) * 100, (stationViewModel.y2 - stationViewModel.y1) * 100];
    stationDiv.addClass("stationDiv");
    stationDiv.addClass(("workstyle-" + stationViewModel.workStyle).toLowerCase().replace(/ /g, "-"));
    stationDiv.css("width", size[0] + "%");
    stationDiv.css("height", size[1] + "%");
    stationDiv.css("left", (stationViewModel.x1 * 100) + "%");
    stationDiv.css("top", (stationViewModel.y1 * 100) + "%");
    //stationDiv.data("targetsize", .05);
    //stationDiv.data("duration", 2000);
    stationDiv.data("stationID", stationViewModel.deskID);
    if (stationViewModel.occupied == true) {
        stationDiv.addClass("occupied");
    }

    $(stationDiv).on("click", function (event) {
        //console.log("station " + $(this).data("stationID") + " clicked");
        var settings = {
            targetsize: .05,
            duration: 2000,
            easing: "ease-in-out",
            root: $(".zoomViewport")
        }
        $("#followTooltip").fadeOut(200);
        //$(this).zoomTarget();
        $(this).zoomTo(settings);
        //checkDetailStatus();
        showDetails($(this).data("stationID"));
        activeStation = parseInt($(this).data("stationID"));
        event.stopPropagation();
    });

    $(stationDiv).on("mouseover", function (event) {
        $("#tooltipText").text(stationViewModel.location);
        $("#followTooltip").fadeIn(100);
    });

    $(stationDiv).on("mouseleave", function (event) {
        $("#followTooltip").fadeOut(200);
    })

    return stationDiv;
};

function refreshData() {
    if (activeFloor > 0) {
        loadMap(activeFloor);
        if (activeStation > 0) {
            updateDetails(activeStation);
        }
    }
}

function showDetails(stationID) {
    updateDetails(stationID);
    $("#roomDetails").slideDown();
    $("#mapLegend").slideUp();
}

function hideDetails() {
    $("#roomDetails").slideUp();
    $("#mapLegend").slideDown();
}

function resetZoom() {
    activeStation = 0;
    var settings = {
        duration: 600,
        easing: "ease-in-out",
        root: $(".zoomViewport")
    }
    $(".zoomViewport").zoomTo(settings);
}

function updateDetails(stationID) {
    $.getJSON('/api/Desk/' + stationID, function (data) {
        //console.log("loaded details for station " + data.deskID);

        $("#rdLocation").text(data.location);
        $("#rdType").text(data.workStyle);
        $("#rdCapacity").text(data.capacity);
        if (data.lastUpdate != null && data.userName != null) {
            $("#rdCheckin").show();
            $("#rdPerson").text(data.userName);
            $("#rdTime").text(data.lastUpdate);
        } else {
            $("#rdCheckin").hide();
        }
        $("#rdEquipment").empty();
        for (var i = 0; i < data.equipment.length; i++) {
            var item = $(document.createElement('li'));
            item.text(data.equipment[i]);
            $("#rdEquipment").append(item);
        }
        if (data.equipment.length == 0) {
            var itm = $(document.createElement('li'));
            itm.text("None listed");
            $("#rdEquipment").append(itm);
        }
        if (data.imagePath != null) {
            $("#rdImage").attr('src', data.imagePath);
            $("#rdImage").css("display", "block");
        } else {
            $("#rdImage").css("display", "none");
        }
    });
}

function getUrlParams(prop) {
    var params = {};
    var search = decodeURIComponent(window.location.href.slice(window.location.href.indexOf('?') + 1));
    var definitions = search.split('&');

    definitions.forEach(function (val, key) {
        var parts = val.split('=', 2);
        params[parts[0]] = parts[1];
    });

    return (prop && prop in params) ? params[prop] : params;
};