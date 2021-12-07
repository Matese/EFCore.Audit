using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCore.Audit
{
    /// <summary>
    /// A cache of <see cref="AuditInfo"/>, used to allow audit configuration through
    /// <see cref="EntityTypeBuilder{TEntity}"/> extension.
    /// </summary>
    /// <remarks>
    /// For references about the singleton pattern, look
    /// <see href="https://stackoverflow.com/questions/12316406/thread-safe-c-sharp-singleton-pattern">here</see>
    /// and <see href="https://csharpindepth.com/Articles/Singleton">here</see>
    /// </remarks>
    internal sealed class AuditInfoCache
    {
        private static readonly Lazy<AuditInfoCache> lazy = new(() => new AuditInfoCache());
        private readonly Annotatable annotatable = new();

        private AuditInfoCache()
        {
        }

        public static AuditInfoCache Instance { get => lazy.Value; }

        /// <summary>
        /// Adds an <see cref="AuditInfo"/>. Throws if an <see cref="AuditInfo"/> already exists.
        /// </summary>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The newly added <see cref="AuditInfo"/>.</returns>
        public AuditInfo AddInfo(AuditInfo value)
            => this.annotatable.AddAnnotation(value.ToString(), value).Value as AuditInfo;

        /// <summary>
        /// Gets the <see cref="AuditInfo"/> with the given key, returning null if it does not exist.
        /// </summary>
        /// <param name="type">The key of the <see cref="AuditInfo"/> to find.</param>
        /// <returns>The existing <see cref="AuditInfo"/> if already exists. Otherwise, null.</returns>
        public AuditInfo FindInfo(Type type)
        {
            var annotation = this.annotatable.FindAnnotation(type.FullName);
            return annotation != null ? annotation.Value as AuditInfo : null;
        }

        /// <summary>
        /// Gets the <see cref="AuditInfo"/> with the given key, throwing if it does not exist.
        /// </summary>
        /// <param name="type">The key of the <see cref="AuditInfo"/> to find.</param>
        /// <returns>The <see cref="AuditInfo"/> with the specified key.</returns>
        public AuditInfo GetInfo(Type type)
            => (AuditInfo)this.annotatable.GetAnnotation(type.FullName).Value;
        
        /// <summary>
        /// Gets all infos on the current object.
        /// </summary>
        /// <returns>All the infos on the current object.</returns>
        public IEnumerable<AuditInfo> GetInfos()
            => this.annotatable.GetAnnotations().Select(w => (AuditInfo)w.Value);
    }
}
