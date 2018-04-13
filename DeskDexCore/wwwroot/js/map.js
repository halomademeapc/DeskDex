$(document).ready(function () {
    //console.log("let's do this!");

    var fs = $("#floorSelect");
    
    fs.on("change", function () {
        var targetFloor = $("#floorSelect").val();
        loadMap(targetFloor);
    });

    $("#mapImage").on("dragstart", function (event) {
        event.preventDefault();
    })

    setOverlay();
    $(window).resize(setOverlay);

    $(".zoomViewport").on("click", checkDetailStatus);
    $("#overlays").on("click", checkDetailStatus);

    var defaultFloor = fs.data("default")
    if (defaultFloor > 0) {
        fs.val(defaultFloor);
    }
    loadMap(fs.val());
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
    // set cookie to remember current floor
    document.cookie = "mapFloor=" + floor + ";";

    var olDiv = $("#overlays");

    //clear existing elements
    olDiv.empty();

    // call API to get desk locations on this floor
    $.getJSON('/api/floor/' + floor, function (data) {
        //console.log(data);
        // load image
        $("#mapImage").attr('src', data.floorImage);
        // load stations
        for (var i = 0; i < data.stations.length; i++) {
            var thisStation = createStation(data.stations[i]);
            //console.log(thisStation);
            olDiv.append(thisStation);
        }
    });
}

function createStation(stationViewModel) {
    var stationDiv = $(document.createElement('div'));
    var size = [(stationViewModel.x2 - stationViewModel.x1) * 100, (stationViewModel.y2 - stationViewModel.y1) * 100];
    stationDiv.addClass("stationDiv");
    stationDiv.addClass(("workstyle-" + stationViewModel.workStyle).toLowerCase().replace(" ", "-"));
    stationDiv.css("width", size[0] + "%");
    stationDiv.css("height", size[1] + "%");
    stationDiv.css("left", (stationViewModel.x1 * 100) + "%");
    stationDiv.css("top", (stationViewModel.y1 * 100) + "%");
    stationDiv.data("targetsize", .5);
    stationDiv.data("stationID", stationViewModel.deskID);

    $(stationDiv).on("click", function (event) {
        //console.log("station clicked");
        $("#followTooltip").fadeOut(200);
        $(this).zoomTarget();
        checkDetailStatus();
    });

    $(stationDiv).on("mouseover", function (event) {
        $("#tooltipText").text(stationViewModel.location);
        $("#followTooltip").fadeIn(100);
    });

    $(stationDiv).on("mouseleave", function (event) {
        $("#followTooltip").fadeOut(200);
    })

    return stationDiv;
}

function checkDetailStatus() {
    setTimeout(function () {
        //console.log("cds called");
        var tar = $(".stationDiv.selectedZoomTarget");
        if (tar.length > 0) {
            try {
                if (tar.data("stationID") > 0) {
                    updateDetails(tar.data("stationID"));
                    $("#roomDetails").slideDown();
                }
            } catch (e) {
                //console.log(e);
                $("#roomDetails").slideUp();
            }
        } else {
            $("#roomDetails").slideUp();
        }
    }, 700);
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
}