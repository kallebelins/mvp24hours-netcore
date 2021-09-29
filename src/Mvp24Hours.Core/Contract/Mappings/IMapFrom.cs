//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using AutoMapper;

namespace Mvp24Hours.Core.Contract.Mappings
{
    /// <summary>
    /// Dynamic service mapping contract
    /// </summary>
    public interface IMapFrom<T>
    {
        /// <summary>
        /// Apply mapping
        /// </summary>
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
