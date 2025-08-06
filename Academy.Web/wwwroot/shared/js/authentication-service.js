document.addEventListener("DOMContentLoaded", function () {
    const tabButtons = document.querySelectorAll(".single__tab__link");

    tabButtons.forEach(btn => {
        btn.addEventListener("click", () => {
            const isLoginByPassword = btn.getAttribute("data-loginbypassword") === "true";

            document.querySelectorAll("input[name='IsLoginByPassword']").forEach(input => {
                input.value = isLoginByPassword;
            });
        });
    });
});

$(".confirm-opt-form").on("submit", function (e) {
    let values = [];
    $(".confirm-opt-input").each(function (e) {
        let value = $(this).val();
        if (!value) {
            e.preventDefault();
            showToaster("لطفا کد تایید را وارد کنید", "error")
        }
        values.push(value);
    })
    
    let finalCode = values.toString().trim().replace(/,/g, "");
    if (finalCode) {
        $("#Code").val(finalCode);
    } else {
        e.preventDefault();
    }
})

$(".confirm-opt-input").on("input", function (e) {
    let value = $(this).val();
    if (value) {
        if (!/^\d*$/.test(value)) {
            $(this).val('');
        } else {
            let inputs = $(".confirm-opt-input");
            let currentIndex = inputs.index($(this));
            let nextIndex = currentIndex + 1;
            if (nextIndex > -1 && nextIndex < inputs.length) {
                $(inputs).eq(nextIndex).focus();
            } else {
                $(".confirm-opt-form").submit();
            }
        }   
    }
}).keydown(function (e) {
    if (e.keyCode === 8) {
        $(this).val('');
        let inputs = $(".confirm-opt-input");
        let currentIndex = inputs.index($(this));
        let prevIndex = currentIndex - 1;
        if (prevIndex > -1 && prevIndex < inputs.length) {
            $(this).val('');
            $(inputs).eq(prevIndex).focus();
        } else {
            $(this).val('');
        }
        e.preventDefault();
    }    
});

initialOtp();

function initialOtp() {
    let otpElement = $("#otp-countdown");
    let resendOtpElement = $("#resend-otp");
    countDown(otpElement, (element) => {
        element.html("مدت زمان اعتبار کد به پایان رسید");
        resendOtpElement.removeClass("d-none");
        resendOtpElement.addClass("d-inline-block");
    }, "مانده تا دریافت مجدد کد : ");
}
function successResendOtp(data, status, xhr){
    close_waiting();
    showToaster(data.message, 'success');

    let otpElement = $("#otp-countdown");
    let resendOtpElement = $("#resend-otp");
    
    otpElement.attr("enddate", data.value.otpExpire)
    
    resendOtpElement.removeClass("d-inline-block");
    resendOtpElement.addClass("d-none");
    
    initialOtp();
    
}

