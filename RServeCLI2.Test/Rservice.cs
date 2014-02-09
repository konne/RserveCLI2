//-----------------------------------------------------------------------
// Copyright (c) 2013, Suraj Gupta
// All rights reserved.
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Reflection;
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
            string assemblyDir = new Uri( Path.GetDirectoryName( Assembly.GetExecutingAssembly().CodeBase ) ).AbsolutePath;
            // ReSharper restore AssignNullToNotNullAttribute
            string rExeFilePath = Path.Combine( assemblyDir , "R-2.15.3" , "bin" , "x64" , "Rterm.exe" );

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

            // launch RTerm and tell it load Rserve, tell it wait until RServe has completed.  We want RTerm to stay open because
            // our only strategy for finding/killing RServe is to search RTerm's child processes.  If wait = FALSE then RTerm completes
            // but RServe does not die
            // ReSharper disable UseObjectOrCollectionInitializer
            _rtermProcess = new Process();
            _rtermProcess.StartInfo.FileName = rExeFilePath;
            _rtermProcess.StartInfo.Arguments = string.Format( "-e \"library( Rserve ); Rserve( port = {0} , wait = TRUE {1});\"" , Port , args );
            _rtermProcess.StartInfo.UseShellExecute = false;
            _rtermProcess.StartInfo.CreateNoWindow = !showWindow;
            _rtermProcess.Start();
            Thread.Sleep( 3000 );
            // ReSharper restore UseObjectOrCollectionInitializer

            // create a connection to the server
            // ReSharper disable RedundantArgumentDefaultValue
            RConnection = new RConnection( port: Port );
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
                    // dispose the connetion to server
                    if ( RConnection != null )
                    {
                        RConnection.Dispose();
                    }

                    // kill the server by searching/killing all of Rterm's child processes
                    KillAllProcessesSpawnedBy( _rtermProcess );
                    if ( !_rtermProcess.HasExited )
                    {
                        // killing RServe will allow Rterm to complete, but there could be a timing issue where HasExited = FALSE, but then
                        // the call to Kill() below cannot complete because the process is already dead
                        try
                        {
                            _rtermProcess.Kill();
                        }
                        // ReSharper disable EmptyGeneralCatchClause
                        catch ( Exception ) { }
                        // ReSharper restore EmptyGeneralCatchClause
                    }
                }
                _disposed = true;
            }
        }

        #endregion

        #region Private Members

        private bool _disposed; // to detect redundant calls
        private readonly Process _rtermProcess;

        /// <summary>
        /// Kills all processes spawned by a parent process
        /// </summary>
        /// <remarks>
        /// See:  http://stackoverflow.com/questions/7189117/find-all-child-processes-of-my-own-net-process-find-out-if-a-given-process-is
        /// </remarks>
        /// <param name="parentProcess"></param>
        private static void KillAllProcessesSpawnedBy( Process parentProcess )
        {

            // NOTE: Process Ids are reused!
            var searcher = new ManagementObjectSearcher( "SELECT * FROM Win32_Process WHERE ParentProcessId=" + parentProcess.Id );
            ManagementObjectCollection collection = searcher.Get();
            foreach ( var item in collection )
            {
                var childProcessId = ( UInt32 )item[ "ProcessId" ];
                if ( ( int )childProcessId != Process.GetCurrentProcess().Id )
                {
                    Process childProcess = Process.GetProcessById( ( int )childProcessId );
                    KillAllProcessesSpawnedBy( childProcess );
                    childProcess.Kill();
                }
            }
        }

        #endregion
    }
}
