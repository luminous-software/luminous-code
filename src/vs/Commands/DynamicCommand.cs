using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using ShellInterop = Microsoft.VisualStudio.Shell.Interop;

using static Microsoft.VisualStudio.Shell.Interop.UIContextGuids80;

namespace Luminous.Code.VisualStudio.Commands
{
    using Packages;

    public abstract class DynamicCommand : CommandBase
    {
        protected DynamicCommand(PackageBase package, int id) : base(package, id)
        { }

        protected static void Instantiate(DynamicCommand instance)
        {
            Instance = instance;

            var commandID = new CommandID(Package.CommandSet, Id);
            var command = new OleMenuCommand(instance.ExecuteHandler, instance.ChangeHandler, instance.QueryStatusHandler, commandID);

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

        protected static bool HasSolution
            => ContextIsActive(SolutionExists);

        protected static bool HasNoSolution
            => ContextIsActive(NoSolution);

        protected static bool SolutionIsBuilding
            => ContextIsActive(SolutionBuilding);

        protected static bool SolutionIsNotBuilding
            => !SolutionIsBuilding;

        protected static bool SolutionIsEmpty
            => ContextIsActive(EmptySolution);

        protected static bool SolutionIsNotEmpty
            => !SolutionIsEmpty;

        protected static bool SolutionHasProjects
            => ContextIsActive(SolutionHasSingleProject, SolutionHasMultipleProjects);

        protected static bool SolutionOrProjectIsUpgrading
            => ContextIsActive(SolutionOrProjectUpgrading);

        protected static bool SolutionOrProjectIsNotUpgrading
            => SolutionOrProjectIsUpgrading;

        protected static bool SolutionExistsAndIsNotBuildingOrDebugging
            => ContextIsActive(SolutionExistsAndNotBuildingAndNotDebugging);

        protected static bool BuildingOrDebugging
            => !NotBuildingOrDebugging;

        protected static bool NotBuildingOrDebugging
            => ContextIsActive(NotBuildingAndNotDebugging);

        protected static bool Debugging
            => ContextIsActive(ShellInterop.UIContextGuids80.Debugging);

        protected static bool NotDebugging
            => !Debugging;

        protected static bool InDesignMode
            => ContextIsActive(DesignMode);

        protected static bool NotInDesignMode
            => !InDesignMode;

        protected static bool InCodeWindow
            => ContextIsActive(CodeWindow);

        protected static bool NotInCodeWindow
            => !InCodeWindow;

        protected static bool Dragging
            => ContextIsActive(Microsoft.VisualStudio.Shell.Interop.UIContextGuids80.Dragging);

        protected static bool NotDragging
            => !Dragging;
    }
}