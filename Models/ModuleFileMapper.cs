// <copyright file="ModuleFileMapper.cs" company="Engage Software">
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
    using System.Web.Hosting;

    using DotNetNuke.Entities.Modules;

    /// <summary>A file mapper for a module</summary>
    /// <seealso cref="Engage.Dnn.ApplicationInsights.IFileMapper" />
    public class ModuleFileMapper : IFileMapper
    {
        /// <summary>Initializes a new instance of the <see cref="ModuleFileMapper"/> class.</summary>
        /// <param name="desktopModuleInfo">The desktop module for which to map files.</param>
        public ModuleFileMapper(DesktopModuleInfo desktopModuleInfo)
            : this(desktopModuleInfo.FolderName)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ModuleFileMapper"/> class.</summary>
        /// <param name="folderName">The module's folder name.</param>
        private ModuleFileMapper(string folderName)
        {
            this.FolderName = folderName;
        }

        /// <summary>Gets the name of the module's folder.</summary>
        private string FolderName { get; set; }

        /// <summary>Gets the absolute physical file path for the given <paramref name="relativeFilePath" />.</summary>
        /// <param name="relativeFilePath">The relative path to the file (relative to the module's folder).</param>
        /// <returns>The absolute physical file path</returns>
        public string MapFilePath(string relativeFilePath)
        {
            return HostingEnvironment.MapPath("~/DesktopModules/" + this.FolderName + "/" + relativeFilePath);
        }
    }
}