var dir = window.location.origin;
//dir += '/Kabugwason';

var monthLong = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
var monthShort = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'June', 'July', 'Aug', 'Sept', 'Oct', 'Nov', 'Dec'];

const currencyOptions = {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
};


$(document).ready(function () {
    try {
        $('[data-mask]').inputmask();
        $('.decimal').inputmask('currency', { rightAlign: false });
        $('.currency').inputmask({
            'alias': 'currency',
            'groupSeparator': ',',
            'autoGroup': true,
            'digits': 2,
            'digitsOptional': false,
            'placeholder': '0.00'
        });
        $('.whole-number').inputmask({
            alias: 'numeric',
            allowMinus: false,
            digits: 0
        });
    } catch (e) {

    }

    try {
        $(".select2").select2({
            theme: 'bootstrap4'
        });
    } catch (e) {

    }
});

$(document).ajaxStart(function () {
    $("#loader").css("display", "flex");
});

$(document).ajaxComplete(function () {
    $("#loader").css("display", "none");
});


function initSelect2() {
    // REQUIRED IMPORTS
    // ---------- CSS ----------
    //<!-- Select2 -->
    //<link href="~/Content/plugins/select2/css/select2.min.css" rel="stylesheet" />
    //<link href="~/Content/plugins/select2-bootstrap4-theme/select2-bootstrap4.min.css" rel="stylesheet" />
    // ---------- JS ----------
    //<!-- Select2 -->
    //<script src="~/Content/plugins/select2/js/select2.full.min.js"></script>

    $('.select2').each(function () {
        let firstOption = $(this).find('option:first-child')[0];

        if (firstOption === undefined) {
            $(this).select2({
                theme: 'bootstrap4',
                allowClear: true
            });
        } else {
            let placeholder = firstOption.text;

            $(this).find('option:first-child').text('');

            $(this).select2({
                theme: 'bootstrap4',
                allowClear: true,
                placeholder: placeholder
            });
        }
    });

    $(document).on('select2:open', () => {
        document.querySelector('.select2-search__field').focus();
    });
}

function initValidationMobileNumber() {
    // REQUIRED IMPORTS
    // ---------- JS ----------

    //<!-- InputMask -->
    //<script src="~/Content/plugins/inputmask/jquery.inputmask.min.js"></script>
    //<!-- jquery-validation -->
    //<script src="~/Content/plugins/jquery-validation/jquery.validate.min.js"></script>
    //<script src="~/Content/plugins/jquery-validation/additional-methods.min.js"></script>

    $('.custom-mobile').inputmask({
        mask: '9999-999-9999',
        //clearIncomplete: true,
        autoUnmask: true,
        placeholder: '09xx-xxx-xxxx',
    });

    $.validator.addMethod('mobile-number', function (value, element) {
        return this.optional(element) || /^0(8|9)\d{9}$/.test(value);
    }, 'Please enter a valid mobile number');
}

function showToastr(type, message) {
    switch (type) {
        case 'success':
            toastr.success(message);
            break;
        case 'warning':
            toastr.warning(message);
            break;
        case 'error':
            toastr.error(message);
            break;
        case 'info':
        default:
            toastr.info(message);
            break;
    }
}

$('select').on('change', function (event) {
    $(this).trigger('change.select2');
});

$('.select2').on('change', function (e) {
    try {
        var isRequired = $(this).rules().required || false;

        if (isRequired) {
            $(this).valid();
        }
    }
    catch (err) {

    }
});

function GetFormData(form) {
    var formValues = {};
    var elements = $(form)[0].elements;

    for (var i = 0, element; element = elements[i++];) {
        if (element.id.trim() != "" && element.name.trim() != "") {
            if (element.type == "checkbox" || element.type == "radio") {
                formValues[element.id] = $(element).prop("checked");
            }
            else if (element.type == "select-one" || element.type == "select-multiple") {
                formValues[element.id] = $(element).select2("val");
            }
            else {
                formValues[element.id] = element.value.trim();
            }
        }
    }

    return formValues;
}

function ClearFormFields(form) {
    var formName = form.currentForm.id;

    var inputs = document.forms[formName].getElementsByTagName('input');
    for (i = 0; i < inputs.length; i++) {
        var element = inputs[i];

        switch (element.type) {
            case "checkbox":
                $(element).prop("checked", true).trigger('change');
                break;
            case "radio":
                $(element).prop("checked", false).trigger('change');
                break;
            default:
                $(element).val("");
        }
    }

    var selects = document.forms[formName].getElementsByTagName('select');
    for (i = 0; i < selects.length; i++) {
        var element = selects[i];

        $(element).val($(element).find('option:first-child').val()).trigger("change");
    }

    form.resetForm();
}


function showToast(message, icon) {
    if (icon == 'success') {
        toastr.success(message);
    }
    else if (icon == 'warning') {
        toastr.warning(message);
    }
    else if (icon == 'error') {
        toastr.error(message);
    }
    else {
        toastr.info(message);
    }
}

function alertMessage({ tittle, message, icon = 'info' }) {
    return action = Swal.fire({
        icon: icon,
        title: tittle,
        text: message,
    });
}

function alertMessage2({ tittle, message, icon = 'info' }) {
    return action = Swal.fire({
        icon: icon,
        title: tittle,
        customClass: 'swal-wide',
        html: message
    });
}

function confirmAction({ tittle, message, icon = 'question', confirmButton = 'Yes', cancelButton = 'Cancel' }) {
    return action = Swal.fire({
        title: tittle,
        text: message,
        icon: icon,
        reverseButtons: true,
        confirmButtonText: confirmButton,
        cancelButtonText: cancelButton,
        showCancelButton: true,
        confirmButtonText: confirmButton,
    }).then((result) => result.isConfirmed);
}

function confirmAction2({ tittle, message, icon = 'question', confirmButton = 'Yes', cancelButton = 'Cancel', denyButton = 'Close' }) {
    return action = Swal.fire({
        title: tittle,
        text: message,
        icon: icon,
        reverseButtons: true,
        cancelButtonText: cancelButton,
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: confirmButton,
        denyButtonText: denyButton,
        customClass: {
            confirmButton: 'bg-success',
            denyButton: 'bg-primary',
        }
    }).then((result) => result);
}


$('.whole-number').inputmask({
    alias: 'numeric',
    allowMinus: false,
    digits: 0
});