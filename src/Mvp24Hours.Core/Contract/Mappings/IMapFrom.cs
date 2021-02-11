using AutoMapper;

namespace Mvp24Hours.Core.Contract.Mappings
{
    /// <summary>
    /// Dynamic service mapping contract
    /// </summary>
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
