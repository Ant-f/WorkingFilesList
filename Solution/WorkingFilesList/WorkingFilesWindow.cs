// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2016 Anthony Fung

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using WorkingFilesList.ToolWindow.View;

namespace WorkingFilesList
{
    using Microsoft.VisualStudio.Shell;
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts
    /// a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by
    /// the shell) and a pane, usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF
    /// in order to use its implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("1a5129bb-eaa3-4a09-91e0-8e572ac81214")]
    public class WorkingFilesWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkingFilesWindow"/> class.
        /// </summary>
        public WorkingFilesWindow() : base(null)
        {
            this.Caption = "Working Files List";

            // This is the user control hosted by the tool window; Note that,
            // even if this class implements IDisposable, we are not calling
            // Dispose on this object. This is because ToolWindowPane calls
            // Dispose on the object returned by the Content property.

            this.Content = new WorkingFilesWindowControl();
        }
    }
}
