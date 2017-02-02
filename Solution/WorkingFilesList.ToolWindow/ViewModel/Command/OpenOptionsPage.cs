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

using System;
using System.Windows.Input;

namespace WorkingFilesList.ToolWindow.ViewModel.Command
{
    public class OpenOptionsPage : ICommand
    {
        #pragma warning disable 67
        // ICommand interface member is only used by XAML
        public event EventHandler CanExecuteChanged;
        #pragma warning restore 67

        public event EventHandler OptionsPageRequested;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            OptionsPageRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
