using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Tasks = System.Threading.Tasks;

namespace Luminous.Code.VisualStudio.Commands
{
    using Packages;

    public abstract class AsyncDynamicCommand : AsyncCommandBase, IDisposable
    {
        protected AsyncDynamicCommand(AsyncPackageBase package, int id) : base(package, id)
        { }

        public async static Tasks.Task InstantiateAsync(AsyncDynamicCommand instance)
        {
            var commandID = new CommandID(Package.CommandSet, Id);
            var command = new OleMenuCommand(instance.ExecuteHandler, instance.ChangeHandler, instance.QueryStatusHandler, commandID);

            Instance = instance;

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            Package?.CommandService?.AddCommand(command);
        }

        protected static void Instantiate(AsyncDynamicCommand instance)
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

        protected virtual void OnDisposeManaged(AsyncDynamicCommand command)
        { }

        protected virtual void OnDisposeUnmanaged(AsyncDynamicCommand command)
        { }

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