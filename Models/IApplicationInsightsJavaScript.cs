﻿// <copyright file="IApplicationInsightsJavaScript.cs" company="Engage Software">
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
    /// <summary>A contract specifying the ability to enable, disable, and update the JavaScript component of an App Insights configuration</summary>
    public interface IApplicationInsightsJavaScript
    {
        /// <summary>Sets the instrumentation key in the <c>ai.js</c> file.</summary>
        /// <param name="instrumentationKey">The new instrumentation key.</param>
        void SetInstrumentationKey(string instrumentationKey);

        /// <summary>Enables the JavaScript component of Application Insights.</summary>
        /// <param name="instrumentationKey">The instrumentation key.</param>
        void EnableApplicationInsights(string instrumentationKey);

        /// <summary>Disables the JavaScript component of Application Insights.</summary>
        void DisableApplicationInsights();
    }
}