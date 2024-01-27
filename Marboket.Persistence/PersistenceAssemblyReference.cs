using System.Reflection;

namespace Marboket.Persistence;

public sealed class PersistenceAssemblyReference
{
    public static Assembly Assembly => Assembly.GetExecutingAssembly();
}

