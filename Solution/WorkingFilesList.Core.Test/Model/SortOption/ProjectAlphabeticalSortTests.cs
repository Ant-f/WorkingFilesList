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
using System.Collections.Generic;
using System.Windows.Data;
using WorkingFilesList.Core.Model;
using WorkingFilesList.Core.Model.SortOption;

namespace WorkingFilesList.Core.Test.Model.SortOption
{
    [TestFixture]
    public class ProjectAlphabeticalSortTests
    {
        private static DocumentMetadata CreateDocumentMetadata(string projectDisplayName)
        {
            var info = new DocumentMetadataInfo
            {
                ProjectDisplayName = projectDisplayName
            };

            var metadata = new DocumentMetadata(info, string.Empty, null);
            return metadata;
        }

        [Test]
        public void BeginningOfAlphabetAppearsAtStartOfSortedList()
        {
            // Arrange

            const string projectNameA = "A";

            var metadataList = new List<DocumentMetadata>
            {
                CreateDocumentMetadata("B"),
                CreateDocumentMetadata("C"),
                CreateDocumentMetadata(projectNameA)
            };

            var sortOption = new ProjectAlphabeticalSort();
            var sortDescription = sortOption.GetSortDescription();
            var view = new ListCollectionView(metadataList);

            // Act

            view.SortDescriptions.Add(sortDescription);

            // Assert

            view.MoveCurrentToFirst();
            var firstItem = (DocumentMetadata)view.CurrentItem;

            Assert.That(
                firstItem.ProjectNames.DisplayName,
                Is.EqualTo(projectNameA));
        }

        [Test]
        public void HasSortDescriptionIsTrue()
        {
            // Arrange

            var sortOption = new ProjectAlphabeticalSort();

            // Assert

            Assert.IsTrue(sortOption.HasSortDescription);
        }

        [Test]
        public void ApplicableTypeIsCorrect()
        {
            // Arrange

            var sortOption = new ProjectAlphabeticalSort();

            // Act

            var isProjectType =
                sortOption.ApplicableType == ProjectItemType.Project;

            // Assert

            Assert.IsTrue(isProjectType);
        }
    }
}
