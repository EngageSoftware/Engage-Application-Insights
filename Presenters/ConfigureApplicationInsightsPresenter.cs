// <copyright file="ConfigureApplicationInsightsPresenter.cs" company="Engage Software">
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
    using System.Linq;

    using DotNetNuke.Web.Mvp;

    using WebFormsMvp;

    /// <summary>Acts as a presenter for <see cref="IConfigureApplicationInsightsView"/></summary>
    public sealed class ConfigureApplicationInsightsPresenter : ModulePresenter<IConfigureApplicationInsightsView, ConfigureApplicationInsightsViewModel>
    {
        /// <summary>The XML merge service</summary>
        private readonly Lazy<XmlMergeService> xmlMergeService;

        /// <summary>The component for managing the main Application Insights configuration</summary>
        private readonly Lazy<IApplicationInsightsConfig> applicationInsightsConfig;

        /// <summary>The component for managing Application Insights JavaScript</summary>
        private readonly Lazy<IApplicationInsightsJavaScript> applicationInsightsJavaScript;

        /// <summary>The module file mapper</summary>
        private readonly Lazy<IFileMapper> moduleFileMapper;

        /// <summary>Initializes a new instance of the <see cref="ConfigureApplicationInsightsPresenter"/> class.</summary>
        /// <param name="view">The view.</param>
        public ConfigureApplicationInsightsPresenter(IConfigureApplicationInsightsView view)
            : base(view)
        {
            this.moduleFileMapper = new Lazy<IFileMapper>(() => new ModuleFileMapper(this.ModuleInfo.DesktopModule));
            this.xmlMergeService = new Lazy<XmlMergeService>(() => new XmlMergeService(this.ModuleInfo.DesktopModule));
            this.applicationInsightsConfig = new Lazy<IApplicationInsightsConfig>(() => new ApplicationInsightsConfig(this.ModuleInfo.DesktopModule));
            this.applicationInsightsJavaScript = new Lazy<IApplicationInsightsJavaScript>(() => new ApplicationInsightsJavaScript(this.ModuleInfo.DesktopModule));

            this.View.Initialize += this.View_Initialize;
            this.View.UpdateConfiguration += this.View_UpdateConfiguration;
        }

        /// <summary>Gets the component for managing Application Insights JavaScript</summary>
        private IApplicationInsightsJavaScript ApplicationInsightsJavaScript
        {
            get { return this.applicationInsightsJavaScript.Value; }
        }

        /// <summary>Gets the component for managing the main Application Insights configuration</summary>
        private IApplicationInsightsConfig ApplicationInsightsConfig
        {
            get { return this.applicationInsightsConfig.Value; }
        }

        /// <summary>Gets the XML merge service</summary>
        private XmlMergeService XmlMergeService
        {
            get { return this.xmlMergeService.Value; }
        }

        /// <summary>Gets the module file mapper</summary>
        private IFileMapper ModuleFileMapper
        {
            get { return this.moduleFileMapper.Value; }
        }

        /// <summary>Handles the <see cref="IModuleViewBase.Initialize"/> event of the <see cref="Presenter{TView}.View"/>.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void View_Initialize(object sender, EventArgs e)
        {
            try
            {
                this.View.Model.InstrumentationKey = this.ApplicationInsightsConfig.InstrumentationKey;
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
                var isEnabled = !string.IsNullOrWhiteSpace(this.ApplicationInsightsConfig.InstrumentationKey);
                var willBeEnabled = !string.IsNullOrWhiteSpace(e.InstrumentationKey);
                if (willBeEnabled)
                {
                    if (!isEnabled)
                    {
                        this.EnableApplicationInsights(e.InstrumentationKey);
                    }
                    else
                    {
                        this.UpdateInstrumentationKey(e.InstrumentationKey);
                    }
                }
                else
                {
                    this.DisableApplicationInsights();
                }

                this.View.Model.InstrumentationKey = this.ApplicationInsightsConfig.InstrumentationKey;
            }
            catch (Exception exc)
            {
                this.ProcessModuleLoadException(exc);
            }
        }

        private void UpdateInstrumentationKey(string instrumentationKey)
        {
            this.ApplicationInsightsConfig.SetInstrumentationKey(instrumentationKey);
            this.ApplicationInsightsJavaScript.SetInstrumentationKey(instrumentationKey);
        }

        /// <summary>Enables Application Insights integration for the site.</summary>
        /// <param name="instrumentationKey">The instrumentation key.</param>
        private void EnableApplicationInsights(string instrumentationKey)
        {
            this.ApplicationInsightsJavaScript.EnableApplicationInsights(instrumentationKey);
            this.ApplicationInsightsConfig.EnableApplicationInsights(instrumentationKey);
            this.XmlMergeService.ApplyXmlMerge(this.ModuleFileMapper.MapFilePath("EnableApplicationInsights.xml"));
        }

        /// <summary>Disables Application Insights integration for the site.</summary>
        private void DisableApplicationInsights()
        {
            this.XmlMergeService.ApplyXmlMerge(this.ModuleFileMapper.MapFilePath("DisableApplicationInsights.xml"));
            this.ApplicationInsightsConfig.DisableApplicationInsights();
            this.ApplicationInsightsJavaScript.DisableApplicationInsights();
        }
    }
}
