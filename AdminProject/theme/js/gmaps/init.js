(function ($) {
    "use strict";

    var $maphelp = $('.ct-googleMap--accordion .ct-googleMap');
    $(".ct-googleMap--accordion .ct-js-mapToogle").click(function () {
        var $this = $(this);
        var $map = $this.parent().find('.ct-googleMap-container');
        $this.html($this.html() == '<i class="fa fa-map-marker"></i> Hide map' ? '<i class="fa fa-map-marker"></i> Show map' : '<i class="fa fa-map-marker"></i> Hide map');

        if ($map.height() != "0") {
            $map.animate({height: '0px'}, 500);
        } else {
            $map.animate({height: $maphelp.data("height") + "px"}, 500);
            setTimeout(function () {
                $('html, body').animate({
                    scrollTop: $map.offset().top - 180
                }, 2000);
            }, 500);
        }
    });
    /* ============================================= */
    /* ==== GOOGLE MAP ==== */

    function initmap() {

        if (($(".ct-googleMap").length > 0) && (typeof google === 'object' && typeof google.maps === 'object')) {
            $('.ct-googleMap').each(function () {
                var atcenter = "";
                var $this = $(this);
                var location = $this.data("location");
                var zoom = $this.data("zoom");

                var offset = -30;

                if (validatedata($this.data("offset"))) {
                    offset = $this.data("offset");
                }

                if (validatedata(location)) {
                    $this.gmap3({
                        marker: {
                            //latLng: [40.616439, -74.035540],
                            address: location, options: {
                                //visible: false
                                icon: new google.maps.MarkerImage("assets/images/marker.png")
                            }, callback: function (marker) {
                                atcenter = marker.getPosition();
                            }
                        }, map: {
                            options: {
                                //maxZoom:11,
                                zoom: zoom,
                                mapTypeId: google.maps.MapTypeId.ROADMAP, // ('ROADMAP', 'SATELLITE', 'HYBRID','TERRAIN');
                                scrollwheel: false,
                                disableDoubleClickZoom: false,
                                draggable: false, //disableDefaultUI: true,
                                mapTypeControlOptions: {
                                    //mapTypeIds: [google.maps.MapTypeId.ROADMAP, google.maps.MapTypeId.HYBRID],
                                    //style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR,
                                    //position: google.maps.ControlPosition.RIGHT_CENTER
                                    mapTypeIds: []
                                }
                            }, events: {
                                idle: function () {
                                    if (!$this.data('idle')) {
                                        $this.gmap3('get').panBy(0, offset);
                                        $this.data('idle', true);
                                    }
                                }
                            }
                        }, overlay: {
                            address: location, options: {
                                offset: {
                                    y: -100, x: -25
                                }
                            }
                        }
                        //},"autofit"
                    });

                    // center on resize
                    google.maps.event.addDomListener(window, "resize", function () {
                        //var userLocation = new google.maps.LatLng(53.8018,-1.553);
                        setTimeout(function () {
                            $this.gmap3('get').setCenter(atcenter);
                            $this.gmap3('get').panBy(0, offset);
                        }, 400);

                    });

                    // set height
                    $this.css("min-height", $this.data("height") + "px");
                }

                if ($this.parent().parent().hasClass('hidemap')) {
                    $this.parent().animate({height: '0px'}, 500);
                }

            })
        }

    }

    initmap();
})(jQuery);