//TODO Stop pollution the global namespace
isScrollExecuting = false;

function scrollListener(infiniteScrollParams) {
    if (areRemaningPages(infiniteScrollParams)) {

        //Make sure your not already at the bottom of the page
        executeConditionalScroll(infiniteScrollParams);

        //unbinds itself every time it fires
        $(window).one("scroll", function () {
            executeConditionalScroll(infiniteScrollParams);

            //rebinds itself after 200ms
            setTimeout(function () { scrollListener(infiniteScrollParams); }, 200);
        });
    }
};

function areRemaningPages(infiniteScrollParams)
{
    return parseInt($(infiniteScrollParams.pageNumberSelector).val()) <= parseInt(infiniteScrollParams.totalNumberOfPages);
}

function isWithingMinimumRequestHeight() {
    var minHeight = 300;
    if ($(window).width() <= 767)
    {
        minHeight = 900;
    }
    else if ($(window).width() <= 992)
    {
        minHeight = 600;
    }

    return $(window).scrollTop() >= $(document).height() - $(window).height() - minHeight;
}

function executeConditionalScroll(infiniteScrollParams) {
    //Hack during the first bottom check your might accidently ajax before the other ajax returns firing multiple events
    // there is still a bug because your firing twice
    if (areRemaningPages(infiniteScrollParams) && false == isScrollExecuting)
    {
        var pageNumber = parseInt($(infiniteScrollParams.pageNumberSelector).val());
        if (isWithingMinimumRequestHeight()) {

            isScrollExecuting = true;

            $.ajax({
                url: infiniteScrollParams.url,
                data: { pageNumber: pageNumber },
                traditional: true,
                dataType: 'json',
                type: 'GET',
                success: function (data) {
                    var pageCount = pageNumber;
                    pageCount++;

                    $(infiniteScrollParams.pageNumberSelector).val(pageCount);

                    $(infiniteScrollParams.contentDivSelector).append(data.htmldata)

                    if (infiniteScrollParams.postCallBack) {
                        infiniteScrollParams.postCallBack();
                    }

                    isScrollExecuting = false;
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log("error:" + thrownError.toString())
                }
            });
        }
    }
}