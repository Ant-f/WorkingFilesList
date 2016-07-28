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
using System.Windows;
using WorkingFilesList.ToolWindow.View.Converter;

namespace WorkingFilesList.ToolWindow.Test.View.Converter
{
    [TestFixture]
    public class BoolToFontWeightTests
    {
        [Test]
        public void TrueConvertsToBold()
        {
            // Arrange

            var converter = new BoolToFontWeight();

            // Act

            var fontWeight = converter.Convert(true, null, null, null);

            // Assert

            Assert.That(fontWeight, Is.EqualTo(FontWeights.Bold));
        }

        [Test]
        public void FalseConvertsToNormal()
        {
            // Arrange

            var converter = new BoolToFontWeight();

            // Act

            var fontWeight = converter.Convert(false, null, null, null);

            // Assert

            Assert.That(fontWeight, Is.EqualTo(FontWeights.Normal));
        }

        [Test]
        public void BoldConvertsBackToTrue()
        {
            // Arrange

            var converter = new BoolToFontWeight();

            // Act

            var fontWeight = converter.ConvertBack(FontWeights.Bold, null, null, null);

            // Assert

            Assert.That(fontWeight, Is.EqualTo(true));
        }

        [Test]
        public void NormalConvertsBackToFalse()
        {
            // Arrange

            var converter = new BoolToFontWeight();

            // Act

            var fontWeight = converter.ConvertBack(FontWeights.Normal, null, null, null);

            // Assert

            Assert.That(fontWeight, Is.EqualTo(false));
        }
    }
}
