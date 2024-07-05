using dnlib.DotNet;
using FieldAttributes = dnlib.DotNet.FieldAttributes;
using MethodAttributes = dnlib.DotNet.MethodAttributes;
using TypeAttributes = dnlib.DotNet.TypeAttributes;

namespace AssemblyPublicizer;

public class Publicizer(string? outputPath = null)
{
    private readonly string _outputPath = outputPath ?? Path.Combine(Environment.CurrentDirectory + @"\Publicized-Assemblies");
        
    private AssemblyDef? _assembly;

    public void Start(ModuleDef loadModule)
    { 
        _assembly = loadModule.Assembly;
        
        SaveDefaultAssembly(loadModule);
        PubliciseAssembly(loadModule);
    }
        
    private void SaveDefaultAssembly(ModuleDef def)
    {
        if (!Directory.Exists(_outputPath))
            Directory.CreateDirectory(_outputPath);

        def.Write(Path.Combine(_outputPath, $"{_assembly?.Name.String}.dll"));
        
        Console.WriteLine($"Wrote {_assembly?.Name.String}.dll to {Stringify(_outputPath)} folder");
    }

    private void PubliciseAssembly(ModuleDef def)
    {
        var types = def.Assembly.ManifestModule.Types.ToList();
        var nested = new List<TypeDef>();

        foreach (var type in types)
        {
            if (!type.IsPublic)
            {
                var isInterface = type.IsInterface;
                var isAbstract = type.IsAbstract;

                type.Attributes = type.IsNested ? TypeAttributes.NestedPublic : TypeAttributes.Public;
                    
                if (isInterface)
                    type.IsInterface = true;
                
                if (isAbstract)
                    type.IsAbstract = true;
            }
                
            if (type.CustomAttributes.Find("System.Runtime.CompilerServices.CompilerGeneratedAttribute") != null)
                continue;
                
            nested.AddRange(type.NestedTypes.ToList());
        }
            
        foreach (var classDef in nested.Where(nest => nest.CustomAttributes.Find("System.Runtime.CompilerServices.CompilerGeneratedAttribute") == null))
            classDef.Attributes = classDef.IsNested ? TypeAttributes.NestedPublic : TypeAttributes.Public;
            
        types.AddRange(nested);
            
        foreach (var methodDef in types.SelectMany(typeDef => typeDef.Methods).Where(method => !method?.IsPublic ?? false))
            methodDef.Access = MethodAttributes.Public;

        foreach (var field in from type in types
                 let events = type.Events.Select(defEvent => defEvent.Name).ToArray()
                 from field in type.Fields
                 let isEventBackingField = events.Any(storedField => string.Equals(storedField, field.Name, StringComparison.InvariantCultureIgnoreCase))
                 where (!field?.IsPublic ?? false) && !isEventBackingField select field)
        {
            field.Access = FieldAttributes.Public;
        }
            
        def.Write(Path.Combine(_outputPath, _assembly?.Name.String + "-Publicized.dll"));
        
        Console.WriteLine("Wrote " + _assembly?.Name.String + $"-Publicized.dll to {Stringify(_outputPath)} folder");
    }

    private static string Stringify(string input)
    {
        var lastIndex = input.LastIndexOf('\\');

        return input[(lastIndex + 1)..];
    }
}