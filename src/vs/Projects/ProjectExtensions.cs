using EnvDTE;

namespace Luminous.Code.VisualStudio.Projects.ProjectExtensions
{
    using Solutions;

    public static class ProjectExtensions
    {
        public static SolutionExplorerItemType GetKind(this Project project)
        {
            if (project.Kind == SolutionItemKind.ProjectAsSolutionFolder_string)
            {
                return SolutionExplorerItemType.SolutionFolder;
            }
            return SolutionExplorerItemType.Project;
        }
    }
}