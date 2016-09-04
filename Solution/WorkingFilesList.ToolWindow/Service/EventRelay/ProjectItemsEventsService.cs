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

using System.IO;
using EnvDTE;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Service.EventRelay
{
    /// <summary>
    /// Contains methods for processing event handler parameters for relevant
    /// <see cref="ProjectItemsEvents"/> events, and invoking appropriate
    /// methods on other services to respond to such events
    /// </summary>
    public class ProjectItemsEventsService : IProjectItemsEventsService
    {
        private readonly IDocumentMetadataManager _documentMetadataManager;

        public ProjectItemsEventsService(IDocumentMetadataManager documentMetadataManager)
        {
            _documentMetadataManager = documentMetadataManager;
        }

        public void ItemRenamed(ProjectItem projectItem, string oldName)
        {
            if (projectItem?.Document != null)
            {
                var directoryName = Path.GetDirectoryName(
                    projectItem.Document.FullName);

                var oldFullName = directoryName != null
                    ? Path.Combine(directoryName, oldName)
                    : oldName;

                _documentMetadataManager.UpdateFullName(
                    projectItem.Document.FullName,
                    oldFullName);
            }
        }
    }
}