//*********************************************************************************************************************
// DataSourceRecord.cs
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
    /// Represents a data source record in a PQDIF file. The data source
    /// record contains information about the source of the data in an
    /// <see cref="ObservationRecord"/>.
    /// </summary>
    public class DataSourceRecord
    {
        #region [ Members ]

        // Fields
        private Record m_physicalRecord;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="DataSourceRecord"/> class.
        /// </summary>
        /// <param name="physicalRecord">The physical structure of the data source record.</param>
        private DataSourceRecord(Record physicalRecord)
        {
            m_physicalRecord = physicalRecord;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the name of the data source.
        /// </summary>
        public string DataSourceName
        {
            get
            {
                VectorElement dataSourceNameElement = m_physicalRecord.Body.Collection.GetVectorByTag(DataSourceNameTag);
                return Encoding.ASCII.GetString(dataSourceNameElement.GetValues()).Trim((char)0);
            }
        }

        /// <summary>
        /// Gets the definitions for the channels defined in the data source.
        /// </summary>
        public IList<ChannelDefinition> ChannelDefinitions
        {
            get
            {
                return m_physicalRecord.Body.Collection
                    .GetCollectionByTag(ChannelDefinitionsTag)
                    .GetElementsByTag(OneChannelDefinitionTag)
                    .Cast<CollectionElement>()
                    .Select(collection => new ChannelDefinition(collection, this))
                    .ToList();
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields

        /// <summary>
        /// Tag that identifies the data source type.
        /// </summary>
        public static readonly Guid DataSourceTypeTag = new Guid("b48d8581-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the data source name.
        /// </summary>
        public static readonly Guid DataSourceNameTag = new Guid("b48d8587-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the channel definitions collection.
        /// </summary>
        public static readonly Guid ChannelDefinitionsTag = new Guid("b48d858d-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the a single channel definition in the collection.
        /// </summary>
        public static readonly Guid OneChannelDefinitionTag = new Guid("b48d858e-f5f5-11cf-9d89-0080c72e70a3");

        /// <summary>
        /// Tag that identifies the time that the data source record becomes effective.
        /// </summary>
        public static readonly Guid EffectiveTag = new Guid("62f28183-f9c4-11cf-9d89-0080c72e70a3");

        // Static Methods

        /// <summary>
        /// Creates a new data source record from the given physical record
        /// if the physical record is of type data source. Returns null if
        /// it is not.
        /// </summary>
        /// <param name="physicalRecord">The physical record used to create the data source record.</param>
        /// <returns>The new data source record, or null if the physical record does not define a data source record.</returns>
        public static DataSourceRecord CreateDataSourceRecord(Record physicalRecord)
        {
            bool isValidDataSourceRecord = physicalRecord.Header.TypeOfRecord == RecordType.DataSource;
            return isValidDataSourceRecord ? new DataSourceRecord(physicalRecord) : null;
        }

        #endregion
        
    }
}
