$(document).ready(function () {

    var preloadbg = document.createElement("img");
    preloadbg.src = "https://s3-us-west-2.amazonaws.com/s.cdpn.io/245657/timeline1.png";

    $('body').on('click', '#chatbox', function (e) {
        //prevent bootstrap dropdown propagation
        e.stopPropagation();
    });

    var navContainerIds = ['#friends', '#chats']
    $('body').on('click', '.chats', function () {
        $(navContainerIds.join()).addClass('hidden');
        $('#chats').removeClass('hidden');
        $('.chats').addClass('active');
        $('.friends').removeClass('active');
    });

    $('body').on('click', '.friends', function () {
        $(navContainerIds.join()).addClass('hidden');
        $('#friends').removeClass('hidden');
        $('.friends').addClass('active');
        $('.chats').removeClass('active');
    });

    $('body').on('click', '.friend', function (e) {

        e.stopPropagation();

        var childOffset = $(this).offset();
        var parentOffset = $(this).parent().parent().offset();
        var childTop = childOffset.top - parentOffset.top;
        var clone = $(this).find('img').eq(0).clone();
        var top = childTop + 12 + "px";

        $(clone).css({
            'top': top
        }).addClass("floatingImg").appendTo("#chatbox");

        setTimeout(function () {
            $("#profile p").addClass("animate");
            $("#profile").addClass("animate");
        }, 100);
        setTimeout(function () {
            $("#chat-messages").addClass("animate");
            $('.cx, .cy').addClass('s1');
            setTimeout(function () {
                $('.cx, .cy').addClass('s2');
            }, 100);
            setTimeout(function () {
                $('.cx, .cy').addClass('s3');
            }, 200);
        }, 150);

        $('.floatingImg').animate({
            'width': "68px",
            'left': '108px',
            'top': '20px'
        }, 200);

        var name = $(this).find("p strong").html();
        var email = $(this).find("p span").html();
        $("#profile p").html(name);
        $("#profile span").html(email);

        $(".message").not(".right").find("img").attr("src", $(clone).attr("src"));
        $('#friendslist').fadeOut();
        $('#chatview').fadeIn();

        $('#close').unbind("click").click(function (e) {
            e.stopPropagation();
            $("#chat-messages, #profile, #profile p").removeClass("animate");
            $('.cx, .cy').removeClass("s1 s2 s3");
            $('.floatingImg').animate({
                'width': "40px",
                'top': top,
                'left': '12px'
            }, 200, function () {
                $('.floatingImg').remove()
            });

            setTimeout(function () {
                $('#chatview').fadeOut();
                $('#friendslist').fadeIn();
            }, 50);
        });
    });
});