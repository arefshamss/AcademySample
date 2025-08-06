function setCkeditorThemeForSite(theme) {
    const darkThemeCssLink = $("<link rel=\"stylesheet\" href=\"/Common/lib/ckeditor5/ckeditor5-site-dark.css\" id=\"ckeditor-dark\">");

    switch (theme) {
        case "dark":
            $("#ckeditor").after(darkThemeCssLink);
            break;
        case "light":
            $("#ckeditor-dark").remove();
            break;
    }
}
function showSweetAlert(message, icon) {
    if (icon === null || icon === undefined || icon === '') {
        icon = 'info';
    }
    $(() => {
        Swal.fire({
            icon: icon,
            title: message,
            confirmButtonText: "تایید"
        });
    })
}
function showToaster(message, type, title = null) {

    switch (type) {
        case 'warning': {
            toastr.warning(message, `<strong>${title ?? "هشدار"}</strong>`);
            break;
        }
        case 'info': {
            toastr.info(message, `<strong>${title ?? "اطلاع رسانی"}</strong>`);
            break;
        }
        case 'success': {
            toastr.success(message, `<strong>${title ?? "موفقیت آمیز"}</strong>`);
            break;
        }
        case 'error': {
            toastr.error(message, `<strong>${title ?? "خطا"}</strong>`);
            break;
        }
    }
}


function FillPageId(id) {
    $("#Page").val(id);
    document.getElementById("filter-search").submit();
}
function FillPageIdByFromId(pageId, formId) {
    const form = $(`#${formId}`);
    form.find("[name='Page']").val(pageId);
    const elem = '<button class="d-none submit-hidden" type="submit"></button>';
    form.append(elem);
    form.find(".submit-hidden").click();
}


function toggleOnCheckBoxChanged(input, targetId) {
    if ($(input).is(':checked')) {
        $(targetId).show('slow');
    } else {
        $(targetId).hide('slow');
    }
}
function showOnSelectChange(select, targetId, targetIndex) {
    let value = $(select).val();
    if (value == targetIndex) {
        $(targetId).show('slow');
    } else {
        $(targetId).hide('slow');
    }
}

function countDown(element, onFinish = null, targetElementText = null) {
    let endDateAttr = element.attr("enddate");
    let endDate = new Date(endDateAttr).getTime();
    if (endDate) {
        var dateTimeNow = new Date().getTime();
        var remainingTime = endDate - dateTimeNow;

        if (remainingTime < 1) {
            if (onFinish)
                onFinish(element)

            return;
        }

        var minutes = Math.floor((remainingTime % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((remainingTime % (1000 * 60)) / 1000);

        // Add leading zeros
        var displayMinutes = String(minutes).padStart(2, '0');
        var displaySeconds = String(seconds).padStart(2, '0');

        if (targetElementText)
            element.html(`${targetElementText}${displayMinutes}:${displaySeconds}`);
        else
            element.html(`${displayMinutes}:${displaySeconds}`);

        setTimeout(() => {
            countDown(element, onFinish, targetElementText)
        }, 1000)
    } else {
        console.error("You must set endDate attribute value !")
    }
}
function redirect(url, timeout = null) {
    if (timeout) {
        setTimeout(function () {
            window.location.href = url;
        }, Number(timeout));
    } else {
        window.location.href = url;
    }
}


//#region Update Avatar

let inputAvatarFile = $("input[type='file']#Avatar");
inputAvatarFile.on("change", function (e) {
    $("#UpdateAvatarForm").submit();
})

function successUpdateAvatar(data) {
    let avatarName = data.value.avatarName;
    let message = data.value.message;

    $("img#AvatarImage").attr("src", avatarName);
    showToaster(message, "success");

    let deleteAvatarButton = `<a class="*:text-red-400 flex" 
                                        href="/UserPanel/User/DeleteAvatar"
                                        data-ajax="true"
                                        data-ajax-method="get"
                                        data-ajax-mode="replace"
                                        data-ajax-success="close_waiting();successDeleteAvatar(data);"
                                        data-ajax-failure="onAjaxFailure(xhr, status, error);"
                                        data-ajax-begin="open_waiting();"
                                        onclick="showConfirmableAlert(event, 'حذف تصویر پروفایل', 'آیا از حذف تصویر پروفایل خود مطمئن هستید ؟')"
                                        id="delete-avatar">
                    <iconify-icon icon="tabler:trash" width="24" height="24"></iconify-icon>
                </a>`;
    let avatarActions = $("#avatar-actions");
    if (avatarActions.find("#delete-avatar").length < 1) {
        avatarActions.append(deleteAvatarButton);
    }

    inputAvatarFile.val('');
}


function successDeleteAvatar(data) {
    let avatarName = data.value.avatarName;
    let message = data.value.message;

    $("img#AvatarImage").attr("src", avatarName);
    showToaster(message, "success");

    $("#delete-avatar").remove();
}

//#endregion