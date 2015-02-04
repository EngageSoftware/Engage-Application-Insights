// <copyright file="SettingsRepositoryBase.cs" company="Engage Software">
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

    /// <summary>Base class for helping implement <see cref="ISettingsRepository"/></summary>
    public abstract class SettingsRepositoryBase : ISettingsRepository
    {
        /// <summary>Determines whether the specified setting has any value.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <returns><c>true</c> if the specified setting has a value; otherwise, <c>false</c>.</returns>
        public virtual bool HasValue<T>(Setting<T> setting)
        {
            Requires.PropertyNotNullOrEmpty("setting", "SettingName", setting.SettingName);
            return this.GetSettings(setting.Scope).Contains(setting.SettingName);
        }

        /// <summary>Gets the value of the setting.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <returns>The setting's value (returns the default value of the setting if it hasn't been set yet)</returns>
        public virtual T GetValue<T>(Setting<T> setting)
        {
            Requires.PropertyNotNullOrEmpty("setting", "SettingName", setting.SettingName);
            return this.GetSettings(setting.Scope).GetValueOrDefault(setting.SettingName, setting.DefaultValue);
        }

        /// <summary>Gets the value of the setting.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <param name="converter">A function which converts the string representation of the value into <typeparamref name="T" />.</param>
        /// <returns>The setting's value (returns the default value of the setting if it hasn't been set yet)</returns>
        public virtual T GetValue<T>(Setting<T> setting, Func<string, T> converter)
        {
            Requires.PropertyNotNullOrEmpty("setting", "SettingName", setting.SettingName);
            return this.GetSettings(setting.Scope).GetValueOrDefault(setting.SettingName, setting.DefaultValue, converter);
        }

        /// <summary>Sets the value of the setting.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <param name="value">The value.</param>
        public virtual void SetValue<T>(Setting<T> setting, T value)
        {
            this.SetValue(setting, value, DefaultStringifier);
        }

        /// <summary>Sets the value of the setting.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <param name="value">The value.</param>
        /// <param name="converter">A function which converts the value into its string representation.</param>
        public virtual void SetValue<T>(Setting<T> setting, T value, Func<T, string> converter)
        {
            var convertedValue = converter(value);
            var setSettingValue = this.GetSetter(setting);
            setSettingValue(convertedValue);
        }

        /// <summary>Gets a function which will update the setting's value.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <returns>A function which takes the <see cref="string"/> value to which to update the setting's value</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="setting"/> has a <see cref="Setting{T}.Scope"/> which is an invalid <see cref="SettingScope"/> value</exception>
        protected abstract Action<string> GetSetter<T>(Setting<T> setting);

        /// <summary>Gets the settings collection for the given <paramref name="settingScope"/>.</summary>
        /// <param name="settingScope">The scope of settings to get.</param>
        /// <returns>A <see cref="IDictionary"/> instance mapping between setting names and setting values as (both as <see cref="string"/> values).</returns>
        /// <exception cref="InvalidOperationException"><paramref name="settingScope"/> was an invalid <see cref="SettingScope"/> value</exception>
        protected abstract IDictionary GetSettings(SettingScope settingScope);

        /// <summary>The default conversion algorithm from a value to a <see cref="string"/> (for storing a setting).</summary>
        /// <typeparam name="T">The type of the value to convert</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <see cref="string"/> representation of the <paramref name="value"/></returns>
        private static string DefaultStringifier<T>(T value)
        {
            if (ReferenceEquals(value, null))
            {
                return null;
            }

            var convertibleValue = value as IConvertible;
            if (convertibleValue != null)
            {
                return convertibleValue.ToString(CultureInfo.InvariantCulture);
            }

            return value.ToString();
        }
    }
}