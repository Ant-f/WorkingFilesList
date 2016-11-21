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

using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.OptionsDialoguePage.ViewModel.Command;

namespace WorkingFilesList.OptionsDialoguePage.ViewModel
{
    public class DialoguePageCommands : IDialoguePageCommands
    {
        public ICommand Navigate { get; }

        public DialoguePageCommands(IList<ICommand> commandCollection)
        {
            Navigate = commandCollection.OfType<Navigate>().Single();
        }
    }
}
