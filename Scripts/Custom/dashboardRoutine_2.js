const specificColors = ['#89CFF0', '#0096FF', '#6495ED', '#96DED1', '#40E0D0', '#50C878', '#2AAA8A', '#40B5AD', '#CCCCFF', '#00FFFF', '#9FE2BF', '#7FFFD4', '#B6D0E2']; // Add more colors as needed '#FFAC1C', '#FFBF00', '#FF7F50', '#F88379', '#FAC898'
let region_id = 1;
let province = 0;
let citymun = 0;
var BarChartProvince = null;
var BarChartCityMun = null;

$(document).ready(function () {
    /*Data Table*/
   
    fetchDataProvince(region_id);
});

$("#filter-button").click(function () {
    var region = 1;
    var province = $("#province_id").val();
    var citymun = $("#cm_id").val();

    console.log("province:", province);
    console.log("citymun:", citymun);


    if (province && (citymun == 0)) {
        toastr.success("loaded succesfully..");
        fetchDataCityMun(province);
    } else if (province && citymun) {
        fetchDataBarangay(citymun);
    } else {
        toastr.success("loaded succesfully..");
        fetchDataProvince(region);
    }
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
    var ctx = document.getElementById('vaccineChartRoutine').getContext('2d');
    //var BarChartProvince = null; // Declare BarChartProvince variable

    // Check if a chart instance already exists
    if (BarChartProvince !== null) {
        BarChartProvince.destroy(); // Destroy the previous chart instance
    }

    $.ajax({
        url: dir + '/Dashboard/LineChartRoutineProvinceData',
        method: 'GET',
        data: { region_id: region_id },
        success: function (result) {
            if (result.isSuccess) {
                var data = result.data;

                // Extract the relevant data for the chart
                var labels = [];
                var NoVData = []; // Data for both bar and line charts

                for (var i = 0; i < data.length; i++) {
                    if (data[i].Province !== "TOTAL") {
                        labels.push(data[i].VM); // Use VM (date) for labels
                        NoVData.push(data[i].NoV); // Use NoV for both bar and line charts
                    }
                }

                BarChartProvince = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: labels,
                        datasets: [
                            {
                                label: 'Number of Vaccinations',
                                data: NoVData,
                                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                                borderColor: 'rgba(75, 192, 192, 1)',
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

                // Add line chart to the existing bar chart
                BarChartProvince.data.datasets.push({
                    label: 'Number of Vaccinations (Line)',
                    data: NoVData,
                    type: 'line',
                    borderColor: 'rgba(255, 206, 86, 1)',
                    borderWidth: 2,
                    fill: false
                });

                BarChartProvince.update(); // Update the chart to reflect the changes
            }
        }
    });
}


function fetchDataCityMun(province_id) {
    var ctx = document.getElementById('vaccineChartRoutine').getContext('2d');
    //var BarChartProvince = null; // Declare BarChartProvince variable

    // Check if a chart instance already exists
    if (BarChartProvince !== null) {
        BarChartProvince.destroy(); // Destroy the previous chart instance
    }

    $.ajax({
        url: dir + '/Dashboard/LineChartRoutineCityMunData',
        method: 'GET',
        data: { province_id: province_id },
        success: function (result) {
            if (result.isSuccess) {
                var data = result.data;

                // Extract the relevant data for the chart
                var labels = [];
                var NoVData = []; // Data for both bar and line charts

                for (var i = 0; i < data.length; i++) {
                    if (data[i].Province !== "TOTAL") {
                        labels.push(data[i].VM); // Use VM (date) for labels
                        NoVData.push(data[i].NoV); // Use NoV for both bar and line charts
                    }
                }

                BarChartProvince = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: labels,
                        datasets: [
                            {
                                label: 'Number of Vaccinations',
                                data: NoVData,
                                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                                borderColor: 'rgba(75, 192, 192, 1)',
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

                // Add line chart to the existing bar chart
                BarChartProvince.data.datasets.push({
                    label: 'Number of Vaccinations (Line)',
                    data: NoVData,
                    type: 'line',
                    borderColor: 'rgba(255, 206, 86, 1)',
                    borderWidth: 2,
                    fill: false
                });

                BarChartProvince.update(); // Update the chart to reflect the changes
            }
        }
    });
}

function fetchDataBarangay(citymun_id) {
    var ctx = document.getElementById('vaccineChartRoutine').getContext('2d');
    //var BarChartProvince = null; // Declare BarChartProvince variable

    // Check if a chart instance already exists
    if (BarChartProvince !== null) {
        BarChartProvince.destroy(); // Destroy the previous chart instance
    }

    $.ajax({
        url: dir + '/Dashboard/LineChartRoutineBarangayData',
        method: 'GET',
        data: { citymun_id: citymun_id },
        success: function (result) {
            if (result.isSuccess) {
                var data = result.data;

                // Extract the relevant data for the chart
                var labels = [];
                var NoVData = []; // Data for both bar and line charts

                for (var i = 0; i < data.length; i++) {
                    if (data[i].Province !== "TOTAL") {
                        labels.push(data[i].VM); // Use VM (date) for labels
                        NoVData.push(data[i].NoV); // Use NoV for both bar and line charts
                    }
                }

                BarChartProvince = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: labels,
                        datasets: [
                            {
                                label: 'Number of Vaccinations',
                                data: NoVData,
                                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                                borderColor: 'rgba(75, 192, 192, 1)',
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

                // Add line chart to the existing bar chart
                BarChartProvince.data.datasets.push({
                    label: 'Number of Vaccinations (Line)',
                    data: NoVData,
                    type: 'line',
                    borderColor: 'rgba(255, 206, 86, 1)',
                    borderWidth: 2,
                    fill: false
                });

                BarChartProvince.update(); // Update the chart to reflect the changes
            }
        }
    });
}














//function fetchDataProvince(region_id) {
//    var ctx = document.getElementById('vaccineChartRoutine').getContext('2d');

//    // Check if a chart instance already exists
//    if (BarChartProvince !== null) {
//        BarChartProvince.destroy(); // Destroy the previous chart instance
//    }

//    $.ajax({
//        url: dir + '/Dashboard/LineChartRoutineProvinceData',
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

//function createAliasForSiblinRank(SiblingRank) {

//    if (SiblingRank.length > 10) {
//        return SiblingRank.substring(0, 10) + '...';
//    } else {
//        return SiblingRank;
//    }
//}