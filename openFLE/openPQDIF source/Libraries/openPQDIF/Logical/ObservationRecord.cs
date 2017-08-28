//*********************************************************************************************************************
// ObservationRecord.cs
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
//  05/03/2012 - Stephen C. Wills, Grid Protection Alliance
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
    /// Represents an observation record in a PQDIF file.
    /// </summary>
    public class ObservationRecord
    {
        #region [ Members ]

        // Fields
        private Record m_physicalRecord;
        private DataSourceRecord m_dataSource;
        private MonitorSettingsRecord m_settings;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="ObservationRecord"/> class.
        /// </summary>
        /// <param name="physicalRecord">The physical structure of the observation record.</param>
        /// <param name="dataSource">The data source record that defines the channels in this observation record.</param>
        /// <param name="settings">The monitor settings to be applied to this observation record.</param>
        private ObservationRecord(Record physicalRecord, DataSourceRecord dataSource, MonitorSettingsRecord settings)
        {
            m_physicalRecord = physicalRecord;
            m_dataSource = dataSource;
            m_settings = settings;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the data source record that defines
        /// the channels in this observation record.
        /// </summary>
        public DataSourceRecord DataSource
        {
            get
            {
                return m_dataSource;
            }
        }

        /// <summary>
        /// Gets the monitor settings record that defines the
        /// settings to be applied to this observation record.
        /// </summary>
        public MonitorSettingsRecord Settings
        {
            get
            {
                return m_settings;
            }
        }

        /// <summary>
        /// Gets the name of the observation record.
        /// </summary>
        public string Name
        {
            get
            {
                VectorElement nameVector = m_physicalRecord.Body.Collection.GetVectorByTag(ObservationNameTag);
                return Encoding.ASCII.GetString(nameVector.GetValues()).Trim((char)0);
            }
        }

        /// <summary>
        /// Gets the starting time of the data in the observation record.
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return m_physicalRecord.Body.Collection
                    .GetScalarByTag(TimeStartTag)
                    .GetTimestamp();
            }
        }

        /// <summary>
        /// Gets the channel instances in this observation record.
        /// </summary>
        public IList<ChannelInstance> ChannelInstances
        {
            get
            {
                return m_physicalRecord.Body.Collection
                    .GetCollectionByTag(ChannelInstancesTag)
                    .GetElementsByTag(OneChannelInstanceTag)
                    .Cast<CollectionElement>()
                    .Select(collection => new ChannelInstance(collection, this))
                    .ToList();
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields

        /// <summary>
        /// Tag that identifies the name of the observation record.
        /// </summary>
        public static readonly Guid ObservationNameTag = new Guid("3d786f8a-f76e-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the time that the observation record was created.
        /// </summary>
        public static readonly Guid TimeCreateTag = new Guid("3d786f8b-f76e-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the start time of the data in the observation record.
        /// </summary>
        public static readonly Guid TimeStartTag = new Guid("3d786f8c-f76e-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the type of trigger that caused the observation.
        /// </summary>
        public static readonly Guid TriggerMethodTag = new Guid("3d786f8d-f76e-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the channel instances collection.
        /// </summary>
        public static readonly Guid ChannelInstancesTag = new Guid("3d786f91-f76e-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies a single channel instance in the collection.
        /// </summary>
        public static readonly Guid OneChannelInstanceTag = new Guid("3d786f92-f76e-11cf-9d89-0080c72e70a3");

        // Static Methods

        /// <summary>
        /// Creates a new observation record from the given physical record
        /// if the physical record is of type observation. Returns null if
        /// it is not.
        /// </summary>
        /// <param name="physicalRecord">The physical record used to create the observation record.</param>
        /// <param name="dataSource">The data source record that defines the channels in this observation record.</param>
        /// <param name="settings">The monitor settings to be applied to this observation record.</param>
        /// <returns>The new observation record, or null if the physical record does not define a observation record.</returns>
        public static ObservationRecord CreateObservationRecord(Record physicalRecord, DataSourceRecord dataSource, MonitorSettingsRecord settings)
        {
            bool isValidObservationRecord = physicalRecord.Header.TypeOfRecord == RecordType.Observation;
            return isValidObservationRecord ? new ObservationRecord(physicalRecord, dataSource, settings) : null;
        }

        #endregion

    }
}
