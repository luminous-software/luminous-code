using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace Luminous.Code.VisualStudio.Commands
{
    using Packages;

    public abstract class AsyncCommandBase
    {
        private static IVsMonitorSelection _selectionService;
        protected static AsyncPackageBase Package { get; private set; }

        protected static int Id { get; private set; }

        protected static AsyncCommandBase Instance { get; set; }

        protected static IVsMonitorSelection SelectionService
            => _selectionService ?? (_selectionService = Package?.SelectionService);

        protected AsyncCommandBase(AsyncPackageBase package, int id)
        {
            Package = package ?? throw new ArgumentNullException(nameof(package));
            Id = id;
        }

        public static bool ContextIsActive(params string[] contexts)
        {
            foreach (var context in contexts)
            {
                if (ContextIsActive(context))
                    return true;
            }

            return false;
        }

        protected static bool ContextIsActive(string context)
        {
            var contextGuid = new Guid(context);

            return ContextIsActive(contextGuid);
        }

        protected static bool ContextIsActive(Guid context)
        {
            if (ErrorHandler.Failed(SelectionService.GetCmdUIContextCookie(ref context, out var contextCookie)))
                return false;

            if (ErrorHandler.Failed(SelectionService.IsCmdUIContextActive(contextCookie, out var active)))
                return false;

            return (active == 1);
        }
    }
}