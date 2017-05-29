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
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;
using WorkingFilesList.ToolWindow.ViewModel.Command;
using static WorkingFilesList.ToolWindow.Test.TestingInfrastructure.CommonMethods;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.Command
{
    [TestFixture]
    public class ActivateWindowTests
    {
        [Test]
        public void CanExecuteReturnsTrue()
        {
            // Arrange

            var command = new ActivateWindow(Mock.Of<DTE2>());

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

            var command = new ActivateWindow(dte2);

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

            var command = new ActivateWindow(dteMock.Object);

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

            var solution = Mock.Of<Solution>(s =>
                s.FindProjectItem(info.FullName) == Mock.Of<ProjectItem>(p =>
                    p.Open(Constants.vsViewKindPrimary) == window));

            var dte2 = Mock.Of<DTE2>(d =>
                d.Documents == documents &&
                d.Solution == solution);

            var builder = new DocumentMetadataFactoryBuilder();
            var factory = builder.CreateDocumentMetadataFactory(true);
            var metadata = factory.Create(info);

            var command = new ActivateWindow(dte2);

            // Act

            command.Execute(metadata);

            // Assert

            Mock.Get(solution).Verify(s =>
                s.FindProjectItem(info.FullName));

            Mock.Get(window).Verify(w => w.Activate());
        }
    }
}
