//-----------------------------------------------------------------------
// Original work Copyright (c) 2011, Oliver M. Haynold
// Modified work Copyright (c) 2013, Suraj Gupta
// All rights reserved.
//-----------------------------------------------------------------------

using System;
using Xunit;

namespace RserveCLI2.Test
{
    public class RConnectionTest
    {
        
        [Fact]
        public void WriteFile_TransferLargeFile_CanReadSameFileBack()
        {
            using ( var service = new Rservice() )
            {
                // Arrange
                const string fileName = "myfile.dat";
                byte[] originalFile = CreateAndTransferFile( service.RConnection , fileName , 100000 );

                // Act
                byte[] fileFromServer = ReadFile( service.RConnection , fileName );

                // Assert
                Assert.Equal( originalFile , fileFromServer );

                // Cleanup
                service.RConnection.RemoveFile( fileName );
            }
        }

        [Fact]
        public void WriteFile_TransferSmallFile_CanReadSameFileBack()
        {
            using ( var service = new Rservice() )
            {
                // Arrange
                const string fileName = "myfile.dat";
                byte[] originalFile = CreateAndTransferFile( service.RConnection , fileName , 100 );

                // Act
                byte[] fileFromServer = ReadFile( service.RConnection , fileName );

                // Assert
                Assert.Equal( originalFile , fileFromServer );

                // Cleanup
                service.RConnection.RemoveFile( fileName );
            }
        }

        #region Private Methods

        /// <summary>
        /// Create a file with random data and transfer a file to R
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="connection">The RConnection</param>
        /// <param name="length">The length of the file to be transferred.</param>
        private static byte[] CreateAndTransferFile( RConnection connection , string fileName , int length )
        {
            var data = new byte[ length ];
            var rnd = new Random( 2302 );
            rnd.NextBytes( data );
            using ( var os = new System.IO.MemoryStream( data ) )
            {
                connection.WriteFile( fileName , os );
            }
            return data;           
        }

        /// <summary>
        /// Reads a specified file using an RConnection
        /// </summary>
        private static byte[] ReadFile( RConnection connection , string fileName )
        {
            using ( var ist = connection.ReadFile( fileName ) )
            {
                var checkstream = new System.IO.MemoryStream();
                ist.CopyTo( checkstream );
                var checkbytes = checkstream.ToArray();
                return checkbytes;
            }
        }

        #endregion


    }
}
