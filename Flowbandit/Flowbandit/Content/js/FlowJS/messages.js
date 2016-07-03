$(function () {
    $.ajax({
        url: "/messages/GetUnreadMessages",
        //data: { pageNumber: pageNumber },
        traditional: true,
        dataType: 'json',
        type: 'GET',
    }).done(function (data) {
        var messages = data;

        for (var i = 0; i < messages.length; i++)
        {
            setMessage(messages[i]);
        }
    }).fail(function (xhr, ajaxOptions, thrownError) {
        console.log("error:" + thrownError.toString())
    })
});

$(function () {
    $('body').on('click', '.messageContainer', function ()
    {
        console.log($(this).data('id'));
    });
})


function setMessage(message)
{
    var messageTemplate = $("#messageTemplate").clone().removeAttr('id').removeClass("hidden");
    var timestamp = message.Timestamp;
    console.log(timestamp)
    var relativeTime = moment(timestamp).fromNow();

    messageTemplate.find(".message").html(message.Data);
    messageTemplate.find(".moment").html(relativeTime);
    messageTemplate.find(".senderName").html(message.SenderUser.Username);

    $(messageTemplate).find(".messageContainer").attr('data-id', message.SenderUserId)

    $("#message-dropdown").prepend(messageTemplate.html());
}