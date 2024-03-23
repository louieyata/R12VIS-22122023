const cityMunicipalityDropdown = document.getElementById("Person_CityMunicipalityID");

cityMunicipalityDropdown.addEventListener("change", () => {
    const cityMunicipality_id = cityMunicipalityDropdown.value;
    fetchBarangayList(cityMunicipality_id);

    if (cityMunicipality_id != "") {
        document.getElementById("Person_BarangayID").disabled = false;
    }
    else {
        document.getElementById("Person_BarangayID").disabled = true;
    }
});

function fetchBarangayList (cityMunicipality_id) {
    const url = `/Barangays/GetBarangays?cityMunicipality_id=${cityMunicipality_id}`;
    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(barangayList => {
            console.log(barangayList);
            fillBarangayDropdown(barangayList);
        })
        .catch(error => console.log(error));
}

function fillBarangayDropdown(barangayList) {
    const barangayDropdown = document.getElementById("Person_BarangayID");
    barangayDropdown.innerHTML = "";
    barangayDropdown.innerHTML += `<option value=""></option>`;

    barangayList.forEach(barangayList => {
        barangayDropdown.innerHTML += `<option value="${barangayList.id}">${barangayList.name}</option>`;
    });
}

