let floorSelect;
let cropper;
let loaded = false;

$(document).ready(function () {
    floorSelect = $("#selectedFloor");
    console.log(floorSelect.val());
    if (floorSelect.val() != "") {
        loadMap(floorSelect.val());
    }

    floorSelect.on("change", function () {
        loadMap($(this).val());
    });

    var $image = $(".imageSelect");

    $("#imageZoomout").on("click", function () {
        $image.cropper("zoom", - 0.2);
        initCrop();
    });
    $("#imageZoomin").on("click", function () {
        $image.cropper("zoom", 0.2);
        initCrop();
    });
    $("#imageZoomReset").on("click", function () {
        var containerData = $image.cropper('getContainerData');
        var imageData = $image.cropper('getImageData');
        var xScale = containerData.width / imageData.naturalWidth;
        var yScale = containerData.height / imageData.naturalHeight;
        console.log({ xScale: xScale, yScale: yScale });
        $image.cropper("zoomTo", xScale > yScale ? yScale : xScale);
        initCrop();
    });

    $("#imageRotateCCW").on("click", function () {
        $image.cropper("rotate", -30);
        initCrop();
    });
    $("#imageRotateCW").on("click", function () {
        $image.cropper("rotate", 30);
        initCrop();
    });
    $("#imageRotateReset").on("click", function () {
        $image.cropper("rotateTo", 0);
        initCrop();
    });
    var $panButton = $("#imageModePan");
    var $cropButton = $("#imageModeCrop");
    var $dragButton = $("#imageModeReset");

    $panButton.on("click", function () {
        $image.cropper("setDragMode", "move");
        $(this).addClass("active");
        $cropButton.removeClass("active");
        updateFields();
    });
    $cropButton.on("click", function () {
        $image.cropper("setDragMode", "crop");
        $(this).addClass("active");
        $panButton.removeClass("active");
        updateFields();
    });
    $("#imageModeReset").on("click", function () {
        $image.cropper("setDragMode", "none");
        $panButton.removeClass("active");
        $cropButton.removeClass("active");
        updateFields();
    });
});

function setCoords(c) {
    $("#Station_x1").val(c.left);
    $("#Station_x2").val(c.right);
    $("#Station_y1").val(c.top);
    $("#Station_y2").val(c.bottom);
}

function initCrop() {
    setCrop(getCoords());
}

getCoords = () => ({
    left: $("#Station_x1").val(),
    right: $("#Station_x2").val(),
    top: $("#Station_y1").val(),
    bottom: $("#Station_y2").val()
});

function setCrop(c) {
    var $image = $(".imageSelect");
    var target = convertDbCoordsToCropper(c);
    console.log({ input: c, target: target });

    $image.cropper("setCropBoxData", target);
}

convertDbCoordsToCropper = (c) => {
    var $image = $(".imageSelect");
    var imageData = $image.cropper("getImageData");
    var canvasData = $image.cropper("getCanvasData");
    var target = {
        left: (imageData.width * (c.left)) + canvasData.left - imageData.left,
        top: (imageData.height * (c.top)) + canvasData.top - imageData.top,
        width: 0,
        height: 0
    };
    return {
        left: target.left,
        top: target.top,
        width: (imageData.width * (c.right)) + canvasData.left - target.left - imageData.left,
        height: (imageData.height * (c.bottom)) + canvasData.top - target.top - imageData.top
    };
}

updateFields = () => {
    var image = $(".imageSelect");
    var imageData = image.cropper('getImageData');
    var cropData = image.cropper('getCropBoxData');
    var canvasData = image.cropper('getCanvasData');

    var canvasScale = imageData.width / imageData.naturalWidth;
    console.log(canvasScale);

    var coords = {
        left: (imageData.left + cropData.left - canvasData.left) / imageData.width,
        top: (imageData.top + cropData.top - canvasData.top) / imageData.height,
        right: (imageData.left + cropData.left - canvasData.left + cropData.width) / imageData.width,
        bottom: (imageData.top + cropData.top + cropData.height - canvasData.top) / imageData.height
    };
    // find size
    var size = {
        width: cropData.width / imageData.width,
        height: cropData.height / imageData.height
    }

    // find center
    var center = {
        x: ((size.width / 2) + cropData.left - canvasData.left) / canvasData.width,
        y: ((size.height / 2) + cropData.top - canvasData.top) / canvasData.height
    };

    // calc result
    var result = {
        left: center.x - (size.width / 2),
        top: center.x - (size.height / 2),
        right: center.x + (size.width / 2),
        bottom: center.x + (size.height / 2),
        angle: imageData.rotate
    }

    setCoords(coords);

    console.log({
        result: result,
        size: size,
        center: center
    });
};

function loadMap(floor) {
    $.getJSON('/api/map/' + floor, function (data) {
        // load background svg
        console.log("loading floor " + floor);
        var image = $(".imageSelect");
        if (!loaded) {
            var options = {
                background: false,
                modal: false,
                scalable: false,
                zoomOnWheel: false,
                dragMode: "crop",
                crop: function () {
                    if (!loaded) {
                        loaded = true;
                        initCrop();
                    }
                },
                viewMode: 1
            };

            image.attr('src', data.filePath);
            image.removeAttr("style");
            image.attr("max-width", "100%");
            image.on("load", function () {
                $("#imageSelect").on("cropend", function (e) {
                    console.log("cropend triggered");
                    updateFields();
                }).cropper(options);
            });
        }
        else {
            console.log("replacing image");
            image.cropper("replace", data.filePath);
            initCrop();
        }

    });
}