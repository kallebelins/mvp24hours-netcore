//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Infrastructure.Extensions;
using System;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Mvp24Hours.Infrastructure.Helpers
{
    /// <summary>
    /// Contains functions to transform object (clone, serialize, ...)
    /// </summary>
    public class ObjectHelper
    {
        /// <summary>
        /// Clone object instance with binary method in memory
        /// </summary>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            if (source == null)
            {
                return default;
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static dynamic ConvertToDynamic(object obj)
        {
            var json = obj.ToSerialize();
            return json.ToDeserialize<ExpandoObject>();
        }
    }
}
