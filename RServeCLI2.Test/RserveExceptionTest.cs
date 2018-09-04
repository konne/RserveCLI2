//-----------------------------------------------------------------------
// Original work Copyright (c) 2011, Oliver M. Haynold
// Modified work Copyright (c) 2013, Suraj Gupta
// All rights reserved.
//-----------------------------------------------------------------------

using System;
using System.Net;
#if BINARY_SERIALIZATION
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
#endif
using Xunit;

namespace RserveCLI2.Test
{
    public class RserveExceptionTest
    {
        [Fact]
        public static void Ctor_ConstructsRserveExceptionAndSavesParameters()
        {
            // Arrange
            const string message = "something went wrong";
            const int serverErrorCode = Qap1.ErrDataOverflow;
            var inner = new WebException( "BadFile" );

            // Act
            var exception1 = new RserveException();
            var exception2 = new RserveException( message );
            var exception3 = new RserveException( serverErrorCode );
            var exception4 = new RserveException( message , inner );
            
            // Assert
            Assert.Equal( "Exception of type 'RserveCLI2.RserveException' was thrown." , exception1.Message );
            Assert.Null( exception1.InnerException );
            Assert.Null( exception1.ServerErrorCode );
            
            Assert.Equal( message , exception2.Message );
            Assert.Null( exception2.InnerException );
            Assert.Null( exception2.ServerErrorCode );

            Assert.Contains( "Error received from server:" , exception3.Message );
            Assert.Null( exception3.InnerException );
            Assert.Equal( serverErrorCode , exception3.ServerErrorCode );
            
            Assert.Equal( message , exception4.Message );
            Assert.Equal( inner , exception4.InnerException );
            Assert.Null( exception4.ServerErrorCode );
            
        }
        
        [Fact]
        public static void Ctor_CalledWithUnrecognizedServerErrorCode_DoesNotThrow()
        {
            // Arrange, Act, Assert
            new RserveException( int.MaxValue );            
        }

        [Fact]
        public static void ToString_IncludesMessageAndServerErrorCodeWhenProvided()
        {
            // Arrange
            const string message = "something went wrong";
            const int serverErrorCode = Qap1.ErrDataOverflow;
            var inner = new WebException( "BadFile" );

            // Act
            var exception1 = new RserveException();
            var exception2 = new RserveException( message );
            var exception3 = new RserveException( serverErrorCode );
            var exception4 = new RserveException( message , inner );
            
            // Assert
            Assert.Contains( "Exception of type 'RserveCLI2.RserveException' was thrown." , exception1.ToString() , StringComparison.CurrentCulture );

            Assert.Contains( message , exception2.ToString() , StringComparison.CurrentCulture );

            Assert.Contains( "Error received from server:" , exception3.ToString() , StringComparison.CurrentCulture );
            Assert.Contains( serverErrorCode.ToString() , exception3.ToString() , StringComparison.CurrentCulture );

            Assert.Contains( message , exception4.ToString() , StringComparison.CurrentCulture );
            Assert.Contains( inner.Message , exception4.ToString() , StringComparison.CurrentCulture );
        }

#if BINARY_SERIALIZATION
        [Fact]
        public static void Serialize_SerializesException()
        {

            // Arrange
            const string message = "something went wrong";
            const int serverErrorCode = Qap1.ErrDataOverflow;
            var inner = new WebException( "BadFile" );
            
            var exception1 = new RserveException();
            var exception2 = new RserveException( message );
            var exception3 = new RserveException( serverErrorCode );
            var exception4 = new RserveException( message , inner );
            
            // Act & Assert
            SerializeToBinaryMemoryStream( exception1 ).Dispose();
            SerializeToBinaryMemoryStream( exception2 ).Dispose();
            SerializeToBinaryMemoryStream( exception3 ).Dispose();
            SerializeToBinaryMemoryStream( exception4 ).Dispose();
        }

        [Fact]
        public static void Deserialize_ReconstitutesExceptionMaintainingPropertyValues()
        {

            // Arrange
            const string message = "something went wrong";
            const int serverErrorCode = Qap1.ErrDataOverflow;
            var inner = new WebException( "BadFile" );

            var exception1 = new RserveException();
            var exception2 = new RserveException( message );
            var exception3 = new RserveException( serverErrorCode );
            var exception4 = new RserveException( message , inner );
            
            MemoryStream ms1 = SerializeToBinaryMemoryStream( exception1 );
            MemoryStream ms2 = SerializeToBinaryMemoryStream( exception2 );
            MemoryStream ms3 = SerializeToBinaryMemoryStream( exception3 );
            MemoryStream ms4 = SerializeToBinaryMemoryStream( exception4 );
            
            // Act
            var exception1Deserialized = DeserializeFromBinary<RserveException>( ms1 );
            var exception2Deserialized = DeserializeFromBinary<RserveException>( ms2 );
            var exception3Deserialized = DeserializeFromBinary<RserveException>( ms3 );
            var exception4Deserialized = DeserializeFromBinary<RserveException>( ms4 );
            
            // Assert
            Assert.Equal( "Exception of type 'RserveCLI2.RserveException' was thrown." , exception1.Message );
            Assert.Null( exception1Deserialized.InnerException );
            Assert.Null( exception1Deserialized.ServerErrorCode );

            Assert.Equal( message , exception2Deserialized.Message );
            Assert.Null( exception2Deserialized.InnerException );
            Assert.Null( exception2Deserialized.ServerErrorCode );

            Assert.Contains( "Error received from server:" , exception3Deserialized.Message );
            Assert.Null( exception3Deserialized.InnerException );
            Assert.Equal( serverErrorCode , exception3Deserialized.ServerErrorCode );
            
            Assert.Equal( message , exception4Deserialized.Message );
            Assert.Equal( inner.Message , exception4Deserialized.InnerException.Message );
            Assert.Null( exception4Deserialized.ServerErrorCode );
        }

        /// <summary>
        /// Serializes an object to a MemoryStream using the BinaryFormatter
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>A memory stream that containing the serialized object, at position 0.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="SerializationException">An error has occurred during serialization, such as if an object in the graph parameter is not marked as serializable.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        private static MemoryStream SerializeToBinaryMemoryStream( object value )
        {
            if ( value == null )
            {
                throw new ArgumentNullException();
            }

            var ms = new MemoryStream();
            try
            {
                var binFormat = new BinaryFormatter();
                binFormat.Serialize( ms , value );
                ms.Seek( 0 , 0 );
                return ms;
            }
            catch
            {
                ms.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Deserializes an object that has been serialized into a binary MemoryStream.
        /// </summary>
        /// <typeparam name="T">Type of object stored in MemoryStream.</typeparam>
        /// <param name="value">MemoryStream containing object to deserialize.</param>
        /// <returns>Deserialized object.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentException">Stream does not support reading.</exception>
        /// <exception cref="ArgumentException">Stream does not support seeking.</exception>
        /// <exception cref="ArgumentException">Cannot deserialize a zero-length stream.</exception>
        /// <exception cref="ArgumentException">Expected type of deserialized object does not match its actual type.</exception>
        /// <exception cref="SerializationException">
        /// The target type is a Decimal, but the value is out of range of the Decimal type.
        /// -or-
        /// The stream was not serialized in Binary
        /// -or-
        /// There was a issue deserializing the stream.
        /// </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        private static T DeserializeFromBinary<T>( Stream value )
        {
            if ( value == null )
            {
                throw new ArgumentNullException();
            }

            // independent of our calls to Length and Seek, BinaryFormatter.Deserialize
            // requires a stream that can read and seek.
            if ( !value.CanRead )
            {
                throw new ArgumentException( "Stream does not supporting read." );
            }
            if ( !value.CanSeek )
            {
                throw new ArgumentException( "Stream does not supporting seeking." );
            }
            if ( value.Length == 0 )
            {
                throw new ArgumentException( "Cannot deserialize a zero-length stream." );
            }
            value.Seek( 0 , SeekOrigin.Begin );

            var binFormat = new BinaryFormatter();
            try
            {
                return ( T )binFormat.Deserialize( value );
            }
            catch ( InvalidCastException )
            {
                throw new ArgumentException( "Expected type of deserialized object does not match its actual type." );
            }
        }
#endif
    }
}