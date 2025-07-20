/*using FluentValidation;
using SurveyBasket.Contracts.Answers;

namespace SurveyBasket.Contracts.Validations
{
    public class AnswerRequestValidator : AbstractValidator<AnswerRequest>
    {
        public AnswerRequestValidator()
        {
            //RuleFor(r => r.Content).NotEmpty().Length(3, 1000);

            ////must create 2 answer at least in pull for this question
            //RuleFor(r => r.Answers)
            //    .Must(answer => answer.Count > 1).WithMessage("Question should has at least 2 answer");

            ////don't repeat the answer
            //RuleFor(r => r.Answers)
            //    .Must(answer => answer.Distinct().Count() == answer.Count())
            //    .WithMessage("You Can not add duplicated answer for the same question");


        }

    }
}
*/