(function ($) {
   
    var originalAjaxSubmit = $.ajax;

    $.ajax = function (options) {
        if (options && options.data && options.data instanceof FormData) {
            options.contentType = false; 
            options.processData = false; 
        }

        return originalAjaxSubmit.call(this, options);
    };

    $(document).ready(function () {
        $(document).on('submit', 'form[data-upload-ajax]', function (event) {
            event.preventDefault();

            var $form = $(this);
            var formData = new FormData(this);
            $.ajax({
                url: $form.attr('data-ajax-url') || $form.attr('action'),
                type: $form.attr('data-ajax-method') || $form.attr('method') || 'GET',
                data: formData,
                contentType: false, 
                processData: false,
                beforeSend: function () {
                    open_waiting();
                },
                success: function (xhr, status, error) {
                    ajaxSubmitSuccess(xhr, status, error);
                    close_waiting();
                },
                error: function (xhr, status, error) {
                    onAjaxFailure(xhr, status, error);
                    close_waiting();
                }

            });
        });
    });
})(jQuery);