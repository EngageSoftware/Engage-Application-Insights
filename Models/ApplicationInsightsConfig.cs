// <copyright file="ApplicationInsightsConfig.cs" company="Engage Software">
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
    using System.IO;
    using System.Web.Hosting;
    using System.Xml.Linq;

    using DotNetNuke.Entities.Modules;

    /// <summary>An implementation of <see cref="IApplicationInsightsConfig"/> which creates, deletes, and updates the <c>ApplicationInsights.config</c> file</summary>
    /// <seealso cref="Engage.Dnn.ApplicationInsights.IApplicationInsightsConfig" />
    public class ApplicationInsightsConfig : IApplicationInsightsConfig
    {
        /// <summary>The name of the Application Insights configuration file</summary>
        private const string ApplicationInsightsConfigFileName = "ApplicationInsights.config";

        /// <summary>The XML namespace for the Application Insights config document</summary>
        private static readonly XNamespace Ns = "http://schemas.microsoft.com/ApplicationInsights/2013/Settings";

        /// <summary>Gets the mapped path to the Application Insights configuration file.</summary>
        private static readonly string ApplicationInsightsTemplateMapPath = HostingEnvironment.MapPath("~/Config/" + ApplicationInsightsConfigFileName);

        /// <summary>Gets the mapped path to the Application Insights configuration file.</summary>
        private static readonly string ApplicationInsightsConfigMapPath = HostingEnvironment.MapPath("~/" + ApplicationInsightsConfigFileName);

        /// <summary>Initializes a new instance of the <see cref="ApplicationInsightsConfig"/> class.</summary>
        /// <param name="desktopModule">The desktop module which is managing the configuration.</param>
        public ApplicationInsightsConfig(DesktopModuleInfo desktopModule)
        {
            this.DesktopModule = desktopModule;
        }

        /// <summary>Gets the configured instrumentation key.</summary>
        public string InstrumentationKey
        {
            get
            {
                if (!File.Exists(ApplicationInsightsConfigMapPath))
                {
                    return null;
                }

                return XDocument.Load(ApplicationInsightsConfigMapPath).Element(Ns + "ApplicationInsights").Element(Ns + "InstrumentationKey").Value;
            }
        }

        private DesktopModuleInfo DesktopModule { get; set; }

        /// <summary>Enables Application Insights integration for the site.</summary>
        public void EnableApplicationInsights(string instrumentationKey)
        {
            File.Copy(ApplicationInsightsTemplateMapPath, ApplicationInsightsConfigMapPath, false);
            this.SetInstrumentationKey(instrumentationKey);
        }

        /// <summary>Disables Application Insights integration for the site.</summary>
        public void DisableApplicationInsights()
        {
            File.Delete(ApplicationInsightsConfigMapPath);
        }

        /// <summary>Sets the instrumentation key in the <c>ApplicationInsights.config</c> file.</summary>
        /// <param name="instrumentationKey">The new instrumentation key.</param>
        public void SetInstrumentationKey(string instrumentationKey)
        {
            var xmlMergeConfiguration = new XDocument(
                                            new XElement(
                                                "configuration",
                                                new XElement(
                                                    "nodes",
                                                    new XAttribute("configfile", ApplicationInsightsConfigFileName),
                                                    new XElement(
                                                        "node",
                                                        new XAttribute("path", "/ns:ApplicationInsights"),
                                                        new XAttribute("targetpath", "/ns:ApplicationInsights/ns:InstrumentationKey"),
                                                        new XAttribute("action", "update"),
                                                        new XAttribute("collision", "save"),
                                                        new XAttribute("nameSpace", Ns.NamespaceName),
                                                        new XAttribute("nameSpacePrefix", "ns"),
                                                        new XElement("InstrumentationKey", instrumentationKey)))));

            new XmlMergeService(this.DesktopModule).ApplyXmlMerge(xmlMergeConfiguration);
        }
    }
}