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

namespace WorkingFilesList.Service
{
    /// <summary>
    /// Subscribes to <see cref="DTE2"/> (Development Tools Environment) events,
    /// returning services that act on them
    /// </summary>
    public class DteEventsSubscriber : IDteEventsSubscriber
    {
        private readonly IDteEventsServicesFactory _dteEventsServicesFactory;

        private IDteEventsServices _services;

        public DteEventsSubscriber(IDteEventsServicesFactory dteEventsServicesFactory)
        {
            _dteEventsServicesFactory = dteEventsServicesFactory;
        }

        /// <summary>
        /// Subscribe to all events necessary for correct operation of this
        /// Visual Studio extension
        /// </summary>
        /// <param name="dteEvents"><see cref="DTE2.Events"/> property</param>
        /// <returns>
        /// An object containing the services that are used as part of handling
        /// events from <see cref="DTE2"/>
        /// </returns>
        public IDteEventsServices SubscribeTo(Events2 dteEvents)
        {
            _services = _dteEventsServicesFactory.CreateDteEventsServices();

            dteEvents.WindowEvents.WindowActivated += WindowEventsWindowActivated;
            dteEvents.WindowEvents.WindowClosing += WindowEventsWindowClosing;
            dteEvents.WindowEvents.WindowCreated += WindowEventsWindowCreated;

            dteEvents.ProjectItemsEvents.ItemRenamed += ProjectItemsEventsItemRenamed;

            return _services;
        }

        private void WindowEventsWindowActivated(Window gotFocus, Window lostFocus)
        {
            _services.WindowEventsService.WindowActivated(gotFocus, lostFocus);
        }

        private void WindowEventsWindowClosing(Window window)
        {
            _services.WindowEventsService.WindowClosing(window);
        }

        private void WindowEventsWindowCreated(Window window)
        {
            _services.WindowEventsService.WindowCreated(window);
        }

        private void ProjectItemsEventsItemRenamed(ProjectItem projectItem, string oldName)
        {
            _services.ProjectItemsEventsService.ItemRenamed(projectItem, oldName);
        }
    }
}