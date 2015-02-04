// <copyright file="HostSettingsRepository.cs" company="Engage Software">
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

    using DotNetNuke.Entities.Controllers;

    /// <summary>An <see cref="ISettingsRepository"/> implementation which does not have any context</summary>
    public class HostSettingsRepository : SettingsRepositoryBase
    {
        /// <summary>Gets a function which will update the setting's value.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <returns>A function which takes the <see cref="string"/> value to which to update the setting's value</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="setting"/> has a <see cref="Setting{T}.Scope"/> which is an invalid <see cref="SettingScope"/> value</exception>
        protected override Action<string> GetSetter<T>(Setting<T> setting)
        {
            switch (setting.Scope)
            {
                case SettingScope.TabModule:
                    throw new NotSupportedException("HostSettingsRepository does not support TabModule scope");
                case SettingScope.Module:
                    throw new NotSupportedException("HostSettingsRepository does not support Module scope");
                case SettingScope.Tab:
                    throw new NotSupportedException("HostSettingsRepository does not support Tab scope");
                case SettingScope.Portal:
                    throw new NotSupportedException("HostSettingsRepository does not support Portal scope");
                case SettingScope.Host:
                    return value => HostController.Instance.Update(setting.SettingName, value);
                default:
                    throw new InvalidOperationException("Invalid SettingScope");
            }
        }

        /// <summary>Gets the settings collection for the given <paramref name="settingScope"/>.</summary>
        /// <param name="settingScope">The scope of settings to get.</param>
        /// <returns>A <see cref="IDictionary"/> instance mapping between setting names and setting values as (both as <see cref="string"/> values).</returns>
        /// <exception cref="InvalidOperationException"><paramref name="settingScope"/> was an invalid <see cref="SettingScope"/> value</exception>
        protected override IDictionary GetSettings(SettingScope settingScope)
        {
            switch (settingScope)
            {
                case SettingScope.TabModule:
                    throw new NotSupportedException("HostSettingsRepository does not support TabModule scope");
                case SettingScope.Module:
                    throw new NotSupportedException("HostSettingsRepository does not support Module scope");
                case SettingScope.Tab:
                    throw new NotSupportedException("HostSettingsRepository does not support Tab scope");
                case SettingScope.Portal:
                    throw new NotSupportedException("HostSettingsRepository does not support Portal scope");
                case SettingScope.Host:
                    return HostController.Instance.GetSettingsDictionary();
                default:
                    throw new InvalidOperationException("Invalid SettingScope");
            }
        }
    }
}