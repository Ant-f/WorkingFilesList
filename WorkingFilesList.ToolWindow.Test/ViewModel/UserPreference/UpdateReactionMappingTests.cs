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

using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.UserPreference
{
    [TestFixture]
    public class UpdateReactionMappingTests
    {
        private IUpdateReaction[] CreateUpdateReactions()
        {
            var updateReactions = new IUpdateReaction[]
            {
                new GroupByProjectUpdateReaction(),
                new PathSegmentCountUpdateReaction(Mock.Of<IFilePathService>()),
                new SelectedSortOptionUpdateReaction(Mock.Of<ISortOptionsService>()),
                new ShowRecentUsageUpdateReaction(Mock.Of<INormalizedUseOrderService>())
            };

            return updateReactions;
        }

        [TestCase(
            nameof(IUserPreferences.PathSegmentCount),
            typeof(PathSegmentCountUpdateReaction))]

        [TestCase(
            nameof(IUserPreferences.SelectedDocumentSortOption),
            typeof(SelectedSortOptionUpdateReaction))]

        [TestCase(
            nameof(IUserPreferences.SelectedProjectSortOption),
            typeof(SelectedSortOptionUpdateReaction))]

        [TestCase(
            nameof(IUserPreferences.GroupByProject),
            typeof(GroupByProjectUpdateReaction))]

        [TestCase(
            nameof(IUserPreferences.ShowRecentUsage),
            typeof(ShowRecentUsageUpdateReaction))]

        public void MappingTableMapsCorrespondingUpdateReaction(
            string mappingKey,
            Type updateReactionType)
        {
            // Arrange

            var allUpdateReactions = CreateUpdateReactions();

            // Act

            var mapping = new UpdateReactionMapping(allUpdateReactions);

            // Assert

            Assert.That(mapping.MappingTable, Contains.Key(mappingKey));

            var reactionsForMapping = mapping.MappingTable[mappingKey];
            var matchingReaction = reactionsForMapping
                .SingleOrDefault(reaction => reaction.GetType() == updateReactionType);

            Assert.That(matchingReaction, Is.Not.Null);
        }
    }
}
