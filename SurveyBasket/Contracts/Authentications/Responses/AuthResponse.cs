namespace SurveyBasket.Contracts.Authentications.Responses
{
    public record AuthResponse(
        string Id,
        string? Email,
        string FirstName,
        string LastName,
        string Token,
        int ExpireIn
        );

}
