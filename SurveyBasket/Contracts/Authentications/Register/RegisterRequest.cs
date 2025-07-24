namespace SurveyBasket.Contracts.Authentications.Register
{
    public record RegisterRequest(
        string Email,
        string Password,
        string FirstName,
        string LastName
        );
}
