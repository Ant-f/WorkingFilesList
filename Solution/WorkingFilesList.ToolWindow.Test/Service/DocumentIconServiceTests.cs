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

using System;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using NUnit.Framework;
using WorkingFilesList.ToolWindow.Service;

namespace WorkingFilesList.ToolWindow.Test.Service
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class DocumentIconServiceTests
    {
        /// <summary>
        /// <see cref="Uri"/> instances in <see cref="DocumentIconService"/>
        /// are in the pack URI format. Creating a <see cref="FrameworkElement"/>
        /// initializes enough of the framework to enable reading pack URIs
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            new FrameworkElement();
        }

        [TestCase(".config", "ConfigurationFile_16x.png")]
        [TestCase(".cpp", "CPP_16x.png")]
        [TestCase(".cs", "CS_16x.png")]
        [TestCase(".fs", "FS_16x.png")]
        [TestCase(".js", "JS_16x.png")]
        [TestCase(".ts", "TS_16x.png")]
        [TestCase(".vb", "VB_16x.png")]
        [TestCase(".xaml", "WPFPage_16x.png")]
        public void UriIsAppropriateForFileExtension(
            string fileExtension,
            string expectedPathSegment)
        {
            // Arrange

            var service = new DocumentIconService();

            // Act

            var icon = (BitmapImage) service.GetIcon(fileExtension);

            // Assert

            Assert.IsTrue(icon.UriSource.AbsolutePath.Contains(expectedPathSegment));
        }

        [Test]
        public void FileExtensionIsCaseInsensitive()
        {
            // Arrange

            var service = new DocumentIconService();

            // Act

            var lowerCaseIcon = (BitmapImage) service.GetIcon(".cs");
            var upperCaseIcon = (BitmapImage) service.GetIcon(".CS");

            // Assert

            Assert.That(lowerCaseIcon, Is.EqualTo(upperCaseIcon));
        }

        [Test]
        public void UnspecifiedExtensionReturnsDefaultIcon()
        {
            // Arrange

            var service = new DocumentIconService();

            // Act

            var icon = (BitmapImage)service.GetIcon(".FileExtension");

            // Assert

            Assert.IsTrue(icon.UriSource.AbsolutePath.Contains("Document_16x.png"));
        }
    }
}
