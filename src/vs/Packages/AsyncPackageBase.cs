using System;
using System.ComponentModel.Design;
using System.Threading;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Tasks = System.Threading.Tasks;

using static EnvDTE.Constants;

namespace Luminous.Code.VisualStudio.Packages
{
    using Exceptions.ExceptionExtensions;
    using Commands;

    using static Commands.CommandKeys;
    using System.IO;

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

        public static AsyncPackageBase Instance { get; set; }


        public OleMenuCommandService CommandService { get; private set; }

        public AsyncPackageBase(Guid commandSet, string title, string description)
        {
            CommandSet = commandSet;
            PackageTitle = title;
            PackageDescription = description;
            Instance = this;
        }

        protected override async Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
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

        public static T GetDialogPage<T>()
            where T : DialogPage
            => (T)Instance.GetDialogPage(typeof(T));

        public async Tasks.Task<CommandResult> ExecuteCommandAsync(string command, string args = "", string success = null, string problem = null)
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                Dte?.ExecuteCommand(command, args);

                return new SuccessResult(message: success);
            }
            catch (Exception ex)
            {
                return new ProblemResult(message: problem ?? ex.ExtendedMessage());
            }
        }

        public async Tasks.Task<CommandResult> OpenFileAsync(string name, string viewKind = vsViewKindAny, string problem = null)
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                if (!File.Exists(name))
                    return new ProblemResult($"Unable to open '{name}'");

                Dte?.ItemOperations.OpenFile(name, viewKind);

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ProblemResult(message: problem ?? ex.ExtendedMessage());
            }
        }

        public async Tasks.Task<CommandResult> OpenExtensionsAndUpdatesAsync(string problem = null)
        {
            try
            {
                return await ExecuteCommandAsync(ExtensionsAndUpdates);
            }
            catch (Exception ex)
            {
                return new ProblemResult(message: problem ?? ex.ExtendedMessage());
            }
        }

        public async Tasks.Task<CommandResult> OpenManageExtensionsAsync(string problem = null)
        {
            try
            {
                return await ExecuteCommandAsync(ManageExtensions);
            }
            catch (Exception ex)
            {
                return new ProblemResult(message: problem ?? ex.ExtendedMessage());
            }
        }
    }
}