namespace SurveyBasket.Contracts.Authentications.Register
{
    public record AddRegisterRequest(
        string Email,
        string Password,
        string FirstName,
        string LastName
        );
}
