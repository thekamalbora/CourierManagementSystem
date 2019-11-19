
var GlobalURL = "http://localhost:2825/";

$(document).ready(function () {
    $("#ddlCountry").append($('<option/>').attr("value", "0").text("Select"));
    $("#ddlCountry").prop("disabled", true);
});

$('#txtPinCode').on("keypress", function (e)
{
    if (e.keyCode == 13) {
        var pp = $('#txtPinCode').val();
        $.ajax({
            url: GlobalURL + "Master/GetCountryCodeByUsingPincode",
            type: "POST",
            data: { PinCode: pp },
            success: function (data) {
                data = JSON.parse(data);
                $("#ddlCountry").empty();
                $("#ddlCountry").prop("disabled", false);
                $("#ddlState").empty();
                $("#ddlState").prop("disabled", false);
                $("#ddlcity").empty();
                $("#ddlcity").prop("disabled", false);
                for (var i = 0; i < data.length; i++) {
                    //$("#ddlCountry").append($('<option/>').attr("value", data[i].CountryCode).text(data[i].CountryCode));
                    $("#ddlCountry").append($('<option value=' + data[i].CountryCode + '>' + data[i].CountryName + '</option>'));
                    $("#ddlState").append($('<option value=' + data[i].StateCode + '>' + data[i].StateName + '</option>'));
                    $("#ddlcity").append($('<option value=' + data[i].CityID + '>' + data[i].CityName + '</option>'));

                }
            },
            error: function () {
                alert("Address  Not Load");
            }
        })
        return false; // prevent the button click from happening
    }
  
    });


