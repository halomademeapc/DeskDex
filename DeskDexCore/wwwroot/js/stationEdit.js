let floorSelect;
$(document).ready(function () {
    floorSelect = $("#selectedFloor");
    //console.log(floorSelect.val());
    if (floorSelect.val() != "") {
        loadMap(floorSelect.val());
    }

    floorSelect.on("change", function () {
        loadMap($(this).val());
    });
});

function setCoords(c) {
    //console.log(c);
    var map = $("#mapContainer").children("img");
    xs = map.width();
    ys = map.height();
    var x1 = c.x / xs;
    var x2 = c.x2 / xs;
    var y1 = c.y / ys;
    var y2 = c.y2 / ys;
    $("#Station_x1").val(x1);
    $("#Station_x2").val(x2);
    $("#Station_y1").val(y1);
    $("#Station_y2").val(y2);
}

function loadMap(floor) {
    $.getJSON('/api/map/' + floor, function (data) {
        // load background svg
        //console.log("loading floor " + floor);
        var mapContainer = $("#mapContainer");
        mapContainer.empty();
        var image = $(document.createElement('img'));
        image.attr('src', data.filePath);
        image.removeAttr("style");
        image.attr("width", "100%");
        mapContainer.append(image);
        image.Jcrop({
            onChange: setCoords,
            onSelect: setCoords,
            bgColor: 'white',
            setSelect: [image.width() * parseFloat($("#Station_x1").val()), image.height() * parseFloat($("#Station_y1").val()), image.width() * parseFloat($("#Station_x2").val()), image.height() * parseFloat($("#Station_y2").val())],
        });
    });
}