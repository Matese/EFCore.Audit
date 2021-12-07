using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace EFCore.Audit
{
    public static class AuditExtensions
    {
        public static EntityTypeBuilder<TEntity> Auditable<TEntity>(
            this EntityTypeBuilder<TEntity> builder) where TEntity : class
        {
            var type = typeof(TEntity);

            var info = AuditInfoCache.Instance
                .FindInfo(type);

            if (info == null)
            {
                AuditInfoCache.Instance
                   .AddInfo(new AuditInfo(type));
            }

            return builder;
        }

        public static EntityTypeBuilder<TEntity> NotAuditable<TEntity>(
            this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, object>> expression) where TEntity : class
        {
            if (expression.IsProperty())
            {
                builder = builder
                    .Auditable();

                var type = typeof(TEntity);

                var info = AuditInfoCache.Instance
                    .GetInfo(type);

                info.AddNotAuditableProperty(expression.GetProperty());
            }

            return builder;
        }

        public static Guid ToGuidHash(this string readablePrimaryKey)
        {
            using (var sha512 = SHA512.Create())
            {
                byte[] hashValue = sha512
                    .ComputeHash(Encoding.Default.GetBytes(readablePrimaryKey));

                byte[] reducedHashValue = new byte[16];

                for (int i = 0; i < 16; i++)
                    reducedHashValue[i] = hashValue[i * 4];

                return new Guid(reducedHashValue);
            }
        }

        public static string ToReadablePrimaryKey(this EntityEntry entry)
        {
            var primaryKey = entry.Metadata
                .FindPrimaryKey();

            if (primaryKey == null)
                return null;

            var dictionary = primaryKey.Properties
                .ToDictionary(x => x.Name, x => x.PropertyInfo
                    .GetValue(entry.Entity));

            var readablePrimaryKey = string
                .Join(",", dictionary
                    .Select(x => x.Key + "=" + x.Value));

            return readablePrimaryKey;
        }

        internal static bool IsAuditable(this EntityEntry entityEntry)
        {
            var entityType = entityEntry.Entity
                .GetType();

            var isAuditable = Attribute
                .GetCustomAttribute(entityType, typeof(AuditableAttribute)) != null;

            if (isAuditable)
                return true;

            var info = AuditInfoCache.Instance
                .FindInfo(entityType);

            isAuditable = info != null && info
                .IsAuditableType(entityType);

            return isAuditable;
        }

        internal static bool IsAuditable(this PropertyEntry propertyEntry)
        {
            var entityType = propertyEntry.EntityEntry.Entity
                .GetType();

            var propertyInfo = entityType.
                GetProperty(propertyEntry.Metadata.Name);

            var isNotAuditable = Attribute
                .IsDefined(propertyInfo, typeof(NotAuditableAttribute));

            if (!isNotAuditable)
            {
                var info = AuditInfoCache.Instance
                    .FindInfo(entityType);

                isNotAuditable = info != null && info
                    .IsNotAuditableProperty(propertyInfo);
            }

            var isAuditable = IsAuditable(propertyEntry.EntityEntry) && !isNotAuditable;

            return isAuditable;
        }

        internal static bool ShouldBeAudited(this EntityEntry entry)
        {
            var shouldBeAudited =
                entry.State != EntityState.Detached &&
                entry.State != EntityState.Unchanged &&
                !(entry.Entity is AuditEntity) &&
                !(entry.Entity is AuditMetaDataEntity) &&
                entry.IsAuditable();

            return shouldBeAudited;
        }

        internal static List<AuditEntry> ToBeAudited(
            this IEnumerable<EntityEntry> entries, IAuditUserProvider provider)
        {
            var auditEntries = new List<AuditEntry>();

            foreach (EntityEntry entry in entries)
            {
                if (!entry.ShouldBeAudited())
                    continue;

                auditEntries.Add(new AuditEntry(entry, provider));
            }

            return auditEntries;
        }
    }
}
