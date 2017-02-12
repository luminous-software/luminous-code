using System;
using EnvDTE;
using VSLangProj;

namespace Luminous.Code.VisualStudio.SelectedItemsExtensions
{
    using Packages;
    using Solutions;
    using Projects.ProjectExtensions;
    using ProjectItems.ProjectItemExtensions;
    using VSHierarchies;
    using Microsoft.VisualStudio.Shell.Interop;
    using System.Runtime.InteropServices;

    public static class SelectedItemsExtensions
    {
        public static SolutionExplorerItemType GetSolutionExplorerItemType(this SelectedItems selectedItems)
        {
            try
            {
                var itemCount = selectedItems.Count;

                if (itemCount == 0)
                    return SolutionExplorerItemType.NoSelection;

                if (itemCount > 1)
                    return SolutionExplorerItemType.MultiSelection;

                var selectedItem = selectedItems.Item(1);
                var selectedItemName = selectedItem.Name;
                var project = selectedItem.Project;

                if (project == null || string.IsNullOrEmpty(selectedItemName))
                    return SolutionExplorerItemType.Unknown;

                if (selectedItemName.Equals("references", StringComparison.InvariantCultureIgnoreCase))
                    return SolutionExplorerItemType.ReferenceRoot;

                //var obj = selectedItems.GetSelectedItem();
                if (selectedItem == null)
                    return SolutionExplorerItemType.Solution;

                if (selectedItem is Project)
                    return (selectedItem as Project).GetKind();

                if (selectedItem is Reference)
                    return SolutionExplorerItemType.Reference;

                if (selectedItem is ProjectItem)
                    return (selectedItem as ProjectItem).GetSolutionExplorerItemType();

                return SolutionExplorerItemType.Unknown;
            }
            catch (Exception)
            {
                return SolutionExplorerItemType.Unknown;
            }
        }

        public static Project GetSelectedProject(this SelectedItems selectedItems)
        {
            try
            {
                var selectedItem = selectedItems.Item(1);
                if (!(selectedItem is Project))
                    return null;

                return (Project)selectedItem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public static object GetSelectedItem(this SelectedItems selectedItems)
        //{
        //    var vsHierarchyFromSelection = selectedItems.GetNodeInfoFromCurrentSelection();
        //    if (vsHierarchyFromSelection == null)
        //        return null;

        //    object obj;
        //    var hierarchy = vsHierarchyFromSelection.IVsHierarchy.GetProperty(vsHierarchyFromSelection.HierarchyNodeId, -2027, out obj);
        //    if (hierarchy != 0)
        //        return null;

        //    return obj;
        //}

        //public static IVSHierarchyNodeInfo GetNodeInfoFromCurrentSelection(this SelectedItems selectedItems)
        //{
        //    var selectionService = PackageBase.GetGlobalService<IVsMonitorSelection, IVsMonitorSelection>();
        //    if (selectionService == null)
        //        return null;

        //    IntPtr hierarchy;
        //    uint itemId;
        //    IVsMultiItemSelect multiItemSelect;
        //    IntPtr selectionContainer;

        //    var result = selectionService.GetCurrentSelection(out hierarchy, out itemId, out multiItemSelect, out selectionContainer);
        //    if ((result != 0) || (hierarchy == IntPtr.Zero))
        //        return null;

        //    var vsHierarchy = Marshal.GetObjectForIUnknown(hierarchy) as IVsHierarchy;
        //    Marshal.Release(hierarchy);

        //    return new IVSHierarchyNodeInfo
        //    {
        //        HierarchyNodeId = itemId,
        //        IVsHierarchy = vsHierarchy
        //    };
        //}
    }
}