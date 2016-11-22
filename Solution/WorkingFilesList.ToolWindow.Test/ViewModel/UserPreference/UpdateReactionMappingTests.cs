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

using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference.UpdateReaction;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.UserPreference
{
    [TestFixture]
    public class UpdateReactionMappingTests
    {
        private IUpdateReaction[] CreateUpdateReactions()
        {
            var updateReactions = new IUpdateReaction[]
            {
                new AssignProjectColoursReaction(Mock.Of<IProjectBrushService>()),
                new GroupByProjectReaction(),

                new PathSegmentCountReaction(
                    Mock.Of<IDisplayNameHighlightEvaluator>(),
                    Mock.Of<IFilePathService>()),

                new SelectedSortOptionReaction(Mock.Of<ISortOptionsService>()),
                new ShowRecentUsageReaction(Mock.Of<INormalizedUsageOrderService>())
            };

            return updateReactions;
        }

        [TestCase(
            nameof(IUserPreferences.AssignProjectColours),
            typeof(AssignProjectColoursReaction))]

        [TestCase(
            nameof(IUserPreferences.PathSegmentCount),
            typeof(PathSegmentCountReaction))]

        [TestCase(
            nameof(IUserPreferences.DocumentSortOption),
            typeof(SelectedSortOptionReaction))]

        [TestCase(
            nameof(IUserPreferences.ProjectSortOption),
            typeof(SelectedSortOptionReaction))]

        [TestCase(
            nameof(IUserPreferences.GroupByProject),
            typeof(GroupByProjectReaction))]

        [TestCase(
            nameof(IUserPreferences.ShowRecentUsage),
            typeof(ShowRecentUsageReaction))]

        [TestCase(
            nameof(IUserPreferences.ShowRecentUsage),
            typeof(AssignProjectColoursReaction))]

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
