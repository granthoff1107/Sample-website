function DefaultAddTagsToTable(value, name) {
    return AddTagsToTable('#suggestedtags', '#tagtemplate', value, name)
}

function AddTagsToTable(tableSelector, templateSelector, value, name) {
    var template = $(templateSelector).clone().removeAttr("id");
    $(template).removeClass('hidden');
    template.data('tagval', value)

    // remove text
    $(template).contents().filter(function () {
        return this.nodeType === 3;
    }).remove();

    $(template).prepend(name);

    $(tableSelector).find('tr:eq(1) td').append(template);

}

function ActivateCloseTag() {
    $('body').on('click', '.closetag', function (e) {
        e.preventDefault();
        $(this).parent().remove();
    });
}

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

function OverrideSubmitFormWithTags(formSelector, tagSelector) {
    $(formSelector).on('submit', function (e) {
        e.preventDefault();
        
        var tmpFormData = $(formSelector).serializeObject();

        var TagIds = $(tagSelector).map(function () {
            return $(this).data('tagval');
        }).get();

        tmpFormData["TagIds"] = TagIds;

        var tmpTinyMce = $(formSelector + ' .tinyMce');

        var tmpTinyID = tmpTinyMce.attr('id');
        var tmpTinyName = tmpTinyMce.attr('name');
        var mceHtmlData = tinyMCE.get(tmpTinyID).getContent()

        tmpFormData[tmpTinyName] = mceHtmlData;

        $.ajax({
            url: $(formSelector).attr('action'),
            data: tmpFormData,
            traditional: true,
            dataType: 'json',
            type: 'POST',
            success: function (data) {
                location = data.redirectUrl;
            }
        });
    });
}

function TagsAutoComplete(InputSelector, URL, SelectDelegate) {
    $(InputSelector).autocomplete({
        minLength: 2,
        source: function (request, response) {
            $.ajax({
                url: URL,
                type: "POST",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        var label = item.label;
                        var value = item.value;
                        return { label: label, value: value };
                    }))

                }
            })
        },
        select: function (event, ui) {
            event.preventDefault();
            console.log(ui.item.value);
            if (SelectDelegate) {
                SelectDelegate(ui.item.value, ui.item.label);
            }

            //window.location.href = ui.item.value;
        },

        focus: function (e, ui) {
            e.preventDefault();
            $(this).val(ui.item.label)
        },
        messages: {
            noResults: '',
            results: function () { }
        }

    });
}
