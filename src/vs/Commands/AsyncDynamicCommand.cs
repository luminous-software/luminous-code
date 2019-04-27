using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using Contexts = Microsoft.VisualStudio.Shell.Interop.UIContextGuids80;
using Tasks = System.Threading.Tasks;

namespace Luminous.Code.VisualStudio.Commands
{
    using Packages;

    public abstract class AsyncDynamicCommand : AsyncCommandBase, IDisposable
    {
        protected AsyncDynamicCommand(AsyncPackageBase package, int id) : base(package, id)
        { }

        protected async static Tasks.Task InstantiateAsync(AsyncDynamicCommand instance)
        {
            var commandID = new CommandID(Package.CommandSet, Id);
            var command = new OleMenuCommand(instance.ExecuteHandler, instance.ChangeHandler, instance.QueryStatusHandler, commandID);

            Instance = instance;

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            Package?.CommandService?.AddCommand(command);
        }

        protected virtual bool CanExecute
            => true;

        protected virtual bool IsActive
            => true;

        protected virtual string Text
            => null;

        protected virtual bool IsVisible
            => CanExecute;

        protected virtual bool IsEnabled
            => (CanExecute && IsActive);

        protected virtual void OnExecute(OleMenuCommand command)
        { }

        protected virtual void OnChange(OleMenuCommand command)
        { }

        protected virtual void OnQueryStatus(OleMenuCommand command)
        {
            var visible = IsVisible;
            var enabled = IsEnabled;
            var text = Text;

            command.Visible = visible;
            command.Enabled = enabled;
            command.Text = text ?? command.Text;
        }

        protected virtual void OnDisposeManaged(AsyncDynamicCommand command)
        { }

        protected virtual void OnDisposeUnmanaged(AsyncDynamicCommand command)
        { }

        protected static bool HasSolution
            => ContextIsActive(Contexts.SolutionExists);

        protected static bool HasNoSolution
            => ContextIsActive(Contexts.NoSolution);

        protected static bool SolutionIsBuilding
            => ContextIsActive(Contexts.SolutionBuilding);

        protected static bool SolutionIsNotBuilding
            => !SolutionIsBuilding;

        protected static bool SolutionIsEmpty
            => ContextIsActive(Contexts.EmptySolution);

        protected static bool SolutionIsNotEmpty
            => !SolutionIsEmpty;

        protected static bool SolutionHasSingleProject
            => ContextIsActive(Contexts.SolutionHasSingleProject);

        protected static bool SolutionHasMultipleProject
            => ContextIsActive(Contexts.SolutionHasMultipleProjects);

        protected static bool SolutionHasProjects
            => ContextIsActive(Contexts.SolutionHasSingleProject, Contexts.SolutionHasMultipleProjects);

        protected static bool SolutionExistsAndIsNotBuildingAndNotDebugging
            => ContextIsActive(Contexts.SolutionExistsAndNotBuildingAndNotDebugging);

        protected static bool BuildingOrDebugging
            => !NotBuildingOrDebugging;

        protected static bool NotBuildingOrDebugging
            => ContextIsActive(Contexts.NotBuildingAndNotDebugging);

        protected static bool Debugging
            => ContextIsActive(Contexts.Debugging);

        protected static bool NotDebugging
            => !Debugging;

        protected static bool InDesignMode
            => ContextIsActive(Contexts.DesignMode);

        protected static bool NotInDesignMode
            => !InDesignMode;

        protected static bool InCodeWindow
            => ContextIsActive(Contexts.CodeWindow);

        protected static bool NotInCodeWindow
            => !InCodeWindow;

        protected static bool Dragging
            => ContextIsActive(Contexts.Dragging);

        protected static bool NotDragging
            => !Dragging;

        protected static bool SolutionOrProjectIsUpgrading
            => ContextIsActive(Contexts.SolutionOrProjectUpgrading);

        protected static bool SolutionOrProjectIsNotUpgrading
            => SolutionOrProjectIsUpgrading;

        private void ExecuteHandler(object sender, EventArgs e)
        {
            if (!(sender is OleMenuCommand command))
                return;

            OnExecute(command);
        }

        private void ChangeHandler(object sender, EventArgs e)
        {
            if (!(sender is OleMenuCommand command))
                return;

            OnChange(command);
        }

        private void QueryStatusHandler(object sender, EventArgs e)
        {
            if (!(sender is OleMenuCommand command))
                return;

            OnQueryStatus(command);
        }

        #region IDisposable Support
        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                OnDisposeManaged(this);
            }

            OnDisposeUnmanaged(this);

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}