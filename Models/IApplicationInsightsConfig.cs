// <copyright file="IApplicationInsightsConfig.cs" company="Engage Software">
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
    /// <summary>A contract specifying the ability to enable, disable, and update the primary App Insights configuration</summary>
    public interface IApplicationInsightsConfig
    {
        /// <summary>Gets the configured instrumentation key.</summary>
        string InstrumentationKey { get; }

        /// <summary>Enables Application Insights integration for the site.</summary>
        /// <param name="instrumentationKey">The instrumentation key.</param>
        void EnableApplicationInsights(string instrumentationKey);

        /// <summary>Disables Application Insights integration for the site.</summary>
        void DisableApplicationInsights();

        /// <summary>Sets the instrumentation key in the <c>ApplicationInsights.config</c> file.</summary>
        /// <param name="instrumentationKey">The new instrumentation key.</param>
        void SetInstrumentationKey(string instrumentationKey);
    }
}