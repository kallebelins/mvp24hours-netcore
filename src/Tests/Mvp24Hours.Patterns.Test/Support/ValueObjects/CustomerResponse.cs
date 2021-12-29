using AutoMapper;
using Mvp24Hours.Core.Contract.Mappings;
using Mvp24Hours.Patterns.Test.Support.Entities;

namespace Mvp24Hours.Patterns.Test.Support.ValueObjects
{
    public class CustomerResponse : IMapFrom<Customer>
    {
        public string Name { get; set; }
        public void Mapping(Profile profile) => profile.CreateMap<Customer, CustomerResponse>();
    }
}
