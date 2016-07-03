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

using System.Collections.Generic;
using EnvDTE;
using EnvDTE80;
using Moq;
using NUnit.Framework;
using WorkingFilesList.Model;
using WorkingFilesList.ViewModel.Command;
using static WorkingFilesList.Test.TestingInfrastructure.CommonMethods;

namespace WorkingFilesList.Test.ViewModel.Command
{
    [TestFixture]
    public class CloseDocumentTests
    {
        [Test]
        public void CanExecuteReturnsTrue()
        {
            // Arrange

            var command = new CloseDocument(Mock.Of<DTE2>());

            // Act

            var canExecute = command.CanExecute(null);

            // Assert

            Assert.IsTrue(canExecute);
        }

        [Test]
        public void ExecuteClosesDocument()
        {
            // Arrange

            const string documentName = "DocumentName";

            var documentMock = new Mock<Document>();
            documentMock.Setup(d => d.FullName).Returns(documentName);

            var documentMockList = new List<Document>
            {
                documentMock.Object
            };

            var documents = CreateDocuments(documentMockList);

            var dte2Mock = new Mock<DTE2>();
            dte2Mock.Setup(d => d.Documents).Returns(documents);

            var command = new CloseDocument(dte2Mock.Object);
            var metadata = new DocumentMetadata
            {
                FullName = documentName
            };

            // Act

            command.Execute(metadata);

            // Assert

            documentMock.Verify(d => d.Close(It.IsAny<vsSaveChanges>()));
        }

        [Test]
        public void ExecuteDoesNotThrowExceptionWithNullParameter()
        {
            // Arrange

            var dteMock = new Mock<DTE2>();
            dteMock.Setup(d => d.Documents).Returns<Documents>(null);

            var command = new CloseDocument(dteMock.Object);

            // Act

            Assert.DoesNotThrow(() => command.Execute(null));

            // Assert

            dteMock.Verify(d => d.Documents, Times.Never);
        }
    }
}
