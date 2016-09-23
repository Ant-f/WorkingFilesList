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

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WorkingFilesList.ToolWindow.View.Converter
{
    /// <summary>
    /// Instances of this can be passed as a parameter when using a
    /// <see cref="MappedValuesConverter"/>. The mappings, which can be declared
    /// using XAML, will act as a lookup table when converting values.
    /// </summary>
    public class ConverterMappingCollection : DependencyObject
    {
        /// <summary>
        /// Holds the contents of <see cref="MappedValuesProperty"/> as a
        /// dictionary
        /// </summary>
        private readonly Dictionary<object, object> _mappedValuesDictionary
            = new Dictionary<object, object>();

        public static readonly DependencyPropertyKey MappedValuesPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "MappedValues",
                typeof(IList),
                typeof(ConverterMappingCollection),
                new FrameworkPropertyMetadata());

        public static readonly DependencyProperty MappedValuesProperty =
            MappedValuesPropertyKey.DependencyProperty;

        public IList MappedValues => (IList) GetValue(MappedValuesProperty);

        /// <summary>
        /// A read only view of the contents of <see cref="MappedValuesProperty"/>
        /// as a dictionary
        /// </summary>
        public readonly ReadOnlyDictionary<object, object> MappedValuesDictionary;

        public ConverterMappingCollection()
        {
            MappedValuesDictionary = new ReadOnlyDictionary<object, object>(
                _mappedValuesDictionary);

            var collection = new ObservableCollection<ConverterMapping>();
            collection.CollectionChanged += (s, e) =>
            {
                _mappedValuesDictionary.Clear();

                foreach (var mapping in collection)
                {
                    if (mapping.From != null)
                    {
                        if (_mappedValuesDictionary.ContainsKey(mapping.From))
                        {
                            _mappedValuesDictionary[mapping.From] = mapping.To;
                        }
                        else
                        {
                            _mappedValuesDictionary.Add(
                                mapping.From,
                                mapping.To);
                        }
                    }
                }
            };

            SetValue(MappedValuesPropertyKey, collection);
        }
    }
}
