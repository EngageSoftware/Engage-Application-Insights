// <copyright file="SettingScope.cs" company="Engage Software">
// Engage.ApplicationInsights
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

    /// <summary>The scope of a <see cref="Setting{T}"/></summary>
    public enum SettingScope
    {
        /// <summary>A setting that applied to an instance of a module on a particular page</summary>
        TabModule = 0,

        /// <summary>A setting that applies to a module instance (including on any page to which a reference of it is shared)</summary>
        Module = 1,

        /// <summary>A setting that applies to a page</summary>
        Tab = 2,

        /// <summary>A setting that applies to a site</summary>
        Portal = 3,

        /// <summary>A setting that applies to the entire DNN installation</summary>
        Host = 4,
    }
}
