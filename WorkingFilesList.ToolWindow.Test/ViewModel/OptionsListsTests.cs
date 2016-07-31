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
using System.Collections.Generic;
using System.ComponentModel;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model.SortOption;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;
using WorkingFilesList.ToolWindow.ViewModel;

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

            var input = new List<ISortOption>
            {
                documentSortOption,
                projectSortOption
            };

            // Act

            var options = new OptionsLists(input);

            // Assert

            Assert.That(options.DocumentSortOptions.Count, Is.EqualTo(1));
            Assert.That(options.DocumentSortOptions[0], Is.EqualTo(documentSortOption));
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

            var input = new List<ISortOption>
            {
                documentSortOption,
                projectSortOption
            };

            // Act

            var options = new OptionsLists(input);

            // Assert

            Assert.That(options.ProjectSortOptions.Count, Is.EqualTo(1));
            Assert.That(options.ProjectSortOptions[0], Is.EqualTo(projectSortOption));
        }
    }
}
