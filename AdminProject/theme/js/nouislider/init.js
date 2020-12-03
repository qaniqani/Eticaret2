

(function ($) {
    "use strict";
    
        $(".ct-js-noUiSliderDisabled").noUiSlider({
            start: 30,
            connect: "lower",
            range: {
                'min': 0,
                'max': 100
            }
        });
    
        $(".ct-js-noUiSlider").noUiSlider({
            start: 50,
            connect: "lower",
            range: {
                'min': 0,
                'max': 100
            },
            step: 15
        });
    
    $('.ct-js-noUiSliderDisabled').attr('disabled', 'disabled');

        $(".ct-js-noUiSliderPrice")
        .noUiSlider({
            connect: false,
            behaviour: 'tap',
            start: [2000, 7000],
            range: {
                // Starting at 500, step the value by 500,
                // until 4000 is reached. From there, step by 1000.
                'min': [0],
                '10%': [500, 50],
                '50%': [4000, 500],
                'max': [10000]
            }
        });

        // Write the CSS 'left' value to a span.
        //function leftValue ( value, handle, slider ) {
        //    $(this).text( handle.parent()[0].style.left );
        //}

        // Bind two elements to the lower handle.
        // The first item will display the slider value,
        // the second shows how far the handle moved
        // from the left edge of the slider.
        $(".ct-js-noUiSliderPrice").Link('lower').to($('#lower-value'));
        //$(".ct-js-noUiSliderPrice").Link('lower').to($('#lower-offset'), leftValue);


        // Do the same for the upper handle.
        $(".ct-js-noUiSliderPrice").Link('upper').to($('#upper-value'));
        //$(".ct-js-noUiSliderPrice").Link('upper').to($('#upper-offset'), leftValue);

})(jQuery);