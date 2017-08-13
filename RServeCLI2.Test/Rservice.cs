//-----------------------------------------------------------------------
// Copyright (c) 2013, Suraj Gupta
// All rights reserved.
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace RserveCLI2.Test
{

    /// <summary>
    /// Starts RServe and opens a connections to the server.
    /// </summary>
    /// <remarks>
    /// We are launching RServ using Rterm because its a reliable way to do that.
    /// R CMD Rserve requires RHOME to be in the registry.
    /// </remarks>
    public class Rservice : IDisposable
    {

        #region Constants and Fields

        /// <summary>
        /// The Sexp attributes, if any
        /// </summary>
        private const int Port = 6311;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a self-hosted Rserve.
        /// </summary>
        /// <param name="showWindow">If true then the Rserve window will be visible.  Useful for debugging.  Default is false.</param>
        /// <param name="maxInputBufferSizeInKb">The maximal allowable size of the input buffer in kilobytes.  That is, the maximal size of data transported from the client to the server.</param>
        public Rservice( bool showWindow = false , int maxInputBufferSizeInKb = 0 )
        {
            // ReSharper disable AssignNullToNotNullAttribute
#if RTERM_PROCESS
            var codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            var is64BitOperatingSystem = Environment.Is64BitOperatingSystem;
            string assemblyDir = new Uri( Path.GetDirectoryName( codeBase ) ).AbsolutePath;
            // ReSharper restore AssignNullToNotNullAttribute
            string rExeFilePath = Path.Combine( assemblyDir , "R-2.15.3" , "bin" , is64BitOperatingSystem ? "x64" : "i386" , "Rterm.exe" );

            // the only way to set maxinbuf is via configuration file
            // generate a config file and reference it as part of the args parameter to Rserve() below
            string args = "";
            if ( maxInputBufferSizeInKb > 0 )
            {
                string configFile = Path.GetTempFileName();
                // plaintext warning only shows when using a config file.  setting plaintext enable to eliminate the warning
                File.WriteAllText( configFile , "maxinbuf " + maxInputBufferSizeInKb + "\r\n" + "plaintext enable" );
                args = string.Format( ", args = '--RS-conf {0}' " , configFile.Replace( @"\" , "/" ) );
            }

            // launch RTerm and tell it load Rserve.
            // Keep RTerm open, otherwise the child process will be killed.
            // We will use CmdShutdown to stop the server
            // ReSharper disable UseObjectOrCollectionInitializer
            _rtermProcess = new Process();
            _rtermProcess.StartInfo.FileName = rExeFilePath;
            _rtermProcess.StartInfo.Arguments = string.Format( "--no-site-file --no-init-file --no-save -e \"library( Rserve ); Rserve( port = {0} , wait = TRUE {1});\"" , Port , args );
            _rtermProcess.StartInfo.UseShellExecute = false;
            _rtermProcess.StartInfo.CreateNoWindow = !showWindow;
            _rtermProcess.Start();
            Thread.Sleep( 3000 );
            // ReSharper restore UseObjectOrCollectionInitializer
#endif
            string hostname = "localhost";

            // create a connection to the server
            // ReSharper disable RedundantArgumentDefaultValue
            RConnection = RConnection.Connect( port: Port, hostname: hostname);
            // ReSharper restore RedundantArgumentDefaultValue
        }

#endregion

#region Properties

        /// <summary>
        /// Get the wrapped RConnection
        /// </summary>
        public RConnection RConnection { get; private set; }

#endregion

#region Public Members

        public void Dispose()
        {
            Dispose( true );
        }

#endregion

#region Interface Implimentations

        protected virtual void Dispose( bool disposing )
        {
            if ( !_disposed )
            {
                if ( disposing )
                {
                    // dispose the connection to server
                    if ( RConnection != null )
                    {
#if RTERM_PROCESS
                        // Kill the server
                        RConnection.Shutdown();
#endif
                        RConnection.Dispose();
                    }
                }
                _disposed = true;
            }
        }

#endregion

#region Private Members

        private bool _disposed; // to detect redundant calls
        
#if RTERM_PROCESS
        private readonly Process _rtermProcess;
#endif
        
#endregion
    }
}
