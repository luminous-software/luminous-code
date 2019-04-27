using EnvDTE;

using static EnvDTE.Constants;

namespace Luminous.Code.VisualStudio.Extensions.ProjectItemExtensions
{
    using Solutions;

    internal static class ProjectItemExtensions
    {
        public static SolutionExplorerItemType GetSolutionExplorerItemType(this ProjectItem projectItem)
        {
            var itemKind = projectItem.Kind.ToLower();

            switch (itemKind)
            {
                case vsProjectItemKindPhysicalFolder:
                    return SolutionExplorerItemType.Folder;

                case vsProjectItemKindPhysicalFile:
                    return SolutionExplorerItemType.File;

                case vsProjectItemKindSolutionItems:
                    return SolutionExplorerItemType.SolutionFolder;

                default:
                    return SolutionExplorerItemType.Unknown;
            }

            //var lower = projectItem.Kind.ToLower();
            //var str = lower;
            //if (lower != null)
            //{
            //    //    internal const string VsProjectItemKindPhysicalFolder = "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";
            //    if (str == "{6bb5f8ef-4483-11d3-8bcf-00c04f8ec28c}")
            //    {
            //        return SolutionExplorerItemType.Folder;
            //    }
            //    //    internal const string VsProjectItemKindPhysicalFile = "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}";
            //    if (str == "{6bb5f8ee-4483-11d3-8bcf-00c04f8ec28c}")
            //    {
            //        return SolutionExplorerItemType.File;
            //    }
            //    //    internal const string VsProjectItemKindSolutionItem = "{66A26722-8FB5-11D2-AA7E-00C04F688DDE}";
            //    if (str == "{66a26722-8fb5-11d2-aa7e-00c04f688dde}")
            //    {
            //        return SolutionExplorerItemType.SolutionFolder;
            //    }
            //}
            //return SolutionExplorerItemType.Unknown;
        }
    }
}

//internal static class VsConstants
//{
//    // Project type guids
//    internal const string WebApplicationProjectTypeGuid = "{349C5851-65DF-11DA-9384-00065B846F21}";
//    internal const string WebSiteProjectTypeGuid = "{E24C65DC-7377-472B-9ABA-BC803B73C61A}";
//    internal const string CsharpProjectTypeGuid = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";
//    internal const string VbProjectTypeGuid = "{F184B08F-C81C-45F6-A57F-5ABD9991F28F}";
//    internal const string CppProjectTypeGuid = "{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}";
//    internal const string FsharpProjectTypeGuid = "{F2A71F9B-5D33-465A-A702-920D77279786}";
//    internal const string JsProjectTypeGuid = "{262852C6-CD72-467D-83FE-5EEB1973A190}";
//    internal const string WixProjectTypeGuid = "{930C7802-8A8C-48F9-8165-68863BCCD9DD}";
//    internal const string LightSwitchProjectTypeGuid = "{ECD6D718-D1CF-4119-97F3-97C25A0DFBF9}";
//    internal const string NemerleProjectTypeGuid = "{edcc3b85-0bad-11db-bc1a-00112fde8b61}";
//    internal const string InstallShieldLimitedEditionTypeGuid = "{FBB4BD86-BF63-432a-A6FB-6CF3A1288F83}";
//    internal const string WindowsStoreProjectTypeGuid = "{BC8A1FFA-BEE3-4634-8014-F334798102B3}";
//    internal const string SynergexProjectTypeGuid = "{BBD0F5D1-1CC4-42fd-BA4C-A96779C64378}";
//    internal const string NomadForVisualStudioProjectTypeGuid = "{4B160523-D178-4405-B438-79FB67C8D499}";
//}

//    // EnvDTE.Constants
//
//    internal const string VsProjectItemKindPhysicalFile = "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}";
//    internal const string VsProjectItemKindPhysicalFolder = "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";
//    internal const string VsProjectItemKindSolutionFolder = "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";
//    internal const string VsProjectItemKindSolutionItem = "{66A26722-8FB5-11D2-AA7E-00C04F688DDE}";
//    internal const string VsWindowKindSolutionExplorer = "{3AE79031-E1BC-11D0-8F78-00A0C9110057}";

//    A project item in the miscellaneous files folder of the solution.
//    public const string vsProjectItemKindMisc = "{66A2671F-8FB5-11D2-AA7E-00C04F688DDE}";

//    A project item located in the miscellaneous files folder of the solution.
//    public const string vsProjectItemsKindMisc = "{66A2671E-8FB5-11D2-AA7E-00C04F688DDE}";

//    A file in the system.
//    public const string vsProjectItemKindPhysicalFile = "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}";

//    A folder in the system.
//    public const string vsProjectItemKindPhysicalFolder = "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";

//    A project item type in the solution.
//    public const string vsProjectItemKindSolutionItems = "{66A26722-8FB5-11D2-AA7E-00C04F688DDE}";

//    A collection of items in the solution items folder of the solution.
//    public const string vsProjectItemsKindSolutionItems = "{66A26721-8FB5-11D2-AA7E-00C04F688DDE}";

//    A subproject under the project. If returned by EnvDTE.ProjectItem.Kind, then
//    EnvDTE.ProjectItem.SubProject returns as a EnvDTE.Project object.
//    public const string vsProjectItemKindSubProject = "{EA6618E8-6E24-4528-94BE-6889FE16485C}";

//    Indicates that the folder in the project does not physically appear on disk.
//    public const string vsProjectItemKindVirtualFolder = "{6BB5F8F0-4483-11D3-8BCF-00C04F8EC28C}";

//    All unloaded projects have this Kind value
//    internal const string UnloadedProjectTypeGuid = "{67294A52-A4F0-11D2-AA88-00C04F688DDE}";