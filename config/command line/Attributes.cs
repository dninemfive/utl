using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
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
    /// <summary>
    /// Specifies that the given field or property is a <see cref="CommandLineArgParser{T}"/> which will be registered in the
    /// <see cref="CommandLineArgParsers">relevant database</see> at startup.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CommandLineArgParserAttribute : Attribute
    {
        /// <summary>
        /// The name of this arg parser, which will be combined with the type it can parse (determined by reflection) to form the key in the database.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Defines a <see cref="CommandLineArgParserAttribute"/>.
        /// </summary>
        /// <param name="name"><inheritdoc cref="Name" path="/summary"/></param>
        public CommandLineArgParserAttribute(string name)
        {
            Name = name;
        }
    }
    /// <summary>
    /// If on a class, specifies that some member(s) have <see cref="CommandLineArgAttribute">command line args</see>.<br/><br/>
    /// If on an assembly, specifies that some type(s) in that assembly have such members.<br/><br/>
    /// Command-line arg fields and properties will <b>only</b> be initialized if this attribute is present on <u>both</u> their containing type
    /// and the assembly containing that type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
    public class HasCommandLineArgsAttribute : Attribute { }
    /// <summary>
    /// If on a class, specifies that some member(s) are <see cref="CommandLineArgParser{T}">command line arg parsers</see>.<br/><br/>
    /// If on an assembly, specifies that some type(s) in that assembly have such members.<br/><br/>
    /// Command-line arg parsers will <b>only</b> be registered if this attribute is present on <u>both</u> their containing type
    /// and the assembly containing that type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
    public class HasCommandLineArgParsersAttribute : Attribute { }
}
