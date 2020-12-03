
//TOGLE SHOW HIDE
    $('.nav-toggle-alt').click(function() {
        alert("asdads");
        //get collapse content selector
        var collapseContentSelector = $(this).attr('href');

        //make the collapse content to be shown or hide
        var toggleSwitch = $(this);
        $(collapseContentSelector).slideToggle(function() {
            if ($(this).css('display') == 'block') {
                //change the button label to be 'Show'
                toggleSwitch.html('<span class="entypo-up-open"></span>');
            } else {
                //change the button label to be 'Hide'
                toggleSwitch.html('<span class="entypo-down-open"></span>');
            }
        });
        return false;
    });
    //CLOSE ELEMENT
        $(".gone").click(function() {
        var collapse_content_close = $(this).attr('href');
        $(collapse_content_close).hide();
    


    });
