// <copyright file="XmlExtensions.cs" company="Engage Software">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>Extension methods for dealing with <see cref="XElement"/>, <see cref="XmlNode"/> and related types</summary>
    public static class XmlExtensions
    {
        /// <summary>Converts an <see cref="XmlNode"/> to an <see cref="XElement"/></summary>
        /// <param name="node">The node to convert</param>
        /// <returns>A new <see cref="XElement"/> instance</returns>
        /// <remarks>based on <see href="http://blogs.msdn.com/b/ericwhite/archive/2010/03/05/convert-xdocument-to-xmldocument-and-convert-xmldocument-to-xdocument.aspx"/></remarks>
        public static XElement AsXElement(this XmlNode node)
        {
            var doc = new XDocument();
            using (var writer = doc.CreateWriter())
            {
                node.WriteTo(writer);
            }

            return doc.Root;
        }

        /// <summary>Converts an <see cref="XElement"/> to an <see cref="XmlNode"/></summary>
        /// <param name="element">The element to convert</param>
        /// <returns>A new <see cref="XmlNode"/> instance</returns>
        /// <remarks>based on <see href="http://blogs.msdn.com/b/ericwhite/archive/2010/03/05/convert-xdocument-to-xmldocument-and-convert-xmldocument-to-xdocument.aspx"/></remarks>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Breaking change")]
        public static XmlNode AsXmlNode(this XElement element)
        {
            var doc = new XmlDocument();
            using (var reader = element.CreateReader())
            {
                doc.Load(reader);
            }

            return doc;
        }

        /// <summary>Converts an <see cref="XElement"/> to an <see cref="XmlElement"/></summary>
        /// <param name="element">The element to convert</param>
        /// <returns>A new <see cref="XmlElement"/> instance</returns>
        /// <remarks>based on <see href="http://blogs.msdn.com/b/ericwhite/archive/2010/03/05/convert-xdocument-to-xmldocument-and-convert-xmldocument-to-xdocument.aspx"/></remarks>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Breaking change")]
        public static XmlElement AsXmlElement(this XElement element)
        {
            var doc = new XmlDocument();
            using (var reader = element.CreateReader())
            {
                doc.Load(reader);
            }

            return doc.DocumentElement;
        }

        /// <summary>Converts an <see cref="XmlDocument"/> to an <see cref="XDocument"/></summary>
        /// <param name="document">The document to convert</param>
        /// <returns>A new <see cref="XDocument"/> instance</returns>
        /// <remarks>based on <see href="http://blogs.msdn.com/b/ericwhite/archive/2010/03/05/convert-xdocument-to-xmldocument-and-convert-xmldocument-to-xdocument.aspx"/></remarks>
        public static XDocument AsXDocument(this XmlDocument document)
        {
            var doc = new XDocument();
            using (var writer = doc.CreateWriter())
            {
                document.WriteTo(writer);
            }

            var declaration = document.ChildNodes.OfType<XmlDeclaration>().FirstOrDefault();
            if (declaration != null)
            {
                doc.Declaration = new XDeclaration(declaration.Version, declaration.Encoding, declaration.Standalone);
            }

            return doc;
        }

        /// <summary>Converts an <see cref="XDocument"/> to an <see cref="XmlDocument"/></summary>
        /// <param name="document">The document to convert</param>
        /// <returns>A new <see cref="XmlDocument"/> instance</returns>
        /// <remarks>based on <see href="http://blogs.msdn.com/b/ericwhite/archive/2010/03/05/convert-xdocument-to-xmldocument-and-convert-xmldocument-to-xdocument.aspx"/></remarks>
        public static XmlDocument AsXmlDocument(this XDocument document)
        {
            var doc = new XmlDocument();
            using (var reader = document.CreateReader())
            {
                doc.Load(reader);
            }

            if (document.Declaration != null)
            {
                var declaration = doc.CreateXmlDeclaration(document.Declaration.Version, document.Declaration.Encoding, document.Declaration.Standalone);
                doc.InsertBefore(declaration, doc.FirstChild);
            }

            return doc;
        }
    }
}