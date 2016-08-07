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

using NUnit.Framework;
using System.ComponentModel;
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
            var metadata = new DocumentMetadata(info, string.Empty)
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
            var metadata = new DocumentMetadata(info, string.Empty)
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
            var metadata = new DocumentMetadata(info, string.Empty)
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
            var metadata = new DocumentMetadata(info, string.Empty)
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
        public void SettingUseOrderToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const double value = 0.7;
            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty)
            {
                UseOrder = value
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.UseOrder = value;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingUseOrderToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty)
            {
                UseOrder = 0.3
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.UseOrder = 0.7;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }
    }
}
