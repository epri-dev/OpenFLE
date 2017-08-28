﻿//*********************************************************************************************************************
// Record.cs
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
//  03/30/2012 - Mehulbhai Thakkar, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace openPQDIF.Physical
{
    #region [ Enumerations ]

    /// <summary>
    /// Enumeration that defines type of record as per header definition.
    /// </summary>
    public enum RecordType
    {
        /// <summary>
        /// Represents a record level tag which identifies the container record.
        /// Always the first one in the file, and only one per file.
        /// </summary>
        Container,

        /// <summary>
        /// Represents a record level tag which identifies the data source record.
        /// Instrument level information.
        /// </summary>
        DataSource,

        /// <summary>
        /// Represents an optional record level tag which identifies configuration parameters.
        /// Used to capture configuration changes on the data source.
        /// </summary>
        MonitorSettings,

        /// <summary>
        /// Represents a record-level tag which identifies an observation.
        /// Used to capture an event, measurements etc.
        /// </summary>
        Observation,

        /// <summary>
        /// Represents a record-level tag which is unknown to this library.
        /// </summary>
        Unknown
    }

    #endregion

    /// <summary>
    /// Represents a record in a PQDIF file. This class
    /// exposes the physical structure of the record.
    /// </summary>
    public class Record
    {
        #region [ Members ]

        // Fields
        private RecordHeader m_header;
        private RecordBody m_body;

        #endregion

        #region [ Constructor ]

        /// <summary>
        /// Creates an instance of <see cref="Record"/>.
        /// </summary>
        /// <param name="header">The record header.</param>
        /// <param name="body">The record body.</param>
        public Record(RecordHeader header, RecordBody body)
        {
            Header = header;
            Body = body;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the record header.
        /// </summary>
        public RecordHeader Header
        {
            get
            {
                return m_header;
            }
            set
            {
                if ((object)value == null)
                    throw new ArgumentNullException("value");

                m_header = value;
            }
        }

        /// <summary>
        /// Gets or sets the record body.
        /// </summary>
        public RecordBody Body
        {
            get
            {
                return m_body;
            }
            set
            {
                m_body = value;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Returns a string representation of the record.
        /// </summary>
        /// <returns>A string representation of the record.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(m_header);
            builder.AppendLine();
            builder.AppendLine();
            builder.Append(m_body);

            return builder.ToString();
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly Dictionary<Guid, RecordType> RecordTypeTagMap = CreateRecordTypeTagMap();

        // Static Methods

        /// <summary>
        /// Gets the <see cref="RecordType"/> identified by the given tag.
        /// </summary>
        /// <param name="recordTypeTag">The tag of the <see cref="RecordType"/>.</param>
        /// <returns>The <see cref="RecordType"/> identified by the given tag.</returns>
        public static RecordType GetRecordType(Guid recordTypeTag)
        {
            RecordType type;

            if (RecordTypeTagMap.TryGetValue(recordTypeTag, out type))
                return type;

            return RecordType.Unknown;
        }

        /// <summary>
        /// Gets the globally unique identifier used to identify the given record type.
        /// </summary>
        /// <param name="recordType">The record type to search for.</param>
        /// <returns>The globally unique identifier used to identify the given record type.</returns>
        public static Guid GetTypeAsTag(RecordType recordType)
        {   
            return RecordTypeTagMap.First(pair => pair.Value == recordType).Key;
        }

        // Creates the dictionary mapping tags to their record type.
        private static Dictionary<Guid, RecordType> CreateRecordTypeTagMap()
        {
            return new Dictionary<Guid, RecordType>
            {
                { new Guid("89738606-f1c3-11cf-9d89-0080c72e70a3"), RecordType.Container },
                { new Guid("89738619-f1c3-11cf-9d89-0080c72e70a3"), RecordType.DataSource },
                { new Guid("b48d858c-f5f5-11cf-9d89-0080c72e70a3"), RecordType.MonitorSettings },
                { new Guid("8973861a-f1c3-11cf-9d89-0080c72e70a3"), RecordType.Observation }
            };
        }

        #endregion
    }
}
