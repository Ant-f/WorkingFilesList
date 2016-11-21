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
using System.Diagnostics;
using WorkingFilesList.OptionsDialoguePage.Service;

namespace WorkingFilesList.OptionsDialoguePage.Test.Service
{
    [TestFixture]
    public class ProcessStarterTests
    {
        /// <summary>
        /// This is an inelegant test: the aim is to test that a
        /// <see cref="Process"/> is created and started. As there is no way to
        /// create a mock <see cref="Process"/> that will not actually run, a
        /// default <see cref="ProcessStartInfo"/> instance is created and
        /// assigned; assertions are made against the resulting exception that
        /// is expected to be thrown
        /// </summary>
        [Test]
        public void StartNewProcessCreatesAndStartsNewProcess()
        {
            // Arrange

            var exceptionThrown = false;
            var exceptionMessage = string.Empty;

            var info = new ProcessStartInfo();
            var processStarter = new ProcessStarter();

            // Act

            try
            {
                processStarter.StartNewProcess(info);
            }
            catch (InvalidOperationException e)
            {
                exceptionThrown = true;
                exceptionMessage = e.Message;
            }

            // Assert

            Assert.IsTrue(exceptionThrown);

            Assert.That(exceptionMessage, Is.EqualTo(
                "Cannot start process because a file name has not been provided."));
        }
    }
}
