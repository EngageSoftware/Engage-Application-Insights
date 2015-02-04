// <copyright file="ApplicationInsightsModule.cs" company="Engage Software">
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
    using System.Web;
    using System.Web.UI;

    using DotNetNuke.Common;
    using DotNetNuke.Web.Client.ClientResourceManagement;

    /// <summary>Injects Application Insights JS script into all pages</summary>
    /// <remarks>based on <see href="http://stackoverflow.com/a/5240313/2688"/></remarks>
    public class ApplicationInsightsModule : IHttpModule
    {
        /// <summary>Initializes a module and prepares it to handle requests.</summary>
        /// <param name="application">An <see cref="HttpApplication" /> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication application)
        {
            Requires.NotNull("application", application);
            application.PostMapRequestHandler += (sender, args) =>
            {
                var page = application.Context.CurrentHandler as Page;
                if (page != null)
                {
                    page.PreRenderComplete += (o, eventArgs) => ClientResourceManager.RegisterScript(page, "~/DesktopModules/Engage/ApplicationInsights/ai.js");
                }
            };
        }

        /// <summary>Disposes of the resources (other than memory) used by the module that implements <see cref="IHttpModule" />.</summary>
        public void Dispose()
        {
        }
    }
}