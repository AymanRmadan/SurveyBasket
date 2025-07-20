namespace SurveyBasket.Errors
{

    public static class QuestionErrors
    {
        public static readonly Error QuestionNotFound =
            new Error("Question.NotFound", "No Question was found given Id", StatusCodes.Status404NotFound);

        public static readonly Error DuplicatedQuestionContent =
            new Error("Question.DuplicatedQuestionContent", "Another Question with the same Content is already exists", StatusCodes.Status409Conflict);
    }
}
