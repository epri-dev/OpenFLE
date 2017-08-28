//*********************************************************************************************************************
// FaultLocationDataSet.cs
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
//
// This software incorporates work covered by the following copyright and permission notice: 
//
// •    TVA Code Library 4.0.4.3 - Tennessee Valley Authority, tvainfo@tva.gov
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
//  06/19/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System.Collections.Generic;
using System.Linq;
using TVA;

namespace FaultAlgorithms
{
    public enum FaultType
    {
        AN = 1,
        BN = 2,
        CN = 3,
        AB = 4,
        BC = 5,
        CA = 6,
        ABC = 7,
        None = 0
    }

    public class FaultLocationDataSet
    {
        #region [ Members ]

        // Fields
        private MeasurementDataSet m_voltages;
        private MeasurementDataSet m_currents;
        private CycleDataSet m_cycles;

        private ComplexNumber m_positiveImpedance;
        private ComplexNumber m_zeroImpedance;

        private FaultType m_faultType;
        private IList<int> m_faultedCycles;
        private int m_faultCalculationCycle;
        private double m_lineDistance;
        private double m_faultDistance;

        private Dictionary<string, object> m_values;

        #endregion

        #region [ Constructors ]

        public FaultLocationDataSet(MeasurementDataSet voltages, MeasurementDataSet currents, CycleDataSet cycles)
        {
            m_voltages = voltages;
            m_currents = currents;
            m_cycles = cycles;

            m_values = new Dictionary<string, object>();
        }

        #endregion

        #region [ Properties ]

        public MeasurementDataSet Voltages
        {
            get
            {
                return m_voltages;
            }
        }

        public MeasurementDataSet Currents
        {
            get
            {
                return m_currents;
            }
        }

        public CycleDataSet Cycles
        {
            get
            {
                return m_cycles;
            }
        }

        /// <summary>
        /// Gets or sets the positive sequence impedance.
        /// </summary>
        public ComplexNumber PositiveImpedance
        {
            get
            {
                return m_positiveImpedance;
            }
            set
            {
                m_positiveImpedance = value;
            }
        }

        /// <summary>
        /// Gets or sets the zero sequence impedance.
        /// </summary>
        public ComplexNumber ZeroImpedance
        {
            get
            {
                return m_zeroImpedance;
            }
            set
            {
                m_zeroImpedance = value;
            }
        }

        /// <summary>
        /// Gets the loop impedance <c>[(Z0 + 2*Z1) / 3]</c>.
        /// </summary>
        public ComplexNumber LoopImpedance
        {
            get
            {
                return (m_zeroImpedance + 2.0D * m_positiveImpedance) / 3.0D;
            }
        }

        /// <summary>
        /// Gets or sets the positive sequence impedance.
        /// </summary>
        public ComplexNumber Z1
        {
            get
            {
                return m_positiveImpedance;
            }
            set
            {
                m_positiveImpedance = value;
            }
        }

        /// <summary>
        /// Gets or sets the zero sequence impedance.
        /// </summary>
        public ComplexNumber Z0
        {
            get
            {
                return m_zeroImpedance;
            }
            set
            {
                m_zeroImpedance = value;
            }
        }

        /// <summary>
        /// Gets the loop impedance <c>[(Z0 + 2*Z1) / 3]</c>.
        /// </summary>
        public ComplexNumber Zs
        {
            get
            {
                return LoopImpedance;
            }
        }

        public FaultType FaultType
        {
            get
            {
                return m_faultType;
            }
            set
            {
                m_faultType = value;
            }
        }

        public IList<int> FaultedCycles
        {
            get
            {
                return m_faultedCycles;
            }
            set
            {
                m_faultedCycles = value;
            }
        }

        public int FaultCycleCount
        {
            get
            {
                return m_faultedCycles.Count();
            }
        }

        public int FaultCalculationCycle
        {
            get
            {
                return m_faultCalculationCycle;
            }
            set
            {
                m_faultCalculationCycle = value;
            }
        }

        public double LineDistance
        {
            get
            {
                return m_lineDistance;
            }
            set
            {
                m_lineDistance = value;
            }
        }

        public double FaultDistance
        {
            get
            {
                return m_faultDistance;
            }
            set
            {
                m_faultDistance = value;
            }
        }

        public object this[string name]
        {
            get
            {
                return m_values[name];
            }
            set
            {
                m_values[name] = value;
            }
        }

        #endregion

    }
}
