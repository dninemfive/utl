using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl;
public partial class Log
{
    public static class Components
    {
        public static ILogComponent Console
            => new DelegateLogComponent(System.Console.Write, System.Console.WriteLine);
        public static ILogComponent FromStream(Stream stream)
            => new StreamLogComponent(stream);
        public static ILogComponent FromStreamWriter(StreamWriter sw)
            => new StreamLogComponent(sw);
        public static StreamLogComponent OpenFile(string path,
            FileMode mode = FileMode.Create,
            FileAccess access = FileAccess.ReadWrite,
            FileShare share = FileShare.ReadWrite)
        {
            FileStream fs = File.Open(path, mode, access, share);
            return new(fs);
        }
    }
}