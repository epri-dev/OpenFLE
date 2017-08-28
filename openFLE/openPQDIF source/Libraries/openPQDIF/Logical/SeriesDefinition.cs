//*********************************************************************************************************************
// SeriesDefinition.cs
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
//  05/04/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;
using System.Text;
using openPQDIF.Physical;
using TVA;

namespace openPQDIF.Logical
{
    #region [ Enumerations ]

    /// <summary>
    /// Defines flags that determine the how the
    /// data is stored in a series instance.
    /// </summary>
    [Flags]
    public enum StorageMethods : uint
    {
        /// <summary>
        /// Straight array of data points.
        /// </summary>
        Values = (uint)Bits.Bit00,
        
        /// <summary>
        /// Data values are scaled.
        /// </summary>
        Scaled = (uint)Bits.Bit01,

        /// <summary>
        /// Start, count, and increment are stored and
        /// the series is recreated from those values.
        /// </summary>
        Increment = (uint)Bits.Bit02
    }

    /// <summary>
    /// Units of data defined in a PQDIF file.
    /// </summary>
    public enum QuantityUnits : uint
    {
        /// <summary>
        /// Unitless.
        /// </summary>
        None = 0u,

        /// <summary>
        /// Absolute time. Each timestamp in the series must be in absolute
        /// time using the <see cref="PhysicalType.Timestamp"/> type.
        /// </summary>
        Timestamp = 1u,

        /// <summary>
        /// Seconds relative to the start time of an observation.
        /// </summary>
        /// <seealso cref="ObservationRecord.StartTime"/>
        Seconds = 2u,

        /// <summary>
        /// Cycles relative to the start time of an observation.
        /// </summary>
        /// <seealso cref="ObservationRecord.StartTime"/>
        Cycles = 3u,

        /// <summary>
        /// Volts.
        /// </summary>
        Volts = 6u,

        /// <summary>
        /// Amperes.
        /// </summary>
        Amps = 7u,

        /// <summary>
        /// Volt-amperes.
        /// </summary>
        VoltAmps = 8u,

        /// <summary>
        /// Watts.
        /// </summary>
        Watts = 9u,

        /// <summary>
        /// Volt-amperes reactive.
        /// </summary>
        Vars = 10u,

        /// <summary>
        /// Ohms.
        /// </summary>
        Ohms = 11u,

        /// <summary>
        /// Siemens.
        /// </summary>
        Siemens = 12u,

        /// <summary>
        /// Volts per ampere.
        /// </summary>
        VoltsPerAmp = 13u,

        /// <summary>
        /// Joules.
        /// </summary>
        Joules = 14u,

        /// <summary>
        /// Hertz.
        /// </summary>
        Hertz = 15u,

        /// <summary>
        /// Celcius.
        /// </summary>
        Celcius = 16u,

        /// <summary>
        /// Degrees of arc.
        /// </summary>
        Degrees = 17u,

        /// <summary>
        /// Decibels.
        /// </summary>
        Decibels = 18u,

        /// <summary>
        /// Percent.
        /// </summary>
        Percent = 19u,

        /// <summary>
        /// Per-unit.
        /// </summary>
        PerUnit = 20u,

        /// <summary>
        /// Number of counts or samples.
        /// </summary>
        Samples = 21u,

        /// <summary>
        /// Energy in var-hours.
        /// </summary>
        VarHours = 22u,

        /// <summary>
        /// Energy in watt-hours.
        /// </summary>
        WattHours = 23u,

        /// <summary>
        /// Energy in VA-hours.
        /// </summary>
        VoltAmpHours = 24u,

        /// <summary>
        /// Meters/second.
        /// </summary>
        MetersPerSecond = 25u,

        /// <summary>
        /// Miles/hour.
        /// </summary>
        MilesPerHour = 26u,

        /// <summary>
        /// Pressure in bars.
        /// </summary>
        Bars = 27u,

        /// <summary>
        /// Pressure in pascals.
        /// </summary>
        Pascals = 28u,

        /// <summary>
        /// Force in newtons.
        /// </summary>
        Newtons = 29u,

        /// <summary>
        /// Torque in newton-meters.
        /// </summary>
        NewtonMeters = 30u,

        /// <summary>
        /// Revolutions/minute.
        /// </summary>
        RevolutionsPerMinute = 31u,

        /// <summary>
        /// Radians/second.
        /// </summary>
        RadiansPerSecond = 32u,

        /// <summary>
        /// Meters.
        /// </summary>
        Meters = 33u,

        /// <summary>
        /// Flux linkage in Weber Turns.
        /// </summary>
        WeberTurns = 34u,

        /// <summary>
        /// Flux density in teslas.
        /// </summary>
        Teslas = 35u,

        /// <summary>
        /// Magnetic field in webers.
        /// </summary>
        Webers = 36u,

        /// <summary>
        /// Volts/volt transfer function.
        /// </summary>
        VoltsPerVolt = 37u,

        /// <summary>
        /// Amps/amp transfer function.
        /// </summary>
        AmpsPerAmp = 38u,
        
        /// <summary>
        /// Impedance transfer function.
        /// </summary>
        AmpsPerVolt = 39u
    }

    #endregion

    /// <summary>
    /// Definition of a <see cref="SeriesInstance"/>.
    /// </summary>
    public class SeriesDefinition
    {
        #region [ Members ]

        // Fields
        private CollectionElement m_physicalStructure;
        private ChannelDefinition m_channelDefinition;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="SeriesDefinition"/> class.
        /// </summary>
        /// <param name="physicalStructure">The collection that is the physical structure of the series definition.</param>
        /// <param name="channelDefinition">The channel definition in which the series definition resides.</param>
        public SeriesDefinition(CollectionElement physicalStructure, ChannelDefinition channelDefinition)
        {
            m_physicalStructure = physicalStructure;
            m_channelDefinition = channelDefinition;
        }

        #endregion

        #region [ Properties ]
        
        /// <summary>
        /// Gets the channel definition in which the series definition resides.
        /// </summary>
        public ChannelDefinition ChannelDefinition
        {
            get
            {
                return m_channelDefinition;
            }
        }

        /// <summary>
        /// Gets the value type ID of the series.
        /// </summary>
        /// <seealso cref="SeriesValueType"/>
        public Guid ValueTypeID
        {
            get
            {
                return m_physicalStructure
                    .GetScalarByTag(ValueTypeIDTag)
                    .GetGuid();
            }
        }

        /// <summary>
        /// Gets the units of the data in the series.
        /// </summary>
        public QuantityUnits QuantityUnits
        {
            get
            {
                return (QuantityUnits)m_physicalStructure
                    .GetScalarByTag(QuantityUnitsIDTag)
                    .GetUInt4();
            }
        }

        /// <summary>
        /// Gets additional detail about the meaning of the series data.
        /// </summary>
        public Guid QuantityCharacteristicID
        {
            get
            {
                return m_physicalStructure
                    .GetScalarByTag(QuantityCharacteristicIDTag)
                    .GetGuid();
            }
        }

        /// <summary>
        /// Gets the storage method ID, which can be used with
        /// <see cref="StorageMethods"/> to determine how the data is stored.
        /// </summary>
        public StorageMethods StorageMethodID
        {
            get
            {
                return (StorageMethods)m_physicalStructure
                    .GetScalarByTag(StorageMethodIDTag)
                    .GetUInt4();
            }
        }

        /// <summary>
        /// Gets the value type name of the series.
        /// </summary>
        public string ValueTypeName
        {
            get
            {
                VectorElement valueTypeNameVector = m_physicalStructure.GetVectorByTag(ValueTypeNameTag);

                if ((object)valueTypeNameVector == null)
                    return null;

                return Encoding.ASCII.GetString(valueTypeNameVector.GetValues()).Trim((char)0);
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields

        /// <summary>
        /// Tag that identifies the value type ID of the series.
        /// </summary>
        public static readonly Guid ValueTypeIDTag = new Guid("b48d859c-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the quantity units ID of the series.
        /// </summary>
        public static readonly Guid QuantityUnitsIDTag = new Guid("b48d859b-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the characteristic ID of the series.
        /// </summary>
        public static readonly Guid QuantityCharacteristicIDTag = new Guid("3d786f9e-f76e-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the storage method ID of the series.
        /// </summary>
        public static readonly Guid StorageMethodIDTag = new Guid("b48d85a1-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the value type name of the series.
        /// </summary>
        public static readonly Guid ValueTypeNameTag = new Guid("b48d859d-f5f5-11cf-9d89-0080c72e70a3");

        #endregion

    }
}
