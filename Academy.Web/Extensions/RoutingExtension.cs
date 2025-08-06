namespace Academy.Web.Extensions;

public static class RoutingExtension
{
    #region UserPanel

    public static class UserPanel
    {
        private const string BaseUrl = "/account/";

        #region Home

        public static class Home
        {
            public const string Index = BaseUrl + "dashboard";
        }

        #endregion

        #region Ticket

        public static class Ticket
        {
            public const string List = BaseUrl + "tickets";
            public const string Detail = BaseUrl + "ticket-detail/{id}";
            public const string Create = BaseUrl + "create-ticket";
        }

        #endregion

        #region User

        public static class User
        {
            public const string Update = BaseUrl + "edit-profile";
        }
        


        #endregion
    }

    #endregion

    public static class Site
    {
        #region Account
        public static class Account
        {
            public const string Login = "login";
            public const string Logout = "logout";
            public const string ConfirmOtp = "confirm-otp";
            public const string GoogleSignIn = "signin-with-google";
            public const string ForgotPassword = "forgot-password";
            public const string VerifyResetPassword = "verify-reset-password";
            public const string ChangePassword = "change-password";

            public static class Activation
            {
                private const string BaseUrl = "activation";

                public const string Account = BaseUrl + "account";
                public const string Confirm = BaseUrl + "confirm";
            }

            public static class Resend
            {
                private const string BaseUrl = "resend-";

                public const string Otp = BaseUrl + "otp/{mobileOrEmail}";
            }
        }

        #endregion
        
        #region Errors

        public const string NotFound = "/not-found";
        public const string ServerError = "/server-error";

        #endregion
    }
}