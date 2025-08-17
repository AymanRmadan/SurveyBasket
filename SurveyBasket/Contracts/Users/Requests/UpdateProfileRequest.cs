namespace SurveyBasket.Contracts.Users.Requests
{
    public record UpdateProfileRequest(
        string FirstName,
        string LastName
    );
}
