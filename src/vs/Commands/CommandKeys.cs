using System;

namespace Luminous.Code.VisualStudio.Commands
{
    public static class CommandKeys
    {
        public const string BuildProject = "Build.BuildSelection";
        public const string BuildSolution = "Build.BuildSolution";
        public const string CancelBuild = "Build.Cancel";
        public const string CloseSolution = "File.CloseSolution";
        public const string DeleteProject = "Project.DeleteProject";
        public const string ExtensionsAndUpdates = "Tools.ExtensionsAndUpdates";
        public const string FindInSolutionExplorer = "SolutionExplorer.SyncWithActiveDocument";
        public const string RebuildProject = "Build.RebuildSelection";
        public const string RebuildSolution = "Build.RebuildSolution";
        public const string SaveAll = "File.SaveAll";
        public const string ToolsOptions = "Tools.Options";
        public const string UnloadProject = "Project.UnloadProject";
        public const string ViewWebBrowser = "View.WebBrowser";

        public const string CustomOptionsGuid_string = "1D9ECCF3-5D2F-4112-9B25-264596873DC9";
        public const string KeyboardOptions_string = "BAFF6A1A-0CF2-11D1-8C8D-0000F87570EE";

        public static Guid CustomOptionsGuid = new Guid("1D9ECCF3-5D2F-4112-9B25-264596873DC9");
        public static Guid KeyboardOptions = new Guid("BAFF6A1A-0CF2-11D1-8C8D-0000F87570EE");
    }
}