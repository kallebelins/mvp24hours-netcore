//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Microsoft.AspNetCore.Mvc;
using Mvp24Hours.Core.Contract.Logic;
using Mvp24Hours.Core.Contract.Logic.DTO;
using Mvp24Hours.Core.DTO.Logic;
using Mvp24Hours.Core.Enums.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvp24Hours.Infrastructure.Helpers
{
    public static class HateoasHelper
    {
        #region [ ResolveUrl ]

        private static string ResolveUrl(string routeName, object routeValues)
        {
            var _urlHelper = HttpContextHelper.GetService<IUrlHelper>();
            return _urlHelper.Link(routeName, routeValues);
        }

        private static string ResolveUrlTemplate(string routeName, object routeValues)
        {
            var _urlHelper = HttpContextHelper.GetService<IUrlHelper>();
            string url = _urlHelper.RouteUrl(routeName, routeValues);

            if (!string.IsNullOrEmpty(url))
            {
                if (url.EndsWith("0"))
                    return url.Replace("/0", "/{id}");
                else if (url.EndsWith(Guid.Empty.ToString()))
                    return url.Replace($"/{Guid.Empty.ToString()}", "/{oid}");
                else
                    return url;
            }
            return string.Empty;
        }

        #endregion

        #region [ CreateLink ]

        public static ILinkResult CreateLink(string routeName, HateoasType type)
        {
            return CreateLink(routeName, null, type);
        }

        public static ILinkResult CreateLink(string routeName, HateoasType type, HateoasMethodType method)
        {
            return CreateLink(routeName, null, type, false, method);
        }

        public static ILinkResult CreateLink(string routeName, object routeValues, HateoasType type)
        {
            return CreateLink(routeName, routeValues, type, false);
        }

        public static ILinkResult CreateLink(string routeName, object routeValues, HateoasType type, bool isTemplate)
        {
            return CreateLink(routeName, routeValues, type, false, HateoasMethodType.GET);
        }

        public static ILinkResult CreateLink(string routeName, object routeValues, HateoasType type, bool isTemplate, HateoasMethodType method)
        {
            string rel = string.Empty;

            switch (type)
            {
                case HateoasType.Self:
                    rel = "self";
                    CorrectRouteValues(routeValues);
                    break;
                case HateoasType.Start:
                    rel = "start";
                    break;
                case HateoasType.Item:
                    rel = "item";
                    break;
                case HateoasType.Collection:
                    rel = "collection";
                    break;
                case HateoasType.Previous:
                    rel = "previous";
                    CorrectRouteValues(routeValues);
                    break;
                case HateoasType.Next:
                    rel = "next";
                    CorrectRouteValues(routeValues);
                    break;
                case HateoasType.First:
                    rel = "first";
                    CorrectRouteValues(routeValues);
                    break;
                case HateoasType.Last:
                    rel = "last";
                    CorrectRouteValues(routeValues);
                    break;
                case HateoasType.Create:
                    rel = "create";
                    break;
                case HateoasType.Edit:
                    rel = "edit";
                    break;
                case HateoasType.Delete:
                    rel = "delete";
                    break;
                case HateoasType.Related:
                    rel = "related";
                    break;
            }

            return CreateLink(routeName, routeValues, rel, isTemplate, method);
        }

        static void CorrectRouteValues(object routeValues)
        {
            IDictionary<string, object> values = routeValues as Dictionary<string, object>;

            if (values != null)
            {
                foreach (var value in HttpContextHelper.GetContext().Request.Query.ToDictionary(q => q.Key.ToLower(), q => q.Value))
                {
                    if (!values.Keys.Any(x => x.ToLower() == value.Key))
                    {
                        values.Add(value.Key, value.Value);
                    }
                }
            }
        }

        public static ILinkResult CreateLink(string routeName, object routeValues, string rel, bool isTemplate, HateoasMethodType method)
        {
            string url = isTemplate ? ResolveUrlTemplate(routeName, routeValues) : ResolveUrl(routeName, routeValues);
            if (!string.IsNullOrEmpty(url))
            {
                return new LinkResult
                {
                    Href = url,
                    Rel = rel,
                    Method = method.ToString(),
                    IsTemplate = isTemplate
                };
            }
            return null;
        }

        #endregion

        #region [ Paging Link ]

        public static ILinkResult CreateLink<T, U>(string routeName, IPagingResult<T> result, IPagingCriteria<U> Paging, HateoasType type)
        {
            return CreateLink<T, U>(routeName, result, Paging, type, HateoasMethodType.GET);
        }

        public static ILinkResult CreateLink<T, U>(string routeName, IPagingResult<T> result, IPagingCriteria<U> Paging, HateoasType type, HateoasMethodType method)
        {
            if (type == HateoasType.Self)
            {
                if (result != null)
                {
                    if (Paging == null)
                    {
                        return CreateLink(routeName, new Dictionary<string, object>(), HateoasType.Self, false, method);
                    }
                    else
                    {
                        var routeValues = new Dictionary<string, object>()
                        {
                            { "limit", Paging.Limit },
                            { "offset", Paging.Offset }
                        };
                        return CreateLink(routeName, routeValues, HateoasType.Self, false, method);
                    }
                }
            }

            if (type == HateoasType.Previous)
            {
                int offset = (Paging != null ? Paging.Offset : 0);
                int limit = (Paging != null ? Paging.Limit : ConfigurationPropertiesHelper.MaxQtyByQueryPage);
                if (offset > 0)
                {
                    var routeValues = new Dictionary<string, object>()
                    {
                        { "limit", limit },
                        { "offset", offset - 1 }
                    };
                    return CreateLink(routeName, routeValues, HateoasType.Previous, false, method);
                }
            }

            if (type == HateoasType.First)
            {
                int offset = (Paging != null ? Paging.Offset : 0);
                int limit = (Paging != null ? Paging.Limit : ConfigurationPropertiesHelper.MaxQtyByQueryPage);
                if (offset > 0)
                {
                    var routeValues = new Dictionary<string, object>()
                    {
                        { "limit", limit },
                        { "offset", 0 }
                    };
                    return CreateLink(routeName, routeValues, HateoasType.First, false, method);
                }
            }

            if (type == HateoasType.Next)
            {
                int offset = (Paging != null ? Paging.Offset : 0);
                int limit = (Paging != null ? Paging.Limit : ConfigurationPropertiesHelper.MaxQtyByQueryPage);
                if (result.Summary != null &&
                    offset < result.Summary.TotalPages - 1)
                {
                    var routeValues = new Dictionary<string, object>()
                    {
                        { "limit", limit },
                        { "offset", offset + 1 }
                    };
                    return CreateLink(routeName, routeValues, HateoasType.Next, false, method);
                }
            }

            if (type == HateoasType.Last)
            {
                int offset = (Paging != null ? Paging.Offset : 0);
                int limit = (Paging != null ? Paging.Limit : ConfigurationPropertiesHelper.MaxQtyByQueryPage);
                if (result.Summary != null
                    && offset < result.Summary.TotalPages - 1)
                {
                    var routeValues = new Dictionary<string, object>()
                    {
                        { "limit", limit },
                        { "offset", result.Summary.TotalPages }
                    };
                    return CreateLink(routeName, routeValues, HateoasType.Last, false, method);
                }
            }

            return null;
        }

        #endregion

    }
}