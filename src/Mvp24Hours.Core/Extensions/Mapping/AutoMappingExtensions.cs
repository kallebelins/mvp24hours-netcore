//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using AutoMapper;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Mvp24Hours.Extensions
{
    /// <summary>
    /// 
    /// </summary>
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
            if (source == null)
            {
                return default;
            }

            IMapper mapper = ServiceProviderHelper.GetService<IMapper>();
            if (mapper == null)
            {
                throw new ArgumentException("Profile not registered for AutoMapper.");
            }

            return mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// Convert instance to mapped object
        /// </summary>
        public static IPagingResult<TDestination> MapPagingTo<TSource, TDestination>(this IPagingResult<TSource> source)
        {
            if (source == null)
            {
                return default;
            }

            IMapper mapper = ServiceProviderHelper.GetService<IMapper>();
            if (mapper == null)
            {
                throw new ArgumentException("Profile not registered for AutoMapper.");
            }

            if (source.Messages.AnyOrNotNull())
            {
                return source.Data
                    .MapTo<TDestination>()
                    .ToBusinessPaging(
                        source.Paging,
                        source.Summary,
                        source.Messages?.ToList()
                    );
            }
            else
            {
                return source.Data
                    .MapTo<TDestination>()
                    .ToBusinessPaging(
                        source.Paging,
                        source.Summary
                    );
            }
        }

        /// <summary>
        /// Convert instance to mapped object
        /// </summary>
        public static IBusinessResult<TDestination> MapBusinessTo<TSource, TDestination>(this IBusinessResult<TSource> source)
        {
            if (source == null)
            {
                return default;
            }

            IMapper mapper = ServiceProviderHelper.GetService<IMapper>();
            if (mapper == null)
            {
                throw new ArgumentException("Profile not registered for AutoMapper.");
            }

            if (source.Messages.AnyOrNotNull())
            {
                return source.Data
                    .MapTo<TDestination>()
                    .ToBusiness(source.Messages.ToArray());
            }
            else
            {
                return source.Data
                    .MapTo<TDestination>()
                    .ToBusiness();
            }
        }
    }
}
