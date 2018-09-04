//-----------------------------------------------------------------------
// Copyright (c) 2011, Elmar Langholz
// Copyright (c) 2011, Oliver M. Haynold
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Text;

namespace RserveCLI2
{
    
    /// <summary>
    /// DES Encryption algorithm.
    /// Based on FreeBSD's libcrypt:
    ///    secure/lib/libcrypt/crypt-des.c
    /// </summary>
    internal class DES
    {
        #region Constants and Fields

        private readonly byte[] _ip = new byte[]
            {
                58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 
                40, 32, 24, 16, 8, 57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3, 61, 53, 45, 37, 29, 21, 13
                , 5, 63, 55, 47, 39, 31, 23, 15, 7
            };

        private readonly byte[] ascii64 = new[]
            {
                (byte)'.', (byte)'/', (byte)'0', (byte)'1', (byte)'2', (byte)'3', (byte)'4', (byte)'5', (byte)'6', 
                (byte)'7', (byte)'8', (byte)'9', (byte)'A', (byte)'B', (byte)'C', (byte)'D', (byte)'E', (byte)'F', 
                (byte)'G', (byte)'H', (byte)'I', (byte)'J', (byte)'K', (byte)'L', (byte)'M', (byte)'N', (byte)'O', 
                (byte)'P', (byte)'Q', (byte)'R', (byte)'S', (byte)'T', (byte)'U', (byte)'V', (byte)'W', (byte)'X', 
                (byte)'Y', (byte)'Z', (byte)'a', (byte)'b', (byte)'c', (byte)'d', (byte)'e', (byte)'f', (byte)'g', 
                (byte)'h', (byte)'i', (byte)'j', (byte)'k', (byte)'l', (byte)'m', (byte)'n', (byte)'o', (byte)'p', 
                (byte)'q', (byte)'r', (byte)'s', (byte)'t', (byte)'u', (byte)'v', (byte)'w', (byte)'x', (byte)'y', 
                (byte)'z'
            };

        private readonly uint[] bits32 = new uint[]
            {
                0x80000000, 0x40000000, 0x20000000, 0x10000000, 0x08000000, 0x04000000, 0x02000000, 0x01000000, 0x00800000
                , 0x00400000, 0x00200000, 0x00100000, 0x00080000, 0x00040000, 0x00020000, 0x00010000, 0x00008000, 
                0x00004000, 0x00002000, 0x00001000, 0x00000800, 0x00000400, 0x00000200, 0x00000100, 0x00000080, 0x00000040
                , 0x00000020, 0x00000010, 0x00000008, 0x00000004, 0x00000002, 0x00000001
            };

        private readonly byte[] bits8 = new byte[] { 0x80 , 0x40 , 0x20 , 0x10 , 0x08 , 0x04 , 0x02 , 0x01 };

        private readonly uint[ , ] compMaskl = new uint[ 8 , 128 ];

        private readonly uint[ , ] compMaskr = new uint[ 8 , 128 ];

        private readonly byte[] compPerm = new byte[]
            {
                14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47
                , 55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32
            };

        private readonly uint[] deKeysl = new uint[ 16 ];

        private readonly uint[] deKeysr = new uint[ 16 ];

        private readonly uint[] enKeysl = new uint[ 16 ];

        private readonly uint[] enKeysr = new uint[ 16 ];

        private readonly byte[] finalPerm = new byte[ 64 ];

        private readonly uint[ , ] fpMaskl = new uint[ 8 , 256 ];

        private readonly uint[ , ] fpMaskr = new uint[ 8 , 256 ];

        private readonly byte[] initPerm = new byte[ 64 ];

        private readonly byte[] invCompPerm = new byte[ 56 ];

        private readonly byte[] invKeyPerm = new byte[ 64 ];

        private readonly uint[ , ] ipMaskl = new uint[ 8 , 256 ];

        private readonly uint[ , ] ipMaskr = new uint[ 8 , 256 ];

        private readonly byte[] keyPerm = new byte[]
            {
                57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36
                , 63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 
                4
            };

        private readonly uint[ , ] keyPermMaskl = new uint[ 8 , 128 ];

        private readonly uint[ , ] keyPermMaskr = new uint[ 8 , 128 ];

        private readonly byte[] keyShifts = new byte[] { 1 , 1 , 2 , 2 , 2 , 2 , 2 , 2 , 1 , 2 , 2 , 2 , 2 , 2 , 2 , 1 };

        private readonly byte[ , ] mSbox = new byte[ 4 , 4096 ];

        private readonly byte[] pbox = new byte[]
            {
                16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31, 10, 2, 8, 24, 14, 32, 27, 3, 9, 19, 13, 30, 6, 22
                , 11, 4, 25
            };

        private readonly uint[ , ] psbox = new uint[ 4 , 256 ];

        private readonly byte[ , ] sbox = new byte[ 8 , 64 ]
            {
                {
                    14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7, 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 
                    3, 8, 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0, 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10
                    , 0, 6, 13
                }, 
                {
                    15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10, 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 
                    11, 5, 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15, 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 
                    0, 5, 14, 9
                }, 
                {
                    10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8, 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 
                    15, 1, 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7, 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 
                    11, 5, 2, 12
                }, 
                {
                    7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15, 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 
                    14, 9, 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4, 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 
                    12, 7, 2, 14
                }, 
                {
                    2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9, 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 
                    8, 6, 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14, 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 
                    10, 4, 5, 3
                }, 
                {
                    12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11, 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 
                    3, 8, 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6, 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6
                    , 0, 8, 13
                }, 
                {
                    4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1, 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 
                    8, 6, 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2, 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14
                    , 2, 3, 12
                }, 
                {
                    13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7, 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 
                    9, 2, 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8, 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3
                    , 5, 6, 11
                }
            };

        private readonly byte[ , ] uSbox = new byte[ 8 , 64 ];

        private readonly byte[] unPbox = new byte[ 32 ];

        private int desInitialised;

        private uint oldRawkey0;

        private uint oldRawkey1;

        private uint oldSalt;

        private uint saltbits;

        #endregion

        #region Public Methods

        /// <summary>
        /// This method encrypt the given string with the salt indicated, using DES Algorithm
        /// </summary>
        /// <param name="k">
        /// The string to be encrypted.
        /// </param>
        /// <param name="s">
        /// The salt.
        /// </param>
        /// <returns>
        /// The encrypted string.
        /// </returns>
        public string Encrypt( string k , string s )
        {
            var desHash = string.Empty;
            uint count , salt , r0 = 0 , r1 = 0;
            var it = 0;
            var q = new byte[ 8 ];
            var key = ( new ASCIIEncoding() ).GetBytes( k );
            var setting = ( new ASCIIEncoding() ).GetBytes( s );
            if ( !Convert.ToBoolean( desInitialised ) )
            {
                DESInit();
            }

            for ( var i = 0 ; i < 8 && i < key.Length ; i++ )
            {
                q[ i ] = ( byte )( key[ it = i ] << 1 );
            }

            it++;
            for ( var i = key.Length ; i < 8 ; i++ )
            {
                q[ i ] = 0;
            }

            if ( Convert.ToBoolean( DESSetkey( q ) ) )
            {
                return null;
            }

            if ( setting[ 0 ] == '_' )
            {
                count = 0;
                for ( int i = 1 ; i < 5 ; i++ )
                {
                    count |= ( uint )( AsciiToBin( ( char )setting[ i ] ) << ( ( i - 1 ) * 6 ) );
                }

                salt = 0;
                for ( int i = 5 ; i < 9 ; i++ )
                {
                    salt |= ( uint )( AsciiToBin( ( char )setting[ i ] ) << ( ( i - 5 ) * 6 ) );
                }

                for ( int i = it ; i < key.Length ; )
                {
                    if ( Convert.ToBoolean( DESCipher( q , ref q , 0 , 1 ) ) )
                    {
                        return null;
                    }

                    for ( int j = 0 ; j < 8 && i < key.Length ; )
                    {
                        q[ j++ ] ^= ( byte )( key[ i++ ] << 1 );
                    }

                    if ( Convert.ToBoolean( DESSetkey( q ) ) )
                    {
                        return null;
                    }
                }

                for ( int i = 0 ; i < 9 ; i++ )
                {
                    desHash += ( char )setting[ i ];
                }
            }
            else
            {
                count = 25;
                salt = ( uint )( ( AsciiToBin( ( char )setting[ 1 ] ) << 6 ) | AsciiToBin( ( char )setting[ 0 ] ) );
                desHash += ( char )setting[ 0 ];
                desHash += ( char )( Convert.ToBoolean( setting[ 1 ] ) ? setting[ 1 ] : setting[ 0 ] );
            }

            SetupSalt( salt );
            if ( Convert.ToBoolean( DoDES( 0 , 0 , ref r0 , ref r1 , ( int )count ) ) )
            {
                return null;
            }

            uint l = r0 >> 8;
            desHash += ( char )ascii64[ ( l >> 18 ) & 0x3f ];
            desHash += ( char )ascii64[ ( l >> 12 ) & 0x3f ];
            desHash += ( char )ascii64[ ( l >> 6 ) & 0x3f ];
            desHash += ( char )ascii64[ l & 0x3f ];
            l = ( r0 << 16 ) | ( ( r1 >> 16 ) & 0xffff );
            desHash += ( char )ascii64[ ( l >> 18 ) & 0x3f ];
            desHash += ( char )ascii64[ ( l >> 12 ) & 0x3f ];
            desHash += ( char )ascii64[ ( l >> 6 ) & 0x3f ];
            desHash += ( char )ascii64[ l & 0x3f ];
            l = r1 << 2;
            desHash += ( char )ascii64[ ( l >> 12 ) & 0x3f ];
            desHash += ( char )ascii64[ ( l >> 6 ) & 0x3f ];
            desHash += ( char )ascii64[ l & 0x3f ];
            return desHash;
        }

        #endregion

        #region Methods

        private static int AsciiToBin( char ch )
        {
            if ( ch > 'z' )
            {
                return 0;
            }

            if ( ch >= 'a' )
            {
                return ch - 'a' + 38;
            }

            if ( ch > 'Z' )
            {
                return 0;
            }

            if ( ch >= 'A' )
            {
                return ch - 'A' + 12;
            }

            if ( ch > '9' )
            {
                return 0;
            }

            if ( ch >= '.' )
            {
                return ch - '.';
            }

            return 0;
        }

        private int DESCipher( byte[] @in , ref byte[] @out , ulong salt , int count )
        {
            uint lOut = 0 , rOut = 0;
            if ( !Convert.ToBoolean( desInitialised ) )
            {
                DESInit();
            }

            SetupSalt( ( uint )salt );
            var rawl = ( uint )( @in[ 0 ] << 24 | @in[ 1 ] << 16 | @in[ 2 ] << 8 | @in[ 3 ] );
            var rawr = ( uint )( @in[ 4 ] << 24 | @in[ 5 ] << 16 | @in[ 6 ] << 8 | @in[ 7 ] );
            int retval = DoDES( rawl , rawr , ref lOut , ref rOut , count );
            @out[ 3 ] = ( byte )( lOut >> 24 );
            @out[ 2 ] = ( byte )( lOut >> 16 );
            @out[ 1 ] = ( byte )( lOut >> 8 );
            @out[ 0 ] = ( byte )lOut;
            @out[ 7 ] = ( byte )( rOut >> 24 );
            @out[ 6 ] = ( byte )( rOut >> 16 );
            @out[ 5 ] = ( byte )( rOut >> 8 );
            @out[ 4 ] = ( byte )rOut;
            return retval;
        }

        private void DESInit()
        {
            int b;
            oldRawkey0 = oldRawkey1 = 0;
            saltbits = 0;
            oldSalt = 0;
            for ( int i = 0 ; i < 8 ; i++ )
            {
                for ( int j = 0 ; j < 64 ; j++ )
                {
                    b = ( j & 0x20 ) | ( ( j & 1 ) << 4 ) | ( ( j >> 1 ) & 0xf );
                    uSbox[ i , j ] = sbox[ i , b ];
                }
            }

            for ( b = 0 ; b < 4 ; b++ )
            {
                for ( int i = 0 ; i < 64 ; i++ )
                {
                    for ( int j = 0 ; j < 64 ; j++ )
                    {
                        mSbox[ b , ( i << 6 ) | j ] =
                            ( byte )( ( uSbox[ b << 1 , i ] << 4 ) | uSbox[ ( b << 1 ) + 1 , j ] );
                    }
                }
            }

            for ( int i = 0 ; i < 64 ; i++ )
            {
                finalPerm[ i ] = ( byte )( _ip[ i ] - 1 );
                initPerm[ finalPerm[ i ] ] = ( byte )i;
                invKeyPerm[ i ] = 255;
            }

            for ( int i = 0 ; i < 56 ; i++ )
            {
                invKeyPerm[ keyPerm[ i ] - 1 ] = ( byte )i;
                invCompPerm[ i ] = 255;
            }

            for ( int i = 0 ; i < 48 ; i++ )
            {
                invCompPerm[ compPerm[ i ] - 1 ] = ( byte )i;
            }

            for ( int k = 0 ; k < 8 ; k++ )
            {
                int inbit;
                int obit;
                for ( int i = 0 ; i < 256 ; i++ )
                {
                    ipMaskl[ k , i ] = 0;
                    ipMaskr[ k , i ] = 0;
                    fpMaskl[ k , i ] = 0;
                    fpMaskr[ k , i ] = 0;
                    for ( int j = 0 ; j < 8 ; j++ )
                    {
                        inbit = 8 * k + j;
                        if ( !Convert.ToBoolean( i & bits8[ j ] ) )
                        {
                            continue;
                        }
                        if ( ( obit = initPerm[ inbit ] ) < 32 )
                        {
                            ipMaskl[ k , i ] |= bits32[ obit ];
                        }
                        else
                        {
                            ipMaskr[ k , i ] |= bits32[ obit - 32 ];
                        }

                        if ( ( obit = finalPerm[ inbit ] ) < 32 )
                        {
                            fpMaskl[ k , i ] |= bits32[ obit ];
                        }
                        else
                        {
                            fpMaskr[ k , i ] |= bits32[ obit - 32 ];
                        }
                    }
                }

                for ( int i = 0 ; i < 128 ; i++ )
                {
                    keyPermMaskl[ k , i ] = 0;
                    keyPermMaskr[ k , i ] = 0;
                    for ( int j = 0 ; j < 7 ; j++ )
                    {
                        inbit = 8 * k + j;
                        if ( !Convert.ToBoolean( i & bits8[ j + 1 ] ) )
                        {
                            continue;
                        }
                        if ( ( obit = invKeyPerm[ inbit ] ) == 255 )
                        {
                            continue;
                        }

                        if ( obit < 28 )
                        {
                            keyPermMaskl[ k , i ] |= bits32[ 4 + obit ];
                        }
                        else
                        {
                            keyPermMaskr[ k , i ] |= bits32[ 4 + obit - 28 ];
                        }
                    }

                    compMaskl[ k , i ] = 0;
                    compMaskr[ k , i ] = 0;
                    for ( int j = 0 ; j < 7 ; j++ )
                    {
                        inbit = 7 * k + j;
                        if ( !Convert.ToBoolean( i & bits8[ j + 1 ] ) )
                        {
                            continue;
                        }
                        if ( ( obit = invCompPerm[ inbit ] ) == 255 )
                        {
                            continue;
                        }

                        if ( obit < 24 )
                        {
                            compMaskl[ k , i ] |= bits32[ 8 + obit ];
                        }
                        else
                        {
                            compMaskr[ k , i ] |= bits32[ 8 + obit - 24 ];
                        }
                    }
                }
            }

            for ( int i = 0 ; i < 32 ; i++ )
            {
                unPbox[ pbox[ i ] - 1 ] = ( byte )i;
            }

            for ( b = 0 ; b < 4 ; b++ )
            {
                for ( int i = 0 ; i < 256 ; i++ )
                {
                    psbox[ b , i ] = 0;
                    for ( int j = 0 ; j < 8 ; j++ )
                    {
                        if ( Convert.ToBoolean( i & bits8[ j ] ) )
                        {
                            psbox[ b , i ] |= bits32[ unPbox[ 8 * b + j ] ];
                        }
                    }
                }
            }

            desInitialised = 1;
        }

        private int DESSetkey( byte[] k )
        {
            if ( !Convert.ToBoolean( desInitialised ) )
            {
                DESInit();
            }

            var rawkey0 = ( uint )( k[ 0 ] << 24 | k[ 1 ] << 16 | k[ 2 ] << 8 | k[ 3 ] );
            var rawkey1 = ( uint )( k[ 4 ] << 24 | k[ 5 ] << 16 | k[ 6 ] << 8 | k[ 7 ] );
            if ( Convert.ToBoolean( rawkey0 | rawkey1 ) && rawkey0 == oldRawkey0 && rawkey1 == oldRawkey1 )
            {
                return 0;
            }

            oldRawkey0 = rawkey0;
            oldRawkey1 = rawkey1;
            uint k0 = keyPermMaskl[ 0 , rawkey0 >> 25 ] | keyPermMaskl[ 1 , ( rawkey0 >> 17 ) & 0x7f ] |
                      keyPermMaskl[ 2 , ( rawkey0 >> 9 ) & 0x7f ] | keyPermMaskl[ 3 , ( rawkey0 >> 1 ) & 0x7f ] |
                      keyPermMaskl[ 4 , rawkey1 >> 25 ] | keyPermMaskl[ 5 , ( rawkey1 >> 17 ) & 0x7f ] |
                      keyPermMaskl[ 6 , ( rawkey1 >> 9 ) & 0x7f ] | keyPermMaskl[ 7 , ( rawkey1 >> 1 ) & 0x7f ];
            uint k1 = keyPermMaskr[ 0 , rawkey0 >> 25 ] | keyPermMaskr[ 1 , ( rawkey0 >> 17 ) & 0x7f ] |
                      keyPermMaskr[ 2 , ( rawkey0 >> 9 ) & 0x7f ] | keyPermMaskr[ 3 , ( rawkey0 >> 1 ) & 0x7f ] |
                      keyPermMaskr[ 4 , rawkey1 >> 25 ] | keyPermMaskr[ 5 , ( rawkey1 >> 17 ) & 0x7f ] |
                      keyPermMaskr[ 6 , ( rawkey1 >> 9 ) & 0x7f ] | keyPermMaskr[ 7 , ( rawkey1 >> 1 ) & 0x7f ];
            int shifts = 0;
            for ( int round = 0 ; round < 16 ; round++ )
            {
                shifts += keyShifts[ round ];
                uint t0 = ( k0 << shifts ) | ( k0 >> ( 28 - shifts ) );
                uint t1 = ( k1 << shifts ) | ( k1 >> ( 28 - shifts ) );
                deKeysl[ 15 - round ] =
                    enKeysl[ round ] =
                    compMaskl[ 0 , ( t0 >> 21 ) & 0x7f ] | compMaskl[ 1 , ( t0 >> 14 ) & 0x7f ] |
                    compMaskl[ 2 , ( t0 >> 7 ) & 0x7f ] | compMaskl[ 3 , t0 & 0x7f ] |
                    compMaskl[ 4 , ( t1 >> 21 ) & 0x7f ] | compMaskl[ 5 , ( t1 >> 14 ) & 0x7f ] |
                    compMaskl[ 6 , ( t1 >> 7 ) & 0x7f ] | compMaskl[ 7 , t1 & 0x7f ];
                deKeysr[ 15 - round ] =
                    enKeysr[ round ] =
                    compMaskr[ 0 , ( t0 >> 21 ) & 0x7f ] | compMaskr[ 1 , ( t0 >> 14 ) & 0x7f ] |
                    compMaskr[ 2 , ( t0 >> 7 ) & 0x7f ] | compMaskr[ 3 , t0 & 0x7f ] |
                    compMaskr[ 4 , ( t1 >> 21 ) & 0x7f ] | compMaskr[ 5 , ( t1 >> 14 ) & 0x7f ] |
                    compMaskr[ 6 , ( t1 >> 7 ) & 0x7f ] | compMaskr[ 7 , t1 & 0x7f ];
            }

            return 0;
        }

        private int DoDES( uint lIn , uint rIn , ref uint lOut , ref uint rOut , int count )
        {
            uint f = 0;
            uint[] kl1 , kr1;
            if ( count == 0 )
            {
                return 1;
            }
            if ( count > 0 )
            {
                kl1 = enKeysl;
                kr1 = enKeysr;
            }
            else
            {
                count = -count;
                kl1 = deKeysl;
                kr1 = deKeysr;
            }

            uint l = ipMaskl[ 0 , lIn >> 24 ] | ipMaskl[ 1 , ( lIn >> 16 ) & 0xff ] |
                     ipMaskl[ 2 , ( lIn >> 8 ) & 0xff ] | ipMaskl[ 3 , lIn & 0xff ] | ipMaskl[ 4 , rIn >> 24 ] |
                     ipMaskl[ 5 , ( rIn >> 16 ) & 0xff ] | ipMaskl[ 6 , ( rIn >> 8 ) & 0xff ] |
                     ipMaskl[ 7 , rIn & 0xff ];
            uint r = ipMaskr[ 0 , lIn >> 24 ] | ipMaskr[ 1 , ( lIn >> 16 ) & 0xff ] |
                     ipMaskr[ 2 , ( lIn >> 8 ) & 0xff ] | ipMaskr[ 3 , lIn & 0xff ] | ipMaskr[ 4 , rIn >> 24 ] |
                     ipMaskr[ 5 , ( rIn >> 16 ) & 0xff ] | ipMaskr[ 6 , ( rIn >> 8 ) & 0xff ] |
                     ipMaskr[ 7 , rIn & 0xff ];
            while ( Convert.ToBoolean( count-- ) )
            {
                uint[] kl = kl1;
                uint[] kr = kr1;
                uint j = 0;
                int round = 16;
                while ( Convert.ToBoolean( round-- ) )
                {
                    uint r48L = ( ( r & 0x00000001 ) << 23 ) | ( ( r & 0xf8000000 ) >> 9 ) | ( ( r & 0x1f800000 ) >> 11 ) |
                                ( ( r & 0x01f80000 ) >> 13 ) | ( ( r & 0x001f8000 ) >> 15 );
                    uint r48R = ( ( r & 0x0001f800 ) << 7 ) | ( ( r & 0x00001f80 ) << 5 ) | ( ( r & 0x000001f8 ) << 3 ) |
                                ( ( r & 0x0000001f ) << 1 ) | ( ( r & 0x80000000 ) >> 31 );
                    f = ( r48L ^ r48R ) & saltbits;
                    r48L ^= f ^ kl[ j ];
                    r48R ^= f ^ kr[ j++ ];
                    f = psbox[ 0 , mSbox[ 0 , r48L >> 12 ] ] | psbox[ 1 , mSbox[ 1 , r48L & 0xfff ] ] |
                        psbox[ 2 , mSbox[ 2 , r48R >> 12 ] ] | psbox[ 3 , mSbox[ 3 , r48R & 0xfff ] ];
                    f ^= l;
                    l = r;
                    r = f;
                }

                r = l;
                l = f;
            }

            lOut = fpMaskl[ 0 , l >> 24 ] | fpMaskl[ 1 , ( l >> 16 ) & 0xff ] | fpMaskl[ 2 , ( l >> 8 ) & 0xff ] |
                    fpMaskl[ 3 , l & 0xff ] | fpMaskl[ 4 , r >> 24 ] | fpMaskl[ 5 , ( r >> 16 ) & 0xff ] |
                    fpMaskl[ 6 , ( r >> 8 ) & 0xff ] | fpMaskl[ 7 , r & 0xff ];
            rOut = fpMaskr[ 0 , l >> 24 ] | fpMaskr[ 1 , ( l >> 16 ) & 0xff ] | fpMaskr[ 2 , ( l >> 8 ) & 0xff ] |
                    fpMaskr[ 3 , l & 0xff ] | fpMaskr[ 4 , r >> 24 ] | fpMaskr[ 5 , ( r >> 16 ) & 0xff ] |
                    fpMaskr[ 6 , ( r >> 8 ) & 0xff ] | fpMaskr[ 7 , r & 0xff ];
            return 0;
        }

        private void SetupSalt( uint salt )
        {
            if ( salt == oldSalt )
            {
                return;
            }

            oldSalt = salt;
            saltbits = 0;
            uint saltbit = 1;
            uint obit = 0x800000;
            for ( int i = 0 ; i < 24 ; i++ )
            {
                if ( Convert.ToBoolean( salt & saltbit ) )
                {
                    saltbits |= obit;
                }

                saltbit <<= 1;
                obit >>= 1;
            }
        }

        #endregion
    }
}