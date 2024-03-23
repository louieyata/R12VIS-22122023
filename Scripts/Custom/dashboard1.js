const specificColors = ['#89CFF0', '#0096FF', '#6495ED', '#96DED1', '#40E0D0', '#50C878', '#2AAA8A', '#40B5AD', '#CCCCFF', '#00FFFF', '#9FE2BF', '#7FFFD4', '#B6D0E2']; // Add more colors as needed '#FFAC1C', '#FFBF00', '#FF7F50', '#F88379', '#FAC898'
let BoosterPieChart = null; // Store the Chart instance
var vaccine_idlist = [];
let SecondBoosterPieChart = null;
let BoosterSeniorPieChart = null;
let SecondBoosterSeniorPieChart = null;

$(document).ready(function () {
    //vaccine_idlist = [1, 2, 3, 4, 5, 6, 7, 8]; /* , 9, 10, 11 - vaccine ids sputnik and bivalents */
    /*Data Table*/
    PopulateVaccineTable(vaccine_idlist, 0, 0);

    /*Pie chart*/
    loadSecondBoosterSeniorPieChart(vaccine_idlist, 0, 0, 0);
    loadBoosterPieChart(vaccine_idlist, 0, 0, 0);
    loadSecondBoosterPieChart(vaccine_idlist, 0, 0, 0);
    loadBoosterSeniorPieChart(vaccine_idlist, 0, 0, 0);
    console.log("vaccine_ids " + vaccine_idlist);
    // Get all the checkboxes with the name "vaccine_brands"
    const checkboxes = document.querySelectorAll('input[name="vaccine_brands"]');
    // Add a click event listener to each checkbox
    checkboxes.forEach(checkbox => {
        checkbox.addEventListener('click', function () {
            var province = $("#province_id").val();
            var citymun = $("#cm_id").val();
            var barangay_id = $("barangay_id").val();
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
            loadSecondBoosterSeniorPieChart(vaccine_idlist, province, citymun, barangay_id);
            PopulateVaccineTable(vaccine_idlist, province, citymun, barangay_id);
        });
    });
});

$("#filter-button").click(function () {
    var province = $("#province_id").val();
    var citymun = $("#cm_id").val();
    var barangay_id = $("#barangay_id").val();
    toastr.success("Please wait..");

    /*Pie chart*/
    loadSecondBoosterSeniorPieChart(vaccine_idlist, province, citymun, barangay_id);
    loadBoosterPieChart(vaccine_idlist, province, citymun, barangay_id);
    loadSecondBoosterPieChart(vaccine_idlist, province, citymun, barangay_id);
    loadBoosterSeniorPieChart(vaccine_idlist, province, citymun, barangay_id);
    //loadtotal(vaccine_idlist, province, citymun);

    /*Data Table*/
    PopulateVaccineTable(vaccine_idlist, province, citymun, barangay_id);
});

function loadBoosterPieChart(vaccine_ids, province_id, citymun_id, barangay_id) {
    if (BoosterPieChart !== null) {
        BoosterPieChart.destroy(); // Destroy the previous chart instance
    }
    var priority_group_idlist = [1, 2, 3, 4, 6, 7, 8, 11, 12, 13];

    fetch("/Dashboard/GetPieChartData", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            dose_id: 3,
            prioritygroup_ids: priority_group_idlist,
            vaccine_ids: vaccine_ids,
            province_id: province_id,
            citymun_id: citymun_id,
            barangay_id: barangay_id
        })
    }).then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        return response.json();
    })
        .then(data => {
            const ctx = document.getElementById('vaccinePieChartBooster');
            const listOfVaccines = data.chartDatas.map(x => x.VaccineBrandList[0]);
            const totalPerVaccineBrand = data.chartDatas.map(x => x.TotalPerVaccineBrand); /*const totalPerVaccineBrand = data.chartDatas.map(x => x.TotalPerVaccineBrand != null && x.TotalPerVaccineBrand !== '' ? parseInt(x.TotalPerVaccineBrand).toLocaleString('en', currencyOptions).split('.')[0] : '');*/
            const percentages = data.chartDatas.map(x => x.Percentage != null ? x.Percentage.toFixed(2) + '%' : '');
            const randomColorIndexes = generateRandomIndexes(totalPerVaccineBrand.length, specificColors.length);
            const backgroundColor = randomColorIndexes.map(index => specificColors[index]);

            console.log('List of Vaccines:', listOfVaccines);
            console.log('Data.chartDatas:', data.chartDatas);
            console.log('Total Per Vaccine Brand:', totalPerVaccineBrand);


            BoosterPieChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: listOfVaccines.map((vaccine, index) => `${vaccine} (${percentages[index]})`), // Combine vaccine name and percentage
                    datasets: [{
                        data: totalPerVaccineBrand,
                        label: "Vaccine Count",
                        backgroundColor: backgroundColor
                    }]
                },
                options: {
                    scales: {
                        //y: {
                        //    beginAtZero: true
                        //}
                    }
                }
            });
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

function loadSecondBoosterPieChart(vaccine_ids, province_id, citymun_id, barangay_id) {
    if (SecondBoosterPieChart !== null) {
        SecondBoosterPieChart.destroy(); // Destroy the previous chart instance
    }
    var priority_group_idlist = [1, 2, 3, 4, 6, 7, 8, 11, 12, 13];

    fetch("/Dashboard/GetPieChartData", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            dose_id: 4,
            prioritygroup_ids: priority_group_idlist,
            vaccine_ids: vaccine_ids,
            province_id: province_id,
            citymun_id: citymun_id,
            barangay_id: barangay_id
        })
    }).then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        return response.json();
    })
        .then(data => {
            const ctx = document.getElementById('vaccinePieChartSecondBooster');
            const listOfVaccines = data.chartDatas.map(x => x.VaccineBrandList);
            const totalPerVaccineBrand = data.chartDatas.map(x => x.TotalPerVaccineBrand); /*const totalPerVaccineBrand = data.chartDatas.map(x => x.TotalPerVaccineBrand != null && x.TotalPerVaccineBrand !== '' ? parseInt(x.TotalPerVaccineBrand).toLocaleString('en', currencyOptions).split('.')[0] : '');*/
            const percentages = data.chartDatas.map(x => x.Percentage != null ? x.Percentage.toFixed(2) + '%' : '');
            const randomColorIndexes = generateRandomIndexes(totalPerVaccineBrand.length, specificColors.length);
            const backgroundColor = randomColorIndexes.map(index => specificColors[index]);

            SecondBoosterPieChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: listOfVaccines.map((vaccine, index) => `${vaccine} (${percentages[index]})`), // Combine vaccine name and percentage
                    datasets: [{
                        data: totalPerVaccineBrand,
                        label: "Vaccine Count",
                        backgroundColor: backgroundColor
                    }]
                },
                options: {
                    scales: {
                        //y: {
                        //    beginAtZero: true
                        //}
                    }
                }
            });
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

function loadBoosterSeniorPieChart(vaccine_ids, province_id, citymun_id, barangay_id) {
    if (BoosterSeniorPieChart !== null) {
        BoosterSeniorPieChart.destroy(); // Destroy the previous chart instance
    }

    var priority_group_idlist = [5];

    fetch("/Dashboard/GetPieChartData", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            dose_id: 3,
            prioritygroup_ids: priority_group_idlist,
            vaccine_ids: vaccine_ids,
            province_id: province_id,
            citymun_id: citymun_id,
            barangay_id: barangay_id
        })
    }).then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        return response.json();
    })
        .then(data => {
            const ctx = document.getElementById('vaccinePieChartSeniorBooster');
            const listOfVaccines = data.chartDatas.map(x => x.VaccineBrandList);
            const totalPerVaccineBrand = data.chartDatas.map(x => x.TotalPerVaccineBrand); /* const totalPerVaccineBrand = data.chartDatas.map(x => x.TotalPerVaccineBrand != null && x.TotalPerVaccineBrand !== '' ? parseInt(x.TotalPerVaccineBrand).toLocaleString('en', currencyOptions).split('.')[0] : '');*/
            const percentages = data.chartDatas.map(x => x.Percentage != null ? x.Percentage.toFixed(2) + '%' : '');
            const randomColorIndexes = generateRandomIndexes(totalPerVaccineBrand.length, specificColors.length);
            const backgroundColor = randomColorIndexes.map(index => specificColors[index]);

            BoosterSeniorPieChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: listOfVaccines.map((vaccine, index) => `${vaccine} (${percentages[index]})`), // Combine vaccine name and percentage
                    datasets: [{
                        data: totalPerVaccineBrand,
                        label: "Vaccine Count",
                        backgroundColor: backgroundColor
                    }]
                },
                options: {
                    scales: {
                         //y: {
                        //    beginAtZero: true
                        //}
                    }
                }
            });
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

function loadSecondBoosterSeniorPieChart(vaccine_ids, province_id, citymun_id, barangay_id) {
    $('#dashboard1total').text("loading");
    if (SecondBoosterSeniorPieChart !== null) {
        SecondBoosterSeniorPieChart.destroy(); // Destroy the previous chart instance
    }

    var priority_group_idlist = [5];

    fetch("/Dashboard/GetPieChartData", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            dose_id: 4,
            prioritygroup_ids: priority_group_idlist,
            vaccine_ids: vaccine_ids,
            province_id: province_id,
            citymun_id: citymun_id,
            barangay_id: barangay_id
        })
    }).then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        return response.json();
    })
        .then(data => {
            const ctx = document.getElementById('vaccinePieChartSeniorSecondBooster');
            const listOfVaccines = data.chartDatas.map(x => x.VaccineBrandList);
            const totalPerVaccineBrand = data.chartDatas.map(x => x.TotalPerVaccineBrand); /*const totalPerVaccineBrand = data.chartDatas.map(x => x.TotalPerVaccineBrand != null && x.TotalPerVaccineBrand !== '' ? parseInt(x.TotalPerVaccineBrand).toLocaleString('en', currencyOptions).split('.')[0] : '');*/
            const percentages = data.chartDatas.map(x => x.Percentage != null ? x.Percentage.toFixed(2) + '%' : '');
            const randomColorIndexes = generateRandomIndexes(totalPerVaccineBrand.length, specificColors.length);
            const backgroundColor = randomColorIndexes.map(index => specificColors[index]);

            $('#dashboard1total').text(data.total != null && data.total !== '' ? parseInt(data.total).toLocaleString('en', currencyOptions).split('.')[0] : '');
            if (data.total == null)
                $('#dashboard1total').text("No data");

            console.log("TOTAL " + data.total);
            SecondBoosterSeniorPieChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: listOfVaccines.map((vaccine, index) => `${vaccine} (${percentages[index]})`), // Combine vaccine name and percentage
                    datasets: [{
                        data: totalPerVaccineBrand,
                        label: "Total Vaccine Count",
                        backgroundColor: backgroundColor
                    }]
                },
                options: {
                    scales: {
                        //y: {
                        //    beginAtZero: true
                        //}
                    }
                }
            });
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

function generateRandomColors(count) {
    const colors = [];
    for (let i = 0; i < count; i++) {
        const red = 10;
        const green = Math.floor(Math.random() * (256 - 150 + 1)) + 150;
        const blue = Math.floor(Math.random() * (256 - 170 + 1)) + 170;
        const randomColor = `rgba(${red}, ${green}, ${blue}, 17)`;
        colors.push(randomColor);
    }
    return colors;
}

function generateRandomIndexes(count, maxIndex) {
    const indexes = new Set();

    while (indexes.size < count) {
        indexes.add(Math.floor(Math.random() * maxIndex));
    }

    return Array.from(indexes);
}

function PopulateVaccineTable(vaccine_ids, province_id, citymun_id, barangay_id) {
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
            { "name": "dosename", "data": "dosename", "width": "25%" },
            { "name": "fivetoelevenyo", "data": "fivetoelevenyo", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "twelvetoseventeenyo", "data": "twelvetoseventeenyo", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "eighteentofiftynineyo", "data": "eighteentofiftynineyo", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "sixtyaboveyo", "data": "sixtyaboveyo", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "name": "total", "data": "total", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } }
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/VaccinesDataTable",
        data: {
            vaccine_ids: vaccine_ids,
            province_id: province_id,
            citymun_id: citymun_id,
            barangay_id: barangay_id
        },
        datatype: "json",
    };

    // Create the DataTable using the defined configuration options
    tblVaccines = $('#tbl-vaccines').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}

//function PopulateVaccineTable(vaccine_ids, province_id, citymun_id) {

//    tblVaccines = $('#tbl-vaccines').DataTable({
//        "destroy": true,
//        "ajax": {
//            "type:": "POST",
//            "url": dir + "/Dashboard/VaccinesDataTable",
//            "data": {
//                vaccine_ids: vaccine_ids,
//                province_id: province_id,
//                citymun_id: citymun_id
//            },
//            "datatype": "json"
//        },
//        "responsive": true,
//        "lengthChange": true,
//        "autoWidth": false,
//        "searching": false,
//        "paging": false,
//        "info": false,
//        "entries": false,
//        "ordering": false,
//        "select": { style: 'single' },
//        "columnDefs": [],
//        "columns": [

//            { "name": "dosename", "data": "dosename", "width": "25%" },
//            { "name": "fivetoelevenyo", "data": "fivetoelevenyo", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "name": "twelvetoseventeenyo", "data": "twelvetoseventeenyo", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "name": "eighteentofiftynineyo", "data": "eighteentofiftynineyo", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "name": "sixtyaboveyo", "data": "sixtyaboveyo", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "name": "total", "data": "total", "width": "15%", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } }
//        ],
//    });
//}

//function loadtotal(vaccine_ids, province_id, citymun_id) {
//    $('#dashboard1total').text("loading..");

//    $.ajax({
//        type: "POST",
//        url: dir + "/Dashboard/LoadTotal",
//        data: {
//            vaccine_ids: vaccine_ids,
//            province_id: province_id,
//            citymun_id: citymun_id
//        },
//        dataType: "json",
//        async: false,
//        success: function (data) {
//            if (data.isSuccess) {
//                $('#dashboard1total').text(data.Total != null && data.Total !== '' ? parseInt(data.Total).toLocaleString('en', currencyOptions).split('.')[0] : '');
//            } else {
//                toastr.error("There was an error on retrieving data, please contact your system administrator.");
//            }
//        }
//    });

//}

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