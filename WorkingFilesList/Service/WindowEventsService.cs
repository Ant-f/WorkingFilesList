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
using WorkingFilesList.Interface;

namespace WorkingFilesList.Service
{
    public class WindowEventsService : IWindowEventsService
    {
        private readonly IDocumentMetadataService _documentMetadataService;

        public WindowEventsService(IDocumentMetadataService documentMetadataService)
        {
            _documentMetadataService = documentMetadataService;
        }

        public void WindowActivated(Window gotFocus, Window lostFocus)
        {
            WindowCreated(gotFocus);
        }

        public void WindowClosing(Window window)
        {
            throw new System.NotImplementedException();
        }

        public void WindowCreated(Window window)
        {
            if (window.Type == vsWindowType.vsWindowTypeDocument &&
                window.Document.ActiveWindow != null)
            {
                _documentMetadataService.Upsert(window.Document.FullName);
            }
        }
    }
}