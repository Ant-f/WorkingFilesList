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
using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Data;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.ViewModel.UserPreference.UpdateReaction;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.UserPreference.UpdateReaction
{
    [TestFixture]
    public class ShowRecentUsageReactionTests
    {
        [Test]
        public void UpdateCollectionUsesNormalizedUsageOrderService()
        {
            // Arrange

            var serviceMock = new Mock<INormalizedUsageOrderService>();

            var updateReaction = (IUpdateReaction)new ShowRecentUsageReaction(
                serviceMock.Object);

            var preferences = Mock.Of<IUserPreferences>();

            var collection = new List<DocumentMetadata>();
            var view = new ListCollectionView(collection);

            // Act

            updateReaction.UpdateCollection(view, preferences);

            // Assert

            serviceMock.Verify(s => s.SetUsageOrder(collection, preferences));
        }
    }
}
