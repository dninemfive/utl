using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.console
{
    public static class ConsoleArgs
    {
        private static IntermediateArgs _args;
        public static object? Get(IConsoleArg ica, string key) => ica.Parse(_args, key);
        public static void SetVariables(string[] args)
        {
            _args = new(args);
            // todo: assembly- and type-level attributes to filter faster
            foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(Type type in assembly.GetTypes())
                {
                    foreach(FieldInfo field in type.GetFields())
                    {
                        Attribute[] attributes = Attribute.GetCustomAttributes(field, typeof(IConsoleArg));
                        if (!attributes.Any()) continue;
                        if (attributes.Length > 1) throw new Exception($"Can't have multiple console arg attributes on one field!");
                        if(attributes.First() is IConsoleArg ica)
                        {
                            field.SetValue(null, Get(ica, field.Name));
                        }
                    }
                }
            }
        }
    }
}
