using System.Reflection;

[assembly: AssemblyCompany(Company.Name)]
[assembly: AssemblyCopyright(Company.Copyright)]
[assembly: AssemblyTrademark(Company.Trademark)]

static class Company
{
    public const string Name = "Luminous Software Solutions";
    public const string Copyright = "© 2017 " + Name;
    public const string Trademark = "";
}