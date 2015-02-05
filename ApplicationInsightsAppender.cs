// <copyright file="ApplicationInsightsAppender.cs" company="Engage Software">
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
    using System.Collections.Generic;
    using System.Globalization;

    using log4net.Appender;
    using log4net.Core;

    using Microsoft.ApplicationInsights;

    /// <summary>A Log4Net appender for Application Insights</summary>
    /// <remarks>We can't use the official appender because DNN doesn't use the official Log4Net assembly</remarks>
    public class ApplicationInsightsAppender : AppenderSkeleton
    {
        /// <summary>The telemetry client</summary>
        private readonly TelemetryClient telemetryClient = new TelemetryClient();

        /// <summary>Appends the logging event.</summary>
        /// <param name="loggingEvent">The logging event.</param>
        /// <exception cref="LogException">When there's an exception</exception>
        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.telemetryClient.Context.InstrumentationKey))
                {
                    return;
                }

                if (loggingEvent.ExceptionObject == null)
                {
                    this.telemetryClient.TrackTrace(loggingEvent.RenderedMessage ?? string.Empty, GetProperties(loggingEvent));
                }
                else
                {
                    this.telemetryClient.TrackException(loggingEvent.ExceptionObject, GetProperties(loggingEvent), new Dictionary<string, double>(0));
                }
            }
            catch (ArgumentNullException ex)
            {
                throw new LogException(ex.Message, ex);
            }
        }

        /// <summary>Gets the properties of the event.</summary>
        /// <param name="loggingEvent">The logging event.</param>
        /// <returns>A <see cref="IDictionary{T,U}" /> instance.</returns>
        private static IDictionary<string, string> GetProperties(LoggingEvent loggingEvent)
        {
            var properties = new Dictionary<string, string> { { "SourceType", "Log4Net" }, };
            AddLogProperty("LoggerName", loggingEvent.LoggerName, properties);
            AddLogProperty("LoggingLevel", loggingEvent.Level != null ? loggingEvent.Level.Name : null, properties);
            AddLogProperty("ThreadName", loggingEvent.ThreadName, properties);
            AddLogProperty("TimeStamp", loggingEvent.TimeStamp, properties);
            AddLogProperty("UserName", loggingEvent.UserName, properties);
            AddLogProperty("Domain", loggingEvent.Domain, properties);
            AddLogProperty("Identity", loggingEvent.Identity, properties);
            
            var locationInformation = loggingEvent.LocationInformation;
            if (locationInformation != null)
            {
                AddLogProperty("ClassName", locationInformation.ClassName, properties);
                AddLogProperty("FileName", locationInformation.FileName, properties);
                AddLogProperty("MethodName", locationInformation.MethodName, properties);
                AddLogProperty("LineNumber", locationInformation.LineNumber, properties);
            }

            return properties;
        }

        /// <summary>Adds the property to the log properties, if <paramref name="value"/> is not <c>null</c>.</summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="properties">The properties collection.</param>
        private static void AddLogProperty(string key, string value, IDictionary<string, string> properties)
        {
            if (value == null)
            {
                return;
            }

            properties.Add(key, value);
        }

        /// <summary>Adds the property to the log properties, if <paramref name="value"/> is not <c>null</c>.</summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="properties">The properties collection.</param>
        private static void AddLogProperty(string key, IConvertible value, IDictionary<string, string> properties)
        {
            if (value == null)
            {
                return;
            }

            AddLogProperty(key, value.ToString(CultureInfo.InvariantCulture), properties);
        }
    }
}