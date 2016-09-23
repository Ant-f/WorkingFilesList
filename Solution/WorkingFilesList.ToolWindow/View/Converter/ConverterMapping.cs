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

using System.Windows;

namespace WorkingFilesList.ToolWindow.View.Converter
{
    /// <summary>
    /// To be used within a <see cref="ConverterMappingCollection"/> in
    /// conjunction with a <see cref="MappedValuesConverter"/>. Defines the
    /// converter output value for a given input value.
    /// </summary>
    public class ConverterMapping : DependencyObject
    {
        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register(
                "From",
                typeof(object),
                typeof(ConverterMapping));

        public object From
        {
            get { return GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register(
                "To",
                typeof(object),
                typeof(ConverterMapping));

        public object To
        {
            get { return GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }
    }
}
