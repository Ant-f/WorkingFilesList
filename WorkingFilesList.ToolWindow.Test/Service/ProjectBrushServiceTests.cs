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
using System.Windows.Media;
using WorkingFilesList.ToolWindow.Service;

namespace WorkingFilesList.ToolWindow.Test.Service
{
    [TestFixture]
    public class ProjectBrushServiceTests
    {
        private readonly Brush[] _colourBrushes =
        {
            Brushes.AliceBlue,
            Brushes.Brown,
            Brushes.CornflowerBlue
        };

        [Test]
        public void GetBrushReturnsBrush()
        {
            // Arrange

            var service = new ProjectBrushService(_colourBrushes);

            // Act

            var brush = service.GetBrush("Id");

            // Assert

            Assert.That(brush, Is.Not.Null);
        }

        [Test]
        public void SameBrushIsReturnedForSameId()
        {
            // Arrange

            const string id = "Id";

            var service = new ProjectBrushService(_colourBrushes);

            // Act

            var brush1 = service.GetBrush(id);
            var brush2 = service.GetBrush(id);

            // Assert

            Assert.That(brush1, Is.EqualTo(brush2));
        }

        [Test]
        public void DifferentBrushIsReturnedForDifferentIds()
        {
            // Arrange

            var service = new ProjectBrushService(_colourBrushes);

            // Act

            var brush1 = service.GetBrush("Id1");
            var brush2 = service.GetBrush("Id2");

            // Assert

            Assert.That(brush1, Is.Not.EqualTo(brush2));
        }

        [Test]
        public void BrushesAreAssignedInOrderOfServiceConstructorParameter()
        {
            // Arrange

            var service = new ProjectBrushService(_colourBrushes);

            // Act

            var brush1 = service.GetBrush("Id1");
            var brush2 = service.GetBrush("Id2");
            var brush3 = service.GetBrush("Id3");

            // Assert

            Assert.That(brush1, Is.EqualTo(_colourBrushes[0]));
            Assert.That(brush2, Is.EqualTo(_colourBrushes[1]));
            Assert.That(brush3, Is.EqualTo(_colourBrushes[2]));
        }

        [Test]
        public void FirstBrushIsReusedForFourthIdWhenThreeDifferentBrushesAreAvailable()
        {
            // Arrange

            var service = new ProjectBrushService(_colourBrushes);

            // Act

            var brush1 = service.GetBrush("Id1");
            service.GetBrush("Id2");
            service.GetBrush("Id3");
            var brush4 = service.GetBrush("Id4");

            // Assert

            Assert.That(brush4, Is.EqualTo(brush1));
        }
    }
}
