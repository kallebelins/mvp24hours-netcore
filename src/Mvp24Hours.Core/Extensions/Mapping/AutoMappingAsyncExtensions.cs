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
using System.Threading.Tasks;

namespace Mvp24Hours.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutoMappingAsyncExtensions
    {
        /// <summary>
        /// Convert instance to mapped object
        /// </summary>
        public static async Task<TDestination> MapToAsync<TSource, TDestination>(this Task<TSource> sourceAsync)
        {
            var source = await sourceAsync;

            if (source == null)
            {
                return default;
            }

            IMapper mapper = ServiceProviderHelper.GetService<IMapper>()
                ?? throw new ArgumentException("Profile not registered for AutoMapper.");
            return mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// Convert instance to mapped object
        /// </summary>
        public static async Task<IPagingResult<TDestination>> MapPagingToAsync<TSource, TDestination>(this Task<IPagingResult<TSource>> sourceAsync)
        {
            var source = await sourceAsync;

            if (source == null)
            {
                return default;
            }

            _ = ServiceProviderHelper.GetService<IMapper>()
                ?? throw new ArgumentException("Profile not registered for AutoMapper.");
            if (source.Messages.AnySafe())
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
        public static async Task<IBusinessResult<TDestination>> MapBusinessToAsync<TSource, TDestination>(this Task<IBusinessResult<TSource>> sourceAsync)
        {
            var source = await sourceAsync;

            if (source == null)
            {
                return default;
            }

            _ = ServiceProviderHelper.GetService<IMapper>()
                ?? throw new ArgumentException("Profile not registered for AutoMapper.");
            if (source.Messages.AnySafe())
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
