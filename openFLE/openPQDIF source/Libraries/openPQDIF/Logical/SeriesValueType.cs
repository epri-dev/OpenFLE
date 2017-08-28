﻿//*********************************************************************************************************************
// SeriesValueType.cs
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
//  05/08/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;

namespace openPQDIF.Logical
{
    /// <summary>
    /// Defines tags used to identify different series value types.
    /// </summary>
    public static class SeriesValueType
    {
        /// <summary>
        /// Value type for a measurement.
        /// </summary>
        public static readonly Guid Val = new Guid("67f6af97-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Time.
        /// </summary>
        public static readonly Guid Time = new Guid("c690e862-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Minimum.
        /// </summary>
        public static readonly Guid Min = new Guid("67f6af98-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Maximum.
        /// </summary>
        public static readonly Guid Max = new Guid("67f6af99-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Average.
        /// </summary>
        public static readonly Guid Avg = new Guid("67f6af9a-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Instantaneous.
        /// </summary>
        public static readonly Guid Inst = new Guid("67f6af9b-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Phase angle.
        /// </summary>
        public static readonly Guid PhaseAngle = new Guid("67f6af9d-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Phase angle which corresponds to a <see cref="Min"/> series.
        /// </summary>
        public static readonly Guid PhaseAngleMin = new Guid("dc762340-3c56-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Phase angle which corresponds to a <see cref="Max"/> series.
        /// </summary>
        public static readonly Guid PhaseAngleMax = new Guid("dc762341-3c56-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Phase angle which corresponds to an <see cref="Avg"/> series.
        /// </summary>
        public static readonly Guid PhaseAngleAvg = new Guid("dc762342-3c56-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Area under the signal, usually an rms voltage, current, or other quantity.
        /// </summary>
        public static readonly Guid Area = new Guid("c7825ce0-8ace-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Latitude.
        /// </summary>
        public static readonly Guid Latitude = new Guid("c690e864-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Duration.
        /// </summary>
        public static readonly Guid Duration = new Guid("c690e863-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Longitude.
        /// </summary>
        public static readonly Guid Longitude = new Guid("c690e865-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Polarity.
        /// </summary>
        public static readonly Guid Polarity = new Guid("c690e866-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Ellipse (for lightning flash density).
        /// </summary>
        public static readonly Guid Ellipse = new Guid("c690e867-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// BinID.
        /// </summary>
        public static readonly Guid BinID = new Guid("c690e869-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// BinHigh.
        /// </summary>
        public static readonly Guid BinHigh = new Guid("c690e86a-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// BinLow.
        /// </summary>
        public static readonly Guid BinLow = new Guid("c690e86b-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// XBinHigh.
        /// </summary>
        public static readonly Guid XBinHigh = new Guid("c690e86c-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// XBinLow.
        /// </summary>
        public static readonly Guid XBinLow = new Guid("c690e86d-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// YBinHigh.
        /// </summary>
        public static readonly Guid YBinHigh = new Guid("c690e86e-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// YBinLow.
        /// </summary>
        public static readonly Guid YBinLow = new Guid("c690e86f-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Count.
        /// </summary>
        public static readonly Guid Count = new Guid("c690e870-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Transition event code series.
        /// </summary>
        /// <remarks>
        /// This series contains codes corresponding to values in a value
        /// series that indicates what kind of transition caused the event
        /// to be recorded. Used only with VALUELOG data.
        /// </remarks>
        public static readonly Guid Transition = new Guid("5369c260-c347-11d2-923f-00104b2b84b1");

        /// <summary>
        /// Cumulative probability in percent.
        /// </summary>
        public static readonly Guid Prob = new Guid("6763cc71-17d6-11d4-9f1c-002078e0b723");

        /// <summary>
        /// Interval data.
        /// </summary>
        public static readonly Guid Interval = new Guid("72e82a40-336c-11d5-a4b3-444553540000");

        /// <summary>
        /// Status data.
        /// </summary>
        public static readonly Guid Status = new Guid("b82b5c82-55c7-11d5-a4b3-444553540000");

        /// <summary>
        /// Probability: 1%.
        /// </summary>
        public static readonly Guid P1 = new Guid("67f6af9c-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Probability: 5%.
        /// </summary>
        public static readonly Guid P5 = new Guid("67f6af9d-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Probability: 10%.
        /// </summary>
        public static readonly Guid P10 = new Guid("67f6af9e-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Probability: 90%.
        /// </summary>
        public static readonly Guid P90 = new Guid("67f6af9f-f753-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Probability: 95%.
        /// </summary>
        public static readonly Guid P95 = new Guid("c690e860-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Probability: 99%.
        /// </summary>
        public static readonly Guid P99 = new Guid("c690e861-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Frequency.
        /// </summary>
        public static readonly Guid Frequency = new Guid("c690e868-f755-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Returns the name of the given series value type.
        /// </summary>
        /// <param name="seriesValueType">The GUID tag which identifies the series value type.</param>
        /// <returns>The name of the given series value type.</returns>
        public static string ToString(Guid seriesValueType)
        {
            if (seriesValueType == Val)
                return "Values";
            else if (seriesValueType == Time)
                return "Time";
            else if (seriesValueType == Min)
                return "Minimum";
            else if (seriesValueType == Max)
                return "Maximum";
            else if (seriesValueType == Avg)
                return "Average";
            else if (seriesValueType == Inst)
                return "Instantaneous";
            else if (seriesValueType == PhaseAngle)
                return "Phase Angle";
            else if (seriesValueType == PhaseAngleMin)
                return "Phase Angle Mininum";
            else if (seriesValueType == PhaseAngleMax)
                return "Phase Angle Maximum";
            else if (seriesValueType == PhaseAngleAvg)
                return "Phase Angle Average";
            else if (seriesValueType == Area)
                return "Area";
            else if (seriesValueType == Latitude)
                return "Latitude";
            else if (seriesValueType == Duration)
                return "Duration";
            else if (seriesValueType == Longitude)
                return "Longitude";
            else if (seriesValueType == Polarity)
                return "Polarity";
            else if (seriesValueType == Ellipse)
                return "Ellipse";
            else if (seriesValueType == BinID)
                return "Bin ID";
            else if (seriesValueType == BinHigh)
                return "Bin High";
            else if (seriesValueType == BinLow)
                return "Bin Low";
            else if (seriesValueType == XBinHigh)
                return "X Bin High";
            else if (seriesValueType == XBinLow)
                return "X Bin Low";
            else if (seriesValueType == YBinHigh)
                return "Y Bin High";
            else if (seriesValueType == YBinLow)
                return "Y Bin Low";
            else if (seriesValueType == Count)
                return "Count";
            else if (seriesValueType == Transition)
                return "Transition";
            else if (seriesValueType == Prob)
                return "Probability";
            else if (seriesValueType == Interval)
                return "Interval";
            else if (seriesValueType == P1)
                return "Probability: 1%";
            else if (seriesValueType == P5)
                return "Probability: 5%";
            else if (seriesValueType == P10)
                return "Probability: 10%";
            else if (seriesValueType == P90)
                return "Probability: 90%";
            else if (seriesValueType == P95)
                return "Probability: 95%";
            else if (seriesValueType == P99)
                return "Probability: 99%";
            else if (seriesValueType == Frequency)
                return "Frequency";

            throw new ArgumentOutOfRangeException();
        }
    }
}
