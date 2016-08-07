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

using NUnit.Framework;
using System;
using WorkingFilesList.ToolWindow.Model;
using WorkingFilesList.ToolWindow.Service;

namespace WorkingFilesList.ToolWindow.Test.Service
{
    [TestFixture]
    public class NormalizedUseOrderServiceTests
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
        public void UseOrderIsCalculatedFromActivatedAtTime()
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

            var service = new NormalizedUseOrderService();

            // Act

            service.SetUseOrder(metadata);

            // Assert

            var interval = 1/(double) metadata.Length;

            var expectedFirst = interval*3;
            var expectedSecond = interval*2;
            var expectedThird = interval*1;

            Assert.That(first.UseOrder, Is.EqualTo(expectedFirst));
            Assert.That(second.UseOrder, Is.EqualTo(expectedSecond));
            Assert.That(third.UseOrder, Is.EqualTo(expectedThird));
        }

        [Test]
        public void MaximumUseOrderValueIs1()
        {
            // Arrange

            var utcNow = DateTime.UtcNow;
            var first = CreateDocumentMetadata(utcNow);
            var metadata = new[] {first};
            var service = new NormalizedUseOrderService();

            // Act

            service.SetUseOrder(metadata);

            // Assert

            Assert.That(first.UseOrder, Is.Not.GreaterThan(1));
        }
    }
}
