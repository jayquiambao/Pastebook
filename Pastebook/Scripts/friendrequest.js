﻿$(document).ready(function () {
    $(document).on('click', ".btnConfirm", function () {
        var data = {
            relationshipID: $(this).val(),
            status: "Confirm"
        };
        $.ajax({
            url: acceptRejectRequestUrl,
            data: data,
            type: 'GET',
            success: function (data) {
                CheckResultForRequest(data);
            },
            error: function () {
                $(location).attr('href', errorLink);
            }

        });
    });

    $(document).on('click', ".btnReject", function () {
        var data = {
            relationshipID: $(this).val(),
            status: "Reject"
        };
        $.ajax({
            url: acceptRejectRequestUrl,
            data: data,
            type: 'GET',
            success: function (data) {
                CheckResultForRequest(data);
            },
            error: function () {
                $(location).attr('href', errorLink);
            }

        });
    });

    function CheckResultForRequest(data) {
        if (data.Result == 1) {
            $('#friendRequestPartial').load(friendRequestPartialUrl);
        }
        else {
            alert("Fail Inserting");
        }
    };
});