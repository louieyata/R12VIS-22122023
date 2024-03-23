const specificColors = ['#89CFF0', '#0096FF', '#6495ED', '#96DED1', '#40E0D0', '#50C878', '#2AAA8A', '#40B5AD', '#CCCCFF', '#00FFFF', '#9FE2BF', '#7FFFD4', '#B6D0E2']; // Add more colors as needed '#FFAC1C', '#FFBF00', '#FF7F50', '#F88379', '#FAC898'
let region_id = 1;
let province = 0;
let citymun = 0;
let schedule_is_active = 1;
let vaccine_type_id = 2;

$(document).ready(function () {
    PopulateROUTINEDataTableProvince(region_id);
    PopulateROUTINEDataTableSchedule(schedule_is_active);
});

$("#filter-button").click(function () {
    var region = 1;
    var province = $("#province_id").val();
    var citymun = $("#cm_id").val();

    console.log("province:", province);
    console.log("citymun:", citymun);


    if (province && (citymun == 0)) {
        toastr.success("loaded succesfully..");
        PopulateROUTINEDataTableCityMun(province);
    } else if (province && citymun) {
        PopulateROUTINEDataTableBarangay(citymun);
    } else {
        toastr.success("loaded succesfully..");
        PopulateROUTINEDataTableProvince(region_id);
    }
});

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

            var chartData = {
                name: 'Total', // You can customize the name
                data: data

                    .filter(function (row) {
                        return row.Province !== 'TOTAL'; // Exclude rows where Province is 'TOTAL'
                    })


                    .map(function (row) {
                    return row.TOTAL; // Replace 'TOTAL' with the actual property name for the total
                })
            };

            var chart = new ApexCharts(document.querySelector("#columnChart"), {
                series: [chartData], // Update this line with the extracted chart data
                chart: {
                    type: 'bar',
                    height: 340
                },
                plotOptions: {
                    bar: {
                        horizontal: false,
                        columnWidth: '55%',
                        endingShape: 'rounded'
                    },
                },
                dataLabels: {
                    enabled: false
                },
                stroke: {
                    show: true,
                    width: 2,
                    colors: ['transparent']
                },
                xaxis: {
                    categories: data.map(function (row) {
                        return row.Province;
                    }).filter(function (category) {
                        return category !== 'TOTAL';
                    }),
                },



                yaxis: {
                    title: {
                        text: 'Vaccinated'
                    }
                },
                fill: {
                    opacity: 1
                },
                tooltip: {
                    y: {
                        formatter: function (val) {
                            return val
                        }
                    }
                }
            });

            chart.render();
        


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
        });
    });
});

function PopulateROUTINEDataTableSchedule(schedule_is_active) {
    tblMRSIAVaccineRecords = $('#tbl-vpd-vaccinations-records').DataTable({
        "destroy": true,
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
        ],
        "ajax": {
            "type": "POST",
            "url": dir + "/Dashboard/RoutineSchedules",
            "data": { schedule_is_active: schedule_is_active },
            "datatype": "json",
            success: function (responseData) {
                var data = responseData.data; // DataTable data
                var totalData = responseData.total; // Total data

                tblMRSIAVaccineRecords.clear().rows.add(data).draw();

                // Update the "dashboard2total" element with the total values
                $("#dashboardroutine2total").html(totalData.No_);
                if (totalData.No_ == 0)
                    $('#dashboardroutine2total').text("No data");
            }
        },
    });
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

//<!--End Column Chart-- >

//$(document).ready(function () {
//    PopulateROUTINEDataTableProvince(region_id);
//    PopulateROUTINEDataTableSchedule(schedule_is_active);
//});



//$(document).ready(function () {
//    new ApexCharts(document.querySelector("#columnChart"), {
//        series: [
//            //{
//            //name: 'Net Profit',
//            //data: [44, 55, 57, 56, 61, 58, 63, 60, 66]
//            //},
//            //{
//            //name: 'Revenue',
//            //data: [76, 85, 101, 98, 87, 105, 91, 114, 94]
//            //},
//            {
//            name: 'Free Cash Flow',
//            data: [35, 41, 36, 26, 45, 48, 52, 53, 41]
//        }],
//        chart: {
//            type: 'bar',
//            height: 340
//        },
//        plotOptions: {
//            bar: {
//                horizontal: false,
//                columnWidth: '55%',
//                endingShape: 'rounded'
//            },
//        },
//        dataLabels: {
//            enabled: false
//        },
//        stroke: {
//            show: true,
//            width: 2,
//            colors: ['transparent']
//        },
//        xaxis: {
//            categories: ['Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct'],
//        },
//        yaxis: {
//            title: {
//                text: '$ (thousands)'
//            }
//        },
//        fill: {
//            opacity: 1
//        },
//        tooltip: {
//            y: {
//                formatter: function (val) {
//                    return "$ " + val + " thousands"
//                }
//            }
//        }
//    }).render();
//});

//<!--End Column Chart-- >                                   


























//function fetchDataProvince(region_id) {
//    var ctx = document.getElementById('VaccinatedChart').getContext('2d');

//    // Check if a chart instance already exists
//    if (BarChartProvince !== null) {
//        BarChartProvince.destroy(); // Destroy the previous chart instance
//    }

//    $.ajax({
//        url: '/Dashboard/MRSIADataTable',
//        method: 'GET',
//        data: { region_id: region_id },
//        success: function (result) {
//            if (result.isSuccess) {
//                var data = result.data;

//                // Extract the relevant data for the chart
//                var labels = [];
//                var OPVData = [];
//                var MRVData = [];
//                var TARGET1Data = [];
//                var TARGET2Data = [];

//                for (var i = 0; i < data.length; i++) {
//                    if (data[i].Province !== "TOTAL") {
//                        labels.push(data[i].Province);
//                        OPVData.push(data[i].OPV);
//                        MRVData.push(data[i].MRV);
//                        TARGET1Data.push(data[i].TARGET1);
//                        TARGET2Data.push(data[i].TARGET2);
//                    }
//                }

//                BarChartProvince = new Chart(ctx, {
//                    type: 'bar',
//                    data: {
//                        labels: labels,
//                        datasets: [
//                            {
//                                label: 'OPV',
//                                data: OPVData,
//                                backgroundColor: 'rgba(75, 192, 192, 0.2)',
//                                borderColor: 'rgba(75, 192, 192, 1)',
//                                borderWidth: 1,
//                            },
//                            {
//                                label: 'MRV',
//                                data: MRVData,
//                                backgroundColor: 'rgba(255, 99, 132, 0.2)',
//                                borderColor: 'rgba(255, 99, 132, 1)',
//                                borderWidth: 1,
//                            },
//                            {
//                                label: 'OPV TARGETS',
//                                data: TARGET1Data,
//                                backgroundColor: 'rgba(54, 162, 235, 0.2)',
//                                borderColor: 'rgba(54, 162, 235, 1)',
//                                borderWidth: 1,
//                            },
//                            {
//                                label: 'MRV TARGETS',
//                                data: TARGET2Data,
//                                backgroundColor: 'rgba(255, 205, 86, 0.2)',
//                                borderColor: 'rgba(255, 205, 86, 1)',
//                                borderWidth: 1,
//                            }
//                        ]

//                    },
//                    options: {
//                        scales: {
//                            y: {
//                                beginAtZero: true
//                            }
//                        }
//                    }
//                });
//            }
//        }
//    });
//}

//function fetchDataCityMun(province_id) {
//    var ctx = document.getElementById('VaccinatedChart').getContext('2d');
//    // Check if a chart instance already exists and destroy it
//    if (BarChartProvince !== null) {
//        BarChartProvince.destroy();
//    }

//    $.ajax({
//        url: '/Dashboard/MRSIADataTableCityMun',
//        method: 'GET',
//        data: { province_id: province_id },
//        success: function (result) {
//            if (result.isSuccess) {
//                var data = result.data;

//                // Extract the relevant data for the chart
//                var labels = [];
//                var OPVData = [];
//                var MRVData = [];
//                var TARGET1Data = [];
//                var TARGET2Data = [];

//                for (var i = 0; i < data.length; i++) {
//                    if (data[i].CityMunName !== "TOTAL") {
//                        labels.push(data[i].CityMunName);
//                        OPVData.push(data[i].OPV);
//                        MRVData.push(data[i].MRV);
//                        TARGET1Data.push(data[i].TARGET1);
//                        TARGET2Data.push(data[i].TARGET2);
//                    }
//                }
//                BarChartProvince = new Chart(ctx, {
//                    type: 'bar',
//                    data: {
//                        labels: labels,
//                        datasets: [
//                            {
//                                label: 'OPV',
//                                data: OPVData,
//                                backgroundColor: 'rgba(75, 192, 192, 0.2)',
//                                borderColor: 'rgba(75, 192, 192, 1)',
//                                borderWidth: 1,
//                            },
//                            {
//                                label: 'MRV',
//                                data: MRVData,
//                                backgroundColor: 'rgba(255, 99, 132, 0.2)',
//                                borderColor: 'rgba(255, 99, 132, 1)',
//                                borderWidth: 1,
//                            },
//                            {
//                                label: 'OPV TARGETS',
//                                data: TARGET1Data,
//                                backgroundColor: 'rgba(54, 162, 235, 0.2)',
//                                borderColor: 'rgba(54, 162, 235, 1)',
//                                borderWidth: 1,
//                            },
//                            {
//                                label: 'MRV TARGETS',
//                                data: TARGET2Data,
//                                backgroundColor: 'rgba(255, 205, 86, 0.2)',
//                                borderColor: 'rgba(255, 205, 86, 1)',
//                                borderWidth: 1,
//                            }
//                        ]
//                    },
//                    options: {
//                        scales: {
//                            y: {
//                                beginAtZero: true
//                            }
//                        }
//                    }
//                });
//            }
//        }
//    });
//}

//function fetchDataBarangay(cm_id) {
//    var ctx = document.getElementById('VaccinatedChart').getContext('2d');
//    // Check if a chart instance already exists and destroy it
//    if (BarChartProvince !== null) {
//        BarChartProvince.destroy();
//    }

//    $.ajax({
//        url: '/Dashboard/MRSIADataTableBarangay',
//        method: 'GET',
//        data: { cm_id: cm_id },
//        success: function (result) {
//            if (result.isSuccess) {
//                var data = result.data;

//                // Extract the relevant data for the chart
//                var labels = [];
//                var OPVData = [];
//                var MRVData = [];
//                var TARGET1Data = [];
//                var TARGET2Data = [];

//                for (var i = 0; i < data.length; i++) {
//                    if (data[i].BarangayName !== "TOTAL") {
//                        labels.push(data[i].BarangayName);
//                        OPVData.push(data[i].OPV);
//                        MRVData.push(data[i].MRV);
//                        TARGET1Data.push(data[i].TARGET1);
//                        TARGET2Data.push(data[i].TARGET2);
//                    }
//                }
//                BarChartProvince = new Chart(ctx, {
//                    type: 'bar',
//                    data: {
//                        labels: labels,
//                        datasets: [
//                            {
//                                label: 'OPV',
//                                data: OPVData,
//                                backgroundColor: 'rgba(75, 192, 192, 0.2)',
//                                borderColor: 'rgba(75, 192, 192, 1)',
//                                borderWidth: 1,
//                            },
//                            {
//                                label: 'MRV',
//                                data: MRVData,
//                                backgroundColor: 'rgba(255, 99, 132, 0.2)',
//                                borderColor: 'rgba(255, 99, 132, 1)',
//                                borderWidth: 1,
//                            },
//                            {
//                                label: 'OPV TARGETS',
//                                data: TARGET1Data,
//                                backgroundColor: 'rgba(54, 162, 235, 0.2)',
//                                borderColor: 'rgba(54, 162, 235, 1)',
//                                borderWidth: 1,
//                            },
//                            {
//                                label: 'MRV TARGETS',
//                                data: TARGET2Data,
//                                backgroundColor: 'rgba(255, 205, 86, 0.2)',
//                                borderColor: 'rgba(255, 205, 86, 1)',
//                                borderWidth: 1,
//                            }
//                        ]
//                    },
//                    options: {
//                        scales: {
//                            y: {
//                                beginAtZero: true
//                            }
//                        }
//                    }
//                });
//            }
//        }
//    });
//}