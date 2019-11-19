var GlobalURL = "http://localhost:2825/";

$(document).ready(function ()
{
    GetPacketStatusMasterRecord();
    $("#btnSave").click(function () {
        PacketStatusMasterDataInsertUpdate();
    });
});

 
 
//function GetPacketStatusMasterRecord() {
//    $.ajax({
//        url: GlobalURL + "Master/GetPacketStatusMasterRecord",
//        type: "POST",
//        data: {},
//        success: function (data) {
//            $("#lbPacketStatusDetails").html(data.Grid);
//            $("#TotalNoOfRecordPacketStatus").html(data.RowCount);
//        },
//        error: function () {
//            alert("Record Not Load");
//        }
//    })
//}

function GetPacketStatusMasterRecord() {
    $.ajax({
        url: GlobalURL + "Master/GetPacketStatusMasterRecord",
        type: "POST",
        data: {},
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

function PacketStatusMasterEditRecord(PacketStatusID) {
    $.ajax({
        url: GlobalURL + "Master/PacketStatusMasterEditRecord",
        type: "POST",
        data: { PacketStatusID: PacketStatusID },
        success: function (data) {
            data = JSON.parse(data);
            $("#hfPacketStatusID").val(PacketStatusID);
            $("#txtPacketStatusID").val(data[0].PacketID);
            $("#txtPacketStatusDate").val(data[0].StatusDate);
            $("#txtPacketStatusTime").val(data[0].StatusTime);
            $("#ddlPacketStatus").val(data[0].PacketStatus);
            
            $("#txtCreatedBy").val(data[0].CreteadBy);
            $("#txtCreatedDate").val(data[0].CreatedDate);
            $("#txtModifiedBy").val(data[0].ModifiedBy);
            $("#txtModifiedDate").val(data[0].ModifiedDate);
            $("#txtLocation").val(data[0].Location);
            $("#btnSave").val("Update");
        },
        error: function () {
            alert("Record not Edited");
        }
    });
}
function PacketStatusMasterDataDelete(PacketStatusID) {
    if (confirm("Are you sure to delete!!")) {
        $.ajax({
            url: GlobalURL + "Master/PacketStatusMasterDataDelete",
            type: "POST",
            data: { PacketStatusID: PacketStatusID },
            success: function () {
                alert("Record Deleted Successfully");
                GetCountryRecord();
            },
            error: function () {
                alert("Record Not Deleted");
            }
        });
    }
}

function PacketStatusMasterDataInsertUpdate()
{
    $.ajax({
        url: GlobalURL + "Master/PacketStatusMasterDataInsertUpdate",
        type: "POST",
        data: {
            PacketStatusID: $("#hfPacketStatusID").val(),
            PacketID: $("#txtPacketStatusID").val(),
            StatusDate: $("#txtPacketStatusDate").val(),
            StatusTime: $("#txtPacketStatusTime").val(),
            PacketStatus: $("#ddlPacketStatus").val(),
            CreteadBy: $("#txtCreatedBy").val(),
            CreatedDate: $("#txtCreatedDate").val(),
            ModifiedBy: $("#txtModifiedBy").val(),
            ModifiedDate: $("#txtModifiedDate").val(),
            Location: $("#txtLocation").val(),
        },
        success: function (data) {
            data = JSON.parse(data.d);
            if (data.Status == "1" || data.Status == "2") {
                alert(data.Result);
                GetStateRecord();
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
