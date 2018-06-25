// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2018 Anthony Fung

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Service;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;

namespace WorkingFilesList.ToolWindow.Test.Service
{
    [TestFixture]
    public class PinnedItemStorageServiceTests
    {
        [Test]
        public void ReaderIsDisposed()
        {
            // Arrange

            using (var reader = new TestingTextReader())
            {
                var ioService = Mock.Of<IIOService>(s =>
                    s.GetReader(It.IsAny<string>()) == reader);

                var service = new PinnedItemStorageService(ioService);

                // Act

                service.Read("FullName");

                // Assert

                Assert.IsTrue(reader.DisposeInvoked);
            }
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void ExceptionIsThrownWhenFullNameIsEmptyWhenGettingReader(string fullName)
        {
            // Arrange

            var service = new PinnedItemStorageService(
                Mock.Of<IIOService>());

            // Act, Assert

            Assert.Throws<ArgumentException>(() =>
                service.Read(fullName));
        }

        [Test]
        public void ReadReturnsExistingMetadataInfo()
        {
            // Arrange

            const string savedData =
                "[{\"FullName\":\"ItemFullName\",\"ProjectDisplayName\":\"ItemProjectDisplayName\",\"ProjectFullName\":\"ItemProjectFullName\"}]\r\n";

            var ioService = Mock.Of<IIOService>(s =>
                s.GetReader(It.IsAny<string>()) == Mock.Of<TextReader>(r =>
                    r.ReadToEnd() == savedData));

            var service = new PinnedItemStorageService(ioService);

            // Act

            var info = service.Read("FullName");

            // Assert

            var item = info.Single();

            Assert.AreEqual("ItemFullName", item.FullName);
            Assert.AreEqual("ItemProjectDisplayName", item.ProjectDisplayName);
            Assert.AreEqual("ItemProjectFullName", item.ProjectFullName);
        }

        [Test]
        public void ReadReturnsEmptyMetadataInfoListIfFileDoesNotExist()
        {
            // Arrange

            var service = new PinnedItemStorageService(
                Mock.Of<IIOService>());

            // Act

            var info = service.Read("FullName");

            // Assert

            Assert.IsEmpty(info);
        }

        [Test]
        public void FullNameIsHashedWhenRequestingReader()
        {
            // Arrange

            using (var reader = new TestingTextReader())
            {
                var ioService = Mock.Of<IIOService>(s =>
                    s.GetReader(It.IsAny<string>()) == reader);

                var service = new PinnedItemStorageService(ioService);

                // Act

                service.Read("FullName");

                // Assert

                // 90C4E69DCE7A11BFCB705B1A6540A847AAC993FA is SHA1 of FullName

                var expectedPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    "WorkingFilesList",
                    "1.3_90C4E69DCE7A11BFCB705B1A6540A847AAC993FA.json");

                Mock.Get(ioService).Verify(s =>
                    s.GetReader(expectedPath));
            }
        }

        [Test]
        public void WriterIsDisposed()
        {
            // Arrange

            using (var writer = new TestingTextWriter())
            {
                var ioService = Mock.Of<IIOService>(s =>
                    s.GetWriter(It.IsAny<string>()) == writer);

                var service = new PinnedItemStorageService(ioService);

                // Act

                service.Write(new DocumentMetadata[0], "FullName");

                // Assert

                Assert.IsTrue(writer.DisposeInvoked);
            }
        }

        [Test]
        public void OutputDirectoryIsCreated()
        {
            // Arrange

            using (var writer = new TestingTextWriter())
            {
                var ioService = Mock.Of<IIOService>(s =>
                    s.GetWriter(It.IsAny<string>()) == writer);

                var service = new PinnedItemStorageService(ioService);

                // Act

                service.Write(new DocumentMetadata[0], "FullName");

                // Assert

                var expectedPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    "WorkingFilesList");

                Mock.Get(ioService).Verify(s =>
                    s.CreateDirectory(expectedPath));
            }
        }

        [Test]
        public void FullNameIsHashedWhenRequestingWriter()
        {
            // Arrange

            using (var writer = new TestingTextWriter())
            {
                var ioService = Mock.Of<IIOService>(s =>
                    s.GetWriter(It.IsAny<string>()) == writer);

                var service = new PinnedItemStorageService(ioService);

                // Act

                service.Write(new DocumentMetadata[0], "FullName");

                // Assert

                // 90C4E69DCE7A11BFCB705B1A6540A847AAC993FA is SHA1 of FullName

                var expectedPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    "WorkingFilesList",
                    "1.3_90C4E69DCE7A11BFCB705B1A6540A847AAC993FA.json");

                Mock.Get(ioService).Verify(s =>
                    s.GetWriter(expectedPath));
            }
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void ExceptionIsThrownWhenFullNameIsEmptyWhenGettingWriter(string fullName)
        {
            // Arrange

            var service = new PinnedItemStorageService(
                Mock.Of<IIOService>());

            // Act, Assert

            Assert.Throws<ArgumentException>(() =>
                service.Write(new DocumentMetadata[0], fullName));
        }

        [Test]
        public void MetadataIsWritten()
        {
            // Arrange

            var metadata = new[]
            {
                new DocumentMetadata(
                    new DocumentMetadataInfo
                    {
                        FullName = "ItemFullName",
                        ProjectDisplayName = "ItemProjectDisplayName",
                        ProjectFullName = "ItemProjectFullName"
                    }, null, null)
            };

            using (var writer = new TestingTextWriter())
            {
                var ioService = Mock.Of<IIOService>(s =>
                    s.GetWriter(It.IsAny<string>()) == writer);

                var service = new PinnedItemStorageService(ioService);

                // Act

                service.Write(metadata, "FullName");

                // Assert

                Assert.AreEqual(
                    "[{\"FullName\":\"ItemFullName\",\"ProjectDisplayName\":\"ItemProjectDisplayName\",\"ProjectFullName\":\"ItemProjectFullName\"}]\r\n",
                    writer.WrittenData);
            }
        }

        [Test]
        public void OutputDirectoryIsCreatedBeforeDataIsWritten()
        {
            // Arrange

            var directoryCreated = false;
            var directoryCreatedBeforeWrite = false;

            using (var writer = Mock.Of<TextWriter>())
            {
                Mock.Get(writer)
                    .Setup(w => w.WriteLine(It.IsAny<string>()))
                    .Callback(() => directoryCreatedBeforeWrite = directoryCreated);

                var ioService = Mock.Of<IIOService>(s =>
                    s.GetWriter(It.IsAny<string>()) == writer);

                Mock.Get(ioService)
                    .Setup(s => s.CreateDirectory(It.IsAny<string>()))
                    .Callback(() => directoryCreated = true);

                var service = new PinnedItemStorageService(ioService);

                // Act

                service.Write(new DocumentMetadata[0], "FullName");

                // Assert

                Assert.IsTrue(directoryCreatedBeforeWrite);
            }
        }
    }
}
