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
        private static ParsedArgs _parsed;
        public static T? Get<T>(string key) => (T?)_parsed[key];
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
                        
                        Type fieldType = field.FieldType;
                        field.SetValue(null, Get(field.Name, field.FieldType));
                    }
                }
            }
        }
    }
}
