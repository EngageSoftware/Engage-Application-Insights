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
        private readonly XmlMergeService xmlMergeService;

        /// <summary>The application insights configuration</summary>
        private readonly IApplicationInsightsConfig applicationInsightsConfig;

        /// <summary>The application insights java script</summary>
        private readonly IApplicationInsightsJavaScript applicationInsightsJavaScript;

        private readonly IFileMapper moduleFileMapper;

        /// <summary>Initializes a new instance of the <see cref="ConfigureApplicationInsightsPresenter"/> class.</summary>
        /// <param name="view">The view.</param>
        public ConfigureApplicationInsightsPresenter(IConfigureApplicationInsightsView view)
            : base(view)
        {
            this.moduleFileMapper = new ModuleFileMapper(this.ModuleInfo.DesktopModule);
            this.xmlMergeService = new XmlMergeService(this.ModuleInfo.DesktopModule);
            this.applicationInsightsConfig = new ApplicationInsightsConfig(this.ModuleInfo.DesktopModule);
            this.applicationInsightsJavaScript = new ApplicationInsightsJavaScript(this.ModuleInfo.DesktopModule);

            this.View.Initialize += this.View_Initialize;
            this.View.UpdateConfiguration += this.View_UpdateConfiguration;
        }

        /// <summary>Handles the <see cref="IModuleViewBase.Initialize"/> event of the <see cref="Presenter{TView}.View"/>.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void View_Initialize(object sender, EventArgs e)
        {
            try
            {
                this.View.Model.InstrumentationKey = this.applicationInsightsConfig.InstrumentationKey;
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
                var isEnabled = !string.IsNullOrWhiteSpace(this.applicationInsightsConfig.InstrumentationKey);
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

                this.View.Model.InstrumentationKey = this.applicationInsightsConfig.InstrumentationKey;
            }
            catch (Exception exc)
            {
                this.ProcessModuleLoadException(exc);
            }
        }

        private void UpdateInstrumentationKey(string instrumentationKey)
        {
            this.applicationInsightsConfig.SetInstrumentationKey(instrumentationKey);
            this.applicationInsightsJavaScript.SetInstrumentationKey(instrumentationKey);
        }

        /// <summary>Enables Application Insights integration for the site.</summary>
        private void EnableApplicationInsights(string instrumentationKey)
        {
            this.applicationInsightsJavaScript.EnableApplicationInsights(instrumentationKey);
            this.applicationInsightsConfig.EnableApplicationInsights(instrumentationKey);
            this.xmlMergeService.ApplyXmlMerge(this.moduleFileMapper.MapFilePath("EnableApplicationInsights.xml"));
        }

        /// <summary>Disables Application Insights integration for the site.</summary>
        private void DisableApplicationInsights()
        {
            this.xmlMergeService.ApplyXmlMerge(this.moduleFileMapper.MapFilePath("DisableApplicationInsights.xml"));
            this.applicationInsightsConfig.DisableApplicationInsights();
            this.applicationInsightsJavaScript.DisableApplicationInsights();
        }
    }
}
