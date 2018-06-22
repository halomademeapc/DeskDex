let floorSelect;
let cropper;

$(document).ready(function () {
    floorSelect = $("#selectedFloor");
    console.log(floorSelect.val());
    if (floorSelect.val() != "") {
        loadMap(floorSelect.val());
    }

    floorSelect.on("change", function () {
        loadMap($(this).val());
    });
});

function setCoords(c) {
    console.log(c);
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
        console.log("loading floor " + floor);
        var image = $(".imageSelect");
        image.attr('src', data.filePath);
        image.removeAttr("style");
        image.attr("max-width", "100%");
        image.on("load", function () {
            $("#imageSelect").cropper({
                background: false,
                modal: false,
                scalable: false,
                wheelZoomRatio: 0.03,
                crop: function (event) {
                    console.log(event.detail);
                }
            });
            var cropper = $image.data('cropper');
        });
    });
}