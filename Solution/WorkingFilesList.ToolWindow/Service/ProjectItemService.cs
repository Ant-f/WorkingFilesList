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
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Service
{
    public class ProjectItemService : IProjectItemService
    {
        private readonly DTE2 _dte2;
        private readonly IIOService _ioService;

        public ProjectItemService(DTE2 dte2, IIOService ioService)
        {
            _dte2 = dte2;
            _ioService = ioService;
        }

        /// <summary>
        /// Find the existing <see cref="ProjectItem"/> associated with the
        /// provided file name
        /// </summary>
        /// <param name="itemName">Name of project item to find</param>
        /// <returns>
        /// The <see cref="ProjectItem"/> associated with the provided file name,
        /// if it exists, and the actual file it references exists
        /// </returns>
        public ProjectItem FindProjectItem(string itemName)
        {
            ProjectItem itemToReturn = null;
            var projectItem = _dte2.Solution.FindProjectItem(itemName);

            if (projectItem != null)
            {
                var fullName = projectItem.FileNames[1];
                var exists = _ioService.FileExists(fullName);

                if (exists)
                {
                    itemToReturn = projectItem;
                }
            }

            return itemToReturn;
        }
    }
}
