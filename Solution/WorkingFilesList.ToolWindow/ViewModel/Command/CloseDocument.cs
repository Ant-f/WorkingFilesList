// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2016 Anthony Fung

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using EnvDTE;
using EnvDTE80;
using System;
using System.Windows.Input;
using WorkingFilesList.ToolWindow.Model;

namespace WorkingFilesList.ToolWindow.ViewModel.Command
{
    public class CloseDocument : ICommand
    {
        private readonly DTE2 _dte2;

        #pragma warning disable 67
        // ICommand interface member is only used by XAML
        public event EventHandler CanExecuteChanged;
        #pragma warning restore 67

        public CloseDocument(DTE2 dte2)
        {
            _dte2 = dte2;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var metadata =  parameter as DocumentMetadata;

            if (metadata == null)
            {
                return;
            }

            foreach (var itm in _dte2.Documents)
            {
                var document = (Document) itm;

                var match = string.CompareOrdinal(
                    document.FullName,
                    metadata.FullName) == 0;

                if (match)
                {
                    document.Close();
                    break;
                }
            }
        }
    }
}
