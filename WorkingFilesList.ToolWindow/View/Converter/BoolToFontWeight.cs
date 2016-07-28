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

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WorkingFilesList.ToolWindow.Model;

namespace WorkingFilesList.ToolWindow.View.Converter
{
    /// <summary>
    /// Converts true to <see cref="FontWeights.Bold"/> and false to
    /// <see cref="FontWeights.Normal"/>. Used in the UI to highlight the
    /// active document in bindings to <see cref="DocumentMetadata.IsActive"/>
    /// </summary>
    public class BoolToFontWeight : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = (bool) value;
            var fontWeight = boolValue ? FontWeights.Bold : FontWeights.Normal;
            return fontWeight;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fontWeight = value as FontWeight?;
            var isBold = fontWeight == FontWeights.Bold;
            return isBold;
        }
    }
}
