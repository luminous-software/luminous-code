using System;
using System.ComponentModel.Design;
using System.IO;
using System.Threading;
using System.Windows;
using EnvDTE;
using EnvDTE80;
using Luminous.Code.Extensions.StringExtensions;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using static System.Environment;
using static EnvDTE.Constants;
using static Microsoft.VisualStudio.Editor.DefGuidList;
using Button = System.Windows.MessageBoxButton;
using Icon = System.Windows.MessageBoxImage;
using Result = System.Windows.MessageBoxResult;
using Tasks = System.Threading.Tasks;

namespace Luminous.Code.VisualStudio.Packages
{
    using Code.Extensions.ExceptionExtensions;
    using Commands;
    using Extensions.IntegerExtensions;
    using Extensions.IWpfTextViewHostExtensions;
    using static Commands.CommandKeys;

    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]

    public class AsyncPackageBase : AsyncPackage
    {
        internal Guid CommandSet { get; }

        protected string PackageTitle { get; }

        protected string PackageDescription { get; }

        public static AsyncPackageBase Instance { get; private set; }

        public static DTE2 Dte { get; private set; }

        public string VisualStudioVersion { get; private set; }

        public OleMenuCommandService CommandService { get; private set; }

        public IVsMonitorSelection SelectionService { get; private set; }

        protected IVsShell4 VsShell { get; private set; }

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

            Dte = await GetServiceAsync<_DTE, DTE2>();
            CommandService = await GetServiceAsync<IMenuCommandService, OleMenuCommandService>();
            SelectionService = await GetServiceAsync<SVsShellMonitorSelection, IVsMonitorSelection>();
            VsShell = await GetServiceAsync<SVsShell, IVsShell4>();
            VisualStudioVersion = Dte.Version;
        }

        public static TTarget GetGlobalService<TSource, TTarget>()
            where TTarget : class
            => (GetGlobalService(typeof(TSource)) as TTarget);

        public async Tasks.Task<T> GetServiceAsync<T>()
            where T : class
            => (await GetServiceAsync(typeof(T)) as T);

        public T GetService<T>()
            where T : class
            => (GetService(typeof(T)) as T);

        public async Tasks.Task<TTarget> GetServiceAsync<TSource, TTarget>()
            where TSource : class
            where TTarget : class
            => await GetServiceAsync(typeof(TSource)) as TTarget;

        public TTarget GetService<TSource, TTarget>()
            where TSource : class
            where TTarget : class
            => GetService(typeof(TSource)) as TTarget;

        public CommandResult ExecuteCommand(string command, string args = "", string success = null, string problem = null)
        {
            try
            {
                Dte?.ExecuteCommand(command, args);

                return new SuccessResult(message: success);
            }
            catch (Exception ex)
            {
                return new ProblemResult(message: problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult OpenFile(string name, string viewKind = vsViewKindAny, string problem = null)
        {
            try
            {
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

        public CommandResult OpenFileInBrowser(string name, string problem = null)
        {
            try
            {
                return File.Exists(name)
                    ? ExecuteCommand(ViewWebBrowser, name, problem: problem)
                    : new ProblemResult($"Unable to open '{name}'");
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult ReplaceSelectedText(Func<string> newText, string prefix = "", string suffix = "", string success = null, string problem = null)
        {
            var viewHost = GetCurrentViewHost();
            if (viewHost == null)
                return new ProblemResult(problem ?? "Unable to get current view host");

            var selection = viewHost.GetSelection();
            if (selection == null)
                return new ProblemResult(problem ?? "Unable to get current selection");

            var textView = selection.TextView;

            using (var edit = textView?.TextBuffer?.CreateEdit())
            {
                // note, selection can contain multiple spans when using "box selection"
                foreach (var span in selection.SelectedSpans)
                {
                    edit.Replace(span, $"{prefix}{newText()}{suffix}");
                }
                edit.Apply();
            }
            return new SuccessResult(message: success);
        }

        public CommandResult BuildProject(bool rebuild = true, string problem = null)
        {
            try
            {
                var action = rebuild
                    ? CommandKeys.RebuildProject
                    : CommandKeys.BuildProject;
                var verb = rebuild
                    ? "rebuild"
                    : "build";

                return ExecuteCommand(action, problem: problem ?? $"Unable to {verb} the project");
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult BuildSolution(bool rebuild = true, string problem = null)
        {
            try
            {
                var action = rebuild
                    ? CommandKeys.RebuildSolution
                    : CommandKeys.BuildSolution;
                var verb = rebuild
                    ? "rebuild"
                    : "build";

                return ExecuteCommand(action, problem: problem ?? $"Unable to {verb} the solution");
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult CancelBuild(string problem = null)
            => ExecuteCommand(CommandKeys.CancelBuild, problem: problem ?? "Unable to cancel the current build");

        public CommandResult OpenCodeFile(string name, string problem = null)
            => OpenFile(name, vsViewKindCode, problem);

        public CommandResult OpenTextFile(string name, string problem = null)
            => OpenFile(name, vsViewKindTextView, problem);

        public CommandResult OpenExtensionsAndUpdates(string problem = null)
           => ExecuteCommand(ExtensionsAndUpdates, problem: problem);

        public CommandResult OpenManageExtensions(string problem = null)
            => ExecuteCommand(ManageExtensions);

        public OutputWindowPane PackageOutputPane
        {
            get
            {
                var outputPanes = Dte?.ToolWindows.OutputWindow.OutputWindowPanes;

                try
                {
                    return outputPanes.Item(PackageTitle);
                }
                catch (Exception)
                {
                    return outputPanes.Add(PackageTitle);
                }
            }
        }

        public CommandResult ActivateOutputWindow(string success = null, string problem = null)
        {
            try
            {
                Dte?.ToolWindows.OutputWindow.Parent.Activate();

                return new SuccessResult(success);
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult ActivateBuildOutputPane(string problem = null)
        {
            try
            {
                var outputPane = Dte?.ToolWindows.OutputWindow.OutputWindowPanes.Item("Build");

                if (outputPane is null)
                    return new ProblemResult("Unable to find build output pane");

                outputPane.Activate();

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult ActivatePackageOutputPane(string problem = null)
        {
            try
            {
                var outputPane = PackageOutputPane;

                if (outputPane is null)
                    return new ProblemResult("Unable to find package output pane");

                outputPane.Activate();

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult ActivateSolutionExplorer(string problem = null)
        {
            try
            {
                var solutionExplorer = Dte?.Windows.Item(vsWindowKindSolutionExplorer);

                if (solutionExplorer is null)
                    return new ProblemResult("Unable to find solution explorer");

                solutionExplorer.Activate();

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult RestartVisualStudio(bool confirm = true, bool saveFiles = true, bool elevated = false)
        {
            try
            {
                if (!GetRestartConfirmation(saveFiles, elevated))
                    return new CancelledResult();

                if (saveFiles && !SaveAllFiles().Succeeded)
                    return new ProblemResult(message: "Unable to save open files");

                return (elevated)
                    ? RestartElevated()
                    : RestartNormal();
            }
            catch (Exception ex)
            {
                return new ProblemResult(message: ex.ExtendedMessage());
            }
        }

        private static bool GetRestartConfirmation(bool saveFiles, bool elevated)
        {
            var suffix = (elevated) ? " As Administrator" : "";
            var message = (saveFiles) ? " Any open files will automatically be saved for you first though." : "";

            return DisplayQuestion(messageText: $"You're about to restart Visual Studio{suffix}.{message}");
        }

        private CommandResult RestartNormal(string problem = null)
        {
            const uint mode = (uint)__VSRESTARTTYPE.RESTART_Normal;

            return Restart(mode, problem);
        }

        private CommandResult RestartElevated(string problem = null)
        {
            const uint mode = (uint)__VSRESTARTTYPE.RESTART_Elevated;

            return Restart(mode, problem);
        }

        private CommandResult Restart(uint mode, string problem = null)
        {
            try
            {
                return ErrorHandler.Failed(VsShell.Restart(mode))
                    ? new ProblemResult(message: problem ?? "Unable to restart Visual Studio")
                    : (CommandResult)new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ProblemResult(message: problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult SaveAllFiles(string success = null, string problem = null)
           => ExecuteCommand(SaveAll, success: success, problem: problem);

        public static Result DisplayMessage(string title = null, string message = "", Button button = Button.OK, Icon icon = Icon.None)
            => MessageBox.Show(message, title, button, icon);

        public static bool DisplayQuestion(string title = null, string messageText = null, string questionText = "")
        {
            var message = messageText.JoinWith(questionText, separator: NewLine + NewLine);

            return (DisplayMessage(title: title ?? "Question", message: message,
                button: Button.YesNo, icon: Icon.Question) == Result.Yes);
        }

        public static bool DisplayConfirm(string title = null, string messageText = null, string questionText = "")
        {
            var message = messageText.JoinWith(questionText, separator: NewLine + NewLine);

            return (DisplayMessage(title: title ?? "Please Confirm", message: message,
                button: Button.YesNoCancel, icon: Icon.Warning) == Result.Yes);
        }

        public static void DisplaySuccess(string title = null, string message = "")
            => DisplayMessage(title: title ?? "Success", message: message, icon: Icon.Exclamation);

        public static void DisplayProblem(string title = null, string message = "")
            => DisplayMessage(title: title ?? "Problem", message: message, icon: Icon.Error);

        public static void DisplayCancelled(string title = null, string message = "")
            => DisplayMessage(title: title ?? "Cancelled", message: message, icon: Icon.Information);

        public static void DisplayInformation(string title = null, string message = "")
            => DisplayMessage(title: title ?? "Please Note", message: message, icon: Icon.Information);

        public static void DisplayNotImplemented(string title = null, string message = "")
            => DisplayInformation(title: title ?? "Not Implemented", message: message);

        public static T GetDialogPage<T>()
            where T : DialogPage
            => (T)Instance.GetDialogPage(typeof(T));

        public CommandResult ShowToolWindow<T>(string problem = null)
            where T : ToolWindowPane
        {
            try
            {
                var toolWindow = FindToolWindow(typeof(T), 0, true);

                if (toolWindow is null)
                    throw new Exception("Unable to create window");

                var windowFrame = (IVsWindowFrame)toolWindow.Frame;

                if (windowFrame is null)
                    throw new Exception("Unable to access window frame");


                ErrorHandler.ThrowOnFailure(windowFrame.Show());
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }

        }

        public CommandResult ShowNewToolWindow<T>(int maxWindows = 1, string problem = null)
            where T : ToolWindowPane
        {
            try
            {
                for (var id = 0; id < maxWindows; id++)
                {
                    var window = FindToolWindow(typeof(T), id, create: false);

                    // does the window already exist?
                    if (window != null)
                        continue;

                    // create a new window using the first non-existing id
                    window = FindToolWindow(typeof(T), id, create: true);

                    if (window is null)
                        return new ProblemResult("Unable to create a new window");

                    var windowFrame = (IVsWindowFrame)window?.Frame;
                    if (windowFrame is null)
                        return new ProblemResult("Unable to access window frame");

                    ErrorHandler.ThrowOnFailure(windowFrame.Show());

                    return new SuccessResult();
                }
                return new ProblemResult($"Maximum of {maxWindows} new windows already created");
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult ShowOptions(Guid guid, string problem = null)
            => ExecuteCommand(ToolsOptions, guid.ToString(), problem: problem ?? "Unable to show options");

        public CommandResult ShowOptionsPage<T>(string problem = null)
           where T : DialogPage
        {
            try
            {
                ShowOptionPage(typeof(T));

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult EditSelectedProject(string problem = null)
        {
            try
            {
                var name = GetSelectedItem()?.Project.FullName;
                var result = UnloadSelectedProject(problem: problem);

                return result.Succeeded
                    ? OpenCodeFile(name, problem)
                    : result;
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        protected SelectedItem GetSelectedItem()
           => Dte?.SelectedItems.Item(1);

        public int GetSelectedItemCount()
            => (Dte?.SelectedItems.Count).ToInt();

        public CommandResult UnloadSelectedProject(string problem = null)
            => ExecuteCommand(UnloadProject, problem: problem);

        public CommandResult CloseSolution(string problem = null)
        {
            try
            {
                Dte?.Solution.Close();

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ProblemResult(message: problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult EditSolution(string problem = null)
        {
            try
            {
                CommandResult result = null;
                var fullName = Dte?.Solution.FullName;

                result = CloseSolution(problem);

                return result.Succeeded
                    ? OpenCodeFile(fullName, problem)
                    : result;
            }
            catch (Exception ex)
            {
                return new ProblemResult(message: problem ?? ex.ExtendedMessage());
            }
        }

        protected IWpfTextViewHost GetCurrentViewHost()
        {
            const int mustHaveFocus = 1;

            var textManager = GetService<SVsTextManager, IVsTextManager>();
            if (textManager == null)
                return null;

            textManager.GetActiveView(mustHaveFocus, null, out var textView);

            if (!(textView is IVsUserData userData))
                return null;

            var guidViewHost = guidIWpfTextViewHost;

            userData.GetData(ref guidViewHost, out var holder);

            return (IWpfTextViewHost)holder;
        }
    }
}