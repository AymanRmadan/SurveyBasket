namespace SurveyBasket.Errors
{

    public static class PollErrors
    {
        public static readonly Error PollNotFound =
            new Error("Poll.NotFound", "No Poll was found given Id", StatusCodes.Status404NotFound);

        public static readonly Error DuplicatedPollTitle =
            new Error("Poll.DuplicatedTitle", "Another poll with the same title is already exists", StatusCodes.Status409Conflict);
    }
}
