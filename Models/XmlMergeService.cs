// <copyright file="XmlMerge.cs" company="Engage Software">
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
    using System.Xml.Linq;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Installer;

    /// <summary>A wrapper around DNN's <see cref="XmlMerge"/> functionality</summary>
    public class XmlMergeService
    {
        /// <summary>Initializes a new instance of the <see cref="XmlMergeService"/> class.</summary>
        /// <param name="desktopModule">The desktop module which is requesting the merge.</param>
        public XmlMergeService(DesktopModuleInfo desktopModule)
            : this(desktopModule.Version, desktopModule.FriendlyName)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="XmlMergeService"/> class.</summary>
        /// <param name="version">The version of the component requesting the merge.</param>
        /// <param name="sender">The name of the component requesting the merge.</param>
        private XmlMergeService(string version, string sender)
        {
            this.Version = version;
            this.Sender = sender;
        }

        /// <summary>Gets the name of the component requesting the merge.</summary>
        private string Sender { get; set; }

        /// <summary>Gets the version of the component requesting the merge.</summary>
        private string Version { get; set; }

        /// <summary>Applies the XML Merge instructions to the configuration files.</summary>
        /// <param name="sourceFileName">Path to the XML Merge configuration file.</param>
        public void ApplyXmlMerge(string sourceFileName)
        {
            var xmlMerge = new XmlMerge(sourceFileName, this.Version, this.Sender);
            xmlMerge.UpdateConfigs();
        }

        /// <summary>Applies the XML Merge instructions to the configuration files.</summary>
        /// <param name="sourceDoc">The XML document with the merge instructions.</param>
        public void ApplyXmlMerge(XDocument sourceDoc)
        {
            var xmlMerge = new XmlMerge(sourceDoc.AsXmlDocument(), this.Version, this.Sender);
            xmlMerge.UpdateConfigs();
        }
    }
}
