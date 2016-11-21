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

using Ninject.Modules;
using System.Windows.Input;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.OptionsDialoguePage.ViewModel;
using WorkingFilesList.OptionsDialoguePage.ViewModel.Command;
using WorkingFilesList.ToolWindow.ViewModel;
using WorkingFilesList.ToolWindow.ViewModel.Command;

namespace WorkingFilesList.Ioc.Modules
{
    /// <summary>
    /// Ninject module that creates bindings for the commands used within the
    /// extension. Buttons within the application UI are bound to these commands.
    /// </summary>
    internal class CommandModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ICommand>().To<ActivateWindow>().InSingletonScope();
            Kernel.Bind<ICommand>().To<CloseDocument>().InSingletonScope();
            Kernel.Bind<ICommand>().To<OpenTestFile>().InSingletonScope();
            Kernel.Bind<IToolWindowCommands>().To<ToolWindowCommands>().InSingletonScope();

            Kernel.Bind<ICommand>().To<Navigate>().InSingletonScope();
            Kernel.Bind<IDialoguePageCommands>().To<DialoguePageCommands>().InSingletonScope();
        }
    }
}
