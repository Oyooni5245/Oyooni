using Oyooni.Server.Commands.Accounts;
using Oyooni.Server.Commands.AI;
using Oyooni.Server.Commands.AvailableTimes;
using Oyooni.Server.Constants;
using Oyooni.Server.Requests.Accounts;
using Oyooni.Server.Requests.AI;
using Oyooni.Server.Requests.AvailableTimes;
using System;

namespace Oyooni.Server.Extensions
{
    /// <summary>
    /// Extensions to map requests to mediator requests
    /// </summary>
    public static class MediatorRequestsMappingsExtensions
    {
        /// <summary>
        /// Maps <see cref="LoginRequest"/> to <see cref="Login.Request"/>
        /// </summary>
        public static Login.Request ToMediatorRequest(this LoginRequest request)
            => new Login.Request(request.Email, request.Password);

        /// <summary>
        /// Maps <see cref="RefreshTokenRequest"/> to <see cref="RefreshUserToken.Request"/>
        /// </summary>
        public static RefreshUserToken.Request ToMediatorRequest(this RefreshTokenRequest request)
            => new RefreshUserToken.Request(request.Token, request.RefreshToken);

        /// <summary>
        /// Maps <see cref="SignupRequest"/> to <see cref="Signup.Request"/>
        /// </summary>
        public static Signup.Request ToMediatorRequest(this SignupRequest request)
            => new Signup.Request(request.FirstName, request.LastName, request.Email, request.Password);

        /// <summary>
        /// Maps <see cref="EditProfileRequest"/> to <see cref="EditProfile.Request"/>
        /// </summary>
        public static EditProfile.Request ToMediatorRequest(this EditProfileRequest request)
            => new EditProfile.Request(request.FirstName, request.LastName);

        /// <summary>
        /// Maps <see cref="AddAvailableTimeRequest"/> to <see cref="AddAvailableTime.Request"/>
        /// </summary>
        public static AddAvailableTime.Request ToMediatorRequest(this AddAvailableTimeRequest request)
            => new AddAvailableTime.Request((DayOfWeek)request.DayOfWeekId, request.From, request.To);

        /// <summary>
        /// Maps <see cref="RecognizeBankNotetRequest"/> to <see cref="RecognizeBankNote.Request"/>
        /// </summary>
        public static RecognizeBankNote.Request ToMediatorRequest(this RecognizeBankNotetRequest request)
            => new RecognizeBankNote.Request(request.File);

        /// <summary>
        /// Maps <see cref="RecognizeColorRequest"/> to <see cref="RecognizeColors.Request"/>
        /// </summary>
        public static RecognizeColors.Request ToMediatorRequest(this RecognizeColorRequest request)
            => new RecognizeColors.Request(request.File, request.NumberOfColorsToDetect);

        /// <summary>
        /// Maps <see cref="ChangePasswordRequest"/> to <see cref="ChangePassword.Request"/>
        /// </summary>
        public static ChangePassword.Request ToMediatorRequest(this ChangePasswordRequest request)
            => new ChangePassword.Request(request.OldPassword, request.NewPassword);

        /// <summary>
        /// Maps <see cref="CaptionImageRequest"/> to <see cref="CaptionImage.Request"/>
        /// </summary>
        public static CaptionImage.Request ToMediatorRequest(this CaptionImageRequest request)
            => new CaptionImage.Request(request.File);

        /// <summary>
        /// Maps <see cref="RecognizeTextRequest"/> to <see cref="RecognizeText.Request"/>
        /// </summary>
        public static RecognizeText.Request ToMediatorRequest(this RecognizeTextRequest request)
            => new RecognizeText.Request(request.File);
    }
}
