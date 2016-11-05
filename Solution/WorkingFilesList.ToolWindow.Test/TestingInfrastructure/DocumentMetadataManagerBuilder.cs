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

using Moq;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.ToolWindow.Service;
using WorkingFilesList.ToolWindow.ViewModel;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference.UpdateReaction;

namespace WorkingFilesList.ToolWindow.Test.TestingInfrastructure
{
    internal class DocumentMetadataManagerBuilder
    {
        /// <summary>
        /// Generator to pass as constructor parameter when creating new
        /// <see cref="DocumentMetadataManager"/> instances. A
        /// <see cref="ToolWindow.Service.CollectionViewGenerator"/>
        /// instance will be created if this property is left null.
        /// </summary>
        public ICollectionViewGenerator CollectionViewGenerator { get; set; }

        /// <summary>
        /// Service to test whether metadata structures refer to the same
        /// document, to pass as constructor parameter when creating new
        /// <see cref="DocumentMetadataManager"/> instances. A
        /// <see cref="ToolWindow.Service.DocumentMetadataEqualityService"/>
        /// instance will be created if this property is left null.
        /// </summary>
        public IDocumentMetadataEqualityService DocumentMetadataEqualityService { get; set; }

        /// <summary>
        /// Factory to pass as constructor parameter when creating new
        /// <see cref="DocumentMetadataManager"/> instances. A mock factory that
        /// outputs its input parameter is created if this property is left null.
        /// </summary>
        public IDocumentMetadataFactory DocumentMetadataFactory { get; set; }

        public Mock<INormalizedUsageOrderService> NormalizedUsageOrderServiceMock { get; }
            = new Mock<INormalizedUsageOrderService>();

        public Mock<ITimeProvider> TimeProviderMock { get; }
            = new Mock<ITimeProvider>();

        public ISortOptionsService SortOptionsService { get; set; }
            = new SortOptionsService();

        public IUpdateReactionManager UpdateReactionManager { get; set; }

        public IUpdateReactionMapping UpdateReactionMapping { get; set; }

        public IUserPreferences UserPreferences { get; set; }

        /// <summary>
        /// Creates a new <see cref="ToolWindow.ViewModel.UserPreferences"/>
        /// instance when <see cref="UserPreferences"/> is null.
        /// </summary>
        public UserPreferencesBuilder UserPreferencesBuilder { get; }
            = new UserPreferencesBuilder();

        /// <summary>
        /// Create and return a new <see cref="DocumentMetadataManager"/>,
        /// configured with the properties available in this builder instance
        /// </summary>
        /// <returns>
        /// A new <see cref="DocumentMetadataManager"/> for use in unit tests
        /// </returns>
        public DocumentMetadataManager CreateDocumentMetadataManager()
        {
            if (DocumentMetadataFactory == null)
            {
                var builder = new DocumentMetadataFactoryBuilder(TimeProviderMock);
                DocumentMetadataFactory = builder.CreateDocumentMetadataFactory(true);
            }

            if (UserPreferences == null)
            {
                UserPreferences = UserPreferencesBuilder.CreateUserPreferences();
            }

            if (UpdateReactionMapping == null)
            {
                var displayNameHighlightEvaluator = new DisplayNameHighlightEvaluator();
                var filePathService = new FilePathService();

                var updateReactions = new IUpdateReaction[]
                {
                    new AssignProjectColoursReaction(Mock.Of<IProjectBrushService>()),
                    new GroupByProjectReaction(),
                    new PathSegmentCountReaction(displayNameHighlightEvaluator, filePathService),
                    new SelectedSortOptionReaction(SortOptionsService),
                    new ShowRecentUsageReaction(NormalizedUsageOrderServiceMock.Object)
                };

                UpdateReactionMapping = new UpdateReactionMapping(updateReactions);
            }

            if (UpdateReactionManager == null)
            {
                UpdateReactionManager = new UpdateReactionManager(
                    UpdateReactionMapping,
                    UserPreferences);
            }

            var manager = new DocumentMetadataManager(
                CollectionViewGenerator ?? new CollectionViewGenerator(),
                DocumentMetadataEqualityService ?? new DocumentMetadataEqualityService(),
                DocumentMetadataFactory,
                NormalizedUsageOrderServiceMock.Object,
                TimeProviderMock.Object,
                UpdateReactionManager,
                UserPreferences);

            // Initialization logic in the constructor of
            // DocumentMetadataManager will make calls on the mock
            // NormalizedUsageOrderService. Reset calls so that these are not
            // counted in the tests that the created DocumentMetadataManager
            // will be used in.

            NormalizedUsageOrderServiceMock.ResetCalls();

            return manager;
        }
    }
}
