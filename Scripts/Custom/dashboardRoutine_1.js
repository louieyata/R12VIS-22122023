const specificColors = ['#89CFF0', '#0096FF', '#6495ED', '#96DED1', '#40E0D0', '#50C878', '#2AAA8A', '#40B5AD', '#CCCCFF', '#00FFFF', '#9FE2BF', '#7FFFD4', '#B6D0E2']; // Add more colors as needed '#FFAC1C', '#FFBF00', '#FF7F50', '#F88379', '#FAC898'
let region_id = 1;
let province = 0;
let citymun = 0;

$(document).ready(function () {
    /*Data Table*/
    PopulateAtBirthVaccsProvince(region_id);
    PopulatePVVaccsProvince(region_id);
    PopulateOPVVaccsProvince(region_id);
    PopulateIPVVaccsProvince(region_id);
    PopulatePCVVaccsProvince(region_id);
    PopulateMMRVaccsProvince(region_id);
});

$("#filter-button").click(function () {
    var region = 1;
    var province = $("#province_id").val();
    var citymun = $("#cm_id").val();

    console.log("province:", province);
    console.log("citymun:", citymun);


    if (province && (citymun == 0)) {
        toastr.success("loaded succesfully");
        PopulateAtBirthVaccsCityMun(province);
        PopulatePVVaccsCityMun(province);
        PopulateOPVVaccsCityMun(province);
        PopulateIPVVaccsCityMun(province);
        PopulatePCVVaccsCityMun(province);
        PopulateMMRVaccsCityMun(province);
    } else if (province && citymun) {
        toastr.success("loaded succesfully");
        PopulateAtBirthVaccsBarangay(citymun);
        PopulatePVVaccsBarangay(citymun);
        PopulateOPVVaccsBarangay(citymun);
        PopulateIPVVaccsBarangay(citymun);
        PopulatePCVVaccsBarangay(citymun);
        PopulateMMRVaccsBarangay(citymun);
    } else {
        toastr.success("loaded succesfully");
        PopulateAtBirthVaccsProvince(region);
        PopulatePVVaccsProvince(region);
        PopulateOPVVaccsProvince(region);
        PopulateIPVVaccsProvince(region);
        PopulatePCVVaccsProvince(region);
        PopulateMMRVaccsProvince(region);
    }
});






//function PopulateAtBirthVaccsProvince(region_id) {
//    // Define the DataTable configuration options
//    var dataTableConfig = {
//        destroy: true,
//        responsive: true,
//        lengthChange: true,
//        autoWidth: false,
//        searching: false,
//        paging: false,
//        info: false,
//        entries: false,
//        ordering: false,
//        select: { style: 'single' },
//        columnDefs: [],
//        "columns": [
//            { "title": "Province", "data": "ProvinceName", "width": "50%"},
//            { "data": "BCG", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "data": "Hepatitis_B", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "data": "PVI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "data": "PVII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "data": "PVIII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "data": "OPVI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "data": "OPVII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "data": "OPVIII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "data": "IPVI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "data": "IPVII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "data": "PCVI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "data": "PCVII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "data": "PCVIII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "data": "MMRI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
//            { "data": "MMRII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },



//        ],
//    };

//    // Define the AJAX configuration for fetching data
//    var ajaxConfig = {
//        type: "POST",
//        url: dir + "/Dashboard/RoutineAtBirthVaccinesProvinceData",
//        data: {
//            region_id: region_id
//        },
//        datatype: "json",

//        success: function (responseData) {
//            var data = responseData.data; // DataTable data
//            /*var totalData = responseData.total; // Total data*/

//            // Update the DataTable
//            tblRoutineAtBirthVaccs.clear().rows.add(data).draw();
//        },

//        error: function (xhr, status, error) {
//            if (xhr.status === 404) {
//                console.error("Resource not found. Please check the URL.");
//            } else if (xhr.status === 500) {
//                console.error("Internal server error. Please try again later.");
//            } else {
//                console.error("An error occurred. Please refresh the page and try again.");
//            }
//        }
//    };

//    // Create the DataTable using the defined configuration options
//    tblRoutineAtBirthVaccs = $('#tbl-at-birth-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
//}


function PopulateAtBirthVaccsProvince(region_id) {
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
            { "title": "Province", "data": "ProvinceName", "width": "50%" },
            { "data": "BCG", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "Hepatitis_B", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutineAtBirthVaccinesProvinceData",
        data: {
            region_id: region_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutineAtBirthVaccs.clear().rows.add(data).draw();
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
    tblRoutineAtBirthVaccs = $('#tbl-at-birth-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}
function PopulatePVVaccsProvince(region_id) {
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
            { "title": "Province", "data": "ProvinceName", "width": "50%" },
            { "data": "PVI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "PVII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "PVIII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },


        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutinePVVaccinesProvinceData",
        data: {
            region_id: region_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutinePVVaccs.clear().rows.add(data).draw();
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
    tblRoutinePVVaccs = $('#tbl-pv-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}
function PopulateOPVVaccsProvince(region_id) {
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
            { "title": "Province", "data": "ProvinceName", "width": "50%" },
            { "data": "OPVI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "OPVII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "OPVIII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },


        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutineOPVVaccinesProvinceData",
        data: {
            region_id: region_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutineOPVVaccs.clear().rows.add(data).draw();
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
    tblRoutineOPVVaccs = $('#tbl-opv-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}
function PopulateIPVVaccsProvince(region_id) {
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
            { "title": "Province", "data": "ProvinceName", "width": "50%" },
            { "data": "IPVI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "IPVII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutineIPVVaccinesProvinceData",
        data: {
            region_id: region_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutineIPVVaccs.clear().rows.add(data).draw();
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
    tblRoutineIPVVaccs = $('#tbl-ipv-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}
function PopulatePCVVaccsProvince(region_id) {
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
            { "title": "Province", "data": "ProvinceName", "width": "50%" },
            { "data": "PCVI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "PCVII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "PCVIII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutinePCVVaccinesProvinceData",
        data: {
            region_id: region_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutinePCVVaccs.clear().rows.add(data).draw();
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
    tblRoutinePCVVaccs = $('#tbl-pcv-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}
function PopulateMMRVaccsProvince(region_id) {
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
            { "title": "Province", "data": "ProvinceName", "width": "50%" },
            { "data": "MMRI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "MMRII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutineMMRVaccinesProvinceData",
        data: {
            region_id: region_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutineMMRVaccs.clear().rows.add(data).draw();
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
    tblRoutineMMRVaccs = $('#tbl-mmr-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}






function PopulateAtBirthVaccsCityMun(province_id) {
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
        scrollY: 350,
        "columns": [
            { "title": "CityMun", "data": "CityMunName", "width": "50%" },
            { "data": "BCG", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "Hepatitis_B", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutineAtBirthVaccinesCityMunData",
        data: {
            province_id: province_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutineAtBirthVaccs.clear().rows.add(data).draw();
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
    tblRoutineAtBirthVaccs = $('#tbl-at-birth-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}
function PopulatePVVaccsCityMun(province_id) {
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
        scrollY: 350,
        "columns": [
            { "title": "CityMun", "data": "CityMunName", "width": "50%" },
            { "data": "PVI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "PVII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "PVIII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },


        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutinePVVaccinesCityMunData",
        data: {
            province_id: province_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutinePVVaccs.clear().rows.add(data).draw();
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
    tblRoutinePVVaccs = $('#tbl-pv-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}
function PopulateOPVVaccsCityMun(province_id) {
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
        scrollY: 350,
        "columns": [
            { "title": "CityMun", "data": "CityMunName", "width": "50%" },
            { "data": "OPVI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "OPVII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "OPVIII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },


        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutineOPVVaccinesCityMunData",
        data: {
            province_id: province_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutineOPVVaccs.clear().rows.add(data).draw();
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
    tblRoutineOPVVaccs = $('#tbl-opv-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}
function PopulateIPVVaccsCityMun(province_id) {
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
        scrollY: 350,
        "columns": [
            { "title": "CityMun", "data": "CityMunName", "width": "50%" },
            { "data": "IPVI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "IPVII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutineIPVVaccinesCityMunData",
        data: {
            province_id: province_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutineIPVVaccs.clear().rows.add(data).draw();
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
    tblRoutineIPVVaccs = $('#tbl-ipv-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}
function PopulatePCVVaccsCityMun(province_id) {
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
        scrollY: 350,
        "columns": [
            { "title": "CityMun", "data": "CityMunName", "width": "50%" },
            { "data": "PCVI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "PCVII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "PCVIII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutinePCVVaccinesCityMunData",
        data: {
            province_id: province_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutinePCVVaccs.clear().rows.add(data).draw();
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
    tblRoutinePCVVaccs = $('#tbl-pcv-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}
function PopulateMMRVaccsCityMun(province_id) {
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
        scrollY: 350,
        "columns": [
            { "title": "CityMun", "data": "CityMunName", "width": "50%" },
            { "data": "MMRI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "MMRII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutineMMRVaccinesCityMunData",
        data: {
            province_id: province_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutineMMRVaccs.clear().rows.add(data).draw();
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
    tblRoutineMMRVaccs = $('#tbl-mmr-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}







function PopulateAtBirthVaccsBarangay(citymun_id) {
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
        scrollY: 350,
        "columns": [
            { "title": "Barangay", "data": "BarangayName", "width": "50%" },
            { "data": "BCG", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "Hepatitis_B", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutineAtBirthVaccinesBarangayData",
        data: {
            citymun_id: citymun_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutineAtBirthVaccs.clear().rows.add(data).draw();
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
    tblRoutineAtBirthVaccs = $('#tbl-at-birth-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}
function PopulatePVVaccsBarangay(citymun_id) {
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
        scrollY: 350,
        "columns": [
            { "title": "Barangay", "data": "BarangayName", "width": "50%" },
            { "data": "PVI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "PVII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "PVIII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },


        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutinePVVaccinesBarangayData",
        data: {
            citymun_id: citymun_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutinePVVaccs.clear().rows.add(data).draw();
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
    tblRoutinePVVaccs = $('#tbl-pv-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}
function PopulateOPVVaccsBarangay(citymun_id) {
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
        scrollY: 350,
        "columns": [
            { "title": "Barangay", "data": "BarangayName", "width": "50%" },
            { "data": "OPVI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "OPVII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "OPVIII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },


        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutineOPVVaccinesBarangayData",
        data: {
            citymun_id: citymun_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutineOPVVaccs.clear().rows.add(data).draw();
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
    tblRoutineOPVVaccs = $('#tbl-opv-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}
function PopulateIPVVaccsBarangay(citymun_id) {
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
        scrollY: 350,
        "columns": [
            { "title": "Barangay", "data": "BarangayName", "width": "50%" },
            { "data": "IPVI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "IPVII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutineIPVVaccinesBarangayData",
        data: {
            citymun_id: citymun_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutineIPVVaccs.clear().rows.add(data).draw();
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
    tblRoutineIPVVaccs = $('#tbl-ipv-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}
function PopulatePCVVaccsBarangay(citymun_id) {
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
        scrollY: 350,
        "columns": [
            { "title": "Barangay", "data": "BarangayName", "width": "50%" },
            { "data": "PCVI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "PCVII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "PCVIII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutinePCVVaccinesBarangayData",
        data: {
            citymun_id: citymun_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutinePCVVaccs.clear().rows.add(data).draw();
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
    tblRoutinePCVVaccs = $('#tbl-pcv-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
}
function PopulateMMRVaccsBarangay(citymun_id) {
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
        scrollY: 350,
        "columns": [
            { "title": "Barangay", "data": "BarangayName", "width": "50%" },
            { "data": "MMRI", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
            { "data": "MMRII", "render": function (data, type, row) { return data != null && data !== '' ? parseInt(data).toLocaleString('en', currencyOptions).split('.')[0] : ''; } },
        ],
    };

    // Define the AJAX configuration for fetching data
    var ajaxConfig = {
        type: "POST",
        url: dir + "/Dashboard/RoutineMMRVaccinesBarangayData",
        data: {
            citymun_id: citymun_id
        },
        datatype: "json",

        success: function (responseData) {
            var data = responseData.data; // DataTable data
            /*var totalData = responseData.total; // Total data*/

            // Update the DataTable
            tblRoutineMMRVaccs.clear().rows.add(data).draw();
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
    tblRoutineMMRVaccs = $('#tbl-mmr-routine-vaccs').DataTable($.extend(dataTableConfig, { ajax: ajaxConfig }));
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