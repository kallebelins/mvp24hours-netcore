//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Mvp24Hours.Core.Serialization.Json
{
    /// <summary>
    /// Serializes anonymous classes
    /// <see href="https://dotnetfiddle.net/S4GPil"/>
    /// </summary>
    /// <remarks>
    /// <code>
    /// JsonConvert.DefaultSettings = () => { return new JsonSerializerSettings{ ContractResolver = new AnonymousTypeContractResolver() }; };
    /// var o = new { foo = "123", bar = "456" };
    /// JsonConvert.PopulateObject(@"{'foo': 'abc'}", o);
    /// Console.WriteLine(o.foo);
    /// </code>
    /// </remarks>
    public class AnonymousTypeContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
        {
            var x = base.CreateProperty(member, memberSerialization);
            var t = member.DeclaringType;
            if (Attribute.IsDefined(t, typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute)) && t.Name.Contains("AnonymousType"))
            {
                try
                {
                    x.ValueProvider = new AnonymousTypeValueProvider(member);
                    x.Writable = true;
                }
                catch (Exception)
                {
                    // left empty code
                }
            }

            return x;
        }

        class AnonymousTypeValueProvider : IValueProvider
        {
            private Func<object, object> _getter;
            private Action<object, object> _setter;
            private readonly FieldInfo _fieldInfo;
            private readonly MemberInfo _memberInfo;

            public AnonymousTypeValueProvider(MemberInfo memberInfo)
            {
                _memberInfo = memberInfo;
                var fieldName = string.Format("<{0}>i__Field", memberInfo.Name);
                _fieldInfo = memberInfo.DeclaringType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                        .First(fi => fi.Name.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));
            }

            public void SetValue(object target, object value)
            {
                try
                {
                    if (_setter == null)
                    {
                        _setter = (t, v) => { _fieldInfo.SetValue(t, v); };
                    }

                    _setter(target, value);
                }
                catch (Exception ex)
                {
                    throw new JsonSerializationException(message: $"Error setting value to '{CultureInfo.InvariantCulture}' on '{_memberInfo.Name}'.", innerException: ex);
                }
            }

            public object GetValue(object target)
            {
                try
                {
                    if (_getter == null)
                    {
                        _getter = t => { return _fieldInfo.GetValue(t); };
                    }

                    return _getter(target);
                }
                catch (Exception ex)
                {
                    throw new JsonSerializationException(message: $"Error getting value from '{CultureInfo.InvariantCulture}' on '{_memberInfo.Name}'.", innerException: ex);
                }
            }
        }
    }
}
