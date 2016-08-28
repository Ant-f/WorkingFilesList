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
    public class ConverterMappingCollectionTests
    {
        [Test]
        public void AddedMappingAppearsInDictionary()
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

            // Act

            collection.MappedValues.Add(mapping);

            // Assert

            Assert.That(collection.MappedValuesDictionary.Count, Is.EqualTo(1));
            Assert.Contains(from, collection.MappedValuesDictionary.Keys);
            Assert.That(collection.MappedValuesDictionary[from], Is.EqualTo(to));
        }

        [Test]
        public void NullKeyMappingIsNotAddedToDictionary()
        {
            // Arrange

            var mapping = new ConverterMapping
            {
                From = null,
                To = new object()
            };

            var collection = new ConverterMappingCollection();

            // Act

            collection.MappedValues.Add(mapping);

            // Assert

            Assert.IsEmpty(collection.MappedValuesDictionary);
        }

        [Test]
        public void DuplicateKeysOverwritePreviousValue()
        {
            // Arrange

            const string key = "Key";
            var value1 = new object();
            var value2 = new object();

            var mapping1 = new ConverterMapping
            {
                From = key,
                To = value1
            };

            var mapping2 = new ConverterMapping
            {
                From = key,
                To = value2
            };

            var collection = new ConverterMappingCollection();
            collection.MappedValues.Add(mapping1);

            // Act

            collection.MappedValues.Add(mapping2);

            // Assert

            Assert.That(value1, Is.Not.EqualTo(value2));

            Assert.That(collection.MappedValuesDictionary.Count, Is.EqualTo(1));
            Assert.Contains(key, collection.MappedValuesDictionary.Keys);
            Assert.That(collection.MappedValuesDictionary[key], Is.EqualTo(value2));
        }
    }
}
