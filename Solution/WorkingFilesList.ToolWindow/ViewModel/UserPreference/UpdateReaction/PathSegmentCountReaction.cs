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
using System.Linq;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.ViewModel.UserPreference.UpdateReaction
{
    public class PathSegmentCountReaction : IUpdateReaction
    {
        private readonly IDisplayNameHighlightEvaluator _displayNameHighlightEvaluator;
        private readonly IFilePathService _filePathService;

        public PathSegmentCountReaction(
            IDisplayNameHighlightEvaluator displayNameHighlightEvaluator,
            IFilePathService filePathService)
        {
            _displayNameHighlightEvaluator = displayNameHighlightEvaluator;
            _filePathService = filePathService;
        }

        public void UpdateCollection(
            ICollectionView view,
            IUserPreferences userPreferences)
        {
            foreach (var metadata in view.Cast<DocumentMetadata>())
            {
                var displayName = _filePathService.ReducePath(
                    metadata.CorrectedFullName,
                    userPreferences.PathSegmentCount);

                var preHighlight = _displayNameHighlightEvaluator
                    .GetPreHighlight(
                        displayName,
                        userPreferences.HighlightFileName);

                var highlight = _displayNameHighlightEvaluator
                    .GetHighlight(
                        displayName,
                        userPreferences.HighlightFileName);

                var postHighlight = _displayNameHighlightEvaluator
                    .GetPostHighlight(
                        displayName,
                        userPreferences.HighlightFileName);

                metadata.DisplayNamePreHighlight = preHighlight;
                metadata.DisplayNameHighlight = highlight;
                metadata.DisplayNamePostHighlight = postHighlight;
            }

            // Collection may need to be sorted: changing the number of path
            // segments could make the displayed item order no longer agree with
            // the selected sorting option, e.g. if an alphabetical sort is
            // applied.

            view.Refresh();
        }
    }
}
