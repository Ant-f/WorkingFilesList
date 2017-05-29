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

using EnvDTE;
using EnvDTE80;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;
using WorkingFilesList.ToolWindow.ViewModel.Command;
using static WorkingFilesList.ToolWindow.Test.TestingInfrastructure.CommonMethods;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.Command
{
    [TestFixture]
    public class ActivateWindowTests
    {
        private static ActivateWindow CreateActivateWindow(
            DTE2 dte2 = null,
            IDocumentMetadataManager documentMetadataManager = null,
            IProjectItemService projectItemService = null)
        {
            var command = new ActivateWindow(
                dte2 ?? Mock.Of<DTE2>(),
                documentMetadataManager ?? Mock.Of<IDocumentMetadataManager>(),
                projectItemService ?? Mock.Of<IProjectItemService>());

            return command;
        }

        [Test]
        public void CanExecuteReturnsTrue()
        {
            // Arrange

            var command = CreateActivateWindow();

            // Act

            var canExecute = command.CanExecute(null);

            // Assert

            Assert.IsTrue(canExecute);
        }

        [Test]
        public void ExecuteActivatesFirstDocumentWindow()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "DocumentName"
            };

            var windowMock = new Mock<Window>();
            var windowList = new List<Window>
            {
                windowMock.Object
            };

            var windowsMock = CreateWindows(windowList);
            var documentMockList = new List<Document>
            {
                Mock.Of<Document>(d =>
                    d.FullName == info.FullName &&
                    d.Windows == windowsMock)
            };

            var documents = CreateDocuments(documentMockList);
            var dte2 = Mock.Of<DTE2>(d => d.Documents == documents);

            var builder = new DocumentMetadataFactoryBuilder();
            var factory = builder.CreateDocumentMetadataFactory(true);
            var metadata = factory.Create(info);

            var command = CreateActivateWindow(dte2);

            // Act

            command.Execute(metadata);

            // Assert

            windowMock.Verify(w => w.Activate());
        }

        [Test]
        public void ExecuteDoesNotThrowExceptionWithNullParameter()
        {
            // Arrange

            var dteMock = new Mock<DTE2>();
            dteMock.Setup(d => d.Documents).Returns<Documents>(null);

            var command = CreateActivateWindow(dteMock.Object);

            // Act

            Assert.DoesNotThrow(() => command.Execute(null));

            // Assert

            dteMock.Verify(d => d.Documents, Times.Never);
        }

        [Test]
        public void ExecuteFindsProjectItemAndOpensWindowIfNoDocumentWindowExists()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "DocumentName"
            };

            var window = Mock.Of<Window>();
            var windowsMock = CreateWindows(new List<Window>());
            var documentMockList = new List<Document>
            {
                Mock.Of<Document>(d =>
                    d.FullName == info.FullName &&
                    d.Windows == windowsMock)
            };

            var documents = CreateDocuments(documentMockList);

            var builder = new DocumentMetadataFactoryBuilder();
            var factory = builder.CreateDocumentMetadataFactory(true);
            var metadata = factory.Create(info);

            var dte2 = Mock.Of<DTE2>(d =>
                d.Documents == documents);

            var projectItemService = Mock.Of<IProjectItemService>(s =>
                s.FindProjectItem(info.FullName) == Mock.Of<ProjectItem>(p =>
                    p.Open(Constants.vsViewKindPrimary) == window));

            var command = CreateActivateWindow(
                dte2: dte2,
                projectItemService: projectItemService);

            // Act

            command.Execute(metadata);

            // Assert

            Mock.Get(projectItemService).Verify(s =>
                s.FindProjectItem(info.FullName));

            Mock.Get(window).Verify(w => w.Activate());
        }

        [Test]
        public void ExecuteSynchronizesDocumentsIfNoWindowActivated()
        {
            // Arrange

            var info = new DocumentMetadataInfo
            {
                FullName = "DocumentName"
            };

            var documentMockList = new List<Document>();
            var documents = CreateDocuments(documentMockList);

            var dte2 = Mock.Of<DTE2>(d =>
                d.Documents == documents &&
                d.Solution == Mock.Of<Solution>());

            var builder = new DocumentMetadataFactoryBuilder();
            var factory = builder.CreateDocumentMetadataFactory(true);
            var metadata = factory.Create(info);

            var documentMetadataManager = Mock.Of<IDocumentMetadataManager>();
            var command = CreateActivateWindow(dte2, documentMetadataManager);

            // Act

            command.Execute(metadata);

            // Assert

            Mock.Get(documentMetadataManager).Verify(d =>
                d.Synchronize(documents, true));
        }
    }
}
