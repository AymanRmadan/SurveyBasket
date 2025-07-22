using FluentValidation;
using SurveyBasket.Contracts.Votes.Requests;

namespace SurveyBasket.Contracts.Validations
{
    public class VoteAnswerRequestValidator : AbstractValidator<VoteAnswerRequest>
    {
        public VoteAnswerRequestValidator()
        {
            RuleFor(r => r.QuestionId).GreaterThan(0);
            RuleFor(r => r.AnswerId).GreaterThan(0);
        }

    }
}
