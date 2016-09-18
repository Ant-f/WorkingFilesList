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
