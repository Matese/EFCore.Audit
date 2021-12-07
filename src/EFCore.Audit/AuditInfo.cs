using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace EFCore.Audit
{
    /// <summary>
    /// Used to label which classes are auditable and wich properties are not.
    /// </summary>
    /// <remarks>
    /// This class substitutes the need for annotating entities with <see cref="AuditableAttribute"/> and 
    /// <see cref="NotAuditableAttribute"/> attributes through
    /// <see cref="AuditExtensions.Auditable{TEntity}(EntityTypeBuilder{TEntity})"/> and
    /// <see cref="AuditExtensions.NotAuditable{TEntity}(EntityTypeBuilder{TEntity}, Expression{Func{TEntity, object}})"/>
    /// extensions.
    /// </remarks>
    internal class AuditInfo
    {
        private readonly Type auditableType;
        private readonly List<PropertyInfo> notAuditableProperties;

        public AuditInfo(Type auditableType)
        {
            this.notAuditableProperties = new List<PropertyInfo>();
            this.auditableType = auditableType;
        }

        public void AddNotAuditableProperty(PropertyInfo notAuditableProperty)
        {
            if (!this.IsNotAuditableProperty(notAuditableProperty))
                this.notAuditableProperties.Add(notAuditableProperty);
        }

        public bool IsAuditableType(Type type)
                    => this.auditableType == type;

        public bool IsNotAuditableProperty(PropertyInfo notAuditableProperty)
            => this.notAuditableProperties.Contains(notAuditableProperty);
        
        public override string ToString()
        {
            return this.auditableType.FullName;
        }
    }
}
