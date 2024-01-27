using System.Reflection;

namespace Marboket.Application;

public sealed class ApplicationAssemblyReference
{
    public static Assembly Assembly => Assembly.GetExecutingAssembly();
}

