using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.TypeSystem;
using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.Extensions.DependencyInjection;
using Mono.Cecil;
using System;
using System.IO;
using System.Linq;

namespace bilspy.Client {
    public class Program {
        static void Main(string[] args) {
            var serviceProvider = new BrowserServiceProvider(services => {
                // Add any custom services here
            });



            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }

    public static class Decompile {
        public static void Dec(string base64) {
            byte[] bytes = Convert.FromBase64String(base64);

            Console.WriteLine("decompiling file of size " + bytes.Length);
            using (var sr = new MemoryStream(bytes)) {
                var assemblyResolver = new MyAssemblyResolver();
                var parameters = new ReaderParameters(ReadingMode.Deferred) {
                    AssemblyResolver = assemblyResolver
                };
                Console.WriteLine("1");
                var module = ModuleDefinition.ReadModule(sr, parameters);
                Console.WriteLine("2");
                var dec = new CSharpDecompiler(module, new DecompilerSettings());
                Console.WriteLine("3");
                var allTypes = dec.TypeSystem.Compilation.GetAllTypeDefinitions();
                Console.WriteLine(allTypes.FirstOrDefault().FullName);
                Console.WriteLine("4");
                Console.WriteLine(dec.DecompileTypeAsString(new FullTypeName(allTypes.FirstOrDefault().FullName)));
                Console.WriteLine("5");
            }
        }
    }

    public class MyAssemblyResolver : IAssemblyResolver {
        public void Dispose() { }

        public AssemblyDefinition Resolve(AssemblyNameReference name) {
            Console.WriteLine("Resolve(name)");
            throw new AssemblyResolutionException(name);
        }

        public AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters) {
            Console.WriteLine("Resolve(name,parameters)");
            throw new AssemblyResolutionException(name);
        }
    }
}
