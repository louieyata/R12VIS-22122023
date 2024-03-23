var p_id = 0;
var c_id = 0;
var vac_p_id = 0;
var vac_c_id = 0;

$(document).ready(function () {

    // Validation for required fields start
    $('#form-individual-vpd').validate({
        // Define validation rules and messages here
        rules: {
            individual_first_name: { required: true },
            individual_last_name: { required: true },
            individual_date_of_birth: { required: true },
            sibling_rank_list: { required: true },
            province_id: { required: true },
            cm_id: { required: true },
            barangay_id: { required: true },
            religion_list: { required: true },
            educational_attainment_list: { required: true },
            income_class_list: { required: true },
            mrsia_occupation_list: { required: true },
            vaccination_province_id: { required: true },
            vaccination_cm_id: { required: true },
            vaccination_barangay_id: { required: true },
            vaccination_date_of_vaccination: { required: true },
            vaccination_vaccine_id: { required: true },
            vaccination_vaccinator_name: { required: true }
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            var elem = $(element);
            if (elem.hasClass("select2-hidden-accessible")) {
                $("#select2-" + elem.attr("id") + "-container").parent().addClass('is-invalid');
            } else {
                elem.addClass('is-invalid');
            }
        },
        unhighlight: function (element, errorClass, validClass) {
            var elem = $(element);
            if (elem.hasClass("select2-hidden-accessible")) {
                $("#select2-" + elem.attr("id") + "-container").parent().removeClass('is-invalid');
            } else {
                elem.removeClass('is-invalid');
            }
        }
    });

    // Validation for required fields end

    $('#individual_date_of_birth').datepicker({
        endDate: '0d',              // Set the maximum date to today (0 days in the future)
        autoclose: true,            // Automatically close the date picker when a date is selected
        format: 'mm/dd/yyyy',         // Format date as needed
        keyboardNavigation: false   // Disable keyboard navigation
    });
    $('#vaccination_date_of_vaccination').datepicker({
        endDate: '0d',              // Set the maximum date to today (0 days in the future)
        autoclose: true,            // Automatically close the date picker when a date is selected
        format: 'mm/dd/yyyy',         // Format date as needed
        keyboardNavigation: false   // Disable keyboard navigation
    });
    $('.select2').select2().on('select2:open', () => {
        document.querySelector('.select2-search__field').focus();
    });
    PopulateMRSIAVaccinationRecords();
});

$('#individual_sc_member').on('change', function () {
    if ($(this).is(':checked')) 
        $('#individual_sc_organization_name').prop('disabled', false);
    else
        $('#individual_sc_organization_name').prop('disabled', true);
});

$('#radio-living-arrangement').on('change', function () {
    if ($('#individual_sc_living_arrangement_other').is(':checked'))
        $('#individual_sc_living_arrangement_others_name').prop('disabled', false);
    else
        $('#individual_sc_living_arrangement_others_name').prop('disabled', true);
});

function isDateValid(dateString) {
    // Check if the date string is a valid date
    return moment(dateString, "MM/DD/YYYY", true).isValid();
}

function showDateError() {
    toastr.error("Error: Date of vaccination cannot be before date of birth.");
}

$("#form-individual-vpd").submit(function (event) {
    event.preventDefault();

    if ($('#form-individual-vpd').valid()) {
        //Remove.input-invalid class from all input elements
        $("#form-individual .is-invalid").removeClass("is-invalid");
        var formData = GetFormData(this);

        if ($("#individual_date_of_birth").val() > $("#vaccination_date_of_vaccination").val()) {
            toastr.error("Error: Date of vaccination cannot be before date of birth.");
            return;
        }

        Swal.fire({
            title: "Do you want to save this vaccination?",
            //text: "You won't be able to revert this!",
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Save"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "POST",
                    url: dir + "/VaccinationsCS/SaveVaccination",
                    data: { jsonData: JSON.stringify(formData) },
                    dataType: "json",
                    async: true,
                    success: function (jsonData) {
                        if (jsonData.isSuccess) {
                            Swal.fire({
                                title: "Vaccination was successfully saved.",
                                //text: "You won't be able to revert this!",
                                icon: "success",
                                showCancelButton: false,
                                confirmButtonText: "Okay"
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    location.reload();
                                }
                            });
                        } else {
                            toastr.error("There was an error saving the vaccination. " + jsonData.message);
                        }
                    }
                });
            }
        });
    }
});

// Person address
$('#province_id').on('change', function () {
    p_id = $('#province_id').val();
    if (p_id == 0) {
        $('#barangay_id').empty();
        $('#barangay_id').append(`<option value="0" selected>- Barangay -</option>`)
    }
    PopulateCityMunicipality(p_id);
});
$('#cm_id').on('change', function () {
    c_id = $('#cm_id').val();
    PopulateBarangay(c_id);
});
// Vaccine address
$('#vaccination_province_id').on('change', function () {
    vac_p_id = $('#vaccination_province_id').val();
    if (vac_p_id == 0) {
        $('#vaccination_barangay_id').empty();
        $('#vaccination_barangay_id').append(`<option value="0" selected>- Barangay -</option>`)
    }
    PopulateVaccinationCityMunicipality(vac_p_id);
});
$('#vaccination_cm_id').on('change', function () {
    vac_c_id = $('#vaccination_cm_id').val();
    PopulateVaccinationBarangay(vac_c_id);
});

function PopulateCityMunicipality(p_id) {
    var cmb = $('#cm_id');
    cmb.empty();
    cmb.append(`<option value="0" selected>- City / Municipality -</option>`);
    cmb.val('').trigger('change');

    $.ajax({
        type: "POST",
        url: dir + "/Dashboard/CityMunicipalitiesDataTable",
        data: { provinceId: p_id },
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.isSuccess) {
                $.each(data.data, function (i, rec) {
                    cmb.append(`<option value="${rec.city_municipality_id}">${rec.CityMunicipalityName}</option>`);
                });
                if (data.data.length == 1) {
                    var cityMunicipalityId = $('#cm_id').attr('id');
                    var drp = document.getElementById(cityMunicipalityId);
                    drp.selectedIndex = 1;
                    $('#cm_id').trigger("change").blur();
                    c_id = $('#cm_id').val();
                    PopulateBarangay(c_id);
                }
                else {
                    $('#barangay_id').empty();
                    $('#barangay_id').append(`<option value="0" selected>- Barangay -</option>`);
                }
            } else {
                JsonResultError(data.isRedirect, data.returnUrl, '');
            }
        }
    });
}

function PopulateBarangay(c_id) {
    var barangay = $('#barangay_id');
    barangay.empty();
    barangay.append(`<option value="0" selected>- Barangay -</option>`);
    barangay.val('').trigger('change');

    $.ajax({
        type: "POST",
        url: dir + "/Dashboard/BarangayDataTable",
        data: { citymunId: c_id },
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.isSuccess) {
                $.each(data.data, function (i, rec) {
                    barangay.append(`<option value="${rec.barangay_id}">${rec.barangay_name}</option>`);
                });
            } else {
                JsonResultError(data.isRedirect, data.returnUrl, '');
            }
        }
    });
}


function PopulateVaccinationCityMunicipality(vac_p_id) {
    var cmb = $('#vaccination_cm_id');
    cmb.empty();
    cmb.append(`<option value="0" selected>- City / Municipality -</option>`);

    $.ajax({
        type: "POST",
        url: dir + "/Dashboard/CityMunicipalitiesDataTable",
        data: { provinceId: vac_p_id },
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.isSuccess) {
                $.each(data.data, function (i, rec) {
                    cmb.append(`<option value="${rec.city_municipality_id}">${rec.CityMunicipalityName}</option>`);
                });
                if (data.data.length == 1) {
                    var cityMunicipalityId = $('#vaccination_cm_id').attr('id');
                    var drp = document.getElementById(cityMunicipalityId);
                    drp.selectedIndex = 1;
                    $('#vaccination_cm_id').trigger("change").blur();
                    vac_c_id = $('#vaccination_cm_id').val();
                    PopulateVaccinationBarangay(vac_c_id);
                }
                else {
                    $('#vaccination_barangay_id').empty();
                    $('#vaccination_barangay_id').append(`<option value="0" selected>- Barangay -</option>`)
                }
            } else {
                JsonResultError(data.isRedirect, data.returnUrl, '');
            }
        }
    });
}

function PopulateVaccinationBarangay(vac_c_id) {
    var barangay = $('#vaccination_barangay_id');
    barangay.empty();
    barangay.append(`<option value="0" selected>- Barangay -</option>`);

    $.ajax({
        type: "POST",
        url: dir + "/Dashboard/BarangayDataTable",
        data: { citymunId: vac_c_id },
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.isSuccess) {
                $.each(data.data, function (i, rec) {
                    barangay.append(`<option value="${rec.barangay_id}">${rec.barangay_name}</option>`);
                });
            } else {
                JsonResultError(data.isRedirect, data.returnUrl, '');
            }
        }
    });
}