//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
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
        public static async Task<IPagingResult<TDestination>> MapPagingToAsync<TSource, TDestination>(this Task<IPagingResult<TSource>> sourceAsync, IServiceProvider provider)
        {
            var source = await sourceAsync;

            if (source == null)
            {
                return default;
            }

            IMapper mapper = provider.GetService<IMapper>()
                ?? throw new ArgumentException("Profile not registered for AutoMapper.");
            if (source.Messages.AnySafe())
            {
                return mapper
                    .Map<TDestination>(source.Data)
                    .ToBusinessPaging(
                        source.Paging,
                        source.Summary,
                        source.Messages.ToList()
                    );
            }
            else
            {
                return mapper
                    .Map<TDestination>(source.Data)
                    .ToBusinessPaging(
                        source.Paging,
                        source.Summary
                    );
            }
        }

        /// <summary>
        /// Convert instance to mapped object
        /// </summary>
        public static async Task<IBusinessResult<TDestination>> MapBusinessToAsync<TSource, TDestination>(this Task<IBusinessResult<TSource>> sourceAsync, IServiceProvider provider)
        {
            var source = await sourceAsync;

            if (source == null)
            {
                return default;
            }

            IMapper mapper = provider.GetService<IMapper>()
                ?? throw new ArgumentException("Profile not registered for AutoMapper.");
            if (source.Messages.AnySafe())
            {
                return mapper
                    .Map<TDestination>(source.Data)
                    .ToBusiness(source.Messages.ToList());
            }
            else
            {
                return mapper
                    .Map<TDestination>(source.Data)
                    .ToBusiness();
            }
        }
    }
}
