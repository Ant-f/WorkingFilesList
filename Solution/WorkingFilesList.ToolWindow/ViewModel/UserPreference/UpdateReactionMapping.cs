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
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference.UpdateReaction;

namespace WorkingFilesList.ToolWindow.ViewModel.UserPreference
{
    public class UpdateReactionMapping : IUpdateReactionMapping
    {
        /// <summary>
        /// Maps the names of <see cref="IUserPreferences"/> properties to
        /// <see cref="IUpdateReaction"/> instances that should be applied to a
        /// collection view in order for it to reflect user preference values
        /// </summary>
        public IDictionary<string, IEnumerable<IUpdateReaction>> MappingTable { get; }

        public UpdateReactionMapping(IList<IUpdateReaction> updateReactions)
        {
            MappingTable = new Dictionary<string, IEnumerable<IUpdateReaction>>
            {
                [nameof(IUserPreferences.AssignProjectColours)] = new IUpdateReaction[]
                {
                    updateReactions.OfType<AssignProjectColoursReaction>().Single()
                },

                [nameof(IUserPreferences.PathSegmentCount)] = new IUpdateReaction[]
                {
                    updateReactions.OfType<PathSegmentCountReaction>().Single()
                },

                [nameof(IUserPreferences.DocumentSortOption)] = new IUpdateReaction[]
                {
                    updateReactions.OfType<SelectedSortOptionReaction>().Single()
                },

                [nameof(IUserPreferences.HighlightFileName)] = new IUpdateReaction[]
                {
                    updateReactions.OfType<PathSegmentCountReaction>().Single()
                },

                [nameof(IUserPreferences.ProjectSortOption)] = new IUpdateReaction[]
                {
                    updateReactions.OfType<SelectedSortOptionReaction>().Single()
                },

                [nameof(IUserPreferences.GroupByProject)] = new IUpdateReaction[]
                {
                    updateReactions.OfType<GroupByProjectReaction>().Single()
                },

                [nameof(IUserPreferences.ShowRecentUsage)] = new IUpdateReaction[]
                {
                    // Showing/hiding recent usage needs to update project colours
                    // in addition to usage order: when project-specific-colours
                    // is disabled, the colour still needs to be set to either a
                    // generic project colour, or Transparent depending on whether
                    // recent file usage is visually represented.

                    updateReactions.OfType<AssignProjectColoursReaction>().Single(),
                    updateReactions.OfType<ShowRecentUsageReaction>().Single()
                }
            };
        }
    }
}
