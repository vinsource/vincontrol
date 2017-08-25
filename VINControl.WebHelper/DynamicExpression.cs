using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VINControl.WebHelper
{
    public class DynamicExpression
    {
        private DynamicExpression()
        {

        }

        private static CompilerResults CompileScript(string expression)
        {
            var compiler = CodeDomProvider.CreateProvider("CSharp");
            var parms = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true,
                IncludeDebugInformation = false
            };

            return compiler.CompileAssemblyFromSource(parms, expression);
        }

        public static string Evaluate(string expression)
        {
            var code = string.Format  // Note: Use "{{" to denote a single "{"  
            (
                "public static class Func{{ public static string func(){{ return {0};}}}}",
                expression
            );

            CompilerResults compilerResults = CompileScript(code);

            if (compilerResults.Errors.HasErrors)
            {
                throw new InvalidOperationException("Expression has a syntax error.");
            }

            Assembly assembly = compilerResults.CompiledAssembly;
            MethodInfo method = assembly.GetType("Func").GetMethod("func");

            return (string)method.Invoke(null, null);
        }
    }
}
