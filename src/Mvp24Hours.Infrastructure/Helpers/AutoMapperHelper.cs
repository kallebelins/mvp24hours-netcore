using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mvp24Hours.Infrastructure.Helpers
{
    /// <summary>
    /// Helper class to assist in mapping multiple entities to one single
    /// entity.
    /// </summary>
    /// <remarks>
    /// Code courtesy of Owain Wraggs' EMC Consulting Blog
    /// Ref:
    ///     http://consultingblogs.emc.com/owainwragg/archive/2010/12/22/automapper-mapping-from-multiple-objects.aspx
    /// </remarks>
    public static class AutoMapperHelper
    {
        /// <summary>
        /// Maps the specified sources to the specified destination type.
        /// </summary>
        /// <typeparam name="T">The type of the destination</typeparam>
        /// <param name="sources">The sources.</param>
        /// <returns></returns>
        /// <example>
        /// Retrieve the person, address and comment entities 
        /// and map them on to a person view model entity.
        /// 
        /// var personId = 23;
        /// var person = _personTasks.GetPerson(personId);
        /// var address = _personTasks.GetAddress(personId);
        /// var comment = _personTasks.GetComment(personId);
        /// 
        /// var personViewModel = EntityMapper.Map<PersonViewModel>(person, address, comment);
        /// </example>
        public static T Map<T>(params object[] sources) where T : class
        {
            // If there are no sources just return the destination object
            if (!sources.Any())
            {
                return default(T);
            }

            // Get the inital source and map it
            var initialSource = sources[0];
            var mappingResult = Map<T>(initialSource);

            // Now map the remaining source objects
            if (sources.Count() > 1)
            {
                Map(mappingResult, sources.Skip(1).ToArray());
            }

            // return the destination object
            return mappingResult;
        }

        /// <summary>
        /// Maps the specified sources to the specified destination.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="sources">The sources.</param>
        private static void Map(object destination, params object[] sources)
        {
            // If there are no sources just return the destination object
            if (!sources.Any())
            {
                return;
            }

            // Get the destination type
            var destinationType = destination.GetType();
            var mapper = ServiceProviderHelper.GetService<IMapper>();

            // Itereate through all of the sources...
            foreach (var source in sources)
            {
                // ... get the source type and map the source to the destination
                var sourceType = source.GetType();
                mapper.Map(source, destination, sourceType, destinationType);
            }
        }

        /// <summary>
        /// Maps the specified source to the destination.
        /// </summary>
        /// <typeparam name="T">type of teh destination</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        private static T Map<T>(object source) where T : class
        {
            // Get thr source and destination types
            var destinationType = typeof(T);
            var sourceType = source.GetType();

            // Get the destination using AutoMapper's Map
            var mapper = ServiceProviderHelper.GetService<IMapper>();
            var mappingResult = mapper.Map(source, sourceType, destinationType);

            // Return the destination
            return mappingResult as T;
        }
    }
}
