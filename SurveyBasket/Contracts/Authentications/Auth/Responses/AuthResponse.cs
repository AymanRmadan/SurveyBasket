namespace SurveyBasket.Contracts.Authentications.Auth.Responses
{
    public record AuthResponse(
        string Id,
        string? Email,
        string FirstName,
        string LastName,
        string Token,
        int ExpireIn,
        string RefreshToken,
        DateTime RefreshTokenExpiration
        );

}
