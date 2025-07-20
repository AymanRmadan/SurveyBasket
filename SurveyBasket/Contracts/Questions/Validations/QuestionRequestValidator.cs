using FluentValidation;
using SurveyBasket.Contracts.Questions;

namespace SurveyBasket.Contracts.Validations
{
    public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
    {
        public QuestionRequestValidator()
        {
            RuleFor(r => r.Content).NotEmpty().Length(3, 1000);


            RuleFor(r => r.Answers)
               .NotNull();
            //must create 2 answer at least in pull for this question
            RuleFor(r => r.Answers)
                .Must(answer => answer.Count > 1)
                .WithMessage("Question should has at least 2 answer")
                .When(answer => answer.Answers != null);

            //don't repeat the answer
            RuleFor(r => r.Answers)
                .NotNull()
                .Must(answer => answer.Distinct().Count() == answer.Count())
                .WithMessage("You Can not add duplicated answer for the same question")
                .When(answer => answer.Answers != null);


        }

    }
}
