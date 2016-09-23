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

using EnvDTE;
using EnvDTE80;
using System;
using System.IO;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;

namespace WorkingFilesList.ToolWindow.Service.EventRelay
{
    public class SolutionEventsService : ISolutionEventsService
    {
        private readonly DTE2 _dte2;
        private readonly IDocumentMetadataManager _documentMetadataManager;
        private readonly IProjectBrushService _projectBrushService;

        public event EventHandler<SolutionNameChangedEventArgs> SolutionNameChanged;

        public SolutionEventsService(
            DTE2 dte2,
            IDocumentMetadataManager documentMetadataManager,
            IProjectBrushService projectBrushService)
        {
            _dte2 = dte2;
            _documentMetadataManager = documentMetadataManager;
            _projectBrushService = projectBrushService;
        }

        public void AfterClosing()
        {
            _projectBrushService.ClearBrushIdCollection();
            RaiseSolutionNameChanged(string.Empty);
        }

        public void Opened()
        {
            var name = Path.GetFileNameWithoutExtension(_dte2.Solution.FullName);
            RaiseSolutionNameChanged(name);
        }

        public void ProjectRenamed(Project project, string oldName)
        {
            _projectBrushService.UpdateBrushId(oldName, project.FullName);

            // Synchronize after updating brush ID so the project continues to
            // use the same brush
            _documentMetadataManager.Synchronize(project.DTE.Documents, true);
        }

        private void RaiseSolutionNameChanged(string name)
        {
            SolutionNameChanged?.Invoke(
                this,
                new SolutionNameChangedEventArgs(name));
        }
    }
}
