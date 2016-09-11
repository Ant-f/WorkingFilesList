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

using Ninject.Modules;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model.SortOption;
using WorkingFilesList.ToolWindow.ViewModel;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference.UpdateReaction;

namespace WorkingFilesList.Ioc
{
    internal class ViewModelModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ISortOption>().To<AlphabeticalSort>().InSingletonScope();
            Kernel.Bind<ISortOption>().To<ChronologicalSort>().InSingletonScope();
            Kernel.Bind<ISortOption>().To<DisableSorting>().InSingletonScope();
            Kernel.Bind<ISortOption>().To<ProjectAlphabeticalSort>().InSingletonScope();
            Kernel.Bind<ISortOption>().To<ProjectReverseAlphabeticalSort>().InSingletonScope();
            Kernel.Bind<ISortOption>().To<ReverseAlphabeticalSort>().InSingletonScope();

            Kernel.Bind<IUpdateReaction>().To<AssignProjectColoursReaction>().InSingletonScope();
            Kernel.Bind<IUpdateReaction>().To<GroupByProjectReaction>().InSingletonScope();
            Kernel.Bind<IUpdateReaction>().To<PathSegmentCountReaction>().InSingletonScope();
            Kernel.Bind<IUpdateReaction>().To<SelectedSortOptionReaction>().InSingletonScope();
            Kernel.Bind<IUpdateReaction>().To<ShowRecentUsageReaction>().InSingletonScope();

            Kernel.Bind<IDisplayOrderContainer>().To<DisplayOrderContainer>().InSingletonScope();
            Kernel.Bind<IDocumentMetadataManager>().To<DocumentMetadataManager>().InSingletonScope();
            Kernel.Bind<IOptionsLists>().To<OptionsLists>().InSingletonScope();
            Kernel.Bind<IProjectBrushes>().To<ProjectBrushes>().InSingletonScope();
            Kernel.Bind<IUpdateReactionManager>().To<UpdateReactionManager>().InSingletonScope();
            Kernel.Bind<IUpdateReactionMapping>().To<UpdateReactionMapping>().InSingletonScope();
            Kernel.Bind<IUserPreferences>().To<UserPreferences>().InSingletonScope();
        }
    }
}
