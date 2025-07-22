using FluentValidation;
using SurveyBasket.Contracts.Requests;

namespace SurveyBasket.Contracts.Validations
{
    public class VoteRequestValidator : AbstractValidator<VoteRequest>
    {
        public VoteRequestValidator()
        {
            RuleFor(r => r.Answers).NotEmpty();

            RuleForEach(a => a.Answers)
                .SetInheritanceValidator(v => v.Add(new VoteAnswerRequestValidator()));
        }

    }
}
