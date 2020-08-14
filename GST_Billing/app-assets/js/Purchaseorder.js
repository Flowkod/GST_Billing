$(document).ready(function () {
  

    /* Dropdown close onclick of cancel btn*/
    $(document).on("click", ".invoice-cancel-btn", function () {
        $("#DiscountSpan").html('0%');
         $("#CGSTSpan").html('0%');
        $("#SGSTSpan").html('0%');
        $("#Discount").val('');
        $("#TaxList").val('').change();

        $('.dropdown-button').dropdown("close");
    });

    /* Initialize Dropdown */
    $('.dropdown-button').dropdown({
        constrainWidth: false, // Does not change width of dropdown to that of the activator
        closeOnClick: false
    });

})

$(document).ready(function () {

    if (localStorage.Company_Logo != "") {
        $(".logo-div").append("<img src = " + localStorage.Company_Logo + " alt = 'logo' width = '164' />");
    }
    else {
        $(".logo-div").append("<h4 class='indigo-text orange-text companyname'>" + localStorage.Company_Name + "</h4>");
    }

});

$(".item").select2({
    /* the following code is used to disable x-scrollbar when click in select input and
    take 100% width in responsive also */
    dropdownAutoWidth: true,
    width: '100%',
}).select2()
    .on('select2:open', () => {
        $(".select2-results:not(:has(a))").append('<div style="width:100%; border-top:solid 1px #cccccc; padding:15px;"><a id="AddItemClick" class="modal-trigger" href="#modal1"><i class="material-icons left">add</i> Add New</a></div>');
    });


$(document).on('change', '.PoContent', function () {
    PoActionManagement($(this));
});

function PoActionManagement(htmlcontrol) {
    var name = $(htmlcontrol).attr('datavalue');

    if ($(htmlcontrol).prop("checked") == true) {
        $("#" + name + "Div").removeClass("hide");
    }
    else {
        $("#" + name + "Div").addClass("hide");
    }
}

$(document).on('change', '#ShippingAddressType', function () {

    ShipAddressManagement($(this));
});

function ShipAddressManagement(htmlcontrol) {
    var value = $(htmlcontrol).attr('value');

    $("#ShipClientAddressDiv").addClass("hide");

    if (value == "ShipCompanyAddress") {
        $("#ClientList").val('');
        $("#ClientList").change();
        $("#Ship_Address_text").val(localStorage.getItem("Shipping_Address"));
        $("#Ship_GSTIN_text").val(localStorage.getItem("GSTIN"));
        $("#Ship_Tel_text").val(localStorage.getItem("Mobile_No"));

    }
    else if (value == "ShipClientAddress") {
        $("#ShipClientAddressDiv").removeClass("hide");
        $("#ShipAddressTextDiv input,#ShipAddressTextDiv textarea").val('');
    }
    else if (value == "ShipAddnewAddress") {
        $("#ShipAddressTextDiv input,#ShipAddressTextDiv textarea").val('');
        $("#ClientList").val('');
        $("#ClientList").change();
    }

}

$(document).on('change', '#BillingAddressType', function () {
    BillAddressManagement($(this));
});

function BillAddressManagement(htmlcontrol) {
    var value = $(htmlcontrol).attr('value');

    $("#BillClientAddressDiv").addClass("hide");

    if (value == "BillCompanyAddress") {
        $("#BillClientList").val('');
        $("#BillClientList").change();
        $("#Bill_Address_text").val(localStorage.getItem("Address") + ", " + localStorage.getItem("City") + ", " + localStorage.getItem("State") + "-" + localStorage.getItem("Pin_Code"));
        $("#Bill_GSTIN_text").val(localStorage.getItem("GSTIN"));
        $("#Bill_Tel_text").val(localStorage.getItem("Mobile_No"));

    }
    else if (value == "BillClientAddress") {
        $("#BillClientAddressDiv").removeClass("hide");
        $("#BillAddressTextDiv input,#BillAddressTextDiv textarea").val('');
    }
    else if (value == "BillAddnewAddress") {
        $("#BillAddressTextDiv input,#BillAddressTextDiv textarea").val('');
        $("#BillClientList").val('');
        $("#BillClientList").change();
    }

}

$("#SupplierList").select2({
    /* the following code is used to disable x-scrollbar when click in select input and
    take 100% width in responsive also */
    dropdownAutoWidth: true,
    width: '100%',
}).select2()
    .on('select2:open', () => {
        $(".select2-results:not(:has(a))").append('<div style="width:100%; border-top:solid 1px #cccccc; padding:15px;"><a id="AddSupplierClick" href="javascript:void(0)"><i class="material-icons left">add</i> Add New</a></div>');
    });

$(document).on('click', '#AddSupplierClick', function () {

    let newAddSupplierWindow = window.open('../Master/QuickAddEntry?Addedfrom=QuickAddSupplier', 'Supplier');

    newAddSupplierWindow.onunload = function () {
        //newWindow.close();
        if (newAddSupplierWindow.closed == true) {

            $.ajax({
                url: "/Master/GetNewAddeddSupplierRecord",
                type: "Get",
                dataType: 'json',
                beforeSend: function () {
                    $("#loading").removeClass("hide");
                },
                complete: function () {
                    $("#loading").addClass("hide");
                },
                success: function (response) {

                    $("#SupplierList").append("<option value=" + response[0] + ">" + response[1] + " </option>");
                    $("#SupplierList").val(response[0]);
                    $("#SupplierList").change();
                    $('#SupplierList option[value=' + response[0] + ']').prop('selected', true);

                    swal({ title: 'Success', icon: 'success' });
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
        }
    };
});


$(document).on('change', '#SupplierList', function () {

    var Supplier_ID = $("#SupplierList").val();

    if (Supplier_ID != "") {
        $.ajax({
            url: "/Master/GetsupplierBillAddress",
            type: "Get",
            data: { Supplier_Id: Supplier_ID },
            dataType: 'json',
            beforeSend: function () {
                $("#loading").removeClass("hide");
            },
            complete: function () {
                $("#loading").addClass("hide");
            },
            success: function (response) {

                $("#PoToAddressText").val(response[0]);
                $("#GSTIN").val(response[1]);
                $("#Tel").val(response[2]);
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
    else {
        $("#PoToAddressText").val('');
        $("#GSTIN").val('');
        $("#Tel").val('');
    }
});


$("#ClientList").select2({
    /* the following code is used to disable x-scrollbar when click in select input and
    take 100% width in responsive also */
    dropdownAutoWidth: true,
    width: '100%',
}).select2()
    .on('select2:open', () => {
        $(".select2-results:not(:has(a))").append('<div style="width:100%; border-top:solid 1px #cccccc; padding:15px;"><a id="AddClientClick" href="javascript:void(0)" ><i class="material-icons left">add</i> Add New</a></div>');
    });

$(document).on('click', '#AddClientClick', function () {
    let newAddClientWindow = window.open('../Master/QuickAddEntry?Addedfrom=QuickAddClient', 'Client');

    newAddClientWindow.onunload = function () {
        //newWindow.close();
        if (newAddClientWindow.closed == true) {

            $.ajax({
                url: "/Master/GetNewAddeddClientRecord",
                type: "Get",
                dataType: 'json',
                beforeSend: function () {
                    $("#loading").removeClass("hide");
                },
                complete: function () {
                    $("#loading").addClass("hide");
                },
                success: function (response) {

                    $("#ClientList").append("<option value=" + response[0] + ">" + response[1] + " </option>");
                    $("#ClientList").val(response[0]);
                    $("#ClientList").change();
                    $('#ClientList option[value=' + response[0] + ']').prop('selected', true);

                    swal({ title: 'Success', icon: 'success' });
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
        }
    };
});


$(document).on('change', '#ClientList', function () {

    var Client_ID = $("#ClientList").val();

    if (Client_ID != "") {

        $('#modal1').modal('open');

        $.ajax({
            url: "/Master/GetClientShipAddress",
            type: "Get",
            data: { Client_ID: Client_ID },
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            beforeSend: function () {
                $("#loading").removeClass("hide");
            },
            complete: function () {
                $("#loading").addClass("hide");
            },
            success: function (response) {

                $('#dialog').empty();
                $('#dialog').html(response);
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    } else {
        $("#ShipAddressTextDiv input,#ShipAddressTextDiv textarea").val('');
    }
});

$(document).on('change', '#popup_rdoClientShippingAddress', function () {
    var address = $(this).attr('data-Address');
    var Gstin = $(this).attr('data-GSTIN');
    var Tel = $(this).attr('data-Tel');
    var data_value = $(this).attr('data-value');

    $("#Client_Ship_Address_Id").val(data_value);
    $("#Ship_Address_text").val(address);
    $("#Ship_GSTIN_text").val(Gstin);
    $("#Ship_Tel_text").val(Tel);
});


$("#BillClientList").select2({
    /* the following code is used to disable x-scrollbar when click in select input and
    take 100% width in responsive also */
    dropdownAutoWidth: true,
    width: '100%',
}).select2();


$(document).on('change', '#BillClientList', function () {

    var Client_ID = $("#BillClientList").val();

    if (Client_ID != "") {

        $.ajax({
            url: "/Master/GetClientBillingAddress",
            type: "Get",
            data: { Client_ID: Client_ID },
            dataType: 'json',
            beforeSend: function () {
                $("#loading").removeClass("hide");
            },
            complete: function () {
                $("#loading").addClass("hide");
            },
            success: function (response) {
                $("#Bill_Address_text").val(response[0]);
                $("#Bill_GSTIN_text").val(response[1]);
                $("#Bill_Tel_text").val(response[2]);
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    } else {
        $("#BillAddressTextDiv input,#BillAddressTextDiv textarea").val('');
    }
});


$("#ItemList").select2({

    dropdownAutoWidth: true,
    width: '100%',
}).select2()
    .on('select2:open', () => {
        $(".select2-results:not(:has(a))").append('<div style="width:100%; border-top:solid 1px #cccccc; padding:15px;"><a id="AddItemClick" href="javascript:void(0)" ><i class="material-icons left">add</i> Add New</a></div>');
    });

$(document).on('click', '#AddItemClick', function () {
    let newAddItemWindow = window.open('../Master/QuickAddEntry?Addedfrom=QuickAddItem', 'Item');

    newAddItemWindow.onunload = function () {
        //newWindow.close();
        if (newAddItemWindow.closed == true) {

            $("#AddNewItem").empty();
            GetAddnewPannel();

            $.ajax({
                url: "/Master/GetNewAddeddItemRecord",
                type: "Get",
                dataType: 'json',
                beforeSend: function () {
                    $("#loading").removeClass("hide");
                },
                complete: function () {
                    $("#loading").addClass("hide");
                },
                success: function (response) {

                    $("#ItemList").append("<option value=" + response[0] + ">" + response[1] + " </option>");
                    $("#ItemList").val(response[0]);
                    $("#ItemList").change();
                    $('#ItemList option[value=' + response[0] + ']').prop('selected', true);

                    swal({ title: 'Success', icon: 'success' });
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
        }
    };
});


$(document).on('change', '#ItemList', function () {

    var Item_Id = $("#ItemList").val();

    if (Item_Id != "") {

        $(".invoice-item").attr("style", "");
        $("#poitemvalidate").addClass("hide");

        $.ajax({
            url: "/Purchase/GetItemDetails",
            type: "Get",
            data: { Item_Id: Item_Id },
            dataType: 'json',
            beforeSend: function () {
                $("#loading").removeClass("hide");
            },
            complete: function () {
                $("#loading").addClass("hide");
            },
            success: function (response) {
                $(".invoice-item #Item_NameHid").val(response[0].Item_Name);
                $(".invoice-item #ItemUnitList").val(response[0].UOM_Unit).change();
                $(".invoice-item #Item_Unit_NameHid").val(response[0].Unit_Name);
                $(".invoice-item #Qty").val("1");
                $(".invoice-item #Rate").val(response[0].Purchase_Price);
                $(".invoice-item #Note").val(response[0].Description);
                $(".invoice-item #HSN_SAC").val(response[0].HSN_SAC);
                $(".invoice-item #TaxList option:selected").val(response[0].Tax);
                $(".invoice-item #Discount").val(response[0].Discount);
                Getcalculateitem();
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
    else {

        Emptypoitem();
    }
});


$(document).on("click", ".invoice-apply-btn", function () {
    Getcalculateitem();
    $('.dropdown-button').dropdown("close");
});

$(document).on('change', '.invoice-item-filed input', function () {
    Getcalculateitem();
});

function Getcalculateitem() {

    var qty = parseFloat($(".invoice-item #Qty").val());
    var rate = parseFloat($(".invoice-item #Rate").val());
    var discount = parseFloat($(".invoice-item #Discount").val());
    var Tax = parseFloat($(".invoice-item #TaxList").val());

    if (Tax == "" || isNaN(Tax) == true) {
        Tax = 0;
    }

    if (discount == "" || isNaN(discount) == true) {
        discount = 0;
    }

    if (rate == "" || isNaN(rate) == true) {
        rate = 0;
    }

    if (qty == "" || isNaN(qty) == true) {
        qty = 0;
    }
    $(".invoice-item #DiscountSpan").html(discount + "%");
    $(".invoice-item #DiscountHid").val(discount);
    $(".invoice-item #CGSTSpan").html((Tax / 2) + "%");
    $(".invoice-item #SGSTSpan").html((Tax / 2) + "%");
    $(".invoice-item #CGSTHid").val((Tax / 2));
    $(".invoice-item #SGSTHid").val((Tax / 2));

    var Total = qty * rate;

    var Discount_Amount = ((Total * discount) / 100);
    var TaxAmount = ((Total * Tax) / 100);

    Total = (Total + TaxAmount) - Discount_Amount

    $(".invoice-item #TotalSpan").html("Total: <br> " + Total.toFixed(2) + " Rs");
    $(".invoice-item #TotalHid").val(Total.toFixed(2));
}


$(document).on("click", "#AddNewItem-row", function () {

    if ($(".invoice-item #ItemList").val() != "" && $(".invoice-item #Qty").val() != "" && $(".invoice-item #Rate").val() != "") {

        $(".invoice-item").attr("style", "");
        $("#poitemvalidate").addClass("hide");

        var valdata = $(".invoice-item-repeater").find('select, textarea, input').serialize();

        $.ajax({
            url: "../Purchase/PurchaseItemList",
            type: "Get",
            data: valdata,
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            beforeSend: function () {
                $("#loading").removeClass("hide");
            },
            complete: function () {
                $("#loading").addClass("hide");
            },
            success: function (response) {
                //$('#AddedItemList').empty();

                $("#AddedItemList table").removeClass("hide");
                $('#ItemBodylist').append(response);
                CalculatePurchaseorder();
                $("#ItemList").val('').change();
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
    else {
        $(".invoice-item").attr("style", "border:solid 1px #ff4081 !important");
        $("#poitemvalidate").removeClass("hide");
    }
});

$(document).on("click", "#edit-row", function () {
    var $this = $(this);
    var row = $this.closest("tr");
    var valdata = row.find('select, textarea, input').serialize();

    $.ajax({
        url: "../Purchase/PurchaseItemList",
        type: "Get",
        data: valdata,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        beforeSend: function () {
            $("#loading").removeClass("hide");
        },
        complete: function () {
            $("#loading").addClass("hide");
        },
        success: function (response) {
            row.empty();
            row.append(response);
            $("#AddNewItem").empty();

            $("#ItemList").select2({
                dropdownAutoWidth: true,
                width: '100%',
            }).select2()
                .on('select2:open', () => {
                    $(".select2-results:not(:has(a))").append('<div style="width:100%; border-top:solid 1px #cccccc; padding:15px;"><a id="AddItemClick" href="javascript:void(0)" ><i class="material-icons left">add</i> Add New</a></div>');
                });

            $('.dropdown-button').dropdown({
                constrainWidth: false, // Does not change width of dropdown to that of the activator
                closeOnClick: false
            });

            $('span#edit-row').each(function () {
                $(this).addClass("hide");
            });
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

});

$(document).on("click", "#update-row", function () {

    if ($(".invoice-item #ItemList").val() != "" && $(".invoice-item #Qty").val() != "" && $(".invoice-item #Rate").val() != "") {

        $(".invoice-item").attr("style", "");
        $("#poitemvalidate").addClass("hide");

        var $this = $(this);
        var row = $this.closest("tr");

        var valdata = $(".invoice-item-repeater").find('select, textarea, input').serialize();

        $.ajax({
            url: "../Purchase/PurchaseItemList",
            type: "Get",
            data: valdata,
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            beforeSend: function () {
                $("#loading").removeClass("hide");
            },
            complete: function () {
                $("#loading").addClass("hide");
            },
            success: function (response) {
                row.empty();
                row.append(response);
                $('span#edit-row').each(function () {
                    $(this).removeClass("hide");
                });
                CalculatePurchaseorder();
                GetAddnewPannel();
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
    else {
        $(".invoice-item").attr("style", "border:solid 1px #ff4081 !important");
        $("#poitemvalidate").removeClass("hide");
    }
});

function GetAddnewPannel() {

    $.ajax({
        url: "../Purchase/PurchaseItemList",
        type: "Get",
        data: "",
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        beforeSend: function () {
            $("#loading").removeClass("hide");
        },
        complete: function () {
            $("#loading").addClass("hide");
        },
        success: function (response) {

            $("#AddNewItem").append(response);

            $("#ItemList").select2({
                dropdownAutoWidth: true,
                width: '100%',
            }).select2()
                .on('select2:open', () => {
                    $(".select2-results:not(:has(a))").append('<div style="width:100%; border-top:solid 1px #cccccc; padding:15px;"><a id="AddItemClick" href="javascript:void(0)" ><i class="material-icons left">add</i> Add New</a></div>');
                });

            $('.dropdown-button').dropdown({
                constrainWidth: false, // Does not change width of dropdown to that of the activator
                closeOnClick: false
            });

        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

$(document).on("change", "#RoundOff", function () {
    CalculatePurchaseorder();
});

function CalculatePurchaseorder() {
    var ItemTotal = 0;

    $('#AddedItemList input[name=Total]').each(function () {
        ItemTotal = ItemTotal + parseFloat($(this).val());
    });

    if (ItemTotal == "" || isNaN(ItemTotal) == true) {
        ItemTotal = 0;
    }

    $("#Net_TotalHid").val(ItemTotal);
    $("#NetTotalSpan").html((ItemTotal).toFixed(2) + " Rs");

    var RoundOff = parseFloat($("#RoundOff").val());
    if (RoundOff == "" || isNaN(RoundOff) == true) {
        RoundOff = 0;
    }

    var GrossTotal = (ItemTotal) - RoundOff;

    $("#Gross_TotalHid").val(GrossTotal);
    $("#GrossTotalSpan").html(GrossTotal.toFixed(2) + " Rs");
}

function Emptypoitem() {
    $(".invoice-item input").val('');
    $(".invoice-item textarea").val('');
    $(".invoice-item #CGSTSpan").html("0%");
    $(".invoice-item #SGSTSpan").html("0%");
    $(".invoice-item #TotalSpan").html("0 Rs");
    $(".invoice-item #DiscountSpan").html("0%");
    $(".invoice-item #DiscountSpan").html("0%");
}

$(document).on("click", "#delete-row", function () {
    var $this = $(this);
    $this.closest("tr").remove();
    CalculatePurchaseorder();

});

$(document).on("click", ".po", function () {
    var itemCount = 0;

    $('input#Item_Id').each(function () {
        itemCount += 1;
    });

    if (itemCount <= 0) {
        swal('At least one item must be required..', {
            title: 'Error',
            icon: 'error'
        })
        return false;
    }
    else {
        return true;
    }
});

//------------report


$("#SupplierSearch").on("input", function () {
    $("#SupplierFilter").val($(this).val());
    Getpurchaedatatable();
});

$(document).on('click', '#filter-purchase', function () {

    $("#hid-page-no").val($(this).attr("data-value"));
    Getpurchaedatatable();
});

function Getpurchaedatatable() {
    var valdata = $("#FormFilter").serialize();

    $.ajax({
        url: "/Purchase/PurchaseDataTable",
        type: "Post",
        data: valdata,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        beforeSend: function () {
            $('#dialog-data-table').empty();
            $("#loading").removeClass("hide");
        },
        complete: function () {
            $("#loading").addClass("hide");
        },
        success: function (response) {
            $('#dialog-data-table').empty();
            $('#dialog-data-table').html(response);
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

//For filter sidepanel
$(document).ready(function () {
    "use strict";

    // email-overlay and sidebar hide
    // --------------------------------------------
    $(".compose-email-trigger").on("click", function () {
        $(".email-overlay").addClass("show");
        $(".email-compose-sidebar").addClass("show");

    })
    $(
        ".email-compose-sidebar .cancel-email-item, .email-compose-sidebar .close-icon, .email-compose-sidebar .send-item, .email-overlay"
    ).on("click", function () {
        $(".email-overlay").removeClass("show");
        $(".email-compose-sidebar").removeClass("show");
    });

    if ($(".email-compose-sidebar").length > 0) {
        var ps_sidebar_compose = new PerfectScrollbar(".email-compose-sidebar", {
            theme: "dark",
            wheelPropagation: false
        });
    }
});
//------------short report

if ($(".invoice-print").length > 0) {
    $(".invoice-print").on("click", function () {
        alert();
        document.getElementById("preview-iframe").contentWindow.print();
    })
}
if ($(".invoice-action-wrapper").length > 0) {
    var ps_email_collection = new PerfectScrollbar(".invoice-action-wrapper", {
        theme: "dark",
        wheelPropagation: false
    });
}

$("#ShortSupplierSearch").on("input", function () {
    $("#SupplierFilter").val($(this).val());
    GetShortpurchaedatatable();
});

$(document).on('click', '#Shortfilter-purchase', function () {

    $("#hid-page-no").val($(this).attr("data-value"));
    GetShortpurchaedatatable();
});

function GetShortpurchaedatatable() {
    var valdata = $("#FormFilter").serialize();

    $.ajax({
        url: "/Purchase/ShortDataTable",
        type: "Post",
        data: valdata,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        beforeSend: function () {
            $('#dialog-data-table').empty();
            $("#loading").removeClass("hide");
        },
        complete: function () {
            $("#loading").addClass("hide");
        },
        success: function (response) {
            $('#dialog-data-table').empty();
            $('#dialog-data-table').html(response);
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

$(document).on('click', '.invoice-print', function () {
    document.getElementById("preview-iframe").contentWindow.print();
})

//--------View Purchase



$(document).on('click', '#view-po', function () {
    var id = $(this).attr('data-value');

    if ($("#po-id").val() != id) {
        $("#po-id").val(id);
        $("#loading").removeClass("hide");
        $("#preview-iframe").attr("src", "../Document/Print.aspx?purchaseid=" + id + "");

        GetActionButtons();
    }

    $('#preview-iframe').on('load', () => {
        $("#loading").addClass("hide");
    })

});

$(document).on('click', '#po-action-status', function () {

    var id = $("#po-id").val();

    $.ajax({
        url: "/Purchase/PurchaseStatusUpdate",
        type: "Get",
        data: { id: id, status: $(this).val() },
        dataType: 'json',
        beforeSend: function () {
            $("#loading").removeClass("hide");
        },
        complete: function () {
            $("#loading").addClass("hide");
        },
        success: function (response) {
            GetActionButtons();
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
});

function GetActionButtons() {

    var id = $("#po-id").val();

    $.ajax({
        url: "/Purchase/Getpurchasestatus",
        type: "Get",
        data: { id: id },
        dataType: 'json',
        beforeSend: function () {
            $("#loading").removeClass("hide");
        },
        complete: function () {
            $("#loading").addClass("hide");
        },
        success: function (response) {

            $(".action-buttons").empty();
            var action_buttons = "";

            if (response.Status == "Converted" || response.Status == "Cancelled") {
                if (response.Status != "Cancelled") {
                    action_buttons = '<button class="btn green waves-effect waves-light mb-1 mr-1 pl-1 disabled" id="po-action-status" value="Converted"><i class="material-icons left">receipt</i>Converted</button>';
                }
            }
            else {
                action_buttons = '<button class="btn green waves-effect waves-light mb-1 mr-1 pl-1" id="po-action-status" value="Converted"><i class="material-icons left">receipt</i>Convert To Bill</button>';
            }

            action_buttons += '<a href="javascript:void(0)" class="waves-effect waves-light btn-light-cyan btn mb-1 mr-1 pl-1 invoice-print"><i class="material-icons left">print</i> Print</a>';
            action_buttons += '<a class="waves-effect waves-light btn mb-1 mr-1 pl-1" href="../Document/Print.aspx?DocumentType=pdf&purchaseid=' + id + '" target="_blank"><i class="material-icons left">picture_as_pdf</i> PDF</a>';

            if (response.Status == "Open") {
                action_buttons += '<a class="waves-effect waves-light btn-light-indigo btn mb-1 mr-1 pl-1" href="../Purchase/AddPurchaseorder?purchaseid=' + id + '"><i class="material-icons left">create</i> Edit</a>';
            }

            if (response.Status == "Cancelled") {
                action_buttons += '<button class="waves-effect waves-light red btn mb-1 mr-1 pl-1 disabled" id="po-action-status" value="Cancelled"><i class="material-icons left">cancel</i> Cancelled</button>';
            }
            else {
                action_buttons += '<button class="waves-effect waves-light red btn mb-1 mr-1 pl-1" id="po-action-status" value="Cancelled"><i class="material-icons left">cancel</i> Cancel</button>';
            }
            if (response.Status == "Cancelled") {
                action_buttons += '<button class="btn indigo waves-effect waves-light mb-1 mr-1 pl-1" id="po-action-status" value="Open"><i class="material-icons left">lock_open</i>Reopen</button>';
            }

            $(".action-buttons").append(action_buttons);


            $("#Created_on").html("Created On-" + response.Created_On);

            if (response.Modified_On != null) {
                $("#Modified_on").html("Modified On-" + response.Modified_On);
            }
            else {
                $("#Modified_on").html("Modified On-");
            }

            if (response.Modified_By != null) {
                $("#Modified_by").html("Modified By-" + response.Modified_By);
            }
            else {
                $("#Modified_by").html("Modified By-");
            }           

        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}