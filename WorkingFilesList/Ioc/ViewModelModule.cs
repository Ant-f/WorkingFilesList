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

using Ninject.Modules;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model.SortOption;
using WorkingFilesList.ToolWindow.ViewModel;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference;

namespace WorkingFilesList.Ioc
{
    internal class ViewModelModule : NinjectModule
    {
        public override void Load()
        {
            // Bound in order of display
            Kernel.Bind<ISortOption>().To<AlphabeticalSort>().InSingletonScope();
            Kernel.Bind<ISortOption>().To<ReverseAlphabeticalSort>().InSingletonScope();
            Kernel.Bind<ISortOption>().To<ChronologicalSort>().InSingletonScope();

            Kernel.Bind<IDocumentMetadataManager>().To<DocumentMetadataManager>().InSingletonScope();
            Kernel.Bind<IOptionsLists>().To<OptionsLists>().InSingletonScope();
            Kernel.Bind<IUpdateReaction>().To<PathSegmentCountUpdateReaction>().InSingletonScope();
            Kernel.Bind<IUpdateReaction>().To<SelectedSortOptionUpdateReaction>().InSingletonScope();
            Kernel.Bind<IUserPreferences>().To<UserPreferences>().InSingletonScope();
        }
    }
}
