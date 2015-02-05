// <copyright file="ConfigureApplicationInsightsPresenter.cs" company="Engage Software">
// Engage: Application Insights
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
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Hosting;
    using System.Xml.Linq;

    using DotNetNuke.Entities.Controllers;
    using DotNetNuke.Services.Installer;
    using DotNetNuke.Web.Mvp;

    using WebFormsMvp;

    /// <summary>Acts as a presenter for <see cref="IConfigureApplicationInsightsView"/></summary>
    public sealed class ConfigureApplicationInsightsPresenter : ModulePresenter<IConfigureApplicationInsightsView, ConfigureApplicationInsightsViewModel>
    {
        /// <summary>The name of the Application Insights configuration file</summary>
        private const string ApplicationInsightsConfigFileName = "ApplicationInsights.config";

        /// <summary>The XML namespace for the Application Insights config document</summary>
        private static readonly XNamespace Ns = "http://schemas.microsoft.com/ApplicationInsights/2013/Settings";

        /// <summary>Initializes a new instance of the <see cref="ConfigureApplicationInsightsPresenter"/> class.</summary>
        /// <param name="view">The view.</param>
        public ConfigureApplicationInsightsPresenter(IConfigureApplicationInsightsView view)
            : base(view)
        {
            this.View.Initialize += this.View_Initialize;
            this.View.UpdateConfiguration += this.View_UpdateConfiguration;
        }

        /// <summary>Gets the mapped path to the Application Insights configuration file.</summary>
        private static string ApplicationInsightsTemplateMapPath
        {
            get { return HostingEnvironment.MapPath("~/Config/" + ApplicationInsightsConfigFileName); }
        }

        /// <summary>Gets the mapped path to the Application Insights configuration file.</summary>
        private static string ApplicationInsightsConfigMapPath
        {
            get { return HostingEnvironment.MapPath("~/" + ApplicationInsightsConfigFileName); }
        }

        /// <summary>Gets the configured instrumentation key.</summary>
        private static string InstrumentationKey
        {
            get
            {
                return File.Exists(ApplicationInsightsConfigMapPath)
                    ? XDocument.Load(ApplicationInsightsConfigMapPath).Element(Ns + "ApplicationInsights").Element(Ns + "InstrumentationKey").Value
                    : null;
            }
        }

        /// <summary>Handles the <see cref="IModuleViewBase.Initialize"/> event of the <see cref="Presenter{TView}.View"/>.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void View_Initialize(object sender, EventArgs e)
        {
            try
            {
                this.View.Model.InstrumentationKey = InstrumentationKey;
            }
            catch (Exception exc)
            {
                this.ProcessModuleLoadException(exc);
            }
        }

        /// <summary>Handles the <see cref="IConfigureApplicationInsightsView.UpdateConfiguration"/> event of the <see cref="Presenter{TView}.View"/>.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="UpdatingConfigurationEventArgs"/> instance containing the event data.</param>
        private void View_UpdateConfiguration(object sender, UpdatingConfigurationEventArgs e)
        {
            try
            {
                var isEnabled = !string.IsNullOrWhiteSpace(InstrumentationKey);
                var willBeEnabled = !string.IsNullOrWhiteSpace(e.InstrumentationKey);
                if (willBeEnabled)
                {
                    if (!isEnabled)
                    {
                        this.EnableApplicationInsights();
                    }

                    this.SetInstrumentationKeyInConfig(e.InstrumentationKey);
                    this.SetInstrumentationKeyInJavaScript(e.InstrumentationKey);
                }
                else
                {
                    this.DisableApplicationInsights();
                }

                this.View.Model.InstrumentationKey = InstrumentationKey;
            }
            catch (Exception exc)
            {
                this.ProcessModuleLoadException(exc);
            }
        }

        /// <summary>Applies the XML Merge instructions to the configuration files.</summary>
        /// <param name="sourceFileName">Path to the XML Merge configuration file.</param>
        private void ApplyXmlMerge(string sourceFileName)
        {
            var xmlMerge = new XmlMerge(sourceFileName, this.ModuleInfo.DesktopModule.Version, this.ModuleInfo.DesktopModule.FriendlyName);
            xmlMerge.UpdateConfigs();
        }

        /// <summary>Applies the XML Merge instructions to the configuration files.</summary>
        /// <param name="sourceDoc">The XML document with the merge instructions.</param>
        private void ApplyXmlMerge(XDocument sourceDoc)
        {
            var xmlMerge = new XmlMerge(sourceDoc.AsXmlDocument(), this.ModuleInfo.DesktopModule.Version, this.ModuleInfo.DesktopModule.FriendlyName);
            xmlMerge.UpdateConfigs();
        }

        /// <summary>Enables Application Insights integration for the site.</summary>
        private void EnableApplicationInsights()
        {
            File.Copy(ApplicationInsightsTemplateMapPath, ApplicationInsightsConfigMapPath, false);
            this.ApplyXmlMerge(this.MapModuleFilePath("EnableApplicationInsights.xml"));
        }

        /// <summary>Disables Application Insights integration for the site.</summary>
        private void DisableApplicationInsights()
        {
            this.ApplyXmlMerge(this.MapModuleFilePath("DisableApplicationInsights.xml"));
            File.Delete(this.MapModuleFilePath("ai.js"));
            File.Delete(ApplicationInsightsConfigMapPath);
        }

        /// <summary>Sets the instrumentation key in the <c>ApplicationInsights.config</c> file.</summary>
        /// <param name="instrumentationKey">The new instrumentation key.</param>
        private void SetInstrumentationKeyInConfig(string instrumentationKey)
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

            this.ApplyXmlMerge(xmlMergeConfiguration);
        }

        /// <summary>Sets the instrumentation key in the <c>ai.js</c> file.</summary>
        /// <param name="instrumentationKey">The new instrumentation key.</param>
        private void SetInstrumentationKeyInJavaScript(string instrumentationKey)
        {
            var templateInstrumentationKeyString = '"' + Guid.Empty.ToString() + '"';
            var encodedInstrumentationKeyString = HttpUtility.JavaScriptStringEncode(instrumentationKey, true);
            var lines = from line in File.ReadAllLines(this.MapModuleFilePath("ai.template.js"))
                        select line.Replace(templateInstrumentationKeyString, encodedInstrumentationKeyString);

            File.WriteAllLines(this.MapModuleFilePath("ai.js"), lines);

            HostController.Instance.IncrementCrmVersion(true);
        }

        /// <summary>Gets the mapped path for a file in the module's folder.</summary>
        /// <param name="relativeFilePath">The file path relative to the module's folder.</param>
        /// <returns>The physical path on the server.</returns>
        private string MapModuleFilePath(string relativeFilePath)
        {
            return HostingEnvironment.MapPath("~/DesktopModules/" + this.ModuleInfo.DesktopModule.FolderName + "/" + relativeFilePath);
        }
    }
}
