

(function ($) {
    "use strict";

    $(".spinner" ).spinner({
        min: 0,
        max: 999,
        numberFormat: "n",
        value: 10,
        default: 10
    });

})(jQuery);