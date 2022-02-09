//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Entities;
using Mvp24Hours.Patterns.Test.Support.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mvp24Hours.Patterns.Test.Support.Entities
{
    public class Contact : EntityBase<Contact, int>
    {
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ContactType Type { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
