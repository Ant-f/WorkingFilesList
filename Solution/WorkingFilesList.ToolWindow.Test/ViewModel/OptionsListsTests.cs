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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model.SortOption;
using WorkingFilesList.ToolWindow.Service;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;

namespace WorkingFilesList.ToolWindow.Test.ViewModel
{
    [TestFixture]
    public class OptionsListsTests
    {
        [Test]
        public void DocumentSortOptionsContainsOnlyApplicableItems()
        {
            // Arrange

            var documentSortOption = new TestingSortOption(
                "Document",
                null,
                ListSortDirection.Ascending,
                ProjectItemType.Document);

            var projectSortOption = new TestingSortOption(
                "Project",
                null,
                ListSortDirection.Ascending,
                ProjectItemType.Project);

            var options = new List<ISortOption>
            {
                documentSortOption,
                projectSortOption
            };

            // Act

            var builder = new OptionsListsBuilder
            {
                SortOptions = options
            };

            var lists = builder.CreateOptionsLists();

            // Assert

            Assert.That(lists.DocumentSortOptions.Count, Is.EqualTo(1));
            Assert.That(lists.DocumentSortOptions[0], Is.EqualTo(documentSortOption));
        }

        [Test]
        public void ProjectSortOptionsContainsOnlyApplicableItems()
        {
            // Arrange

            var documentSortOption = new TestingSortOption(
                "Document",
                null,
                ListSortDirection.Ascending,
                ProjectItemType.Document);

            var projectSortOption = new TestingSortOption(
                "Project",
                null,
                ListSortDirection.Ascending,
                ProjectItemType.Project);

            var options = new List<ISortOption>
            {
                documentSortOption,
                projectSortOption
            };

            // Act

            var builder = new OptionsListsBuilder
            {
                SortOptions = options
            };

            var lists = builder.CreateOptionsLists();

            // Assert

            Assert.That(lists.ProjectSortOptions.Count, Is.EqualTo(1));
            Assert.That(lists.ProjectSortOptions[0], Is.EqualTo(projectSortOption));
        }

        [Test]
        public void DocumentSortOptionsAppearInSpecifiedOrder()
        {
            // Arrange

            const int chronologicalSortIndex = 0;
            const int alphabeticalSortIndex = 1;

            var alphabeticalSort = new AlphabeticalSort();
            var chronologicalSort = new ChronologicalSort();

            var options = new ISortOption[]
            {
                // Order should be different from indexes specified above
                alphabeticalSort,
                chronologicalSort
            };

            var displayOrder = new Type[2];
            displayOrder[chronologicalSortIndex] = typeof(ChronologicalSort);
            displayOrder[alphabeticalSortIndex] = typeof(AlphabeticalSort);

            // Act

            var builder = new OptionsListsBuilder
            {
                SortOptions = options,
                SortOptionsDisplayOrder = displayOrder,
                SortOptionsService = new SortOptionsService()
            };

            var lists = builder.CreateOptionsLists();

            // Assert

            Assert.That(lists.DocumentSortOptions.Count, Is.EqualTo(2));

            Assert.That(
                lists.DocumentSortOptions[chronologicalSortIndex],
                Is.EqualTo(chronologicalSort));

            Assert.That(
                lists.DocumentSortOptions[alphabeticalSortIndex],
                Is.EqualTo(alphabeticalSort));
        }

        [Test]
        public void ProjectSortOptionsAppearInSpecifiedOrder()
        {
            // Arrange

            const int reverseAlphabeticalSortIndex = 0;
            const int alphabeticalSortIndex = 1;

            var projectAlphabeticalSort = new ProjectAlphabeticalSort();
            var projectReverseAlphabeticalSort = new ProjectReverseAlphabeticalSort();

            var options = new ISortOption[]
            {
                // Order should be different from indexes specified above
                projectAlphabeticalSort,
                projectReverseAlphabeticalSort
            };

            var displayOrder = new Type[2];
            displayOrder[reverseAlphabeticalSortIndex] = typeof(ProjectReverseAlphabeticalSort);
            displayOrder[alphabeticalSortIndex] = typeof(ProjectAlphabeticalSort);

            // Act

            var builder = new OptionsListsBuilder
            {
                SortOptions = options,
                SortOptionsDisplayOrder = displayOrder,
                SortOptionsService = new SortOptionsService()
            };

            var lists = builder.CreateOptionsLists();

            // Assert

            Assert.That(lists.ProjectSortOptions.Count, Is.EqualTo(2));

            Assert.That(
                lists.ProjectSortOptions[reverseAlphabeticalSortIndex],
                Is.EqualTo(projectReverseAlphabeticalSort));

            Assert.That(
                lists.ProjectSortOptions[alphabeticalSortIndex],
                Is.EqualTo(projectAlphabeticalSort));
        }
    }
}
