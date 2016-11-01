// <copyright file="IFileMapper.cs" company="Engage Software">
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
    /// <summary>Specifies the ability to map a relative path to an absolute physical path</summary>
    public interface IFileMapper
    {
        /// <summary>Gets the absolute physical file path for the given <paramref name="relativeFilePath"/>.</summary>
        /// <param name="relativeFilePath">The relative path to the file (relative to the file mapper implementation's root).</param>
        /// <returns>The absolute physical file path</returns>
        string MapFilePath(string relativeFilePath);
    }
}