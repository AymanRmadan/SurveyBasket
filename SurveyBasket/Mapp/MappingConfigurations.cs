﻿using Mapster;
using SurveyBasket.Contracts.Questions;
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
                .Map(dest => dest.Summary, src => src.Summary);

            /* config.NewConfig<QuestionRequest, Question>()
                 .Ignore(nameof(Question.Answers));*/

            config.NewConfig<QuestionRequest, Question>()
                .Map(dest => dest.Answers, src => src.Answers.Select(answer => new Answer { Content = answer }));


            /*config.NewConfig<RegisterRequest, ApplicationUser>()
              .Map(dest => dest.UserName, src => src.Email);*/


        }
    }
}
