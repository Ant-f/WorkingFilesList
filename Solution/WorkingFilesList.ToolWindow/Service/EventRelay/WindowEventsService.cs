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
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Service.EventRelay
{
    /// <summary>
    /// Contains methods for processing event handler parameters for relevant
    /// <see cref="WindowEvents"/> events, and invoking appropriate methods on
    /// other services to respond to such events
    /// </summary>
    public class WindowEventsService : IWindowEventsService
    {
        private readonly IDocumentMetadataManager _documentMetadataManager;

        public WindowEventsService(IDocumentMetadataManager documentMetadataManager)
        {
            _documentMetadataManager = documentMetadataManager;
        }

        public void WindowActivated(Window gotFocus, Window lostFocus)
        {
            if (gotFocus.Type == vsWindowType.vsWindowTypeDocument &&
                gotFocus.Document?.ActiveWindow != null)
            {
                _documentMetadataManager.Activate(
                    gotFocus.Document.FullName);
            }
        }

        public void WindowClosing(Window window)
        {
            if (window.Type == vsWindowType.vsWindowTypeDocument)
            {
                _documentMetadataManager.Synchronize(window.DTE.Documents, true);
            }
        }

        public void WindowCreated(Window window)
        {
            if (window.Type == vsWindowType.vsWindowTypeDocument &&
                window.Document?.ActiveWindow != null)
            {
                if (_documentMetadataManager.ActiveDocumentMetadata.IsEmpty)
                {
                    _documentMetadataManager.Synchronize(window.DTE.Documents, false);
                }
                else
                {
                    var info = new DocumentMetadataInfo
                    {
                        FullName = window.Document.FullName,

                        ProjectDisplayName =
                            window.Document.ProjectItem.ContainingProject.Name,

                        ProjectFullName =
                            window.Document.ProjectItem.ContainingProject.FullName
                    };

                    _documentMetadataManager.Add(info);
                }

                _documentMetadataManager.Activate(window.Document.FullName);
            }
        }
    }
}