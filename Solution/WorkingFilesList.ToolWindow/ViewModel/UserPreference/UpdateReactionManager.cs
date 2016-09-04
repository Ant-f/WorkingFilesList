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

using System.ComponentModel;
using System.Linq;
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
