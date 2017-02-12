using System;

namespace Luminous.Code.VisualStudio.Solutions
{
    public static class SolutionItemKind
    {
        //***

        public const string SharedProject_string = "{d954291e-2a0b-460d-934e-dc6b0785db48}";

        public const string CSharpProject_string = "{fae04ec0-301f-11d3-bf4b-00c04f79efbc}";

        public const string FSharpProject_string = "{f2a71f9b-5d33-465a-a702-920d77279786}";

        public const string VBProject_string = "{f184b08f-c81c-45f6-a57f-5abd9991f28f}";

        public const string VSA_string = "{13b7a3ee-4614-11d3-9bc7-00c04f79de25}";

        public const string ProjectAsSolutionFolder_string = "{66a26720-8fb5-11d2-aa7e-00c04f688dde}";

        public const string ProjectItemAsSolutionFolder_string = "{66a26722-8fb5-11d2-aa7e-00c04f688dde}";

        public const string VisualCPPProject_string = "{8bc9ceb8-8b4a-11d0-8d11-00a0c91bc942}";

        public const string VisualJSharpProject_string = "{e6fdf86b-f3d1-11d4-8576-0002a516ece8}";

        public const string WebProject_string = "{e24c65dc-7377-472b-9aba-bc803b73c61a}";

        public const string WebApplicationProject_string = "{349c5851-65df-11da-9384-00065b846f21}";

        public const string Project_string = "{66a26722-8fb5-11d2-aa7e-00c04f688dde}";

        public const string Folder_string = "{6bb5f8ef-4483-11d3-8bcf-00c04f8ec28c}";

        public const string File_string = "{6bb5f8ee-4483-11d3-8bcf-00c04f8ec28c}";

        public const string MiscFolder_string = "{66a2671d-8fb5-11d2-aa7e-00c04f688dde}";

        public const string SetupProject_string = "{54435603-dbb4-11d2-8724-00a0c9a8b90c}";

        public const string UnmodeledProject_string = "{67294a52-a4f0-11d2-aa88-00c04f688dde}";

        //---

        public readonly static Guid SharedProject = new Guid(SharedProject_string);

        public readonly static Guid CSharpProject = new Guid(CSharpProject_string);

        public readonly static Guid FSharpProject = new Guid(FSharpProject_string);

        public readonly static Guid VBProject = new Guid(VBProject_string);

        public readonly static Guid VSA = new Guid(VSA_string);

        public readonly static Guid ProjectAsSolutionFolder = new Guid(ProjectAsSolutionFolder_string);

        public readonly static Guid ProjectItemAsSolutionFolder = new Guid(ProjectItemAsSolutionFolder_string);

        public readonly static Guid VisualCPPProject = new Guid(VisualCPPProject_string);

        public readonly static Guid VisualJSharpProject = new Guid(VisualJSharpProject_string);

        public readonly static Guid WebProject = new Guid(WebProject_string);

        public readonly static Guid WebApplicationProject = new Guid(WebApplicationProject_string);

        public readonly static Guid Project = new Guid(Project_string);

        public readonly static Guid Folder = new Guid(Folder_string);

        public readonly static Guid File = new Guid(File_string);

        public readonly static Guid MiscFolder = new Guid(MiscFolder_string);

        public readonly static Guid SetupProject = new Guid(SetupProject_string);

        public readonly static Guid UnmodeledProject = new Guid(UnmodeledProject_string);

        //***
    }
}