$(() => {
   initializeLucideIcons();
});

function initializeLucideIcons(){
    lucide.createIcons();
}

function open_waiting() {
    $(".loading-page").attr("class" , "[&.loading-page--before-hide]:h-screen [&.loading-page--before-hide]:relative loading-page loading-page--before-hide [&.loading-page--before-hide]:before:block [&.loading-page--hide]:before:opacity-0 before:content-[''] before:transition-opacity before:duration-300 before:hidden before:inset-0 before:h-screen before:w-screen before:fixed before:bg-gradient-to-b before:from-theme-1 before:to-theme-2 before:z-[60] [&.loading-page--before-hide]:after:block [&.loading-page--hide]:after:opacity-0 after:content-[''] after:transition-opacity after:duration-300 after:hidden after:h-16 after:w-16 after:animate-pulse after:fixed after:opacity-50 after:inset-0 after:m-auto after:bg-loading-puff after:bg-cover after:z-[61]");

}


function close_waiting() {
    $(".loading-page").attr("class" , "[&.loading-page--before-hide]:h-screen [&.loading-page--before-hide]:relative loading-page [&.loading-page--before-hide]:before:block [&.loading-page--hide]:before:opacity-0 before:content-[''] before:transition-opacity before:duration-300 before:hidden before:inset-0 before:h-screen before:w-screen before:fixed before:bg-gradient-to-b before:from-theme-1 before:to-theme-2 before:z-[60] [&.loading-page--before-hide]:after:block [&.loading-page--hide]:after:opacity-0 after:content-[''] after:transition-opacity after:duration-300 after:hidden after:h-16 after:w-16 after:animate-pulse after:fixed after:opacity-50 after:inset-0 after:m-auto after:bg-loading-puff after:bg-cover after:z-[61]");
}


function showToaster(message, type) {
    switch (type) {
        case 'warning': {
            toastr.warning(message, "هشدار");
            break;
        }
        case 'info': {
            toastr.info(message,"پیام");
            break;
        }
        case 'success': {
            toastr.success(message, "عملیات موفق");
            break;
        }
        case 'error': {
            toastr.error(message, "خطا");
            break;
        }
    }
}

function fillTakeEntity(formId = "filter-search") {
    let takeEntity = $(`#${formId}-entries :selected`).val();
    $("#TakeEntity").val(takeEntity);
    $(`#${formId}`).submit();
}

function FillPageIdByFromId(pageId, formId) {
    const form = $(`#${formId}`);
    form.find("[name='Page']").val(pageId);
    const elem = '<button class="d-none submit-hidden" type="submit"></button>';
    form.append(elem);
    form.find(".submit-hidden").click();
}

function FillPageId(id) {
    $("#Page").val(id);
    $("#filter-search").submit();
}


function onAjaxFailure(xhr, status, error) {
    close_waiting();
    showToaster(xhr.responseText, 'error');
}

function initializeSelect2Components() {
    $("[data-select2-url]").each(function () {
        let $dropdownParent = $(this).closest(".modal");
        if ($dropdownParent.length === 0) {
            $dropdownParent = null;
        }

        let url = $(this).attr("data-select2-url")
        let additionalData = $(this).attr("data-select2-additional-item") | undefined;
        let currentPage;
        $(this).select2({
            language: {
                searching: function () {
                    return "در حال جستجو...";
                },
                noResults: function () {
                    return "موردی یافت نشد";
                },
                loadingMore: function () {
                    return "نمایش اطلاعات بیشتر...";
                }
            },
            ajax: {
                url: url,
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    currentPage = params.page || 1;
                    if (additionalData !== undefined && additionalData !== null && additionalData !== "null") {
                        return {
                            additionalItem: additionalData,
                            parameter: params.term,
                            currentPage: params.page || 1
                        };
                    }
                    return {
                        parameter: params.term,
                        currentPage: params.page || 1
                    };
                },
                processResults: function (data) {

                    let totalPageCount = data.pageCount;
                    let results = data.entities;
                    if (currentPage === 1) {
                        results.unshift({id: "", text: 'انتخاب کنید...'});
                    }

                    if (totalPageCount > currentPage) {
                        return {
                            results: results,
                            pagination: {
                                more: true
                            }
                        };
                    } else {
                        return {
                            results: results,
                        };
                    }
                },
                cache: true
            },
            dropdownParent: $dropdownParent// Ensures dropdown is within the modal
        });
        $(this).on('select2:opening', function (event) {
            additionalData = event.target.getAttribute("data-select2-additional-item");
        });
    });
}


function reinitializeTemplateComponents() {
    try {
        initializeCkEditor();
        initializeSelect2Components()
        $('[data-bs-toggle="tooltip"]').tooltip();
    } catch {
    }
}

function validateForm(event) {
    $(event.target).data('validator', null);
    $.validator.unobtrusive.parse(event.target);
}

function validateFormByElement(elem) {
    $(elem).removeData('validator');
    $(elem).removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(elem);

    let isValid = $(elem).valid();

    // Prevent form submission if not valid
    if (!isValid) {
        event.preventDefault();
        event.stopPropagation();
    }
}


function getModalSelectorByType(type) {
    let selector;
    switch (type) {
        case "sm": {
            selector = "#modal-center-sm";
            break;
        }
        case "md": {
            selector = "#modal-center-md";
            break;
        }
        case "lg": {
            selector = "#modal-center-lg";
            break;
        }
        case "left": {
            selector = "#modal-left";
            break;
        }
        case "right": {
            selector = "#modal-right";
            break;
        }
    }
    return selector;
}


function getModalDataAttributeByType(type) {
    let attribute;
    switch (type) {
        case "sm": {
            attribute = "data-modal-sm-index";
            break;
        }
        case "md": {
            attribute = "data-modal-md-index";
            break;
        }
        case "lg": {
            attribute = "data-modal-lg-index";
            break;
        }
        case "left": {
            attribute = "data-modal-left-index";
            break;
        }
        case "right": {
            attribute = "data-modal-right-index";
            break;
        }
    }
    return attribute;
}


function setModalBodyIdByType(type, index) {
    let id;
    switch (type) {
        case "sm": {
            id = "modal-center-sm-body-";
            break;
        }
        case "md": {
            id = "modal-center-md-body-";
            break;
        }
        case "lg": {
            id = "modal-center-lg-body-";
            break;
        }
        case "left": {
            id = "modal-center-left-body-";
            break;
        }
        case "right": {
            id = "modal-center-right-body-";
            break;
        }
    }
    return `${id}${index}`;
}

function cloneModal(type, index) {
    let selector = getModalSelectorByType(type);
    index = Number(index);
    if (index === 1) {
        return;
    }
    let modalDataAttribute = getModalDataAttributeByType(type);
    if ($(`[${modalDataAttribute}="${index}"]`).length > 0) {
        return;
    }

    let clonedModal = $(selector).clone(false, false);

    clonedModal.attr("id", "");
    clonedModal.attr(modalDataAttribute, index);
    clonedModal.find(".modal-title").attr("id", "");
    clonedModal.find(".modal-body").attr("id", setModalBodyIdByType(type, index));
    $("body").append(clonedModal);
    let clonedModalZIndex = Number(clonedModal.css("z-index"));
    clonedModal.css("z-index", clonedModalZIndex + index);
}

function openModal(type, title, index) {
    let selector = getModalSelectorByType(type);
    index = Number(index);

    if (index === 1) {
        $(selector).addClass('show');
        $(selector).css("z-index", 1099 + index);
        $(selector).find(".modal-title").html(title);
        return;
    }
    let modal = $(`[${getModalDataAttributeByType(type)}="${index}"]`);
    modal.css("z-index", 1099 + index);
    modal.find(".modal-title").html(title);
    modal.addClass("show");
}


function opSmModal(title, index) {
    openModal("sm", title, index);
}

function opModal(title, index) {
    openModal("md", title, index);
}

function opLgModal(title, index) {
    openModal("lg", title, index);
}


function opLeftModal(title, index) {
    openModal("left", title, index);
}

function opRightModal(title, index) {
    openModal("right", title, index);
}

function showSweetAlert(message, icon) {
    if (icon === null || icon === undefined || icon === '') {
        icon = 'info';
    }
    $(() => {
        Swal.fire({
            icon: icon,
            title: message,
            confirmButtonText: 'باشه'
        });
    })
}

function closeModalByType(type, index) {
    let selector = getModalSelectorByType(type);
    if (index === 1) {
        let selectedModal = $(selector);
        selectedModal.removeClass('show');
        selectedModal.find(".modal-title").html("");
        return;
    }
    let modal = $(`[${getModalDataAttributeByType(type)}="${index}"]`);
    modal.removeClass("show");

}

// function closeModalByElementParent(element) {
//     let modal = $(element).closest(".modal");
//     modal.modal("hide");
// }

function closeModalByElementParent(element) {
    const modal = element.closest('.modal');
    modal.classList.add('hidden');
}

function closeAllModals() {
    let modal = $(".modal").closest(".modal");
    modal.modal("hide");
}


function closeLgModal(index = 1) {
    closeModalByType("lg", index);
}

function closeModal(index = 1) {
    closeModalByType("md", index);
}


function closeSmModal(index = 1) {
    closeModalByType("sm", index);
}

function closeLeftModal(index = 1) {
    closeModalByType("left", index);
}


function closeRightModal(index = 1) {
    closeModalByType("right", index);
}


function ajaxSubmitSuccess(data, status, xhr) {
    showToaster(xhr.responseText , "success");

}

function ajaxSubmitLoginSuccess(data, status, xhr) {
    showToaster(xhr.responseText , "success");

}

async function onAjaxFailure(xhr, status, error) {
    close_waiting();
    const errorMessage = xhr.responseText;
    showToaster(xhr.responseText , "error");
}


$(() => {
    initializeSelect2Components();
})

function initializeTagifyInstances() {
    $("[tagify]").each(function () {
        let tagify = new Tagify(this, {
            delimiters: ",",
        });


        tagify.on('change', function () {
            formatTags(tagify);
        });


        $(this).closest('form').on('submit', function (e) {
            formatTags(tagify);
        });
    });
}

function onMainCategoryChange(elem, id) {
    let value = $(elem).val();
    $("#subCategory").attr("data-select2-additional-item", value).val("").trigger("change");
    $(`#${id}`).val("");
}

function onSubCategoryChange(elem, id) {
    let value = $(elem).val();
    $(`#${id}`).attr("data-select2-additional-item", value).val("").trigger("change");
}

function submitFormToAction(action , formSelector = "#filter-search"){
    const form = $(formSelector);
    if(form){
        const defaultAction =  form.attr("action");
        if(!defaultAction)return;
        form.attr("action", action);
        form.submit();
        form.attr("action"  , defaultAction);
    }
}
function toggleSidebarActiveLink(){
    let path = window.location.pathname;
    let target = $('.side-menu__content .side-menu__link[href="' + path + '"]');
    console.log(path);
    console.log(target);
    $('.side-menu__content .side-menu__link').each(function () {
        $(this).removeClass("side-menu__link--active");
    })
    target.addClass('side-menu__link--active');
}
$(() => {
    toggleSidebarActiveLink();
    addImageInputsChangeDetection();
})

function addImageInputsChangeDetection(){
    $("[ImageInput]").change(function () {
        let x = $(this).attr("ImageInput");
        let submitFormAfterUpload = $(this).attr("SubmitFormAfterUpload");

        if (submitFormAfterUpload !== null && submitFormAfterUpload !== undefined && submitFormAfterUpload !== "") {
            $(`#${submitFormAfterUpload}`).submit();
        } else {
            if (this.files && this.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $("[ImageFile=" + x + "]").attr('src', e.target.result);
                };
                reader.readAsDataURL(this.files[0]);
            }
        }
    });
}