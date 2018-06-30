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
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Service;

namespace WorkingFilesList.ToolWindow.Test.Service
{
    [TestFixture]
    public class ProjectItemServiceTests
    {
        [Test]
        public void FindProjectItemReturnsItemIfFoundInProjectAndExists()
        {
            // Arrange

            const string itemName = "ItemName";
            const string fullName = "FullName";

            var ioService = Mock.Of<IIOService>(f =>
                f.FileExists(fullName) == true);

            var projectItem = Mock.Of<ProjectItem>(p =>
                p.get_FileNames(1) == fullName);

            var dte = Mock.Of<DTE2>(d =>
                d.Solution == Mock.Of<Solution>(s =>
                    s.FindProjectItem(itemName) == projectItem));

            var service = new ProjectItemService(dte, ioService);

            // Act

            var item = service.FindProjectItem(itemName);

            // Assert

            Mock.Get(ioService).Verify(f => f.FileExists(fullName));
            Assert.AreEqual(projectItem, item);
        }

        [Test]
        public void FindProjectItemReturnsNullIfFoundInProjectButIsNonExistent()
        {
            // Arrange

            const string itemName = "ItemName";
            const string fullName = "FullName";

            var ioService = Mock.Of<IIOService>(f =>
                f.FileExists(It.IsAny<string>()) == false);

            var dte = Mock.Of<DTE2>(d =>
                d.Solution == Mock.Of<Solution>(s =>
                    s.FindProjectItem(itemName) == Mock.Of<ProjectItem>(p =>
                        p.get_FileNames(1) == fullName)));

            var service = new ProjectItemService(dte, ioService);

            // Act

            var item = service.FindProjectItem(itemName);

            // Assert

            Mock.Get(ioService).Verify(f => f.FileExists(It.IsAny<string>()));
            Assert.IsNull(item);
        }

        [Test]
        public void FindProjectItemDoesNotCheckFileExistenceIfItemIsNotFoundInProject()
        {
            // Arrange

            const string itemName = "ItemName";

            var solutionMock = new Mock<Solution>();

            solutionMock
                .Setup(s => s.FindProjectItem(It.IsAny<string>()))
                .Returns<ProjectItem>(null);

            var ioService = Mock.Of<IIOService>();
            var dte = Mock.Of<DTE2>(d => d.Solution == solutionMock.Object);
            var service = new ProjectItemService(dte, ioService);

            // Act

            var item = service.FindProjectItem(itemName);

            // Assert

            Mock.Get(ioService)
                .Verify(f => f.FileExists(It.IsAny<string>()),
                    Times.Never);

            Assert.IsNull(item);
        }
    }
}
