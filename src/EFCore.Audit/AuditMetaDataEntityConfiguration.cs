using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Audit
{
    public class AuditMetaDataEntityConfiguration : IEntityTypeConfiguration<AuditMetaDataEntity>
    {
        public AuditMetaDataEntityConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<AuditMetaDataEntity> builder)
        {
            builder.ToTable("AuditMetaDatas");
            builder.HasKey(x => new { x.HashPrimaryKey, x.SchemaTable });
        }
    }
}
