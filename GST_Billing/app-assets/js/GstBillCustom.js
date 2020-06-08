
//Item Managment--------------------------------------------------------------------

$(document).ready(function () {

    /* Basic Select2 select */
    $(".item-unit").select2({
        /* the following code is used to disable x-scrollbar when click in select input and
        take 100% width in responsive also */
        dropdownAutoWidth: true,
        width: '100%',
    }).select2()
        .on('select2:open', () => {
            $(".select2-results:not(:has(a))").append('<div style="width:100%; border-top:solid 1px #cccccc; padding:15px;"><a id="Addunitclick" class="modal-trigger" href="#modal1"><i class="material-icons left">add</i> Add New</a></div>');
        });

    $(".tax").select2({
        /* the following code is used to disable x-scrollbar when click in select input and
        take 100% width in responsive also */
        dropdownAutoWidth: true,
        width: '100%',
    }).select2()
        .on('select2:open', () => {
            $(".select2-results:not(:has(a))").append('<div style="width:100%; border-top:solid 1px #cccccc; padding:15px;"><a id="AddTaxClick" class="modal-trigger" href="#modal1"><i class="material-icons left">add</i> Add New</a></div>');
        });


    $(document).on('click', '#Addunitclick', function () {
        $.ajax({
            url: "/Master/AddItemUnit",
            type: "Get",
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            beforeSend: function () {
                document.getElementById('loading').style.display = "block";
            },
            complete: function () {
                document.getElementById('loading').style.display = "none";
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

    });

    $(document).on('click', '#AddTaxClick', function () {
        $.ajax({
            url: "/Master/AddNewTax",
            type: "Get",
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            beforeSend: function () {
                document.getElementById('loading').style.display = "block";
            },
            complete: function () {
                document.getElementById('loading').style.display = "none";
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

    });
});

$("#ItemNameSearch").on("input", function () {
    $("#ItemNameFilter").val($(this).val());
    Getdatatable();
});

$(document).on('click', '#filter-item', function () {

    $("#hid-page-no").val($(this).attr("data-value"));
    Getdatatable();
});

function Getdatatable() {
    var valdata = $("#FormFilter").serialize();

    $.ajax({
        url: "/Master/ItemDataTable",
        type: "Post",
        data: valdata,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        beforeSend: function () {
            $('#dialog-data-table').empty();
            document.getElementById('loading').style.display = "block";
        },
        complete: function () {
            document.getElementById('loading').style.display = "none";
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

//End item management-------------------------------------------------------------------------------

//For data table
//        var table=
//$(".invoice-data-table").DataTable({
//    columnDefs: [
//        { targets: [0, 7], orderable: !1 },
//        { orderable: !1, targets: 8 }
//    ],
//    paging:false,
//    order: [2, "asc"],
//    dom: '<"top display-flex  mb-2"<"action-filters"f><"actions action-btns display-flex align-items-center">><"result"><"clear">rt<"bottom"p>',
//    language: { search: "", searchPlaceholder: "Search item" },
//    select: { style: "multi", selector: "td:first-child>", items: "row" },
//    responsive: { details: { type: "column", target: 0 } }
//});
//var e = $(".filter-action"), t = $(".create-btn"), i = $(".filter-btn"), r = $(".result-count"); $(".action-btns").append(e, t), $(".result").append(r), $(".dataTables_filter label").append(i)



function CustomToastCall(msg) {
    var toastHTML = '<div class="card-alert card gradient-45deg-green-teal"><div class="card-content white-text"><p><i class="material-icons">check</i> SUCCESS : <span id="TopSuccessAlert">' + msg + '</span> </p></div><button type="button" class="close white-text" data-dismiss="alert" aria-label="Close"><span aria-hidden="true"><i class="material-icons">clear</i></span></button></div>';
    M.toast({
        html: toastHTML
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

//Supplier Management-----------------------------------------------------

$("#AddNewBankbtn").click(function (e) {

    var appendContent = '<div class="row">';
    appendContent += '<div class="input-field col m3 s12">';
    appendContent += '<input id="Bank_Name" name="Bank_Name" type="text" value="">';
    appendContent += '<input id="SupplierBankDetailsId" name="SupplierBankDetailsId" type="hidden" value="0">';
    appendContent += '<label>Bank Name</label></div>';
    appendContent += '<div class="input-field col m3 s12">';
    appendContent += '<input id="Bank_Branch" name="Bank_Branch" type="text" value="">';
    appendContent += '<label for="first_name01">Bank Branch</label></div>';
    appendContent += '<div class="input-field col m3 s12">';
    appendContent += '<input id="Bank_Acc_No" name="Bank_Acc_No" type="text" value="">';
    appendContent += '<label>Bank Account No</label></div>';
    appendContent += '<div class="input-field col m2 s12">';
    appendContent += '<input id="Bank_IFSC" name="Bank_IFSC" type="text" value="">';
    appendContent += '<label for="first_name01">IFSC Code</label></div>';
    appendContent += '<div class="col m1 s12">';
    appendContent += '<button type="button" id="RemoveBankbtn" style="width:30px; height:30px; line-height:0; margin-top:20px;" class="btn-floating waves-effect waves-light amber darken-4">';
    appendContent += '<i style="line-height:10px; font-size:14px;" class="material-icons">clear</i></button></div></div>';

    $("#AddNewBank").append(appendContent);
});

$(document).on('click', '#RemoveBankbtn', function () {
    //var value = $(this).val();
    //$('#AddNewBank #row-' + value).remove();
    $(this).parent().parent(".row").remove();
});


$("#CompNameSearch").on("input", function () {
    $("#CompanyNameFilter").val($(this).val());
    GetSupplierdatatable();
});

$(document).on('click', '#filter-Supplier', function () {

    $("#hid-page-no").val($(this).attr("data-value"));
    GetSupplierdatatable();
});

function GetSupplierdatatable() {
    var valdata = $("#FormFilter").serialize();

    $.ajax({
        url: "/Master/SupplierDataTable",
        type: "Post",
        data: valdata,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        beforeSend: function () {
            $('#dialog-data-table').empty();
            document.getElementById('loading').style.display = "block";
        },
        complete: function () {
            document.getElementById('loading').style.display = "none";
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

//End Supplier Management-----------------------------------------------------
//Client Management-----------------------------------------------------

$("#AddNewShipAddressbtn").click(function (e) {
    var appendContent = '<div id="row-remove" class="row"><div class="col s12 m12 l12"><div id="Form-advance" class="card card card-default scrollspy"><div class="card-content"><div class="row">';
    appendContent += '<div class="input-field col m6 s12"> <input id="Ship_Company_Name" name="Ship_Company_Name" type="text" value=""><label>Company Name</label>';
    appendContent += '<input id="ShippingAddressId" name="ShippingAddressId" type="hidden" value="0"></div >';
    appendContent += '<div class="input-field col m5 s12"><input id="Ship_GSTIN" name="Ship_GSTIN" type="text" value=""><label for="first_name01">GSTIN</label></div>';
    appendContent += '<div class="col m1">';
    appendContent += '<button type="button" id="RemoveShipAddressbtn" style="width:30px; height:30px; line-height:0; margin-top:20px;" class="btn-floating waves-effect waves-light amber darken-4">';
    appendContent += '<i style="line-height:10px; font-size:14px;" class="material-icons">clear</i></button></div>';
    appendContent += '<div class="input-field col m6 s12"><input id="Ship_Contact_Person" name="Ship_Contact_Person" type="text" value=""><label>Contact Person</label></div>';
    appendContent += '<div class="input-field col m6 s12"><input id="Ship_Mobile_No" name="Ship_Mobile_No" type="text" value=""><label for="first_name01">Phone No</label></div>';
    appendContent += '<div class="input-field col m12 s12"><input id="Ship_Address" name="Ship_Address" type="text" value="" placeholder = "Building no/ street/ near by" maxlength = "100"><label>Detail Address</label></div>';
    appendContent += '<div class="input-field col m3 s12"><input id="Ship_Country" name="Ship_Country" type="text" value="India"><label for="first_name01">Country</label></div>';
    appendContent += '<div class="input-field col m3 s12"><input id="Ship_State" name="Ship_State" type="text" value=""><label for="Description">State</label></div>';
    appendContent += '<div class="input-field col m3 s12"><input id="Ship_City" name="Ship_City" type="text" value=""><label for="Description">City</label></div>';
    appendContent += '<div class="input-field col m3 s12"><input id="Ship_Pin_Code" name="Ship_Pin_Code" type="text" value=""><label for="Description">Pin Code</label></div>';
    appendContent += '</div></div></div></div></div>';
    $("#AddNewShipAddress").append(appendContent);
});

$(document).on('click', '#RemoveShipAddressbtn', function () {
    $(this).closest('#row-remove').remove();
});


$("#ClientCompNameSearch").on("input", function () {
    $("#ClientCompanyNameFilter").val($(this).val());
    GetClientdatatable();
});

$(document).on('click', '#filter-Client', function () {

    $("#hid-page-no").val($(this).attr("data-value"));
    GetClientdatatable();
});

function GetClientdatatable() {
    var valdata = $("#FormFilter").serialize();

    $.ajax({
        url: "/Master/ClientDataTable",
        type: "Post",
        data: valdata,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        beforeSend: function () {
            $('#dialog-data-table').empty();
            document.getElementById('loading').style.display = "block";
        },
        complete: function () {
            document.getElementById('loading').style.display = "none";
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

//Expenses type


$("#ExpensesTypeSearch").on("input", function () {
    $("#hid-type").val($(this).val());
    GetExtypedatatable();
});

$(document).on('click', '#filter-ExType', function () {

    $("#hid-page-no").val($(this).attr("data-value"));
    GetExtypedatatable();
});

function GetExtypedatatable() {
    var valdata = $("#FormFilter").serialize();

    $.ajax({
        url: "/Master/ExpensesTypeDataTable",
        type: "Post",
        data: valdata,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        beforeSend: function () {
            $('#dialog-data-table').empty();
            document.getElementById('loading').style.display = "block";
        },
        complete: function () {
            document.getElementById('loading').style.display = "none";
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



//Employee Management


$(document).ready(function () {

    /* Basic Select2 select */
    $(".Department").select2({
        /* the following code is used to disable x-scrollbar when click in select input and
        take 100% width in responsive also */
        dropdownAutoWidth: true,
        width: '100%',
    }).select2()
        .on('select2:open', () => {
            $(".select2-results:not(:has(a))").append('<div style="width:100%; border-top:solid 1px #cccccc; padding:15px;"><a id="AddDepartmentclick" class="modal-trigger" href="#modal1"><i class="material-icons left">add</i> Add New</a></div>');
        });

    $(".Designation").select2({
        /* the following code is used to disable x-scrollbar when click in select input and
        take 100% width in responsive also */
        dropdownAutoWidth: true,
        width: '100%',
    }).select2()
        .on('select2:open', () => {
            $(".select2-results:not(:has(a))").append('<div style="width:100%; border-top:solid 1px #cccccc; padding:15px;"><a id="AddDesignationClick" class="modal-trigger" href="#modal1"><i class="material-icons left">add</i> Add New</a></div>');
        });


    $(document).on('click', '#AddDepartmentclick', function () {
        $.ajax({
            url: "/Master/AddDepartment",
            type: "Get",
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            beforeSend: function () {
                document.getElementById('loading').style.display = "block";
            },
            complete: function () {
                document.getElementById('loading').style.display = "none";
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

    });

    $(document).on('click', '#AddDesignationClick', function () {
        $.ajax({
            url: "/Master/AddDesignation",
            type: "Get",
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            beforeSend: function () {
                document.getElementById('loading').style.display = "block";
            },
            complete: function () {
                document.getElementById('loading').style.display = "none";
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

    });
});


$("#EmpNameSearch").on("input", function () {
    $("#EmpNameFilter").val($(this).val());
    GetEmployeedatatable();
});

$(document).on('click', '#filter-employee', function () {

    $("#hid-page-no").val($(this).attr("data-value"));
    GetEmployeedatatable();
});

function GetEmployeedatatable() {
    var valdata = $("#FormFilter").serialize();

    $.ajax({
        url: "/Master/EmployeeDataTable",
        type: "Post",
        data: valdata,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        beforeSend: function () {
            $('#dialog-data-table').empty();
            document.getElementById('loading').style.display = "block";
        },
        complete: function () {
            document.getElementById('loading').style.display = "none";
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