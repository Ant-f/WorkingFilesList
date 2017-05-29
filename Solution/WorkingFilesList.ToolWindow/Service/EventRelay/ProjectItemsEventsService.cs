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
using Microsoft.VisualStudio;
using System;
using System.IO;
using WorkingFilesList.Core.Interface;
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

        public void ItemRemoved(ProjectItem projectItem)
        {
            _documentMetadataManager.Synchronize(projectItem.DTE.Documents, true);
        }

        public void ItemRenamed(ProjectItem projectItem, string oldName)
        {
            if (projectItem == null)
            {
                return;
            }

            if (IsKind(projectItem, VSConstants.GUID_ItemType_PhysicalFile))
            {
                // Document can be null if file has been moved outside of Visual Studio
                if (projectItem.Document == null)
                {
                    return;
                }

                var directoryName = Path.GetDirectoryName(
                    projectItem.Document.FullName);

                var oldFullName = directoryName != null
                    ? Path.Combine(directoryName, oldName)
                    : oldName;

                var updated = _documentMetadataManager.UpdateFullName(
                    projectItem.Document.FullName,
                    oldFullName);

                if (!updated)
                {
                    _documentMetadataManager.Synchronize(
                        projectItem.DTE.Documents,
                        true);
                }
            }
            else if (IsKind(projectItem, VSConstants.GUID_ItemType_PhysicalFolder))
            {
                _documentMetadataManager.Synchronize(
                    projectItem.DTE.Documents,
                    true);
            }
        }
        
        private static bool IsKind(ProjectItem item, Guid typeGuid)
        {
            var success = Guid.TryParse(item.Kind, out Guid itemKindGuid);
            var isMatch = success && itemKindGuid == typeGuid;
            return isMatch;
        }
    }
}