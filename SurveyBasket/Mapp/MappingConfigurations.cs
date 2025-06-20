using Mapster;
using SurveyBasket.Contracts.Responses;

namespace SurveyBasket.Mapp
{
    public class MappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //if there is prop in response is not the same name in model use this way
            // var config = new TypeAdapterConfig();
            config.NewConfig<Poll, PollResponse>()
                .Map(dest => dest.Notes, src => src.Description);
        }
    }
}
