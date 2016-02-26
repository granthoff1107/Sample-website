$(function () {

    var tagsUrl = $('#tags-url').val();

    tagsAutoComplete('#tagsac', tagsUrl, DefaultAddTagsToTable);
    activateCloseTag();
    overrideSubmitFormWithTags("#newpostform", ".suggesttag");
    initTinyMce();
});