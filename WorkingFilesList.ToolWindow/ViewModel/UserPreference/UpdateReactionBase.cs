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
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.ViewModel.UserPreference
{
    /// <summary>
    /// Base for classes that should update an <see cref="ICollectionView"/>
    /// when a specific property within an <see cref="IUserPreferences"/> has
    /// been updated
    /// </summary>
    public abstract class UpdateReactionBase : IUpdateReaction
    {
        protected ICollectionView CollectionView;
        protected IUserPreferences UserPreferences;

        /// <summary>
        /// When a <see cref="IUserPreferences"/> instance raises
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/>,
        /// <see cref="UpdateCollection"/> is invoked if the property names match
        /// </summary>
        protected abstract string PropertyName { get; }

        protected UpdateReactionBase(IUserPreferences userPreferences)
        {
            UserPreferences = userPreferences;
            userPreferences.PropertyChanged += UserPreferencesPropertyChanged;
        }

        public void Initialize(ICollectionView collection)
        {
            CollectionView = collection;
        }

        private void UserPreferencesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (CollectionView != null &&
                e.PropertyName == PropertyName)
            {
                UpdateCollection();
            }
        }

        /// <summary>
        /// Logic to update <see cref="CollectionView"/> should be placed here
        /// in a derived class
        /// </summary>
        public abstract void UpdateCollection();
    }
}
