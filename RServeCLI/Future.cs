// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RConnection.cs" company="Oliver M. Haynold">
// Oliver M. Haynold
// </copyright>
// <summary>
// A connection to an R session
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RserveCli
{
    using System;
    using System.IO;

    #if(FRAMEWORK_BELOW_4)

    /// <summary>
    /// Implementation of .NET 4.0 features on which this project relies
    /// </summary>
    public static class Future
    {
        /// <summary>
        /// Copy from one stream into another
        /// </summary>
        /// <param name="src">
        /// The source stream.
        /// </param>
        /// <param name="dest">
        /// The destination stream.
        /// </param>
        public static void CopyTo(this Stream src, Stream dest)
        {
            var size = src.CanSeek ? Math.Min((int)(src.Length - src.Position), 0x2000) : 0x2000;
            var buffer = new byte[size];
            int n;
            do
            {
                n = src.Read(buffer, 0, buffer.Length);
                dest.Write(buffer, 0, n);
            }
            while (n != 0);
        }

        /// <summary>
        /// Copy from one stream into another
        /// </summary>
        /// <param name="src">
        /// The source stream.
        /// </param>
        /// <param name="dest">
        /// The destination stream.
        /// </param>
        public static void CopyTo(this MemoryStream src, Stream dest)
        {
            dest.Write(src.GetBuffer(), (int)src.Position, (int)(src.Length - src.Position));
        }

        /// <summary>
        /// Copy from one stream into another
        /// </summary>
        /// <param name="src">
        /// The source stream.
        /// </param>
        /// <param name="dest">
        /// The destination stream.
        /// </param>
        public static void CopyTo(this Stream src, MemoryStream dest)
        {
            if (src.CanSeek)
            {
                var pos = (int)dest.Position;
                var length = (int)(src.Length - src.Position) + pos;
                dest.SetLength(length);

                while (pos < length)
                {
                    pos += src.Read(dest.GetBuffer(), pos, length - pos);
                }
            }
            else
            {
                src.CopyTo((Stream)dest);
            }
        }
    }

    #endif

}
