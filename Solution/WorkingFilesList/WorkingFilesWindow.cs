// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2016 Anthony Fung

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using WorkingFilesList.Core.Model;
using WorkingFilesList.Core.Service.Locator;
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
        private const string DefaultCaption = "Working Files List";

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkingFilesWindow"/> class.
        /// </summary>
        public WorkingFilesWindow() : base(null)
        {
            this.Caption = DefaultCaption;

            // This is the user control hosted by the tool window; Note that,
            // even if this class implements IDisposable, we are not calling
            // Dispose on this object. This is because ToolWindowPane calls
            // Dispose on the object returned by the Content property.

            this.Content = new WorkingFilesWindowControl();

            ViewModelService.SolutionEventsService.SolutionNameChanged +=
                SolutionEventsServiceSolutionNameChanged;

            // The name of an already open solution will not be displayed in
            // the caption text. Invoke ISolutionEventsService.Opened to
            // address this. This needs to be done here (instead of within
            // the SolutionEventsService itself) because DTE events are
            // subscribed to on package initialization, which occurs before
            // this constuctor is called.

            ViewModelService.SolutionEventsService.Opened();
        }

        private void SolutionEventsServiceSolutionNameChanged(
            object sender,
            SolutionNameChangedEventArgs e)
        {
            var nameIsEmpty = string.IsNullOrWhiteSpace(e.NewName);

            var suffix = nameIsEmpty
                ? string.Empty
                : $" [{e.NewName}]";

            this.Caption = $"{DefaultCaption}{suffix}";
        }
    }
}
