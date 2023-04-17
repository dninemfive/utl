﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.console
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
    public class HasCommandLineArgsAttribute : Attribute { }
    [AttributeUsage(AttributeTargets.Class)]
    public class HasCommandLineArgParsersAttribute : Attribute { }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CommandLineArgParserAttribute : Attribute
    {
        public string Name { get; private set; }
        public CommandLineArgParserAttribute(string name)
        {
            Name = name;
        }
    }
}
