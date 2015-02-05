// <copyright file="ConfigureApplicationInsights.ascx.cs" company="Engage Software">
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
    using System.Web.UI.WebControls;

    using DotNetNuke.Web.Mvp;

    using WebFormsMvp;

    /// <summary>Displays the Application Insights configuration</summary>
    [PresenterBinding(typeof(ConfigureApplicationInsightsPresenter))]
    public partial class ConfigureApplicationInsights : ModuleView<ConfigureApplicationInsightsViewModel>, IConfigureApplicationInsightsView
    {
        /// <summary>Occurs when updating the configuration.</summary>
        public event EventHandler<UpdatingConfigurationEventArgs> UpdateConfiguration = (_, __) => { };

        /// <summary>Handles the <see cref="Button.Click" /> event of the <see cref="SubmitButton"/> control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.Page.IsValid)
                {
                    return;
                }

                this.UpdateConfiguration(this, new UpdatingConfigurationEventArgs(this.InstrumentationKeyTextBox.Text));
            }
            catch (Exception exc)
            {
                this.ProcessModuleLoadException(exc);
            }
        }
    }
}