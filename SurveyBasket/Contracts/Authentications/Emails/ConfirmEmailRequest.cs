namespace SurveyBasket.Contracts.Authentications.Emails
{
    public record ConfirmEmailRequest(string UserId, string Code);
}
