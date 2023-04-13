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
        private static IntermediateArgs _parsed;
        public static T? Get<T>(IConsoleArg<T> ica, string key) => ica.Parse(_parsed);
        public static object Get(string key, Type type) => Activator.CreateInstance(type, )
        public static void SetVariables()
        {
            // todo: assembly- and type-level attributes to filter faster
            foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(Type type in assembly.GetTypes())
                {
                    foreach(FieldInfo field in type.GetFields())
                    {
                        Attribute[] attributes = Attribute.GetCustomAttributes(field, typeof(IUntypedConsoleArg)); 
                        foreach(Attribute attribute in attributes)
                        {
                            if (attributes.Length > 1) throw new Exception($"Can't have multiple console arg attributes on one field!");
                        }
                        Type fieldType = field.FieldType;
                        field.SetValue(null, Get(field.Name, field.FieldType));
                    }
                }
            }
        }
    }
}
