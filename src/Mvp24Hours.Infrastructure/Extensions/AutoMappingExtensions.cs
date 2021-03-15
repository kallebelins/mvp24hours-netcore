using AutoMapper;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.Linq.Expressions;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class AutoMappingExtensions
    {
        /// <summary>
        /// Maps properties as ignored.
        /// </summary>
        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, object>> selector)
        {
            map.ForMember(selector, config => config.Ignore());

            return map;
        }

        /// <summary>
        /// Sets mapping from source property to destination property. Convenient extension method. 
        /// </summary>
        public static IMappingExpression<TSource, TDestination> MapProperty<TSource, TDestination, TProperty>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TSource, TProperty>> sourceMember,
            Expression<Func<TDestination, object>> targetMember)
        {
            map.ForMember(targetMember, opt => opt.MapFrom(sourceMember));

            return map;
        }

        /// <summary>
        /// Convert instance to mapped object
        /// </summary>
        public static TDestination MapTo<TDestination>(this object source)
        {
            IMapper mapper = ServiceProviderHelper.GetService<IMapper>();
            if (mapper == null)
                throw new ArgumentNullException("Profile not registered for AutoMapper.");
            return mapper.Map<TDestination>(source);
        }
    }
}
