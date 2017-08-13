//-----------------------------------------------------------------------
// Copyright (c) 2013, Suraj Gupta
// All rights reserved.
// Adapted the Codesmith Custom Exception Template
// http://weblogs.asp.net/lhunt/pages/Codesmith-Custom-Exception-Template.aspx
//-----------------------------------------------------------------------

using System;
#if BINARY_SERIALIZATION
using System.Runtime.Serialization;
using System.Security.Permissions;
#endif
using System.Text;

namespace RserveCLI2
{
    
    /// <summary>
    /// The exception that is thrown when the client or server encounters an error.
    /// </summary>
    /// <remarks>
    /// Used for communication errors and when expectations on transmitted data are not met.
    /// </remarks>
    [Serializable]
    public class RserveException : Exception
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RserveException class.
        /// </summary>
        public RserveException()
        {
            HResult = _hResult;
        }

        /// <summary>
        /// Initializes a new instance of the RserveException class with the specified error message.
        /// </summary>
        public RserveException( string message )
            : base( message )
        {
            HResult = _hResult;
        }

        /// <summary>
        /// Initializes a new instance of the RserveException class with the specified error message,
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        public RserveException( string message , Exception innerException )
            : base( message , innerException )
        {
            HResult = _hResult;
        }

        /// <summary>
        /// Initializes a new instance of the RserveException class with the specified message,
        /// and the server error code.
        /// </summary>
        public RserveException( int serverErrorCode )
            : base( "Error received from server: " + ServerErrorCodeDescription( serverErrorCode ) )
        {
            HResult = _hResult;
            _serverErrorCode = serverErrorCode;
        }

#if BINARY_SERIALIZATION
        /// <summary>
        /// Initializes a new instance of the RserveException class with serialized data.
        /// </summary>
        protected RserveException( SerializationInfo info , StreamingContext context )
            : base( info , context )
        {
            _serverErrorCode = ( int? )info.GetValue( "RserveException_serverErrorCode" , typeof( int? ) );
            HResult = _hResult;
        }
#endif

        #endregion

        #region Properties

        /// <summary>
        /// Supports implicit casting to string
        /// </summary>
        public static implicit operator string( RserveException ex )
        {
            return ex.ToString();
        }

        /// <summary>
        /// Gets the server error code.  
        /// Returns null if RserveException was constructed without a server error code.
        /// </summary>
        public int? ServerErrorCode
        {
            get
            {
                return _serverErrorCode;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat( "{0}: {1}" , _className , Message );

            if ( _serverErrorCode != null )
            {
                sb.AppendFormat( "  Server error code: {0}.  " , _serverErrorCode );
                sb.Append( ServerErrorCodeDescription( ( int )_serverErrorCode ) );
            }

            if ( InnerException != null )
            {
                sb.AppendFormat( " ---> {0} <---" , InnerException );
            }

            if ( StackTrace != null )
            {
                sb.Append( Environment.NewLine );
                sb.Append( base.StackTrace );
            }

            return sb.ToString();
        }

#if BINARY_SERIALIZATION
/// <summary>
/// Sets the SerializationInfo object with the server error code.
/// (Overrides Exception.GetObjectData(SerializationInfo, StreamingContext).)
/// </summary>
        [SecurityPermission( SecurityAction.LinkDemand , Flags = SecurityPermissionFlag.SerializationFormatter )]
        public override void GetObjectData( SerializationInfo info , StreamingContext context )
        {
            base.GetObjectData( info , context );
            info.AddValue( "RserveException_serverErrorCode" , _serverErrorCode , typeof( int? ) );
        }
#endif

        #endregion

        #region Private Members

        // ReSharper disable InconsistentNaming
        const string _className = "RserveException";
        const int _hResult = -2146232832; // COR_E_APPLICATION
        readonly int? _serverErrorCode;
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Get the friendly description of a server error code.
        /// </summary>
        private static string ServerErrorCodeDescription( int serverErrorCode )
        {
            // with help from: https://github.com/s-u/REngine/blob/master/Rserve/RserveException.java
            switch ( serverErrorCode )
            {
                case 0:                      return "no error";
                case 2:                      return "R parser: input incomplete";
                case 3:                      return "R parser: syntax error";
                case Qap1.ErrAuthFailed:     return "authorization failed";
                case Qap1.ErrConnBroken:     return "connection closed or broken packet killed it";
                case Qap1.ErrInvCmd:         return "unsupported/invalid command";
                case Qap1.ErrInvPar:         return "some parameters are invalid";
                case Qap1.ErrRerror:         return "R-error occured";
                case Qap1.ErrIoError:        return "i/o error (server-side)";
                case Qap1.ErrNotOpen:        return "attempt to perform file Read/Write on closed file.";
                case Qap1.ErrAccessDenied:   return "access denied (local to the server)";
                case Qap1.ErrUnsupportedCmd: return "unsupported command - known to the server but for some reason (e.g. platform dependent) it's not suported";
                case Qap1.ErrUnknownCmd:     return "unknown command - not recognized by server";
                case Qap1.ErrDataOverflow:   return "data overflow, incoming data too big";
                case Qap1.ErrObjectTooBig:   return "evaluation successful, but returned object is too big to transport";
                case Qap1.ErrOutOfMem:       return "server ran out of memory, closing connection";
                case Qap1.ErrCtrlClosed:     return "control pipe to the master process is closed or broken";
                case Qap1.ErrSessionBusy:    return "session is still busy";
                case Qap1.ErrDetachFailed:   return "unable to detach seesion (cannot determine peer IP or problems creating a listening socket for resume)";
                case Qap1.ErrDisabled:       return "feature is disabled";
                case Qap1.ErrUnavailable:    return "feature is not present in this build";
                case Qap1.ErrCryptError:     return "crypto-system error";
                case Qap1.ErrSecurityClose:  return "server-initiated close due to security violation (too many attempts, excessive timeout etc.)";
                default:
                    return serverErrorCode < 0 ? "Rerror as provided by R_tryEval" : "unrecognized error";
            }
        }
        
        #endregion

    }
}