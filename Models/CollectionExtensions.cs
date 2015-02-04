// <copyright file="CollectionExtensions.cs" company="Engage Software">
// Engage.ApplicationInsights
// Copyright (c) 2004-2015
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
namespace Engage.Dnn.ApplicationInsights
{
    using System;
    using System.Collections;
    using System.Globalization;

    using DotNetNuke.Common;

    /// <summary>Extensions for <see cref="IDictionary"/></summary>
    /// <remarks>from <see href="https://github.com/dnnsoftware/Dnn.Platform/blob/development/DNN Platform/Library/Collections/CollectionExtensions.cs"/></remarks>
    public static class CollectionExtensions
    {
        /// <summary>Gets the value from the dictionary, returning the default value of <typeparamref key="T" /> if the value doesn't exist.</summary>
        /// <typeparam name="T">The type of the value to retrieve</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key by which to get the value.</param>
        /// <returns>A <typeparamref name="T"/> instance.</returns>
        /// <exception cref="InvalidCastException">
        /// the value is <c>null</c> and <typeparamref name="T"/> is a value type, or
        /// the value does not implement the <see cref="IConvertible"/> interface and
        /// no cast is defined from the value to <typeparamref name="T"/>
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <c>null</c></exception>
        public static T GetValueOrDefault<T>(this IDictionary dictionary, string key)
        {
            return dictionary.GetValueOrDefault(key, default(T));
        }

        /// <summary>Gets the value from the dictionary, returning <paramref name="defaultValue"/> if the value doesn't exist.</summary>
        /// <typeparam name="T">The type of the value to retrieve</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key by which to get the value.</param>
        /// <param name="defaultValue">The default value to return if the dictionary doesn't have a value for the given <paramref name="key"/>.</param>
        /// <returns>A <typeparamref name="T"/> instance.</returns>
        /// <exception cref="InvalidCastException">
        /// the value is <c>null</c> and <typeparamref name="T"/> is a value type, or
        /// the value does not implement the <see cref="IConvertible"/> interface and
        /// no cast is defined from the value to <typeparamref name="T"/>
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <c>null</c></exception>
        public static T GetValueOrDefault<T>(this IDictionary dictionary, string key, T defaultValue)
        {
            return dictionary.GetValueOrDefault(key, defaultValue, ConvertValue<T>);
        }

        /// <summary>Gets the value from the dictionary, returning the default value of <typeparamref key="T" /> if the value doesn't exist.</summary>
        /// <typeparam name="T">The type of the value to retrieve</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key by which to get the value.</param>
        /// <param name="converter">A function to convert the value as an <see cref="object"/> to a <typeparamref name="T"/> instance.</param>
        /// <returns>A <typeparamref name="T"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <c>null</c></exception>
        public static T GetValueOrDefault<T>(this IDictionary dictionary, string key, Func<object, T> converter)
        {
            return dictionary.GetValueOrDefault(key, default(T), converter);
        }

        /// <summary>Gets the value from the dictionary, returning the default value of <typeparamref key="T" /> if the value doesn't exist.</summary>
        /// <typeparam name="T">The type of the value to retrieve</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key by which to get the value.</param>
        /// <param name="converter">A function to convert the value as an <see cref="string"/> to a <typeparamref name="T"/> instance.</param>
        /// <returns>A <typeparamref name="T"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <c>null</c></exception>
        public static T GetValueOrDefault<T>(this IDictionary dictionary, string key, Func<string, T> converter)
        {
            return dictionary.GetValueOrDefault(key, default(T), (object value) => ConvertValue(value, converter));
        }

        /// <summary>Gets the value from the dictionary, returning <paramref name="defaultValue"/> if the value doesn't exist.</summary>
        /// <typeparam name="T">The type of the value to retrieve</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key by which to get the value.</param>
        /// <param name="defaultValue">The default value to return if the dictionary doesn't have a value for the given <paramref name="key"/>.</param>
        /// <param name="converter">A function to convert the value as an <see cref="string"/> to a <typeparamref name="T"/> instance.</param>
        /// <returns>A <typeparamref name="T"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <c>null</c></exception>
        public static T GetValueOrDefault<T>(this IDictionary dictionary, string key, T defaultValue, Func<string, T> converter)
        {
            return dictionary.GetValueOrDefault(key, defaultValue, (object value) => ConvertValue(value, converter));
        }

        /// <summary>Gets the value from the dictionary, returning the <paramref key="defaultValue"/> if the value doesn't exist.</summary>
        /// <typeparam name="T">The type of the value to retrieve</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key by which to get the value.</param>
        /// <param name="defaultValue">The default value to return if the dictionary doesn't have a value for the given <paramref name="key"/>.</param>
        /// <param name="converter">A function to convert the value as an <see cref="object"/> to a <typeparamref name="T"/> instance.</param>
        /// <returns>A <typeparamref name="T"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> or <paramref name="converter"/> is <c>null</c></exception>
        public static T GetValueOrDefault<T>(this IDictionary dictionary, string key, T defaultValue, Func<object, T> converter)
        {
            Requires.NotNull("dictionary", dictionary);
            Requires.NotNull("converter", converter);
            return dictionary.Contains(key) ? converter(dictionary[key]) : defaultValue;
        }

        /// <summary>Converts the <paramref name="value"/> into a <typeparamref name="T"/> instance.</summary>
        /// <typeparam name="T">The type of the value to return</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <typeparamref name="T"/> instance.</returns>
        /// <exception cref="InvalidCastException">
        /// the value is <c>null</c> and <typeparamref name="T"/> is a value type, or
        /// the value does not implement the <see cref="IConvertible"/> interface and
        /// no cast is defined from the value to <typeparamref name="T"/>
        /// </exception>
        private static T ConvertValue<T>(object value)
        {
            if (value is T)
            {
                return (T)value;
            }

            if (value is IConvertible)
            {
                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }

            if (value == null)
            {
                if (typeof(T).IsValueType)
                {
                    if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return (T)value;
                    }

                    // TODO: this should probably return the default value if called from GetOrDefault...
                    throw new InvalidCastException();
                }

                return (T)value; // null
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)value.ToString();
            }

            return (T)value;
        }

        /// <summary>Converts the <paramref name="value" /> into a <typeparamref name="T" /> instance.</summary>
        /// <typeparam name="T">The type of the value to return</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="converter">A function to convert a <see cref="string"/> to a <typeparamref name="T"/> instance.</param>
        /// <returns>A <typeparamref name="T" /> instance.</returns>
        private static T ConvertValue<T>(object value, Func<string, T> converter)
        {
            var formattable = value as IFormattable;
            return converter(value == null ? null : formattable == null ? value.ToString() : formattable.ToString(null, CultureInfo.InvariantCulture));
        }
    }
}