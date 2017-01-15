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

using EnvDTE80;
using Moq;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Ioc;
using WorkingFilesList.Test.TestingInfrastructure;
using WorkingFilesList.ToolWindow.Interface;
using WorkingFilesList.ToolWindow.Service;

namespace WorkingFilesList.Test.Ioc
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class NinjectContainerTests
    {
        /// <summary>
        /// <see cref="Uri"/> instances in <see cref="DocumentIconService"/>
        /// are in the pack URI format. Creating a <see cref="FrameworkElement"/>
        /// initializes enough of the framework to enable reading pack URIs.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            new FrameworkElement();
        }
        
        [Test]
        public void InterfaceBindingResolution()
        {
            var factory = new NinjectKernelFactory();
            var kernel = factory.CreateKernel(Mock.Of<DTE2>());

            // Requires actual DTE2 implementation: see class-level comment for
            // SettingsStoreServiceStub class
            kernel.Rebind<ISettingsStoreService>().To<SettingsStoreServiceStub>();

            var typesToResolve = new List<Type>
            {
                typeof(ICommand)
            };

            // IoC bindings not defined
            var excludedTypes = new List<Type>
            {
                typeof(IIntValueControl),
                typeof(IUserPreferencesModel)
            };

            var assembly = Assembly.GetAssembly(typeof(IDteEventsSubscriber));
            var assemblyInterfaces = assembly
                .GetTypes()
                .Where(t => t.IsInterface)
                .Except(excludedTypes);

            typesToResolve.AddRange(assemblyInterfaces);

            foreach (var interfaceType in typesToResolve)
            {
                var objects = kernel.GetAll(interfaceType);
                Assert.IsNotEmpty(objects, $"Unable to resolve instance(s) of {interfaceType.Name}");
            }
        }
    }
}
