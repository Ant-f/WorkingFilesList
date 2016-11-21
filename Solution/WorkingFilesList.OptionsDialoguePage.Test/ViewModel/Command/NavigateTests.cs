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
using System.Diagnostics;
using WorkingFilesList.OptionsDialoguePage.Interface;
using WorkingFilesList.OptionsDialoguePage.ViewModel.Command;

namespace WorkingFilesList.OptionsDialoguePage.Test.ViewModel.Command
{
    public class NavigateTests
    {
        [Test]
        public void CanExecuteReturnsTrue()
        {
            // Arrange

            var command = new Navigate(Mock.Of<IProcessStarter>());

            // Act

            var canExecute = command.CanExecute(null);

            // Assert

            Assert.IsTrue(canExecute);
        }

        [Test]
        public void ExecuteStartsNewProcessWithAbsoluteUri()
        {
            // Arrange

            var uri = new Uri("https://github.com/Ant-f");
            var processStarter = Mock.Of<IProcessStarter>();
            var command = new Navigate(processStarter);

            // Act

            command.Execute(uri);

            // Assert

            Mock.Get(processStarter).Verify(p =>
                p.StartNewProcess(It.Is<ProcessStartInfo>(i =>
                    i.FileName == uri.AbsoluteUri)));
        }

        [Test]
        public void ExecuteDoesNotThrowExceptionWithNullParameter()
        {
            // Arrange

            var processStarter = Mock.Of<IProcessStarter>();
            var command = new Navigate(processStarter);

            // Act

            Assert.DoesNotThrow(() => command.Execute(null));

            // Assert

            Mock.Get(processStarter).Verify(p =>
                p.StartNewProcess(
                    It.IsAny<ProcessStartInfo>()),
                Times.Never);
        }
    }
}
