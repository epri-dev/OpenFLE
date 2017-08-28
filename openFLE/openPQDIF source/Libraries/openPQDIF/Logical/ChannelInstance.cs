//*********************************************************************************************************************
// ChannelInstance.cs
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
using System.Linq;
using System.Text;
using openPQDIF.Physical;

namespace openPQDIF.Logical
{
    /// <summary>
    /// Represents a channel instance in a PQDIF file. A channel instance
    /// resides in an <see cref="ObservationRecord"/>, and is defined by
    /// a <see cref="ChannelDefinition"/> inside the observation record's
    /// <see cref="DataSourceRecord"/>.
    /// </summary>
    public class ChannelInstance
    {
        #region [ Members ]

        // Fields
        private CollectionElement m_physicalStructure;
        private ObservationRecord m_observationRecord;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="ChannelInstance"/> class.
        /// </summary>
        /// <param name="physicalStructure">The collection element which is the physical structure of the channel instance.</param>
        /// <param name="observationRecord">The observation record in which the channel instance resides.</param>
        public ChannelInstance(CollectionElement physicalStructure, ObservationRecord observationRecord)
        {
            m_physicalStructure = physicalStructure;
            m_observationRecord = observationRecord;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the observation record in which the channel instance resides.
        /// </summary>
        public ObservationRecord ObservationRecord
        {
            get
            {
                return m_observationRecord;
            }
        }

        /// <summary>
        /// Gets the index of the <see cref="ChannelDefinition"/>
        /// which defines the channel instance.
        /// </summary>
        public uint ChannelDefinitionIndex
        {
            get
            {
                return m_physicalStructure
                    .GetScalarByTag(ChannelDefinitionIndexTag)
                    .GetUInt4();
            }
        }

        /// <summary>
        /// Gets the channel defintion which defines the channel instance.
        /// </summary>
        public ChannelDefinition Definition
        {
            get
            {
                return m_observationRecord.DataSource.ChannelDefinitions[(int)ChannelDefinitionIndex];
            }
        }

        /// <summary>
        /// Gets the name of the of a device specific code or hardware
        /// module, algorithm, or rule not necessarily channel based
        /// that cause this channel to be recorded.
        /// </summary>
        public string TriggerModuleName
        {
            get
            {
                VectorElement moduleNameVector =  m_physicalStructure.GetVectorByTag(ChannelTriggerModuleNameTag);

                if ((object)moduleNameVector == null)
                    return null;

                return Encoding.ASCII.GetString(moduleNameVector.GetValues()).Trim((char)0);
            }
        }

        /// <summary>
        /// Gets the name of the device involved in
        /// an external cross trigger scenario.
        /// </summary>
        public string CrossTriggerDeviceName
        {
            get
            {
                VectorElement deviceNameVector = m_physicalStructure.GetVectorByTag(CrossTriggerDeviceNameTag);

                if ((object)deviceNameVector == null)
                    return null;

                return Encoding.ASCII.GetString(deviceNameVector.GetValues()).Trim((char)0);
            }
        }

        /// <summary>
        /// Gets the series instances contained in this channel.
        /// </summary>
        public IList<SeriesInstance> SeriesInstances
        {
            get
            {
                return m_physicalStructure
                    .GetCollectionByTag(SeriesInstancesTag)
                    .GetElementsByTag(OneSeriesInstanceTag)
                    .Cast<CollectionElement>()
                    .Zip(Definition.SeriesDefinitions, (collection, seriesDefinition) => new SeriesInstance(collection, this, seriesDefinition))
                    .ToList();
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields

        /// <summary>
        /// Tag that identifies the channel definition index.
        /// </summary>
        public static readonly Guid ChannelDefinitionIndexTag = new Guid("b48d858f-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the series instances collection.
        /// </summary>
        public static readonly Guid SeriesInstancesTag = new Guid("3d786f93-f76e-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies a single series instance in the collection.
        /// </summary>
        public static readonly Guid OneSeriesInstanceTag = new Guid("3d786f94-f76e-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the channel trigger module name.
        /// </summary>
        public static readonly Guid ChannelTriggerModuleNameTag = new Guid("0fa118c6-cb4a-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the cross trigger device name.
        /// </summary>
        public static readonly Guid CrossTriggerDeviceNameTag = new Guid("0fa118c5-cb4a-11cf-9d89-0080c72e70a3");

        #endregion

    }
}
