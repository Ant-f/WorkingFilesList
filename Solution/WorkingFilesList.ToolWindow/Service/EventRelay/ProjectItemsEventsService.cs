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
using System.IO;
using WorkingFilesList.Core.Interface;

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