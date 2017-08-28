//*********************************************************************************************************************
// Cycle.cs
//
// Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC. All rights reserved.
//
// openFLE ("this software") is licensed under BSD 3-Clause license.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
// following conditions are met:
//
//     Redistributions of source code must retain the above copyright  notice, this list of conditions and 
//      the following disclaimer.
//
//     Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//
//     Neither the name of the Electric Power Research Institute, Inc. (EPRI) nor the names of its contributors 
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
//
// This software incorporates work covered by the following copyright and permission notice: 
//
//     TVA Code Library 4.0.4.3 - Tennessee Valley Authority, tvainfo@tva.gov
//      No copyright is claimed pursuant to 17 USC § 105. All Other Rights Reserved.
//
//      Licensed under TVA Custom License based on NASA Open Source Agreement (TVA Custom NOSA); 
//      you may not use TVA Code Library except in compliance with the TVA Custom NOSA. You may  
//      obtain a copy of the TVA Custom NOSA at http://tvacodelibrary.codeplex.com/license.
//
//      TVA Code Library is provided by the copyright holders and contributors "as is" and any express 
//      or implied warranties, including, but not limited to, the implied warranties of merchantability 
//      and fitness for a particular purpose are disclaimed.
//
//*********************************************************************************************************************
//
//  Code Modification History:
//  -------------------------------------------------------------------------------------------------------------------
//  05/23/2012 - J. Ritchie Carroll, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using TVA;
using TVA.Units;

namespace FaultAlgorithms
{
    /// <summary>
    /// Represents a cycle of single phase power frequency-domain data.
    /// </summary>
    public class Cycle
    {
        #region [ Members ]

        // Fields

        /// <summary>
        /// The actual frequency of the cycle in hertz.
        /// </summary>
        public double Frequency;

        /// <summary>
        /// The complex number representation of the RMS phasor.
        /// </summary>
        public ComplexNumber Complex;

        /// <summary>
        /// The most extreme data point in the cycle.
        /// </summary>
        public double Peak;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Root-mean-square of the <see cref="MeasurementData.Values"/> in the cycle.
        /// </summary>
        public double RMS
        {
            get
            {
                return Complex.Magnitude;
            }
            set
            {
                Complex.Magnitude = value;
            }
        }

        /// <summary>
        /// Phase angle of the start of the cycle, relative to the reference angle.
        /// </summary>
        public Angle Phase
        {
            get
            {
                return Complex.Angle;
            }
            set
            {
                Complex.Angle = value;
            }
        }

        #endregion
    }
}