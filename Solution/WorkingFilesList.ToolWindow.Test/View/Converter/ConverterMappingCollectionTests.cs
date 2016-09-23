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
