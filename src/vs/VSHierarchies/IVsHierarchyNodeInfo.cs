using Microsoft.VisualStudio.Shell.Interop;

namespace Luminous.Code.VisualStudio.VSHierarchies
{
    public class IVSHierarchyNodeInfo
    {
        public uint HierarchyNodeId { get; set; }

        public IVsHierarchy IVsHierarchy { get; set; }
    }
}