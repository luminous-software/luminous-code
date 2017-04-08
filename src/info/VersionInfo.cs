using System.Reflection;
using Luminous.Code;

[assembly: AssemblyVersion(VersionNumber.AssemblyVersion)]
[assembly: AssemblyFileVersion(VersionNumber.AssemblyFileVersion)]
[assembly: AssemblyInformationalVersion(VersionNumber.InformationalVersion)]

namespace Luminous.Code
{
    internal static class VersionNumber
    {
        private const string Major = "1";
        private const string Minor = "0";
        private const string Revision = "0";
        private const string Build = "0";

        private const string RevisionVersion = Major + "." + Minor + "." + Revision;
        private const string BuildVersion = RevisionVersion + "." + Build;

        public const string AssemblyVersion = BuildVersion;       //for CLR (used by Nuget if InformationalVersion is not specified)
        public const string AssemblyFileVersion = BuildVersion;   //for Windows (such as in File Explorer)
        public const string InformationalVersion = BuildVersion;  //for humans (used by NuGet if specified)
    }
}