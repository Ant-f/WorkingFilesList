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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model.SortOption;
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
