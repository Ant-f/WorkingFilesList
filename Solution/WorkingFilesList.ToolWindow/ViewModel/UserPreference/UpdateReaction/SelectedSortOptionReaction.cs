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

using System.ComponentModel;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.ViewModel.UserPreference.UpdateReaction
{
    public class SelectedSortOptionReaction : IUpdateReaction
    {
        private readonly ISortOptionsService _sortOptionsService;

        public SelectedSortOptionReaction(ISortOptionsService sortOptionsService)
        {
            _sortOptionsService = sortOptionsService;
        }

        public void UpdateCollection(
            ICollectionView view,
            IUserPreferences userPreferences)
        {
            view.SortDescriptions.Clear();

            var sortDescriptions =
                _sortOptionsService.EvaluateAppliedSortDescriptions(userPreferences);

            foreach (var sortDescription in sortDescriptions)
            {
                view.SortDescriptions.Add(sortDescription);
            }
        }
    }
}
