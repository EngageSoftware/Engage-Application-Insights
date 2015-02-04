// <copyright file="UpdatingConfigurationEventArgs.cs" company="Engage Software">
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

    /// <summary>Contains data about the configuration that have been updated by <see cref="IConfigureApplicationInsightsView"/></summary>
    public class UpdatingConfigurationEventArgs : EventArgs
    {
        /// <summary>Initializes a new instance of the <see cref="UpdatingConfigurationEventArgs"/> class.</summary>
        public UpdatingConfigurationEventArgs()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="UpdatingConfigurationEventArgs"/> class.</summary>
        /// <param name="instrumentationKey">The value of the instrumentation key.</param>
        public UpdatingConfigurationEventArgs(string instrumentationKey)
        {
            this.InstrumentationKey = instrumentationKey;
        }

        /// <summary>Gets the instrumentation key.</summary>
        public string InstrumentationKey { get; private set; }
    }
}