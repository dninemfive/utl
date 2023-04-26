using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
{
    public interface IConsole
    {
        public void Write(object? obj);
        public void WriteLine(object? obj);
    }
}
