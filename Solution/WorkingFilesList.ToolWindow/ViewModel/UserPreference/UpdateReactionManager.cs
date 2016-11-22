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

using System.ComponentModel;
using System.Linq;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.ViewModel.UserPreference
{
    /// <summary>
    /// This class monitors for property value changes within an
    /// <see cref="IUserPreferences"/>, and triggers relevant updates to a
    /// collection view so the preferences are reflected
    /// </summary>
    public class UpdateReactionManager : IUpdateReactionManager
    {
        private readonly IUpdateReactionMapping _updateReactionMapping;
        private readonly IUserPreferences _userPreferences;

        private ICollectionView _collectionView;

        public UpdateReactionManager(
            IUpdateReactionMapping updateReactionMapping,
            IUserPreferences userPreferences)
        {
            _updateReactionMapping = updateReactionMapping;
            _userPreferences = userPreferences;

            _userPreferences.PropertyChanged += UserPreferencesPropertyChanged;
        }

        /// <summary>
        /// Sets a view of the collection that should be updated when values
        /// of certain <see cref="IUserPreferences"/> properties are updated,
        /// and runs all <see cref="IUpdateReaction"/> instances specified by
        /// the <see cref="IUpdateReactionMapping"/> constructor parameter
        /// </summary>
        /// <param name="view">A view of the collection to update</param>
        public void Initialize(ICollectionView view)
        {
            _collectionView = view;

            var allUpdateReactions = _updateReactionMapping
                .MappingTable
                .SelectMany(kvp => kvp.Value);

            foreach (var updateReaction in allUpdateReactions)
            {
                updateReaction.UpdateCollection(
                    _collectionView,
                    _userPreferences);
            }
        }

        private void UserPreferencesPropertyChanged(
            object sender,
            PropertyChangedEventArgs e)
        {
            if (_collectionView != null &&
                _updateReactionMapping.MappingTable.ContainsKey(e.PropertyName))
            {
                foreach (var updateReaction in
                    _updateReactionMapping.MappingTable[e.PropertyName])
                {
                    updateReaction.UpdateCollection(
                        _collectionView,
                        _userPreferences);
                }
            }
        }
    }
}
