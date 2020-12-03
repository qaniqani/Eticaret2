// Table Order Function Begin
$(document).ready(function () {
    var fixHelperModified = function (e, tr) {
        var $originals = tr.children();
        var $helper = tr.clone();
        $helper.children().each(function (index) {
            $(this).width($originals.eq(index).width())
        });
        return $helper;
    },
    updateIndex = function (e, ui) {
        $('td.index', ui.item.parent()).each(function (i) {
            var generatedIndex = i + 1;

            $(this).html(generatedIndex);
            var id = $(this).parent().children('.itemId').val();
            $(this).parent().children('.hiddenField').val(id + "|" + generatedIndex);
        });
    };


    $(".table-order tbody").sortable({
        helper: fixHelperModified,
        stop: updateIndex,
        items: '> tr',
        start: function (event, ui) {
            // Build a placeholder cell that spans all the cells in the row
            var cellCount = 0;
            $('td, th', ui.helper).each(function () {
                // For each TD or TH try and get it's colspan attribute, and add that or 1 to the total
                var colspan = 1;
                var colspanAttr = $(this).attr('colspan');
                if (colspanAttr > 1) {
                    colspan = colspanAttr;
                }
                cellCount += colspan;
            });

            // Add the placeholder UI - note that this is the item's content, so TD rather thanTR
            ui.placeholder.html('<td colspan="' + cellCount + '">&nbsp;</td>');
        }
    }).disableSelection();
});
// Table Order Function End


// Menu Function Begin
$(document).ready(function () {
    $('.main-left-menu ul').each(function () {
        var urls = $(this).attr('data-url').split("|");
        var currentUrl = window.location.pathname.split("/");
        $(this).attr('style', '');
        for (var i = 0; i < urls.length; i++) {
            var url = urls[i];
            if (url == currentUrl[2]) {
                $(this).css('display', 'block');
            }
        }
    });

    $('.main-left-menu ul li a').each(function () {
        var aUrl = $(this).attr('href');
        if (window.location.pathname == aUrl) $(this).addClass('selected');
    });
});
// Menu Function End

$(document).ready(function () {
    jQuery("form").validationEngine();

    $(".fancy").fancybox({
        width: '70%'
        //autoHeight:"auto"
    });

    $(".lblSelectGalleryCategory").click(function () {
        $(this).parent("div").children(".galleryCategories").show();
    });

    $(".GalleryCategory .galleryCategories em").click(function () {
        $(this).parent("div.galleryCategories").hide();
    });

    $(".GalleryCategory .galleryCategories div").click(function () {
        var text = $(this).text();
        var id = $(this).attr("data-id");

        $(this).parents("div.GalleryCategory").children("i").text(text);
        $(this).parents("div.GalleryCategory").children("input").val(id);

        $(this).parents("div.galleryCategories").hide();
    });

    try {
        tinymce.init({
            selector: ".richEditor",
            height: 300,
            theme: "modern",
            plugins: [
              "advlist autolink lists link image charmap print preview hr pagebreak",
              "searchreplace wordcount visualblocks visualchars code fullscreen",
              "insertdatetime media nonbreaking save table contextmenu directionality",
              "template paste textcolor colorpicker textpattern imagetools"
            ],
            toolbar1: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
            toolbar2: "print preview media | forecolor backcolor",
            image_advtab: true,
            content_css: [
              "//fast.fonts.net/cssapi/e6dc9b99-64fe-4292-ad98-6974f93cd2a2.css",
              "//www.tinymce.com/css/codepen.min.css"
            ]
        });
    } catch (e) {
        console.log("Error Tinymce Editor plugin.");
        console.log(e);
    }

    try {
        $(".sortable").nestedSortable({
            handle: "div",
            items: "li",
            toleranceElement: "> div"
            //stop: function() {
                 //save_order();
            //}
        });
    } catch (e) { }

    try {
        $('.tooltipButton').tooltip();
        $('.commentDetail').tooltip();
    } catch (e) {} 

    $("#saveOrderMenu").click(function() {
        save_order();
    });

    $(".btnRemoveImage").click(function () {
        var id = $(this).attr("data-id");
        var divName = "#image-" + id;
        $("div").remove(divName);
    });

    $("#VideoType").change(function () {
        VideoTypeChange();
    });

    $('.footable-res').footable();

    $('#footable-res2').footable().bind('footable_filtering', function (e) {
        var selected = $('.filter-status').find(':selected').text();
        if (selected && selected.length > 0) {
            e.filter += (e.filter && e.filter.length > 0) ? ' ' + selected : selected;
            e.clear = !e.filter;
        }
    });

    $('.clear-filter').click(function (e) {
        e.preventDefault();
        $('.filter-status').val('');
        $('table.demo').trigger('footable_clear_filter');
    });

    $('.filter-status').change(function (e) {
        e.preventDefault();
        $('table.demo').trigger('footable_filter', {
            filter: $('#filter').val()
        });
    });

    $('.filter-api').click(function (e) {
        e.preventDefault();

        //get the footable filter object
        var footableFilter = $('table').data('footable-filter');

        alert('about to filter table by "tech"');
        //filter by 'tech'
        footableFilter.filter('tech');

        //clear the filter
        if (confirm('clear filter now?')) {
            footableFilter.clearFilter();
        }
    });

    //products add
    $("#wizard").steps({
        headerTag: "h2",
        bodyTag: "section",
        enableFinishButton: false,
        enablePagination: false,
        enableAllSteps: true
    });

    //stepy
    $('#transition-duration-demo').stepy({
        duration: 400,
        transition: 'fade'
    });

    $("#categorySelect").treeview({
        animated: "fast",
        collapsed: true,
        control: "#treecontrol"
    });

    $(".productSelectMeasure").click(function() {
        var id = $(this).attr("data-value");
        if ($(this).is(':checked'))
            $(this).closest("td").find("input[type=hidden]").first().val(id);
        else
            $(this).closest("td").find("input[type=hidden]").first().removeAttr("value");
    });

    $(".productSelectCategory").click(function () {
        var id = $(this).attr("data-value");
        if ($(this).is(':checked'))
            $(this).closest("li").find("input[type=hidden]").first().val(id);
        else
            $(this).closest("li").find("input[type=hidden]").first().removeAttr("value");
    });

    $(".MultiFile-remove").click(function() {
        var divId = $(this).attr("data-id");
        //console.log(divId);
        $("#image-" + divId).remove();
    });

    //User list
    $("#CountryUser").change(function() {
        var selectedCountry = $(this).val();
        var selectedCountryText = $("#CountryUser option:selected").text();

        $("#CountryName").val(selectedCountryText);
        $("#CityName").val("");
        $("#RegionName").val("");

        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "GET",
            url: "/Admin/User/GetCity",
            data: {
                countryId: selectedCountry
            },
            success: function (response) {
                $('#CityUser')
                    .empty()
                    .append('<option selected="selected" value="0">All</option>');

                $.each(response, function (index, value) {
                    $("#CityUser").append($('<option>',
                        {
                            value: value.Id,
                            text: value.Name
                        }));
                });

                $('#RegionUser')
                    .empty()
                    .append('<option selected="selected" value="0">Select City</option>');
            }
        });
    });

    $("#CityUser").change(function () {
        var selectedCity = $(this).val();
        var selectedCityText = $("#CityUser option:selected").text();

        $("#CityName").val(selectedCityText);
        $("#RegionName").val("");

        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "GET",
            url: "/Admin/User/GetRegion",
            data: {
                cityId: selectedCity
            },
            success: function (response) {
                console.log(response);
                $('#RegionUser')
                    .empty()
                    .append('<option selected="selected" value="0">All</option>');

                $.each(response, function (index, value) {
                    $("#RegionUser").append($('<option>',
                        {
                            value: value.Id,
                            text: value.Name
                        }));
                });
            }
        });
    });

    $("#RegionUser").change(function () {
        var selectedRegionText = $("#RegionUser option:selected").text();
        $("#RegionName").val(selectedRegionText);
    });

    $(".update-order-row").click(function() {
        var id = $(this).attr("data-id");
        var selectedStatus = $(this).parent().parent().parent().find(".order-row-status").val();
        var selectedText = $(this).parent().parent().parent().find(".order-row-status").children(":selected").text();
        var note = $(this).parent().parent().find(".order-row-causeRefund").val();

        //console.log(id + " - " + selectedStatus + " - " + note + " - " + selectedText);

        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "POST",
            url: "/Admin/Order/ChangeProductOrderType",
            data: JSON.stringify({
                id: id,
                orderType: selectedStatus,
                note: note
            }),
            success: function () {
                $(this).parent().parent().parent().parent().parent().parent().parent().find("td.row-status").html(selectedText);
                $(this).parent().parent().parent().parent().parent().toggle();
                alert("Save change success!");
            }
        });
    });

    $(".show-update-order-row").click(function () {
        $(this).parent().find(".order-row-update-box").toggle();
    });

    $(".delete-order-row").click(function () {
        var id = $(this).attr("data-id");

        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "POST",
            url: "/Admin/Order/DeleteOrderProductRow",
            data: JSON.stringify({
                id: id
            }),
            success: function () {
                console.log("Delete row!");
            }
        });

        $(this).parent().parent().remove();
    });
});

function productSetMeasures() {
    var measures = $("#measureIds").val();
    var measureIds = measures.split(",");
    $.each(measureIds, function(index, id) {
        if (id && id != "") {
            $(".measure-" + id).prop("checked", "checked");
            $(".measure-value-" + id).attr("value", id);
        }
    });
}

function productSetCategories() {
    var categories = $("#categoryIds").val();
    var categoryIds = categories.split(",");
    $.each(categoryIds, function (index, id) {
        if (id && id != "") {
            $(".productSelectCategory-" + id).prop("checked", "checked");
            $(".productSelectCategory-value-" + id).attr("value", id);
        }

        console.log(id);
    });
}

function VideoTypeChange() {
    var value = $("#VideoType").val();
    if (value == "0") {
        $("#VideoUrl").hide();
        $("#EmbedCode").hide();
        $("#PictureUpload").show();
    }

    if (value == "1") {
        $("#VideoUrl").show();
        $("#EmbedCode").hide();
        $("#PictureUpload").hide();
    }

    if (value == "2") {
        $("#VideoUrl").hide();
        $("#EmbedCode").show();
        $("#PictureUpload").hide();
    }
}

function save_order() {
    var serialized = $('ol.sortable').nestedSortable('toArray', { startDepthCount: 0 });
    var list = [];

    $.each(serialized, function (index, value) {
        var row = {
            Depth: value.depth,
            ItemId: value.item_id,
            Left: value.left,
            ParentId: value.parent_id,
            Right: value.right
        };

        list.push(row);
    });

    list = JSON.stringify({ 'order': list });

    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: "POST",
        url: "/Admin/Menu/OrderMenu",
        data: list,
        success: function () {
            $(".savechange").show();
        }
    });

    setTimeout($(".savechange").hide(), 5000);
}