//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.Enums.Infrastructure;
using Mvp24Hours.Infrastructure.Helpers;
using System.Collections.Generic;

namespace Mvp24Hours.WebAPI.Extensions
{
    public static class HateoasExtensions
    {
        #region [ Item ]

        public static IBusinessResult<T> AddLinkItem<T>(this IBusinessResult<T> result, string routeName)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, new Dictionary<string, object>(), HateoasType.Item, true, HateoasMethodType.GET);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        public static IBusinessResult<T> AddLinkItem<T>(this IBusinessResult<T> result, string routeName, object routeValues)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Item, true, HateoasMethodType.GET);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        public static IBusinessResult<T> AddLinkItem<T>(this IBusinessResult<T> result, string routeName, object routeValues, HateoasMethodType method)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Item, true, method);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        #endregion

        #region [ Create ]

        public static IBusinessResult<T> AddLinkItemCreate<T>(this IBusinessResult<T> result, string routeName)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, new Dictionary<string, object>(), HateoasType.Create, true, HateoasMethodType.POST);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        public static IBusinessResult<T> AddLinkItemCreate<T>(this IBusinessResult<T> result, string routeName, object routeValues)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Create, true, HateoasMethodType.POST);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        public static IBusinessResult<T> AddLinkItemCreate<T>(this IBusinessResult<T> result, string routeName, object routeValues, HateoasMethodType method)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Create, true, method);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        #endregion

        #region [ Edit ]

        public static IBusinessResult<T> AddLinkItemEdit<T>(this IBusinessResult<T> result, string routeName)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, new Dictionary<string, object>(), HateoasType.Edit, true, HateoasMethodType.PUT);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        public static IBusinessResult<T> AddLinkItemEdit<T>(this IBusinessResult<T> result, string routeName, object routeValues)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Edit, true, HateoasMethodType.PUT);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        public static IBusinessResult<T> AddLinkItemEdit<T>(this IBusinessResult<T> result, string routeName, object routeValues, HateoasMethodType method)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Edit, true, method);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        #endregion

        #region [ Delete ]

        public static IBusinessResult<T> AddLinkItemDelete<T>(this IBusinessResult<T> result, string routeName)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, new Dictionary<string, object>(), HateoasType.Delete, true, HateoasMethodType.DELETE);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        public static IBusinessResult<T> AddLinkItemDelete<T>(this IBusinessResult<T> result, string routeName, object routeValues)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Delete, true, HateoasMethodType.DELETE);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        public static IBusinessResult<T> AddLinkItemDelete<T>(this IBusinessResult<T> result, string routeName, object routeValues, HateoasMethodType method)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Delete, true, method);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        #endregion

        #region [ Self ]

        public static IBusinessResult<T> AddLinkSelf<T>(this IBusinessResult<T> result, string routeName)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, new Dictionary<string, object>(), HateoasType.Self, false, HateoasMethodType.GET);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        public static IPagingResult<T> AddLinkSelf<T>(this IPagingResult<T> result, string routeName, object routeValues)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Self, false, HateoasMethodType.GET);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        public static IPagingResult<T> AddLinkSelf<T>(this IPagingResult<T> result, string routeName, object routeValues, HateoasMethodType method)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Self, false, method);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        public static IPagingResult<T> AddLinkSelf<T>(this IPagingResult<T> result, string routeName, IPagingCriteria Paging)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, result, Paging, HateoasType.Self, HateoasMethodType.GET);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        public static IPagingResult<T> AddLinkSelf<T>(this IPagingResult<T> result, string routeName, IPagingCriteria Paging, HateoasMethodType method)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, result, Paging, HateoasType.Self, method);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        #endregion

        #region [ Collection ]

        public static IBusinessResult<T> AddLinkCollection<T>(this IBusinessResult<T> result, string routeName)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, new Dictionary<string, object>(), HateoasType.Collection, true, HateoasMethodType.GET);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        public static IBusinessResult<T> AddLinkCollection<T>(this IBusinessResult<T> result, string routeName, object routeValues)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Collection, true, HateoasMethodType.GET);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        public static IBusinessResult<T> AddLinkCollection<T>(this IBusinessResult<T> result, string routeName, object routeValues, HateoasMethodType method)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, routeValues, HateoasType.Collection, true, method);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        public static IPagingResult<T> AddLinkCollection<T>(this IPagingResult<T> result, string routeName, IPagingCriteria Paging)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, result, Paging, HateoasType.Collection, HateoasMethodType.GET);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        public static IPagingResult<T> AddLinkCollection<T>(this IPagingResult<T> result, string routeName, IPagingCriteria Paging, HateoasMethodType method)
        {
            if (result != null && !string.IsNullOrEmpty(routeName))
            {
                var link = HateoasHelper.CreateLink(routeName, result, Paging, HateoasType.Collection, method);
                if (link != null)
                    result.Links.Add(link);
            }
            return result;
        }

        #endregion

        #region [ Paging ]

        public static IPagingResult<T> AddLinkPaging<T>(this IPagingResult<T> result, string routeName, IPagingCriteria Paging)
        {
            AddLinkPaging(result, routeName, Paging, HateoasMethodType.GET);
            return result;
        }

        public static IPagingResult<T> AddLinkPaging<T>(this IPagingResult<T> result, string routeName, IPagingCriteria Paging, HateoasMethodType method)
        {
            if (result == null) return result;

            var link = HateoasHelper.CreateLink(routeName, result, Paging, HateoasType.First, method);
            if (link != null)
                result.Links.Add(link);

            link = HateoasHelper.CreateLink(routeName, result, Paging, HateoasType.Previous, method);
            if (link != null)
                result.Links.Add(link);

            link = HateoasHelper.CreateLink(routeName, result, Paging, HateoasType.Next, method);
            if (link != null)
                result.Links.Add(link);

            link = HateoasHelper.CreateLink(routeName, result, Paging, HateoasType.Last, method);
            if (link != null)
                result.Links.Add(link);

            return result;
        }

        #endregion
    }
}
