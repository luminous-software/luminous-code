using System.Reflection;
using Luminous.Code;

[assembly: AssemblyCompany(Company.Name)]
[assembly: AssemblyCopyright(Company.Copyright)]
[assembly: AssemblyTrademark(Company.Trademark)]

namespace Luminous.Code
{
    static class Company
    {
        public const string Name = "Luminous Software Solutions";
        public const string Copyright = "© 2017 " + Name;
        public const string Trademark = "";
    }
}