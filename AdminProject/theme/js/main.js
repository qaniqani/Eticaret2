var snapper;
$(document).ready(function () {
    $('.main-navigation li.dropdown').each(function () {
        $(this).find('ul').addClass('dropdown-menu');
        if ($(this).children('ul').children('li').find('ul').length != 0) {
            $(this).children('ul').find('li').addClass('dropdown-submenu');
        }
    });

    if (document.getElementById('ct-js-wrapper')) {
        snapper = new Snap({
            element: document.getElementById('ct-js-wrapper')
        });

        snapper.settings({
            addBodyClasses: true
        });
    }

    function setPageSkinLocation() {
        $("div#PageSkinLeft, div#PageSkinRight").show();

        var fromLeft = $(".ct-contentWrapper").offset().left;
        fromLeft = (parseInt(fromLeft) - parseInt(500)) + "px";
        $("div#PageSkinLeft").css("margin-left", fromLeft);

        var screenWidth = $(window).width();
        var siteWidth = $(".ct-contentWrapper").width();

        var fromRight = (((screenWidth - siteWidth) / 2)) - 500;
        fromRight = fromRight + "px";

        var pageSkinWidth = screenWidth - siteWidth;
        $("div#PageSkinRight").css("margin-right", "0").css("width", pageSkinWidth / 2);
        $("div#PageSkinLeft").css("margin-left", "0").css("width", pageSkinWidth / 2);

        if (screenWidth < 1350) {
            $("div#PageSkinRight, div#PageSkinLeft").hide();
        }
    }

    $(window).load(function () {
        setPageSkinLocation();
    });

    $(window).resize(function () {
        setPageSkinLocation();
    });
});

var $devicewidth = (window.innerWidth > 0) ? window.innerWidth : screen.width;
var $deviceheight = (window.innerHeight > 0) ? window.innerHeight : screen.height;
var $bodyel = jQuery("body");
var $navbarel = jQuery(".navbar");
var $topbarel = jQuery(".ct-topBar");

var $lgWidth = 1199;
var $mdWidth = 991;
var $smWidth = 767;
var $xsWidth = 479;

function validatedata($attr, $defaultValue) {
    "use strict";
    if ($attr !== undefined) {
        return $attr
    }
    return $defaultValue;
}

function parseBoolean(str, $defaultValue) {
    "use strict";
    if (str == 'true') {
        return true;
    } else if (str == "false") {
        return false;
    }
    return $defaultValue;
}

function showLoading() {
    $("#loading").show();
}

function hideLoading() {
    $("#loading").hide();
}

function showMessageBox(message) {
    $(".showMessage").html(message);
    $(".showMessageBox").show();
}

function hideMessageBox() {
    $(".showMessageBox").hide();
    $(".showSuccessMessageBox").hide();
}

function showSuccessMessageBox(message) {
    $(".showSuccessMessage").html(message);
    $(".showSuccessMessageBox").show();
}

function isValidEmailAddress(emailAddress) {
    var pattern = new RegExp(/^[+a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i);
    // alert( pattern.test(emailAddress) );
    return pattern.test(emailAddress);
};

function hideSuccessMessageBox() {
    $(".showSuccessMessageBox").hide();
}

function getMiniBasketList() {
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: "POST",
        url: "/basket/list",
        beforeSend: function () {
            showLoading();
        },
        success: function (result) {
            $("#basketListNotFound").hide();
            $("#basketMiniProductList").html("");
            $("#mobileBasketMiniProductList").html("");
            var productList = result.ProductList;
            var basketTotalSum = result.BasketTotalSum;

            var prodLength = productList.length;

            $("#basketQuantity").html(prodLength + " ürün");
            $("#mobileBasketQuantity").html("(" + prodLength + " ürün)");

            var basketTotalAmount = parseFloat(basketTotalSum.TotalAmount);

            $("#basketTotalAmount").html(" - " + basketTotalAmount.toFixed(2) + " <i class=\"fa fa-try\" aria-hidden=\"true\"></i>");
            $("#mobileBasketTotalAmount").html(basketTotalAmount.toFixed(2) + " <i class=\"fa fa-try\" aria-hidden=\"true\"></i>");

            if (prodLength > 0) {
                for (var i = 0; i < prodLength; i++) {

                    var item = productList[i];

                    var mobileRowTemplate = "<li class=\"ct-shopMenuMobile-basketProduct\">" +
                        "<a href=\"/product/" + item.Url + "/detail\">" +
                            "<div class=\"ct-shopMenuMobile-basketProductContent\">" +
                                "<div class=\"ct-shopMenuMobile-basketProductTitle\">" + item.ProductName + "</div>" +
                                "<div class=\"ct-shopMenuMobile-basketProductPrice ct-fw-600\">" + parseFloat(item.TotalAmount).toFixed(2) + " <i class=\"fa fa-try\" aria-hidden=\"true\"></i></div>" +
                            "</div>" +
                            "<div class=\"clearfix\"></div>" +
                        "</a>" +
                    "</li>";

                    var rowTemplate = "<div class=\"ct-cartItem\">" +
                            "<a href=\"/product/" + item.Url + "/detail\">" +
                                "<div class=\"ct-cartItem-title\">" + item.ProductName + "</div>" +
                                "<div class=\"ct-cartItem-price\">Adet: " + item.Unit + "</div>" +
                                "<div class=\"ct-cartItem-price\">Fiyat: " + parseFloat(item.TotalAmount).toFixed(2) + " <i class=\"fa fa-try\" aria-hidden=\"true\"></i></div>" +
                                "<div class=\"clearfix\"></div>" +
                            "</a>" +
                        "</div>" +
                        "<div class=\"clearfix\"></div>";

                    $("#basketMiniProductList").append(rowTemplate);
                    $("#mobileBasketMiniProductList").append(mobileRowTemplate);
                    console.log(mobileRowTemplate);
                }

                var basketCampigns = result.BasketCampigns;

                $("#discountBasketAmount").html(parseFloat(basketCampigns.DiscountTotalAmount).toFixed(2) + " <i class=\"fa fa-try\" aria-hidden=\"true\"></i>");
                $("#orginalBasketTotalAmount").html(basketTotalAmount.toFixed(2) + " <i class=\"fa fa-try\" aria-hidden=\"true\"></i>");
                $("#topBasketListWrapper").show();
            } else {
                $("#topBasketListWrapper").hide();
                $("#basketListNotFound").show();
            }
        },
        complete: function () {
            hideLoading();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR.responseText + " " + textStatus + " " + errorThrown);
            showMessageBox(jqXHR.responseText);
            hideLoading();
        }
    });
}

function setAccountCountry() {
    var country = $("#Country").val();
    $("#AccountCountry option:contains(" + country + ")").attr("selected", true);
    setAccountCity();
}

function setAccountCity() {
    var selectedCountry = $("#AccountCountry").val();

    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: "GET",
        url: "/user/city",
        data: {
            countryId: selectedCountry
        },
        success: function (response) {
            $('#AccountCity').empty();

            $.each(response, function (index, value) {
                $("#AccountCity").append($('<option>',
                    {
                        value: value.Id,
                        text: value.Name
                    }));
            });

            $('#AccountRegion').empty();

            var country = $("#City").val();
            $("#AccountCity option:contains(" + country + ")").attr("selected", true);
        },
        complete: function () {
            setAccountRegion();
        }
    });
}

function setAccountRegion() {
    var selectedCity = $("#AccountCity").val();

    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: "GET",
        url: "/user/city/region",
        data: {
            cityId: selectedCity
        },
        success: function (response) {
            console.log(response);
            $('#AccountRegion').empty();

            $.each(response, function (index, value) {
                $("#AccountRegion").append($('<option>',
                    {
                        value: value.Id,
                        text: value.Name
                    }));
            });
        },
        complete: function () {
            var country = $("#Region").val();
            $("#AccountRegion option:contains(" + country + ")").attr("selected", true);
        }
    });
}

function setAccountGender() {
    var gender = $("#hfGender").val();
    $("#ddlAccountGender option:contains(" + gender + ")").attr("selected", true);
}

function addFavorite(productName, productUrl, productId) {
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: "POST",
        url: "/user/favorite",
        data: JSON.stringify({
            productId: productId,
            productName: productName,
            productUrl: productUrl
        }),
        beforeSend: function() {
            showLoading();
        },
        success: function(result) {
            showSuccessMessageBox(result);
        },
        complete: function() {
            hideLoading();
        },
        error: function(jqXHR, textStatus, errorThrown) {
            console.log(jqXHR.responseText + " " + textStatus + " " + errorThrown);
            showMessageBox(jqXHR.responseText);
            hideLoading();
        }
    });
}

function addBulletin() {
    var email = $("#bullettinEmail").val();

    if (email.length < 5)
        return;

    if (!isValidEmailAddress(email)) {
        showMessageBox("E-Mail formatı hatalı. Lütfen tekrar deneyiniz.");
        return;
    }

    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: "POST",
        url: "/bulletin/add",
        data: JSON.stringify({
            email: email
        }),
        beforeSend: function () {
            showLoading();
        },
        success: function (result) {
            showSuccessMessageBox(result);
            $("#bullettinEmail").val("");
        },
        complete: function () {
            hideLoading();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR.responseText + " " + textStatus + " " + errorThrown);
            showMessageBox(jqXHR.responseText);
            hideLoading();
        }
    });
}

(function ($) {
    "use strict";
    $(document).ready(function () {
        //if (!(device.mobile() || device.ipad() || device.androidTablet())) {
        //    $(".ct-js-zoomImage").elevateZoom({
        //        zoomType: "lens",
        //        lensSize: 250
        //    });
        //}

        $("#btnCommentSend").click(function() {
            var message = $("#commentMessage").val();
            var productId = $("#favProductId").val();

            if (message == "") {
                showMessageBox("Yorum boş bırakılamaz.");
                return;
            }

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                url: "/product/comment",
                data: JSON.stringify({
                    productId: productId,
                    message: message
                }),
                beforeSend: function () {
                    showLoading();
                },
                success: function () {
                    $("#commentForm").hide();
                    $("#commentSuccess").show();
                },
                complete: function () {
                    hideLoading();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText + " " + textStatus + " " + errorThrown);
                    showMessageBox(jqXHR.responseText);
                    hideLoading();
                }
            });
        });

        $("#btnBullettinEmail").click(function () {
            addBulletin();
        });

        $(".filterTable").DataTable();

        $(".addFavorite").click(function () {
            var userId = $(this).parent("div").find("#favUserId").val();
            if (userId && userId != "0") {
                var productName = $(this).parent("div").find("#favProductName").val();
                var productUrl = $(this).parent("div").find("#favProductUrl").val();
                var productId = $(this).parent("div").find("#favProductId").val();

                addFavorite(productName, productUrl, productId);
            } else
                showMessageBox("Favorilere eklemek için giriş yapmalısınız.");
        });

        $(".userOrderDetail").fancybox({
            width: '70%'
        });

        getMiniBasketList();

        $(".deliverySelected").click(function () {
            $("#btnDeliverySubmit").show();
        });

        $(".invoiceSelected").click(function () {
            $("#btnInvoiceSubmit").show();
        });

        $(".payDoorSelected").click(function () {
            $("#btnPayDoor").show();
        });

        $(".bankSelected").click(function () {
            $("#btnPayBank").show();
        });

        $("#isInvoiceAndDelivery").click(function () {
            if ($(this).is(':checked'))
                $("#deliveryInvoiceDetail").show();
            else
                $("#deliveryInvoiceDetail").hide();
        });

        $(".addBasket").click(function () {
            var redirect = $(this).attr("data-redirect");
            var id = $(this).parent().find(".basketId").val();
            var name = $(this).parent().find(".basketName").val();
            var unit = $(".basketUnit").val();
            var sizes = [];
            var sizenames = [];

            //console.log(id + " - " + name + " - " + unit);

            $.each($(".size"), function () {
                var val = $(this).val();
                var sizename = $(this).attr("data-measure-name");
                sizenames.push(sizename);
                sizes.push(val);
            });

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                url: "/basket/add",
                data: JSON.stringify({
                    id: id,
                    unit: unit,
                    name: name,
                    measureIds: sizes,
                    measureNames: sizenames
                }),
                beforeSend: function () {
                    showLoading();
                },
                success: function (message) {
                    if (message == "Ok") {
                        getMiniBasketList();
                        showSuccessMessageBox("Ürün sepete eklendi. <a href='/basket' style='color:#000; font-weight:bold;'>Sepete gitmek için tıklayınız.</a>");
                    }
                    else
                        showMessageBox(message);
                },
                complete: function () {
                    hideLoading();
                    console.log(redirect);
                    if (redirect && redirect != "") {
                        location.href = redirect;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText + " " + textStatus + " " + errorThrown);
                    showMessageBox(jqXHR.responseText);
                }
            });
        });

        $(".showMessageBox, .showSuccessMessageBox").click(function () {
            hideMessageBox();
        });

        $("#AccountCountry").change(function () {
            var selectedCountryText = $("#AccountCountry option:selected").text();
            $("#Country").val(selectedCountryText);

            setAccountCity();

            var selectedCityText = $("#AccountCity option:selected").text();
            $("#City").val(selectedCityText);

            $("#Region").val("");
        });

        $("#AccountCity").change(function () {
            var selectedCityText = $("#AccountCity option:selected").text();
            $("#City").val(selectedCityText);

            setAccountRegion();

            var selectedRegionText = $("#AccountRegion option:selected").text();
            $("#Region").val(selectedRegionText);
        });

        $("#AccountRegion").change(function () {
            var selectedRegionText = $("#AccountRegion option:selected").text();
            $("#Region").val(selectedRegionText);
        });

        $(document).on("click", "a.basketListUpdateItemButton", function () {
            var basketId = $(this).attr("data-basketId");
            var number = $(this).parent().parent().find(".basketListUpdateItemNumber").val();

            if (number < 1)
                number = 1;
            else if (number > 30)
                number = 30;

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                url: "/basket/set-basket",
                data: JSON.stringify({
                    basketId: basketId,
                    count: number
                }),
                beforeSend: function () {
                    showLoading();
                },
                success: function (result) {
                    hideLoading();
                    showSuccessMessageBox("Sepet güncellendi.");
                    location.href = "/basket";
                },
                complete: function () {
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText + " - " + textStatus + " - " + errorThrown);
                    showMessageBox(jqXHR.responseText);
                }
            });
        });

        $(document).on("click", "a.basketListItemDelete", function () {
            var basketId = $(this).attr("data-basketId");

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                url: "/basket/delete-basket",
                data: JSON.stringify({
                    basketId: basketId
                }),
                beforeSend: function () {
                    showLoading();
                },
                success: function (result) {
                    hideLoading();
                    showSuccessMessageBox("Sepet güncellendi.");
                    location.href = "/basket";
                },
                complete: function () {
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText + " - " + textStatus + " - " + errorThrown);
                    showMessageBox(jqXHR.responseText);
                }
            });
        });

        //delivery--------------------------------
        $("#deliveryCancel").click(function () {
            $("#id").val("");
            $("#addressSaveName").val("");
            $("#nameSurname").val("");
            $("#addressDetail").val("");
            $("#city").val("");
            $("#region").val("");
            $("#phone").val("");
            $("#gsm").val("");
            $("#tcNr").val("");
            $("#addressNote").val("");
            $("#status").val("");

            $("#deliveryUpdatePanel").hide();
        });
        $("#deliverySave").click(function () {
            var id = $("#id").val();
            var addressSaveName = $("#addressSaveName").val();
            var nameSurname = $("#nameSurname").val();
            var addressDetail = $("#addressDetail").val();
            var city = $("#city").val();
            var region = $("#region").val();
            var phone = $("#phone").val();
            var gsm = $("#gsm").val();
            var tcNr = $("#tcNr").val();
            var addressNote = $("#addressNote").val();
            var status = $("#status").val();

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                url: "/user/address/set-delivery",
                data: JSON.stringify({
                    id: id,
                    addressSaveName: addressSaveName,
                    nameSurname: nameSurname,
                    addressDetail: addressDetail,
                    city: city,
                    region: region,
                    phone: phone,
                    gsm: gsm,
                    tcNr: tcNr,
                    addressNote: addressNote,
                    status: status
                }),
                beforeSend: function () {
                    showLoading();
                },
                success: function (result) {
                    showSuccessMessageBox(result);
                    location.href = "/user/address/delivery";
                },
                complete: function () {
                    hideLoading();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText + " " + textStatus + " " + errorThrown);
                    showMessageBox(jqXHR.responseText);
                }
            });
        });

        $("#deliveryNew").click(function () {
            $("#statusNew").val("1");
            $("#deliveryNewPanel").show();
            $("#deliveryUpdatePanel").hide();
        });
        $("#deliveryNewCancel").click(function () {
            $("#addressNewSaveName").val("");
            $("#nameNewSurname").val("");
            $("#addressNewDetail").val("");
            $("#cityNew").val("");
            $("#regionNew").val("");
            $("#phoneNew").val("");
            $("#gsmNew").val("");
            $("#tcNewNr").val("");
            $("#addressNewNote").val("");
            $("#statusNew").val("");

            $("#deliveryNewPanel").hide();
        });
        $("#deliveryNewSave").click(function () {
            var addressSaveName = $("#addressNewSaveName").val();
            var nameSurname = $("#nameNewSurname").val();
            var addressDetail = $("#addressNewDetail").val();
            var city = $("#cityNew").val();
            var region = $("#regionNew").val();
            var phone = $("#phoneNew").val();
            var gsm = $("#gsmNew").val();
            var tcNr = $("#tcNewNr").val();
            var addressNote = $("#addressNewNote").val();
            var status = $("#statusNew").val();

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                url: "/user/address/add-delivery",
                data: JSON.stringify({
                    addressSaveName: addressSaveName,
                    nameSurname: nameSurname,
                    addressDetail: addressDetail,
                    city: city,
                    region: region,
                    phone: phone,
                    gsm: gsm,
                    tcNr: tcNr,
                    addressNote: addressNote,
                    status: status
                }),
                beforeSend: function () {
                    showLoading();
                },
                success: function (result) {
                    $("#deliveryNewPanel").hide();
                    showSuccessMessageBox(result);

                    var redirectUrl = $("#deliveryNewSave").attr("data-redirect");

                    location.href = redirectUrl;
                },
                complete: function () {
                    hideLoading();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText + " " + textStatus + " " + errorThrown);
                    showMessageBox(jqXHR.responseText);
                }
            });
        });

        $(".accountDeliveryEdit").click(function () {
            var id = $(this).attr("data-id");

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "GET",
                url: "/user/address/get-delivery",
                data: {
                    id: id
                },
                beforeSend: function () {
                    showLoading();
                },
                success: function (delivery) {
                    $("#id").val(delivery.Id);
                    $("#addressSaveName").val(delivery.AddressSaveName);
                    $("#nameSurname").val(delivery.NameSurname);
                    $("#addressDetail").text(delivery.AddressDetail);
                    $("#city").val(delivery.City);
                    $("#region").val(delivery.Region);
                    $("#phone").val(delivery.Phone);
                    $("#gsm").val(delivery.Gsm);
                    $("#tcNr").val(delivery.TcNr);
                    $("#addressNote").text(delivery.AddressNote);
                    $("#status").val(delivery.Status);

                    $("#deliveryUpdatePanel").show();
                },
                complete: function () {
                    hideLoading();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText + " " + textStatus + " " + errorThrown);
                    showMessageBox(jqXHR.responseText);
                }
            });
        });
        $(".accountDeliveryDelete").click(function () {
            var id = $(this).attr("data-id");

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "GET",
                url: "/user/address/delete-delivery",
                data: {
                    id: id
                },
                beforeSend: function () {
                    showLoading();
                },
                success: function () {
                    location.href = "/user/address/delivery";
                },
                complete: function () {
                    hideLoading();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText + " " + textStatus + " " + errorThrown);
                    showMessageBox(jqXHR.responseText);
                }
            });
        });

        //invoice--------------------------------
        $("#invoiceCancel").click(function () {
            $("#id").val("");
            $("#invoiceSaveName").val("");
            $("#nameSurname").val("");
            $("#invoiceType").val("0");
            $("#phone").val("");
            $("#gsm").val("");
            $("#country").val("");
            $("#city").val("");
            $("#region").val("");
            $("#taxNr").val("");
            $("#taxOffice").val("");
            $("#address").val("");
            $("#status").val("0");

            $("#invoiceUpdatePanel").hide();
        });
        $("#invoiceSave").click(function () {
            var id = $("#id").val();
            var invoiceSaveName = $("#invoiceSaveName").val();
            var nameSurname = $("#nameSurname").val();
            var invoiceType = $("#invoiceType").val();
            var phone = $("#phone").val();
            var gsm = $("#gsm").val();
            var country = $("#country").val();
            var city = $("#city").val();
            var region = $("#region").val();
            var taxNr = $("#taxNr").val();
            var taxOffice = $("#taxOffice").val();
            var address = $("#address").val();
            var status = $("#status").val();

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                url: "/user/address/set-invoice",
                data: JSON.stringify({
                    id: id,
                    invoiceSaveName: invoiceSaveName,
                    nameSurname: nameSurname,
                    invoiceType: invoiceType,
                    phone: phone,
                    gsm: gsm,
                    country: country,
                    city: city,
                    region: region,
                    taxNr: taxNr,
                    taxOffice: taxOffice,
                    address: address,
                    status: status
                }),
                beforeSend: function () {
                    showLoading();
                },
                success: function (result) {
                    showSuccessMessageBox(result);
                    $("#invoiceUpdatePanel").hide();
                    location.href = "/user/address/invoice";
                },
                complete: function () {
                    hideLoading();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText + " " + textStatus + " " + errorThrown);
                    showMessageBox(jqXHR.responseText);
                }
            });
        });

        $("#invoiceNew").click(function () {
            $("#statusNew").val("1");
            $("#invoiceNewType").val("0");
            $("#invoiceNewPanel").show();
            $("#taxOfficeNewBox").hide();
            $("#invoiceUpdatePanel").hide();
        });
        $("#invoiceNewCancel").click(function () {
            $("#invoiceNewSaveName").val("");
            $("#nameNewSurname").val("");
            $("#phoneNew").val("");
            $("#gsmNew").val("");
            $("#countryNew").val("");
            $("#cityNew").val("");
            $("#regionNew").val("");
            $("#taxNrNew").val("");
            $("#taxOfficeNew").val("");
            $("#addressNew").val("");
            $("#statusNew").val("0");

            $("#invoiceNewPanel").hide();
        });
        $("#invoiceNewSave").click(function () {
            var invoiceSaveName = $("#invoiceNewSaveName").val();
            var nameSurname = $("#nameNewSurname").val();
            var invoiceType = $("#invoiceNewType").val();
            var phone = $("#phoneNew").val();
            var gsm = $("#gsmNew").val();
            var country = $("#countryNew").val();
            var city = $("#cityNew").val();
            var region = $("#regionNew").val();
            var taxNr = $("#taxNrNew").val();
            var taxOffice = $("#taxOfficeNew").val();
            var address = $("#addressNew").val();
            var status = $("#statusNew").val();

            var returnUrl = $(this).attr("data-redirect");

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                url: "/user/address/add-invoice",
                data: JSON.stringify({
                    invoiceSaveName: invoiceSaveName,
                    nameSurname: nameSurname,
                    invoiceType: invoiceType,
                    phone: phone,
                    gsm: gsm,
                    country: country,
                    city: city,
                    region: region,
                    taxNr: taxNr,
                    taxOffice: taxOffice,
                    address: address,
                    status: status
                }),
                beforeSend: function () {
                    showLoading();
                },
                success: function (result) {
                    showSuccessMessageBox(result);
                    $("#invoiceNewPanel").hide();
                    location.href = returnUrl;
                },
                complete: function () {
                    hideLoading();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText + " " + textStatus + " " + errorThrown);
                    showMessageBox(jqXHR.responseText);
                }
            });
        });

        $(".accountInvoiceEdit").click(function () {
            var id = $(this).attr("data-id");

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "GET",
                url: "/user/address/get-invoice",
                data: {
                    id: id
                },
                beforeSend: function () {
                    showLoading();
                },
                success: function (invoice) {
                    $("#id").val(invoice.Id);
                    $("#invoiceSaveName").val(invoice.InvoiceSaveName);
                    $("#nameSurname").val(invoice.NameSurname);
                    $("#invoiceType").val(invoice.InvoiceType);
                    $("#phone").val(invoice.Phone);
                    $("#gsm").val(invoice.Gsm);
                    $("#country").val(invoice.Country);
                    $("#city").val(invoice.City);
                    $("#region").val(invoice.Region);
                    $("#taxNr").val(invoice.TaxNr);
                    $("#taxOffice").val(invoice.TaxOffice);
                    $("#address").val(invoice.Address);
                    $("#status").val(invoice.Status);

                    if (invoice.InvoiceType == "1")
                        $("#taxOfficeBox").show();
                    else
                        $("#taxOfficeBox").hide();

                    $("#invoiceUpdatePanel").show();
                },
                complete: function () {
                    hideLoading();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText + " " + textStatus + " " + errorThrown);
                    showMessageBox(jqXHR.responseText);
                }
            });
        });
        $(".accountInvoiceDelete").click(function () {
            var id = $(this).attr("data-id");

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "GET",
                url: "/user/address/delete-invoice",
                data: {
                    id: id
                },
                beforeSend: function () {
                    showLoading();
                },
                success: function () {
                    location.href = "/user/address/invoice";
                },
                complete: function () {
                    hideLoading();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText + " " + textStatus + " " + errorThrown);
                    showMessageBox(jqXHR.responseText);
                }
            });
        });

        $("#invoiceType").change(function () {
            var selected = $(this).val();
            if (selected == "1")
                $("#taxOfficeBox").show();
            else
                $("#taxOfficeBox").hide();
        });

        $("#invoiceNewType").change(function () {
            var selected = $(this).val();
            if (selected == "1")
                $("#taxOfficeNewBox").show();
            else
                $("#taxOfficeNewBox").hide();
        });

        // Add Color // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        $(".ct-js-color").each(function () {
            $(this).css("color", '#' + $(this).attr("data-color"))
        });
        // Add Space // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        $(".ct-js-spacer").each(function () {
            $(this).css("height", $(this).attr("data-value"))
        });
        // Snap Navigation in Mobile // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        if ($devicewidth > 767 && document.getElementById('ct-js-wrapper')) {
            try {
                snapper.disable();
            } catch (e) { }
        }
        $(".navbar-toggle").click(function () {
            if ($("body").hasClass('snapjs-left')) {
                snapper.close();
            } else {
                snapper.open('left');
            }
        });
        $(".navbarShop-toggle").click(function () {
            if ($("body").hasClass('snapjs-right')) {
                snapper.close();
            } else {
                snapper.open('right');
            }
        });
        $('.ct-menuMobile .ct-menuMobile-navbar .dropdown > a').click(function (e) {
            return false; // iOS SUCKS
        });
        $('.ct-menuMobile .ct-menuMobile-navbar .dropdown > a').click(function (e) {
            var $this = $(this);
            if ($this.parent().hasClass('open')) {
                $(this).parent().removeClass('open');
            } else {
                $('.ct-menuMobile .ct-menuMobile-navbar .dropdown.open').toggleClass('open');
                $(this).parent().addClass('open');
            }
        });
        // Animations Init // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        if ($().appear) {
            if (device.mobile() || device.tablet()) {
                $("body").removeClass("cssAnimate");
            } else {

                $('.cssAnimate .animated').appear(function () {
                    var $this = $(this);

                    $this.each(function () {
                        if ($this.data('time') != undefined) {
                            setTimeout(function () {
                                $this.addClass('activate');
                                $this.addClass($this.data('fx'));
                            }, $this.data('time'));
                        } else {
                            $this.addClass('activate');
                            $this.addClass($this.data('fx'));
                        }
                    });
                }, { accX: 50, accY: -350 });
            }
        }
        // Tooltips and Popovers // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        $("[data-toggle='tooltip']").tooltip();
        $("[data-toggle='popover']").popover({ trigger: "hover", html: true });
        // Link Scroll to Section // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        $('.ct-js-btnScroll[href^="#"]').click(function (e) {
            e.preventDefault();

            var target = this.hash, $target = $(target);

            $('html, body').stop().animate({
                'scrollTop': $target.offset().top - 70
            }, 900, 'swing', function () {
                window.location.hash = target;
            });
        });
        $('.ct-js-btnScrollUp').click(function (e) {
            e.preventDefault();
            $("body,html").animate({ scrollTop: 0 }, 1200);
            return false;
        });
        // Navbar Search // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        var $searchform = $(".ct-navbar-search");
        $('#ct-js-navSearch').click(function (e) {
            e.preventDefault();
            $navbarel.addClass('is-inactive');


            $searchform.fadeIn();

            if (($searchform).is(":visible")) {
                $searchform.find("[type=text]").focus();
            }

            return false;
        })
        // Placeholder Fallback // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        if ($().placeholder) {
            $("input[placeholder],textarea[placeholder]").placeholder();
        }
        /* ******************
         MAGIC BORDER IN MENU
         ******************** */
        if ($devicewidth > 767) {
            $(function () {

                var $el, leftPos, newWidth, $mainNav = $(".navbar-nav");

                if ($mainNav.length > 0) {
                    $mainNav.each(function () {
                        var $this = $(this);
                        //if ($this.find("> li.active").length > 0) {
                        if ($this.find("> li.active").length > 0) {
                            $this.append("<li id='magic-line'></li>");
                            var $magicLine = $("#magic-line");

                            $magicLine.width($mainNav.find("> li.active").width()).css("left", $mainNav.find("> li.active").position().left).data("origLeft", $magicLine.position().left).data("origWidth", $magicLine.width());

                            $(".navbar-nav > li").hover(function () {
                                $el = $(this);
                                leftPos = $el.position().left;
                                newWidth = $el.width();
                                $magicLine.stop().animate({
                                    left: leftPos, width: newWidth
                                });
                            }, function () {
                                if ($('body').hasClass('onepager')) {
                                    $magicLine.stop().animate({
                                        left: $mainNav.find("> li.active").position().left, width: $mainNav.find("> li.active").width()
                                    });
                                } else {
                                    $magicLine.stop().animate({
                                        left: $magicLine.data("origLeft"), width: $magicLine.data("origWidth")
                                    });
                                }
                            });
                            if ($().attrchange) {
                                $mainNav.find('> li:not(#magic-line)').attrchange({
                                    trackValues: true, /* Default to false, if set to true the event object is
                                     updated with old and new value.*/
                                    callback: function (event) {
                                        //event    	          - event object
                                        //event.attributeName - Name of the attribute modified
                                        //event.oldValue      - Previous value of the modified attribute
                                        //event.newValue      - New value of the modified attribute
                                        //Triggered when the selected elements attribute is added/updated/removed
                                        $magicLine.stop().animate({
                                            left: $mainNav.find("> li.active").position().left, width: $mainNav.find("> li.active").width()
                                        });
                                    }
                                });
                            }
                        }
                    })
                }
            });
        }
        // INIT SCRIPTS FILES -------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //STACKTABLE
        $('.ct-js-wishList').stacktable();
        //MAGNIFIC POPUP
        if (jQuery().magnificPopup) {
            jQuery('.ct-js-popupGallery').each(function () { // the containers for all your galleries
                jQuery(this).magnificPopup({
                    disableOn: 700,
                    type: 'image',
                    mainClass: 'ct-magnificPopup--image',
                    removalDelay: 160,
                    preloader: true,
                    delegate: '.ct-js-magnificPopupImage',
                    closeBtnInside: true,
                    closeOnContentClick: false,
                    closeOnBgClick: true,
                    gallery: {
                        enabled: true
                    }
                });
            });
        }
        /*
         ****************
         shopList and shopDefault displaying and hiding elements by buttons
         ****************
         */

        var $tilesItems = $("#ct-js-showTiles");
        var $listItems = $("#ct-js-showList");
        var $showProducts = $(".ct-showProducts");

        if (typeof (Storage) === "undefined"){
            localStorage.setItem("selectedViewType", "categoryView");

            var $existListClass1 = $(".ct-showProducts--default");

            if ($existListClass1) {
                $showProducts.removeClass("ct-showProducts--list");
                $showProducts.addClass("ct-showProducts--default");
                $showProducts.css("display", "none");
                $showProducts.fadeIn();

                $(this).addClass("is-active");
                $listItems.removeClass("is-active");
            }
        }

        if (typeof (Storage) !== "undefined") {
            var selectedViewType = localStorage.getItem("selectedViewType");

            if (selectedViewType === "listView") {
                var $existListClass = $(".ct-showProducts--list");

                if ($existListClass) {
                    $showProducts.removeClass("ct-showProducts--default");
                    $showProducts.addClass("ct-showProducts--list");
                    $showProducts.css("display", "none");
                    $showProducts.fadeIn();

                    $listItems.addClass("is-active");
                    $tilesItems.removeClass("is-active");
                }
            } else {
                var $existDefaultClass = $(".ct-showProducts--default");

                if ($existDefaultClass) {
                    $showProducts.removeClass("ct-showProducts--list");
                    $showProducts.addClass("ct-showProducts--default");
                    $showProducts.css("display", "none");
                    $showProducts.fadeIn();

                    $listItems.removeClass("is-active");
                    //$("#ct-js-showList").addClass("is-active");
                    $tilesItems.addClass("is-active");
                }
            }
        }

        if ($tilesItems && $listItems) {
            $tilesItems.on("click", function (e) {
                e.preventDefault();

                localStorage.setItem("selectedViewType", "categoryView");

                var $existListClass = $(".ct-showProducts--default");

                if ($existListClass) {
                    $showProducts.removeClass("ct-showProducts--list");
                    $showProducts.addClass("ct-showProducts--default");
                    $showProducts.css("display", "none");
                    $showProducts.fadeIn();

                    $(this).addClass("is-active");
                    $listItems.removeClass("is-active");
                }
            });

            $listItems.on("click", function (e) {
                e.preventDefault();

                localStorage.setItem("selectedViewType", "listView");

                var $existDefaultClass = $(".ct-showProducts--list");

                if ($existDefaultClass) {
                    $showProducts.removeClass("ct-showProducts--default");
                    $showProducts.addClass("ct-showProducts--list");
                    $showProducts.css("display", "none");
                    $showProducts.fadeIn();

                    $(this).addClass("is-active");
                    $tilesItems.removeClass("is-active");
                }
            });
        }
        /* WISHLIST PRODUCTS DELETING ITEMS FROM CURRENT LIST*/
        var $buttonX = $(".ct-wishList .ct-js-buttonX");
        if ($devicewidth > 768) {
            var pom = 0;
            var $wishTr = $("table.ct-wishList.large-only tbody tr");
            var $len = $wishTr.length;


            $buttonX.on("click", function () {
                $(this).closest('tr').fadeOut();


                $wishTr.each(function () {
                    if ($(this).css("display") === "none") {
                        pom += 1;
                    }
                });

                //if we have displayed all elements to none, then we can show the announcement about there are no any products here
                if (pom == $len) {
                    $(".ct-wishList-noProducts").fadeIn();
                    $(".ct-shopSections").fadeOut();
                }

                return false;
            });

            if ($(".ct-wishList.large-only tbody tr").length === 0) {
                $(".ct-wishList-noProducts").css("display", "block");
                $(".ct-shopSections").css("display", "none");
            }
        }
        else {

            //Hide one element which is single
            $(".st-head-row-main").hide();

            $buttonX.on("click", function () {

                /* Script wishtlist works in different amount of columns so we need to have an if for checking different tables where 1 column more */
                var $howMany;
                if ($("table.ct-wishList.ct-js-cartShop.small-only").length == 1) {
                    $howMany = 5;
                }
                else {
                    $howMany = 4;
                }

                /* stacktable js is creating another table via js with a lot of rows so we need to delete 4 or 5 previous rows together with button row ofc*/
                for (var i = 0; i < $howMany; i++) {
                    $(this).closest('tr').prev().remove();
                }
                $(this).closest('tr').remove();

                /* On mobile it will stay one row tr > td.st-head-row-main which we needed to hide earlier */
                if ($(".ct-wishList.small-only tbody tr").length == 1) {
                    $(".ct-wishList-noProducts").css("display", "block");
                    $(".ct-shopSections").css("display", "none");

                }
                return false;
            });

            /* On mobile it will stay one row tr > td.st-head-row-main which we needed to hide earlier */
            if ($(".ct-wishList.small-only tbody tr").length === 1) {
                $(".ct-wishList-noProducts").css("display", "block");
                $(".ct-shopSections").css("display", "none");
            }
        }
        // CALCULATE SHIPPING - SHOW BLOCK OF ELEMENTS AND HIDE, HIDING SHOP sections when there is no any elements */
        $("#ct-js-calculateShipping").on("click", function () {

            var $shipSection = $(".ct-calculateShippingSection");

            if ($(".ct-calculateShippingSection.is-active").length < 1) {
                //$shipSection.fadeIn();
                $shipSection.css("height", "auto");
                $shipSection.css("opacity", "1");
                $shipSection.addClass("is-active");
                $(".ct-js-changeTriangle").removeClass("ct-triangleDown");
                $(".ct-js-changeTriangle").addClass("ct-triangleUp");
            }
            else {
                //$shipSection.fadeOut();
                $shipSection.css("height", "0");
                $shipSection.css("opacity", "0");
                $shipSection.removeClass("is-active");
                $(".ct-js-changeTriangle").removeClass("ct-triangleUp");
                $(".ct-js-changeTriangle").addClass("ct-triangleDown");
            }
        });
        // DISPLAY DIFFERENT ADDRESS FORM in checkout
        $(".ct-js-differentAddress").on("click", function () {

            var $differentAddressSection = $(".ct-differentShippingAddress");

            if ($(".ct-differentShippingAddress.is-active").length < 1) {
                //$differentAddressSection.fadeIn();
                $differentAddressSection.css("height", "auto");
                $differentAddressSection.css("opacity", "1");
                $differentAddressSection.addClass("is-active");
                $("#differentAddress").prop("checked", true);
            }
            else {
                $differentAddressSection.css("height", "0");
                $differentAddressSection.css("opacity", "0");
                //$differentAddressSection.fadeOut();
                $differentAddressSection.removeClass("is-active");
                $("#differentAddress").prop("checked", false);
            }

            return false;
        });
        //RESIZE TEXT + -
        $('a.plus').on("click", function () {
            var curSize = parseInt($('.adjust-text').css('font-size'), 10) + 1;
            if (curSize <= 24)
                $('.adjust-text').css('font-size', curSize);
            return false;
        });
        $('a.minus').on("click", function () {
            var curSize = parseInt($('.adjust-text').css('font-size'), 10) - 1;
            if (curSize >= 10)
                $('.adjust-text').css('font-size', curSize);
            return false;
        });
        //VIdeo TAbs
        $('.ct-videoBox:first').css('display', 'block');
        $('.ct-videoProduct').on("click", function () {
            if ($('.ct-videoProduct.is-active')) {
                $('.ct-videoProduct.is-active').each(function () {
                    $(this).removeClass('is-active');
                });
            }
            if (!($(this).hasClass('is-active')))
                $(this).addClass('is-active');

            var elnum = $(this).index();
            $('.ct-videoBox').each(function () {
                $(this).css("display", "none");
            });
            $('.ct-videoBox').eq(elnum).css("display", "block");
        });
        //Colourful stars in single products ratings
        $(".ct-ratingsRight ul li span:nth-child(2) span").each(function () {
            if ($(this).attr("data-width")) {
                var $barWidth = $(this).attr("data-width");
                $(this).css("width", $barWidth + '%');
            }
        });
        //MULTIPLE SEARCHER
        $(".ct-js-filter").on("click", function () {
            var $this = $(this);
            var next = $this.next();

            if (next.hasClass("open")) {
                next.hide();
                next.removeClass("open");
                if ($this.hasClass("active")) {
                    $this.removeClass("active");
                }
            }
            else {
                next.addClass("open");
                next.show();
                $this.addClass("active");
            }
            return false;
        });
        //ADDING EXTRA DOLAR TO INPUTS
        $('#lower-value').val('$' + $('#lower-value').val());
        $('#upper-value').val('$' + $('#upper-value').val());
        //DATA Bg in highlights
        if ($(".ct-highlightsItem-content").length > 0) {
            $(".ct-highlightsItem-content").each(function () {
                if ($(this).attr("data-bg")) {
                    $(this).css('background-image', 'url("' + $(this).attr("data-bg") + '")');
                }
                else {
                    $(this).css("background-color", "grey");
                }
            });
        }
    });
    $(window).on('resize', function () {
        if ($(window).width() < 768) {
            snapper.enable();
        } else {
            try {
                snapper.close();
                snapper.disable();
            } catch (e) { }
        }
    });
    $(document).mouseup(function (e) {
        var $searchform = $(".ct-navbar-search");

        if (!$('#ct-js-navSearch').is(e.target)) {
            if (!$searchform.is(e.target) // if the target of the click isn't the container...
                && $searchform.has(e.target).length === 0) // ... nor a descendant of the container
            {
                $navbarel.removeClass('is-inactive');
                $searchform.fadeOut();
            }
        }
    });
    $(window).on("load", function () {
        // Masonry For Sidebar // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        if (jQuery().masonry && (jQuery(window).width() < 992) && (jQuery(window).width() > 767)) {

            jQuery('.ct-js-sidebar .row').masonry({
                itemSelector: '.col-sm-6.col-md-12',
                layoutMode: 'sloppyMasonry',
                resizable: false, // disable normal resizing
                // set columnWidth to a percentage of container width
                masonry: {}
            });
        }

        var $preloader = $('.ct-preloader');
        var $content = $('.ct-preloader-content');

        var $timeout = setTimeout(function () {
            $($preloader).addClass('animated').addClass('fadeOut');
            $($content).addClass('animated').addClass('fadeOut');
        }, 0);
        var $timeout2 = setTimeout(function () {
            $($preloader).css('display', 'none').css('z-index', '-9999');
        }, 500);
    });
})(jQuery);