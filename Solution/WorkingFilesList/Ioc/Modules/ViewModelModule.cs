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
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model.SortOption;
using WorkingFilesList.ToolWindow.ViewModel;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference.UpdateReaction;

namespace WorkingFilesList.Ioc.Modules
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
