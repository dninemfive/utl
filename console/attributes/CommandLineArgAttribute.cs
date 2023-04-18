﻿using d9.utl.console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.console
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CommandLineArgAttribute : Attribute
    {
        public string Key { get; private set; }
        public string ParserKey { get; private set; }
        public string Description { get; private set; }
        public char Alias { get; private set; }        
        public CommandLineArgAttribute(string key, string parserKey, string description = "", char alias = Constants.NullCharacter)
        {
            Key = key;
            ParserKey = parserKey;
            Description = description;
            Alias = alias;
        }
    }
}
