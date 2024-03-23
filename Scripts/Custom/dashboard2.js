const specificColors = ['#89CFF0', '#0096FF', '#6495ED', '#96DED1', '#40E0D0', '#50C878', '#2AAA8A', '#40B5AD', '#CCCCFF', '#00FFFF', '#9FE2BF', '#7FFFD4', '#B6D0E2']; // Add more colors as needed '#FFAC1C', '#FFBF00', '#FF7F50', '#F88379', '#FAC898'
var vaccine_idlist = [];
let SecondBoosterChart = null;// Store the Chart instance
let FirstDoseChart = null;
let FullyVaccinatedChart = null;
let FirstBoosterChart = null;
let ExpandedVaccinatedChart = null;
let ChartID = 0;
let province = 0;
let citymun = 0;
$(document).ready(function () {
    let chartData = [];

    /*Data Table*/
    PopulateVaccineTable(0, 0, 0);

    //Line and Bar chart
    loadSecondBoosterChart(0, 0, 0);
    loadFirstDoseChart(0, 0, 0);
    loadFullyVaccinatedChart(0, 0, 0);
    loadFirstBoosterChart(0, 0, 0);

    /*loadBoosterPieChart(vaccine_ids, province_id, citymun_id);*/
    $('#start-date').datepicker({
        autoclose: true,
        endDate: new Date(),
        defaultViewDate: new Date(),
        format: 'yyyy-mm-dd', // Add a date format that matches your datepicker's format
        onSelect: function (selectedDate) {
            // Set the minimum date of the end-date datepicker to the selected date of start-date
            $('#end-date').datepicker('setStartDate', selectedDate);

            // Disable dates before the selected start date in the end-date datepicker
            $('#end-date').datepicker('setDatesDisabled', function (date) {
                return date < new Date(selectedDate);
            });
        }
    });

    $('#end-date').datepicker({
        autoclose: true,
        endDate: new Date().end,
        defaultViewDate: new Date(),
        format: 'yyyy-mm-dd' // Add a date format that matches your datepicker's format
    });
});

$("#filter-button").click(function () {
    province = $("#province_id").val();
    citymun = $("#cm_id").val();
    barangay_id = $("3barangay_id").val();
    toastr.success("Please wait..");

    //Line and Bar chart
    loadSecondBoosterChart(province, citymun, barangay_id);
    loadFirstDoseChart(province, citymun, barangay_id);
    loadFullyVaccinatedChart(province, citymun, barangay_id);
    loadFirstBoosterChart(province, citymun, barangay_id);

    /*Data Table*/
    PopulateVaccineTable(province, citymun, barangay_id);
});
$('#btn-expand-first-dose').on('click', function (e) {
    e.preventDefault;
    $('#expandedDoseTpe').text("First Dose");
    $('#per-month').prop('checked', true);
    ChartID = 1;
    province = $("#province_id").val();
    citymun = $("#cm_id").val();

    loadExpandedVaccinationChart(ChartID, province, citymun, barangay_id, "month");
})
$('#btn-expand-fully-vaccinated').on('click', function (e) {
    e.preventDefault;
    $('#expandedDoseTpe').text("Fully Vaccinated");
    $('#per-month').prop('checked', true);
    ChartID = 2;
    province = $("#province_id").val();
    citymun = $("#cm_id").val();

    loadExpandedVaccinationChart(ChartID, province, citymun, barangay_id, "month");
})
$('#btn-expand-first-booster').on('click', function (e) {
    e.preventDefault;
    $('#expandedDoseTpe').text("First Booster");
    $('#per-month').prop('checked', true);
    ChartID = 3;
    province = $("#province_id").val();
    citymun = $("#cm_id").val();

    loadExpandedVaccinationChart(ChartID, province, citymun, barangay_id, "month");
})
$('#btn-expand-second-booster').on('click', function (e) {
    e.preventDefault;
    $('#expandedDoseTpe').text("Second Booster");
    $('#per-month').prop('checked', true);
    ChartID = 4;
    province = $("#province_id").val();
    citymun = $("#cm_id").val();

    loadExpandedVaccinationChart(ChartID, province, citymun, barangay_id, "month");
})
$('#per-day').on('click', function (e) {
    loadExpandedVaccinationChart(ChartID, province, citymun, barangay_id, "day");
});
$('#per-month').on('click', function (e) {
    loadExpandedVaccinationChart(ChartID, province, citymun, barangay_id, "month");
});
$('#per-year').on('click', function (e) {
    loadExpandedVaccinationChart(ChartID, province, citymun, barangay_id, "year");
});

function loadFirstDoseChart(province_id, citymun_id, barangay_id) {
    if (FirstDoseChart !== null) {
        FirstDoseChart.destroy(); // Destroy the previous chart instance
    }

    const formData = new FormData();
    formData.append("dose_id", 1);
    formData.append("province_id", province_id);
    formData.append("citymun_id", citymun_id);
    formData.append("barangay_id", barangay_id);
    formData.append("groupBy", "month");

    fetch("/Dashboard/GetBarChartData", {
        method: "POST",
        body: formData
    }).then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        return response.json();
    })
        .then(data => {
            if (data.isSuccess) {

                const ctx = document.getElementById('vaccineChartFirstDose');
                const listOfDates = data.chartDatas.map(x => (moment(x.Date).format('MMMM YYYY')));
                const totalEncoded = data.chartDatas.map(x => x.TotalEncoded);
                const AccumulativeTotal = data.chartDatas.map(x => x.AccumulativeTotal);
                FirstDoseChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: listOfDates,
                        datasets: [{
                            label: 'Cumulative Total',
                            data: AccumulativeTotal,
                            borderWidth: 1
                        },
                        {
                            label: 'Vaccinated this day',
                            data: totalEncoded,
                            borderWidth: 1,
                            type: `bar`,
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
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

function loadFullyVaccinatedChart(province_id, citymun_id, barangay_id) {
    if (FullyVaccinatedChart !== null) {
        FullyVaccinatedChart.destroy(); // Destroy the previous chart instance
    }
    const formData = new FormData();
    formData.append("dose_id", 2);
    formData.append("province_id", province_id);
    formData.append("citymun_id", citymun_id);
    formData.append("barangay_id", barangay_id);
    formData.append("groupBy", "month");

    fetch("/Dashboard/GetBarChartData", {
        method: "POST",
        body: formData
    }).then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        return response.json();
    })
        .then(data => {
            if (data.isSuccess) {
                const ctx = document.getElementById('vaccineChartFullyVaccinated');
                const listOfDates = data.chartDatas.map(x => (moment(x.Date).format('MMMM YYYY')));
                const totalEncoded = data.chartDatas.map(x => x.TotalEncoded);
                const AccumulativeTotal = data.chartDatas.map(x => x.AccumulativeTotal);
                FullyVaccinatedChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: listOfDates,
                        datasets: [{
                            label: 'Cumulative Total',
                            data: AccumulativeTotal,
                            borderWidth: 1
                        },
                        {
                            label: 'Vaccinated this day',
                            data: totalEncoded,
                            borderWidth: 1,
                            type: `bar`,
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
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

function loadFirstBoosterChart(province_id, citymun_id, barangay_id) {
    if (FirstBoosterChart !== null) {
        FirstBoosterChart.destroy(); // Destroy the previous chart instance
    }
    const formData = new FormData();
    formData.append("dose_id", 3);
    formData.append("province_id", province_id);
    formData.append("citymun_id", citymun_id);
    formData.append("barangay_id", barangay_id);
    formData.append("groupBy", "month");

    fetch("/Dashboard/GetBarChartData", {
        method: "POST",
        body: formData
    }).then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        return response.json();
    })
        .then(data => {
            if (data.isSuccess) {
                const ctx = document.getElementById('vaccineChartFirstBooster');
                const listOfDates = data.chartDatas.map(x => (moment(x.Date).format('MMMM YYYY')));
                const totalEncoded = data.chartDatas.map(x => x.TotalEncoded);
                const AccumulativeTotal = data.chartDatas.map(x => x.AccumulativeTotal);
                FirstBoosterChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: listOfDates,
                        datasets: [{
                            label: 'Cumulative Total',
                            data: AccumulativeTotal,
                            borderWidth: 1
                        },
                        {
                            label: 'Vaccinated this day',
                            data: totalEncoded,
                            borderWidth: 1,
                            type: `bar`,
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
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

function loadSecondBoosterChart(province_id, citymun_id, barangay_id) {
    $('#dashboard1total').text("loading");
    if (SecondBoosterChart !== null) {
        SecondBoosterChart.destroy(); // Destroy the previous chart instance
    }
    const formData = new FormData();
    formData.append("dose_id", 4);
    formData.append("province_id", province_id);
    formData.append("citymun_id", citymun_id);
    formData.append("barangay_id", barangay_id);
    formData.append("groupBy", "month");

    fetch("/Dashboard/GetBarChartData", {
        method: "POST",
        body: formData
    }).then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        return response.json(); s
    })
        .then(data => {
            if (data.isSuccess) {
                const ctx = document.getElementById('vaccineChartSecondBooster');
                const listOfDates = data.chartDatas.map(x => (moment(x.Date).format('MMMM YYYY')));
                const totalEncoded = data.chartDatas.map(x => x.TotalEncoded);
                const AccumulativeTotal = data.chartDatas.map(x => x.AccumulativeTotal);

                $('#dashboard1total').text(data.total != null && data.total !== '' ? parseInt(data.total).toLocaleString('en', currencyOptions).split('.')[0] : '');
                if (data.total == null)
                    $('#dashboard1total').text("No data");

                SecondBoosterChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: listOfDates,
                        datasets: [{
                            label: 'Cumulative Total',
                            data: AccumulativeTotal,
                            borderWidth: 1
                        },
                        {
                            label: 'Vaccinated this day',
                            data: totalEncoded,
                            borderWidth: 1,
                            type: `bar`,
                        }]
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
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

function loadExpandedVaccinationChart(dose_id, province_id, citymun_id, barangay_id, group_by) {
    if (ExpandedVaccinatedChart !== null) {
        ExpandedVaccinatedChart.destroy(); // Destroy the previous chart instance
    }

    const formData = new FormData();
    formData.append("dose_id", dose_id);
    formData.append("province_id", province_id);
    formData.append("citymun_id", citymun_id);
    formData.append("barangay_id", barangay_id);
    formData.append("groupBy", group_by);

    fetch("/Dashboard/GetBarChartData", {
        method: "POST",
        body: formData
    }).then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        return response.json();
    })
        .then(data => {
            if (data.isSuccess) {
                const ctx = document.getElementById('expandedVaccineChart');
                const listOfDates = data.chartDatas.map(x => (moment(x.Date).format(group_by === "day" ? 'MMMM DD, YYYY' : (group_by === "year" ? 'YYYY' : 'MMMM YYYY'))));
                const totalEncoded = data.chartDatas.map(x => x.TotalEncoded);
                const AccumulativeTotal = data.chartDatas.map(x => x.AccumulativeTotal);

                ExpandedVaccinatedChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: listOfDates,
                        datasets: [{
                            label: 'Cumulative Total',
                            data: AccumulativeTotal,
                            borderWidth: 1
                        },
                        {
                            label: `Vaccinated this ${group_by}`,
                            data: totalEncoded,
                            borderWidth: 1,
                            type: 'bar',
                        }]
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
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

function PopulateVaccineTable(province_id, citymun_id, barangay_id) {

    tblVaccines = $('#tbl-vaccines').DataTable({
        "destroy": true,
        "ajax": {
            "type:": "POST",
            "url": dir + "/Dashboard/VaccinesDataTable",
            "data": {
                vaccine_ids: vaccine_idlist,
                province_id: province_id,
                citymun_id: citymun_id,
                barangay_id: barangay_id
            },
            "datatype": "json"
        },
        "responsive": true,
        "lengthChange": true,
        "autoWidth": false,
        "searching": false,
        "paging": false,
        "info": false,
        "entries": false,
        "ordering": false,
        "select": { style: 'single' },
        "columnDefs": [],
        "columns": [

            { "name": "dosename", "data": "dosename", "width": "25%" },
            { "name": "fivetoelevenyo", "data": "fivetoelevenyo", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "twelvetoseventeenyo", "data": "twelvetoseventeenyo", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "eighteentofiftynineyo", "data": "eighteentofiftynineyo", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "sixtyaboveyo", "data": "sixtyaboveyo", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "total", "data": "total", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } }
        ],
    });
}

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

//$('#modal-school #province_id').on('change', PopulateCityMunicipality);

//function PopulateCityMunicipality() {
//    var provinceId = $("#modal-school #province_id").val();
//    var cmb = $('#modal-school #cm_id');
//    cmb.empty();
//    cmb.append($('<option></option>').attr('value', "").text("- Select City / Municipality -"));

//    $.ajax({
//        type: "POST",
//        url: dir + "/FileMaintenance/CityMunicipalitiesDataTable",
//        data: { provinceId: provinceId },
//        dataType: "json",
//        async: false,
//        success: function (data) {
//            if (data.isSuccess) {
//                $.each(data.data, function (i, rec) {
//                    cmb.append($('<option></option>').attr('value', rec.cm_id).text(rec.cm_name.trim()));
//                });
//            } else {
//                JsonResultError(data.isRedirect, data.returnUrl, '');
//            }
//        }
//    });
//}

//function loadFirstDoseChart() {
//    fetch("/Dashboard/GetDoseData?dose_id=1")
//        .then(response => {
//            if (!response.ok) {
//                throw new Error(`HTTP error! Status: ${response.status}`);
//            }
//            return response.json();
//        })
//        .then(data => {
//            const ctx = document.getElementById('vaccineChartFirstDose');
//            const listOfDates = data.map(x => (moment(x.Date).format('MMMM YYYY')));
//            const totalEncoded = data.map(x => x.TotalEncoded);
//            const AccumulativeTotal = data.map(x => x.AccumulativeTotal);
//            new Chart(ctx, {
//                type: 'line',
//                data: {
//                    labels: listOfDates,
//                    datasets: [{
//                        label: 'Cumulative Total',
//                        data: AccumulativeTotal,
//                        borderWidth: 1
//                    },
//                    {
//                        label: 'Vaccinated this day',
//                        data: totalEncoded,
//                        borderWidth: 5,
//                        type: `bar`,
//                    }
//                    ]
//                },
//                options: {
//                    //plugins: {
//                    //    title: {
//                    //        display: true,
//                    //        text: 'First Dose (Vaccination Data)', // Your title/header text here
//                    //        font: {
//                    //            size: 16
//                    //        }
//                    //    }
//                    //},
//                    scales: {
//                        y: {
//                            beginAtZero: true
//                        }
//                    }
//                }
//            });
//        })
//        .catch(error => {
//            console.error('Fetch error:', error);
//        });
//}


//function renderFullyVaccinatedDoseChart(data) {
//    const ctx = document.getElementById('vaccineChartFullyVaccinated');
//    const listOfDates = data.map(x => (moment(x.Date).format('MMMM YYYY')));
//    const totalEncoded = data.map(x => x.TotalEncoded);
//    const AccumulativeTotal = data.map(x => x.AccumulativeTotal);
//    new Chart(ctx, {
//        type: 'line',
//        data: {
//            labels: listOfDates,
//            datasets: [{
//                label: 'Cumulative Total',
//                data: AccumulativeTotal,
//                borderWidth: 1
//            },
//            {
//                label: 'Vaccinated this day',
//                data: totalEncoded,
//                borderWidth: 5,
//                type: `bar`,
//            }
//            ]
//        },
//        options: {
//            //plugins: {
//            //    title: {
//            //        display: true,
//            //        text: 'First Dose (Vaccination Data)', // Your title/header text here
//            //        font: {
//            //            size: 16
//            //        }
//            //    }
//            //},
//            scales: {
//                y: {
//                    beginAtZero: true
//                }
//            }
//        }
//    });
//}