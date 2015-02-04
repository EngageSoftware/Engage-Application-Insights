// <copyright file="ConfigureApplicationInsightsPresenter.cs" company="Engage Software">
// Engage: Application Insights for DNN
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
    using System.Xml;
    using System.Xml.Linq;

    using DotNetNuke.Common;
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
        private static string ApplicationInsightsConfigMapPath
        {
            get { return Path.Combine(Globals.ApplicationMapPath, ApplicationInsightsConfigFileName); }
        }

        /// <summary>Gets the configured instrumentation key.</summary>
        private string InstrumentationKey
        {
            get { return XDocument.Load(ApplicationInsightsConfigMapPath).Element(Ns + "ApplicationInsights").Element(Ns + "InstrumentationKey").Value; }
        }

        /// <summary>Handles the <see cref="IModuleViewBase.Initialize"/> event of the <see cref="Presenter{TView}.View"/>.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void View_Initialize(object sender, EventArgs e)
        {
            try
            {
                this.View.Model.InstrumentationKey = this.InstrumentationKey;
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
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(@"<configuration><nodes>
<node path='/ns:ApplicationInsights' 
      targetpath='/ns:ApplicationInsights/ns:InstrumentationKey' 
      action='update' 
      collision='save' 
      nameSpace='http://schemas.microsoft.com/ApplicationInsights/2013/Settings' 
      nameSpacePrefix='ns'>
    <InstrumentationKey>"
                    + e.InstrumentationKey
+ "</InstrumentationKey></node></nodes></configuration>");
                var xmlMerge = new XmlMerge(xmlDocument, this.ModuleInfo.DesktopModule.Version, this.ModuleInfo.DesktopModule.ModuleName);
                var targetDocument = new XmlDocument();
                targetDocument.Load(ApplicationInsightsConfigMapPath);
                xmlMerge.UpdateConfig(targetDocument, ApplicationInsightsConfigFileName);

                this.View.Model.InstrumentationKey = this.InstrumentationKey;
            }
            catch (Exception exc)
            {
                this.ProcessModuleLoadException(exc);
            }
        }
    }
}
