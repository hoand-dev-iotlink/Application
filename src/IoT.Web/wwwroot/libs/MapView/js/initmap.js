var MapView = {
    GLOBAL:
    {
        mapParam: {
            zoom: 17,
            center: [106.3778061599233, 10.24121967517452],
            geolocate: true,
            accessKey: "b77745a2eb604d0989e2b5648d0019b2",
            controls: true,
        },
        MyLocation: null,
        ClickInfoBusId: null,
        ObjMap: null,
        MapLocation: null,
        Marker: new map4d.Marker({
            position: { lat: 0, lng: 0},
            icon: new map4d.Icon(22, 32, document.location.origin + "/images/tramanten.png"),
            anchor: [0.5, 1.0],
            draggable: true
        })
    },
    CONSTS:
    {
    },
    SELECTORS: {
        mapDiv: 'map4d',
    },
    init: function () {
        this.MapInit();
        this.setUpEvent();
    },
    setUpEvent: function () {

    },
    MapInit: function () {
        MapView.GLOBAL.map = new map4d.Map(document.getElementById(MapView.SELECTORS.mapDiv), MapView.GLOBAL.mapParam);
        MapView.GLOBAL.map.setSwitchMode("Auto3DTo2D");
        MapView.GLOBAL.map.setPlacesEnabled(false);
        MapView.GLOBAL.map.setMaxNativeZoom(18);
        MapControl.init();
        //let marker = new map4d.Marker({
        //    position: { lat: 16.073519653221666, lng: 108.22190370001414 },
        //    icon: new map4d.Icon(32, 32, "https://localhost:44340/Medias/Images/Setting/Icon_BusStop_Back.png"),
        //    anchor: [0.5, 0.5],
        //})

        //marker.setMap(MapView.GLOBAL.map);
        
    }
}

$(document).ready(function () {
    MapView.init();
    var obj = document.getElementById(MapView.SELECTORS.mapDiv);
})
