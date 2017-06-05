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
using WorkingFilesList.Core.Model;

namespace WorkingFilesList.Core.Test.Model
{
    [TestFixture]
    public class DocumentMetadataTests
    {
        [Test]
        public void PinOrderIsMinusTwoOnCreation()
        {
            // Arrange, Act

            var metadata = new DocumentMetadata(
                new DocumentMetadataInfo(),
                string.Empty,
                null);

            // Assert

            Assert.That(metadata.PinOrder, Is.EqualTo(-2));
        }

        [TestCase(-2, false)]
        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(5, true)]
        [TestCase(12, true)]
        public void IsPinnedCorrespondsToPinOrder(int pinOrder, bool expectedIsPinned)
        {
            // Arrange

            var metadata = new DocumentMetadata(
                new DocumentMetadataInfo(),
                string.Empty,
                null);

            // Act

            metadata.PinOrder = pinOrder;

            // Assert

            Assert.That(metadata.IsPinned, Is.EqualTo(expectedIsPinned));
        }

        [Test]
        public void SettingDisplayNamePreHighlightToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const string preHighlight = "PreHighlight";
            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                DisplayNamePreHighlight = preHighlight
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.DisplayNamePreHighlight = preHighlight;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingDisplayNamePreHighlightToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                DisplayNamePreHighlight = "PreHighlight"
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.DisplayNamePreHighlight = "NewPreHighlight";
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }

        [Test]
        public void SettingDisplayNameHighlightToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const string highlight = "Highlight";
            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                DisplayNameHighlight = highlight
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.DisplayNameHighlight = highlight;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingDisplayNameHighlightToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                DisplayNameHighlight = "Highlight"
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.DisplayNameHighlight = "NewHighlight";
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }

        [Test]
        public void SettingDisplayNamePostHighlightToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const string postHighlight = "PostHighlight";
            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                DisplayNamePostHighlight = postHighlight
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.DisplayNamePostHighlight = postHighlight;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingDisplayNamePostHighlightToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                DisplayNamePostHighlight = "PostHighlight"
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.DisplayNamePostHighlight = "NewPostHighlight";
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

        [Test]
        public void DisplayNameIsConcatenationOfDisplayNameHighlightProperties()
        {
            // Arrange

            const string preHighlight = "[PreHighlight]";
            const string highlight = "[Highlight]";
            const string postHighlight = "[PostHighlight]";

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                DisplayNamePreHighlight = preHighlight,
                DisplayNameHighlight = highlight,
                DisplayNamePostHighlight = postHighlight
            };

            // Assert

            var expected = $"{preHighlight}{highlight}{postHighlight}";
            Assert.That(metadata.DisplayName, Is.EqualTo(expected));
        }

        [Test]
        public void SettingPinOrderToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const int pinOrder = 3;
            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                PinOrder = pinOrder
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.PinOrder = pinOrder;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingPinOrderToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                PinOrder = 3
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.PinOrder = 7;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }

        [TestCase(0, 2, false)]
        [TestCase(-2, -2, false)]
        [TestCase(2, -2, true)]
        [TestCase(-2, 2, true)]
        public void SettingPinOrderRaisesIsPinnedPropertyChanged(
            int initialPinOrder,
            int newPinOrder,
            bool shouldRaisePropertyChanged)
        {
            // Arrange

            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                PinOrder = initialPinOrder
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                if (e.PropertyName == nameof(DocumentMetadata.IsPinned))
                {
                    propertyChangedRaised = true;
                }
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.PinOrder = newPinOrder;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.That(propertyChangedRaised, Is.EqualTo(shouldRaisePropertyChanged));
        }

        [Test]
        public void SettingReorderingDirectionToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const Direction reorderingDirection = Direction.Up;
            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                ReorderingDirection = reorderingDirection
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.ReorderingDirection = reorderingDirection;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingReorderingDirectionToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                ReorderingDirection = Direction.None
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.ReorderingDirection = Direction.Up;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }

        [Test]
        public void SettingIsReorderingToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const bool isReordering = true;
            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                IsReordering = isReordering
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.IsReordering = isReordering;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingIsReorderingToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            var propertyChangedRaised = false;

            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                IsReordering = false
            };

            var handler = new PropertyChangedEventHandler((s, e) =>
            {
                propertyChangedRaised = true;
            });

            metadata.PropertyChanged += handler;

            // Act

            metadata.IsReordering = true;
            metadata.PropertyChanged -= handler;

            // Assert

            Assert.IsTrue(propertyChangedRaised);
        }
    }
}
