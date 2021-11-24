//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.Enums.Infrastructure;
using Mvp24Hours.Infrastructure.Helpers;
using System.Collections.Generic;

namespace Mvp24Hours.WebAPI.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class HateoasExtensions
    {
        #region [ Item ]

        public static IHateoasContext AddLinkItem(this IHateoasContext result, string routeName)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, new Dictionary<string, object>(), HateoasType.Item, true, HateoasMethodType.GET);
                if (link != null)
                {
                    result.AddLink(link);
                }
            }
            return result;
        }

        public static IHateoasContext AddLinkItem(this IHateoasContext result, string routeName, object routeValues)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Item, true, HateoasMethodType.GET);
                if (link != null)
                {
                    result.AddLink(link);
                }
            }
            return result;
        }

        public static IHateoasContext AddLinkItem(this IHateoasContext result, string routeName, object routeValues, HateoasMethodType method)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Item, true, method);
                if (link != null)
                {
                    result.AddLink(link);
                }
            }
            return result;
        }

        #endregion

        #region [ Create ]

        public static IHateoasContext AddLinkItemCreate(this IHateoasContext result, string routeName)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, new Dictionary<string, object>(), HateoasType.Create, true, HateoasMethodType.POST);
                if (link != null)
                {
                    result.AddLink(link);
                }
            }
            return result;
        }

        public static IHateoasContext AddLinkItemCreate(this IHateoasContext result, string routeName, object routeValues)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Create, true, HateoasMethodType.POST);
                if (link != null)
                {
                    result.AddLink(link);
                }
            }
            return result;
        }

        public static IHateoasContext AddLinkItemCreate(this IHateoasContext result, string routeName, object routeValues, HateoasMethodType method)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Create, true, method);
                if (link != null)
                {
                    result.AddLink(link);
                }
            }
            return result;
        }

        #endregion

        #region [ Edit ]

        public static IHateoasContext AddLinkItemEdit(this IHateoasContext result, string routeName)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, new Dictionary<string, object>(), HateoasType.Edit, true, HateoasMethodType.PUT);
                if (link != null)
                {
                    result.AddLink(link);
                }
            }
            return result;
        }

        public static IHateoasContext AddLinkItemEdit(this IHateoasContext result, string routeName, object routeValues)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Edit, true, HateoasMethodType.PUT);
                if (link != null)
                {
                    result.AddLink(link);
                }
            }
            return result;
        }

        public static IHateoasContext AddLinkItemEdit(this IHateoasContext result, string routeName, object routeValues, HateoasMethodType method)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Edit, true, method);
                if (link != null)
                {
                    result.AddLink(link);
                }
            }
            return result;
        }

        #endregion

        #region [ Delete ]

        public static IHateoasContext AddLinkItemDelete(this IHateoasContext result, string routeName)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, new Dictionary<string, object>(), HateoasType.Delete, true, HateoasMethodType.DELETE);
                if (link != null)
                {
                    result.AddLink(link);
                }
            }
            return result;
        }

        public static IHateoasContext AddLinkItemDelete(this IHateoasContext result, string routeName, object routeValues)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Delete, true, HateoasMethodType.DELETE);
                if (link != null)
                {
                    result.AddLink(link);
                }
            }
            return result;
        }

        public static IHateoasContext AddLinkItemDelete(this IHateoasContext result, string routeName, object routeValues, HateoasMethodType method)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Delete, true, method);
                if (link != null)
                {
                    result.AddLink(link);
                }
            }
            return result;
        }

        #endregion

        #region [ Self ]

        public static IHateoasContext AddLinkSelf(this IHateoasContext result, string routeName)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, new Dictionary<string, object>(), HateoasType.Self, false, HateoasMethodType.GET);
                if (link != null)
                {
                    result.AddLink(link);
                }
            }
            return result;
        }

        public static IHateoasContext AddLinkSelf(this IHateoasContext context, string routeName, object routeValues)
        {
            if (context != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Self, false, HateoasMethodType.GET);
                if (link != null)
                {
                    context.AddLink(link);
                }
            }
            return context;
        }

        public static IHateoasContext AddLinkSelf(this IHateoasContext context, string routeName, object routeValues, HateoasMethodType method)
        {
            if (context != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Self, false, method);
                if (link != null)
                {
                    context.AddLink(link);
                }
            }
            return context;
        }

        public static IHateoasContext AddLinkSelf<T>(this IHateoasContext context, IPagingResult<T> result, string routeName, IPagingCriteria Paging)
        {
            if (context != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, result, Paging, HateoasType.Self, HateoasMethodType.GET);
                if (link != null)
                {
                    context.AddLink(link);
                }
            }
            return context;
        }

        public static IHateoasContext AddLinkSelf<T>(this IHateoasContext context, IPagingResult<T> result, string routeName, IPagingCriteria Paging, HateoasMethodType method)
        {
            if (context != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, result, Paging, HateoasType.Self, method);
                if (link != null)
                {
                    context.AddLink(link);
                }
            }
            return context;
        }

        #endregion

        #region [ Collection ]

        public static IHateoasContext AddLinkCollection(this IHateoasContext result, string routeName)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, new Dictionary<string, object>(), HateoasType.Collection, true, HateoasMethodType.GET);
                if (link != null)
                {
                    result.AddLink(link);
                }
            }
            return result;
        }

        public static IHateoasContext AddLinkCollection(this IHateoasContext result, string routeName, object routeValues)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Collection, true, HateoasMethodType.GET);
                if (link != null)
                {
                    result.AddLink(link);
                }
            }
            return result;
        }

        public static IHateoasContext AddLinkCollection(this IHateoasContext result, string routeName, object routeValues, HateoasMethodType method)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Collection, true, method);
                if (link != null)
                {
                    result.AddLink(link);
                }
            }
            return result;
        }

        public static IHateoasContext AddLinkCollection<T>(this IHateoasContext context, IPagingResult<T> result, string routeName, IPagingCriteria Paging)
        {
            if (context != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, result, Paging, HateoasType.Collection, HateoasMethodType.GET);
                if (link != null)
                {
                    context.AddLink(link);
                }
            }
            return context;
        }

        public static IHateoasContext AddLinkCollection<T>(this IHateoasContext context, IPagingResult<T> result, string routeName, IPagingCriteria Paging, HateoasMethodType method)
        {
            if (context != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, result, Paging, HateoasType.Collection, method);
                if (link != null)
                {
                    context.AddLink(link);
                }
            }
            return context;
        }

        #endregion

        #region [ Paging ]

        public static IHateoasContext AddLinkPaging<T>(this IHateoasContext context, IPagingResult<T> result, string routeName, IPagingCriteria Paging)
        {
            AddLinkPaging(context, result, routeName, Paging, HateoasMethodType.GET);
            return context;
        }

        public static IHateoasContext AddLinkPaging<T>(this IHateoasContext context, IPagingResult<T> result, string routeName, IPagingCriteria Paging, HateoasMethodType method)
        {
            if (context == null)
            {
                return context;
            }

            var link = HateoasHelper.CreateLink(routeName, result, Paging, HateoasType.First, method);
            if (link != null)
            {
                context.AddLink(link);
            }

            link = HateoasHelper.CreateLink(routeName, result, Paging, HateoasType.Previous, method);
            if (link != null)
            {
                context.AddLink(link);
            }

            link = HateoasHelper.CreateLink(routeName, result, Paging, HateoasType.Next, method);
            if (link != null)
            {
                context.AddLink(link);
            }

            link = HateoasHelper.CreateLink(routeName, result, Paging, HateoasType.Last, method);
            if (link != null)
            {
                context.AddLink(link);
            }

            return context;
        }

        #endregion
    }
}
