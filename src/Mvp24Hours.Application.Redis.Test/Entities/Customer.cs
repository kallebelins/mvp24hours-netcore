//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using System;

namespace Mvp24Hours.Application.Redis.Test.Entities
{
    public class Customer
    {
        public Guid Oid { get; set; }

        public DateTime Created { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }
    }
}
