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

using NUnit.Framework;
using System.ComponentModel;
using System.Windows.Media;
using WorkingFilesList.ToolWindow.Model;

namespace WorkingFilesList.ToolWindow.Test.Model
{
    [TestFixture]
    public class DocumentMetadataTests
    {
        [Test]
        public void SettingDisplayNameToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const string displayName = "DisplayName";
            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                DisplayName = displayName
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.DisplayName = displayName;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingDisplayNameToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                DisplayName = "DisplayName"
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.DisplayName = "NewDisplayName";
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }

        [Test]
        public void SettingIsActiveToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const bool isActive = true;
            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                IsActive = isActive
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.IsActive = isActive;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingIsActiveToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                IsActive = true
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.IsActive = false;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }

        [Test]
        public void SettingUsageOrderToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const double value = 0.7;
            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                UsageOrder = value
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.UsageOrder = value;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingUsageOrderToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                UsageOrder = 0.3
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.UsageOrder = 0.7;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }

        [Test]
        public void ProjectBrushIsTransparentIfNull()
        {
            // Arrange

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                ProjectBrush = null
            };

            // Assert

            Assert.That(metadata.ProjectBrush, Is.EqualTo(Brushes.Transparent));
        }

        [Test]
        public void SettingProjectBrushToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            var projectBrush = Brushes.MidnightBlue;
            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                ProjectBrush = projectBrush
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.ProjectBrush = projectBrush;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingProjectBrushToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                ProjectBrush = Brushes.MidnightBlue
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.ProjectBrush = Brushes.White;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }
    }
}
