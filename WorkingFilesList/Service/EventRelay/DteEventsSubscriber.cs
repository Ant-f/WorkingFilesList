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
using EnvDTE80;
using WorkingFilesList.Interface;

namespace WorkingFilesList.Service.EventRelay
{
    /// <summary>
    /// Subscribes to <see cref="DTE2"/> (Development Tools Environment) events,
    /// calling the appropriate methods on the injected services when events are
    /// raised
    /// </summary>
    public class DteEventsSubscriber : IDteEventsSubscriber
    {
        private readonly IProjectItemsEventsService _projectItemsEventsService;
        private readonly IWindowEventsService _windowEventsService;

        public DteEventsSubscriber(
            IProjectItemsEventsService projectItemsEventsService,
            IWindowEventsService windowEventsService)
        {
            _projectItemsEventsService = projectItemsEventsService;
            _windowEventsService = windowEventsService;
        }

        /// <summary>
        /// Subscribe to all events necessary for correct operation of this
        /// Visual Studio extension
        /// </summary>
        /// <param name="dteEvents"><see cref="DTE2.Events"/> property</param>
        public void SubscribeTo(Events2 dteEvents)
        {
            dteEvents.WindowEvents.WindowActivated += WindowEventsWindowActivated;
            dteEvents.WindowEvents.WindowClosing += WindowEventsWindowClosing;
            dteEvents.WindowEvents.WindowCreated += WindowEventsWindowCreated;

            dteEvents.ProjectItemsEvents.ItemRenamed += ProjectItemsEventsItemRenamed;
        }

        private void WindowEventsWindowActivated(Window gotFocus, Window lostFocus)
        {
            _windowEventsService.WindowActivated(gotFocus, lostFocus);
        }

        private void WindowEventsWindowClosing(Window window)
        {
            _windowEventsService.WindowClosing(window);
        }

        private void WindowEventsWindowCreated(Window window)
        {
            _windowEventsService.WindowCreated(window);
        }

        private void ProjectItemsEventsItemRenamed(ProjectItem projectItem, string oldName)
        {
            _projectItemsEventsService.ItemRenamed(projectItem, oldName);
        }
    }
}