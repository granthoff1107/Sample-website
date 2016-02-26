$(function () {
    var tagsUrl = $('#tags-url').val();

    tagsAutoComplete('#tagsac', tagsUrl, DefaultAddTagsToTable);
    activateCloseTag();
    overrideSubmitFormWithTags("#newvideoform", ".suggesttag");
    initTinyMce();
});