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
        //***

        //===M

        protected DynamicCommand(PackageBase package, int id) : base(package, id)
        { }

        //===M

        protected static void Instantiate(DynamicCommand instance)
        {
            Instance = instance;

            var commandID = new CommandID(Package.CommandSet, Id);
            var command = new OleMenuCommand(instance.ExecuteHandler, instance.ChangeHandler, instance.QueryStatusHandler, commandID);

            Package?.CommandService?.AddCommand(command);
        }

        //---

        protected virtual bool CanExecute
            => true;

        protected virtual bool IsActive
            => true;

        protected virtual string Text
            => null;

        //---

        protected virtual bool IsVisible
            => CanExecute;

        protected virtual bool IsEnabled
            => (CanExecute && IsActive);

        //---

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

        //protected int SelectedCount()
        //    => Package?.Ide?.SelectedItems.Count ?? 0;

        //---

        private void ExecuteHandler(object sender, EventArgs e)
        {
            var command = sender as OleMenuCommand;
            if (command == null) return;

            OnExecute(command);
        }

        private void ChangeHandler(object sender, EventArgs e)
        {
            var command = sender as OleMenuCommand;
            if (command == null) return;

            OnChange(command);
        }

        private void QueryStatusHandler(object sender, EventArgs e)
        {
            var command = sender as OleMenuCommand;
            if (command == null) return;

            OnQueryStatus(command);
        }

        //---

        protected bool HasSolution
            => ContextIsActive(SolutionExists);

        protected bool HasNoSolution
            => ContextIsActive(NoSolution);

        //---C

        protected bool SolutionIsBuilding
            => ContextIsActive(SolutionBuilding);

        protected bool SolutionIsNotBuilding
            => !SolutionIsBuilding;

        //---C

        protected bool SolutionIsEmpty
            => ContextIsActive(EmptySolution);

        protected bool SolutionIsNotEmpty
            => !SolutionIsEmpty;

        protected bool SolutionHasProjects
            => ContextIsActive(SolutionHasSingleProject, SolutionHasMultipleProjects);

        //---C

        protected bool SolutionOrProjectIsUpgrading
            => ContextIsActive(SolutionOrProjectUpgrading);

        protected bool SolutionOrProjectIsNotUpgrading
            => SolutionOrProjectIsUpgrading;

        //---C

        protected bool SolutionExistsAndIsNotBuildingOrDebugging
            => ContextIsActive(SolutionExistsAndNotBuildingAndNotDebugging);

        //---C

        protected bool BuildingOrDebugging
            => !NotBuildingOrDebugging;

        protected bool NotBuildingOrDebugging
            => ContextIsActive(NotBuildingAndNotDebugging);

        //---C

        protected bool Debugging
            => ContextIsActive(ShellInterop.UIContextGuids80.Debugging);

        protected bool NotDebugging
            => !Debugging;

        //---C

        protected bool InDesignMode
            => ContextIsActive(DesignMode);

        protected bool NotInDesignMode
            => !InDesignMode;

        //---C

        protected bool Dragging
            => ContextIsActive(Microsoft.VisualStudio.Shell.Interop.UIContextGuids80.Dragging);

        protected bool NotDragging
            => !Dragging;

        //***
    }
}