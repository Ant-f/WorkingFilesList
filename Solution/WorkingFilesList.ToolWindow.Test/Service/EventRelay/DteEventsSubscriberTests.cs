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
using EnvDTE80;
using Moq;
using NUnit.Framework;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;

namespace WorkingFilesList.ToolWindow.Test.Service.EventRelay
{
    [TestFixture]
    public class DteEventsSubscriberTests
    {
        private class TestingMocks
        {
            public Mock<Events2> Events2Mock { get; }
                = new Mock<Events2>();

            public Mock<ProjectItemsEvents> ProjectItemsEventsMock { get; }
                = new Mock<ProjectItemsEvents>();

            public Mock<SolutionEvents> SolutionEventsMock { get; }
                = new Mock<SolutionEvents>();

            public Mock<WindowEvents> WindowEventsMock { get; }
                = new Mock<WindowEvents>();

            public TestingMocks()
            {
                Events2Mock
                    .Setup(e => e.ProjectItemsEvents)
                    .Returns(ProjectItemsEventsMock.Object);

                Events2Mock
                    .Setup(e => e.SolutionEvents)
                    .Returns(SolutionEventsMock.Object);

                Events2Mock
                    .Setup(e => e.get_WindowEvents(It.IsAny<Window>()))
                    .Returns(WindowEventsMock.Object);
            }
        }

        [Test]
        public void SubscribesToWindowActivated()
        {
            // Arrange

            var mocks = new TestingMocks();
            var builder = new DteEventsSubscriberBuilder();

            var subscriber = builder.CreateDteEventsSubscriber();
            subscriber.SubscribeTo(mocks.Events2Mock.Object);

            // Act

            mocks.WindowEventsMock.Raise(w =>
                w.WindowActivated += null,
                    null,
                    null);

            // Assert

            builder.WindowEventsServiceMock.Verify(w => w
                .WindowActivated(
                    It.IsAny<Window>(),
                    It.IsAny<Window>()));
        }

        [Test]
        public void SubscribesToWindowCreated()
        {
            // Arrange

            var mocks = new TestingMocks();
            var builder = new DteEventsSubscriberBuilder();

            var subscriber = builder.CreateDteEventsSubscriber();
            subscriber.SubscribeTo(mocks.Events2Mock.Object);

            // Act

            mocks.WindowEventsMock.Raise(w =>
                w.WindowCreated += null,
                    Mock.Of<Window>());

            // Assert

            builder.WindowEventsServiceMock.Verify(w => w
                .WindowCreated(It.IsAny<Window>()));
        }

        [Test]
        public void SubscribesToWindowClosing()
        {
            // Arrange

            var mocks = new TestingMocks();
            var builder = new DteEventsSubscriberBuilder();

            var subscriber = builder.CreateDteEventsSubscriber();
            subscriber.SubscribeTo(mocks.Events2Mock.Object);

            // Act

            mocks.WindowEventsMock.Raise(w =>
                w.WindowClosing += null,
                    Mock.Of<Window>());

            // Assert

            builder.WindowEventsServiceMock.Verify(w => w
                .WindowClosing(It.IsAny<Window>()));
        }

        [Test]
        public void SubscribesToItemRenamed()
        {
            // Arrange

            var mocks = new TestingMocks();
            var builder = new DteEventsSubscriberBuilder();

            var subscriber = builder.CreateDteEventsSubscriber();
            subscriber.SubscribeTo(mocks.Events2Mock.Object);

            // Act

            mocks.ProjectItemsEventsMock.Raise(w =>
                w.ItemRenamed += null,
                    Mock.Of<ProjectItem>(),
                    string.Empty);

            // Assert

            builder.ProjectItemsEventsServiceMock.Verify(w => w
                .ItemRenamed(
                    It.IsAny<ProjectItem>(),
                    It.IsAny<string>()));
        }

        [Test]
        public void SubscribesToAfterClosing()
        {
            // Arrange

            var mocks = new TestingMocks();
            var builder = new DteEventsSubscriberBuilder();

            var subscriber = builder.CreateDteEventsSubscriber();
            subscriber.SubscribeTo(mocks.Events2Mock.Object);

            // Act

            mocks.SolutionEventsMock.Raise(s =>
                s.AfterClosing += null);

            // Assert

            builder.SolutionEventsServiceMock.Verify(w => w.AfterClosing());
        }

        [Test]
        public void SubscribesToProjectRenamed()
        {
            // Arrange

            var mocks = new TestingMocks();
            var builder = new DteEventsSubscriberBuilder();

            var subscriber = builder.CreateDteEventsSubscriber();
            subscriber.SubscribeTo(mocks.Events2Mock.Object);

            // Act

            mocks.SolutionEventsMock.Raise(s =>
                s.ProjectRenamed += null,
                    Mock.Of<Project>(),
                    null);

            // Assert

            builder.SolutionEventsServiceMock.Verify(w =>
                w.ProjectRenamed(
                    It.IsAny<Project>(),
                    It.IsAny<string>()));
        }

        [Test]
        public void SubscribesToProjectRemoved()
        {
            // Arrange

            var mocks = new TestingMocks();
            var builder = new DteEventsSubscriberBuilder();

            var subscriber = builder.CreateDteEventsSubscriber();
            subscriber.SubscribeTo(mocks.Events2Mock.Object);

            // Act

            mocks.SolutionEventsMock.Raise(s =>
                s.ProjectRemoved += null,
                    Mock.Of<Project>());

            // Assert

            builder.SolutionEventsServiceMock.Verify(w =>
                w.ProjectRemoved(
                    It.IsAny<Project>()));
        }
    }
}
