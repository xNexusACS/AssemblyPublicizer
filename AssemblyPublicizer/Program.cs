using dnlib.DotNet;

namespace AssemblyPublicizer;

internal static class Program
{
    public static void Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
                return;

            var loadModule = ModuleDefMD.Load(args[0], new ModuleCreationOptions());
                
            var location = args.Length > 1 ? args[1] : null;
                
            var publicizer = new Publicizer(location);

            publicizer.Start(loadModule);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Thread.Sleep(10000);
        }

        Thread.Sleep(2000);
    }
}