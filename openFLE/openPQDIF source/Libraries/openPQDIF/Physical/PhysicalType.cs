﻿//*********************************************************************************************************************
// PhysicalType.cs
//
// Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC. All rights reserved.
//
// openFLE ("this software") is licensed under BSD 3-Clause license.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
// following conditions are met:
//
// •    Redistributions of source code must retain the above copyright  notice, this list of conditions and 
//      the following disclaimer.
//
// •    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//
// •    Neither the name of the Electric Power Research Institute, Inc. (“EPRI”) nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
// DISCLAIMED. IN NO EVENT SHALL EPRI BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
// OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
//
//*********************************************************************************************************************
//
//  Code Modification History:
//  -------------------------------------------------------------------------------------------------------------------
//  05/02/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;

namespace openPQDIF.Physical
{

    #region [ Enumerations ]

    /// <summary>
    /// Enumeration that defines the types of values stored in
    /// <see cref="ScalarElement"/>s and <see cref="VectorElement"/>s.
    /// </summary>
    public enum PhysicalType : byte
    {
        /// <summary>
        /// 1-byte boolean
        /// </summary>
        Boolean1 = 1,

        /// <summary>
        /// 2-byte boolean
        /// </summary>
        Boolean2 = 2,

        /// <summary>
        /// 4-byte boolean
        /// </summary>
        Boolean4 = 3,

        /// <summary>
        /// 1-byte character (ASCII)
        /// </summary>
        Char1 = 10,

        /// <summary>
        /// 2-byte character (UTF-16)
        /// </summary>
        Char2 = 11,

        /// <summary>
        /// 8-bit signed integer
        /// </summary>
        Integer1 = 20,

        /// <summary>
        /// 16-bit signed integer
        /// </summary>
        Integer2 = 21,

        /// <summary>
        /// 32-bit signed integer
        /// </summary>
        Integer4 = 22,

        /// <summary>
        /// 8-bit unsigned integer
        /// </summary>
        UnsignedInteger1 = 30,

        /// <summary>
        /// 16-bit unsigned integer
        /// </summary>
        UnsignedInteger2 = 31,

        /// <summary>
        /// 32-bit unsigned integer
        /// </summary>
        UnsignedInteger4 = 32,

        /// <summary>
        /// 32-bit floating point number
        /// </summary>
        Real4 = 40,

        /// <summary>
        /// 64-bit floating point number
        /// </summary>
        Real8 = 41,

        /// <summary>
        /// 8-byte complex number
        /// </summary>
        /// <remarks>
        /// The first four bytes represent the real part of the complex
        /// number, and the last four bytes represent the imaginary part.
        /// Both values are 64-bit floating point numbers.
        /// </remarks>
        Complex8 = 42,

        /// <summary>
        /// 16-byte complex number
        /// </summary>
        /// <remarks>
        /// The first eight bytes represent the real part of the complex
        /// number, and the last eight bytes represent the imaginary part.
        /// Both values are 64-bit floating point numbers.
        /// </remarks>
        Complex16 = 43,

        /// <summary>
        /// 12-byte timestamp
        /// </summary>
        /// <remarks>
        /// The first four bytes represent the days since January 1, 1900
        /// UTC. The last eight bytes represent the number of seconds since
        /// midnight. The number of days is an unsigned 32-bit integer, and
        /// the number of seconds is a 64-bit floating point number.
        /// </remarks>
        Timestamp = 50,

        /// <summary>
        /// 128-bit globally unique identifier
        /// </summary>
        Guid = 60
    }

    #endregion

    /// <summary>
    /// Defines extension methods for <see cref="PhysicalType"/>.
    /// </summary>
    public static class PysicalTypeExtensions
    {
        /// <summary>
        /// Gets the size of the physical type, in bytes.
        /// </summary>
        /// <param name="type">The physical type.</param>
        /// <returns>The size of the physical type, in bytes.</returns>
        public static int GetByteSize(this PhysicalType type)
        {
            switch (type)
            {
                case PhysicalType.Boolean1:
                case PhysicalType.Char1:
                case PhysicalType.Integer1:
                case PhysicalType.UnsignedInteger1:
                    return 1;

                case PhysicalType.Boolean2:
                case PhysicalType.Char2:
                case PhysicalType.Integer2:
                case PhysicalType.UnsignedInteger2:
                    return 2;

                case PhysicalType.Boolean4:
                case PhysicalType.Integer4:
                case PhysicalType.UnsignedInteger4:
                case PhysicalType.Real4:
                    return 4;

                case PhysicalType.Real8:
                case PhysicalType.Complex8:
                    return 8;

                case PhysicalType.Timestamp:
                    return 12;

                case PhysicalType.Complex16:
                case PhysicalType.Guid:
                    return 16;

                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }
    }
}
