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
using System.ComponentModel;
using WorkingFilesList.Core.Model.SortOption;
using WorkingFilesList.Core.Test.TestingInfrastructure;

namespace WorkingFilesList.Core.Test.Model.SortOption
{
    [TestFixture]
    public class SortOptionBaseTests
    {
        [Test]
        public void SortDescriptionPropertyNameMatchesConstructorParameter()
        {
            // Arrange

            const string propertyName = "PropertyName";

            var option = new TestingSortOption(
                "DisplayName",
                propertyName,
                ListSortDirection.Ascending,
                ProjectItemType.Document);

            // Act

            var sortDescription = option.GetSortDescription();

            // Assert

            Assert.That(sortDescription.PropertyName, Is.EqualTo(propertyName));
        }

        [Test]
        public void SortDescriptionSortDirectionMatchesConstructorParameter()
        {
            // Arrange

            const ListSortDirection sortDirection = ListSortDirection.Descending;

            var option = new TestingSortOption(
                "DisplayName",
                "PropertyName",
                sortDirection,
                ProjectItemType.Document);

            // Act

            var sortDescription = option.GetSortDescription();

            // Assert

            Assert.That(sortDescription.Direction, Is.EqualTo(sortDirection));
        }
    }
}
