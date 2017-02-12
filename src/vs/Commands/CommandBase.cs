using System;
using Microsoft.VisualStudio.Shell.Interop;

namespace Luminous.Code.VisualStudio.Commands
{
    using Luminous.Code.VisualStudio.Packages;

    public abstract class CommandBase
    {
        private static IVsMonitorSelection _selectionService;

        //***

        protected static PackageBase Package { get; private set; }

        protected static int Id { get; private set; }

        protected static CommandBase Instance { get; set; }

        protected static IVsMonitorSelection SelectionService
            => _selectionService ?? (_selectionService = Package?.SelectionService);

        //===M

        protected CommandBase(PackageBase package, int id)
        {
            if (package == null)
                throw new ArgumentNullException(nameof(package));

            Package = package;
            Id = id;
        }

        //===M

        public bool ContextIsActive(params string[] contexts)
        {
            var result = false;

            foreach (var context in contexts)
            {
                result = (result || ContextIsActive(context));
            }

            return result;
        }

        //---

        private bool ContextIsActive(string context)
        {
            var contextGuid = new Guid(context);
            uint contextCookie;
            int active;

            SelectionService.GetCmdUIContextCookie(ref contextGuid, out contextCookie);
            SelectionService.IsCmdUIContextActive(contextCookie, out active);

            return (active == 1);
        }

        //***
    }
}