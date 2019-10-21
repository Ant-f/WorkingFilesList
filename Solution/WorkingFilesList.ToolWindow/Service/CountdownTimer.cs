// Working Files List
// Visual Studio extension tool window that shows a selectable list of files
// that are open in the editor
// Copyright © 2019 Anthony Fung

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Windows.Threading;
using WorkingFilesList.ToolWindow.Interface;

namespace WorkingFilesList.ToolWindow.Service
{
    /// <summary>
    /// Wrapper for a <see cref="DispatcherTimer"/>; facilitate testing.
    /// </summary>
    public class CountdownTimer : ICountdownTimer
    {
        private readonly DispatcherTimer _timer;

        public Action Callback { get; set; }
        public TimeSpan Interval { get; set; }

        public CountdownTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += CountdownTimerTick;
        }

        private void CountdownTimerTick(object sender, EventArgs e)
        {
            _timer.Stop();
            Callback();
        }

        public void Restart()
        {
            _timer.Interval = Interval;
            _timer.Start();
        }
    }
}
