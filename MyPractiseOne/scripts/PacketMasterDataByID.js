var GlobalURL = "http://localhost:2825/";

    $(document).ready(function () {
        $("#btnSave").click(function () {
            GetPacketStatusMasterRecordByID();
        });
        $('#txtPacketStatusID').autocomplete({
            source: '/PacketHandler.ashx'
        });
    });



    function GetPacketStatusMasterRecordByID() {
    $.ajax({
        url: GlobalURL + "Master/GetPacketStatusMasterRecordByID",
        type: "POST",
        data: { PacketID: $("#txtPacketStatusID").val() },
        success: function (data) {
            data = JSON.parse(data);
            $("#tbl").find("tr:gt(0)").remove();
            for (var i = 0; i < data.length; i++) {
                //$("#ddlCountry").append($('<option/>').attr("value", data[i].CountryCode).text(data[i].CountryCode));
                $("#tbl").append('<tr> <td><input type="button" id="btnedit" class="btn btn-primary  form-control" value="Edit"  onclick="PacketStatusMasterEditRecord(' + data[i].PacketStatusID + ')"</td> <td><input type="button" id="btndelete" class="btn btn-danger  form-control" value="Delete"  onclick="PacketStatusMasterDataDelete(' + data[i].PacketStatusID + ')"</td>  <td>' + data[i].PacketID + '</td> <td>' + data[i].NewStatusDate + '</td> <td>' + (data[i].NewStatusTime) + '</td> <td>' + data[i].Location + '</td>  <td>' + data[i].PacketStatus + '</td> <td>' + data[i].CreteadBy + '</td> <td>' + data[i].NewCreatedDate + '</td> <td>' + data[i].NewModifiedBy + '</td> <td>' + data[i].NewModifiedDate + '</td> </tr>');

            }
        },
        error: function () {
            alert("Record Not Load");
        }
    })
}