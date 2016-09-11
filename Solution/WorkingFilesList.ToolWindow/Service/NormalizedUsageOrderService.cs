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

using System.Collections.Generic;
using System.Linq;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;

namespace WorkingFilesList.ToolWindow.Service
{
    public class NormalizedUsageOrderService : INormalizedUsageOrderService
    {
        /// <summary>
        /// Sets <see cref="DocumentMetadata.UsageOrder"/> according to sorted
        /// <see cref="DocumentMetadata.ActivatedAt"/> values to determine order
        /// of recent usage
        /// </summary>
        /// <param name="metadataCollection">
        /// Collection over which to establish historic order of usage 
        /// </param>
        /// <param name="userPreferences">
        /// <see cref="IUserPreferences"/> instance that represents whether to
        /// show recent file usage
        /// </param>
        public void SetUsageOrder(
            IList<DocumentMetadata> metadataCollection,
            IUserPreferences userPreferences)
        {
            var interval = 1/(double) metadataCollection.Count;
            var sortedCollection = metadataCollection.OrderBy(m => m.ActivatedAt);

            var counter = 0;

            foreach (var metadata in sortedCollection)
            {
                counter++;

                var value = userPreferences.ShowRecentUsage
                    ? counter*interval
                    : 1; // Set to 1 so entire list-item length is coloured

                metadata.UsageOrder = value;
            }
        }
    }
}
