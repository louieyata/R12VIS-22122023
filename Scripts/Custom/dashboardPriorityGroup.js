const specificColors = ['#89CFF0', '#0096FF', '#6495ED', '#96DED1', '#40E0D0', '#50C878', '#2AAA8A', '#40B5AD', '#CCCCFF', '#00FFFF', '#9FE2BF', '#7FFFD4', '#B6D0E2']; // Add more colors as needed '#FFAC1C', '#FFBF00', '#FF7F50', '#F88379', '#FAC898'
let BoosterPieChart = null; // Store the Chart instance
var vaccine_idlist = [];

$(document).ready(function () {
    //vaccine_idlist = [1, 2, 3, 4, 5, 6, 7, 8]; /* , 9, 10, 11 - vaccine ids sputnik and bivalents */
    /*Data Table*/
    PopulatePriorityGroupDataTable(vaccine_idlist, 0, 0, 0);

    console.log("vaccine_ids " + vaccine_idlist);
    // Get all the checkboxes with the name "vaccine_brands"
    const checkboxes = document.querySelectorAll('input[name="vaccine_brands"]');
    // Add a click event listener to each checkbox
    checkboxes.forEach(checkbox => {
        checkbox.addEventListener('click', function () {
            var province = $("#province_id").val();
            var citymun = $("#cm_id").val();
            var barangays = $("#barangay_id").val();
            const checkboxValue = parseInt(this.value, 10);

            // Check if the checkbox is checked or unchecked
            if (this.checked) {
                // Checkbox is checked, add the integer value to the array
                vaccine_idlist.push(checkboxValue);
                console.log(`Checkbox "${this.value}" is checked.`);
            } else {
                // Checkbox is unchecked, remove the integer value from the array
                const index = vaccine_idlist.indexOf(checkboxValue);
                if (index !== -1) {
                    vaccine_idlist.splice(index, 1);
                }
                console.log(`Checkbox "${this.value}" is unchecked.`);
            }

            // Display the current contents of the vaccine_idlist array
            console.log("vaccine_idlist:", vaccine_idlist);
            //loadSecondBoosterSeniorPieChart(vaccine_idlist, province, citymun);
            PopulatePriorityGroupDataTable(vaccine_idlist, province, citymun, barangays);
            //PopulateVaccineTable(vaccine_idlist, province, citymun);
        });
    });
});

$("#filter-button").click(function () {
    var province = $("#province_id").val();
    var citymun = $("#cm_id").val();
    var barangays = $("#barangay_id").val();
    toastr.success("Please wait..");

    /*Data Table*/
    PopulatePriorityGroupDataTable(vaccine_idlist, province, citymun, barangays);
});

function PopulatePriorityGroupDataTable(vaccine_ids, province_id, citymun_id, barangay_id) {
    // Define the DataTable configuration options
    var dataTableConfig = {
        destroy: true,
        responsive: true,
        lengthChange: true,
        autoWidth: false,
        searching: false,
        paging: false,
        info: false,
        entries: false,
        ordering: false,
        select: { style: 'single' },
        columnDefs: [],
        "columns": [
            { "name": "categoryname", "data": "categoryname", "width": "25%" },
            { "name": "firstdose", "data": "firstdose", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "seconddose", "data": "seconddose", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "firstbooster", "data": "firstbooster", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "secondbooster", "data": "secondbooster", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "thirdbooster", "data": "thirdbooster", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "total", "data": "total", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } }
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/PriorityGroupDataTable",
        data: {
            vaccine_ids: vaccine_ids,
            province_id: province_id,
            citymun_id: citymun_id,
            barangay_id: barangay_id,
        },
        datatype: "json",

        //begin added code
        success: function (responseData) {
            var data = responseData.data; // DataTable data

            // Update the DataTable
            tblPriorityGroup.clear().rows.add(data).draw();

            // Calculate the sum of the "total" column
            var totalSum = 0;
            var totalColumnData = tblPriorityGroup.column('total:name').data();
            totalColumnData.each(function (value, index) {
                if (!isNaN(value) && index < totalColumnData.length - 1) {
                    totalSum += parseInt(value);
                }
            });

            // Update the content of #dashboard1total based on the DataTable's total data
            if (!isNaN(totalSum)) {
                $('#dashboard1total').text(totalSum.toLocaleString('en-US', { maximumFractionDigits: 0 }));
            } else {
                $('#dashboard1total').text('TOTAL: N/A');
            }
        },
        error: function (xhr, status, error) {
            if (xhr.status === 404) {
                console.error("Resource not found. Please check the URL.");
            } else if (xhr.status === 500) {
                console.error("Internal server error. Please try again later.");
            } else {
                console.error("An error occurred. Please refresh the page and try again.");
            }
        }
        //end added code
    };

    // Create the DataTable using the defined configuration options
    tblPriorityGroup = $('#tbl-prioritygroup').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}

$('#province_id').on('change', function(){
    p_id = $('#province_id').val();
    if (p_id == 0) {
        $('#barangay_id').empty();
        $('#barangay_id').append(`<option value="0" selected>- Barangay -</option>`)
    }
    PopulateCityMunicipality(p_id);
});

$('#cm_id').on('change', PopulateBarangay);


$('#province_id').on('select2:open', () => {
    document.querySelector('.select2-search__field').focus();
});
$('#cm_id').on('select2:open', () => {
    document.querySelector('.select2-search__field').focus();
});
$('#barangay_id').on('select2:open', () => {
    document.querySelector('.select2-search__field').focus();
});

function PopulateCityMunicipality() {
    var provinceId = $("#province_id").val();
    var cmb = $('#cm_id');
    cmb.empty();
    cmb.append(`<option value="0" selected>- City / Municipality -</option>`);

    $.ajax({
        type: "POST",
        url: dir + "/Dashboard/CityMunicipalitiesDataTable",
        data: { provinceId: provinceId },
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.isSuccess) {
                $.each(data.data, function (i, rec) {
                    cmb.append(`<option value="${rec.city_municipality_id}">${rec.CityMunicipalityName}</option>`);
                });
                console.log("length " + data.data.length);

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

function PopulateBarangay() {
    var citymunId = $("#cm_id").val();
    var brgy = $('#barangay_id');
    brgy.empty();
    brgy.append(`<option value="0" selected>- Barangay -</option>`);

    $.ajax({
        type: "POST",
        url: dir + "/Dashboard/BarangayDataTable",
        data: { citymunId: citymunId },
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.isSuccess) {
                $.each(data.data, function (i, rec) {
                    brgy.append(`<option value="${rec.barangay_id}">${rec.barangay_name}</option>`);
                });
                console.log("length " + data.data.length);

                if (data.data.length == 1) {
                    var barangayId = $('#barangay_id').attr('id');
                    var drp = document.getElementById(barangayId);
                    drp.selectedIndex = 1;
                    $('#barangay_id').trigger("change").blur();

                    //cmb.selectedIndex = 1;
                    //cmb.trigger("change").blur();
                }
            } else {
                JsonResultError(data.isRedirect, data.returnUrl, '');
            }
        }
    });
}