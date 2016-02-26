
/*
 * We are gonna initialize all checkbox and radio inputs to 
 * iCheck plugin in.
 * You can find the documentation at http://fronteed.com/iCheck/
 */
$("input[type='checkbox'], input[type='radio']").iCheck({
    checkboxClass: 'icheckbox_minimal',
    radioClass: 'iradio_minimal'
});

function initTinyMce() {
    tinymce.init({
        selector: ".tinyMce",
        plugins: [
            "advlist autolink lists link image charmap print preview anchor",
            "searchreplace visualblocks code fullscreen",
            "insertdatetime media table contextmenu paste"
        ],
        toolbar: "insertfile undo redo | | alignleft aligncenter alignright alignjustify | styleselect | bold italic | link image | bullist numlist | outdent indent"
    });
}

function showMembersPanel() {
    $('.loginPanel').addClass("hidden")
    $('.membersPanel').removeClass("hidden")
}

function showLoginPanel() {
    $('.loginPanel').removeClass("hidden")
    $('.membersPanel').addClass("hidden")
}

function setLoginPanel(isAnon) {
    (isAnon ? showLoginPanel : showMembersPanel)()
}

function setElementsHeight(selector) {

    // clear the previous set heights on the div
    clearHeight(selector);

    console.log('setting Max element height');

    // Get an array of all element heights
    var elementHeights = $(selector).map(function () {
        return $(this).height();
    }).get();

    // Math.max takes a variable number of arguments
    // `apply` is equivalent to passing each height as an argument
    var maxHeight = Math.max.apply(null, elementHeights);

    // Set each height to the max height
    $(selector).height(maxHeight);
}

function clearHeight(selector) {
    $(selector).height('auto');
}

//this isn't working for some reason https://github.com/chriscoyier/Fluid-Width-Video/blob/master/demo.html
function AutoResizeAllVideos() {
    var $allVideos = $("iframe[src^='http://player.vimeo.com'], iframe[src^='https://www.youtube.com'], object, embed"),
    $fluidEl = $("figure");

    $allVideos.each(function () {
        $(this)
          // jQuery .data does not work on object/embed elements
          .attr('data-aspectRatio', this.height / this.width)
          .removeAttr('height')
          .removeAttr('width');
    });

    $(window).resize(function () {
        var newWidth = $fluidEl.width();
        $allVideos.each(function () {
            var $el = $(this);
            $el
                .width(newWidth)
                .height(newWidth * $el.attr('data-aspectRatio'));

        });

    }).resize();
}

$(function () {
    var loginVal = $.parseJSON($('#display-login').val());
    setLoginPanel(loginVal);
})