namespace SurveyBasket.Services.Notifications;

public interface INotificationService
{
    Task SendNewPollsNotification(int? pollId = null);
}