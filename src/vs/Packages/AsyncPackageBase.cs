using System;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

using static Microsoft.VisualStudio.Shell.Interop.__VSPROPID;

namespace Luminous.Code.VisualStudio.Packages
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]

    public class AsyncPackageBase : AsyncPackage
    {
        private DTE2 _dte;

        internal Guid CommandSet { get; }

        protected string PackageTitle { get; }

        protected string PackageDescription { get; }

        protected DTE2 Dte
            => _dte ?? (_dte = GetDte());

        public OleMenuCommandService CommandService { get; private set; }

        public AsyncPackageBase(Guid commandSet, string title, string description)
        {
            CommandSet = commandSet;
            PackageTitle = title;
            PackageDescription = description;
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);

            CommandService = await GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;

            //await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
        }

        public static DTE2 GetDte()
            => GetGlobalService<_DTE, DTE2>();

        public static TTarget GetGlobalService<TSource, TTarget>()
            where TTarget : class
            => (GetGlobalService(typeof(TSource)) as TTarget);

        public T GetService<T>()
            where T : class
            => (GetService(typeof(T)) as T);

        public TTarget GetService<TSource, TTarget>()
            where TSource : class
            where TTarget : class
            => GetService(typeof(TSource)) as TTarget;
    }
}