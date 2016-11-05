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

using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Ninject;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using WorkingFilesList.Ioc;
using WorkingFilesList.OptionsDialoguePage;
using WorkingFilesList.Core.Interface;
using WorkingFilesList.Core.Service.Locator;

namespace WorkingFilesList
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(OptionsPage), "Working Files List", "General", 1000, 1001, true)]
    [ProvideToolWindow(typeof(WorkingFilesWindow))]
    [Guid(WorkingFilesWindowPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class WorkingFilesWindowPackage : Package
    {
        private IKernel _kernel;

        /// <summary>
        /// WorkingFilesWindowPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "26bf5782-13d2-4168-b979-bfcde9bd63dd";

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkingFilesWindow"/>
        /// class.
        /// </summary>
        public WorkingFilesWindowPackage()
        {
            // Inside this method you can place any initialization code that
            // does not require any Visual Studio service because at this point
            // the package object is created but not sited yet inside Visual
            // Studio environment. The place to do all the other initialization
            // is the Initialize method.
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the
        /// package is sited, so this is the place where you can put all the
        /// initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            WorkingFilesWindowCommand.Initialize(this);
            base.Initialize();

            var dte2 = (DTE2) GetService(typeof(DTE));
            var kernelFactory = new NinjectKernelFactory();
            _kernel = kernelFactory.CreateKernel(dte2);

            InitializeServices(_kernel);
        }

        #endregion

        /// <summary>
        /// Initialize <see cref="WorkingFilesList.ToolWindow"/> services and
        /// view model objects
        /// </summary>
        public void InitializeServices(IKernel kernel)
        {
            var dte2 = kernel.Get<DTE2>();

            var events2 = (Events2)dte2.Events;
            var subscriber = kernel.Get<IDteEventsSubscriber>();
            subscriber.SubscribeTo(events2);

            // Inject properties by resolving a ViewModelService instance

            kernel.Get<ViewModelService>();

            // Synchronize to make sure already open documents are listed in
            // the tool window

            var metadataManager = kernel.Get<IDocumentMetadataManager>();
            metadataManager.Synchronize(dte2.Documents, true);
        }
    }
}
