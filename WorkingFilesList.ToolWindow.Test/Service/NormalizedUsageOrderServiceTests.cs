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

using Moq;
using NUnit.Framework;
using System;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Model;
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
            var metadata = new DocumentMetadata(info, string.Empty)
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
        public void UsageOrderIs0WhenShowRecentUsageIsFalse()
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

            Assert.That(first.UsageOrder, Is.EqualTo(0));
            Assert.That(second.UsageOrder, Is.EqualTo(0));
            Assert.That(third.UsageOrder, Is.EqualTo(0));
        }
    }
}
