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

using System;
using NUnit.Framework;
using WorkingFilesList.Core.Model.SortOption;

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

        [Test]
        public void ApplicableTypeIsCorrect()
        {
            // Arrange

            var sortOption = new DisableSorting();

            // Act

            var isProjectType =
                sortOption.ApplicableType == ProjectItemType.Project;

            // Assert

            Assert.IsTrue(isProjectType);
        }
    }
}
