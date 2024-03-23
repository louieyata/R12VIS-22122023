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

$('#vaccination-search-existing-vaccination-record').on('click', function (e) {
    e.preventDefault();
    $('#modal-search-person').modal('show');
});

$('#search-vpd-vaccination-record-cancel-button').on('click', function (e) {
    e.preventDefault();
    $('#modal-search-person').modal('hide');
});

$(document).on('click', 'a.lnkSelectPeson', function (e) {
    e.preventDefault();
    var person_id = $(this).siblings('#hidPersonID').val();

    $.ajax({
        type: "GET",
        url: dir + "/VaccinationsVPD/GetVPDPerson?person_id=" + person_id,
        dataType: "json",
        success: function (data) {
            if (data.isSuccess) {
                var person = data.person;
                /* Person details here */
                $("#hidPersonId").val(person.person_id);
                $("#individual_first_name").val(person.person_first_name).prop("disabled", true);
                $("#individual_middle_name").val(person.person_middle_name || '').prop("disabled", true);
                $("#individual_last_name").val(person.person_last_name).prop("disabled", true);
                $("#individual_suffix").val(person.person_suffix).trigger('change').prop("disabled", true);
                if (person.person_sex != null) {
                    if (person.person_sex.toLowerCase() == "m") {
                        $("#individual_sex_male").prop("checked", true);
                    } else if (person.person_sex.toLowerCase() == "f") {
                        $("#individual_sex_female").prop("checked", true);
                    }

                    $("#individual_sex_male").prop("disabled", true);
                    $("#individual_sex_female").prop("disabled", true);
                }
                $("#ethnic_group_list").val(person.person_ethnic_group_id).trigger('change').prop("disabled", true);
                $("#individual_date_of_birth").val(person.person_birth_date ? moment(person.person_birth_date.toString()).format('MM/DD/YY') : '').prop("disabled", true);
                $("#individual_pwd").prop("checked", person.person_is_pwd).prop("disabled", true);

                $("#region_id").val(person.person_region_address).trigger('change');
                $("#province_id").val(person.person_province_address).trigger('change').prop("disabled", true);
                $("#cm_id").val(person.person_city_municipality_address).trigger('change').prop("disabled", true);
                $("#barangay_id").val(person.person_barangay_address).trigger('change').prop("disabled", true);

                $("#religion_list").val(person.person_religion_id).trigger('change');//.prop("disabled", true);
                $("#educational_attainment_list").val(person.person_parent_educational_attainment_id).trigger('change');//.prop("disabled", true);
                $("#income_class_list").val(person.person_parent_income_class_id).trigger('change');//.prop("disabled", true);
                $("#mrsia_occupation_list").val(person.person_parent_occupation_id).trigger('change');//.prop("disabled", true);
                $("#sibling_rank_list").val(person.person_sibling_rank).trigger('change');//.prop("disabled", true);

                $('#modal-search-person').modal('hide');

                var vaccination_type_id = $("input[name='radio-vaccination-type']:checked").val();
                console.log("Vaccine type id " + vaccination_type_id);
                PopulateVaccines(vaccination_type_id);
            }
        }
    });
});

$("#vaccination-clear-existing-vaccination-record").on('click', function () {
    // Change the value of #hidPersonId to 0
    //$("#hidPersonId").val(0);

    //$("#individual_first_name").val('').prop("disabled", false);
    //$("#individual_middle_name").val('').prop("disabled", false);
    //$("#individual_last_name").val('').prop("disabled", false);
    //$("#individual_suffix").val('').trigger('change').prop("disabled", false);

    //$("#individual_sex_male").prop("checked", true).prop("disabled", false);
    //$("#individual_sex_female").prop("checked", false).prop("disabled", false);

    //$("#ethnic_group_list").val('').trigger('change').prop("disabled", false);
    //$("#individual_date_of_birth").val('').prop("disabled", false);
    //$("#individual_pwd").prop("checked", true).prop("disabled", false);

    //$("#province_id").val('').trigger('change').prop("disabled", false);

    //$("#religion_list").val('').trigger('change').prop("disabled", false);
    //$("#educational_attainment_list").val('').trigger('change').prop("disabled", false);
    //$("#income_class_list").val('').trigger('change').prop("disabled", false);
    //$("#mrsia_occupation_list").val('').trigger('change').prop("disabled", false);
    //$("#sibling_rank_list").val(1).trigger('change').prop("disabled", false);

    //$('#form-individual-vpd').trigger("reset");
    //var validator = $('#form-individual-vpd').validate();
    //validator.resetForm();
    //validator.settings.ignore = "";

    //PopulateVaccines(0);
    location.reload();
});

function isDateValid(dateString) {
    // Check if the date string is a valid date
    return moment(dateString, "MM/DD/YYYY", true).isValid();
}

function showDateError() {
    toastr.error("Error: Date of vaccination cannot be before date of birth.");
}

function populateVaccines() {
    var vaccination_type_id = $("input[name='radio-vaccination-type']:checked").val();
    console.log("Vaccine type id " + vaccination_type_id);
    PopulateVaccines(vaccination_type_id);
}

$("#vaccination_date_of_vaccination, #individual_date_of_birth, input[name='radio-vaccination-type']").on('change', function () {
    var dob = $("#individual_date_of_birth").val();
    var vaccinationDate = $("#vaccination_date_of_vaccination").val();
    if ((dob != null && dob != "") && (vaccinationDate != null && vaccinationDate != "")) {
        if (!isDateValid(dob) && !isDateValid(vaccinationDate)) {
            // Handle invalid dates
            showDateError();
            return;
        }

        if (moment(dob) > moment(vaccinationDate)) {
            showDateError();
        } else {
            populateVaccines();
        }
    }
});

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
                    url: dir + "/VaccinationsVPD/SaveVaccination",
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

function PopulateVaccines(vaccination_type) {
    var date_of_birth = $("#individual_date_of_birth").val();
    var vaccination_date = $("#vaccination_date_of_vaccination").val();
    var person_id = $("#hidPersonId").val();

    var vac = $('#vaccination_vaccine_id');
    vac.empty();
    vac.append(`<option value="0" selected>- Vaccine -</option>`);

    $.ajax({
        type: "GET",
        url: dir + "/VaccinationsVPD/GetVaccinesAvailable",
        data: { date_of_birth: date_of_birth, vaccination_date: vaccination_date, vaccination_type_id: vaccination_type, person_id: person_id },
        dataType: "json",
        async: true,
        success: function (data) {
            if (data.isSuccess) {
                $.each(data.data, function (i, rec) {
                    vac.append(`<option value="${rec.vaccine_id}">${rec.vaccine_description}</option>`);
                });
            }
        }
    });
}

function PopulateMRSIAVaccinationRecords() {
    tblMRSIAVaccineRecords = $('#tbl-vpd-vaccinations-records').DataTable({
        "destroy": true,
        "ajax": {
            "type": "POST",
            "url": dir + "/VaccinationsVPD/PeopleDataTable",
            "data": {},
            "datatype": "json"
        },
        "responsive": true,
        "lengthChange": true,
        "autoWidth": false,
        "searching": true, // Enable global searching
        "ordering": false,
        "columnDefs": [
            {
                "targets": [0],
                "visible": false
            },
            {
                render: function (data) {
                    return (moment(data).format('MMMM DD, YYYY'));
                    console.log(data)
                },
                targets: [3],
                className: "text-nowrap"
            }
        ],
        "columns": [
            { "name": "person_id", "data": "person_id" },
            { "name": "person_first_name", "data": "person_first_name", "width": "35%" },
            { "name": "person_last_name", "data": "person_last_name", "width": "35%" },
            { "name": "person_birth_date", "data": "person_birth_date", "width": "17%" },
            { "name": "person_sex", "data": "person_sex", "width": "5%" },
            {
                data: null, render: function (data, type, row) {
                    var actionContent = "";
                    actionContent += '<input id="hidPersonID" name="hidPersonID" type="hidden" value="' + row.person_id + '">';
                    actionContent += '<a class="p-l-5 p-r-5 lnkSelectPeson" title="Select" href=""><i class="bi bi-check2-square"></i> Select</a>';/* href = "/People/Edit/' + row.ID + '"*/
                    return actionContent;
                }
            }
        ]
    });
}