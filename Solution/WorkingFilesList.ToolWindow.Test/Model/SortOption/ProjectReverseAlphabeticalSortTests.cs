﻿// Working Files List
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
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.Model.SortOption;

namespace WorkingFilesList.ToolWindow.Test.Model.SortOption
{
    [TestFixture]
    public class ProjectReverseAlphabeticalSortTests
    {
        private static DocumentMetadata CreateDocumentMetadata(string projectDisplayName)
        {
            var info = new DocumentMetadataInfo
            {
                ProjectDisplayName = projectDisplayName
            };

            var metadata = new DocumentMetadata(info, string.Empty);
            return metadata;
        }

        [Test]
        public void EndOfAlphabetAppearsAtStartOfSortedList()
        {
            // Arrange

            const string projectNameC = "C";

            var metadataList = new List<DocumentMetadata>
            {
                CreateDocumentMetadata("B"),
                CreateDocumentMetadata(projectNameC),
                CreateDocumentMetadata("A")
            };

            var sortOption = new ProjectReverseAlphabeticalSort();
            var sortDescription = sortOption.GetSortDescription();
            var view = new ListCollectionView(metadataList);

            // Act

            view.SortDescriptions.Add(sortDescription);

            // Assert

            view.MoveCurrentToFirst();
            var firstItem = (DocumentMetadata) view.CurrentItem;

            Assert.That(
                firstItem.ProjectNames.DisplayName,
                Is.EqualTo(projectNameC));
        }

        [Test]
        public void HasSortDescriptionIsTrue()
        {
            // Arrange

            var sortOption = new ProjectReverseAlphabeticalSort();

            // Assert

            Assert.IsTrue(sortOption.HasSortDescription);
        }

        [Test]
        public void ApplicableTypeIsCorrect()
        {
            // Arrange

            var sortOption = new ProjectReverseAlphabeticalSort();

            // Act

            var isProjectType =
                sortOption.ApplicableType == ProjectItemType.Project;

            // Assert

            Assert.IsTrue(isProjectType);
        }
    }
}
