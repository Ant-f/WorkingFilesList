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

using System;
using NUnit.Framework;
using WorkingFilesList.ToolWindow.Model.SortOption;

namespace WorkingFilesList.ToolWindow.Test.Model.SortOption
{
    [TestFixture]
    public class DisableSortingTests
    {
        [Test]
        public void GetSortDescriptionThrowsNotSupportedException()
        {
            // Arrange

            var sortOption = new DisableSorting();

            // Assert

            Assert.Throws<NotSupportedException>(() => sortOption.GetSortDescription());
        }

        [Test]
        public void HasSortDescriptionIsFalse()
        {
            // Arrange

            var sortOption = new DisableSorting();

            // Assert

            Assert.IsFalse(sortOption.HasSortDescription);
        }

        [TestCase(ProjectItemType.Document, false)]
        [TestCase(ProjectItemType.Project, true)]
        public void ApplicableTypesAreCorrect(ProjectItemType type, bool isApplicable)
        {
            // Arrange

            var sortOption = new DisableSorting();

            // Act

            var hasFlag = sortOption.ApplicableTypes.HasFlag(type);

            // Assert

            Assert.That(hasFlag, Is.EqualTo(isApplicable));
        }
    }
}
