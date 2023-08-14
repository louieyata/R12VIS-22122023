const province = document.getElementById("Person_ProvinceID");

province.addEventListener("change",  () => {
    const province_id = province.value;
    fetchListOfCityMunicipality(province_id);
    clearBarangayDropdown();
    //if province has value, enable cityMunicipality dropdown
    if (province_id != "") {
        document.getElementById("Person_CityMunicipalityID").disabled = false;
    }
    else {
        document.getElementById("Person_CityMunicipalityID").disabled = true;
    }
});

function fetchListOfCityMunicipality (province_id) {
    const url = `/CityMunicipalities/GetCityMunicipality?province_id=${province_id}`;
    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(cityMunicipalityList => {
            console.log(cityMunicipalityList);
            fillCityMunicipalityDropdown(cityMunicipalityList);
        })
        .catch(error => console.log(error));
}

function fillCityMunicipalityDropdown(listOfCityMunicipality) {
    const cityMunicipalityDropdown = document.getElementById("Person_CityMunicipalityID");
    cityMunicipalityDropdown.innerHTML = "";
    cityMunicipalityDropdown.innerHTML += `<option value=""></option>`;
    listOfCityMunicipality.forEach(cityMunicipality => {
        cityMunicipalityDropdown.innerHTML += `<option value="${cityMunicipality.id}">${cityMunicipality.name}</option>`;
    });
}

function clearBarangayDropdown(){
    const barangayDropdown = document.getElementById("Person_BarangayID");
    barangayDropdown.innerHTML = "";
    barangayDropdown.innerHTML += `<option value=""></option>`;
    document.getElementById("Person_BarangayID").disabled = true;
}