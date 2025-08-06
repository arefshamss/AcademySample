

function ConfirmDelete(element, requestUrl, formId = null , withTitle = true) {
    Swal.fire({
        title: "حذف",
        text: "آیا از حذف مطمئن هستید ؟",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "تایید",
        cancelButtonText: "لغو",
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: requestUrl,
                type: "Get",
                beforeSend: function () {
                    open_waiting();
                },
                success: function (response) {
                    close_waiting();
                    let trElement = $(element).closest('tr');
                    let tdElement = $(element).closest("td");

                    trElement.addClass("removed");
                    tdElement.find('.removeAfterDelete')
                        .addClass("hidden");

                    $(element).removeClass("text-danger");
                    $(element).addClass("text-success");
                    if (withTitle) {
                        $(element).html('<i data-tw-merge="" data-lucide="undo" class="h-4 w-4"></i>');
                    }else{
                        $(element).html('<i data-tw-merge="" data-lucide="undo" class="h-4 w-4"></i>');
                    }
                    $(element).attr("data-bs-original-title", "بازگردانی")
                    if(formId === null){
                        $(element).attr("onclick", `ConfirmRecover(this, '${requestUrl}'  , null , ${withTitle})`)
                    }else{
                        $(element).attr("onclick", `ConfirmRecover(this, '${requestUrl}'  , '${formId}' , ${withTitle})`)
                    }

                    $(element)
                        .removeClass("text-danger")
                        .addClass("text-info");

                    if (formId) {
                        ajaxSubstitutionFormId(formId);
                    }
                    showToaster(response, "success");
                    lucide.createIcons();
                },
                error: function (xhr) {
                    close_waiting();
                    showToaster(response.message, "error");
                }
            });
        }
    });
}

function ConfirmRecover(element, requestUrl, formId = null, withTitle = true) {
    Swal.fire({
        title: "بازگردانی",
        text: "آیا از بازگردانی مطمئن هستید ؟",
        showDenyButton: true,
        icon: 'warning',
        confirmButtonText: "تایید",
        denyButtonText: "لغو"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: requestUrl,
                type: "get",
                beforeSend: function () {
                    open_waiting();
                },
                success: function (response) {
                    close_waiting();
                    
                    let trElement = $(element).closest('tr');
                    let tdElement = $(element).closest("td");

                    trElement.removeClass("removed");
                    tdElement.find('.removeAfterDelete')
                        .removeClass("hidden");

                    $(element).removeClass("text-success");
                    $(element).addClass("text-danger");
                    if(withTitle){
                        $(element).html('<i data-tw-merge="" data-lucide="trash2" class="h-4 w-4"></i>');
                    }else{
                        $(element).html('<i data-tw-merge="" data-lucide="trash2" class="h-4 w-4"></i>');
                    }

                    $(element).attr("data-bs-original-title", "حذف");
                    if(formId === null){
                        $(element).attr("onclick", `ConfirmDelete(this, '${requestUrl}' , null , ${withTitle})`)
                    }else{
                        $(element).attr("onclick", `ConfirmDelete(this, '${requestUrl}' , '${formId}' , ${withTitle})`)
                    }

                    $(element)
                        .removeClass("text-info")
                        .addClass("text-danger");

                    if (formId) {
                        ajaxSubstitutionFormId(formId);
                    }
                    showToaster(response, "success");

                    lucide.createIcons();
                    close_waiting();
                },
                error: function (err) {
                    close_waiting();
                    showToaster(err.responseText, "error");
                }
            });
        }
    });
}


function ConfirmHardDelete(element, requestUrl, formId = null) {    
    Swal.fire({
        title: "حذف",
        text: "آیا از حذف مطمئن هستید ؟",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "تایید",
        cancelButtonText: "لغو",
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: requestUrl,
                type: "Get",
                beforeSend: function () {
                    open_waiting();
                },
                success: function (response) {
                    close_waiting();
                    let trElement = $(element).closest('tr');
                    let tdElement = $(element).closest("td");

                    trElement.addClass("removed");
                    tdElement.find('.removeAfterDelete')
                        .addClass("hidden");
                    
                    $(element).remove();
                    if (formId) {
                        ajaxSubstitutionFormId(formId);
                    }
                    showToaster(response, "success");
                },
                error: function (xhr) {
                    close_waiting();
                    showToaster(response.message, "error");
                }
            });
        }
    });
}

//TODO: Fix this part