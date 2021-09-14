using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Audit.IntegrationTest
{
    public class PersonEntityConfiguration : IEntityTypeConfiguration<PersonEntity>
    {
        public PersonEntityConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<PersonEntity> builder)
        {
            builder.ToTable("Persons");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
