using System;
using System.ComponentModel.Design;
using System.Windows;
using System.IO;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;
using EnvDTE80;

using OleInterop = Microsoft.VisualStudio.OLE.Interop;
using Button = System.Windows.MessageBoxButton;
using Icon = System.Windows.MessageBoxImage;
using Result = System.Windows.MessageBoxResult;

using static System.Environment;
using static Microsoft.VisualStudio.Editor.DefGuidList;
using static EnvDTE.Constants;

namespace Luminous.Code.VisualStudio.Packages
{
    using Commands;
    using Extensions.IntegerExtensions;
    using Exceptions.ExceptionExtensions;
    using Extensions.IWpfTextViewHostExtensions;
    using SelectedItemsExtensions;
    using Solutions;
    using static Strings.Concatenation;
    using static Commands.CommandKeys;

    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]

    public abstract class PackageBase : Package
    {
        //***

        public static PackageBase Instance { get; set; }

        protected string PackageTitle { get; }

        protected string PackageDescription { get; }

        internal Guid CommandSet { get; }

        //---

        private DTE2 _dte;

        protected DTE2 Dte
            => _dte ?? (_dte = GetDte());

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

        public bool SolutionIsSelected
        {
            get
            {
                var selectedItems = Dte?.SelectedItems;
                var itemType = selectedItems.GetSolutionExplorerItemType();

                return (itemType == SolutionExplorerItemType.Solution);
            }
        }

        public bool ProjectIsSelected
        {
            get
            {
                var selectedItems = Dte?.SelectedItems;
                var itemType = selectedItems.GetSolutionExplorerItemType();
                var x = selectedItems.GetSelectedProject();

                return (itemType == SolutionExplorerItemType.Project);
            }
        }

        //---

        private ServiceProvider _serviceProvider;

        public IServiceProvider ServiceProvider
            => _serviceProvider ?? (_serviceProvider = new ServiceProvider(Dte as OleInterop.IServiceProvider));

        //---

        private IMenuCommandService _commandService;

        internal IMenuCommandService CommandService
            => _commandService ?? (_commandService = GetService<IMenuCommandService>());

        //---

        private IVsShell4 _vsShell;

        protected IVsShell4 VsShell
            => _vsShell ?? (_vsShell = GetService<SVsShell, IVsShell4>());

        //---

        private IVsMonitorSelection _selectionService;

        public IVsMonitorSelection SelectionService
            => _selectionService ?? (_selectionService = GetGlobalService<SVsShellMonitorSelection, IVsMonitorSelection>());

        //!!!

        protected PackageBase(Guid commandSet, string title, string description)
        {
            CommandSet = commandSet;
            PackageTitle = title;
            PackageDescription = description;
            Instance = this;
        }

        //!!!

        //protected PackageBase(Guid commandSet, string title, string description) : this(commandSet, title, description, null)
        //{ }

        //protected PackageBase(string commandSet, string title, string description) : this(new Guid(commandSet), title, description)
        //{ }

        //===M

        protected override void Initialize()
        {
            base.Initialize();

            //DteEvents.OnStartupComplete += OnStartupComplete;
            //DteEvents.ModeChanged += OnModeChanged;
            //DteEvents.OnBeginShutdown += OnBeginShutdown;
        }

        //---

        public static DTE2 GetDte()
            => GetGlobalService<_DTE, DTE2>();

        //---

        public static TTarget GetGlobalService<TSource, TTarget>()
            where TTarget : class
            => (GetGlobalService(typeof(TSource)) as TTarget);

        public T GetService<T>()
            where T : class
            //=> (Instance.GetService(typeof(T)) as T);
            => (GetService(typeof(T)) as T);

        //public static TTarget GetService<TSource, TTarget>()
        public TTarget GetService<TSource, TTarget>()
            where TSource : class
            where TTarget : class
            => GetService(typeof(TSource)) as TTarget;

        //---

        private string _vsVersion;

        public string VsVersion
            => _vsVersion ?? (_vsVersion = Dte.Version);

        //---

        protected bool IsVisualStudio2010
            => VsVersion.Equals("10.0", StringComparison.Ordinal);

        protected bool IsVisualStudio2012
            => VsVersion.Equals("11.0", StringComparison.Ordinal);

        protected bool IsVisualStudio2013
            => VsVersion.Equals("12.0", StringComparison.Ordinal);

        protected bool IsVisualStudio2015
            => VsVersion.Equals("14.0", StringComparison.Ordinal);

        //---

        public static Result DisplayMessage(string title = null, string message = "", Button button = Button.OK, Icon icon = Icon.None)
            => MessageBox.Show(message, title, button, icon);

        //---

        public bool DisplayQuestion(string title = null, string messageText = null, string questionText = "")
        {
            //var separator = (messageText == null || questionText == null)
            //    ? ""
            //    : NewLine + NewLine;
            //var message = messageText + separator + questionText;

            var message = JoinStrings(first: messageText, second: questionText, separator: NewLine + NewLine);

            return (DisplayMessage(title: title ?? "Question", message: message,
                button: Button.YesNo, icon: Icon.Question) == Result.Yes);
        }

        public bool DisplayConfirm(string title = "Please Confirm", string messageText = null, string questionText = "")
        {
            var separator = (messageText == null || questionText == null)
                ? ""
                : NewLine + NewLine;
            var message = messageText + separator + questionText;

            return (DisplayMessage(title: title, message: message, button: Button.YesNoCancel, icon: Icon.Warning) == Result.Yes);
        }

        //---

        public static void DisplaySuccess(string title = null, string message = "")
        {
            DisplayMessage(title: title ?? "Success", message: message, icon: Icon.Exclamation);
        }

        public static void DisplayProblem(string title = null, string message = "")
        {
            DisplayMessage(title: title ?? "Problem", message: message, icon: Icon.Error);
        }

        public static void DisplayCancelled(string title = null, string message = "")
        {
            DisplayMessage(title: title ?? "Cancelled", message: message, icon: Icon.Information);
        }

        public static void DisplayInformation(string title = null, string message = "")
        {
            DisplayMessage(title: title ?? "Please Note", message: message, icon: Icon.Information);
        }

        public static void DisplayNotImplemented(string title = null, string message = "")
        {
            DisplayInformation(title: title ?? "Not Implemented", message: message);
        }

        //---

        protected SelectedItem GetSelectedItem()
            => Dte?.SelectedItems.Item(1);

        public int GetSelectedItemCount()
            => (Dte?.SelectedItems.Count).ToInt();

        //---

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

        public CommandResult SaveAllFiles(string success = null, string problem = null)
        {
            return ExecuteCommand(SaveAll, success: success, problem: problem);
        }

        public CommandResult RestartVisualStudio(bool confirm = true, bool saveFiles = true, bool elevated = false)
        {
            try
            {
                if (confirm)
                {
                    var confirmed = GetRestartConfirmation(elevated);

                    if (!confirmed)
                        return new CancelledResult();
                }

                if (saveFiles)
                {
                    var filesSaved = SaveAllFiles().Succeeded;

                    if (!filesSaved)
                        return new ProblemResult(message: "Unable to save open files");
                }

                if (elevated)
                    return RestartElevated();

                return RestartNormal();
            }
            catch (Exception ex)
            {
                return new ProblemResult(message: ex.ExtendedMessage());
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

        public CommandResult OpenCodeFile(string name, string problem = null)
        {
            try
            {
                Dte?.ItemOperations.OpenFile(name, vsViewKindCode);

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ProblemResult(message: problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult OpenLogFile(string name, string problem = null)
        {
            try
            {
                if (!File.Exists(name))
                    return new ProblemResult("Unable to open '{name}'");

                return ExecuteCommand(ViewWebBrowser, name, problem: problem);
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult OpenExtensionsAndUpdates(string problem = null)
        {
            try
            {
                return ExecuteCommand(ExtensionsAndUpdates);
            }
            catch (Exception ex)
            {
                return new ProblemResult(message: problem ?? ex.ExtendedMessage());
            }
        }

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

                try
                {
                    result = CloseSolution(problem);

                    if (!result.Succeeded)
                        return result;

                    return OpenCodeFile(fullName, problem);
                }
                catch (Exception ex)
                {
                    return new ProblemResult(message: problem ?? ex.ExtendedMessage());
                }
            }
            catch (Exception ex)
            {
                return new ProblemResult(message: problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult UnloadSelectedProject(string problem = null)
        {
            try
            {
                return ExecuteCommand(UnloadProject, problem: problem);
            }
            catch (Exception ex)
            {
                return new ProblemResult(message: problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult EditSelectedProject(string problem = null)
        {
            try
            {
                var name = GetSelectedItem()?.Project.FullName;

                var result = UnloadSelectedProject(problem: problem);
                if (!result.Succeeded)
                    return result;

                return OpenCodeFile(name, problem);
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult DeleteSelectedProject(string success = null, string problem = null)
        {
            try
            {
                try
                {
                    //TODO: get confirmation
                    //if (!ProjectIsSelected)
                    //    return new ProblemResult(problem ?? $"Selection is not a project");

                    return ExecuteCommand(DeleteProject, success: success, problem: problem);
                }
                catch (Exception ex)
                {
                    return new ProblemResult(problem ?? ex.ExtendedMessage());
                }
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult BuildProject(bool rebuild = true, string problem = null)
        {
            try
            {
                var action = rebuild ? CommandKeys.RebuildProject : CommandKeys.BuildProject;
                var verb = rebuild ? "rebuild" : "build";

                return ExecuteCommand(action, problem: problem ?? $"Unable to {verb} the current project");
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
                var action = rebuild ? CommandKeys.RebuildSolution : CommandKeys.BuildSolution;
                var verb = rebuild ? "rebuild" : "build";

                return ExecuteCommand(action, problem: problem ?? $"Unable to {verb} the solution");
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        public CommandResult CancelBuild(string problem = null)
        {
            try
            {
                return ExecuteCommand(CommandKeys.CancelBuild, problem: problem ?? "Unable to cancel the current build");
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        //---

        public CommandResult ShowToolWindow<T>(string problem = null)
            where T : ToolWindowPane
        {
            try
            {
                var window = FindToolWindow(typeof(T), 0, true);

                if ((window == null) || (window.Frame == null))
                {
                    return new ProblemResult("Unable to create window");
                }
                var windowFrame = (IVsWindowFrame)window.Frame;

                ErrorHandler.ThrowOnFailure(windowFrame.Show());

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        //---

        public CommandResult ShowOptions(Guid guid, string problem = null)
        {
            try
            {
                return ExecuteCommand(ToolsOptions, guid.ToString(), problem: problem);
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

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

        public static T GetDialogPage<T>()
            where T : DialogPage
            => (T)Instance.GetDialogPage(typeof(T));

        //---

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
                Dte?.ToolWindows.OutputWindow.OutputWindowPanes.Item("Build")?.Activate();

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
                Dte?.ToolWindows.OutputWindow.OutputWindowPanes.Item(PackageTitle)?.Activate();

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
                Dte?.Windows.Item(vsWindowKindSolutionExplorer).Activate();

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ProblemResult(problem ?? ex.ExtendedMessage());
            }
        }

        //---

        protected IWpfTextViewHost GetCurrentViewHost()
        {
            const int mustHaveFocus = 1;

            var textManager = GetService<SVsTextManager, IVsTextManager>();
            if (textManager == null)
                return null;

            textManager.GetActiveView(mustHaveFocus, null, out IVsTextView textView);

            var userData = textView as IVsUserData;
            if (userData == null)
                return null;

            var guidViewHost = guidIWpfTextViewHost;

            userData.GetData(ref guidViewHost, out object holder);

            return (IWpfTextViewHost)holder;
        }

        //---

        private bool GetRestartConfirmation(bool elevated)
        {
            var suffix = (elevated)
                    ? " As Administrator"
                    : "";

            return DisplayQuestion(messageText: $"You're about to restart Visual Studio{suffix}. "
                    + "Any open files will automatically be saved for you first though.");
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
                if (ErrorHandler.Failed(VsShell.Restart(mode)))
                {
                    return new ProblemResult(message: problem);
                }
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ProblemResult(message: problem ?? ex.ExtendedMessage());
            }
        }

        //---

        //private CommandResult EditProject(string name, string success = null, string problem = null)
        //{
        //    try
        //    {
        //        var result = ExecuteCommand("Project.UnloadProject", name, success: success, problem: problem);
        //        if (!result.Succeeded)
        //            return result;

        //        return OpenCodeFile(name, success, problem);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ProblemResult(problem ?? ex.ExtendedMessage());
        //    }
        //}

        //public CommandResult DeleteProject(string name, string success = null, string problem = null)
        //{
        //    try
        //    {
        //        return ExecuteCommand("Project.Delete", name, success: success, problem: problem);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ProblemResult(problem ?? ex.ExtendedMessage());
        //    }
        //}

        //***
    }
}