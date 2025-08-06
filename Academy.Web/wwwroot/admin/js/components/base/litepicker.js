(() => {
    (function () {
        "use strict";
        $(".datepicker").each(function () {
            let e = {
                autoApply: false,
                singleMode: false,
                numberOfColumns: 2,
                numberOfMonths: 2,
                showWeekNumbers: true,
                format: "YYYY/MM/DD",
                dropdowns: {
                    minYear: 1990,
                    maxYear: null,
                    months: true,
                    years: true
                },
                lang: document.documentElement.lang,
                buttonText: {
                    apply: document.documentElement.lang === "fa" ? "انتخاب" : "Apply",
                    cancel: document.documentElement.lang === "fa" ? "کنسل" : "Cancel"
                }
            };

            if ($(this).data("single-mode")) {
                e.singleMode = true;
                e.numberOfColumns = 1;
                e.numberOfMonths = 1;
            }

            if ($(this).data("format")) {
                e.format = $(this).data("format");
            }

            new Litepicker({
                element: this,
                ...e
            });
        });
    })();
})();
