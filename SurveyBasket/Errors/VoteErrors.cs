namespace SurveyBasket.Errors
{

    public static class VoteErrors
    {
        public static readonly Error InvalidQuestions =
            new Error("Vote.InvalidQuestions", "Invalid Questions", StatusCodes.Status400BadRequest);

        public static readonly Error DuplicatedVote =
            new Error("Vote.DuplicatedVote", "the user already vote on this poll", StatusCodes.Status409Conflict);
    }
}
