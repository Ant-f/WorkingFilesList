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
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Service.Locator;

namespace WorkingFilesList.Test
{
    [TestFixture]
    public class WorkingFilesWindowPackageTests
    {
        [Test]
        public void InitializeServicesSubscribesToDte2Events()
        {
            // Arrange

            var package = new WorkingFilesWindowPackage();
            var kernel = new MoqMockingKernel();

            var events2 = Mock.Of<Events2>();

            var dte2 = Mock.Of<DTE2>(d => d.Events == events2);
            kernel.Bind<DTE2>().ToConstant(dte2);

            var subscriberMock = new Mock<IDteEventsSubscriber>();
            kernel.Bind<IDteEventsSubscriber>().ToConstant(subscriberMock.Object);

            // Act

            package.InitializeServices(kernel);

            // Assert

            subscriberMock.Verify(s => s.SubscribeTo(events2));
        }

        /// <summary>
        /// Properties in <see cref="ViewModelService"/> are injected by
        /// resolving an instance, and can be used in subsequent instances
        /// created by XAML
        /// </summary>
        [Test]
        public void InitializeServicesResolvesViewModelServiceInstance()
        {
            // Arrange

            var serviceResolved = false;

            var package = new WorkingFilesWindowPackage();
            var kernel = new MoqMockingKernel();

            var events2 = Mock.Of<Events2>();

            var dte2 = Mock.Of<DTE2>(d => d.Events == events2);
            kernel.Bind<DTE2>().ToConstant(dte2);

            kernel.Bind<ViewModelService>().ToMethod(context =>
            {
                serviceResolved = true;
                return new ViewModelService();
            });

            // Act

            package.InitializeServices(kernel);

            // Assert

            Assert.IsTrue(serviceResolved);
        }

        [Test]
        public void InitializeServicesSynchronizesDocuments()
        {
            // Arrange

            var package = new WorkingFilesWindowPackage();
            var kernel = new MoqMockingKernel();

            var documents = Mock.Of<Documents>();

            var dte2 = Mock.Of<DTE2>(d =>
                d.Events == Mock.Of<Events2>() &&
                d.Documents == documents);

            kernel.Bind<DTE2>().ToConstant(dte2);

            var subscriberMock = new Mock<IDteEventsSubscriber>();
            kernel.Bind<IDteEventsSubscriber>().ToConstant(subscriberMock.Object);

            var managerMock = new Mock<IDocumentMetadataManager>();
            kernel.Bind<IDocumentMetadataManager>().ToConstant(managerMock.Object);

            // Act

            package.InitializeServices(kernel);

            // Assert

            managerMock.Verify(m => m.Synchronize(documents));
        }
    }
}
