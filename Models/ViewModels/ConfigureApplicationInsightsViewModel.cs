// <copyright file="ConfigureApplicationInsightsViewModel.cs" company="Engage Software">
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

    /// <summary>The view model for configuring Application Insights, to be displayed by <see cref="IConfigureApplicationInsightsView"/></summary>
    public class ConfigureApplicationInsightsViewModel
    {
        /// <summary>Gets or sets the instrumentation key.</summary>
        public string InstrumentationKey { get; set; }
    }
}
