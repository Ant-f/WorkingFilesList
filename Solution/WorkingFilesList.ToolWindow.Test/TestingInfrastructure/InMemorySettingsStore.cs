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

using Microsoft.VisualStudio.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using WorkingFilesList.ToolWindow.Repository;

namespace WorkingFilesList.ToolWindow.Test.TestingInfrastructure
{
    /// <summary>
    /// <see cref="WritableSettingsStore"/> implementation to be used when
    /// testing functionality of repositories, e.g. <see cref="StoredSettingsRepository"/>
    /// </summary>
    internal class InMemorySettingsStore : WritableSettingsStore
    {
        private readonly Dictionary<string, Dictionary<string, object>> _settingsStore
            = new Dictionary<string, Dictionary<string, object>>();

        private T Get<T>(string collectionPath, string propertyName, T defaultValue)
        {
            try
            {
                var value = (T)_settingsStore[collectionPath][propertyName];
                return value;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public override bool GetBoolean(string collectionPath, string propertyName)
        {
            throw new NotImplementedException();
        }

        public override bool GetBoolean(string collectionPath, string propertyName, bool defaultValue)
        {
            var value = Get(collectionPath, propertyName, defaultValue);
            return value;
        }

        public override int GetInt32(string collectionPath, string propertyName)
        {
            throw new NotImplementedException();
        }

        public override int GetInt32(string collectionPath, string propertyName, int defaultValue)
        {
            var value = Get(collectionPath, propertyName, defaultValue);
            return value;
        }

        public override uint GetUInt32(string collectionPath, string propertyName)
        {
            throw new NotImplementedException();
        }

        public override uint GetUInt32(string collectionPath, string propertyName, uint defaultValue)
        {
            throw new NotImplementedException();
        }

        public override long GetInt64(string collectionPath, string propertyName)
        {
            throw new NotImplementedException();
        }

        public override long GetInt64(string collectionPath, string propertyName, long defaultValue)
        {
            throw new NotImplementedException();
        }

        public override ulong GetUInt64(string collectionPath, string propertyName)
        {
            throw new NotImplementedException();
        }

        public override ulong GetUInt64(string collectionPath, string propertyName, ulong defaultValue)
        {
            throw new NotImplementedException();
        }

        public override string GetString(string collectionPath, string propertyName)
        {
            throw new NotImplementedException();
        }

        public override string GetString(string collectionPath, string propertyName, string defaultValue)
        {
            var value = Get(collectionPath, propertyName, defaultValue);
            return value;
        }

        public override MemoryStream GetMemoryStream(string collectionPath, string propertyName)
        {
            throw new NotImplementedException();
        }

        public override SettingsType GetPropertyType(string collectionPath, string propertyName)
        {
            throw new NotImplementedException();
        }

        public override bool PropertyExists(string collectionPath, string propertyName)
        {
            throw new NotImplementedException();
        }

        public override bool CollectionExists(string collectionPath)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetLastWriteTime(string collectionPath)
        {
            throw new NotImplementedException();
        }

        public override int GetSubCollectionCount(string collectionPath)
        {
            throw new NotImplementedException();
        }

        public override int GetPropertyCount(string collectionPath)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> GetSubCollectionNames(string collectionPath)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> GetPropertyNames(string collectionPath)
        {
            throw new NotImplementedException();
        }

        public override void SetBoolean(string collectionPath, string propertyName, bool value)
        {
            _settingsStore[collectionPath][propertyName] = value;
        }

        public override void SetInt32(string collectionPath, string propertyName, int value)
        {
            _settingsStore[collectionPath][propertyName] = value;
        }

        public override void SetUInt32(string collectionPath, string propertyName, uint value)
        {
            throw new NotImplementedException();
        }

        public override void SetInt64(string collectionPath, string propertyName, long value)
        {
            throw new NotImplementedException();
        }

        public override void SetUInt64(string collectionPath, string propertyName, ulong value)
        {
            throw new NotImplementedException();
        }

        public override void SetString(string collectionPath, string propertyName, string value)
        {
            _settingsStore[collectionPath][propertyName] = value;
        }

        public override void SetMemoryStream(string collectionPath, string propertyName, MemoryStream value)
        {
            throw new NotImplementedException();
        }

        public override void CreateCollection(string collectionPath)
        {
            _settingsStore[collectionPath] = new Dictionary<string, object>();
        }

        public override bool DeleteCollection(string collectionPath)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteProperty(string collectionPath, string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
