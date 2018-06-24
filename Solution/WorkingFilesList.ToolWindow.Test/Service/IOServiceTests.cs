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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WorkingFilesList.ToolWindow.Service;

namespace WorkingFilesList.ToolWindow.Test.Service
{
    [TestFixture]
    public class IOServiceTests
    {
        private readonly HashSet<string> _directories = new HashSet<string>();
        private readonly HashSet<string> _paths = new HashSet<string>();

        [OneTimeTearDown]
        public void RemoveCreatedItems()
        {
            if (_paths.Count > 0)
            {
                var pathsToDelete = _paths.Where(File.Exists);

                foreach (var path in pathsToDelete)
                {
                    File.Delete(path);
                }
            }

            if (_directories.Count > 0)
            {
                var directoriesToDelete = _directories.Where(Directory.Exists);

                foreach (var path in directoriesToDelete)
                {
                    Directory.Delete(path);
                }
            }
        }

        [Test]
        public void GetReaderReturnStreamReader()
        {
            // Arrange

            var service = new IOService();
            var path = GetTestDataPath("IOServiceTestFile.txt");

            // Act

            using (var writer = service.GetReader(path))
            {
                // Assert

                Assert.IsInstanceOf<StreamReader>(writer);
            }
        }

        [Test, Explicit]
        public void GetWriterReturnStreamWriter()
        {
            // Arrange

            var service = new IOService();
            var path = GetTestDataPath("IOServiceTestWriteFile.txt");

            _paths.Add(path);

            // Act

            using (var writer = (StreamWriter) service.GetWriter(path))
            {
                // Assert

                Assert.IsInstanceOf<StreamWriter>(writer);
            }
        }

        [Test, Explicit]
        public void CreateDirectoryCreatesDirectory()
        {
            // Arrange

            var service = new IOService();
            var path = GetTestDataPath("IOServiceTestDirectory");

            _directories.Add(path);

            // Act

            service.CreateDirectory(path);

            // Assert

            Assert.IsTrue(Directory.Exists(path));
        }

        private static string GetTestDataPath(string fileName)
        {
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TestingData",
                $"{fileName}");

            return path;
        }
    }
}
