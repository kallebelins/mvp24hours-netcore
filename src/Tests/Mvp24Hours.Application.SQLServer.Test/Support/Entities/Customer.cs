//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mvp24Hours.Application.SQLServer.Test.Support.Entities
{
    public class Customer : EntityBase<Customer, int>
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
