// <copyright file="ApplicationInsightsJavaScript.cs" company="Engage Software">
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
    using System.IO;
    using System.Linq;
    using System.Web;

    using DotNetNuke.Entities.Controllers;
    using DotNetNuke.Entities.Modules;

    /// <summary>An implementation of <see cref="IApplicationInsightsJavaScript"/> which creates, deletes, and updates the <c>ai.js</c> file in the module's folder</summary>
    /// <seealso cref="Engage.Dnn.ApplicationInsights.IApplicationInsightsJavaScript" />
    public class ApplicationInsightsJavaScript : IApplicationInsightsJavaScript
    {
        /// <summary>The module file mapper</summary>
        private readonly IFileMapper moduleFileMapper;

        /// <summary>Initializes a new instance of the <see cref="ApplicationInsightsJavaScript"/> class.</summary>
        /// <param name="desktopModuleInfo">The desktop module which contains the <c>ai.js</c> file.</param>
        public ApplicationInsightsJavaScript(DesktopModuleInfo desktopModuleInfo)
        {
            this.moduleFileMapper = new ModuleFileMapper(desktopModuleInfo);
        }

        /// <summary>Enables the JavaScript component of Application Insights.</summary>
        /// <param name="instrumentationKey">The instrumentation key.</param>
        public void EnableApplicationInsights(string instrumentationKey)
        {
            this.SetInstrumentationKey(instrumentationKey);
        }

        /// <summary>Disables the JavaScript component of Application Insights.</summary>
        public void DisableApplicationInsights()
        {
            File.Delete(this.moduleFileMapper.MapFilePath("ai.js"));
        }

        /// <summary>Sets the instrumentation key in the <c>ai.js</c> file.</summary>
        /// <param name="instrumentationKey">The new instrumentation key.</param>
        public void SetInstrumentationKey(string instrumentationKey)
        {
            var templateInstrumentationKeyString = '"' + Guid.Empty.ToString() + '"';
            var encodedInstrumentationKeyString = HttpUtility.JavaScriptStringEncode(instrumentationKey, true);
            var lines = from line in File.ReadAllLines(this.moduleFileMapper.MapFilePath("ai.template.js"))
                        select line.Replace(templateInstrumentationKeyString, encodedInstrumentationKeyString);

            File.WriteAllLines(this.moduleFileMapper.MapFilePath("ai.js"), lines);

            HostController.Instance.IncrementCrmVersion(true);
        }
    }
}