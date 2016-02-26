$(function () {
    $('#postPageNumber').val(1);

    var postsUrl = $('#get-posts-url').val();
    var totalPages = $('#total-pages').val();

    var postInfiniteScrollParams = {
        url: postsUrl,
        contentDivSelector: '#postcontent',
        pageNumberSelector: '#postPageNumber',
        totalNumberOfPages: totalPages
    }

    scrollListener(postInfiniteScrollParams)
});