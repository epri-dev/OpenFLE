﻿//*********************************************************************************************************************
// QuantityType.cs
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
//  Updates and documentation to openPQDIF can be found at http://openPQDIF.codeplex.com/
//
//  Code Modification History:
//  -------------------------------------------------------------------------------------------------------------------
//  05/16/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;

namespace openPQDIF.Logical
{
    /// <summary>
    /// The high-level description of the type of
    /// quantity which is being captured by a channel.
    /// </summary>
    public static class QuantityType
    {
        /// <summary>
        /// Point-on-wave measurements.
        /// </summary>
        public static readonly Guid WaveForm = new Guid("67f6af80-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Time-based logged entries.
        /// </summary>
        public static readonly Guid ValueLog = new Guid("67f6af82-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Time-domain measurements including
        /// magnitudes and (optionally) phase angle.
        /// </summary>
        public static readonly Guid Phasor = new Guid("67f6af81-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Frequency-domain measurements including
        /// magnitude and (optionally) phase angle.
        /// </summary>
        public static readonly Guid Response = new Guid("67f6af85-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Time, latitude, longitude, value, polarity, ellipse.
        /// </summary>
        public static readonly Guid Flash = new Guid("67f6af83-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// BinLow, BinHigh, BinID, count.
        /// </summary>
        public static readonly Guid Histogram = new Guid("67f6af87-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// XBinLow, XBinHigh, YBinLow, YBinHigh, BinID, count.
        /// </summary>
        public static readonly Guid Histogram3D = new Guid("67f6af88-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Probability, value.
        /// </summary>
        public static readonly Guid CPF = new Guid("67f6af89-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// X-values and y-values.
        /// </summary>
        public static readonly Guid XY = new Guid("67f6af8a-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Magnitude and duration.
        /// </summary>
        public static readonly Guid MagDur = new Guid("67f6af8b-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// X-values, y-values, and z-values.
        /// </summary>
        public static readonly Guid XYZ = new Guid("67f6af8c-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Time, magnitude, and duration.
        /// </summary>
        public static readonly Guid MagDurTime = new Guid("67f6af8d-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Time, magnitude, duration, and count.
        /// </summary>
        public static readonly Guid MagDurCount = new Guid("67f6af8e-f753-11cf-9d89-0080c72e70a3");
    }
}
