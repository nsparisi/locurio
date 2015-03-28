using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Abyss
{
    class Program
    {
        static void Main(string[] args)
        {

            CSharpCodeProvider provider = new CSharpCodeProvider();
            ICodeCompiler compiler = provider.CreateCompiler();

            CompilerParameters parameters = new CompilerParameters()
            {
                GenerateExecutable = false,
                GenerateInMemory = true
            };
            parameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);


            CompilerResults result = provider.CompileAssemblyFromFile(parameters, "MyScript.txt");

            if (result.Errors.HasErrors)
            {
                Console.WriteLine("Has Errors");
            }

            if (result.Errors.HasWarnings)
            {
                Console.WriteLine("Has Warning");
            }

            Assembly compiledAssembly = result.CompiledAssembly;

            foreach (Type type in compiledAssembly.GetExportedTypes())
            {
                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                foreach(MethodInfo method in type.GetMethods())
                {
                    if(method.Name == "Run")
                    {
                        object instance = constructor.Invoke(null);
                        method.Invoke(instance, null);
                    }
                }
            }

            Console.WriteLine("Done Execution");
        }
    }
}
