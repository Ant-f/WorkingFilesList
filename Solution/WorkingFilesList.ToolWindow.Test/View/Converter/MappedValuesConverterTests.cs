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
    public class MappedValuesConverterTests
    {
        [Test]
        public void ConvertUsesMappingCollectionToConvertValue()
        {
            // Arrange

            const bool from = false;
            var to = FontWeights.Normal;

            var mapping = new ConverterMapping
            {
                From = from,
                To = to
            };

            var collection = new ConverterMappingCollection();
            collection.MappedValues.Add(mapping);

            var converter = new MappedValuesConverter();

            // Act

            var result = converter.Convert(from, null, collection, null);

            // Assert

            Assert.That(result, Is.EqualTo(to));
        }

        [Test]
        public void ConvertBackUsesMappingCollectionToConvertValue()
        {
            // Arrange

            const bool from = false;
            var to = FontWeights.Normal;

            var mapping = new ConverterMapping
            {
                From = from,
                To = to
            };

            var collection = new ConverterMappingCollection();
            collection.MappedValues.Add(mapping);

            var converter = new MappedValuesConverter();

            // Act

            var result = converter.ConvertBack(from, null, collection, null);

            // Assert

            Assert.That(result, Is.EqualTo(to));
        }

        [Test]
        public void ConvertReturnsInputIfInputNotFoundInMappingCollection()
        {
            // Arrange

            const bool input = true;

            var collection = new ConverterMappingCollection();
            var converter = new MappedValuesConverter();

            // Act

            var result = converter.Convert(input, null, collection, null);

            // Assert

            Assert.That(result, Is.EqualTo(input));
        }

        [Test]
        public void ConvertBackReturnsInputIfInputNotFoundInMappingCollection()
        {
            // Arrange

            const bool input = true;

            var collection = new ConverterMappingCollection();
            var converter = new MappedValuesConverter();

            // Act

            var result = converter.ConvertBack(input, null, collection, null);

            // Assert

            Assert.That(result, Is.EqualTo(input));
        }

        [Test]
        public void ConvertReturnsInputIfMappingCollectionIsNotAvailable()
        {
            // Arrange

            const bool input = true;

            var converter = new MappedValuesConverter();

            // Act

            var result = converter.Convert(input, null, null, null);

            // Assert

            Assert.That(result, Is.EqualTo(input));
        }

        [Test]
        public void ConvertBackReturnsInputIfMappingCollectionIsNotAvailable()
        {
            // Arrange

            const bool input = true;

            var converter = new MappedValuesConverter();

            // Act

            var result = converter.ConvertBack(input, null, null, null);

            // Assert

            Assert.That(result, Is.EqualTo(input));
        }
    }
}
