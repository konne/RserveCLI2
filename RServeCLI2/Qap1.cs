//-----------------------------------------------------------------------
// Original work Copyright (c) 2011, Oliver M. Haynold
// Modified work Copyright (c) 2013, Suraj Gupta
// All rights reserved.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RserveCLI2
{

    /// <summary>
    /// An implementation of the QAP1 protocol used to communicate with Rserve
    /// </summary>
    internal class Qap1
    {

        #region Constants and Fields

        #region DT_ declarations

        /// <summary>
        /// Int data
        /// </summary>
        internal const byte DtInt = 1;

        /// <summary>
        /// String data
        /// </summary>
        internal const byte DtString = 4;

        /// <summary>
        /// Byte stream data
        /// </summary>
        internal const byte DtByteStream = 5;

        /// <summary>
        /// The data stream containts a Sexp
        /// </summary>
        internal const byte DtSexp = 10;

        /// <summary>
        /// Large data flag
        /// </summary>
        internal const byte DtLarge = 64;

        #endregion

        #region XT_ declarations

        /// <summary>
        /// The Sexp is NULL
        /// </summary>
        internal const byte XtNull = 0;

        /// <summary>
        /// S4 object Sexp
        /// </summary>
        internal const byte XtS4 = 7;

        /// <summary>
        /// Vector Sexp
        /// </summary>
        internal const byte XtVector = 16;

        /// <summary>
        /// Closure Sexp
        /// </summary>
        internal const byte XtClos = 18;

        /// <summary>
        /// Symbol name Sexp
        /// </summary>
        internal const byte XtSymName = 19;

        /// <summary>
        /// List without tags
        /// </summary>
        internal const byte XtListNoTag = 20;

        /// <summary>
        /// List with tags
        /// </summary>
        internal const byte XtListTag = 21;

        /// <summary>
        /// Lang without tags
        /// </summary>
        internal const byte XtLangNoTag = 22;

        /// <summary>
        /// Lang with tags
        /// </summary>
        internal const byte XtLangTag = 23;

        /// <summary>
        /// Vector expression
        /// </summary>
        internal const byte XtVectorExp = 26;

        /// <summary>
        /// Vector string
        /// </summary>
        internal const byte XtVectorStr = 27;

        /// <summary>
        /// Array of integers
        /// </summary>
        internal const byte XtArrayInt = 32;

        /// <summary>
        /// Array of doubles
        /// </summary>
        internal const byte XtArrayDouble = 33;

        /// <summary>
        /// Array of strings
        /// </summary>
        internal const byte XtArrayString = 34;

        /// <summary>
        /// Array of Bool UA
        /// </summary>
        internal const byte XtArrayBoolUa = 35;

        /// <summary>
        /// Array of Bool
        /// </summary>
        internal const byte XtArrayBool = 36;

        /// <summary>
        /// The Sexp contains raw data
        /// </summary>
        internal const byte XtRaw = 37;

        /// <summary>
        /// Array of complex
        /// </summary>
        internal const byte XtArrayComplex = 38;

        /// <summary>
        /// Unknown data type
        /// </summary>
        internal const byte XtUnknown = 48;

        /// <summary>
        /// The length of the Sexp is coded as a 56-bit integer, enlarging the header by 4 bytes
        /// </summary>
        internal const byte XtLarge = 64;

        /// <summary>
        /// Flag for the presence of attributes
        /// </summary>
        internal const byte XtHasAttr = 128;

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Qap1 class.
        /// </summary>
        /// <param name="socket">The socket through which we'll communicate with the server.</param>
        public Qap1( Socket socket )
        {
            if ( !BitConverter.IsLittleEndian )
            {
                throw new PlatformNotSupportedException( "As of now, this works only on little-endian machines. " +
                    "If you have access to CLI on a big-endian machine, feel free to check whether it works and remove this check." );
            }

            _socket = socket;
        }

        /// <summary>
        /// Encode a Sexp in Qap1 format
        /// </summary>
        /// <param name="s">The Sexp to be encoded</param>
        /// <returns>QAP4-encoded bit stream</returns>
        public List<byte> EncodeSexp( Sexp s )
        {
            var t = s.GetType();
            var res = new List<byte>();
            byte xt;
            SexpTaggedList attrs = null;
            if ( s.Attributes.Count > 0 )
            {
                attrs = new SexpTaggedList();
                foreach ( var a in s.Attributes )
                {
                    attrs.Add( a.Key , a.Value );
                }

                res.AddRange( EncodeSexp( attrs ) );
            }
            if ( t == typeof( SexpNull ) )
            {
                xt = XtNull;
            }
            else if ( t == typeof( SexpArrayDouble ) )
            {
                xt = XtArrayDouble;
                var v = ( ( SexpArrayDouble )s ).Value;
                foreach ( var t1 in v )
                {
                    res.AddRange( BitConverter.GetBytes( t1 ) );
                }
            }
            else if ( t == typeof( SexpArrayInt ) )
            {
                xt = XtArrayInt;
                var v = ( ( SexpArrayInt )s ).Value;
                foreach ( var t1 in v )
                {
                    res.AddRange( BitConverter.GetBytes( t1 ) );
                }
            }
            else if ( t == typeof( SexpArrayDate ) )
            {
                xt = XtArrayInt;
                var v = ( ( SexpArrayInt )s ).Value;
                foreach ( var t1 in v )
                {
                    res.AddRange( BitConverter.GetBytes( t1 ) );
                }
            }
            else if ( t == typeof( SexpArrayBool ) )
            {
                xt = XtArrayBool;
                var v = ( SexpArrayBool )s;

                // ReSharper disable RedundantCast
                res.AddRange( BitConverter.GetBytes( v.Count ) );

                // ReSharper restore RedundantCast
                // R logical is false if 0, true if 1, and NA if 2
                res.AddRange( v.Cast<SexpArrayBool>().Select( x => x.AsByte ) );

                // protocol requires us to pad with null
                while ( res.Count % 4 != 0 )
                {
                    res.Add( 0 );
                }
            }
            else if ( t == typeof( SexpTaggedList ) )
            {
                xt = XtListTag;
                var v = ( SexpTaggedList )s;
                foreach ( var a in v.AsSexpDictionary )
                {
                    res.AddRange( EncodeSexp( a.Value ) );
                    res.AddRange( EncodeSexp( new SexpSymname( a.Key ) ) );
                }
            }
            else if ( t == typeof( SexpList ) )
            {
                xt = XtVector;
                var v = ( ( SexpList )s ).Value;
                foreach ( var a in v )
                {
                    res.AddRange( EncodeSexp( a ) );
                }
            }
            else if ( t == typeof( SexpArrayString ) )
            {
                xt = XtArrayString;
                var v = ( ( SexpArrayString )s ).Value;
                foreach ( var a in v )
                {
                    // Rserve represents NA strings using 0xff (255).  
                    if ( a == null )
                    {
                        res.Add( 255 );
                    }
                    else
                    {
                        var b = Encoding.UTF8.GetBytes( a );
                        
                        // If 0xff occurs in the beginning of a string it should be doubled to avoid misrepresentation. 
                        if ( ( b.Length > 0 ) && ( b[ 0 ] == 255 ) )
                        {
                            res.Add( 255 );
                        }

                        res.AddRange( b );
                    }

                    res.Add( 0 );
                }
            }
            else if ( t == typeof( SexpSymname ) )
            {
                xt = XtSymName;
                var v = ( ( SexpSymname )s ).Value;
                var b = Encoding.UTF8.GetBytes( v );
                res.AddRange( b );
                res.Add( 0 );
            }
            else
            {
                throw new ArgumentException( "Unknown Sexp type " + t.GetType().Name );
            }

            if ( attrs != null )
            {
                xt |= XtHasAttr;
            }

            // get payload length
            long len = res.LongCount();
            byte[] lenBytes = BitConverter.GetBytes( len );

            // populate header (first four bytes)
            IEnumerable<byte> header = lenBytes.Take( 3 );

            // a large dataset is > 16MB, it requires the XtLarge flag and an extra 4 bytes in the header to esablish correct payload size
            bool isLargeData = len > 0xfffff0;
            if ( isLargeData )
            {
                xt |= XtLarge;
                header = lenBytes.Take( 7 );
            }

            // insert header
            res.InsertRange( 0 , header );
            res.Insert( 0 , xt );
            
            return res;
        }

        /// <summary>
        /// Decode a Qap1-encoded Sexp
        /// </summary>
        /// <param name="data">The byte stream in which the Sexp is encoded</param>
        /// <param name="start">At which index of data does the Sexp begin?</param>
        /// <returns>The decoded Sexp.</returns>
        public Sexp DecodeSexp( byte[] data , ref int start )
        {
            byte xt = data[ start + 0 ];

            // ReSharper disable RedundantCast
            var length = data[ start + 1 ] + ( ( ( int )data[ start + 2 ] ) << 8 ) + ( ( ( int )data[ start + 3 ] ) << 16 );

            // ReSharper restore RedundantCast
            start += 4;
            SexpTaggedList attrs = null;
            if ( ( xt & XtHasAttr ) == XtHasAttr )
            {
                xt -= XtHasAttr;
                int oldstart = start;
                attrs = ( SexpTaggedList )DecodeSexp( data , ref start );
                length -= start - oldstart;
            }

            int end = start + length;
            Sexp result;
            if ( xt == XtNull && length == 0 )
            {
                result = new SexpNull();
            }
            else
            {
                switch ( xt )
                {
                    case XtSymName:
                        {
                            var res = Encoding.UTF8.GetString( data , start , length );
                            result = new SexpSymname( res.Split( '\x00' )[ 0 ] );
                        }
                        break;
                    case XtArrayInt:
                        {
                            var res = new int[ length / 4 ];
                            for ( int i = 0 ; i < length ; i += 4 )
                            {
                                res[ i / 4 ] = BitConverter.ToInt32( data , start + i );
                            }

                            // is date or just an integer?
                            if ( ( attrs != null ) && ( attrs.ContainsKey( "class" ) && attrs[ "class" ].AsStrings.Contains( "Date" ) ) )
                            {
                                result = new SexpArrayDate( res );
                            }
                            else
                            {
                                result = new SexpArrayInt( res );
                            }                            
                        }
                        break;
                    case XtArrayBool:
                        {
                            // This doesn't match the description on the Web page and doesn't make much sense, but that's what the server seems to do
                            if ( length < 4 )
                            {
                                throw new WebException( "Bool array doesn't seem to contain a data length field." );
                            }

                            var datalength = BitConverter.ToInt32( data , start );
                            if ( datalength > length - 4 )
                            {
                                throw new WebException( "Transmitted data field too short for number of entries." );
                            }

                            start += 4;
                            var res = new bool?[ datalength ];
                            for ( int i = 0 ; i < datalength ; i++ )
                            {
                                // R logical is false if 0, true if 1, and NA if 2
                                switch ( data[ start + i ] )
                                {
                                    case 0:
                                        res[ i ] = false;
                                        break;
                                    case 1:
                                        res[ i ] = true;
                                        break;
                                    case 2:
                                        res[ i ] = null;
                                        break;
                                    default:
                                        throw new NotSupportedException( "I cannot convert R bool" + data[ start + i ] );
                                }                              
                            }

                            result = new SexpArrayBool( res );
                        }
                        break;
                    case XtArrayDouble:
                        {
                            var res = new double[ length / 8 ];
                            for ( int i = 0 ; i < length ; i += 8 )
                            {
                                res[ i / 8 ] = BitConverter.ToDouble( data , start + i );
                            }

                            // is date or just a double?
                            if ( ( attrs != null ) && ( attrs.ContainsKey( "class" ) && attrs[ "class" ].AsStrings.Contains( "Date" ) ) )
                            {
                                result = new SexpArrayDate( res.Select( Convert.ToInt32 ) );
                            }
                            else
                            {
                                result = new SexpArrayDouble( res );
                            }
                        }
                        break;
                    case XtArrayString:
                        {
                            var res = new List<string>();
                            int i = 0;
                            for ( int j = 0 ; j < length ; j++ )
                            {
                                if ( data[ start + j ] != 0 )
                                {
                                    continue;
                                }

                                if ( j == i + 1 && data[ start + i ] == 255 )
                                {
                                    res.Add( null );
                                }
                                else
                                {
                                    if ( data[ start + i ] == 255 )
                                    {
                                        i++;
                                    }

                                    res.Add( Encoding.UTF8.GetString( data , start + i , j - i ) );
                                }

                                i = j + 1;
                            }

                            result = new SexpArrayString( res );
                        }
                        break;
                    case XtListNoTag:
                    case XtLangNoTag:
                    case XtVector:
                        result = new SexpList();
                        while ( start < end )
                        {
                            result.Add( DecodeSexp( data , ref start ) );
                        }
                        break;
                    case XtLangTag:
                    case XtListTag:
                        result = new SexpTaggedList();
                        while ( start < end )
                        {
                            var val = DecodeSexp( data , ref start );
                            var key = DecodeSexp( data , ref start );
                            result.Add( key.IsNull ? String.Empty : key.AsString , val );
                        }

                        break;
                    default:
                        {
                            var d = new byte[ length ];
                            Array.Copy( data , start , d , 0 , length );
                            result = new SexpQap1Raw( xt , d );
                        }
                        break;
                }
            }

            if ( start > end )
            {
                throw new WebException( "More data consumed than provided." );
            }

            start = end;
            if ( attrs != null )
            {
                foreach ( var a in attrs.AsSexpDictionary )
                {
                    result.Attributes.Add( a.Key , a.Value );
                }
            }

            return result;
        }

        /// <summary>
        /// Send a command and read the stream it returns (used for reading files)
        /// </summary>
        /// <param name="cmd">Command to be sent to the server</param>
        /// <param name="data">Arguments for the command</param>
        /// <returns>The data read</returns>
        public byte[] CommandReadStream( int cmd , IList<object> data )
        {
            int toConsume = SubmitCommand( cmd , data );
            var res = new byte[ toConsume ];
            var received = 0;
            while ( received < toConsume )
            {
                received += _socket.Receive( res , received , toConsume - received , SocketFlags.None );
            }

            if ( received != toConsume )
            {
                throw new WebException( "Expected " + toConsume + " bytes of data, but received " + received + "." );
            }

            return res;
        }

        /// <summary>
        /// Send a command and read the result
        /// </summary>
        /// <param name="cmd">Command to be sent to the server</param>
        /// <param name="data">Arguments for the command</param>
        /// <returns>The result, parsed into appropriate objects (string or Sexp)</returns>
        public List<object> Command( int cmd , IList<object> data )
        {
            int toConsume = SubmitCommand( cmd , data );
            var res = new List<object>();
            while ( toConsume > 0 )
            {
                var dhbuf = new byte[ 4 ];
                if ( _socket.Receive( dhbuf ) != 4 )
                {
                    throw new WebException( "Didn't receive a header." );
                }

                byte typ = dhbuf[ 0 ];

                // ReSharper disable RedundantCast
                int dlength = ( ( int )dhbuf[ 1 ] ) + ( ( ( int )dhbuf[ 2 ] ) << 8 ) + ( ( ( int )dhbuf[ 3 ] ) << 16 );
                int receivedTotal = 0;

                // ReSharper restore RedundantCast
                var dvbuf = new byte[ dlength ];
                while ( receivedTotal < dlength )
                {
                    var buf = new byte[ Math.Min( dlength - receivedTotal , 1014 * 1024 ) ];
                    var received = _socket.Receive( buf );
                    if ( received > 0 )
                    {
                        Array.Copy( buf , 0 , dvbuf , receivedTotal , received );
                        receivedTotal += received;
                    }
                    else
                    {
                        throw new WebException( "Expected " + dvbuf.Length + " bytes of data, but received " + receivedTotal + "." );
                    }
                }


                switch ( typ )
                {
                    case DtString:
                        {
                            var a = new List<byte>();
                            a.AddRange( dvbuf );
                            while ( a.Last() == 0 )
                            {
                                a.RemoveAt( a.Count - 1 );
                            }

                            res.Add( Encoding.UTF8.GetString( a.ToArray() ) );
                        }

                        break;
                    case DtSexp:
                        {
                            int start = 0;
                            res.Add( DecodeSexp( dvbuf , ref start ) );
                        }

                        break;
                    default:
                        throw new WebException( "Unknown data type." );
                }

                toConsume -= 4 + dlength;
            }

            return res;
        }

        #endregion

        #region Private Members

        /// <summary>
        /// The socket used to communicate with Rserve
        /// </summary>
        private readonly Socket _socket;

        /// <summary>
        /// Submit a command to Rserve
        /// </summary>
        /// <param name="cmd">The command to be submitted to the server</param>
        /// <param name="data">The arguments for the command</param>
        /// <returns>Length of the response in bytes</returns>
        private int SubmitCommand( int cmd , IList<object> data )
        {
            // Build command
            var sbuf = new List<byte>();
            foreach ( var a in data )
            {
                var argbuf = new List<byte>();
                byte dt;
                if ( a is string )
                {
                    argbuf.AddRange( Encoding.UTF8.GetBytes( a as string ) );
                    argbuf.Add( 0 ); // string must be null terminated
                    
                    // strings must be padded with zeros so length of the content is divisible by 4
                    while ( argbuf.Count % 4 != 0 )
                    {
                        argbuf.Add( 0 );
                    }

                    dt = DtString;
                }
                else if ( a is Sexp )
                {
                    argbuf.AddRange( EncodeSexp( a as Sexp ) );
                    dt = DtSexp;
                }
                else if ( a is byte[] )
                {
                    argbuf.AddRange( ( byte[] )a );
                    dt = DtByteStream;
                }
                else if ( a is int )
                {
                    argbuf.AddRange( BitConverter.GetBytes( ( int )a ) );
                    dt = DtInt;
                }
                else
                {
                    throw new ArgumentException( "Invalid argument type." );
                }

                // get payload length
                long len = argbuf.LongCount();
                byte[] lenBytes = BitConverter.GetBytes( len );

                // populate header (first four bytes)
                IEnumerable<byte> header = lenBytes.Take( 3 );

                // a large dataset is > 16MB, it requires the DtLarge flag and an extra 4 bytes in the header to esablish correct payload size
                bool isLargeData = len > 0xfffff0;
                if ( isLargeData )
                {
                    dt |= DtLarge;
                    header = lenBytes.Take( 7 );
                }

                // insert header
                argbuf.InsertRange( 0 , header );
                argbuf.Insert( 0 , dt );

                sbuf.AddRange( argbuf );
            }

            // header structure:
            // [0]  (int) command - specifies the request or response type
            // [4]  (int) length of the message (bits 0-31) - specifies the number of bytes belonging to this message (excluding the header)
            // [8]  (int) offset of the data part - specifies the offset of the data part, where 0 means directly after the header (which is normally the case)
            // [12] (int) length of the message (bits 32-63) - high bits of the length (must be 0 if the packet size is smaller than 4GB)
            long mlen = sbuf.LongCount();
            byte[] mlenBytes = BitConverter.GetBytes( mlen );
            sbuf.InsertRange( 0 , BitConverter.GetBytes( cmd ) );
            sbuf.InsertRange( 4 , mlenBytes.Take( 4 ) );
            sbuf.InsertRange( 8 , new byte[ 4 ] );
            sbuf.InsertRange( 12 , mlenBytes.Skip( 4 ) );

            // Execute Command
            _socket.Send( sbuf.ToArray() );

            // Read Response
            var hdrbuf = new byte[ 16 ];
            if ( _socket.Receive( hdrbuf ) != 16 )
            {
                throw new WebException( "Didn't receive a header." );
            }

            int cmdResult = BitConverter.ToInt32( hdrbuf , 0 );
            if ( ( cmdResult & 15 ) != 1 )
            {
                throw new WebException( "R threw an error." );
            }

            var length = BitConverter.ToUInt32( hdrbuf , 4 );
            var offset = BitConverter.ToUInt32( hdrbuf , 8 );
            var largeLength = BitConverter.ToUInt32( hdrbuf , 12 );
            if ( offset != 0 || largeLength != 0 )
            {
                throw new NotSupportedException( "Large data not supported at this time." );
            }

            var toConsume = ( int )length;
            return toConsume;
        }

        #endregion

    }
}
