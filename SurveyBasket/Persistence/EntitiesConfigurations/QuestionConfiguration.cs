using Microsoft.EntityFrameworkCore;

namespace SurveyBasket.Persistence.EntitiesConfigurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasIndex(x => new { x.PollId, x.Content }).IsUnique();

            builder.Property(p => p.Content).HasMaxLength(1000);
        }
    }
}
