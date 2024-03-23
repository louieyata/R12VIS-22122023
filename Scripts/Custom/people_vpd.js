$(document).ready(function () {
    populatePeopleTable();

    $('#form-individual-vpd').validate({
        // Define validation rules and messages here
        rules: {
            individual_first_name: { required: true },
            individual_last_name: { required: true },
            individual_date_of_birth: { required: true },
            province_id: { required: true },
            cm_id: { required: true },
            barangay_id: { required: true },
            sibling_rank_list: { required: true },
            religion_list: { required: true },
            educational_attainment_list: { required: true },
            income_class_list: { required: true },
            occupation_list: { required: true },
            educational_attainment_list: { required: true },
            mrsia_occupation_list: { required: true }
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
    $('#individual_date_of_birth').datepicker({
        endDate: '0d',        // Set the maximum date to today (0 days in the future)
        autoclose: true       // Automatically close the date picker when a date is selected
    });

    $('.select2').select2({
        dropdownParent: $('#modal-edit-person')
    }).on('select2:open', () => {
        document.querySelector('.select2-search__field').focus();
    });
})

$("#form-individual-vpd").submit(function (event) {
    event.preventDefault();

    if ($('#form-individual-vpd').valid()) {
        //Remove.input-invalid class from all input elements
        $("#form-individual .is-invalid").removeClass("is-invalid");
        var formData = GetFormData(this);
        var person_id = $("#hidPersonID").val();

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
                    url: dir + "/PeopleVPD/SavePersonVPD",
                    data: { jsonData: JSON.stringify(formData), person_id: person_id },
                    dataType: "json",
                    async: true,
                    success: function (jsonData) {
                        if (jsonData.isSuccess) {
                            toastr.success("Person successfully saved.");
                            populatePeopleTable();
                            $('#modal-edit-person').modal('hide');
                            //Swal.fire({
                            //    title: "Person was successfully saved.",
                            //    //text: "You won't be able to revert this!",
                            //    icon: "success",
                            //    showCancelButton: false,
                            //    confirmButtonText: "Okay"
                            //}).then((result) => {
                            //    if (result.isConfirmed) {
                            //        populatePeopleTable();
                            //        $('#modal-edit-person').modal('hide');
                            //    }
                            //});
                        } else {
                            toastr.error("There was an error saving the vaccination. " + jsonData.message);
                        }
                    }
                });
            }
        });
    }
});

$('#individual-add-person').on('click', function () {
    $("#individual-modal-header").text("New Person");
    $("#individual-save-button").text("Save");
    isNewPerson = true;
});

$(document).on('click', 'a.lnkEditPeson', function (e) {
    e.preventDefault();
    isNewPerson = false;
    //var personID = $(this).siblings('#hidPersonID').val();
    var personID = $(this).data('personid');

    $("#individual-modal-header").text("Update");
    $("#individual-save-button").text("Update");

    $.ajax({
        type: "GET",
        url: dir + "/PeopleVPD/GetIndividual?person_id=" + personID,
        dataType: "json",
        success: function (data) {
            if (data.isSuccess) {
                /* Person details here */
                $("#hidPersonID").val(data.person_id);
                $("#individual_middle_name").val(data.person_middle_name || '');
                $("#individual_suffix").val(data.Suffix).trigger('change');
                if (data.person_sex != null) {
                    if (data.person_sex.toLowerCase() == "m") {
                        $("#individual_sex_male").prop("checked", true);
                    } else if (data.person_sex.toLowerCase() == "f") {
                        $("#individual_sex_female").prop("checked", true);
                    }
                }
                $("#ethnic_group_list").val(data.person_ethnic_group_id).trigger('change');
                $("#individual_first_name").val(data.person_first_name);
                $("#individual_last_name").val(data.person_last_name);
                $("#individual_pwd").prop("checked", data.person_is_pwd);
                $("#individual_date_of_birth").val(data.person_birth_date ? moment(data.person_birth_date.toString()).format('MM/DD/YYYY') : '');
                $("#province_id").val(data.person_province_address).trigger('change');
                $("#cm_id").val(data.person_city_municipality_address).trigger('change');
                $("#barangay_id").val(data.person_barangay_address).trigger('change');

                $("#sibling_rank_list").val(data.person_sibling_rank).trigger('change');
                $("#religion_list").val(data.person_religion_id).trigger('change');
                $("#educational_attainment_list").val(data.person_parent_educational_attainment_id).trigger('change');
                $("#income_class_list").val(data.person_parent_income_class_id).trigger('change');
                $("#mrsia_occupation_list").val(data.person_parent_occupation_id).trigger('change');

                $('#modal-edit-person').modal('show');
            }
        }
    });
});

$('#province_id').on('change', function () {
    p_id = $('#province_id').val();
    if (p_id == 0) {
        $('#barangay_id').empty();
        $('#barangay_id').append(`<option value="0" selected>- Barangay -</option>`)
    }
    PopulateCityMunicipality(p_id);
});
$('#cm_id').on('change', function () {
    var c_id = $('#cm_id').val();
    PopulateBarangay(c_id);
});

function PopulateCityMunicipality(p_id) {
    var cmb = $('#cm_id');
    cmb.empty();
    cmb.append(`<option value="0" selected>- City / Municipality -</option>`);

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
                    $('#barangay_id').append(`<option value="0" selected>- Barangay -</option>`)
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

function populatePeopleTable() {
    tblPeople = $('#tbl-people').DataTable({
        "destroy": true,
        "ajax": {
            "type": "POST",
            "url": dir + "/PeopleVPD/PeopleDataTable",
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
                "targets": [0, 3], // Index of columns
                "searchable": false, // Disable searching for this column
            }
        ],
        "columns": [
            { "name": "person_id", "data": "person_id" },
            { "name": "person_last_name", "data": "person_last_name", "width": "25%" },
            { "name": "person_first_name", "data": "person_first_name", "width": "25%" },
            { "name": "Address", "data": "Address", "width": "40%" },
            { "name": "Gender", "data": "Gender", "width": "5%" },
            {
                data: null, render: function (data, type, row) {
                    var actionContent = "";
                    actionContent += '<input id="hidPersonID" name="hidPersonID" type="hidden" value="' + row.person_id + '">';
                    actionContent += '<a class="p-l-5 p-r-5 lnkEditPeson" data-personid="' + row.person_id + '" title="Edit" href=""><i class="bi bi-pencil-square"></i></a>';
                    actionContent += '<a class="p-l-5 p-r-5 lnkViewPersonalDetails" style="margin-left:4px" href="/PeopleVPD/PersonalDetails/' + row.person_id + '" title="View Vaccine Details"><i class="bi bi-filter-square"></i></a>';
                    return actionContent;
                }
            }
        ]
    });
}

$('#individual-cancel-button').on('click', function () {
    isNewPerson = true;

    // Clear all fields
    $('#form-individual-vpd').trigger("reset");

    $("#ethnic_group_list").val(0).trigger('change');
    $("#province_id").val(null).trigger('change');
    $("#cm_id").empty();
    $("#cm_id").append(`<option value="0" selected>- City / Municipality -</option>`);
    $("#barangay_id").empty();
    $("#barangay_id").append(`<option value="0" selected>- Barangay -</option>`);
    $("#sibling_rank_list").val(1).trigger('change');
    $("#religion_list").val(0).trigger('change');
    $("#educational_attainment_list").val(0).trigger('change');
    $("#income_class_list").val(0).trigger('change');
    $("#occupation_list").val(0).trigger('change');
    $("#individual_suffix").val(1).trigger('change');

    // Reset validation form and settings
    var validator = $('#form-individual-vpd').validate();
    validator.resetForm();
    validator.settings.ignore = "";

    // Hide modal
    $('#modal-edit-person').modal('hide');
});

// Reset validation on modal show
$('#modal-edit-person').on('show.bs.modal', function () {
    var validator = $('#form-individual-vpd').validate();
    validator.resetForm();
    validator.settings.ignore = "";
});