var GlobalURL = "http://localhost:2825/";
$(document).ready(function () {

    GetPincodeDetails();
    $("#btnSave").click(function () {
        PincodeInsertUpdateData();
    });

});


function PincodeInsertUpdateData() {


    $.ajax({
        url: GlobalURL + "Master/PincodeInsertUpdateData",
        type: "POST",
        data: {
            PincodeID: $("#hfPincodeID").val(),
            PinCode: $("#txtPinCode").val(),
            CountryCode: $("#ddlCountry").val(),
            StateCode: $("#ddlState").val(),
            CityID: $("#ddlcity").val(),
            IsActive: $("#chckIsAactive").is(':checked') ? "1" : "0"
        },
        success: function (data) {
            data = JSON.parse(data.d);
            if (data.Status == "1" || data.Status == "2") {
                alert(data.Result);
                alert("Data Saved Successfully.");
                GetPincodeDetails();
                //Reset();
            } else {
                alert(data.Result);
            }
            if (data.Focus != "") {
                $("#" + data.Focus).focus();
            }
        },
        error: function () {
            alert("Record not saved,Somethong wrong");
        }
    });
}




function GetPincodeDetails() {
    $.ajax({
        url: GlobalURL + "Master/GetPincodeDetails",
        type: "POST",
        data: {},
        success: function (data) {
            $("#lbPincodeDetails").html(data.Grid);
            $("#TotalNoOfRecordsInPincode").html(data.RowCount);
        },
        error: function () {
            alert("Record Not Load");
        }
    })
}

function PincodeDataEdit(PincodeID) {
    $.ajax({
        url: GlobalURL + "Master/PincodeDataEdit",
        type: "POST",
        data: { PincodeID: PincodeID },
        success: function (data) {
            data = JSON.parse(data);
            $("#hfPincodeID").val(PincodeID);
            $("#txtPinCode").val(data[0].PinCode);
            $("#ddlCountry").val(data[0].CountryCode);
            GetStateCode();
            setTimeout(function () {
                $("#ddlState").val(data[0].StateCode);
            }, 500)
            $("#ddlState").val(data[0].StateCode);
            GetCityCode();
            setTimeout(function () {
                $("#ddlcity").val(data[0].CityCode);
            }, 500)
            $("#ddlcity").val(data[0].CityCode);
            if ($("#chckIsAactive").val() == true) {
                $("#chckIsAactive").prop("checked", true);
            }
            else {
                $("#chckIsAactive").attr("checked", false);
            }

            $("#btnSave").val("Update");
        },
        error: function () {
            alert("Record not Edited");
        }
    });
}

function PincodeDataDelete(PincodeID) {
    if (confirm("Are you sure to delete!!")) {
        $.ajax({
            url: GlobalURL + "Master/PincodeDataDelete",
            type: "POST",
            data: { PincodeID: PincodeID },
            success: function () {
                alert("Record Deleted Successfully");
                GetPincodeDetails();
            },
            error: function () {
                alert("Record Not Deleted");
            }
        });
    }
}
