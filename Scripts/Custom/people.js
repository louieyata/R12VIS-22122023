
var isNewPerson = true;
var p_id = 0;
var c_id = 0;
$(document).ready(function () {
    populatePeopleTable();

    $('#form-individual').validate({
        // Define validation rules and messages here
        rules: {
            individual_unique_person_id: {
                required: true
            },
            individual_first_name: {
                required: true
            },
            individual_last_name: {
                required: true
            },
            individual_contact_number: {
                required: true
            },
            individual_date_of_birth: {
                required: true
            },
            province_id: {
                required: true
            },
            cm_id: {
                required: true
            },
            barangay_id: {
                required: true
            },
            sibling_rank_list: {
                required: true
            },
            religion_list: {
                required: true
            },
            educational_attainment_list: {
                required: true
            },
            income_class_list: {
                required: true
            },
            occupation_list: {
                required: true
            }            
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
        url: dir + "/People/GetIndividual?person_id=" + personID,
        dataType: "json",
        success: function (data) {
            if (data.isSuccess) {
                /* Person details here */

                $("#hidPersonID").val(data.ID);
                $("#individual_middle_name").val(data.MiddleName || '');
                $("#individual_suffix").val(data.Suffix).trigger('change');
                $("#individual_guardian_name").val(data.GuardianName || '');
                if (data.Gender != null) {
                    if (data.Gender.toLowerCase() == "m") {
                        $("#individual_sex_male").prop("checked", true);
                    } else if (data.Gender.toLowerCase() == "f") {
                        $("#individual_sex_female").prop("checked", true);
                    }
                }
                $("#ethnic_group_list").val(data.EthnicGroupID).trigger('change');
                $("#individual_unique_person_id").val(data.UniquePersonID);
                $("#individual_first_name").val(data.FirstName);
                $("#individual_last_name").val(data.LastName);
                $("#individual_contact_number").val(data.ContactNumber);
                $("#individual_pwd").prop("checked", data.isPWD);
                $("#individual_date_of_birth").val(data.BirthDate ? moment(data.BirthDate.toString()).format('MM/DD/YYYY') : '');
                $("#province_id").val(data.ProvinceID).trigger('change');
                $("#cm_id").val(data.CityMunicipalityID).trigger('change');
                $("#barangay_id").val(data.BarangayID).trigger('change');

                $("#sibling_rank_list").val(data.SiblingRank).trigger('change');
                $("#religion_list").val(data.ReligionID).trigger('change');
                $("#educational_attainment_list").val(data.EducationalAttainmentID).trigger('change');
                $("#income_class_list").val(data.IncomeClassID).trigger('change');
                $("#occupation_list").val(data.OccupationID).trigger('change');


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

$("#form-individual").submit(function (event) {
    event.preventDefault();

    if ($('#form-individual').valid()) {
        //Remove.input-invalid class from all input elements
        $("#form-individual .is-invalid").removeClass("is-invalid");

        const id = $("#hidPersonID").val();
        let up_id = $('#individual_unique_person_id').val();
        let f_name = $("#individual_first_name").val();
        let m_name = $("#individual_middle_name").val() == "" ? "NONE" : $("#individual_middle_name").val();
        let l_name = $("#individual_last_name").val();
        let suffix = $("#individual_suffix option:selected").text() || "N/A";
        let c_num = $("#individual_contact_number").val();
        let g_name = $("#individual_guardian_name").val() == "" ? "N/A" : $("#individual_guardian_name").val();;
        let gen = $("input[type='radio'][name='radio-sex']:checked").val();
        let pwd = $("#individual_pwd").prop("checked");
        let e_group = $("#ethnic_group_list option:selected").val();
        let b_date = $("#individual_date_of_birth").val();
        let prov = $("#province_id option:selected").val();
        let cm = $("#cm_id option:selected").val();
        let brgy = $("#barangay_id option:selected").val();

        let sr = $("#sibling_rank_list").val();
        let rel = $("#religion_list").val();
        let edat = $("#educational_attainment_list").val();
        let ic = $("#income_class_list").val();
        let occ = $("#occupation_list").val();

        let individual = {
            ID: id,
            UniquePersonID: up_id,
            FirstName: f_name,
            MiddleName: m_name,
            LastName: l_name,
            Suffix: suffix,
            ContactNumber: c_num,
            GuardianName: g_name,
            Gender: gen,
            isPWD: pwd,
            EthnicGroupID: e_group,
            BirthDate: b_date,
            ProvinceID: prov,
            CityMunicipalityID: cm,
            BarangayID: brgy,
            SiblingRank: sr,
            Religion: rel,
            EducationalAttainment: edat,
            IncomeClass: ic,
            Occupation: occ
        }
        Swal.fire({
            title: "Do you want to save this data?",
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Save"
        }).then(async (result) => {
            if (result.isConfirmed) {
                try {
                    let response = await $.ajax({
                        type: "POST",
                        url: dir + "/People/SaveIndividual",
                        data: JSON.stringify({ individual: individual, isNew: isNewPerson }),
                        contentType: 'application/json',
                        dataType: "json"
                    });

                    if (response.isSuccess) {
                        toastr.success("Person data saved successfully.");
                        populatePeopleTable();
                        $("#modal-edit-person").modal("hide");
                    } else {
                        toastr.error(`Error: ${response.message}`);
                        console.log(`Error: ${response.message}`);
                    }
                } catch (error) {
                    console.error("An error occurred during the AJAX request:", error);
                }
            }
        });
    }
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
            "url": dir + "/People/PeopleDataTable",
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
            { "name": "ID", "data": "ID" },
            { "name": "LastName", "data": "LastName", "width": "25%" },
            { "name": "FirstName", "data": "FirstName", "width": "25%" },
            { "name": "Address", "data": "Address", "width": "40%" },
            { "name": "Gender", "data": "Gender", "width": "5%" },
            {
                data: null, render: function (data, type, row) {
                    var actionContent = "";
                    actionContent += '<input id="hidPersonID" name="hidPersonID" type="hidden" value="' + row.ID + '">';
                    actionContent += '<a class="p-l-5 p-r-5 lnkEditPeson" data-personid="' + row.ID + '" title="Edit" href=""><i class="bi bi-pencil-square"></i></a>';
                    actionContent += '<a class="p-l-5 p-r-5 lnkViewPersonalDetails" style="margin-left:4px" href="/People/PersonalDetails/' + row.ID + '" title="View Vaccine Details"><i class="bi bi-filter-square"></i></a>';
                    return actionContent;
                }
            }
        ]
    });
}

$('#individual-cancel-button').on('click', function () {
    isNewPerson = true;

    // Clear all fields
    $('#form-individual').trigger("reset");

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
    var validator = $('#form-individual').validate();
    validator.resetForm();
    validator.settings.ignore = "";

    // Hide modal
    $('#modal-edit-person').modal('hide');
});

// Reset validation on modal show
$('#modal-edit-person').on('show.bs.modal', function () {
    var validator = $('#form-individual').validate();
    validator.resetForm();
    validator.settings.ignore = "";
});
