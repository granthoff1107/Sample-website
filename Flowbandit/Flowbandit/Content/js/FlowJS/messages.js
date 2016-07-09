$(function () {
    
    getUnreadMessages();
    getFriends();

    $('#sendmessage input').on('keypress', function (event) {
        if (event.which === 13) {
            sendMessage();
        }
    });

    $('body').on('click', '#sendmessage button', function () {
        sendMessage();
    });

    $('body').on('click', '.friend', function () {
        var friendId = $(this).data('id');
        getConversationMessages(friendId);
    });
});

function sendMessage()
{
    var message = $('#sendmessage input').val();
    $('#sendmessage input').val('');

    if (message) {
        var receivierId = $('#chatview').data('friendId');
        $.ajax({
            url: "/messages/AddMessage",
            data: {
                Data: message,
                ReceiverUserId: receivierId,
                IsViewed: false,
            },
            traditional: true,
            dataType: 'json',
            type: 'post',
        }).done(function (data) {
            setConversationMessageWindow(data);
        }).fail(function (xhr, ajaxOptions, thrownError) {
            console.log("error:" + thrownError.toString())
        });
    }
}

function getFriends()
{
    $.ajax({
        url: "/friends/GetAllFriends",
        traditional: true,
        dataType: 'json',
        type: 'GET',
    }).done(function (friends) {
        for (var i = 0; i < friends.length; i++) {
            setFriends(friends[i]);
        }
    }).fail(function (xhr, ajaxOptions, thrownError) {
        console.log("error:" + thrownError.toString())
    });
}

function getUnreadMessages()
{
    $.ajax({
        url: "/messages/GetUnreadMessages",
        traditional: true,
        dataType: 'json',
        type: 'GET',
    }).done(function (messages) {
        $('#unread-messages').html(messages.length)
        
        for (var i = 0; i < messages.length; i++) {
            setFriendMessage(messages[i]);
        }
    }).fail(function (xhr, ajaxOptions, thrownError) {
        console.log("error:" + thrownError.toString())
    });
}

function getConversationMessages(friendId)
{
    $.ajax({
        url: "/messages/GetConversationMessages/" + friendId,
        traditional: true,
        dataType: 'json',
        type: 'GET',
    }).done(function (data) {
        var messages = data;
        $('#chatview').data('friendId', friendId);
        setConversationMessageWindow(messages);
    }).fail(function (xhr, ajaxOptions, thrownError) {
        console.log("error:" + thrownError.toString())
    })
}

function setConversationMessageWindow(messages)
{
    var userId = $('#userId').val();

    $('#chat-messages').empty();
    //TODO: set the username
    //$('#friendUsername').val()

    for (var i = 0; i < messages.length; i++) {
        setConversationMessage(messages[i], userId);
    }

    $("#chat-messages").scrollTop($("#chat-messages")[0].scrollHeight);
}

function setFriends(friend)
{
    var friendTemplate = $("#friendTemplate").clone().removeAttr('id');
    
    friendTemplate.find(".friendName").html(friend.FriendUser.Username);
    friendTemplate.find(".status").addClass("inactive");

    $(friendTemplate).attr('data-id', friend.FriendUserId)

    $("#friends").prepend(friendTemplate);
}


function setFriendMessage(message)
{
    var messageTemplate = $("#inboxTemplate").clone().removeAttr('id');
    var timestamp = message.Timestamp;
    console.log(timestamp)
    var relativeTime = moment(timestamp).fromNow();

    messageTemplate.find(".inbox-message").html(message.Data);
    messageTemplate.find(".moment").html(relativeTime);
    messageTemplate.find(".senderName").html(message.SenderUser.Username);

    $(messageTemplate).find(".friend").attr('data-id', message.SenderUserId)

    $("#chats").prepend(messageTemplate.html());
}

function setConversationMessage(message, userId) {

    var template = $("#conversationMessageTemplate").clone().removeAttr('id');

    if (userId == message.SenderUserId)
    {
        template.addClass("right")
    }
    
    var timestamp = message.Timestamp;
    var relativeTime = moment(timestamp).fromNow();

    //TODO: set the user thumbnail
    //template.find(".user-thumbnail").html(message);
    var cache = template.find(".bubble").children();
    template.find(".bubble").text(message.Data).append(cache);

    template.find(".moment").html(relativeTime);

    $("#chat-messages").prepend(template);
}
