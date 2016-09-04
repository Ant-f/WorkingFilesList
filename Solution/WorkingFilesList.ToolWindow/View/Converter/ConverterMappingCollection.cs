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
