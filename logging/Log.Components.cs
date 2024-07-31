namespace d9.utl;
public partial class Log
{
    /// <summary>
    /// Commonly-used <see cref="ILogComponent"/> s.
    /// </summary>
    public static class Components
    {
        /// <summary>
        /// An <see cref="ILogComponent"/> which just writes to the <see cref="System.Console"/> as usual.
        /// </summary>
        public static ILogComponent Console
            => new DelegateLogComponent(System.Console.Write, System.Console.WriteLine);
        /// <summary>
        /// An <see cref="ILogComponent"/> which opens, writes to, and saves a given file on each call.
        /// </summary>
        /// <param name="filePath">The path of the file to write to.</param>
        public static ILogComponent WriteTextTo(string filePath)
            => new DelegateLogComponent((obj) => File.AppendAllText(filePath, $"{obj}"),
                                        (obj) => File.AppendAllText(filePath, $"{obj}\n"));
        /// <summary>
        /// An <see cref="ILogComponent"/> which writes to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="synchronize"><inheritdoc cref="TextWriterLogComponent(TextWriter, bool)" path="/param[@name='synchronize']"/></param>
        public static ILogComponent FromStream(Stream stream, bool synchronize = false)
            => new TextWriterLogComponent(stream, synchronize);
        /// <summary>
        /// An <see cref="ILogComponent"/> which uses the specified <see cref="StreamWriter"/>.
        /// </summary>
        /// <param name="streamWriter">
        /// The <see cref="StreamWriter"/> which will handle writing for this log component.
        /// </param>
        /// <param name="synchronize"><inheritdoc cref="TextWriterLogComponent(TextWriter, bool)" path="/param[@name='synchronize']"/></param>
        public static ILogComponent FromStreamWriter(StreamWriter streamWriter, bool synchronize = false)
            => new TextWriterLogComponent(streamWriter, synchronize);
        /// <summary>
        /// Creates a new <see cref="ILogComponent"/> which writes to the specified file using a stream.
        /// </summary>
        /// <param name="path">The path to the file which will be created.</param>
        /// <param name="mode">The <see cref="FileMode"/> the file stream will use.</param>
        /// <param name="access">
        /// The <see cref="FileAccess"/> type the file stream will have. MUST have the <c>Write</c>
        /// bit set, i.e. be either <c>Write</c> or <c>ReadWrite</c>.
        /// </param>
        /// <param name="share">The access other threads will have to the file in question.</param>
        /// <param name="synchronize"><inheritdoc cref="TextWriterLogComponent(TextWriter, bool)" path="/param[@name='synchronize']"/></param>
        /// <remarks>
        /// <b>NOTE:</b> by default, this will <b>overwrite</b> the file at the specified <paramref
        /// name="path"/>! You can change this by setting a different <paramref name="mode"/>.
        /// </remarks>
        public static ILogComponent OpenFile(string path,
            FileMode mode = FileMode.Create,
            FileAccess access = FileAccess.ReadWrite,
            FileShare share = FileShare.ReadWrite,
            bool synchronize = false)
        {
            if ((access & FileAccess.Write) != FileAccess.Write)
                throw new Exception($"Cannot use a FileStream for writing if it doesn't have the Write permission!");
            FileStream fs = File.Open(path, mode, access, share);
            return new TextWriterLogComponent(fs, synchronize);
        }
    }
}