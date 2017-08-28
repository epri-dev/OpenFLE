//*********************************************************************************************************************
// SeriesInstance.cs
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
//  05/04/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;
using System.Collections.Generic;
using openPQDIF.Physical;

namespace openPQDIF.Logical
{
    /// <summary>
    /// Represents an instance of a series in a PQDIF file. A series
    /// instance resides in a <see cref="ChannelInstance"/> and is
    /// defined by a <see cref="SeriesDefinition"/>.
    /// </summary>
    public class SeriesInstance
    {
        #region [ Members ]

        // Fields
        private CollectionElement m_physicalStructure;
        private ChannelInstance m_channel;
        private SeriesDefinition m_definition;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="SeriesInstance"/> class.
        /// </summary>
        /// <param name="physicalStructure">The physical structure of the series instance.</param>
        /// <param name="channel">The channel instance that this series instance resides in.</param>
        /// <param name="definition">The series definition that defines this series instance.</param>
        public SeriesInstance(CollectionElement physicalStructure, ChannelInstance channel, SeriesDefinition definition)
        {
            m_physicalStructure = physicalStructure;
            m_channel = channel;
            m_definition = definition;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the channel instance in which the series instance resides.
        /// </summary>
        public ChannelInstance Channel
        {
            get
            {
                return m_channel;
            }
        }

        /// <summary>
        /// Gets the series definition that defines the series.
        /// </summary>
        public SeriesDefinition Definition
        {
            get
            {
                return m_definition;
            }
        }

        /// <summary>
        /// Gets the value by which to scale the values in
        /// order to restore the original data values.
        /// </summary>
        public ScalarElement SeriesScale
        {
            get
            {
                return m_physicalStructure.GetScalarByTag(SeriesScaleTag);
            }
        }

        /// <summary>
        /// Gets the value added to the values in order
        /// to restore the original data values.
        /// </summary>
        public ScalarElement SeriesOffset
        {
            get
            {
                return m_physicalStructure.GetScalarByTag(SeriesOffsetTag);
            }
        }

        /// <summary>
        /// Gets the values contained in this series instance.
        /// </summary>
        public VectorElement SeriesValues
        {
            get
            {
                return m_physicalStructure.GetVectorByTag(SeriesValuesTag);
            }
        }

        /// <summary>
        /// Gets the original data values, after expanding
        /// sequences and scale and offset modifications.
        /// </summary>
        public IList<object> OriginalValues
        {
            get
            {
                return GetOriginalValues();
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets the original data values by expanding
        /// sequences and applying scale and offset.
        /// </summary>
        /// <returns>A list of the original data values.</returns>
        private IList<object> GetOriginalValues()
        {
            IList<object> values = new List<object>();
            VectorElement valuesVector = SeriesValues;
            StorageMethods storageMethods = Definition.StorageMethodID;

            bool incremented = (storageMethods & StorageMethods.Increment) != 0;
            dynamic start, count, increment;

            bool scaled = (storageMethods & StorageMethods.Scaled) != 0;
            dynamic offset = ((object)SeriesOffset != null) ? SeriesOffset.Get() : 0;
            dynamic scale = ((object)SeriesScale != null) ? SeriesScale.Get() : 1;
            dynamic value;

            if (!scaled)
            {
                offset = 0;
                scale = 1;
            }

            if (incremented)
            {
                start = valuesVector.Get(0);
                count = valuesVector.Get(1);
                increment = valuesVector.Get(2);

                for (int i = 0; i < count; i++)
                    values.Add((object)(start + (i * increment)));
            }
            else
            {
                for (int i = 0; i < valuesVector.Size; i++)
                    values.Add(valuesVector.Get(i));
            }

            for (int i = 0; i < values.Count; i++)
            {
                value = values[i];
                values[i] = offset + (value * scale);
            }

            return values;
        }

        #endregion

        #region [ Static ]

        // Static Fields

        /// <summary>
        /// Tag that identifies the scale value to apply to the series.
        /// </summary>
        public static readonly Guid SeriesScaleTag = new Guid("3d786f96-f76e-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the offset value to apply to the series.
        /// </summary>
        public static readonly Guid SeriesOffsetTag = new Guid("3d786f97-f76e-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the values contained in the series.
        /// </summary>
        public static readonly Guid SeriesValuesTag = new Guid("3d786f99-f76e-11cf-9d89-0080c72e70a3");

        #endregion

    }
}
