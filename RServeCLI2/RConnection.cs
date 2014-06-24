//-----------------------------------------------------------------------
// Original work Copyright (c) 2011, Oliver M. Haynold
// Modified work Copyright (c) 2013, Suraj Gupta
// All rights reserved.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RserveCLI2
{
    
    /// <summary>
    /// A connection to an R session
    /// </summary>
    public class RConnection : IDisposable
    {
        #region Constants and Fields

        /// <summary>
        /// Assign a Sexp
        /// </summary>
        internal const int CmdAssignSexp = 0x021;

        /// <summary>
        /// Attach session
        /// </summary>
        internal const int CmdAttachSession = 0x032;

        /// <summary>
        /// Close a file
        /// </summary>
        internal const int CmdCloseFile = 0x012;

        /// <summary>
        /// Create a file for writing
        /// </summary>
        internal const int CmdCreateFile = 0x011;

        /// <summary>
        /// Control command Evaluate
        /// </summary>
        internal const int CmdCtrlEval = 0x42;

        /// <summary>
        /// Control command Shutdown
        /// </summary>
        internal const int CmdCtrlShutdown = 0x44;

        /// <summary>
        /// Control command Source
        /// </summary>
        internal const int CmdCtrlSource = 0x45;

        /// <summary>
        /// Detached evaluation without returning a result
        /// </summary>
        internal const int CmdDetachedVoidEval = 0x031;

        /// <summary>
        /// Detach session, but keep it around
        /// </summary>
        internal const int CmdDettachSession = 0x030;

        /// <summary>
        /// Evaluate a Sexp
        /// </summary>
        internal const int CmdEval = 0x003;

        /// <summary>
        /// Login with username and password
        /// </summary>
        internal const int CmdLogin = 0x001;

        /// <summary>
        /// Open a file for reading
        /// </summary>
        internal const int CmdOpenFile = 0x010;

        /// <summary>
        /// Read from an open file
        /// </summary>
        internal const int CmdReadFile = 0x013;

        /// <summary>
        /// Remove a file
        /// </summary>
        internal const int CmdRemoveFile = 0x015;

        /// <summary>
        /// Set buffer size
        /// </summary>
        internal const int CmdSetBufferSize = 0x081;

        /// <summary>
        /// Set Encoding (I recommend UTF8)
        /// </summary>
        internal const int CmdSetEncoding = 0x082;

        /// <summary>
        /// Set a Sexp
        /// </summary>
        internal const int CmdSetSexp = 0x020;

        /// <summary>
        /// Shut down the Server
        /// </summary>
        internal const int CmdShutdown = 0x004;

        /// <summary>
        /// Evaluate without returning a result
        /// </summary>
        internal const int CmdVoidEval = 0x002;

        /// <summary>
        /// Write to an open file
        /// </summary>
        internal const int CmdWriteFile = 0x014;

        /// <summary>
        /// The socket we use to talk to Rserve
        /// </summary>
        private readonly Socket _socket;

        /// <summary>
        /// The connection parameters we received from Rserve
        /// </summary>
        private string[] _connectionParameters;

        /// <summary>
        /// The protocol that handles communication for us
        /// </summary>
        private Qap1 _protocol;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the RConnection class.
        /// </summary>
        /// <param name="hostname">
        /// Hostname of the server
        /// </param>
        /// <param name="port">
        /// Port on which Rserve listens
        /// </param>
        /// <param name="user">
        /// User name for the user, or nothing for no authentication
        /// </param>
        /// <param name="password">
        /// Password for the user, or nothing
        /// </param>
        public RConnection( string hostname , int port = 6311 , string user = null , string password = null )
        {
            foreach ( IPAddress addr in Dns.GetHostEntry( hostname ).AddressList )
            {
                var ipe = new IPEndPoint( addr , port );
                var s = new Socket( ipe.AddressFamily , SocketType.Stream , ProtocolType.Tcp );
                s.Connect( ipe );
                if ( !s.Connected )
                {
                    continue;
                }

                _socket = s;
                break;
            }

            Init( user , password );
        }

        /// <summary>
        /// Initializes a new instance of the RConnection class.
        /// </summary>
        /// <param name="addr">
        /// Address of the Rserve server, or nothing for localhost
        /// </param>
        /// <param name="port">
        /// Port on which the server listens
        /// </param>
        /// <param name="user">
        /// User name, or nothing for no authentication
        /// </param>
        /// <param name="password">
        /// Password for the user
        /// </param>
        public RConnection( IPAddress addr = null , int port = 6311 , string user = null , string password = null )
        {
            if ( addr == null )
            {
                addr = new IPAddress( new byte[] { 127 , 0 , 0 , 1 } );
            }

            var ipe = new IPEndPoint( addr , port );
            _socket = new Socket( ipe.AddressFamily , SocketType.Stream , ProtocolType.Tcp );
            _socket.Connect( ipe );
            Init( user , password );
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Syntactic sugar for variable assignment and expression evaluation.
        /// </summary>
        /// <param name="s">The variable to be assigned to or the expression to be evaluated</param>
        /// <returns>The value of the expression</returns>
        public Sexp this[ string s ]
        {
            get
            {
                return Eval( s );
            }

            set
            {
                Assign( s , value );
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Assign an R variable on the server. this[symbol] = value is syntactic sugar for this operation.
        /// </summary>
        /// <param name="symbol">
        /// Variable name
        /// </param>
        /// <param name="val">
        /// Sexp to be assigned to the variable
        /// </param>
        public void Assign( string symbol , Sexp val )
        {
            _protocol.Command( CmdAssignSexp , new object[] { symbol , val } );
        }

        /// <summary>
        /// Evaluate an R command and return the result. this[string] is syntactic sugar for the same operation.
        /// </summary>
        /// <param name="s">
        /// Command to be evaluated
        /// </param>
        /// <returns>
        /// Sexp that resulted from the command
        /// </returns>
        public Sexp Eval( string s )
        {
            List<object> res = _protocol.Command( CmdEval , new object[] { s } );
            return ( Sexp )res[ 0 ];
        }

        /// <summary>
        /// Read a file from the server
        /// </summary>
        /// <param name="fileName">
        /// Name of the file to be read. Avoid jumping across directories.
        /// </param>
        /// <returns>
        /// Stream with the file data.
        /// </returns>
        public Stream ReadFile( string fileName )
        {
            _protocol.Command( CmdOpenFile , new object[] { fileName } );
            var resList = new List<byte>();
            while ( true )
            {
                byte[] res = _protocol.CommandReadStream( CmdReadFile , new object[] { } );
                if ( res.Length == 0 )
                {
                    break;
                }

                resList.AddRange( res );
            }

            _protocol.Command( CmdCloseFile , new object[] { } );
            return new MemoryStream( resList.ToArray() );
        }

        /// <summary>
        /// Delete a file from the server
        /// </summary>
        /// <param name="fileName">
        /// Name of the file to be deleted
        /// </param>
        public void RemoveFile( string fileName )
        {
            _protocol.Command( CmdRemoveFile , new object[] { fileName } );
        }

        /// <summary>
        /// Evaluate an R command and don't return the result (for efficiency)
        /// </summary>
        /// <param name="s">
        /// R command tp be evaluated
        /// </param>
        public void VoidEval( string s )
        {
            _protocol.Command( CmdVoidEval , new object[] { s } );
        }

        /// <summary>
        /// Write a file to the server.
        /// Note: It'd be better to return a Stream object to be written to, but Rserve doesn't seem to support an asynchronous connection
        /// for file reading and writing.
        /// </summary>
        /// <param name="fileName">
        /// Name of the file to be written. Avoid jumping across directories.
        /// </param>
        /// <param name="data">
        /// Data to be written to the file
        /// </param>
        public void WriteFile( string fileName , Stream data )
        {
            var ms = new MemoryStream();
            CopyTo( data , ms );

            _protocol.Command( CmdCreateFile , new object[] { fileName } );
            _protocol.Command( CmdWriteFile , new object[] { ms.ToArray() } );
            _protocol.Command( CmdCloseFile , new object[] { } );
        }

        /// <summary>
        /// Attempt to shut down the server process cleanly. 
        /// </summary>
        public void Shutdown()
        {
           _protocol.Command(CmdShutdown, new object[] { });
        }

        /// <summary>
        /// Attempt to shut down the server process cleanly. 
        /// This command is asynchronous!
        /// </summary>
        public void ServerShutdown()
        {
           _protocol.Command(CmdCtrlShutdown, new object[] { });
        }

        #endregion

        #region Implemented Interfaces

        #region IDisposable

        /// <summary>
        /// Dispose of the connection
        /// </summary>
        public void Dispose()
        {
            if ( _socket != null )
            {
                ( ( IDisposable )_socket ).Dispose();
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Common initialization routine called by the constructures
        /// </summary>
        /// <param name="user">
        /// User name, or null for no authentication
        /// </param>
        /// <param name="password">
        /// Password for the user, or null
        /// </param>
        private void Init( string user , string password )
        {
            var buf = new byte[ 32 ];
            int received = _socket.Receive( buf );
            if ( received == 0 )
            {
                throw new RserveException( "Rserve connection was closed by the remote host" );
            }

            var parms = new List<string>();
            for ( int i = 0 ; i < buf.Length ; i += 4 )
            {
                var b = new byte[ 4 ];
                Array.Copy( buf , i , b , 0 , 4 );
                parms.Add( Encoding.ASCII.GetString( b ) );
            }

            _connectionParameters = parms.ToArray();
            if ( _connectionParameters[ 0 ] != "Rsrv" )
            {
                throw new ProtocolViolationException( "Did not receive Rserve ID signature." );
            }

            if ( _connectionParameters[ 2 ] != "QAP1" )
            {
                throw new NotSupportedException( "Only QAP1 protocol is supported." );
            }

            _protocol = new Qap1( _socket );
            if ( _connectionParameters.Contains( "ARuc" ) )
            {
                string key = _connectionParameters.FirstOrDefault( x => !String.IsNullOrEmpty( x ) && x[ 0 ] == 'K' );
                key = String.IsNullOrEmpty( key ) ? "rs" : key.Substring( 1 , key.Length - 1 );

                Login( user , password , "uc" , key );
            }
            else if ( _connectionParameters.Contains( "ARpt" ) )
            {
                Login( user , password , "pt" );
            }
        }

        /// <summary>
        /// Login to Rserver
        /// </summary>
        /// <param name="user">
        /// User name for the user
        /// </param>
        /// <param name="password">
        /// Password for the user
        /// </param>
        /// <param name="method">
        /// pt for plain text
        /// </param>
        /// <param name="salt">
        /// The salt to use to encrypt the password
        /// </param>
        private void Login( string user , string password , string method , string salt = null )
        {
            if ( user == null )
            {
                throw new ArgumentNullException( "user" );
            }

            if ( password == null )
            {
                throw new ArgumentNullException( "password" );
            }

            switch ( method )
            {
                case "pt":
                    _protocol.Command( CmdLogin , new object[] { user + "\n" + password } );
                    break;
                case "uc":
                    password = new DES().Encrypt( password , salt );
                    _protocol.Command( CmdLogin , new object[] { user + "\n" + password } );
                    break;
                default:
                    throw new ArgumentException( "Could not interpret login method '" + method + "'" );
            }
        }

        /// <summary>
        /// Decompile of .NET 4's CopyTo
        /// </summary>
        public void CopyTo( Stream source , Stream destination )
        {
            if ( destination != null )
            {
                if ( source.CanRead || source.CanWrite )
                {
                    if ( destination.CanRead || destination.CanWrite )
                    {
                        if ( source.CanRead )
                        {
                            if ( destination.CanWrite )
                            {
                                CopyTo( source , destination , 4096 );
                                return;
                            }
                            throw new NotSupportedException( "Stream does not support writing" );
                        }
                        throw new NotSupportedException( "Stream does not support reading" );
                    }
                    throw new ObjectDisposedException( "destination" , "Can not access a closed Stream." );
                }
                throw new ObjectDisposedException( null , "Can not access a closed Stream." );
            }
            throw new ArgumentNullException( "destination" );
        }

        /// <summary>
        /// Decompile of .NET 4's InternalCopyTo
        /// </summary>
        private void CopyTo( Stream source , Stream destination , int bufferSize )
        {
            byte[] numArray = new byte[ bufferSize ];
            while ( true )
            {
                int num = source.Read( numArray , 0 , ( int )numArray.Length );
                int num1 = num;
                if ( num == 0 )
                {
                    break;
                }
                destination.Write( numArray , 0 , num1 );
            }
        }

        #endregion
    }
}