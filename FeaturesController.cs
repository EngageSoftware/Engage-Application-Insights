// <copyright file="FeaturesController.cs" company="Engage Software">
// Engage: Application Insights
// Copyright (c) 2004-2016
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
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;

    /// <summary>Contains basic information about the module and exposes which DNN integration points the module implements</summary>
    [SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors", Justification = "DNN instantiates this class via reflection, so it needs an accessible constructor")]
    public class FeaturesController : IUpgradeable
    {
        /// <summary>Upgrades the module.</summary>
        /// <param name="Version">The module version.</param>
        /// <returns>A status message</returns>
        public string UpgradeModule(string Version)
        {
            var version = new Version(Version);
            if (version == new Version(1, 0, 1))
            {
                return UpgradeToVersion101();
            }

            return string.Format(CultureInfo.CurrentCulture, "Version {0} is unknown, no action taken", Version);
        }

        /// <summary>Re-enables the configuration if it was previously enabled.</summary>
        /// <returns>A status message</returns>
        private static string UpgradeToVersion101()
        {
            var module = DesktopModuleController.GetDesktopModuleByModuleName("Engage: Application Insights", Null.NullInteger);
            var config = new ApplicationInsightsConfig(module);
            if (string.IsNullOrEmpty(config.InstrumentationKey))
            {
                return "Upgraded Engage: Application Insights to version 1.0.1, Application Insights was not enable and remains disabled";
            }

            config.EnableApplicationInsights(config.InstrumentationKey);
            new ApplicationInsightsJavaScript(module).EnableApplicationInsights(config.InstrumentationKey);
            new XmlMergeService(module).ApplyXmlMerge(new ModuleFileMapper(module).MapFilePath("EnableApplicationInsights.xml"));

            return "Upgraded to Engage: Application Insights to version 1.0.1, re-enabled Application Insights";
        }
    }
}
