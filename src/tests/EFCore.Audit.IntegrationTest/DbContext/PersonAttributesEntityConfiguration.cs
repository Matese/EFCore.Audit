using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Audit.IntegrationTest
{
    public class PersonAttributesEntityConfiguration : IEntityTypeConfiguration<PersonAttributesEntity>
    {
        public PersonAttributesEntityConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<PersonAttributesEntity> builder)
        {
            builder.ToTable("PersonAttributes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Auditable().NotAuditable(cx => cx.DummyString);
        }
    }
}
