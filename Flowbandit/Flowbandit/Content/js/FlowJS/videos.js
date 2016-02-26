$(function () {

    $('#videoPageNumber').val(1);

    var setVideoElements = function () { setElementsHeight('.videothumbnail'); };

    //loads when the window loads body load doesn't fire for some reason
    $(window).load(function () {
        setVideoElements();
    });

    //Loads after infinite scroll fires
    $('body').on('load', setVideoElements);

    $(window).resize(function () {
        clearHeight('.videothumbnail');
        setVideoElements();
    });

    var videoUrl = $('#get-video-url').val();
    var totalPages = $('#total-pages').val();

    var postInfiniteScrollParams = {
        url: videoUrl,
        contentDivSelector: '#videocontent',
        pageNumberSelector: '#videoPageNumber',
        totalNumberOfPages: totalPages,
        postCallBack: setVideoElements
    }

    scrollListener(postInfiniteScrollParams)
});