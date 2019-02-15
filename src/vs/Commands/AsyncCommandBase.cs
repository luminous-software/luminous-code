using System;

namespace Luminous.Code.VisualStudio.Commands
{
    using Packages;

    public abstract class AsyncCommandBase
    {
        protected static AsyncPackageBase Package { get; private set; }

        protected static int Id { get; private set; }

        protected static AsyncCommandBase Instance { get; set; }

        protected AsyncCommandBase(AsyncPackageBase package, int id)
        {
            Package = package ?? throw new ArgumentNullException(nameof(package));
            Id = id;
        }
    }
}