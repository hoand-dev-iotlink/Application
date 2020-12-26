var MapControl = {
    SELECTORS: {
        appendRL: "#map4d .mf-map4d .mf-ui-bottom-right",
        appendUD: "#map4d .mf-map4d .mf-ui-bottom-right .horizontal .mf-button",
        btnRight: "#map4d .mf-right",
        btnLeft: "#map4d .mf-left",
        btnUp: "#map4d .mf-up",
        btnDown: "#map4d .mf-down",

        appendRL3D: "#mapMode3D .mf-map4d .mf-ui-bottom-right",
        appendUD3D: "#mapMode3D .mf-map4d .mf-ui-bottom-right .horizontal .mf-button",
        btnRight3D: "#mapMode3D .mf-right",
        btnLeft3D: "#mapMode3D .mf-left",
        btnUp3D: "#mapMode3D .mf-up",
        btnDown3D: "#mapMode3D .mf-down",
    },
    init: function () {
        $(MapControl.SELECTORS.appendRL).append("<div class='footer-map'><div class='mf-button'><a class='mf-right'></a></div><div class='mf-button'><a class='mf-left'></a></div></div>");
        $(MapControl.SELECTORS.appendUD).before("<div class='mf-button'><a class='mf-up'></a></div><div class='mf-button'><a class='mf-down'></a></div>");
        var camera = null;
        $(MapControl.SELECTORS.btnRight).on("click", function () {
            camera = MapView.GLOBAL.map.getCamera();
            let r = camera.getBearing() + 10;
            camera.setBearing(r);
            MapView.GLOBAL.map.setCamera(camera);
        });
        $(MapControl.SELECTORS.btnLeft).on("click", function () {
            camera = MapView.GLOBAL.map.getCamera();
            let l = camera.getBearing() - 10;
            camera.setBearing(l);
            MapView.GLOBAL.map.setCamera(camera);
        });
        $(MapControl.SELECTORS.btnUp).on("click", function () {
            camera = MapView.GLOBAL.map.getCamera();
            let u = camera.getTilt() + 10;
            camera.setTilt(u);
            MapView.GLOBAL.map.setCamera(camera);
        });
        $(MapControl.SELECTORS.btnDown).on("click", function () {
            camera = MapView.GLOBAL.map.getCamera();
            let d = camera.getTilt() - 10;
            camera.setTilt(d);
            MapView.GLOBAL.map.setCamera(camera);
        });
    },
    initMode3D: function () {
        $(MapControl.SELECTORS.appendRL3D).append("<div class='footer-map'><div class='mf-button'><a class='mf-right'></a></div><div class='mf-button'><a class='mf-left'></a></div></div>");
        $(MapControl.SELECTORS.appendUD3D).before("<div class='mf-button'><a class='mf-up'></a></div><div class='mf-button'><a class='mf-down'></a></div>");
        var camera = null;
        $(MapControl.SELECTORS.btnRight3D).on("click", function () {
            camera = mapMode3D.getCamera();
            let r = camera.getBearing() + 10;
            camera.setBearing(r);
            mapMode3D.setCamera(camera);
        });
        $(MapControl.SELECTORS.btnLeft3D).on("click", function () {
            camera = mapMode3D.getCamera();
            let l = camera.getBearing() - 10;
            camera.setBearing(l);
            mapMode3D.setCamera(camera);
        });
        $(MapControl.SELECTORS.btnUp3D).on("click", function () {
            camera = mapMode3D.getCamera();
            let u = camera.getTilt() + 10;
            let check2D = mapMode3D.is3DMode();
            if (!check2D && u > 40) {
                camera.setTilt(40);
                mapMode3D.setCamera(camera);
            } else {
                camera.setTilt(u);
                mapMode3D.setCamera(camera);
            }
        });
        $(MapControl.SELECTORS.btnDown3D).on("click", function () {
            camera = mapMode3D.getCamera();
            let d = camera.getTilt() - 10;
            camera.setTilt(d);
            mapMode3D.setCamera(camera);
        });
    },
    
}