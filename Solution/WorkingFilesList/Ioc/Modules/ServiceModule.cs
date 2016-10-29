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
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Service;
using WorkingFilesList.ToolWindow.Service.EventRelay;

namespace WorkingFilesList.Ioc.Modules
{
    internal class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IAboutPanelService>().To<AboutPanelService>().InSingletonScope();
            Kernel.Bind<ICollectionViewGenerator>().To<CollectionViewGenerator>().InSingletonScope();
            Kernel.Bind<IDisplayNameHighlightEvaluator>().To<DisplayNameHighlightEvaluator>().InSingletonScope();
            Kernel.Bind<IDocumentIconService>().To<DocumentIconService>().InSingletonScope();
            Kernel.Bind<IDocumentMetadataEqualityService>().To<DocumentMetadataEqualityService>().InSingletonScope();
            Kernel.Bind<IDteEventsSubscriber>().To<DteEventsSubscriber>().InSingletonScope();
            Kernel.Bind<IFilePathService>().To<FilePathService>().InSingletonScope();
            Kernel.Bind<INormalizedUsageOrderService>().To<NormalizedUsageOrderService>().InSingletonScope();
            Kernel.Bind<IPathCasingRestorer>().To<PathCasingRestorer>().InSingletonScope();
            Kernel.Bind<IProjectBrushService>().To<ProjectBrushService>().InSingletonScope();
            Kernel.Bind<IProjectItemsEventsService>().To<ProjectItemsEventsService>().InSingletonScope();
            Kernel.Bind<ISolutionEventsService>().To<SolutionEventsService>().InSingletonScope();
            Kernel.Bind<ISortOptionsService>().To<SortOptionsService>().InSingletonScope();
            Kernel.Bind<ITestFileNameEvaluator>().To<TestFileNameEvaluator>().InSingletonScope();
            Kernel.Bind<ITimeProvider>().To<TimeProvider>().InSingletonScope();
            Kernel.Bind<IWindowEventsService>().To<WindowEventsService>().InSingletonScope();
        }
    }
}
