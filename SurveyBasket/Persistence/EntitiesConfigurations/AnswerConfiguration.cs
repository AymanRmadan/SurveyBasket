using Microsoft.EntityFrameworkCore;

namespace SurveyBasket.Persistence.EntitiesConfigurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasIndex(x => new { x.QuestionId, x.Content }).IsUnique();
            builder.Property(p => p.Content).HasMaxLength(1000);
        }
    }
}
