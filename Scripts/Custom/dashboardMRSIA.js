const specificColors = ['#89CFF0', '#0096FF', '#6495ED', '#96DED1', '#40E0D0', '#50C878', '#2AAA8A', '#40B5AD', '#CCCCFF', '#00FFFF', '#9FE2BF', '#7FFFD4', '#B6D0E2']; // Add more colors as needed '#FFAC1C', '#FFBF00', '#FF7F50', '#F88379', '#FAC898'
let region_id = 1;
let province = 0;
let citymun = 0;
let schedule_is_active = 1;
let vaccine_type_id = 2;
var BarChartProvince = null;
var BarChartCityMun = null;
var BarChartOccupation = null;
var BarChartIncome = null;
var BarChartReligion = null;
var BarChartSiblingRank = null;

$(document).ready(function () {
    /*Data Table*/
    PopulateMRSIADataTable(region_id);
    PopulateROUTINEDataTableProvince(region_id);
    PopulateROUTINEVaxxDashboard(vaccine_type_id);
    fetchDataProvince(region_id);
    fetchDataOccupation();
    fetchDataIncome();
    fetchDataReligion();
    fetchDataSiblingRank();
});

$("#filter-button").click(function () {
    var region = 1;
    var province = $("#province_id").val();
    var citymun = $("#cm_id").val();

    console.log("province:", province);
    console.log("citymun:", citymun);


    if (province && (citymun == 0)) {
        toastr.success("loaded succesfully..");
        PopulateMRSIADataTableCityMun(province);
        PopulateROUTINEDataTableCityMun(province);
        fetchDataCityMun(province);
        fetchDataOccupation(region, province);
        fetchDataIncome(region, province);
        fetchDataReligion(region, province);
        fetchDataSiblingRank(region, province);
    } else if (province && citymun) {
        PopulateMRSIADataTableBarangay(citymun);
        PopulateROUTINEDataTableBarangay(citymun);
        fetchDataBarangay(citymun);
        fetchDataOccupation(region, province, citymun);
        fetchDataIncome(region, province, citymun);
        fetchDataReligion(region, province, citymun);
        fetchDataSiblingRank(region, province, citymun);
    } else {
        toastr.success("loaded succesfully..");
        PopulateMRSIADataTable(region_id);
        PopulateROUTINEDataTableProvince(region_id);
        fetchDataProvince(region_id);
        fetchDataOccupation(region, province);
        fetchDataIncome(region, province);
        fetchDataReligion(region, province);
        fetchDataSiblingRank(region, province);
    }
});
function PopulateMRSIADataTable(region_id) {
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
        //scrollX: true,
        //scrollY: 350,
        "columns": [
            { "name": "Province", "data": "Province", "width": "25%" },
            { "name": "TARGET", "data": "TARGET1", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "ACCOMPLISHMENT", "data": "OPV", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "PERCENT", "data": "PERCENT1", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseFloat(data).toFixed(2) + '%' : ''; } },
            { "name": "TARGET", "data": "TARGET2", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "ACCOMPLISHMENT", "data": "MRV", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "PERCENT", "data": "PERCENT2", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseFloat(data).toFixed(2) + '%' : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/MRSIADataTable",
        data: {
            region_id: region_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            var totalData = responseData.total; // Total data

            // Update the DataTable
            tblMRSIA.clear().rows.add(data).draw();

            // Update the "dashboard1total" element with the total values
            $("#dashboard1total").html(totalData.OPV + totalData.MRV + totalData.VitaminA);
            $("#dashboard2total").html(totalData.OPV + totalData.MRV + totalData.VitaminA);
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
    };

    // Create the DataTable using the defined configuration options
    tblMRSIA = $('#tbl-mr-opv-sia').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}

function PopulateMRSIADataTableCityMun(province_id) {
    // Define the DataTable configuration options
    var dataTableConfig = {
        destroy: true,
        responsive: true,
        lengthChange: true,
        autoWidth: false,
        searching: true,
        paging: true,
        info: false,
        entries: false,
        ordering: false,
        select: { style: 'single' },
        columnDefs: [],
        scrollX: false,
        scrollY: 352,
        "columns": [
            { "title": "City/Municipality", "data": "CityMunName", "width": "25%" },
            { "title": "TARGET", "data": "TARGET1", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "title": "ACCOMPLISHMENT", "data": "OPV", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "title": "PERCENT", "data": "PERCENT1", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseFloat(data).toFixed(2) + '%' : ''; } },
            { "title": "TARGET", "data": "TARGET2", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "title": "ACCOMPLISHMENT", "data": "MRV", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "title": "PERCENT", "data": "PERCENT2", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseFloat(data).toFixed(2) + '%' : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/MRSIADataTableCityMun",
        data: {
            province_id: province_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            var totalData = responseData.total; // Total data

            // Update the DataTable
            tblMRSIA.clear().rows.add(data).draw();

            // Update the "dashboard1total" element with the total values
            $("#dashboard1total").html(totalData.OPV + totalData.MRV + totalData.VitaminA);
            $("#dashboard2total").html(totalData.OPV + totalData.MRV + totalData.VitaminA);
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
    };

    // Create the DataTable using the defined configuration options
    tblMRSIA = $('#tbl-mr-opv-sia').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}

function PopulateMRSIADataTableBarangay(cm_id) {
    // Define the DataTable configuration options
    var dataTableConfig = {
        destroy: true,
        responsive: true,
        lengthChange: true,
        autoWidth: false,
        searching: true,
        paging: true,
        info: false,
        entries: false,
        ordering: false,
        select: { style: 'single' },
        columnDefs: [],
        scrollX: false,
        scrollY: 352,
        "columns": [
            { "title": "Barangay", "data": "BarangayName", "width": "25%" },
            { "title": "TARGET", "data": "TARGET1", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "title": "ACCOMPLISHMENT", "data": "OPV", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "title": "PERCENT", "data": "PERCENT1", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseFloat(data).toFixed(2) + '%' : ''; } },
            { "title": "TARGET", "data": "TARGET2", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "title": "ACCOMPLISHMENT", "data": "MRV", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "title": "PERCENT", "data": "PERCENT2", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseFloat(data).toFixed(2) + '%' : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/MRSIADataTableBarangay",
        data: {
            cm_id: cm_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            var totalData = responseData.total; // Total data

            // Update the DataTable
            tblMRSIA.clear().rows.add(data).draw();

            // Update the "dashboard1total" element with the total values
            $("#dashboard1total").html(totalData.OPV + totalData.MRV + totalData.VitaminA);
            $("#dashboard2total").html(totalData.OPV + totalData.MRV + totalData.VitaminA);
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
    };

    // Create the DataTable using the defined configuration options
    tblMRSIA = $('#tbl-mr-opv-sia').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}

function PopulateROUTINEDataTableProvince(region_id) {
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
        fixedHeader: true,
        columnDefs: [{ "className": "text-center", "targets": [0, 1] }],
        "columns": [
            { "name": "Province", "data": "Province", "width": "15%" },
            { "name": "TOTAL", "data": "TOTAL", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutineProvinceDataTable",
        data: {
            region_id: region_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data;
            var totalData = responseData.total;

            // Update the DataTable
            tblROUTINE.clear().rows.add(data).draw();

            // Update the "dashboard1total" element with the total values
            $("#dashboardroutinetotal").html(totalData.TOTAL);
            /*$("#dashboardroutine2total").html(totalData.TOTAL);*/
            if (totalData.TOTAL == 0)
                $('#dashboardroutinetotal').text("No data");
            //    /*$('#dashboardroutine2total').text("No data");*/
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
    };

    // Create the DataTable using the defined configuration options
    tblROUTINE = $('#tbl-routine').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}

function PopulateROUTINEDataTableCityMun(province_id) {
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
        columnDefs: [{ "className": "text-center", "targets": [0, 1] }],
        "columns": [
            { "name": "CityMun", "data": "CityMun", "width": "15%" },
            { "name": "TOTAL", "data": "TOTAL", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutineCityMunDataTable",
        data: {
            province_id: province_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            var totalData = responseData.total; // Total data

            // Update the DataTable
            tblROUTINE.clear().rows.add(data).draw();

            // Update the "dashboard1total" element with the total values
            $("#dashboardroutinetotal").html(totalData.TOTAL);
            $("#dashboardroutine2total").html(totalData.TOTAL);
            if (totalData.TOTAL == 0)
                $('#dashboardroutinetotal').text("No data");
                /*$('#dashboardroutine2total').text("No data");*/
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
    };

    // Create the DataTable using the defined configuration options
    tblROUTINE = $('#tbl-routine').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}

function PopulateROUTINEDataTableBarangay(cm_id) {
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
        columnDefs: [{ "className": "text-center", "targets": [0, 1] }],
        scrollY: 360,
        "columns": [
            { "name": "BarangayName", "data": "BarangayName", "width": "15%" },
            { "name": "TOTAL", "data": "TOTAL", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutineBarangayDataTable",
        data: {
            cm_id: cm_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            var totalData = responseData.total; // Total data

            // Update the DataTable
            tblROUTINE.clear().rows.add(data).draw();

            // Update the "dashboard1total" element with the total values
            $("#dashboardroutinetotal").html(totalData.TOTAL);
            $("#dashboardroutine2total").html(totalData.TOTAL);
            if (totalData.TOTAL == 0)
                $('#dashboardroutinetotal').text("No data");
                /*$('#dashboardroutine2total').text("No data");*/
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
    };

    // Create the DataTable using the defined configuration options
    tblROUTINE = $('#tbl-routine').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}



document.addEventListener('DOMContentLoaded', function () {
    var cards = document.querySelectorAll('.card-dashboard');

    cards.forEach(function (card) {
        card.addEventListener('click', function () {
            $('#modal-search-person').modal('show');
            console.log("Card clicked!");
            PopulateROUTINEDataTableSchedule(schedule_is_active);
        });
    });
});

function PopulateROUTINEDataTableSchedule(schedule_is_active) {
    tblMRSIAVaccineRecords = $('#tbl-vpd-vaccinations-records').DataTable({
        "destroy": true,
        "ajax": {
            "type": "POST",
            "url": dir + "/Dashboard/RoutineSchedules",
            "data": { schedule_is_active: schedule_is_active },
            "datatype": "json"
        },
        "responsive": true,
        "lengthChange": true,
        "autoWidth": false,
        "searching": true, // Enable global searching
        "ordering": false,
        "columnDefs": [
            {
                //"targets": [0],
                //"visible": false
            },
            {
                render: function (data) {
                    return (moment(data).format('MMMM DD, YYYY'));
                    console.log(data)
                },
                targets: [4],
                className: "text-nowrap"
            }
        ],
        "columns": [

            { "name": "NO", "data": "No_", "className": "left-aligned" },
            { "name": "person_first_name", "data": "First_Name", "className": "left-aligned" },
            { "name": "person_last_name", "data": "Last_Name", "className": "left-aligned" },
            { "name": "Vaccine Dose", "data": "Vaccine_Dose", "className": "left-aligned" },
            { "name": "Schedule", "data": "Schedule", "className": "left-aligned" },
        ]
    });
}


function PopulateROUTINEVaxxDashboard(vaccine_type_id) {
    // Define the DataTable configuration options
    var dataTableConfig = {
        destroy: true,
        responsive: true,
        lengthChange: true,
        autoWidth: true,
        searching: false,
        paging: false,
        info: false,
        entries: false,
        ordering: false,
        select: { style: 'single' },
        fixedHeader: true,
        columnDefs: [/*{ "className": "text-center", "targets": [0, 1] }*/],
        scrollX: false,
        "columns": [
            { "title": "Vaccine", "data": "Vaccine", "width": "40%"},
            { "title": "AT BIRTH", "data": "AT_BIRTH", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "title": "1ST VISIT", "data": "C1ST_VISIT", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "title": "2ND VISIT", "data": "C2ND_VISIT", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "title": "3RD VISIT", "data": "C3RD_VISIT", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "title": "4TH VISIT", "data": "C4TH_VISIT", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "title": "5TH VISIT", "data": "C5TH_VISIT", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
           
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutineVaxxDashboard",
        data: {
            vaccine_type_id: vaccine_type_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            var totalData = responseData.total; // Total data

            // Update the DataTable
            tblROUTINE.clear().rows.add(data).draw();
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
    };

    // Create the DataTable using the defined configuration options
    tblROUTINE = $('#tbl-routine-vaxx').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}






$('#search-vpd-vaccination-record-cancel-button').on('click', function (e) {
    e.preventDefault();
    $('#modal-search-person').modal('hide');
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

$('#province_id').on('select2:open', () => {
    document.querySelector('.select2-search__field').focus();
});
$('#cm_id').on('select2:open', () => {
    document.querySelector('.select2-search__field').focus();
});
$('#barangay_id').on('select2:open', () => {
    document.querySelector('.select2-search__field').focus();
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

function fetchDataProvince(region_id) {
    var ctx = document.getElementById('VaccinatedChart').getContext('2d');

    // Check if a chart instance already exists
    if (BarChartProvince !== null) {
        BarChartProvince.destroy(); // Destroy the previous chart instance
    }

    $.ajax({
        url: '/Dashboard/MRSIADataTable',
        method: 'GET',
        data: { region_id: region_id },
        success: function (result) {
            if (result.isSuccess) {
                var data = result.data;

                // Extract the relevant data for the chart
                var labels = [];
                var OPVData = [];
                var MRVData = [];
                var TARGET1Data = [];
                var TARGET2Data = [];

                for (var i = 0; i < data.length; i++) {
                    if (data[i].Province !== "TOTAL") {
                        labels.push(data[i].Province);
                        OPVData.push(data[i].OPV);
                        MRVData.push(data[i].MRV);
                        TARGET1Data.push(data[i].TARGET1);
                        TARGET2Data.push(data[i].TARGET2);
                    }
                }

                BarChartProvince = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: labels,
                        datasets: [
                            {
                                label: 'OPV',
                                data: OPVData,
                                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                                borderColor: 'rgba(75, 192, 192, 1)',
                                borderWidth: 1,
                            },
                            {
                                label: 'MRV',
                                data: MRVData,
                                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                                borderColor: 'rgba(255, 99, 132, 1)',
                                borderWidth: 1,
                            },
                            {
                                label: 'OPV TARGETS',
                                data: TARGET1Data,
                                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                                borderColor: 'rgba(54, 162, 235, 1)',
                                borderWidth: 1,
                            },
                            {
                                label: 'MRV TARGETS',
                                data: TARGET2Data,
                                backgroundColor: 'rgba(255, 205, 86, 0.2)',
                                borderColor: 'rgba(255, 205, 86, 1)',
                                borderWidth: 1,
                            }
                        ]

                    },
                    options: {
                        scales: {
                            y: {
                                beginAtZero: true
                            }
                        }
                    }
                });
            }
        }
    });
}

function fetchDataCityMun(province_id) {
    var ctx = document.getElementById('VaccinatedChart').getContext('2d');
    // Check if a chart instance already exists and destroy it
    if (BarChartProvince !== null) {
        BarChartProvince.destroy();
    }

    $.ajax({
        url: '/Dashboard/MRSIADataTableCityMun',
        method: 'GET',
        data: { province_id: province_id },
        success: function (result) {
            if (result.isSuccess) {
                var data = result.data;

                // Extract the relevant data for the chart
                var labels = [];
                var OPVData = [];
                var MRVData = [];
                var TARGET1Data = [];
                var TARGET2Data = [];

                for (var i = 0; i < data.length; i++) {
                    if (data[i].CityMunName !== "TOTAL") {
                        labels.push(data[i].CityMunName);
                        OPVData.push(data[i].OPV);
                        MRVData.push(data[i].MRV);
                        TARGET1Data.push(data[i].TARGET1);
                        TARGET2Data.push(data[i].TARGET2);
                    }
                }
                BarChartProvince = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: labels,
                        datasets: [
                            {
                                label: 'OPV',
                                data: OPVData,
                                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                                borderColor: 'rgba(75, 192, 192, 1)',
                                borderWidth: 1,
                            },
                            {
                                label: 'MRV',
                                data: MRVData,
                                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                                borderColor: 'rgba(255, 99, 132, 1)',
                                borderWidth: 1,
                            },
                            {
                                label: 'OPV TARGETS',
                                data: TARGET1Data,
                                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                                borderColor: 'rgba(54, 162, 235, 1)',
                                borderWidth: 1,
                            },
                            {
                                label: 'MRV TARGETS',
                                data: TARGET2Data,
                                backgroundColor: 'rgba(255, 205, 86, 0.2)',
                                borderColor: 'rgba(255, 205, 86, 1)',
                                borderWidth: 1,
                            }
                        ]
                    },
                    options: {
                        scales: {
                            y: {
                                beginAtZero: true
                            }
                        }
                    }
                });
            }
        }
    });
}

function fetchDataBarangay(cm_id) {
    var ctx = document.getElementById('VaccinatedChart').getContext('2d');
    // Check if a chart instance already exists and destroy it
    if (BarChartProvince !== null) {
        BarChartProvince.destroy();
    }

    $.ajax({
        url: '/Dashboard/MRSIADataTableBarangay',
        method: 'GET',
        data: { cm_id: cm_id },
        success: function (result) {
            if (result.isSuccess) {
                var data = result.data;

                // Extract the relevant data for the chart
                var labels = [];
                var OPVData = [];
                var MRVData = [];
                var TARGET1Data = [];
                var TARGET2Data = [];

                for (var i = 0; i < data.length; i++) {
                    if (data[i].BarangayName !== "TOTAL") {
                        labels.push(data[i].BarangayName);
                        OPVData.push(data[i].OPV);
                        MRVData.push(data[i].MRV);
                        TARGET1Data.push(data[i].TARGET1);
                        TARGET2Data.push(data[i].TARGET2);
                    }
                }
                BarChartProvince = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: labels,
                        datasets: [
                            {
                                label: 'OPV',
                                data: OPVData,
                                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                                borderColor: 'rgba(75, 192, 192, 1)',
                                borderWidth: 1,
                            },
                            {
                                label: 'MRV',
                                data: MRVData,
                                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                                borderColor: 'rgba(255, 99, 132, 1)',
                                borderWidth: 1,
                            },
                            {
                                label: 'OPV TARGETS',
                                data: TARGET1Data,
                                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                                borderColor: 'rgba(54, 162, 235, 1)',
                                borderWidth: 1,
                            },
                            {
                                label: 'MRV TARGETS',
                                data: TARGET2Data,
                                backgroundColor: 'rgba(255, 205, 86, 0.2)',
                                borderColor: 'rgba(255, 205, 86, 1)',
                                borderWidth: 1,
                            }
                        ]
                    },
                    options: {
                        scales: {
                            y: {
                                beginAtZero: true
                            }
                        }
                    }
                });
            }
        }
    });
}


function fetchDataOccupation(region_id, province_id, cm_id) {
    var ctx = document.getElementById('vaccineChartParentOccupation').getContext('2d');
    // Check if a chart instance already exists and destroy it
    if (BarChartOccupation !== null) {
        BarChartOccupation.destroy();
    }

    $.ajax({
        url: '/Dashboard/BarchartMRSIAOccupationData',
        method: 'GET',
        data: { region_id: region_id, province_id: province_id, cm_id: cm_id },
        success: function (result) {
            if (result.isSuccess) {
                var data = result.data;

                // Extract the relevant data for the chart
                var labels = [];
                var Occupationdescriptions = [];
                var OPVData = [];
                var MRVData = [];

                for (var i = 0; i < data.length; i++) {
                    if (data[i].descriptionName !== "TOTAL") {
                        // Store the original description
                        Occupationdescriptions.push(data[i].descriptionName);

                        // Create aliases for long descriptions
                        var OccupationAlias = createAliasForOccupationDescription(data[i].descriptionName);
                        labels.push(OccupationAlias);

                        //labels.push(data[i].descriptionName);
                        OPVData.push(data[i].OPV);
                        MRVData.push(data[i].MRV);
                    }
                }
                BarChartOccupation = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: labels,
                        datasets: [
                            {
                                label: 'OPV',
                                data: OPVData,
                                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                                borderColor: 'rgba(75, 192, 192, 1)',
                                borderWidth: 1,
                            },
                            {
                                label: 'MRV',
                                data: MRVData,
                                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                                borderColor: 'rgba(255, 99, 132, 1)',
                                borderWidth: 1,
                            }
                        ]
                    },
                    options: {
                        scales: {
                            y: {
                                beginAtZero: true
                            }
                        },
                        plugins: {
                            tooltip: {
                                callbacks: {
                                    title: function (context) {
                                        var index = context[0].dataIndex;
                                        return Occupationdescriptions[index];
                                    },
                                },
                            },
                        },
                    }
                });
            }
        }
    });
}

function fetchDataIncome(region_id, province_id, cm_id) {
    var ctx = document.getElementById('vaccineChartIncome').getContext('2d');
    // Check if a chart instance already exists and destroy it
    if (BarChartIncome !== null) {
        BarChartIncome.destroy();
    }

    $.ajax({
        url: '/Dashboard/BarchartMRSIAIncomeClassData',
        method: 'GET',
        data: { region_id: region_id, province_id: province_id, cm_id: cm_id },
        success: function (result) {
            if (result.isSuccess) {
                var data = result.data;

                // Extract the relevant data for the chart
                var labels = [];
                var Incomedescriptions = [];
                var OPVData = [];
                var MRVData = [];

                for (var i = 0; i < data.length; i++) {
                    if (data[i].descriptionName !== "TOTAL") {
                        // Store the original description
                        Incomedescriptions.push(data[i].descriptionName);

                        // Create aliases for long descriptions
                        var IncomeAlias = createAliasForIncomeDescription(data[i].descriptionName);
                        labels.push(IncomeAlias);

                        //labels.push(data[i].descriptionName);
                        OPVData.push(data[i].OPV);
                        MRVData.push(data[i].MRV);
                    }
                }
                BarChartIncome = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: labels,
                        datasets: [
                            {
                                label: 'OPV',
                                data: OPVData,
                                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                                borderColor: 'rgba(75, 192, 192, 1)',
                                borderWidth: 1,
                            },
                            {
                                label: 'MRV',
                                data: MRVData,
                                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                                borderColor: 'rgba(255, 99, 132, 1)',
                                borderWidth: 1,
                            }
                        ]
                    },
                    options: {
                        scales: {
                            y: {
                                beginAtZero: true
                            }
                        },
                        plugins: {
                            tooltip: {
                                callbacks: {
                                    title: function (context) {
                                        var index = context[0].dataIndex;
                                        return Incomedescriptions[index];
                                    },
                                },
                            },
                        },
                    }
                });
            }
        }
    });
}

function fetchDataReligion(region_id, province_id, cm_id) {
    var ctx = document.getElementById('vaccineChartReligion').getContext('2d');
    // Check if a chart instance already exists and destroy it
    if (BarChartReligion !== null) {
        BarChartReligion.destroy();
    }

    $.ajax({
        url: '/Dashboard/BarchartMRSIAReligionData',
        method: 'GET',
        data: { region_id: region_id, province_id: province_id, cm_id: cm_id },
        success: function (result) {
            if (result.isSuccess) {
                var data = result.data;

                // Extract the relevant data for the chart
                var labels = [];
                var descriptions = [];
                var OPVData = [];
                var MRVData = [];

                for (var i = 0; i < data.length; i++) {
                    if (data[i].descriptionName !== "TOTAL") {
                        // Store the original description
                        descriptions.push(data[i].descriptionName);

                        // Create aliases for long descriptions
                        var alias = createAliasForReligion(data[i].descriptionName);
                        labels.push(alias);

                        OPVData.push(data[i].OPV);
                        MRVData.push(data[i].MRV);
                    }
                }
                BarChartReligion = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: labels,
                        datasets: [
                            {
                                label: 'OPV',
                                data: OPVData,
                                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                                borderColor: 'rgba(75, 192, 192, 1)',
                                borderWidth: 1,
                            },
                            {
                                label: 'MRV',
                                data: MRVData,
                                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                                borderColor: 'rgba(255, 99, 132, 1)',
                                borderWidth: 1,
                            }
                        ]
                    },
                    options: {
                        scales: {
                            y: {
                                beginAtZero: true
                            }
                        },
                        plugins: {
                            tooltip: {
                                callbacks: {
                                    title: function (context) {
                                        var index = context[0].dataIndex;
                                        return descriptions[index];
                                    },
                                },
                            },
                        },
                    }
                });
            }
        }
    });
}

function fetchDataSiblingRank(region_id, province_id, cm_id) {
    var ctx = document.getElementById('vaccineChartSiblings').getContext('2d');
    // Check if a chart instance already exists and destroy it
    if (BarChartSiblingRank !== null) {
        BarChartSiblingRank.destroy();
    }

    $.ajax({
        url: '/Dashboard/BarchartMRSIAPersonRankData',
        method: 'GET',
        data: { region_id: region_id, province_id: province_id, cm_id: cm_id },
        success: function (result) {
            if (result.isSuccess) {
                var data = result.data;

                // Extract the relevant data for the chart
                var labels = [];
                var siblingdescriptions = [];
                var OPVData = [];
                var MRVData = [];

                for (var i = 0; i < data.length; i++) {
                    if (data[i].SiblingRank !== "TOTAL") {
                        // Store the original description
                        siblingdescriptions.push(data[i].SiblingRank);

                        // Create aliases for long descriptions
                        var alias = createAliasForSiblinRank(data[i].SiblingRank);
                        labels.push(alias);

                        OPVData.push(data[i].OPV);
                        MRVData.push(data[i].MRV);
                    }
                }

                BarChartSiblingRank = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: labels,
                        datasets: [
                            {
                                label: 'OPV',
                                data: OPVData,
                                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                                borderColor: 'rgba(75, 192, 192, 1)',
                                borderWidth: 1,
                            },
                            {
                                label: 'MRV',
                                data: MRVData,
                                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                                borderColor: 'rgba(255, 99, 132, 1)',
                                borderWidth: 1,
                            }
                        ]
                    },
                    options: {
                        scales: {
                            x: {
                                title: {
                                    display: true,
                                    text: 'Sibling Rank'
                                }
                            },
                            y: {
                                beginAtZero: true,
                                title: {
                                    display: true,
                                    text: 'Vaccinations'
                                }
                            }
                        },
                        plugins: {
                            tooltip: {
                                callbacks: {
                                    title: function (context) {
                                        var index = context[0].dataIndex;
                                        return siblingdescriptions[index];
                                    },
                                },
                            },
                        },
                    }
                });
            }
        }
    });
}

function createAliasForReligion(description) {
    if (description.length > 5) {
        return description.substring(0, 5) + '...';
    } else {
        return description;
    }
}

function createAliasForIncomeDescription(IncomeDescription) {
    if (IncomeDescription.length > 10) {
        return IncomeDescription.substring(0, 10) + '...';
    } else {
        return IncomeDescription;
    }
}

function createAliasForOccupationDescription(OccupationDescription) {

    if (OccupationDescription.length > 10) {
        return OccupationDescription.substring(0, 10) + '...';
    } else {
        return OccupationDescription;
    }
}

function createAliasForSiblinRank(SiblingRank) {

    if (SiblingRank.length > 10) {
        return SiblingRank.substring(0, 10) + '...';
    } else {
        return SiblingRank;
    }
}