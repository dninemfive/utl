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
            => new DelegateLogComponent((obj) => File.WriteAllText(filePath, $"{obj}"),
                                        (obj) => File.WriteAllText(filePath, $"{obj}\n"));
        /// <summary>
        /// An <see cref="ILogComponent"/> which writes to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public static ILogComponent FromStream(Stream stream)
            => new StreamLogComponent(stream);
        /// <summary>
        /// An <see cref="ILogComponent"/> which uses the specified <see cref="StreamWriter"/>.
        /// </summary>
        /// <param name="streamWriter">
        /// The <see cref="StreamWriter"/> which will handle writing for this log component.
        /// </param>
        public static ILogComponent FromStreamWriter(StreamWriter streamWriter)
            => new StreamLogComponent(streamWriter);
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
        /// <remarks>
        /// <b>NOTE:</b> by default, this will <b>overwrite</b> the file at the specified <paramref
        /// name="path"/>! You can change this by setting a different <paramref name="mode"/>.
        /// </remarks>
        public static ILogComponent OpenFile(string path,
            FileMode mode = FileMode.Create,
            FileAccess access = FileAccess.ReadWrite,
            FileShare share = FileShare.ReadWrite)
        {
            if ((access & FileAccess.Write) != FileAccess.Write)
                throw new Exception($"Cannot use a FileStream for writing if it doesn't have the Write permission!");
            FileStream fs = File.Open(path, mode, access, share);
            return new StreamLogComponent(fs);
        }
    }
}