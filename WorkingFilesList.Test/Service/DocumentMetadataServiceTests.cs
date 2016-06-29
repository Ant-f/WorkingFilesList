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
using WorkingFilesList.Model;
using WorkingFilesList.Service;

namespace WorkingFilesList.Test.Service
{
    [TestFixture]
    public class DocumentMetadataServiceTests
    {
        [Test]
        public void UpsertAddsDocumentMetadataToListIfFullPathDoesNotExist()
        {
            // Arrange

            const string documentName = "DocumentName";

            var service = new DocumentMetadataService();

            // Act

            service.Upsert(documentName);

            // Assert

            Assert.That(service.ActiveDocumentMetadata.Count, Is.EqualTo(1));
            Assert.That(
                ((DocumentMetadata) service.ActiveDocumentMetadata.GetItemAt(0)).FullName,
                Is.EqualTo(documentName));
        }

        [Test]
        public void UpsertDoesNotAddDocumentMetadataToListIfFullPathAlreadyExist()
        {
            // Arrange

            const string documentName = "DocumentName";

            var service = new DocumentMetadataService();

            // Act

            service.Upsert(documentName);
            service.Upsert(documentName);

            // Assert

            Assert.That(service.ActiveDocumentMetadata.Count, Is.EqualTo(1));
            Assert.That(
                ((DocumentMetadata) service.ActiveDocumentMetadata.GetItemAt(0)).FullName,
                Is.EqualTo(documentName));
        }
    }
}
