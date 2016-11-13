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

using Microsoft.VisualStudio.Shell;
using System.Windows;
using WorkingFilesList.Core.Model;
using WorkingFilesList.Core.Service.Locator;

namespace WorkingFilesList.OptionsDialoguePage
{
    public class OptionsPage : UIElementDialogPage
    {
        private readonly UserPreferencesModel _preferencesModel;

        protected override UIElement Child { get; }

        public OptionsPage()
        {
            _preferencesModel = ViewModelService.UserPreferencesModelFactory
                .CreateModel();

            var optionsPageControl = ViewModelService.OptionsPageControlFactory
                .CreateControl();

            optionsPageControl.DataContext = _preferencesModel;

            Child = optionsPageControl;
        }

        public override void LoadSettingsFromStorage()
        {
            ViewModelService.UserPreferencesModelRepository
                .LoadInto(_preferencesModel);
        }

        public override void SaveSettingsToStorage()
        {
            ViewModelService.UserPreferencesModelRepository
                .SaveModel(_preferencesModel);
        }
    }
}
