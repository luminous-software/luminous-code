using System;
using System.ComponentModel.Design;

namespace Luminous.Code.VisualStudio.Commands
{
    using Microsoft.VisualStudio.Shell;
    using Packages;

    public abstract class StaticCommand : CommandBase
    {
        protected StaticCommand(PackageBase package, int id) : base(package, id)
        { }

        protected static void Instantiate(StaticCommand instance)
        {
            Instance = instance;

            var id = new CommandID(Package.CommandSet, Id);
            var command = new MenuCommand(instance.ExecuteHandler, id);

            Package?.CommandService?.AddCommand(command);
        }

        protected virtual void OnExecute(OleMenuCommand command)
        {
        }

        private void ExecuteHandler(object sender, EventArgs e)

        {
            if (!(sender is OleMenuCommand command))
                return;

            OnExecute(command);
        }
    }
}