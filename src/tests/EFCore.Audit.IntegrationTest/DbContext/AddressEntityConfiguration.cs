using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Audit.IntegrationTest
{
    public class AddressEntityConfiguration : IEntityTypeConfiguration<AddressEntity>
    {
        public AddressEntityConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<AddressEntity> builder)
        {
            builder.ToTable("Addresses");
            builder.HasKey(x => new { x.PersonId, x.Type });
        }
    }
}
