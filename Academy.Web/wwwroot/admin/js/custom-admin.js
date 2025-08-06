function formatFloatingLabel() {
    $('.floating-labels .form-control').on('focus blur', function (e) {
        $(this).parents('.form-group').toggleClass('focused', (e.type === 'focus' || this.value.length > 0));
    }).trigger('blur');
}

function formatFloatingLabelByFilter(filter = null) {
    $(filter).on('focus blur', function (e) {
        $(this).parents('.form-group').toggleClass('focused', (e.type === 'focus' || this.value.length > 0));
    }).trigger('blur');
}

function selectItemFromModal(idValue, displayName, data) {
    $(`[data-select-display="${data}"]`).val(displayName);
    $(`[data-select-input="${data}"]`).val(idValue);
    $(`[data-select-input="${data}"]`).trigger("change");
    formatFloatingLabel()
}

function reinitializeTemplateComponents(selector) {
    try {
        window.initializeCkEditor(selector);
        initializeSelect2Components();
        initialFileInputs();
        initializeLucideIcons();
    } catch (error) {
        console.error(error);
    }
}

function initialFileInputs() {
    $("[ImageInput]").change(function () {
        let x = $(this).attr("ImageInput");
        let submitFormAfterUpload = $(this).attr("SubmitFormAfterUpload");
        if (submitFormAfterUpload !== null && submitFormAfterUpload !== undefined && submitFormAfterUpload !== "") {
            $(`#${submitFormAfterUpload}`).submit();
        } else {
            if (this.files && this.files[0]) {
                var reader = new FileReader();
                reader.onload = (e) => {
                    $("[ImageFile=" + x + "]").attr('src', e.target.result);
                    if (this.files && this.files[0]) {
                        let fileName = this.files[0].name;
                        let imageName = $("[ImageName=" + x + "]");
                        if (imageName.prop("tagName") == "INPUT")
                            imageName.val(fileName);
                        else
                            imageName.html(fileName);
                    }
                };
                reader.readAsDataURL(this.files[0]);
            }
        }
    });

    $("[ImageButton]").click(function (e) {
        let x = $(this).attr("ImageButton");
        $("[ImageInput=" + x + "]").click();
    })
}

$(() => {
    formatFloatingLabel();
    initializeSelect2Components();
    initialFileInputs();
    // validateForms();
})

function generateSlug(titleId, slugId) {
    let title = document.getElementById(titleId).value;
    let slugInput = document.getElementById(slugId);

    if (slugInput) {
        let slug = title.toLowerCase().trim().replace(/ /g, `-`);
        slugInput.value = slug;

        formatFloatingLabelByFilter(`#${slugId}`);
        $(`#${slugId}`).trigger("change");
    }
}