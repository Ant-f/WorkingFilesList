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

                [nameof(IUserPreferences.SelectedDocumentSortOption)] = new IUpdateReaction[]
                {
                    updateReactions.OfType<SelectedSortOptionReaction>().Single()
                },

                [nameof(IUserPreferences.SelectedProjectSortOption)] = new IUpdateReaction[]
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
