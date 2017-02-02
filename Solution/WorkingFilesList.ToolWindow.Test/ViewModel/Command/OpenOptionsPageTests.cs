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

using NUnit.Framework;
using System;
using WorkingFilesList.ToolWindow.ViewModel.Command;

namespace WorkingFilesList.ToolWindow.Test.ViewModel.Command
{
    public class OpenOptionsPageTests
    {
        [Test]
        public void CanExecuteReturnsTrue()
        {
            // Arrange

            var command = new OpenOptionsPage();

            // Act

            var canExecute = command.CanExecute(null);

            // Assert

            Assert.IsTrue(canExecute);
        }

        [Test]
        public void ExecuteDoesNotThrowExceptionWhenInvokedWithNoEventSubscribers()
        {
            // Arrange

            var command = new OpenOptionsPage();

            // Act, Assert

            Assert.DoesNotThrow(() =>
                command.Execute(null));
        }

        [Test]
        public void ExecuteRaisesOptionsPageRequested()
        {
            // Arrange

            var eventRaised = false;

            var handler = new EventHandler((s, e) =>
            {
                eventRaised = true;
            });

            var command = new OpenOptionsPage();
            command.OptionsPageRequested += handler;

            // Act

            command.Execute(null);
            command.OptionsPageRequested -= handler;

            // Assert

            Assert.IsTrue(eventRaised);
        }
    }
}
