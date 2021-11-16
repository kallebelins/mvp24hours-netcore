//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mvp24Hours.Patterns.Test.Entities
{
    public class Customer : EntityBase<Customer, int>, IEntityBase
    {
        public Customer()
        {
            Contacts = new List<Contact>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool Active { get; set; }

        // collections

        public ICollection<Contact> Contacts { get; set; }
    }
}
