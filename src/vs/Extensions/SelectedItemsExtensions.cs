using EnvDTE;
using System;
using VSLangProj;

namespace Luminous.Code.VisualStudio.SelectedItemsExtensions
{
    using Extensions.ProjectExtensions;
    using Extensions.ProjectItemExtensions;
    using Solutions;

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

                if ((project == null) || string.IsNullOrEmpty(selectedItemName))
                    return SolutionExplorerItemType.Unknown;

                if (selectedItemName.Equals("references", StringComparison.InvariantCultureIgnoreCase))
                    return SolutionExplorerItemType.ReferenceRoot;

                if (selectedItem is null)
                    return SolutionExplorerItemType.Solution;

                if (selectedItem is Project)
                    return (selectedItem as Project)?.GetKind() ?? SolutionExplorerItemType.Unknown;

                if (selectedItem is Reference)
                    return SolutionExplorerItemType.Reference;

                if (selectedItem is ProjectItem)
                    return (selectedItem as ProjectItem)?.GetSolutionExplorerItemType() ?? SolutionExplorerItemType.Unknown;

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
    }
}