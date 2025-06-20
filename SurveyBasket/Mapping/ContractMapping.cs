namespace SurveyBasket.Mapping
{
    // // This is Manual Mappining
    public static class ContractMapping
    {
        /* // This is Manual Mappining

         // Convert from Poll To PollResponse
         public static PollResponse MapToPollResponse(this Poll poll)
         {
             return new PollResponse()
             {
                 Id = poll.Id,
                 Title = poll.Title,
                 Description = poll.Description,
             };
         }*/


        /* public static IEnumerable<PollResponse> MapToPollResponse(this IEnumerable<Poll> poll)
         {
             return poll.Select(MapToPollResponse);

         }*/


        // Convert from PollResponse To Poll
        /* public static Poll MapToPoll(this CreatePollRequest poll)
         {
             return new Poll()
             {
                 Title = poll.Description,
                 Description = poll.Description,
             };
         }*/
    }
}
