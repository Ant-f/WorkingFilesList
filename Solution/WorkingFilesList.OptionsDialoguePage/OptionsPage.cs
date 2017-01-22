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
using System;
using System.ComponentModel;
using System.Windows;
using WorkingFilesList.Core.Model;
using WorkingFilesList.Core.Service.Locator;

namespace WorkingFilesList.OptionsDialoguePage
{
    public class OptionsPage : UIElementDialogPage
    {
        private readonly UserPreferencesModel _preferencesModel;

        /// <summary>
        /// The user preferences shown by this dialogue page are shared and can
        /// be modified by other parts of the extension. In order to show the
        /// most up to date settings, they need to be loaded when this page
        /// is displayed, i.e. activated. If this is done on every activation,
        /// it is possible for unconfirmed changes to be lost when reactivating
        /// the page without confirming/dismissing the dialogue; this can happen
        /// when, e.g. switching between different pages in the Options dialogue.
        /// This flag is set to 'true' in <see cref="OnClosed"/> to indicate
        /// that user preferences should be reloaded when next activating the page.
        /// On the very first activation of an instance's life cycle, preferences
        /// are read from storage, so it is not necessary for <see cref="_isClosed"/>
        /// to have a default value of 'true'
        /// </summary>
        private bool _isClosed = false;

        protected override UIElement Child { get; }

        public override object AutomationObject => _preferencesModel;

        public OptionsPage()
        {
            _preferencesModel = ViewModelService.UserPreferencesModelFactory
                .CreateModel();

            var optionsPageControl = ViewModelService.OptionsPageControlFactory
                .CreateControl();

            optionsPageControl.DataContext = _preferencesModel;

            Child = optionsPageControl;
        }

        protected override void OnActivate(CancelEventArgs e)
        {
            if (_isClosed)
            {
                ViewModelService.UserPreferencesModelRepository
                    .LoadInto(_preferencesModel);

                _isClosed = false;
            }
            
            base.OnActivate(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            _isClosed = true;
            base.OnClosed(e);

            ViewModelService.UserPreferencesModelRepository.LoadInto(
                ViewModelService.UserPreferences);
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
