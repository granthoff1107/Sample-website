function scrollListener(url, contentDivSelector, pageNumberSelector, totalNumberOfPages, postCallBack) {
    if (parseInt($(pageNumberSelector).val()) <= parseInt(totalNumberOfPages)) {

        //Make sure your not already at the bottom of the page
        executeConditionalScroll(url, contentDivSelector, pageNumberSelector, postCallBack, totalNumberOfPages);

        //unbinds itself every time it fires
        $(window).one("scroll", function () {
            executeConditionalScroll(url, contentDivSelector, pageNumberSelector, postCallBack, totalNumberOfPages);

            //rebinds itself after 200ms
            setTimeout(function () { scrollListener(url, contentDivSelector, pageNumberSelector, totalNumberOfPages, postCallBack); }, 200);
        });
    }
};

function executeConditionalScroll(url, contentDivSelector, pageNumberSelector, postCallBack, totalNumberOfPages) {
    //Hack during the first bottom check your might accidently ajax before the other ajax returns firing multiple events
    // there is still a bug because your firing twice
    if (parseInt($(pageNumberSelector).val()) <= parseInt(totalNumberOfPages)) 
    {
        if ($(window).scrollTop() >= $(document).height() - $(window).height() - 100) {
            $.ajax({
                url: url,
                data: { pageNumber: $(pageNumberSelector).val() },
                traditional: true,
                dataType: 'json',
                type: 'GET',
                success: function (data) {

                    var pageCount = parseInt($(pageNumberSelector).val());

                    pageCount++;

                    $(pageNumberSelector).val(pageCount);

                    $(contentDivSelector).append(data.htmldata)

                    if (postCallBack) {
                        postCallBack();
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log("error:" + thrownError.toString())
                }
            });
        }
    }
}