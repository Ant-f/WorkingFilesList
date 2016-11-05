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
using System;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Model;
using WorkingFilesList.ToolWindow.Service;
using WorkingFilesList.ToolWindow.Test.TestingInfrastructure;

namespace WorkingFilesList.ToolWindow.Test.Service
{
    [TestFixture]
    public class NormalizedUsageOrderServiceTests
    {
        private static DocumentMetadata CreateDocumentMetadata(DateTime activatedAt)
        {
            var info = new DocumentMetadataInfo();
            var metadata = new DocumentMetadata(info, string.Empty, null)
            {
                ActivatedAt = activatedAt
            };

            return metadata;
        }

        [Test]
        public void UsageOrderIsCalculatedFromActivatedAtTime()
        {
            // Arrange

            var utcNow = DateTime.UtcNow;

            var first = CreateDocumentMetadata(utcNow);
            var second = CreateDocumentMetadata(utcNow - TimeSpan.FromSeconds(1));
            var third = CreateDocumentMetadata(utcNow - TimeSpan.FromSeconds(2));

            var metadata = new[]
            {
                second,
                first,
                third
            };

            var preferences = Mock.Of<IUserPreferences>(u =>
                u.ShowRecentUsage == true);

            var service = new NormalizedUsageOrderService();

            // Act

            service.SetUsageOrder(metadata, preferences);

            // Assert

            var interval = 1/(double) metadata.Length;

            var expectedFirst = interval*3;
            var expectedSecond = interval*2;
            var expectedThird = interval*1;

            Assert.That(first.UsageOrder, Is.EqualTo(expectedFirst));
            Assert.That(second.UsageOrder, Is.EqualTo(expectedSecond));
            Assert.That(third.UsageOrder, Is.EqualTo(expectedThird));
        }

        [Test]
        public void MaximumUsageOrderValueIs1()
        {
            // Arrange

            var utcNow = DateTime.UtcNow;
            var first = CreateDocumentMetadata(utcNow);
            var metadata = new[] {first};

            var preferences = Mock.Of<IUserPreferences>(u =>
                u.ShowRecentUsage == true);

            var service = new NormalizedUsageOrderService();

            // Act

            service.SetUsageOrder(metadata, preferences);

            // Assert

            Assert.That(first.UsageOrder, Is.Not.GreaterThan(1));
        }

        [Test]
        public void UsageOrderIs1WhenShowRecentUsageIsFalse()
        {
            // Arrange

            var utcNow = DateTime.UtcNow;

            var first = CreateDocumentMetadata(utcNow);
            var second = CreateDocumentMetadata(utcNow - TimeSpan.FromSeconds(1));
            var third = CreateDocumentMetadata(utcNow - TimeSpan.FromSeconds(2));

            var metadata = new[]
            {
                second,
                first,
                third
            };

            var service = new NormalizedUsageOrderService();

            var builder = new UserPreferencesBuilder();
            var preferences = builder.CreateUserPreferences();
            preferences.ShowRecentUsage = false;

            // Act

            service.SetUsageOrder(metadata, preferences);

            // Assert

            const double desiredValue = 1;

            Assert.That(first.UsageOrder, Is.EqualTo(desiredValue));
            Assert.That(second.UsageOrder, Is.EqualTo(desiredValue));
            Assert.That(third.UsageOrder, Is.EqualTo(desiredValue));
        }
    }
}
