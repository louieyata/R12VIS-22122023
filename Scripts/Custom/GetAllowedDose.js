const vaccineDropdown = document.getElementById("VaccineID");

//when vaccineDropdown changes, get the value of the selected option
vaccineDropdown.addEventListener("change", function () {
  const vaccineID = vaccineDropdown.value;
  //if vaccine does not have value, disable allowedDose dropdown
  if (vaccineID == "") {
    document.getElementById("DoseID").disabled = true;
  } else {
    document.getElementById("DoseID").disabled = false;
  }
  fetchAllowedDose(vaccineID);
  console.log(vaccineID);
});

function fetchAllowedDose(vaccineID) {
  const url = `/Vaccinations/GetAllowedDose?vaccineID=${vaccineID}`;
  fetch(url)
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.json();
    })
    .then((allowedDose) => {
      console.log(allowedDose);
      fillAllowedDoseDropdown(allowedDose);
    })
    .catch((error) => console.log(error));
}

function fillAllowedDoseDropdown(allowedDose) {
  const allowedDoseDropdown = document.getElementById("DoseID");
  allowedDoseDropdown.innerHTML = "";
  allowedDoseDropdown.innerHTML += `<option value=""></option>`;
  allowedDose.forEach((dose) => {
    allowedDoseDropdown.innerHTML += `<option value="${dose.ID}">${dose.VaccineDose}</option>`;
  });
}
