// WorkingFilesList
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright(C) 2016 Anthony Fung

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program. If not, see<http://www.gnu.org/licenses/>.

using EnvDTE;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Service.EventRelay
{
    public class SolutionEventsService : ISolutionEventsService
    {
        private readonly IProjectBrushService _projectBrushService;

        public SolutionEventsService(IProjectBrushService projectBrushService)
        {
            _projectBrushService = projectBrushService;
        }

        public void AfterClosing()
        {
            _projectBrushService.ClearBrushIdCollection();
        }

        public void ProjectRenamed(Project project, string oldName)
        {
            _projectBrushService.UpdateBrushId(oldName, project.FullName);
        }
    }
}
