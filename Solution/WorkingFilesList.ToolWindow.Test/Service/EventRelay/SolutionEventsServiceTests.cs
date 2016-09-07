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

using EnvDTE;
using Moq;
using NUnit.Framework;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Service.EventRelay;

namespace WorkingFilesList.ToolWindow.Test.Service.EventRelay
{
    [TestFixture]
    public class SolutionEventsServiceTests
    {
        [Test]
        public void ProjectBrushIdCollectionIsClearedAfterSolutionClosing()
        {
            // Arrange

            var projectBrushServiceMock = new Mock<IProjectBrushService>();
            var service = new SolutionEventsService(projectBrushServiceMock.Object);

            // Act

            service.AfterClosing();

            // Assert

            projectBrushServiceMock.Verify(p =>
                p.ClearBrushIdCollection());
        }

        [Test]
        public void BrushIdIsUpdatedWithNewAndOldProjectNamesAfterRenamingProject()
        {
            // Arrange

            const string oldName = "OldName";
            const string newName = "NewName";

            var projectBrushServiceMock = new Mock<IProjectBrushService>();
            var service = new SolutionEventsService(projectBrushServiceMock.Object);
            var project = Mock.Of<Project>(p => p.FullName == newName);

            // Act

            service.ProjectRenamed(project, oldName);

            // Assert

            projectBrushServiceMock.Verify(p =>
                p.UpdateBrushId(
                    oldName,
                    newName));
        }
    }
}
