﻿namespace SurveyBasket.Errors
{
    public static class UserErrors
    {
        public static readonly Error InvalidCredentials =
            new Error("User.InvalidCreddntials", "Invalid email / password", StatusCodes.Status401Unauthorized);

        public static readonly Error DisabledUser =
        new("User.DisabledUser", "Disabled user, please contact your administrator", StatusCodes.Status401Unauthorized);

        public static readonly Error LockedUser =
        new("User.LockedUser", "Locked user, please contact your administrator", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidJwtToken =
            new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidRefreshToken =
            new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);

        public static readonly Error DuplicatedEmail =
            new("User.DuplicatedEmail", "Another user use the same emails", StatusCodes.Status409Conflict);


        public static readonly Error EmailNotConfirmed =
            new("User.EmailNotConfirmed", "Email is not confirmed", StatusCodes.Status401Unauthorized);


        public static readonly Error InvalidCode =
            new("User.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);

        public static readonly Error DuplicatedConfirmation =
            new("User.DuplicatedConfirmation", "Email aleady confirmed", StatusCodes.Status400BadRequest);


    }
}
