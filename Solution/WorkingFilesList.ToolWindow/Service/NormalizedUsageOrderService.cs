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

using System.Collections.Generic;
using System.Linq;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Interface;

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
