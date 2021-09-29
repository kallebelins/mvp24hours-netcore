//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using System;

namespace Mvp24Hours.Core.Extensions
{
    public static class GuidExtensions
    {
        public static Guid SafeNewGuid(this Guid oid)
        {
            return oid == Guid.Empty ? Guid.NewGuid() : oid;
        }

        public static Guid ToGuid(this string oid)
        {
            Guid id = Guid.Empty;
            return Guid.TryParse(oid, out id) ? id : Guid.Empty;
        }

        public static bool IsValidGuid(this string oid)
        {
            if (string.IsNullOrEmpty(oid))
            {
                return false;
            }

            Guid id = Guid.Empty;
            return Guid.TryParse(oid, out id);
        }

        public static bool IsNullOrEmpty(this Guid? oid)
        {
            return (oid == null || oid == Guid.Empty);
        }

        public static bool IsEmpty(this Guid oid)
        {
            return (oid == Guid.Empty);
        }
    }
}
